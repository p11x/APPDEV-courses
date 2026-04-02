# Web3 Basics for Node.js

## What You'll Learn

- Understanding Web3 and blockchain fundamentals
- Setting up Node.js for Web3 development
- Connecting to Ethereum and EVM-compatible networks
- Basic wallet and transaction concepts

---

## Layer 1: Academic Foundation

### What is Web3?

Web3 represents the third generation of the internet, built on blockchain technology. It enables:
- **Decentralization**: No single point of control
- **Trustlessness**: Code executes exactly as written
- **Ownership**: Users own their data and assets
- **Native Payments**: Cryptocurrencies replace payment processors

### Blockchain Fundamentals

A blockchain is a distributed ledger where:
- **Blocks**: Contain batches of transactions
- **Chain**: Linked through cryptographic hashes
- **Consensus**: Network validates transactions
- **Immutability**: Once confirmed, data cannot be changed

**Ethereum Virtual Machine (EVM)**: The runtime environment for smart contracts on Ethereum and EVM-compatible chains.

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Basic Connection

```typescript
// web3-basic.ts
import { ethers } from 'ethers';

async function connectToNetwork() {
  // Connect to Ethereum mainnet via Infura
  const provider = new ethers.JsonRpcProvider(
    'https://mainnet.infura.io/v3/YOUR_INFURA_KEY'
  );

  // Get latest block
  const block = await provider.getBlock('latest');
  console.log('Block number:', block?.number);
  console.log('Block hash:', block?.hash);

  // Get block timestamp
  console.log('Timestamp:', new Date(Number(block?.timestamp) * 1000));
}

connectToNetwork().catch(console.error);
```

### Paradigm 2 — Wallet Connection

```typescript
// wallet-basics.ts
import { ethers } from 'ethers';

async function walletOperations() {
  // Create a random wallet
  const wallet = ethers.Wallet.createRandom();
  console.log('New wallet address:', wallet.address);
  console.log('Private key (keep secret!):', wallet.privateKey);
  console.log('Mnemonic:', wallet.mnemonic.phrase);

  // Connect to a provider
  const provider = new ethers.JsonRpcProvider('http://localhost:8545');
  const connectedWallet = wallet.connect(provider);

  // Check balance
  const balance = await provider.getBalance(wallet.address);
  console.log('Balance:', ethers.formatEther(balance), 'ETH');
}
```

### Paradigm 3 — Transaction

```typescript
// transaction.ts
async function sendTransaction() {
  const provider = new ethers.JsonRpcProvider('http://localhost:8545');
  
  // Connect existing wallet
  const wallet = new ethers.Wallet(process.env.PRIVATE_KEY!, provider);

  // Send ETH
  const tx = await wallet.sendTransaction({
    to: '0x742d35Cc6634C0532925a3b844Bc9e7595f2b',
    value: ethers.parseEther('0.01')
  });

  console.log('Transaction hash:', tx.hash);
  
  // Wait for confirmation
  const receipt = await tx.wait();
  console.log('Block number:', receipt?.blockNumber);
  console.log('Gas used:', receipt?.gasUsed);
}
```

---

## Layer 3: Performance Engineering

### Provider Selection

| Provider | Latency | Reliability | Cost |
|----------|---------|-------------|------|
| Infura | ~100ms | High | Free tier |
| Alchemy | ~100ms | High | Free tier |
| QuickNode | ~50ms | High | Paid |
| Local (Anvil) | ~10ms | Custom | Free |

---

## Layer 4: Security

### Key Security Practices

- **Never expose private keys**: Use environment variables or secret managers
- **Validate addresses**: Check address format before sending
- **Use testnets**: Always test on testnets first
- **Estimate gas**: Always estimate before sending

---

## Layer 5: Testing

### Test Networks

```typescript
const TESTNETWORKS = {
  sepolia: {
    name: 'Sepolia',
    chainId: 11155111,
    rpc: 'https://rpc.sepolia.org'
  },
  goerli: {
    name: 'Goerli',
    chainId: 5,
    rpc: 'https://goerli.infura.io/v3/YOUR_KEY'
  }
};
```

---

## Next Steps

Continue to [Ethers vs Web3.js](./02-ethers-vs-web3js.md)