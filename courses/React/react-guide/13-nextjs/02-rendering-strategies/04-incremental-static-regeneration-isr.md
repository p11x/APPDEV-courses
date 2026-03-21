# Incremental Static Regeneration (ISR)

## Overview
Incremental Static Regeneration (ISR) is a powerful hybrid rendering strategy that combines the performance of static generation with the freshness of server-side rendering. ISR allows you to update static content after deployment without requiring a full rebuild. When a page is requested, Next.js serves the cached version while regenerating it in the background for subsequent requests. This provides the best of both worlds: fast static page delivery with automatic content updates.

## Prerequisites
- Understanding of Static Generation (SSG)
- Familiarity with Server Components
- Knowledge of caching concepts

## Core Concepts

### How ISR Works
ISR creates a hybrid approach between SSG and SSR:

```tsx
// [File: app/blog/[slug]/page.tsx]
import { getPostBySlug } from '@/lib/api';

export async function generateStaticParams() {
  const slugs = await getAllPostSlugs();
  return slugs.map(slug => ({ slug }));
}

// ISR: Regenerate this page at most once per 60 seconds
export const revalidate = 60;

export default async function PostPage({ params }) {
  const post = await getPostBySlug(params.slug);
  
  return (
    <article>
      <h1>{post.title}</h1>
      <div dangerouslySetInnerHTML={{ __html: post.content }} />
    </article>
  );
}

// Flow:
// 1. First request: Generate HTML, cache it, serve it
// 2. Subsequent requests (within 60s): Serve cached HTML instantly
// 3. After 60s: Serve stale cache, regenerate in background
// 4. Next request: Serve newly generated HTML
```

### On-Demand Revalidation
Trigger revalidation programmatically when content changes:

```tsx
// [File: app/api/revalidate/route.ts]
import { NextRequest, NextResponse } from 'next/server';
import { revalidatePath, revalidateTag } from 'next/cache';

export async function POST(request: NextRequest) {
  const secret = request.headers.get('x-revalidate-token');
  
  if (secret !== process.env.REVALIDATE_SECRET) {
    return NextResponse.json({ message: 'Invalid token' }, { status: 401 });
  }
  
  const body = await request.json();
  
  // Option 1: Revalidate by path
  if (body.path) {
    revalidatePath(body.path);
    return NextResponse.json({ revalidated: true, path: body.path });
  }
  
  // Option 2: Revalidate by cache tag
  if (body.tag) {
    revalidateTag(body.tag);
    return NextResponse.json({ revalidated: true, tag: body.tag });
  }
  
  return NextResponse.json({ message: 'Missing path or tag' }, { status: 400 });
}
```

### Using Cache Tags
Organize cached content with tags for targeted revalidation:

```tsx
// [File: app/blog/page.tsx]
import { getPosts } from '@/lib/api';

export const revalidate = 3600; // Default: revalidate every hour
export const fetchCache = 'force-cache';

export default async function BlogPage() {
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

```tsx
// [File: lib/api.ts]
export async function getPosts() {
  const res = await fetch('https://api.example.com/posts', {
    next: { 
      tags: ['posts'], // Tag this fetch for revalidation
      revalidate: 3600 
    }
  });
  
  if (!res.ok) throw new Error('Failed to fetch posts');
  return res.json();
}

// In your CMS or admin panel, when a post is published:
// POST /api/revalidate { tag: 'posts' }
```

### Revalidation Strategies

| Strategy | Use Case | Example |
|----------|----------|---------|
| Time-based | Regular content updates | Blog posts, product catalogs |
| On-demand | CMS-triggered updates | Content edits, inventory changes |
| Path-based | Specific page updates | Single blog post |
| Tag-based | Bulk content updates | All blog posts, all products |

## Common Mistakes

### Mistake 1: Setting revalidate Too Low
```tsx
// ❌ WRONG - Revalidating every second defeats the purpose
export const revalidate = 1;

// ✅ CORRECT - Balance between freshness and performance
export const revalidate = 60; // 1 minute
export const revalidate = 3600; // 1 hour (for less critical content)
```

### Mistake 2: Not Handling Stale Data
```tsx
// ❌ WRONG - Assuming data is always fresh
export default async function Page() {
  const data = await getData(); // Might return stale cache
  
  return <div>{data.value}</div>; // No fallback for stale state
}

// ✅ CORRECT - Use Suspense for graceful loading
import { Suspense } from 'react';

export default async function Page() {
  return (
    <Suspense fallback={<Skeleton />}>
      <DataContent />
    </Suspense>
  );
}
```

## Real-World Example

Complete news site with ISR:

```tsx
// [File: app/news/page.tsx]
import { getArticles } from '@/lib/api';

export const revalidate = 300; // 5 minutes

export default async function NewsPage() {
  const articles = await getArticles();
  
  return (
    <main>
      <h1>Latest News</h1>
      <div className="grid">
        {articles.map(article => (
          <ArticleCard key={article.id} article={article} />
        ))}
      </div>
    </main>
  );
}
```

```tsx
// [File: app/news/[category]/page.tsx]
import { getArticlesByCategory } from '@/lib/api';

export const revalidate = 300;

export default async function CategoryPage({ params }) {
  const articles = await getArticlesByCategory(params.category);
  
  return (
    <main>
      <h1>{params.category} News</h1>
      <ArticleList articles={articles} />
    </main>
  );
}
```

```tsx
// [File: app/api/publish/route.ts]
import { NextRequest, NextResponse } from 'next/server';
import { revalidatePath, revalidateTag } from 'next/cache';

export async function POST(request: NextRequest) {
  // Verify webhook signature
  const signature = request.headers.get('x-cms-signature');
  if (!verifyWebhook(signature, request.body)) {
    return NextResponse.json({ error: 'Invalid signature' }, { status: 401 });
  }
  
  const body = await request.json();
  
  // Revalidate based on content type
  if (body.type === 'article') {
    revalidatePath(`/news/${body.category}`);
    revalidatePath('/news');
    revalidateTag('articles');
  }
  
  return NextResponse.json({ revalidated: true });
}
```

## Key Takeaways
- ISR combines static performance with dynamic freshness
- Set `revalidate` to control update frequency
- Use on-demand revalidation for instant updates when content changes
- Use cache tags for organized, targeted revalidation
- Balance revalidation time based on content update frequency
- ISR is ideal for blogs, news sites, e-commerce catalogs

## What's Next
Continue to [Next.js API Routes](03-nextjs-features/01-nextjs-api-routes.md) to learn about creating backend API endpoints in Next.js.