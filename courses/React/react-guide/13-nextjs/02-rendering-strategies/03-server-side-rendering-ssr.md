# Server-Side Rendering (SSR)

## Overview
Server-Side Rendering (SSR) generates HTML on each request, ensuring users always receive the most up-to-date content. Unlike Static Generation where pages are built once at compile time, SSR renders pages dynamically on the server for every incoming request. This is essential for personalized content, real-time data, and pages that need access to request-specific information like cookies or headers.

## Prerequisites
- Understanding of Next.js App Router
- Familiarity with Server Components
- Knowledge of data fetching patterns

## Core Concepts

### Dynamic Rendering
By default, Server Components in Next.js App Router are dynamically rendered - they execute on each request:

```tsx
// [File: app/dashboard/page.tsx]
import { getUserData } from '@/lib/api';

export default async function DashboardPage() {
  // This runs on EVERY request
  const user = await getUserData();
  const now = new Date().toISOString();
  
  return (
    <div>
      <h1>Welcome back, {user.name}!</h1>
      <p>Current time: {now}</p>
      <p>Your last login: {user.lastLogin}</p>
    </div>
  );
}

// Each request:
// 1. Server receives request
// 2. Executes this component with fresh data
// 3. Returns new HTML with current data
// 4. Page always shows real-time information
```

### Request-Specific Data
SSR is the only option when you need access to request-specific data:

```tsx
// [File: app/profile/page.tsx]
import { cookies, headers } from 'next/headers';

export default async function ProfilePage() {
  // Access cookies (only available in Server Components)
  const cookieStore = cookies();
  const token = cookieStore.get('auth-token')?.value;
  
  // Access request headers
  const headerStore = headers();
  const userAgent = headerStore.get('user-agent');
  
  // Use this data for personalization
  const user = await verifyAndGetUser(token);
  
  return (
    <div>
      <h1>Your Profile</h1>
      <p>Logged in as: {user.name}</p>
      <p>Device: {userAgent}</p>
    </div>
  );
}
```

### When to Use SSR
SSR is ideal for:

```tsx
// [File: app/examples/ssr-use-cases.tsx]

// 1. Personalized content based on user
export default async function PersonalizedFeed() {
  const user = await getCurrentUser();
  const feed = await getPersonalizedFeed(user.id);
  
  return <Feed items={feed} />;
}

// 2. Real-time data (stock prices, scores)
export default async function StockPage({ params }: { params: { symbol: string } }) {
  const price = await getRealTimePrice(params.symbol);
  const changes = await getPriceHistory(params.symbol);
  
  return <StockChart price={price} history={changes} />;
}

// 3. Request-dependent content (AB tests, geo-location)
export default async function LandingPage() {
  const cookieStore = cookies();
  const abVariant = cookieStore.get('ab-test-variant')?.value || 'control';
  
  return <ABTestContent variant={abVariant} />;
}

// 4. Protected routes with auth
export default async function DashboardPage() {
  const session = await getSession();
  
  if (!session) {
    redirect('/login');
  }
  
  return <Dashboard data={session.data} />;
}
```

### Streaming with Suspense
SSR works seamlessly with Suspense for progressive loading:

```tsx
// [File: app/dashboard/page.tsx]
import { Suspense } from 'react';

export default function DashboardPage() {
  return (
    <div className="dashboard">
      {/* Load immediately - fast query */}
      <Suspense fallback={<UserCardSkeleton />}>
        <UserCard />
      </Suspense>
      
      {/* Load with delay */}
      <Suspense fallback={<NotificationsSkeleton />}>
        <Notifications />
      </Suspense>
      
      {/* Load last */}
      <Suspense fallback={<RecommendationsSkeleton />}>
        <Recommendations />
      </Suspense>
    </div>
  );
}

// Components can be fetched in parallel
async function UserCard() {
  const user = await getCurrentUser(); // Fast - loads first
  return <UserCardComponent user={user} />;
}

async function Notifications() {
  const notifications = await getNotifications(); // Slower
  return <NotificationsList notifications={notifications} />;
}

async function Recommendations() {
  const recs = await getRecommendations(); // Slowest
  return <RecommendationsList recommendations={recs} />;
}
```

## Key Differences: SSR vs SSG vs ISR

| Feature | SSR | SSG | ISR |
|---------|-----|-----|-----|
| **When rendered** | Every request | Build time | On demand + cache |
| **Data freshness** | Always fresh | Fixed at build | Fresh after revalidate |
| **Performance** | Slower (per request) | Fastest | Fast after first |
| **Use case** | Personalized | Static content | Blog, docs |

## Common Mistakes

### Mistake 1: Using SSR When Not Needed
```tsx
// ❌ WRONG - Using SSR for static blog content
export default async function BlogPost({ params }) {
  // Renders on every request!
  const post = await getPost(params.slug);
  return <div>{post.content}</div>;
}

// ✅ CORRECT - Use SSG with ISR for blog posts
export const revalidate = 3600; // Regenerate every hour
```

### Mistake 2: Blocking Data Fetching
```tsx
// ❌ WRONG - Sequential fetches block rendering
export default async function Page() {
  const user = await getUser();     // Wait for this
  const posts = await getPosts();  // Then wait for this
  const notifs = await getNotifs(); // Then wait for this
  
  return <div>{/* All data */}</div>;
}

// ✅ CORRECT - Parallel fetches
export default async function Page() {
  const [user, posts, notifs] = await Promise.all([
    getUser(),
    getPosts(),
    getNotifs(),
  ]);
  
  return <div>{/* All data */}</div>;
}
```

## Real-World Example

```tsx
// [File: app/api/revalidate/route.ts]
import { NextRequest, NextResponse } from 'next/server';
import { revalidatePath } from 'next/cache';

export async function POST(request: NextRequest) {
  const secret = request.headers.get('x-revalidate-token');
  
  // Verify secret to prevent unauthorized revalidation
  if (secret !== process.env.REVALIDATE_SECRET) {
    return NextResponse.json({ message: 'Invalid token' }, { status: 401 });
  }
  
  try {
    const body = await request.json();
    const path = body.path || '/';
    
    // Revalidate the path
    revalidatePath(path);
    
    return NextResponse.json({ 
      revalidated: true, 
      path,
      now: Date.now() 
    });
  } catch (err) {
    return NextResponse.json({ 
      message: 'Error revalidating' 
    }, { status: 500 });
  }
}
```

## Key Takeaways
- SSR renders pages on each request for fresh, personalized content
- Use SSR for user-specific data, real-time information, and request-dependent content
- SSR can access cookies, headers, and other request-specific data
- Combine SSR with Suspense for progressive loading experiences
- Choose SSR, SSG, or ISR based on your content's freshness requirements
- Always use Promise.all for parallel data fetching in Server Components

## What's Next
Continue to [Next.js API Routes](03-nextjs-features/01-nextjs-api-routes.md) to learn about creating backend API endpoints in Next.js.