# Geolocation-Based Routing

## What You'll Learn
- Using Vercel's geolocation data in middleware
- Redirecting users to regional versions of your site
- Customizing content based on user location
- Setting up country-specific redirects

## Prerequisites
- Understanding of middleware basics
- Knowledge of HTTP redirects
- Familiarity with Vercel deployment (provides geo data)

## Concept Explained Simply

Imagine you have a chain of stores in different countries. When someone walks in, you want to greet them in their language and show them products available in their country. Geolocation-based routing does exactly this for websites — it detects where your user is located and shows them the most relevant version of your site.

Vercel automatically provides geographic information (country, city, region) for requests deployed on their platform. Middleware can read this data and redirect users or customize content accordingly.

## Complete Code Example

```typescript
// middleware.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  
  // Skip for API routes, static files, and logged-in users
  if (
    pathname.startsWith("/api") ||
    pathname.startsWith("/_next") ||
    pathname.includes(".")
  ) {
    return NextResponse.next();
  }
  
  // Get geolocation data (provided by Vercel)
  const country = request.geo?.country || "US";
  const city = request.geo?.city || "Unknown";
  const region = request.geo?.region || "Unknown";
  
  console.log(`Request from ${city}, ${region}, ${country}`);
  
  // 1. Redirect root to localized version
  if (pathname === "/") {
    // Check if user already has a locale preference
    const hasLocaleCookie = request.cookies.has("preferred-locale");
    
    if (!hasLocaleCookie) {
      // Redirect based on country
      const localeRedirects: Record<string, string> = {
        DE: "/de",
        FR: "/fr",
        ES: "/es",
        JP: "/ja",
        CN: "/zh",
        BR: "/pt",
        US: "/en",
        GB: "/en",
      };
      
      const redirectPath = localeRedirects[country] || "/en";
      
      // Don't redirect if already on correct locale
      if (pathname !== redirectPath) {
        const response = NextResponse.redirect(
          new URL(redirectPath, request.url)
        );
        // Set cookie so we remember their choice
        response.cookies.set("preferred-locale", redirectPath);
        return response;
      }
    }
  }
  
  // 2. Show country-specific pricing
  const response = NextResponse.next();
  
  // Pass geo info to the app via headers
  response.headers.set("x-user-country", country);
  response.headers.set("x-user-city", city);
  response.headers.set("x-user-region", region);
  
  // Set currency based on country
  const currencyMap: Record<string, string> = {
    US: "USD",
    GB: "GBP",
    EU: "EUR",
    JP: "JPY",
  };
  
  const currency = currencyMap[country] || "USD";
  response.headers.set("x-currency", currency);
  
  return response;
}

export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico).*)",
  ],
};
```

### Country-Specific Content in Server Components

```typescript
// app/products/page.tsx
import { headers } from "next/headers";

export default async function ProductsPage() {
  const headersList = headers();
  const country = headersList.get("x-user-country") || "US";
  const currency = headersList.get("x-currency") || "USD";
  
  // Fetch products with regional pricing
  const products = await fetchProducts(country);
  
  const formatPrice = (price: number) => {
    return new Intl.NumberFormat(country, {
      style: "currency",
      currency: currency,
    }).format(price);
  };
  
  return (
    <div>
      <h1>Products</h1>
      <p>Showing prices in {currency}</p>
      <ul>
        {products.map((product) => (
          <li key={product.id}>
            {product.name} - {formatPrice(product.price)}
          </li>
        ))}
      </ul>
    </div>
  );
}
```

### EU Cookie Compliance

```typescript
// middleware.ts - EU cookie notice redirect
export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  const country = request.geo?.country || "US";
  
  // EU countries that need cookie consent
  const euCountries = ["DE", "FR", "ES", "IT", "NL", "BE", "AT", "PT"];
  
  // Check if user needs to see cookie consent
  if (!pathname.startsWith("/cookie") && !pathname.startsWith("/api")) {
    const hasConsent = request.cookies.has("cookie-consent");
    const isEU = euCountries.includes(country);
    
    if (isEU && !hasConsent) {
      // Redirect to cookie consent page for EU users
      const response = NextResponse.redirect(
        new URL("/cookie-consent", request.url)
      );
      // But let them skip if they came from a different country
      return response;
    }
  }
  
  return NextResponse.next();
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `request.geo?.country` | Get country code | Provided by Vercel, e.g., "US", "DE" |
| `request.geo?.city` | Get city name | e.g., "San Francisco" |
| `localeRedirects` | Country to locale mapping | Maps countries to site versions |
| `response.cookies.set()` | Set cookie to remember choice | Prevents redirect loops |
| `x-user-country` header | Pass geo to app | Available in Server Components |
| `euCountries` | Array of EU country codes | For GDPR compliance |

## Common Mistakes

### Mistake 1: Assuming Geo Data Always Exists

```typescript
// WRONG - Geo might be undefined on local/dev
const country = request.geo.country; // Crashes if undefined!

// CORRECT - Use optional chaining
const country = request.geo?.country || "US"; // Default fallback
```

### Mistake 2: Redirect Loops

```typescript
// WRONG - Redirects infinitely if no locale cookie
if (!request.cookies.has("locale")) {
  return NextResponse.redirect(new URL("/en", request.url));
  // Every request redirects!
}

// CORRECT - Add a way to accept the redirect
if (!request.cookies.has("locale") && !pathname.startsWith("/en")) {
  const response = NextResponse.redirect(new URL("/en", request.url));
  response.cookies.set("locale", "en");
  return response;
}
```

### Mistake 3: Not Considering CDN/Proxy

```typescript
// WRONG - Geo might be from CDN edge location, not user
// If your app is behind Cloudflare, request.geo might show CDN location!

// CORRECT - Use True-Client-IP header if available
const trueClientIP = request.headers.get("True-Client-IP");
const country = trueClientIP 
  ? lookupCountryFromIP(trueClientIP)  // Use a geo-IP service
  : request.geo?.country || "US";
```

## Summary

- Vercel provides `request.geo` with country, city, and region
- Always provide fallback values since geo isn't always available
- Use cookies to remember user preferences and prevent redirect loops
- Pass geo info via headers to Server Components for dynamic content
- Useful for locale redirects, regional pricing, and compliance
- Not available in self-hosted deployments without additional setup

## Next Steps

- [routing-with-locales.md](../14-internationalization/01-i18n-basics/routing-with-locales.md) - Setting up internationalization routes
- [edge-middleware.md](../18-edge-runtime/02-edge-functions/edge-middleware.md) - Running geo routing on Edge for speed
