# Gas Optimization

## What You'll Learn

- Understanding Ethereum gas mechanics
- Reducing gas costs
- Optimizing contract deployment
- Batch operations

---

## Layer 1: Gas Optimization Tips

| Technique | Savings |
|-----------|---------|
| Use `calldata` instead of `memory` | ~2000 gas |
| Short-circuit boolean expressions | Variable |
| Packing struct variables | Up to 50% |
| Unchecked math where safe | ~50% |
| Using events instead of storage | Major |

---

## Next Steps

Continue to [Web3 Security](./17-web3-security.md)