/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 03_Readonly_Type
 * Purpose: Makes all properties readonly in a type
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe immutability
 */

/**
 * Readonly<T> - Built-in Utility Type
 * ==================================
 * 
 * 📚 WHAT: Makes all properties of a type readonly (immutable)
 * 💡 WHY: Use when you want to prevent mutations
 * 🔧 HOW: Adds readonly modifier to all properties
 */

// ============================================================================
// SECTION 1: BASIC READONLY USAGE
// ============================================================================

// Example 1.1: Using Readonly utility type
interface Point {
  x: number;
  y: number;
}

type ReadonlyPoint = Readonly<Point>;

// Result:
// type ReadonlyPoint = {
//   readonly x: number;
//   readonly y: number;
// }

// Example 1.2: Preventing mutation
function movePoint(point: Readonly<Point>, newX: number, newY: number): Readonly<Point> {
  // Cannot modify point directly
  return { x: newX, y: newY };
}

const origin: Readonly<Point> = { x: 0, y: 0 };
const moved = movePoint(origin, 5, 10);

// ============================================================================
// SECTION 2: DEEP READONLY (CUSTOM)
// ============================================================================

// Example 2.1: Deep Readonly implementation
type DeepReadonly<T> = {
  readonly [P in keyof T]: T[P] extends object ? DeepReadonly<T[P]> : T[P];
};

interface Config {
  database: {
    host: string;
    port: number;
  };
  features: {
    auth: boolean;
  };
}

type ReadonlyConfig = DeepReadonly<Config>;

// ============================================================================
// SECTION 3: PRACTICAL PATTERNS
// ============================================================================

// Example 3.1: Constants
const appConfig: Readonly<{ name: string; version: string }> = {
  name: "MyApp",
  version: "1.0.0",
};

// Example 3.2: API response
interface ApiResponse {
  data: unknown;
  status: number;
}

function cacheResponse(response: Readonly<ApiResponse>): void {
  console.log(`Caching response with status: ${response.status}`);
}

// Example 3.3: Readonly arrays
function processItems(items: ReadonlyArray<string>): void {
  // Cannot modify array
  for (const item of items) {
    console.log(item);
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Readonly<T> is entirely compile-time. No runtime overhead.
 * Deep Readonly may slow compilation for deeply nested types.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Readonly<T> requires TypeScript 2.8+.
 * Deep Readonly requires TypeScript 4.1+ for recursive conditional types.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Readonly prevents accidental mutations. Use for immutable
 * data structures and constants. Prevents security issues from mutable state.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test that all properties become readonly. Verify nested types.
 * Test that mutations fail at compile time.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result. Check errors for mutations.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Object.freeze: Runtime immutability
 * - Manual readonly: More explicit
 * - Custom utility: For deep readonly
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 01_Required_Type.ts: Required properties
 * - 02_Partial_Type.ts: Optional properties
 * - 04_Record_Type.ts: Record types
 */

console.log("=== Readonly Type Complete ===");
