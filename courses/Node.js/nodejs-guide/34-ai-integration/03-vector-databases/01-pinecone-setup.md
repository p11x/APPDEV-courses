# Pinecone Setup

## What You'll Learn

- How to set up Pinecone for vector search
- How to create and manage indexes
- How to upsert and query vectors
- How Pinecone compares to other vector databases

## Setup

```bash
npm install @pinecone-database/pinecone
```

```ts
// pinecone.ts

import { Pinecone } from '@pinecone-database/pinecone';

const pc = new Pinecone({
  apiKey: process.env.PINECONE_API_KEY!,
});

export { pc };
```

## Create Index

```ts
import { pc } from './pinecone.js';

// Create a serverless index
await pc.createIndex({
  name: 'my-index',
  dimension: 1536,  // Must match embedding dimensions
  metric: 'cosine',  // cosine, euclidean, dotproduct
  spec: {
    serverless: {
      cloud: 'aws',
      region: 'us-east-1',
    },
  },
});
```

## Upsert Vectors

```ts
const index = pc.index('my-index');

// Upsert vectors
await index.upsert([
  {
    id: 'doc-1',
    values: embedding1,  // 1536-dimensional array
    metadata: { title: 'Node.js Guide', chapter: 1 },
  },
  {
    id: 'doc-2',
    values: embedding2,
    metadata: { title: 'Express Guide', chapter: 5 },
  },
]);
```

## Query Vectors

```ts
const index = pc.index('my-index');

// Query similar vectors
const results = await index.query({
  vector: queryEmbedding,
  topK: 5,
  includeMetadata: true,
});

for (const match of results.matches) {
  console.log(`Score: ${match.score}, ID: ${match.id}, Title: ${match.metadata?.title}`);
}
```

## Next Steps

For Chroma, continue to [Chroma Setup](./02-chroma-setup.md).
