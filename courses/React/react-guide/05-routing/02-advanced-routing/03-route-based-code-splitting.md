# Route-Based Code Splitting

## Overview

Route-based code splitting is a technique that splits your application into separate JavaScript bundles for each route. Users only download the code for routes they visit, improving initial load performance.

## Implementation

Use React.lazy with React Router to implement route-based code splitting:

```jsx
import { lazy, Suspense } from 'react';
import { Routes, Route } from 'react-router-dom';

const Home = lazy(() => import('./pages/Home'));
const Dashboard = lazy(() => import('./pages/Dashboard'));

function App() {
  return (
    <Suspense fallback={<Loading />}>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/dashboard" element={<Dashboard />} />
      </Routes>
    </Suspense>
  );
}
```

## Key Takeaways

- Each route gets its own bundle
- Suspense shows loading state
- Reduces initial bundle size
- Vite/Webpack handles the splitting

## What's Next

Let's explore breadcrumb navigation.
