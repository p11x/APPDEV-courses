# Web3 Testing

## What You'll Learn

- Testing smart contracts
- Unit tests with Hardhat
- Integration tests
- Fork testing

---

## Layer 1: Testing with Hardhat

```typescript
import { expect } from 'chai';
import { ethers } from 'hardhat';

describe('Token', () => {
  let token: any;
  let owner: any;
  let addr1: any;

  beforeEach(async () => {
    const Token = await ethers.getContractFactory('MyToken');
    token = await Token.deploy();
    [owner, addr1] = await ethers.getSigners();
  });

  it('should mint tokens to owner', async () => {
    const balance = await token.balanceOf(owner.address);
    expect(balance).to.equal(ethers.parseEther('1000000'));
  });

  it('should transfer tokens', async () => {
    await token.transfer(addr1.address, ethers.parseEther('100'));
    const balance = await token.balanceOf(addr1.address);
    expect(balance).to.equal(ethers.parseEther('100'));
  });
});
```
