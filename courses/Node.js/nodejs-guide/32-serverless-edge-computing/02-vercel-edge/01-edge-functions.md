# Vercel Edge Functions

## What You'll Learn

- What Vercel Edge Functions are
- How to create Edge Functions
- How Edge Functions differ from Serverless Functions
- How to use the Edge Runtime APIs

## What Are Edge Functions?

Vercel Edge Functions run on Vercel's Edge Network — globally distributed, close to users. They use the Web Standard API and run on the V8 runtime (like Cloudflare Workers).

| Feature | Serverless Function | Edge Function |
|---------|-------------------|---------------|
| Runtime | Node.js | V8 (Web Standard) |
| Cold start | 250ms+ | <1ms |
| Region | Single (closest to data) | Global (closest to user) |
| Size limit | 50MB | 4MB |
| Duration | 60s | 30s |
| Node.js APIs | Full | Limited |

## Setup

```ts
// app/api/hello/route.ts — Edge Runtime

export const runtime = 'edge';  // Use Edge Runtime instead of Node.js

export async function GET(request: Request) {
  return Response.json({
    message: 'Hello from the Edge!',
    timestamp: new Date().toISOString(),
    geo: request.geo,
  });
}
```

## API Routes with Edge Runtime

```ts
// app/api/users/route.ts

export const runtime = 'edge';

export async function GET() {
  const users = [
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' },
  ];

  return Response.json(users, {
    headers: {
      'Cache-Control': 'public, s-maxage=60',
    },
  });
}

export async function POST(request: Request) {
  const body = await request.json();

  if (!body.name) {
    return Response.json({ error: 'Name required' }, { status: 400 });
  }

  return Response.json({ id: 3, name: body.name }, { status: 201 });
}
```

## Using with Hono

```ts
// app/api/[...route]/route.ts

import { Hono } from 'hono';
import { handle } from 'hono/vercel';

export const runtime = 'edge';

const app = new Hono().basePath('/api');

app.get('/hello', (c) => c.json({ message: 'Hello from Hono on Vercel!' }));
app.get('/users', (c) => c.json([{ id: 1, name: 'Alice' }]));

export const GET = handle(app);
export const POST = handle(app);
```

## Common Mistakes

### Mistake 1: Using Node.js APIs in Edge Functions

```ts
// WRONG — Node.js APIs not available in Edge Runtime
import { readFile } from 'node:fs';  // Error!
import { createServer } from 'node:http';  // Error!

// CORRECT — use Web Standard APIs
export const runtime = 'edge';

export async function GET() {
  const response = await fetch('https://api.example.com/data');
  return Response.json(await response.json());
}
```

## Next Steps

For Edge Middleware, continue to [Edge Middleware](./02-edge-middleware.md).
