# Provider Wallet Integration

## What You'll Learn

- MetaMask integration
- Browser provider setup
- Network switching
- Account changes

---

## Layer 1: MetaMask Integration

```typescript
import { ethers } from 'ethers';

declare global {
  interface Window {
    ethereum?: {
      isMetaMask: boolean;
      request: (args: { method: string; params?: unknown[] }) => Promise<unknown>;
      on: (event: string, callback: (...args: unknown[]) => void) => void;
      removeListener: (event: string, callback: (...args: unknown[]) => void) => void;
    };
  }
}

async function connectMetaMask() {
  if (!window.ethereum) throw new Error('MetaMask not installed');
  
  const provider = new ethers.BrowserProvider(window.ethereum);
  const accounts = await provider.send('eth_requestAccounts', []);
  const signer = await provider.getSigner();
  
  return { provider, signer, account: accounts[0] };
}
```

---

## Next Steps

Continue to [dApp Setup](./12-web3-dapp-setup.md)