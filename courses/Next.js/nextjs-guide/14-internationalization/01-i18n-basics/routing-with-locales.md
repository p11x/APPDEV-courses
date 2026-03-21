# Routing with Locales

## What You'll Learn
- Setting up locale-based routing
- Creating localized page components
- Handling localized navigation
- Making all routes work with locales

## Prerequisites
- Understanding of i18n basics
- Knowledge of App Router
- Familiarity with middleware

## Concept Explained Simply

Localized routing means your URLs include the language code: instead of just `/about`, you have `/en/about`, `/es/about`, `/fr/about`. This is great for SEO because search engines understand which language each page is for, and users can share links knowing they'll get the right language.

The trick is setting up your app so that every page automatically works with any locale. You don't want to create separate pages for each language — instead, you create ONE page that reads the locale from the URL and shows the right translations.

## Complete Code Example

### Project Structure

```
app/
├── [locale]/                    # Dynamic locale segment
│   ├── page.tsx                 # Home page (/en, /es, /fr, etc.)
│   ├── about/
│   │   └── page.tsx             # About page (/en/about, etc.)
│   ├── products/
│   │   ├── page.tsx             # Products list
│   │   └── [id]/
│   │       └── page.tsx         # Product detail
│   └── layout.tsx               # Root layout with locale
├── api/
│   └── [...locale]/
│       └── route.ts             # Catch-all API routes
└── page.tsx                     # Redirects to default locale
```

### Root Layout with Locale

```typescript
// app/[locale]/layout.tsx
import { notFound } from "next/navigation";
import { NextIntlClientProvider } from "next-intl";
import { getMessages } from "next-intl/server";

const locales = ["en", "es", "fr", "de"];

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
  
  // Get translations for this locale
  const messages = await getMessages();
  
  return (
    <html lang={locale}>
      <body>
        <NextIntlClientProvider messages={messages}>
          <nav>
            {/* Navigation component */}
            <LocaleSwitcher />
          </nav>
          <main>{children}</main>
        </NextIntlClientProvider>
      </body>
    </html>
  );
}

// Generate static params for build time
export async function generateStaticParams() {
  return locales.map((locale) => ({ locale }));
}
```

### Localized Home Page

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
    <div>
      <h1>{t("title")}</h1>
      <p>{t("description")}</p>
      
      {/* Using translations with variables */}
      <p>{t("greeting", { name: "User" })}</p>
    </div>
  );
}

// Optional: Set metadata for this page
export async function generateMetadata({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  const t = await getTranslations({ locale, namespace: "metadata" });
  
  return {
    title: t("home.title"),
    description: t("home.description"),
  };
}
```

### Localized Product Page

```typescript
// app/[locale]/products/[id]/page.tsx
import { getTranslations } from "next-intl/server";
import { notFound } from "next/navigation";
import { db } from "@/lib/db";

export default async function ProductPage({
  params,
}: {
  params: Promise<{ locale: string; id: string }>;
}) {
  const { locale, id } = await params;
  const t = await getTranslations({ locale, namespace: "products" });
  
  // Fetch product
  const product = await db.product.findUnique({
    where: { id },
  });
  
  if (!product) {
    notFound();
  }
  
  // Get localized product name
  const localizedName = product.name[locale as keyof typeof product.name] || product.name.en;
  const localizedDescription = product.description[locale as keyof typeof product.description] || product.description.en;
  
  return (
    <div>
      <h1>{localizedName}</h1>
      <p>{localizedDescription}</p>
      <p>
        {t("price", { 
          price: product.price.toFixed(2),
          currency: locale === "en" ? "USD" : "EUR" 
        })}
      </p>
    </div>
  );
}
```

### Locale Switcher Component

```typescript
// components/LocaleSwitcher.tsx
"use client";

import { useRouter, usePathname } from "next/navigation";

const locales = [
  { code: "en", name: "English", flag: "🇺🇸" },
  { code: "es", name: "Español", flag: "🇪🇸" },
  { code: "fr", name: "Français", flag: "🇫🇷" },
  { code: "de", name: "Deutsch", flag: "🇩🇪" },
];

export default function LocaleSwitcher() {
  const router = useRouter();
  const pathname = usePathname();
  
  // Extract current locale from path
  const currentLocale = pathname.split("/")[1] || "en";
  
  const handleLocaleChange = (newLocale: string) => {
    // Replace current locale in path
    const newPath = pathname.replace(`/${currentLocale}`, `/${newLocale}`);
    router.push(newPath);
  };
  
  return (
    <select
      value={currentLocale}
      onChange={(e) => handleLocaleChange(e.target.value)}
      className="locale-select"
    >
      {locales.map((locale) => (
        <option key={locale.code} value={locale.code}>
          {locale.flag} {locale.name}
        </option>
      ))}
    </select>
  );
}
```

### Redirect from Root to Default Locale

```typescript
// app/page.tsx
import { redirect } from "next/navigation";

export default function RootPage() {
  // Redirect to default locale (en)
  redirect("/en");
}
```

## Key Patterns

| Pattern | File | URL |
|---------|------|-----|
| Root with locale | `app/[locale]/page.tsx` | `/en`, `/es` |
| Nested page | `app/[locale]/about/page.tsx` | `/en/about` |
| Dynamic segment | `app/[locale]/products/[id]/page.tsx` | `/en/products/123` |
| API route | `app/api/[locale]/route.ts` | `/api/en/users` |

## Common Mistakes

### Mistake 1: Not Validating Locale Parameter

```typescript
// WRONG - Any value in locale param works
export default function Page({ params }: { params: { locale: string } }) {
  const { locale } = params;
  // locale could be anything!
}

// CORRECT - Validate against supported locales
const locales = ["en", "es", "fr", "de"];

export default function Page({ params }: { params: { locale: string } }) {
  const { locale } = params;
  
  if (!locales.includes(locale)) {
    notFound(); // Or redirect to default locale
  }
}
```

### Mistake 2: Forgetting to Handle Static Generation

```typescript
// WRONG - Without generateStaticParams, localized static pages won't work
// app/[locale]/page.tsx

// CORRECT - Add generateStaticParams
export async function generateStaticParams() {
  return locales.map((locale) => ({ locale }));
}
```

### Mistake 3: Not Localizing Links

```typescript
// WRONG - Links lose locale context
<Link href="/about">About</Link> 
// Goes to /about, which might 404!

// CORRECT - Include locale in href
<Link href={`/${locale}/about`}>About</Link>
```

## Summary

- Use `[locale]` as a dynamic segment in your route structure
- Validate locale parameter against supported locales
- Use `generateStaticParams` for static site generation
- Create a LocaleSwitcher component for users to change language
- Include locale in all internal links
- Redirect root path to default locale
- Access locale in components via params

## Next Steps

- [installing-next-intl.md](../02-next-intl-setup/installing-next-intl.md) - Setting up next-intl library
- [translation-files.md](../02-next-intl-setup/translation-files.md) - Creating and organizing translation files
