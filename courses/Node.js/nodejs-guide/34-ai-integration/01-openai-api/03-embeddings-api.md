# Embeddings API

## What You'll Learn

- What embeddings are and why they matter
- How to generate embeddings with OpenAI
- How to use embeddings for semantic search
- How to store and query embeddings

## What Are Embeddings?

Embeddings are numerical representations of text that capture semantic meaning. Similar texts have similar embeddings, enabling semantic search (find meaning, not just keywords).

```
"dog" → [0.2, -0.1, 0.8, ...]
"puppy" → [0.19, -0.12, 0.78, ...]  ← Similar to "dog"
"car" → [-0.5, 0.3, -0.2, ...]      ← Different from "dog"
```

## Generate Embeddings

```ts
// embeddings.ts

import openai from './openai.js';

async function getEmbedding(text: string): Promise<number[]> {
  const response = await openai.embeddings.create({
    model: 'text-embedding-3-small',  // 1536 dimensions, fast and cheap
    input: text,
  });

  return response.data[0].embedding;
}

// Batch embeddings (more efficient)
async function getEmbeddings(texts: string[]): Promise<number[][]> {
  const response = await openai.embeddings.create({
    model: 'text-embedding-3-small',
    input: texts,  // Send multiple texts at once
  });

  return response.data.map((item) => item.embedding);
}

// Usage
const embedding = await getEmbedding('What is Node.js?');
console.log(`Embedding dimensions: ${embedding.length}`);  // 1536
console.log(`First 5 values:`, embedding.slice(0, 5));
```

## Semantic Search

```ts
// semantic-search.ts

import openai from './openai.js';

interface Document {
  id: string;
  text: string;
  embedding: number[];
}

// Cosine similarity between two embeddings
function cosineSimilarity(a: number[], b: number[]): number {
  let dotProduct = 0;
  let normA = 0;
  let normB = 0;

  for (let i = 0; i < a.length; i++) {
    dotProduct += a[i] * b[i];
    normA += a[i] * a[i];
    normB += b[i] * b[i];
  }

  return dotProduct / (Math.sqrt(normA) * Math.sqrt(normB));
}

// Index documents with embeddings
async function indexDocuments(texts: string[]): Promise<Document[]> {
  const embeddings = await getEmbeddings(texts);

  return texts.map((text, i) => ({
    id: String(i),
    text,
    embedding: embeddings[i],
  }));
}

// Search for similar documents
async function search(
  query: string,
  documents: Document[],
  topK = 5
): Promise<{ text: string; score: number }[]> {
  const queryEmbedding = await getEmbedding(query);

  const scores = documents.map((doc) => ({
    text: doc.text,
    score: cosineSimilarity(queryEmbedding, doc.embedding),
  }));

  return scores
    .sort((a, b) => b.score - a.score)
    .slice(0, topK);
}

// Usage
const docs = await indexDocuments([
  'Node.js is a JavaScript runtime built on V8',
  'Express is a web framework for Node.js',
  'React is a frontend library by Meta',
  'TypeScript adds types to JavaScript',
  'Docker containers package applications',
]);

const results = await search('What is a Node.js framework?', docs);

console.log('Search results:');
results.forEach((r, i) => {
  console.log(`  ${i + 1}. [${r.score.toFixed(3)}] ${r.text}`);
});
// 1. [0.892] Express is a web framework for Node.js
// 2. [0.845] Node.js is a JavaScript runtime built on V8
// 3. [0.612] TypeScript adds types to JavaScript
```

## Embedding Models

| Model | Dimensions | Best For | Cost |
|-------|-----------|----------|------|
| `text-embedding-3-small` | 1536 | General use | $0.02/1M tokens |
| `text-embedding-3-large` | 3072 | High accuracy | $0.13/1M tokens |
| `text-embedding-ada-002` | 1536 | Legacy | $0.10/1M tokens |

## Next Steps

For function calling, continue to [Function Calling](./04-function-calling.md).
