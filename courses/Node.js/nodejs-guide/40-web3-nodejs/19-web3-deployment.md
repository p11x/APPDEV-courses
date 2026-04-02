# Web3 Deployment

## What You'll Learn

- Deploying to different networks
- Verifying contracts
- Etherscan verification
- Multi-chain deployment

---

## Layer 1: Deployment

```typescript
// hardhat.config.js
module.exports = {
  networks: {
    sepolia: {
      url: process.env.SEPOLIA_URL,
      accounts: [process.env.PRIVATE_KEY]
    }
  },
  etherscan: {
    apiKey: process.env.ETHERSCAN_API_KEY
  }
};
```

```bash
npx hardhat deploy --network sepolia
npx hardhat verify --network sepolia CONTRACT_ADDRESS
```
