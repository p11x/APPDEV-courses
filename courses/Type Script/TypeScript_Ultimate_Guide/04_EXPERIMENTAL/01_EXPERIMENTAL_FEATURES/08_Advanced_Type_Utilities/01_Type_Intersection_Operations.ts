/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 08_Advanced_Type_Utilities
 * Topic: Type_Intersection_Operations
 * Purpose: Advanced type intersection patterns and operations
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: O(n) for key operations
 * Security: Compile-time only
 */

/**
 * Type Intersection Operations - Comprehensive Guide
 * =====================================================
 * 
 * 📚 WHAT: Advanced type intersection patterns
 * 💡 WHERE: Type composition, API combinations, feature merging
 * 🔧 HOW: Intersection types, conditional intersections, merging
 */

// ============================================================================
// SECTION 1: WHAT - Type Intersections
// ============================================================================

/**
 * WHAT are type intersections?
 * - Combining multiple types into one
 * - All properties from all types available
 * - Used for type composition
 * - Different from union types
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use type intersections?
 * - Combine API response types
 * - Create feature-rich interfaces
 * - Merge configuration types
 * - Implement composition patterns
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Basic Intersection
// --------------------------------

type Person = { name: string; age: number };
type Employee = { company: string; role: string };

type PersonEmployee = Person & Employee;
// { name: string; age: number; company: string; role: string }

// Example 3.2: Intersection with Mapped Types
// ------------------------------------------

type WithTimestamp<T> = T & { createdAt: Date; updatedAt: Date };

type TimedUser = WithTimestamp<{ name: string }>;
// { name: string; createdAt: Date; updatedAt: Date }

// Example 3.3: Conditional Intersection
// ------------------------------------

type MaybeIntersect<T, B extends boolean> = B extends true ? T & { id: string } : T;

type WithId = MaybeIntersect<{ name: string }, true>;
// { name: string; id: string }

type WithoutId = MaybeIntersect<{ name: string }, false>;
// { name: string }

// Example 3.4: Merge Object Types
// --------------------------------

type Merge<T> = {
  [K in keyof T]: T[K];
};

type A = { a: string; common: number };
type B = { b: string; common: string };

type Merged = Merge<A & B>;
// { a: string; b: string; common: string }

// Example 3.5: Selective Intersection
// ------------------------------------

type IntersectKeys<T, U> = keyof T & keyof U;

type CommonKeys = IntersectKeys<{ a: 1; b: 2 }, { b: 2; c: 3 }>;
// "b"

type PickCommon<T, U> = {
  [K in IntersectKeys<T, U>]: T[K];
};

// Example 3.6: Intersection with Utility Types
// --------------------------------------------

type Base = { id: string; createdAt: Date };
type Optional = Partial<Base>;
type RequiredBase = Required<Base>;

type Combined = Optional & { id: string };
// { id?: string; createdAt?: Date } & { id: string }

// Example 3.7: Type Guard Intersections
// ------------------------------------

type FlatIntersection<T> = {
  [K in keyof T]: T[K];
};

type DeepMerge<T, U> = T & U extends infer V 
  ? V extends object 
    ? FlatIntersection<V> 
    : V 
  : never;

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Intersection operations are O(n) where n is total keys
 * - Deep intersections may slow compilation
 * - Generally efficient
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
 * - Test with overlapping keys
 * - Test with different property types
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over intersection type
 * - Break into individual types
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Manual type composition
 * - Extend/interfaces
 * - Helper libraries
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Type Intersection Operations Complete ===");
console.log("Next: 08_Advanced_Type_Utilities/02_Type_Union_Operations.ts");
console.log("Related: 02_Conditional_and_Mapped_Types/02_Mapped_Types.ts, 03_Type_Transformation.ts");