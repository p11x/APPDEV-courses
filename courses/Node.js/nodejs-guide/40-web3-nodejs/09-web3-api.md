# Web3 API Integration

## What You'll Learn

- REST API integration with blockchain data
- TheGraph and subgraphs
- Covalent API
- Multi-chain data aggregation

---

## Layer 1: API Providers

### TheGraph (Subgraphs)

```typescript
const SUBGRAPH_URL = 'https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v3';

async function querySubgraph(query: string, variables: object = {}) {
  const response = await fetch(SUBGRAPH_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ query, variables })
  });
  return response.json();
}

const query = `
  query GetPools($first: Int!) {
    pools(first: $first, orderBy: totalValueLockedUSD, orderDirection: desc) {
      id
      token0 { symbol }
      token1 { symbol }
      totalValueLockedUSD
    }
  }
`;
```

---

## Next Steps

Continue to [Wallet Connect](./10-web3-wallet-connect.md)