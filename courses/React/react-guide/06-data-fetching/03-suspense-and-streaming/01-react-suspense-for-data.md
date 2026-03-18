# React Suspense for Data Fetching

## Overview
React Suspense is a powerful feature introduced in React 16.6 that allows components to "wait" for something before rendering. While initially designed for code splitting, Suspense now supports data fetching as well. This guide explores how Suspense works with data fetching, enabling you to declare loading states at a component level and coordinate multiple data dependencies efficiently.

## Prerequisites
- Understanding of React fundamentals and hooks
- Familiarity with React component lifecycle
- Knowledge of async/await and Promises
- Basic understanding of React.lazy and dynamic imports

## Core Concepts

### Understanding Suspense Basics
Suspense lets you specify a fallback UI that's shown while child components are loading. This is particularly powerful for data fetching because you can declare loading states at any level of your component tree.

```jsx
// File: src/components/SuspenseDemo.jsx

import React, { Suspense, useState } from 'react';

// A simple component that simulates loading
function Post({ id }) {
  // In a real app, this would fetch data
  // With Suspense, the component "throws" a promise when loading
  const post = usePostData(id); // Custom hook that integrates with Suspense
  
  return (
    <article>
      <h2>{post.title}</h2>
      <p>{post.body}</p>
    </article>
  );
}

// The loading fallback component
function PostSkeleton() {
  return (
    <div className="skeleton">
      <div className="skeleton-title"></div>
      <div className="skeleton-body"></div>
      <div className="skeleton-body"></div>
    </div>
  );
}

// Error boundary to catch failed Suspense boundaries
function ErrorFallback({ error, reset }) {
  return (
    <div>
      <h2>Something went wrong</h2>
      <p>{error.message}</p>
      <button onClick={reset}>Try again</button>
    </div>
  );
}

// Main component using Suspense
function SuspenseDemo() {
  const [postId, setPostId] = useState(1);

  return (
    <div>
      <button onClick={() => setPostId(1)}>Post 1</button>
      <button onClick={() => setPostId(2)}>Post 2</button>
      <button onClick={() => setPostId(3)}>Post 3</button>

      {/* Suspense wraps components that might suspend */}
      {/* The fallback renders while any child is loading */}
      <Suspense fallback={<PostSkeleton />}>
        <Post id={postId} />
      </Suspense>
    </div>
  );
}

// Complex example with multiple Suspense boundaries
function BlogPage() {
  return (
    <div className="blog-page">
      <header>
        <h1>My Blog</h1>
      </header>

      {/* Each section can have its own loading state */}
      <aside>
        <Suspense fallback={<SidebarSkeleton />}>
          <Sidebar />
        </Suspense>
      </aside>

      <main>
        <Suspense fallback={<PostListSkeleton />}>
          <PostList />
        </Suspense>
      </main>

      <Suspense fallback={<FooterSkeleton />}>
        <Footer />
      </Suspense>
    </div>
  );
}

export default SuspenseDemo;
```

### Creating Suspense-Compatible Data Fetching
For Suspense to work with data fetching, you need to create resources that "suspend" when data is being fetched. This is done using a pattern where components throw promises.

```jsx
// File: src/resources/postResource.js

import { createResource } from '@tanstack/react-query';

// Using TanStack Query's createResource for Suspense integration
// This is available in @tanstack/query-sync-storage-persister or react-query's experimental

// Alternatively, create your own resource pattern:
function createResource(fetcher, initialValue) {
  let promise = null;
  let data = initialValue;
  let error = null;

  const read = () => {
    if (error) throw error;
    if (promise) throw promise;
    return data;
  };

  const resolve = (newData) => {
    data = newData;
    promise = null;
  };

  const reject = (err) => {
    error = err;
    promise = null;
  };

  const refresh = () => {
    promise = fetcher()
      .then(resolve)
      .catch(reject);
  };

  return { read, refresh };
}

// Example: Custom resource with Suspense
function createPostResource(postId) {
  let status = 'pending';
  let result;
  let suspender;

  const fetchPost = async () => {
    const response = await fetch(
      `https://jsonplaceholder.typicode.com/posts/${postId}`
    );
    if (!response.ok) throw new Error('Failed to fetch post');
    return response.json();
  };

  suspender = fetchPost()
    .then((data) => {
      status = 'success';
      result = data;
    })
    .catch((error) => {
      status = 'error';
      result = error;
    });

  return {
    read() {
      if (status === 'pending') {
        throw suspender;
      }
      if (status === 'error') {
        throw result;
      }
      return result;
    },
    refresh() {
      status = 'pending';
      suspender = fetchPost()
        .then((data) => {
          status = 'success';
          result = data;
        })
        .catch((error) => {
          status = 'error';
          result = error;
        });
    },
  };
}

// React component using the resource
function PostWithSuspense({ postId }) {
  // Create or get the resource
  const [resource] = React.useState(() => createPostResource(postId));

  // Reading the resource triggers Suspense if still loading
  const post = resource.read();

  return (
    <article>
      <h1>{post.title}</h1>
      <p>{post.body}</p>
    </article>
  );
}
```

### Using Suspense with TanStack Query
TanStack Query provides experimental Suspense support that integrates well with React's Suspense boundary.

```jsx
// File: src/components/QuerySuspenseExample.jsx

import React, { Suspense } from 'react';
import { useQuery, useQueryClient, QueryClient, QueryClientProvider } from '@tanstack/react-query';

// Create query client with Suspense enabled
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      suspense: true, // Enable Suspense mode
      staleTime: 5 * 60 * 1000,
    },
  },
});

function PostList() {
  // When suspense is enabled, useQuery throws during loading
  // This allows Suspense to catch it and show fallback
  const { data } = useQuery({
    queryKey: ['posts'],
    queryFn: async () => {
      const response = await fetch(
        'https://jsonplaceholder.typicode.com/posts'
      );
      if (!response.ok) throw new Error('Failed to fetch');
      return response.json();
    },
    suspense: true, // Enable for this query
  });

  return (
    <ul>
      {data?.map(post => (
        <li key={post.id}>{post.title}</li>
      ))}
    </ul>
  );
}

function PostListFallback() {
  // This will be shown while PostList is loading
  return (
    <div className="loading">
      <div className="spinner"></div>
      <p>Loading posts...</p>
    </div>
  );
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <Suspense fallback={<PostListFallback />}>
        <PostList />
      </Suspense>
    </QueryClientProvider>
  );
}

export default App;
```

### Coordinating Multiple Data Dependencies
Suspense excels at coordinating multiple data dependencies, allowing you to handle loading states at different granularities.

```jsx
// File: src/components/CoordinatedLoading.jsx

import React, { Suspense } from 'react';

// Components that each fetch their own data
function UserProfile({ userId }) {
  const user = useUser(userId); // Suspends if loading
  
  return (
    <div>
      <h2>{user.name}</h2>
      <p>{user.email}</p>
    </div>
  );
}

function UserPosts({ userId }) {
  const posts = useUserPosts(userId); // Suspends if loading
  
  return (
    <ul>
      {posts.map(post => (
        <li key={post.id}>{post.title}</li>
      ))}
    </ul>
  );
}

function UserActivity({ userId }) {
  const activity = useUserActivity(userId); // Suspends if loading
  
  return (
    <div>
      <h3>Recent Activity</h3>
      {activity.map(item => (
        <p key={item.id}>{item.description}</p>
      ))}
    </div>
  );
}

// Skeleton components for loading states
function ProfileSkeleton() {
  return <div className="skeleton profile-skeleton" />;
}

function PostsSkeleton() {
  return <div className="skeleton posts-skeleton" />;
}

function ActivitySkeleton() {
  return <div className="skeleton activity-skeleton" />;
}

// Coordinated loading with Suspense
function UserDashboard({ userId }) {
  return (
    <div className="dashboard">
      {/* Each section loads independently with its own fallback */}
      {/* If one section is loading, only that section shows skeleton */}
      <div className="profile-section">
        <Suspense fallback={<ProfileSkeleton />}>
          <UserProfile userId={userId} />
        </Suspense>
      </div>

      <div className="posts-section">
        <Suspense fallback={<PostsSkeleton />}>
          <UserPosts userId={userId} />
        </Suspense>
      </div>

      <div className="activity-section">
        <Suspense fallback={<ActivitySkeleton />}>
          <UserActivity userId={userId} />
        </Suspense>
      </div>
    </div>
  );
}

// Alternative: Single Suspense for everything
function UserDashboardLoadingEverything({ userId }) {
  // All data loads together - one loading state for entire dashboard
  return (
    <Suspense fallback={<FullPageSkeleton />}>
      <UserProfile userId={userId} />
      <UserPosts userId={userId} />
      <UserActivity userId={userId} />
    </Suspense>
  );
}

export default CoordinatedLoading;
```

### Streaming with Suspense
React 18 introduced streaming capabilities that work with Suspense, allowing progressive loading of content.

```jsx
// File: src/components/StreamingExample.jsx

import React, { Suspense } from 'react';
import { defer, Await } from 'react-router-dom';

// Server-side: Create deferred data (available in React Router v6.4+)
async function loadDashboardData() {
  // These load in parallel on the server
  const criticalData = await loadCriticalData(); // Blocked - essential
  const deferredPosts = loadPosts(); // Not blocked - streamed
  
  return {
    criticalData,
    posts: deferredPosts,
  };
}

// Component that uses deferred data
function Dashboard({ loaderData }) {
  return (
    <div>
      {/* Critical data is already loaded - renders immediately */}
      <CriticalSection data={loaderData.criticalData} />

      {/* Posts stream in when available */}
      <Suspense fallback={<PostsSkeleton />}>
        <Await
          resolve={loaderData.posts}
          errorElement={<PostsError />}
        >
          {(posts) => <PostList posts={posts} />}
        </Await>
      </Suspense>
    </div>
  );
}

// Progressive enhancement with streaming
function ProgressiveDashboard() {
  return (
    <div className="dashboard">
      {/* Critical above-the-fold content */}
      <Suspense fallback={<NavigationSkeleton />}>
        <Navigation />
      </Suspense>

      <div className="content">
        {/* Main content streams in */}
        <Suspense fallback={<MainContentSkeleton />}>
          <MainContent />
        </Suspense>

        {/* Secondary content streams later */}
        <Suspense fallback={<SidebarSkeleton />}>
          <Sidebar />
        </Suspense>
      </div>
    </div>
  );
}

export default StreamingExample;
```

## Common Mistakes

### Mistake 1: Not Providing Fallback
Always provide a fallback to Suspense to show meaningful loading states.

```jsx
// ❌ WRONG — No fallback causes blank screen during load
<Suspense>
  <DataComponent />
</Suspense>

// ✅ CORRECT — Fallback shows loading UI
<Suspense fallback={<LoadingSpinner />}>
  <DataComponent />
</Suspense>
```

### Mistake 2: Wrapping Too Much or Too Little
Finding the right granularity for Suspense boundaries is important.

```jsx
// ❌ WRONG — One Suspense for entire app - everything blocks everything
<Suspense fallback={<Loading />}>
  <Header />
  <Navigation />
  <MainContent />
  <Footer />
</Suspense>

// ✅ CORRECT — Granular Suspense for independent sections
<Suspense fallback={<NavSkeleton />}>
  <Navigation />
</Suspense>
<main>
  <Suspense fallback={<ContentSkeleton />}>
    <MainContent />
  </Suspense>
</main>
<Suspense fallback={<FooterSkeleton />}>
  <Footer />
</Suspense>
```

### Mistake 3: Not Handling Errors
Suspense catches loading, but you still need error boundaries.

```jsx
// ❌ WRONG — No error handling for failed data
<Suspense fallback={<Loading />}>
  <PostList />
</Suspense>

// ✅ CORRECT — Wrap with error boundary too
<ErrorBoundary>
  <Suspense fallback={<Loading />}>
    <PostList />
  </Suspense>
</ErrorBoundary>
```

## Real-World Example
Building a complete data-driven application with Suspense patterns.

```jsx
// File: src/hooks/useSuspenseQuery.js

import { useQuery, useQueryClient } from '@tanstack/react-query';
import React, { Suspense } from 'react';

/**
 * Custom hook that throws during loading
 * This enables Suspense to catch and handle the loading state
 */
function useSuspenseQuery(options) {
  const { queryKey, queryFn, ...rest } = options;
  
  // Track if we've tried to fetch at least once
  const hasBeenMounted = React.useRef(false);
  
  const query = useQuery({
    queryKey,
    queryFn: async (...args) => {
      // Mark that we've attempted a fetch
      hasBeenMounted.current = true;
      return queryFn(...args);
    },
    suspense: true, // Enable Suspense mode
    ...rest,
  });

  // If data is undefined and we haven't mounted, it means we're loading
  // Throw to trigger Suspense (only after initial mount)
  if (query.data === undefined && hasBeenMounted.current) {
    throw query.fetchStatus === 'fetching' ? query.promise : query.error;
  }

  return query;
}

// File: src/components/ResourceList.jsx

import React, { Suspense } from 'react';
import { useSuspenseQuery } from '../hooks/useSuspenseQuery';

// Posts component that suspends
function PostList() {
  const { data: posts } = useSuspenseQuery({
    queryKey: ['posts'],
    queryFn: () => fetch('/api/posts').then(r => r.json()),
    staleTime: 5 * 60 * 1000,
  });

  return (
    <ul>
      {posts.map(post => (
        <li key={post.id}>{post.title}</li>
      ))}
    </ul>
  );
}

// Users component that suspends
function UserList() {
  const { data: users } = useSuspenseQuery({
    queryKey: ['users'],
    queryFn: () => fetch('/api/users').then(r => r.json()),
  });

  return (
    <ul>
      {users.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}

// File: src/components/AppLayout.jsx

import React from 'react';

// Skeleton loaders
function PostListSkeleton() {
  return (
    <div className="skeleton-list">
      {[1, 2, 3].map(i => (
        <div key={i} className="skeleton-item" />
      ))}
    </div>
  );
}

function UserListSkeleton() {
  return (
    <div className="skeleton-list">
      {[1, 2, 3, 4].map(i => (
        <div key={i} className="skeleton-item" />
      ))}
    </div>
  );
}

function ErrorMessage({ error }) {
  return (
    <div className="error">
      <p>Failed to load: {error.message}</p>
      <button onClick={() => window.location.reload()}>Retry</button>
    </div>
  );
}

// Error boundary wrapper
class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  render() {
    if (this.state.hasError) {
      return this.props.fallback || <ErrorMessage error={this.state.error} />;
    }
    return this.props.children;
  }
}

// Main app with Suspense boundaries
function AppLayout() {
  return (
    <div className="app">
      <ErrorBoundary>
        <header>
          <h1>My App</h1>
        </header>
      </ErrorBoundary>

      <main>
        <div className="posts-container">
          <h2>Posts</h2>
          <ErrorBoundary>
            <Suspense fallback={<PostListSkeleton />}>
              <PostList />
            </Suspense>
          </ErrorBoundary>
        </div>

        <div className="users-container">
          <h2>Users</h2>
          <ErrorBoundary>
            <Suspense fallback={<UserListSkeleton />}>
              <UserList />
            </Suspense>
          </ErrorBoundary>
        </div>
      </main>
    </div>
  );
}

export default AppLayout;
```

## Key Takeaways
- React Suspense allows components to "wait" for data before rendering
- Components throw promises during loading, which Suspense catches
- Each Suspense boundary can have its own fallback for granular loading states
- Error boundaries should wrap Suspense boundaries to handle failures
- TanStack Query has experimental Suspense support via the suspense option
- Suspense enables coordinated loading of multiple data dependencies
- Finding the right granularity for Suspense boundaries is crucial for UX

## What's Next
Continue to [Error Boundaries](02-error-boundaries.md) to learn how to properly handle and recover from JavaScript errors in your React applications.
