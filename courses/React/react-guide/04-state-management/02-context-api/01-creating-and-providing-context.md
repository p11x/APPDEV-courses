# Creating and Providing Context

## Overview

The Context API provides a way to pass data through the component tree without having to pass props manually at every level. Creating context involves using React.createContext and providing values through a Provider component.

## Prerequisites

- Understanding of React components
- Knowledge of props drilling problem

## Core Concepts

```jsx
// File: src/context-creation.jsx

import React, { createContext, useState } from 'react';

// Create context
const ThemeContext = createContext();

// Provider component
function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  
  const value = {
    theme,
    setTheme
  };
  
  return (
    <ThemeContext.Provider value={value}>
      {children}
    </ThemeContext.Provider>
  );
}
```

## Key Takeaways

- createContext creates the context
- Provider wraps children and supplies value
- Value should be memoized for performance

## What's Next

Let's explore consuming context with hooks.
