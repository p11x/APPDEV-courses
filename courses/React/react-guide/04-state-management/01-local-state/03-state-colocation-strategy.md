# State Colocation Strategy

## Overview

State colocation is the practice of keeping state as close as possible to where it's used. This improves performance, maintainability, and makes code easier to understand. Instead of lifting all state to the top, you keep it in the component that actually needs it.

## Prerequisites

- Understanding of useState
- Knowledge of component composition

## Core Concepts

```jsx
// File: src/colocation.jsx

import React, { useState } from 'react';

// ❌ BAD: State lifted too high
function BadParent() {
  const [buttonClicked, setButtonClicked] = useState(false);
  // Button is deeply nested but state is here
  
  return <DeepChild clicked={buttonClicked} onClick={() => setButtonClicked(true)} />;
}

// ✅ GOOD: State co-located with where it's used
function GoodParent() {
  return <ChildWithLocalState />;
}

function ChildWithLocalState() {
  const [clicked, setClicked] = useState(false);
  
  return <button onClick={() => setClicked(true)}>{clicked ? 'Clicked!' : 'Click me'}</button>;
}
```

## Key Takeaways

- Keep state where it's used
- Only lift when siblings need to share
- Avoid premature optimization
- Colocation improves re-render performance
