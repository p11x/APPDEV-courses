# Full Route Cache in Next.js

## What You'll Learn
- The Full Route Cache
- How HTML is cached

## Concept Explained Simply

The Full Route Cache stores the rendered HTML and React Server Component payload. It's built from the Data Cache and is invalidated when data changes.

```typescript
// Static page - cached as HTML
export default async function AboutPage() {
  return <h1>About Us</h1>; // Cached as HTML
}
```

Pages are cached until:
- Cache expires (revalidate time)
- Data changes (on-demand revalidation)
- The page uses dynamic functions (cookies, headers)
