# Installing next-intl

## What You'll Learn
- Installing next-intl package
- Setting up the configuration
- Creating the i18n.ts file
- Wrapping your app with providers

## Prerequisites
- A Next.js 14/15 project with App Router
- Basic understanding of TypeScript
- Familiarity with project structure

## Concept Explained Simply

next-intl is like a translation library that works perfectly with Next.js. It handles all the tricky parts of internationalization: loading the right translations, formatting dates and numbers correctly, and making translations easy to use in your components.

Think of it as hiring a professional translator who not only translates your menus but also teaches your staff how to use them properly. The library does the heavy lifting so you can focus on building your app.

## Complete Code Example

### Installation

```bash
# Install next-intl
npm install next-intl

# Or with yarn
yarn add next-intl

# Or with pnpm
pnpm add next-intl
```

### Project Setup

```
your-app/
├── messages/                  # Translation files
│   ├── en.json
│   ├── es.json
│   └── fr.json
├── lib/
│   └── i18n.ts               # i18n configuration
├── app/
│   └── [locale]/
│       ├── layout.tsx        # Root layout
│       └── page.tsx
├── middleware.ts             # Locale detection
└── next.config.mjs
```

### Creating i18n Configuration

```typescript
// lib/i18n.ts
import { notFound } from "next/navigation";

// List of supported locales
export const locales = ["en", "es", "fr", "de", "ja"];

// Default locale when no locale is detected
export const defaultLocale = "en";

// This is the function that provides translations to Server Components
import { getRequestConfig } from "next-intl/server";

export default getRequestConfig(async ({ locale }) => {
  // Validate that the incoming `locale` parameter is valid
  if (!locales.includes(locale as any)) notFound();
 
  return {
    // Import translations
    messages: (await import(`../messages/${locale}.json`)).default
  };
});
```

### Setting Up Middleware

```typescript
// middleware.ts
import createMiddleware from "next-intl/middleware";

export default createMiddleware({
  // A list of all locales that are supported
  locales: ["en", "es", "fr", "de", "ja"],
  
  // Used when no locale matches
  defaultLocale: "en",
  
  // Don't prefix the default locale
  localePrefix: "always"
});

export const config = {
  // Skip all paths that should not be internationalized
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"]
};
```

### Configuring next.config

```javascript
// next.config.mjs
import createNextIntlPlugin from "next-intl/plugin";

const withNextIntl = createNextIntlPlugin("./lib/i18n.ts");

/** @type {import('next').NextConfig} */
const nextConfig = {
  // Your existing config
};

export default withNextIntl(nextConfig);
```

### Wrapping the Root Layout

```typescript
// app/[locale]/layout.tsx
import { NextIntlClientProvider } from "next-intl";
import { getMessages } from "next-intl/server";
import { notFound } from "next/navigation";

const locales = ["en", "es", "fr", "de", "ja"];

export default async function LocaleLayout({
  children,
  params,
}: {
  children: React.ReactNode;
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  
  // Validate locale
  if (!locales.includes(locale)) {
    notFound();
  }
  
  // Get messages for this locale
  const messages = await getMessages();
  
  return (
    <html lang={locale}>
      <body>
        <NextIntlClientProvider messages={messages}>
          {children}
        </NextIntlClientProvider>
      </body>
    </html>
  );
}

// Generate static params for build
export async function generateStaticParams() {
  return locales.map((locale) => ({ locale }));
}
```

### Using in Client Components

```typescript
// components/ClientComponent.tsx
"use client";

import { useTranslations } from "next-intl";

export default function ClientComponent() {
  const t = useTranslations("home");
  
  return (
    <div>
      <h1>{t("title")}</h1>
      <p>{t("description")}</p>
    </div>
  );
}
```

### Using in Server Components

```typescript
// app/[locale]/page.tsx
import { getTranslations } from "next-intl/server";

export default async function Page({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  
  const t = await getTranslations({ locale, namespace: "home" });
  
  return (
    <div>
      <h1>{t("title")}</h1>
      <p>{t("description")}</p>
      <p>{t("greeting", { name: "User" })}</p>
    </div>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `getRequestConfig` | Configures request handling | next-intl needs this to know how to load translations |
| `import(\`../messages/${locale}.json\`)` | Dynamic import | Loads correct translation file for locale |
| `createMiddleware` | Sets up locale detection | Handles redirects and locale in URL |
| `createNextIntlPlugin` | Wraps Next.js config | Enables next-intl in your app |
| `getMessages()` | Gets translations for current locale | Used in Server Components |
| `useTranslations()` | Hook for client components | Access translations in interactive components |

## Common Mistakes

### Mistake 1: Forgetting to Validate Locale

```typescript
// WRONG - Any locale works
export default getRequestConfig(async ({ locale }) => {
  return {
    messages: (await import(`../messages/${locale}.json`)).default
  };
  // If locale is "invalid", this crashes!
});

// CORRECT - Validate locale
export default getRequestConfig(async ({ locale }) => {
  if (!locales.includes(locale as any)) notFound();
  return {
    messages: (await import(`../messages/${locale}.json`)).default
  };
});
```

### Mistake 2: Not Wrapping with Provider

```typescript
// WRONG - Missing provider
export default function Layout({ children }) {
  return <html><body>{children}</body></html>;
  // Translations won't work!
}

// CORRECT - Wrap with provider
export default async function Layout({ children }) {
  const messages = await getMessages();
  
  return (
    <html>
      <body>
        <NextIntlClientProvider messages={messages}>
          {children}
        </NextIntlClientProvider>
      </body>
    </html>
  );
}
```

### Mistake 3: Wrong Path for Messages

```typescript
// WRONG - Wrong path
messages: (await import(`./messages/${locale}.json`)).default
// Looking in wrong folder!

// CORRECT - Correct path from lib/i18n.ts
messages: (await import(`../messages/${locale}.json`)).default
```

## Summary

- Install next-intl: `npm install next-intl`
- Create `lib/i18n.ts` with `getRequestConfig`
- Add middleware for automatic locale routing
- Update `next.config.mjs` with the plugin
- Wrap layout with `NextIntlClientProvider`
- Use `getTranslations` in Server Components
- Use `useTranslations` hook in Client Components

## Next Steps

- [translation-files.md](./translation-files.md) - Creating and organizing translation files
- [useTranslations-hook.md](./useTranslations-hook.md) - Deep dive into the translations hook
