# Caching and Invalidation in TanStack Query

## Overview
Understanding how TanStack Query handles caching and invalidation is crucial for building performant React applications. This guide covers advanced cache management techniques, including stale time configuration, garbage collection, background refetching strategies, and how to implement real-time updates. You'll learn how to balance between fresh data and reduced network requests.

## Prerequisites
- Completed previous TanStack Query guides
- Understanding of useQuery and useMutation hooks
- Familiarity with React state management

## Core Concepts

### Understanding Cache Behavior
TanStack Query uses intelligent caching to minimize network requests while keeping data fresh. Understanding the lifecycle of cached data helps you configure it properly.

```jsx
// File: src/components/CacheBehaviorDemo.jsx

import React from 'react';
import { useQuery } from '@tanstack/react-query';

const fetchUser = async (userId) => {
  const response = await fetch(
    `https://jsonplaceholder.typicode.com/users/${userId}`
  );
  if (!response.ok) throw new Error('Failed to fetch user');
  return response.json();
};

function CacheBehaviorDemo() {
  // First render - query fetches from network
  const { data: user1, isLoading: loading1 } = useQuery({
    queryKey: ['user', 1],
    queryFn: () => fetchUser(1),
    staleTime: 5 * 60 * 1000, // 5 minutes - data stays "fresh" for this long
    gcTime: 10 * 60 * 1000, // 10 minutes - garbage collection time
  });

  // Component unmounts and remounts within 10 minutes
  // Data is still in cache - no network request needed
  const { data: user1Again, isLoading: loading1Again } = useQuery({
    queryKey: ['user', 1], // Same key = same cached data
    queryFn: () => fetchUser(1),
    staleTime: 5 * 60 * 1000,
    gcTime: 10 * 60 * 1000,
  });

  // Query with different key - fetches from network
  const { data: user2 } = useQuery({
    queryKey: ['user', 2], // Different ID = different cache entry
    queryFn: () => fetchUser(2),
  });

  return (
    <div>
      <h2>Cache Behavior Demo</h2>
      {loading1 ? (
        <p>Loading user 1...</p>
      ) : (
        <p>User 1: {user1?.name}</p>
      )}
      {loading1Again ? (
        <p>Loading user 1 again...</p>
      ) : (
        <p>User 1 (from cache): {user1Again?.name}</p>
      )}
      <p>User 2: {user2?.name}</p>
    </div>
  );
}

export default CacheBehaviorDemo;
```

### Stale Time Configuration
The staleTime option determines how long data is considered "fresh." Data that's stale will be refetched in the background when conditions are met.

```jsx
// File: src/lib/cacheConfig.js

// Cache configuration examples for different data types

export const cacheConfigs = {
  // Static data that rarely changes - long stale time
  // e.g., app settings, countries list, categories
  static: {
    staleTime: 60 * 60 * 1000, // 1 hour
    gcTime: 24 * 60 * 60 * 1000, // 24 hours
    refetchOnMount: false,
    refetchOnWindowFocus: false,
  },

  // Semi-static data that changes occasionally
  // e.g., user profile, product details
  semiStatic: {
    staleTime: 5 * 60 * 1000, // 5 minutes
    gcTime: 30 * 60 * 1000, // 30 minutes
    refetchOnMount: true,
    refetchOnWindowFocus: true,
  },

  // Dynamic data that changes frequently
  // e.g., notifications, messages, stock prices
  dynamic: {
    staleTime: 0, // Always considered stale
    gcTime: 5 * 60 * 1000, // 5 minutes
    refetchOnMount: true,
    refetchOnWindowFocus: true,
    // Optional: refetch interval for real-time-like updates
    refetchInterval: 30 * 1000, // Every 30 seconds
  },

  // Paginated data
  paginated: {
    staleTime: 5 * 60 * 1000,
    gcTime: 30 * 60 * 1000,
    placeholderData: true, // Keep old pages while fetching new
  },

  // Infinite data
  infinite: {
    staleTime: 5 * 60 * 1000,
    gcTime: 30 * 60 * 1000,
  },
};

// Apply to queries
function ExampleQuery() {
  const { data } = useQuery({
    queryKey: ['countries'],
    queryFn: fetchCountries,
    ...cacheConfigs.static, // Spread the config
  });
}
```

### Background Refetching
TanStack Query automatically refetches data in the background under certain conditions. Understanding these triggers helps optimize your app's data flow.

```jsx
// File: src/components/BackgroundRefetchDemo.jsx

import React, { useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';

const fetchPosts = async () => {
  const response = await fetch('https://jsonplaceholder.typicode.com/posts');
  if (!response.ok) throw new Error('Failed to fetch');
  return response.json();
};

function BackgroundRefetchDemo() {
  const queryClient = useQueryClient();
  const [refetchCount, setRefetchCount] = useState(0);

  const { data, dataUpdatedAt } = useQuery({
    queryKey: ['posts'],
    queryFn: fetchPosts,
    // Stale time - how long before data is considered stale
    staleTime: 30 * 1000, // 30 seconds
    
    // Refetch on mount (if data exists but is stale)
    refetchOnMount: true,
    
    // Refetch when window gains focus (if data is stale)
    refetchOnWindowFocus: true,
    
    // Refetch at intervals (polling)
    refetchInterval: false, // or: 60000 for 1 minute interval
    
    // Refetch even when tab is in background
    refetchIntervalInBackground: false,
    
    // Callback when data is fetched
    onSuccess: (data) => {
      console.log('Data fetched successfully');
      setRefetchCount(c => c + 1);
    },
    
    // Callback when error occurs
    onError: (error) => {
      console.error('Fetch error:', error);
    },
  });

  const handleManualRefetch = () => {
    queryClient.refetchQueries({ queryKey: ['posts'] });
  };

  const handlePrefetch = async () => {
    await queryClient.prefetchQuery({
      queryKey: ['posts'],
      queryFn: fetchPosts,
    });
    console.log('Prefetched!');
  };

  return (
    <div>
      <h2>Background Refetch Demo</h2>
      <p>Data updated at: {new Date(dataUpdatedAt).toLocaleTimeString()}</p>
      <p>Refetch count: {refetchCount}</p>
      
      <button onClick={handleManualRefetch}>Manual Refetch</button>
      <button onClick={handlePrefetch}>Prefetch</button>
      
      {/* Show post count */}
      <p>Posts loaded: {data?.length || 0}</p>
    </div>
  );
}

export default BackgroundRefetchDemo;
```

### Advanced Invalidation Patterns
Query invalidation is the key to keeping your UI in sync with server state after mutations. TanStack Query provides flexible invalidation strategies.

```jsx
// File: src/lib/invalidationStrategies.js

import { queryClient } from '../main';

/**
 * Invalidate all queries matching a key prefix
 */
function invalidateRelatedQueries() {
  // Invalidate all 'posts' queries (includes ['posts'], ['posts', id], etc.)
  queryClient.invalidateQueries({ queryKey: ['posts'] });
  
  // Invalidate all queries
  queryClient.invalidateQueries();
}

/**
 * Invalidate with predicate function
 * Only invalidates queries that match the predicate
 */
function invalidateSpecificQueries() {
  queryClient.invalidateQueries({
    queryKey: ['posts'],
    predicate: (query) => {
      // Only invalidate if querying a specific user's posts
      const filters = query.queryKey[1];
      return filters?.userId === 123;
    },
  });
}

/**
 * Invalidate based on mutation response
 */
async function handleCreatePost(newPost) {
  const result = await api.createPost(newPost);
  
  // Invalidate user's posts since new post affects the list
  queryClient.invalidateQueries({
    queryKey: ['posts'],
    predicate: (query) => {
      const filters = query.queryKey[1];
      // Only invalidate if it's the same user's posts
      return filters?.userId === result.userId;
    },
  });
  
  // Also invalidate post count
  queryClient.invalidateQueries({ queryKey: ['postCount'] });
}

/**
 * Optimistic update with smart invalidation
 */
function useOptimisticInvalidation() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: updatePost,
    onMutate: async (updatedPost) => {
      // Cancel outgoing refetches
      await queryClient.cancelQueries({ queryKey: ['posts'] });
      
      // Snapshot previous state
      const previousPosts = queryClient.getQueryData(['posts']);
      
      // Optimistically update
      queryClient.setQueryData(['posts'], (old) =>
        old.map(post =>
          post.id === updatedPost.id ? { ...post, ...updatedPost } : post
        )
      );
      
      return { previousPosts };
    },
    onError: (err, variables, context) => {
      // Rollback on error
      queryClient.setQueryData(['posts'], context.previousPosts);
    },
    onSettled: (data, error, variables) => {
      // Smart invalidation based on what changed
      if (variables) {
        // Only invalidate the specific post and lists containing it
        queryClient.invalidateQueries({ queryKey: ['post', variables.id] });
        queryClient.invalidateQueries({
          queryKey: ['posts'],
          predicate: (query) => {
            // Keep other posts lists intact if only one post changed
            return true; // Could be more specific
          },
        });
      }
    },
  });
}

/**
 * Invalidate and refetch specific queries
 */
async function refreshDashboard() {
  await queryClient.invalidateQueries({ queryKey: ['dashboard'] });
  // invalidateQueries marks as stale but doesn't fetch immediately
  // Components will refetch on next mount or focus
  
  // Or fetch immediately:
  await queryClient.refetchQueries({ queryKey: ['dashboard'] });
}
```

### Direct Cache Manipulation
Sometimes you need to directly modify the cache without invalidating and refetching.

```jsx
// File: src/components/CacheManipulation.jsx

import React from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';

function CacheManipulation() {
  const queryClient = useQueryClient();

  // Get current cache data (without triggering fetch)
  const handleGetCache = () => {
    const data = queryClient.getQueryData(['posts']);
    console.log('Cached posts:', data);
  };

  // Set cache data directly (e.g., from WebSocket)
  const handleSetCache = () => {
    queryClient.setQueryData(['posts'], (old) => [
      { id: 1, title: 'New from WebSocket', body: '' },
      ...(old || []),
    ]);
  };

  // Update specific item in cache
  const handleUpdateCache = (postId) => {
    queryClient.setQueryData(['posts'], (old) =>
      old?.map(post =>
        post.id === postId
          ? { ...post, title: 'Updated!', updated: true }
          : post
      )
    );
  };

  // Remove specific item from cache
  const handleRemoveFromCache = (postId) => {
    queryClient.setQueryData(['posts'], (old) =>
      old?.filter(post => post.id !== postId)
    );
  };

  // Clear all cache
  const handleClearAll = () => {
    queryClient.clear();
  };

  // Check if query is cached
  const handleCheckCache = () => {
    const isCached = queryClient.getQueryState(['posts'])?.data;
    console.log('Is cached:', !!isCached);
  };

  return (
    <div>
      <button onClick={handleGetCache}>Get Cache</button>
      <button onClick={handleSetCache}>Set Cache</button>
      <button onClick={handleCheckCache}>Check Cache</button>
      <button onClick={handleClearAll}>Clear All</button>
    </div>
  );
}

// WebSocket integration example
function usePostsWithWebSocket() {
  const queryClient = useQueryClient();

  React.useEffect(() => {
    const ws = new WebSocket('wss://api.example.com/ws');

    ws.onmessage = (event) => {
      const message = JSON.parse(event.data);

      if (message.type === 'POST_CREATED') {
        // Add new post to cache
        queryClient.setQueryData(['posts'], (old) => [
          message.data,
          ...(old || []),
        ]);
      } else if (message.type === 'POST_UPDATED') {
        // Update existing post in cache
        queryClient.setQueryData(['posts'], (old) =>
          old?.map(post =>
            post.id === message.data.id ? message.data : post
          )
        );
      } else if (message.type === 'POST_DELETED') {
        // Remove post from cache
        queryClient.setQueryData(['posts'], (old) =>
          old?.filter(post => post.id !== message.data.id)
        );
      }
    };

    return () => ws.close();
  }, [queryClient]);

  return useQuery({
    queryKey: ['posts'],
    queryFn: fetchPosts,
  });
}
```

## Common Mistakes

### Mistake 1: Not Understanding Stale vs. Cache
Many developers confuse staleTime and gcTime, leading to unexpected behavior.

```jsx
// ❌ WRONG — Confusing concepts
useQuery({
  queryKey: ['data'],
  queryFn: fetchData,
  staleTime: 0, // Data always stale but doesn't mean it will refetch immediately
  gcTime: 0, // Immediately garbage collected - data lost!
});

// ✅ CORRECT — Proper understanding
useQuery({
  queryKey: ['data'],
  queryFn: fetchData,
  staleTime: 5 * 60 * 1000, // Data fresh for 5 min, then refetched on next access
  gcTime: 10 * 60 * 1000, // Cached for 10 min, then garbage collected
});
```

### Mistake 2: Over-Invalidation
Invalidating too many queries forces unnecessary refetches.

```jsx
// ❌ WRONG — Invalidates everything on any post update
useMutation({
  mutationFn: updatePost,
  onSettled: () => {
    queryClient.invalidateQueries(); // Too broad!
  },
});

// ✅ CORRECT — Only invalidate affected queries
useMutation({
  mutationFn: updatePost,
  onSettled: () => {
    queryClient.invalidateQueries({ queryKey: ['posts'] });
  },
});
```

### Mistake 3: Not Using Structural Sharing
By default, TanStack Query applies structural sharing to maintain object references where possible.

```jsx
// ❌ WRONG — Mutating data directly in cache
queryClient.setQueryData(['posts'], (old) => {
  old.push(newPost); // Mutates original array!
  return old;
});

// ✅ CORRECT — Create new array
queryClient.setQueryData(['posts'], (old) => {
  return [...(old || []), newPost]; // New array, new reference
});
```

## Real-World Example
Building a comprehensive data management system with smart caching strategies.

```jsx
// File: src/lib/queryConfig.js

import { QueryClient } from '@tanstack/react-query';

// Create optimized QueryClient for production
export const createQueryClient = () => {
  return new QueryClient({
    defaultOptions: {
      // Global query defaults
      queries: {
        // Data is fresh for 5 minutes by default
        staleTime: 5 * 60 * 1000,
        
        // Keep unused data for 30 minutes
        gcTime: 30 * 60 * 1000,
        
        // Retry failed requests 3 times with exponential backoff
        retry: 3,
        retryDelay: (attemptIndex) =>
          Math.min(1000 * 2 ** attemptIndex, 30000),
        
        // Don't refetch on mount if data exists
        refetchOnMount: false,
        
        // Don't refetch on window focus unless data is stale
        refetchOnWindowFocus: 'always',
      },
      
      // Global mutation defaults
      mutations: {
        // Retry mutations once
        retry: 1,
        
        // Call onError on all mutations by default
        onError: (error) => {
          console.error('Mutation error:', error);
          // Could show toast notification here
        },
      },
    },
  });
};

// File: src/components/SmartPostManager.jsx

import React, { useCallback } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';

const POSTS_KEY = ['posts'];

// Optimized query with proper caching
function usePosts({ status } = {}) {
  return useQuery({
    queryKey: [...POSTS_KEY, { status }],
    queryFn: () => fetch(`/api/posts?status=${status}`).then(r => r.json()),
    // Keep previous data while fetching new filter
    placeholderData: (previousData) => previousData,
    // Only refetch if data is older than 1 minute
    staleTime: 60 * 1000,
  });
}

// Smart invalidation hook
function useSmartPosts() {
  const queryClient = useQueryClient();

  const query = usePosts();

  // Prefetch next page when user is near the end
  const prefetchNextPage = useCallback(() => {
    const nextPage = (query.data?.page || 0) + 1;
    queryClient.prefetchQuery({
      queryKey: [...POSTS_KEY, { page: nextPage }],
      queryFn: () => fetch(`/api/posts?page=${nextPage}`).then(r => r.json()),
    });
  }, [queryClient, query.data?.page]);

  // Update cache without refetch
  const updatePostLocally = useCallback((postId, updates) => {
    queryClient.setQueryData(POSTS_KEY, (old) =>
      old?.pages.map(page => ({
        ...page,
        data: page.data.map(post =>
          post.id === postId ? { ...post, ...updates } : post
        ),
      }))
    );
  }, [queryClient]);

  return {
    ...query,
    prefetchNextPage,
    updatePostLocally,
  };
}

// File: src/App.jsx - Setting up with React Query DevTools

import React from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { createQueryClient } from './lib/queryConfig';

const queryClient = createQueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <YourApp />
      {/* DevTools for debugging - only in development */}
      <ReactQueryDevtools 
        initialIsOpen={false}
        position="bottom-right"
      />
    </QueryClientProvider>
  );
}
```

## Key Takeaways
- staleTime determines how long data stays "fresh" before needing refetch
- gcTime determines how long unused data stays in cache before being garbage collected
- Use invalidateQueries to mark data as stale and trigger refetch
- Use setQueryData to directly modify cache without network requests
- Background refetching can be controlled with refetchOnMount, refetchOnWindowFocus, and refetchInterval
- Use placeholderData to keep showing old data while fetching updates
- React Query DevTools help visualize and debug cache behavior

## What's Next
Continue to [React Suspense for Data](../03-suspense-and-streaming/01-react-suspense-for-data.md) to learn how to integrate TanStack Query with React Suspense for an alternative data fetching approach.
