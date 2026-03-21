# Async Server Components

## What You'll Learn
- How to fetch data directly in Server Components
- Why async components are powerful
- Understanding data flow in Server Components

## Prerequisites
- Understanding of Server Components
- Basic async/await knowledge

## Concept Explained Simply

One of the most powerful features of Server Components is that they can be **async**. This means you can directly await data fetching inside your component without any special setup. No useEffect, no loading states in your component — just fetch and render.

Think of it like cooking with a smart oven. Traditional React (useEffect) is like cooking on a regular stove where you have to constantly check if the food is ready. Async Server Components are like a smart oven that automatically knows when the food is done — you just put it in and wait for the result.

## How It Works

In a Server Component, you can mark your component as `async` and use `await`:

```typescript
// This is an async Server Component
export default async function Page() {
  const data = await fetchData();  // Direct await!
  return <div>{data.content}</div>;
}
```

That's it! The component waits for the data before rendering.

## Complete Code Example

Let's build a blog with data fetching:

```typescript
// src/lib/api.ts - Simulated API functions
export interface Post {
  id: string;
  title: string;
  content: string;
  author: string;
  publishedAt: string;
}

export async function getPosts(): Promise<Post[]> {
  // Simulate network delay
  await new Promise((resolve) => setTimeout(resolve, 500));
  
  return [
    {
      id: "1",
      title: "Getting Started with Next.js",
      content: "Next.js is an amazing framework for building React applications...",
      author: "Jane Doe",
      publishedAt: "2024-01-15",
    },
    {
      id: "2",
      title: "Understanding Server Components",
      content: "Server Components allow you to render components on the server...",
      author: "John Smith",
      publishedAt: "2024-01-20",
    },
    {
      id: "3",
      title: "Data Fetching Patterns",
      content: "There are many ways to fetch data in Next.js...",
      author: "Jane Doe",
      publishedAt: "2024-01-25",
    },
  ];
}

export async function getPost(id: string): Promise<Post | null> {
  await new Promise((resolve) => setTimeout(resolve, 300));
  
  const posts = await getPosts();
  return posts.find((p) => p.id === id) || null;
}
```

```typescript
// src/app/blog/page.tsx - Blog listing with async fetch
import Link from "next/link";
import { getPosts } from "@/lib/api";

export default async function BlogPage() {
  // Fetch data directly in the component
  const posts = await getPosts();

  return (
    <main style={{ maxWidth: "800px", margin: "0 auto", padding: "2rem" }}>
      <h1 style={{ fontSize: "2.5rem", marginBottom: "2rem" }}>Blog</h1>
      
      <div style={{ display: "flex", flexDirection: "column", gap: "1.5rem" }}>
        {posts.map((post) => (
          <article
            key={post.id}
            style={{
              padding: "1.5rem",
              border: "1px solid #ddd",
              borderRadius: "8px",
            }}
          >
            <Link
              href={`/blog/${post.id}`}
              style={{ textDecoration: "none", color: "inherit" }}
            >
              <h2 style={{ marginBottom: "0.5rem" }}>{post.title}</h2>
            </Link>
            <div
              style={{
                color: "#666",
                fontSize: "0.9rem",
                marginBottom: "1rem",
              }}
            >
              By {post.author} • {post.publishedAt}
            </div>
            <p style={{ lineHeight: "1.6" }}>{post.content}</p>
          </article>
        ))}
      </div>
    </main>
  );
}
```

```typescript
// src/app/blog/[id]/page.tsx - Individual post with async fetch
import Link from "next/link";
import { getPost, getPosts } from "@/lib/api";
import { notFound } from "next/navigation";

interface Props {
  params: Promise<{ id: string }>;
}

// Generate static params for static export
export async function generateStaticParams() {
  const posts = await getPosts();
  return posts.map((post) => ({ id: post.id }));
}

export default async function BlogPostPage({ params }: Props) {
  const { id } = await params;
  const post = await getPost(id);

  if (!post) {
    notFound();
  }

  return (
    <main style={{ maxWidth: "800px", margin: "0 auto", padding: "2rem" }}>
      <Link
        href="/blog"
        style={{
          color: "#0070f3",
          textDecoration: "none",
          display: "inline-block",
          marginBottom: "2rem",
        }}
      >
        ← Back to Blog
      </Link>
      
      <article>
        <header style={{ marginBottom: "2rem" }}>
          <h1 style={{ fontSize: "2.5rem", marginBottom: "0.5rem" }}>
            {post.title}
          </h1>
          <div style={{ color: "#666" }}>
            By {post.author} • {post.publishedAt}
          </div>
        </header>
        
        <div style={{ lineHeight: "1.8", fontSize: "1.1rem" }}>
          <p>{post.content}</p>
        </div>
      </article>
    </main>
  );
}
```

## How Data Flows

```
User visits /blog
        ↓
Server Component (BlogPage) starts rendering
        ↓
await getPosts() - component PAUSES here
        ↓
Data arrives from API
        ↓
Component RESUMES with the data
        ↓
HTML is generated with all data
        ↓
HTML is sent to browser (complete!)
```

## Parallel Data Fetching

You can fetch multiple things at once:

```typescript
// src/app/dashboard/page.tsx
import { getUser } from "@/lib/auth";
import { getNotifications } from "@/lib/notifications";
import { getRecentOrders } from "@/lib/orders";

export default async function DashboardPage() {
  // Fetch all data in parallel - faster!
  const [user, notifications, orders] = await Promise.all([
    getUser(),
    getNotifications(),
    getRecentOrders(),
  ]);

  return (
    <main>
      <h1>Welcome, {user.name}</h1>
      {/* Render data */}
    </main>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `async function Page()` | Async component | Can use await |
| `await getPosts()` | Fetch data | Component pauses until data arrives |
| `generateStaticParams()` | Static generation | Pre-generates pages at build time |
| `Promise.all([...])` | Parallel fetch | Faster - fetches simultaneously |

## Common Mistakes

### Mistake #1: Forgetting to Await

```typescript
// ✗ Wrong: Not awaiting the data
export default function Page() {
  const posts = getPosts();  // Returns a Promise, not the data!
  return <div>{posts[0]?.title}</div>; // Error!
}

// ✓ Correct: Await the data
export default async function Page() {
  const posts = await getPosts();
  return <div>{posts[0]?.title}</div>;
}
```

### Mistake #2: Using useEffect for Server Data

```typescript
// ✗ Wrong: Old React pattern, not needed in Server Components
"use client";

import { useEffect, useState } from "react";

export function OldWay() {
  const [posts, setPosts] = useState([]);
  
  useEffect(() => {
    fetchPosts().then(setPosts);
  }, []);
  
  return <div>Loading...</div>;
}

// ✓ Correct: Fetch directly in Server Component
export default async function NewWay() {
  const posts = await fetchPosts();
  return <div>{posts.map(p => <Post key={p.id} post={p} />)}</div>;
}
```

### Mistake #3: Not Handling Errors

```typescript
// ✗ Wrong: No error handling
export default async function Page() {
  const data = await fetchRiskyData(); // Can throw!
  return <div>{data}</div>;
}

// ✓ Correct: Try/catch or error boundary
export default async function Page() {
  try {
    const data = await fetchRiskyData();
    return <div>{data}</div>;
  } catch (e) {
    return <div>Failed to load data</div>;
  }
}
```

## Summary

- Server Components can be async and use await directly
- No need for useEffect or loading states
- Fetch data at the top level of your component
- Use Promise.all for parallel fetching
- Errors can be caught with try/catch or error boundaries

## Next Steps

Let's learn about fetch caching:

- [Fetch with Caching →](./fetch-with-caching.md)
