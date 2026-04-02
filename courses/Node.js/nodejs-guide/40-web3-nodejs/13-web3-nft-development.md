# NFT Development

## What You'll Learn

- ERC-721 and ERC-1155 standards
- Minting NFTs
- Metadata management
- Marketplace integration

---

## Layer 1: NFT Standards

### ERC-721 Contract

```solidity
// contracts/MyNFT.sol
// SPDX-License-Identifier: MIT
pragma solidity ^0.8.19;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

contract MyNFT is ERC721, ERC721URIStorage, Ownable {
    uint256 private _nextTokenId;
    
    constructor() ERC721("MyNFT", "MNFT") Ownable(msg.sender) {}
    
    function mintNFT(address to, string memory uri) external onlyOwner returns (uint256) {
        uint256 tokenId = _nextTokenId++;
        _mint(to, tokenId);
        _setTokenURI(tokenId, uri);
        return tokenId;
    }
    
    function tokenURI(uint256 tokenId) public view override returns (string memory) {
        return super.tokenURI(tokenId);
    }
}
```

---

## Next Steps

Continue to [DeFi Basics](./14-web3-defi-basics.md)