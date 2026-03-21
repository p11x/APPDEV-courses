# Passing Server Data to Client Components

## What You'll Learn
- How to pass data from Server to Client Components
- The prop drilling pattern
- Serialization requirements

## Prerequisites
- Understanding of Server and Client Components
- Basic React props knowledge

## Concept Explained Simply

In Next.js, Server Components fetch data and pass it to Client Components via **props**. This is the main way to share data between the server and client.

Think of it like a relay race. Server Components are the first runner — they fetch data and prepare everything. Then they hand off to Client Components (the second runner) who handle interactivity. The handoff happens through props.

## The Data Flow

```
Server Component                    Client Component
┌─────────────────────────┐        ┌─────────────────────────┐
│  fetch("api/data")      │        │  function Component    │
│  await data             │ props  │  ({ data }) {           │
│  return (              │───────►│    return (             │
│    <Component          │        │      <div>{data}</div>  │
│      data={data}        │        │    )                    │
│    />                   │        │  }                      │
│  )                      │        └─────────────────────────┘
└─────────────────────────┘
```

## Complete Code Example

Let's build a product page that fetches on server and passes to client:

```typescript
// src/types/index.ts - Shared types
export interface Product {
  id: string;
  name: string;
  price: number;
  description: string;
  inStock: boolean;
}
```

```typescript
// src/components/ProductCard.tsx - Client Component
"use client";

import { useState } from "react";
import { Product } from "@/types";

interface ProductCardProps {
  product: Product;
}

export function ProductCard({ product }: ProductCardProps) {
  const [quantity, setQuantity] = useState(1);
  const [isAdding, setIsAdding] = useState(false);

  const handleAddToCart = async () => {
    setIsAdding(true);
    // In a real app, this would call a server action
    await new Promise((resolve) => setTimeout(resolve, 1000));
    setIsAdding(false);
    alert(`Added ${quantity} ${product.name} to cart!`);
  };

  return (
    <div
      style={{
        padding: "1.5rem",
        border: "1px solid #ddd",
        borderRadius: "8px",
        backgroundColor: "white",
      }}
    >
      <h3 style={{ marginBottom: "0.5rem" }}>{product.name}</h3>
      <p style={{ color: "#666", marginBottom: "1rem" }}>
        {product.description}
      </p>
      <p style={{ fontSize: "1.25rem", fontWeight: "bold", marginBottom: "1rem" }}>
        ${product.price}
      </p>
      <p style={{ color: product.inStock ? "green" : "red", marginBottom: "1rem" }}>
        {product.inStock ? "In Stock" : "Out of Stock"}
      </p>
      
      {product.inStock && (
        <div style={{ display: "flex", gap: "0.5rem" }}>
          <input
            type="number"
            min="1"
            max="10"
            value={quantity}
            onChange={(e) => setQuantity(Number(e.target.value))}
            style={{ padding: "0.5rem", width: "60px" }}
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
            }}
          >
            {isAdding ? "Adding..." : "Add to Cart"}
          </button>
        </div>
      )}
    </div>
  );
}
```

```typescript
// src/app/products/[id]/page.tsx - Server Component fetches data
import { ProductCard } from "@/components/ProductCard";
import { notFound } from "next/navigation";

interface Props {
  params: Promise<{ id: string }>;
}

interface Product {
  id: string;
  name: string;
  price: number;
  description: string;
  inStock: boolean;
}

// Simulated database
const products: Record<string, Product> = {
  "1": {
    id: "1",
    name: "Laptop Pro",
    price: 1299,
    description: "Powerful laptop for professionals",
    inStock: true,
  },
  "2": {
    id: "2",
    name: "Wireless Mouse",
    price: 49,
    description: "Ergonomic wireless mouse",
    inStock: true,
  },
  "3": {
    id: "3",
    name: "Mechanical Keyboard",
    price: 199,
    description: "RGB mechanical keyboard",
    inStock: false,
  },
};

async function getProduct(id: string): Promise<Product | null> {
  // Simulate network delay
  await new Promise((resolve) => setTimeout(resolve, 500));
  return products[id] || null;
}

export default async function ProductPage({ params }: Props) {
  const { id } = await params;
  const product = await getProduct(id);

  if (!product) {
    notFound();
  }

  // Pass server-fetched data to client component
  return (
    <main style={{ padding: "2rem", maxWidth: "800px", margin: "0 auto" }}>
      <ProductCard product={product} />
    </main>
  );
}
```

## Serialization Requirements

When passing data from Server to Client Components, the data must be **serializable**. This means it must be convertible to a string and back.

### Serializable Types

- Strings, numbers, booleans
- Arrays, Objects (with serializable values)
- null, undefined
- Dates (become strings)
- Maps, Sets (become objects)

### Non-Serializable (Can't Pass!)

- Functions
- Classes
- React Components
- Promises

```typescript
// ✗ Wrong: Can't pass functions to client
export default async function Page() {
  const processData = () => { /* ... */ }; // Function!
  return <ClientComponent process={processData} />; // Error!
}

// ✓ Correct: Pass data only
export default async function Page() {
  const data = await fetchData();
  return <ClientComponent data={data} />;
}
```

## Props vs Direct Fetching

You can also fetch directly in Client Components, but it's usually better to fetch in Server Components:

```typescript
// ✗ Not recommended: Fetch in client
"use client";

export function NotRecommended() {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    fetchData().then(setData);
  }, []);
  
  return <div>{data}</div>;
}

// ✓ Recommended: Fetch in server, pass as props
export default async function Recommended() {
  const data = await fetchData();
  return <ClientComponent data={data} />;
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| Server fetches data | `const data = await getData()` | Data fetched on server |
| Pass as prop | `<Component data={data} />` | Handoff to client |
| Receive as prop | `function Component({ data })` | Client receives data |
| Use in JSX | `{data.name}` | Render server data in client |

## Common Mistakes

### Mistake #1: Passing Functions

```typescript
// ✗ Wrong: Functions can't be serialized
export default async function Page() {
  const handleSubmit = async () => { /* ... */ };
  return <Form onSubmit={handleSubmit} />; // Error!
}

// ✓ Correct: Use server actions instead
// Form calls a server action directly
```

### Mistake #2: Fetching in Client When Not Needed

```typescript
// ✗ Wrong: Unnecessary client-side fetch
"use client";

export function Unnecessary({ id }) {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    fetch(`/api/${id}`).then(res => res.json()).then(setData);
  }, [id]);
}

// ✓ Correct: Fetch in server, pass to client
export default async function Page({ params }) {
  const data = await fetchData(params.id);
  return <ClientComponent data={data} />;
}
```

### Mistake #3: Not Handling Serialization

```typescript
// ✗ Wrong: Dates become strings automatically
const user = { createdAt: new Date() };
// This works but you might get unexpected results

// ✓ Correct: Convert dates to ISO strings explicitly
const user = { createdAt: new Date().toISOString() };
```

## Summary

- Pass data from Server to Client Components via props
- Only pass serializable data (no functions!)
- Fetch in Server Components when possible
- This pattern keeps your app fast and SEO-friendly

## Next Steps

Now let's learn about context in Server Components:

- [Context in Server Components →](./context-in-server-components.md)
