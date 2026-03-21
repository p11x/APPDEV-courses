# When to Use Redux Toolkit

## Overview
Redux Toolkit (RTK) is the official, recommended way to write Redux logic. It simplifies store setup, reduces boilerplate, and provides powerful patterns for complex state management. While alternatives like Zustand are simpler, Redux Toolkit excels in specific scenarios.

## Prerequisites
- Understanding of state management
- Familiarity with React
- Knowledge of Redux concepts (optional)

## Core Concepts

### When Redux Toolkit Makes Sense

**1. Large Teams with Complex State**
When multiple teams work on the same codebase and need predictable state updates.

```jsx
// File: src/store/index.js

import { configureStore, createSlice } from '@reduxjs/toolkit';

// Slice for cart functionality
const cartSlice = createSlice({
  name: 'cart',
  initialState: {
    items: [],
    total: 0,
  },
  reducers: {
    addItem: (state, action) => {
      const existingItem = state.items.find(item => item.id === action.payload.id);
      if (existingItem) {
        existingItem.quantity += 1;
      } else {
        state.items.push({ ...action.payload, quantity: 1 });
      }
      state.total = state.items.reduce((sum, item) => 
        sum + item.price * item.quantity, 0
      );
    },
    removeItem: (state, action) => {
      state.items = state.items.filter(item => item.id !== action.payload);
      state.total = state.items.reduce((sum, item) => 
        sum + item.price * item.quantity, 0
      );
    },
    clearCart: (state) => {
      state.items = [];
      state.total = 0;
    },
  },
});

export const { addItem, removeItem, clearCart } = cartSlice.actions;

// Configure store
const store = configureStore({
  reducer: {
    cart: cartSlice.reducer,
  },
});

export default store;
```

**2. Complex Async Logic with RTK Query**
When you need powerful data fetching with caching.

```jsx
// File: src/services/api.js

import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const api = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({ baseUrl: '/api' }),
  endpoints: (builder) => ({
    getPosts: builder.query({
      query: () => '/posts',
    }),
    getPost: builder.query({
      query: (id) => `/posts/${id}`,
    }),
    createPost: builder.mutation({
      query: (newPost) => ({
        url: '/posts',
        method: 'POST',
        body: newPost,
      }),
    }),
  }),
});

export const { useGetPostsQuery, useGetPostQuery, useCreatePostMutation } = api;

// Add to store
// reducerPath: api.reducerPath,
// middleware: (getDefaultMiddleware) =>
//   getDefaultMiddleware().concat(api.middleware),
```

**3. Time Travel Debugging**
When you need Redux DevTools for complex debugging.

```jsx
// Redux DevTools automatically enabled with configureStore
const store = configureStore({
  reducer: rootReducer,
  // DevTools enabled in development
  devTools: process.env.NODE_ENV !== 'production',
});

// Can replay actions, inspect state, etc.
```

### When Simpler Alternatives Work

**For Simple State: useState or Context**
- Few pieces of state
- No complex interactions
- Simple UI state

```jsx
// ✅ Use React state for simple cases
const [theme, setTheme] = useState('light');
```

**For Medium Complexity: Zustand**
- Moderate state needs
- Want less boilerplate
- Simple async needs

```jsx
// ✅ Use Zustand for medium complexity
const useStore = create((set) => ({
  theme: 'light',
  toggleTheme: () => set(s => ({ theme: s.theme === 'light' ? 'dark' : 'light' })),
}));
```

## Decision Matrix

| Scenario | Recommendation |
|----------|----------------|
| Simple UI state | useState + Context |
| Medium complexity | Zustand |
| Multiple developers | Redux Toolkit |
| Complex async/caching | RTK Query |
| Need time travel | Redux Toolkit |
| Quick prototyping | Zustand |

## Key Takeaways
- Use Redux Toolkit for large teams and complex state
- Use simpler solutions (useState, Zustand) for simpler needs
- RTK Query is powerful for server state
- Don't over-engineer - choose the right tool

## What's Next
This concludes the state management section. Continue to [Setting Up React Router v6](../05-routing/01-react-router-basics/01-setting-up-react-router-v6.md) to learn about client-side routing.
