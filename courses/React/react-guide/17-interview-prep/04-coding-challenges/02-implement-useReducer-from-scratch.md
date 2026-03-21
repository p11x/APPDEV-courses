# Implement useReducer from Scratch

## Overview
This challenge asks you to implement your own version of useReducer to deeply understand how it works under the hood.

## Prerequisites
- useState and useEffect knowledge
- Reducer concept understanding

## Challenge

### Requirements

Implement a custom useReducer hook that:
1. Takes a reducer function and initial state
2. Returns current state and dispatch function
3. Handles state updates based on action types

### Starting Point

```typescript
// [File: src/challenges/myUseReducer.ts]
import { useState } from 'react';

// Type definitions
type Reducer<S, A> = (state: S, action: A) => S;
type Dispatch<A> = (action: A) => void;

export function myUseReducer<S, A>(
  reducer: Reducer<S, A>,
  initialState: S
): [S, Dispatch<A>] {
  // TODO: Implement this
}
```

### Solution

```typescript
// [File: src/challenges/myUseReducer.solution.ts]
import { useState, useEffect, useCallback } from 'react';

// Type definitions
type Reducer<S, A> = (state: S, action: A) => S;
type Dispatch<A> = (action: A) => void;

export function myUseReducer<S, A>(
  reducer: Reducer<S, A>,
  initialState: S
): [S, Dispatch<A>] {
  // Store state
  const [state, setState] = useState<S>(initialState);

  // Create dispatch function
  const dispatch = useCallback((action: A) => {
    // reducer returns new state
    setState(prevState => reducer(prevState, action));
  }, [reducer]);

  return [state, dispatch];
}

// Example reducer for testing
type CounterState = { count: number };
type CounterAction = { type: 'increment' } | { type: 'decrement' };

const counterReducer = (state: CounterState, action: CounterAction): CounterState => {
  switch (action.type) {
    case 'increment':
      return { count: state.count + 1 };
    case 'decrement':
      return { count: state.count - 1 };
    default:
      return state;
  }
};

// Test component
function Counter() {
  const [state, dispatch] = myUseReducer(counterReducer, { count: 0 });

  return (
    <div>
      Count: {state.count}
      <button onClick={() => dispatch({ type: 'increment' })}>+</button>
      <button onClick={() => dispatch({ type: 'decrement' })}>-</button>
    </div>
  );
}
```

### Advanced: Add Lazy Initialization

```typescript
// [File: src/challenges/myUseReducer.advanced.ts]
import { useState, useCallback } from 'react';

type Reducer<S, A> = (state: S, action: A) => S;
type Dispatch<A> = (action: A) => void;
type Initializer<S> = () => S;

export function myUseReducer<S, A>(
  reducer: Reducer<S, A>,
  initialState: S | Initializer<S>
): [S, Dispatch<A>] {
  // Handle lazy initialization
  const [state, setState] = useState<S>(() => {
    if (typeof initialState === 'function') {
      return (initialState as Initializer<S>)();
    }
    return initialState;
  });

  const dispatch = useCallback((action: A) => {
    setState(prevState => reducer(prevState, action));
  }, [reducer]);

  return [state, dispatch];
}
```

## Key Takeaways
- useReducer uses useState internally
- Reducer is a pure function
- Dispatch is stable across renders

## What's Next
Continue to [Virtual DOM Implementation](03-virtual-dom-implementation.md) for an advanced challenge.