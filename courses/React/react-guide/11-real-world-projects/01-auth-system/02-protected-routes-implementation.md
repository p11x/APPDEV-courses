# Protected Routes Implementation

## Overview

Protected routes restrict access to authenticated users. This guide covers implementing route guards using React Router, handling redirects, and managing role-based access control.

## Prerequisites

- React Router v6 knowledge
- Auth context understanding

## Core Concepts

### Private Route Component

```tsx
// File: src/components/PrivateRoute.tsx

import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

interface PrivateRouteProps {
  redirectTo?: string;
}

export function PrivateRoute({ redirectTo = '/login' }: PrivateRouteProps) {
  const { user, isLoading } = useAuth();

  // Show loading while checking auth
  if (isLoading) {
    return <div>Loading...</div>;
  }

  // Redirect to login if not authenticated
  if (!user) {
    return <Navigate to={redirectTo} replace />;
  }

  // Render child routes
  return <Outlet />;
}
```

### Role-Based Access

```tsx
// File: src/components/RoleRoute.tsx

import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

interface RoleRouteProps {
  allowedRoles: string[];
  redirectTo?: string;
}

export function RoleRoute({ 
  allowedRoles, 
  redirectTo = '/unauthorized' 
}: RoleRouteProps) {
  const { user } = useAuth();

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  if (!allowedRoles.includes(user.role)) {
    return <Navigate to={redirectTo} replace />;
  }

  return <Outlet />;
}
```

### Route Configuration

```tsx
// File: src/App.tsx

import { Routes, Route } from 'react-router-dom';
import { PrivateRoute } from './components/PrivateRoute';
import { RoleRoute } from './components/RoleRoute';
import { Login } from './pages/Login';
import { Dashboard } from './pages/Dashboard';
import { AdminPanel } from './pages/AdminPanel';
import { Unauthorized } from './pages/Unauthorized';

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/unauthorized" element={<Unauthorized />} />
      
      {/* Protected routes */}
      <Route element={<PrivateRoute />}>
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/settings" element={<Settings />} />
      </Route>
      
      {/* Admin-only routes */}
      <Route element={<RoleRoute allowedRoles={['admin']} />}>
        <Route path="/admin" element={<AdminPanel />} />
      </Route>
    </Routes>
  );
}
```

## Key Takeaways

- Use PrivateRoute wrapper for protected routes
- Check authentication before rendering children
- Handle loading states during auth check
- Implement role-based access control

## What's Next

Continue to [Refresh Token Strategy](/11-real-world-projects/01-auth-system/03-refresh-token-strategy.md) to learn about maintaining sessions.