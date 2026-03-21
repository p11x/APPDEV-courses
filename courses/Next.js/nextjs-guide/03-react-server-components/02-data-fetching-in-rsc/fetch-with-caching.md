# Fetch with Caching in Next.js

## What You'll Learn
- How fetch caching works in Next.js
- Cache options and when to use them
- Static vs dynamic data

## Prerequisites
- Understanding of async Server Components
- Basic fetch API knowledge

## Concept Explained Simply

When you fetch data in a Server Component, Next.js automatically **caches** the result by default. Caching means saving the fetched data so you don't have to fetch it again on every request. This makes your site incredibly fast!

Think of caching like taking notes. Instead of looking up a phone number every time you need it, you write it down once and refer to your notes. Next.js does this automatically with fetched data — it remembers the results so your pages load faster.

## Cache Options

The fetch API in Next.js accepts these cache options:

| Option | What It Does | When to Use |
|--------|---------------|-------------|
| `force-cache` | Cache forever (default) | Static data |
| `no-store` | Never cache | Real-time data |
| `next: { revalidate: N }` | Cache for N seconds | Data that changes occasionally |

## Complete Code Examples

### Static Data (Cached Forever)

Use for data that rarely changes:

```typescript
// src/app/blog/page.tsx
export default async function BlogPage() {
  const posts = await fetch('https://api.example.com/posts', {
    cache: 'force-cache',  // Default, but explicit is better
  }).then(res => res.json());

  return (
    <main>
      {posts.map(post => (
        <article key={post.id}>
          <h2>{post.title}</h2>
          <p>{post.excerpt}</p>
        </article>
      ))}
    </main>
  );
}
```

### Dynamic Data (Not Cached)

Use for data that must always be fresh:

```typescript
// src/app/stock-prices/page.tsx
export default async function StockPricesPage() {
  const stocks = await fetch('https://api.example.com/stocks', {
    cache: 'no-store',  // Never cache - always fetch fresh
  }).then(res => res.json());

  return (
    <main>
      <h1>Live Stock Prices</h1>
      {stocks.map(stock => (
        <div key={stock.symbol}>
          {stock.symbol}: ${stock.price}
        </div>
      ))}
    </main>
  );
}
```

### Revalidated Data (Time-Based)

Use for data that changes occasionally:

```typescript
// src/app/products/page.tsx
export default async function ProductsPage() {
  const products = await fetch('https://api.example.com/products', {
    next: { revalidate: 3600 },  // Cache for 1 hour (3600 seconds)
  }).then(res => res.json());

  return (
    <main>
      <h1>Products</h1>
      <p>Cached for 1 hour</p>
      {products.map(product => (
        <div key={product.id}>{product.name}</div>
      ))}
    </main>
  );
}
```

### On-Demand Revalidation

Revalidate specific data when needed:

```typescript
// src/app/api/revalidate/route.ts
import { revalidatePath } from "next/cache";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
  const secret = request.headers.get("x-revalidate-secret");
  
  // Verify it's from your CMS or admin
  if (secret !== process.env.REVALIDATION_SECRET) {
    return NextResponse.json({ message: "Invalid secret" }, { status: 401 });
  }
  
  const body = await request.json();
  
  // Revalidate the blog page
  revalidatePath("/blog");
  revalidatePath(`/blog/${body.slug}`);
  
  return NextResponse.json({ revalidated: true });
}
```

## Complete Example with All Types

Let's build a page with different caching strategies:

```typescript
// src/app/page.tsx - Dashboard with mixed caching
import { getUser } from "@/lib/auth";
import { getProducts } from "@/lib/products";
import { getStockPrices } from "@/lib/stocks";

async function getUserData() {
  // User data - fresh every time (personalized)
  const res = await fetch("/api/user", { cache: "no-store" });
  return res.json();
}

async function getFeaturedProducts() {
  // Products - cached for 1 hour (changes occasionally)
  const res = await fetch("/api/products?featured=true", {
    next: { revalidate: 3600 },
  });
  return res.json();
}

async function getStockPrices() {
  // Stock prices - fresh (real-time)
  const res = await fetch("/api/stocks", { cache: "no-store" });
  return res.json();
}

async function getCategories() {
  // Categories - cached forever (rarely change)
  const res = await fetch("/api/categories", { cache: "force-cache" });
  return res.json();
}

export default async function DashboardPage() {
  // Fetch all data in parallel - most efficient!
  const [user, products, stocks, categories] = await Promise.all([
    getUserData(),
    getFeaturedProducts(),
    getStockPrices(),
    getCategories(),
  ]);

  return (
    <main style={{ padding: "2rem" }}>
      <header>
        <h1>Welcome, {user.name}</h1>
      </header>

      <section style={{ marginTop: "2rem" }}>
        <h2>Featured Products (cached 1hr)</h2>
        <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: "1rem" }}>
          {products.map((p: any) => (
            <div key={p.id} style={{ padding: "1rem", border: "1px solid #ddd" }}>
              {p.name} - ${p.price}
            </div>
          ))}
        </div>
      </section>

      <section style={{ marginTop: "2rem" }}>
        <h2>Stock Prices (live)</h2>
        {stocks.map((s: any) => (
          <div key={s.symbol}>{s.symbol}: ${s.price}</div>
        ))}
      </section>

      <section style={{ marginTop: "2rem" }}>
        <h2>Categories (cached forever)</h2>
        <ul>
          {categories.map((c: any) => (
            <li key={c.id}>{c.name}</li>
          ))}
        </ul>
      </section>
    </main>
  );
}
```

## Cache Tags (Advanced)

Next.js also supports cache tags for more control:

```typescript
// Fetch with tag
const data = await fetch('/api/products', {
  next: { tags: ['products'] },
});

// Later, revalidate by tag
import { revalidateTag } from "next/cache";

revalidateTag('products');  // All fetches with 'products' tag will revalidate
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `cache: 'force-cache'` | Cache forever | Fastest - never fetches again |
| `cache: 'no-store'` | No caching | Always fresh - slower |
| `next: { revalidate: 3600 }` | Time-based cache | Balance between fresh and fast |
| `revalidateTag('products')` | On-demand revalidate | Update cache when data changes |

## Common Mistakes

### Mistake #1: Using Wrong Cache Type

```typescript
// ✗ Wrong: Caching real-time data
const stocks = await fetch('/api/stocks', {
  cache: 'force-cache',  // Users see stale stock prices!
});

// ✓ Correct: Don't cache real-time data
const stocks = await fetch('/api/stocks', {
  cache: 'no-store',  // Always fresh
});
```

### Mistake #2: Not Understanding Default Behavior

```typescript
// ✗ Wrong: Assuming fetch is not cached by default
const data = await fetch('/api/data');
// Actually, it's cached by default!

// Be explicit about your intentions
const staticData = await fetch('/api/static', { cache: 'force-cache' });
const dynamicData = await fetch('/api/dynamic', { cache: 'no-store' });
```

### Mistake #3: Wrong Revalidation Time

```typescript
// ✗ Wrong: Too short = too many requests
const data = await fetch('/api/products', {
  next: { revalidate: 1 },  // Every second = no benefit
});

// ✓ Correct: Reasonable time
const data = await fetch('/api/products', {
  next: { revalidate: 3600 },  // 1 hour makes sense for products
});
```

## Summary

- Fetch defaults to `force-cache` (cached forever)
- Use `no-store` for real-time data
- Use `next: { revalidate: N }` for time-based caching
- Use tags for on-demand revalidation
- Choose the right cache strategy for your data type

## Next Steps

Now let's learn about parallel vs sequential fetching:

- [Parallel vs Sequential Fetching →](./parallel-vs-sequential-fetching.md)
