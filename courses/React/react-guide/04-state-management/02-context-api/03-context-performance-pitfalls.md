# Context Performance Pitfalls

## Overview

While Context API is powerful, it can cause performance issues if not used carefully. The main problem is that any change to context causes all consumers to re-render. Understanding these pitfalls helps you build performant applications.

## Prerequisites

- Understanding of Context API
- Knowledge of React rendering

## Core Concepts

### The Re-render Problem

```jsx
// File: src/context-performance.jsx

import React, { createContext, useState, useMemo } from 'react';

// ❌ BAD: New object every render
function BadProvider({ children }) {
  const [value, setValue] = useState('');
  
  return (
    <Context.Provider value={{ value, setValue }}>
      {children}
    </Context.Provider>
  );
}

// ✅ GOOD: Memoize the value
function GoodProvider({ children }) {
  const [value, setValue] = useState('');
  
  const contextValue = useMemo(() => ({
    value,
    setValue
  }), [value]);
  
  return (
    <Context.Provider value={contextValue}>
      {children}
    </Context.Provider>
  );
}
```

## Key Takeaways

- Context changes trigger re-renders in all consumers
- Memoize context values to prevent unnecessary renders
- Split contexts for frequently vs rarely changing values
- Consider using selectors

## What's Next

Let's explore external state management with Zustand.
