# Understanding React Renders

## Overview

React's rendering behavior is fundamental to understanding performance optimization. When a component renders, React executes its function to determine what UI to produce. However, rendering doesn't always mean DOM updates—React reconciles changes virtually before updating the actual DOM. This guide explains when and why React re-renders, how to identify unnecessary renders, and using React DevTools for analysis.

## Prerequisites

- Solid understanding of React components and hooks
- Familiarity with JavaScript closures
- Basic understanding of the virtual DOM concept

## Core Concepts

### When React Re-renders

React re-renders a component when:

1. **State changes** - Using useState or setState
2. **Props change** - Parent passes new props
3. **Context changes** - Using useContext and the value changes
4. **Parent re-renders** - A component's parent re-renders (even if props haven't changed)

```tsx
// File: src/components/RenderExamples.tsx

import { useState } from 'react';

// Parent component - re-renders when its state changes
function ParentComponent() {
  // This state causes re-render when changed
  const [count, setCount] = useState(0);
  
  // This callback is recreated on every render
  const handleClick = () => {
    console.log('clicked');
  };
  
  return (
    <div>
      <p>Count: {count}</p>
      {/* Child receives new prop reference on every render */}
      <ChildComponent onClick={handleClick} />
      <button onClick={() => setCount(c => c + 1)}>
        Increment
      </button>
    </div>
  );
}

// Child component - re-renders when:
// 1. Parent re-renders (props or state changes)
// 2. Its own state changes
// 3. Context it consumes changes
function ChildComponent({ onClick }: { onClick: () => void }) {
  console.log('Child rendered');
  
  return (
    <button onClick={onClick}>
      Child Button
    </button>
  );
}

export default ParentComponent;
```

### Render vs DOM Update

Important distinction: rendering doesn't equal DOM updates.

```tsx
// File: src/components/RenderVsDOM.tsx

import { useState, useEffect } from 'react';

function RenderVsDOM() {
  const [count, setCount] = useState(0);
  
  // This effect runs after every render
  useEffect(() => {
    console.log('Component rendered');
  });
  
  // When count changes:
  // 1. Component re-renders (function runs)
  // 2. React compares virtual DOM
  // 3. Only "Count: X" text updates in actual DOM
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(c => c + 1)}>
        Increment
      </button>
    </div>
  );
}

export default RenderVsDOM;
```

### Using React DevTools Profiler

The React DevTools Profiler helps identify performance issues:

```tsx
// File: src/components/ProfileExample.tsx

import { useState, useMemo } from 'react';

// Example component with potential performance issues
function ProductList() {
  const [products, setProducts] = useState([
    { id: 1, name: 'Product 1', price: 100 },
    { id: 2, name: 'Product 2', price: 200 },
    { id: 3, name: 'Product 3', price: 300 },
  ]);
  
  const [filter, setFilter] = useState('');
  
  // Expensive operation on every render - no memoization
  const filteredProducts = products.filter(p => 
    p.name.toLowerCase().includes(filter.toLowerCase())
  );
  
  return (
    <div>
      <input 
        value={filter} 
        onChange={e => setFilter(e.target.value)}
        placeholder="Filter products"
      />
      <ul>
        {filteredProducts.map(p => (
          <li key={p.id}>{p.name} - ${p.price}</li>
        ))}
      </ul>
    </div>
  );
}

export default ProductList;
```

### Strict Mode Double Rendering

In development, React StrictMode intentionally double-renders to help find side effects:

```tsx
// File: src/App.tsx

import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import App from './App';

// StrictMode helps identify problems by double-invoking:
// - useEffect setup/cleanup functions
// - useState initializers (but not subsequent renders)
// - useMemo/memo callbacks (but not the memoized values)

function Root() {
  return (
    <StrictMode>
      <App />
    </StrictMode>
  );
}

const root = createRoot(document.getElementById('root')!);
root.render(<Root />);
```

## Common Mistakes

### Assuming Re-renders Are Always Bad

Don't optimize prematurely—React is fast enough for most cases.

```tsx
// ❌ WRONG - Premature optimization
const Button = React.memo(function Button({ onClick }) {
  return <button onClick={onClick}>Click</button>;
});

// ✅ CORRECT - Only optimize when there's a real performance problem
// Profile first, then optimize
```

### Not Understanding Prop Reference Changes

Creating functions/objects inline causes unnecessary re-renders:

```tsx
// ❌ WRONG - New function created every render
function Parent() {
  return <Child onClick={() => console.log('hi')} />;
}

// ✅ CORRECT - Stable function reference
function Parent() {
  const handleClick = useCallback(() => console.log('hi'), []);
  return <Child onClick={handleClick} />;
}
```

## Key Takeaways

- React re-renders when state, props, or context changes
- Rendering ≠ DOM update—React only updates changed DOM nodes
- Use React DevTools Profiler to identify slow renders
- StrictMode double-renders in development (not production)
- Optimize only after profiling—don't prematurely optimize

## What's Next

Continue to [React.memo Explained](/09-performance/01-rendering-optimization/02-react-memo-explained.md) to learn how to prevent unnecessary re-renders with memoization.