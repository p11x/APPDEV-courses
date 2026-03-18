# Queries and Mutations in TanStack Query

## Overview
Building on the basics from the setup guide, this document dives deeper into advanced TanStack Query patterns. You'll learn how to implement pagination, infinite scrolling, dependent queries, mutations with optimistic updates, and how to manage query invalidation for data consistency. These patterns are essential for building production-ready React applications.

## Prerequisites
- Completed TanStack Query setup guide
- Understanding of React hooks
- Knowledge of async/await and Promises
- Familiarity with REST APIs

## Core Concepts

### Paginated Queries
Implementing pagination allows users to load data in chunks rather than all at once, improving performance for large datasets.

```jsx
// File: src/components/PaginatedPosts.jsx

import React, { useState } from 'react';
import { useQuery } from '@tanstack/react-query';

const fetchPosts = async ({ queryKey }) => {
  const [_, { page, limit }] = queryKey;
  const response = await fetch(
    `https://jsonplaceholder.typicode.com/posts?_page=${page}&_limit=${limit}`
  );
  if (!response.ok) throw new Error('Failed to fetch posts');
  return response.json();
};

function PaginatedPosts() {
  // Track current page in component state
  const [page, setPage] = useState(1);
  const limit = 10;

  // Query automatically refetches when page changes
  // because page is part of the query key
  const { 
    data: posts,
    isLoading,
    isFetching, // Shows while background refetch is happening
    isError,
    error,
  } = useQuery({
    queryKey: ['posts', { page, limit }],
    queryFn: fetchPosts,
    // Keep previous data while fetching new page
    // Shows stale data instead of loading indicator
    placeholderData: (previousData) => previousData,
    // Or keep previous data for 5 minutes while fetching
    placeholderData: (previousData) => previousData,
    staleTime: 5 * 60 * 1000,
  });

  return (
    <div>
      <h2>Posts (Page {page})</h2>
      
      {/* Show loading indicator for initial load only */}
      {isLoading ? (
        <div>Loading...</div>
      ) : isError ? (
        <div>Error: {error.message}</div>
      ) : (
        <>
          {/* Show subtle loading for background fetches */}
          {isFetching && <span className="refreshing">Refreshing...</span>}
          
          <ul>
            {posts?.map(post => (
              <li key={post.id}>{post.title}</li>
            ))}
          </ul>
          
          {/* Pagination Controls */}
          <div className="pagination">
            <button
              onClick={() => setPage(p => Math.max(1, p - 1))}
              disabled={page === 1}
            >
              Previous
            </button>
            
            <span>Page {page}</span>
            
            <button
              onClick={() => setPage(p => p + 1)}
              disabled={posts?.length < limit} // Disable if no more data
            >
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
}

export default PaginatedPosts;
```

### Infinite Queries
Infinite scrolling automatically loads more data as users scroll, providing a seamless browsing experience.

```jsx
// File: src/components/InfinitePostList.jsx

import React from 'react';
import { useInfiniteQuery } from '@tanstack/react-query';

const fetchPostsInfinite = async ({ pageParam = 1 }) => {
  const response = await fetch(
    `https://jsonplaceholder.typicode.com/posts?_page=${pageParam}&_limit=10`
  );
  if (!response.ok) throw new Error('Failed to fetch posts');
  
  const data = await response.json();
  
  // Return data along with next page param
  // TanStack Query uses hasMore or nextId to determine if more data exists
  return {
    data,
    nextPage: data.length === 10 ? pageParam + 1 : undefined,
  };
};

function InfinitePostList() {
  const {
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    isLoading,
    isError,
    error,
    refetch,
  } = useInfiniteQuery({
    queryKey: ['posts', 'infinite'],
    queryFn: fetchPostsInfinite,
    // Initial page param
    initialPageParam: 1,
    // How to get the next page param from the result
    getNextPageParam: (lastPage) => lastPage.nextPage,
    // Maximum number of pages to fetch
    staleTime: 5 * 60 * 1000,
  });

  if (isLoading) return <div>Loading...</div>;
  
  if (isError) return (
    <div>
      <p>Error: {error.message}</p>
      <button onClick={() => refetch()}>Retry</button>
    </div>
  );

  return (
    <div>
      <h2>Infinite Post List</h2>
      
      {/* Flatten pages into single array */}
      {data?.pages.map((page, pageIndex) => (
        <React.Fragment key={pageIndex}>
          {page.data.map(post => (
            <div key={post.id} className="post-item">
              <h3>{post.title}</h3>
              <p>{post.body}</p>
            </div>
          ))}
        </React.Fragment>
      ))}
      
      {/* Load More Button / Infinite Scroll Trigger */}
      <div className="load-more">
        <button
          onClick={() => fetchNextPage()}
          disabled={!hasNextPage || isFetchingNextPage}
        >
          {isFetchingNextPage
            ? 'Loading more...'
            : hasNextPage
            ? 'Load More'
            : 'No more posts'}
        </button>
      </div>
      
      {/* Optional: Intersection Observer for automatic loading */}
      {hasNextPage && (
        <div ref={(element) => {
          if (element) {
            const observer = new IntersectionObserver((entries) => {
              if (entries[0].isIntersecting) {
                fetchNextPage();
              }
            });
            observer.observe(element);
          }
        }} />
      )}
    </div>
  );
}

export default InfinitePostList;
```

### Dependent Queries
Dependent queries only run after another query completes, useful when one query's data depends on another.

```jsx
// File: src/components/UserProfile.jsx

import React, { useState } from 'react';
import { useQuery } from '@tanstack/react-query';

// Fetch user by ID
const fetchUser = async ({ queryKey }) => {
  const [_, userId] = queryKey;
  const response = await fetch(
    `https://jsonplaceholder.typicode.com/users/${userId}`
  );
  if (!response.ok) throw new Error('Failed to fetch user');
  return response.json();
};

// Fetch user's posts
const fetchUserPosts = async ({ queryKey }) => {
  const [_, userId] = queryKey;
  const response = await fetch(
    `https://jsonplaceholder.typicode.com/posts?userId=${userId}`
  );
  if (!response.ok) throw new Error('Failed to fetch posts');
  return response.json();
};

function UserProfile({ userId }) {
  // First query: Fetch user - runs immediately
  const { 
    data: user, 
    isLoading: isUserLoading 
  } = useQuery({
    queryKey: ['user', userId],
    queryFn: fetchUser,
    enabled: !!userId, // Only fetch when userId exists
  });

  // Second query: Fetch posts - depends on user being loaded
  // enabled checks both: userId exists AND user query is successful
  const { 
    data: posts, 
    isLoading: isPostsLoading,
    isFetching: isPostsFetching,
  } = useQuery({
    queryKey: ['posts', 'user', userId], // Includes userId in key
    queryFn: fetchUserPosts,
    // Only fetch posts after user is loaded
    enabled: !!user,
  });

  if (isUserLoading) return <div>Loading user...</div>;
  if (!user) return <div>User not found</div>;

  return (
    <div>
      <h1>{user.name}</h1>
      <p>{user.email}</p>
      <p>{user.company.name}</p>
      
      <h2>Posts</h2>
      {isPostsLoading ? (
        <div>Loading posts...</div>
      ) : (
        <>
          {isPostsFetching && <span>Updating...</span>}
          <ul>
            {posts?.map(post => (
              <li key={post.id}>{post.title}</li>
            ))}
          </ul>
        </>
      )}
    </div>
  );
}

export default UserProfile;
```

### Mutations with Optimistic Updates
Optimistic updates provide instant feedback by updating the UI before the server responds, then rolling back if the request fails.

```jsx
// File: src/components/TodoList.jsx

import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';

const fetchTodos = async () => {
  const response = await fetch('https://jsonplaceholder.typicode.com/todos?_limit=10');
  if (!response.ok) throw new Error('Failed to fetch todos');
  return response.json();
};

const addTodo = async (newTodo) => {
  const response = await fetch('https://jsonplaceholder.typicode.com/todos', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(newTodo),
  });
  if (!response.ok) throw new Error('Failed to add todo');
  return response.json();
};

const toggleTodo = async ({ id, completed }) => {
  const response = await fetch(`https://jsonplaceholder.typicode.com/todos/${id}`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ completed }),
  });
  if (!response.ok) throw new Error('Failed to update todo');
  return response.json();
};

const deleteTodo = async (id) => {
  const response = await fetch(`https://jsonplaceholder.typicode.com/todos/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) throw new Error('Failed to delete todo');
  return id;
};

function TodoList() {
  const [newTodoTitle, setNewTodoTitle] = useState('');
  const queryClient = useQueryClient();

  // Fetch todos
  const { data: todos, isLoading, isError, error } = useQuery({
    queryKey: ['todos'],
    queryFn: fetchTodos,
  });

  // Add todo mutation with optimistic update
  const addTodoMutation = useMutation({
    mutationFn: addTodo,
    // Called before mutationFn
    onMutate: async (newTodo) => {
      // Cancel any outgoing refetches
      await queryClient.cancelQueries({ queryKey: ['todos'] });

      // Snapshot previous todos
      const previousTodos = queryClient.getQueryData(['todos']);

      // Optimistically add new todo
      queryClient.setQueryData(['todos'], (old) => {
        const optimisticTodo = {
          ...newTodo,
          id: Date.now(), // Temporary ID
          completed: false,
        };
        return [optimisticTodo, ...(old || [])];
      });

      // Return context for rollback
      return { previousTodos };
    },
    // If error, rollback to previous state
    onError: (err, newTodo, context) => {
      queryClient.setQueryData(['todos'], context.previousTodos);
    },
    // Always refetch after error or success
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ['todos'] });
    },
  });

  // Toggle todo mutation with optimistic update
  const toggleMutation = useMutation({
    mutationFn: toggleTodo,
    onMutate: async ({ id, completed }) => {
      await queryClient.cancelQueries({ queryKey: ['todos'] });
      const previousTodos = queryClient.getQueryData(['todos']);

      queryClient.setQueryData(['todos'], (old) =>
        (old || []).map(todo =>
          todo.id === id ? { ...todo, completed } : todo
        )
      );

      return { previousTodos };
    },
    onError: (err, variables, context) => {
      queryClient.setQueryData(['todos'], context.previousTodos);
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ['todos'] });
    },
  });

  // Delete todo mutation
  const deleteMutation = useMutation({
    mutationFn: deleteTodo,
    onMutate: async (id) => {
      await queryClient.cancelQueries({ queryKey: ['todos'] });
      const previousTodos = queryClient.getQueryData(['todos']);

      queryClient.setQueryData(['todos'], (old) =>
        (old || []).filter(todo => todo.id !== id)
      );

      return { previousTodos };
    },
    onError: (err, id, context) => {
      queryClient.setQueryData(['todos'], context.previousTodos);
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ['todos'] });
    },
  });

  const handleAddTodo = (e) => {
    e.preventDefault();
    if (!newTodoTitle.trim()) return;
    
    addTodoMutation.mutate({ title: newTodoTitle, userId: 1 });
    setNewTodoTitle('');
  };

  if (isLoading) return <div>Loading todos...</div>;
  if (isError) return <div>Error: {error.message}</div>;

  return (
    <div>
      <h2>Todo List</h2>
      
      {/* Add Todo Form */}
      <form onSubmit={handleAddTodo}>
        <input
          value={newTodoTitle}
          onChange={(e) => setNewTodoTitle(e.target.value)}
          placeholder="Add new todo..."
        />
        <button 
          type="submit"
          disabled={addTodoMutation.isPending}
        >
          {addTodoMutation.isPending ? 'Adding...' : 'Add'}
        </button>
      </form>
      
      {/* Error from mutations */}
      {addTodoMutation.isError && (
        <p className="error">Failed to add todo</p>
      )}
      
      {/* Todo List */}
      <ul>
        {todos?.map(todo => (
          <li key={todo.id} className={todo.completed ? 'completed' : ''}>
            <input
              type="checkbox"
              checked={todo.completed}
              onChange={() => toggleMutation.mutate({
                id: todo.id,
                completed: !todo.completed
              })}
              disabled={toggleMutation.isPending}
            />
            <span>{todo.title}</span>
            <button
              onClick={() => deleteMutation.mutate(todo.id)}
              disabled={deleteMutation.isPending}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default TodoList;
```

### Query Invalidation Strategies
Proper query invalidation ensures data consistency after mutations, making sure the UI always shows accurate data.

```jsx
// File: src/lib/queryHelpers.js

import { queryClient } from '../main';

/**
 * Invalidate all queries related to a specific topic
 * Call this after mutations that affect multiple queries
 */
export function invalidateRelatedQueries() {
  // Invalidate all 'posts' queries
  queryClient.invalidateQueries({ queryKey: ['posts'] });
  
  // Invalidate all queries starting with 'posts'
  queryClient.invalidateQueries({ queryKey: ['posts'] }); // Matches ['posts'], ['posts', id], etc.
  
  // Invalidate specific pattern
  queryClient.invalidateQueries({
    queryKey: ['comments'],
    predicate: (query) => {
      // Only invalidate comments for specific post
      const postId = query.queryKey[1]?.postId;
      return postId === 123;
    },
  });
}

/**
 * Update query data directly without refetching
 */
export function updateTodoInCache(todoId, updates) {
  queryClient.setQueryData(['todos'], (old) =>
    (old || []).map(todo =>
      todo.id === todoId ? { ...todo, ...updates } : todo
    )
  );
}

/**
 * Prefetch data for anticipated user actions
 */
export async function prefetchPost(postId) {
  await queryClient.prefetchQuery({
    queryKey: ['post', postId],
    queryFn: () => fetch(`/api/posts/${postId}`).then(r => r.json()),
    // Keep prefetched data for 5 minutes
    staleTime: 5 * 60 * 1000,
  });
}

/**
 * Set data directly (e.g., from WebSocket)
 */
export function updateFromWebSocket(data) {
  queryClient.setQueryData(['posts'], (old) => {
    // Update or add the new post
    const exists = old?.find(p => p.id === data.id);
    if (exists) {
      return old.map(p => p.id === data.id ? data : p);
    }
    return [data, ...(old || [])];
  });
}
```

## Common Mistakes

### Mistake 1: Not Providing Unique Query Keys
Each query needs a unique key. Sharing keys between different queries causes data to overwrite.

```jsx
// ❌ WRONG — Different queries with same key
useQuery({ queryKey: ['data'], queryFn: fetchUsers });
useQuery({ queryKey: ['data'], queryFn: fetchPosts }); // Overwrites!

// ✅ CORRECT — Unique keys
useQuery({ queryKey: ['users'], queryFn: fetchUsers });
useQuery({ queryKey: ['posts'], queryFn: fetchPosts });

// Or with IDs
useQuery({ queryKey: ['user', userId], queryFn: () => fetchUser(userId) });
useQuery({ queryKey: ['post', postId], queryFn: () => fetchPost(postId) });
```

### Mistake 2: Not Handling Pending Mutations
Always check mutation state to provide feedback during async operations.

```jsx
// ❌ WRONG — No feedback during mutation
<button onClick={() => mutate(data)}>Save</button>

// ✅ CORRECT — Show loading state
<button 
  onClick={() => mutate(data)}
  disabled={isPending}
>
  {isPending ? 'Saving...' : 'Save'}
</button>
```

### Mistake 3: Not Invalidating After Mutations
After creating/updating/deleting data, invalidate queries to refetch and show accurate data.

```jsx// ❌ WRONG — Cache not updated after create
const createTodo = useMutation({
  mutationFn: createTodoApi,
  // Missing onSettled - cache not updated!
});

// ✅ CORRECT — Invalidate to refetch
const createTodo = useMutation({
  mutationFn: createTodoApi,
  onSettled: () => {
    queryClient.invalidateQueries({ queryKey: ['todos'] });
  },
});
```

## Real-World Example
Building a complete blog management system with posts, comments, and real-time updates.

```jsx
// File: src/hooks/useBlog.js

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import axios from 'axios';

const api = axios.create({ baseURL: '/api' });

// Blog hooks - comprehensive example of all patterns
export function usePosts(filters = {}) {
  return useQuery({
    queryKey: ['posts', filters],
    queryFn: async () => {
      const { data } = await api.get('/posts', { params: filters });
      return data;
    },
    // Keep previous data while fetching new filters
    placeholderData: (previousData) => previousData,
  });
}

export function usePost(postId) {
  return useQuery({
    queryKey: ['post', postId],
    queryFn: () => api.get(`/posts/${postId}`).then(r => r.data),
    enabled: !!postId,
    // Stale time of 0 since posts are dynamic
    staleTime: 0,
  });
}

export function useCreatePost() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (newPost) => api.post('/posts', newPost),
    onMutate: async (newPost) => {
      await queryClient.cancelQueries({ queryKey: ['posts'] });
      const previousPosts = queryClient.getQueryData(['posts', {}]);
      
      queryClient.setQueryData(['posts', {}], (old = []) => [{
        ...newPost,
        id: `temp-${Date.now()}`,
        createdAt: new Date().toISOString(),
      }, ...old]);
      
      return { previousPosts };
    },
    onError: (err, newPost, context) => {
      queryClient.setQueryData(['posts', {}], context.previousPosts);
    },
    onSettled: () => {
      // Refetch to get server data with real ID
      queryClient.invalidateQueries({ queryKey: ['posts'] });
    },
  });
}

export function useUpdatePost() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ id, ...updates }) => api.patch(`/posts/${id}`, updates),
    onMutate: async ({ id, ...updates }) => {
      await queryClient.cancelQueries({ queryKey: ['posts'] });
      
      // Update the specific post in cache
      queryClient.setQueryData(['posts', {}], (old = []) =>
        old.map(post => post.id === id ? { ...post, ...updates } : post)
      );
      
      // Also update the individual post cache
      queryClient.setQueryData(['post', id], (old) => ({
        ...old,
        ...updates,
      }));
    },
    onSettled: (_, { id }) => {
      queryClient.invalidateQueries({ queryKey: ['posts'] });
      queryClient.invalidateQueries({ queryKey: ['post', id] });
    },
  });
}

export function useDeletePost() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (id) => api.delete(`/posts/${id}`),
    onMutate: async (id) => {
      await queryClient.cancelQueries({ queryKey: ['posts'] });
      const previousPosts = queryClient.getQueryData(['posts', {}]);
      
      queryClient.setQueryData(['posts', {}], (old = []) =>
        old.filter(post => post.id !== id)
      );
      
      return { previousPosts };
    },
    onError: (err, id, context) => {
      queryClient.setQueryData(['posts', {}], context.previousPosts);
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ['posts'] });
    },
  });
}

// Dependent query: comments depend on post
export function useComments(postId) {
  return useQuery({
    queryKey: ['comments', { postId }],
    queryFn: () => api.get(`/posts/${postId}/comments`).then(r => r.data),
    enabled: !!postId,
  });
}
```

## Key Takeaways
- Use placeholderData to keep showing old data while fetching new pages
- Infinite queries use getNextPageParam to determine pagination
- Dependent queries use enabled to wait for parent data
- Optimistic updates provide instant UI feedback
- Always invalidate queries after mutations for data consistency
- Query keys should include all variables that affect the data
- Mutations provide isPending, isError, and isSuccess states

## What's Next
Continue to [Caching and Invalidation](03-caching-and-invalidation.md) to learn advanced cache management techniques, background updates, and performance optimization strategies.
