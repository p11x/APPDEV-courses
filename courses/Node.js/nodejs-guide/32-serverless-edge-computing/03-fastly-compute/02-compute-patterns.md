# Fastly Compute Patterns

## What You'll Learn

- Request routing patterns
- Origin shielding
- Cache control at the edge
- Error handling

## Request Routing

```js
// src/index.js

addEventListener('fetch', (event) => {
  event.respondWith(handleRequest(event.request));
});

async function handleRequest(request) {
  const url = new URL(request.url);

  // Route to different backends
  if (url.pathname.startsWith('/api/')) {
    return fetch(new Request('https://api-backend.example.com' + url.pathname, request));
  }

  if (url.pathname.startsWith('/static/')) {
    // Serve from cache with long TTL
    return fetch(request, {
      backend: 'static-origin',
      cacheOverride: new CacheOverride('override', { ttl: 86400 }),
    });
  }

  return fetch(request, { backend: 'origin' });
}
```

## Next Steps

For deployment, continue to [Compute Deployment](./03-compute-deployment.md).
