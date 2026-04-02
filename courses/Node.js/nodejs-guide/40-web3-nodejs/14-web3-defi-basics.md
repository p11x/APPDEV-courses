# DeFi Basics

## What You'll Learn

- Decentralized exchange concepts
- AMM (Automated Market Maker)
- Liquidity pools
- Token swaps

---

## Layer 1: Uniswap V3 Integration

```typescript
import { ethers } from 'ethers';
import IUniswapV3Pool from '@uniswap/v3-core/artifacts/contracts/interfaces/IUniswapV3Pool.sol';

const QUOTER_ABI = [
  'function quoteExactInputSingle((address tokenIn, address tokenOut, uint256 amountIn, uint24 fee, uint160 sqrtPriceLimitX96)) returns (uint256 amountOut)'
];

async function getSwapQuote() {
  const quoter = new ethers.Contract(QUOTER_ADDRESS, QUOTER_ABI, provider);
  const amountOut = await quoter.callStatic.quoteExactInputSingle({
    tokenIn: TOKEN_IN,
    tokenOut: TOKEN_OUT,
    amountIn: ethers.parseEther('1'),
    fee: 3000,
    sqrtPriceLimitX96: 0
  });
  return amountOut;
}
```
