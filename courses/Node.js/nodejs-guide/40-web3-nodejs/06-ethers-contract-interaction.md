# Ethers.js Contract Interaction

## What You'll Learn

- Deploying smart contracts
- Calling contract methods
- Handling contract errors
- Working with different contract types

---

## Layer 1: Contract Deployment

```typescript
// Deploy contract
const factory = new ethers.ContractFactory(abi, bytecode, signer);
const contract = await factory.deploy(...constructorArgs);
await contract.waitForDeployment();
const address = await contract.getAddress();
```

---

## Layer 2: Contract Calls

```typescript
// Read (view/pure functions)
const result = await contract.functionName(args);

// Write (state-changing)
const tx = await contract.functionName(args);
await tx.wait();

// With overrides
const tx = await contract.functionName(args, {
  gasLimit: 100000,
  value: ethers.parseEther('0.1')
});
```

---

## Next Steps

Continue to [Ethers Events](./07-ethers-events.md)