# File-Based Routing in Depth

## Overview
Next.js App Router provides a powerful file-based routing system that goes beyond simple static routes. It supports dynamic segments, catch-all routes, optional catch-all routes, and advanced patterns like route groups and parallel routes. This guide dives deep into the various routing patterns available in Next.js, with practical examples for building complex navigation structures.

## Prerequisites
- Understanding of Next.js App Router basics
- Familiarity with the `app/` directory structure
- Knowledge of special files like `layout.tsx` and `page.tsx`

## Core Concepts

### Dynamic Segments
Dynamic segments allow you to create routes that match multiple URLs based on a parameter:

```bash
# [File: app/ directory structure]
app/
├── blog/
│   ├── layout.tsx
│   ├── page.tsx          # /blog
│   └── [slug]/
│       └── page.tsx      # /blog/:slug (e.g., /blog/my-first-post)
├── products/
│   ├── layout.tsx
│   ├── page.tsx          # /products
│   └── [category]/
│       ├── layout.tsx
│       ├── page.tsx      # /products/:category
│       └── [id]/
│           └── page.tsx  # /products/:category/:id
└── users/
    ├── layout.tsx
    ├── page.tsx          # /users
    └── [userId]/
        └── page.tsx      # /users/:userId
```

```tsx
// [File: app/blog/[slug]/page.tsx]
import { getPostBySlug } from '@/lib/api';
import { notFound } from 'next/navigation';

export default async function BlogPostPage({
  params,
}: {
  params: { slug: string };
}) {
  const post = await getPostBySlug(params.slug);
  
  if (!post) {
    // 404 if post not found
    notFound();
  }
  
  return (
    <article>
      <h1>{post.title}</h1>
      <div dangerouslySetInnerHTML={{ __html: post.content }} />
    </article>
  );
}
```

### Catch-All Segments
Catch-all segments match all remaining paths using `...` (three dots):

```bash
# [File: app/ directory structure]
app/
├── docs/
│   ├── layout.tsx
│   ├── page.tsx          # /docs
│   └── [...slug]/
│       └── page.tsx      # Matches /docs/* (e.g., /docs/getting-started, /docs/api/auth)
└── shop/
    ├── layout.tsx
    ├── page.tsx          # /shop
    └── [...path]/
        └── page.tsx      # Matches /shop/* (e.g., /shop/electronics/phones/iphone)
```

```tsx
// [File: app/docs/[...slug]/page.tsx]
import { getDocByPath } from '@/lib/api';
import { notFound } from 'next/navigation';

export default async function DocPage({
  params,
}: {
  params: { slug: string[] }; // Note: slug is an array!
}) {
  const path = params.slug.join('/'); // Convert array to path
  const doc = await getDocByPath(path);
  
  if (!doc) {
    notFound();
  }
  
  return (
    <article>
      <h1>{doc.title}</h1>
      <div dangerouslySetInnerHTML={{ __html: doc.content }} />
    </article>
  );
}
```

### Optional Catch-All Segments
Optional catch-all segments use double brackets `[[...slug]]` and can match zero or more segments:

```bash
# [File: app/ directory structure]
app/
├── shop/
│   ├── layout.tsx
│   ├── page.tsx          # /shop (matches zero segments)
│   └── [[...category]]/
│       ├── layout.tsx
│       ├── page.tsx      # Matches /shop/* (including zero segments)
│       └── [productId]/
│           └── page.tsx  # Matches /shop/*/:productId
└── blog/
    ├── layout.tsx
    ├── page.tsx          # /blog
    └── [[...slug]]/
        └── page.tsx      # Matches /blog/* (including zero segments)
```

```tsx
// [File: app/shop/[[...category]]/page.tsx]
import { getCategoryProducts } from '@/lib/api';

export default async function ShopCategoryPage({
  params,
}: {
  params: { category: string[] | undefined }; // Can be undefined!
}) {
  // Handle the case where category is undefined (zero segments)
  const categoryPath = params.category ? params.category.join('/') : '';
  
  const products = await getCategoryProducts(categoryPath);
  
  return (
    <section>
      <h1>Shop {categoryPath || 'All Products'}</h1>
      <div className="products-grid">
        {products.map(product => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </section>
  );
}
```

### Route Groups
Route groups allow you to organize routes without affecting the URL path using parentheses:

```bash
# [File: app/ directory structure]
app/
├── (marketing)/
│   ├── layout.tsx
│   ├── page.tsx          # / (marketing home)
│   ├── about/
│   │   └── page.tsx      # /about
│   └── blog/
│       ├── layout.tsx
│       ├── page.tsx      # /blog
│       └── [slug]/
│           └── page.tsx  # /blog/[slug]
├── (shop)/
│   ├── layout.tsx
│   ├── page.tsx          # / (shop home - same URL as marketing!)
│   └── products/
│       ├── layout.tsx
│       ├── page.tsx      # /products
│       └── [id]/
│           └── page.tsx  # /products/[id]
└── (auth)/
    ├── layout.tsx
    ├── sign-in/
    │   └── page.tsx      # /sign-in
    └── sign-up/
        └── page.tsx      # /sign-up
```

Note: Only one route group can match a given path. The order of route groups matters (first match wins).

### Parallel Routes
Parallel routes allow you to render multiple pages in the same layout using the `@` convention:

```bash
# [File: app/ directory structure]
app/
├── dashboard/
│   ├── layout.tsx
│   ├── page.tsx              # Main dashboard content
│   ├── @analytics/
│   │   └── page.tsx          # Appears in <slot name="analytics" />
│   ├── @notifications/
│   │   └── page.tsx          # Appears in <slot name="notifications" />
│   └── @settings/
│       └── page.tsx          # Appears in <slot name="settings" />
└── (marketing)/
    └── ... 
```

```tsx
// [File: app/dashboard/layout.tsx]
export default function DashboardLayout({
  children, // Main page content (from dashboard/page.tsx)
  analytics, // @analytics/page content
  notifications, // @notifications/page content
  settings, // @settings/page content
}: {
  children: React.ReactNode;
  analytics: React.ReactNode;
  notifications: React.ReactNode;
  settings: React.ReactNode;
}) {
  return (
    <div className="dashboard">
      <aside className="analytics">
        <h2>Analytics</h2>
        {analytics}
      </aside>
      <main className="main">
        <h2>Dashboard</h2>
        {children}
      </main>
      <sidebar className="sidebar">
        <h2>Notifications</h2>
        {notifications}
        <h2>Settings</h2>
        {settings}
      </sidebar>
    </div>
  );
}
```

### Intercepting Routes
Intercepting routes allow you to intercept a route from another section and show it in a modal or overlay using `(.)` and `(..)`:

```bash
# [File: app/ directory structure]
app/
├── photos/
│   ├── layout.tsx
│   ├── page.tsx              # /photos
│   └── [id]/
│       ├── page.tsx          # /photos/[id]
│       ├── (.)edit/
│       │   └── page.tsx      # /photos/[id]/edit (but shows in modal)
│       └── (..)share/
│           └── page.tsx      # /photos/[id]/share (but shows in modal from parent)
└── shared/
    └── edit-photo/
        └── page.tsx          # /shared/edit-photo (actual route)
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

// The (.) means "same level" - it intercepts the route from the same directory level
// The (..) means "parent level" - it intercepts from the parent directory
```

### Loading.js and Error.js
Automatic loading and error boundaries for Server Components:

```bash
# [File: app/ directory structure]
app/
├── dashboard/
│   ├── layout.tsx
│   ├── page.tsx
│   ├── loading.tsx      # Shows while dashboard/page.tsx is loading
│   ├── error.tsx        # Shows if dashboard/page.tsx throws an error
│   └── analytics/
│       ├── page.tsx
│       ├── loading.tsx  # Shows while analytics/page.tsx is loading
│       └── error.tsx    # Shows if analytics/page.tsx throws an error
```

```tsx
// [File: app/dashboard/loading.tsx]
export default function DashboardLoading() {
  return (
    <div className="dashboard-loading">
      <h2>Loading dashboard...</h2>
    </div>
  );
}
```

```tsx
// [File: app/dashboard/error.tsx]
export default function DashboardError({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  return (
    <div className="dashboard-error">
      <h2>Something went wrong</h2>
      <p>{error.message}</p>
      <button onClick={reset}>Try again</button>
    </div>
  );
}
```

### Template Files
Template files (`template.tsx`) are similar to layouts but don't preserve state between navigations:

```bash
# [File: app/ directory structure]
app/
├── dashboard/
│   ├── layout.tsx      # Persists state (e.g., sidebar state)
│   ├── template.tsx    # Does NOT persist state (re-mounted on navigation)
│   └── page.tsx
```

```tsx
// [File: app/dashboard/template.tsx]
export default function DashboardTemplate({
  children,
}: {
  children: React.ReactNode;
}) {
  // This component re-mounts on every navigation
  // Use for things like animation entry/exit
  return (
    <div className="dashboard-template">
      {children}
    </div>
  );
}
```

### Default.js
Default files (`default.tsx`) are used for parallel routes when a specific slot is not provided:

```bash
# [File: app/ directory structure]
app/
├── dashboard/
│   ├── layout.tsx
│   ├── page.tsx
│   ├── @analytics/
│   │   └── page.tsx
│   └── @notifications/
│       └── page.tsx
└── (shop)/
    ├── layout.tsx
    ├── page.tsx
    ├── @sidebar/
    │   └── page.tsx
    └── @default/
        └── default.tsx  # Shown when @sidebar is not provided
```

```tsx
// [File: app/shop/@default/default.tsx]
export default function ShopDefault() {
  return (
    <div className="shop-default">
      <h2>Welcome to our shop!</h2>
      <p>Select a category from the sidebar to get started.</p>
    </div>
  );
}
```

## Common Mistakes

### Mistake 1: Confusing Catch-All and Optional Catch-All
```typescript
// ❌ WRONG - Using [...slug] when you want to match zero segments
// [...slug] requires at least one segment

// ✅ CORRECT - Use [[...slug]] for zero or more segments
```

### Mist 2: Forgetting that Dynamic Segment Params Are Arrays for Catch-All
```typescript
// ❌ WRONG - Assuming params.slug is a string
export default function Page({ params }: { params: { slug: string } }) {
  // Error when using [...slug] or [[...slug]]
}

// ✅ CORRECT - For catch-all, params.slug is string[]
export default function Page({ params }: { params: { slug: string[] } }) {
  const path = params.slug.join('/');
}
```

### Mistake 3: Not Handling Undefined in Optional Catch-All
```typescript
// ❌ WRONG - Not checking for undefined
export default function Page({ params }: { params: { category: string[] } }) {
  // Will error when category is undefined (zero segments)
}

// ✅ CORRECT - Check for undefined
export default function Page({ params }: { params: { category: string[] | undefined } }) {
  const path = params.category ? params.category.join('/') : '';
}
```

## Real-World Example

Complete e-commerce routing example:

```bash
# [File: app/ directory structure]
app/
├── layout.tsx              # Root layout
├── page.tsx                # / (home)
├── about/
│   ├── layout.tsx
│   └── page.tsx            # /about
├── shop/
│   ├── layout.tsx
│   ├── page.tsx            # /shop
│   ├── [[...category]]/
│   │   ├── layout.tsx
│   │   ├── page.tsx        # /shop/* (category index)
│   │   └── [productId]/
│   │       ├── page.tsx    # /shop/*/:productId
│   │       └── actions/
│   │           ├── (.)buy/
│   │           │   └── page.tsx  # /shop/*/:productId/actions/buy (modal)
│   │           └── (.)save/
│   │               └── page.tsx  # /shop/*/:productId/actions/save (modal)
│   └── cart/
│       ├── layout.tsx
│       ├── page.tsx        # /shop/cart
│       └── [itemId]/
│           └── page.tsx    # /shop/cart/:itemId
├── blog/
│   ├── layout.tsx
│   ├── page.tsx            # /blog
│   └── [slug]/
│       ├── page.tsx        # /blog/[slug]
│       └── [...tags]/
│           └── page.tsx    # /blog/[slug]/tags/* (e.g., /blog/post/tags/react)
├── (auth)/
│   ├── layout.tsx
│   ├── sign-in/
│   │   └── page.tsx        # /sign-in
│   └── sign-up/
│       └── page.tsx        # /sign-up
└── dashboard/
    ├── layout.tsx
    ├── page.tsx            # /dashboard
    ├── @analytics/
    │   └── page.tsx        # Analytics panel
    ├── @notifications/
    │   └── page.tsx        # Notifications panel
    └── settings/
        ├── layout.tsx
        ├── page.tsx        # /dashboard/settings
        └── [section]/
            └── page.tsx    # /dashboard/settings/[section]
```

```tsx
// [File: app/shop/[[...category]]/page.tsx]
import { getCategory, getProductsByCategory } from '@/lib/api';

export default async function ShopCategoryPage({
  params,
}: {
  params: { category: string[] | undefined };
}) {
  const categoryPath = params.category ? params.category.join('/') : '';
  
  // Get category metadata
  const category = await getCategory(categoryPath);
  
  // Get products in this category
  const products = await getProductsByCategory(categoryPath);
  
  if (!category && categoryPath !== '') {
    // Category not found but we have a path
    notFound();
  }
  
  return (
    <section className="shop-category">
      <header>
        <h1>{category?.name || 'All Products'}</h1>
        {category?.description && <p>{category.description}</p>}
      </header>
      
      <div className="products-grid">
        {products.map(product => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </section>
  );
}
```

```tsx
// [File: app/shop/[[...category]]/[productId]/page.tsx]
import { getProductById } from '@/lib/api';
import { notFound } from 'next/navigation';

export default async function ShopProductPage({
  params,
}: {
  params: { category: string[] | undefined; productId: string };
}) {
  const product = await getProductById(params.productId);
  
  if (!product) {
    notFound();
  }
  
  return (
    <div className="product-detail">
      <h1>{product.name}</h1>
      <p>{product.description}</p>
      <p className="price">${product.price}</p>
      
      <div className="product-actions">
        <button className="buy-btn">Buy Now</button>
        <button className="save-btn">Save for Later</button>
      </div>
    </div>
  );
}
```

```tsx
// [File: app/shop/[[...category]]/[productId]/actions/(.)buy/page.tsx]
'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';

export default function BuyNowModal() {
  const router = useRouter();
  const [isProcessing, setIsProcessing] = useState(false);
  
  const handleBuy = async () => {
    setIsProcessing(true);
    try {
      // Process payment
      await processPayment();
      alert('Purchase successful!');
      router.refresh(); // Refresh cart count
    } catch (error) {
      alert('Purchase failed: ' + error.message);
    } finally {
      setIsProcessing(false);
    }
  };
  
  return (
    <div className="modal">
      <div className="modal-content">
        <h2>Confirm Purchase</h2>
        <p>Are you sure you want to buy this item?</p>
        <button 
          onClick={handleBuy}
          disabled={isProcessing}
          className={isProcessing ? 'processing' : ''}
        >
          {isProcessing ? 'Processing...' : 'Confirm Purchase'}
        </button>
        <button onClick={() => router.back()} className="cancel">
          Cancel
        </button>
      </div>
    </div>
  );
}
```

## Key Takeaways
- Dynamic segments `[param]` match single path segments
- Catch-all segments `[...param]` match one or more path segments (params as array)
- Optional catch-all segments `[[...param]]` match zero or more path segments (params can be undefined)
- Route groups `()` organize without affecting URL path
- Parallel routes `@slot` render multiple views in same layout
- Intercepting routes `(.)` and `(..)` enable modal-like navigation
- `loading.tsx` and `error.tsx` provide automatic Suspense boundaries
- `template.tsx` re-mounts on navigation (unlike `layout.tsx`)
- `default.tsx` provides fallback for parallel routes
- Always handle undefined params for optional catch-all segments
- Use `notFound()` from `next/navigation` for 404 pages

## What's Next
Continue to [Server Components Explained](02-rendering-strategies/01-server-components-explained.md) to understand how Server Components work and when to use them.