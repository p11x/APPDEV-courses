/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 08_Advanced_Type_Utilities
 * Topic: Type_Union_Operations
 * Purpose: Advanced type union patterns and operations
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(n) for union manipulation
 * Security: Compile-time only
 */

/**
 * Type Union Operations - Comprehensive Guide
 * ============================================
 * 
 * 📚 WHAT: Advanced type union patterns and operations
 * 💡 WHERE: State management, API responses, type filtering
 * 🔧 HOW: Union types, distributive conditional types, filtering
 */

// ============================================================================
// SECTION 1: WHAT - Type Unions
// ============================================================================

/**
 * WHAT are type unions?
 * - Multiple types combined with |
 * - Value can be any of the union members
 * - Common in state management and API responses
 * - Distributive over conditional types
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use type unions?
 * - Handle multiple states (loading, error, success)
 * - Model API response variations
 * - Create discriminated unions
 * - Filter and transform union members
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Basic Union Type
// --------------------------------

type StringOrNumber = string | number;
type Result = StringOrNumber extends string ? true : false; // true

// Example 3.2: Union to Tuple
// --------------------------------

type UnionToTuple<T> = 
  T extends any ? [T] : never;

type TupleFromUnion = UnionToTuple<"a" | "b" | "c">;
// ["a"] | ["b"] | ["c"]

// Example 3.3: Filter Union by Type
// ------------------------------------

type FilterUnion<T, U> = T extends U ? T : never;

type Filtered = FilterUnion<"a" | 1 | "b" | 2, string>;
// "a" | "b"

// Example 3.4: Exclude from Union
// --------------------------------

type ExcludeUnion<T, U> = 
  T extends U ? never : T;

type Excluded = ExcludeUnion<"a" | "b" | "c", "a">;
// "b" | "c"

// Example 3.5: Extract from Union
// --------------------------------

type ExtractUnion<T, U> = 
  T extends U ? T : never;

type Extracted = ExtractUnion<"a" | "b", "a">;
// "a"

// Example 3.6: Union to Intersection (Advanced)
// ----------------------------------------------

type UnionToIntersection<U> = 
  (U extends any ? (x: U) => void : never) extends (x: infer I) => void ? I : never;

type ResultUnion = string | number;
type IntersectionResult = UnionToIntersection<ResultUnion>;
// string & number

// Example 3.7: Distributive Union Operations
// -----------------------------------------

type MapUnion<T, F> = 
  T extends any ? F extends (x: T) => void ? ReturnType<F> : never : never;

// Example 3.8: Union Member Types
// --------------------------------

type UnionMember<T> = T extends T ? T : never;

type Member = UnionMember<"a" | "b" | "c">;
// "a" | "b" | "c"

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Union operations are O(n) for n members
 * - Distribution adds overhead for large unions
 * - Generally efficient for typical use cases
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - TypeScript 4.1+ for full support
 * - Works in all modern environments
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Compile-time only
 * - No runtime implications
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test with various union members
 * - Test distribution behavior
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over union type
 * - Break into filtered components
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Use built-in utility types
 * - Manual union handling
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Type Union Operations Complete ===");
console.log("Next: 08_Advanced_Type_Utilities/03_Deep_Readonly_Types.ts");
console.log("Previous: 01_Type_Intersection_Operations.ts");
console.log("Related: 02_Conditional_and_Mapped_Types/01_Conditional_Types.ts, 05_Type_Level_Programming/05_Type_IsEqual.ts");