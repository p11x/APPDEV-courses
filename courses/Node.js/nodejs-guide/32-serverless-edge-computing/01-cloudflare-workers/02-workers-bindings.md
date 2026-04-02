# Cloudflare Workers Bindings

## What You'll Learn

- How to use KV (Key-Value) storage
- How to use D1 (SQLite) database
- How to use R2 (S3-compatible) object storage
- How to use Durable Objects for stateful edge computing

## KV (Key-Value Store)

```toml
# wrangler.toml
kv_namespaces = [
  { binding = "CACHE", id = "abc123..." }
]
```

```ts
// src/index.ts — Using KV

export default {
  async fetch(request: Request, env: Env): Promise<Response> {
    const url = new URL(request.url);

    // GET — read from KV
    if (request.method === 'GET') {
      const value = await env.CACHE.get(url.pathname);
      if (value) {
        return new Response(value, {
          headers: { 'X-Cache': 'HIT' },
        });
      }
      return new Response('Not cached', { status: 404 });
    }

    // PUT — write to KV
    if (request.method === 'PUT') {
      const body = await request.text();
      await env.CACHE.put(url.pathname, body, {
        expirationTtl: 3600,  // Expire in 1 hour
      });
      return Response.json({ stored: true });
    }

    // DELETE — remove from KV
    if (request.method === 'DELETE') {
      await env.CACHE.delete(url.pathname);
      return Response.json({ deleted: true });
    }

    return new Response('Method not allowed', { status: 405 });
  },
};

interface Env {
  CACHE: KVNamespace;
}
```

## D1 (SQLite Database)

```toml
# wrangler.toml
[[d1_databases]]
binding = "DB"
database_name = "my-db"
database_id = "abc123..."
```

```ts
// src/index.ts — Using D1

export default {
  async fetch(request: Request, env: Env): Promise<Response> {
    const url = new URL(request.url);

    // Create table (run once)
    if (url.pathname === '/setup') {
      await env.DB.prepare(`
        CREATE TABLE IF NOT EXISTS users (
          id INTEGER PRIMARY KEY AUTOINCREMENT,
          name TEXT NOT NULL,
          email TEXT UNIQUE NOT NULL,
          created_at DATETIME DEFAULT CURRENT_TIMESTAMP
        )
      `).run();
      return Response.json({ message: 'Table created' });
    }

    // List users
    if (url.pathname === '/users' && request.method === 'GET') {
      const { results } = await env.DB.prepare(
        'SELECT * FROM users ORDER BY created_at DESC'
      ).all();
      return Response.json(results);
    }

    // Create user
    if (url.pathname === '/users' && request.method === 'POST') {
      const { name, email } = await request.json();

      const result = await env.DB.prepare(
        'INSERT INTO users (name, email) VALUES (?, ?)'
      ).bind(name, email).run();

      return Response.json({ id: result.meta.last_row_id }, { status: 201 });
    }

    // Get user by ID
    if (url.pathname.match(/^\/users\/\d+$/) && request.method === 'GET') {
      const id = url.pathname.split('/').pop();
      const user = await env.DB.prepare(
        'SELECT * FROM users WHERE id = ?'
      ).bind(id).first();

      if (!user) {
        return Response.json({ error: 'Not found' }, { status: 404 });
      }
      return Response.json(user);
    }

    return new Response('Not Found', { status: 404 });
  },
};

interface Env {
  DB: D1Database;
}
```

## R2 (Object Storage)

```toml
# wrangler.toml
[[r2_buckets]]
binding = "BUCKET"
bucket_name = "my-bucket"
```

```ts
// src/index.ts — Using R2

export default {
  async fetch(request: Request, env: Env): Promise<Response> {
    const url = new URL(request.url);
    const key = url.pathname.slice(1);  // Remove leading /

    // Upload file
    if (request.method === 'PUT') {
      await env.BUCKET.put(key, request.body, {
        httpMetadata: {
          contentType: request.headers.get('Content-Type') || 'application/octet-stream',
        },
      });
      return Response.json({ uploaded: key });
    }

    // Download file
    if (request.method === 'GET') {
      const object = await env.BUCKET.get(key);
      if (!object) {
        return new Response('Not Found', { status: 404 });
      }

      return new Response(object.body, {
        headers: {
          'Content-Type': object.httpMetadata.contentType,
          'Content-Length': String(object.size),
          'ETag': object.httpEtag,
        },
      });
    }

    // List files
    if (url.pathname === '/' && request.method === 'GET') {
      const listed = await env.BUCKET.list();
      return Response.json({
        files: listed.objects.map((obj) => ({
          key: obj.key,
          size: obj.size,
          modified: obj.uploaded.toISOString(),
        })),
      });
    }

    return new Response('Method not allowed', { status: 405 });
  },
};

interface Env {
  BUCKET: R2Bucket;
}
```

## Durable Objects

```ts
// src/counter.ts — Durable Object (stateful edge computing)

export class Counter {
  private state: DurableObjectState;

  constructor(state: DurableObjectState) {
    this.state = state;
  }

  async fetch(request: Request): Promise<Response> {
    // Get current count from persistent storage
    let count = (await this.state.storage.get<number>('count')) || 0;

    if (request.method === 'POST') {
      count++;
      await this.state.storage.put('count', count);
    }

    return Response.json({ count });
  }
}
```

```toml
# wrangler.toml
[[durable_objects.bindings]]
name = "COUNTER"
class_name = "Counter"
```

## Next Steps

For runtime APIs, continue to [Workers Runtime APIs](./03-workers-runtime-apis.md).
