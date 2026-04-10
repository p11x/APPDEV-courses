/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 03_Recursive_and_Distributive_Types
 * Topic: Type_Recursion_Limits
 * Purpose: Understanding and working with TypeScript recursion limits
 * Difficulty: expert
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: Limited by recursion depth (~50-100 levels)
 * Security: N/A - compile-time only
 */

/**
 * Type Recursion Limits - Comprehensive Guide
 * ============================================
 * 
 * 📚 WHAT: Understanding TypeScript's type recursion depth limits
 * 💡 WHERE: Deep type transformations, complex recursive types
 * 🔧 HOW: Working within and around recursion limits
 */

// ============================================================================
// SECTION 1: WHAT - Recursion Limits Overview
// ============================================================================

/**
 * WHAT are recursion limits?
 * - TypeScript limits how deeply recursive types can be evaluated
 * - Prevents infinite type expansion
 * - Default limit is around 50-100 levels of nesting
 * - Exceeding limit causes "Type instantiation is excessively deep" error
 */

// ============================================================================
// SECTION 2: WHY - Why Limits Exist
// ============================================================================

/**
 * WHY do limits exist?
 * - Prevents infinite type expansion from malformed types
 * - Protects against stack overflow during compilation
 * - Ensures reasonable compilation times
 * - Necessary for terminating recursive type definitions
 */

// ============================================================================
// SECTION 3: HOW - Working with Limits
// ============================================================================

// Example 3.1: Basic Recursive Type (May Hit Limit)
// --------------------------------------------------

// This deep type can hit the limit:
type DeepPick<T, Path extends string> = 
  Path extends `${infer K}.${infer R}`
    ? T extends Record<string, any>
      ? { [key in K]: DeepPick<T[key], R> }
      : never
    : Path extends keyof T
      ? Pick<T, Path>
      : never;

// Example 3.2: Truncated Type to Avoid Limits
// --------------------------------------------

type ShallowPick<T, K extends keyof T> = {
  [P in K]: T[P];
};

// Safer for deeply nested objects:
type SafePick<T, K extends keyof T> = K extends string 
  ? { [P in K]: T[P] }
  : never;

// Example 3.3: Iteration Count Limiting
// -------------------------------------

// Limit iterations with a counter:
type LimitDepth<T, Depth extends number = 0> = 
  Depth extends 10 ? T : T extends object 
    ? { [K in keyof T]: LimitDepth<T[K], Depth extends number ? [...[], Depth] extends [] ? 1 : [...[], Depth][0] extends infer D ? D : never : never> }
    : T;

// Simpler counter approach:
type Counter = [never, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

// Example 3.4: Flatten with Depth Limit
// -------------------------------------

type FlattenDepth<T, Depth extends number = 3> = 
  Depth extends 0 ? T 
  : T extends Array<infer U> 
    ? FlattenDepth<U, [-1] extends [Depth] ? 0 : [Depth] extends [infer D] ? D : 0>
    : T;

// Example 3.5: Safe Deep Readonly
// -------------------------------

type DeepReadonly<T, Depth extends number = 3> = 
  Depth extends 0 ? T : {
    readonly [K in keyof T]: T[K] extends object 
      ? DeepReadonly<T[K], [-1] extends [Depth] ? 0 : [Depth] extends [infer D] ? D : 0>
      : T[K];
  };

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Deep recursion significantly slows compilation
 * - Hitting limit causes compilation failure
 * - Use shallow types when possible
 * - Cache complex types to avoid re-evaluation
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - Limit behavior consistent across TypeScript versions
 * - Error messages may vary slightly
 * - Consider polyfilling with simpler types
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Limits prevent DoS via malicious types
 * - No runtime security concerns
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test with various depth levels
 * - Verify error messages when limit exceeded
 * - Create fallback types for edge cases
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Error message indicates "excessively deep"
 * - Reduce type complexity to fix
 * - Use intermediate types to simplify
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Use library utilities (ts-toolbelt, type-fest)
 * - Implement iterative solutions
 * - Use shallow types with explicit depth parameters
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Type Recursion Limits Complete ===");
console.log("Previous: 01_Recursive_Types.ts, 02_Distributive_Conditional_Types.ts, 03_Infer_Distributive.ts");
console.log("Related: 08_Advanced_Type_Utilities/03_Deep_Readonly_Types.ts, 04_Deep_Partial_Types.ts");