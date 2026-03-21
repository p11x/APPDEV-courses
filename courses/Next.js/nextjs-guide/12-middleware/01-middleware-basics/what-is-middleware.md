# Middleware Basics

## What You'll Learn
- What middleware is and when to use it
- How to create a middleware function
- The role of matcher config in controlling which routes run middleware

## Prerequisites
- Understanding of the request/response cycle
- Basic familiarity with Next.js App Router
- Knowledge of TypeScript basics

## Concept Explained Simply

Think of middleware like a security guard at the entrance of a building. Every request that comes to your application passes through the middleware first — just like every person entering a building passes through security. The guard can:
- Let people in (pass the request through)
- Turn people away (redirect or block the request)
- Check someone's ID (verify authentication)
- Add a sticker to their badge (modify request headers)

Middleware runs before a page or API route loads. It's perfect for things you need to do for EVERY request, like checking if a user is logged in, redirecting based on where they're from, or adding extra information to requests.

## Complete Code Example

```typescript
// middleware.ts (must be at root of your project)
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

// This function runs before every request
export function middleware(request: NextRequest) {
  // Get the pathname from the URL
  const { pathname } = request.nextUrl;
  
  // Check if user has a session token
  const sessionToken = request.cookies.get("session-token");
  
  // If trying to access protected route without session
  if (pathname.startsWith("/dashboard") && !sessionToken) {
    // Redirect to login page
    const loginUrl = new URL("/login", request.url);
    loginUrl.searchParams.set("redirect", pathname);
    return NextResponse.redirect(loginUrl);
  }
  
  // If user is logged in but tries to access login page
  if (pathname === "/login" && sessionToken) {
    // Redirect to dashboard
    return NextResponse.redirect(new URL("/dashboard", request.url));
  }
  
  // Allow the request to continue
  return NextResponse.next();
}

// Matcher config - controls which routes this middleware runs on
export const config = {
  matcher: [
    /*
     * Match all request paths except for the ones starting with:
     * - api (API routes)
     * - _next/static (static files)
     * - _next/image (image optimization files)
     * - favicon.ico (browser icon)
     */
    "/((?!api|_next/static|_next/image|favicon.ico).*)",
  ],
};
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `import { NextResponse } from "next/server"` | Imports Next.js response helper | Needed to return responses from middleware |
| `import type { NextRequest } from "next/server"` | Imports request type for TypeScript | Gives you type safety for request objects |
| `export function middleware(request: NextRequest)` | Defines the middleware function | Every request passes through this function |
| `request.nextUrl` | Gets the URL from the request | Allows you to inspect and modify the URL |
| `request.cookies.get("session-token")` | Reads a cookie from the request | Access session data to check authentication |
| `NextResponse.redirect(url)` | Creates a redirect response | Sends user to a different page |
| `NextResponse.next()` | Passes request to next step | Allows request to continue to the page/endpoint |
| `export const config` | Defines middleware configuration | Controls which routes the middleware applies to |
| `matcher` | Array of route patterns | Only matching routes run this middleware |

## Common Mistakes

### Mistake 1: Not Using Matcher (Running on All Routes)

```typescript
// WRONG - Middleware runs on every single request including static files
export function middleware(request: NextRequest) {
  // This runs for /, /api, /_next/image, everything!
  console.log("Middleware running");
}

// CORRECT - Use matcher to control which routes
export const config = {
  matcher: ["/((?!api|_next/static|favicon.ico).*)"],
};
```

### Mistake 2: Blocking Static Files

```typescript
// WRONG - Matcher blocks all paths including _next files
export const config = {
  matcher: ["/:path*"], // This blocks _next/image, _next/static!
};

// CORRECT - Exclude the paths you don't need
export const config = {
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};
```

### Mistake 3: Forgetting Middleware Runs on Edge

```typescript
// WRONG - Using Node.js specific APIs that don't work in Edge
import { readFile } from "fs/promises"; // This won't work!

// CORRECT - Use only Edge-compatible APIs
export function middleware(request: NextRequest) {
  // Use request.cookies, request.headers - these work in Edge
  const token = request.cookies.get("token");
  // No fs module, no child_process, etc.
}
```

## Summary

- Middleware acts as a checkpoint before pages or API routes load
- Use it for authentication checks, redirects, geolocation, and request modifications
- Always include a matcher config to avoid running on unnecessary routes
- Middleware runs on Edge Runtime, so avoid Node.js-specific APIs
- It can read cookies, headers, and the request URL, but cannot read the request body

## Next Steps

- [middleware-ts-setup.md](./middleware-ts-setup.md) - Set up TypeScript for middleware
- [auth-guards.md](../02-middleware-patterns/auth-guards.md) - Learn to protect routes with middleware
