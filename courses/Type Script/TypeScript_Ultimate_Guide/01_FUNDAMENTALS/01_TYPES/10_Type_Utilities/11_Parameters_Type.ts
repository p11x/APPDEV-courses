/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 11_Parameters_Type
 * Purpose: Extracts parameter types from a function
 * Difficulty: beginner
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe parameter handling
 */

/**
 * Parameters<T> - Built-in Utility Type
 * ====================================
 * 
 * 📚 WHAT: Extracts the parameter types of function T as a tuple
 * 💡 WHY: Use when you need to work with function parameter types
 * 🔧 HOW: Extracts parameters as tuple type
 */

// ============================================================================
// SECTION 1: BASIC PARAMETERS USAGE
// ============================================================================

// Example 1.1: Using Parameters utility type
function createUser(name: string, age: number): { name: string; age: number } {
  return { name, age };
}

type CreateUserParams = Parameters<typeof createUser>;

// Result:
// type CreateUserParams = [name: string, age: number]

// Example 1.2: Parameters with multiple args
type Fn = (a: number, b: string, c: boolean) => void;
type FnParams = Parameters<Fn>;

// Result:
// type FnParams = [a: number, b: string, c: boolean]

// ============================================================================
// SECTION 2: PRACTICAL PATTERNS
// ============================================================================

// Example 2.1: Proxy function
function originalFn(a: string, b: number): string {
  return `${a}: ${b}`;
}

function proxy(...args: Parameters<typeof originalFn>): ReturnType<typeof originalFn> {
  console.log("Calling with:", args);
  return originalFn(...args);
}

// Example 2.2: Event handler wrapper
type EventHandler = (event: MouseEvent, data: { x: number; y: number }) => void;
type EventParams = Parameters<EventHandler>;

function wrapHandler<T extends (...args: any[]) => void>(handler: T): (...args: Parameters<T>) => void {
  return (...args: Parameters<T>) => {
    console.log("Handler called with:", args);
    handler(...args);
  };
}

// ============================================================================
// SECTION 3: COMPOSITION WITH OTHER UTILITIES
// ============================================================================

// Example 3.1: Parameters with Partial
function configure(host: string, port: number, ssl: boolean): void {}
type ConfigParams = Partial<Parameters<typeof configure>>;

// Example 3.2: Parameters in generics
function apply<T extends (...args: any[]) => unknown>(
  fn: T, 
  args: Parameters<T>
): ReturnType<T> {
  return fn(...args);
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Parameters<T> is compile-time only. No runtime overhead.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Parameters<T> requires TypeScript 2.8+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Parameters helps maintain type safety for function calls.
 * Use for proxy functions and type-safe wrappers.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with various function signatures. Verify tuple types.
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
 * - typeof with parameters: More explicit
 * - ReturnType: For return types
 * - Argument types: Custom extraction
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 10_ReturnType_Type.ts: Extracts return type
 * - 12_InstanceType_Type.ts: Extracts instance type
 * - 10_Infer_Type_Patterns.ts: Custom inference
 */

console.log("=== Parameters Type Complete ===");
