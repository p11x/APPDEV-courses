# Static Rendering in Next.js

## What You'll Learn
- What is static rendering
- When to use it
- How it works

## Concept Explained Simply

**Static Rendering** renders pages at build time. The HTML is generated once and reused for every request. This is the fastest rendering method.

## When to Use

- Pages that don't change often
- Blog posts, documentation
- Marketing pages
- Product pages with infrequent updates

## Complete Example

```typescript
// src/app/about/page.tsx
// This page is statically rendered by default

export default async function AboutPage() {
  // This runs at build time
  return (
    <main>
      <h1>About Us</h1>
      <p>This content is statically generated.</p>
    </main>
  );
}
```

```typescript
// With dynamic data - still static if cached
export default async function BlogPage() {
  const posts = await fetch("https://api.example.com/posts", {
    cache: "force-cache",
  }).then((res) => res.json());

  return (
    <ul>
      {posts.map((p: any) => <li key={p.id}>{p.title}</li>)}
    </ul>
  );
}
```

## Summary

- Static = fastest performance
- HTML generated at build time
- Perfect for content that rarely changes
