# React.lazy and Suspense

## Overview

React.lazy enables you to render a dynamic import as a regular component. Suspense lets you show fallback content while the lazy component is loading. Together, they provide a powerful way to implement code splitting at the component level.

## Prerequisites

- Understanding of dynamic imports
- Familiarity with React components

## Core Concepts

### Using React.lazy

React.lazy takes a function that returns a dynamic import:

```tsx
// File: src/App.tsx

import { lazy, Suspense } from 'react';

// Lazy load the component - it loads when rendered
const HeavyComponent = lazy(() => import('./HeavyComponent'));

function App() {
  return (
    <div>
      <h1>My App</h1>
      {/* Suspense shows fallback while loading */}
      <Suspense fallback={<div>Loading...</div>}>
        <HeavyComponent />
      </Suspense>
    </div>
  );
}
```

### Lazy Loading Routes

Split your app by routes:

```tsx
// File: src/App.tsx

import { Routes, Route, BrowserRouter } from 'react-router-dom';
import { lazy, Suspense } from 'react';

// Lazy load pages
const Home = lazy(() => import('./pages/Home'));
const About = lazy(() => import('./pages/About'));
const Dashboard = lazy(() => import('./pages/Dashboard'));
const Settings = lazy(() => import('./pages/Settings'));

function Loading() {
  return <div>Loading...</div>;
}

function App() {
  return (
    <BrowserRouter>
      <Suspense fallback={<Loading />}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/settings" element={<Settings />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
}
```

### Nested Suspense Boundaries

Multiple Suspense boundaries for granular loading:

```tsx
// File: src/components/NestedLazy.tsx

import { lazy, Suspense } from 'react';

const Sidebar = lazy(() => import('./Sidebar'));
const Feed = lazy(() => import('./Feed'));
const Widget = lazy(() => import('./Widget'));

function Dashboard() {
  return (
    <div>
      <Suspense fallback={<SidebarLoading />}>
        <Sidebar />
      </Suspense>
      
      <Suspense fallback={<FeedLoading />}>
        <Feed />
      </Suspense>
      
      <Suspense fallback={<WidgetLoading />}>
        <Widget />
      </Suspense>
    </div>
  );
}
```

## Key Takeaways

- React.lazy accepts a function that returns a Promise
- Components must be default exports
- Suspense boundary required around lazy components
- Can nest Suspense for fine-grained control

## What's Next

Continue to [Bundle Analysis with Vite](/09-performance/02-code-splitting/03-bundle-analysis-with-vite.md) to learn about analyzing and optimizing your bundle.