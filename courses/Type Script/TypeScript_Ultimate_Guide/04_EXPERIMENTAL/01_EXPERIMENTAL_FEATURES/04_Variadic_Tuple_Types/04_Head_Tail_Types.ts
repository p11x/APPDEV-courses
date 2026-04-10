/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 04_Variadic_Tuple_Types
 * Topic: Head_Tail_Types
 * Purpose: Implementing head and tail operations on tuple types
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 4.0+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(1) for basic operations
 * Security: Compile-time only
 */

/**
 * Head and Tail Types - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Extracting first and remaining elements from tuples
 * 💡 WHERE: Tuple manipulation, functional patterns, type utilities
 * 🔧 HOW: Using infer with rest patterns
 */

// ============================================================================
// SECTION 1: WHAT - Head and Tail Operations
// ============================================================================

/**
 * WHAT are head and tail?
 * - Head: First element of a tuple
 * - Tail: All elements except the first
 * - Fundamental operations for tuple manipulation
 * - Foundation for recursive tuple processing
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use head and tail?
 * - Implement recursive type transformations
 * - Build functional programming utilities
 * - Create type-safe list operations
 * - Enable tuple decomposition
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Basic Head Type
// --------------------------------

type Head<T extends any[]> = T extends [infer H, ...any[]] ? H : never;

type FirstString = Head<[string, number, boolean]>;  // string
type FirstNumber = Head<[number, string]>;           // number
type FirstNever = Head<[]>;                          // never

// Example 3.2: Basic Tail Type
// --------------------------------

type Tail<T extends any[]> = T extends [any, ...infer R] ? R : never;

type RestNumbers = Tail<[string, number, boolean]>;  // [number, boolean]
type RestString = Tail<[string, number]>;            // [number]
type RestEmpty = Tail<[string]>;                     // []

// Example 3.3: Last Element
// --------------------------------

type Last<T extends any[]> = T extends [...any[], infer L] ? L : never;

type LastBool = Last<[string, number, boolean]>;  // boolean
type LastString = Last<[string, number]>;         // number

// Example 3.4: Init (All Except Last)
// ------------------------------------

type Init<T extends any[]> = T extends [...infer I, any] ? I : never;

type Initial = Init<[string, number, boolean]>;
// [string, number]

// Example 3.5: Element at Index
// --------------------------------

type ElementAt<T extends any[], I extends number> = 
  I extends keyof T ? T[I] : never;

type AtIndex0 = ElementAt<[string, number], 0>;  // string
type AtIndex1 = ElementAt<[string, number], 1>;  // number

// Example 3.6: Drop First N Elements
// ---------------------------------

type DropFirst<T extends any[], N extends number = 1> = 
  T extends [any, ...infer R] 
    ? N extends 1 
      ? R 
      : DropFirst<R, [-1] extends [N] ? 0 : N>
    : [];

type Dropped = DropFirst<[1, 2, 3, 4], 2>;
// [3, 4]

// Example 3.7: Drop Last N Elements
// ---------------------------------

type DropLast<T extends any[], N extends number = 1> = 
  T extends [...infer R, any] 
    ? N extends 1 
      ? R 
      : DropLast<R, [-1] extends [N] ? 0 : N>
    : [];

type DroppedLast = DropLast<[1, 2, 3, 4], 2>;
// [1, 2]

// Example 3.8: Tuple Length
// --------------------------------

type Length<T extends any[]> = T["length"];

type Len3 = Length<[string, number, boolean]>;  // 3
type Len0 = Length<[]>;                        // 0

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Head/Tail operations are O(1) type checking
 * - Recursive operations are O(n)
 * - Generally fast for typical tuple sizes
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - TypeScript 4.0+ for full support
 * - Works in earlier versions with limitations
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Compile-time only operations
 * - No runtime implications
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test with empty tuples
 * - Test with single element tuples
 * - Test with various element types
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over type to see expansion
 * - Test with simple tuples first
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Use built-in utility types
 * - Manual array access
 * - Library utilities
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Head Tail Types Complete ===");
console.log("Next: 04_Variadic_Tuple_Types/05_Tuple_Patterns.ts");
console.log("Previous: 01_Variadic_Tuple_Types.ts, 02_Spread_Operators.ts, 03_Tuple_Inference.ts");
console.log("Related: 03_Recursive_and_Distributive_Types/03_Infer_Distributive.ts");