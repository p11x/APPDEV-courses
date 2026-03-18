# Lifting State Up

## Overview

Lifting state up is a fundamental React pattern where you move state from child components to their closest common ancestor. This allows multiple components to share and synchronize state. When multiple components need access to the same changing data, lifting state up to their common parent is the recommended approach.

## Prerequisites

- Understanding of React components and props
- Knowledge of useState hook
- Familiarity with component composition

## Core Concepts

### When to Lift State

```jsx
// File: src/lifting-state.jsx

import React, { useState } from 'react';

// Two siblings need to share state - lift to parent
function ParentComponent() {
  const [sharedValue, setSharedValue] = useState('');
  
  return (
    <>
      <ChildA value={sharedValue} onChange={setSharedValue} />
      <ChildB value={sharedValue} />
    </>
  );
}

function ChildA({ value, onChange }) {
  return (
    <input value={value} onChange={e => onChange(e.target.value)} />
  );
}

function ChildB({ value }) {
  return <p>Value: {value}</p>;
}
```

## Key Takeaways

- Lift state to the closest common ancestor
- Pass down state via props
- Pass up updates via callback props
- Keep state as low in the tree as possible

## What's Next

Let's explore controlled vs uncontrolled inputs.
