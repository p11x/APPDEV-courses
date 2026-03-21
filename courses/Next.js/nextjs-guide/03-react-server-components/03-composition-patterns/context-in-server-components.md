# Context in Server Components

## What You'll Learn
- How React Context works with Server Components
- What works and what doesn't
- Workarounds for server-side context

## Prerequisites
- Understanding of React Context
- Knowledge of Server and Client Components

## Concept Explained Simply

React Context is a way to share data across components without passing props manually. However, Context has limitations in Server Components that are important to understand.

The key rule: **Server Components cannot consume React Context**. Only Client Components can use `useContext()`. This is because Context is designed for client-side state management, and Server Components run on the server where there's no "context" to share.

## What Works and What Doesn't

| Approach | Server Component | Client Component |
|----------|------------------|------------------|
| `useContext()` | ❌ Doesn't work | ✅ Works |
| Passing data via props | ✅ Works | ✅ Works |
| Creating context in layout | ⚠️ Limited | ✅ Works |

## Complete Code Example

Here's how to properly handle context-like patterns:

```typescript
// ❌ WRONG: Can't use context in Server Components
// src/components/ThemeContext.tsx - Don't do this in server!
"use client";

import { createContext, useContext, useState } from "react";

const ThemeContext = createContext<{ theme: string; toggleTheme: () => void } | null>(null);

export function ThemeProvider({ children }: { children: React.ReactNode }) {
  const [theme, setTheme] = useState("light");
  
  const toggleTheme = () => {
    setTheme(t => t === "light" ? "dark" : "light");
  };

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

export function useTheme() {
  const context = useContext(ThemeContext);
  if (!context) throw new Error("useTheme must be used within ThemeProvider");
  return context;
}
```

```typescript
// ✅ CORRECT: Server Components pass data to Client Components
// For theme, use cookies or a different approach

// src/components/ThemeToggle.tsx - Client Component
"use client";

import { useState, useEffect } from "react";

export function ThemeToggle() {
  const [theme, setTheme] = useState("light");
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
    // Check localStorage or cookie
    const stored = localStorage.getItem("theme") || "light";
    setTheme(stored);
  }, []);

  const toggleTheme = () => {
    const newTheme = theme === "light" ? "dark" : "light";
    setTheme(newTheme);
    localStorage.setItem("theme", newTheme);
    document.documentElement.classList.toggle("dark");
  };

  if (!mounted) return null;

  return (
    <button
      onClick={toggleTheme}
      style={{
        padding: "0.5rem 1rem",
        backgroundColor: theme === "light" ? "#eee" : "#333",
        color: theme === "light" ? "#333" : "#fff",
        border: "none",
        borderRadius: "4px",
        cursor: "pointer",
      }}
    >
      {theme === "light" ? "🌙 Dark" : "☀️ Light"}
    </button>
  );
}
```

```typescript
// src/app/layout.tsx - Use client components for context providers
import { ThemeToggle } from "@/components/ThemeToggle";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html>
      <body>
        <header style={{ padding: "1rem", borderBottom: "1px solid #ddd" }}>
          <nav style={{ display: "flex", justifyContent: "space-between" }}>
            <span>My App</span>
            <ThemeToggle />
          </nav>
        </header>
        {children}
      </body>
    </html>
  );
}
```

## Alternative: Pass Data Through Props

Since Server Components can't use context, pass data through props instead:

```typescript
// ✗ Wrong: Trying to use server context
// src/app/layout.tsx
export default function Layout({ children }) {
  const user = getUser(); // Can't access!
  return <div>{children}</div>;
}

// ✓ Correct: Fetch in Server Component, pass as props
// src/app/dashboard/layout.tsx
import { getUser } from "@/lib/auth";

export default async function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const user = await getUser(); // Fetch on server

  return (
    <div>
      <header>Welcome, {user.name}</header>
      {children}
    </div>
  );
}
```

## Using Client Providers for Client-Side Context

Some context is inherently client-side. Wrap your app with providers:

```typescript
// src/components/Providers.tsx
"use client";

import { createContext, useContext, useState } from "react";

// Example: Cart context (real-time, client-side)
const CartContext = createContext<{
  items: Array<{ id: string; quantity: number }>;
  addItem: (id: string) => void;
} | null>(null);

export function CartProvider({ children }: { children: React.ReactNode }) {
  const [items, setItems] = useState<Array<{ id: string; quantity: number }>>([]);

  const addItem = (id: string) => {
    setItems((prev) => {
      const existing = prev.find((item) => item.id === id);
      if (existing) {
        return prev.map((item) =>
          item.id === id ? { ...item, quantity: item.quantity + 1 } : item
        );
      }
      return [...prev, { id, quantity: 1 }];
    });
  };

  return (
    <CartContext.Provider value={{ items, addItem }}>
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  const context = useContext(CartContext);
  if (!context) throw new Error("useCart must be used within CartProvider");
  return context;
}
```

```typescript
// src/app/layout.tsx - Wrap with provider
import { CartProvider } from "@/components/Providers";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html>
      <body>
        <CartProvider>
          {children}
        </CartProvider>
      </body>
    </html>
  );
}
```

## Mixing Server and Client Context

Here's a realistic pattern combining both:

```typescript
// src/app/products/[id]/page.tsx
// Server fetches product data
// Client component handles cart (real-time)

import { ProductForm } from "@/components/ProductForm";

interface Props {
  params: Promise<{ id: string }>;
}

async function getProduct(id: string) {
  const res = await fetch(`https://api.example.com/products/${id}`);
  return res.json();
}

export default async function ProductPage({ params }: Props) {
  const { id } = await params;
  const product = await getProduct(id);

  return (
    <main>
      <h1>{product.name}</h1>
      <p>${product.price}</p>
      {/* Server data passed to client component */}
      <ProductForm productId={product.id} price={product.price} />
    </main>
  );
}
```

```typescript
// src/components/ProductForm.tsx - Client Component with cart context
"use client";

import { useCart } from "@/components/Providers";

export function ProductForm({ productId, price }: { productId: string; price: number }) {
  const { addItem } = useCart();

  return (
    <button onClick={() => addItem(productId)}>
      Add to Cart - ${price}
    </button>
  );
}
```

## Summary

- Server Components cannot use React Context (`useContext`)
- Client Components can use context normally
- Pass data through props from Server to Client Components
- For client-side state (like cart), use Client Component providers
- For server-side data, fetch in Server Components and pass as props

## Next Steps

Now let's learn about using third-party components:

- [Third-party Components →](./third-party-components.md)
