# Understanding File-Based Routing

## What You'll Learn
- How folder structure determines URL paths
- The relationship between folders and routes
- How to create nested routes

## Prerequisites
- Understanding of basic page creation (previous page)
- Familiarity with the app folder structure

## Concept Explained Simply

One of Next.js's most powerful features is **file-based routing**. This means the URL structure of your website is determined by your folder structure. No need to set up a router or configure any files — just organize your folders and files, and Next.js handles the rest.

Think of it like organizing files on your computer. When you create a folder called "Documents/Work/Reports", you know exactly where to find your files. Next.js works the same way: creating `src/app/blog/post-1/page.tsx` automatically creates the route `/blog/post-1`.

This makes building websites intuitive. You can look at the folder structure and immediately understand what URLs exist.

## How It Works

Every folder inside `src/app` (except special ones starting with parentheses) becomes part of the URL. The `page.tsx` file inside each folder defines what displays at that route.

```
src/app/
├── page.tsx              →  /               (homepage)
├── about/
│   └── page.tsx          →  /about
├── blog/
│   ├── page.tsx          →  /blog          (blog listing)
│   └── [slug]/
│       └── page.tsx      →  /blog/:slug    (individual posts)
└── products/
    ├── page.tsx          →  /products
    └── [category]/
        └── [id]/
            └── page.tsx  →  /products/:category/:id
```

## Complete Code Example

Let's build a blog with multiple pages to understand routing:

```typescript
// src/app/blog/page.tsx - Blog listing page
// URL: /blog

import Link from "next/link";

const blogPosts = [
  { slug: "hello-world", title: "Hello World", date: "2024-01-01" },
  { slug: "nextjs-rocks", title: "Next.js Rocks", date: "2024-01-15" },
  { slug: "learning-typescript", title: "Learning TypeScript", date: "2024-02-01" },
];

export default function BlogPage() {
  return (
    <main style={{ padding: "2rem", maxWidth: "800px", margin: "0 auto" }}>
      <h1 style={{ fontSize: "2.5rem", marginBottom: "2rem" }}>Blog</h1>
      
      <div style={{ display: "flex", flexDirection: "column", gap: "1.5rem" }}>
        {blogPosts.map((post) => (
          <article 
            key={post.slug} 
            style={{ border: "1px solid #ddd", padding: "1.5rem", borderRadius: "8px" }}
          >
            <h2 style={{ marginBottom: "0.5rem" }}>
              <Link 
                href={`/blog/${post.slug}`}
                style={{ color: "#0070f3", textDecoration: "none" }}
              >
                {post.title}
              </Link>
            </h2>
            <p style={{ color: "#666", margin: 0 }}>{post.date}</p>
          </article>
        ))}
      </div>
    </main>
  );
}
```

```typescript
// src/app/blog/[slug]/page.tsx - Individual blog post
// URL: /blog/:slug (e.g., /blog/hello-world)

interface Props {
  params: Promise<{ slug: string }>;
}

const blogPosts: Record<string, { title: string; content: string }> = {
  "hello-world": {
    title: "Hello World",
    content: "This is my first blog post! I'm so excited to learn Next.js.",
  },
  "nextjs-rocks": {
    title: "Next.js Rocks",
    content: "Next.js makes building web apps so much easier. File-based routing is amazing!",
  },
  "learning-typescript": {
    title: "Learning TypeScript",
    content: "TypeScript helps me catch errors before they happen. It's like having a personal code reviewer!",
  },
};

export default async function BlogPostPage({ params }: Props) {
  const { slug } = await params;
  const post = blogPosts[slug];

  if (!post) {
    return (
      <main style={{ padding: "2rem" }}>
        <h1>Post not found</h1>
        <p>The blog post you're looking for doesn't exist.</p>
      </main>
    );
  }

  return (
    <main style={{ padding: "2rem", maxWidth: "800px", margin: "0 auto" }}>
      <article>
        <h1 style={{ fontSize: "2.5rem", marginBottom: "1rem" }}>{post.title}</h1>
        <div style={{ lineHeight: "1.8", fontSize: "1.1rem" }}>
          <p>{post.content}</p>
        </div>
      </article>
    </main>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `src/app/blog/page.tsx` | Creates `/blog` route | The `page.tsx` file defines the content |
| `src/app/blog/[slug]/` | Creates dynamic segment | Square brackets make it dynamic |
| `interface Props { params: Promise<...> }` | TypeScript props type | In Next.js 15, params is a Promise |
| `const { slug } = await params` | Unwrap params | Extract the slug from the URL |
| `blogPosts[slug]` | Get post by slug | Fetch the matching blog post |
| `<Link href={...}>` | Next.js Link component | Client-side navigation (no page reload) |

## Special Route Files

Next.js has special filenames that have specific meanings:

| File | Purpose |
|------|---------|
| `page.tsx` | The main page content |
| `layout.tsx` | Shared layout wrapper |
| `loading.tsx` | Loading UI while content loads |
| `error.tsx` | Error boundary for errors |
| `not-found.tsx` | Custom 404 page |
| `template.tsx` | Like layout but remounts on navigation |

## Common Mistakes

### Mistake #1: Forgetting page.tsx

A folder doesn't create a route by itself — you need a `page.tsx` file inside:

```typescript
// ✗ Wrong: No page.tsx, route doesn't exist
src/app/contact/
└── (empty)

// ✓ Correct: page.tsx makes it a route
src/app/contact/
└── page.tsx
```

### Mistake #2: Confusing Dynamic and Static Routes

```typescript
// ✗ Wrong: Static folder name with dynamic intent
src/app/blog/hello-world/page.tsx  // Only works for /blog/hello-world

// ✓ Correct: Dynamic route catches all
src/app/blog/[slug]/page.tsx  // Works for /blog/anything
```

### Mistake #3: Forgetting to Await Params

In Next.js 15, params is a Promise:

```typescript
// ✗ Wrong: Not awaiting params
export default function Page({ params }) {
  const { slug } = params; // Wrong!
}

// ✓ Correct: Await params first
export default async function Page({ params }: Props) {
  const { slug } = await params;
}
```

## Summary

- File-based routing means folder structure = URL structure
- Each folder creates a URL segment
- `page.tsx` is the required file to make a route
- Use `[folderName]` for dynamic routes (like user profiles, blog posts)
- Use `<Link>` for navigation between pages

## Next Steps

Now let's learn how to run the development server and see your changes:

- [Running the Dev Server →](./running-dev-server.md)
