/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 05_Pick_Type
 * Purpose: Picks specific properties from a type
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe property selection
 */

/**
 * Pick<T, Keys> - Built-in Utility Type
 * =====================================
 * 
 * 📚 WHAT: Creates a type with only the specified Keys from T
 * 💡 WHY: Use when you need a subset of a type's properties
 * 🔧 HOW: Filters properties by key
 */

// ============================================================================
// SECTION 1: BASIC PICK USAGE
// ============================================================================

// Example 1.1: Using Pick utility type
interface User {
  id: number;
  name: string;
  email: string;
  password: string;
  createdAt: Date;
}

type UserBasic = Pick<User, "id" | "name">;

// Result:
// type UserBasic = {
//   id: number;
//   name: string;
// }

// Example 1.2: Picking multiple properties
type UserProfile = Pick<User, "name" | "email">;

const userProfile: UserProfile = {
  name: "John",
  email: "john@example.com",
};

// ============================================================================
// SECTION 2: PRACTICAL PATTERNS
// ============================================================================

// Example 2.1: API response DTO
interface FullUser {
  id: number;
  name: string;
  email: string;
  passwordHash: string;
  role: string;
  createdAt: Date;
  updatedAt: Date;
}

type UserDTO = Pick<FullUser, "id" | "name" | "email">;

// Example 2.2: Form input type
interface FullForm {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
  bio: string;
}

type RegisterForm = Pick<FullForm, "username" | "email" | "password" | "confirmPassword">;

// Example 2.3: Update type
type UpdateUser = Pick<FullUser, "name" | "email" | "bio">;

function updateUser(id: number, updates: UpdateUser): void {
  console.log(`Updating user ${id}`, updates);
}

// ============================================================================
// SECTION 3: COMPOSITION WITH OTHER UTILITIES
// ============================================================================

// Example 3.1: Pick with Partial
type PartialPick<T, K extends keyof T> = Partial<Pick<T, K>>;

type OptionalUserBasic = PartialPick<User, "name" | "email">;

// Example 3.2: Pick with Readonly
type ImmutableUserBasic = Readonly<Pick<User, "id" | "name">;

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Pick<T, K> is compile-time only. No runtime overhead.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Pick<T, K> requires TypeScript 2.8+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Pick helps expose only necessary properties. Use for DTOs
 * and API responses to prevent leaking sensitive data.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with single and multiple keys. Verify correct types.
 * Test with invalid keys.
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
 * - Omit: The inverse operation
 * - Destructuring: Runtime selection
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 06_Omit_Type.ts: Removes specific properties
 * - 04_Record_Type.ts: Creates record types
 * - 07_Exclude_Type.ts: Excludes from union
 */

console.log("=== Pick Type Complete ===");
