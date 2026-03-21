# What is Partial Prerendering (PPR)

## What You'll Learn
- Understanding PPR
- Static + dynamic in one page

## Concept Explained Simply

**Partial Prerendering (PPR)** combines static and dynamic content in a single page. Static parts are prerendered, dynamic parts stream in.

```typescript
// Experimental feature in Next.js 15
// Enable in next.config.ts:
export default withPPR(nextConfig);
```

```typescript
// The page automatically mixes static and dynamic
export default async function Page() {
  return (
    <div>
      <header>Static Header</header>
      <Suspense fallback={<Loading />}>
        <UserContent />
      </Suspense>
    </div>
  );
}
```
