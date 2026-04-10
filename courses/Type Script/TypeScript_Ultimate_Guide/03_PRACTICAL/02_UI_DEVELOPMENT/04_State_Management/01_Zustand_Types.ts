/**
 * Category: PRACTICAL
 * Subcategory: UI_DEVELOPMENT
 * Concept: State_Management
 * Purpose: Zustand state management types
 * Difficulty: intermediate
 * UseCase: web
 */

/**
 * Zustand Types - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: TypeScript types for Zustand state management
 * 💡 WHERE: Lightweight state management
 * 🔧 HOW: Store creation, actions, selectors
 */

// ============================================================================
// SECTION 1: ZUSTAND STORE TYPES
// ============================================================================

// Example 1.1: Basic Store
// -----------------------

import { create } from "zustand";

interface CounterState {
  count: number;
  increment: () => void;
  decrement: () => void;
  reset: () => void;
}

const useCounterStore = create<CounterState>((set) => ({
  count: 0,
  increment: () => set((state) => ({ count: state.count + 1 })),
  decrement: () => set((state) => ({ count: state.count - 1 })),
  reset: () => set({ count: 0 })
}));

// Usage
function Counter() {
  const count = useCounterStore((state) => state.count);
  const increment = useCounterStore((state) => state.increment);
  
  return (
    <div>
      <p>{count}</p>
      <button onClick={increment}>Increment</button>
    </div>
  );
}

// ============================================================================
// SECTION 2: STORE WITH SELECTORS
// ============================================================================

// Example 2.1: Selective Store Access
// ---------------------------------

interface UserState {
  user: { id: string; name: string } | null;
  isLoading: boolean;
  setUser: (user: { id: string; name: string }) => void;
  clearUser: () => void;
}

const useUserStore = create<UserState>((set) => ({
  user: null,
  isLoading: false,
  setUser: (user) => set({ user }),
  clearUser: () => set({ user: null })
}));

// Selectors
const selectUser = (state: UserState) => state.user;
const selectIsLoading = (state: UserState) => state.isLoading;
const selectName = (state: UserState) => state.user?.name;

console.log("\n=== Zustand Types Complete ===");
console.log("Next: PRACTICAL/UI_DEVELOPMENT/01_React_Integration/06_Next_JS_Types.ts");