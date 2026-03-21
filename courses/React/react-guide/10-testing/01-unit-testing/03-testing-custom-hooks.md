# Testing Custom Hooks

## Overview

Custom hooks are a powerful pattern in React, but testing them requires special consideration since they can't be rendered directly. React Testing Library provides renderHook for testing hooks in isolation.

## Prerequisites

- Understanding of custom hooks
- RTL testing basics

## Core Concepts

### Installing renderHook

```bash
npm install @testing-library/react-hooks
# Or use the built-in version in newer RTL
```

### Testing a Custom Hook

```tsx
// File: src/hooks/useCounter.test.ts

import { renderHook, act } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import { useCounter } from './useCounter';

describe('useCounter', () => {
  it('initializes with default value', () => {
    const { result } = renderHook(() => useCounter());
    expect(result.current.count).toBe(0);
  });

  it('initializes with custom value', () => {
    const { result } = renderHook(() => useCounter(10));
    expect(result.current.count).toBe(10);
  });

  it('increments count', () => {
    const { result } = renderHook(() => useCounter());
    
    act(() => {
      result.current.increment();
    });
    
    expect(result.current.count).toBe(1);
  });

  it('decrements count', () => {
    const { result } = renderHook(() => useCounter(5));
    
    act(() => {
      result.current.decrement();
    });
    
    expect(result.current.count).toBe(4);
  });
});
```

### Testing Hooks with Dependencies

```tsx
import { renderHook, act } from '@testing-library/react';
import { useDebounce } from './useDebounce';

describe('useDebounce', () => {
  it('debounces value', async () => {
    const { result, rerender } = renderHook(
      ({ value }) => useDebounce(value, 500),
      { initialProps: { value: 'initial' } }
    );
    
    expect(result.current).toBe('initial');
    
    rerender({ value: 'updated' });
    expect(result.current).toBe('initial');
    
    // Wait for debounce delay
    await act(async () => {
      await new Promise(resolve => setTimeout(resolve, 500));
    });
    
    expect(result.current).toBe('updated');
  });
});
```

## Key Takeaways

- Use renderHook to test hooks
- Wrap state updates in act()
- Test different initial props
- Mock dependencies when needed

## What's Next

Continue to [User Event Testing](/10-testing/02-integration-testing/01-user-event-testing.md) to learn about integration testing patterns.