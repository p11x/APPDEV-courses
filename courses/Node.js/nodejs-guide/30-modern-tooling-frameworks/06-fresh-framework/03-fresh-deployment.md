# Fresh Deployment

## What You'll Learn

- How to deploy Fresh to Deno Deploy
- How to deploy Fresh with Docker
- How to configure Fresh for production

## Deno Deploy

```bash
# Install deployctl
deno install -gArf jsr:@deno/deployctl

# Deploy
deployctl deploy --project=my-fresh-app main.ts
```

```ts
// main.ts — Entry point for Deno Deploy

import { start } from '$fresh/server.ts';
import manifest from './fresh.gen.ts';

await start(manifest);
```

## Docker Deployment

```dockerfile
FROM denoland/deno:2.1.4

WORKDIR /app

# Cache dependencies
COPY deno.json deno.lock ./
RUN deno install

# Copy source
COPY . .

# Build (optional — for production optimizations)
RUN deno task build

# Run
EXPOSE 8000
CMD ["deno", "task", "start"]
```

```bash
docker build -t fresh-app .
docker run -p 8000:8000 fresh-app
```

## Production Configuration

```ts
// main.ts — Production-ready entry

import { start } from '$fresh/server.ts';
import manifest from './fresh.gen.ts';

await start(manifest, {
  port: Number(Deno.env.get('PORT')) || 8000,
  hostname: '0.0.0.0',
});
```

```jsonc
// deno.json
{
  "tasks": {
    "start": "deno run -A main.ts",
    "dev": "deno run -A --watch main.ts",
    "build": "deno run -A dev.ts build"
  }
}
```

## Next Steps

For comparison with Next.js, continue to [Fresh vs Next.js](./04-fresh-vs-nextjs.md).
