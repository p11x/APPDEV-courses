# Data Cache in Next.js

## What You'll Learn
- How the Data Cache works
- Persistent caching across requests

## Concept Explained Simply

The Data Cache stores fetched data persistently across requests. Once data is fetched, it's cached and reused for subsequent requests until it expires or is revalidated.

```typescript
// First request - fetches from API
const posts = await fetch("/api/posts", { cache: "force-cache" });

// Subsequent requests - uses cached data
const posts = await fetch("/api/posts", { cache: "force-cache" });
```

The cache is shared across all users and persists between server restarts.
