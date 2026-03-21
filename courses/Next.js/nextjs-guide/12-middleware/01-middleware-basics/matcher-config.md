# Middleware Matcher Config

## What You'll Learn
- How to configure which routes middleware runs on
- Different matcher patterns and syntax
- Best practices for efficient middleware matching

## Prerequisites
- Understanding of what middleware is
- Basic knowledge of URL patterns
- Familiarity with regular expressions (helpful but not required)

## Concept Explained Simply

Think of the matcher like a bouncer's guest list. The bouncer (middleware) only lets in people whose names are on the list. The matcher is that guest list — it tells Next.js exactly which URLs should run through your middleware code.

You can match exact paths like `/about`, patterns like `/dashboard/:path*`, or even use regular expressions. Being precise with matchers is important because middleware runs on every request, and you don't want to waste resources processing routes that don't need your middleware logic.

## Complete Code Example

```typescript
// middleware.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  // Your middleware logic here
  return NextResponse.next();
}

// Matcher with multiple configurations
export const config = {
  // Option 1: Array of specific paths
  matcher: ["/about", "/dashboard/:path*", "/api/auth/:path*"],
  
  // Option 2: Using source property with advanced patterns
  // matcher: [
  //   {
  //     source: "/((?!api|_next/static|_next/image|favicon.ico).*)",
  //     missing: [
  //       { type: "header", key: "next-router-prefetch" },
  //       { type: "header", key: "purpose", value: "prefetch" },
  //     ],
  //   },
  // ],
};
```

### More Complex Matcher Examples

```typescript
// Match all routes except static files
export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico|robots.txt|sitemap.xml).*)",
  ],
};

// Match only routes starting with /dashboard or /admin
export const config = {
  matcher: ["/dashboard/:path*", "/admin/:path*"],
};

// Match routes with optional segments
export const config = {
  matcher: ["/((?!api).*)"],
};

// Multiple matchers with different conditions
export const config = {
  matcher: [
    // Match all paths except API routes and static files
    {
      source: "/((?!api|_next/static).*)",
      missing: [
        { type: "header", key: "x-middleware-prefetch" },
      ],
    },
  ],
};
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `matcher: ["/about", ...]` | Array of path patterns | Defines which URLs trigger middleware |
| `"/dashboard/:path*"` | Dynamic segment pattern | Matches /dashboard/anything |
| `"/api/auth/:path*"` | API route subfolder | Targets auth-related API routes |
| `"source"` | Path pattern string | The URL pattern to match |
| `"missing"` | Array of conditions | Only match when these conditions are NOT met |
| `"type: "header""` | Check for header existence | Useful for excluding prefetch requests |
| `"purpose: "prefetch""` | Header value to check | Next.js sets this for prefetch requests |

## Common Mistakes

### Mistake 1: Too Broad Matcher

```typescript
// WRONG - Runs on absolutely everything
export const config = {
  matcher: ["/:path*"], // This includes /api, /_next, etc!
};

// CORRECT - Exclude what you don't need
export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico).*)",
  ],
};
```

### Mistake 2: Forgetting Static File Exclusions

```typescript
// WRONG - Middleware runs on every request including images
export const config = {
  matcher: ["/:path*"],
};

// Images get processed by middleware - wasteful!
```

### Mistake 3: Complex Regex Without Testing

```typescript
// WRONG - Complex regex that's hard to debug
export const config = {
  matcher: ["/^(?!.*\\.(js|css|png|jpg|jpeg|gif|svg|ico)$).*$"],
};

// CORRECT - Simpler, easier to understand
export const config = {
  matcher: [
    "/((?!_next/static|_next/image|favicon.ico).*)",
  ],
};
```

## Summary

- Always exclude static file paths (_next/static, _next/image, favicon.ico)
- Use simple path patterns when possible — complex regex is hard to maintain
- The matcher array can contain multiple patterns to match
- You can use regex-like patterns with :path* for dynamic segments
- Consider excluding API routes unless you specifically need to intercept them

## Next Steps

- [redirects-and-rewrites.md](../02-middleware-patterns/redirects-and-rewrites.md) - Learn to redirect and rewrite URLs in middleware
- [geolocation-based-routing.md](../03-middleware-advanced/geolocation-based-routing.md) - Use matcher with geolocation for routing
