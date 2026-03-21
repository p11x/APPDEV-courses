# Build Custom Hook Challenge

## Overview
This coding challenge tests your ability to create custom hooks. You'll build a useMousePosition hook from scratch.

## Prerequisites
- React hooks knowledge
- TypeScript basics

## Challenge

### Requirements

Create a custom hook called `useMousePosition` that:
1. Tracks the mouse position on the window
2. Returns x and y coordinates
3. Cleans up event listeners on unmount

### Starting Point

```tsx
// [File: src/challenges/useMousePosition.ts]
import { useState, useEffect } from 'react';

// Your implementation here
export function useMousePosition() {
  // TODO: Implement this hook
}

// Test component
function TestComponent() {
  const { x, y } = useMousePosition();
  return (
    <div>
      Mouse position: {x}, {y}
    </div>
  );
}
```

### Solution

```tsx
// [File: src/challenges/useMousePosition.solution.ts]
import { useState, useEffect } from 'react';

interface MousePosition {
  x: number;
  y: number;
}

export function useMousePosition(): MousePosition {
  // Initialize with null or defaults
  const [position, setPosition] = useState<MousePosition>({
    x: 0,
    y: 0
  });

  useEffect(() => {
    // Handler updates state with latest mouse position
    const handleMouseMove = (event: MouseEvent) => {
      setPosition({
        x: event.clientX,
        y: event.clientY
      });
    };

    // Add event listener when component mounts
    window.addEventListener('mousemove', handleMouseMove);

    // Cleanup: remove listener when component unmounts
    return () => {
      window.removeEventListener('mousemove', handleMouseMove);
    };
  }, []); // Empty dependency array = run once on mount

  return position;
}
```

### TypeScript Version

```tsx
// [File: src/challenges/useMousePosition.typed.ts]
import { useState, useEffect, useCallback } from 'react';

interface MousePosition {
  x: number;
  y: number;
}

// Using useCallback for better performance
export const useMousePosition = (): MousePosition => {
  const [position, setPosition] = useState<MousePosition>({ x: 0, y: 0 });

  const handleMouseMove = useCallback((event: MouseEvent) => {
    setPosition({
      x: event.clientX,
      y: event.clientY
    });
  }, []);

  useEffect(() => {
    window.addEventListener('mousemove', handleMouseMove);
    return () => window.removeEventListener('mousemove', handleMouseMove);
  }, [handleMouseMove]);

  return position;
};
```

## Key Takeaways
- Use useEffect for subscriptions
- Always clean up in return function
- Use useCallback to prevent function recreation

## What's Next
Continue to [Implement useReducer from Scratch](02-implement-useReducer-from-scratch.md) for another challenge.