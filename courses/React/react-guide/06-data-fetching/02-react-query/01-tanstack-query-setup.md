# TanStack Query Setup and Introduction

## Overview
TanStack Query (formerly React Query) is a powerful data fetching library that simplifies server state management in React applications. It handles caching, background refetching, optimistic updates, and pagination out of the box, eliminating much of the boilerplate code required for data fetching. This guide will walk you through setting up TanStack Query and understanding its core concepts.

## Prerequisites
- Understanding of React hooks and components
- Knowledge of async/await and Promises
- Familiarity with HTTP methods (GET, POST, PUT, DELETE)
- Basic understanding of state management in React
- Node.js and npm/pnpm installed

## Core Concepts

### Installing TanStack Query
TanStack Query can be installed via npm, yarn, or pnpm. The package is now scoped under @tanstack, so make sure you install the correct package name.

```bash
# Using npm
npm install @tanstack/react-query

# Using yarn
yarn add @tanstack/react-query

# Using pnpm
pnpm add @tanstack/react-query
```

### Setting Up the QueryClient
Before using any TanStack Query hooks, you need to wrap your application with the QueryClientProvider and create a QueryClient instance. The QueryClient is the main entry point for configuring caching behavior and managing query cache.

```jsx
// File: src/main.jsx

import React from 'react';
import ReactDOM from 'react-dom/client';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import App from './App';
import './index.css';

// Create a QueryClient with default options
// These options apply to all queries unless overridden
const queryClient = new QueryClient({
  // Default options for queries
  defaultOptions: {
    queries: {
      // Time in milliseconds before cached data is considered stale
      // Default is 0, meaning data is immediately stale after fetch
      staleTime: 5 * 60 * 1000, // 5 minutes
      
      // Time in milliseconds before unused data is garbage collected
      // Default is 10 minutes
      gcTime: 10 * 60 * 1000, // 10 minutes (formerly cacheTime)
      
      // Number of times to retry failed requests
      // Default is 3
      retry: 3,
      
      // Delay between retries in milliseconds
      retryDelay: (attemptIndex) => Math.min(1000 * 2 ** attemptIndex, 30000),
      
      // Don't fetch on window focus by default (can be enabled per-query)
      refetchOnWindowFocus: false,
      
      // Don't refetch when component mounts (use refetchOnMount for specific behavior)
      refetchOnMount: true,
    },
  },
});

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    {/* Wrap your entire app with QueryClientProvider */}
    <QueryClientProvider client={queryClient}>
      <App />
    </QueryClientProvider>
  </React.StrictMode>
);
```

### Your First Query with useQuery
The useQuery hook is the primary way to fetch data in TanStack Query. It automatically handles caching, background refetching, and error states, drastically reducing the code you need to write.

```jsx
// File: src/components/UserList.jsx

import React from 'react';
import { useQuery } from '@tanstack/react-query';

// Define a function that fetches the data
// This should be a pure function that returns a Promise
const fetchUsers = async () => {
  const response = await fetch('https://jsonplaceholder.typicode.com/users');
  
  // TanStack Query doesn't throw on 4xx/5xx by default
  // You need to check and throw manually if needed
  if (!response.ok) {
    throw new Error('Failed to fetch users');
  }
  
  return response.json();
};

function UserList() {
  // useQuery takes an options object with required and optional properties
  const { 
    data,           // The returned data (undefined until loading finishes)
    isError,        // Boolean: true if query threw an error
    error,          // The error object if isError is true
    isLoading,      // Boolean: true while query is executing and no cached data
    isFetching,     // Boolean: true while fetching (including background refetch)
    isSuccess,      // Boolean: true if query has data without error
    refetch,        // Function to manually refetch the query
    remove,         // Function to remove the query from cache
  } = useQuery({
    // Unique key for this query - used for caching and invalidation
    // When this key changes, React Query knows to refetch
    queryKey: ['users'],
    
    // The function that fetches the data
    queryFn: fetchUsers,
    
    // Optional: Configure behavior
    staleTime: 5 * 60 * 1000, // Data stays fresh for 5 minutes
    enabled: true, // Whether to automatically fetch (can be controlled)
  });

  // Loading state - shown when no cached data exists
  if (isLoading) {
    return <div>Loading users...</div>;
  }

  // Error state
  if (isError) {
    return (
      <div>
        <p>Error: {error.message}</p>
        <button onClick={() => refetch()}>Try Again</button>
      </div>
    );
  }

  // Success state - render the data
  return (
    <div>
      {/* Show fetching indicator even with cached data */}
      {isFetching && <span className="loading-indicator">Updating...</span>}
      
      <ul>
        {data?.map(user => (
          <li key={user.id}>{user.name}</li>
        ))}
      </ul>
    </div>
  );
}

export default UserList;
```

### Understanding Query Keys
Query keys are crucial for TanStack Query's caching mechanism. They should uniquely identify your data and include any variables that affect the fetched data.

```jsx
// File: src/components/Post.jsx

import React from 'react';
import { useQuery } from '@tanstack/react-query';

// Query key with variables - include dynamic values in the key
// This ensures different parameters create different cache entries
const fetchPost = async ({ queryKey }) => {
  const [_, { postId }] = queryKey;
  const response = await fetch(
    `https://jsonplaceholder.typicode.com/posts/${postId}`
  );
  if (!response.ok) throw new Error('Failed to fetch post');
  return response.json();
};

function Post({ postId }) {
  // Pass variables via the queryKey
  // The key ['posts', { postId }] uniquely identifies this specific post
  const { data: post, isLoading } = useQuery({
    queryKey: ['posts', { postId }], // Different from queryKey: ['posts', postId]
    queryFn: fetchPost,
    enabled: !!postId, // Only fetch when postId exists
  });

  if (isLoading) return <div>Loading post...</div>;

  return (
    <article>
      <h1>{post.title}</h1>
      <p>{post.body}</p>
    </article>
  );
}

// File: src/components/PostList.jsx

function PostList({ userId, status }) {
  // Query key as array with multiple variables
  // Changing userId or status will trigger a refetch
  const { data: posts } = useQuery({
    queryKey: ['posts', { userId, status }], // ['posts', { userId: 1, status: 'published' }]
    queryFn: async () => {
      const params = new URLSearchParams();
      if (userId) params.append('userId', userId);
      if (status) params.append('status', status);
      
      const response = await fetch(
        `https://jsonplaceholder.typicode.com/posts?${params}`
      );
      return response.json();
    },
    // Query won't run until userId is truthy
    enabled: !!userId,
  });

  return (
    <ul>
      {posts?.map(post => (
        <li key={post.id}>{post.title}</li>
      ))}
    </ul>
  );
}
```

### Query Key Factories
For large applications, create utility functions to generate consistent query keys across your codebase.

```jsx
// File: src/utils/queryKeys.js

// Centralized query key factory
// Ensures consistent naming across the app
export const queryKeys = {
  // Users
  users: ['users'] as const,
  user: (id: number) => ['users', id] as const,
  
  // Posts with filters
  posts: (filters?: object) => ['posts', filters] as const,
  post: (id: number) => ['posts', id] as const,
  
  // Comments
  comments: (postId: number) => ['comments', postId] as const,
};

// Usage in components:
// useQuery({ queryKey: queryKeys.users })
// useQuery({ queryKey: queryKeys.user(123) })
// useQuery({ queryKey: queryKeys.posts({ status: 'published' }) })
```

### Initial Data and Placeholder Data
TanStack Query supports providing initial data to show while fetching or as a fallback, improving perceived performance.

```jsx
// File: src/components/UserListWithInitialData.jsx

import React from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';

// Simulated initial data (could come from SSR/SSG)
const initialUsers = [
  { id: 1, name: 'Initial User' },
];

function UserListWithInitialData() {
  const { data: users, isLoading } = useQuery({
    queryKey: ['users'],
    queryFn: async () => {
      const response = await fetch('https://jsonplaceholder.typicode.com/users');
      return response.json();
    },
    // Provide initial data to show immediately
    // Shows this data while fetching in the background
    initialData: initialUsers,
    
    // OR: Use initialDataUpdatedAt to show stale indicator
    // initialDataUpdatedAt: Date.now() - 60000, // Data is 1 minute old
  });

  if (isLoading && !users) {
    return <div>Loading...</div>;
  }

  return (
    <ul>
      {users?.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}

// File: src/components/OptimisticUpdate.jsx

function OptimisticUpdate() {
  const queryClient = useQueryClient();

  // Pre-populate query cache before component mounts
  // Useful when navigating to a page that needs data immediately
  React.useEffect(() => {
    queryClient.setQueryData(['initial-data'], { 
      message: 'Pre-loaded data' 
    });
  }, [queryClient]);

  const { data } = useQuery({
    queryKey: ['initial-data'],
    queryFn: () => fetch('/api/data').then(r => r.json()),
    // Don't fetch since we set initial data manually
    enabled: false,
  });

  return <div>{data?.message}</div>;
}
```

## Common Mistakes

### Mistake 1: Forgetting Query Keys
Query keys are required and should uniquely identify your data. Using the same key for different queries causes data to overwrite incorrectly.

```jsx
// ❌ WRONG — Same key for different queries
useQuery({ queryKey: ['data'], queryFn: fetchUserData });
useQuery({ queryKey: ['data'], queryFn: fetchPostData }); // Overwrites!

// ✅ CORRECT — Unique keys for different data
useQuery({ queryKey: ['user'], queryFn: fetchUserData });
useQuery({ queryKey: ['posts'], queryFn: fetchPostData });

// Or with variables
useQuery({ queryKey: ['user', userId], queryFn: () => fetchUser(userId) });
useQuery({ queryKey: ['post', postId], queryFn: () => fetchPost(postId) });
```

### Mistake 2: Not Handling Undefined Data
useQuery returns undefined until data is loaded. Always handle loading and undefined states.

```jsx
// ❌ WRONG — Accessing data without checking
function UserList() {
  const { data } = useQuery({ queryKey: ['users'], queryFn: fetchUsers });
  
  // Crashes if data is undefined
  return <ul>{data.map(u => <li>{u.name}</li>)}</ul>;
}

// ✅ CORRECT — Handle all states
function UserList() {
  const { data, isLoading } = useQuery({ queryKey: ['users'], queryFn: fetchUsers });
  
  if (isLoading) return <div>Loading...</div>;
  
  return (
    <ul>
      {data?.map(u => <li key={u.id}>{u.name}</li>)}
    </ul>
  );
}
```

### Mistake 3: Not Using enabled Properly
The enabled option controls when a query should run. Not using it can cause unnecessary requests or errors.

```jsx
// ❌ WRONG — Query runs with undefined variable
function Post({ postId }) {
  const { data } = useQuery({
    queryKey: ['post', postId],
    queryFn: () => fetch(`/posts/${postId}`).then(r => r.json()),
    // If postId is undefined initially, query might fail or show loading
  });
  // ...
}

// ✅ CORRECT — Only fetch when postId exists
function Post({ postId }) {
  const { data } = useQuery({
    queryKey: ['post', postId],
    queryFn: () => fetch(`/posts/${postId}`).then(r => r.json()),
    enabled: !!postId, // Won't fetch until postId is defined
  });
  // ...
}
```

## Real-World Example
Building a complete data fetching layer with TanStack Query, including mutations and cache invalidation.

```jsx
// File: src/lib/api.js

import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://jsonplaceholder.typicode.com',
});

// API functions that TanStack Query will use
export const api = {
  // Posts
  getPosts: async () => {
    const { data } = await apiClient.get('/posts');
    return data;
  },
  
  getPost: async (postId) => {
    const { data } = await apiClient.get(`/posts/${postId}`);
    return data;
  },
  
  createPost: async (newPost) => {
    const { data } = await apiClient.post('/posts', newPost);
    return data;
  },
  
  updatePost: async (updatedPost) => {
    const { data } = await apiClient.put(
      `/posts/${updatedPost.id}`, 
      updatedPost
    );
    return data;
  },
  
  deletePost: async (postId) => {
    await apiClient.delete(`/posts/${postId}`);
    return postId;
  },
};

// File: src/hooks/usePosts.js

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { api } from '../lib/api';

export function usePosts() {
  return useQuery({
    queryKey: ['posts'],
    queryFn: api.getPosts,
  });
}

export function usePost(postId) {
  return useQuery({
    queryKey: ['post', postId],
    queryFn: () => api.getPost(postId),
    enabled: !!postId,
  });
}

export function useCreatePost() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: api.createPost,
    // Called before mutationFn
    onMutate: async (newPost) => {
      // Cancel any outgoing refetches
      await queryClient.cancelQueries({ queryKey: ['posts'] });
      
      // Snapshot previous value
      const previousPosts = queryClient.getQueryData(['posts']);
      
      // Optimistically update cache
      queryClient.setQueryData(['posts'], (old) => [
        ...(old || []),
        { ...newPost, id: Date.now() }, // Temporary ID
      ]);
      
      // Return context with previous posts for rollback
      return { previousPosts };
    },
    // Called on error - rollback
    onError: (err, newPost, context) => {
      queryClient.setQueryData(['posts'], context.previousPosts);
    },
    // Always refetch after error or success
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ['posts'] });
    },
  });
}

export function useDeletePost() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: api.deletePost,
    onSuccess: (deletedPostId) => {
      // Update cache by removing the deleted post
      queryClient.setQueryData(['posts'], (old) =>
        (old || []).filter(post => post.id !== deletedPostId)
      );
    },
  });
}

// File: src/components/PostManager.jsx

import React, { useState } from 'react';
import { usePosts, useCreatePost, useDeletePost } from '../hooks/usePosts';

function PostManager() {
  const [title, setTitle] = useState('');
  const [body, setBody] = useState('');
  
  const { data: posts, isLoading, isError, error, refetch } = usePosts();
  const createPost = useCreatePost();
  const deletePost = useDeletePost();

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    createPost.mutate(
      { title, body, userId: 1 },
      {
        onSuccess: () => {
          setTitle('');
          setBody('');
        },
      }
    );
  };

  if (isLoading) return <div>Loading posts...</div>;
  
  if (isError) return (
    <div>
      <p>Error: {error.message}</p>
      <button onClick={() => refetch()}>Retry</button>
    </div>
  );

  return (
    <div>
      <h2>Posts</h2>
      
      {/* Create Post Form */}
      <form onSubmit={handleSubmit}>
        <input
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="Post title"
          required
        />
        <textarea
          value={body}
          onChange={(e) => setBody(e.target.value)}
          placeholder="Post body"
          required
        />
        <button 
          type="submit"
          disabled={createPost.isPending}
        >
          {createPost.isPending ? 'Creating...' : 'Create Post'}
        </button>
      </form>
      
      {/* Error from mutation */}
      {createPost.isError && (
        <p>Error creating post: {createPost.error.message}</p>
      )}
      
      {/* Post List */}
      <ul>
        {posts?.map(post => (
          <li key={post.id}>
            <h3>{post.title}</h3>
            <p>{post.body}</p>
            <button
              onClick={() => deletePost.mutate(post.id)}
              disabled={deletePost.isPending}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default PostManager;
```

## Key Takeaways
- TanStack Query handles caching, background refetching, and error states automatically
- Use useQuery for fetching data and useMutation for modifying data
- Query keys uniquely identify cached data - include dynamic variables in keys
- The enabled option controls when queries should run
- Use initialData to provide fallback data while loading
- Mutations support optimistic updates for better UX
- Always handle isLoading, isError, and isSuccess states in your UI

## What's Next
Continue to [Queries and Mutations](02-queries-and-mutations.md) to dive deeper into advanced query patterns, pagination, infinite scrolling, and mutation best practices.
