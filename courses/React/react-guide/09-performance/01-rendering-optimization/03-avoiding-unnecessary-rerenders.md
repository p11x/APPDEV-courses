# Avoiding Unnecessary Re-renders

## Overview

Unnecessary re-renders can significantly impact application performance, especially in large component trees. This guide covers patterns and techniques to minimize re-renders: moving state down, stabilizing references with useCallback and useMemo, and avoiding inline objects and functions in JSX.

## Prerequisites

- Understanding of React rendering
- Familiarity with hooks (useState, useCallback, useMemo)

## Core Concepts

### Moving State Down

Keep state as close to where it's used as possible:

```tsx
// File: src/components/StateColocation.tsx

import { useState } from 'react';

// ❌ BAD - State at the top causes all children to re-render
function BadParent() {
  const [count, setCount] = useState(0);
  
  return (
    <div>
      <ExpensiveChild1 />
      <ExpensiveChild2 />
      <ExpensiveChild3 />
      <button onClick={() => setCount(c => c + 1)}>{count}</button>
    </div>
  );
}

// ✅ GOOD - Move state to where it's used
function GoodParent() {
  return (
    <div>
      <ChildWithState />
      <ExpensiveChild1 />
      <ExpensiveChild2 />
    </div>
  );
}

function ChildWithState() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}

function ExpensiveChild1() {
  return <div>Child 1</div>;
}
function ExpensiveChild2() {
  return <div>Child 2</div>;
}
```

### Stabilizing with useCallback

Prevent new function references:

```tsx
// File: src/components/StableCallbacks.tsx

import { useState, useCallback } from 'react';
import { memo } from 'react';

// Child component wrapped with memo
const Button = memo(function Button({ 
  onClick, 
  children 
}: { 
  onClick: () => void; 
  children: React.ReactNode;
}) {
  console.log('Button rendered');
  return <button onClick={onClick}>{children}</button>;
});

function Parent() {
  const [count, setCount] = useState(0);
  const [text, setText] = useState('');
  
  // ❌ WRONG - New function every render defeats memo
  const handleClick1 = () => console.log('click');
  
  // ✅ CORRECT - Stable reference
  const handleClick2 = useCallback(() => console.log('click'), []);
  
  // ✅ CORRECT - Stable reference with dependency
  const handleIncrement = useCallback(() => {
    setCount(c => c + 1);
  }, []);
  
  return (
    <div>
      <Button onClick={handleClick1}>Bad Button</Button>
      <Button onClick={handleClick2}>Good Button</Button>
      <Button onClick={handleIncrement}>Increment</Button>
      <input value={text} onChange={e => setText(e.target.value)} />
    </div>
  );
}
```

### Using useMemo for Derived Data

Prevent expensive recalculations:

```tsx
// File: src/components/UseMemoExample.tsx

import { useState, useMemo } from 'react';

function ExpensiveComponent() {
  const [items, setItems] = useState([
    { id: 1, name: 'Item 1', category: 'A', price: 100 },
    { id: 2, name: 'Item 2', category: 'B', price: 200 },
    { id: 3, name: 'Item 3', category: 'A', price: 150 },
  ]);
  
  const [filter, setFilter] = useState('');
  
  // ✅ GOOD - Memoized filter results
  const filteredItems = useMemo(() => {
    return items.filter(item => 
      item.name.toLowerCase().includes(filter.toLowerCase())
    );
  }, [items, filter]);
  
  // ✅ GOOD - Memoized expensive calculation
  const totalPrice = useMemo(() => {
    return filteredItems.reduce((sum, item) => sum + item.price, 0);
  }, [filteredItems]);
  
  // ✅ GOOD - Memoized sorted array
  const sortedItems = useMemo(() => {
    return [...filteredItems].sort((a, b) => a.price - b.price);
  }, [filteredItems]);
  
  return (
    <div>
      <input value={filter} onChange={e => setFilter(e.target.value)} />
      <p>Total: ${totalPrice}</p>
      <ul>
        {sortedItems.map(item => (
          <li key={item.id}>{item.name} - ${item.price}</li>
        ))}
      </ul>
    </div>
  );
}
```

## Common Mistakes

### Creating Objects Inline

```tsx
// ❌ WRONG - New object every render
function Bad() {
  return <Child style={{ color: 'red' }} />;
}

// ✅ CORRECT - Stable object
const style = { color: 'red' };
function Good() {
  return <Child style={style} />;
}

// ✅ ALSO CORRECT - Use useMemo
function AlsoGood() {
  const style = useMemo(() => ({ color: 'red' }), []);
  return <Child style={style} />;
}
```

## Key Takeaways

- Move state down to limit re-render scope
- Use useCallback to stabilize function references
- Use useMemo to memoize expensive calculations
- Avoid inline objects and functions in JSX
- Profile before and after optimizations

## What's Next

Continue to [Dynamic Imports](/09-performance/02-code-splitting/01-dynamic-imports.md) to learn about code splitting for faster initial loads.