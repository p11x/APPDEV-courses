# Ethers.js vs Web3.js

## What You'll Learn

- Comparing the two most popular Ethereum libraries
- API differences and similarities
- Choosing the right library for your project
- Migration strategies

---

## Layer 1: Library Comparison

### Feature Comparison

| Feature | Ethers.js | Web3.js |
|---------|-----------|---------|
| Size | ~80KB | ~500KB |
| API Design | Promise-based | Callback/Promise |
| Documentation | Excellent | Good |
| TypeScript | First-class | Partial |
| Maintenance | Active | Active |
| Wallet Support | Built-in | Plugin-based |

### Code Comparison

**Ethers.js:**
```typescript
import { ethers } from 'ethers';

const provider = new ethers.JsonRpcProvider(url);
const balance = await provider.getBalance(address);
const contract = new ethers.Contract(addr, abi, signer);
```

**Web3.js:**
```javascript
import Web3 from 'web3';

const web3 = new Web3(provider);
const balance = await web3.eth.getBalance(address);
const contract = new web3.eth.Contract(abi, addr);
```

---

## Layer 2: Decision Guide

### Choose Ethers.js When:
- Building a dApp with TypeScript
- Need smaller bundle size
- Prefer clean, promise-based API
- Need advanced wallet features

### Choose Web3.js When:
- Need extensive protocol support
- Working with WebSocket providers
- Need built-in solidity compiler

---

## Next Steps

Continue to [Smart Contracts Setup](./03-smart-contracts-setup.md)