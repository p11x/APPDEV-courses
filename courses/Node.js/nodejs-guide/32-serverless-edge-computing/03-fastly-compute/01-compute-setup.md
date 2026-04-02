# Fastly Compute Setup

## What You'll Learn

- What Fastly Compute is
- How to create a Fastly Compute service
- How Fastly's edge computing differs from Workers
- How to use the Compute@Edge SDK

## What Is Fastly Compute?

Fastly Compute runs custom code at the edge using **WebAssembly** (Wasm). Unlike Workers (V8), Fastly compiles your code to Wasm, offering consistent performance and support for multiple languages.

| Feature | Cloudflare Workers | Fastly Compute |
|---------|-------------------|----------------|
| Runtime | V8 | WebAssembly |
| Languages | JS/TS, Rust, C | Rust, JS, Go |
| Cold start | <5ms | <50μs |
| Streaming | Yes | Yes |
| KV Store | Workers KV | Edge Dictionary |

## Setup

```bash
# Install Fastly CLI
brew install fastly/tap/fastly

# Login
fastly profile create

# Create project
fastly compute init --from=https://github.com/fastly/compute-starter-kit-javascript-default

# Build and deploy
fastly compute build
fastly compute deploy
```

## Basic Service

```js
// src/index.js — Fastly Compute service

/// <reference types="@fastly/js-compute" />

addEventListener('fetch', (event) => {
  event.respondWith(handleRequest(event.request));
});

async function handleRequest(request) {
  const url = new URL(request.url);

  if (url.pathname === '/') {
    return new Response('Hello from Fastly Compute!', {
      headers: { 'Content-Type': 'text/plain' },
    });
  }

  if (url.pathname === '/api/data') {
    return Response.json({
      message: 'Data from the edge',
      pop: request.headers.get('fastly-ff'),  // Fastly POP info
    });
  }

  return new Response('Not Found', { status: 404 });
}
```

## Using Hono on Fastly

```js
// src/index.js

import { Hono } from 'hono';

const app = new Hono();

app.get('/', (c) => c.text('Hello from Hono on Fastly!'));
app.get('/api/time', (c) => c.json({ time: new Date().toISOString() }));

addEventListener('fetch', (event) => {
  event.respondWith(app.fetch(event.request));
});
```

## Next Steps

For patterns, continue to [Compute Patterns](./02-compute-patterns.md).
