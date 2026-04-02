# Vector Storage

## What You'll Learn

- How to store embeddings for efficient retrieval
- How to use local vector storage with HNSW
- How to implement a simple vector store in Node.js
- How to choose between local and cloud vector databases

## Local Vector Store

```ts
// vector-store.ts — Simple in-memory vector store

interface VectorEntry {
  id: string;
  embedding: number[];
  metadata: Record<string, unknown>;
}

export class VectorStore {
  private entries: VectorEntry[] = [];

  // Add an embedding with metadata
  add(id: string, embedding: number[], metadata: Record<string, unknown> = {}) {
    this.entries.push({ id, embedding, metadata });
  }

  // Add multiple entries
  addBatch(items: Array<{ id: string; embedding: number[]; metadata?: Record<string, unknown> }>) {
    for (const item of items) {
      this.add(item.id, item.embedding, item.metadata || {});
    }
  }

  // Search for similar vectors
  search(queryEmbedding: number[], topK = 5): Array<{ id: string; score: number; metadata: Record<string, unknown> }> {
    const scores = this.entries.map((entry) => ({
      id: entry.id,
      score: this.cosineSimilarity(queryEmbedding, entry.embedding),
      metadata: entry.metadata,
    }));

    return scores.sort((a, b) => b.score - a.score).slice(0, topK);
  }

  // Delete an entry
  delete(id: string) {
    this.entries = this.entries.filter((e) => e.id !== id);
  }

  // Get count
  get size() {
    return this.entries.length;
  }

  private cosineSimilarity(a: number[], b: number[]): number {
    let dot = 0, normA = 0, normB = 0;
    for (let i = 0; i < a.length; i++) {
      dot += a[i] * b[i];
      normA += a[i] * a[i];
      normB += b[i] * b[i];
    }
    return dot / (Math.sqrt(normA) * Math.sqrt(normB));
  }
}
```

## Persistent Vector Store

```ts
// persistent-store.ts — Save/load vector store to disk

import { writeFile, readFile } from 'node:fs/promises';
import { existsSync } from 'node:fs';
import { VectorStore } from './vector-store.js';

export class PersistentVectorStore extends VectorStore {
  private filePath: string;

  constructor(filePath: string) {
    super();
    this.filePath = filePath;
  }

  async save() {
    const data = {
      entries: this.entries,
      savedAt: new Date().toISOString(),
    };
    await writeFile(this.filePath, JSON.stringify(data, null, 2));
  }

  async load() {
    if (!existsSync(this.filePath)) return;

    const data = JSON.parse(await readFile(this.filePath, 'utf-8'));
    this.entries = data.entries;
  }
}

// Usage
const store = new PersistentVectorStore('./vectors.json');
await store.load();

// Add embeddings
store.add('doc-1', embedding1, { title: 'Node.js Guide', chapter: 1 });
store.add('doc-2', embedding2, { title: 'Express Guide', chapter: 5 });

// Search
const results = store.search(queryEmbedding, 5);
console.log(results);

// Persist
await store.save();
```

## Comparison: Local vs Cloud

| Feature | Local (In-Memory) | Cloud (Pinecone/Weaviate) |
|---------|-------------------|---------------------------|
| Setup | No setup | API key, index creation |
| Scale | Limited by RAM | Billions of vectors |
| Latency | <1ms | 5-50ms |
| Cost | Free | Per-query pricing |
| Persistence | Manual (JSON) | Automatic |
| Best for | Prototyping, <100K vectors | Production, millions of vectors |

## Choosing a Vector Database

```
Need vector search?
├── < 100K vectors → Local store (in-memory + JSON)
├── 100K - 1M vectors → Chroma (self-hosted) or Qdrant
└── > 1M vectors → Pinecone (managed) or Weaviate
```

## Next Steps

For Pinecone, Chroma, and Weaviate, continue to [Vector Databases](../03-vector-databases/01-pinecone-setup.md).
