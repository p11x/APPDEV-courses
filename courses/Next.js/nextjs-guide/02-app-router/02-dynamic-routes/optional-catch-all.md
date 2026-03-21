# Optional Catch-all Routes in Next.js

## What You'll Learn
- The difference between catch-all and optional catch-all
- When to use optional catch-all routes
- Building flexible page handlers

## Prerequisites
- Understanding of catch-all routes
- Knowledge of dynamic segments

## Concept Explained Simply

An **optional catch-all route** is just like a catch-all route, but it can also match nothing at all. The difference is in the syntax: `[...slug]` requires at least one segment, while `[[...slug]]` (note the double brackets) makes the entire segment optional.

This is useful when you want one page to handle both the base URL and any sub-paths. For example, a docs page might want to handle `/docs` AND `/docs/anything/else`.

Think of it like a phone number that's optional: regular catch-all is like asking for at least one digit, while optional catch-all is like saying "you can give me digits, but you don't have to."

## Syntax Comparison

| Syntax | Required? | Example URL | Captured |
|--------|-----------|-------------|----------|
| `[slug]` | Yes | `/hello` | `{ slug: "hello" }` |
| `[...slug]` | Yes | `/a/b` | `{ slug: ["a", "b"] }` |
| `[[...slug]]` | No | `/` or `/a/b` | `{ slug: [] }` or `{ slug: ["a", "b"] }` |

## Complete Code Example

Let's build a docs page that handles the root AND nested paths:

```typescript
// src/app/docs/[[...slug]]/page.tsx
// URL: /docs, /docs/, /docs/anything

interface Props {
  params: Promise<{ slug?: string[] }>;
}

const docs: Record<string, { title: string; content: string }> = {
  "": {
    title: "Documentation",
    content: "Welcome to our comprehensive documentation. Use the navigation to find what you need.",
  },
  "getting-started": {
    title: "Getting Started",
    content: "Welcome! Let's get you up and running quickly.",
  },
  "getting-started/installation": {
    title: "Installation",
    content: "Follow these steps to install our product...",
  },
  "api/reference": {
    title: "API Reference",
    content: "Complete API documentation with all endpoints.",
  },
};

export default async function DocsPage({ params }: Props) {
  const { slug } = await params;
  
  // Handle both /docs and /docs/something
  // If slug is undefined or empty, use empty string as key
  const key = (slug && slug.length > 0) ? slug.join("/") : "";
  const doc = docs[key];

  if (!doc) {
    return (
      <main style={{ padding: "2rem" }}>
        <h1>Page Not Found</h1>
        <p>The documentation page "{key || "home"}" doesn't exist.</p>
        <a href="/docs">← Back to Docs Home</a>
      </main>
    );
  }

  return (
    <main style={{ maxWidth: "800px", margin: "0 auto", padding: "2rem" }}>
      <header style={{ marginBottom: "2rem" }}>
        <h1 style={{ fontSize: "2.5rem", marginBottom: "0.5rem" }}>{doc.title}</h1>
      </header>
      <div style={{ lineHeight: "1.8", fontSize: "1.1rem" }}>
        <p>{doc.content}</p>
      </div>
      {key && (
        <footer style={{ marginTop: "3rem", paddingTop: "1rem", borderTop: "1px solid #ddd" }}>
          <a href="/docs">← Back to Documentation Home</a>
        </footer>
      )}
    </main>
  );
}
```

## When to Use Optional Catch-all

### Good Use Cases

1. **Multi-language support**
   - `[[...lang]]/page.tsx` handles `/`, `/en`, `/fr`, `/en/about`

2. **Documentation with home**
   - `[[...slug]]/page.tsx` handles `/docs` AND `/docs/anything`

3. **Optional path segments**
   - `/products[[...category]]` for `/products`, `/products/electronics`, etc.

### Comparison

```typescript
// Catch-all: [...slug]
// Must have at least one segment
// URL: /docs/a/b → OK
// URL: /docs → ERROR (no match)

// Optional catch-all: [[...slug]]
// Zero or more segments
// URL: /docs/a/b → OK  
// URL: /docs → OK (empty slug array)
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `[[...slug]]` | Optional catch-all syntax | Allows zero or more segments |
| `slug?: string[]` | Optional type | Can be undefined |
| `slug?.join("/")` | Safe join | Handles undefined case |
| `(slug && slug.length > 0)` | Check for content | Differentiates root from sub-paths |

## Common Mistakes

### Mistake #1: Forgetting It's Optional

```typescript
// ✗ Wrong: Not handling the empty case
export default async function Page({ params }: Props) {
  const { slug } = await params;
  const doc = docs[slug.join("/")]; // Crashes if slug is undefined!
}

// ✓ Correct: Handle optional case
export default async function Page({ params }: Props) {
  const { slug } = await params;
  const key = slug ? slug.join("/") : "";
  const doc = docs[key];
}
```

### Mistake #2: Type Error with Optional

```typescript
// ✗ Wrong: Required type for optional param
params: { slug: string[] }

// ✓ Correct: Optional type
params: { slug?: string[] }
```

### Mistake #3: Confusing with Regular Catch-all

```typescript
// ✗ Wrong: Using required catch-all when optional is better
[...slug]  // Requires at least one segment

// ✓ Correct: Optional when you want to handle root
[[...slug]]  // Handles root AND sub-paths
```

## Summary

- Optional catch-all uses double brackets: `[[...slug]]`
- Can match zero segments (the base URL) or multiple segments
- Useful for handling both `/docs` and `/docs/anything`
- Always handle the undefined/empty case in your code
- Types must reflect that the parameter is optional

## Next Steps

Now let's learn about route groups and how to organize routes:

- [What Are Route Groups →](../../02-app-router/03-route-groups/what-are-route-groups.md)
