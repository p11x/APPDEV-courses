/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 08_Extract_Type
 * Purpose: Extracts types from a union
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe union manipulation
 */

/**
 * Extract<T, U> - Built-in Utility Type
 * =====================================
 * 
 * 📚 WHAT: Extracts from T all types that are assignable to U
 * 💡 WHY: Use when you need to filter a union to specific types
 * 🔧 HOW: Selects types assignable to U
 */

// ============================================================================
// SECTION 1: BASIC EXTRACT USAGE
// ============================================================================

// Example 1.1: Using Extract utility type
type Status = "pending" | "active" | "completed" | "failed";
type SuccessStatus = Extract<Status, "pending" | "active" | "completed">;

// Result:
// type SuccessStatus = "pending" | "active" | "completed"

// Example 1.2: Extracting numeric types
type Numeric = string | number | boolean | null;
type OnlyNumbers = Extract<Numeric, number>;

// Result:
// type OnlyNumbers = number

// ============================================================================
// SECTION 2: PRACTICAL PATTERNS
// ============================================================================

// Example 2.1: Extracting error types
type ErrorCode = 400 | 401 | 403 | 404 | 500 | "auth" | "notFound" | "server";
type HttpErrorCodes = Extract<ErrorCode, number>;

// Example 2.2: Extracting from complex unions
type ResponseType = 
  | { type: "success"; data: unknown }
  | { type: "error"; error: Error }
  | { type: "loading" };

type SuccessResponse = Extract<ResponseType, { type: "success" }>;
type ErrorResponse = Extract<ResponseType, { type: "error" }>;

// ============================================================================
// SECTION 3: COMPOSITION WITH OTHER UTILITIES
// ============================================================================

// Example 3.1: Extract with Exclude
type Combined = "a" | "b" | "c" | 1 | 2 | 3;
type Letters = Extract<Combined, string>;
type Numbers = Exclude<Combined, string>;

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Extract<T, U> is compile-time only. No runtime overhead.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Extract<T, U> requires TypeScript 2.8+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Extract helps filter union types to safe subsets.
 * Use for type narrowing and validation.
 */

// ============================================================================
// TESTING
// ============================================================================

/parameter name="content">/**
 * Testing: Test with various source and target types. Verify correct extraction.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result. Check errors for invalid extraction.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Exclude: The inverse operation
 * - Manual union: More explicit
 * - Conditional types: More flexible
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 07_Exclude_Type.ts: Excludes types from union
 * - 09_NonNullable_Type.ts: Removes null/undefined
 * - 03_Discriminated_Unions.ts: Discriminant pattern
 */

console.log("=== Extract Type Complete ===");
