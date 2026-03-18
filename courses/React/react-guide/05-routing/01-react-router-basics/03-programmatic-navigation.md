# Programmatic Navigation

## Overview

Programmatic navigation allows you to navigate users through your application via JavaScript code rather than link clicks. This is essential for redirects after form submissions, authentication flows, and other automated navigation scenarios.

## Core Concepts

```jsx
// File: src/navigation.jsx

import { useNavigate } from 'react-router-dom';

function LoginForm() {
  const navigate = useNavigate();
  
  const handleSubmit = async (e) => {
    e.preventDefault();
    await login();
    navigate('/dashboard');
  };
  
  return <form onSubmit={handleSubmit}>...</form>;
}

// Using with replace (no history)
navigate('/page', { replace: true });

// Using with state
navigate('/user', { state: { from: '/login' } });
```

## Key Takeaways

- useNavigate hook provides navigation function
- navigate(path) for basic navigation
- navigate(path, { replace: true }) to replace history entry
- navigate(path, { state }) to pass state

## What's Next

Let's explore protected routes and authentication guards.
