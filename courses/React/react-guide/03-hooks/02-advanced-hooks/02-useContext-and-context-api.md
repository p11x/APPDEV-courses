# useContext and Context API

## Overview

useContext is a React hook that lets you consume context in functional components. The Context API provides a way to pass data through the component tree without having to pass props manually at every level. This is incredibly useful for global state like themes, authentication, user preferences, or any data that needs to be accessed by many components at different nesting levels.

## Prerequisites

- Understanding of React components
- Knowledge of props drilling problem
- Familiarity with useState and useEffect hooks

## Core Concepts

### What is Context?

Context provides a way to share values between components without explicitly passing props through every level of the component tree.

```jsx
// File: src/context-creation.jsx

import React, { createContext, useContext, useState } from 'react';

// 1. Create context with default value
const ThemeContext = createContext({
  theme: 'light',
  toggleTheme: () => {}
});

// 2. Create provider component
function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  
  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };
  
  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

// 3. Consume context in components
function ThemedButton() {
  const { theme, toggleTheme } = useContext(ThemeContext);
  
  return (
    <button
      onClick={toggleTheme}
      style={{
        backgroundColor: theme === 'dark' ? '#333' : '#fff',
        color: theme === 'dark' ? '#fff' : '#333'
      }}
    >
      Current: {theme}
    </button>
  );
}

// 4. Use provider at root
function App() {
  return (
    <ThemeProvider>
      <ThemedButton />
    </ThemeProvider>
  );
}
```

### Multiple Contexts

```jsx
// File: src/multiple-contexts.jsx

import React, { createContext, useContext, useState } from 'react';

// Create separate contexts
const AuthContext = createContext(null);
const ThemeContext = createContext(null);

// Auth Provider
function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  
  const login = (userData) => setUser(userData);
  const logout = () => setUser(null);
  
  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

// Theme Provider
function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  
  const toggleTheme = () => setTheme(t => t === 'light' ? 'dark' : 'light');
  
  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

// Combined Provider
function AppProviders({ children }) {
  return (
    <AuthProvider>
      <ThemeProvider>
        {children}
      </ThemeProvider>
    </AuthProvider>
  );
}

// Using multiple contexts
function UserProfile() {
  const { user, logout } = useContext(AuthContext);
  const { theme, toggleTheme } = useContext(ThemeContext);
  
  return (
    <div style={{ backgroundColor: theme === 'dark' ? '#333' : '#fff' }}>
      <p>Welcome, {user?.name}</p>
      <button onClick={logout}>Logout</button>
      <button onClick={toggleTheme}>Toggle Theme</button>
    </div>
  );
}
```

### Context with Complex State

```jsx
// File: src/context-complex.jsx

import React, { createContext, useContext, useReducer } from 'react';

// Action types
const ActionTypes = {
  SET_USER: 'SET_USER',
  UPDATE_PREFERENCES: 'UPDATE_PREFERENCES',
  SET_LOADING: 'SET_LOADING'
};

// Initial state
const initialState = {
  user: null,
  preferences: {
    theme: 'light',
    language: 'en',
    notifications: true
  },
  loading: false
};

// Context
const UserContext = createContext(null);

// Reducer
function userReducer(state, action) {
  switch (action.type) {
    case ActionTypes.SET_USER:
      return { ...state, user: action.payload, loading: false };
    case ActionTypes.UPDATE_PREFERENCES:
      return {
        ...state,
        preferences: { ...state.preferences, ...action.payload }
      };
    case ActionTypes.SET_LOADING:
      return { ...state, loading: action.payload };
    default:
      return state;
  }
}

// Provider
function UserProvider({ children }) {
  const [state, dispatch] = useReducer(userReducer, initialState);
  
  const setUser = (user) => dispatch({ type: ActionTypes.SET_USER, payload: user });
  const updatePreferences = (prefs) => dispatch({ 
    type: ActionTypes.UPDATE_PREFERENCES, 
    payload: prefs 
  });
  
  const value = {
    ...state,
    setUser,
    updatePreferences
  };
  
  return (
    <UserContext.Provider value={value}>
      {children}
    </UserContext.Provider>
  );
}

// Custom hook for convenience
function useUser() {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error('useUser must be used within UserProvider');
  }
  return context;
}

export { UserProvider, useUser };
```

## Common Mistakes

### Mistake 1: Creating Context Outside Provider

```jsx
// ❌ WRONG - Context has no default provider
function BadComponent() {
  const value = useContext(MyContext); // undefined!
  return <div>{value}</div>;
}

// ✅ CORRECT - Provide default or wrap in provider
const MyContext = createContext(defaultValue); // Default value

// Or
<MyContext.Provider>
  <Component />
</MyContext.Provider>
```

### Mistake 2: Not Memoizing Context Value

```jsx
// ❌ WRONG - New object every render causes re-renders
function BadProvider({ children }) {
  const [state, setState] = useState({});
  
  return (
    <Context.Provider value={{ state, setState }}>
      {children}
    </Context.Provider>
  );
}

// ✅ CORRECT - Memoize context value
function GoodProvider({ children }) {
  const [state, setState] = useState({});
  
  const value = useMemo(() => ({ state, setState }), [state]);
  
  return (
    <Context.Provider value={value}>
      {children}
    </Context.Provider>
  );
}
```

## Real-World Example

```jsx
// File: src/contexts/AuthContext.jsx

import React, { createContext, useContext, useState, useEffect } from 'react';

const AuthContext = createContext(null);

function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    // Check for existing session
    async function checkAuth() {
      try {
        const token = localStorage.getItem('token');
        if (token) {
          const response = await fetch('/api/auth/me', {
            headers: { Authorization: `Bearer ${token}` }
          });
          if (response.ok) {
            const userData = await response.json();
            setUser(userData);
          }
        }
      } catch (error) {
        console.error('Auth check failed:', error);
      } finally {
        setLoading(false);
      }
    }
    
    checkAuth();
  }, []);
  
  const login = async (email, password) => {
    const response = await fetch('/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });
    
    if (!response.ok) {
      throw new Error('Login failed');
    }
    
    const { token, user } = await response.json();
    localStorage.setItem('token', token);
    setUser(user);
  };
  
  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
  };
  
  const value = {
    user,
    loading,
    login,
    logout,
    isAuthenticated: !!user
  };
  
  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
}

export { AuthProvider, useAuth };
```

```jsx
// File: src/components/Navbar.jsx

import React from 'react';
import { useAuth } from '../contexts/AuthContext';

function Navbar() {
  const { user, logout, isAuthenticated } = useAuth();
  
  return (
    <nav style={{ 
      display: 'flex', 
      justifyContent: 'space-between', 
      padding: '1rem',
      backgroundColor: '#333',
      color: 'white'
    }}>
      <div>My App</div>
      
      <div>
        {isAuthenticated ? (
          <>
            <span>Welcome, {user?.name}</span>
            <button onClick={logout} style={{ marginLeft: '1rem' }}>
              Logout
            </button>
          </>
        ) : (
          <a href="/login">Login</a>
        )}
      </div>
    </nav>
  );
}

export default Navbar;
```

## Key Takeaways

- Context API provides a way to pass data through component tree
- useContext lets functional components consume context
- Always wrap components that need the context in a Provider
- Memoize context values to prevent unnecessary re-renders
- Provide default values for context to avoid undefined errors
- Custom hooks provide clean API for context consumption

## What's Next

Now let's explore useMemo and useCallback - hooks for performance optimization.
