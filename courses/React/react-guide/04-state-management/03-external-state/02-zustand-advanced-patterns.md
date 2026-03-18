# Zustand Advanced Patterns

## Overview

Zustand supports advanced patterns including middleware, async actions, and selectors. These features make it suitable for complex applications.

## Core Concepts

```jsx
// File: src/store-advanced.js

import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';

const useStore = create(
  devtools(
    persist(
      (set, get) => ({
        todos: [],
        addTodo: (todo) => set(state => ({
          todos: [...state.todos, todo]
        })),
        removeTodo: (id) => set(state => ({
          todos: state.todos.filter(t => t.id !== id)
        }))
      }),
      { name: 'todo-storage' }
    )
  )
);

// Using selectors
const TodoList = () => {
  const todos = useStore(state => state.todos);
  return todos.map(todo => <div key={todo.id}>{todo.text}</div>);
};
```

## Key Takeaways

- Middleware adds functionality (persistence, devtools)
- Async actions work naturally
- Selectors prevent unnecessary re-renders
