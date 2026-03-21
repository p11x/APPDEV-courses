# Dynamic Rendering in Next.js

## What You'll Learn
- What is dynamic rendering
- When it happens automatically
- Opting into dynamic behavior

## Concept Explained Simply

**Dynamic Rendering** generates pages at request time. Each request gets a fresh HTML response, allowing for personalized or real-time content.

## When It Happens

- Using cookies or headers
- Using `no-store` cache option
- Using dynamic functions like `cookies()` or `headers()`

## Complete Example

```typescript
// src/app/dashboard/page.tsx
import { cookies } from "next/headers";

export default async function DashboardPage() {
  // This makes the page dynamic
  const cookieStore = cookies();
  const session = cookieStore.get("session");
  
  return (
    <main>
      <h1>Dashboard</h1>
      <p>Welcome, user!</p>
    </main>
  );
}
```

```typescript
// src/app/api/products/route.ts
// Dynamic route with no-store
export async function GET() {
  const products = await fetch("https://api.example.com/products", {
    cache: "no-store",
  });
  
  return Response.json(products);
}
```

## Summary

- Dynamic = fresh on every request
- Happens automatically with dynamic functions
- Use for personalized/user-specific content
