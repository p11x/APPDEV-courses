# Authentication Guards with Middleware

## What You'll Learn
- Protecting routes using middleware
- Checking authentication status before page loads
- Redirecting unauthenticated users gracefully
- Role-based access control in middleware

## Prerequisites
- Understanding of middleware basics
- Knowledge of cookies and sessions
- Familiarity with Next.js authentication patterns

## Concept Explained Simply

An authentication guard is like a club bouncer who checks your ID before letting you in. In web applications, middleware acts as that bouncer — it checks if a user is logged in before letting them access protected areas like `/dashboard`, `/settings`, or `/admin`.

The cool thing about doing this in middleware is that the page never even loads for unauthorized users. They get redirected BEFORE any sensitive code runs. This is more secure than loading the page and then hiding content with JavaScript, because a clever user could still access that content.

## Complete Code Example

```typescript
// middleware.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

// Types for your application
interface JWTPayload {
  sub: string;
  email: string;
  role: "admin" | "user" | "guest";
  exp: number;
}

// Decode and verify JWT token (simplified)
function decodeToken(token: string): JWTPayload | null {
  try {
    const parts = token.split(".");
    if (parts.length !== 3) return null;
    
    const payload = JSON.parse(atob(parts[1])) as JWTPayload;
    
    // Check expiration
    if (payload.exp * 1000 < Date.now()) {
      return null;
    }
    
    return payload;
  } catch {
    return null;
  }
}

// Get user from request cookies
function getUser(request: NextRequest): JWTPayload | null {
  const token = request.cookies.get("auth-token")?.value;
  
  if (!token) {
    return null;
  }
  
  return decodeToken(token);
}

// Route protection rules
const protectedRoutes = ["/dashboard", "/profile", "/settings"];
const adminRoutes = ["/admin", "/admin/:path*"];

export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  const user = getUser(request);
  
  // Check if accessing protected route without auth
  const isProtectedRoute = protectedRoutes.some(route => 
    pathname === route || pathname.startsWith(`${route}/`)
  );
  
  if (isProtectedRoute && !user) {
    const loginUrl = new URL("/login", request.url);
    loginUrl.searchParams.set("redirect", pathname);
    return NextResponse.redirect(loginUrl);
  }
  
  // Check if accessing admin route without admin role
  const isAdminRoute = adminRoutes.some(route => {
    if (route.includes(":path*")) {
      return pathname.startsWith(route.replace("/:path*", ""));
    }
    return pathname === route;
  });
  
  if (isAdminRoute && user?.role !== "admin") {
    // Redirect to dashboard if not admin
    return NextResponse.redirect(new URL("/dashboard", request.url));
  }
  
  // Redirect logged-in users away from auth pages
  if ((pathname === "/login" || pathname === "/signup") && user) {
    return NextResponse.redirect(new URL("/dashboard", request.url));
  }
  
  return NextResponse.next();
}

export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico).*)",
  ],
};
```

### Using with Server Components

```typescript
// app/dashboard/page.tsx
import { cookies } from "next/headers";
import { redirect } from "next/navigation";

export default async function DashboardPage() {
  // Double-check auth in server component (defense in depth)
  const cookieStore = cookies();
  const token = cookieStore.get("auth-token");
  
  if (!token) {
    redirect("/login");
  }
  
  // User is authenticated - fetch their data
  return (
    <div>
      <h1>Dashboard</h1>
      <p>Welcome back!</p>
    </div>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `interface JWTPayload` | Defines token data shape | TypeScript knows what's in the token |
| `decodeToken()` | Parses and validates JWT | Extracts user info from signed token |
| `getUser()` | Extracts user from cookies | Centralized user retrieval |
| `protectedRoutes` | Array of protected paths | Easy to add/remove protected routes |
| `isProtectedRoute` | Checks current path | Determines if route needs protection |
| `user?.role !== "admin"` | Role check | Implements role-based access control |
| `loginUrl.searchParams.set()` | Passes redirect URL | User redirected back after login |
| `redirect()` | Server-side redirect | Used in Server Components as backup |

## Common Mistakes

### Mistake 1: Only Checking Middleware

```typescript
// WRONG - Only protecting in middleware, client could bypass
// If middleware fails for some reason, route is exposed!

// CORRECT - Always add server component check too
export default async function ProtectedPage() {
  const session = await getSession();
  if (!session) redirect("/login");
  // Now truly protected
}
```

### Mistake 2: Not Handling Token Expiration

```typescript
// WRONG - Token could be expired but still "valid"
function getUser(request: NextRequest) {
  const token = request.cookies.get("token")?.value;
  return token ? decodeJWT(token) : null; // Could return expired token!
}

// CORRECT - Check expiration
function decodeToken(token: string): JWTPayload | null {
  const payload = decodeJWT(token);
  if (payload.exp * 1000 < Date.now()) {
    return null; // Token expired
  }
  return payload;
}
```

### Mistake 3: Forgetting Static File Routes in Matcher

```typescript
// WRONG - Middleware runs on every request
export const config = {
  matcher: ["/:path*"], // Too broad!
};

// CORRECT - Exclude static files
export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico).*)",
  ],
};
```

## Summary

- Middleware provides the first line of defense for protected routes
- Always redirect unauthenticated users before the page loads
- Include role-based checks for admin-only areas
- Pass the original URL so users get redirected back after login
- Use server component checks as a second layer of protection
- Handle token expiration properly to avoid security holes

## Next Steps

- [protecting-routes.md](../../10-authentication/01-auth-concepts/protecting-routes.md) - Learn about route protection in auth context
- [middleware-protection.md](../../10-authentication/01-auth-concepts/middleware-protection.md) - Deep dive into auth middleware patterns
