# Protected Routes and Auth Guard

## Overview

Protected routes restrict access to certain pages based on authentication status or other conditions. This is essential for protecting authenticated content like dashboards, user profiles, and admin areas.

## Core Concepts

```jsx
// File: src/ProtectedRoute.jsx

import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from './auth';

function ProtectedRoute({ children }) {
  const { user, loading } = useAuth();
  const location = useLocation();
  
  if (loading) {
    return <div>Loading...</div>;
  }
  
  if (!user) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }
  
  return children;
}

// Usage
<Route 
  path="/dashboard" 
  element={
    <ProtectedRoute>
      <Dashboard />
    </ProtectedRoute>
  } 
/>
```

## Key Takeaways

- Create wrapper component for protected routes
- Check authentication status
- Redirect to login with return location
- Use Navigate component for redirects
