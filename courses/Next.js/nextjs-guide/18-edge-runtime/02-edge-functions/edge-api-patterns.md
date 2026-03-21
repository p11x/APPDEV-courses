# Edge API Patterns

## What You'll Learn
- Common patterns for building Edge APIs
- Real-world use cases and solutions
- Best practices for Edge route handlers

## Prerequisites
- Understanding of Edge Route Handlers
- Knowledge of Edge middleware

## Do I Need This Right Now?
This file covers practical patterns you'll use frequently. If you're building any Edge-based API, these patterns will help you implement common features faster and more reliably.

## Concept Explained Simply

These are like recipes in a cookbook. Once you know the basic techniques, you can combine them to create more complex dishes. These patterns solve common problems you'll encounter when building Edge APIs.

## Complete Code Examples

### Pattern 1: JWT Verification at Edge

```typescript
// lib/edge-jwt.ts
import { jwtVerify, SignJWT } from 'jose';

const secret = new TextEncoder().encode(
  process.env.JWT_SECRET || 'your-secret-key'
);

export async function verifyToken(token: string) {
  try {
    const { payload } = await jwtVerify(token, secret);
    return payload;
  } catch {
    return null;
  }
}

export async function signToken(payload: Record<string, unknown>) {
  return new SignJWT(payload)
    .setProtectedHeader({ alg: 'HS256' })
    .setIssuedAt()
    .setExpirationTime('24h')
    .sign(secret);
}
```

```typescript
// app/api/auth/verify/route.ts
import { verifyToken } from '@/lib/edge-jwt';

export const runtime = 'edge';

export async function GET(request: Request) {
  const authHeader = request.headers.get('authorization');
  const token = authHeader?.replace('Bearer ', '');
  
  if (!token) {
    return Response.json({ error: 'No token provided' }, { status: 401 });
  }
  
  const payload = await verifyToken(token);
  
  if (!payload) {
    return Response.json({ error: 'Invalid token' }, { status: 401 });
  }
  
  return Response.json({ 
    valid: true, 
    user: payload 
  });
}
```

### Pattern 2: Request Validation

```typescript
// lib/edge-validation.ts
export function validateEmail(email: string): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

export function validateUrl(url: string): boolean {
  try {
    new URL(url);
    return true;
  } catch {
    return false;
  }
}

export interface ValidationError {
  field: string;
  message: string;
}

export function validateRequest(
  body: Record<string, unknown>,
  rules: Record<string, (value: unknown) => boolean>
): ValidationError[] {
  const errors: ValidationError[] = [];
  
  for (const [field, validator] of Object.entries(rules)) {
    if (!validator(body[field])) {
      errors.push({
        field,
        message: `Invalid value for ${field}`,
      });
    }
  }
  
  return errors;
}
```

```typescript
// app/api/users/route.ts
import { validateEmail, validateRequest } from '@/lib/edge-validation';

export const runtime = 'edge';

export async function POST(request: Request) {
  const body = await request.json();
  
  const errors = validateRequest(body, {
    email: (v) => typeof v === 'string' && validateEmail(v),
    name: (v) => typeof v === 'string' && v.length >= 2,
    age: (v) => typeof v === 'number' && v >= 18,
  });
  
  if (errors.length > 0) {
    return Response.json({ errors }, { status: 400 });
  }
  
  // Process valid request
  return Response.json({ success: true, data: body });
}
```

### Pattern 3: CORS Headers

```typescript
// app/api/cors-example/route.ts
export const runtime = 'edge';

const ALLOWED_ORIGINS = [
  'https://yourdomain.com',
  'https://app.yourdomain.com',
];

function getCorsHeaders(origin: string | null): Record<string, string> {
  const isAllowed = origin && ALLOWED_ORIGINS.includes(origin);
  
  return {
    'Access-Control-Allow-Origin': isAllowed ? origin : ALLOWED_ORIGINS[0],
    'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
    'Access-Control-Allow-Headers': 'Content-Type, Authorization',
    'Access-Control-Max-Age': '86400',
  };
}

export async function OPTIONS(request: Request) {
  const origin = request.headers.get('origin');
  
  return new Response(null, {
    status: 204,
    headers: getCorsHeaders(origin),
  });
}

export async function GET(request: Request) {
  const origin = request.headers.get('origin');
  
  return Response.json(
    { message: 'Hello with CORS!' },
    {
      headers: getCorsHeaders(origin),
    }
  );
}
```

### Pattern 4: Response Caching

```typescript
// app/api/cached-data/route.ts
export const runtime = 'edge';

// Cache configuration
const CACHE_CONTROL = {
  // Cache for 5 minutes, stale-while-revalidate for 1 minute
  'Cache-Control': 'public, s-maxage=300, stale-while-revalidate=60',
};

export async function GET(request: Request) {
  // Generate cache key from URL
  const url = new URL(request.url);
  const cacheKey = url.searchParams.get('key') || 'default';
  
  // In production, use a proper cache like Vercel KV or Redis
  // This is a simple in-memory example
  const cached = globalThis.cache?.get(cacheKey);
  
  if (cached && Date.now() < cached.expiry) {
    return Response.json(cached.data, {
      headers: {
        ...CACHE_CONTROL,
        'X-Cache': 'HIT',
      },
    });
  }
  
  // Fetch fresh data
  const data = {
    message: 'Fresh data',
    timestamp: Date.now(),
    random: Math.random(),
  };
  
  // Cache for 5 minutes
  if (!globalThis.cache) {
    globalThis.cache = new Map();
  }
  globalThis.cache.set(cacheKey, {
    data,
    expiry: Date.now() + 5 * 60 * 1000,
  });
  
  return Response.json(data, {
    headers: {
      ...CACHE_CONTROL,
      'X-Cache': 'MISS',
    },
  });
}
```

### Pattern 5: Proxy to External API

```typescript
// app/api/proxy/route.ts
export const runtime = 'edge';

export async function GET(request: Request) {
  const url = new URL(request.url);
  const targetUrl = url.searchParams.get('url');
  
  if (!targetUrl) {
    return Response.json({ error: 'Missing url parameter' }, { status: 400 });
  }
  
  // Validate URL to prevent SSRF attacks
  try {
    const parsed = new URL(targetUrl);
    if (!['http:', 'https:'].includes(parsed.protocol)) {
      return Response.json({ error: 'Invalid protocol' }, { status: 400 });
    }
  } catch {
    return Response.json({ error: 'Invalid URL' }, { status: 400 });
  }
  
  // Fetch from external API
  const response = await fetch(targetUrl, {
    headers: {
      'User-Agent': 'Next.js Edge',
      ...Object.fromEntries(
        Array.from(request.headers.entries()).filter(
          ([key]) => key.startsWith('x-')
        )
      ),
    },
  });
  
  // Return with CORS headers
  return new Response(response.body, {
    status: response.status,
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': response.headers.get('Content-Type') || 'application/json',
    },
  });
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `jwtVerify()` | Verifies JWT token | Secure authentication at edge |
| `jose` library | Edge-compatible JWT | Lightweight, works in Edge |
| `validateRequest()` | Validates input data | Prevents bad data from reaching your app |
| `'Access-Control-Allow-Origin'` | CORS header | Allows cross-origin requests |
| `'Cache-Control'` | Cache headers | Controls CDN caching behavior |
| `globalThis.cache` | Simple in-memory cache | Demonstrates caching concept |

## Common Mistakes

### Mistake #1: Using Node.js Crypto for JWT
```typescript
// Wrong: Node.js crypto doesn't work in Edge
import { sign, verify } from 'crypto';

export function verify(token: string) {
  // Won't work!
}
```

```typescript
// Correct: Use jose library (Edge-compatible)
import { jwtVerify } from 'jose';
```

### Mistake #2: Not Validating URLs (SSRF Vulnerability)
```typescript
// Wrong: Could allow attacks to internal services
const targetUrl = url.searchParams.get('url');
const response = await fetch(targetUrl); // Dangerous!
```

```typescript
// Correct: Always validate URLs
const parsed = new URL(targetUrl);
if (!['http:', 'https:'].includes(parsed.protocol)) {
  return Response.json({ error: 'Invalid protocol' }, { status: 400 });
}
```

### Mistake #3: CORS Preflight Not Handled
```typescript
// Wrong: Preflight requests will fail
export async function GET(request: Request) {
  // No OPTIONS handler!
  return Response.json({ hello: 'world' });
}
```

```typescript
// Correct: Handle OPTIONS for CORS
export async function OPTIONS(request: Request) {
  return new Response(null, {
    status: 204,
    headers: { 'Access-Control-Allow-Origin': '*' },
  });
}
```

## Summary
- Use jose library for JWT verification in Edge
- Always validate and sanitize input data
- Handle OPTIONS requests for CORS
- Use proper cache headers for performance
- Validate URLs to prevent SSRF attacks
- These patterns can be combined for complex APIs

## Next Steps
- [unsupported-apis.md](../03-edge-limitations/unsupported-apis.md) — What doesn't work in Edge
