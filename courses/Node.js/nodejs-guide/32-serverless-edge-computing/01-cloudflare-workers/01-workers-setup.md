# Cloudflare Workers Setup

## What You'll Learn

- What Cloudflare Workers are and how they work
- How to create and deploy a Worker
- How the Workers runtime differs from Node.js
- How to use Wrangler CLI for development

## What Are Cloudflare Workers?

Cloudflare Workers run JavaScript/TypeScript at the **edge** — on Cloudflare's global network of 300+ data centers. When a request hits your Worker, it executes at the data center closest to the user, typically under 5ms cold start.

| Feature | Node.js Server | Cloudflare Workers |
|---------|---------------|-------------------|
| Deployment | Your server | 300+ edge locations |
| Cold start | ~40ms | <5ms |
| Scaling | Manual/Auto | Automatic (zero config) |
| Pricing | Server cost | Per-request (free tier: 100K/day) |
| Runtime | V8/Node.js | V8 (Workers runtime) |
| API style | Express/Fastify | Web Standard (Request/Response) |

## Setup

```bash
# Install Wrangler CLI
npm install -g wrangler

# Login to Cloudflare
wrangler login

# Create a new project
wrangler init my-worker
cd my-worker
```

## Project Structure

```
my-worker/
├── src/
│   └── index.ts      → Worker entry point
├── wrangler.toml     → Configuration
├── package.json
└── tsconfig.json
```

## wrangler.toml

```toml
name = "my-worker"
main = "src/index.ts"
compatibility_date = "2024-01-01"
```

## Basic Worker

```ts
// src/index.ts — Cloudflare Worker

export default {
  // The fetch handler receives every incoming request
  async fetch(request: Request, env: Env, ctx: ExecutionContext): Promise<Response> {
    const url = new URL(request.url);

    // Route handling
    if (url.pathname === '/') {
      return new Response('Hello from Cloudflare Workers!', {
        headers: { 'Content-Type': 'text/plain' },
      });
    }

    if (url.pathname === '/api/time') {
      return Response.json({
        timestamp: new Date().toISOString(),
        cf: request.cf,  // Cloudflare-specific data (country, city, etc.)
      });
    }

    if (url.pathname === '/api/users' && request.method === 'GET') {
      const users = [
        { id: 1, name: 'Alice' },
        { id: 2, name: 'Bob' },
      ];
      return Response.json(users);
    }

    return new Response('Not Found', { status: 404 });
  },
};

// Type definitions for environment bindings
interface Env {
  // KV namespace, D1 database, R2 bucket, etc.
}
```

## Development

```bash
# Local development server
wrangler dev
# Opens at http://localhost:8787

# Deploy to Cloudflare
wrangler deploy

# View logs
wrangler tail
```

## Hono Integration

```ts
// src/index.ts — Using Hono with Workers

import { Hono } from 'hono';

const app = new Hono();

app.get('/', (c) => c.text('Hello from Hono on Workers!'));

app.get('/api/users', (c) => {
  return c.json([
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' },
  ]);
});

app.get('/api/geo', (c) => {
  const country = c.req.header('cf-ipcountry');
  const city = c.req.header('cf-ipcity');
  return c.json({ country, city });
});

// Export the fetch handler
export default app;
```

## Common Mistakes

### Mistake 1: Using Node.js APIs

```ts
// WRONG — Node.js APIs are not available in Workers
import { readFile } from 'node:fs';  // Error!
import { createServer } from 'node:http';  // Error!

// CORRECT — use Web Standard APIs
const response = await fetch('https://api.example.com/data');
const text = await response.text();
```

### Mistake 2: Not Handling Cold Starts

```ts
// WRONG — global state may not persist between requests
let counter = 0;  // May reset on cold start

// CORRECT — use Durable Objects or KV for persistent state
export default {
  async fetch(request, env) {
    const count = await env.COUNTER.get('count') || '0';
    await env.COUNTER.put('count', String(Number(count) + 1));
    return Response.json({ count });
  },
};
```

## Next Steps

For bindings, continue to [Workers Bindings](./02-workers-bindings.md).
