# Zustand Advanced Patterns

## Overview
Zustand provides several advanced features including middleware, transient updates, and persistence. These patterns help you build more sophisticated applications with complex state management requirements.

## Prerequisites
- Basic Zustand knowledge
- Understanding of React hooks
- Familiarity with state management

## Core Concepts

### Middleware
Middleware allows you to extend Zustand's functionality with logging, persistence, and more.

```jsx
// File: src/store/middlewareStore.js

import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';

// Devtools middleware - Redux DevTools integration
const useDevToolsStore = create(
  devtools(
    (set) => ({
      count: 0,
      increment: () => set((state) => ({ count: state.count + 1 })),
    }),
    { name: 'counter-store' } // Name in DevTools
  )
);

// Persistence middleware - saves to localStorage
const usePersistedStore = create(
  persist(
    (set) => ({
      theme: 'light',
      setTheme: (theme) => set({ theme }),
    }),
    {
      name: 'theme-storage', // unique name
      storage: localStorage, // or sessionStorage
    }
  )
);

// Combined middleware
const useStore = create(
  devtools(
    persist(
      (set) => ({
        user: null,
        setUser: (user) => set({ user }),
      }),
      { name: 'user-store' }
    ),
    { name: 'user-devtools' }
  )
);
```

### Transient Updates
For high-frequency updates, use transient subscriptions to avoid re-renders.

```jsx
// File: src/store/transientStore.js

import { create } from 'zustand';

const useAnimationStore = create((set) => ({
  position: { x: 0, y: 0 },
  setPosition: (x, y) => set({ position: { x, y } }),
}));

// Component that updates without re-rendering
function AnimationComponent() {
  const positionRef = React.useRef(null);

  // Use transient subscription - doesn't trigger re-render
  useAnimationStore.subscribe(
    (state) => state.position,
    (position) => {
      // Direct DOM manipulation - no React render
      if (positionRef.current) {
        positionRef.current.style.transform = `translate(${position.x}px, ${position.y}px)`;
      }
    }
  );

  return <div ref={positionRef}>Moving element</div>;
}
```

### Store Slices
For large applications, split stores into slices and combine them.

```jsx
// File: src/store/slices/userSlice.js

// User slice
const createUserSlice = (set) => ({
  user: null,
  isAuthenticated: false,
  login: async (credentials) => {
    const response = await fetch('/api/login', {
      method: 'POST',
      body: JSON.stringify(credentials),
    });
    const user = await response.json();
    set({ user, isAuthenticated: true });
  },
  logout: () => set({ user: null, isAuthenticated: false }),
});

export default createUserSlice;

// File: src/store/slices/cartSlice.js

// Cart slice
const createCartSlice = (set) => ({
  items: [],
  addItem: (product) => set((state) => ({
    items: [...state.items, product],
  })),
  removeItem: (productId) => set((state) => ({
    items: state.items.filter((item) => item.id !== productId),
  })),
  clearCart: () => set({ items: [] }),
});

export default createCartSlice;

// File: src/store/index.js

import { create } from 'zustand';
import createUserSlice from './slices/userSlice';
import createCartSlice from './slices/cartSlice';

// Combine slices
const useStore = create((set, get, api) => ({
  ...createUserSlice(set, get, api),
  ...createCartSlice(set, get, api),
  // Shared actions
  reset: () => {
    set({ user: null, items: [] });
  },
}));

export default useStore;
```

### Immer Integration
Zustand supports Immer for mutable-style state updates.

```jsx
// File: src/store/immerStore.js

import { create } from 'zustand';
import { immer } from 'zustand/middleware/immer';

const useImmerStore = create(
  immer((set) => ({
    // Can write "mutating" code that gets converted to immutable updates
    addTodo: (todo) => set((state) => {
      state.todos.push({ ...todo, id: Date.now() });
    }),
    
    toggleTodo: (id) => set((state) => {
      const todo = state.todos.find((t) => t.id === id);
      if (todo) todo.completed = !todo.completed;
    }),
    
    removeTodo: (id) => set((state) => {
      state.todos = state.todos.filter((t) => t.id !== id);
    }),
  }))
);
```

## Common Mistakes

### Not Cleaning Up Subscriptions
Always clean up transient subscriptions.

```jsx
// ❌ WRONG - Memory leak
useStore.subscribe((state) => state.value, callback);

// ✅ CORRECT - Store and unsubscribe
const unsubscribe = useStore.subscribe((state) => state.value, callback);
React.useEffect(() => () => unsubscribe(), []);
```

## Key Takeaways
- Use middleware for persistence, devtools, and logging
- Use transient updates for high-frequency animations
- Split large stores into slices
- Use Immer for complex state updates

## What's Next
Continue to [When to Use Redux Toolkit](03-when-to-use-redux-toolkit.md) to learn when Redux is the better choice over simpler alternatives.
