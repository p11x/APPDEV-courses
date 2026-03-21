# Lazy Loading Routes in React Router v6

## Overview
Lazy loading routes improves initial load time by splitting your bundle and loading routes only when needed. This is essential for large applications.

## Core Concepts

### Using React.lazy and Suspense

```jsx
// File: src/App.jsx

import { Suspense, lazy } from 'react';
import { Routes, Route } from 'react-router-dom';
import LoadingSpinner from './components/LoadingSpinner';

// Lazy load route components
const Home = lazy(() => import('./pages/Home'));
const Dashboard = lazy(() => import('./pages/Dashboard'));
const Settings = lazy(() => import('./pages/Settings'));
const Admin = lazy(() => import('./pages/Admin'));

function App() {
  return (
    <Suspense fallback={<LoadingSpinner />}>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/settings" element={<Settings />} />
        <Route path="/admin" element={<Admin />} />
      </Routes>
    </Suspense>
  );
}
```

## Key Takeaways
- Use React.lazy() for dynamic imports
- Wrap routes in Suspense component
- Show loading fallback during route load
