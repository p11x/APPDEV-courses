# What Is Internationalization (i18n)?

## What You'll Learn
- Understanding internationalization and localization
- Why i18n matters for web applications
- Key terminology in i18n
- How Next.js handles i18n

## Prerequisites
- Basic understanding of web development
- Familiarity with JavaScript/TypeScript
- No prior i18n experience needed

## Concept Explained Simply

Internationalization (often shortened to i18n because there are 18 letters between "i" and "n") is the process of making your website work in multiple languages and regions. It's like building a restaurant that can serve customers from anywhere in the world — you need menus in different languages, appropriate food for different cultures, and prices in different currencies.

Localization (l10n) is the actual process of adapting your app for a specific locale — translating the menu to French, for example. Internationalization is the foundation that makes localization possible.

Think of i18n as the infrastructure: it gives you tools to store translations, switch languages, and format dates/numbers correctly. Without it, you'd have to manually create separate versions of every page for every language.

## Why i18n Matters

### Without i18n
- You would need separate pages for each language
- Updating text means updating every version
- Dates, numbers, and currencies would be wrong for different regions
- Adding a new language means copying and pasting entire page sets

### With i18n
- One page structure, multiple languages
- Translations in separate files — easy to update
- Automatic formatting for each locale
- Adding a language = adding a translation file

## Complete Code Example

### Setting Up i18n Structure

```
your-app/
├── messages/
│   ├── en.json       # English translations
│   ├── es.json       # Spanish translations
│   ├── fr.json       # French translations
│   └── de.json       # German translations
├── app/
│   ├── [locale]/     # Locale as dynamic segment
│   │   ├── page.tsx
│   │   └── layout.tsx
│   └── page.tsx      # Redirects to default locale
├── middleware.ts     # Detects user locale
└── i18n.ts          # i18n configuration
```

### Translation Files

```json
// messages/en.json
{
  "common": {
    "welcome": "Welcome",
    "login": "Log in",
    "logout": "Log out"
  },
  "home": {
    "title": "Welcome to Our App",
    "description": "The best app for your needs"
  },
  "products": {
    "title": "Our Products",
    "price": "Price: ${price}"
  }
}
```

```json
// messages/es.json
{
  "common": {
    "welcome": "Bienvenido",
    "login": "Iniciar sesión",
    "logout": "Cerrar sesión"
  },
  "home": {
    "title": "Bienvenido a Nuestra App",
    "description": "La mejor aplicación para tus necesidades"
  },
  "products": {
    "title": "Nuestros Productos",
    "price": "Precio: ${price}"
  }
}
```

### Middleware for Locale Detection

```typescript
// middleware.ts
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

const locales = ["en", "es", "fr", "de"];
const defaultLocale = "en";

export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  
  // Skip API routes and static files
  if (
    pathname.startsWith("/api") ||
    pathname.startsWith("/_next") ||
    pathname.includes(".")
  ) {
    return NextResponse.next();
  }
  
  // Check if pathname already has a locale
  const pathnameHasLocale = locales.some(
    (locale) => pathname.startsWith(`/${locale}/`) || pathname === `/${locale}`
  );
  
  if (pathnameHasLocale) return NextResponse.next();
  
  // Get locale from headers or default
  const locale = getLocale(request);
  
  // Redirect to localized URL
  const newUrl = new URL(`/${locale}${pathname}`, request.url);
  
  return NextResponse.redirect(newUrl);
}

function getLocale(request: NextRequest): string {
  // Check header for locale preference
  const acceptLanguage = request.headers.get("accept-language");
  
  if (acceptLanguage) {
    // Extract preferred locale (simplified)
    const preferred = acceptLanguage.split(",")[0].split("-")[0];
    if (locales.includes(preferred)) {
      return preferred;
    }
  }
  
  return defaultLocale;
}

export const config = {
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};
```

### Using Translations in Components

```typescript
// app/[locale]/page.tsx
import { getTranslations } from "next-intl/server";

export default async function HomePage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  const t = await getTranslations({ locale, namespace: "home" });
  
  return (
    <main>
      <h1>{t("title")}</h1>
      <p>{t("description")}</p>
    </main>
  );
}
```

## Key Terminology

| Term | Definition |
|------|------------|
| **i18n** | Short for "internationalization" — the process of designing software to support multiple languages |
| **l10n** | Short for "localization" — adapting i18n software for a specific region |
| **Locale** | A combination of language and region (e.g., en-US, fr-FR) |
| **Translation key** | A string identifier for translatable text (e.g., "home.title") |
| **Namespace** | A grouping of translations (e.g., "home", "common", "products") |
| **Pluralization** | Different forms for singular/plural (e.g., "1 item" vs "2 items") |

## Common Mistakes

### Mistake 1: Hardcoding Text

```typescript
// WRONG - Hardcoded English everywhere
export default function Page() {
  return <h1>Welcome to my app</h1>;
}

// CORRECT - Use translation keys
export default async function Page() {
  const t = await getTranslations("home");
  return <h1>{t("title")}</h1>;
}
```

### Mistake 2: Not Handling Missing Translations

```typescript
// WRONG - No fallback if translation missing
const title = t("nonexistent.key"); // Returns key if not found, could break

// CORRECT - Provide fallback
const title = t("nonexistent.key", { fallback: "Default Title" });
```

### Mistake 3: Ignoring Regional Differences

```typescript
// WRONG - Treating all English as the same
// en-GB uses "colour", en-US uses "color"
// en-GB uses "dd/mm/yyyy", en-US uses "mm/dd/yyyy"

// CORRECT - Use full locale (en-GB, en-US)
const locale = "en-GB"; // Not just "en"
```

## Summary

- i18n makes your app accessible to users worldwide
- Key concepts: locales, translation keys, namespaces
- Use middleware to detect and redirect to user's preferred locale
- Keep translations in separate JSON files
- Next.js supports i18n natively through the App Router
- Always use translation keys instead of hardcoded text

## Next Steps

- [locale-detection.md](./locale-detection.md) - Different methods to detect user locale
- [routing-with-locales.md](./routing-with-locales.md) - Setting up localized routing
