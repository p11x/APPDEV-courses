# Consuming Context with Hooks in React

## Overview
React's useContext hook provides the modern way to consume context values in functional components. It makes accessing global data simple and eliminates the need for Context Consumer wrapper components.

## Prerequisites
- Understanding of context creation
- Knowledge of React hooks
- Familiarity with Provider components

## Core Concepts

### Using useContext Hook
The useContext hook accepts a context object and returns its current value.

```jsx
// File: src/context/ThemeContext.jsx

import React, { createContext, useContext, useState } from 'react';

// Create context
const ThemeContext = createContext();

function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  
  const toggleTheme = () => {
    setTheme(t => t === 'light' ? 'dark' : 'light');
  };

  return (
    <ThemeContext.Provider value={{ theme, setTheme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

// Custom hook for cleaner consumption
function useTheme() {
  const context = useContext(ThemeContext);
  if (!context) {
    throw new Error('useTheme must be used within ThemeProvider');
  }
  return context;
}

export { ThemeContext, ThemeProvider, useTheme };

// File: src/components/ThemedButton.jsx

import React from 'react';
import { useTheme } from '../context/ThemeContext';

function ThemedButton({ children }) {
  const { theme, toggleTheme } = useTheme();

  const styles = {
    padding: '10px 20px',
    backgroundColor: theme === 'light' ? '#fff' : '#333',
    color: theme === 'light' ? '#333' : '#fff',
    border: '1px solid',
    cursor: 'pointer',
  };

  return (
    <button style={styles} onClick={toggleTheme}>
      {children}
    </button>
  );
}

export default ThemedButton;
```

### Consuming Multiple Contexts
You can use multiple contexts in one component.

```jsx
// File: src/components/UserMenu.jsx

import React from 'react';
import { useAuth } from '../context/AuthContext';
import { useTheme } from '../context/ThemeContext';

function UserMenu() {
  const { user, logout } = useAuth();
  const { theme } = useTheme();

  if (!user) {
    return <LoginPrompt />;
  }

  return (
    <div style={{ borderColor: theme === 'light' ? '#ccc' : '#555' }}>
      <span>Welcome, {user.name}</span>
      <button onClick={logout}>Logout</button>
    </div>
  );
}
```

### Context with Complex State
Context can hold complex state and provide multiple ways to update it.

```jsx
// File: src/context/CartContext.jsx

import React, { createContext, useContext, useState, useCallback } from 'react';

const CartContext = createContext();

function CartProvider({ children }) {
  const [items, setItems] = useState([]);
  const [isOpen, setIsOpen] = useState(false);

  const addItem = useCallback((product) => {
    setItems(prev => {
      const existing = prev.find(item => item.id === product.id);
      if (existing) {
        return prev.map(item =>
          item.id === product.id
            ? { ...item, quantity: item.quantity + 1 }
            : item
        );
      }
      return [...prev, { ...product, quantity: 1 }];
    });
  }, []);

  const removeItem = useCallback((productId) => {
    setItems(prev => prev.filter(item => item.id !== productId));
  }, []);

  const clearCart = useCallback(() => {
    setItems([]);
  }, []);

  const toggleCart = useCallback(() => {
    setIsOpen(prev => !prev);
  }, []);

  const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);

  return (
    <CartContext.Provider
      value={{
        items,
        isOpen,
        addItem,
        removeItem,
        clearCart,
        toggleCart,
        total,
        itemCount: items.reduce((sum, item) => sum + item.quantity, 0),
      }}
    >
      {children}
    </CartContext.Provider>
  );
}

function useCart() {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error('useCart must be used within CartProvider');
  }
  return context;
}

export { CartContext, CartProvider, useCart };
```

## Common Mistakes

### Using Context Without Provider
Always ensure your component is wrapped by the appropriate Provider.

```jsx
// ❌ WRONG - Using context without Provider
function BadComponent() {
  const value = useContext(MyContext); // Error if no Provider!
  return <div>{value}</div>;
}

// ✅ CORRECT - Wrap with Provider
function GoodComponent() {
  return (
    <MyContext.Provider value="hello">
      <Child />
    </MyContext.Provider>
  );
}
```

### Unnecessary Re-renders
Be careful with context - any change causes all consumers to re-render.

```jsx
// ❌ WRONG - Creating new objects in render
function BadProvider() {
  const [state, setState] = useState({ count: 0 });
  
  // New object every render - causes re-renders!
  return (
    <Context.Provider value={{ state, setState }}>
      {children}
    </Context.Provider>
  );
}

// ✅ CORRECT - Use multiple contexts or memoize
const StateContext = createContext();
const ActionsContext = createContext();

function GoodProvider() {
  const [state, setState] = useState({ count: 0 });
  
  // Separate contexts for state and actions
  return (
    <StateContext.Provider value={state}>
      <ActionsContext.Provider value={setState}>
        {children}
      </ActionsContext.Provider>
    </StateContext.Provider>
  );
}
```

## Key Takeaways
- Use useContext hook to consume context in functional components
- Create custom hooks for cleaner context consumption
- Always check context exists before using
- Be aware of re-render performance implications

## What's Next
Continue to [Context Performance Pitfalls](03-context-performance-pitfalls.md) to learn about common performance issues with context and how to avoid them.
