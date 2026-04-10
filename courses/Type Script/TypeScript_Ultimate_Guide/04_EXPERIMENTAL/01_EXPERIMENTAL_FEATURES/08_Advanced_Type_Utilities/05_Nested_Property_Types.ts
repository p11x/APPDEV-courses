/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 08_Advanced_Type_Utilities
 * Topic: Nested_Property_Types
 * Purpose: Accessing and manipulating nested property types
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(d) where d is depth
 * Security: Compile-time only
 */

/**
 * Nested Property Types - Comprehensive Guide
 * ============================================
 * 
 * 📚 WHAT: Accessing and manipulating nested property types
 * 💡 WHERE: Deep property access, type-safe paths, form types
 * 🔧 HOW: Template literal types, recursive types, key manipulation
 */

// ============================================================================
// SECTION 1: WHAT - Nested Property Types
// ============================================================================

/**
 * WHAT are nested property types?
 * - Types representing paths to nested properties
 * - Dot-notation string paths as types
 * - Extracting types from deep paths
 * - Type-safe property access
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use nested property types?
 * - Type-safe deep property access
 * - Form validation with paths
 * - Configuration path types
 * - API update operations
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Get Nested Property Type
// ------------------------------------

type NestedKeyOf<T> = T extends object 
  ? { [K in keyof T]: K extends string 
      ? T[K] extends object 
        ? K | `${K}.${NestedKeyOf<T[K]>}` 
        : K 
      : never 
    }[keyof T] 
  : never;

type Keys = NestedKeyOf<{ a: { b: { c: string } } }>;
// "a" | "a.b" | "a.b.c"

// Example 3.2: Get Type at Path
// --------------------------------

type GetTypeAtPath<T, P extends string> = 
  P extends `${infer K}.${infer R}` 
    ? K extends keyof T 
      ? GetTypeAtPath<T[K], R> 
      : never 
    : P extends keyof T 
      ? T[P] 
      : never;

type DeepType = GetTypeAtPath<{ a: { b: { c: string } } }, "a.b.c">;
// string

// Example 3.3: Set Type at Path
// --------------------------------

type SetTypeAtPath<T, P extends string, V> = 
  P extends `${infer K}.${infer R}` 
    ? K extends keyof T 
      ? { [k in keyof T]: k extends K ? SetTypeAtPath<T[K], R, V> : T[k] }
      : T
    : P extends keyof T 
      ? { [k in keyof T]: k extends P ? V : T[k] }
      : T;

// Example 3.4: Deep Pick
// --------------------------------

type DeepPick<T, P extends string> = 
  P extends `${infer K}.${infer R}` 
    ? K extends keyof T 
      ? { [key in K]: DeepPick<T[K], R> }
      : never 
    : P extends keyof T 
      ? Pick<T, P> 
      : never;

type Picked = DeepPick<{ a: { b: string; c: number } }, "a.b">;
// { a: { b: string } }

// Example 3.5: Deep Omit
// --------------------------------

type DeepOmit<T, P extends string> = 
  P extends `${infer K}.${infer R}` 
    ? K extends keyof T 
      ? { [key in keyof T as key extends K ? never : key]: DeepOmit<T[key], R> }
      : T
    : Omit<T, P>;

// Example 3.6: Path Validation
// --------------------------------

type ValidPath<T, P extends string> = 
  GetTypeAtPath<T, P> extends never ? false : true;

type IsValid = ValidPath<{ a: { b: string } }, "a.b">; // true
type IsInvalid = ValidPath<{ a: { b: string } }, "a.c">; // false

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Path operations are O(d) for depth
 * - Complex paths slow compilation
 * - May hit recursion limits with deep paths
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - TypeScript 4.1+ for template literal paths
 * - Works in all modern environments
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Compile-time only
 * - No runtime security implications
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test with various path depths
 * - Test invalid path handling
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over path type to see expansion
 * - Test GetTypeAtPath for specific paths
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Use lodash/get/set patterns
 * - Manual type definitions
 * - Library: ts-toolbelt
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Nested Property Types Complete ===");
console.log("Next: 08_Advanced_Type_Utilities/06_Required_Keys_Types.ts");
console.log("Previous: 03_Deep_Readonly_Types.ts, 04_Deep_Partial_Types.ts");
console.log("Related: 01_Template_Literal_Types/03_Pattern_Matching_Types.ts, 04_Advanced_Template_Literals/01_Regex_Template_Types.ts");