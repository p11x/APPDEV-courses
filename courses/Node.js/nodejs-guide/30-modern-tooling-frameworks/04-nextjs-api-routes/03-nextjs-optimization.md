# Next.js Optimization

## What You'll Learn

- How to optimize Next.js API routes
- Caching strategies with revalidation
- Database connection pooling
- Response streaming

## Caching with revalidate

```ts
// app/api/users/route.ts

import { NextResponse } from 'next/server';

// Cache for 60 seconds — returns cached data, revalidates in background
export async function GET() {
  const users = await fetchUsers();

  return NextResponse.json(users, {
    headers: {
      'Cache-Control': 'public, s-maxage=60, stale-while-revalidate=300',
    },
  });
}
```

## Database Connection Pooling

```ts
// lib/db.ts — Singleton connection pool

import { Pool } from 'pg';

// Singleton — reused across all requests
const pool = new Pool({
  connectionString: process.env.DATABASE_URL,
  max: 20,              // Max connections
  idleTimeoutMillis: 30_000,
  connectionTimeoutMillis: 5_000,
});

export async function query(sql: string, params?: unknown[]) {
  const client = await pool.connect();
  try {
    return await client.query(sql, params);
  } finally {
    client.release();  // Return connection to pool
  }
}
```

## Response Streaming

```ts
// app/api/export/route.ts

import { NextRequest } from 'next/server';

export async function GET(request: NextRequest) {
  // Stream a large CSV file
  const encoder = new TextEncoder();

  const stream = new ReadableStream({
    async start(controller) {
      controller.enqueue(encoder.encode('id,name,email\n'));

      for (let i = 0; i < 10000; i++) {
        controller.enqueue(encoder.encode(`${i},User${i},user${i}@example.com\n`));
      }

      controller.close();
    },
  });

  return new Response(stream, {
    headers: {
      'Content-Type': 'text/csv',
      'Content-Disposition': 'attachment; filename="users.csv"',
    },
  });
}
```

## Common Mistakes

### Mistake 1: Creating New Database Connections Per Request

```ts
// WRONG — new connection per request (slow, exhausting)
export async function GET() {
  const client = new Client({ connectionString: process.env.DATABASE_URL });
  await client.connect();
  const result = await client.query('SELECT * FROM users');
  await client.end();
  return NextResponse.json(result.rows);
}

// CORRECT — use a shared connection pool
export async function GET() {
  const result = await pool.query('SELECT * FROM users');
  return NextResponse.json(result.rows);
}
```

## Next Steps

For API patterns, continue to [Next.js API Patterns](./04-nextjs-api-patterns.md).
