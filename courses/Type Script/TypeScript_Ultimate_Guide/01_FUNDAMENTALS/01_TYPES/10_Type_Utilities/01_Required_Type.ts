/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 01_Required_Type
 * Purpose: Makes all properties required in a type
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe transformations
 */

/**
 * Required<T> - Built-in Utility Type
 * ==================================
 * 
 * 📚 WHAT: Makes all properties of a type required (non-optional)
 * 💡 WHY: Use when you need all properties to be present
 * 🔧 HOW: Removes the ? modifier from all properties
 */

// ============================================================================
// SECTION 1: BASIC REQUIRED USAGE
// ============================================================================

// Example 1.1: Using Required utility type
interface User {
  name?: string;
  age?: number;
  email?: string;
}

type RequiredUser = Required<User>;

// Result:
// type RequiredUser = {
//   name: string;
//   age: number;
//   email: string;
// }

// Example 1.2: Working with partial types
interface Config {
  host?: string;
  port?: number;
  ssl?: boolean;
}

function createConfig(defaults: Required<Config>): Config {
  return defaults;
}

const fullConfig = createConfig({
  host: "localhost",
  port: 5432,
  ssl: true,
});

// ============================================================================
// SECTION 2: DEEP REQUIRED (CUSTOM)
// ============================================================================

// Example 2.1: Deep Required implementation
type DeepRequired<T> = {
  [P in keyof T]-?: T[P] extends object ? DeepRequired<T[P]> : T[P];
};

interface NestedConfig {
  database?: {
    host?: string;
    port?: number;
  };
  cache?: {
    enabled?: boolean;
    ttl?: number;
  };
}

type FullConfig = DeepRequired<NestedConfig>;

// Result:
// type FullConfig = {
//   database: {
//     host: string;
//     port: number;
//   };
//   cache: {
//     enabled: boolean;
//     ttl: number;
//   };
// }

// ============================================================================
// SECTION 3: PRACTICAL PATTERNS
// ============================================================================

// Example 3.1: Validation after Required
interface FormInput {
  username?: string;
  password?: string;
}

function validateRequired(input: FormInput): Required<FormInput> {
  if (!input.username || !input.password) {
    throw new Error("Missing required fields");
  }
  return input as Required<FormInput>;
}

// Example 3.2: Function requiring all fields
function sendRequest(config: Required<{ url: string; method: string; headers?: Record<string, string> }>) {
  console.log(`Sending ${config.method} request to ${config.url}`);
}

sendRequest({
  url: "https://api.example.com",
  method: "GET",
  headers: { "Content-Type": "application/json" },
});

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Required<T> is entirely compile-time. No runtime overhead.
 * Deep Required may slow compilation for deeply nested types.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Required<T> requires TypeScript 2.8+.
 * Deep Required requires TypeScript 4.1+ for recursive conditional types.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Required helps ensure all fields are present before processing.
 * Use for validation and ensuring complete configuration.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test that all properties become required. Verify nested
 * types work correctly. Test with optional methods.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result. Check errors for missing
 * properties.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Manual required types: More explicit
 * - Partial then validate: More flexible
 * - Custom utility: For deep required
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 02_Partial_Type.ts: Makes all properties optional
 * - 03_Readonly_Type.ts: Makes all properties readonly
 * - 05_Pick_Type.ts: Selects specific properties
 */

console.log("=== Required Type Complete ===");
