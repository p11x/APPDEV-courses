# Backend Patterns

## What You'll Learn

- Building Web3 backends
- Event indexing
- Transaction monitoring
- Webhook notifications

---

## Layer 1: Event Indexer

```typescript
// indexer.ts
import { ethers } from 'ethers';

const provider = new ethers.JsonRpcProvider(process.env.RPC_URL);

async function indexEvents(fromBlock: number) {
  const contract = new ethers.Contract(CONTRACT_ADDRESS, ABI, provider);
  
  const filter = contract.filters.Transfer();
  const events = await contract.queryFilter(filter, fromBlock);
  
  for (const event of events) {
    console.log('Transfer:', {
      from: event.args.from,
      to: event.args.to,
      value: event.args.value.toString(),
      block: event.blockNumber
    });
  }
}
```
