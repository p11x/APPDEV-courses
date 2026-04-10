/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 08_Advanced_Type_System
 * Topic: 09_Assertion_Types
 * Purpose: Type assertion patterns for complex type transformations
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Use assertions carefully to avoid type safety issues
 */

/**
 * Assertion Types - Type Assertion Patterns
 * =========================================
 * 
 * 📚 WHAT: Advanced type assertion patterns for type transformations
 * 💡 WHY: Enables working with complex types and type inference
 * 🔧 HOW: Type assertions, casting, and assertion functions
 */

// ============================================================================
// SECTION 1: BASIC TYPE ASSERTIONS
// ============================================================================

// Example 1.1: Basic type assertion
function basicAssertion(): void {
  const value: unknown = "hello";
  const str = value as string;
  console.log(str.toUpperCase());
}

// Example 1.2: Double assertion for complex types
type Complex = { a: { b: { c: string } } };

function doubleAssertion(value: unknown): void {
  const result = value as unknown as Complex;
  console.log(result.a.b.c);
}

// Example 1.3: Non-null assertion
function nonNullAssertion(): void {
  const arr: number[] = [];
  const first = arr[0]!;
  console.log(first * 2);
}

// ============================================================================
// SECTION 2: CONST ASSERTIONS
// ============================================================================

// Example 2.1: Const assertion for literal types
const colors = ["red", "green", "blue"] as const;
type Color = typeof colors[number]; // "red" | "green" | "blue"

// Example 2.2: Const assertion with objects
const config = {
  endpoint: "https://api.example.com",
  timeout: 5000,
} as const;

type Config = typeof config;
// {
//   readonly endpoint: "https://api.example.com";
//   readonly timeout: 5000;
// }

// Example 2.3: Const assertion for function arguments
function route(path: "/home" | "/about" | "/contact"): void {
  console.log(path);
}

route("/home" as const);

// ============================================================================
// SECTION 3: ASSERTION FUNCTIONS
// ============================================================================

// Example 3.1: Type assertion function
function assertIsString(value: unknown): asserts value is string {
  if (typeof value !== "string") {
    throw new Error("Value is not a string");
  }
}

function processWithAssertion(value: unknown): void {
  assertIsString(value);
  console.log(value.toUpperCase());
}

// Example 3.2: Assertion with custom error
function assertNonNull<T>(value: T | null | undefined, message: string): asserts value is T {
  if (value === null || value === undefined) {
    throw new Error(message);
  }
}

// Example 3.3: Condition assertion
function assert(condition: boolean, message: string): asserts condition {
  if (!condition) {
    throw new Error(message);
  }
}

// ============================================================================
// SECTION 4: COMPOUND ASSERTIONS
// ============================================================================

// Example 4.1: Branded type assertions
type Brand<T, B> = T & { __brand: B };

type UserId = Brand<string, "UserId">;

function toUserId(value: string): UserId {
  return value as UserId;
}

function assertIsUserId(value: string): asserts value is UserId {
  if (!value.startsWith("user_")) {
    throw new Error("Invalid UserId");
  }
}

// Example 4.2: Union type assertions
type Status = "pending" | "active" | "completed";

function assertIsStatus(value: string): asserts value is Status {
  if (!["pending", "active", "completed"].includes(value)) {
    throw new Error(`Invalid status: ${value}`);
  }
}

// ============================================================================
// SECTION 5: MAPPED TYPE ASSERTIONS
// ============================================================================

// Example 5.1: Partial assertion
function assertIsPartial<T>(value: unknown): asserts value is Partial<T> {
  if (typeof value !== "object" || value === null) {
    throw new Error("Value is not an object");
  }
}

// Example 5.2: Required assertion
function assertIsRequired<T>(value: unknown): asserts value is Required<T> {
  if (typeof value !== "object" || value === null) {
    throw new Error("Value is not an object");
  }
  
  const obj = value as Record<string, unknown>;
  const keys = Object.keys(obj);
  for (const key of keys) {
    if (obj[key] === undefined) {
      throw new Error(`Missing required property: ${key}`);
    }
  }
}

// ============================================================================
// SECTION 6: GENERIC ASSERTION PATTERNS
// ============================================================================

// Example 6.1: Generic type assertion
function as<T>(value: unknown): T {
  return value as T;
}

function processGeneric<T>(value: unknown): T {
  return as<T>(value);
}

// Example 6.2: Generic assertion function
function assertArrayOf<T>(
  value: unknown, 
  guard: (item: unknown) => item is T
): asserts value is T[] {
  if (!Array.isArray(value)) {
    throw new Error("Value is not an array");
  }
  
  for (const item of value) {
    if (!guard(item)) {
      throw new Error("Array contains invalid items");
    }
  }
}

function isString4(item: unknown): item is string {
  return typeof item === "string";
}

// ============================================================================
// SECTION 7: PRACTICAL ASSERTION PATTERNS
// ============================================================================

// Example 7.1: JSON parsing with assertion
interface JSONResult<T> {
  success: true;
  data: T;
} | {
  success: false;
  error: Error;
};

function parseJSON<T>(json: string): JSONResult<T> {
  try {
    const data = JSON.parse(json) as T;
    return { success: true, data };
  } catch (e) {
    return { success: false, error: e as Error };
  }
}

// Example 7.2: Event target assertion
type EventMap = {
  click: { x: number; y: number };
  keypress: { key: string };
};

function handleEvent<K extends keyof EventMap>(
  type: K, 
  event: unknown
): EventMap[K] {
  return event as EventMap[K];
}

// Example 7.3: API response assertion
interface ApiResponse<T> {
  data: T;
  meta: {
    timestamp: number;
    version: string;
  };
}

function assertApiResponse<T>(
  response: unknown
): asserts response is ApiResponse<T> {
  if (typeof response !== "object" || response === null) {
    throw new Error("Invalid response");
  }
  
  const r = response as Record<string, unknown>;
  if (!("data" in r) || !("meta" in r)) {
    throw new Error("Missing required properties");
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Type assertions are compile-time only. They have no runtime
 * overhead beyond the assertion function execution. Use sparingly as they
 * can mask type errors.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Type assertions require TypeScript 1.6+. Const assertions
 * require TypeScript 3.4+. Asserts parameter require TypeScript 3.9+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Type assertions bypass TypeScript's type checking. Use assertion
 * functions for validation to maintain type safety. Never assert external
 * input types without validation.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test assertion functions with valid and invalid inputs. Verify
 * that assertions throw errors for invalid data. Test compound assertions
 * with edge cases.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use assertion functions with custom error messages. Break
 * complex assertions into smaller assertions. Add logging in assertion
 * functions for complex cases.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Type guards: Safer than assertions, compiler verifies narrowing
 * - Discriminated unions: More explicit type narrowing
 * - Runtime validation: More flexible but requires library
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 08_Type_Predicates_Advanced.ts: Type predicates with assertions
 * - 04_Type_Narrowing.ts: Safe type narrowing
 * - 07_Conditional_Type_Chaining.ts: Conditional assertions
 */

console.log("=== Assertion Types Complete ===");
