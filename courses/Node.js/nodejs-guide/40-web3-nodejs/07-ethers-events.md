# Ethers.js Events

## What You'll Learn

- Listening to smart contract events
- Filtering events
- Event parsing
- Historical event queries

---

## Layer 1: Event Listening

```typescript
// Listen to new events
contract.on('Transfer', (from, to, value, event) => {
  console.log('Transfer:', from, to, value);
});

// Once
contract.once('Transfer', (from, to, value) => {
  console.log('First transfer:', from, to);
});

// All events
contract.on('*', (event) => {
  console.log('Any event:', event.eventName);
});
```

---

## Layer 2: Historical Events

```typescript
// Query historical events
const filter = contract.filters.Transfer(userAddress);
const events = await contract.queryFilter(filter, fromBlock, toBlock);

// Parse event data
for (const event of events) {
  const [from, to, value] = event.args;
  console.log(from, to, value);
}
```

---

## Next Steps

Continue to [Ethers Utils](./08-ethers-utils.md)