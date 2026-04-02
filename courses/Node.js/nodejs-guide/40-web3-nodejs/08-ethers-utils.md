# Ethers.js Utils

## What You'll Learn

- Utility functions in Ethers.js
- Address validation and checksum
- Unit conversions
- Cryptographic utilities

---

## Layer 1: Utilities

```typescript
import { ethers } from 'ethers';

// Address utilities
ethers.getAddress(address);  // Validate & checksum
ethers.isAddress(address);  // Check validity
ethers.computeAddress(privateKey);  // Derive address

// Unit conversions
ethers.parseEther('1.0');      // ETH → Wei
ethers.formatEther(wei);      // Wei → ETH
ethers.parseUnits('100', 6); // USDC → Wei
ethers.formatUnits(wei, 6);  // Wei → USDC

// Hashing
ethers.keccak256(ethers.toUtf8Bytes(data));
ethers.id(text);

// Random
ethers.randomBytes(32);
```
