# Request Memoization in Next.js

## What You'll Learn
- How Next.js memoizes requests
- Automatic deduplication

## Concept Explained Simply

Next.js automatically memoizes fetch requests within the same render pass. If you call the same URL twice, it only makes one network request.

```typescript
// Only ONE request is made!
const data1 = await fetch("/api/products");
const data2 = await fetch("/api/products"); // Uses cached result
```

This works across components too - if multiple components fetch the same data, it's only fetched once.
