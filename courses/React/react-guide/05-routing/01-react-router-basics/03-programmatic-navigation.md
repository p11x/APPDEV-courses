# Programmatic Navigation in React Router v6

## Overview
Programmatic navigation allows you to navigate users through your application using JavaScript instead of clicks. This is essential for form submissions, authentication flows, and conditional redirects.

## Prerequisites
- React Router v6 basics
- Understanding of hooks

## Core Concepts

### useNavigate Hook
The useNavigate hook returns a function for programmatic navigation.

```jsx
// File: src/components/AuthButton.jsx

import { useNavigate } from 'react-router-dom';

function AuthButton({ isLoggedIn }) {
  const navigate = useNavigate();

  const handleAuth = () => {
    if (isLoggedIn) {
      navigate('/dashboard');
    } else {
      navigate('/login');
    }
  };

  return (
    <button onClick={handleAuth}>
      {isLoggedIn ? 'Go to Dashboard' : 'Login'}
    </button>
  );
}
```

### Navigate with Options
The navigate function accepts options for control.

```jsx
// File: src/utils/navigation.js

import { useNavigate } from 'react-router-dom';

function NavigationExamples() {
  const navigate = useNavigate();

  const examples = {
    // Simple navigation
    simple: () => navigate('/dashboard'),
    
    // With state (pass data to next route)
    withState: () => navigate('/dashboard', { 
      state: { from: 'login', message: 'Welcome!' } 
    }),
    
    // Replace instead of push (don't add to history)
    replace: () => navigate('/dashboard', { replace: true }),
    
    // Go back in history
    goBack: () => navigate(-1),
    
    // Go forward in history
    goForward: () => navigate(1),
    
    // Go back 2 pages
    goBack2: () => navigate(-2),
  };

  return examples;
}
```

### Using Navigate Component
For class components or when hooks aren't available.

```jsx
// File: src/components/RequireAuth.jsx

import { Navigate, useLocation } from 'react-router-dom';

function RequireAuth({ isLoggedIn, children }) {
  const location = useLocation();

  if (!isLoggedIn) {
    // Redirect to login, but save the current location
    return (
      <Navigate 
        to="/login" 
        state={{ from: location }} 
        replace 
      />
    );
  }

  return children;
}

function LoginPage() {
  const location = useLocation();
  const from = location.state?.from?.pathname || '/dashboard';

  const handleLogin = () => {
    // After login, redirect back to where they came from
    return <Navigate to={from} replace />;
  };

  return (
    <div>
      <h1>Login</h1>
      <p>Redirecting from: {from}</p>
      <button onClick={handleLogin}>Login</button>
    </div>
  );
}
```

### Form Submission Navigation

```jsx
// File: src/components/SearchForm.jsx

import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function SearchForm() {
  const [query, setQuery] = useState('');
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    if (query.trim()) {
      navigate(`/search?q=${encodeURIComponent(query)}`);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        placeholder="Search..."
      />
      <button type="submit">Search</button>
    </form>
  );
}
```

## Key Takeaways
- Use useNavigate hook for functional components
- Navigate accepts path and options (state, replace)
- Use -1, 1 etc. for history navigation
- Navigate component for class components or redirects

## What's Next
Continue to [Protected Routes Auth Guard](02-advanced-routing/01-protected-routes-auth-guard.md) to learn about authentication-protected routes.
