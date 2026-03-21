# Request Header Manipulation

## What You'll Learn
- Reading request headers in middleware
- Adding custom headers to requests
- Modifying existing headers
- Using headers for feature flags and analytics

## Prerequisites
- Understanding of HTTP headers basics
- Knowledge of middleware function structure
- Familiarity with NextRequest and NextResponse

## Concept Explained Simply

HTTP headers are like the address written on the outside of an envelope — they contain meta-information about the request or response. Just like you might add a "Return Address" or "Fragile" sticker to an envelope, middleware can add, modify, or read headers on their way through your application.

This is incredibly useful for passing information between different parts of your app. For example, you might want to add a user ID header so your API routes know who's making the request, or add a feature flag header so different parts of your app can enable/disable features.

## Complete Code Example

```typescript
// middleware.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  // 1. READING HEADERS
  // Get existing headers from the request
  const userAgent = request.headers.get("user-agent");
  const acceptLanguage = request.headers.get("accept-language");
  const referer = request.headers.get("referer");
  
  // Log for debugging (in production, use proper logging)
  console.log("User Agent:", userAgent);
  console.log("Language:", acceptLanguage);
  
  // 2. ADDING CUSTOM HEADERS
  // Create a response that continues to the destination
  const response = NextResponse.next();
  
  // Add custom headers to the response
  response.headers.set("X-Custom-Header", "my-value");
  response.headers.set("X-Request-ID", crypto.randomUUID());
  
  // 3. ADDING USER INFORMATION
  // Get user from cookie and add to headers for backend
  const userCookie = request.cookies.get("auth-token");
  if (userCookie) {
    // Decode token (simplified)
    try {
      const payload = JSON.parse(atob(userCookie.value.split(".")[1]));
      response.headers.set("X-User-ID", payload.sub);
      response.headers.set("X-User-Role", payload.role);
    } catch {
      // Invalid token, don't add user headers
    }
  }
  
  // 4. ADDING FEATURE FLAGS
  // Check query params or cookies for feature flags
  const isBeta = request.nextUrl.searchParams.get("beta") === "true" ||
                 request.cookies.get("beta-user")?.value === "true";
  
  if (isBeta) {
    response.headers.set("X-Feature-Beta", "true");
  }
  
  // 5. ANALYTICS HEADERS
  // Add timing information
  response.headers.set("X-Request-Time", new Date().toISOString());
  
  // 6. GEOGRAPHIC INFORMATION
  // Add country code if available (Vercel provides this)
  const country = request.geo?.country || "US";
  response.headers.set("X-User-Country", country);
  
  return response;
}

export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico).*)",
  ],
};
```

### Using Headers in Server Components

```typescript
// app/page.tsx
import { headers } from "next/headers";

export default async function Page() {
  // Read headers added by middleware
  const headersList = headers();
  const userId = headersList.get("x-user-id");
  const country = headersList.get("x-user-country");
  const featureBeta = headersList.get("x-feature-beta");
  
  return (
    <div>
      <h1>Welcome!</h1>
      <p>User ID: {userId || "Guest"}</p>
      <p>Country: {country}</p>
      {featureBeta === "true" && <p>🎉 Beta features enabled!</p>}
    </div>
  );
}
```

### Using Headers in API Routes

```typescript
// app/api/data/route.ts
import { NextRequest } from "next/server";

export async function GET(request: NextRequest) {
  // Access headers passed from middleware
  const userId = request.headers.get("x-user-id");
  const country = request.headers.get("x-user-country");
  
  // Use this information to personalize response
  const data = await fetchUserData(userId, country);
  
  return Response.json(data);
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `request.headers.get("user-agent")` | Read header value | Get client browser/device info |
| `response.headers.set()` | Add/modify header | Pass data to downstream handlers |
| `crypto.randomUUID()` | Generate unique ID | Track individual requests |
| `payload.sub` | Get user ID from JWT | Identify the user |
| `request.geo?.country` | Get geographic info | Available on Vercel deployments |
| `headersList.get()` | Read headers in server | Access middleware-passed data |

## Common Mistakes

### Mistake 1: Trying to Modify Request Headers

```typescript
// WRONG - Request headers can't be modified
export function middleware(request: NextRequest) {
  request.headers.set("x-new-header", "value"); // Won't work!
  return NextResponse.next();
}

// CORRECT - Add headers to the response
export function middleware(request: NextRequest) {
  const response = NextResponse.next();
  response.headers.set("x-new-header", "value"); // This works!
  return response;
}
```

### Mistake 2: Headers Not Available in Server Components

```typescript
// WRONG - Using request object directly
export default async function Page({ request }: { request: NextRequest }) {
  // This doesn't work in Server Components!
  const header = request.headers.get("x-header");
}

// CORRECT - Use headers() from next/headers
import { headers } from "next/headers";

export default async function Page() {
  const headersList = headers();
  const header = headersList.get("x-header");
}
```

### Mistake 3: Not Handling Missing Headers

```typescript
// WRONG - Assume header always exists
const userId = request.headers.get("x-user-id");
console.log(userId.toUpperCase()); // Crashes if null!

// CORRECT - Handle null/undefined
const userId = request.headers.get("x-user-id");
if (userId) {
  console.log(userId.toUpperCase());
}
```

## Summary

- Request headers can be READ but not MODIFIED directly
- Add headers to the response object to pass data forward
- Headers added in middleware are available in Server Components and API routes
- Use headers for user identification, analytics, feature flags, and geo-info
- The `headers()` function in Server Components gives you read access
- Always handle the case where headers might not exist

## Next Steps

- [chaining-middleware.md](../03-middleware-advanced/chaining-middleware.md) - Learn to combine multiple middleware
- [geolocation-based-routing.md](../03-middleware-advanced/geolocation-based-routing.md) - Use geo headers for routing
