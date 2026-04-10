/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 07_Exclude_Type
 * Purpose: Excludes types from a union
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe union manipulation
 */

/**
 * Exclude<T, U> - Built-in Utility Type
 * =====================================
 * 
 * 📚 WHAT: Excludes from T all types that are assignable to U
 * 💡 WHY: Use when you need to remove types from a union
 * 🔧 HOW: Filters union types
 */

// ============================================================================
// SECTION 1: BASIC EXCLUDE USAGE
// ============================================================================

// Example 1.1: Using Exclude utility type
type AllStatus = "pending" | "active" | "completed" | "failed" | "cancelled";
type FinalStatus = Exclude<AllStatus, "pending" | "active">;

// Result:
// type FinalStatus = "completed" | "failed" | "cancelled"

// Example 1.2: Excluding null and undefined
type NullableString = string | null | undefined;
type NonNullableString = Exclude<NullableString, null | undefined>;

// Result:
// type NonNullableString = string

// ============================================================================
// SECTION 2: PRACTICAL PATTERNS
// ============================================================================

// Example 2.1: API error handling
type HttpStatus = 200 | 201 | 204 | 400 | 401 | 403 | 404 | 500 | 502 | 503;
type SuccessStatus = Exclude<HttpStatus, 400 | 401 | 403 | 404 | 500 | 502 | 503>;
type ErrorStatus = Exclude<HttpStatus, 200 | 201 | 204>;

// Example 2.2: Removing certain keys from union
type PropertyKeys = "id" | "name" | "email" | "password" | "createdAt" | "updatedAt";
type PublicKeys = Exclude<PropertyKeys, "password">;

// ============================================================================
// SECTION 3: COMPOSITION WITH OTHER UTILITIES
// ============================================================================

// Example 3.1: Exclude with keyof
type ObjectKeys = { a: 1; b: 2; c: 3 };
type ExcludedKeys = Exclude<keyof ObjectKeys, "c">;

// Example 3.2: Exclude in conditional types
type Filtered<T, Excluded> = T extends Excluded ? never : T;

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Exclude<T, U> is compile-time only. No runtime overhead.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Exclude<T, U> requires TypeScript 2.8+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Exclude helps filter out invalid or sensitive types from unions.
 * Use for validation and error handling.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with single and multiple excluded types. Verify correct result.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result. Check errors for invalid exclusion.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Extract: The inverse operation
 * - Manual union: More explicit
 * - Conditional types: More flexible
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 08_Extract_Type.ts: Extracts types from union
 * - 09_NonNullable_Type.ts: Removes null/undefined
 * - 06_Omit_Type.ts: Excludes properties
 */

console.log("=== Exclude Type Complete ===");
