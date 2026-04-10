/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 06_Omit_Type
 * Purpose: Omits specific properties from a type
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe property removal
 */

/**
 * Omit<T, Keys> - Built-in Utility Type
 * =====================================
 * 
 * 📚 WHAT: Creates a type excluding the specified Keys from T
 * 💡 WHY: Use when you need a type with some properties removed
 * 🔧 HOW: Filters out properties by key
 */

// ============================================================================
// SECTION 1: BASIC OMIT USAGE
// ============================================================================

// Example 1.1: Using Omit utility type
interface User {
  id: number;
  name: string;
  email: string;
  password: string;
}

type UserWithoutPassword = Omit<User, "password">;

// Result:
// type UserWithoutPassword = {
//   id: number;
//   name: string;
//   email: string;
// }

// Example 1.2: Omitting multiple properties
type PublicUser = Omit<User, "password" | "id">;

const publicUser: PublicUser = {
  name: "John",
  email: "john@example.com",
};

// ============================================================================
// SECTION 2: PRACTICAL PATTERNS
// ============================================================================

// Example 2.1: Excluding sensitive data
interface FullResponse {
  data: unknown;
  error: string | null;
  statusCode: number;
  timestamp: Date;
  internalId: string;
}

type PublicResponse = Omit<FullResponse, "internalId">;

// Example 2.2: Creating update types
interface FullEntity {
  id: number;
  createdAt: Date;
  updatedAt: Date;
  name: string;
  description: string;
}

type CreateEntity = Omit<FullEntity, "id" | "createdAt" | "updatedAt">;
type UpdateEntity = Partial<Omit<FullEntity, "id" | "createdAt">>;

// ============================================================================
// SECTION 3: COMPOSITION WITH OTHER UTILITIES
// ============================================================================

// Example 3.1: Omit with Partial
type OptionalUser = Partial<Omit<User, "id">>;

// Example 3.2: Omit with Readonly
type ReadonlyUserNoPassword = Readonly<Omit<User, "password">>;

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Omit<T, K> is compile-time only. No runtime overhead.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Omit<T, K> requires TypeScript 2.8+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Omit helps remove sensitive properties. Use for DTOs
 * and API responses to prevent data leakage.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with single and multiple keys. Verify correct types.
 * Test that omitted keys are not present.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result. Check errors for invalid keys.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Manual type: More explicit
 * - Pick: The inverse operation
 * - Intersection: For combining exclusions
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 05_Pick_Type.ts: Selects specific properties
 * - 02_Partial_Type.ts: Makes all optional
 * - 07_Exclude_Type.ts: Excludes from union
 */

console.log("=== Omit Type Complete ===");
