/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 04_Variadic_Tuple_Types
 * Topic: Spread_Operators
 * Purpose: Using spread operators in variadic tuple types
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 4.0+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(n) for tuple concatenation
 * Security: Compile-time only
 */

/**
 * Spread Operators in Tuples - Comprehensive Guide
 * =================================================
 * 
 * 📚 WHAT: Using spread operators within tuple types
 * 💡 WHERE: Function parameters, tuple concatenation, type inference
 * 🔧 HOW: ... spread syntax in type definitions
 */

// ============================================================================
// SECTION 1: WHAT - Spread Operators in Types
// ============================================================================

/**
 * WHAT are spread operators in tuple types?
 * - Using ... to unpack tuple types
 * - Concatenating multiple tuple types
 * - Spreading types into new tuple structures
 * - Type-level array operations
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use spread operators?
 * - Create flexible tuple manipulation utilities
 * - Build parameter lists dynamically
 * - Merge types for function composition
 * - Implement type-safe list operations
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Basic Tuple Concatenation
// --------------------------------------

type Concat<T extends any[], U extends any[]> = [...T, ...U];

type Combined = Concat<[string, number], [boolean]>;
// [string, number, boolean]

// Example 3.2: Prepend Element
// --------------------------------

type Prepend<T extends any[], E> = [E, ...T];

type Prepended = Prepend<[2, 3], 1>;
// [1, 2, 3]

// Example 3.3: Append Element
// --------------------------------

type Append<T extends any[], E> = [...T, E];

type Appended = Append<[1, 2], 3>;
// [1, 2, 3]

// Example 3.4: Spread in Function Parameters
// -------------------------------------------

type Params<T extends any[]> = T;

type MyParams = Params<[string, number]>;
// [string, number]

function typedFn<T extends any[]>(...args: T): T {
  return args;
}

// Example 3.5: Merge Tuple Types
// --------------------------------

type MergeTuples<T extends any[], U extends any[]> = {
  [K in keyof T | keyof U]: K extends keyof T ? T[K] : K extends keyof U ? U[K] : never;
};

// Example 3.6: Conditional Spread
// --------------------------------

type IfLengthLessThan5<T extends any[], R extends any[]> = 
  T["length"] extends 0 | 1 | 2 | 3 | 4 ? [...T, ...R] : T;

type Extended = IfLengthLessThan5<[1, 2], [3]>;
// [1, 2, 3]

type NotExtended = IfLengthLessThan5<[1, 2, 3, 4, 5, 6], [3]>;
// [1, 2, 3, 4, 5, 6]

// Example 3.7: Reverse with Spread
// ---------------------------------

type Reverse<T extends any[]> = T extends [...infer Rest, infer Last] 
  ? [Last, ...Reverse<Rest>] 
  : [];

type Reversed = Reverse<[1, 2, 3]>;
// [3, 2, 1]

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Tuple concatenation is O(n + m)
 * - Complex nested spreads can slow compilation
 * - Use simple operations when possible
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - TypeScript 4.0+ for full variadic tuple support
 * - Works in earlier versions with limitations
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Compile-time only operations
 * - No runtime security implications
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test with empty tuples, single elements, many elements
 * - Verify type inference in function parameters
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over types to see expanded versions
 * - Use length property to debug tuple size
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Array types instead of tuples
 * - Manual type construction
 * - Helper libraries
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Spread Operators Complete ===");
console.log("Next: 04_Variadic_Tuple_Types/03_Tuple_Inference.ts");
console.log("Related: 01_Variadic_Tuple_Types.ts, 04_Head_Tail_Types.ts, 05_Tuple_Patterns.ts");