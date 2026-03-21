# A/B Testing with Middleware

## What You'll Learn
- Implementing A/B testing using middleware
- Persisting user bucket assignments with cookies
- Using rewrites to serve different content
- Measuring A/B test results

## Prerequisites
- Understanding of middleware basics
- Knowledge of cookies and headers
- Familiarity with redirects and rewrites

## Concept Explained Simply

A/B testing is like showing two different versions of your store window to different customers to see which one sells more products. You randomly split visitors into groups — some see version A, others see version B — and then you track which version gets better results.

Middleware is perfect for this because it can:
1. Check if the user already has a bucket assignment (so they always see the same version)
2. Randomly assign new visitors to a bucket
3. Use rewrite to show different content without changing the URL

This is much better than client-side A/B testing because there's no "flash" of wrong content — the right version loads from the start.

## Complete Code Example

```typescript
// middleware.ts - Simple A/B testing
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

// Define your test buckets
type TestBucket = "control" | "variant-a" | "variant-b";

interface TestConfig {
  name: string;
  buckets: TestBucket[];
  weight: number[]; // Probability weights
}

const tests: Record<string, TestConfig> = {
  "homepage-hero": {
    name: "Homepage Hero",
    buckets: ["control", "variant-a"],
    weight: [50, 50], // 50% each
  },
  "checkout-button": {
    name: "Checkout Button Color",
    buckets: ["control", "variant-a", "variant-b"],
    weight: [33, 33, 34], // Roughly equal split
  },
};

function getBucket(testName: string, request: NextRequest): TestBucket {
  // Check if user already has a bucket assigned
  const cookieName = `ab-${testName}`;
  const existingBucket = request.cookies.get(cookieName)?.value as TestBucket;
  
  if (existingBucket && tests[testName].buckets.includes(existingBucket)) {
    return existingBucket;
  }
  
  // Assign new bucket based on weights
  const test = tests[testName];
  const random = Math.random() * 100;
  let cumulative = 0;
  
  for (let i = 0; i < test.buckets.length; i++) {
    cumulative += test.weight[i];
    if (random < cumulative) {
      return test.buckets[i];
    }
  }
  
  return test.buckets[0]; // Fallback
}

export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  const response = NextResponse.next();
  
  // Apply homepage hero test
  if (pathname === "/" || pathname === "/home") {
    const bucket = getBucket("homepage-hero", request);
    
    // Set cookie to persist assignment
    response.cookies.set("ab-homepage-hero", bucket, {
      maxAge: 60 * 60 * 24 * 30, // 30 days
      path: "/",
    });
    
    // Rewrite to different page version (URL stays the same!)
    if (bucket === "variant-a") {
      return NextResponse.rewrite(new URL("/home-variant-a", request.url));
    }
  }
  
  // Apply checkout button test
  if (pathname.startsWith("/checkout")) {
    const bucket = getBucket("checkout-button", request);
    
    response.cookies.set("ab-checkout-button", bucket, {
      maxAge: 60 * 60 * 24 * 30,
      path: "/",
    });
    
    // Pass bucket to app via headers
    response.headers.set("x-ab-checkout-button", bucket);
  }
  
  return response;
}

export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico).*)",
  ],
};
```

### Using A/B Test Results in Components

```typescript
// app/page.tsx
import { headers } from "next/headers";

export default function HomePage() {
  const headersList = headers();
  const heroBucket = headersList.get("x-ab-homepage-hero") || "control";
  
  return (
    <main>
      {heroBucket === "variant-a" ? (
        <HeroVariantA />
      ) : (
        <HeroControl />
      )}
    </main>
  );
}

function HeroControl() {
  return (
    <section>
      <h1>Welcome to Our Store</h1>
      <button>Shop Now</button>
    </section>
  );
}

function HeroVariantA() {
  return (
    <section>
      <h1>🎉 Special Offer Just for You!</h1>
      <button>Claim Your Discount</button>
    </section>
  );
}
```

### Tracking Conversions

```typescript
// middleware.ts - Track conversions
export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  
  // Track when someone converts (purchases, signs up, etc.)
  const conversionPages = ["/purchase-success", "/signup-complete"];
  
  if (conversionPages.includes(pathname)) {
    // Get all test buckets
    const heroBucket = request.cookies.get("ab-homepage-hero")?.value;
    const checkoutBucket = request.cookies.get("ab-checkout-button")?.value;
    
    // In real app, send to analytics
    console.log("Conversion!", {
      heroBucket,
      checkoutBucket,
      timestamp: new Date().toISOString(),
    });
  }
  
  return NextResponse.next();
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `type TestBucket` | Define bucket type | TypeScript knows valid values |
| `tests` object | Test configuration | Easy to add/modify tests |
| `cookieName` | Unique cookie per test | Users keep same bucket |
| `Math.random() * 100` | Random 0-100 | Used for weighted assignment |
| `response.cookies.set()` | Persist bucket | User stays in same bucket |
| `NextResponse.rewrite()` | Show different content | URL stays the same |
| `x-ab-*` header | Pass to app | Server Components can read |

## Common Mistakes

### Mistake 1: No Cookie Persistence

```typescript
// WRONG - New bucket every request!
function getBucket(testName: string) {
  return Math.random() < 0.5 ? "control" : "variant-a";
  // User sees different version on every page refresh!
}

// CORRECT - Check for existing assignment
function getBucket(testName: string, request: NextRequest) {
  const existing = request.cookies.get(`ab-${testName}`)?.value;
  if (existing) return existing; // Use existing
  // ... assign new bucket and persist
}
```

### Mistake 2: Rewrite Without Setting Cookie

```typescript
// WRONG - Content changes but no cookie
if (bucket === "variant-a") {
  return NextResponse.rewrite(new URL("/variant-a", request.url));
  // Refresh page = back to random bucket!
}

// CORRECT - Set cookie first
const response = NextResponse.next();
response.cookies.set("ab-test", bucket, { maxAge: 2592000 });
if (bucket === "variant-a") {
  return NextResponse.rewrite(new URL("/variant-a", request.url));
}
return response;
```

### Mistake 3: Not Handling All Test Variations

```typescript
// WRONG - Only handling control
if (bucket === "variant-a") {
  return NextResponse.rewrite(...);
}
// What about variant-b? It gets control!

// CORRECT - Handle all buckets
if (bucket === "variant-a") {
  return NextResponse.rewrite(new URL("/page-a", request.url));
} else if (bucket === "variant-b") {
  return NextResponse.rewrite(new URL("/page-b", request.url));
}
return NextResponse.next(); // control
```

## Summary

- Use cookies to persist bucket assignment so users see consistent version
- Use rewrite instead of redirect to keep URL clean
- Pass bucket via headers to Server Components for conditional rendering
- Track conversions to measure which version performs better
- Set cookie max-age for long-term persistence (30+ days typical)
- Middleware A/B testing has no "flash" of wrong content

## Next Steps

- [streaming-with-suspense.md](../06-rendering/02-suspense/streaming-with-suspense.md) - Combine with Suspense for loading states
- [vercel-speed-insights.md](../21-performance-auditing/02-nextjs-analytics/vercel-speed-insights.md) - Measure real user performance in A/B tests
