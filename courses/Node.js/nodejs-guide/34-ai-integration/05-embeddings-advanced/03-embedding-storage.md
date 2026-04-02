# Embedding Storage

## What You'll Learn

- How to store embeddings efficiently
- How to choose between storage backends
- How to handle embedding updates
- How to manage embedding versions

## Storage Options

| Backend | Scale | Latency | Best For |
|---------|-------|---------|----------|
| In-memory | <100K | <1ms | Prototyping |
| PostgreSQL + pgvector | <1M | 5-50ms | Existing Postgres |
| Redis + RediSearch | <1M | 1-5ms | Low latency |
| Pinecone | Billions | 5-50ms | Managed scale |
| Chroma | <10M | 5-20ms | Self-hosted |

## PostgreSQL with pgvector

```ts
// pgvector.ts

import pg from 'pg';
const { Pool } = pg;

const pool = new Pool({ connectionString: process.env.DATABASE_URL });

// Create table with vector column
await pool.query(`
  CREATE TABLE IF NOT EXISTS documents (
    id SERIAL PRIMARY KEY,
    content TEXT,
    embedding vector(1536),
    metadata JSONB DEFAULT '{}'
  )
`);

// Insert embedding
await pool.query(
  'INSERT INTO documents (content, embedding, metadata) VALUES ($1, $2, $3)',
  [content, `[${embedding.join(',')}]`, JSON.stringify(metadata)]
);

// Query similar vectors
const results = await pool.query(`
  SELECT id, content, metadata,
         1 - (embedding <=> $1::vector) as similarity
  FROM documents
  ORDER BY embedding <=> $1::vector
  LIMIT 5
`, [`[${queryEmbedding.join(',')}]`]);
```

## Next Steps

For search, continue to [Embedding Search](./04-embedding-search.md).
