# Zustand with Server Components

## What You'll Learn
- Combining Zustand with server data
- Passing server state to client stores
- Best practices for hybrid apps
- Avoiding common pitfalls

## Prerequisites
- Understanding of Zustand
- Knowledge of Server Components
- Familiarity with data fetching

## Concept Explained Simply

In Next.js, you often have a mix: server data that comes from your database, and client state that's specific to user interactions. The trick is combining these correctly. You fetch data on the server (where it's fast and secure), then pass it to client components that need interactivity.

Think of it like a restaurant: the kitchen (server) prepares the food (fetches data), then the waiter (client component) serves it and handles your requests (interactivity).

## Complete Code Example

### Passing Initial Data to Zustand

```typescript
// store/useProductStore.ts
import { create } from "zustand";

interface Product {
  id: string;
  name: string;
  price: number;
}

interface ProductState {
  products: Product[];
  filter: string;
  setFilter: (filter: string) => void;
  getFilteredProducts: () => Product[];
}

export const useProductStore = create<ProductState>((set, get) => ({
  products: [], // Will be initialized from server
  filter: "all",
  
  setFilter: (filter) => set({ filter }),
  
  getFilteredProducts: () => {
    const { products, filter } = get();
    if (filter === "all") return products;
    return products.filter((p) => p.category === filter);
  },
}));
```

```typescript
// components/ProductFilter.tsx - Client Component
"use client";

import { useProductStore } from "@/store/useProductStore";

export default function ProductFilter() {
  const filter = useProductStore((state) => state.filter);
  const setFilter = useProductStore((state) => state.setFilter);
  
  return (
    <select value={filter} onChange={(e) => setFilter(e.target.value)}>
      <option value="all">All Products</option>
      <option value="electronics">Electronics</option>
      <option value="clothing">Clothing</option>
    </select>
  );
}
```

```typescript
// app/products/page.tsx - Server Component
import { db } from "@/lib/db";
import { useProductStore } from "@/store/useProductStore";
import ProductFilter from "@/components/ProductFilter";
import ProductList from "@/components/ProductList";

// Initialize store with server data
function initializeStore(products: any[]) {
  const store = useProductStore.getState();
  useProductStore.setState({ products });
}

export default async function ProductsPage() {
  // Fetch on server
  const products = await db.product.findMany();
  
  // Initialize store on server (runs on client)
  return (
    <div>
      <h1>Products</h1>
      <InitializeProducts products={products} />
      <ProductFilter />
      <ProductList />
    </div>
  );
}

// Separate component to initialize
function InitializeProducts({ products }: { products: any[] }) {
  const setProducts = useProductStore((state) => state.setProducts as any);
  
  // Initialize once on mount
  useEffect(() => {
    setProducts(products);
  }, [products]);
  
  return null;
}
```

### Better: Direct Props with Store for Interactive Parts

```typescript
// Better pattern: Pass data as props, use store only for interactivity

// app/products/page.tsx
export default async function ProductsPage() {
  const products = await db.product.findMany();
  
  return (
    <ProductList initialProducts={products} />
  );
}
```

```typescript
// components/ProductList.tsx
"use client";

import { useState } from "react";

export default function ProductList({ initialProducts }) {
  const [filter, setFilter] = useState("all");
  
  const filteredProducts = filter === "all" 
    ? initialProducts 
    : initialProducts.filter(p => p.category === filter);
  
  return (
    <div>
      <select value={filter} onChange={(e) => setFilter(e.target.value)}>
        <option value="all">All</option>
        <option value="electronics">Electronics</option>
      </select>
      
      <div>
        {filteredProducts.map(product => (
          <div key={product.id}>{product.name}</div>
        ))}
      </div>
    </div>
  );
}
```

### Using Zustand for Global Client State Only

```typescript
// For truly global client state that persists

// store/useAuthStore.ts
import { create } from "zustand";

interface AuthState {
  user: any | null;
  setUser: (user: any) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  setUser: (user) => set({ user }),
  logout: () => set({ user: null }),
}));
```

```typescript
// app/layout.tsx - Server Component
import { getUser } from "@/lib/auth";
import AuthProvider from "@/components/AuthProvider";

export default async function RootLayout({ children }) {
  const user = await getUser();
  
  return (
    <html>
      <body>
        <AuthProvider initialUser={user}>
          {children}
        </AuthProvider>
      </body>
    </html>
  );
}
```

```typescript
// components/AuthProvider.tsx
"use client";

import { useEffect, useState } from "react";
import { useAuthStore } from "@/store/useAuthStore";

export default function AuthProvider({ children, initialUser }) {
  const setUser = useAuthStore((state) => state.setUser);
  const [mounted, setMounted] = useState(false);
  
  useEffect(() => {
    setUser(initialUser);
    setMounted(true);
  }, [initialUser]);
  
  if (!mounted) {
    return null; // Or loading state
  }
  
  return <>{children}</>;
}
```

## Common Mistakes

### Mistake 1: Using Store Instead of Props

```typescript
// WRONG - Trying to avoid props
// Server fetches, puts in store
// Client reads from store

// PROBLEM: Store is empty on initial render!
// SSR and store don't mix well

// CORRECT - Pass data as props, use store for local state
export default async function Page() {
  const data = await fetchData();
  return <ClientComponent data={data} />;
}
```

### Mistake 2: Not Handling SSR

```typescript
// WRONG - Store used in Server Component
// app/page.tsx
import { useCartStore } from "@/store";

export default function Page() {
  const items = useCartStore(state => state.items);
  // This causes issues!
}

// CORRECT - Client components only
// app/page.tsx
export default function Page() {
  return <Cart />; // Cart is "use client"
}
```

### Mistake 3: Initial State Mismatch

```typescript
// WRONG - Hydration error
export default function Component() {
  const theme = useThemeStore(state => state.theme);
  // Server: undefined, Client: "dark" - mismatch!
  
  return <div>{theme}</div>;
}

// CORRECT - Handle mounting
export default function Component() {
  const theme = useThemeStore(state => state.theme);
  const [mounted, setMounted] = useState(false);
  
  useEffect(() => setMounted(true), []);
  
  if (!mounted) return null;
  
  return <div>{theme}</div>;
}
```

## Summary

- Pass server data as props to client components
- Use Zustand only for truly global client state
- Don't try to share state between server and client directly
- Handle hydration properly
- Server Components handle data, Client Components handle interactivity

## Next Steps

- [persisting-state.md](./persisting-state.md) - Persisting state to localStorage
- [atoms-and-derived-state.md](../03-jotai/atoms-and-derived-state.md) - Alternative state management
