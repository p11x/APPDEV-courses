# Cloudflare Workers Runtime APIs

## What You'll Learn

- Web Standard APIs available in Workers
- How to use the Cache API
- How to use the WebSocket API
- How to use the Streams API

## Web Standard APIs

Workers support these Web Standard APIs (no Node.js):

| API | Available | Notes |
|-----|-----------|-------|
| `fetch()` | Yes | Full support |
| `Request`/`Response` | Yes | Core API |
| `URL`/`URLSearchParams` | Yes | Full support |
| `Headers` | Yes | Full support |
| `ReadableStream`/`WritableStream` | Yes | Full support |
| `TextEncoder`/`TextDecoder` | Yes | Full support |
| `crypto.subtle` | Yes | Web Crypto |
| `caches` | Yes | Cache API |
| `WebSocket` | Yes | Client + Server |
| `structuredClone` | Yes | Deep cloning |
| `setTimeout`/`setInterval` | Limited | Max 30 seconds |

## Cache API

```ts
// src/index.ts — Using the Cache API

export default {
  async fetch(request: Request): Promise<Response> {
    const cache = caches.default;

    // Check cache
    let response = await cache.match(request);
    if (response) {
      return response;
    }

    // Fetch from origin
    response = await fetch('https://api.example.com/data');

    // Clone and cache (response body can only be read once)
    const cachedResponse = new Response(response.clone().body, response);
    cachedResponse.headers.append('Cache-Control', 's-maxage=3600');

    // Store in cache (non-blocking)
    await cache.put(request, cachedResponse);

    return response;
  },
};
```

## WebSocket

```ts
// src/index.ts — WebSocket server

export default {
  async fetch(request: Request): Promise<Response> {
    const url = new URL(request.url);

    if (url.pathname === '/ws') {
      // Check for WebSocket upgrade
      const upgradeHeader = request.headers.get('Upgrade');
      if (upgradeHeader !== 'websocket') {
        return new Response('Expected WebSocket', { status: 400 });
      }

      // Create WebSocket pair
      const [client, server] = Object.values(new WebSocketPair());

      // Handle WebSocket connection
      server.accept();

      server.addEventListener('message', (event) => {
        console.log('Received:', event.data);
        server.send(`Echo: ${event.data}`);
      });

      server.addEventListener('close', () => {
        console.log('WebSocket closed');
      });

      // Return the client WebSocket to the browser
      return new Response(null, {
        status: 101,
        webSocket: client,
      });
    }

    return new Response('Not Found', { status: 404 });
  },
};
```

## Streams API

```ts
// src/index.ts — Streaming response

export default {
  async fetch(request: Request): Promise<Response> {
    // Create a readable stream
    const stream = new ReadableStream({
      async start(controller) {
        const encoder = new TextEncoder();

        // Send chunks over time
        for (let i = 0; i < 10; i++) {
          controller.enqueue(encoder.encode(`Chunk ${i}\n`));
          await new Promise((r) => setTimeout(r, 100));
        }

        controller.close();
      },
    });

    return new Response(stream, {
      headers: { 'Content-Type': 'text/plain' },
    });
  },
};
```

## Crypto API

```ts
// src/index.ts — Web Crypto

export default {
  async fetch(request: Request): Promise<Response> {
    const data = new TextEncoder().encode('Hello, Workers!');

    // SHA-256 hash
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashHex = hashArray.map((b) => b.toString(16).padStart(2, '0')).join('');

    return Response.json({ hash: hashHex });
  },
};
```

## Environment Variables

```toml
# wrangler.toml
[vars]
API_KEY = "secret"
ENVIRONMENT = "production"
```

```ts
export default {
  async fetch(request: Request, env: Env): Promise<Response> {
    // env.API_KEY is available
    return Response.json({
      environment: env.ENVIRONMENT,
      hasApiKey: !!env.API_KEY,
    });
  },
};

interface Env {
  API_KEY: string;
  ENVIRONMENT: string;
}
```

## Common Mistakes

### Mistake 1: Using Node.js Built-ins

```ts
// WRONG — not available in Workers
import { Buffer } from 'node:buffer';
import { setTimeout } from 'node:timers';

// CORRECT — use Web Standard alternatives
const buffer = new TextEncoder().encode('hello');
await new Promise((r) => setTimeout(r, 100));  // Works but limited to 30s
```

### Mistake 2: Not Using Streams for Large Responses

```ts
// WRONG — loading entire file into memory
const file = await env.BUCKET.get('large.zip');
const buffer = await file.arrayBuffer();  // 500MB in memory!

// CORRECT — stream the response
const file = await env.BUCKET.get('large.zip');
return new Response(file.body);  // Streamed, constant memory
```

## Next Steps

For best practices, continue to [Workers Best Practices](./04-workers-best-practices.md).
