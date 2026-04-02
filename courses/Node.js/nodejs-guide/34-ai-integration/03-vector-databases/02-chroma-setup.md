# Chroma Setup

## What You'll Learn

- How to set up Chroma for local vector search
- How to use Chroma with OpenAI embeddings
- How Chroma compares to Pinecone
- How to persist Chroma data

## Setup

```bash
npm install chromadb
```

```ts
// chroma.ts

import { ChromaClient } from 'chromadb';

const client = new ChromaClient();

// Create or get collection
const collection = await client.getOrCreateCollection({
  name: 'documents',
  metadata: { description: 'My document embeddings' },
});

export { collection };
```

## Add Documents

```ts
import OpenAI from 'openai';
import { collection } from './chroma.js';

const openai = new OpenAI();

// Generate embeddings
const docs = [
  'Node.js is a JavaScript runtime',
  'Express is a web framework',
  'React is a UI library',
];

const response = await openai.embeddings.create({
  model: 'text-embedding-3-small',
  input: docs,
});

// Add to Chroma
await collection.add({
  ids: ['doc-1', 'doc-2', 'doc-3'],
  embeddings: response.data.map((d) => d.embedding),
  documents: docs,
  metadatas: [
    { topic: 'runtime' },
    { topic: 'framework' },
    { topic: 'library' },
  ],
});
```

## Query

```ts
// Generate query embedding
const queryEmbedding = await openai.embeddings.create({
  model: 'text-embedding-3-small',
  input: 'What is a Node.js framework?',
});

// Query Chroma
const results = await collection.query({
  queryEmbeddings: queryEmbedding.data[0].embedding,
  nResults: 3,
});

console.log(results.documents);  // [['Express is a web framework', ...]]
console.log(results.distances);  // [[0.12, 0.45, ...]]
```

## Next Steps

For Weaviate, continue to [Weaviate Setup](./03-weaviate-setup.md).
