/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 11_Type_Libraries
 * Topic: 05_Utility_Type_Composition
 * Purpose: Combining utility types for complex type transformations
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe transformations
 */

/**
 * Utility Type Composition
 * ========================
 * 
 * 📚 WHAT: Combining multiple utility types for complex transformations
 * 💡 WHY: Real-world types often require composing multiple utilities
 * 🔧 HOW: Chain and combine built-in and custom utility types
 */

// ============================================================================
// SECTION 1: BASIC COMPOSITION
// ============================================================================

// Example 1.1: Pick + Partial
interface User {
  id: number;
  name: string;
  email: string;
  password: string;
  createdAt: Date;
}

type UpdateUser = Partial<Pick<User, "name" | "email">>;

// Result: { name?: string; email?: string }

// Example 1.2: Omit + Partial
type PartialUser = Partial<Omit<User, "id" | "createdAt">>;

// Result: { name?: string; email?: string; password?: string }

// ============================================================================
// SECTION 2: ADVANCED COMPOSITION
// ============================================================================

// Example 2.1: Pick + Required + Readonly
type FrozenUser = Readonly<Required<Pick<User, "name" | "email">>>;

// Result: { readonly name: string; readonly email: string }

// Example 2.2: Record + Partial
type UsersMap = Record<string, Partial<User>>;

// Result: { [key: string]: Partial<User> }

// ============================================================================
// SECTION 3: CUSTOM + BUILT-IN COMPOSITION
// ============================================================================

// Example 3.1: Deep utilities composition
type DeepPartial<T> = {
  [P in keyof T]?: T[P] extends object ? DeepPartial<T[P]> : T[P];
};

type DeepPartialUser = DeepPartial<User>;
// { id?: number; name?: string; email?: string; ... }

// Example 3.2: Composed with Extract
type UserKeys = keyof User;
type StringKeys = Extract<UserKeys, "name" | "email">;
type PickedStrings = Pick<User, StringKeys>;

// ============================================================================
// SECTION 4: PRACTICAL COMPOSITIONS
// ============================================================================

// Example 4.1: API DTO composition
interface FullEntity {
  id: number;
  name: string;
  description: string;
  createdAt: Date;
  updatedAt: Date;
  internalNote: string;
}

type PublicDTO = Omit<FullEntity, "internalNote">;
type CreateDTO = Partial<Omit<FullEntity, "id" | "createdAt" | "updatedAt">>;
type UpdateDTO = Partial<Pick<FullEntity, "name" | "description">>;

// Example 4.2: Form composition
interface FormState {
  username: string;
  email: string;
  password: string;
  bio: string;
}

type FormField<K extends keyof FormState> = Pick<FormState, K>;
type FormFields = Partial<FormState>;

// ============================================================================
// SECTION 5: GENERIC COMPOSITION
// ============================================================================

// Example 5.1: Generic type helper
type CreateFrom<T, K extends keyof T> = 
  Readonly<Required<Pick<T, K>>>;

type RequiredFields = CreateFrom<User, "name" | "email">;

// Example 5.2: Generic API wrapper
type ApiResponse<T, ExcludeKeys extends keyof T = never> = {
  data: Omit<T, ExcludeKeys>;
  meta: {
    timestamp: number;
    version: string;
  };
};

type UserResponse = ApiResponse<User, "password">;

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Complex compositions increase compilation time.
 * Use caching for frequently used composed types.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Composition uses built-in types requiring TypeScript 2.8+.
 * Custom deep types require TypeScript 4.1+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Composition helps create type-safe APIs.
 * Use to restrict exposed data in DTOs.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test each composition with known inputs.
 * Verify output types are correct.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Break complex compositions into steps.
 * Check intermediate types.
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 01_Utility_Types_Overview.ts: Utility overview
 * - 05_Pick_Type.ts: Pick type
 * - 06_Omit_Type.ts: Omit type
 * - 01_Required_Type.ts: Required type
 * - 03_Readonly_Type.ts: Readonly type
 */

console.log("=== Utility Type Composition Complete ===");
