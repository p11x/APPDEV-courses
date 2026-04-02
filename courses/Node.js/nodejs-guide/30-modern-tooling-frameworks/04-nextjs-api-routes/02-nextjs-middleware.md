# Next.js Middleware

## What You'll Learn

- How Next.js Edge Middleware works
- How to protect routes with middleware
- How to rewrite and redirect requests
- How middleware differs from route handlers

## Middleware File

Next.js middleware runs at the **edge** (on the CDN, before your server). Create a `middleware.ts` file in the project root:

```ts
// middleware.ts

import { NextRequest, NextResponse } from 'next/server';

export function middleware(request: NextRequest) {
  // Check if the user is authenticated
  const token = request.cookies.get('token')?.value;

  // Protected routes
  if (request.nextUrl.pathname.startsWith('/dashboard')) {
    if (!token) {
      // Redirect to login
      return NextResponse.redirect(new URL('/login', request.url));
    }
  }

  // API routes — check Authorization header
  if (request.nextUrl.pathname.startsWith('/api/protected')) {
    if (!token) {
      return NextResponse.json(
        { error: 'Unauthorized' },
        { status: 401 }
      );
    }
  }

  // Continue to the route handler
  return NextResponse.next();
}

// Configure which routes middleware runs on
export const config = {
  matcher: [
    '/dashboard/:path*',
    '/api/protected/:path*',
    '/((?!_next/static|_next/image|favicon.ico).*)',
  ],
};
```

## Request Rewriting

```ts
// middleware.ts — Rewrite requests to a different path

export function middleware(request: NextRequest) {
  // Rewrite /blog/:slug to /api/blog/:slug
  if (request.nextUrl.pathname.startsWith('/blog/')) {
    const slug = request.nextUrl.pathname.replace('/blog/', '');
    return NextResponse.rewrite(
      new URL(`/api/blog/${slug}`, request.url)
    );
  }
}
```

## Geo-Location and IP

```ts
// middleware.ts — Use geo data (Vercel Edge)

export function middleware(request: NextRequest) {
  const country = request.geo?.country || 'US';
  const city = request.geo?.city || 'Unknown';

  // Redirect based on location
  if (country === 'DE') {
    return NextResponse.redirect(new URL('/de', request.url));
  }

  // Add geo headers to the response
  const response = NextResponse.next();
  response.headers.set('x-country', country);
  response.headers.set('x-city', city);
  return response;
}
```

## Rate Limiting

```ts
// middleware.ts — Simple rate limiting at the edge

import { NextRequest, NextResponse } from 'next/server';

const rateLimit = new Map<string, { count: number; resetAt: number }>();

export function middleware(request: NextRequest) {
  const ip = request.ip || 'unknown';
  const now = Date.now();
  const windowMs = 60_000;  // 1 minute
  const maxRequests = 100;

  let entry = rateLimit.get(ip);

  if (!entry || now > entry.resetAt) {
    entry = { count: 0, resetAt: now + windowMs };
    rateLimit.set(ip, entry);
  }

  entry.count++;

  if (entry.count > maxRequests) {
    return NextResponse.json(
      { error: 'Too many requests' },
      { status: 429 }
    );
  }

  return NextResponse.next();
}
```

## Common Mistakes

### Mistake 1: Using Node.js APIs in Middleware

```ts
// WRONG — middleware runs at the edge, not on Node.js
import fs from 'node:fs';  // Not available at the edge!

export function middleware() {
  const data = fs.readFileSync('file.txt');  // Crashes
}

// CORRECT — only use Web Standard APIs in middleware
export function middleware(request: NextRequest) {
  // Use request.cookies, request.headers, request.nextUrl
}
```

### Mistake 2: Not Excluding Static Files

```ts
// WRONG — middleware runs on every request, including static files
// This adds latency to image and CSS loading

// CORRECT — exclude static assets from middleware
export const config = {
  matcher: [
    '/((?!_next/static|_next/image|favicon.ico).*)',
  ],
};
```

## Next Steps

For optimization, continue to [Next.js Optimization](./03-nextjs-optimization.md).
