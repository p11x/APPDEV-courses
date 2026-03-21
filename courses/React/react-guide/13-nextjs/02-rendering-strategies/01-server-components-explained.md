# Server Components Explained

## Overview
Server Components are a revolutionary feature in React that allows components to render on the server, reducing client-side JavaScript bundle size and improving performance. In Next.js App Router, components are Server Components by default. This guide explains what Server Components are, how they differ from Client Components, when to use each, and the rules governing their usage.

## Prerequisites
- Understanding of React components
- Familiarity with Next.js App Router
- Basic knowledge of client-side vs server-side rendering

## Core Concepts

### What Are Server Components?
Server Components are React components that render exclusively on the server. They never send JavaScript to the client for that component's rendering logic, resulting in smaller bundle sizes and faster initial loads.

```tsx
// [File: app/server-component.tsx]
// This is a Server Component by default in App Router

import { getUserData } from '@/lib/api';

// This entire function runs on the server
// Zero JavaScript is sent to the client for this component's logic
export default async function ServerComponent() {
  // Can directly access server resources
  const user = await getUserData();
  
  // Can read files, environment variables, access database
  const config = process.env.NEXT_PUBLIC_API_URL;
  
  // Returns plain HTML to the client
  return (
    <div>
      <h1>Welcome, {user.name}!</h1>
      <p>Your account was created on {user.createdAt}</p>
    </div>
  );
}

// What the client receives:
// <div>
//   <h1>Welcome, John!</h1>
//   <p>Your account was created on 2023-05-15</p>
// </div>
// No React code, no state, no effects - just HTML!
```

### Client Components
Client Components are traditional React components that run in the browser. They can use state, effects, and event handlers.

```tsx
// [File: app/client-component.tsx]
'use client'; // This makes it a Client Component

import { useState, useEffect } from 'react';

export default function ClientComponent() {
  const [count, setCount] = useState(0);
  const [data, setData] = useState(null);

  // Can use state, effects, refs, etc.
  useEffect(() => {
    // Fetch data on mount
    fetch('/api/data').then(res => res.json()).then(setData);
  }, []);

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(c => c + 1)}>
        Increment
      </button>
      {data && <pre>{JSON.stringify(data)}</pre>}
    </div>
  );
}

// What the client receives:
// The full component code including React logic, state management, etc.
```

### Key Differences

| Feature | Server Components | Client Components |
|---------|-------------------|-------------------|
| **Where they run** | Server only | Browser only |
| **JavaScript sent to client** | None (HTML only) | Full component code |
| **Can use state** | ❌ No | ✅ Yes (useState, useReducer) |
| **Can use effects** | ❌ No | ✅ Yes (useEffect, useLayoutEffect) |
| **Can use event handlers** | ❌ No | ✅ Yes (onClick, onChange) |
| **Can access browser APIs** | ❌ No (window, document) | ✅ Yes |
| **Can access server resources** | ✅ Yes (DB, file system, env vars) | ❌ No |
| **Data fetching** | ✅ Direct (no network waterfall) | ❌ Requires network request |
| **Bundle impact** | Zero | Included in JavaScript bundle |

### The 'use client' Directive
To create a Client Component, add `'use client'` at the top of the file:

```tsx
// [File: app/interactive-button.tsx]
'use client'; // MUST be first line (except comments)

import { useState } from 'react';

export default function InteractiveButton() {
  const [clicked, setClicked] = useState(false);
  
  return (
    <button onClick={() => setClicked(true)}>
      {clicked ? 'Clicked!' : 'Click me'}
    </button>
  );
}
```

Rules for `'use client'`:
- Must be at the very top of the file
- Applies to the entire file and all imported components
- Cannot be used in Server Components
- Once a file is marked as Client Component, all its imports are treated as Client Components

### Data Fetching in Server Components
Server Components can fetch data directly without creating a network waterfall:

```tsx
// [File: app/dashboard/page.tsx]
import { getUser, getPosts, getNotifications } from '@/lib/api';

// All these run in parallel on the server
export default async function DashboardPage() {
  // These requests happen in parallel, not sequentially!
  const [user, posts, notifications] = await Promise.all([
    getUser(),
    getPosts(),
    getNotifications(),
  ]);
  
  return (
    <div>
      <h1>Welcome, {user.name}!</h1>
      <section>
        <h2>Your Posts</h2>
        <ul>
          {posts.map(post => (
            <li key={post.id}>{post.title}</li>
          ))}
        </ul>
      </section>
      <section>
        <h2>Notifications</h2>
        <ul>
          {notifications.map(notif => (
            <li key={notif.id}>{notif.message}</li>
          ))}
        </ul>
      </section>
    </div>
  );
}

// In a traditional React SPA, this would be:
// 1. Fetch user -> wait
// 2. Fetch posts -> wait  
// 3. Fetch notifications -> wait
// (Sequential network waterfall)
//
// In Server Components:
// 1. Fetch user, posts, notifications -> all happen at same time
// (Parallel data fetching)
```

### Streaming and Suspense
Server Components work seamlessly with React Suspense for streaming HTML:

```tsx
// [File: app/dashboard/layout.tsx]
import { Suspense } from 'react';
import { Navbar } from '@/components/navbar';
import { Sidebar } from '@/components/sidebar';

export default function DashboardLayout({
  children, // This could be a slow Server Component
}: {
  children: React.ReactNode;
}) {
  return (
    <div>
      <Navbar />
      <div className="dashboard-content">
        <Sidebar />
        <main>
          {/* Show loading state while children load */}
          <Suspense fallback={<DashboardSkeleton />}>
            {children}
          </Suspense>
        </main>
      </div>
    </div>
  );
}

// The server can start sending HTML immediately:
// 1. Send navbar HTML
// 2. Send sidebar HTML  
// 3. Send loading skeleton
// 4. Stream the rest as it becomes available
```

### Server Component Limitations
Server Components have important restrictions:

```tsx
// [File: app/server-limitations.tsx]
import { useState, useEffect } from 'react'; // ❌ ERROR: Hooks not allowed
import { useRouter } from 'next/navigation'; // ❌ ERROR: Hooks not allowed
import { ref } from 'react'; // ❌ ERROR: Refs not allowed

export default function ServerComponent() {
  // ❌ ERROR: No state
  const [count, setCount] = useState(0);
  
  // ❌ ERROR: No effects
  useEffect(() => {
    console.log('mounted');
  }, []);
  
  // ❌ ERROR: No refs
  const inputRef = useRef(null);
  
  // ❌ ERROR: No event handlers in JSX
  return (
    <div>
      <button onClick={() => alert('clicked')}> // ❌ ERROR
        Click me
      </button>
    </div>
  );
}
```

### When to Use Each

#### Use Server Components When:
- Fetching data from databases or APIs
- Accessing server-only resources (environment variables, file system)
- Rendering static or mostly static content
- Trying to minimize client-side JavaScript
- Building layouts and page shells

```tsx
// Good use cases for Server Components:
import { getPosts } from '@/lib/api';

export default async function BlogPage() {
  const posts = await getPosts(); // Direct DB access
  
  return (
    <article>
      <h1>Blog Posts</h1>
      {posts.map(post => (
        <PostCard key={post.id} post={post} /> {/* Can pass data to Client Components */}
      ))}
    </article>
  );
}
```

#### Use Client Components When:
- Need state (useState, useReducer)
- Need effects (useEffect, useLayoutEffect)
- Need event handlers (onClick, onChange, etc.)
- Using browser APIs (localStorage, geolocation, canvas)
- Using third-party libraries that require DOM
- Building interactive widgets

```tsx
// Good use cases for Client Components:
'use client';

import { useState } from 'react';
import { Button } from '@/components/ui';

export default function Counter() {
  const [count, setCount] = useState(0);
  
  return (
    <div>
      <p>Count: {count}</p>
      <Button onClick={() => setCount(c => c + 1)}>
        Increment
      </button>
      <Button onClick={() => setCount(0)}>
        Reset
      </button>
    </div>
  );
}
```

## Common Mistakes

### Mistake 1: Using Hooks in Server Components
```tsx
// ❌ WRONG - Hooks not allowed in Server Components
import { useState } from 'react';

export default function ServerComponent() {
  const [data, setData] = useState(null); // Error!
  
  return <div>{data}</div>;
}

// ✅ CORRECT - Fetch data directly or pass as props
export default async function ServerComponent() {
  const data = await fetchData(); // No hooks needed
  
  return <div>{data}</div>;
}
```

### Mistake 2: Passing Server Components as Props to Client Components Incorrectly
```tsx
// ❌ WRONG - Trying to pass Server Component as prop
'use client';

import { useState } from 'react';

export default function ClientWrapper({ children }) {
  const [show, setShow] = useState(false);
  
  return (
    <div>
      <button onClick={() => setShow(!show)}>Toggle</button>
      {show && children} // children might be Server Component
    </div>
  );
}

// Usage:
// <ClientWrapper>
//   <ServerComponent /> // This won't work as expected
// </ClientWrapper>

// ✅ CORRECT - Pass data, not components
'use client';

import { useState } from 'react';

export default function ClientWrapper({ data }) {
  const [show, setShow] = useState(false);
  
  return (
    <div>
      <button onClick={() => setShow(!show)}>Toggle</button>
      {show && <div>{data}</div>}
    </div>
  );
}

// Usage:
// <ClientWrapper data={serverData} />
```

### Mistake 3: Not Understanding the Bundle Impact
```tsx
// ❌ WRONG - Thinking all components add to bundle
import { HeavyLibrary } from 'heavy-library'; // 500KB library

export default function ServerComponent() {
  // This HeavyLibrary code NEVER goes to the client!
  // Only the resulting HTML is sent
  const processedData = HeavyLibrary.process(someData);
  
  return <div>{processedData}</div>;
}

// ✅ This is actually GOOD for performance!
// HeavyLibrary runs on server, zero client impact
```

## Real-World Example

Complete e-commerce product page demonstrating Server and Client Components:

```tsx
// [File: app/products/[slug]/page.tsx] - Server Component
import { getProductBySlug } from '@/lib/api';
import { ProductGallery } from '@/components/product-gallery';
import { ProductDescription } from '@/components/product-description';
import { AddToCartForm } from '@/components/add-to-cart-form';
import { RelatedProducts } from '@/components/related-products';
import { Loading } from '@/components/ui';

export default async function ProductPage({
  params,
}: {
  params: { slug: string };
}) {
  // All data fetching happens on server
  const product = await getProductBySlug(params.slug);
  
  if (!product) {
    notFound();
  }
  
  // Fetch related products in parallel
  const [productData, relatedProducts] = await Promise.all([
    getProductBySlug(params.slug),
    getRelatedProducts(product.category, product.id),
  ]);
  
  return (
    <div className="product-page">
      <ProductGallery 
        images={product.images} 
        // Pass data to Client Component
      />
      
      <ProductDescription 
        title={product.title}
        description={product.description}
        price={product.price}
        // Pass data to Client Component
      />
      
      {/* This Client Component handles state and interactivity */}
      <AddToCartForm 
        productId={product.id}
        // Initial data passed as props
      />
      
      <RelatedProducts 
        products={relatedProducts}
        // Pass data to Client Component
      />
    </div>
  );
}
```

```tsx
// [File: app/components/add-to-cart-form.tsx] - Client Component
'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { Button } from '@/components/ui';

interface AddToCartFormProps {
  productId: string;
}

export default function AddToCartForm({ productId }: AddToCartFormProps) {
  const [quantity, setQuantity] = useState(1);
  const [isAdding, setIsAdding] = useState(false);
  const router = useRouter();
  
  const handleAddToCart = async () => {
    setIsAdding(true);
    try {
      // Call API to add to cart
      await addToCart(productId, quantity);
      
      // Show success feedback
      alert('Added to cart!');
      
      // Optionally update cart count in header
      router.refresh();
    } catch (error) {
      alert('Failed to add to cart: ' + error.message);
    } finally {
      setIsAdding(false);
    }
  };
  
  return (
    <div className="add-to-cart-form">
      <div className="quantity-selector">
        <label>Quantity:</label>
        <input
          type="number"
          value={quantity}
          onChange={(e) => setQuantity(Number(e.target.value))}
          min={1}
          max={99}
        />
      </div>
      
      <Button 
        onClick={handleAddToCart}
        disabled={isAdding}
        className={isAdding ? 'adding' : ''}
      >
        {isAdding ? 'Adding...' : 'Add to Cart'}
      </Button>
    </div>
  );
}
```

## Key Takeaways
- Server Components render on the server and send only HTML to the client
- Client Components run in the browser and can use state, effects, and event handlers
- Server Components have zero JavaScript bundle impact for their rendering logic
- Use `'use client'` at the top of a file to make it a Client Component
- Server Components can fetch data directly and in parallel
- Client Components are needed for interactivity and state management
- Pass data from Server Components to Client Components via props
- Never use hooks, refs, or event handlers in Server Components
- Combine both patterns for optimal performance and functionality

## What's Next
Continue to [Static Generation (SSG)](02-rendering-strategies/02-static-generation-ssg.md) to learn about pre-rendering pages at build time for maximum performance.