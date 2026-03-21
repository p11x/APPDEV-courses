# React Reconciliation Deep Dive

## Overview
Reconciliation is React's algorithm for diffing the virtual DOM and determining which changes need to be applied to the actual DOM. Understanding this process is crucial for writing performant React applications.

## Prerequisites
- JavaScript fundamentals
- Basic React knowledge

## Core Concepts

### What is Reconciliation?

React uses a diffing algorithm to compare the new virtual DOM with the previous one. This process, called reconciliation, happens every time state or props change.

### The Diffing Algorithm

React uses two key heuristics:
1. **Different elements produce different trees** - If an element type changes, React tears down the old tree and builds a new one
2. **Elements with stable keys stay the same** - Keys help React identify which elements changed

```jsx
// [File: src/components/ReconciliationDemo.jsx]
import React, { useState } from 'react';

function ReconciliationDemo() {
  const [items, setItems] = useState([
    { id: 1, name: 'Apple' },
    { id: 2, name: 'Banana' }
  ]);

  const addItem = () => {
    const newId = items.length + 1;
    setItems([...items, { id: newId, name: `Item ${newId}` }]);
  };

  return (
    <div>
      {items.map(item => (
        // Using stable keys (id) ensures efficient reconciliation
        <div key={item.id}>
          {item.name}
        </div>
      ))}
      <button onClick={addItem}>Add Item</button>
    </div>
  );
}

export default ReconciliationDemo;
```

### Fiber Architecture

React 16 introduced Fiber, a new reconciliation engine. Fiber breaks work into units and can pause, resume, or prioritize work.

```tsx
// [File: src/components/PriorityDemo.jsx]
import React, { useTransition, useState } from 'react';

function PriorityDemo() {
  const [isPending, startTransition] = useTransition();
  const [input, setInput] = useState('');
  const [list, setList] = useState([]);

  function handleChange(e) {
    setInput(e.target.value);
    // Mark this update as lower priority
    startTransition(() => {
      setList(expensiveCalculation(e.target.value));
    });
  }

  return (
    <>
      <input value={input} onChange={handleChange} />
      {isPending ? <Loading /> : <List items={list} />}
    </>
  );
}

export default PriorityDemo;
```

## Common Mistakes

### Using Index as Key

```jsx
// ❌ WRONG - Using index as key
{items.map((item, index) => (
  <Item key={index} {...item} />
))}

// ✅ CORRECT - Using stable ID as key
{items.map(item => (
  <Item key={item.id} {...item} />
))}
```

### Not Understanding Element Type Changes

```jsx
// ❌ WRONG - Changing element type causes full remount
return <div><Child /></div>;
// Changed to:
return <span><Child /></span>;

// ✅ CORRECT - Keep same element type
return <div><Child /></div>;
// Changed to:
return><Child /></div>;
```

## Key Takeaways
- Reconciliation is the diffing algorithm
- Keys must be stable and unique
- Element type changes cause full remounts

## What's Next
Continue to [Fiber Architecture Explained](02-fiber-architecture-explained.md) to learn more about React's rendering engine.