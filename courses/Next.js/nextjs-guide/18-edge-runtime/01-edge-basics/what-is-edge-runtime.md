# What Is Edge Runtime

## What You'll Learn
- Understanding Edge Runtime
- How it differs from Node.js
- Benefits and limitations
- When to use Edge

## Prerequisites
- Basic JavaScript knowledge
- Understanding of server-side rendering
- Familiarity with Next.js

## Concept Explained Simply

Edge Runtime is a lightweight JavaScript environment that runs your code closer to users — at the "edge" of the network. Instead of running in a data center far away, your code runs on servers distributed worldwide, close to where your users are.

Think of it like a franchise restaurant vs. one central kitchen. With traditional servers, everyone orders from one kitchen. With Edge, there are small kitchens everywhere, so food (your code) arrives faster.

## Edge vs Node.js

| Feature | Node.js | Edge Runtime |
|---------|---------|--------------|
| **Where it runs** | Server | Global edge locations |
| **Cold start** | Slower | Very fast |
| **API availability** | Full Node.js | Subset of Web APIs |
| **Response time** | Depends on location | Ultra-low latency |
| **Serverless** | Usually | Always |

## What's Available in Edge

```typescript
// These work in Edge:
- fetch API
- Web Streams
- Web Crypto (subtle)
- URL and URLSearchParams
- Headers (read-only in some places)

// These DON'T work in Edge:
- Node.js modules (fs, path, child_process)
- Many npm packages
- Dynamic requires
- setTimeout/setInterval (limited)

// Common compatible replacements:
import { createCookie, readCookie } from "next/headers"; // ✓ Edge
import fs from "fs"; // ✗ Not Edge
```

## When to Use Edge

### Great for Edge:
- Authentication/authorization
- Redirects and rewrites
- A/B testing
- Personalization
- Geo-based routing
- Simple API routes

### Don't Use Edge For:
- Heavy database operations
- Image processing
- Large file operations
- Complex computations
- Libraries without Edge support

## Complete Code Example

### Edge API Route

```typescript
// app/api/hello/route.ts
export const runtime = "edge";

export async function GET(request: Request) {
  return Response.json({
    message: "This runs on Edge!",
    region: request.headers.get("x-vercel-id")?.split("::")[0] || "unknown",
  });
}
```

### Edge Middleware

```typescript
// middleware.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export const runtime = "edge";

export function middleware(request: NextRequest) {
  const country = request.geo?.country || "US";
  
  // Fast geo-based redirect at the edge
  if (country === "DE" && !request.nextUrl.pathname.startsWith("/de")) {
    return NextResponse.redirect(new URL("/de", request.url));
  }
  
  return NextResponse.next();
}

export const config = {
  matcher: ["/((?!api|_next/static|_next/image).*)"],
};
```

### Edge with Streaming

```typescript
// app/api/stream/route.ts
export const runtime = "edge";

export async function GET() {
  const stream = new ReadableStream({
    start(controller) {
      const encoder = new TextEncoder();
      
      for (let i = 0; i < 10; i++) {
        controller.enqueue(encoder.encode(`data: ${i}\n\n`));
      }
      controller.close();
    },
  });
  
  return new Response(stream, {
    headers: {
      "Content-Type": "text/event-stream",
    },
  });
}
```

## Common Mistakes

### Mistake 1: Using Node.js APIs

```typescript
// WRONG - fs doesn't work in Edge
import { readFile } from "fs/promises";

export async function GET() {
  const data = await readFile("./data.json"); // Will fail!
}
```

### Mistake 2: Not Setting Runtime

```typescript
// WRONG - Defaults to Node.js
export async function GET() {
  return Response.json({ ok: true });
}

// CORRECT - Explicitly set Edge
export const runtime = "edge";

export async function GET() {
  return Response.json({ ok: true });
}
```

### Mistake 3: Using Incompatible Libraries

```typescript
// WRONG - moment.js doesn't work well in Edge
import moment from "moment";

// CORRECT - Use compatible alternatives
import { format } from "date-fns"; // Works in Edge!
```

## Summary

- Edge Runtime runs code globally, close to users
- Faster response times for distributed users
- Limited API availability (no Node.js modules)
- Perfect for middleware, redirects, auth
- Always specify `export const runtime = "edge"`

## Next Steps

- [nodejs-vs-edge-runtime.md](./nodejs-vs-edge-runtime.md) - Detailed comparison
- [edge-middleware.md](../02-edge-functions/edge-middleware.md) - Edge middleware
