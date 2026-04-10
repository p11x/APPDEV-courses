/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 09_NonNullable_Type
 * Purpose: Removes null and undefined from a type
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe null handling
 */

/**
 * NonNullable<T> - Built-in Utility Type
 * ======================================
 * 
 * 📚 WHAT: Removes null and undefined from type T
 * 💡 WHY: Use when you need to ensure a type is not nullable
 * 🔧 HOW: Filters out null and undefined
 */

// ============================================================================
// SECTION 1: BASIC NONNULLABLE USAGE
// ============================================================================

// Example 1.1: Using NonNullable utility type
type NullableString = string | null | undefined;
type NonNullString = NonNullable<NullableString>;

// Result:
// type NonNullString = string

// Example 1.2: NonNullable with union
type Mixed = string | number | null | undefined | boolean;
type NonMixed = NonNullable<Mixed>;

// Result:
// type NonMixed = string | number | boolean

// ============================================================================
// SECTION 2: PRACTICAL PATTERNS
// ============================================================================

// Example 2.1: Filtering array type
type ArrayWithNull = (string | null | undefined)[];
type CleanArray = NonNullable<string | null | undefined>[];

// Example 2.2: Function return type
function getValue(): string | null {
  return null;
}

type ReturnTypeNonNull = NonNullable<ReturnType<typeof getValue>>;

// ============================================================================
// SECTION 3: COMPOSITION WITH OTHER UTILITIES
// ============================================================================

// Example 3.1: NonNullable with Extract
type Status = "active" | null | undefined | "pending";
type NonNullStatus = NonNullable<Status>;

// Example 3.2: NonNullable in generics
function filterNonNull<T>(items: T[]): NonNullable<T>[] {
  return items.filter((item): item is NonNullable<T> => item != null) as NonNullable<T>[];
}

const result = filterNonNull([1, 2, null, 3, undefined, 4]);

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: NonNullable<T> is compile-time only. No runtime overhead.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: NonNullable<T> requires TypeScript 2.8+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: NonNullable helps ensure values are present.
 * Use for strict null checking and validation.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with null, undefined, and valid values.
 * Verify correct filtering.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Exclude: Manual exclusion
 * - Conditional types: More flexible
 * - Type guards: Runtime checking
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 07_Exclude_Type.ts: Excludes types
 * - 04_Type_Narrowing.ts: Type narrowing
 * - 08_Type_Predicates_Advanced.ts: Type predicates
 */

console.log("=== NonNullable Type Complete ===");
