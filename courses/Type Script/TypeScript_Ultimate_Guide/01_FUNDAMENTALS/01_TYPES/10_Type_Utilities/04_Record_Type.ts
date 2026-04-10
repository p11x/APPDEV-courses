/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 04_Record_Type
 * Purpose: Constructs object type with keys and values
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe object construction
 */

/**
 * Record<Keys, Type> - Built-in Utility Type
 * ==========================================
 * 
 * 📚 WHAT: Constructs an object type with Keys as keys and Type as values
 * 💡 WHY: Use when you need to create object types with specific keys
 * 🔧 HOW: Maps key types to value types
 */

// ============================================================================
// SECTION 1: BASIC RECORD USAGE
// ============================================================================

// Example 1.1: Using Record with union keys
type Role = "admin" | "user" | "guest";
type RolePermissions = Record<Role, string[]>;

const permissions: RolePermissions = {
  admin: ["read", "write", "delete"],
  user: ["read", "write"],
  guest: ["read"],
};

// Example 1.2: Record with number keys
type StringMap = Record<number, string>;

const strings: StringMap = {
  0: "zero",
  1: "one",
  2: "two",
};

// ============================================================================
// SECTION 2: RECORD WITH CUSTOM KEYS
// ============================================================================

// Example 2.1: Creating lookup tables
interface User {
  id: number;
  name: string;
}

const userMap: Record<number, User> = {
  1: { id: 1, name: "Alice" },
  2: { id: 2, name: "Bob" },
};

function getUser(id: number): User | undefined {
  return userMap[id];
}

// Example 2.2: API endpoints
type HttpMethod = "GET" | "POST" | "PUT" | "DELETE";
type Endpoint = { path: string; requiresAuth: boolean };

const endpoints: Record<HttpMethod, Endpoint[]> = {
  GET: [{ path: "/users", requiresAuth: false }],
  POST: [{ path: "/users", requiresAuth: true }],
  PUT: [{ path: "/users/:id", requiresAuth: true }],
  DELETE: [{ path: "/users/:id", requiresAuth: true }],
};

// ============================================================================
// SECTION 3: PRACTICAL PATTERNS
// ============================================================================

// Example 3.1: Type-safe configuration
type ConfigKey = "host" | "port" | "timeout";
type ConfigValue = string | number | boolean;

const config: Record<ConfigKey, ConfigValue> = {
  host: "localhost",
  port: 5432,
  timeout: 5000,
};

// Example 3.2: Validation rules
type ValidationRule<T> = {
  required: boolean;
  validate: (value: T) => boolean;
};

type FormFields = "name" | "email" | "age";
const rules: Record<FormFields, ValidationRule<string | number>> = {
  name: { required: true, validate: (v) => typeof v === "string" && v.length > 0 },
  email: { required: true, validate: (v) => typeof v === "string" && v.includes("@") },
  age: { required: false, validate: (v) => typeof v === "number" && v > 0 },
};

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Record<K, T> is compile-time only. No runtime overhead.
 * Large record types may slow compilation.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Record<K, T> requires TypeScript 2.8+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Record provides type-safe object construction. Use for
 * configuration, lookup tables, and validation rules.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with various key types. Verify all keys are present.
 * Test with optional keys.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result. Check errors for missing keys.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Manual object type: More explicit
 * - Index signatures: Less type-safe
 * - Map type: For dynamic keys
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 05_Pick_Type.ts: Selects specific properties
 * - 06_Omit_Type.ts: Removes properties
 * - 07_Exclude_Type.ts: Excludes from union
 */

console.log("=== Record Type Complete ===");
