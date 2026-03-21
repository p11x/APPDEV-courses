# Chaining Middleware

## What You'll Learn
- Understanding middleware execution order
- Creating modular middleware for different concerns
- Using the `middleware` file to import and chain multiple functions
- Best practices for organizing middleware code

## Prerequisites
- Understanding of middleware basics
- Knowledge of how middleware runs (before page/API loads)
- Familiarity with Next.js project structure

## Concept Explained Simply

Think of middleware like a factory assembly line. Each station (middleware function) does one specific job — one checks quality, another adds a sticker, another packages the product. The product (request) moves through each station in order, and each station can modify it or decide to send it back.

In Next.js, while you typically have one `middleware.ts` file, you can organize your code by importing multiple handler functions and deciding which ones to run based on the route. This keeps your code clean and maintainable.

## Complete Code Example

```typescript
// middleware.ts - Main middleware that chains smaller functions
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

// Import your modular middleware functions
import { authMiddleware } from "./lib/middleware/auth";
import { loggingMiddleware } from "./lib/middleware/logging";
import { featureFlagMiddleware } from "./lib/middleware/feature-flags";
import { geoMiddleware } from "./lib/middleware/geo";

// Chain function - runs all middleware in order
export async function middleware(request: NextRequest) {
  // 1. Logging - always runs first (side effects)
  await loggingMiddleware(request);
  
  // 2. Feature flags - can short-circuit
  const featureResponse = await featureFlagMiddleware(request);
  if (featureResponse) {
    return featureResponse; // Stop the chain
  }
  
  // 3. Authentication - can redirect
  const authResponse = await authMiddleware(request);
  if (authResponse) {
    return authResponse; // Stop the chain
  }
  
  // 4. Geo - always runs last, adds headers
  return await geoMiddleware(request);
}

// Export matcher for the main middleware
export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico).*)",
  ],
};
```

### Modular Middleware Functions

```typescript
// lib/middleware/auth.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export async function authMiddleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  const publicPaths = ["/login", "/signup", "/about", "/"];
  
  // Skip auth check for public paths
  if (publicPaths.includes(pathname)) {
    return null; // Continue to next middleware
  }
  
  const token = request.cookies.get("auth-token");
  
  if (!token) {
    const loginUrl = new URL("/login", request.url);
    loginUrl.searchParams.set("redirect", pathname);
    return NextResponse.redirect(loginUrl);
  }
  
  return null; // Continue to next middleware
}
```

```typescript
// lib/middleware/logging.ts
import type { NextRequest } from "next/server";

export async function loggingMiddleware(request: NextRequest) {
  const start = Date.now();
  
  // Log request
  console.log(`${request.method} ${request.nextUrl.pathname}`);
  
  // You could add timing to response headers for performance monitoring
  // This runs but doesn't affect the response directly
}
```

```typescript
// lib/middleware/geo.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export async function geoMiddleware(request: NextRequest) {
  const response = NextResponse.next();
  
  // Add geo headers from Vercel
  if (request.geo) {
    response.headers.set("x-user-country", request.geo.country || "US");
    response.headers.set("x-user-city", request.geo.city || "Unknown");
    response.headers.set("x-user-region", request.geo.region || "Unknown");
  }
  
  return response;
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `export async function middleware()` | Main entry point | Next.js calls this for every matched request |
| `await loggingMiddleware(request)` | Run logging first | Always runs as it only does side effects |
| `if (featureResponse)` | Check for early exit | If non-null returned, stop the chain |
| `return null` | Continue chain | Null means "keep going to next middleware" |
| `return response` | Pass response forward | Final response after all middleware |
| `publicPaths` | Whitelist of paths | Routes that skip this middleware |

## Common Mistakes

### Mistake 1: Not Handling Async Properly

```typescript
// WRONG - Not awaiting async functions
export function middleware(request: NextRequest) {
  authMiddleware(request); // Fire and forget!
  return NextResponse.next();
}

// CORRECT - Always await async middleware
export async function middleware(request: NextRequest) {
  await authMiddleware(request);
  return NextResponse.next();
}
```

### Mistake 2: Wrong Order of Middleware

```typescript
// WRONG - Logging after redirect (might not run)
export async function middleware(request: NextRequest) {
  if (!isAuthenticated) {
    return NextResponse.redirect("/login");
  }
  await loggingMiddleware(request); // Might not run!
}

// CORRECT - Logging first (runs every time)
export async function middleware(request: NextRequest) {
  await loggingMiddleware(request); // Always runs first
  if (!isAuthenticated) {
    return NextResponse.redirect("/login");
  }
}
```

### Mistake 3: Creating Circular Dependencies

```typescript
// WRONG - Files importing each other
// auth.ts imports geo.ts
// geo.ts imports auth.ts
// This causes infinite loops!

// CORRECT - Keep middleware files independent
// auth.ts only handles auth
// geo.ts only handles geo
// Neither imports the other
```

## Summary

- Organize middleware into separate files by concern (auth, logging, geo)
- Use async/await to ensure proper execution order
- Return `null` to continue the chain, return a Response to stop
- Put logging/side-effect middleware first so it always runs
- Keep middleware functions independent to avoid circular imports
- Chain gives you flexibility to conditionally skip middleware

## Next Steps

- [a-b-testing-with-middleware.md](./a-b-testing-with-middleware.md) - Chaining applied to A/B testing
- [edge-middleware.md](../18-edge-runtime/02-edge-functions/edge-middleware.md) - Running middleware on Edge
