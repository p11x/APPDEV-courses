# Loading UI in Next.js

## What You'll Learn
- How to create loading states with loading.tsx
- How loading UI improves user experience
- Creating skeleton loaders

## Prerequisites
- Understanding of pages and layouts
- Basic React knowledge

## Concept Explained Simply

When your page fetches data, there might be a delay before the content appears. Instead of showing a blank screen or a spinning wheel, you can show a **loading UI** that gives users feedback that something is happening.

In Next.js, you create a `loading.tsx` file, and Next.js automatically shows it while the page is loading. This is called **Suspense streaming** — it allows different parts of your page to load at different times.

Think of it like a restaurant: when you order food, they bring you bread and water while you wait. Loading UI is that bread — it keeps users satisfied while the main content (the food) is being prepared.

## How It Works

1. Create `loading.tsx` in the same folder as your `page.tsx`
2. Next.js automatically wraps your page in a Suspense boundary
3. While the page fetches data, the loading UI shows
4. When data is ready, the loading UI swaps to the actual page

## Complete Code Example

Let's create a product listing page with a loading state:

```typescript
// src/app/products/page.tsx - The actual page
async function getProducts() {
  // Simulate slow API call
  await new Promise((resolve) => setTimeout(resolve, 2000));
  
  return [
    { id: 1, name: "Laptop Pro", price: 1299 },
    { id: 2, name: "Wireless Mouse", price: 49 },
    { id: 3, name: "Mechanical Keyboard", price: 199 },
    { id: 4, name: "Monitor 4K", price: 499 },
  ];
}

export default async function ProductsPage() {
  const products = await getProducts();

  return (
    <main style={{ padding: "2rem", maxWidth: "800px", margin: "0 auto" }}>
      <h1>Products</h1>
      <div style={{ display: "grid", gap: "1rem", marginTop: "2rem" }}>
        {products.map((product) => (
          <div
            key={product.id}
            style={{
              padding: "1.5rem",
              border: "1px solid #ddd",
              borderRadius: "8px",
            }}
          >
            <h3 style={{ margin: "0 0 0.5rem" }}>{product.name}</h3>
            <p style={{ margin: 0, color: "#0070f3", fontWeight: "bold" }}>
              ${product.price}
            </p>
          </div>
        ))}
      </div>
    </main>
  );
}
```

```typescript
// src/app/products/loading.tsx - The loading UI
export default function LoadingProducts() {
  return (
    <main style={{ padding: "2rem", maxWidth: "800px", margin: "0 auto" }}>
      <h1>Products</h1>
      <div style={{ display: "grid", gap: "1rem", marginTop: "2rem" }}>
        {/* Skeleton loaders - fake placeholders */}
        {[1, 2, 3, 4].map((i) => (
          <div
            key={i}
            style={{
              padding: "1.5rem",
              border: "1px solid #ddd",
              borderRadius: "8px",
              backgroundColor: "#f5f5f5",
            }}
          >
            <div
              style={{
                width: "60%",
                height: "24px",
                backgroundColor: "#ddd",
                borderRadius: "4px",
                marginBottom: "0.5rem",
                animation: "pulse 1.5s infinite",
              }}
            />
            <div
              style={{
                width: "30%",
                height: "20px",
                backgroundColor: "#ddd",
                borderRadius: "4px",
              }}
            />
          </div>
        ))}
      </div>
      <style>{`
        @keyframes pulse {
          0%, 100% { opacity: 1; }
          50% { opacity: 0.5; }
        }
      `}</style>
    </main>
  );
}
```

## How the Loading Works

```
User visits /products
        ↓
loading.tsx shows immediately
        ↓
page.tsx fetches data (2 second delay)
        ↓
loading.tsx is replaced by page.tsx
```

The user sees the skeleton immediately, then the real content appears after 2 seconds.

## Skeleton Loaders

Skeleton loaders are fake placeholders that look like the real content. They're better than spinners because:

1. They show the expected layout
2. They reduce perceived wait time
3. They look more professional

Here's a card skeleton:

```typescript
// src/app/components/SkeletonCard.tsx
export function SkeletonCard() {
  return (
    <div
      style={{
        padding: "1.5rem",
        border: "1px solid #ddd",
        borderRadius: "8px",
      }}
    >
      {/* Image placeholder */}
      <div
        style={{
          width: "100%",
          height: "150px",
          backgroundColor: "#ddd",
          borderRadius: "4px",
          marginBottom: "1rem",
        }}
      />
      {/* Title placeholder */}
      <div
        style={{
          width: "70%",
          height: "20px",
          backgroundColor: "#ddd",
          borderRadius: "4px",
          marginBottom: "0.5rem",
        }}
      />
      {/* Description placeholder */}
      <div
        style={{
          width: "90%",
          height: "16px",
          backgroundColor: "#ddd",
          borderRadius: "4px",
        }}
      />
    </div>
  );
}
```

## Nested Loading

Loading files also work with nested routes:

```
src/app/
├── products/
│   ├── loading.tsx      ← Shows when /products loads
│   ├── page.tsx
│   └── [id]/
│       ├── loading.tsx  ← Shows when /products/:id loads
│       └── page.tsx
```

When visiting `/products/1`:
1. First shows `/products/loading.tsx`
2. Then shows `/products/[id]/loading.tsx` 
3. Finally shows `/products/[id]/page.tsx`

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `loading.tsx` | Special Next.js file | Auto-wrapped in Suspense |
| Skeleton divs | Fake content | Shows expected layout |
| `@keyframes pulse` | Animation | Makes skeletons feel alive |
| `await new Promise` | Simulated delay | Shows loading UI in action |

## Common Mistakes

### Mistake #1: Not Creating loading.tsx

If your page takes time to load and you don't have a loading file, users see nothing:

```typescript
// ✗ Wrong: No loading file, blank screen while loading

// ✓ Correct: Create loading.tsx
export default function Loading() {
  return <p>Loading...</p>;
}
```

### Mistake #2: Making Loading Too Complex

```typescript
// ✗ Wrong: Loading UI is as complex as the page itself
export default function Loading() {
  // This is too much work for a temporary loading state
  return <FullPageReplicate />;
}

// ✓ Correct: Simple skeleton
export default function Loading() {
  return <SimpleSkeleton />;
}
```

### Mistake #3: Confusing loading.tsx with page.tsx

```typescript
// ✗ Wrong: These are different files!
src/app/products/page.tsx     // The actual page
src/app/products/loading.tsx  // The loading state
```

## Summary

- Create `loading.tsx` in any route folder for loading states
- Next.js automatically wraps pages in Suspense
- Show skeleton loaders while content loads
- Loading UI improves perceived performance
- Works with nested routes

## Next Steps

Now let's learn about error handling:

- [Error Boundaries →](./error-boundaries.md)
