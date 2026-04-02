# Blockchain Indexing

## What You'll Learn

- Indexing blockchain data
- TheGraph subgraphs
- Custom indexers
- Real-time updates

---

## Layer 1: Indexing Strategies

```typescript
// simple-indexer.ts
class BlockchainIndexer {
  private provider: ethers.JsonRpcProvider;
  private db: Database;
  private lastBlock: number;

  async start() {
    this.lastBlock = await this.getLastProcessedBlock();
    
    this.provider.on('block', async (blockNumber) => {
      await this.processBlock(blockNumber);
      await this.saveLastProcessedBlock(blockNumber);
    });
  }

  async processBlock(blockNumber: number) {
    const block = await this.provider.getBlock(blockNumber, true);
    for (const txHash of block?.transactions || []) {
      const tx = await this.provider.getTransaction(txHash);
      if (tx?.to === CONTRACT_ADDRESS) {
        await this.indexTransaction(tx);
      }
    }
  }
}
```
