# Database Transaction Management and Patterns

## What You'll Learn

- ACID properties and isolation levels
- Transaction patterns across databases
- Distributed transaction patterns
- Savepoints and nested transactions
- Transaction retry strategies

## ACID Properties

```
ACID Properties:
─────────────────────────────────────────────
Atomicity:   All operations succeed or all fail
Consistency: Database moves from one valid state to another
Isolation:   Concurrent transactions don't interfere
Durability:  Committed data survives system failures

Isolation Levels (PostgreSQL):
─────────────────────────────────────────────
Level               Dirty Read  Non-Repeatable  Phantom
                                 Read
─────────────────────────────────────────────
READ UNCOMMITTED    Possible     Possible        Possible
READ COMMITTED      No           Possible        Possible
REPEATABLE READ     No           No              Possible
SERIALIZABLE        No           No              No
```

## PostgreSQL Transactions

```javascript
import { Pool } from 'pg';

const pool = new Pool();

// Basic transaction
async function basicTransaction() {
    const client = await pool.connect();
    try {
        await client.query('BEGIN');
        await client.query('INSERT INTO orders (user_id, total) VALUES ($1, $2)', [1, 100]);
        await client.query('UPDATE users SET order_count = order_count + 1 WHERE id = $1', [1]);
        await client.query('COMMIT');
    } catch (err) {
        await client.query('ROLLBACK');
        throw err;
    } finally {
        client.release();
    }
}

// Transaction with isolation level
async function readCommittedTransaction() {
    const client = await pool.connect();
    try {
        await client.query('BEGIN ISOLATION LEVEL READ COMMITTED');
        // Operations here see committed data only
        const { rows } = await client.query('SELECT balance FROM accounts WHERE id = $1', [1]);
        await client.query('UPDATE accounts SET balance = balance - $1 WHERE id = $2', [50, 1]);
        await client.query('COMMIT');
    } catch (err) {
        await client.query('ROLLBACK');
        throw err;
    } finally {
        client.release();
    }
}

// Serializable transaction (strongest isolation)
async function serializableTransaction() {
    const client = await pool.connect();
    try {
        await client.query('BEGIN ISOLATION LEVEL SERIALIZABLE');
        const { rows } = await client.query('SELECT balance FROM accounts WHERE id = $1 FOR UPDATE', [1]);
        if (rows[0].balance < 100) throw new Error('Insufficient funds');
        await client.query('UPDATE accounts SET balance = balance - $1 WHERE id = $2', [100, 1]);
        await client.query('COMMIT');
    } catch (err) {
        await client.query('ROLLBACK');
        throw err;
    } finally {
        client.release();
    }
}
```

## Savepoints

```javascript
// Savepoints for partial rollback
async function transferWithSavepoint(fromId, toId, amount) {
    const client = await pool.connect();
    try {
        await client.query('BEGIN');

        // Debit source
        await client.query(
            'UPDATE accounts SET balance = balance - $1 WHERE id = $2',
            [amount, fromId]
        );

        // Create savepoint before risky operation
        await client.query('SAVEPOINT before_credit');

        try {
            // Credit destination
            await client.query(
                'UPDATE accounts SET balance = balance + $1 WHERE id = $2',
                [amount, toId]
            );
        } catch (err) {
            // Rollback to savepoint, not entire transaction
            await client.query('ROLLBACK TO SAVEPOINT before_credit');
            // Log to dead letter queue instead
            await client.query(
                'INSERT INTO failed_credits (account_id, amount, reason) VALUES ($1, $2, $3)',
                [toId, amount, err.message]
            );
        }

        await client.query('COMMIT');
    } catch (err) {
        await client.query('ROLLBACK');
        throw err;
    } finally {
        client.release();
    }
}
```

## MongoDB Transactions

```javascript
import mongoose from 'mongoose';

// Session-based transactions
async function mongoTransaction(fromId, toId, amount) {
    const session = await mongoose.startSession();
    try {
        session.startTransaction({
            readConcern: { level: 'snapshot' },
            writeConcern: { w: 'majority' },
        });

        const from = await Account.findOneAndUpdate(
            { _id: fromId, balance: { $gte: amount } },
            { $inc: { balance: -amount } },
            { session, new: true }
        );

        if (!from) throw new Error('Insufficient funds or account not found');

        await Account.findOneAndUpdate(
            { _id: toId },
            { $inc: { balance: amount } },
            { session, new: true }
        );

        await session.commitTransaction();
        return { success: true };
    } catch (err) {
        await session.abortTransaction();
        throw err;
    } finally {
        session.endSession();
    }
}

// Multi-collection transaction
async function createOrderWithInventory(orderData, items) {
    const session = await mongoose.startSession();
    try {
        session.startTransaction();

        // Create order
        const order = await Order.create([orderData], { session });

        // Decrement inventory for each item
        for (const item of items) {
            const result = await Inventory.findOneAndUpdate(
                { productId: item.productId, quantity: { $gte: item.quantity } },
                { $inc: { quantity: -item.quantity } },
                { session, new: true }
            );
            if (!result) throw new Error(`Insufficient inventory for ${item.productId}`);
        }

        // Create shipment record
        await Shipment.create([{
            orderId: order[0]._id,
            status: 'pending',
        }], { session });

        await session.commitTransaction();
        return order[0];
    } catch (err) {
        await session.abortTransaction();
        throw err;
    } finally {
        session.endSession();
    }
}
```

## MySQL Transactions

```javascript
import mysql from 'mysql2/promise';

const pool = mysql.createPool({ /* config */ });

// MySQL transaction with isolation level
async function mysqlTransaction() {
    const connection = await pool.getConnection();
    try {
        await connection.beginTransaction();

        // MySQL supports: READ UNCOMMITTED, READ COMMITTED, REPEATABLE SERIALIZABLE
        await connection.query('SET TRANSACTION ISOLATION LEVEL REPEATABLE READ');

        const [accounts] = await connection.execute(
            'SELECT balance FROM accounts WHERE id = ? FOR UPDATE',
            [1]
        );

        if (accounts[0].balance < 100) throw new Error('Insufficient funds');

        await connection.execute(
            'UPDATE accounts SET balance = balance - ? WHERE id = ?',
            [100, 1]
        );

        await connection.commit();
    } catch (err) {
        await connection.rollback();
        throw err;
    } finally {
        connection.release();
    }
}
```

## Transaction Retry with Serialization Failures

```javascript
async function withRetry(fn, maxRetries = 3) {
    for (let attempt = 1; attempt <= maxRetries; attempt++) {
        try {
            return await fn();
        } catch (err) {
            // PostgreSQL serialization failure codes
            const retryableCodes = ['40001', '40P01'];
            
            if (retryableCodes.includes(err.code) && attempt < maxRetries) {
                const delay = Math.min(100 * Math.pow(2, attempt), 2000);
                const jitter = Math.random() * 100;
                console.log(`Transaction retry ${attempt}/${maxRetries}, waiting ${delay + jitter}ms`);
                await new Promise(r => setTimeout(r, delay + jitter));
                continue;
            }
            throw err;
        }
    }
}

// Usage
const result = await withRetry(async () => {
    const client = await pool.connect();
    try {
        await client.query('BEGIN ISOLATION LEVEL SERIALIZABLE');
        
        const { rows } = await client.query(
            'SELECT balance FROM accounts WHERE id = $1 FOR UPDATE',
            [1]
        );
        
        await client.query(
            'UPDATE accounts SET balance = balance - $1 WHERE id = $2',
            [100, 1]
        );
        
        await client.query('COMMIT');
        return { success: true };
    } catch (err) {
        await client.query('ROLLBACK');
        throw err;
    } finally {
        client.release();
    }
});
```

## Sagas Pattern for Distributed Transactions

```javascript
// Saga pattern for multi-service transactions
class Saga {
    constructor() {
        this.steps = [];
    }

    addStep(execute, compensate) {
        this.steps.push({ execute, compensate });
        return this;
    }

    async run(context = {}) {
        const completed = [];

        for (const step of this.steps) {
            try {
                const result = await step.execute(context);
                completed.push({ step, result });
                Object.assign(context, result);
            } catch (err) {
                console.error('Saga step failed:', err.message);
                // Compensate completed steps in reverse
                for (const { step: s, result } of completed.reverse()) {
                    try {
                        await s.compensate(context, result);
                    } catch (compErr) {
                        console.error('Compensation failed:', compErr.message);
                    }
                }
                throw err;
            }
        }

        return context;
    }
}

// Usage: Order creation saga
const orderSaga = new Saga()
    .addStep(
        async (ctx) => {
            const order = await createOrder(ctx.orderData);
            return { orderId: order.id };
        },
        async (ctx) => {
            await cancelOrder(ctx.orderId);
        }
    )
    .addStep(
        async (ctx) => {
            await reserveInventory(ctx.orderId, ctx.items);
            return { inventoryReserved: true };
        },
        async (ctx) => {
            await releaseInventory(ctx.orderId);
        }
    )
    .addStep(
        async (ctx) => {
            const payment = await processPayment(ctx.orderId, ctx.total);
            return { paymentId: payment.id };
        },
        async (ctx) => {
            await refundPayment(ctx.paymentId);
        }
    );

try {
    const result = await orderSaga.run({
        orderData: { userId: 1, items: [{ id: 1, qty: 2 }] },
        total: 100,
    });
    console.log('Order created:', result.orderId);
} catch (err) {
    console.error('Order creation failed:', err.message);
}
```

## Transaction Performance Comparison

```
Transaction Performance Impact:
─────────────────────────────────────────────
Isolation Level    Overhead    Use Case
─────────────────────────────────────────────
READ COMMITTED     ~5%         Most web apps (default)
REPEATABLE READ    ~10%        Reports, analytics
SERIALIZABLE       ~30%        Financial, inventory

Best practices:
├── Use READ COMMITTED by default (PostgreSQL default)
├── Use SERIALIZABLE only for critical financial ops
├── Keep transactions as short as possible
├── Acquire locks in consistent order to prevent deadlocks
└── Always set a statement_timeout
```

## Best Practices Checklist

- [ ] Use transactions for multi-step write operations
- [ ] Keep transactions as short as possible
- [ ] Always release connections in `finally` blocks
- [ ] Use appropriate isolation levels for your use case
- [ ] Implement retry logic for serialization failures
- [ ] Use savepoints for partial rollback scenarios
- [ ] Use `FOR UPDATE` to explicitly lock rows
- [ ] Consider saga pattern for distributed transactions
- [ ] Set `statement_timeout` to prevent long-running transactions
- [ ] Monitor transaction duration and lock contention

## Cross-References

- See [Connection Pooling](./04-connection-pooling.md) for connection management
- See [Error Handling](./06-error-handling-recovery.md) for error recovery patterns
- See [Microservices DB](../05-scalability-patterns/03-microservices-database.md) for distributed patterns

## Next Steps

Continue to [Error Handling and Recovery](./06-error-handling-recovery.md) for database error management.
