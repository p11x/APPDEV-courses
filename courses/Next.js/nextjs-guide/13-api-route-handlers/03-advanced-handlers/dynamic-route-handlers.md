# Dynamic Route Handlers

## What You'll Learn
- Using dynamic segments in API routes
- Handling different ID formats
- Combining dynamic segments with query parameters
- Type-safe parameter handling

## Prerequisites
- Understanding of route handlers
- Knowledge of dynamic routes in App Router
- Familiarity with URL parsing

## Concept Explained Simply

Dynamic route handlers work exactly like dynamic pages — the folder name determines what part of the URL becomes a variable. For API routes, if you have `app/api/users/[id]/route.ts`, then the `[id]` part becomes a parameter you can read in your code.

This is incredibly useful because one handler file can handle ALL users, not just one. Instead of making 100 different files for 100 users, you make ONE file that handles all of them dynamically.

## Complete Code Example

### Basic Dynamic Segment

```typescript
// app/api/users/[id]/route.ts
import { NextResponse } from "next/server";
import { db } from "@/lib/db";

export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  
  const user = await db.user.findUnique({
    where: { id },
  });
  
  if (!user) {
    return NextResponse.json(
      { error: "User not found" },
      { status: 404 }
    );
  }
  
  return NextResponse.json(user);
}

export async function PUT(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  const body = await request.json();
  
  const user = await db.user.update({
    where: { id },
    data: body,
  });
  
  return NextResponse.json(user);
}

export async function DELETE(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  
  await db.user.delete({
    where: { id },
  });
  
  return new NextResponse(null, { status: 204 });
}
```

### Multiple Dynamic Segments

```typescript
// app/api/users/[userId]/posts/[postId]/route.ts
import { NextResponse } from "next/server";

export async function GET(
  request: Request,
  { params }: { params: Promise<{ userId: string; postId: string }> }
) {
  const { userId, postId } = await params;
  
  // Fetch specific post by specific user
  const post = await getUserPost(userId, postId);
  
  if (!post) {
    return NextResponse.json(
      { error: "Post not found" },
      { status: 404 }
    );
  }
  
  return NextResponse.json(post);
}
```

### With Query Parameters

```typescript
// app/api/products/[category]/route.ts
import { NextResponse } from "next/server";

export async function GET(
  request: Request,
  { params }: { params: Promise<{ category: string }> }
) {
  const { category } = await params;
  
  // Also get query parameters
  const { searchParams } = new URL(request.url);
  const sort = searchParams.get("sort") || "newest";
  const minPrice = searchParams.get("minPrice");
  const limit = parseInt(searchParams.get("limit") || "20");
  
  const products = await db.product.findMany({
    where: { 
      category,
      price: {
        gte: minPrice ? parseFloat(minPrice) : 0,
      },
    },
    orderBy: sort === "price" ? { price: "asc" } : { createdAt: "desc" },
    take: limit,
  });
  
  return NextResponse.json(products);
}
```

### Catch-All Segments

```typescript
// app/api/docs/[...slug]/route.ts
// Handles: /api/docs/intro, /api/docs/getting-started/install, etc.

export async function GET(
  request: Request,
  { params }: { params: Promise<{ slug: string[] }> }
) {
  const { slug } = await params;
  
  // slug is an array: ["intro"] or ["getting-started", "install"]
  const path = slug.join("/");
  
  const doc = await getDocumentation(path);
  
  if (!doc) {
    return NextResponse.json(
      { error: "Documentation page not found" },
      { status: 404 }
    );
  }
  
  return NextResponse.json(doc);
}
```

### Optional Catch-All

```typescript
// app/api/posts/[[...slug]]/route.ts
// Handles: /api/posts, /api/posts/2024, /api/posts/2024/march

export async function GET(
  request: Request,
  { params }: { params: Promise<{ slug?: string[] }> }
) {
  const { slug } = await params;
  
  // slug can be undefined (for /api/posts)
  if (!slug) {
    // Return all posts
    return NextResponse.json(await getAllPosts());
  }
  
  // Or array for specific path
  const [year, month] = slug;
  const posts = await getPostsByDate(year, month);
  
  return NextResponse.json(posts);
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `{ params: Promise<{ id: string }> }` | Type for dynamic params | params is a Promise in Next.js 15 |
| `await params` | Await to get actual values | Must await before using |
| `userId: string` | Multiple dynamic segments | Access each separately |
| `slug: string[]` | Catch-all as array | Each segment is array element |
| `slug?: string[]` | Optional catch-all | Can be undefined |

## Common Mistakes

### Mistake 1: Not Awaiting Params (Next.js 15)

```typescript
// WRONG - In Next.js 15, params is a Promise
export async function GET(
  request: Request,
  { params }: { params: { id: string } }
) {
  const { id } = params; // Wrong!
}

// CORRECT - Await params
export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
}
```

### Mistake 2: Not Validating Dynamic Parameters

```typescript
// WRONG - Using ID directly without validation
export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  const user = await db.user.findUnique({ where: { id } });
  // What if id is "abc" (not a valid UUID)?
}

// CORRECT - Validate the ID format
export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  
  // Validate UUID format
  const uuidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
  if (!uuidRegex.test(id)) {
    return NextResponse.json(
      { error: "Invalid user ID format" },
      { status: 400 }
    );
  }
  
  const user = await db.user.findUnique({ where: { id } });
  // ...
}
```

### Mistake 3: Mixing Query Params with Dynamic Segments

```typescript
// app/api/users/[id]/route.ts

// WRONG - Not reading query params correctly
export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  // How to get query params? Need URL object!
}

// CORRECT - Create URL object
export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  const { searchParams } = new URL(request.url);
  const include = searchParams.get("include");
  // ...
}
```

## Summary

- Dynamic segments in API routes work exactly like pages
- Always await `params` in Next.js 15 (it's a Promise)
- Validate dynamic parameters before using in queries
- Combine dynamic segments with query parameters for more flexibility
- Catch-all routes use arrays (`string[]`)
- Optional catch-all can be undefined

## Next Steps

- [streaming-responses.md](./streaming-responses.md) - Using streaming for large responses
- [webhooks.md](./webhooks.md) - Handling webhook callbacks
