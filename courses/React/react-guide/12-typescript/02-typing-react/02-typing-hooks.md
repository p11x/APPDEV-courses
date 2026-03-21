# Typing Hooks

## Overview
React hooks are the backbone of modern React applications, and properly typing them is essential for type safety. Each hook has its own typing patterns: useState needs explicit types for complex objects, useReducer requires generic type parameters for state and actions, useRef needs to know what element it references, and useCallback/useMemo benefit from explicit return types in complex scenarios. This guide covers typing all the core React hooks with practical examples.

## Prerequisites
- Understanding of React hooks (useState, useEffect, useRef, useReducer, useCallback, useMemo)
- Basic TypeScript knowledge
- Familiarity with React component structure

## Core Concepts

### Typing useState
The useState hook is used for managing component state. TypeScript can often infer the type from the initial value, but explicit typing is sometimes needed:

```typescript
// [File: src/hooks/typingUseState.tsx]
import React from 'react';

// ======== Simple Types - TypeScript Infers ========
// Initial value provides the type - no explicit annotation needed
function SimpleCounter() {
  // TypeScript infers: number
  const [count, setCount] = React.useState(0);
  
  // TypeScript infers: string
  const [name, setName] = React.useState('Alice');
  
  // TypeScript infers: boolean
  const [isActive, setIsActive] = React.useState(true);
  
  return (
    <div>
      <p>{count}</p>
      <p>{name}</p>
      <button onClick={() => setCount(c => c + 1)}>Increment</button>
    </div>
  );
}

// ======== Complex Types - Explicit Annotation Needed ========

// Object state
interface User {
  id: number;
  name: string;
  email: string;
}

function UserProfile() {
  // Explicit type needed for complex objects
  const [user, setUser] = React.useState<User | null>(null);
  // Now TypeScript knows user can be null
  
  return (
    <div>
      {user ? (
        <p>{user.name}</p>
      ) : (
        <p>No user</p>
      )}
    </div>
  );
}

// Array state with complex objects
function UserList() {
  // Type: User[] - inferred from initial value
  const [users, setUsers] = React.useState<User[]>([]);
  
  // Add user - fully typed
  const addUser = (name: string, email: string) => {
    const newUser: User = {
      id: Date.now(),
      name,
      email,
    };
    setUsers(prev => [...prev, newUser]);
  };
  
  return <div>{/* JSX */}</div>;
}

// ======== Union Types ========

type Status = 'idle' | 'loading' | 'success' | 'error';

function DataFetcher() {
  // Union type - can only be one of these values
  const [status, setStatus] = React.useState<Status>('idle');
  
  // TypeScript narrows based on value
  const getStatusMessage = () => {
    switch (status) {
      case 'idle': return 'Ready to fetch';
      case 'loading': return 'Loading...';
      case 'success': return 'Data loaded!';
      case 'error': return 'Failed to load';
    }
  };
  
  return <div>{getStatusMessage()}</div>;
}

// ======== Functional State Updates ========

function ComplexState() {
  // When state is computed from previous, use function form
  const [todos, setTodos] = React.useState<string[]>([]);
  
  const addTodo = (todo: string) => {
    // Use callback to get previous state
    setTodos(prev => [...prev, todo]);
  };
  
  const removeTodo = (index: number) => {
    setTodos(prev => prev.filter((_, i) => i !== index));
  };
  
  return { todos, addTodo, removeTodo };
}
```

### Typing useRef
The useRef hook is used for accessing DOM elements or storing mutable values that don't trigger re-renders. Properly typing useRef is crucial:

```typescript
// [File: src/hooks/typingUseRef.tsx]
import React from 'react');

// ======== DOM Element Refs ========

function InputFocus() {
  // Typing the ref to a specific HTML element
  // Use HTMLInputElement | null because ref might not be attached yet
  const inputRef = React.useRef<HTMLInputElement | null>(null);
  
  const handleFocus = () => {
    // inputRef.current is typed as HTMLInputElement | null
    inputRef.current?.focus();
  };
  
  return (
    <div>
      <input ref={inputRef} type="text" />
      <button onClick={handleFocus}>Focus Input</button>
    </div>
  );
}

// Multiple refs
function MultiRef() {
  const nameRef = React.useRef<HTMLInputElement | null>(null);
  const emailRef = React.useRef<HTMLInputElement | null>(null);
  
  const handleSubmit = () => {
    // Access values through refs - no state needed!
    const name = nameRef.current?.value;
    const email = emailRef.current?.value;
    console.log({ name, email });
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <input ref={nameRef} placeholder="Name" />
      <input ref={emailRef} placeholder="Email" />
      <button type="submit">Submit</button>
    </form>
  );
}

// ======== Mutable Value Refs ========

function Timer() {
  // useRef can store any value that persists across renders
  // Use generic to specify the value type
  const intervalRef = React.useRef<number | undefined>(undefined);
  const [count, setCount] = React.useState(0);
  
  React.useEffect(() => {
    // Store interval ID in ref (doesn't trigger re-render)
    intervalRef.current = window.setInterval(() => {
      setCount(c => c + 1);
    }, 1000);
    
    // Cleanup on unmount
    return () => {
      if (intervalRef.current) {
        clearInterval(intervalRef.current);
      }
    };
  }, []);
  
  return <div>Count: {count}</div>;
}

// ======== Ref with Initial Value ========

function RefWithInitial() {
  // Provide initial value - ref.current starts with this value
  const countRef = React.useRef(0);
  
  // Type is inferred from initial value: number
  const increment = () => {
    countRef.current += 1;
    console.log('Count:', countRef.current);
  };
  
  return <button onClick={increment}>Increment (see console)</button>;
}
```

### Typing useReducer
The useReducer hook is powerful for complex state logic. It needs explicit types for state and actions:

```typescript
// [File: src/hooks/typingUseReducer.tsx]
import React from 'react');

// ======== Define State and Action Types ========

// State shape
interface CounterState {
  count: number;
  history: number[];
}

// Action types - discriminated union
type CounterAction =
  | { type: 'increment' }
  | { type: 'decrement' }
  | { type: 'reset' }
  | { type: 'set'; value: number }
  | { type: 'undo' };

// Initial state
const initialState: CounterState = {
  count: 0,
  history: [0],
};

// Reducer function - fully typed
function counterReducer(
  state: CounterState, 
  action: CounterAction
): CounterState {
  switch (action.type) {
    case 'increment':
      return {
        count: state.count + 1,
        history: [...state.history, state.count + 1],
      };
      
    case 'decrement':
      return {
        count: state.count - 1,
        history: [...state.history, state.count - 1],
      };
      
    case 'reset':
      return initialState;
      
    case 'set':
      return {
        count: action.value,
        history: [...state.history, action.value],
      };
      
    case 'undo':
      if (state.history.length <= 1) return state;
      const newHistory = [...state.history];
      newHistory.pop(); // Remove current
      return {
        count: newHistory[newHistory.length - 1],
        history: newHistory,
      };
      
    default:
      // Exhaustiveness check - ensures all cases handled
      const _exhaustive: never = action;
      return state;
  }
}

// ======== Using the Reducer ========

function Counter() {
  // Generic: useReducer<State, Action>
  const [state, dispatch] = React.useReducer(counterReducer, initialState);
  
  return (
    <div>
      <p>Count: {state.count}</p>
      <p>History: {state.history.join(', ')}</p>
      <button onClick={() => dispatch({ type: 'increment' })}>+</button>
      <button onClick={() => dispatch({ type: 'decrement' })}>-</button>
      <button onClick={() => dispatch({ type: 'reset' })}>Reset</button>
      <button onClick={() => dispatch({ type: 'set', value: 10 })}>Set to 10</button>
      <button onClick={() => dispatch({ type: 'undo' })}>Undo</button>
    </div>
  );
}

// ======== Complex Form Reducer Example ========

interface FormState {
  values: Record<string, string>;
  errors: Record<string, string>;
  touched: Record<string, boolean>;
  isSubmitting: boolean;
}

type FormAction =
  | { type: 'setValue'; field: string; value: string }
  | { type: 'setError'; field: string; error: string }
  | { type: 'setTouched'; field: string }
  | { type: 'setSubmitting'; isSubmitting: boolean }
  | { type: 'reset' };

function formReducer(state: FormState, action: FormAction): FormState {
  switch (action.type) {
    case 'setValue':
      return {
        ...state,
        values: { ...state.values, [action.field]: action.value },
        // Clear error when user types
        errors: { ...state.errors, [action.field]: '' },
      };
      
    case 'setError':
      return {
        ...state,
        errors: { ...state.errors, [action.field]: action.error },
      };
      
    case 'setTouched':
      return {
        ...state,
        touched: { ...state.touched, [action.field]: true },
      };
      
    case 'setSubmitting':
      return { ...state, isSubmitting: action.isSubmitting };
      
    case 'reset':
      return {
        values: {},
        errors: {},
        touched: {},
        isSubmitting: false,
      };
      
    default:
      return state;
  }
}

function FormWithReducer() {
  const [state, dispatch] = React.useReducer(formReducer, {
    values: {},
    errors: {},
    touched: {},
    isSubmitting: false,
  });
  
  const handleChange = (field: string, value: string) => {
    dispatch({ type: 'setValue', field, value });
  };
  
  const handleBlur = (field: string) => {
    dispatch({ type: 'setTouched', field });
  };
  
  return <div>{/* Form JSX */}</div>;
}
```

### Typing useCallback and useMemo
These hooks cache functions and values. While TypeScript often infers types, explicit typing helps in complex scenarios:

```typescript
// [File: src/hooks/typingUseCallbackMemo.tsx]
import React from 'react');

// ======== useCallback ========

function CallbackExample({ users }: { users: string[] }) {
  // TypeScript usually infers the callback type
  const handleClick = React.useCallback((id: number) => {
    console.log('Clicked:', id);
  }, []); // Empty deps - function never changes
  
  // Explicit type for complex callbacks
  const processUsers = React.useCallback((
    filter: string,
    sortBy: 'asc' | 'desc'
  ): string[] => {
    return users
      .filter(u => u.includes(filter))
      .sort((a, b) => 
        sortBy === 'asc' 
          ? a.localeCompare(b) 
          : b.localeCompare(a)
      );
  }, [users]); // Changes when users changes
  
  return (
    <div>
      {processUsers('a', 'asc').map(user => (
        <button key={user} onClick={() => handleClick(1)}>
          {user}
        </button>
      ))}
    </div>
  );
}

// ======== useMemo ========

function MemoExample({ items }: { items: number[] }) {
  // TypeScript infers: number
  const sum = React.useMemo(() => {
    return items.reduce((acc, val) => acc + val, 0);
  }, [items]);
  
  // Explicit type for complex returns
  const groupedItems = React.useMemo((): Record<string, number[]> => {
    return items.reduce((acc, val) => {
      const key = val > 10 ? 'high' : 'low';
      return {
        ...acc,
        [key]: [...(acc[key] || []), val],
      };
    }, {} as Record<string, number[]>);
  }, [items]);
  
  return (
    <div>
      <p>Sum: {sum}</p>
      <pre>{JSON.stringify(groupedItems, null, 2)}</pre>
    </div>
  );
}

// ======== Expensive Computations ========

interface Product {
  id: string;
  name: string;
  price: number;
  category: string;
}

function ProductList({ 
  products, 
  filter, 
  sortBy 
}: { 
  products: Product[];
  filter: string;
  sortBy: 'price-asc' | 'price-desc' | 'name';
}) {
  // Memoized sorted/filtered list
  const processedProducts = React.useMemo(() => {
    // 1. Filter
    let result = products.filter(p => 
      p.name.toLowerCase().includes(filter.toLowerCase()) ||
      p.category.toLowerCase().includes(filter.toLowerCase())
    );
    
    // 2. Sort
    switch (sortBy) {
      case 'price-asc':
        result = [...result].sort((a, b) => a.price - b.price);
        break;
      case 'price-desc':
        result = [...result].sort((a, b) => b.price - a.price);
        break;
      case 'name':
        result = [...result].sort((a, b) => a.name.localeCompare(b.name));
        break;
    }
    
    return result;
  }, [products, filter, sortBy]);
  
  return (
    <ul>
      {processedProducts.map(p => (
        <li key={p.id}>{p.name} - ${p.price}</li>
      ))}
    </ul>
  );
}
```

## Common Mistakes

### Mistake 1: Not Typing useRef Correctly
```typescript
// ❌ WRONG - Missing null type
const inputRef = React.useRef<HTMLInputElement>(null);
// Error: Type 'HTMLInputElement | null' is not assignable...

// ✅ CORRECT - Include null in the type
const inputRef = React.useRef<HTMLInputElement | null>(null);
// Or use undefined if you don't initialize it
const inputRef = React.useRef<HTMLInputElement | undefined>(undefined);
```

### Mistake 2: Missing Dependency Array
```typescript
// ❌ WRONG - Missing deps, stale closure
const handleClick = React.useCallback(() => {
  console.log(count); // count will always be 0!
}, []); // Missing count!

// ✅ CORRECT - Include all dependencies
const handleClick = React.useCallback(() => {
  console.log(count);
}, [count]);
```

### Mistake 3: Not Using useRef for Values That Don't Need Re-renders
```typescript
// ❌ WRONG - Using useState for values that change frequently but don't need re-render
const [intervalId, setIntervalId] = React.useState<number>();
// This would cause unnecessary re-renders!

// ✅ CORRECT - Use useRef for mutable values that don't need re-render
const intervalId = React.useRef<number>();
// Changes to ref.current don't trigger re-render
```

## Real-World Example

Here's a complete custom hook with full typing:

```typescript
// [File: src/hooks/useLocalStorage.ts]
import { useState, useEffect, useCallback } from 'react';

// Generic hook for persisting state in localStorage
function useLocalStorage<T>(
  key: string,
  initialValue: T
): [T, (value: T | ((val: T) => T)) => void, () => void] {
  // State to store our value
  // Pass initial state function to execute only once
  const [storedValue, setStoredValue] = useState<T>(() => {
    if (typeof window === 'undefined') {
      return initialValue;
    }
    try {
      // Get from localStorage by key
      const item = window.localStorage.getItem(key);
      // Parse stored json or if none return initialValue
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      // If error also return initialValue
      console.warn(`Error reading localStorage key "${key}":`, error);
      return initialValue;
    }
  });

  // Return a wrapped version of useState's setter function
  const setValue = useCallback(
    (value: T | ((val: T) => T)) => {
      try {
        // Allow value to be a function so we have same API as useState
        const valueToStore =
          value instanceof Function ? value(storedValue) : value;
        
        // Save state
        setStoredValue(valueToStore);
        
        // Save to localStorage
        if (typeof window !== 'undefined') {
          window.localStorage.setItem(key, JSON.stringify(valueToStore));
        }
      } catch (error) {
        console.warn(`Error setting localStorage key "${key}":`, error);
      }
    },
    [key, storedValue]
  );

  // Remove from localStorage
  const removeValue = useCallback(() => {
    try {
      if (typeof window !== 'undefined') {
        window.localStorage.removeItem(key);
      }
      setStoredValue(initialValue);
    } catch (error) {
      console.warn(`Error removing localStorage key "${key}":`, error);
    }
  }, [key, initialValue]);

  // Listen for changes to this key in other tabs/windows
  useEffect(() => {
    if (typeof window === 'undefined') return;
    
    const handleStorageChange = (e: StorageEvent) => {
      if (e.key === key && e.newValue) {
        setStoredValue(JSON.parse(e.newValue));
      }
    };
    
    window.addEventListener('storage', handleStorageChange);
    return () => window.removeEventListener('storage', handleStorageChange);
  }, [key]);

  return [storedValue, setValue, removeValue];
}

// ======== Usage Examples ========

// Example 1: Simple string
function ThemeToggle() {
  const [theme, setTheme] = useLocalStorage('theme', 'light');
  
  return (
    <button onClick={() => setTheme(t => t === 'light' ? 'dark' : 'light')}>
      Current theme: {theme}
    </button>
  );
}

// Example 2: Complex object
interface UserPreferences {
  fontSize: number;
  colorScheme: 'light' | 'dark';
  notifications: boolean;
}

function PreferencesPanel() {
  const [prefs, setPrefs] = useLocalStorage<UserPreferences>(
    'user-preferences',
    {
      fontSize: 16,
      colorScheme: 'light',
      notifications: true,
    }
  );
  
  return (
    <div>
      <label>
        Font Size:
        <input
          type="number"
          value={prefs.fontSize}
          onChange={(e) => setPrefs(p => ({ 
            ...p, 
            fontSize: Number(e.target.value) 
          }))}
        />
      </label>
    </div>
  );
}

export default useLocalStorage;
```

## Key Takeaways
- useState: TypeScript infers from initial value; explicit type needed for null, unions, or complex objects
- useRef: Always use `Ref<T> | null` or `Ref<T> | undefined` for DOM refs; use generic for mutable values
- useReducer: Define state interface and action union; use discriminated unions for type narrowing
- useCallback: TypeScript infers most times; explicit type helpful for complex callback signatures
- useMemo: Explicit return type needed for complex computed values
- Always include correct dependency arrays to avoid stale closures
- useRef is better than useState for values that change frequently but don't need re-renders

## What's Next
Continue to [Typing Events and Handlers](03-typing-events-and-handlers.md) to learn how to properly type React event handlers and form events.