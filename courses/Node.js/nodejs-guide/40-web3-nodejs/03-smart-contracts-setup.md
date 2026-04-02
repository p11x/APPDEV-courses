# Smart Contracts Setup

## What You'll Learn

- Setting up a development environment for smart contracts
- Installing Hardhat or Foundry
- Writing your first Solidity contract
- Compiling and deploying

---

## Layer 1: Development Environment

### Installing Hardhat

```bash
# Create project directory
mkdir my-nft-project && cd my-nft-project
npm init -y

# Install Hardhat
npm install --save-dev hardhat @nomicfoundation/hardhat-toolbox

# Initialize Hardhat
npx hardhat init

# Select "Create a JavaScript project"
```

### Project Structure

```
contracts/
├── Token.sol
├── NFT.sol
└── YourContract.sol

scripts/
├── deploy.js
└── interact.js

test/
└── your-contract.js

hardhat.config.js
```

### Configuration

```javascript
// hardhat.config.js
require('@nomicfoundation/hardhat-toolbox');

/** @type import('hardhat/config').HardhatUserConfig */
module.exports = {
  solidity: '0.8.19',
  networks: {
    sepolia: {
      url: process.env.SEPOLIA_RPC_URL,
      accounts: [process.env.PRIVATE_KEY]
    },
    localhost: {
      url: 'http://127.0.0.1:8545'
    }
  }
};
```

---

## Layer 2: Writing a Smart Contract

### Basic ERC-20 Token

```solidity
// contracts/Token.sol
// SPDX-License-Identifier: MIT
pragma solidity ^0.8.19;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";

contract MyToken is ERC20 {
    uint8 private _decimals;
    
    constructor(
        string memory name,
        string memory symbol,
        uint8 decimals_
    ) ERC20(name, symbol) {
        _decimals = decimals_;
    }
    
    function mint(address to, uint256 amount) external {
        _mint(to, amount);
    }
    
    function decimals() public view override returns (uint8) {
        return _decimals;
    }
}
```

---

## Layer 3: Deployment

### Deploy Script

```javascript
// scripts/deploy.js
const hre = require('hardhat');

async function main() {
  const Token = await hre.ethers.getContractFactory('MyToken');
  const token = await Token.deploy('My Token', 'MTK', 18);
  
  await token.waitForDeployment();
  const address = await token.getAddress();
  
  console.log('Token deployed to:', address);
}

main()
  .then(() => process.exit(0))
  .catch((error) => {
    console.error(error);
    process.exit(1);
  });
```

---

## Next Steps

Continue to [Ethers Provider](./04-ethers-provider.md)