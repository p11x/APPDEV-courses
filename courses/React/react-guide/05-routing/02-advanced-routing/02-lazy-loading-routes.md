# Lazy Loading Routes

## Overview

Lazy loading routes allows you to split your code into separate bundles that are loaded on demand. This improves initial load time by only loading the code needed for the current route.

## Core Concepts

```jsx
// File: src/lazy-routes.jsx

import { lazy, Suspense } from 'react';
import { Routes, Route } from 'react-router-dom';

// Lazy import
const Home = lazy(() => import('./pages/Home'));
const About = lazy(() => import('./pages/About'));
const Dashboard = lazy(() => import('./pages/Dashboard'));

function App() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route path="/dashboard" element={<Dashboard />} />
      </Routes>
    </Suspense>
  );
}
```

## Key Takeaways

- Use lazy() for dynamic imports
- Wrap in Suspense for loading state
- Improves initial bundle size
- Better for large applications
