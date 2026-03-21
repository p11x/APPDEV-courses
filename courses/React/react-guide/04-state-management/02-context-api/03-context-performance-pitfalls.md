# Context Performance Pitfalls in React

## Overview
While React Context is powerful for sharing state, it can cause performance issues if not used carefully. Every time the Provider's value changes, all components consuming that context will re-render. Understanding these pitfalls helps you build performant applications.

## Prerequisites
- Understanding of Context API
- Knowledge of React re-rendering
- Familiarity with useMemo and React.memo

## Core Concepts

### The Re-render Problem
All consumers re-render when Provider value changes, even if they only use a small part of the context.

```jsx
// File: src/context/PerformanceProblem.jsx

import React, { createContext, useState, useContext } from 'react';

const UserContext = createContext();

// Problem: All consumers re-render when ANY value changes
function BadUserProvider() {
  const [user, setUser] = useState(null);
  const [theme, setTheme] = useState('light');
  const [notifications, setNotifications] = useState([]);

  // Every time any of these change, ALL consumers re-render!
  const value = { user, theme, notifications, setUser, setTheme, setNotifications };

  return <UserContext.Provider value={value}>{children}</UserContext.Provider>;
}

function Header() {
  // Only uses theme, but re-renders when notifications change!
  const { theme } = useContext(UserContext);
  return <header style={{ backgroundColor: theme }}>Header</header>;
}

function NotificationList() {
  // Only uses notifications, but re-renders when user changes!
  const { notifications } = useContext(UserContext);
  return <div>{notifications.map(n => <p key={n.id}>{n.message}</p>)}</div>;
}

function UserProfile() {
  // Only uses user, but re-renders when theme changes!
  const { user } = useContext(UserContext);
  return <div>{user?.name}</div>;
}
```

### Solution 1: Split Contexts
Separate contexts for frequently and rarely changing values.

```jsx
// File: src/context/SplitContexts.jsx

import React, { createContext, useState, useContext } from 'react';

// Separate contexts for different update frequencies
const UserContext = createContext();
const ThemeContext = createContext();
const NotificationContext = createContext();

// Static data - rarely changes
function UserProvider({ children }) {
  const [user, setUser] = useState(null);
  return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );
}

// Medium frequency changes
function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  return (
    <ThemeContext.Provider value={{ theme, setTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

// High frequency changes
function NotificationProvider({ children }) {
  const [notifications, setNotifications] = useState([]);

  const addNotification = (notification) => {
    setNotifications(prev => [...prev, notification]);
  };

  return (
    <NotificationContext.Provider value={{ notifications, addNotification }}>
      {children}
    </NotificationContext.Provider>
  );
}

// Usage: Only re-renders when specific context changes
function Header() {
  const { theme } = useContext(ThemeContext); // Only re-renders on theme change
  return <header>{theme}</header>;
}
```

### Solution 2: Memoize Context Values
Use useMemo to prevent unnecessary value recreation.

```jsx
// File: src/context/MemoizedContext.jsx

import React, { createContext, useState, useContext, useMemo, useCallback } from 'react';

function OptimizedProvider({ children }) {
  const [user, setUser] = useState(null);
  const [preferences, setPreferences] = useState({ theme: 'light', fontSize: 16 });

  // Memoize callbacks to maintain referential equality
  const updateUser = useCallback((newUser) => {
    setUser(newUser);
  }, []);

  const updateTheme = useCallback((theme) => {
    setPreferences(prev => ({ ...prev, theme }));
  }, []);

  const updateFontSize = useCallback((fontSize) => {
    setPreferences(prev => ({ ...prev, fontSize }));
  }, []);

  // Memoize the context value - only recreates when dependencies change
  const userValue = useMemo(() => ({
    user,
    updateUser,
  }), [user, updateUser]);

  const preferencesValue = useMemo(() => ({
    preferences,
    updateTheme,
    updateFontSize,
  }), [preferences, updateTheme, updateFontSize]);

  return (
    <UserContext.Provider value={userValue}>
      <PreferencesContext.Provider value={preferencesValue}>
        {children}
      </PreferencesContext.Provider>
    </UserContext.Provider>
  );
}
```

### Solution 3: Use useContext Selector
Custom hook to select only needed values.

```jsx
// File: src/context/ContextSelector.jsx

import React, { createContext, useState, useContext, useSyncExternalStore } from 'react';

// Selector pattern for granular subscriptions
function createSelectorContext(initialValue) {
  const Context = createContext(initialValue);

  function Provider({ children, value }) {
    return <Context.Provider value={value}>{children}</Context.Provider>;
  }

  function useContextSelector(selector) {
    const contextValue = useContext(Context);
    return selector(contextValue);
  }

  return { Provider, useContextSelector };
}

const { Provider, useContextSelector } = createSelectorContext({});

// Usage - only subscribes to specific values
function Header() {
  // Only re-renders when theme changes, not when user changes
  const theme = useContextSelector(state => state.theme);
  return <header>{theme}</header>;
}

function UserName() {
  // Only re-renders when user changes, not when theme changes
  const user = useContextSelector(state => state.user);
  return <span>{user?.name}</span>;
}
```

## Common Mistakes

### Putting Everything in One Context
Don't put all app state in one context.

```jsx
// ❌ WRONG - One context for everything
const AppContext = createContext();

function Provider({ children }) {
  const [user, setUser] = useState(null);
  const [posts, setPosts] = useState([]);
  const [comments, setComments] = useState([]);
  const [theme, setTheme] = useState('light');
  const [sidebar, setSidebar] = useState(false);
  // ... 20 more state values

  // This causes everything to re-render on any change!
  return <AppContext.Provider value={{
    user, setUser, posts, setPosts, comments, setComments, theme, setTheme, sidebar, setSidebar
  }}>{children}</AppContext.Provider>;
}

// ✅ CORRECT - Multiple focused contexts
const UserContext = createContext();
const PostsContext = createContext();
const UIContext = createContext();
```

## Key Takeaways
- All context consumers re-render when Provider value changes
- Split contexts by update frequency
- Memoize context values with useMemo
- Consider using selectors for fine-grained subscriptions
- Don't overuse context - only use for truly global data

## What's Next
Continue to [Zustand Introduction and Setup](../03-external-state/01-zustand-intro-and-setup.md) to learn about Zustand, a lightweight state management library.
