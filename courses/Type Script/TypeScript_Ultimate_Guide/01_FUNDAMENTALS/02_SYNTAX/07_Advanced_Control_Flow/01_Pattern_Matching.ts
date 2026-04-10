/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 07
 * Topic: Pattern_Matching
 * Purpose: Understanding pattern matching with discriminated unions and type narrowing
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 * Version: TS 4.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Pattern Matching - Comprehensive Guide
 * =======================================
 * 
 * 📚 WHAT: Pattern matching in TypeScript using discriminated unions and type guards
 * 💡 WHY: Enables exhaustive type checking and safe data handling with compile-time guarantees
 * 🔧 HOW: Discriminated unions, type guards, switch statements with never type checking
 * 
 * WHAT covers the fundamentals of pattern matching syntax and type narrowing
 * WHY explains the benefits for building type-safe applications
 * HOW provides practical implementation patterns and code examples
 * PERFORMANCE discusses optimization considerations for pattern matching
 * COMPATIBILITY covers browser and Node.js support
 * SECURITY addresses type safety and input validation
 * TESTING provides patterns for testing pattern matching functions
 * DEBUGGING offers tips for troubleshooting pattern matching issues
 * ALTERNATIVE discusses alternative approaches
 * CROSS-REFERENCE links to related topics
 */

// ============================================================================
// SECTION 1: DISCRIMINATED UNIONS
// ============================================================================

// Example 1.1: Basic Discriminated Union
// ----------------------------------

type Result<T> = 
  | { ok: true; value: T }
  | { ok: false; error: string };

function processResult<T>(result: Result<T>): T {
  if (result.ok) {
    return result.value;
  }
  throw new Error(result.error);
}

// Example 1.2: Multiple Discriminators
// ---------------------------------

type HttpResponse<T> = 
  | { status: 200; data: T }
  | { status: 201; data: T; location: string }
  | { status: 400; error: string }
  | { status: 500; error: string };

function handleResponse<T>(response: HttpResponse<T>): T | never {
  switch (response.status) {
    case 200:
    case 201:
      return response.data;
    case 400:
    case 500:
      throw new Error(response.error);
  }
}

// ============================================================================
// SECTION 2: TYPED PATTERN MATCHING
// ============================================================================

// Example 2.1: Extract Pattern with Generics
// ---------------------------------------

type ExtractVariant<T> = 
  | { type: "string"; value: string }
  | { type: "number"; value: number }
  | { type: "boolean"; value: boolean }
  | { type: "array"; value: unknown[] };

function matchType(variant: ExtractVariant<string>): string {
  switch (variant.type) {
    case "string":
      return variant.value.toUpperCase();
    case "number":
      return String(variant.value);
    case "boolean":
      return String(variant.value);
    case "array":
      return variant.value.join(",");
  }
}

// Example 2.2: Nested Pattern Matching
// -----------------------------------

type Tree<T> = 
  | { type: "leaf"; value: T }
  | { type: "node"; left: Tree<T>; right: Tree<T> };

function treeToArray<T>(tree: Tree<T>): T[] {
  switch (tree.type) {
    case "leaf":
      return [tree.value];
    case "node":
      return [...treeToArray(tree.left), ...treeToArray(tree.right)];
  }
}

// ============================================================================
// SECTION 3: TYPE NARROWING WITH PATTERNS
// ============================================================================

// Example 3.1: Custom Type Guards
// -----------------------------

interface Fish { swim(): void; }
interface Bird { fly(): void; }
interface Dog { bark(): void; }

type Pet = Fish | Bird | Dog;

function isFish(pet: Pet): pet is Fish {
  return "swim" in pet;
}

function handlePet(pet: Pet): void {
  if (isFish(pet)) {
    pet.swim();
  } else if ("fly" in pet) {
    pet.fly();
  } else {
    pet.bark();
  }
}

// Example 3.2: instanceof Narrowing
// ---------------------------------

class ApiError extends Error {
  constructor(message: string, public code: number) {
    super(message);
  }
}

function handleError(error: Error | ApiError): string {
  if (error instanceof ApiError) {
    return `API Error ${error.code}: ${error.message}`;
  }
  return `Error: ${error.message}`;
}

// ============================================================================
// SECTION 4: MAPPING WITH PATTERNS
// ============================================================================

// Example 4.1: Record Transformation
// ----------------------------------

type TransformResult<T> = 
  | { status: "success"; data: T }
  | { status: "failure"; error: Error };

function mapSuccess<T, U>(result: TransformResult<T>, fn: (t: T) => U): TransformResult<U> {
  if (result.status === "success") {
    return { status: "success", data: fn(result.data) };
  }
  return { status: "failure", error: result.error };
}

// Example 4.2: Either Pattern (Functional)
// -------------------------------------

type Either<L, R> = 
  | { isLeft: true; left: L }
  | { isLeft: false; right: R };

function fromEither<L, R>(either: Either<L, R>): R | undefined {
  if (either.isLeft) {
    return undefined;
  }
  return either.right;
}

// ============================================================================
// SECTION 5: EXHAUSTIVE PATTERN HANDLING
// ============================================================================

// Example 5.1: Never Type for Exhaustiveness
// -----------------------------------------

type Color = "red" | "green" | "blue" | "yellow";

function getColorHex(color: Color): string {
  switch (color) {
    case "red": return "#FF0000";
    case "green": return "#00FF00";
    case "blue": return "#0000FF";
    case "yellow": return "#FFFF00";
    default: {
      const exhaustive: never = color;
      return exhaustive;
    }
  }
}

// Example 5.2: Const Assertion for Patterns
// --------------------------------------

const Direction = ["north", "south", "east", "west"] as const;
type Direction = typeof Direction[number];

function move(dir: Direction): void {
  switch (dir) {
    case "north": console.log("Moving north"); break;
    case "south": console.log("Moving south"); break;
    case "east": console.log("Moving east"); break;
    case "west": console.log("Moving west"); break;
  }
}

// ============================================================================
// PERFORMANCE
// ============================================================================

// Pattern matching in TypeScript has minimal runtime overhead
// The type system operates at compile time
// Switch statements are optimized by JavaScript engines
// For performance-critical code, consider:
// - Placing most common cases first in switch statements
// - Using hash maps for large numbers of cases
// - Avoiding deep recursion in pattern matching

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Supported in TypeScript 4.0+
// Requires ES2015+ for full feature support
// Works with all modern browsers and Node.js 12+
// Use tsconfig target: "ES2015" or higher

// ============================================================================
// SECURITY
// ============================================================================

// Pattern matching provides type safety guarantees
// Exhaustive checking prevents unhandled cases
// Use type guards to validate external input
// Avoid dynamic type checks that could be bypassed

// ============================================================================
// TESTING
// ============================================================================

// Test all branches of pattern matching
// Use parameterized tests for different variants
// Verify exhaustive checking with additional cases
// Test edge cases like empty unions

// ============================================================================
// DEBUGGING
// ============================================================================

// Enable strict mode for better type checking
// Use never type for exhaustive checking
// Add type annotations for complex patterns
// Check inferred types with hover tooltips

// ============================================================================
// ALTERNATIVE
// ============================================================================

// For simple cases, use if/else with type guards
// Consider union types with null for optional values
// Use function overloading for different parameter types

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related concepts:
// - 02_Exhaustive_Checking.ts - for more on exhaustiveness
// - 03_Never_Type_Analysis.ts - for never type details
// - 04_Flow_Sensitive_Types.ts - for flow analysis
// - 05_Control_Flow_Analysis.ts - for control flow

console.log("\n=== Pattern Matching Complete ===");
console.log("Next: 07_Advanced_Control_Flow/02_Exhaustive_Checking");