# Performance Interview Questions

## Overview
Performance questions test your understanding of React's rendering behavior and optimization techniques. These are crucial for senior developer roles.

## Prerequisites
- React rendering concepts
- Performance optimization knowledge

## Core Concepts

### Question 1: Memoization

**Q: When should you use React.memo?**

```tsx
// [File: src/interview-answers/MemoExample.jsx]
import React, { memo, useState } from 'react';

// Memoized component - only re-renders when props change
const ExpensiveChild = memo(function ExpensiveChild({ data, onClick }) {
  console.log('ExpensiveChild rendered');
  return <button onClick={onClick}>{data.label}</button>;
});

function Parent() {
  const [count, setCount] = useState(0);
  
  // ❌ WRONG - New object every render
  const data = { label: 'Click me' };
  
  // ✅ CORRECT - Stable reference
  const dataStable = useMemo(() => ({ label: 'Click me' }), []);
  
  // ✅ CORRECT - Stable callback
  const handleClick = useCallback(() => {
    console.log('clicked');
  }, []);

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(c => c + 1)}>Increment</button>
      <ExpensiveChild data={dataStable} onClick={handleClick} />
    </div>
  );
}

export default Parent;
```

### Question 2: Virtualization

**Q: How do you render large lists efficiently?**

```tsx
// [File: src/interview-answers/VirtualList.jsx]
import React from 'react';
import { FixedSizeList } from 'react-window';

function VirtualList({ items }) {
  const Row = ({ index, style }) => (
    <div style={style}>
      {items[index].name}
    </div>
  );

  return (
    <FixedSizeList
      height={400}
      width={300}
      itemCount={items.length}
      itemSize={50}
    >
      {Row}
    </FixedSizeList>
  );
}

export default VirtualList;
```

### Question 3: Code Splitting

**Q: How do you reduce bundle size?**

```tsx
// [File: src/interview-answers/CodeSplitting.jsx]
import React, { Suspense, lazy } from 'react';

// Lazy load heavy components
const HeavyChart = lazy(() => import('./HeavyChart'));
const Dashboard = lazy(() => import('./Dashboard'));

function App() {
  return (
    <Suspense fallback={<Loading />}>
      <Routes>
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/chart" element={<HeavyChart />} />
      </Routes>
    </Suspense>
  );
}

export default App;
```

## Key Takeaways
- Use memo, useMemo, useCallback appropriately
- Virtualize large lists
- Code split to reduce bundle size

## What's Next
Continue to [Architecture Interview Questions](03-architecture-interview-questions.md) for architecture questions.