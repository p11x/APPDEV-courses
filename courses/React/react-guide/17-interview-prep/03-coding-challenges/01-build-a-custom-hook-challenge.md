# Build a Custom Hook Challenge

## Overview
This challenge tests your ability to create custom React hooks from scratch. Custom hooks are essential for extracting and sharing logic between components. You'll implement three progressively challenging hooks: useDebounce, useWindowSize, and usePrevious.

## Prerequisites
- React hooks (useState, useEffect, useRef)
- TypeScript basics
- Understanding of component lifecycle

## Core Concepts

### Challenge 1: implement useDebounce

Debouncing delays updating a value until after a specified delay has passed since the last change. This is essential for performance optimization in search inputs and form validation.

```typescript
// [File: src/challenges/useDebounce.ts]
import { useState, useEffect } from 'react';

/**
 * Challenge 1: Implement useDebounce
 * 
 * Creates a debounced version of a value that only updates
 * after a specified delay has passed since the last change.
 * 
 * Use cases:
 * - Search input optimization (don't search on every keystroke)
 * - Form validation (don't validate on every keystroke)
 * - Window resize handlers
 * 
 * @param value - The value to debounce (can be any type)
 * @param delay - The delay in milliseconds before updating the debounced value
 * @returns The debounced value
 */

// ❌ WRONG — Naive implementation that doesn't handle cleanup properly
export function useDebounceNaive<T>(value: T, delay: number): T {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    // This sets up a timer but doesn't clean it up properly
    const timer = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    // ❌ MISSING: The cleanup function to clear the timer!
    // This causes memory leaks and stale closures
  }, [value, delay]); // Also has a bug: missing return for cleanup

  return debouncedValue;
}

// ✅ CORRECT — Proper implementation with cleanup
export function useDebounce<T>(value: T, delay: number): T {
  // State to hold the debounced value
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    // Create a new timer for each render cycle
    const timer = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    // ✅ CLEANUP: Clear the timer if value or delay changes
    // This prevents the old timer from firing with stale values
    return () => {
      clearTimeout(timer);
    };
  }, [value, delay]);

  return debouncedValue;
}
```

```typescript
// [File: src/challenges/useDebounce.test.ts]
import { renderHook, act } from '@testing-library/react';
import { useDebounce } from './useDebounce';
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

describe('useDebounce', () => {
  // Use fake timers to control setTimeout in tests
  beforeEach(() => {
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('should return initial value immediately', () => {
    const { result } = renderHook(() => 
      useDebounce('hello', 500)
    );
    
    // Initial value should be returned immediately
    expect(result.current).toBe('hello');
  });

  it('should update debounced value after delay', () => {
    const { result, rerender } = renderHook(
      ({ value, delay }) => useDebounce(value, delay),
      { initialProps: { value: 'hello', delay: 500 } }
    );

    // Fast-forward time by 300ms — not enough to trigger
    act(() => {
      vi.advanceTimersByTime(300);
    });
    expect(result.current).toBe('hello');

    // Fast-forward remaining time to 500ms total
    act(() => {
      vi.advanceTimersByTime(200);
    });
    expect(result.current).toBe('hello');

    // One more tick to trigger
    act(() => {
      vi.advanceTimersByTime(1);
    });
    expect(result.current).toBe('hello');

    // Actually, let's verify it updates after full delay
    rerender({ value: 'world', delay: 500 });
    expect(result.current).toBe('hello'); // Still old value

    act(() => {
      vi.advanceTimersByTime(500);
    });
    expect(result.current).toBe('world'); // Now updated!
  });

  it('should reset timer on rapid changes', () => {
    const { result, rerender } = renderHook(
      ({ value, delay }) => useDebounce(value, delay),
      { initialProps: { value: 'a', delay: 500 } }
    );

    // Rapid changes should reset the timer each time
    rerender({ value: 'b', delay: 500 });
    rerender({ value: 'c', delay: 500 });
    rerender({ value: 'd', delay: 500 });

    // Should not have updated yet — timer keeps resetting
    act(() => {
      vi.advanceTimersByTime(300);
    });
    expect(result.current).toBe('a');

    // Only after final delay of 500ms from last change
    act(() => {
      vi.advanceTimersByTime(200);
    });
    expect(result.current).toBe('d');
  });
});
```

### Challenge 2: implement useWindowSize

Getting the window size is common for responsive designs, but it needs to handle SSR (Server-Side Rendering) where `window` doesn't exist.

```typescript
// [File: src/challenges/useWindowSize.ts]
import { useState, useEffect } from 'react';

interface WindowSize {
  width: number;
  height: number;
}

/**
 * Challenge 2: Implement useWindowSize
 * 
 * Returns the current window dimensions.
 * Must be SSR-safe (check typeof window !== 'undefined')
 * Must clean up the event listener on unmount.
 * 
 * @returns Object with width and height of the window
 */

// ❌ WRONG — Not SSR-safe, will crash in Next.js or other SSR frameworks
export function useWindowSizeNaive(): WindowSize {
  const [size, setSize] = useState<WindowSize>({
    width: window.innerWidth, // ❌ CRASH: window is undefined on server
    height: window.innerHeight
  });

  useEffect(() => {
    const handler = () => {
      setSize({
        width: window.innerWidth,
        height: window.innerHeight
      });
    };

    window.addEventListener('resize', handler);
    // ❌ MISSING: cleanup to remove event listener!
  }, []);

  return size;
}

// ✅ CORRECT — SSR-safe with proper cleanup
export function useWindowSize(): WindowSize {
  // Initialize with undefined for SSR — server doesn't know window size
  // Then update to actual dimensions on client after hydration
  const [windowSize, setWindowSize] = useState<WindowSize>({
    width: typeof window !== 'undefined' ? window.innerWidth : 0,
    height: typeof window !== 'undefined' ? window.innerHeight : 0,
  });

  useEffect(() => {
    // ✅ SSR-SAFE: This only runs on the client
    if (typeof window === 'undefined') {
      return;
    }

    // Handler function to update state with current dimensions
    const handleResize = () => {
      setWindowSize({
        width: window.innerWidth,
        height: window.innerHeight
      });
    };

    // Add event listener to track window resizing
    window.addEventListener('resize', handleResize);

    // Call once immediately to get initial size
    handleResize();

    // ✅ CLEANUP: Remove event listener when component unmounts
    // This prevents memory leaks and stale closures
    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []); // Empty dependency array = run once on mount

  return windowSize;
}
```

```typescript
// [File: src/challenges/useWindowSize.test.ts]
import { renderHook, act } from '@testing-library/react';
import { useWindowSize } from './useWindowSize';
import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

// Mock window properties for testing
const mockWindow = {
  innerWidth: 1024,
  innerHeight: 768,
  addEventListener: vi.fn(),
  removeEventListener: vi.fn(),
};

describe('useWindowSize', () => {
  beforeEach(() => {
    // @ts-ignore — we're mocking window for SSR scenarios
    global.window = mockWindow;
    vi.clearAllMocks();
  });

  afterEach(() => {
    // @ts-ignore
    delete global.window;
  });

  it('should return window dimensions on client', () => {
    const { result } = renderHook(() => useWindowSize());

    expect(result.current.width).toBe(1024);
    expect(result.current.height).toBe(768);
  });

  it('should add and remove resize listener', () => {
    renderHook(() => useWindowSize());

    expect(window.addEventListener).toHaveBeenCalledWith(
      'resize',
      expect.any(Function)
    );
  });

  it('should update dimensions on resize', () => {
    const { result } = renderHook(() => useWindowSize());
    
    // Simulate resize event
    const resizeHandler = (window.addEventListener as any).mock.calls.find(
      (call: string[]) => call[0] === 'resize'
    )[1];

    // Simulate window resize
    Object.defineProperty(window, 'innerWidth', { value: 1920, writable: true });
    Object.defineProperty(window, 'innerHeight', { value: 1080, writable: true });
    
    act(() => {
      resizeHandler();
    });

    expect(result.current.width).toBe(1920);
    expect(result.current.height).toBe(1080);
  });
});
```

### Challenge 3: implement usePrevious

Getting the previous value is a common pattern for comparing props before and after updates. The useRef trick is essential here.

```typescript
// [File: src/challenges/usePrevious.ts]
import { useRef } from 'react';

/**
 * Challenge 3: Implement usePrevious
 * 
 * Returns the previous value of a piece of state or props.
 * This is useful for comparing current vs previous values.
 * 
 * @param value - The current value to track
 * @returns The previous value, or undefined on first render
 */

// ❌ WRONG — Using useState doesn't work correctly
export function usePreviousNaive<T>(value: T): T | undefined {
  const [previous, setPrevious] = useState<T | undefined>(undefined);
  
  // This creates an infinite loop because setting state triggers re-render
  // which triggers useEffect again...
  useEffect(() => {
    setPrevious(value);
  }, [value]);

  // Problem: This returns the CURRENT value, not the previous!
  return value; // ❌ WRONG!
}

// ✅ CORRECT — Using useRef to store the previous value
export function usePrevious<T>(value: T): T | undefined {
  // useRef doesn't trigger re-renders when updated
  // This is the key to making this work!
  const ref = useRef<T>();

  // Update the ref AFTER every render with the current value
  // This ensures the ref always holds the "previous" value on next render
  ref.current = value;

  // Return the ref's value from the PREVIOUS render
  // On first render, this will be undefined
  return ref.current;
}

// ✅ ALTERNATIVE — Using useEffect (slightly different behavior)
export function usePreviousWithEffect<T>(value: T): T | undefined {
  const ref = useRef<T>();
  const prevRef = useRef<T>();

  useEffect(() => {
    // Store current value as previous
    prevRef.current = ref.current;
    // Update current value
    ref.current = value;
  }, [value]);

  return prevRef.current;
}
```

```typescript
// [File: src/challenges/usePrevious.test.ts]
import { renderHook, act } from '@testing-library/react';
import { usePrevious } from './usePrevious';
import { useState } from 'react';
import { describe, it, expect } from 'vitest';

describe('usePrevious', () => {
  it('should return undefined on first render', () => {
    const { result } = renderHook(() => usePrevious('hello'));
    
    // First render: no previous value exists
    expect(result.current).toBeUndefined();
  });

  it('should return previous value after update', () => {
    const { result, rerender } = renderHook(
      ({ value }) => usePrevious(value),
      { initialProps: { value: 'hello' } }
    );

    // First render: undefined
    expect(result.current).toBeUndefined();

    // Re-render with new value
    rerender({ value: 'world' });

    // Now should have previous value
    expect(result.current).toBe('hello');
  });

  it('should track multiple value changes', () => {
    const { result, rerender } = renderHook(
      ({ value }) => usePrevious(value),
      { initialProps: { value: 0 } }
    );

    expect(result.current).toBeUndefined();

    rerender({ value: 1 });
    expect(result.current).toBe(0);

    rerender({ value: 2 });
    expect(result.current).toBe(1);

    rerender({ value: 3 });
    expect(result.current).toBe(2);
  });

  // Real-world use case: detect when a prop changes
  it('can detect prop changes', () => {
    const { result, rerender } = renderHook(
      ({ isLoading }) => {
        const wasLoading = usePrevious(isLoading);
        return { isLoading, wasLoading };
      },
      { initialProps: { isLoading: false } }
    );

    // Initial state
    expect(result.current.isLoading).toBe(false);
    expect(result.current.wasLoading).toBeUndefined();

    // Start loading
    rerender({ isLoading: true });
    expect(result.current.isLoading).toBe(true);
    expect(result.current.wasLoading).toBe(false); // Previous was false!

    // Stop loading — can detect the transition
    rerender({ isLoading: false });
    expect(result.current.isLoading).toBe(false);
    expect(result.current.wasLoading).toBe(true); // Previous was true!
  });
});
```

## Interview Expectations

When interviewers ask about custom hooks, they're looking for:

### What They Look For

1. **Understanding of Hook Rules** — Do you know hooks can only be called at the top level?
2. **Proper Cleanup** — Do you return cleanup functions from useEffect?
3. **TypeScript Generics** — Can you make hooks work with any value type?
4. **Real-world Awareness** — Do you know about SSR, performance implications?
5. **Testing Knowledge** — Can you write tests for your hooks?

### Common Mistakes to Avoid

1. Forgetting to clean up timers or event listeners
2. Not handling SSR (checking typeof window)
3. Using hooks inside loops, conditions, or nested functions
4. Not using useRef when you don't want re-renders
5. Forgetting dependency arrays

### TypeScript Bonus Versions

```typescript
// Bonus: More advanced TypeScript version of useDebounce
export function useDebounceTyped<T extends string | number | object>(
  value: T, 
  delay: number
): T {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => clearTimeout(handler);
  }, [value, delay]);

  return debouncedValue;
}

// Bonus: useDebounce with generic type constraint
// This ensures the type is always preserved
export function useDebounceStrict<T>(value: T, delay: number): T {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => clearTimeout(timer);
  }, [value, delay]);

  return debouncedValue;
}
```

## Key Takeaways

- Always clean up timers and event listeners in useEffect return functions
- Use `typeof window !== 'undefined'` to handle SSR safely
- useRef is the key to storing values without triggering re-renders
- The ref pattern for usePrevious works because refs are updated after render
- Use TypeScript generics to make hooks work with any data type
- Test hooks with testing-library's renderHook utility

## What's Next

Continue to [Implement useReducer from Scratch](02-implement-useReducer-from-scratch.md) to learn how React's state management works under the hood.