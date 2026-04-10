/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 10_ReturnType_Type
 * Purpose: Extracts return type from a function
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe function returns
 */

/**
 * ReturnType<T> - Built-in Utility Type
 * =====================================
 * 
 * 📚 WHAT: Extracts the return type of function type T
 * 💡 WHY: Use when you need to work with function return types
 * 🔧 HOW: Extracts return type from function signature
 */

// ============================================================================
// SECTION 1: BASIC RETURNTYPE USAGE
// ============================================================================

// Example 1.1: Using ReturnType utility type
function getUser(): { id: number; name: string } {
  return { id: 1, name: "John" };
}

type UserReturn = ReturnType<typeof getUser>;

// Result:
// type UserReturn = { id: number; name: string }

// Example 1.2: ReturnType with arrow function
type AsyncResult = ReturnType<() => Promise<string>>;

// Result:
// type AsyncResult = Promise<string>

// ============================================================================
// SECTION 2: PRACTICAL PATTERNS
// ============================================================================

// Example 2.1: Wrapping function return types
function createUser(name: string): { id: number; name: string } {
  return { id: Date.now(), name };
}

function withLogging<T>(fn: () => T): () => T {
  return () => {
    console.log("Function called");
    return fn();
  };
}

const logged = withLogging(createUser);
type LoggedReturn = ReturnType<typeof logged>;

// Example 2.2: Type-safe API response
interface ApiClient {
  getUser(): Promise<{ id: number }>;
}

type ApiReturn = ReturnType<ApiClient["getUser"]>;

// ============================================================================
// SECTION 3: COMPOSITION WITH OTHER UTILITIES
// ============================================================================

// Example 3.1: ReturnType with Partial
function getConfig(): { host: string; port: number } {
  return { host: "localhost", port: 5432 };
}

type PartialConfig = Partial<ReturnType<typeof getConfig>>;

// Example 3.2: ReturnType in generics
function createHandler<F extends () => unknown>(fn: F): ReturnType<F> {
  return fn();
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: ReturnType<T> is compile-time only. No runtime overhead.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: ReturnType<T> requires TypeScript 2.8+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: ReturnType helps maintain type safety when working with
 * function returns. Use for wrapper functions and type inference.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with various function signatures. Verify correct extraction.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - typeof with return type: More explicit
 * - Parameters: For parameter types
 * - InstanceType: For constructor types
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 11_Parameters_Type.ts: Extracts parameters
 * - 12_InstanceType_Type.ts: Extracts instance type
 * - 10_Infer_Type_Patterns.ts: Custom inference
 */

console.log("=== ReturnType Type Complete ===");
