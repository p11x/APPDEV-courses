/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 07
 * Topic: Type_Safe_Error_Handling
 * Purpose: Error handling with full type safety
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 * Version: TS 4.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Type Safe Error Handling - Comprehensive Guide
 * =======================================
 * 
 * 📚 WHAT: Error handling patterns with type safety
 * 💡 WHY: Prevents unhandled errors and improves type inference
 * 🔧 HOW: Result types, error boundaries, typed catches
 */

// ============================================================================
// SECTION 1: RESULT TYPE PATTERN
// ============================================================================

// Example 1.1: Basic Result Type
// ---------------------------

type Result<T, E = Error> = 
  | { ok: true; value: T }
  | { ok: false; error: E };

function safeDivide(a: number, b: number): Result<number, Error> {
  if (b === 0) {
    return { ok: false, error: new Error("Division by zero") };
  }
  return { ok: true, value: a / b };
}

// ============================================================================
// SECTION 2: ERROR BOUNDARIES
// ============================================================================

// Example 2.1: Error Boundary Pattern
// -------------------------------

type ErrorBoundary<T> = 
  | { success: true; data: T }
  | { success: false; error: Error };

async function fetchData(url: string): Promise<ErrorBoundary<string>> {
  try {
    const response = await fetch(url);
    const data = await response.text();
    return { success: true, data };
  } catch (error) {
    return { success: false, error: error as Error };
  }
}

// ============================================================================
// SECTION 3: TYPED CATCH CLAUSES
// ============================================================================

// Example 3.1: Type-Safe Try-Catch
// -----------------------------

function safeJsonParse(json: string): Result<unknown, SyntaxError> {
  try {
    return { ok: true, value: JSON.parse(json) };
  } catch (error) {
    return { ok: false, error: error as SyntaxError };
  }
}

// ============================================================================
// SECTION 4: CUSTOM ERROR TYPES
// ============================================================================

// Example 4.1: Typed Error Classes
// ------------------------------

class ValidationError extends Error {
  constructor(public field: string, message: string) {
    super(message);
    this.name = "ValidationError";
  }
}

type ApiError = ValidationError | SyntaxError | TypeError;

// ============================================================================
// SECTION 5: EXHAUSTIVE ERROR HANDLING
// ============================================================================

// Example 5.1: Exhaustive Error Handling
// ---------------------------------

function handleError(error: ApiError): string {
  if (error instanceof ValidationError) {
    return `Validation error on ${error.field}: ${error.message}`;
  }
  if (error instanceof SyntaxError) {
    return `Syntax error: ${error.message}`;
  }
  if (error instanceof TypeError) {
    return `Type error: ${error.message}`;
  }
  const _exhaustive: never = error;
  return _exhaustive;
}

// ============================================================================
// PERFORMANCE
// ============================================================================

// Uses try-catch which has overhead
// Consider Result type for hot paths

// ============================================================================
// COMPATIBILITY
// ============================================================================

// All TS versions
// All targets

// ============================================================================
// SECURITY
// ============================================================================

// Proper error handling prevents leaks
// Sanitize error messages in production

// ============================================================================
// TESTING
// ============================================================================

// Test success and error paths
// Verify error types

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 01_Pattern_Matching, 02_Exhaustive_Checking

console.log("\n=== Type Safe Error Handling Complete ===");
console.log("Next: 07_Advanced_Control_Flow/08_Async_Flow_Control");