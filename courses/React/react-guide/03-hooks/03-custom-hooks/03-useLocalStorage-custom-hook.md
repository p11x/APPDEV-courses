# useLocalStorage Custom Hook

## Overview

useLocalStorage is a custom hook that syncs state with browser localStorage, allowing data to persist across page refreshes and browser sessions. This is incredibly useful for saving user preferences, theme settings, or any data that should persist.

## Prerequisites

- Understanding of custom hooks
- Knowledge of useState
- Familiarity with localStorage API

## Core Concepts

### Building useLocalStorage

```javascript
// File: src/hooks/useLocalStorage.js

import { useState, useEffect } from 'react';

function useLocalStorage(key, initialValue) {
  // Get stored value or use initial value
  const [storedValue, setStoredValue] = useState(() => {
    try {
      const item = window.localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      console.error('Error reading localStorage:', error);
      return initialValue;
    }
  });
  
  // Update localStorage when value changes
  useEffect(() => {
    try {
      window.localStorage.setItem(key, JSON.stringify(storedValue));
    } catch (error) {
      console.error('Error writing localStorage:', error);
    }
  }, [key, storedValue]);
  
  return [storedValue, setStoredValue];
}

// Usage - Theme toggle
function ThemeToggle() {
  const [theme, setTheme] = useLocalStorage('theme', 'light');
  
  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };
  
  return (
    <button onClick={toggleTheme}>
      Current: {theme}
    </button>
  );
}
```

## Key Takeaways

- useLocalStorage syncs state with localStorage
- Handles JSON serialization/deserialization
- Provides error handling for localStorage access
- Data persists across page refreshes
