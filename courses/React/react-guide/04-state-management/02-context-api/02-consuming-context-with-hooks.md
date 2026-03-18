# Consuming Context with Hooks

## Overview

useContext is the hook that allows functional components to consume context. It lets you access the value provided by a Context.Provider without nesting.

## Prerequisites

- Understanding of context creation
- Knowledge of useState hook

## Core Concepts

```jsx
// File: src/consuming-context.jsx

import React, { createContext, useContext, useState } from 'react';

const ThemeContext = createContext(null);

function ThemedComponent() {
  const { theme, toggleTheme } = useContext(ThemeContext);
  
  return (
    <div style={{ backgroundColor: theme === 'dark' ? '#333' : '#fff' }}>
      <button onClick={toggleTheme}>Toggle</button>
    </div>
  );
}
```

## Key Takeaways

- useContext returns context value
- Returns value from nearest provider
- Causes re-render when context changes
