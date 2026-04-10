/**
 * Category: PRACTICAL
 * Subcategory: UI_DEVELOPMENT
 * Concept: React_Integration
 * Purpose: React hooks types for TypeScript
 * Difficulty: intermediate
 * UseCase: web
 */

/**
 * React Hooks Types - Comprehensive Guide
 * ======================================
 * 
 * 📚 WHAT: TypeScript types for React hooks
 * 💡 WHY: Provides type-safe hooks and state management
 * 🔧 HOW: Generics, event types, effect types
 */

// ============================================================================
// SECTION 1: USE STATE TYPES
// ============================================================================

// Example 1.1: useState with Types
// -----------------------

import { useState, useCallback, useMemo } from "react";

// Basic useState
const [state, setState] = useState<string>("initial");
const [count, setCount] = useState<number>(0);
const [user, setUser] = useState<{ name: string; email: string } | null>(null);

// With function initial state
const [memoized, setMemoized] = useState(() => {
  return expensiveComputation();
});

// Example 1.2: useState with Union Types
// ---------------------------------

type Status = "idle" | "loading" | "success" | "error";
const [status, setStatus] = useState<Status>("idle");

// ============================================================================
// SECTION 2: USE EFFECT TYPES
// ============================================================================

// Example 2.1: useEffect with Cleanup
// ---------------------------------

import { useEffect, useRef } from "react";

useEffect(() => {
  const subscription = someAPI.subscribe();
  
  return () => {
    subscription.unsubscribe();
  };
}, [dependency]);

// Example 2.2: useRef Types
// -----------------------

const inputRef = useRef<HTMLInputElement>(null);
const timerRef = useRef<NodeJS.Timeout | null>(null);

// ============================================================================
// SECTION 3: USE CALLBACK TYPES
// ============================================================================

// Example 3.1: useCallback with Types
// ---------------------------------

type ClickHandler = (event: React.MouseEvent<HTMLButtonElement>) => void;
type ChangeHandler = (event: React.ChangeEvent<HTMLInputElement>) => void;

const handleClick = useCallback<ClickHandler>((event) => {
  console.log(event.clientX);
}, []);

const handleChange = useCallback<ChangeHandler>((event) => {
  console.log(event.target.value);
}, []);

// ============================================================================
// SECTION 4: USE MEMO TYPES
// ============================================================================

// Example 4.1: useMemo for Expensive Calculations
// ---------------------------------

interface User {
  id: number;
  name: string;
}

const sortedUsers = useMemo(() => {
  return users
    .filter(user => user.name.length > 0)
    .sort((a, b) => a.name.localeCompare(b.name));
}, [users]);

// ============================================================================
// SECTION 5: CUSTOM HOOKS
// ============================================================================

// Example 5.1: Custom Hook with Types
// ---------------------------------

function useLocalStorage<T>(key: string, initialValue: T): [T, (value: T) => void] {
  const [storedValue, setStoredValue] = useState<T>(() => {
    try {
      const item = window.localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch {
      return initialValue;
    }
  });
  
  const setValue = (value: T) => {
    try {
      setStoredValue(value);
      window.localStorage.setItem(key, JSON.stringify(value));
    } catch (error) {
      console.error(error);
    }
  };
  
  return [storedValue, setValue];
}

function expensiveComputation(): string {
  return "computed";
}

const someAPI = {
  subscribe: () => ({ unsubscribe: () => {} }),
  getUsers: () => []
};

const users: User[] = [];

console.log("\n=== React Hooks Types Complete ===");
console.log("Next: PRACTICAL/UI_DEVELOPMENT/01_React_Integration/03_React_Context_Types.ts");