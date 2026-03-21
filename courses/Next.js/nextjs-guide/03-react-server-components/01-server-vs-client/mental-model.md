# The Mental Model for Server vs Client Components

## What You'll Learn
- What Server Components are
- What Client Components are
- How to think about where code runs

## Prerequisites
- Basic React knowledge
- Understanding of Next.js pages

## Concept Explained Simply

React Server Components (RSC) are a revolutionary feature that changes how you think about building React apps. Here's the key idea: **some components run only on the server, while others run in the browser**.

Think of it like a restaurant kitchen:
- **Server Components** = The chefs in the kitchen (customers never see them)
- **Client Components** = The waitstaff who interact with customers

The chefs (Server Components) prepare the food (render the initial HTML) and send it to the waitstaff (the browser). The waitstaff (Client Components) then handle any interactions like taking orders or refilling drinks.

## The Big Picture

When a user visits a Next.js page:

1. **Server** runs Server Components and generates HTML
2. **HTML** is sent to the browser (fast!)
3. **Browser** shows the page immediately
4. **Client Components** are hydrated (made interactive)

Server Components never send their JavaScript to the browser. They do the heavy work on the server and send only the resulting HTML. This makes your site faster!

## Server Components vs Client Components

| Feature | Server Component | Client Component |
|---------|------------------|------------------|
| Runs on | Server only | Browser |
| JavaScript sent | None | Full JS bundle |
| Can use hooks | ❌ No | ✅ Yes |
| Can use interactivity | ❌ No | ✅ Yes |
| Can fetch data | ✅ Yes | ⚠️ With Suspense |
| Default in App Router | ✅ Yes | ❌ Need "use client" |

## Complete Code Example

Let's see both types in action:

```typescript
// src/app/products/page.tsx - Server Component (default!)
// This runs ONLY on the server

interface Product {
  id: number;
  name: string;
  price: number;
}

// Fetch data directly in the component
async function getProducts(): Promise<Product[]> {
  const res = await fetch('https://api.example.com/products', {
    cache: 'force-cache'
  });
  
  if (!res.ok) {
    throw new Error('Failed to fetch products');
  }
  
  return res.json();
}

export default async function ProductsPage() {
  // This runs on the server - no client-side loading!
  const products = await getProducts();

  return (
    <main style={{ padding: "2rem" }}>
      <h1>Products</h1>
      <p>This page was rendered on the server!</p>
      
      <div style={{ display: "grid", gap: "1rem", marginTop: "2rem" }}>
        {products.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </main>
  );
}

// This is also a Server Component - it receives data from parent
function ProductCard({ product }: { product: Product }) {
  return (
    <div style={{ padding: "1rem", border: "1px solid #ddd", borderRadius: "8px" }}>
      <h3>{product.name}</h3>
      <p>${product.price}</p>
    </div>
  );
}
```

Now let's add interactivity with a Client Component:

```typescript
// src/app/components/AddToCart.tsx - Client Component
"use client";  // ← This directive is required!

import { useState } from "react";

export function AddToCartButton({ productId }: { productId: number }) {
  const [quantity, setQuantity] = useState(1);
  const [isAdding, setIsAdding] = useState(false);

  const handleAddToCart = async () => {
    setIsAdding(true);
    // Simulate adding to cart
    await new Promise(resolve => setTimeout(resolve, 1000));
    setIsAdding(false);
    alert(`Added ${quantity} items to cart!`);
  };

  return (
    <div style={{ marginTop: "1rem" }}>
      <input
        type="number"
        min="1"
        value={quantity}
        onChange={(e) => setQuantity(Number(e.target.value))}
        style={{ padding: "0.5rem", marginRight: "0.5rem", width: "60px" }}
      />
      <button
        onClick={handleAddToCart}
        disabled={isAdding}
        style={{
          padding: "0.5rem 1rem",
          backgroundColor: "#0070f3",
          color: "white",
          border: "none",
          borderRadius: "4px",
          cursor: isAdding ? "not-allowed" : "pointer",
          opacity: isAdding ? 0.7 : 1,
        }}
      >
        {isAdding ? "Adding..." : "Add to Cart"}
      </button>
    </div>
  );
}
```

```typescript
// src/app/products/page.tsx - Updated with interactivity
import { AddToCartButton } from "@/components/AddToCart";

async function getProducts() {
  const res = await fetch('https://api.example.com/products', {
    cache: 'force-cache'
  });
  if (!res.ok) throw new Error('Failed to fetch');
  return res.json();
}

export default async function ProductsPage() {
  const products = await getProducts();

  return (
    <main style={{ padding: "2rem" }}>
      <h1>Products</h1>
      
      <div style={{ display: "grid", gap: "1rem" }}>
        {products.map((product) => (
          <div
            key={product.id}
            style={{ padding: "1rem", border: "1px solid #ddd", borderRadius: "8px" }}
          >
            <h3>{product.name}</h3>
            <p>${product.price}</p>
            {/* Client Component for interactivity */}
            <AddToCartButton productId={product.id} />
          </div>
        ))}
      </div>
    </main>
  );
}
```

## How to Decide

**Use Server Components when:**
- Fetching data
- Accessing backend resources directly
- Keeping sensitive info on the server (API keys, etc.)
- Large dependencies that shouldn't be sent to client
- No interactivity needed (just display)

**Use Client Components when:**
- Using React hooks (useState, useEffect, useContext)
- Adding event listeners (onClick, onChange)
- Using browser-only APIs
- Need real-time interactivity

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `async function ProductsPage()` | Async Server Component | Can fetch data directly |
| `await getProducts()` | Server-side data fetch | No client-side loading |
| `"use client"` | Client Component directive | Required for interactivity |
| `useState` | React hook | Only works in client |

## Common Mistakes

### Mistake #1: Using Server Components for Interactivity

```typescript
// ✗ Wrong: Server Component can't handle clicks
export default async function Page() {
  return (
    <button onClick={() => alert("clicked!")}>  // Error!
      Click me
    </button>
  );
}

// ✓ Correct: Use "use client"
"use client";

export default function Page() {
  return (
    <button onClick={() => alert("clicked!")}>
      Click me
    </button>
  );
}
```

### Mistake #2: Forgetting "use client"

```typescript
// ✗ Wrong: Using hooks without "use client"
import { useState } from "react";

export default function Counter() {
  const [count, setCount] = useState(0); // Error!
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}

// ✓ Correct: Add "use client"
"use client";

import { useState } from "react";

export default function Counter() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}
```

### Mistake #3: Putting "use client" on Everything

```typescript
// ✗ Wrong: Unnecessary "use client"
"use client";

export default function ProductList({ products }) {
  // Just displaying data, no interactivity needed
  return (
    <ul>
      {products.map(p => <li key={p.id}>{p.name}</li>)}
    </ul>
  );
}

// ✓ Correct: Server Component is fine for display
export default function ProductList({ products }) {
  return (
    <ul>
      {products.map(p => <li key={p.id}>{p.name}</li>)}
    </ul>
  );
}
```

## Summary

- Server Components run only on the server (default in App Router)
- Client Components run in the browser (need "use client")
- Use Server Components for data fetching and display
- Use Client Components for interactivity
- Mix both in the same page for best performance

## Next Steps

Now let's learn when to use each type:

- [When to Use Each →](./when-to-use-each.md)
