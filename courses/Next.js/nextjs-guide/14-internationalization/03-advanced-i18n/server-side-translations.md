# Server-Side Translations

## What You'll Learn
- Using getTranslations in Server Components
- Getting translations in layouts
- Using translations in server actions
- Optimizing translation loading

## Prerequisites
- Understanding of next-intl setup
- Knowledge of Server Components
- Familiarity with async/await

## Concept Explained Simply

Server Components are like the kitchen in a restaurant — they prepare everything on the server side before sending it to the customer. When it comes to translations, using them on the server is more efficient because the translations are loaded once on the server, and the user receives already-translated HTML. No extra JavaScript needs to download for translations.

The `getTranslations` function is the server-side equivalent of `useTranslations`. It's async (because it loads files), but otherwise works similarly.

## Complete Code Example

### Basic Server Component Translation

```typescript
// app/[locale]/page.tsx
import { getTranslations } from "next-intl/server";

export default async function HomePage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  
  // Get translations - must await because it's async
  const t = await getTranslations({ locale, namespace: "home" });
  
  return (
    <main>
      <h1>{t("title")}</h1>
      <p>{t("description")}</p>
      <p>{t("greeting", { name: "User" })}</p>
    </main>
  );
}
```

### Using in Layouts

```typescript
// app/[locale]/layout.tsx
import { getTranslations } from "next-intl/server";
import { notFound } from "next/navigation";

const locales = ["en", "es", "fr", "de"];

export default async function LocaleLayout({
  children,
  params,
}: {
  children: React.ReactNode;
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  
  if (!locales.includes(locale)) {
    notFound();
  }
  
  const t = await getTranslations({ locale, namespace: "layout" });
  
  return (
    <html lang={locale}>
      <body>
        <header>
          <nav>
            <a href={`/${locale}`}>{t("home")}</a>
            <a href={`/${locale}/about`}>{t("about")}</a>
          </nav>
        </header>
        <main>{children}</main>
        <footer>
          <p>{t("footer")}</p>
        </footer>
      </body>
    </html>
  );
}
```

### Multiple Namespaces in Server Components

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
  
  // Get product
  const product = await db.product.findUnique({ where: { id } });
  if (!product) notFound();
  
  // Get translations from multiple namespaces
  const t = await getTranslations({ locale, namespace: "products" });
  const tCommon = await getTranslations({ locale, namespace: "common" });
  
  return (
    <article>
      <h1>{product.name[locale] || product.name.en}</h1>
      <p>{product.description[locale] || product.description.en}</p>
      
      <div>
        <span>{t("price")}: ${product.price}</span>
        <button>{t("addToCart")}</button>
      </div>
      
      <a href={`/${locale}/products`}>
        {tCommon("backToList")}
      </a>
    </article>
  );
}
```

### Using in Server Actions

```typescript
// lib/actions.ts
"use server";

import { getTranslations } from "next-intl/server";
import { revalidatePath } from "next/cache";

export async function createProduct(formData: FormData, locale: string) {
  const name = formData.get("name") as string;
  const price = parseFloat(formData.get("price") as string);
  
  // Create product in database
  const product = await db.product.create({
    data: { name: { en: name }, price }
  });
  
  // Get translation for success message
  const t = await getTranslations({ locale, namespace: "products" });
  
  revalidatePath(`/${locale}/products`);
  
  return { 
    success: true, 
    message: t("createSuccess") 
  };
}
```

### Optimizing with Parallel Requests

```typescript
// app/[locale]/page.tsx
import { getTranslations } from "next-intl/server";

// Using Promise.all for parallel loading
export default async function HomePage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  
  // Load multiple namespaces in parallel
  const [homeT, commonT, footerT] = await Promise.all([
    getTranslations({ locale, namespace: "home" }),
    getTranslations({ locale, namespace: "common" }),
    getTranslations({ locale, namespace: "footer" }),
  ]);
  
  return (
    <main>
      <h1>{homeT("title")}</h1>
      <nav>
        <a href="/">{commonT("home")}</a>
      </nav>
      <footer>
        <p>{footerT("copyright")}</p>
      </footer>
    </main>
  );
}
```

## Comparison: Server vs Client

| Scenario | Use | Reason |
|----------|-----|--------|
| Page content | getTranslations in Server Component | Pre-rendered, no extra JS |
| Interactive UI | useTranslations in Client Component | Responds to user actions |
| Form actions | getTranslations in Server Action | Return translated messages |
| Error messages | useTranslations in Client Component | Dynamic based on user input |

## Common Mistakes

### Mistake 1: Not Awaiting getTranslations

```typescript
// WRONG - Forgot to await
export default async function Page({ params }: { params: Promise<{ locale: string }> }) {
  const { locale } = await params;
  const t = getTranslations({ locale, namespace: "home" }); // Returns Promise!
  
  return <h1>{t("title")}</h1>; // Promise object, not string!
}

// CORRECT - Await the result
export default async function Page({ params }: { params: Promise<{ locale: string }> }) {
  const { locale } = await params;
  const t = await getTranslations({ locale, namespace: "home" });
  
  return <h1>{t("title")}</h1>;
}
```

### Mistake 2: Using in Client Component

```typescript
// WRONG - getTranslations in Client Component
"use client";

import { getTranslations } from "next-intl/server"; // Wrong import!

export default function ClientComponent() {
  const t = getTranslations("home"); // Doesn't work here!
}

// CORRECT - Use useTranslations in Client Components
"use client";

import { useTranslations } from "next-intl";

export default function ClientComponent() {
  const t = useTranslations("home");
  return <h1>{t("title")}</h1>;
}
```

### Mistake 3: Not Passing Locale

```typescript
// WRONG - Missing locale
const t = await getTranslations({ namespace: "home" });
// next-intl won't know which language!

// CORRECT - Always pass locale
const { locale } = await params;
const t = await getTranslations({ locale, namespace: "home" });
```

## Summary

- Use `getTranslations` in Server Components (async)
- Use `useTranslations` in Client Components
- Always await the result of getTranslations
- Load multiple namespaces with Promise.all for efficiency
- Use getTranslations in Server Actions for translated success/error messages
- Pass the locale parameter to getTranslations

## Next Steps

- [pluralization-and-formatting.md](./pluralization-and-formatting.md) - Advanced formatting
- [rtl-support.md](./rtl-support.md) - Right-to-left language support
