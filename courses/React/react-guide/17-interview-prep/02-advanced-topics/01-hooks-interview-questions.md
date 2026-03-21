# Hooks Interview Questions

## Overview
This guide covers common React hooks interview questions with detailed explanations and code examples. Understanding these concepts demonstrates deep knowledge of React.

## Prerequisites
- React hooks experience
- State management knowledge

## Core Concepts

### Question 1: useState with Previous State

**Q: Why do we use the functional form of setState?**

```tsx
// [File: src/interview-answers/UseStateFunctional.jsx]
import React, { useState } from 'react';

function Counter() {
  const [count, setCount] = useState(0);

  // ❌ WRONG - Using stale closure value
  const incrementWrong = () => {
    setCount(count + 1); // count might be stale
  };

  // ✅ CORRECT - Functional update
  const incrementRight = () => {
    setCount(prevCount => prevCount + 1); // Always gets latest
  };

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={incrementWrong}>Wrong</button>
      <button onClick={incrementRight}>Correct</button>
    </div>
  );
}

export default Counter;
```

### Question 2: useEffect Dependencies

**Q: What happens if you don't include dependencies?**

```tsx
// [File: src/interview-answers/UseEffectDeps.jsx]
import React, { useState, useEffect } from 'react';

function UserProfile({ userId }) {
  const [user, setUser] = useState(null);

  // ❌ WRONG - Missing dependency array
  useEffect(() => {
    fetch(`/api/users/${userId}`).then(r => r.json()).then(setUser);
  }); // Runs on EVERY render!

  // ✅ CORRECT - Proper dependencies
  useEffect(() => {
    fetch(`/api/users/${userId}`).then(r => r.json()).then(setUser);
  }, [userId]); // Runs when userId changes

  // ✅ CORRECT - Empty array = run once
  useEffect(() => {
    console.log('Component mounted');
  }, []); // Runs only on mount

  return <div>{user?.name}</div>;
}

export default UserProfile;
```

### Question 3: Custom Hooks

**Q: How do custom hooks promote code reuse?**

```tsx
// [File: src/interview-answers/useLocalStorage.js]
import { useState, useEffect } from 'react';

// Custom hook for localStorage
function useLocalStorage(key, initialValue) {
  const [value, setValue] = useState(() => {
    const stored = localStorage.getItem(key);
    return stored ? JSON.parse(stored) : initialValue;
  });

  useEffect(() => {
    localStorage.setItem(key, JSON.stringify(value));
  }, [key, value]);

  return [value, setValue];
}

export default useLocalStorage;

// Usage in component
function App() {
  const [name, setName] = useLocalStorage('name', '');
  return <input value={name} onChange={e => setName(e.target.value)} />;
}
```

## Common Mistakes

### Stale Closures

```tsx
// ❌ WRONG - Creating new function every render
function Component() {
  const [count, setCount] = useState(0);
  
  useEffect(() => {
    const timer = setInterval(() => {
      console.log(count); // Always logs 0!
    }, 1000);
    return () => clearInterval(timer);
  }, []); // Empty deps = stale closure

  // ✅ CORRECT - Use ref or functional update
  useEffect(() => {
    const timer = setInterval(() => {
      console.log(count); // Still stale...
    }, 1000);
    return () => clearInterval(timer);
  }, [count]); // Add count to deps

  return <div>{count}</div>;
}
```

## Key Takeaways
- Always use functional setState for dependent updates
- Include all used values in dependency array
- Custom hooks enable logic reuse

## What's Next
Continue to [Performance Interview Questions](02-performance-interview-questions.md) for performance-related questions.