# Redirects and Rewrites in Middleware

## What You'll Learn
- Using NextResponse.redirect() for URL redirects
- Using NextResponse.rewrite() for URL masking
- Conditional redirects based on user context
- Implementing trailing slash normalization

## Prerequisites
- Understanding of middleware basics
- Knowledge of URL structure (pathname, search params)
- Familiarity with HTTP redirect concepts

## Concept Explained Simply

Redirects and rewrites are like giving directions to someone visiting your website:

- **Redirect** is like saying "Oh, you wanted Pizza Palace? They moved to a new location — here's their new address, go there instead." The visitor's URL changes to the new location.

- **Rewrite** is like a secret passage. Someone asks for "Room 101" but behind the scenes you take them to "Room 201" without them knowing. The URL stays the same but different content loads.

Both are useful: redirects when content has moved permanently, rewrites when you want to mask the actual URL structure from users.

## Complete Code Example

```typescript
// middleware.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  const { pathname, searchParams } = request.nextUrl;
  
  // 1. Redirect old URLs to new locations (permanent redirect)
  if (pathname === "/about-us") {
    return NextResponse.redirect(new URL("/about", request.url));
  }
  
  // 2. Redirect with query parameter preservation
  if (pathname === "/legacy-product") {
    const newUrl = new URL("/products/new-product", request.url);
    // Keep existing query params
    searchParams.forEach((value, key) => {
      newUrl.searchParams.set(key, value);
    });
    return NextResponse.redirect(newUrl);
  }
  
  // 3. Rewrite - mask the actual URL (user sees original URL)
  if (pathname.startsWith("/secret-admin-panel")) {
    return NextResponse.rewrite(new URL("/admin/dashboard", request.url));
  }
  
  // 4. Conditional redirect based on user
  const user = request.cookies.get("user");
  if (pathname === "/pricing" && user) {
    // Logged-in users see different pricing
    return NextResponse.redirect(new URL("/pricing/pro", request.url));
  }
  
  // 5. Add trailing slash (optional)
  if (!pathname.endsWith("/") && !pathname.includes(".")) {
    const newUrl = new URL(`${pathname}/`, request.url);
    return NextResponse.redirect(newUrl);
  }
  
  // 6. Language-based redirect
  const acceptLanguage = request.headers.get("accept-language");
  if (pathname === "/" && acceptLanguage?.includes("es")) {
    return NextResponse.redirect(new URL("/es", request.url));
  }
  
  return NextResponse.next();
}

export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico).*)",
  ],
};
```

### Rewrite for A/B Testing

```typescript
// middleware.ts - A/B testing with rewrite
export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  
  // Only apply to the landing page
  if (pathname !== "/") {
    return NextResponse.next();
  }
  
  // Check if user already has a bucket assigned
  let bucket = request.cookies.get("ab-bucket")?.value;
  
  // Assign bucket if not exists (50/50 split)
  if (!bucket) {
    bucket = Math.random() < 0.5 ? "a" : "b";
  }
  
  const response = NextResponse.next();
  response.cookies.set("ab-bucket", bucket);
  
  // Rewrite to different version without changing URL
  if (bucket === "b") {
    return NextResponse.rewrite(new URL("/landing-b", request.url));
  }
  
  return response;
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `NextResponse.redirect(url)` | Tells browser to go to new URL | Browser changes address bar |
| `NextResponse.rewrite(url)` | Loads different URL silently | User sees original URL |
| `new URL(path, request.url)` | Creates URL relative to request | Preserves protocol and domain |
| `searchParams.forEach()` | Copies query parameters | Keeps URL params after redirect |
| `pathname.endsWith("/")` | Checks for trailing slash | Used for normalization |
| `accept-language` header | Gets browser language preference | Enables i18n routing |
| `Math.random()` | Random number for A/B | Assigns users to test groups |

## Common Mistakes

### Mistake 1: Infinite Redirect Loop

```typescript
// WRONG - Redirects to itself!
if (pathname === "/") {
  return NextResponse.redirect(new URL("/", request.url)); // Infinite loop!
}

// CORRECT - Check conditions properly
if (pathname === "/old-page") {
  return NextResponse.redirect(new URL("/new-page", request.url));
}
```

### Mistake 2: Rewrite Instead of Redirect (when SEO matters)

```typescript
// WRONG - Using rewrite when you should redirect for SEO
if (pathname === "/products/old-name") {
  return NextResponse.rewrite(new URL("/products/new-name", request.url));
  // SEO: Search engines see both URLs as different pages!
}

// CORRECT - Redirect tells search engines page moved
if (pathname === "/products/old-name") {
  return NextResponse.redirect(new URL("/products/new-name", request.url));
  // SEO: 301 redirect passes ranking to new page
}
```

### Mistake 3: Not Handling Query Parameters

```typescript
// WRONG - Losing query parameters on redirect
if (pathname === "/search") {
  return NextResponse.redirect(new URL("/find", request.url));
  // ?query=hello is lost!
}

// CORRECT - Preserve query params
if (pathname === "/search") {
  const newUrl = new URL("/find", request.url);
  request.nextUrl.searchParams.forEach((value, key) => {
    newUrl.searchParams.set(key, value);
  });
  return NextResponse.redirect(newUrl);
}
```

## Summary

- Use `redirect()` when content has moved permanently (good for SEO)
- Use `rewrite()` when you want to mask the actual URL from users
- Always preserve query parameters when redirecting to avoid breaking functionality
- Rewrite is perfect for A/B testing and feature flags
- Language detection can be done via Accept-Language header
- Be careful of infinite loops — always have an exit condition

## Next Steps

- [a-b-testing-with-middleware.md](../03-middleware-advanced/a-b-testing-with-middleware.md) - Advanced A/B testing patterns
- [chaining-middleware.md](../03-middleware-advanced/chaining-middleware.md) - Multiple middleware functions working together
