# Fiber Architecture Explained

## Overview
React Fiber is the reconciliation engine introduced in React 16. It replaces the old stack-based reconciliation with a new fiber-based system that enables incremental rendering and better priority handling.

## Prerequisites
- Virtual DOM knowledge
- React rendering concepts

## Core Concepts

### What is Fiber?

Fiber is React's new reconciliation algorithm. It's designed to:
- Break rendering work into manageable units
- Pause, resume, or prioritize work
- Support concurrent features

```tsx
// [File: src/components/FiberDemo.jsx]
import React, { useState, useEffect, useRef } from 'react';

function FiberDemo() {
  const [count, setCount] = useState(0);
  const prevCountRef = useRef(count);

  // useEffect runs after render - part of Fiber's commit phase
  useEffect(() => {
    console.log(`Count changed from ${prevCountRef.current} to ${count}`);
    prevCountRef.current = count;
  }, [count]);

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(c => c + 1)}>Increment</button>
    </div>
  );
}

export default FiberDemo;
```

### Fiber Work Phases

1. **Render Phase** - Async, can be paused
2. **Commit Phase** - Sync, cannot be interrupted

```tsx
// [File: src/components/WorkPhases.jsx]
import React, { useState, useEffect, useLayoutEffect } from 'react';

function WorkPhases() {
  const [value, setValue] = useState('');

  // LayoutEffect runs synchronously after DOM mutations
  // Used for measurements that need to happen before paint
  useLayoutEffect(() => {
    const rect = document.getElementById('target')?.getBoundingClientRect();
    console.log('Layout measured:', rect);
  }, [value]);

  return (
    <div>
      <input 
        id="target"
        value={value} 
        onChange={e => setValue(e.target.value)} 
      />
    </div>
  );
}

export default WorkPhases;
```

### Priority Levels

Fiber assigns priority to work:

```tsx
// [File: src/components/PriorityLevels.jsx]
import React, { useTransition, useDeferredValue } from 'react';

function PriorityLevels() {
  const [text, setText] = useState('');
  
  // useTransition marks updates as low priority
  const [isPending, startTransition] = useTransition();
  
  // useDeferredValue is similar but for values
  const deferredText = useDeferredValue(text);

  function handleChange(e) {
    const value = e.target.value;
    setText(value);
    // Process heavy updates with lower priority
    startTransition(() => {
      // Heavy processing here
    });
  }

  return (
    <input onChange={handleChange} />
  );
}

export default PriorityLevels;
```

## Key Takeaways
- Fiber enables incremental rendering
- Render phase is async, commit is sync
- Priority levels help optimize user experience

## What's Next
Continue to [Concurrent Mode and Scheduling](03-concurrent-mode-and-scheduling.md) to understand how React schedules work.