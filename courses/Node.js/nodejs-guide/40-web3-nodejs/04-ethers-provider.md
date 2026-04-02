# Ethers.js Provider

## What You'll Learn

- Different provider types in Ethers.js
- HTTP, WebSocket, and fallback providers
- Connecting to different networks
- Querying blockchain data

---

## Layer 1: Provider Types

### JsonRpcProvider

```typescript
import { ethers } from 'ethers';

// HTTP Provider
const httpProvider = new ethers.JsonRpcProvider(
  'https://eth-mainnet.g.alchemy.com/v2/YOUR_KEY'
);

// Local node
const localProvider = new ethers.JsonRpcProvider('http://localhost:8545');
```

### WebSocketProvider

```typescript
const wsProvider = new ethers.WebSocketProvider(
  'wss://eth-mainnet.g.alchemy.com/v2/YOUR_KEY',
  'mainnet'
);
```

### FallbackProvider

```typescript
const fallbackProvider = new ethers.FallbackProvider([
  new ethers.JsonRpcProvider('https://rpc1.mainnet.io'),
  new ethers.JsonRpcProvider('https://rpc2.mainnet.io'),
  new ethers.JsonRpcProvider('https://rpc3.mainnet.io')
]);
```

---

## Layer 2: Querying Data

```typescript
// Get block data
const block = await provider.getBlock(18000000);
console.log(block.hash, block.transactions.length);

// Get transaction
const tx = await provider.getTransaction(txHash);
console.log(tx.from, tx.to, tx.value);

// Get code at address
const code = await provider.getCode(contractAddress);
```

---

## Next Steps

Continue to [Ethers Signer](./05-ethers-signer.md)