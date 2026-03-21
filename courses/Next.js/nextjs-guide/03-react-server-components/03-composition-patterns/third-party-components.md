# Third-Party Components with Server Components

## What You'll Learn
- How to use third-party libraries in Server Components
- Common compatibility issues
- Best practices for external packages

## Prerequisites
- Understanding of Server and Client Components
- Knowledge of npm packages

## Concept Explained Simply

Third-party components (from npm packages like `swr`, `prisma`, `zustand`) are very common in React apps. Most of them were built for the traditional client-side React model, so they may need special handling in Next.js Server Components.

The general rule: if a package uses browser-only APIs or React hooks, you'll need to wrap it or use it in a Client Component. If it runs on the server (like database clients), you can use it directly in Server Components.

## Categories of Third-Party Packages

| Type | Example | Use In |
|------|---------|--------|
| Database ORM | Prisma, Drizzle | Server Components |
| Data fetching | SWR, React Query | Client Components |
| State management | Redux, Zustand | Client Components |
| UI libraries | Radix, Headless UI | Client Components |
| Auth | NextAuth.js | Both |

## Complete Code Example

Let's see how to use different types of packages:

### Using Database ORMs (Server-Side)

```typescript
// src/lib/prisma.ts - Prisma client
import { PrismaClient } from "@prisma/client";

const globalForPrisma = globalThis as unknown as {
  prisma: PrismaClient | undefined;
};

export const prisma = globalForPrisma.prisma ?? new PrismaClient();

if (process.env.NODE_ENV !== "production") globalForPrisma.prisma = prisma;
```

```typescript
// src/app/blog/page.tsx - Server Component uses Prisma directly
import { prisma } from "@/lib/prisma";

export default async function BlogPage() {
  // Direct database query in Server Component!
  const posts = await prisma.post.findMany({
    where: { published: true },
    orderBy: { createdAt: "desc" },
  });

  return (
    <main style={{ padding: "2rem" }}>
      <h1>Blog</h1>
      {posts.map((post) => (
        <article
          key={post.id}
          style={{ padding: "1rem", border: "1px solid #ddd", marginBottom: "1rem" }}
        >
          <h2>{post.title}</h2>
          <p>{post.excerpt}</p>
          <time>{post.createdAt.toLocaleDateString()}</time>
        </article>
      ))}
    </main>
  );
}
```

### Using SWR (Client-Side)

```typescript
// src/components/useProductData.ts - Custom hook with SWR
"use client";

import useSWR from "swr";

const fetcher = (url: string) => fetch(url).then((res) => res.json());

export function useProductData(productId: string) {
  const { data, error, isLoading, mutate } = useSWR(
    `/api/products/${productId}`,
    fetcher
  );

  return {
    product: data,
    isLoading,
    isError: error,
    mutate,
  };
}
```

```typescript
// src/components/ProductReviews.tsx - Using the hook
"use client";

import { useProductData } from "./useProductData";

export function ProductReviews({ productId }: { productId: string }) {
  const { product, isLoading, isError } = useProductData(productId);

  if (isLoading) return <p>Loading reviews...</p>;
  if (isError) return <p>Error loading reviews</p>;

  return (
    <div>
      <h3>Reviews</h3>
      {product?.reviews?.map((review: any) => (
        <div key={review.id} style={{ padding: "0.5rem" }}>
          <p>{review.text}</p>
          <small>{review.rating} stars</small>
        </div>
      ))}
    </div>
  );
}
```

### Mixing Both Approaches

```typescript
// src/app/products/[id]/page.tsx
// Server Component fetches main data
// Client Component fetches real-time data (reviews, stock)

import { prisma } from "@/lib/prisma";
import { ProductReviews } from "@/components/ProductReviews";
import { AddToCartButton } from "@/components/AddToCartButton";

interface Props {
  params: Promise<{ id: string }>;
}

export default async function ProductPage({ params }: Props) {
  const { id } = await params;
  
  // Server-side: Get main product data
  const product = await prisma.product.findUnique({
    where: { id },
  });

  if (!product) {
    return <div>Product not found</div>;
  }

  return (
    <main style={{ padding: "2rem" }}>
      <h1>{product.name}</h1>
      <p>{product.description}</p>
      <p>${product.price}</p>
      
      {/* Client component for real-time data */}
      <ProductReviews productId={product.id} />
      
      {/* Interactive component */}
      <AddToCartButton productId={product.id} />
    </main>
  );
}
```

## Working with UI Libraries

Many UI libraries need client-side interactivity:

```typescript
// Using a library like Radix UI (needs "use client")
"use client";

import * as Dialog from "@radix-ui/react-dialog";

export function CustomDialog({ children }: { children: React.ReactNode }) {
  return (
    <Dialog.Root>
      <Dialog.Trigger asChild>
        <button>Open Dialog</button>
      </Dialog.Trigger>
      <Dialog.Portal>
        <Dialog.Overlay />
        <Dialog.Content>
          {children}
          <Dialog.Close>Close</Dialog.Close>
        </Dialog.Content>
      </Dialog.Portal>
    </Dialog.Root>
  );
}
```

## Using Auth Libraries

```typescript
// NextAuth works in both contexts
// src/app/api/auth/[...nextauth]/route.ts - API route
import NextAuth from "next-auth";
import GoogleProvider from "next-auth/providers/google";

const handler = NextAuth({
  providers: [
    GoogleProvider({
      clientId: process.env.GOOGLE_CLIENT_ID!,
      clientSecret: process.env.GOOGLE_CLIENT_SECRET!,
    }),
  ],
});

export { handler as GET, handler as POST };
```

```typescript
// src/lib/auth.ts - Server-side auth helpers
import { getServerSession } from "next-auth";

export async function getCurrentUser() {
  const session = await getServerSession();
  return session?.user;
}
```

```typescript
// src/app/dashboard/page.tsx - Use in Server Component
import { getCurrentUser } from "@/lib/auth";

export default async function DashboardPage() {
  const user = await getCurrentUser();
  
  if (!user) {
    return <div>Please log in</div>;
  }

  return (
    <main>
      <h1>Welcome, {user.name}</h1>
    </main>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| Prisma in server | Database queries | Direct server-side access |
| SWR in client | Real-time data | Needs browser + hooks |
| UI library "use client" | Interactive UI | Uses React hooks |
| NextAuth | Authentication | Works in both contexts |

## Common Mistakes

### Mistake #1: Using Client-Only Packages in Server

```typescript
// ✗ Wrong: Using SWR in Server Component
import useSWR from "swr";

export default async function Page() {
  const { data } = useSWR("/api/data", fetcher); // Error!
  return <div>{data}</div>;
}

// ✓ Correct: Use in Client Component
"use client";
export function ClientComponent() {
  const { data } = useSWR("/api/data", fetcher);
  return <div>{data}</div>;
}
```

### Mistake #2: Forgetting "use client" for UI Libraries

```typescript
// ✗ Wrong: UI component without client directive
import { InteractiveComponent } from "some-ui-lib";

export default function Page() {
  return <InteractiveComponent />; // May not work!
}

// ✓ Correct: Add "use client"
"use client";
import { InteractiveComponent } from "some-ui-lib";

export function InteractiveWrapper() {
  return <InteractiveComponent />;
}
```

### Mistake #3: Importing Browser APIs in Server

```typescript
// ✗ Wrong: Using localStorage in server component
export default async function Page() {
  const theme = localStorage.getItem("theme"); // Error - no browser!
  return <div>{theme}</div>;
}

// ✓ Correct: Use in client component
"use client";
export function ThemeComponent() {
  const theme = localStorage.getItem("theme");
  return <div>{theme}</div>;
}
```

## Summary

- Database ORMs (Prisma, Drizzle) work directly in Server Components
- Data fetching hooks (SWR, React Query) need Client Components
- UI libraries typically need "use client"
- Auth libraries (NextAuth) work in both contexts
- Always check if a package needs browser APIs before using in Server Components

Now let's move to Server Actions. Let me update the todo list and continue.
</think>

Given the complexity of creating all remaining files, let me create multiple files in sequence to complete the guide. Let me continue with the remaining sections.
<minimax:tool_call>
<invoke name="update_todo_list">
<parameter name="todos">[x] Task understood - creating complete Next.js guide
[x] Creating 01-introduction folder files
[x] Creating 02-app-router folder files
[x] Creating 03-react-server-components folder files
[-] Creating 04-server-actions folder files
[ ] Creating 05-data-fetching folder files
[ ] Creating 06-rendering folder files
[ ] Creating 07-styling folder files
[ ] Creating 08-optimization folder files
[ ] Creating 09-metadata-and-seo folder files
[ ] Creating 10-authentication folder files
[ ] Creating 11-deployment folder files