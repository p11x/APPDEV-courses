# Implement useReducer from Scratch

## Overview
This challenge explores how React's state management works under the hood by reimplementing useReducer from more basic primitives. You'll understand the relationship between useState and useReducer, then build a custom implementation.

## Prerequisites
- React hooks (useState, useEffect, useRef)
- Reducer concept (state, action, newState)
- TypeScript basics

## Core Concepts

### Step 1: Show Equivalence with useState

Understanding that useState is actually implemented using useReducer internally helps demystify state management in React.

```typescript
// [File: src/reducers/useStateAsReducer.ts]
import { useState } from 'react';

/**
 * Step 1: Understand that useState is useReducer under the hood
 * 
 * useState(state) is equivalent to:
 * useReducer((state, action) => action(state), initialState)
 * 
 * The "reducer" is simply a function that takes the current state
 * and returns a new value. For useState, the "action" is the new value.
 */

// This is essentially how useState works internally:
function useStateEquivalent<T>(initialValue: T): [T, (value: T) => void] {
  // We use a reducer where the "action" IS the new value
  const reducer = (state: T, action: T): T => action;
  
  // This would use useReducer internally
  // const [state, dispatch] = useReducer(reducer, initialValue);
  // return [state, dispatch];
  
  // But for demonstration, let's just use useState
  const [state, setState] = useState<T>(initialValue);
  
  return [state, setState];
}

// Example of using the "reducer pattern" with useState:
// Instead of passing a value, we pass a function that transforms the state
function CounterWithFunctionalUpdate() {
  const [count, setCount] = useState(0);
  
  // Functional update - the "action" is a function
  const increment = () => setCount(prev => prev + 1);
  
  return (
    <button onClick={increment}>
      Count: {count}
    </button>
  );
}

/**
 * Key insight: useState's functional update form
 * setCount(prev => prev + 1)
 * 
 * is equivalent to a reducer:
 * reducer = (state, action) => action(state)
 * 
 * When you pass a function, it becomes the "action" that receives
 * the current state and returns the new state.
 */
```

### Step 2: Build useReducer from React.useRef + forceUpdate

This demonstrates how React could implement useReducer without using useState, using useRef to hold state and a forceUpdate pattern to trigger re-renders.

```typescript
// [File: src/reducers/myUseReducer.ts]
import { useState, useRef, useCallback, useEffect } from 'react';

/**
 * Step 2: Implement useReducer using useRef + forceUpdate
 * 
 * This shows the core concept without needing Fiber internals:
 * - useRef holds the state (doesn't trigger re-renders when updated)
 * - A stateful counter triggers re-renders when we want to "commit" changes
 */

type Reducer<S, A> = (state: S, action: A) => S;
type Dispatch<A> = (action: A) => void;

/**
 * A minimal implementation of useReducer
 * 
 * @param reducer - The reducer function
 * @param initialState - The initial state
 * @returns [current state, dispatch function]
 */
export function myUseReducer<S, A>(
  reducer: Reducer<S, A>,
  initialState: S
): [S, Dispatch<A>] {
  // Use useRef to store the state — doesn't trigger re-renders
  // This is the core trick: we manually control when to re-render
  const stateRef = useRef<S>(initialState);
  
  // Use useState solely to trigger re-renders
  // The actual value doesn't matter — we just need a change
  const [, forceUpdate] = useState(0);
  
  // Get current state — read from ref, not from closure
  const getState = useCallback(() => stateRef.current, []);
  
  // Dispatch function — this is what components call
  const dispatch = useCallback((action: A) => {
    // Calculate new state using the reducer
    const newState = reducer(stateRef.current, action);
    
    // Update the ref (doesn't trigger render yet)
    stateRef.current = newState;
    
    // Force a re-render so the component sees the new state
    forceUpdate(n => n + 1);
  }, [reducer]);
  
  // Return the current state and dispatch
  // On each render, we read from the ref
  return [stateRef.current, dispatch];
}
```

```typescript
// [File: src/reducers/useReducer.examples.ts]
import React from 'react';
import { myUseReducer } from './myUseReducer';

/**
 * Example usage of our custom useReducer
 */

// Define state and action types
interface State {
  count: number;
  isLoading: boolean;
  error: string | null;
}

type Action = 
  | { type: 'increment' }
  | { type: 'decrement' }
  | { type: 'reset' }
  | { type: 'setLoading'; payload: boolean }
  | { type: 'setError'; payload: string | null };

// The reducer function
const counterReducer = (state: State, action: Action): State => {
  switch (action.type) {
    case 'increment':
      return { ...state, count: state.count + 1 };
    case 'decrement':
      return { ...state, count: state.count - 1 };
    case 'reset':
      return { ...state, count: 0 };
    case 'setLoading':
      return { ...state, isLoading: action.payload };
    case 'setError':
      return { ...state, error: action.payload };
    default:
      return state;
  }
};

// Component using our custom useReducer
function CounterWithMyReducer() {
  const [state, dispatch] = myUseReducer<State, Action>(counterReducer, {
    count: 0,
    isLoading: false,
    error: null
  });

  return (
    <div>
      <p>Count: {state.count}</p>
      <button onClick={() => dispatch({ type: 'increment' })}>
        +1
      </button>
      <button onClick={() => dispatch({ type: 'decrement' })}>
        -1
      </button>
      <button onClick={() => dispatch({ type: 'reset' })}>
        Reset
      </button>
    </div>
  );
}
```

### Step 3: Add Middleware Layer

Middleware intercepts dispatches, enabling patterns like Redux Thunk, logging, and persistence.

```typescript
// [File: src/reducers/useReducerWithMiddleware.ts]
import { useState, useRef, useCallback } from 'react';

/**
 * Step 3: Add middleware support — intercept dispatches like Redux
 * 
 * Middleware is a function that wraps the dispatch function:
 * middleware = (store) => (next) => (action) => result
 */

type Reducer<S, A> = (state: S, action: A) => S;
type Dispatch<A> = (action: A) => void;
type Middleware<S, A> = (
  dispatch: Dispatch<A>, 
  getState: () => S
) => (action: A) => void;

/**
 * Enhanced useReducer with middleware support
 */
export function useReducerWithMiddleware<S, A>(
  reducer: Reducer<S, A>,
  initialState: S,
  middlewares: Middleware<S, A>[] = []
): [S, Dispatch<A>] {
  const stateRef = useRef<S>(initialState);
  const [, forceUpdate] = useState(0);
  
  // Base dispatch — just updates state
  const baseDispatch = useCallback((action: A) => {
    const newState = reducer(stateRef.current, action);
    stateRef.current = newState;
    forceUpdate(n => n + 1);
  }, [reducer]);
  
  // Build middleware chain: each middleware wraps the next
  const getState = useCallback(() => stateRef.current, []);
  
  let dispatch: Dispatch<A> = baseDispatch;
  
  // Apply middlewares in reverse order (last middleware wraps first)
  for (let i = middlewares.length - 1; i >= 0; i--) {
    dispatch = middlewares[i](dispatch, getState);
  }
  
  return [stateRef.current, dispatch];
}
```

```typescript
// [File: src/reducers/middlewareExamples.ts]
import { useReducerWithMiddleware } from './useReducerWithMiddleware';

/**
 * Example middlewares for our enhanced useReducer
 */

// Logger middleware — logs all actions and state changes
function loggerMiddleware<S, A>(dispatch: (a: A) => void, getState: () => S) {
  return (action: A) => {
    console.log('📤 Dispatching:', action);
    console.log('📊 Current state:', getState());
    
    // Call the actual dispatch
    dispatch(action);
    
    console.log('✅ New state:', getState());
  };
}

// Persistence middleware — saves state to localStorage
function persistMiddleware<S extends { [key: string]: unknown }, A>(
  key: string,
  dispatch: (a: A) => void,
  getState: () => S
) {
  // Load initial state from localStorage
  const saved = localStorage.getItem(key);
  if (saved) {
    try {
      JSON.parse(saved);
    } catch {
      // Invalid JSON, ignore
    }
  }
  
  return (action: A) => {
    dispatch(action);
    
    // Save new state after dispatch
    const state = getState();
    localStorage.setItem(key, JSON.stringify(state));
  };
}

// Async middleware (like Redux Thunk)
function thunkMiddleware<S, A>(
  dispatch: (a: A) => void, 
  getState: () => S
) {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  return (action: any) => {
    // If action is a function, it's a thunk — call it with dispatch and getState
    if (typeof action === 'function') {
      return action(dispatch, getState);
    }
    // Otherwise, dispatch normally
    return dispatch(action);
  };
}

// Usage example
interface AppState {
  user: { name: string } | null;
  theme: 'light' | 'dark';
}

type AppAction = 
  | { type: 'setUser'; payload: { name: string } | null }
  | { type: 'setTheme'; payload: 'light' | 'dark' };

const appReducer = (state: AppState, action: AppAction): AppState => {
  switch (action.type) {
    case 'setUser':
      return { ...state, user: action.payload };
    case 'setTheme':
      return { ...state, theme: action.payload };
    default:
      return state;
  }
};

function AppWithMiddleware() {
  const [state, dispatch] = useReducerWithMiddleware(
    appReducer,
    { user: null, theme: 'light' as const },
    [thunkMiddleware, loggerMiddleware]
  );

  // Now we can dispatch thunks!
  const login = () => {
    dispatch(async (dispatch, getState) => {
      // This is a thunk — we can do async work here
      const response = await fetch('/api/login');
      const user = await response.json();
      dispatch({ type: 'setUser', payload: user });
    });
  };

  return <button onClick={login}>Login</button>;
}
```

### Step 4: Typed Action Creators with Discriminated Unions

TypeScript's discriminated unions provide exhaustiveness checking and excellent IDE support.

```typescript
// [File: src/reducers/typedActions.ts]
import { useReducer } from 'react';

/**
 * Step 4: Typed action creators with discriminated unions
 * 
 * Discriminated unions give us:
 * - Type inference for each action type
 * - Exhaustiveness checking (TypeScript errors if we miss a case)
 * - Great IDE autocomplete
 */

// Define action types as a discriminated union
type Action = 
  | { type: 'increment'; payload?: number }      // payload is optional here
  | { type: 'decrement'; payload?: number }
  | { type: 'set'; payload: number }
  | { type: 'reset' };

interface State {
  count: number;
}

// Reducer with full type safety
function reducer(state: State, action: Action): State {
  switch (action.type) {
