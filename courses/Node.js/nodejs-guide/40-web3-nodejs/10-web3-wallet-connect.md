# WalletConnect Integration

## What You'll Learn

- Setting up WalletConnect
- QR code connection flow
- Mobile wallet integration
- Session management

---

## Layer 1: WalletConnect v2

```typescript
import { Core } from '@walletconnect/core';
import { Web3Modal } from '@web3modal/html';
import { ethers } from 'ethers';

const core = new Core({
  projectId: 'YOUR_PROJECT_ID'
});

const web3Modal = new Web3Modal({
  core,
  enableExplorer: true,
  chains: [1, 137] // Ethereum, Polygon
});

async function connect() {
  const session = await web3Modal.connect();
  const provider = new ethers.BrowserProvider(session);
  const signer = await provider.getSigner();
  return { session, provider, signer };
}
```

---

## Next Steps

Continue to [Provider Wallet](./11-web3-provider-wallet.md)