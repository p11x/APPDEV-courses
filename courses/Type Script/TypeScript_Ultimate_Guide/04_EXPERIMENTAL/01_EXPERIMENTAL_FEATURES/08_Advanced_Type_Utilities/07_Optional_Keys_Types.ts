/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 08_Advanced_Type_Utilities
 * Topic: Optional_Keys_Types
 * Purpose: Extracting optional keys from types
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(n) for key counting
 * Security: Compile-time only
 */

/**
 * Optional Keys Types - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Extracting optional property keys from types
 * 💡 WHERE: Type transformations, default values, validation
 * 🔧 HOW: Mapped types with conditional type inference
 */

// ============================================================================
// SECTION 1: WHAT - Optional Keys
// ============================================================================

/**
 * WHAT are optional keys?
 * - Keys whose properties may be undefined
 * - Properties marked with ?
 * - Not required in type definition
 * - Opposite of required keys
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY extract optional keys?
 * - Default value handling
 * - Optional property validation
 * - Type transformation utilities
 * - Form field handling
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Basic Optional Keys
// --------------------------------

type OptionalKeys<T> = {
  [K in keyof T]: {} extends Pick<T, K> ? K : never;
}[keyof T];

type TestType = { a: string; b?: number; c?: string };
type Keys = OptionalKeys<TestType>;
// "b" | "c"

// Example 3.2: Filter Optional Keys
// --------------------------------

type FilterOptional<T> = {
  [K in OptionalKeys<T>]: T[K];
};

type Filtered = FilterOptional<{ a: string; b?: number; c?: boolean }>;
// { b?: number; c?: boolean }

// Example 3.3: Required Only Type
// --------------------------------

type RequiredOnly<T> = {
  [K in keyof T as {} extends Pick<T, K> ? never : K]: T[K];
};

type Required = RequiredOnly<{ a: string; b?: number; c?: boolean }>;
// { a: string }

// Example 3.4: Optional Only Type
// --------------------------------

type OptionalOnly<T> = {
  [K in keyof T as {} extends Pick<T, K> ? K : never]?: T[K];
};

type Optional = OptionalOnly<{ a: string; b?: number }>;
// { b?: number }

// Example 3.5: Nested Optional Keys
// --------------------------------

type NestedOptionalKeys<T> = T extends object 
  ? { [K in keyof T]-?: {} extends Pick<T, K> 
      ? K extends string 
        ? K | (T[K] extends object ? `${K}.${NestedOptionalKeys<T[K]>}` : never) 
        : never 
      : never 
    }[keyof T] 
  : never;

type DeepOptionalKeys = NestedOptionalKeys<{ a: { b: string; c?: number } }>;
// "c" | "a.c"

// Example 3.6: Make Optional Required
// --------------------------------

type MakeOptionalKeysRequired<T, K extends keyof T> = Omit<T, K> & Required<Pick<T, K>>;

type MadeRequired = MakeOptionalKeysRequired<{ a: string; b?: number }, "b">;
// { a: string; b: number }

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

console.log("\n=== Optional Keys Types Complete ===");
console.log("Previous: 06_Required_Keys_Types.ts");
console.log("Related: 04_Deep_Partial_Types.ts, 02_Conditional_and_Mapped_Types/02_Mapped_Types.ts, 05_Type_Level_Programming/05_Type_IsEqual.ts");