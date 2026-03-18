# Building Custom Hooks

## Overview

Custom hooks are JavaScript functions that let you extract component logic into reusable functions. A custom hook is a function that uses other hooks and starts with "use" (e.g., useAuth, useFetch, useLocalStorage). Custom hooks are the best way to share logic between components, replacing HOCs and render props in modern React.

## Prerequisites

- Understanding of React hooks (useState, useEffect)
- Knowledge of component composition
- Familiarity with JavaScript functions

## Core Concepts

### What are Custom Hooks?

```jsx
// File: src/custom-hooks/basics.jsx

import { useState, useEffect } from 'react';

// Custom hook for window size
function useWindowSize() {
  const [size, setSize] = useState({
    width: window.innerWidth,
    height: window.innerHeight
  });
  
  useEffect(() => {
    function handleResize() {
      setSize({
        width: window.innerWidth,
        height: window.innerHeight
      });
    }
    
    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);
  
  return size;
}

// Usage
function Component() {
  const { width, height } = useWindowSize();
  
  return (
    <p>Window is {width} x {height}</p>
  );
}
```

### Building Reusable Hooks

```jsx
// File: src/custom-hooks/toggle.js

import { useState, useCallback } from 'react';

// Toggle hook
function useToggle(initialValue = false) {
  const [value, setValue] = useState(initialValue);
  
  const toggle = useCallback(() => {
    setValue(v => !v);
  }, []);
  
  return [value, toggle, setValue];
}

// Usage
function ToggleComponent() {
  const [isOn, toggle] = useToggle();
  
  return (
    <button onClick={toggle}>
      {isOn ? 'ON' : 'OFF'}
    </button>
  );
}
```

## Real-World Example

```jsx
// File: src/hooks/useDebounce.js

import { useState, useEffect } from 'react';

function useDebounce(value, delay = 500) {
  const [debouncedValue, setDebouncedValue] = useState(value);
  
  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);
    
    return () => clearTimeout(timer);
  }, [value, delay]);
  
  return debouncedValue;
}

// Usage
function SearchComponent() {
  const [query, setQuery] = useState('');
  const debouncedQuery = useDebounce(query, 300);
  
  // Only searches when user stops typing
  useEffect(() => {
    if (debouncedQuery) {
      searchAPI(debouncedQuery);
    }
  }, [debouncedQuery]);
  
  return (
    <input 
      value={query} 
      onChange={e => setQuery(e.target.value)} 
    />
  );
}
```

## Key Takeaways

- Custom hooks start with "use"
- They can call other hooks
- They extract component logic into reusable functions
- They share stateful logic between components

## What's Next

Let's look at specific custom hooks like useFetch and useLocalStorage.
