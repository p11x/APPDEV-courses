# Hono Middleware

## What You'll Learn

- How Hono middleware works
- Built-in middleware
- Creating custom middleware
- Middleware ordering and composition

## Middleware Basics

```ts
// middleware/logger.ts

import { Context, Next } from 'hono';

export async function logger(c: Context, next: Next) {
  const start = Date.now();

  await next();  // Call next middleware/route handler

  const duration = Date.now() - start;
  console.log(`${c.req.method} ${c.req.path} - ${c.res.status} (${duration}ms)`);
}
```

```ts
// server.ts

import { Hono } from 'hono';
import { logger } from './middleware/logger.js';

const app = new Hono();

// Apply middleware globally
app.use('*', logger);

// Or apply to specific paths
app.use('/api/*', authMiddleware);
```

## Built-in Middleware

```ts
import { Hono } from 'hono';
import { cors } from 'hono/cors';
import { jwt } from 'hono/jwt';
import { rateLimiter } from 'hono-rate-limiter';  // bun add hono-rate-limiter
import { compress } from 'hono/compress';
import { etag } from 'hono/etag';

const app = new Hono();

// CORS
app.use('*', cors({ origin: '*' }));

// Compression
app.use('*', compress());

// ETag
app.use('*', etag());

// Rate limiting
app.use('*', rateLimiter({
  windowMs: 60_000,
  limit: 100,
  keyGenerator: (c) => c.req.header('x-forwarded-for') || c.req.ip,
}));

// JWT authentication
app.use('/api/*', jwt({ secret: process.env.JWT_SECRET! }));
```

## Custom Auth Middleware

```ts
// middleware/auth.ts

import { Context, Next } from 'hono';
import { jwt } from 'hono/jwt';

export function auth(secret: string) {
  return jwt({ secret });
}

// Role-based middleware
export function requireRole(role: string) {
  return async (c: Context, next: Next) => {
    const payload = c.get('jwtPayload');

    if (payload?.role !== role) {
      return c.json({ error: 'Forbidden' }, 403);
    }

    await next();
  };
}
```

```ts
// server.ts

import { auth, requireRole } from './middleware/auth.js';

app.use('/api/*', auth(process.env.JWT_SECRET!));
app.use('/api/admin/*', requireRole('admin'));
```

## Error Handling

```ts
// Global error handler
app.onError((err, c) => {
  console.error(err);

  return c.json({
    error: err.message,
    status: 500,
  }, 500);
});

// 404 handler
app.notFound((c) => {
  return c.json({ error: 'Not Found' }, 404);
});
```

## Next Steps

For deployment, continue to [Hono Deployment](./03-hono-deployment.md).
