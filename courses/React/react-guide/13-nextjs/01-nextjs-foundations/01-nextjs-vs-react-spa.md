# Next.js vs React SPA

## Overview
Understanding when to use Next.js versus a traditional React Single Page Application (SPA) is crucial for making architectural decisions. Both are React-based but have fundamentally different approaches to rendering, routing, and deployment. Next.js provides server-side rendering and static generation out of the box, while traditional React SPAs render everything in the browser. This guide helps you understand the trade-offs and choose the right framework for your project.

## Prerequisites
- Understanding of React fundamentals
- Knowledge of client-side vs server-side rendering
- Familiarity with deployment concepts

## Core Concepts

### Understanding SPA (Single Page Application)
A traditional React SPA loads a single HTML file and renders all content in the browser:

```javascript
// [File: src/App.jsx - Traditional React SPA]

// All components render client-side
import React, { useState, useEffect } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

function App() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);

  // Data fetching happens in the browser
  useEffect(() => {
    fetch('/api/users')
      .then(res => res.json())
      .then(data => {
        setUsers(data);
        setLoading(false);
      });
  }, []);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/users" element={<UsersPage users={users} loading={loading} />} />
        <Route path="/users/:id" element={<UserProfile />} />
      </Routes>
    </BrowserRouter>
  );
}

// Flow:
// 1. Browser requests index.html
// 2. Server returns static HTML (empty shell)
// 3. Browser downloads React bundle
// 4. React hydrates and renders
// 5. API calls happen AFTER hydration
```

### Understanding Next.js
Next.js is a full-stack React framework with multiple rendering strategies:

```tsx
// [File: app/page.tsx - Next.js App Router]

// Server Component by default - runs on server
import { getUsers } from '@/lib/api';

export default async function HomePage() {
  // Data fetching happens ON THE SERVER before HTML is sent
  const users = await getUsers();

  return (
    <main>
      <h1>Users</h1>
      <ul>
        {users.map(user => (
          <li key={user.id}>{user.name}</li>
        ))}
      </ul>
    </main>
  );
}

// Flow:
// 1. Browser requests page
// 2. Server fetches data
// 3. Server renders React components to HTML
// 4. Server sends complete HTML to browser
// 5. Browser displays immediately (fast FCP)
// 6. React "hydrates" for interactivity
```

### Comparison Table

| Aspect | React SPA | Next.js |
|--------|-----------|---------|
| **Initial Load** | Slower (needs JS download) | Faster (HTML already populated) |
| **SEO** | Poor (content in JS) | Excellent (server-rendered HTML) |
| **Data Fetching** | Client-side only | Server + Client options |
| **TTFB** | Higher (client renders) | Lower (pre-rendered) |
| **Bundle Size** | Larger (everything included) | Smaller (code splitting automatic) |
| **Hosting** | Any static host | Vercel, Node.js, edge |
| **API Routes** | Need separate backend | Built-in API routes |

### When to Choose React SPA
React SPA is the right choice when:

```typescript
// [File: src/examples/spa-use-cases.tsx']

// 1. Internal dashboard with authentication
// - No SEO needed
// - Fast interactions after initial load
// - Heavy client-side interactivity

function Dashboard() {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    // Fetch after auth
    api.getDashboardData().then(setData);
  }, []);
  
  return <Charts data={data} />;
}

// 2. Complex web applications (Figma, Notion-like)
// - Heavy state management
// - Real-time collaboration
// - Offline capabilities

// 3. Mobile-first PWAs
// - Need service workers
// - Offline support
// - App-like experience
```

### When to Choose Next.js
Next.js is better when:

```tsx
// [File: app/examples/nextjs-use-cases.tsx]

// 1. Public websites with SEO needs
// - Marketing pages
// - Blog/Content sites
// - E-commerce

export default async function ProductPage({ params }) {
  // Server-side fetch
  const product = await getProduct(params.id);
  
  // Search engines see full HTML!
  return (
    <article>
      <h1>{product.name}</h1>
      <meta name="description" content={product.description} />
    </article>
  );
}

// 2. E-commerce platforms
// - Product pages need SEO
// - Dynamic pricing
// - Inventory management

// 3. SaaS applications
// - Landing pages need SEO
// - Dashboard is authenticated
// - Mixed public/private routes
```

### App Router vs Pages Router
Next.js 13+ introduced the App Router with new features:

```tsx
// [File: app/page.tsx - App Router (Recommended)]

// Server Components by default
export default async function Page() {
  const data = await fetchData();
  return <div>{data.title}</div>;
}

// Layouts - wrap pages
export default function Layout({ children }) {
  return (
    <html>
      <body>
        <nav />
        {children}
        <footer />
      </body>
    </html>
  );
}

// Loading states
export default function Loading() {
  return <Skeleton />;
}

// Error handling
'use client';
export default function Error({ error, reset }) {
  return <div>Error: {error.message}</div>;
}

// Server Actions
export async function createPost(formData: FormData) {
  'use server';
  // Server-side logic
  await createPostInDb(formData);
}
```

```tsx
// [File: pages/index.tsx - Pages Router (Legacy)]

// Client Component
export default function Page() {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    fetchData().then(setData);
  }, []);
  
  return <div>{data?.title}</div>;
}

// getServerSideProps - runs on every request
export async function getServerSideProps() {
  const data = await fetchData();
  return { props: { data } };
}

// getStaticProps - runs at build time
export async function getStaticProps() {
  const data = await fetchData();
  return { props: { data }, revalidate: 60 };
}
```

## Common Mistakes

### Mistake 1: Using SPA for Public Content
```typescript
// ❌ WRONG - React SPA for public blog
// Search engines struggle to index content
// Slow first contentful paint

// ✅ CORRECT - Use Next.js for public content
// Server-rendered HTML is immediately visible
// Search engines can index properly
```

### Mistake 2: Overcomplicating Internal Tools
```typescript
// ❌ WRONG - Using Next.js for internal dashboard
// Unnecessary complexity
// Authentication overhead

// ✅ CORRECT - Use simple React SPA or Vite
// Faster development
// Simpler deployment
// Less infrastructure
```

### Mistake 3: Mixing Rendering Strategies Incorrectly
```tsx
// ❌ WRONG - Fetching all data client-side in Next.js
export default function Page() {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    // This defeats the purpose of Next.js!
    fetch('/api/data').then(setData);
  }, []);
  
  return <div>{data}</div>;
}

// ✅ CORRECT - Use server-side data fetching
export default async function Page() {
  const data = await fetch('/api/data');
  
  return <div>{data.title}</div>;
}
```

## Real-World Example

Complete project structure showing hybrid approach:

```tsx
// [File: app/layout.tsx - Root Layout]

import './globals.css';
import { Inter } from 'next/font/google';
import { Navbar } from '@/components/navbar';
import { Footer } from '@/components/footer';

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
        <main>{children}</main>
        <Footer />
      </body>
    </html>
  );
}
```

```tsx
// [File: app/page.tsx - Home Page (Server Component)]

import { getFeaturedProducts } from '@/lib/api';

export default async function HomePage() {
  // Data fetched on server
  const products = await getFeaturedProducts();
  
  return (
    <section>
      <h1>Featured Products</h1>
      <div className="grid">
        {products.map(product => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </section>
  );
}
```

```tsx
// [File: app/dashboard/page.tsx - Dashboard (Client Component)]

'use client';

import { useState, useEffect } from 'react';
import { useAuth } from '@/hooks/useAuth';
import { fetchDashboardData } from '@/lib/api';

export default function DashboardPage() {
  const { user } = useAuth();
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);

  // Data fetched client-side (after auth)
  useEffect(() => {
    if (user) {
      setLoading(true);
      fetchDashboardData(user.id)
        .then(setData)
        .finally(() => setLoading(false));
    }
  }, [user]);

  if (!user) return <div>Please log in</div>;
  if (loading) return <SkeletonLoader />;
  
  return <DashboardContent data={data} />;
}
```

```tsx
// [File: app/api/route.ts - API Route (App Router)]

import { NextResponse } from 'next/server';
import { prisma } from '@/lib/prisma';

export async function GET(request: Request) {
  const { searchParams } = new URL(request.url);
  const userId = searchParams.get('userId');
  
  try {
    const data = await prisma.userData.findUnique({
      where: { userId },
    });
    
    return NextResponse.json(data);
  } catch (error) {
    return NextResponse.json(
      { error: 'Failed to fetch data' },
      { status: 500 }
    );
  }
}
```

## Key Takeaways
- Choose React SPA for internal tools, dashboards, and apps where SEO doesn't matter
- Choose Next.js for public websites, blogs, e-commerce, and content that needs SEO
- Next.js App Router is the modern approach with Server Components by default
- Use Server Components for data fetching and Layouts
- Use Client Components (with 'use client') for interactivity and state
- Hybrid approaches work well: public pages with Next.js, internal apps with React SPA
- Consider your team's expertise and deployment requirements when choosing

## What's Next
Continue to [App Router Fundamentals](02-app-router-fundamentals.md) to learn about the Next.js App Router structure, layouts, and routing conventions.