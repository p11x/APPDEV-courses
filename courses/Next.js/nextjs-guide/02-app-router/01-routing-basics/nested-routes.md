# Nested Routes in the App Router

## What You'll Learn
- How to create routes within routes (nesting)
- How layouts nest inside each other
- Building a multi-level navigation structure

## Prerequisites
- Understanding of page and layout files
- Basic knowledge of folder structure

## Concept Explained Simply

**Nested routes** are routes inside other routes. Just like folders can contain other folders, routes can contain sub-routes. When you nest routes, their layouts also nest — each level adds its own wrapper.

Think of it like a Russian nesting doll: each doll contains another smaller doll inside. The outermost layout is the root layout, and each nested route adds another layer of layout around your content.

If you have a blog at `/blog` and want individual posts at `/blog/my-post`, that's a nested route. The blog layout wraps around each individual post.

## How Nesting Works

```
URL: /blog/hello-world

Folder Structure:
src/app/
├── layout.tsx           ← Root layout (wraps everything)
├── blog/
│   ├── layout.tsx       ← Blog layout (wraps blog pages)
│   ├── page.tsx         ← /blog
│   └── [slug]/
│       └── page.tsx     ← /blog/:slug
```

When you visit `/blog/hello-world`:
1. Root layout renders (HTML, body, global nav)
2. Blog layout renders (blog sidebar, header)
3. Blog post page renders (the actual content)

## Complete Code Example

Let's build a blog system with nested routes:

```typescript
// src/app/blog/layout.tsx
import Link from "next/link";

export default function BlogLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div style={{ display: "flex", gap: "2rem", maxWidth: "1200px", margin: "0 auto", padding: "2rem" }}>
      <aside style={{ width: "250px", flexShrink: 0 }}>
        <h3>Blog Categories</h3>
        <nav>
          <ul style={{ listStyle: "none", padding: 0 }}>
            <li><Link href="/blog">All Posts</Link></li>
            <li><Link href="/blog/tech">Technology</Link></li>
            <li><Link href="/blog/lifestyle">Lifestyle</Link></li>
          </ul>
        </nav>
      </aside>
      <main style={{ flex: 1 }}>
        {children}
      </main>
    </div>
  );
}
```

```typescript
// src/app/blog/page.tsx - Blog index
// URL: /blog

export default function BlogIndexPage() {
  const posts = [
    { slug: "hello-world", title: "Hello World", category: "tech" },
    { slug: "nextjs-guide", title: "Next.js Guide", category: "tech" },
    { slug: "my-day", title: "My Day", category: "lifestyle" },
  ];

  return (
    <section>
      <h1>Blog Posts</h1>
      <ul style={{ display: "flex", flexDirection: "column", gap: "1rem" }}>
        {posts.map((post) => (
          <li key={post.slug} style={{ border: "1px solid #ddd", padding: "1rem", borderRadius: "8px" }}>
            <h2>
              <Link href={`/blog/${post.slug}`} style={{ textDecoration: "none", color: "#0070f3" }}>
                {post.title}
              </Link>
            </h2>
            <span style={{ color: "#666", fontSize: "0.9rem" }}>Category: {post.category}</span>
          </li>
        ))}
      </ul>
    </section>
  );
}
```

```typescript
// src/app/blog/[slug]/page.tsx - Individual blog post
// URL: /blog/:slug

interface Props {
  params: Promise<{ slug: string }>;
}

const posts: Record<string, { title: string; content: string; date: string }> = {
  "hello-world": {
    title: "Hello World",
    content: "Welcome to my first blog post! I'm excited to share my journey learning web development.",
    date: "January 1, 2024",
  },
  "nextjs-guide": {
    title: "Complete Next.js Guide",
    content: "Next.js is an amazing framework for building React applications. Let me show you why...",
    date: "January 15, 2024",
  },
  "my-day": {
    title: "My Day",
    content: "Today was a great day. I learned so much about Next.js and React Server Components!",
    date: "February 1, 2024",
  },
};

export default async function BlogPostPage({ params }: Props) {
  const { slug } = await params;
  const post = posts[slug];

  if (!post) {
    return (
      <main>
        <h1>Post Not Found</h1>
        <p>The post you're looking for doesn't exist.</p>
      </main>
    );
  }

  return (
    <article>
      <header style={{ marginBottom: "2rem" }}>
        <h1 style={{ fontSize: "2.5rem", marginBottom: "0.5rem" }}>{post.title}</h1>
        <time style={{ color: "#666" }}>{post.date}</time>
      </header>
      <div style={{ lineHeight: "1.8", fontSize: "1.1rem" }}>
        <p>{post.content}</p>
      </div>
    </article>
  );
}
```

```typescript
// src/app/blog/tech/page.tsx - Category page
// URL: /blog/tech

export default function TechBlogPage() {
  return (
    <section>
      <h1>Technology Posts</h1>
      <p>All about tech, code, and development.</p>
      {/* You could filter posts by category here */}
    </section>
  );
}
```

## The URL Hierarchy

Here's how the URLs map to files:

| URL | File |
|-----|------|
| `/blog` | `src/app/blog/page.tsx` |
| `/blog/tech` | `src/app/blog/tech/page.tsx` |
| `/blog/hello-world` | `src/app/blog/[slug]/page.tsx` |
| `/blog/lifestyle` | `src/app/blog/lifestyle/page.tsx` |

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `{children}` | Injected by Next.js | The current page renders here |
| `<aside>` | Sidebar element | Contains navigation for the blog |
| `Link href={`/blog/${post.slug}`}` | Dynamic link | Links to each blog post |
| `const { slug } = await params` | Get URL parameter | Extracts the dynamic segment |
| `posts[slug]` | Lookup post data | Gets the post by slug |

## Common Mistakes

### Mistake #1: Forgetting the Parent Page

A folder needs a `page.tsx` to be a valid route:

```typescript
// ✗ Wrong: Missing page.tsx in blog folder
src/app/blog/
└── [slug]/
    └── page.tsx

// ✓ Correct: Has page.tsx for /blog
src/app/blog/
├── page.tsx
└── [slug]/
    └── page.tsx
```

### Mistake #2: Not Awaiting Params

In Next.js 15, params is async:

```typescript
// ✗ Wrong: Not awaiting params
export default function Page({ params }) {
  const { slug } = params;
}

// ✓ Correct: Await params first
export default async function Page({ params }: Props) {
  const { slug } = await params;
}
```

### Mistake #3: Breaking the Layout Chain

Every nested route needs its parent to have a layout:

```typescript
// If you have:
src/app/
├── layout.tsx      // Root
├── page.tsx        // Homepage
└── dashboard/
    └── page.tsx    // Needs dashboard/layout.tsx to have proper nesting

// Add layout:
src/app/dashboard/
└── layout.tsx      // Dashboard-specific layout
```

## Summary

- Nested routes are created by folders inside folders
- Each route level adds its own layout wrapper
- The URL reflects the folder structure
- Dynamic segments (like `[slug]`) create variable routes
- Layouts nest: root → section → page

## Next Steps

Now let's learn about dynamic routes in detail:

- [Dynamic Segments →](../../02-app-router/02-dynamic-routes/dynamic-segments.md)
