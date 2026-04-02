# Ethers.js Signer

## What You'll Learn

- Creating and managing signers
- HD wallet support
- Hardware wallet integration
- Transaction signing

---

## Layer 1: Signer Types

### Wallet Signer

```typescript
import { ethers } from 'ethers';

// From private key
const wallet = new ethers.Wallet(privateKey, provider);

// From mnemonic
const walletFromMnemonic = ethers.Wallet.fromPhrase(mnemonic, provider);

// From HD path
const hdWallet = ethers.HDNodeWallet.fromMnemonic(mnemonic);
```

### Browser Wallet (MetaMask)

```typescript
async function getSigner() {
  if (!window.ethereum) {
    throw new Error('MetaMask not installed');
  }
  
  const provider = new ethers.BrowserProvider(window.ethereum);
  const signer = await provider.getSigner();
  
  const address = await signer.getAddress();
  const network = await provider.getNetwork();
  
  return { provider, signer, address, network };
}
```

---

## Next Steps

Continue to [Ethers Contract Interaction](./06-ethers-contract-interaction.md)