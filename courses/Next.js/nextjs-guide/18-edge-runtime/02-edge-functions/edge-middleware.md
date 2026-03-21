# Edge Middleware

## What You'll Learn
- Write middleware that runs on Edge
- Perform request transformations
- Implement geo-based routing and personalization

## Prerequisites
- Understanding of middleware basics
- Knowledge of Edge Runtime

## Do I Need This Right Now?
Edge middleware is one of the most powerful Edge features. Use it for authentication checks, A/B testing, geo-routing, rate limiting, and request transformations. If you need any of these, this is essential.

## Concept Explained Simply

Edge middleware is like a security checkpoint at an airport. Every flight (request) must pass through it before boarding (reaching your pages). The checkpoint can:
- Check if you have a valid ticket (authentication)
- Direct you to the right gate (routing)
- Give you special instructions (response modifications)

And it happens at the edge, close to where you are, so it's very fast.

## Complete Code Examples

### 1. Basic Authentication Middleware

```typescript
// middleware.ts
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export function middleware(request: NextRequest) {
  // Get token from cookies or headers
  const token = request.cookies.get('auth-token')?.value 
    || request.headers.get('authorization')?.replace('Bearer ', '');
  
  // Check if trying to access protected route
  const isProtectedRoute = request.nextUrl.pathname.startsWith('/dashboard');
  
  if (isProtectedRoute && !token) {
    // Redirect to login if no token
    const loginUrl = new URL('/login', request.url);
    loginUrl.searchParams.set('redirect', request.nextUrl.pathname);
    return NextResponse.redirect(loginUrl);
  }
  
  if (isProtectedRoute && token) {
    // Verify token (simplified - in production use proper JWT verification)
    const response = NextResponse.next();
    
    // Add user info to headers for downstream use
    response.headers.set('x-user-id', 'user-123');
    response.headers.set('x-user-role', 'member');
    
    return response;
  }
  
  return NextResponse.next();
}

// Configure which routes to run on
export const config = {
  matcher: [
    '/dashboard/:path*',
    '/api/protected/:path*',
  ],
};
```

### 2. Geo-Based Routing

```typescript
// middleware.ts
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export function middleware(request: NextRequest) {
  // Get user's location from Vercel headers
  const country = request.geo?.country || 'US';
  const city = request.geo?.city || 'Unknown';
  const region = request.geo?.region || 'Unknown';
  
  // Redirect users based on country
  // Example: Redirect to localized versions of the site
  if (request.nextUrl.pathname === '/' && country === 'DE') {
    const germanUrl = new URL('/de', request.url);
    return NextResponse.redirect(germanUrl);
  }
  
  if (request.nextUrl.pathname === '/' && country === 'FR') {
    const frenchUrl = new URL('/fr', request.url);
    return NextResponse.redirect(frenchUrl);
  }
  
  // Add location headers for downstream use
  const response = NextResponse.next();
  response.headers.set('x-user-country', country);
  response.headers.set('x-user-city', city);
  response.headers.set('x-user-region', region);
  
  return response;
}

export const config = {
  matcher: '/:path*',
};
```

### 3. A/B Testing Middleware

```typescript
// middleware.ts
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export function middleware(request: NextRequest) {
  const response = NextResponse.next();
  
  // Check if user already has a bucket assigned
  let bucket = request.cookies.get('ab-bucket')?.value;
  
  if (!bucket) {
    // Assign new bucket based on random assignment
    // 50/50 split
    bucket = Math.random() < 0.5 ? 'A' : 'B';
    
    // Set cookie for 30 days
    response.cookies.set('ab-bucket', bucket, {
      path: '/',
      maxAge: 60 * 60 * 24 * 30, // 30 days
      sameSite: 'lax',
    });
  }
  
  // Set header for downstream components
  response.headers.set('x-ab-bucket', bucket);
  
  return response;
}

export const config = {
  matcher: [
    '/:path*',
  ],
};
```

### 4. Rate Limiting at Edge

```typescript
// In-memory rate limiting (simplified)
// For production, use a distributed solution like Upstash

const rateLimitStore = new Map<string, { count: number; resetTime: number }>();

export function middleware(request: NextRequest) {
  const ip = request.ip || 'unknown';
  const now = Date.now();
  const windowMs = 60 * 1000; // 1 minute
  const limit = 100; // requests per window
  
  const record = rateLimitStore.get(ip);
  
  if (record && now < record.resetTime) {
    if (record.count >= limit) {
      return new NextResponse('Too Many Requests', {
        status: 429,
        headers: {
          'Retry-After': String(Math.ceil((record.resetTime - now) / 1000)),
          'X-RateLimit-Limit': String(limit),
          'X-RateLimit-Remaining': '0',
        },
      });
    }
    record.count++;
  } else {
    rateLimitStore.set(ip, { count: 1, resetTime: now + windowMs });
  }
  
  const response = NextResponse.next();
  response.headers.set('X-RateLimit-Limit', String(limit));
  response.headers.set('X-RateLimit-Remaining', String(limit - (record?.count || 1)));
  
  return response;
}

export const config = {
  matcher: '/api/:path*',
};
```

### 5. Bot Detection

```typescript
// middleware.ts
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

const BOT_USER_AGENTS = [
  'googlebot',
  'bingbot',
  'slurp',
  'duckduckbot',
  'baiduspider',
  'yandex',
  'facebookexternalhit',
  'twitterbot',
  'rogerbot',
  'linkedinbot',
  'embedly',
  'quora link preview',
  'showyoubot',
  'outbrain',
  'pinterest',
  'applebot',
  'discordbot',
  'telegrambot',
];

export function middleware(request: NextRequest) {
  const userAgent = request.headers.get('user-agent')?.toLowerCase() || '';
  
  const isBot = BOT_USER_AGENTS.some(bot => userAgent.includes(bot));
  
  const response = NextResponse.next();
  
  response.headers.set('x-is-bot', String(isBot));
  response.headers.set('x-user-agent', userAgent);
  
  // Optional: Block certain bots
  // if (isBot && shouldBlock(userAgent)) {
  //   return new NextResponse('Forbidden', { status: 403 });
  // }
  
  return response;
}

export const config = {
  matcher: '/:path*',
};
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `request.cookies.get()` | Reads cookies from request | Check authentication state |
| `request.geo?.country` | Gets user location | Enables geo-targeting |
| `NextResponse.next()` | Passes request through | Continues to destination |
| `NextResponse.redirect()` | Redirects user | For auth or geo-routing |
| `response.headers.set()` | Adds/modifies headers | Passes data to downstream |
| `request.nextUrl.pathname` | Gets request path | For conditional logic |
| `matcher` | Configures route matching | Limits middleware scope |

## Common Mistakes

### Mistake #1: Heavy Processing in Middleware
```typescript
// Wrong: Too much work in middleware slows everything down
export function middleware(request: NextRequest) {
  // Database call - too slow!
  const user = await db.user.findFirst();
  
  // Heavy computation
  const result = expensiveOperation();
  
  return NextResponse.next();
}
```

```typescript
// Correct: Lightweight operations only
export function middleware(request: NextRequest) {
  // Quick checks only
  const token = request.cookies.get('token')?.value;
  
  if (!token && request.nextUrl.pathname.startsWith('/protected')) {
    return NextResponse.redirect(new URL('/login', request.url));
  }
  
  return NextResponse.next();
}
```

### Mistake #2: Not Using matcher Configuration
```typescript
// Wrong: Runs on ALL routes, even static assets
export function middleware(request: NextRequest) {
  return NextResponse.next();
}
```

```typescript
// Correct: Only runs on specific routes
export const config = {
  matcher: ['/dashboard/:path*', '/api/:path*'],
};

export function middleware(request: NextRequest) {
  return NextResponse.next();
}
```

### Mistake #3: Blocking Streaming Responses
```typescript
// Wrong: Middleware can interfere with streaming
export function middleware(request: NextRequest) {
  const response = NextResponse.next();
  
  // This might break streaming!
  response.headers.set('Content-Length', '1000');
  
  return response;
}
```

## Summary
- Middleware runs before every request at the edge
- Great for authentication, A/B testing, geo-routing, rate limiting
- Keep middleware lightweight — no heavy processing or database calls
- Use `matcher` to limit which routes it runs on
- Can modify requests, responses, and redirect users
- Runs on all requests, so keep it fast
- Access user location via `request.geo`

## Next Steps
- [edge-api-patterns.md](./edge-api-patterns.md) — Common Edge API patterns
