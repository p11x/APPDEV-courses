/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 02_Conditional_and_Mapped_Types
 * Topic: Mapped_Types
 * Purpose: Advanced mapped type patterns and transformations
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(n) where n is number of keys
 * Security: Type-level operations, compile-time only
 */

/**
 * Mapped Types - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Type transformations over object keys
 * 💡 WHERE: API transformations, type scaffolding, utilities
 * 🔧 HOW: Key remapping, modifiers, conditional mapping
 */

// ============================================================================
// SECTION 1: WHAT - Mapped Types Fundamentals
// ============================================================================

/**
 * WHAT are mapped types?
 * - Types that transform each property of a type simultaneously
 * - Support key remapping with 'as' clause
 * - Can add modifiers (+readonly, -readonly, +?, -?)
 * - Work with any key type that extends keyof any
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use mapped types?
 * - Create type utilities (Readonly, Partial, Required)
 * - Transform API responses to internal types
 * - Add prefixes/suffixes to property names
 * - Filter or exclude certain properties
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Basic Mapped Type
// --------------------------------

type MappedBasic<T> = {
  [K in keyof T]: T[K];
};

type Original = { name: string; age: number };
type Copied = MappedBasic<Original>;
// { name: string; age: number }

// Example 3.2: Key Remapping
// --------------------------------

type PrefixKeys<T, P extends string> = {
  [K in keyof T as `${P}${K & string}`]: T[K];
};

type Prefixed = PrefixKeys<{ a: number; b: string }, "get">;
// { geta: number; getb: string }

// Example 3.3: Filter Keys by Type
// --------------------------------

type FilterKeysByType<T, V> = {
  [K in keyof T as T[K] extends V ? K : never]: T[K];
};

type Filtered = FilterKeysByType<{ a: string; b: number; c: string }, string>;
// { a: string; c: string }

// Example 3.4: Modify Property Types
// --------------------------------

type MapToOptional<T> = {
  [K in keyof T]?: T[K];
};

type ToOptional = MapToOptional<{ a: number; b: string }>;
// { a?: number; b?: string }

// Example 3.5: Conditional Key Mapping
// --------------------------------

type MapToRequired<T> = {
  [K in keyof T]-?: T[K];
};

type ToRequired = MapToRequired<{ a?: number; b?: string }>;
// { a: number; b: string }

// Example 3.6: Numeric Key Manipulation
// --------------------------------

type MakeNumericOptional<T> = {
  [K in keyof T as K extends `${number}` ? K : never]?: T[K];
};

type NumericOptional = MakeNumericOptional<{ 0: string; 1: number; name: string }>;
// { 0?: string; 1?: number }

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Type inference is O(n) for n keys
 * - Complex remapping can slow compilation
 * - Prefer simple mapped types for better IDE support
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - TypeScript 4.1+ for key remapping
 * - Earlier versions support basic mapped types
 * - No runtime dependencies
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Compile-time only, no runtime overhead
 * - Type-safe transformations prevent data leaks
 * - No reflection at runtime
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Assert resulting types with type assertions
 * - Test with empty objects, optional props, unions
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over mapped type to see expansion
 * - Add intermediate type aliases
 * - Use keyof to debug key inference
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Manual type definitions
 * - Helper libraries (type-fest)
 * - Code generation
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Mapped Types Complete ===");
console.log("Next: 02_Conditional_and_Mapped_Types/03_Type_Transformation.ts");
console.log("Related: 01_Conditional_Types.ts, 02_Mapped_Types.ts");