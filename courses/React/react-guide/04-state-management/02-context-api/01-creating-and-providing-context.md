# Creating and Providing Context in React

## Overview
React Context provides a way to pass data through the component tree without having to pass props manually at every level. It's designed for global data like themes, authentication, user preferences, and any data that many components need access to.

## Prerequisites
- Understanding of React component hierarchy
- Knowledge of props in React
- Familiarity with hooks (useState, useEffect)

## Core Concepts

### Creating Context
Context is created using React.createContext(). It returns an object with a Provider and optionally a Consumer (though hooks are preferred now).

```jsx
// File: src/context/ThemeContext.jsx

import React, { createContext, useState } from 'react';

// Create context with a default value
// The default is only used when there's no Provider above in the tree
const ThemeContext = createContext({
  theme: 'light',
  toggleTheme: () => {},
});

// Provider component that wraps children
function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');

  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };

  // The value object is what consumers will receive
  const value = {
    theme,
    toggleTheme,
  };

  return (
    <ThemeContext.Provider value={value}>
      {children}
    </ThemeContext.Provider>
  );
}

export { ThemeContext, ThemeProvider };
```

### Using Multiple Contexts
You can use multiple contexts in one app. Each context works independently.

```jsx
// File: src/context/AuthContext.jsx

import React, { createContext, useState, useEffect } from 'react';

const AuthContext = createContext(null);

function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Check for existing session on mount
    checkAuthStatus();
  }, []);

  const checkAuthStatus = async () => {
    try {
      const response = await fetch('/api/me');
      if (response.ok) {
        const userData = await response.json();
        setUser(userData);
      }
    } finally {
      setIsLoading(false);
    }
  };

  const login = async (credentials) => {
    const response = await fetch('/api/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(credentials),
    });
    
    if (!response.ok) throw new Error('Login failed');
    
    const userData = await response.json();
    setUser(userData);
  };

  const logout = async () => {
    await fetch('/api/logout', { method: 'POST' });
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, isLoading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export { AuthContext, AuthProvider };
```

### Combining Contexts in App
Multiple providers can wrap your app, each providing different global data.

```jsx
// File: src/App.jsx

import React from 'react';
import { ThemeProvider } from './context/ThemeContext';
import { AuthProvider } from './context/AuthContext';
import Dashboard from './components/Dashboard';

function App() {
  return (
    // Providers can be nested - order matters for CSS stacking
    // but not for React context
    <AuthProvider>
      <ThemeProvider>
        <Dashboard />
      </ThemeProvider>
    </AuthProvider>
  );
}

export default App;
```

## Common Mistakes

### Creating Context Outside Components
Always create contexts at module level, not inside components.

```jsx
// ✅ CORRECT - Created at module level
const MyContext = createContext();

function MyProvider({ children }) {
  const [value, setValue] = useState('default');
  return <MyContext.Provider value={{ value, setValue }}>{children}</MyContext.Provider>;
}

// ❌ WRONG - Created inside component (new context on every render!)
function BadProvider({ children }) {
  const MyContext = createContext(); // Bad!
  return <MyContext.Provider>{children}</MyContext.Provider>;
}
```

### Forgetting to Provide Value
Always provide a value to the Provider.

```jsx
// ❌ WRONG - Missing value prop
<MyContext.Provider>
  <Child />
</MyContext.Provider>

// ✅ CORRECT - Value provided
<MyContext.Provider value={someValue}>
  <Child />
</MyContext.Provider>
```

## Key Takeaways
- Use createContext() to create context at module level
- Wrap components with Provider to make data available
- Always provide a value prop to the Provider
- Multiple contexts can be nested in one app

## What's Next
Continue to [Consuming Context with Hooks](02-consuming-context-with-hooks.md) to learn how to access context values in components.
