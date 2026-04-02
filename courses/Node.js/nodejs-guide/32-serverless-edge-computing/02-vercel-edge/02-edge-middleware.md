# Vercel Edge Middleware

## What You'll Learn

- How Vercel Edge Middleware works
- How to rewrite and redirect at the edge
- How to use geo-based routing
- How to implement edge rate limiting

## Basic Middleware

```ts
// middleware.ts — Runs at the edge before any route

import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export function middleware(request: NextRequest) {
  // Redirect unauthenticated users
  if (request.nextUrl.pathname.startsWith('/dashboard')) {
    const token = request.cookies.get('token')?.value;
    if (!token) {
      return NextResponse.redirect(new URL('/login', request.url));
    }
  }

  return NextResponse.next();
}

export const config = {
  matcher: ['/dashboard/:path*'],
};
```

## Geo-Based Routing

```ts
// middleware.ts

export function middleware(request: NextRequest) {
  const country = request.geo?.country || 'US';

  // Redirect to country-specific page
  if (country === 'DE') {
    return NextResponse.redirect(new URL('/de', request.url));
  }

  // Add geo headers
  const response = NextResponse.next();
  response.headers.set('x-country', country);
  response.headers.set('x-city', request.geo?.city || 'Unknown');
  return response;
}
```

## Edge Rate Limiting

```ts
// middleware.ts

const rateLimit = new Map<string, { count: number; resetAt: number }>();

export function middleware(request: NextRequest) {
  const ip = request.ip || request.headers.get('x-forwarded-for') || 'unknown';
  const now = Date.now();
  const windowMs = 60_000;
  const maxRequests = 100;

  let entry = rateLimit.get(ip);
  if (!entry || now > entry.resetAt) {
    entry = { count: 0, resetAt: now + windowMs };
    rateLimit.set(ip, entry);
  }

  entry.count++;

  if (entry.count > maxRequests) {
    return new Response(JSON.stringify({ error: 'Rate limited' }), {
      status: 429,
      headers: { 'Content-Type': 'application/json' },
    });
  }

  return NextResponse.next();
}
```

## Next Steps

For optimization, continue to [Edge Optimization](./03-edge-optimization.md).
