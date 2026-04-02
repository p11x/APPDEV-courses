# Cloudflare Workers Best Practices

## What You'll Learn

- Performance optimization for Workers
- Error handling patterns
- Security best practices
- Cost optimization

## Performance

```ts
// Use Hono for routing (fastest edge framework)
import { Hono } from 'hono';

const app = new Hono();

// Cache static responses
app.get('/api/config', async (c) => {
  return c.json({ version: '1.0.0' }, {
    headers: { 'Cache-Control': 'public, max-age=3600' },
  });
});
```

## Error Handling

```ts
// src/index.ts — Global error handler

export default {
  async fetch(request: Request, env: Env, ctx: ExecutionContext): Promise<Response> {
    try {
      // Your handler logic
      return await handleRequest(request, env);
    } catch (err) {
      // Log to external service
      ctx.waitUntil(
        fetch('https://logs.example.com', {
          method: 'POST',
          body: JSON.stringify({
            error: err.message,
            stack: err.stack,
            url: request.url,
          }),
        })
      );

      return Response.json(
        { error: 'Internal Server Error' },
        { status: 500 }
      );
    }
  },
};
```

## Security

```ts
// Security headers middleware
app.use('*', async (c, next) => {
  await next();

  c.res.headers.set('X-Content-Type-Options', 'nosniff');
  c.res.headers.set('X-Frame-Options', 'DENY');
  c.res.headers.set('Referrer-Policy', 'strict-origin-when-cross-origin');
  c.res.headers.delete('Server');  // Remove Cloudflare server header
});

// Rate limiting with KV
app.use('/api/*', async (c, next) => {
  const ip = c.req.header('cf-connecting-ip') || 'unknown';
  const key = `ratelimit:${ip}`;

  const current = await env.CACHE.get(key);
  if (current && Number(current) > 100) {
    return c.json({ error: 'Rate limited' }, 429);
  }

  await env.CACHE.put(key, String(Number(current || 0) + 1), {
    expirationTtl: 60,
  });

  await next();
});
```

## Cost Optimization

| Optimization | Impact |
|-------------|--------|
| Cache responses in KV | Reduces origin requests |
| Use streaming | Reduces memory usage |
| Minimize CPU time | Reduces billable duration |
| Use Durable Objects sparingly | They have higher cost per request |
| Batch KV operations | Reduces KV read/write costs |

## Common Mistakes

### Mistake 1: Not Using ctx.waitUntil()

```ts
// WRONG — logging may not complete before response is sent
fetch('https://logs.example.com', { body: JSON.stringify(log) });

// CORRECT — use ctx.waitUntil() for non-blocking tasks
ctx.waitUntil(
  fetch('https://logs.example.com', { body: JSON.stringify(log) })
);
```

### Mistake 2: Synchronous CPU-Heavy Operations

```ts
// WRONG — blocks the Worker (10ms CPU limit per request)
for (let i = 0; i < 1_000_000; i++) {
  Math.sqrt(i);
}

// CORRECT — offload to Durable Objects or pre-compute
```

## Next Steps

For Vercel Edge, continue to [Vercel Edge Functions](../02-vercel-edge/01-edge-functions.md).
