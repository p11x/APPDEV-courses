# Embedding Optimization

## What You'll Learn

- How to reduce embedding costs
- How to cache embeddings
- How to batch embedding requests
- How to use dimension reduction

## Cost Optimization

```ts
// Batch embeddings (more efficient)
const embeddings = await openai.embeddings.create({
  model: 'text-embedding-3-small',
  input: texts,  // Batch up to 2048 texts per request
});

// Use smaller model for less critical use cases
const cheapEmbeddings = await openai.embeddings.create({
  model: 'text-embedding-3-small',  // vs large
});

// Truncate long texts
const truncated = text.slice(0, 8000);  // Max 8191 tokens
```

## Caching

```ts
// Cache embeddings to avoid re-computing
import Redis from 'ioredis';
import crypto from 'node:crypto';

const redis = new Redis();

async function getCachedEmbedding(text: string): Promise<number[]> {
  const hash = crypto.createHash('sha256').update(text).digest('hex');
  const key = `embedding:${hash}`;

  const cached = await redis.get(key);
  if (cached) return JSON.parse(cached);

  const embedding = await getEmbedding(text);
  await redis.set(key, JSON.stringify(embedding), 'EX', 86400 * 7);  // 7 days

  return embedding;
}
```

## Next Steps

For storage, continue to [Embedding Storage](./03-embedding-storage.md).
