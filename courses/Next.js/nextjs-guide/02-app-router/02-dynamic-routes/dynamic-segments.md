# Dynamic Segments in Next.js

## What You'll Learn
- How to create URL parameters that change
- How to capture values from the URL
- Building pages that work with any URL value

## Prerequisites
- Understanding of nested routes
- Basic knowledge of page files

## Concept Explained Simply

Sometimes you need pages where the URL isn't fixed. For example, a blog post could be `/blog/hello-world` or `/blog/nextjs-rocks` — you can't create a separate file for every possible blog post. This is where **dynamic segments** come in.

A dynamic segment is like a placeholder in your URL. You use square brackets around a name (like `[slug]`), and Next.js captures whatever value is in that position. The name you choose becomes available in your page's props.

Think of it like a form letter: instead of "Dear John" or "Dear Mary", you write "Dear [Name]" and fill it in later. Dynamic segments work the same way — the URL has a placeholder that gets filled in when someone visits.

## How Dynamic Segments Work

```
Folder Structure:
src/app/blog/
└── [slug]/
    └── page.tsx

URL: /blog/hello-world
Captured: { slug: "hello-world" }

URL: /blog/any-post-title
Captured: { slug: "any-post-title" }
```

## Complete Code Example

Let's create a product page that works for any product:

```typescript
// src/app/products/[id]/page.tsx
// URL: /products/:id

interface Props {
  params: Promise<{ id: string }>;
}

// Mock database
const products: Record<string, { id: string; name: string; price: number; description: string }> = {
  "1": { id: "1", name: "Laptop Pro", price: 1299, description: "Powerful laptop for professionals" },
  "2": { id: "2", name: "Wireless Mouse", price: 49, description: "Ergonomic wireless mouse" },
  "3": { id: "3", name: "Mechanical Keyboard", price: 199, description: "RGB mechanical keyboard" },
};

export default async function ProductPage({ params }: Props) {
  const { id } = await params;
  const product = products[id];

  if (!product) {
    return (
      <main style={{ padding: "2rem" }}>
        <h1>Product Not Found</h1>
        <p>Sorry, we couldn't find product #{id}</p>
      </main>
    );
  }

  return (
    <main style={{ maxWidth: "800px", margin: "0 auto", padding: "2rem" }}>
      <article>
        <header style={{ marginBottom: "2rem" }}>
          <h1 style={{ fontSize: "2.5rem", marginBottom: "0.5rem" }}>{product.name}</h1>
          <p style={{ fontSize: "1.5rem", color: "#0070f3", fontWeight: "bold" }}>
            ${product.price}
          </p>
        </header>
        <section style={{ lineHeight: "1.8" }}>
          <p>{product.description}</p>
        </section>
        <button
          style={{
            marginTop: "2rem",
            padding: "1rem 2rem",
            fontSize: "1rem",
            backgroundColor: "#0070f3",
            color: "white",
            border: "none",
            borderRadius: "8px",
            cursor: "pointer",
          }}
        >
          Add to Cart
        </button>
      </article>
    </main>
  );
}
```

Now let's add multiple dynamic segments:

```typescript
// src/app/products/[category]/[id]/page.tsx
// URL: /products/electronics/1

interface Props {
  params: Promise<{ category: string; id: string }>;
}

export default async function ProductPage({ params }: Props) {
  const { category, id } = await params;

  return (
    <main style={{ padding: "2rem" }}>
      <h1>Product Details</h1>
      <p>Category: {category}</p>
      <p>Product ID: {id}</p>
    </main>
  );
}
```

## Multiple Dynamic Segments

You can have multiple dynamic segments in one URL:

```
src/app/
├── products/
│   └── [category]/
│       ├── page.tsx         → /products/:category
│       └── [id]/
│           └── page.tsx     → /products/:category/:id
```

This creates URLs like:
- `/products/electronics`
- `/products/electronics/1`
- `/products/clothing/summer-2024`

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `[id]` | Dynamic segment syntax | Captures the URL segment |
| `interface Props` | TypeScript props type | Defines what props the page receives |
| `params: Promise<{ id: string }>` | Params as Promise | In Next.js 15, params is a Promise |
| `const { id } = await params` | Unwrap the params | Get the actual values |
| `products[id]` | Lookup by dynamic value | Fetch data based on URL |

## Accessing Dynamic Segments

### In Server Components

```typescript
// Direct in async component
export default async function Page({ params }: Props) {
  const { slug } = await params;
  // Use slug to fetch data
}
```

### In Client Components

```typescript
// With useParams hook
"use client";

import { useParams } from "next/navigation";

export default function ClientComponent() {
  const params = useParams();
  const slug = params.slug as string;
  
  return <p>Current slug: {slug}</p>;
}
```

## Common Mistakes

### Mistake #1: Not Handling Missing Data

```typescript
// ✗ Wrong: Assuming product always exists
export default async function Page({ params }: Props) {
  const { id } = await params;
  const product = products[id];
  return <h1>{product.name}</h1>; // Crashes if product is undefined!
}

// ✓ Correct: Handle missing data
export default async function Page({ params }: Props) {
  const { id } = await params;
  const product = products[id];
  
  if (!product) {
    return <h1>Product not found</h1>;
  }
  
  return <h1>{product.name}</h1>;
}
```

### Mistake #2: Forgetting to Await Params

```typescript
// ✗ Wrong: Not awaiting
export default function Page({ params }) {
  const { id } = params; // May not work correctly
}

// ✓ Correct: Await params
export default async function Page({ params }: Props) {
  const { id } = await params;
}
```

### Mistake #3: Wrong Type for Dynamic Segment

```typescript
// ✗ Wrong: Type is string, not number
export default async function Page({ params }: { params: { id: number } }) {
  const { id } = await params; // TypeScript error
}

// ✓ Correct: Type is string
export default async function Page({ params }: { params: { id: string } }) {
  const { id } = await params;
}
```

## Summary

- Dynamic segments use square brackets: `[slug]`
- The segment name becomes a key in the `params` object
- Multiple segments: `[category]/[id]`
- Always handle cases where the dynamic value doesn't exist
- In Next.js 15, `params` is a Promise — always await it

## Next Steps

Let's learn about catch-all routes for more flexible URL handling:

- [Catch-all Routes →](./catch-all-routes.md)
