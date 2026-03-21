# Enabling Edge Runtime

## What You'll Learn
- Enable Edge Runtime in different parts of Next.js
- Configure edge for route handlers, middleware, and Server Components
- Understand automatic edge detection

## Prerequisites
- Understanding of Node.js vs Edge differences

## Do I Need This Right Now?
This is practical knowledge for implementing Edge features. Once you understand when to use Edge, you need to know how to enable it in your code. This is essential for any Edge-based feature.

## Concept Explained Simply

Enabling Edge Runtime is like choosing express shipping versus standard shipping. You have to explicitly choose it — it's not the default. In Next.js, most things run in Node.js by default, so you need to opt-in to Edge for each route, function, or component.

## Complete Code Examples

### 1. Edge API Routes (Route Handlers)

```typescript
// app/api/hello/route.ts
// Option 1: Using the runtime export
export const runtime = 'edge';

export async function GET(request: Request) {
  return Response.json({
    message: 'Hello from Edge!',
    timestamp: Date.now(),
  });
}

// Option 2: Alternative syntax
export const dynamic = 'force-dynamic';
// This will still use Node.js, not Edge

// For Edge specifically, you must use:
export const runtime = 'edge';
```

### 2. Edge Middleware

```typescript
// middleware.ts
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

// All middleware runs on Edge by default!
export function middleware(request: NextRequest) {
  // Get user's country from Vercel headers
  const country = request.geo?.country || 'US';
  
  // Simple A/B test based on country
  const bucket = country === 'US' ? 'A' : 'B';
  
  const response = NextResponse.next();
  
  // Set cookie for A/B testing
  response.cookies.set('ab-test-bucket', bucket);
  
  return response;
}

// Configure which routes to run middleware on
export const config = {
  matcher: '/:path*',
};
```

### 3. Edge Server Components

```typescript
// app/page.tsx
// This is a Server Component - but we can opt into Edge
export const runtime = 'edge';

async function getData() {
  // Edge-compatible fetch
  const response = await fetch('https://api.example.com/data', {
    cache: 'no-store',
  });
  return response.json();
}

export default async function Page() {
  const data = await getData();
  
  return (
    <main>
      <h1>Edge Server Component</h1>
      <p>Data: {JSON.stringify(data)}</p>
    </main>
  );
}
```

### 4. Edge Server Actions

```typescript
// app/actions.ts
'use server';

import { hash } from 'crypto';

// This runs on Edge
export async function hashPassword(password: string): Promise<string> {
  // Use Web Crypto API instead of Node.js crypto
  const encoder = new TextEncoder();
  const data = encoder.encode(password);
  const hashBuffer = await crypto.subtle.digest('SHA-256', data);
  const hashArray = Array.from(new Uint8Array(hashBuffer));
  return hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
}
```

### 5. Configure in next.config.js

```typescript
// next.config.ts
import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
  // Set default runtime for all API routes
  // But you can still override per-route
  experimental: {
    // Enable edge for server actions by default
    serverActions: {
      bodySizeLimit: '2mb',
    },
  },
  
  // Set edge as default for certain patterns
  async rewrites() {
    return [
      {
        source: '/api/edge/:path*',
        destination: '/api/:path*',
      },
    ];
  },
};

export default nextConfig;
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `export const runtime = 'edge'` | Opts route into Edge Runtime | Required for edge execution |
| `export const dynamic = 'force-dynamic'` | Makes route dynamic | Different from edge - still uses Node.js |
| `request.geo?.country` | Gets user location | Available in Edge middleware |
| `response.cookies.set()` | Sets cookies | Edge-compatible cookie handling |
| `crypto.subtle.digest()` | Hashes with Web Crypto | Node.js crypto doesn't work in Edge |
| `matcher: '/:path*'` | Configures which routes | Limits middleware scope |

## Common Mistakes

### Mistake #1: Confusing dynamic with edge
```typescript
// Wrong: This makes it dynamic Node.js, not Edge
export const dynamic = 'force-dynamic';

export async function GET() {
  return Response.json({ hello: 'world' });
}
```

```typescript
// Correct: Explicitly set runtime to edge
export const runtime = 'edge';

export async function GET() {
  return Response.json({ hello: 'world' });
}
```

### Mistake #2: Using Node.js Crypto
```typescript
// Wrong: Node.js crypto module not available
import { createHash } from 'crypto';

export const runtime = 'edge';

export async function POST(request: Request) {
  const hash = createHash('sha256').update(await request.text()).digest('hex');
  return Response.json({ hash });
}
```

```typescript
// Correct: Use Web Crypto API
export const runtime = 'edge';

export async function POST(request: Request) {
  const encoder = new TextEncoder();
  const data = encoder.encode(await request.text());
  const hashBuffer = await crypto.subtle.digest('SHA-256', data);
  const hashArray = Array.from(new Uint8Array(hashBuffer));
  const hash = hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
  return Response.json({ hash });
}
```

### Mistake #3: Forgetting Middleware Already Uses Edge
```typescript
// Redundant: Middleware is always edge
// This is fine but unnecessary
export const runtime = 'edge';

export function middleware(request: NextRequest) {
  // Already running on edge!
  return NextResponse.next();
}
```

## Summary
- Use `export const runtime = 'edge'` for Route Handlers and Server Components
- Middleware is automatically on Edge — no runtime export needed
- Use Web Crypto API instead of Node.js crypto in Edge
- Can set default runtime in next.config.ts but override per-route
- Edge functions are automatically detected and deployed to edge locations
- Always verify edge compatibility when using third-party packages

## Next Steps
- [edge-route-handlers.md](../02-edge-functions/edge-route-handlers.md) — Building API routes on Edge
