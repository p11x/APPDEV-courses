# Token Standards

## What You'll Learn

- ERC-20 token standard
- ERC-721 NFT standard
- ERC-1155 multi-token standard
- Custom extensions

---

## Layer 1: ERC-20

```solidity
import "@openzeppelin/contracts/token/ERC20/ERC20.sol";

contract MyToken is ERC20 {
    constructor() ERC20("MyToken", "MTK") {
        _mint(msg.sender, 1_000_000 * 10**18);
    }
}
```

---

## Next Steps

Continue to [Gas Optimization](./16-web3-gas-optimization.md)