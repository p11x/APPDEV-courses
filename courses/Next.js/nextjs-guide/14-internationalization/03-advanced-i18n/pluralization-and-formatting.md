# Pluralization and Formatting

## What You'll Learn
- Using ICU message format for plurals
- Number formatting across locales
- Date and time formatting
- Currency formatting

## Prerequisites
- Understanding of translation files
- Knowledge of next-intl
- Familiarity with ICU message format

## Concept Explained Simply

Different languages handle plurals differently. In English, there's "1 item" and "2 items" — just singular and plural. But in Russian, there are three plural forms! And in Arabic, there are six! That's why translation systems use the ICU Message Format — it's an international standard that handles all these variations elegantly.

Beyond plurals, when you show a date or price to someone in France, it should look French: dates as "1er janvier" and prices as "99,99 €". This is called formatting, and next-intl makes it easy.

## Complete Code Example

### Translation File with Plurals

```json
// messages/en.json
{
  "cart": {
    "items": "{count, plural, =0 {Your cart is empty} one {1 item} other {# items}}",
    "notifications": "{count, plural, =0 {No new notifications} one {1 new notification} other {# new notifications}}"
  },
  "messages": {
    "unread": "{count, plural, =0 {No messages} one {You have 1 new message} other {You have # new messages}}"
  }
}
```

```json
// messages/ru.json (Russian has 3 plural forms!)
{
  "cart": {
    "items": "{count, plural, =0 {Ваша корзина пуста} one {1 товар} few {# товара} many {# товаров} other {# товара}}"
  }
}
```

### Using Plurals in Components

```typescript
// components/Cart.tsx
"use client";

import { useTranslations } from "next-intl";

interface CartProps {
  itemCount: number;
}

export default function Cart({ itemCount }: CartProps) {
  const t = useTranslations("cart");
  
  return (
    <div>
      <h2>Shopping Cart</h2>
      <p>{t("items", { count: itemCount })}</p>
    </div>
  );
}

// If itemCount is 0: "Your cart is empty"
// If itemCount is 1: "1 item"
// If itemCount is 5: "5 items"
```

### Number Formatting

```typescript
// components/Stats.tsx
"use client";

import { useTranslations, useLocale } from "next-intl";

export default function Stats({ users, revenue }: { users: number; revenue: number }) {
  const locale = useLocale();
  
  // Format numbers based on locale
  const formattedUsers = new Intl.NumberFormat(locale).format(users);
  const formattedRevenue = new Intl.NumberFormat(locale, {
    style: "currency",
    currency: "USD"
  }).format(revenue);
  
  return (
    <div>
      <p>Total Users: {formattedUsers}</p>
      <p>Revenue: {formattedRevenue}</p>
    </div>
  );
}

// locale "en": "1,234" and "$1,234.56"
// locale "de": "1.234" and "1.234,56 €"
// locale "ja": "1,234" and "¥1,235"
```

### Date Formatting

```typescript
// components/EventDate.tsx
"use client";

import { useLocale } from "next-intl";

interface EventDateProps {
  date: Date;
}

export default function EventDate({ date }: EventDateProps) {
  const locale = useLocale();
  
  // Short date
  const short = new Intl.DateTimeFormat(locale, {
    dateStyle: "short"
  }).format(date);
  
  // Long date
  const long = new Intl.DateTimeFormat(locale, {
    dateStyle: "long"
  }).format(date);
  
  // With time
  const withTime = new Intl.DateTimeFormat(locale, {
    dateStyle: "medium",
    timeStyle: "short"
  }).format(date);
  
  return (
    <div>
      <p>Short: {short}</p>
      <p>Long: {long}</p>
      <p>With time: {withTime}</p>
    </div>
  );
}

// locale "en": "1/15/2025", "January 15, 2025", "Jan 15, 2025, 3:30 PM"
// locale "de": "15.01.2025", "15. Januar 2025", "15.01.2025, 15:30"
// locale "ja": "2025/01/15", "2025年1月15日", "2025/01/15 15:30"
```

### Relative Time

```typescript
// components/RelativeTime.tsx
"use client";

import { useTranslations, useLocale } from "next-intl";

interface RelativeTimeProps {
  timestamp: Date;
}

export default function RelativeTime({ timestamp }: RelativeTimeProps) {
  const locale = useLocale();
  const t = useTranslations("time");
  
  const now = new Date();
  const diff = now.getTime() - timestamp.getTime();
  const minutes = Math.floor(diff / 60000);
  const hours = Math.floor(diff / 3600000);
  const days = Math.floor(diff / 86400000);
  
  let relative;
  if (minutes < 1) {
    relative = t("justNow");
  } else if (minutes < 60) {
    relative = t("minutesAgo", { count: minutes });
  } else if (hours < 24) {
    relative = t("hoursAgo", { count: hours });
  } else {
    relative = t("daysAgo", { count: days });
  }
  
  return <time>{relative}</time>;
}
```

### Complete Translation Examples

```json
// messages/en.json - Time translations
{
  "time": {
    "justNow": "just now",
    "minutesAgo": "{count} minute(s) ago",
    "hoursAgo": "{count} hour(s) ago",
    "daysAgo": "{count} day(s) ago"
  }
}
```

## ICU Message Format Reference

| Format | Example | Description |
|--------|---------|-------------|
| `{count}` | `5` | Simple variable |
| `{count, plural, =0 {zero} one {one} other {many}}` | Varies by count | Plural selection |
| `{date, date, short}` | `1/15/25` | Date format |
| `{price, number, currency}` | `$99.99` | Currency format |

## Common Mistakes

### Mistake 1: Wrong Plural Syntax

```typescript
// WRONG - Missing plural forms
// Translation: "items": "{count} items"
// When count is 0, shows "0 items" which is grammatically wrong!

// CORRECT - Include all forms
// Translation: "{count, plural, =0 {No items} one {1 item} other {# items}}"
t("items", { count: 0 }); // "No items"
t("items", { count: 1 }); // "1 item"
t("items", { count: 5 }); // "5 items"
```

### Mistake 2: Not Using Intl for Numbers

```typescript
// WRONG - Manual formatting
const price = "$" + (99.99).toFixed(2);
// Not locale-aware!

// CORRECT - Use Intl
const price = new Intl.NumberFormat(locale, {
  style: "currency",
  currency: "USD"
}).format(99.99);
```

### Mistake 3: Forgetting Locale for Dates

```typescript
// WRONG - Hardcoded format
const date = new Date().toLocaleDateString();
// Uses system locale, not user's locale!

// CORRECT - Use user's locale
const locale = useLocale();
const date = new Date().toLocaleDateString(locale, {
  dateStyle: "long"
});
```

## Summary

- Use ICU Message Format for plurals in translations
- Different languages have different plural rules
- Use `Intl.NumberFormat` for locale-aware numbers
- Use `Intl.DateTimeFormat` for locale-aware dates
- Use `useLocale()` hook to get current locale
- Test with different locales to ensure proper formatting

## Next Steps

- [rtl-support.md](./rtl-support.md) - Right-to-left language support
- [routing-with-locales.md](../01-i18n-basics/routing-with-locales.md) - Locale-based routing
