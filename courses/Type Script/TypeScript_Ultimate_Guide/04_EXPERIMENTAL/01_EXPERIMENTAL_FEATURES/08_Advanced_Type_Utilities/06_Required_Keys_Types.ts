/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 08_Advanced_Type_Utilities
 * Topic: Required_Keys_Types
 * Purpose: Extracting required keys from types
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(n) for key counting
 * Security: Compile-time only
 */

/**
 * Required Keys Types - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Extracting required property keys from types
 * 💡 WHERE: Type transformations, form validation, API handling
 * 🔧 HOW: Mapped types with conditional type inference
 */

// ============================================================================
// SECTION 1: WHAT - Required Keys
// ============================================================================

/**
 * WHAT are required keys?
 * - Keys whose properties are not optional
 * - Non-nullable properties in a type
 * - Keys that must be present
 * - Opposite of optional keys
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY extract required keys?
 * - Form validation requirements
 * - API request body validation
 * - Type transformation utilities
 * - Property group manipulation
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Basic Required Keys
// --------------------------------

type RequiredKeys<T> = {
  [K in keyof T]-?: {} extends Pick<T, K> ? never : K;
}[keyof T];

type TestType = { a: string; b?: number; c: string };
type Keys = RequiredKeys<TestType>;
// "a" | "c"

// Example 3.2: Filter Required Keys
// --------------------------------

type FilterRequired<T> = {
  [K in RequiredKeys<T>]: T[K];
};

type Filtered = FilterRequired<{ a: string; b?: number; c: boolean }>;
// { a: string; c: boolean }

// Example 3.3: Required Keys Union
// --------------------------------

type AllKeys = keyof { a: string; b?: number };
// "a" | "b"

type OptionalKeys<T> = {
  [K in keyof T]: {} extends Pick<T, K> ? K : never;
}[keyof T];

type Optional = OptionalKeys<{ a: string; b?: number }>;
// "b"

// Example 3.4: Nested Required Keys
// --------------------------------

type NestedRequiredKeys<T> = T extends object 
  ? { [K in keyof T]-?: {} extends Pick<T, K> 
      ? never 
      : K extends string 
        ? K | (T[K] extends object ? `${K}.${NestedRequiredKeys<T[K]>}` : never) 
        : never 
    }[keyof T] 
  : never;

type DeepKeys = NestedRequiredKeys<{ a: { b: string; c?: number } }>;
// "a" | "a.b"

// Example 3.5: Required Keys Count
// --------------------------------

type RequiredKeysCount<T> = RequiredKeys<T> extends never 
  ? 0 
  : RequiredKeys<T> extends string 
    ? 1 
    : RequiredKeys<T> extends `${string},${string}` 
      ? RequiredKeys<T> extends `${string},${string},${string}` 
        ? 3 
        : 2 
      : 1;

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - O(n) for key iteration
 * - Nested version may hit recursion limits
 * - Efficient for typical types
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
 * - Test with various optional/required properties
 * - Test with empty types
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over type to see key union
 * - Test with intermediate types
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Manual key enumeration
 * - Use built-in utility types
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Required Keys Types Complete ===");
console.log("Next: 08_Advanced_Type_Utilities/07_Optional_Keys_Types.ts");
console.log("Previous: 05_Nested_Property_Types.ts");
console.log("Related: 04_Deep_Partial_Types.ts, 05_Type_Level_Programming/05_Type_IsEqual.ts");