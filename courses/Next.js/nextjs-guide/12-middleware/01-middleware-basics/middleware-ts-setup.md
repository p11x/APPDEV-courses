# Middleware TypeScript Setup

## What You'll Learn
- Setting up TypeScript types for middleware
- Understanding NextRequest and NextResponse types
- Typing middleware function parameters correctly

## Prerequisites
- TypeScript basics (types, interfaces)
- Understanding of middleware concept
- A Next.js project with TypeScript configured

## Concept Explained Simply

TypeScript is like having a very detail-oriented coworker who checks your work before you submit it. It catches mistakes before they happen — like trying to use a method that doesn't exist or passing the wrong type of data to a function.

When writing middleware in Next.js, TypeScript helps you understand what properties are available on the request and response objects. Without types, you'd be guessing what methods you can call. With types, your editor can suggest completions and warn you about mistakes.

## Complete Code Example

```typescript
// middleware.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

// Define custom types for your application
interface AuthUser {
  id: string;
  email: string;
  role: "admin" | "user" | "guest";
}

// Typed cookie helper function
function getAuthUser(request: NextRequest): AuthUser | null {
  const token = request.cookies.get("auth-token");
  
  if (!token?.value) {
    return null;
  }
  
  try {
    // In real app, verify JWT token here
    const decoded = JSON.parse(atob(token.value));
    return decoded as AuthUser;
  } catch {
    return null;
  }
}

// Main middleware with proper typing
export function middleware(request: NextRequest): NextResponse {
  // Access typed request properties
  const { pathname, searchParams } = request.nextUrl;
  const user = getAuthUser(request);
  
  // Protected routes that require authentication
  const protectedPaths = ["/dashboard", "/settings", "/admin"];
  const isProtectedPath = protectedPaths.some(path => 
    pathname.startsWith(path)
  );
  
  if (isProtectedPath && !user) {
    const loginUrl = new URL("/login", request.url);
    loginUrl.searchParams.set("redirect", pathname);
    return NextResponse.redirect(loginUrl);
  }
  
  // Admin-only routes
  if (pathname.startsWith("/admin") && user?.role !== "admin") {
    return NextResponse.redirect(new URL("/dashboard", request.url));
  }
  
  // Add user info to headers for downstream use
  const response = NextResponse.next();
  if (user) {
    response.headers.set("x-user-id", user.id);
    response.headers.set("x-user-role", user.role);
  }
  
  return response;
}

// Matcher with type-safe configuration
export const config = {
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};
```

### TypeScript Configuration for Middleware

```typescript
// next-env.d.ts - Types are auto-generated, but you can extend
/// <reference types="next" />
/// <reference types="next/image-types/global" />

// Extend the NextRequest type if needed
declare module "next/server" {
  interface NextRequest {
    // Add custom properties
    customProperty?: string;
  }
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `import type { NextRequest } from "next/server"` | Imports request type | Gives you type safety for the request object |
| `interface AuthUser` | Defines user shape | TypeScript knows what a user object should look like |
| `function getAuthUser(request: NextRequest)` | Typed helper function | Input parameter is typed as NextRequest |
| `request.cookies.get("token")` | Access cookies with types | Returns typed CookieValue |
| `: AuthUser \| null` | Return type annotation | Function always returns correct type |
| `response.headers.set()` | Set custom headers | Headers are also properly typed |
| `export const config` | Matcher configuration | Controls which routes trigger middleware |

## Common Mistakes

### Mistake 1: Not Importing Types

```typescript
// WRONG - Missing type imports
import { NextResponse } from "next/server";
// No type for request parameter!

export function middleware(request) { // any type
  const url = request.nextUrl; // Might not exist
}
```

```typescript
// CORRECT - Import and use types
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  const url = request.nextUrl; // Fully typed
}
```

### Mistake 2: Wrong Return Type

```typescript
// WRONG - Forgetting to return NextResponse
export function middleware(request: NextRequest) {
  if (!isLoggedIn) {
    return NextResponse.redirect("/login");
    // Missing return for else case!
  }
  // What happens here? Returns undefined!
}
```

```typescript
// CORRECT - Always return a response
export function middleware(request: NextRequest) {
  if (!isLoggedIn) {
    return NextResponse.redirect("/login");
  }
  return NextResponse.next(); // Explicitly continue
}
```

### Mistake 3: Mutating Request Objects

```typescript
// WRONG - Can't really "mutate" request in meaningful ways
export function middleware(request: NextRequest) {
  request.url = "http://something-else.com"; // Doesn't work
  return NextResponse.next();
}
```

```typescript
// CORRECT - Create new response or redirect
export function middleware(request: NextRequest) {
  const response = NextResponse.next();
  // Add headers to the response
  response.headers.set("x-custom-header", "value");
  return response;
}
```

## Summary

- Always import and use `NextRequest` and `NextResponse` types
- Create helper functions with proper type annotations for cleaner code
- The middleware function must return a `NextResponse` in all code paths
- You can extend TypeScript types with declaration merging if needed
- TypeScript helps catch errors before runtime, especially with route conditions

## Next Steps

- [auth-guards.md](../02-middleware-patterns/auth-guards.md) - Implement authentication guards
- [request-header-manipulation.md](../02-middleware-patterns/request-header-manipulation.md) - Modify headers in middleware
