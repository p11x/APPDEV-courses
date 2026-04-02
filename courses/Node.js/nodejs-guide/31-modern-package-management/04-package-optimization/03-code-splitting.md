# Code Splitting

## What You'll Learn

- What code splitting is
- How to split code by route
- How to split code by component
- How dynamic imports work

## What Is Code Splitting?

Instead of shipping one large JavaScript file, code splitting creates multiple smaller chunks that load on demand.

```
Before splitting:
  bundle.js (500KB) → loads everything upfront

After splitting:
  main.js (100KB)    → loads immediately
  vendor.js (200KB)  → cached, loads once
  route-home.js (50KB) → loads when visiting /
  route-about.js (30KB) → loads when visiting /about
```

## Dynamic Imports

```js
// Lazy load a module — only downloaded when called
async function loadChart() {
  const { Chart } = await import('./chart.js');
  return new Chart();
}

// React lazy loading
import { lazy, Suspense } from 'react';

const Dashboard = lazy(() => import('./pages/Dashboard.js'));
const Settings = lazy(() => import('./pages/Settings.js'));

function App() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/settings" element={<Settings />} />
      </Routes>
    </Suspense>
  );
}
```

## Route-Based Splitting

```js
// Vite (automatic)
// Just use dynamic imports — Vite splits automatically

// routes.js
export const routes = [
  {
    path: '/',
    component: () => import('./pages/Home.js'),
  },
  {
    path: '/about',
    component: () => import('./pages/About.js'),
  },
];
```

## Vendor Splitting

```js
// vite.config.js
export default {
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ['react', 'react-dom'],
          router: ['react-router-dom'],
          ui: ['@radix-ui/react-dialog', '@radix-ui/react-dropdown'],
        },
      },
    },
  },
};
```

## Measuring Impact

```bash
# Vite
npx vite build --mode production
# Check dist/ for chunk sizes

# webpack
npx webpack --mode production --json > stats.json
npx webpack-bundle-analyzer stats.json
```

## Next Steps

For performance metrics, continue to [Performance Metrics](./04-performance-metrics.md).
