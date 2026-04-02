# Caching Implementation

## What You'll Learn

- Implementing Redis caching for the capstone API
- Cache invalidation strategies
- Response caching middleware

## Cache Middleware

```js
// middleware/cache.js
import Redis from 'ioredis';

const redis = new Redis();

export function cacheMiddleware(ttl = 300) {
  return async (req, res, next) => {
    const key = `cache:${req.originalUrl}`;

    const cached = await redis.get(key);
    if (cached) {
      return res.json(JSON.parse(cached));
    }

    // Override res.json to cache the response
    const originalJson = res.json.bind(res);
    res.json = (data) => {
      redis.set(key, JSON.stringify(data), 'EX', ttl);
      originalJson(data);
    };

    next();
  };
}

// Usage
app.get('/api/bookmarks', cacheMiddleware(60), async (req, res) => {
  const bookmarks = await db.bookmark.findMany();
  res.json(bookmarks);
});
```

## Next Steps

For optimization techniques, continue to [Optimization Techniques](./02-optimization-techniques.md).
