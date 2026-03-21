# Extended Fetch API in Next.js

## What You'll Learn
- Extended fetch options in Next.js
- Cache control
- Request deduping

## Prerequisites
- Understanding of fetch API
- Basic Next.js knowledge

## Concept Explained Simply

Next.js extends the native fetch API with additional caching and revalidation options. This gives you fine-grained control over how your data is cached and when it's refreshed.

## Complete Code Examples

### Basic Fetch with Caching

```typescript
// src/app/blog/page.tsx

// Static - cached forever
export default async function BlogPage() {
  const posts = await fetch("https://api.example.com/posts", {
    cache: "force-cache", // Default
  }).then((res) => res.json());

  return <div>{posts.map((p: any) => p.title).join(", ")}</div>;
}
```

### Dynamic Fetch

```typescript
// Real-time data
const data = await fetch("https://api.example.com/stocks", {
  cache: "no-store", // Never cache
});
```

### Time-Based Revalidation

```typescript
// Revalidate every hour
const data = await fetch("https://api.example.com/products", {
  next: { revalidate: 3600 },
});
```

## Summary

- Use `cache: "force-cache"` for static data
- Use `cache: "no-store"` for real-time data
- Use `next: { revalidate: N }` for time-based caching
