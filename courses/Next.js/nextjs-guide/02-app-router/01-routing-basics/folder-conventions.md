# Folder Conventions in the App Router

## What You'll Learn
- The naming conventions for App Router folders
- Which folder names have special meaning
- How to organize your routes logically

## Prerequisites
- Understanding of file-based routing (from introduction)
- Familiarity with the Next.js project structure

## Concept Explained Simply

The App Router uses a set of conventions that determine how your routes work. Some folder names are just regular folders that create URL paths, while others have special meaning. Understanding these conventions is key to building robust Next.js applications.

Think of it like different types of signs on a highway. Some signs tell you where you're going (like "Paris → 50 miles"), while others warn you about road conditions or tell you what you can and can't do. In Next.js, some folder names create routes, while others modify how routes behave.

## Regular Folders vs. Special Folders

### Regular Folders (Create Routes)

These folders become part of the URL:

```typescript
src/app/
├── about/          →  /about
├── blog/           →  /blog
└── products/       →  /products
```

### Special Folders (Don't Create Routes)

These folders are wrapped in parentheses and are for organization only:

```typescript
src/app/
├── (marketing)/    // Doesn't appear in URL
│   ├── about/
│   └── contact/
└── (shop)/         // Doesn't appear in URL
    ├── blog/
    └── products/
```

The URLs would be `/about`, `/contact`, `/blog`, `/products`.

## Special File Names

Within any route folder, these filenames have special meaning:

| File | Purpose | Creates Route? |
|------|---------|----------------|
| `page.tsx` | The page content | ✅ Yes |
| `layout.tsx` | Shared UI wrapper | ❌ No |
| `loading.tsx` | Loading state UI | ❌ No |
| `error.tsx` | Error boundary UI | ❌ No |
| `not-found.tsx` | 404 page | ❌ No |
| `template.tsx` | Like layout, remounts | ❌ No |
| `route.ts` | API endpoint | ✅ Yes |

## Complete Code Example

Here's a comprehensive example showing different folder types:

```typescript
// src/app/(marketing)/layout.tsx
// Note: This is in a route group, so it's not in the URL
export default function MarketingLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="marketing-layout">
      <nav>Marketing Nav</nav>
      {children}
      <footer>Marketing Footer</footer>
    </div>
  );
}
```

```typescript
// src/app/(marketing)/about/page.tsx
// URL: /about (not /marketing/about)
export default function AboutPage() {
  return (
    <main>
      <h1>About Us</h1>
      <p>Learn about our company!</p>
    </main>
  );
}
```

```typescript
// src/app/(shop)/layout.tsx
export default function ShopLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="shop-layout">
      <header>Shop Header</header>
      {children}
      <footer>Shop Footer</footer>
    </div>
  );
}
```

```typescript
// src/app/(shop)/products/page.tsx
// URL: /products
export default function ProductsPage() {
  return (
    <main>
      <h1>Our Products</h1>
      <p>Browse our catalog</p>
    </main>
  );
}
```

## Route Groups

**Route groups** are folders wrapped in parentheses like `(marketing)` or `(shop)`. They're useful for:

1. **Organizing routes** — Group related pages together
2. **Different layouts** — Apply different layouts to different route groups
3. **Logical structure** — Make your project easier to understand

The folder name in parentheses doesn't appear in the URL.

## Common Mistakes

### Mistake #1: Using Parentheses Incorrectly

```typescript
// ✗ Wrong: Parentheses create route groups, not URL segments
src/app/(about)/page.tsx  // Error: page.tsx must be in a folder

// ✓ Correct: Just use a regular folder or route group properly
src/app/about/page.tsx           // → /about
src/app/(info)/about/page.tsx   // → /about (group is invisible)
```

### Mistake #2: Mixing Up Special Files

```typescript
// ✗ Wrong: Using the wrong special file
src/app/products/error.tsx  // This is for catching errors
// For a product detail page, you need page.tsx

// ✓ Correct: Use page.tsx for content
src/app/products/page.tsx      // The page itself
src/app/products/error.tsx     // Error handling
src/app/products/loading.tsx  // Loading state
```

### Mistake #3: Forgetting the Route Group

If you want different layouts for different sections, use route groups:

```typescript
// Without route group: Same layout everywhere
src/app/
├── about/page.tsx
└── shop/page.tsx

// With route groups: Different layouts
src/app/
├── (marketing)/
│   ├── about/page.tsx
│   └── layout.tsx
└── (shop)/
    ├── shop/page.tsx
    └── layout.tsx
```

## Summary

- Regular folders create URL paths
- Route groups (folders in parentheses) don't appear in URLs
- Special files like `page.tsx`, `layout.tsx` have specific purposes
- Route groups let you apply different layouts to different sections
- Use route groups to organize complex applications

## Next Steps

Now let's learn about the specific special files:

- [Page and Layout Files →](./page-and-layout-files.md)
