# useTranslations Hook

## What You'll Learn
- Using translations in Client Components
- Switching between namespaces
- Using pluralization
- Working with dates and numbers

## Prerequisites
- Understanding of next-intl setup
- Basic React knowledge
- Understanding of Client vs Server Components

## Concept Explained Simply

The `useTranslations` hook is like having a translator right inside your component. Just as you might ask a translator friend "How do I say 'hello' in Spanish?", your component can ask `useTranslations` "Give me the translation for 'greeting'."

This hook is specifically for Client Components — interactive parts of your UI that need to respond to user actions. For Server Components, we use `getTranslations` instead, which works similarly but doesn't need a hook.

## Complete Code Example

### Basic Usage in Client Component

```typescript
// components/Navigation.tsx
"use client";

import { useTranslations } from "next-intl";

export default function Navigation() {
  // Get translations from "common" namespace
  const t = useTranslations("common");
  
  return (
    <nav>
      <a href="/">{t("home")}</a>
      <a href="/about">{t("about")}</a>
      <button>{t("logout")}</button>
    </nav>
  );
}
```

### Using Multiple Namespaces

```typescript
// components/ProductCard.tsx
"use client";

import { useTranslations } from "next-intl";

interface ProductCardProps {
  name: string;
  price: number;
}

export default function ProductCard({ name, price }: ProductCardProps) {
  // Can use multiple namespaces
  const t = useTranslations("products");
  const tCommon = useTranslations("common");
  
  return (
    <div className="product-card">
      <h3>{name}</h3>
      <p>{t("price")}: ${price}</p>
      <button>{t("addToCart")}</button>
    </div>
  );
}
```

### Using Translations with Variables

```typescript
// components/Greeting.tsx
"use client";

import { useTranslations } from "next-intl";

interface GreetingProps {
  name: string;
}

export default function Greeting({ name }: GreetingProps) {
  const t = useTranslations("home");
  
  // Using variables in translations
  return (
    <div>
      <h1>{t("greeting", { name })}</h1>
    </div>
  );
}

// In translation file:
// "greeting": "Hello, {name}!"
// Output: "Hello, John!"
```

### Pluralization

```typescript
// components/CartSummary.tsx
"use client";

import { useTranslations } from "next-intl";

interface CartSummaryProps {
  itemCount: number;
}

export default function CartSummary({ itemCount }: CartSummaryProps) {
  const t = useTranslations("cart");
  
  // Using pluralization
  return (
    <div>
      <p>
        {t("items", { 
          count: itemCount,
          // These are the plural forms in the translation file:
          // =0: "Your cart is empty"
          // one: "1 item in cart" 
          // other: "{count} items in cart"
        })}
      </p>
    </div>
  );
}

// Translation file:
// "items": "{count, plural, =0 {Your cart is empty} one {1 item} other {# items}}"
```

### Dates and Numbers

```typescript
// components/OrderSummary.tsx
"use client";

import { useTranslations, useLocale } from "next-intl";

interface OrderSummaryProps {
  orderDate: Date;
  total: number;
}

export default function OrderSummary({ orderDate, total }: OrderSummaryProps) {
  const t = useTranslations("orders");
  const locale = useLocale();
  
  // Format date using locale
  const formattedDate = new Intl.DateTimeFormat(locale, {
    year: "numeric",
    month: "long",
    day: "numeric"
  }).format(orderDate);
  
  // Format currency using locale
  const formattedTotal = new Intl.NumberFormat(locale, {
    style: "currency",
    currency: "USD"
  }).format(total);
  
  return (
    <div>
      <p>{t("date")}: {formattedDate}</p>
      <p>{t("total")}: {formattedTotal}</p>
    </div>
  );
}
```

### Namespace Switching

```typescript
// components/FormField.tsx
"use client";

import { useTranslations } from "next-intl";

interface FormFieldProps {
  name: string;
  label: string;
  error?: string;
}

export default function FormField({ name, label, error }: FormFieldProps) {
  // Switch namespace dynamically
  const t = useTranslations("validation");
  
  return (
    <div>
      <label>{label}</label>
      <input name={name} />
      {error && <span className="error">{t("required")}</span>}
    </div>
  );
}
```

## Comparison: useTranslations vs getTranslations

| Feature | useTranslations | getTranslations |
|---------|-----------------|-----------------|
| Where to use | Client Components | Server Components |
| How to import | `import { useTranslations } from "next-intl"` | `import { getTranslations } from "next-intl/server"` |
| Returns | Hook function | Promise |
| When to call | Inside component body | Await in async component |

## Common Mistakes

### Mistake 1: Using in Server Components

```typescript
// WRONG - useTranslations in Server Component
// app/page.tsx (Server Component)
import { useTranslations } from "next-intl"; // Wrong!

export default function Page() {
  const t = useTranslations("home"); // Won't work!
  return <h1>{t("title")}</h1>;
}

// CORRECT - Use getTranslations in Server Components
import { getTranslations } from "next-intl/server";

export default async function Page({ params }: { params: Promise<{ locale: string }> }) {
  const { locale } = await params;
  const t = await getTranslations({ locale, namespace: "home" });
  return <h1>{t("title")}</h1>;
}
```

### Mistake 2: Forgetting to Pass Locale

```typescript
// WRONG - Missing namespace
const t = useTranslations("home"); // But locale must be set somewhere!

// CORRECT - Locale is automatically detected by next-intl
// Just use namespace directly
const t = useTranslations("home");
```

### Mistake 3: Wrong Variable Syntax

```typescript
// WRONG - Wrong plural syntax
t("items", { count: itemCount });
// Translation: "items": "{count} items" // Missing plural forms!

// CORRECT - Use proper ICU plural syntax
// Translation: "items": "{count, plural, =0 {No items} one {1 item} other {# items}}"
t("items", { count: itemCount });
```

## Summary

- Use `useTranslations` in Client Components only
- Import from `next-intl` (not next-intl/server)
- Use namespace: `useTranslations("home")`
- Pass variables: `t("greeting", { name: "John" })`
- Handle plurals: `t("items", { count: n })`
- Use `useLocale()` for formatting dates/numbers
- For Server Components, use `getTranslations` instead

## Next Steps

- [server-side-translations.md](../03-advanced-i18n/server-side-translations.md) - Advanced server translation patterns
- [pluralization-and-formatting.md](../03-advanced-i18n/pluralization-and-formatting.md) - Deep dive into formatting
