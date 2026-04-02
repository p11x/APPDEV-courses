# Edge vs Node.js Runtime

## What You'll Learn

- When to use Edge vs Node.js runtime
- API differences between runtimes
- How to choose the right runtime for each route

## Comparison

| Feature | Node.js | Edge |
|---------|---------|------|
| Cold start | 250ms+ | <1ms |
| File system | Yes | No |
| Child processes | Yes | No |
| Node.js modules | Full | Limited |
| Web APIs | Available | Primary API |
| Duration | 60s | 30s |
| Memory | 1024MB | 128MB |
| Best for | Database, files | Auth, redirects, caching |

## When to Use Edge

- Authentication checks
- Geo-based redirects
- A/B testing
- Rate limiting
- Request rewriting
- Simple API responses

## When to Use Node.js

- Database queries (Prisma, Drizzle)
- File system operations
- External API calls that need connection pooling
- CPU-heavy operations
- Long-running tasks

## Hybrid Approach

```ts
// app/api/users/route.ts — Edge for auth, Node.js for DB

export const runtime = 'nodejs';  // Needs database access

export async function GET() {
  // This runs on Node.js (needs Prisma)
  const users = await prisma.user.findMany();
  return Response.json(users);
}
```

```ts
// middleware.ts — Edge for auth check

export function middleware(request: NextRequest) {
  // This runs at the edge (no database needed)
  const token = request.cookies.get('token')?.value;
  if (!token) {
    return NextResponse.redirect(new URL('/login', request.url));
  }
  return NextResponse.next();
}
```

## Next Steps

For Fastly Compute, continue to [Compute Setup](../03-fastly-compute/01-compute-setup.md).
