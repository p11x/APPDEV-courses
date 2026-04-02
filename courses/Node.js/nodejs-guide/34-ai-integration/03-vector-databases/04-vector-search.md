# Vector Search Patterns

## What You'll Learn

- How to implement different search strategies
- How to combine keyword and vector search
- How to filter results by metadata
- How to optimize search performance

## Search Strategies

| Strategy | How It Works | Best For |
|----------|-------------|----------|
| Vector search | Semantic similarity | "Find similar documents" |
| Keyword search | Exact text matching | "Find documents containing X" |
| Hybrid search | Vector + keyword combined | Best of both worlds |
| Filtered search | Vector + metadata filters | "Find similar docs from chapter 5" |

## Hybrid Search

```ts
// hybrid-search.ts

import OpenAI from 'openai';
import { Pinecone } from '@pinecone-database/pinecone';

const openai = new OpenAI();
const pc = new Pinecone({ apiKey: process.env.PINECONE_API_KEY! });
const index = pc.index('docs');

async function hybridSearch(query: string, filters?: Record<string, unknown>, topK = 5) {
  // Generate embedding
  const embedding = await openai.embeddings.create({
    model: 'text-embedding-3-small',
    input: query,
  });

  // Vector search with metadata filter
  const results = await index.query({
    vector: embedding.data[0].embedding,
    topK,
    includeMetadata: true,
    filter: filters,  // e.g., { chapter: { $eq: 5 } }
  });

  return results.matches.map((match) => ({
    id: match.id,
    score: match.score,
    ...match.metadata,
  }));
}

// Usage
const results = await hybridSearch(
  'How to create middleware?',
  { chapter: { $gte: 5 } },  // Only chapters 5+
  10
);
```

## Re-ranking

```ts
// re-rank.ts — Re-rank search results with a cross-encoder

async function rerank(query: string, results: Array<{ text: string; score: number }>) {
  // Use OpenAI to re-rank results
  const prompt = `Query: "${query}"
  
Rank these documents by relevance (1 = most relevant):
${results.map((r, i) => `${i + 1}. ${r.text}`).join('\n')}

Return the document numbers in order of relevance, separated by commas:`;

  const response = await openai.chat.completions.create({
    model: 'gpt-4o-mini',
    messages: [{ role: 'user', content: prompt }],
    temperature: 0,
  });

  const order = response.choices[0].message.content
    ?.split(',')
    .map((n) => parseInt(n.trim()) - 1) || [];

  return order.map((i) => results[i]).filter(Boolean);
}
```

## Next Steps

For RAG patterns, continue to [RAG Patterns](./05-rag-patterns.md).
