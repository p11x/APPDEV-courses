# RAG Patterns

## What You'll Learn

- What RAG (Retrieval-Augmented Generation) is
- How to implement a RAG pipeline
- How to optimize RAG quality
- Common RAG pitfalls and solutions

## What Is RAG?

RAG combines **retrieval** (finding relevant documents) with **generation** (LLM creating responses). Instead of relying on the LLM's training data, you retrieve relevant context and include it in the prompt.

```
User question
  │
  ▼
Generate embedding → Search vector store → Get relevant chunks
  │
  ▼
Build prompt: "Answer based on: [chunks]. Question: [question]"
  │
  ▼
LLM generates answer grounded in retrieved context
```

## Complete RAG Pipeline

```ts
// rag.ts

import OpenAI from 'openai';
import { Pinecone } from '@pinecone-database/pinecone';

const openai = new OpenAI();
const pc = new Pinecone({ apiKey: process.env.PINECONE_API_KEY! });
const index = pc.index('docs');

async function rag(question: string): Promise<string> {
  // Step 1: Generate question embedding
  const embedding = await openai.embeddings.create({
    model: 'text-embedding-3-small',
    input: question,
  });

  // Step 2: Retrieve relevant documents
  const results = await index.query({
    vector: embedding.data[0].embedding,
    topK: 5,
    includeMetadata: true,
  });

  // Step 3: Build context from retrieved documents
  const context = results.matches
    .map((m, i) => `[${i + 1}] ${m.metadata?.content}`)
    .join('\n\n');

  // Step 4: Generate answer with context
  const completion = await openai.chat.completions.create({
    model: 'gpt-4o',
    messages: [
      {
        role: 'system',
        content: `Answer the question based ONLY on the provided context. If the context doesn't contain the answer, say "I don't have enough information."

Context:
${context}`,
      },
      { role: 'user', content: question },
    ],
    temperature: 0,
  });

  return completion.choices[0].message.content || '';
}
```

## Common Pitfalls

| Pitfall | Solution |
|---------|----------|
| Chunks too large | Split into 500-1000 characters |
| Chunks too small | Use overlap (200 chars) |
| No overlap | Context lost at chunk boundaries |
| Wrong embedding model | Match model to use case |
| No re-ranking | Use cross-encoder or LLM re-ranking |
| Hallucination | "Answer only from context" in prompt |

## Next Steps

For Next.js AI, continue to [Next.js AI Setup](../04-nextjs-ai/01-nextjs-ai-setup.md).
