# CDN Setup

## What You'll Learn

- How CDNs work with edge computing
- How to configure Cloudflare CDN
- How to set up origin shielding

## Cloudflare CDN

```ts
// Cache static assets at the edge

// wrangler.toml
[vars]
CACHE_TTL = "86400"

// Worker that caches responses
export default {
  async fetch(request, env) {
    // Let Cloudflare CDN handle caching for static assets
    return fetch(request, {
      cf: {
        cacheTtl: 86400,           // Cache for 24 hours
        cacheEverything: true,      // Cache HTML too
      },
    });
  },
};
```

## Next Steps

For cache invalidation, continue to [Cache Invalidation](./03-cache-invalidation.md).
