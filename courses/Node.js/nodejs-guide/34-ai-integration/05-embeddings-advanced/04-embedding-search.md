# Embedding Search

## What You'll Learn

- How to implement fast embedding search
- How to use approximate nearest neighbor algorithms
- How to combine search with filtering
- How to benchmark search performance

## Search Implementation

```ts
// search.ts — Fast embedding search

import OpenAI from 'openai';
import { Pinecone } from '@pinecone-database/pinecone';

const openai = new OpenAI();
const pc = new Pinecone({ apiKey: process.env.PINECONE_API_KEY! });
const index = pc.index('docs');

async function search(query: string, options: {
  topK?: number;
  filter?: Record<string, unknown>;
  namespace?: string;
} = {}) {
  const { topK = 10, filter, namespace } = options;

  // Generate query embedding
  const embedding = await openai.embeddings.create({
    model: 'text-embedding-3-small',
    input: query,
  });

  // Search with optional filters
  const results = await index.query({
    vector: embedding.data[0].embedding,
    topK,
    includeMetadata: true,
    filter,
    namespace,
  });

  return results.matches.map((match) => ({
    id: match.id,
    score: match.score,
    content: match.metadata?.content,
    title: match.metadata?.title,
    metadata: match.metadata,
  }));
}

// Usage
const results = await search('How to create middleware?', {
  topK: 5,
  filter: { chapter: { $gte: 5 } },
});
```

## Benchmarking

```ts
// benchmark.ts

async function benchmarkSearch() {
  const queries = ['event loop', 'Express middleware', 'database setup'];
  const iterations = 100;

  for (const query of queries) {
    const start = performance.now();

    for (let i = 0; i < iterations; i++) {
      await search(query, { topK: 10 });
    }

    const elapsed = (performance.now() - start) / iterations;
    console.log(`"${query}": ${elapsed.toFixed(2)}ms avg`);
  }
}
```

## Next Steps

For AI monitoring, continue to [AI Metrics](../06-ai-monitoring/01-ai-metrics.md).
