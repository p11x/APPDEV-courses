# Dynamic Imports for Code Splitting

## Overview

Code splitting allows you to break your JavaScript bundle into smaller chunks that can be loaded on demand. This reduces initial load time by only loading the code users need. This guide covers JavaScript's dynamic import syntax and how to use it for lazy loading.

## Prerequisites

- Understanding of JavaScript modules
- Familiarity with React components

## Core Concepts

### Dynamic import() Syntax

Dynamic imports work like regular function calls but return promises:

```tsx
// File: src/utils/dynamicImport.ts

// Static import - loads immediately
import { add } from './math';
console.log(add(1, 2)); // Available immediately

// Dynamic import - loads on demand
async function loadMathModule() {
  // The module loads when this code runs
  const math = await import('./math');
  console.log(math.add(1, 2));
}

// Use dynamic import for conditional loading
async function getFeature() {
  if (someCondition) {
    const { HeavyFeature } = await import('./HeavyFeature');
    return <HeavyFeature />;
  }
  return null;
}
```

### Using Dynamic Imports in React

Load components on demand:

```tsx
// File: src/components/LazyModal.tsx

import { useState } from 'react';

function LazyModal() {
  const [isOpen, setIsOpen] = useState(false);
  const [Modal, setModal] = useState<any>(null);

  const openModal = async () => {
    // Dynamically import the modal component
    const module = await import('./Modal');
    setModal(() => module.default);
    setIsOpen(true);
  };

  return (
    <div>
      <button onClick={openModal}>Open Modal</button>
      {isOpen && Modal && <Modal onClose={() => setIsOpen(false)} />}
    </div>
  );
}
```

### Loading States During Import

Handle the loading state:

```tsx
// File: src/components/WithLoading.tsx

import { useState, useEffect } from 'react';

function useDynamicImport(importFn: () => Promise<any>) {
  const [component, setComponent] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    setLoading(true);
    importFn()
      .then((module) => {
        setComponent(() => module.default);
      })
      .catch((err) => {
        setError(err);
      })
      .finally(() => {
        setLoading(false);
      });
  }, [importFn]);

  return { component, loading, error };
}

function LazyWrapper() {
  const { component: Component, loading, error } = useDynamicImport(
    () => import('./HeavyComponent')
  );

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error loading component</div>;
  if (Component) return <Component />;
  return null;
}
```

## Key Takeaways

- Use dynamic import() for code that isn't needed immediately
- Dynamic imports return promises
- Combine with React.lazy for route-based splitting
- Consider preloading important modules

## What's Next

Continue to [React.lazy and Suspense](/09-performance/02-code-splitting/02-react-lazy-and-suspense.md) to learn about React's built-in lazy loading with Suspense.