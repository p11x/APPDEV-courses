# Protected Routes and Auth Guards in React Router v6

## Overview
Protected routes (auth guards) restrict access to certain pages based on authentication status. This is essential for protecting dashboard pages, admin areas, and other authenticated routes.

## Prerequisites
- React Router v6 basics
- Understanding of authentication
- Familiarity with context

## Core Concepts

### Creating an Auth Guard Component

```jsx
// File: src/components/RequireAuth.jsx

import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

function RequireAuth({ children, allowedRoles }) {
  const { user, isLoading } = useAuth();
  const location = useLocation();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!user) {
    // Redirect to login, but save the location
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (allowedRoles && !allowedRoles.includes(user.role)) {
    // User doesn't have required role
    return <Navigate to="/unauthorized" replace />;
  }

  return children;
}

export default RequireAuth;
```

### Using Protected Routes

```jsx
// File: src/App.jsx

import { Routes, Route } from 'react-router-dom';
import RequireAuth from './components/RequireAuth';
import Dashboard from './pages/Dashboard';
import AdminPanel from './pages/AdminPanel';
import Login from './pages/Login';

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      
      {/* Protected route - requires authentication */}
      <Route
        path="/dashboard"
        element={
          <RequireAuth>
            <Dashboard />
          </RequireAuth>
        }
      />
      
      {/* Protected route - requires specific role */}
      <Route
        path="/admin"
        element={
          <RequireAuth allowedRoles={['admin']}>
            <AdminPanel />
          </RequireAuth>
        }
      />
    </Routes>
  );
}

export default App;
```

### Login with Redirect

```jsx
// File: src/pages/Login.jsx

import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

function Login() {
  const navigate = useNavigate();
  const location = useLocation();
  const { login } = useAuth();

  const from = location.state?.from?.pathname || '/dashboard';

  const handleLogin = async () => {
    await login();
    // Redirect to the page they tried to visit
    navigate(from, { replace: true });
  };

  return (
    <div>
      <h1>Login</h1>
      <button onClick={handleLogin}>Sign In</button>
    </div>
  );
}

export default Login;
```

## Key Takeaways
- Use a wrapper component for protected routes
- Check authentication status before rendering children
- Save location for post-login redirect
- Support role-based access control

## What's Next
Continue to [Lazy Loading Routes](02-lazy-loading-routes.md) to learn about code splitting.
