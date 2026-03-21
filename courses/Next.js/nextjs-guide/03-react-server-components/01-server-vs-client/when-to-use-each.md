# When to Use Server vs Client Components

## What You'll Learn
- Clear guidelines for choosing component types
- Practical decision-making framework
- Real-world examples

## Prerequisites
- Understanding of Server and Client Components
- Basic React knowledge

## Concept Explained Simply

Choosing between Server and Client Components is one of the most important decisions in Next.js. Here's a simple rule: **default to Server Components and only use Client Components when you need interactivity**.

This is opposite to how traditional React apps worked, where everything was client-side. In Next.js with the App Router, Server Components are the default and preferred choice because they're faster and more secure.

## The Decision Framework

Ask yourself these questions in order:

1. **Does this need interactivity?** (clicks, forms, animations)
   - Yes → Client Component
   - No → Server Component

2. **Does this use React hooks?** (useState, useEffect, useContext)
   - Yes → Client Component
   - No → Server Component

3. **Does this use browser-only APIs?** (window, document, localStorage)
   - Yes → Client Component
   - No → Server Component

4. **Is this just displaying data?** (lists, text, images)
   - Yes → Server Component (preferred)
   - Maybe → Server Component

## Quick Reference Table

| Use Case | Component Type |
|----------|----------------|
| Fetching data | Server |
| Displaying text/images | Server |
| Lists and grids | Server |
| Click handlers | Client |
| Form inputs | Client |
| useState/useEffect | Client |
| Browser APIs | Client |
| Real-time updates | Client |

## Complete Code Example

Let's build a product detail page with both types:

```typescript
// src/app/products/[id]/page.tsx - Server Component
// Handles data fetching and display
import { AddToCartButton } from "@/components/AddToCart";
import { ProductReviews } from "@/components/ProductReviews";
import { StarRating } from "@/components/StarRating";

interface Props {
  params: Promise<{ id: string }>;
}

interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  image: string;
  rating: number;
}

async function getProduct(id: string): Promise<Product | null> {
  const res = await fetch(`https://api.example.com/products/${id}`, {
    cache: 'force-cache',
  });
  
  if (!res.ok) return null;
  return res.json();
}

export default async function ProductPage({ params }: Props) {
  const { id } = await params;
  const product = await getProduct(id);

  if (!product) {
    return notFound();
  }

  return (
    <main style={{ maxWidth: "1200px", margin: "0 auto", padding: "2rem" }}>
      {/* Product info - Server Component */}
      <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "2rem" }}>
        <div>
          <img
            src={product.image}
            alt={product.name}
            style={{ width: "100%", borderRadius: "8px" }}
          />
        </div>
        <div>
          <h1 style={{ fontSize: "2.5rem", marginBottom: "0.5rem" }}>
            {product.name}
          </h1>
          <div style={{ marginBottom: "1rem" }}>
            <StarRating rating={product.rating} />
          </div>
          <p style={{ fontSize: "2rem", color: "#0070f3", fontWeight: "bold" }}>
            ${product.price}
          </p>
          <p style={{ margin: "1.5rem 0", lineHeight: "1.8" }}>
            {product.description}
          </p>
          
          {/* Client Component - needs interactivity */}
          <AddToCartButton productId={product.id} />
        </div>
      </div>

      {/* Reviews section - Client Component */}
      <section style={{ marginTop: "3rem" }}>
        <h2>Customer Reviews</h2>
        <ProductReviews productId={product.id} />
      </section>
    </main>
  );
}
```

```typescript
// src/components/StarRating.tsx - Client Component
"use client";

import { useState } from "react";

export function StarRating({ rating }: { rating: number }) {
  const [hoverRating, setHoverRating] = useState(0);

  return (
    <div style={{ display: "flex", gap: "0.25rem" }}>
      {[1, 2, 3, 4, 5].map((star) => (
        <span
          key={star}
          onMouseEnter={() => setHoverRating(star)}
          onMouseLeave={() => setHoverRating(0)}
          style={{
            fontSize: "1.5rem",
            cursor: "default",
            color: star <= (hoverRating || rating) ? "#ffc107" : "#ddd",
          }}
        >
          ★
        </span>
      ))}
    </div>
  );
}
```

```typescript
// src/components/AddToCartButton.tsx - Client Component
"use client";

import { useState } from "react";

export function AddToCartButton({ productId }: { productId: string }) {
  const [quantity, setQuantity] = useState(1);
  const [isLoading, setIsLoading] = useState(false);

  const handleAddToCart = async () => {
    setIsLoading(true);
    // This would call a server action
    await addToCart(productId, quantity);
    setIsLoading(false);
    alert("Added to cart!");
  };

  return (
    <div style={{ display: "flex", gap: "1rem", alignItems: "center" }}>
      <label>
        Quantity:
        <select
          value={quantity}
          onChange={(e) => setQuantity(Number(e.target.value))}
          style={{ marginLeft: "0.5rem", padding: "0.5rem" }}
        >
          {[1, 2, 3, 4, 5].map((n) => (
            <option key={n} value={n}>{n}</option>
          ))}
        </select>
      </label>
      <button
        onClick={handleAddToCart}
        disabled={isLoading}
        style={{
          padding: "0.75rem 1.5rem",
          backgroundColor: "#0070f3",
          color: "white",
          border: "none",
          borderRadius: "8px",
          cursor: isLoading ? "not-allowed" : "pointer",
        }}
      >
        {isLoading ? "Adding..." : "Add to Cart"}
      </button>
    </div>
  );
}

async function addToCart(productId: string, quantity: number) {
  // Server action call
  "use server";
  // Cart logic here
}
```

```typescript
// src/components/ProductReviews.tsx - Client Component
"use client";

import { useState, useEffect } from "react";

interface Review {
  id: string;
  user: string;
  comment: string;
  rating: number;
}

export function ProductReviews({ productId }: { productId: string }) {
  const [reviews, setReviews] = useState<Review[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch(`/api/reviews?productId=${productId}`)
      .then((res) => res.json())
      .then((data) => {
        setReviews(data);
        setLoading(false);
      });
  }, [productId]);

  if (loading) return <p>Loading reviews...</p>;

  return (
    <div>
      {reviews.length === 0 ? (
        <p>No reviews yet.</p>
      ) : (
        reviews.map((review) => (
          <div
            key={review.id}
            style={{
              padding: "1rem",
              border: "1px solid #ddd",
              borderRadius: "8px",
              marginBottom: "1rem",
            }}
          >
            <strong>{review.user}</strong>
            <p style={{ margin: "0.5rem 0" }}>{review.comment}</p>
            <span>{"★".repeat(review.rating)}</span>
          </div>
        ))
      )}
    </div>
  );
}
```

## Component Architecture

```
ProductPage (Server)
├── StarRating (Client) ← needs hover interaction
├── AddToCartButton (Client) ← needs click + state
└── ProductReviews (Client) ← needs useEffect
```

## Common Mistakes

### Mistake #1: Making Everything Client Components

```typescript
// ✗ Wrong: Entire page is client
"use client";

export default function Page() {
  const products = fetchProducts(); // Can't do this!
  return <div>{products.map(p => <ProductCard product={p} />)}</div>;
}

// ✓ Correct: Most is server, only interactive parts are client
export default async function Page() {
  const products = await fetchProducts();
  return <div>{products.map(p => <ProductCard product={p} />)}</div>;
}
```

### Mistake #2: Not Passing Data Correctly

```typescript
// ✗ Wrong: Trying to share state between server and client
// Server can't access client state!

// ✓ Correct: Pass data down from server to client
export default async function Page() {
  const data = await fetchData(); // Server
  return <ClientComponent data={data} />; // Pass as props
}
```

### Mistake #3: Fetching in Client Components

```typescript
// ✗ Wrong: Fetching in useEffect
"use client";

export function Component() {
  useEffect(() => {
    fetchData(); // Runs twice in development!
  }, []);
}

// ✓ Correct: Fetch in Server Component, pass to client
export default async function Page() {
  const data = await fetchData();
  return <ClientComponent data={data} />;
}
```

## Summary

- Default to Server Components (simpler, faster, more secure)
- Use Client Components only for interactivity
- Put "use client" at the top of client files
- Pass data from server to client via props
- Keep client components as leaf nodes in your tree

## Next Steps

Let's learn about the "use client" directive in detail:

- [The use client Directive →](./the-use-client-directive.md)
