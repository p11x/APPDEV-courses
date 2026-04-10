/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 03_Recursive_and_Distributive_Types
 * Topic: Infer_Distributive
 * Purpose: Using infer with distributive conditional types
 * Difficulty: expert
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(n) for union distribution
 * Security: Compile-time type inference
 */

/**
 * Infer with Distributive Types - Comprehensive Guide
 * ===================================================
 * 
 * 📚 WHAT: Combining infer keyword with distributive conditional types
 * 💡 WHERE: Complex type inference, parameter extraction, return types
 * 🔧 HOW: Using infer within distributive conditional type patterns
 */

// ============================================================================
// SECTION 1: WHAT - Infer and Distributivity
// ============================================================================

/**
 * WHAT is infer with distributive types?
 * - Using 'infer' within conditional types that distribute over unions
 * - Extracting types from each union member separately
 * - Useful for tuple/array type manipulation
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use infer with distribution?
 * - Extract first/last elements from tuples
 * - Unwrap nested types in unions
 * - Create type-safe utility functions
 * - Handle heterogeneous tuple types
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Extract First Element (Distributive)
// -------------------------------------------------

type First<T extends any[]> = T extends [infer F, ...any[]] ? F : never;

type FirstString = First<[string, number, boolean]>;  // string
type FirstNumber = First<[number, string]>;           // number
type FirstNever = First<[]>;                          // never

// Example 3.2: Extract Last Element
// ---------------------------------

type Last<T extends any[]> = T extends [...any[], infer L] ? L : never;

type LastBool = First<[string, number, boolean]>;  // boolean
type LastString = First<[string, number]>;        // number

// Example 3.3: Extract Without First
// ---------------------------------

type Tail<T extends any[]> = T extends [any, ...infer R] ? R : never;

type TailResult = Tail<[string, number, boolean]>;
// [number, boolean]

// Example 3.4: Distributive Parameter Extraction
// ----------------------------------------------

type Parameters<T> = T extends (...args: infer P) => any ? P : never;

type Params = Parameters<(a: string, b: number) => void>;
// [string, number]

// Example 3.5: Union to Tuple (Basic)
// ------------------------------------

type UnionToIntersection<U> = 
  (U extends any ? (x: U) => void : never) extends (x: infer I) => void ? I : never;

type Intersected = UnionToIntersection<string | number>;
// string & number

// Example 3.6: Distributive Return Type
// -------------------------------------

type ReturnTypes<T> = T extends (...args: any[]) => infer R ? R : never;

type Fn1 = () => string;
type Fn2 = () => number;

type Returns = ReturnTypes<Fn1 | Fn2>;
// string | number

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Distribution adds O(n) for n union members
 * - Complex inference can slow compilation
 * - Use specific types when possible
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - TypeScript 4.1+ for full infer support
 * - Works with older versions for basic inference
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Compile-time only, no runtime impact
 * - Type-safe by design
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test with various tuple lengths
 * - Verify never for empty tuples
 * - Test with different union members
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover to see inferred types
 * - Test with simple types first
 * - Add intermediate type aliases
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Manual type functions
 * - Helper libraries like ts-toolbelt
 * - TypeScript built-in utilities
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Infer Distributive Complete ===");
console.log("Next: 03_Recursive_and_Distributive_Types/04_Type_Recursion_Limits.ts");
console.log("Related: 01_Recursive_Types.ts, 02_Distributive_Conditional_Types.ts, 04_Variadic_Tuple_Types/04_Head_Tail_Types.ts");