# State Colocation Strategy in React

## Overview
State colocation is the practice of placing state as close as possible to where it's used. This principle helps reduce unnecessary re-renders, makes code more maintainable, and improves performance by avoiding prop drilling.

## Prerequisites
- Understanding of React component lifecycle
- Knowledge of useState and useEffect hooks
- Familiarity with re-rendering in React

## Core Concepts

### The Problem with Global State
Placing all state at the top level causes unnecessary re-renders throughout the app.

```jsx
// ❌ PROBLEM: State at top level causes cascading re-renders
function App() {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [user, setUser] = useState(null);
  const [posts, setPosts] = useState([]);
  const [theme, setTheme] = useState('light');

  return (
    <div>
      <Header theme={theme} setTheme={setTheme} />
      <Sidebar open={sidebarOpen} setOpen={setSidebarOpen} />
      <Main posts={posts} setPosts={setPosts} user={user} />
    </div>
  );
}

function Main({ posts }) {
  // Re-renders when ANY state changes, even unrelated ones
  return <PostList posts={posts} />;
}
```

### The Solution: Colocate State
Place state where it's actually used.

```jsx
// ✅ SOLUTION: Colocate state to reduce re-renders

function App() {
  // Only app-level state here
  return (
    <div>
      <Header />
      <SidebarWithState />
      <Main />
    </div>
  );
}

// Each component manages its own relevant state
function SidebarWithState() {
  const [isOpen, setIsOpen] = useState(false);
  return <Sidebar open={isOpen} onToggle={() => setIsOpen(!isOpen)} />;
}

function ThemeToggle() {
  const [theme, setTheme] = useState('light');
  return <button onClick={() => setTheme(t => t === 'light' ? 'dark' : 'light')}>{theme}</button>;
}

function PostListWithData() {
  const [posts, setPosts] = useState([]);
  // Only re-renders when posts change
  return <PostList posts={posts} />;
}
```

### Colocation with Custom Hooks
Extract and colocate logic using custom hooks.

```jsx
// File: src/hooks/useSearch.js

import { useState, useCallback } from 'react';

// Colocated logic in a custom hook
function useSearch(searchFn) {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  const search = useCallback(async (q) => {
    setQuery(q);
    if (!q.trim()) {
      setResults([]);
      return;
    }
    
    setIsLoading(true);
    try {
      const data = await searchFn(q);
      setResults(data);
    } finally {
      setIsLoading(false);
    }
  }, [searchFn]);

  return { query, results, isLoading, search };
}

// Usage - state is colocated with the component that uses it
function SearchComponent() {
  const { query, results, isLoading, search } = useSearch(api.search);
  
  return (
    <div>
      <input value={query} onChange={e => search(e.target.value)} />
      {isLoading ? <Loading /> : results.map(r => <Item key={r.id} {...r} />)}
    </div>
  );
}
```

## Common Mistakes

### Over-Colocation
Don't colocate state that genuinely needs to be shared.

```jsx
// ❌ WRONG - Over-colocating creates duplicate state
function Parent() {
  const [count, setCount] = useState(0);
  return <Child count={count} />;
}

function Child({ count }) {
  const [localCount, setLocalCount] = useState(count); // Duplicate!
  return <div>{localCount}</div>;
}

// ✅ CORRECT - Share state at appropriate level
function Parent() {
  const [count, setCount] = useState(0);
  return <Child count={count} onIncrement={() => setCount(c => c + 1)} />;
}

function Child({ count, onIncrement }) {
  return <button onClick={onIncrement}>{count}</button>;
}
```

## Key Takeaways
- Place state as close as possible to where it's used
- This reduces unnecessary re-renders
- Use custom hooks to encapsulate colocated logic
- Balance between colocation and necessary sharing

## What's Next
Continue to [Creating and Providing Context](02-context-api/01-creating-and-providing-context.md) to learn about React Context API for sharing state across components.
