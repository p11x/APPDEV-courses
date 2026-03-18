# Zustand Introduction and Setup

## Overview

Zustand is a small, fast state management library for React. It provides a simple API for creating global state stores without the boilerplate of Redux. Zustand uses hooks under the hood and is much simpler than Redux while being just as powerful.

## Prerequisites

- Understanding of React hooks
- Knowledge of state management concepts

## Core Concepts

```jsx
// File: src/store.js

import { create } from 'zustand';

const useStore = create((set) => ({
  count: 0,
  increment: () => set(state => ({ count: state.count + 1 })),
  decrement: () => set(state => ({ count: state.count - 1 })),
  reset: () => set({ count: 0 })
}));

function Counter() {
  const { count, increment } = useStore();
  
  return <button onClick={increment}>{count}</button>;
}
```

## Key Takeaways

- Zustand provides simple global state
- No provider wrapper needed
- Components subscribe to store
- Updates are simple function calls
