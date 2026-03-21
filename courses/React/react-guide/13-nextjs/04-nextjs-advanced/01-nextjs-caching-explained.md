# Next.js Caching Explained

## Overview
Next.js implements a sophisticated multi-layer caching system that makes it incredibly fast. Understanding these four caching layers is essential for building performant applications and debugging caching-related issues.

## Prerequisites
- Next.js App Router fundamentals
- Data fetching with fetch
- Server and Client Components

## Core Concepts

### The Four Caching Layers

Next.js caches at four distinct layers. Each has different behavior and use cases.

```
┌─────────────────────────────────────────────────────────────┐
│                    Next.js Caching Layers                    │
├─────────────────────────────────────────────────────────────┤
│  1. Request Memoization      (Same request, same component)  │
│  2. Data Cache              (Persistent across requests)     │
│  3. Full Route Cache        (Static HTML + JSON)           │
│  4. Router Cache            (Client-side prefetch)         │
└─────────────────────────────────────────────────────────────┘
```

### Layer 1: Request Memoization

When the same fetch request is made multiple times in a single render pass, Next.js only makes one network request.

```tsx
// [File: app/components/PostList.tsx]
/**
 * This component calls fetchPosts() multiple times.
 * Without memoization, we'd make 3 network requests.
 * With memoization, only 1 request is made.
 */
async function fetchPosts() {
  'use server';
  const res = await fetch('https://api.example.com/posts');
  return res.json();
}

export default async function PostList() {
  // First call - makes network request
  const posts = await fetchPosts();
  
  // Second call - uses memoized result (no network request)
  const featured = await fetchPosts();
  
  // Third call - still uses memoized result
  const recent = await fetchPosts();
  
  return (
    <div>
      {/* All three use the same cached data */}
    </div>
  );
}
```

**Key points:**
- Only active during a single render pass
- Automatically applied to all fetch calls
- Resets when the component tree completes rendering

### Layer 2: Data Cache

The Data Cache persists fetched data across server requests. It's the foundation for Next.js's speed.

```tsx
// [File: app/posts/page.tsx]
/**
 * Data Cache Behavior:
 * - First request: Fetch from API, store in cache
 * - Subsequent requests: Return cached data (instant!)
 * 
 * Default: Cache forever until revalidated
 */
async function getPosts() {
  const res = await fetch('https://api.example.com/posts', {
    // Cache for 1 hour (3600 seconds)
    next: { revalidate: 3600 }
  });
  return res.json();
}

export default async function PostsPage() {
  const posts = await getPosts();
  
  return (
    <ul>
      {posts.map(post => (
        <li key={post.id}>{post.title}</li>
      ))}
    </ul>
  );
}
```

**Cache options:**

| Option | Behavior |
|--------|----------|
| `{ next: { revalidate: 0 } }` | No cache - always fresh |
| `{ next: { revalidate: 3600 } }` | Revalidate after 1 hour |
| `{ next: { tags: ['posts'] } }` | Cache with tag for manual invalidation |
| Default | Cache indefinitely |

```tsx
// Tag-based revalidation example
async function getPosts() {
  return fetch('https://api.example.com/posts', {
    next: { tags: ['posts'] }  // Tag this request
  });
}

// Later, invalidate by tag (in Server Action or API route)
import { revalidateTag } from 'next/cache';

export async function createPost(data: PostData) {
  await db.post.create({ data });
  revalidateTag('posts');  // Clear cached posts data
}
```

### Layer 3: Full Route Cache (Static Generation)

This layer stores the rendered HTML and JSON for entire routes. It's what makes static sites so fast.

```tsx
// [File: app/products/page.tsx]
/**
 * This page is statically generated at build time.
 * The HTML is cached and served from CDN.
 * 
 * On first build: Server renders and caches
 * Subsequent visits: Served from cache (instant)
 */
import { getProducts } from '@/lib/api';

export const dynamic = 'force-dynamic'; // Opt out of static generation
// OR
export const revalidate = 3600; // Revalidate every hour

export default async function ProductsPage() {
  const products = await getProducts();
  
  return (
    <div className="grid">
      {products.map(product => (
        <ProductCard key={product.id} product={product} />
      ))}
    </div>
  );
}
```

**Static vs Dynamic:**

| Mode | Cache Behavior |
|------|-----------------|
| Static (default) | Cached forever, rebuilt on deploy |
| ISR (revalidate) | Cached, rebuilt in background after timeout |
| Dynamic | Not cached, rendered per request |

```tsx
// Incremental Static Regeneration (ISR)
export const revalidate = 60;  // Rebuild page every 60 seconds
// First visitor gets cached page
// After 60s, next visitor triggers background rebuild
// Subsequent visitors get new cached version
```

### Layer 4: Router Cache (Client-Side)

The Router Cache stores visited routes in the browser, enabling instant back navigation.

```tsx
// [File: app/layout.tsx]
/**
 * Router Cache happens on the client.
 * When user navigates to a page, it's stored in browser cache.
 * 
 * - Forward/Back navigation is instant
 * - Prefetching loads pages before click
 * - Soft navigation (Link) vs hard navigation (window.location)
 */
import Link from 'next/link';

export default function Layout({ children }) {
  return (
    <>
      <nav>
        {/* Using Link enables client-side routing with prefetching */}
        <Link href="/products">Products</Link>
        <Link href="/about">About</Link>
      </nav>
      {children}
    </>
  );
}
```

**Prefetching behavior:**
- viewport prefetch: Loads when link enters viewport (default)
- hover prefetch: Loads when user hovers over link

```tsx
// Disable prefetching (useful for very large pages)
<Link href="/heavy-page" prefetch={false}>
  Heavy Page
</Link>
```

## Fetch Cache Options

```tsx
// [File: lib/api.ts]
/**
 * Complete fetch options reference
 */

// 1. No caching - always fresh
fetch('https://api.example.com/data', { cache: 'no-store' });

// 2. Static cache - cached indefinitely  
fetch('https://api.example.com/data', { cache: 'force-cache' });

// 3. Revalidate after time
fetch('https://api.example.com/data', { 
  next: { revalidate: 3600 }
});

// 4. Revalidate by tag
fetch('https://api.example.com/data', {
  next: { tags: ['my-data'] }
});

// 5. Force revalidate (bypass cache for this request)
fetch('https://api.example.com/data', { 
  next: { revalidate: 0 }
});
```

## Cache Debugging

Use Next.js built-in tools to understand caching behavior.

```tsx
// [File: app/api/revalidate/route.ts]
/**
 * API route to manually revalidate cache
 */
import { revalidatePath, revalidateTag } from 'next/cache';
import { NextRequest, NextResponse } from 'next/server';

export async function POST(request: NextRequest) {
  const secret = request.headers.get('x-secret');
  
  if (secret !== process.env.REVALIDATION_SECRET) {
    return NextResponse.json({ error: 'Unauthorized' }, { status: 401 });
  }
  
  const body = await request.json();
  
  if (body.type === 'path') {
    revalidatePath(body.path);
    return NextResponse.json({ revalidated: true, path: body.path });
  }
  
  if (body.type === 'tag') {
    revalidateTag(body.tag);
    return NextResponse.json({ revalidated: true, tag: body.tag });
  }
  
  return NextResponse.json({ error: 'Invalid type' });
}
```

## Common Mistakes

### ❌ Forgetting to Revalidate / ✅ Fix

```tsx
// ❌ WRONG - Data never updates
async function getData() {
  return fetch('/api/data'); // Cached forever!
}

// ✅ CORRECT - Add revalidation
async function getData() {
  return fetch('/api/data', { next: { revalidate: 60 } });
}
```

### ❌ Mixing Cache Strategies / ✅ Fix

```tsx
// ❌ WRONG - Trying to use stale data for personalized content
async function getData() {
  return fetch('/api/user', { cache: 'force-cache' });
  // This will show wrong user data!
}

// ✅ CORRECT - Opt out of cache for user-specific data
async function getUserData() {
  return fetch('/api/user', { 
    cache: 'no-store',  // Always fresh
    credentials: 'include'  // Send cookies
  });
}
```

### ❌ Not Using Tags / ✅ Fix

```tsx
// ❌ WRONG - Revalidating entire path when only one item changed
export async function updatePost(id: string, data: PostData) {
  await db.post.update({ where: { id }, data });
  revalidatePath('/posts'); // Invalidates ALL posts!
}

// ✅ CORRECT - Use tags for granular invalidation
async function getPosts() {
  return fetch('/api/posts', { next: { tags: ['posts'] } });
}

export async function updatePost(id: string, data: PostData) {
  await db.post.update({ where: { id }, data });
  revalidateTag('posts'); // Only invalidates posts cache
}
```

## Real-World Example: Blog with Caching

Complete example showing all caching layers working together.

```tsx
// [File: lib/posts.ts]
const API_URL = 'https://api.example.com/posts';

/**
 * Get all posts - cached for 1 hour
 * This uses Data Cache + Full Route Cache
 */
export async function getPosts() {
  const res = await fetch(API_URL, {
    next: { revalidate: 3600, tags: ['posts'] }
  });
  
  if (!res.ok) throw new Error('Failed to fetch posts');
  return res.json();
}

/**
 * Get single post - cached with tag for targeted invalidation
 */
export async function getPost(slug: string) {
  const res = await fetch(`${API_URL}/${slug}`, {
    next: { tags: [`post-${slug}`] }
  });
  
  if (!res.ok) throw new Error('Failed to fetch post');
  return res.json();
}
```

```tsx
// [File: app/actions.ts]
'use server';

import { revalidateTag } from 'next/cache';
import { redirect } from 'next/navigation';

/**
 * Server Action to create a new post.
 * Revalidates cache after creating.
 */
export async function createPost(formData: FormData) {
  const title = formData.get('title') as string;
  const content = formData.get('content') as string;
  
  // Create in database
  const post = await db.post.create({
    data: { title, content, slug: title.toLowerCase().replace(/\s+/g, '-') }
  });
  
  // Invalidate caches - data will be refetched on next request
  revalidateTag('posts');
  revalidateTag(`post-${post.slug}`);
  
  redirect(`/posts/${post.slug}`);
}
```

```tsx
// [File: app/posts/page.tsx]
/**
 * Posts listing page.
 * Leverages Data Cache for fast initial render.
 * Uses Request Memoization if called multiple times.
 */
import { getPosts } from '@/lib/posts';
import { PostCard } from '@/components/PostCard';

export default async function PostsPage() {
  // This fetch is cached for 1 hour
  // Multiple calls in same render use memoization
  const posts = await getPosts();
  
  return (
    <main>
      <h1>Blog Posts</h1>
      <div className="grid">
        {posts.map(post => (
          <PostCard key={post.id} post={post} />
        ))}
      </div>
    </main>
  );
}
```

## Cache Debugging Flowchart

```
Is data not updating?
    │
    ├─► YES: Check fetch options
    │       ├─► cache: 'force-cache'? → Add revalidate or use cache: 'no-store'
    │       └─► next: { revalidate: X }? → Wait for timeout or revalidate manually
    │
    └─► NO: Check full route cache
            ├─► Page static? → Add dynamic = 'force-dynamic' or revalidate: 0
            └─► Page dynamic? → This is expected behavior
```

## Key Takeaways

- **Request Memoization**: Same-fetch optimization within one render pass
- **Data Cache**: Persists across requests, controlled by fetch options
- **Full Route Cache**: Stores rendered HTML, enables static generation
- **Router Cache**: Client-side prefetching and navigation caching
- Use `{ next: { revalidate: N } }` for time-based revalidation
- Use `{ next: { tags: ['name'] } }` + `revalidateTag()` for manual invalidation
- Use `{ cache: 'no-store' }` for dynamic, user-specific data

## What's Next

Now that you understand Next.js caching, continue to [Docker Setup](18-ecosystem/04-docker/01-docker-basics.md