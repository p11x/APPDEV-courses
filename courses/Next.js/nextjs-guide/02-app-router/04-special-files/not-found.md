# Not Found Page in Next.js

## What You'll Learn
- Creating custom 404 pages
- Using the `notFound()` function
- Handling missing resources gracefully

## Prerequisites
- Understanding of pages and dynamic routes
- Basic knowledge of HTTP status codes

## Concept Explained Simply

When someone visits a URL that doesn't exist, they see a "404 Not Found" error. By default, Next.js shows a generic error page, but you can create a custom `not-found.tsx` file to show a friendlier message.

The not-found page is different from the error boundary. Think of it this way:
- **Error boundary** = Something went wrong (a bug, crash)
- **Not-found page** = The page simply doesn't exist (wrong URL, deleted content)

You also use the `notFound()` function in your code to trigger the not-found page programmatically when data doesn't exist.

## How It Works

1. Create `not-found.tsx` in any route folder
2. When `notFound()` is called or the route doesn't exist, it shows your custom page
3. The page automatically gets a 404 status code

## Complete Code Example

Let's create a blog with proper not-found handling:

```typescript
// src/app/blog/[slug]/page.tsx - Blog post page
interface Props {
  params: Promise<{ slug: string }>;
}

const posts: Record<string, { title: string; content: string }> = {
  "hello-world": {
    title: "Hello World",
    content: "Welcome to my blog!",
  },
  "nextjs-rocks": {
    title: "Next.js Rocks",
    content: "Next.js is amazing!",
  },
};

export default async function BlogPostPage({ params }: Props) {
  const { slug } = await params;
  const post = posts[slug];

  if (!post) {
    // This triggers the not-found page
    return notFound();
  }

  return (
    <main style={{ padding: "2rem", maxWidth: "800px", margin: "0 auto" }}>
      <h1>{post.title}</h1>
      <p>{post.content}</p>
    </main>
  );
}
```

```typescript
// src/app/blog/[slug]/not-found.tsx - Custom 404 page
import Link from "next/link";

export default function NotFound() {
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
      <h1 style={{ fontSize: "6rem", margin: 0, color: "#ddd" }}>404</h1>
      <h2 style={{ fontSize: "1.5rem", margin: "1rem 0" }}>
        Post Not Found 😔
      </h2>
      <p style={{ color: "#666", marginBottom: "2rem" }}>
        The blog post you're looking for doesn't exist or has been removed.
      </p>
      <div style={{ display: "flex", gap: "1rem" }}>
        <Link
          href="/blog"
          style={{
            padding: "0.75rem 1.5rem",
            backgroundColor: "#0070f3",
            color: "white",
            textDecoration: "none",
            borderRadius: "8px",
          }}
        >
          View All Posts
        </Link>
        <Link
          href="/"
          style={{
            padding: "0.75rem 1.5rem",
            backgroundColor: "transparent",
            color: "#0070f3",
            border: "1px solid #0070f3",
            textDecoration: "none",
            borderRadius: "8px",
          }}
        >
          Go Home
        </Link>
      </div>
    </main>
  );
}
```

```typescript
// src/app/blog/page.tsx - Blog index
import Link from "next/link";

const posts = [
  { slug: "hello-world", title: "Hello World" },
  { slug: "nextjs-rocks", title: "Next.js Rocks" },
];

export default function BlogPage() {
  return (
    <main style={{ padding: "2rem", maxWidth: "800px", margin: "0 auto" }}>
      <h1>Blog</h1>
      <ul style={{ listStyle: "none", padding: 0 }}>
        {posts.map((post) => (
          <li
            key={post.slug}
            style={{
              padding: "1rem",
              border: "1px solid #ddd",
              marginBottom: "0.5rem",
              borderRadius: "8px",
            }}
          >
            <Link href={`/blog/${post.slug}`} style={{ textDecoration: "none" }}>
              <strong>{post.title}</strong>
            </Link>
          </li>
        ))}
      </ul>
    </main>
  );
}
```

## The notFound() Function

Import and use `notFound()` from `next/navigation`:

```typescript
import { notFound } from "next/navigation";

export default async function Page({ params }: Props) {
  const data = await fetchData(params.id);
  
  if (!data) {
    notFound();  // Triggers not-found.tsx
  }
  
  return <div>{data.content}</div>;
}
```

## Root Not-Found Page

You can create a global not-found page at the root:

```typescript
// src/app/not-found.tsx
import Link from "next/link";

export default function NotFound() {
  return (
    <main style={{ padding: "4rem 2rem", textAlign: "center" }}>
      <h1 style={{ fontSize: "4rem" }}>404</h1>
      <h2>Page Not Found</h2>
      <p>The page you're looking for doesn't exist.</p>
      <Link href="/">Go back home</Link>
    </main>
  );
}
```

## Not-Found vs Error

| Scenario | Use | File |
|----------|-----|------|
| Page doesn't exist | `notFound()` | `not-found.tsx` |
| Something crashed | Throw error | `error.tsx` |

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `notFound()` | Triggers 404 page | Shows custom not-found UI |
| `not-found.tsx` | Custom 404 page | Friendly message for users |
| Link to home | Navigation | Helps users get back on track |

## Common Mistakes

### Mistake #1: Not Handling Missing Data

```typescript
// ✗ Wrong: Not handling missing data
export default async function Page({ params }: Props) {
  const post = posts[params.slug];
  return <div>{post.content}</div>; // Crashes if post is undefined!
}

// ✓ Correct: Use notFound()
export default async function Page({ params }: Props) {
  const post = posts[params.slug];
  if (!post) notFound();
  return <div>{post.content}</div>;
}
```

### Mistake #2: Confusing notFound() with throw Error

```typescript
// ✗ Wrong: Throwing error for missing data
if (!data) {
  throw new Error("Not found");  // This triggers error.tsx, not not-found.tsx
}

// ✓ Correct: Use notFound()
if (!data) {
  notFound();  // This triggers not-found.tsx
}
```

### Mistake #3: Forgetting to Create not-found.tsx

```typescript
// Without not-found.tsx, users see a generic error
// Create the file to show a friendly message!
```

## Summary

- Create `not-found.tsx` for custom 404 pages
- Use `notFound()` function to trigger the 404 page
- Different from error boundary — use for missing content
- Works at any route level (root or nested)

## Next Steps

Now let's understand the difference between templates and layouts:

- [Template vs Layout →](./template-vs-layout.md)
