# Hono Basics

## What You'll Learn

- What Hono is and why it exists
- How to create a Hono server
- How Hono runs on multiple runtimes
- How to define routes with type safety

## What Is Hono?

Hono is an **ultrafast web framework** that runs on every JavaScript runtime — Node.js, Bun, Deno, Cloudflare Workers, AWS Lambda, Vercel Edge, and more. It uses the Web Standard `Request`/`Response` API, making it runtime-agnostic.

| Feature | Express | Fastify | Hono |
|---------|---------|---------|------|
| Requests/sec | 15,000 | 35,000 | 45,000+ |
| Bundle size | ~200KB | ~150KB | ~14KB |
| TypeScript | @types | Built-in | Built-in |
| Runtimes | Node.js | Node.js | Node.js, Deno, Bun, Workers, Lambda |
| API style | Express | Express-like | Web Standard |

## Setup

```bash
mkdir hono-app && cd hono-app
bun init -y
bun add hono
```

## Basic Server

```ts
// server.ts — Hono server

import { Hono } from 'hono';
import { serve } from '@hono/node-server';  // bun add @hono/node-server

const app = new Hono();

// Routes
app.get('/', (c) => {
  return c.json({ message: 'Hello from Hono!' });
});

app.get('/users', (c) => {
  return c.json([
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' },
  ]);
});

app.get('/users/:id', (c) => {
  const id = c.req.param('id');  // Fully typed!
  return c.json({ id: Number(id), name: 'Alice' });
});

app.post('/users', async (c) => {
  const body = await c.req.json();
  return c.json({ id: 3, ...body }, 201);
});

// Start server
serve({
  fetch: app.fetch,
  port: 3000,
});

console.log('Hono server on http://localhost:3000');
```

## Type-Safe Routes

```ts
// typed-routes.ts — Full type inference

import { Hono } from 'hono';

type User = {
  id: number;
  name: string;
  email: string;
};

const app = new Hono()
  .get('/users/:id', (c) => {
    const id = c.req.param('id');  // string (typed from the route pattern)

    const user: User = {
      id: Number(id),
      name: 'Alice',
      email: 'alice@example.com',
    };

    return c.json(user);  // Return type is inferred
  })
  .post('/users', async (c) => {
    const body = await c.req.json<{ name: string; email: string }>();

    const user: User = { id: 1, ...body };
    return c.json(user, 201);
  });
```

## Context Object

```ts
app.get('/example', (c) => {
  // c is the Context object — it provides everything you need:

  c.req.param('id');        // Route parameters
  c.req.query('page');      // Query parameters
  c.req.header('Auth');     // Request headers
  await c.req.json();       // Request body
  c.req.url;                // Full URL
  c.req.method;             // HTTP method

  c.json({ data: [] });     // JSON response
  c.text('Hello');           // Text response
  c.html('<h1>Hello</h1>'); // HTML response
  c.redirect('/other');     // Redirect
  c.status(201);            // Set status code
  c.header('X-Custom', '1'); // Set response header
});
```

## Running on Different Runtimes

```ts
// Node.js
import { serve } from '@hono/node-server';
serve({ fetch: app.fetch, port: 3000 });

// Bun
export default { port: 3000, fetch: app.fetch };

// Cloudflare Workers
export default { fetch: app.fetch };

// Deno
Deno.serve({ port: 3000 }, app.fetch);
```

## Next Steps

For middleware, continue to [Hono Middleware](./02-hono-middleware.md).
