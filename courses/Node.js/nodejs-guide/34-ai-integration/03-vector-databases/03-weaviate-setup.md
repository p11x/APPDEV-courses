# Weaviate Setup

## What You'll Learn

- How to set up Weaviate for vector search
- How to use Weaviate's hybrid search
- How Weaviate compares to Pinecone and Chroma
- How to use Weaviate's built-in vectorizer

## Setup

```bash
npm install weaviate-client
```

```ts
// weaviate.ts

import weaviate from 'weaviate-client';

const client = weaviate.connectToWeaviateCloud(
  process.env.WEAVIATE_URL!,
  {
    authCredentials: new weaviate.ApiKey(process.env.WEAVIATE_API_KEY!),
  }
);

export { client };
```

## Create Collection

```ts
await client.collections.create({
  name: 'Document',
  properties: [
    { name: 'title', dataType: 'text' },
    { name: 'content', dataType: 'text' },
    { name: 'category', dataType: 'text' },
  ],
  // Use OpenAI for vectorization
  vectorizer: weaviate.configure.vectorizer.text2VecOpenAI({
    model: 'text-embedding-3-small',
  }),
});
```

## Add and Query

```ts
const docs = client.collections.get('Document');

// Add objects
await docs.data.insert({
  title: 'Node.js Guide',
  content: 'Node.js is a JavaScript runtime...',
  category: 'backend',
});

// Vector search
const results = await docs.query.nearText('JavaScript runtime', {
  limit: 5,
  returnMetadata: ['distance'],
});

// Hybrid search (keyword + vector)
const hybrid = await docs.query.hybrid('Node.js runtime', {
  limit: 5,
  alpha: 0.5,  // 0 = keyword only, 1 = vector only
});
```

## Next Steps

For vector search patterns, continue to [Vector Search](./04-vector-search.md).
