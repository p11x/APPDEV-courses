# Embedding Models

## What You'll Learn

- How to choose the right embedding model
- How embedding dimensions affect quality and cost
- How to compare models
- How to evaluate embedding quality

## Model Comparison

| Model | Dimensions | Max Tokens | Cost | Best For |
|-------|-----------|-----------|------|----------|
| `text-embedding-3-small` | 1536 | 8191 | $0.02/1M | General use |
| `text-embedding-3-large` | 3072 | 8191 | $0.13/1M | High accuracy |
| `text-embedding-ada-002` | 1536 | 8191 | $0.10/1M | Legacy |

## Evaluation

```ts
// evaluate.ts — Measure embedding quality

async function evaluateEmbeddings(model: string, testCases: TestCase[]) {
  let correct = 0;

  for (const { query, expectedDocs } of testCases) {
    const queryEmbedding = await getEmbedding(query, model);
    const results = await search(queryEmbedding, 5);
    const foundIds = results.map((r) => r.id);

    // Check if expected documents are in top 5
    const hits = expectedDocs.filter((id) => foundIds.includes(id));
    correct += hits.length / expectedDocs.length;
  }

  return correct / testCases.length;  // Recall@5
}
```

## Next Steps

For optimization, continue to [Embedding Optimization](./02-embedding-optimization.md).
