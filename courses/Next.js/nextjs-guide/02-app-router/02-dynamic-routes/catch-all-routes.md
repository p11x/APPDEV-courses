# Catch-all Routes in Next.js

## What You'll Learn
- How to capture multiple URL segments at once
- When to use catch-all vs. regular dynamic segments
- Building flexible URL handlers

## Prerequisites
- Understanding of dynamic segments
- Knowledge of URL structure

## Concept Explained Simply

Sometimes you don't know how many URL segments you'll have. For example, consider a documentation site where URLs could be:
- `/docs/getting-started`
- `/docs/getting-started/installation`
- `/docs/getting-started/installation/windows`

A regular dynamic segment like `[slug]` only captures one part. But a **catch-all segment** uses three dots (`[...slug]`) and captures everything after that point as an array.

Think of it like a greedy eater: regular dynamic segments eat one sandwich, but catch-all segments eat everything on the table and save it all for later.

## Catch-all Syntax

```
[...slug]  ← Catch-all (one or more segments)
```

The captured value is an array of strings:

```
URL: /docs/getting-started/installation
Captured: { slug: ["getting-started", "installation"] }
```

## Complete Code Example

Let's build a documentation page that handles any URL depth:

```typescript
// src/app/docs/[...slug]/page.tsx
// URL: /docs/:path*

interface Props {
  params: Promise<{ slug: string[] }>;
}

// Mock documentation data
const docs: Record<string, { title: string; content: string }> = {
  "getting-started": {
    title: "Getting Started",
    content: "Welcome to our documentation! Let's get you started.",
  },
  "getting-started/installation": {
    title: "Installation",
    content: "Follow these steps to install our product...",
  },
  "getting-started/installation/windows": {
    title: "Windows Installation",
    content: "To install on Windows, download the installer and run it...",
  },
};

export default async function DocsPage({ params }: Props) {
  const { slug } = await params;
  
  // Convert array to key for lookup
  const key = slug.join("/");
  const doc = docs[key];

  if (!doc) {
    return (
      <main style={{ padding: "2rem" }}>
        <h1>Documentation Not Found</h1>
        <p>The page "{key}" doesn't exist.</p>
        <p>Try one of these pages:</p>
        <ul>
          <li><a href="/docs/getting-started">Getting Started</a></li>
          <li><a href="/docs/getting-started/installation">Installation</a></li>
        </ul>
      </main>
    );
  }

  // Generate breadcrumbs
  const breadcrumbs = slug.map((segment, index) => {
    const path = slug.slice(0, index + 1).join("/");
    return { label: segment, href: `/docs/${path}` };
  });

  return (
    <main style={{ maxWidth: "800px", margin: "0 auto", padding: "2rem" }}>
      {/* Breadcrumb navigation */}
      <nav style={{ marginBottom: "2rem" }}>
        <ol style={{ display: "flex", gap: "0.5rem", listStyle: "none", padding: 0 }}>
          <li><a href="/docs">Docs</a></li>
          {breadcrumbs.map((crumb, index) => (
            <li key={crumb.href} style={{ display: "flex", gap: "0.5rem" }}>
              <span>/</span>
              <a href={crumb.href}>{crumb.label}</a>
            </li>
          ))}
        </ol>
      </nav>

      <article>
        <h1 style={{ fontSize: "2.5rem", marginBottom: "1rem" }}>{doc.title}</h1>
        <div style={{ lineHeight: "1.8", fontSize: "1.1rem" }}>
          <p>{doc.content}</p>
        </div>
      </article>
    </main>
  );
}
```

## How It Maps to URLs

| URL | Captured `slug` |
|-----|-----------------|
| `/docs/getting-started` | `["getting-started"]` |
| `/docs/getting-started/installation` | `["getting-started", "installation"]` |
| `/docs/a/b/c/d` | `["a", "b", "c", "d"]` |

## Comparison: Dynamic vs Catch-all

```typescript
// Dynamic segment: [id]
// URL: /products/123
// Captured: { id: "123" }

// Catch-all: [...id]
// URL: /products/123
// Captured: { id: ["123"] }

// Catch-all with multiple:
// URL: /products/123/review
// Captured: { id: ["123", "review"] }
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `[...slug]` | Catch-all syntax | Captures all remaining URL segments |
| `slug: string[]` | Type as array | The captured value is an array |
| `slug.join("/")` | Convert to string | Turns array back into lookup key |
| `slug.map()` | Create breadcrumbs | Builds navigation from captured segments |
| `slice(0, index + 1)` | Partial path | Builds each breadcrumb link |

## Common Mistakes

### Mistake #1: Using Catch-all When Not Needed

```typescript
// ✗ Wrong: Overusing catch-all
[...slug]  // Too flexible, harder to handle

// ✓ Correct: Use regular dynamic segment when you know the depth
[category]/[id]  // Clear structure
```

### Mistake #2: Forgetting It's an Array

```typescript
// ✗ Wrong: Treating as string
export default async function Page({ params }: Props) {
  const { slug } = await params;
  const doc = docs[slug]; // Error: slug is array, not string
}

// ✓ Correct: Join array to string
export default async function Page({ params }: Props) {
  const { slug } = await params;
  const doc = docs[slug.join("/")]; // Convert array to string
}
```

### Mistake #3: Not Handling Edge Cases

```typescript
// ✗ Wrong: Empty slug array could crash
const key = slug.join("/"); // Works, but be careful

// ✓ Correct: Handle the root case
export default async function Page({ params }: Props) {
  const { slug } = await params;
  
  // If someone visits /docs without anything after
  if (slug.length === 0) {
    return <h1>Welcome to Docs</h1>;
  }
}
```

## When to Use Catch-all

- Documentation sites with variable depth
- Multi-language URLs like `/en/products` or `/fr/products`
- File browsers or folder structures
- CMS-style URLs where path depth is unknown

## Summary

- Catch-all uses `[...segment]` syntax
- Captures multiple URL segments as an array
- Useful for variable-depth URLs like documentation
- Always handle the array properly when looking up data
- Consider if you really need catch-all or if regular dynamic segments work better

## Next Steps

Now let's learn about optional catch-all routes:

- [Optional Catch-all →](./optional-catch-all.md)
