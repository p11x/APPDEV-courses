/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 08_Advanced_Type_System
 * Topic: 03_Discriminated_Unions
 * Purpose: Pattern matching with discriminated unions for type-safe conditional logic
 * Difficulty: intermediate
 * UseCase: web, backend, state_management
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe pattern reduces runtime errors
 */

/**
 * Discriminated Unions - Type-Safe Pattern Matching
 * ==================================================
 * 
 * 📚 WHAT: Union types with a common discriminant property for exhaustive type checking
 * 💡 WHY: Enables exhaustive pattern matching and type-safe conditionals
 * 🔧 HOW: Common property (discriminant) narrows union to specific member
 */

// ============================================================================
// SECTION 1: BASIC DISCRIMINATED UNION
// ============================================================================

// Example 1.1: Basic discriminated union with switch
type PaymentStatus = 
  | { status: "pending" }
  | { status: "processing" }
  | { status: "completed"; transactionId: string }
  | { status: "failed"; error: string };

function handlePayment(payment: PaymentStatus): void {
  switch (payment.status) {
    case "pending":
      console.log("Payment is pending...");
      break;
    case "processing":
      console.log("Payment is being processed...");
      break;
    case "completed":
      console.log(`Payment completed: ${payment.transactionId}`);
      break;
    case "failed":
      console.error(`Payment failed: ${payment.error}`);
      break;
  }
}

// Example 1.2: Discriminated union with action payloads
type Action =
  | { type: "increment"; payload: { amount: number } }
  | { type: "decrement"; payload: { amount: number } }
  | { type: "reset" }
  | { type: "setValue"; payload: { value: number } };

function reducer(action: Action): number {
  switch (action.type) {
    case "increment":
      return action.payload.amount;
    case "decrement":
      return -action.payload.amount;
    case "reset":
      return 0;
    case "setValue":
      return action.payload.value;
    default:
      const _exhaustive: never = action;
      return _exhaustive;
  }
}

// ============================================================================
// SECTION 2: DISCRIMINATED UNIONS WITH GENERICS
// ============================================================================

// Example 2.1: Generic discriminated union for API responses
type ApiResponse<T, E = never> = 
  | { ok: true; data: T }
  | { ok: false; error: E };

type UserResponse = ApiResponse<{ id: number; name: string }, Error>;
type UsersResponse = ApiResponse<Array<{ id: number; name: string }>, Error>;

function handleApiResponse<T, E>(response: ApiResponse<T, E>): T | undefined {
  if (response.ok) {
    return response.data;
  }
  console.error(response.error);
  return undefined;
}

// Example 2.2: Event system with discriminated unions
type Event<T extends string = string, P = unknown> = 
  | { type: T; payload: P };

type AppEvent = 
  | Event<"user:login", { userId: string; timestamp: Date }>
  | Event<"user:logout", { userId: string }>
  | Event<"data:sync", { records: number }>
  | Event<"error", { message: string }>;

function dispatchEvent(event: AppEvent): void {
  console.log(`Event: ${event.type}`, event.payload);
}

// ============================================================================
// SECTION 3: EXHAUSTIVE CHECKING
// ============================================================================

// Example 3.1: Compile-time exhaustive checking
type Color = "red" | "green" | "blue";

function getColorName(color: Color): string {
  switch (color) {
    case "red":
      return "Red";
    case "green":
      return "Green";
    case "blue":
      return "Blue";
    default:
      const _exhaustive: never = color;
      throw new Error(`Unknown color: ${_exhaustive}`);
  }
}

// Example 3.2: Runtime exhaustive checking with assertion
function processResult(result: PaymentStatus): string {
  if (result.status === "completed") {
    return `Transaction: ${result.transactionId}`;
  } else if (result.status === "failed") {
    return `Error: ${result.error}`;
  }
  return "Processing...";
}

// ============================================================================
// SECTION 4: ADVANCED PATTERNS
// ============================================================================

// Example 4.1: Nested discriminated unions
type HTTPMethod = "GET" | "POST" | "PUT" | "DELETE";

type RequestConfig<M extends HTTPMethod> = 
  M extends "GET" 
    ? { method: M; params?: Record<string, string> }
    : M extends "POST" | "PUT"
      ? { method: M; body: unknown }
      : { method: M };

function makeRequest<M extends HTTPMethod>(config: RequestConfig<M>): void {
  console.log(`${config.method} request with config:`, config);
}

// Example 4.2: Discriminated union with factory pattern
type Shape = 
  | { kind: "circle"; radius: number }
  | { kind: "rectangle"; width: number; height: number }
  | { kind: "triangle"; base: number; height: number };

type ShapeArea = 
  | { kind: "circle"; radius: number; area: number }
  | { kind: "rectangle"; width: number; height: number; area: number }
  | { kind: "triangle"; base: number; height: number; area: number };

function calculateArea(shape: Shape): ShapeArea {
  switch (shape.kind) {
    case "circle":
      return { ...shape, area: Math.PI * shape.radius ** 2 };
    case "rectangle":
      return { ...shape, area: shape.width * shape.height };
    case "triangle":
      return { ...shape, area: 0.5 * shape.base * shape.height };
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Discriminated unions have zero runtime overhead as they
 * compile to plain JavaScript. The TypeScript compiler ensures type safety
 * at compile time. Use discriminant properties that are strings or numbers
 * for optimal type inference.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Supported in all TypeScript targets. Requires TypeScript
 * 2.0+ for basic discriminated unions, 4.6+ for improved narrowing.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Discriminated unions help prevent runtime errors by ensuring
 * all cases are handled. The exhaustive checking pattern catches missing
 * cases at compile time, reducing security vulnerabilities from unhandled
 * edge cases.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test each union member separately. Use type guards to ensure
 * correct handling. Verify that exhaustive checking works by adding new
 * union members and confirming compile errors.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use TypeScript's error messages to identify missing cases.
 * Enable strictNullChecks for better narrowing. Use debugger to inspect
 * runtime values when type narrowing fails.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Union types without discriminant: Harder to narrow safely
 * - Type guards: More verbose, less maintainable
 * - If-else chains: Error-prone, no exhaustive checking
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 04_Type_Narrowing.ts: Related to type narrowing techniques
 * - 08_Type_Predicates_Advanced.ts: Custom type predicates
 * - 05_Recursive_Type_Definitions.ts: Recursive type patterns
 */

console.log("=== Discriminated Unions Examples Complete ===");
