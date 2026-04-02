# Hono Deployment

## What You'll Learn

- Deploying Hono to Cloudflare Workers
- Deploying Hono to Vercel Edge
- Deploying Hono to AWS Lambda
- Deploying Hono with Docker

## Cloudflare Workers

```ts
// src/index.ts

import { Hono } from 'hono';

const app = new Hono();

app.get('/', (c) => c.json({ message: 'Hello from Cloudflare Workers!' }));

// Export the fetch handler — Cloudflare Workers uses this
export default app;
```

```toml
# wrangler.toml
name = "hono-app"
main = "src/index.ts"
compatibility_date = "2024-01-01"
```

```bash
npm install -g wrangler
wrangler login
wrangler deploy
```

## Vercel Edge

```ts
// api/index.ts

import { Hono } from 'hono';
import { handle } from 'hono/vercel';

const app = new Hono();

app.get('/', (c) => c.json({ message: 'Hello from Vercel Edge!' }));

export const GET = handle(app);
export const POST = handle(app);
```

```json
// vercel.json
{
  "rewrites": [{ "source": "/(.*)", "destination": "/api" }]
}
```

## AWS Lambda

```ts
// lambda.ts

import { Hono } from 'hono';
import { handle } from 'hono/aws-lambda';

const app = new Hono();

app.get('/', (c) => c.json({ message: 'Hello from Lambda!' }));

export const handler = handle(app);
```

## Docker

```dockerfile
FROM oven/bun:1 AS base
WORKDIR /app

COPY package.json bun.lockb ./
RUN bun install --frozen-lockfile --production

COPY src ./src

USER bun
EXPOSE 3000

CMD ["bun", "run", "src/index.ts"]
```

```bash
docker build -t hono-app .
docker run -p 3000:3000 hono-app
```

## Next Steps

For TypeScript with Hono, continue to [Hono TypeScript](./04-hono-typescript.md).
