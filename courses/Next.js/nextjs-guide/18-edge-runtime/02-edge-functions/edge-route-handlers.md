# Edge Route Handlers

## What You'll Learn
- Create API routes that run on Edge
- Use Edge-specific APIs in route handlers
- Build fast, globally distributed APIs

## Prerequisites
- Understanding of Edge Runtime fundamentals
- Knowledge of Next.js Route Handlers

## Do I Need This Right Now?
Edge route handlers are perfect for high-performance APIs that need low latency worldwide. If you're building authentication, A/B testing, or simple transformation APIs, this is essential. For complex database operations, stick with Node.js routes.

## Concept Explained Simply

Edge Route Handlers are like having a fast-food restaurant in every city instead of one central kitchen. When someone orders (makes a request), it's handled at the nearest location (edge location) instead of traveling all the way to the main restaurant (origin server). This makes responses much faster.

## Complete Code Examples

### 1. Basic Edge Route Handler

```typescript
// app/api/edge-hello/route.ts
export const runtime = 'edge';

export async function GET(request: Request) {
  // Get edge location info
  const edgeLocation = request.headers.get('x-vercel-id')?.split('::')[0] || 'unknown';
  
  return Response.json({
    message: 'Hello from the Edge!',
    edgeLocation,
    timestamp: Date.now(),
    runtime: 'edge',
  });
}
```

### 2. A/B Testing with Edge

```typescript
// app/api/ab-test/route.ts
export const runtime = 'edge';

export async function GET(request: Request) {
  // Get user's country for geographic targeting
  const country = request.geo?.country || 'US';
  const city = request.geo?.city || 'Unknown';
  
  // Simple geo-based bucketing
  // In production, use a more sophisticated algorithm
  const variant = country === 'US' ? 'us_variant' : 'global_variant';
  
  // Check for existing variant cookie
  const cookieHeader = request.headers.get('cookie');
  const existingVariant = cookieHeader?.match(/ab_variant=([^;]+)/)?.[1];
  
  const finalVariant = existingVariant || variant;
  
  return Response.json({
    variant: finalVariant,
    country,
    city,
    message: `You're seeing the ${finalVariant} experience`,
  }, {
    headers: {
      'Set-Cookie': `ab_variant=${finalVariant}; Path=/; Max-Age=2592000`,
      'Cache-Control': 'no-store',
    },
  });
}
```

### 3. Request Transformation

```typescript
// app/api/transform/route.ts
export const runtime = 'edge';

export async function POST(request: Request) {
  // Parse incoming JSON
  const body = await request.json();
  
  // Transform data (lightweight processing at edge)
  const transformed = {
    id: crypto.randomUUID(),
    ...body,
    processedAt: new Date().toISOString(),
    processedAtEdge: true,
  };
  
  // Forward to your main API or database
  // This keeps sensitive logic in your main region
  const response = await fetch('https://api.example.com/webhooks', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'X-Edge-Processed': 'true',
    },
    body: JSON.stringify(transformed),
  });
  
  return Response.json({
    success: true,
    originalData: body,
    transformedData: transformed,
    upstreamResponse: await response.text(),
  });
}
```

### 4. Rate Limiting (Edge)

```typescript
// lib/rate-limit.ts
// Simple in-memory rate limiter for edge
// Note: Doesn't work perfectly across multiple edge instances
// For production, use a service like Upstash or Vercel KV

const requests = new Map<string, { count: number; resetTime: number }>();

export function checkRateLimit(ip: string, limit: number = 100, windowMs: number = 60000): boolean {
  const now = Date.now();
  const record = requests.get(ip);
  
  if (!record || now > record.resetTime) {
    requests.set(ip, { count: 1, resetTime: now + windowMs });
    return true;
  }
  
  if (record.count >= limit) {
    return false;
  }
  
  record.count++;
  return true;
}

// Cleanup old entries periodically
setInterval(() => {
  const now = Date.now();
  for (const [ip, record] of requests.entries()) {
    if (now > record.resetTime) {
      requests.delete(ip);
    }
  }
}, 60000);
```

```typescript
// app/api/rate-limited/route.ts
import { checkRateLimit } from '@/lib/rate-limit';

export const runtime = 'edge';

export async function GET(request: Request) {
  // Get client IP from various headers
  const ip = request.headers.get('x-forwarded-for')?.split(',')[0]?.trim() 
    || request.headers.get('x-real-ip') 
    || 'unknown';
  
  const allowed = checkRateLimit(ip);
  
  if (!allowed) {
    return Response.json(
      { error: 'Too many requests' },
      { 
        status: 429,
        headers: {
          'Retry-After': '60',
        },
      }
    );
  }
  
  return Response.json({
    message: 'Request allowed',
    ip,
  });
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `export const runtime = 'edge'` | Enables Edge Runtime | Required for edge execution |
| `request.geo?.country` | Gets user's country | Available in Edge, great for geo-targeting |
| `crypto.randomUUID()` | Generates UUID | Web Crypto API in Edge |
| `request.headers.get('x-vercel-id')` | Gets edge location | Identifies which edge handled the request |
| `'Cache-Control': 'no-store'` | Disables caching | Important for dynamic edge responses |

## Common Mistakes

### Mistake #1: Trying to Use Database Directly
```typescript
// Wrong: Database connections don't work in Edge
export const runtime = 'edge';

import { PrismaClient } from '@prisma/client';

const prisma = new PrismaClient();

export async function GET() {
  const users = await prisma.user.findMany(); // Won't work!
  return Response.json(users);
}
```

```typescript
// Correct: Call a Node.js API or use Edge-compatible storage
export const runtime = 'edge';

export async function GET() {
  // Call your main API
  const response = await fetch('https://api.example.com/users');
  const users = await response.json();
  
  return Response.json(users);
}
```

### Mistake #2: Not Handling Headers Properly
```typescript
// Wrong: Assuming all headers are available
export const runtime = 'edge';

export async function GET(request: Request) {
  const userId = request.headers.get('x-user-id');
  // May be undefined - need to handle!
  return Response.json({ userId });
}
```

```typescript
// Correct: Always provide fallbacks
export const runtime = 'edge';

export async function GET(request: Request) {
  const userId = request.headers.get('x-user-id') || 'anonymous';
  return Response.json({ userId });
}
```

### Mistake #3: Caching Edge Responses Unintentionally
```typescript
// Wrong: Default caching might cause issues
export const runtime = 'edge';

export async function GET(request: Request) {
  return Response.json({
    timestamp: Date.now(), // Will be cached!
  });
}
```

```typescript
// Correct: Disable caching for dynamic content
export const runtime = 'edge';

export async function GET(request: Request) {
  return Response.json({
    timestamp: Date.now(),
  }, {
    headers: {
      'Cache-Control': 'no-store, no-cache, must-revalidate',
    },
  });
}
```

## Summary
- Edge route handlers run at CDN edge locations worldwide
- Use `request.geo` for geographic data
- Great for auth, A/B testing, rate limiting, request transformation
- Cannot connect to databases directly — use API calls instead
- Always set appropriate cache headers for dynamic content
- Use Web Crypto API instead of Node.js crypto

## Next Steps
- [edge-middleware.md](./edge-middleware.md) — Edge middleware patterns
