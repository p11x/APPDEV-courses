# What Are Route Groups in Next.js

## What You'll Learn
- What route groups are and why they exist
- The syntax for creating route groups
- When to use route groups in your project

## Prerequisites
- Understanding of folder conventions
- Basic knowledge of layouts

## Concept Explained Simply

**Route groups** are folders wrapped in parentheses that let you organize your routes without affecting the URL. They're a way to group related pages together while keeping your URL structure clean.

Think of route groups like a filing cabinet with colored tab dividers. The tabs help you organize your files, but they don't appear in the file names. Route groups work the same way: they help you organize your code, but they don't appear in the URL.

This is incredibly useful when you want different layouts for different sections of your site, or when your route folder structure would otherwise get too deep and confusing.

## Syntax

Route groups use parentheses:

```
(src/app)
├── (marketing)/      ← Route group
│   ├── about/
│   └── contact/
└── (shop)/           ← Another route group
    ├── products/
    └── cart/
```

The URLs would be:
- `/about` (not `/marketing/about`)
- `/contact` (not `/marketing/contact`)
- `/products` (not `/shop/products`)

## Complete Code Example

Let's organize a site with marketing and shop sections:

```
src/app/
├── (marketing)/
│   ├── layout.tsx
│   ├── page.tsx          → /
│   ├── about/
│   │   └── page.tsx      → /about
│   └── contact/
│       └── page.tsx      → /contact
└── (shop)/
    ├── layout.tsx
    ├── products/
    │   ├── page.tsx      → /products
    │   └── [id]/
    │       └── page.tsx → /products/:id
    └── cart/
        └── page.tsx      → /cart
```

```typescript
// src/app/(marketing)/layout.tsx
export default function MarketingLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div style={{ minHeight: "100vh", backgroundColor: "#fafafa" }}>
      <header style={{ backgroundColor: "white", padding: "1rem", borderBottom: "1px solid #eee" }}>
        <nav style={{ maxWidth: "1200px", margin: "0 auto", display: "flex", gap: "2rem" }}>
          <a href="/" style={{ fontWeight: "bold" }}>Acme Inc</a>
          <a href="/about">About</a>
          <a href="/contact">Contact</a>
        </nav>
      </header>
      {children}
      <footer style={{ backgroundColor: "#333", color: "white", padding: "2rem", marginTop: "auto" }}>
        <p>© 2024 Acme Inc - Contact us at hello@acme.com</p>
      </footer>
    </div>
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
    <div style={{ minHeight: "100vh", backgroundColor: "#fff" }}>
      <header style={{ backgroundColor: "#0070f3", color: "white", padding: "1rem" }}>
        <nav style={{ maxWidth: "1200px", margin: "0 auto", display: "flex", gap: "2rem" }}>
          <a href="/" style={{ fontWeight: "bold", color: "white" }}>Shop</a>
          <a href="/products" style={{ color: "white" }}>Products</a>
          <a href="/cart" style={{ color: "white" }}>Cart (0)</a>
        </nav>
      </header>
      {children}
    </div>
  );
}
```

```typescript
// src/app/(marketing)/page.tsx → Homepage at /
export default function HomePage() {
  return (
    <main style={{ maxWidth: "1200px", margin: "0 auto", padding: "4rem 2rem", textAlign: "center" }}>
      <h1 style={{ fontSize: "3rem", marginBottom: "1rem" }}>Welcome to Acme Inc</h1>
      <p style={{ fontSize: "1.25rem", color: "#666", marginBottom: "2rem" }}>
        We make amazing products for amazing people.
      </p>
      <div style={{ display: "flex", gap: "1rem", justifyContent: "center" }}>
        <a href="/about" style={{ padding: "1rem 2rem", backgroundColor: "#0070f3", color: "white", textDecoration: "none", borderRadius: "8px" }}>
          Learn More
        </a>
        <a href="/products" style={{ padding: "1rem 2rem", backgroundColor: "#333", color: "white", textDecoration: "none", borderRadius: "8px" }}>
          Shop Now
        </a>
      </div>
    </main>
  );
}
```

## Why Use Route Groups?

### 1. Different Layouts

Apply different layouts to different route groups:

- Marketing pages: header with company nav
- Shop pages: header with cart
- Dashboard pages: sidebar layout

### 2. Code Organization

Keep related files together without polluting the URL:

```
(marketing)/
├── page.tsx
├── about/
└── contact/

(shop)/
├── products/
└── cart/
```

### 3. Multiple Root Layouts

You can have multiple layouts at different levels:

```
src/app/
├── (marketing)/
│   └── layout.tsx    ← Different from shop layout
├── (shop)/
│   └── layout.tsx
└── layout.tsx        ← Root layout (still wraps everything)
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `(marketing)` | Route group folder | Parentheses create route group |
| `layout.tsx` inside group | Section-specific layout | Different UI for different sections |
| No URL impact | Marketing not in URL | `/about`, not `/marketing/about` |

## Common Mistakes

### Mistake #1: Forgetting Parentheses

```typescript
// ✗ Wrong: Regular folder (affects URL)
src/app/marketing/about/page.tsx → /marketing/about

// ✓ Correct: Route group (no URL impact)
src/app/(marketing)/about/page.tsx → /about
```

### Mistake #2: Too Many Route Groups

```typescript
// ✗ Wrong: Over-organizing
src/app/(routes)/(pages)/(public)/(marketing)/about/page.tsx

// ✓ Correct: Keep it simple
src/app/(marketing)/about/page.tsx
```

### Mistake #3: Forgetting Root Layout

```typescript
// ✗ Wrong: Route group without root layout
src/app/(marketing)/about/page.tsx  // Need root layout.tsx too!

// ✓ Correct: Have root layout + route group layout
src/app/layout.tsx  // Root - wraps everything
src/app/(marketing)/layout.tsx  // Marketing - wraps marketing pages
```

## Summary

- Route groups use parentheses: `(folderName)`
- They don't affect the URL structure
- Useful for organizing code and applying different layouts
- Each route group can have its own layout
- Combine with nested routes for complex applications

## Next Steps

Now let's learn how to share layouts across routes:

- [Shared Layouts →](./shared-layouts.md)
