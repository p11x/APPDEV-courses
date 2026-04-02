# Transactions

## What You'll Learn

- What a database transaction is and why it matters
- How to use Prisma's `$transaction` with an array of operations
- How to use interactive transactions for complex logic
- How transactions handle rollback on error
- How to use transactions for consistency guarantees

## What Is a Transaction?

A transaction groups multiple database operations into a single atomic unit. Either **all** operations succeed, or **none** of them do.

```
Without transaction:
  1. Deduct $100 from Alice's account ✓
  2. Server crashes
  3. Add $100 to Bob's account ✗
  → Alice lost $100, Bob got nothing — money disappeared!

With transaction:
  1. Deduct $100 from Alice's account ✓
  2. Server crashes
  3. Transaction rolled back — Alice's $100 is restored
  → Database is consistent
```

## Array Transactions

For a sequence of independent operations that should all succeed together:

```js
import prisma from './lib/prisma.js';

// Array transaction — all operations run in order, commit if all succeed
const [user, post, comment] = await prisma.$transaction([
  prisma.user.create({
    data: { email: 'alice@example.com', name: 'Alice' },
  }),
  prisma.post.create({
    data: { title: 'Transaction Demo', authorId: 1 },
  }),
  prisma.comment.create({
    data: { text: 'Nice post!', authorId: 1, postId: 1 },
  }),
]);

console.log('All created:', { user, post, comment });
// If ANY operation fails, ALL are rolled back
```

## Interactive Transactions

For complex logic where you need to read then conditionally write:

```js
// Interactive transaction — run arbitrary code inside the transaction
const result = await prisma.$transaction(async (tx) => {
  // tx is a transactional PrismaClient — all queries through it are in the transaction

  // Step 1: Read the sender's balance
  const sender = await tx.user.findUnique({
    where: { id: 1 },
  });

  if (sender.balance < 100) {
    throw new Error('Insufficient funds');  // Rollback everything
  }

  // Step 2: Deduct from sender
  const updatedSender = await tx.user.update({
    where: { id: 1 },
    data: { balance: { decrement: 100 } },
  });

  // Step 3: Add to receiver
  const updatedReceiver = await tx.user.update({
    where: { id: 2 },
    data: { balance: { increment: 100 } },
  });

  // Step 4: Record the transfer
  const transfer = await tx.transfer.create({
    data: {
      fromId: 1,
      toId: 2,
      amount: 100,
    },
  });

  return { sender: updatedSender, receiver: updatedReceiver, transfer };
});

console.log('Transfer complete:', result);
```

## Transaction Options

```js
await prisma.$transaction(
  async (tx) => {
    // Your transaction logic
    const users = await tx.user.findMany();
    // ...
  },
  {
    // Maximum time (ms) the transaction can run
    maxWait: 5000,     // 5 seconds to acquire a connection

    // Maximum time (ms) the transaction itself can take
    timeout: 10_000,   // 10 seconds total

    // Isolation level — controls what other transactions can see
    isolationLevel: 'Serializable',  // Strongest guarantee
  }
);
```

### Isolation Levels

| Level | Dirty Read | Non-Repeatable Read | Phantom Read |
|-------|-----------|-------------------|--------------|
| ReadUncommitted | Possible | Possible | Possible |
| ReadCommitted | No | Possible | Possible |
| RepeatableRead | No | No | Possible |
| Serializable | No | No | No |

Most applications use the default. Use `Serializable` for financial operations.

## Real-World Example: Order Placement

```js
// order.js — Place an order within a transaction

async function placeOrder(userId, items) {
  return prisma.$transaction(async (tx) => {
    // 1. Validate user exists
    const user = await tx.user.findUnique({ where: { id: userId } });
    if (!user) throw new Error('User not found');

    // 2. Check stock for all items
    for (const item of items) {
      const product = await tx.product.findUnique({ where: { id: item.productId } });
      if (!product || product.stock < item.quantity) {
        throw new Error(`Insufficient stock for product ${item.productId}`);
      }
    }

    // 3. Create the order
    const order = await tx.order.create({
      data: {
        userId,
        total: items.reduce((sum, i) => sum + i.price * i.quantity, 0),
        items: {
          create: items.map((item) => ({
            productId: item.productId,
            quantity: item.quantity,
            price: item.price,
          })),
        },
      },
      include: { items: true },
    });

    // 4. Decrement stock
    for (const item of items) {
      await tx.product.update({
        where: { id: item.productId },
        data: { stock: { decrement: item.quantity } },
      });
    }

    return order;
  });
}
```

## How It Works

### Transaction Lifecycle

```
$transaction starts
    │
    ├── Begin SQL transaction
    │
    ├── Run operation 1
    ├── Run operation 2
    ├── Run operation 3
    │
    ├── If error thrown → Rollback → throw error to caller
    │
    └── If all succeed → Commit → return result
```

### tx vs prisma

Inside `prisma.$transaction(async (tx) => { ... })`:
- Use `tx` for queries within the transaction
- Use `prisma` for queries outside the transaction (rarely needed)

## Common Mistakes

### Mistake 1: Using prisma Instead of tx Inside Transaction

```js
// WRONG — this query is OUTSIDE the transaction
await prisma.$transaction(async (tx) => {
  const user = await prisma.user.findUnique({ where: { id: 1 } });
  // ↑ Uses prisma, not tx — this is NOT in the transaction!
  // Another concurrent transaction could modify this user
});

// CORRECT — use tx for all queries inside the transaction
await prisma.$transaction(async (tx) => {
  const user = await tx.user.findUnique({ where: { id: 1 } });
  // ↑ Uses tx — properly isolated
});
```

### Mistake 2: Long-Running Transactions

```js
// WRONG — transaction holds a database lock for 30 seconds
await prisma.$transaction(async (tx) => {
  const user = await tx.user.findUnique({ where: { id: 1 } });
  await sendEmail(user.email);  // 30 second operation!
  await tx.user.update({ where: { id: 1 }, data: { emailSent: true } });
});

// CORRECT — keep transactions short; do external work outside
const user = await prisma.user.findUnique({ where: { id: 1 } });
await sendEmail(user.email);
await prisma.user.update({ where: { id: 1 }, data: { emailSent: true } });
// No transaction needed — each operation is independent
```

### Mistake 3: Not Handling Transaction Errors

```js
// WRONG — transaction error crashes the app
const result = await prisma.$transaction(async (tx) => {
  // Throws on validation failure
});

// CORRECT — catch and handle transaction errors
try {
  const result = await prisma.$transaction(async (tx) => {
    // ...
  });
} catch (err) {
  if (err.message.includes('Insufficient funds')) {
    return res.status(400).json({ error: 'Insufficient funds' });
  }
  throw err;
}
```

## Try It Yourself

### Exercise 1: Bank Transfer

Create two users with balances. Transfer $50 between them in a transaction. Verify the balances are correct after the transfer.

### Exercise 2: Insufficient Funds

Try to transfer more money than the sender has. Verify the transaction rolls back and neither balance changes.

### Exercise 3: Concurrent Transactions

Open two Prisma clients. Start two transactions that transfer money between the same accounts simultaneously. Observe how isolation levels prevent race conditions.

## Next Steps

You understand transactions. For debugging Node.js applications, continue to [Chapter 23: Debugging & Profiling](../../23-debugging-profiling/debugging/01-node-inspector.md).
