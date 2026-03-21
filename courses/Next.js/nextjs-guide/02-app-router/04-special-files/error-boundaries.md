# Error Boundaries in Next.js

## What You'll Learn
- How to handle errors gracefully
- Creating error.tsx files
- Using the error boundary component

## Prerequisites
- Understanding of pages and layouts
- Basic React error handling concepts

## Concept Explained Simply

Sometimes things go wrong. A database might be down, an API might fail, or your code might have a bug. Without error handling, your entire app would crash and users would see a confusing error message.

**Error boundaries** catch errors that happen in your pages and show a friendly UI instead of crashing. In Next.js, you create an `error.tsx` file, and it automatically wraps your page in an error-catching component.

Think of error boundaries like safety nets in circuses. If a performer falls, they land on the net instead of hitting the ground. Error boundaries work the same way — they catch errors and prevent them from breaking your entire app.

## How It Works

1. Create `error.tsx` in a route folder
2. When an error occurs in that route, Next.js shows the error UI
3. Users can try again or go back to a safe page
4. The error boundary receives the error details

## Complete Code Example

Let's create a product page with error handling:

```typescript
// src/app/products/page.tsx - Page that might fail
async function getProducts() {
  // Simulate a potential failure
  const shouldFail = Math.random() > 0.5;
  
  if (shouldFail) {
    throw new Error("Failed to fetch products");
  }
  
  return [
    { id: 1, name: "Laptop Pro", price: 1299 },
    { id: 2, name: "Wireless Mouse", price: 49 },
  ];
}

export default async function ProductsPage() {
  const products = await getProducts();

  return (
    <main style={{ padding: "2rem" }}>
      <h1>Products</h1>
      <ul>
        {products.map((p) => (
          <li key={p.id}>{p.name} - ${p.price}</li>
        ))}
      </ul>
    </main>
  );
}
```

```typescript
// src/app/products/error.tsx - Error boundary
"use client";

import { useEffect } from "react";

interface ErrorProps {
  error: Error & { digest?: string };
  reset: () => void;
}

export default function ProductsError({ error, reset }: ErrorProps) {
  useEffect(() => {
    // Log the error to an error reporting service
    console.error("Products page error:", error);
  }, [error]);

  return (
    <main style={{ padding: "2rem", textAlign: "center" }}>
      <div
        style={{
          maxWidth: "500px",
          margin: "0 auto",
          padding: "2rem",
          border: "1px solid #ffccc7",
          backgroundColor: "#fff2f0",
          borderRadius: "8px",
        }}
      >
        <h2 style={{ color: "#cf1322", marginBottom: "1rem" }}>
          Something went wrong! 😔
        </h2>
        <p style={{ marginBottom: "1.5rem", color: "#666" }}>
          We couldn't load the products. Please try again.
        </p>
        {error.message && (
          <p
            style={{
              padding: "0.5rem",
              backgroundColor: "#fff",
              borderRadius: "4px",
              fontSize: "0.9rem",
              color: "#888",
            }}
          >
            Error: {error.message}
          </p>
        )}
        <button
          onClick={() => reset()}
          style={{
            marginTop: "1.5rem",
            padding: "0.75rem 1.5rem",
            backgroundColor: "#0070f3",
            color: "white",
            border: "none",
            borderRadius: "8px",
            cursor: "pointer",
            fontSize: "1rem",
          }}
        >
          Try Again
        </button>
      </div>
    </main>
  );
}
```

## Key Parts of Error Boundaries

| Prop | Description |
|------|-------------|
| `error` | The actual error that occurred |
| `error.message` | Human-readable error message |
| `error.digest` | Unique error ID for logging |
| `reset` | Function to retry the operation |

## The "use client" Directive

Error boundaries must be **Client Components** because they need to handle state and user interactions (like the "Try Again" button):

```typescript
"use client";  // ← Required at the top of error.tsx

export default function ErrorBoundary({ error, reset }: ErrorProps) {
  // Error boundaries are always client components
}
```

## Nested Error Boundaries

Error boundaries work with nested routes too:

```
src/app/
├── error.tsx              ← Catches errors in all routes
├── products/
│   ├── error.tsx          ← Catches errors in /products/*
│   ├── page.tsx
│   └── [id]/
│       ├── error.tsx      ← Catches errors in /products/:id
│       └── page.tsx
```

The most specific error boundary catches the error.

## Complete Error Boundary with Logging

```typescript
// src/app/dashboard/error.tsx
"use client";

import { useEffect } from "react";

interface ErrorProps {
  error: Error & { digest?: string };
  reset: () => void;
}

export default function DashboardError({ error, reset }: ErrorProps) {
  useEffect(() => {
    // In production, send to error tracking service
    // like Sentry, Bugsnag, or LogRocket
    if (process.env.NODE_ENV === "production") {
      fetch("/api/errors", {
        method: "POST",
        body: JSON.stringify({
          message: error.message,
          stack: error.stack,
          digest: error.digest,
          timestamp: new Date().toISOString(),
        }),
      }).catch(console.error);
    }
  }, [error]);

  return (
    <main
      style={{
        padding: "4rem 2rem",
        textAlign: "center",
        minHeight: "50vh",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <h1 style={{ fontSize: "2rem", marginBottom: "1rem" }}>
        Oops! Something went wrong
      </h1>
      <p style={{ color: "#666", marginBottom: "2rem" }}>
        We're sorry, but something unexpected happened.
      </p>
      <div style={{ display: "flex", gap: "1rem" }}>
        <button
          onClick={() => reset()}
          style={{
            padding: "0.75rem 1.5rem",
            backgroundColor: "#0070f3",
            color: "white",
            border: "none",
            borderRadius: "8px",
            cursor: "pointer",
          }}
        >
          Try Again
        </button>
        <button
          onClick={() => window.location.href = "/"}
          style={{
            padding: "0.75rem 1.5rem",
            backgroundColor: "transparent",
            color: "#0070f3",
            border: "1px solid #0070f3",
            borderRadius: "8px",
            cursor: "pointer",
          }}
        >
          Go Home
        </button>
      </div>
    </main>
  );
}
```

## Common Mistakes

### Mistake #1: Forgetting "use client"

```typescript
// ✗ Wrong: Error boundary without "use client"
export default function Error({ error, reset }) {
  return <p>Error!</p>;
}

// ✓ Correct: Must be client component
"use client";

export default function Error({ error, reset }) {
  return <p>Error!</p>;
}
```

### Mistake #2: Not Providing Reset

```typescript
// ✗ Wrong: No way for users to recover
export default function Error({ error }) {
  return <p>Error: {error.message}</p>;
}

// ✓ Correct: Provide reset button
export default function Error({ error, reset }) {
  return (
    <>
      <p>Error: {error.message}</p>
      <button onClick={() => reset()}>Try Again</button>
    </>
  );
}
```

### Mistake #3: Catching All Errors at Root

```typescript
// It's OK to have error.tsx at root, but also have specific ones
src/app/
├── error.tsx              // Catch-all
└── dashboard/
    └── error.tsx          // More specific - better UX
```

## Summary

- Create `error.tsx` to handle page errors
- Error boundaries must use `"use client"`
- Receive `error` and `reset` props
- Use `reset()` to let users try again
- More specific error boundaries override general ones

## Next Steps

Let's learn about the not-found page:

- [Not Found →](./not-found.md)
