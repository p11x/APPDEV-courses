# Edge Cache Strategies

## What You'll Learn

- Cache strategies for edge computing
- Cache-Control header patterns
- Stale-while-revalidate
- Edge caching with KV

## Cache-Control Patterns

```ts
// Static assets — cache for 1 year
res.headers.set('Cache-Control', 'public, max-age=31536000, immutable');

// API responses — cache for 1 minute, serve stale for 5 minutes
res.headers.set('Cache-Control', 'public, s-maxage=60, stale-while-revalidate=300');

// Private data — no caching
res.headers.set('Cache-Control', 'private, no-cache, no-store');

// CDN-only caching (not browser)
res.headers.set('Cache-Control', 'public, s-maxage=3600, max-age=0');
```

## Edge Cache with KV

```ts
// Cache API responses at the edge

export default {
  async fetch(request, env) {
    const cacheKey = new URL(request.url).pathname;

    // Check edge cache
    const cached = await env.CACHE.get(cacheKey, 'json');
    if (cached) {
      return Response.json(cached, {
        headers: { 'X-Cache': 'HIT' },
      });
    }

    // Fetch from origin
    const data = await fetchFromOrigin(request);

    // Store in edge cache
    await env.CACHE.put(cacheKey, JSON.stringify(data), {
      expirationTtl: 3600,
    });

    return Response.json(data, {
      headers: { 'X-Cache': 'MISS' },
    });
  },
};
```

## Next Steps

For CDN setup, continue to [CDN Setup](./02-cdn-setup.md).
