# Zustand Introduction and Setup

## Overview
Zustand is a small, fast, and scalable state management library for React. It uses a simplified hook-based approach that avoids the complexity of Redux while providing powerful features like middleware, transient updates, and TypeScript support.

## Prerequisites
- Basic React knowledge
- Understanding of state management concepts
- Familiarity with hooks

## Core Concepts

### Creating a Zustand Store
Zustand stores are created using the create function from zustand.

```jsx
// File: src/store/useStore.js

import { create } from 'zustand';

// Simple store with state and actions
const useStore = create((set) => ({
  // State
  count: 0,
  user: null,
  posts: [],
  
  // Actions - update state
  increment: () => set((state) => ({ count: state.count + 1 })),
  
  decrement: () => set((state) => ({ count: state.count - 1 })),
  
  reset: () => set({ count: 0 }),
  
  setUser: (user) => set({ user }),
  
  setPosts: (posts) => set({ posts }),
  
  addPost: (post) => set((state) => ({ 
    posts: [...state.posts, post] 
  })),
}));

export default useStore;
```

### Using the Store
Components can access state and actions directly from the hook.

```jsx
// File: src/components/Counter.jsx

import React from 'react';
import useStore from '../store/useStore';

function Counter() {
  const count = useStore((state) => state.count);
  const increment = useStore((state) => state.increment);
  const decrement = useStore((state) => state.decrement);

  return (
    <div>
      <h2>Count: {count}</h2>
      <button onClick={decrement}>-</button>
      <button onClick={increment}>+</button>
    </div>
  );
}

export default Counter;
```

### Selecting State
Zustand allows selecting specific parts of state for optimal re-renders.

```jsx
// File: src/components/UserPanel.jsx

import React from 'react';
import useStore from '../store/useStore';

function UserPanel() {
  // Only re-renders when user changes
  const user = useStore((state) => state.user);
  
  // Only re-renders when count changes
  const count = useStore((state) => state.count);
  
  // Multiple selections in one component
  const { user, count, posts } = useStore((state) => ({
    user: state.user,
    count: state.count,
    posts: state.posts,
  }));

  return (
    <div>
      <p>User: {user?.name}</p>
      <p>Count: {count}</p>
      <p>Posts: {posts.length}</p>
    </div>
  );
}

export default UserPanel;
```

### Async Actions
Zustand handles async actions naturally.

```jsx
// File: src/store/asyncStore.js

import { create } from 'zustand';

const useAsyncStore = create((set, get) => ({
  // Async action
  fetchUser: async (userId) => {
    set({ isLoading: true, error: null });
    
    try {
      const response = await fetch(`/api/users/${userId}`);
      const user = await response.json();
      set({ user, isLoading: false });
    } catch (error) {
      set({ error: error.message, isLoading: false });
    }
  },
  
  // Async with multiple values
  fetchPosts: async () => {
    set({ isFetching: true });
    
    try {
      const [usersRes, postsRes] = await Promise.all([
        fetch('/api/users'),
        fetch('/api/posts'),
      ]);
      
      const [users, posts] = await Promise.all([
        usersRes.json(),
        postsRes.json(),
      ]);
      
      set({ users, posts, isFetching: false });
    } catch (error) {
      set({ error: error.message, isFetching: false });
    }
  },
}));

export default useAsyncStore;
```

### Zustand with TypeScript
Zustand provides excellent TypeScript support.

```tsx
// File: src/store/typedStore.ts

import { create } from 'zustand';

interface User {
  id: number;
  name: string;
  email: string;
}

interface StoreState {
  user: User | null;
  isLoading: boolean;
  error: string | null;
  setUser: (user: User | null) => void;
  fetchUser: (id: number) => Promise<void>;
}

const useStore = create<StoreState>((set) => ({
  user: null,
  isLoading: false,
  error: null,
  
  setUser: (user) => set({ user }),
  
  fetchUser: async (id) => {
    set({ isLoading: true, error: null });
    try {
      const response = await fetch(`/api/users/${id}`);
      const user = await response.json();
      set({ user, isLoading: false });
    } catch (error) {
      set({ error: error.message, isLoading: false });
    }
  },
}));

export default useStore;
```

## Common Mistakes

### Selecting Too Much State
Selecting unnecessary state causes extra re-renders.

```jsx
// ❌ WRONG - Selects entire state
const { user, posts, count } = useStore();

// ✅ CORRECT - Select only what you need
const user = useStore((state) => state.user);
```

### Not Using Shallow Comparison
Multiple values need shallow comparison.

```jsx
// ❌ WRONG - Object created every render
const { name, count } = useStore();

// ✅ CORRECT - Use shallow from zustand
import { shallow } from 'zustand/shallow';

const data = useStore(
  (state) => ({ name: state.name, count: state.count }),
  shallow
);
```

## Key Takeaways
- Zustand stores are simple hook-based state management
- Select only the state you need for optimal re-renders
- Supports both sync and async actions
- Excellent TypeScript support
- No Provider wrapper needed

## What's Next
Continue to [Zustand Advanced Patterns](02-zustand-advanced-patterns.md) to learn about middleware, transient updates, and more advanced usage.
