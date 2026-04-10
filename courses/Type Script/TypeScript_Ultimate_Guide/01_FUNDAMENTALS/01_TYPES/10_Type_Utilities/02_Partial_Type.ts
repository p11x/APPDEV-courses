/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 02_Partial_Type
 * Purpose: Makes all properties optional in a type
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe transformations
 */

/**
 * Partial<T> - Built-in Utility Type
 * ==================================
 * 
 * 📚 WHAT: Makes all properties of a type optional
 * 💡 WHY: Use when creating objects with some fields optional
 * 🔧 HOW: Adds ? modifier to all properties
 */

// ============================================================================
// SECTION 1: BASIC PARTIAL USAGE
// ============================================================================

// Example 1.1: Using Partial utility type
interface User {
  id: number;
  name: string;
  email: string;
  age: number;
}

type PartialUser = Partial<User>;

// Result:
// type PartialUser = {
//   id?: number | undefined;
//   name?: string | undefined;
//   email?: string | undefined;
//   age?: number | undefined;
// }

// Example 1.2: Updating user with Partial
interface UserProfile {
  name: string;
  bio: string;
  avatar: string;
}

function updateProfile(current: UserProfile, updates: Partial<UserProfile>): UserProfile {
  return { ...current, ...updates };
}

const profile = updateProfile(
  { name: "John", bio: "Hello", avatar: "default.png" },
  { bio: "Updated bio" }
);

// ============================================================================
// SECTION 2: DEEP PARTIAL (CUSTOM)
// ============================================================================

// Example 2.1: Deep Partial implementation
type DeepPartial<T> = {
  [P in keyof T]?: T[P] extends object ? DeepPartial<T[P]> : T[P];
};

interface Config {
  database: {
    host: string;
    port: number;
  };
  cache: {
    enabled: boolean;
    ttl: number;
  };
}

type PartialConfig = DeepPartial<Config>;

// ============================================================================
// SECTION 3: PRACTICAL PATTERNS
// ============================================================================

// Example 3.1: Form state with Partial
interface FormState {
  username: string;
  email: string;
  password: string;
}

function initForm(): Partial<FormState> {
  return {};
}

function validateForm(data: Partial<FormState>): string[] {
  const errors: string[] = [];
  if (!data.username) errors.push("Username required");
  if (!data.email) errors.push("Email required");
  if (!data.password) errors.push("Password required");
  return errors;
}

// Example 3.2: Optional configuration
interface ApiOptions {
  timeout: number;
  retries: number;
  headers: Record<string, string>;
}

function createApiClient(options: Partial<ApiOptions> = {}): ApiOptions {
  return {
    timeout: options.timeout ?? 5000,
    retries: options.retries ?? 3,
    headers: options.headers ?? {},
  };
}

const client1 = createApiClient();
const client2 = createApiClient({ timeout: 10000 });

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Partial<T> is entirely compile-time. No runtime overhead.
 * Deep Partial may slow compilation for deeply nested types.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Partial<T> requires TypeScript 2.8+.
 * Deep Partial requires TypeScript 4.1+ for recursive conditional types.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Partial helps create flexible APIs where not all fields
 * are required. Use with validation for security.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test that all properties become optional. Verify nested
 * types work correctly. Test with validation.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result. Check errors for invalid
 * optional access.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Manual partial types: More explicit
 * - Intersection types: For combining partials
 * - Custom utility: For deep partial
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 01_Required_Type.ts: Makes all properties required
 * - 03_Readonly_Type.ts: Makes all properties readonly
 * - 06_Omit_Type.ts: Removes specific properties
 */

console.log("=== Partial Type Complete ===");
