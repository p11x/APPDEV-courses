# App Router Fundamentals

## Overview
The Next.js App Router (introduced in Next.js 13) is a significant evolution of the framework's routing and rendering capabilities. It introduces a new file-based routing system, Server Components by default, nested layouts, and improved data fetching patterns. This guide covers the core concepts of the App Router, including the file structure, layouts, routing conventions, and how to leverage Server and Client Components effectively.

## Prerequisites
- Understanding of Next.js basics
- Familiarity with React components and hooks
- Knowledge of file-based routing concepts

## Core Concepts

### File-Based Routing in App Router
The App Router uses the `app/` directory for routing. Each route segment is a folder, and special files define the UI for that segment:

```bash
# [File: app/ directory structure]
app/
├── layout.tsx          # Root layout (applies to all routes)
├── page.tsx            # Home page (/)
├── about/
│   ├── layout.tsx      # About section layout
│   └── page.tsx        # About page (/about)
├── dashboard/
│   ├── layout.tsx      # Dashboard layout
│   ├── page.tsx        # Dashboard page (/dashboard)
│   └── settings/
│       └── page.tsx    # Settings page (/dashboard/settings)
├── blog/
│   ├── layout.tsx
│   ├── page.tsx        # Blog index (/blog)
│   └── [slug]/
│       └── page.tsx    # Blog post (/blog/[slug])
└── (marketing)/        # Route group (optional)
    ├── layout.tsx
    └── page.tsx
```

### Special Files in App Router
Each folder can contain these special files:

```tsx
// [File: app/dashboard/layout.tsx]
export default function DashboardLayout({
  children, // Children will be the dashboard page or nested routes
}: {
  children: React.ReactNode;
}) {
  return (
    <section>
      <nav>Dashboard Navigation</nav>
      <main>{children}</main>
    </section>
  );
}
```

```tsx
// [File: app/dashboard/page.tsx]
export default function DashboardPage() {
  return <div>Dashboard Content</div>;
}
```

```tsx
// [File: app/dashboard/loading.tsx]
export default function DashboardLoading() {
  return <div>Loading dashboard...</div>;
}
```

```tsx
// [File: app/dashboard/error.tsx]
export default function DashboardError({
  error, // Error object
  reset, // Function to retry
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  return (
    <div>
      <h2>Something went wrong!</h2>
      <p>{error.message}</p>
      <button onClick={reset}>Try again</button>
    </div>
  );
}
```

### Server Components by Default
In the App Router, components are Server Components by default:

```tsx
// [File: app/page.tsx - Server Component]
import { getPosts } from '@/lib/api';

// This runs on the server - no bundle sent to client
export default async function HomePage() {
  const posts = await getPosts(); // Can access DB, env vars, etc.
  
  return (
    <ul>
      {posts.map(post => (
        <li key={post.id}>{post.title}</li>
      ))}
    </ul>
  );
}

// Benefits:
// - Zero JavaScript bundle impact for data fetching
// - Direct access to backend resources
// - Streaming HTML for faster initial load
```

### Client Components
Use `'use client'` to opt into client-side rendering:

```tsx
// [File: app/components/InteractiveButton.tsx]
'use client'; // This component runs in the browser

import { useState } from 'react';

export default function InteractiveButton() {
  const [count, setCount] = useState(0);
  
  return (
    <button onClick={() => setCount(c => c + 1)}>
      Clicked {count} times
    </button>
  );
}

// When to use:
// - Need state (useState, useReducer)
// - Need effects (useEffect, useLayoutEffect)
// - Need event handlers (onClick, onChange)
// - Using browser-only APIs (localStorage, geolocation)
```

### Nested Layouts
Layouts nest automatically based on file structure:

```tsx
// [File: app/layout.tsx] - Root layout
export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>
        {/* This layout wraps ALL pages */}
        <nav>Global Navigation</nav>
        {children}
      </body>
    </html>
  );
}
```

```tsx
// [File: app/dashboard/layout.tsx] - Dashboard layout
export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <section>
      {/* This layout wraps dashboard and its children */}
      <aside>Dashboard Sidebar</aside>
      <main>{children}</main>
    </section>
  );
}
```

```tsx
// [File: app/dashboard/settings/layout.tsx] - Settings layout
export default function SettingsLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="settings-container">
      <h2>Settings</h2>
      {children}
    </div>
  );
}
```

// Resulting hierarchy for /dashboard/settings:
// RootLayout > DashboardLayout > SettingsLayout > SettingsPage

### Route Groups
Use parentheses to create route groups that don't affect the URL path:

```bash
# [File: app/ directory with route groups]
app/
├── (marketing)/
│   ├── layout.tsx
│   ├── page.tsx          # / (marketing home)
│   └── about/
│       └── page.tsx      # /about
├── (shop)/
│   ├── layout.tsx
│   ├── page.tsx          # / (shop home - same URL as marketing!)
│   └── products/
│       └── page.tsx      # /products
└── (auth)/
    ├── layout.tsx
    └── login/
        └── page.tsx      # /login
```

Note: Route groups help organize code without changing URLs. Only one route group can match a given path.

### Parallel Routes
Render multiple pages in the same layout using slots:

```bash
# [File: app/dashboard/ directory]
app/dashboard/
├── layout.tsx
├── page.tsx              # Main dashboard content
├── @analytics/
│   └── page.tsx          # Appears in <slot name="analytics" />
└── @notifications/
    └── page.tsx          # Appears in <slot name="notifications" />
```

```tsx
// [File: app/dashboard/layout.tsx]
export default function DashboardLayout({
  children, // Main page content
  analytics, // @analytics/page content
  notifications, // @notifications/page content
}: {
  children: React.ReactNode;
  analytics: React.ReactNode;
  notifications: React.ReactNode;
}) {
  return (
    <div className="dashboard">
      <aside>
        <h3>Analytics</h3>
        {analytics}
      </aside>
      <main>
        <h2>Dashboard</h2>
        {children}
      </main>
      <sidebar>
        <h3>Notifications</h3>
        {notifications}
      </sidebar>
    </div>
  );
}
```

### Intercepting Routes
Intercepting routes allow you to show a route from another section in a modal or overlay:

```bash
# [File: app/photos/ directory]
app/photos/
├── layout.tsx
├── page.tsx              # /photos
├── [id]/
│   ├── page.tsx          # /photos/[id]
│   └── (.)edit/
│       └── page.tsx      # /photos/[id]/edit (but shows in modal)
```

```tsx
// [File: app/photos/[id]/(.)edit/page.tsx]
export default function EditPhotoModal() {
  return (
    <div className="modal">
      <h2>Edit Photo</h2>
      {/* Edit form */}
    </div>
  );
}

// The (.) syntax means "same level" - it intercepts the route
// but doesn't change the URL, perfect for modals
```

## Common Mistakes

### Mistake 1: Forgetting 'use client' for Interactive Components
```tsx
// ❌ WRONG - Missing 'use client'
import { useState } from 'react';

export default function Counter() {
  const [count, setCount] = useState(0);
  
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}

// ✅ CORRECT - Add 'use client'
'use client';

import { useState } from 'react';

export default function Counter() {
  const [count, setCount] = useState(0);
  
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}
```

### Mistake 2: Doing Expensive Computations in Server Components
```tsx
// ❌ WRONG - Expensive operation in Server Component
export default async function Page() {
  // This blocks the server for every request!
  const data = await expensiveComputation();
  
  return <div>{data}</div>;
}

// ✅ CORRECT - Move to Client Component or use caching
'use client';
import { useState, useEffect } from 'react';

export default function Page() {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    expensiveComputation().then(setData);
  }, []);
  
  return <div>{data}</div>;
}
```

### Mistake 3: Not Using Loading.js for Suspense
```tsx
// ❌ WRONG - No loading state, shows blank screen
export default async function Page() {
  const data = await fetchData(); // Might be slow
  
  return <div>{data.title}</div>;
}

// ✅ CORRECT - Automatic loading state with loading.tsx
// Create app/dashboard/loading.tsx for automatic Suspense boundary
```

## Real-World Example

Complete blog example with App Router features:

```tsx
// [File: app/layout.tsx] - Root layout with metadata
import './globals.css';
import { Inter } from 'next/font/google';
import { Navbar } from '@/components/navbar';

const inter = Inter({ subsets: ['latin'] });

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className={inter.className}>
        <Navbar />
        {children}
      </body>
    </html>
  );
}
```

```tsx
// [File: app/blog/layout.tsx] - Blog section layout
export default function BlogLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <section className="blog-section">
      <header>
        <h1>Blog</h1>
        <nav>
          <Link href="/blog">All Posts</Link>
          <Link href="/blog/about">About</Link>
        </nav>
      </header>
      <main>{children}</main>
    </section>
  );
}
```

```tsx
// [File: app/blog/page.tsx] - Blog index (Server Component)
import { getAllPosts } from '@/lib/api';
import { PostPreview } from '@/components/post-preview';

export default async function BlogPage() {
  const posts = await getAllPosts();
  
  return (
    <div className="blog-posts">
      <h2>Latest Posts</h2>
      {posts.length > 0 ? (
        <ul>
          {posts.map(post => (
            <li key={post.id}>
              <PostPreview post={post} />
            </li>
          ))}
        </ul>
      ) : (
        <p>No posts yet.</p>
      )}
    </div>
  );
}
```

```tsx
// [File: app/blog/loading.tsx] - Automatic loading state
export default function BlogLoading() {
  return (
    <div className="blog-loading">
      <h2>Loading posts...</p>
    </div>
  );
}
```

```tsx
// [File: app/blog/[slug]/page.tsx] - Individual post (Server Component)
import { getPostBySlug } from '@/lib/api';
import { Markdown } from '@/components/markdown';

export default async function PostPage({
  params,
}: {
  params: { slug: string };
}) {
  const post = await getPostBySlug(params.slug);
  
  if (!post) {
    // This will trigger the not-found.js file
    return notFound();
  }
  
  return (
    <article className="blog-post">
      <header>
        <h1>{post.title}</h1>
        <p className="meta">
          By {post.author} on {new Date(post.date).toLocaleDateString()}
        </p>
      </header>
      <Markdown content={post.content} />
    </article>
  );
}
```

```tsx
// [File: app/blog/[slug]/loading.tsx] - Post loading state
export default function PostLoading() {
  return (
    <div className="post-loading">
      <h2>Loading post...</h2>
    </div>
  );
}
```

```tsx
// [File: app/blog/[slug]/not-found.tsx] - Custom 404 for blog
export default function BlogNotFound() {
  return (
    <section className="not-found">
      <h2>Post not found</h2>
      <p>The post you're looking for doesn't exist.</p>
      <a href="/blog">Return to blog</a>
    </section>
  );
}
```

```tsx
// [File: app/components/post-preview.tsx] - Client Component for interactivity
'use client';

import { useState } from 'react';
import { Link } from 'next/link';

interface PostPreviewProps {
  post: {
    id: string;
    title: string;
    excerpt: string;
    date: string;
  };
}

export default function PostPreview({ post }: PostPreviewProps) {
  const [hovered, setHovered] = useState(false);
  
  return (
    <Link href={`/blog/${post.id}`} className="post-preview">
      <div
        onMouseEnter={() => setHovered(true)}
        onMouseLeave={() => setHovered(false)}
        className={hovered ? 'preview hovered' : 'preview'}
      >
        <h3>{post.title}</h3>
        <p>{post.excerpt}</p>
        <time>{new Date(post.date).toLocaleDateString()}</time>
      </div>
    </Link>
  );
}
```

```tsx
// [File: app/api/route.ts] - API route for fetching posts
import { NextResponse } from 'next/server';
import { prisma } from '@/lib/prisma';

export async function GET() {
  try {
    const posts = await prisma.post.findMany({
      select: {
        id: true,
        title: true,
        excerpt: true,
        date: true,
        author: true,
      },
      orderBy: { date: 'desc' },
    });
    
    return NextResponse.json(posts);
  } catch (error) {
    return NextResponse.json(
      { error: 'Failed to fetch posts' },
      { status: 500 }
    );
  }
}
```

## Key Takeaways
- App Router uses `app/` directory with file-based routing
- Components are Server Components by default (zero JS bundle for data)
- Use `'use client'` for interactivity, state, and effects
- Layouts nest automatically based on file structure
- Special files: `layout.tsx`, `page.tsx`, `loading.tsx`, `error.tsx`, `not-found.tsx`
- Route groups `()` organize without affecting URLs
- Parallel routes `@slot` enable multiple views in same layout
- Intercepting routes `(.)` and `(..)` enable modal-like navigation
- Automatic code splitting and prefetching with `next/link`
- Edge Runtime support for API routes and middleware

## What's Next
Continue to [File-Based Routing in Depth](03-file-based-routing-in-depth.md) to learn about dynamic segments, route groups, and advanced routing patterns.