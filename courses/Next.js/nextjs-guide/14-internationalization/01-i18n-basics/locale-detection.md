# Locale Detection

## What You'll Learn
- Different methods to detect user locale
- Priority order for locale detection
- Implementing locale detection in middleware
- Handling edge cases

## Prerequisites
- Understanding of i18n basics
- Knowledge of middleware
- Familiarity with HTTP headers

## Concept Explained Simply

Locale detection is like a detective figuring out who your visitor is and where they're from. You use multiple clues to make your best guess:

1. **URL path** — Did they visit `/fr/products`? They probably want French.
2. **Browser settings** — What languages does their browser say they prefer?
3. **Cookie** — Did they previously choose a language? Remember that!
4. **Geographic location** — They're visiting from France? Show French!

These are checked in order of priority, with URL path being the strongest signal (intentional) and geography being the weakest (just an educated guess).

## Complete Code Example

### Middleware with Priority-Based Detection

```typescript
// middleware.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

const locales = ["en", "es", "fr", "de", "ja", "zh"];
const defaultLocale = "en";

export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  
  // Skip API and static files
  if (
    pathname.startsWith("/api") ||
    pathname.startsWith("/_next") ||
    pathname.includes(".")
  ) {
    return NextResponse.next();
  }
  
  // Check if pathname already has locale
  const pathnameHasLocale = locales.some(
    (locale) => pathname.startsWith(`/${locale}/`) || pathname === `/${locale}`
  );
  
  if (pathnameHasLocale) return NextResponse.next();
  
  // Detect locale (priority order)
  const locale = detectLocale(request);
  
  // Redirect to localized URL
  const newUrl = new URL(`/${locale}${pathname}`, request.url);
  
  return NextResponse.redirect(newUrl);
}

function detectLocale(request: NextRequest): string {
  // 1. Check cookie first (user explicitly chose language)
  const cookieLocale = request.cookies.get("user-locale")?.value;
  if (cookieLocale && locales.includes(cookieLocale)) {
    return cookieLocale;
  }
  
  // 2. Check URL query parameter (?locale=fr)
  const queryLocale = request.nextUrl.searchParams.get("locale");
  if (queryLocale && locales.includes(queryLocale)) {
    return queryLocale;
  }
  
  // 3. Check Accept-Language header (browser preference)
  const acceptLanguage = request.headers.get("accept-language");
  if (acceptLanguage) {
    const preferredLocale = getPreferredLocale(acceptLanguage);
    if (preferredLocale) {
      return preferredLocale;
    }
  }
  
  // 4. Check geography (Vercel provides this)
  const geoLocale = request.geo?.country;
  if (geoLocale) {
    const mapped = mapCountryToLocale(geoLocale);
    if (mapped) return mapped;
  }
  
  // Default to app default
  return defaultLocale;
}

// Parse Accept-Language header
function getPreferredLocale(header: string): string | null {
  // Format: "en-US,en;q=0.9,es;q=0.8"
  const languages = header
    .split(",")
    .map((lang) => {
      const [locale, quality] = lang.trim().split(";");
      return {
        locale: locale.split("-")[0], // "en-US" -> "en"
        quality: quality ? parseFloat(quality.split("=")[1]) : 1,
      };
    })
    .sort((a, b) => b.quality - a.quality)
    .map((l) => l.locale);
  
  // Find first supported locale
  return languages.find((locale) => locales.includes(locale)) || null;
}

// Map country codes to locales
function mapCountryToLocale(country: string): string | null {
  const countryToLocale: Record<string, string> = {
    US: "en",
    GB: "en",
    ES: "es",
    MX: "es",
    AR: "es",
    CO: "es",
    FR: "fr",
    DE: "de",
    AT: "de",
    CH: "de",
    JP: "ja",
    CN: "zh",
    TW: "zh",
  };
  
  return countryToLocale[country] || null;
}

export const config = {
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};
```

### Setting User Preference

```typescript
// app/api/set-locale/route.ts
import { NextResponse } from "next/server";

export async function POST(request: Request) {
  const { locale } = await request.json();
  
  const supportedLocales = ["en", "es", "fr", "de", "ja", "zh"];
  
  if (!supportedLocales.includes(locale)) {
    return NextResponse.json(
      { error: "Unsupported locale" },
      { status: 400 }
    );
  }
  
  const response = NextResponse.json({ success: true });
  
  // Set cookie for 1 year
  response.cookies.set("user-locale", locale, {
    maxAge: 60 * 60 * 24 * 365,
    path: "/",
  });
  
  return response;
}
```

### Client-Side Locale Switcher

```typescript
// components/LocaleSwitcher.tsx
"use client";

import { useRouter, usePathname } from "next/navigation";

const locales = [
  { code: "en", name: "English" },
  { code: "es", name: "Español" },
  { code: "fr", name: "Français" },
  { code: "de", name: "Deutsch" },
];

export default function LocaleSwitcher() {
  const router = useRouter();
  const pathname = usePathname();
  
  const currentLocale = pathname.split("/")[1];
  
  const switchLocale = async (newLocale: string) => {
    // Save preference to cookie
    await fetch("/api/set-locale", {
      method: "POST",
      body: JSON.stringify({ locale: newLocale }),
    });
    
    // Replace locale in URL
    const newPath = pathname.replace(`/${currentLocale}`, `/${newLocale}`);
    router.push(newPath);
    router.refresh();
  };
  
  return (
    <select
      value={currentLocale}
      onChange={(e) => switchLocale(e.target.value)}
      className="locale-select"
    >
      {locales.map((locale) => (
        <option key={locale.code} value={locale.code}>
          {locale.name}
        </option>
      ))}
    </select>
  );
}
```

## Detection Priority

| Priority | Source | Example | Notes |
|----------|--------|---------|-------|
| 1 (Highest) | URL path | `/fr/products` | Explicit locale in URL |
| 2 | Cookie | `user-locale=fr` | User previously selected |
| 3 | Query param | `?locale=fr` | Explicit override |
| 4 | Accept-Language | `fr-FR;q=0.9` | Browser settings |
| 5 (Lowest) | Geography | Request from France | Best guess |
| Default | — | — | App's default locale |

## Common Mistakes

### Mistake 1: Not Considering Sub-Locales

```typescript
// WRONG - Only checking exact match
const browserLocale = acceptLanguage.split(",")[0]; // "en-US"
if (locales.includes(browserLocale)) return browserLocale;
// "en-US" doesn't match "en"!

// CORRECT - Extract base locale
const baseLocale = browserLocale.split("-")[0]; // "en"
if (locales.includes(baseLocale)) return baseLocale;
```

### Mistake 2: Infinite Redirect Loops

```typescript
// WRONG - Redirect loop possible
if (!pathname.startsWith(`/${locale}`)) {
  return NextResponse.redirect(new URL(`/${locale}${pathname}`, request.url));
}
// If locale detection keeps changing, infinite loop!

// CORRECT - Check pathname already has locale
const pathnameHasLocale = locales.some(
  (l) => pathname.startsWith(`/${l}/`) || pathname === `/${l}`
);
if (pathnameHasLocale) return NextResponse.next();
// Only redirect if no locale in URL
```

### Mistake 3: Ignoring Unsupported Locales

```typescript
// WRONG - Using locale without checking support
const locale = acceptLanguage.split("-")[0];
// User might request "kh" (Cambodian) but app only supports "en", "es"!

// CORRECT - Always check against supported locales
if (!locales.includes(detectedLocale)) {
  return defaultLocale;
}
```

## Summary

- Use priority order: URL > Cookie > Query > Accept-Language > Geo
- Always check detected locale against supported locales
- Remember user's choice in a cookie for future visits
- Handle sub-locales (en-US → en)
- Prevent infinite redirect loops by checking existing locale
- Allow users to override detection with query parameters

## Next Steps

- [routing-with-locales.md](./routing-with-locales.md) - Implementing localized routing
- [installing-next-intl.md](../02-next-intl-setup/installing-next-intl.md) - Setting up next-intl library
