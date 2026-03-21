# URL as State

## What You'll Learn
- Using URL parameters for state management
- Benefits of URL-driven state
- Implementing URL state in Next.js
- Syncing components with URL

## Prerequisites
- Understanding of React state
- Knowledge of Next.js routing
- Familiarity with useSearchParams

## Concept Explained Simply

The URL is actually a fantastic place to store state! When you share a link with someone, they see exactly what you see — the filters you applied, the page you're on, the search you ran. This is called URL-driven state, and it's one of the most underused patterns in React apps.

Think of the URL like a bookmark that remembers not just WHERE you are, but WHAT you're looking at. If you bookmark "products?category=shoes&sort=price", you can send that link to someone and they'll see exactly shoes sorted by price.

## Complete Code Example

### Basic URL State Pattern

```typescript
// components/ProductFilters.tsx
"use client";

import { useRouter, useSearchParams } from "next/navigation";

export default function ProductFilters() {
  const router = useRouter();
  const searchParams = useSearchParams();
  
  // Read current state from URL
  const category = searchParams.get("category") || "all";
  const sort = searchParams.get("sort") || "newest";
  
  // Update URL when filters change
  const updateFilter = (key: string, value: string) => {
    const params = new URLSearchParams(searchParams.toString());
    params.set(key, value);
    
    router.push(`?${params.toString()}`, { scroll: false });
  };
  
  return (
    <div className="filters">
      <select 
        value={category}
        onChange={(e) => updateFilter("category", e.target.value)}
      >
        <option value="all">All Categories</option>
        <option value="electronics">Electronics</option>
        <option value="clothing">Clothing</option>
      </select>
      
      <select
        value={sort}
        onChange={(e) => updateFilter("sort", e.target.value)}
      >
        <option value="newest">Newest</option>
        <option value="price-asc">Price: Low to High</option>
        <option value="price-desc">Price: High to Low</option>
      </select>
    </div>
  );
}
```

```typescript
// app/products/page.tsx
import ProductFilters from "@/components/ProductFilters";

export default async function ProductsPage({
  searchParams,
}: {
  searchParams: Promise<{ category?: string; sort?: string }>;
}) {
  const params = await searchParams;
  const category = params.category || "all";
  const sort = params.sort || "newest";
  
  // Fetch products with filters
  const products = await getProducts({ category, sort });
  
  return (
    <div>
      <h1>Products</h1>
      <ProductFilters />
      <ProductList products={products} />
    </div>
  );
}
```

### Advanced URL State Hook

```typescript
// hooks/useUrlState.ts
"use client";

import { useSearchParams, useRouter, useCallback } from "next/navigation";

export function useUrlState<T extends Record<string, string>>(
  keys: (keyof T)[]
) {
  const router = useRouter();
  const searchParams = useSearchParams();
  
  // Get value for a key
  const getValue = useCallback(
    (key: keyof T): string => {
      return searchParams.get(key as string) || "";
    },
    [searchParams]
  );
  
  // Set value for a key
  const setValue = useCallback(
    (key: keyof T, value: string) => {
      const params = new URLSearchParams(searchParams.toString());
      
      if (value) {
        params.set(key as string, value);
      } else {
        params.delete(key as string);
      }
      
      router.push(`?${params.toString()}`, { scroll: false });
    },
    [router, searchParams]
  );
  
  // Get all values as object
  const getValues = useCallback((): T => {
    const result = {} as T;
    keys.forEach((key) => {
      result[key] = (searchParams.get(key as string) || "") as T[keyof T];
    });
    return result;
  }, [searchParams, keys]);
  
  return { getValue, setValue, getValues };
}
```

```typescript
// components/AdvancedFilters.tsx
"use client";

import { useUrlState } from "@/hooks/useUrlState";

type FilterState = {
  category: string;
  sort: string;
  page: string;
};

export default function AdvancedFilters() {
  const { getValue, setValue } = useUrlState<FilterState>([
    "category",
    "sort",
    "page",
  ]);
  
  const category = getValue("category");
  const sort = getValue("sort");
  
  return (
    <div>
      <select
        value={category}
        onChange={(e) => setValue("category", e.target.value)}
      >
        <option value="">All</option>
        <option value="electronics">Electronics</option>
      </select>
      
      <select
        value={sort}
        onChange={(e) => setValue("sort", e.target.value)}
      >
        <option value="newest">Newest</option>
        <option value="price">Price</option>
      </select>
    </div>
  );
}
```

### URL State for Search

```typescript
// components/SearchBox.tsx
"use client";

import { useRouter, useSearchParams } from "next/navigation";
import { useState, useTransition } from "react";

export default function SearchBox() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const [query, setQuery] = useState(searchParams.get("q") || "");
  const [isPending, startTransition] = useTransition();
  
  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    
    startTransition(() => {
      const params = new URLSearchParams(searchParams.toString());
      
      if (query) {
        params.set("q", query);
      } else {
        params.delete("q");
      }
      
      router.push(`?${params.toString()}`);
    });
  };
  
  return (
    <form onSubmit={handleSearch}>
      <input
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        placeholder="Search..."
      />
      <button type="submit" disabled={isPending}>
        {isPending ? "Searching..." : "Search"}
      </button>
    </form>
  );
}
```

## Benefits of URL State

| Benefit | Explanation |
|---------|-------------|
| **Shareable** | Send URL to anyone, they see same view |
| **Bookable** | Users can bookmark filtered/sorted views |
| **Browser history** | Back button works correctly |
| **SEO** | Search engines can index filtered content |
| **No hydration issues** | Server renders with URL params |

## Common Mistakes

### Mistake 1: Not Using replace for Updates

```typescript
// WRONG - Each filter adds to history
router.push(`?${params.toString()}`);
// Back button becomes annoying!

// CORRECT - Use replace for filters
router.push(`?${params.toString()}`, { scroll: false });
// Or consider: { replace: true } to not add history
```

### Mistake 2: Not Handling Missing Params

```typescript
// WRONG - Params could be null
const category = searchParams.get("category");
// category could be null!

// CORRECT - Provide defaults
const category = searchParams.get("category") || "all";
```

### Mistake 3: Not Using useTransition for Updates

```typescript
// WRONG - Causes blocking navigation
router.push(`?${params.toString()}`);
// UI might freeze briefly!

// CORRECT - Use transition for non-blocking
import { useTransition } from "react";
const [isPending, startTransition] = useTransition();

startTransition(() => {
  router.push(`?${params.toString()}`);
});
```

## Summary

- Store filter/sort state in URL for shareability
- Use `useSearchParams` to read URL state
- Use `router.push()` with `{ scroll: false }` to update
- Consider `useTransition` for smooth UI during navigation
- URL state works with Server Components via searchParams prop
- Browser back/forward buttons work correctly

## Next Steps

- [server-vs-client-state.md](./server-vs-client-state.md) - Understanding state types
- [when-not-to-use-redux.md](./when-not-to-use-redux.md) - Avoiding over-engineering
