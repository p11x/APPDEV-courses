/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 07
 * Topic: Assertion_Functions
 * Purpose: Custom type assertions and assertion functions
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 * Version: TS 3.8+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Assertion Functions - Comprehensive Guide
 * ================================
 * 
 * 📚 WHAT: Custom functions that assert types at runtime
 * 💡 WHY: Enables type narrowing without type guards
 * 🔧 HOW: asserts syntax, type predicates
 */

// ============================================================================
// SECTION 1: ASSERTION SYNTAX
// ============================================================================

// Example 1.1: Basic Assertion Function
// -------------------------------

function assertValue(value: unknown): asserts value {
  if (!value) {
    throw new Error("Value is required");
  }
}

// ============================================================================
// SECTION 2: TYPE PREDICATES
// ============================================================================

// Example 2.1: is Type Predicates
// -----------------------------

function isString(value: unknown): value is string {
  return typeof value === "string";
}

function isNumber(value: unknown): value is number {
  return typeof value === "number";
}

// ============================================================================
// SECTION 3: COMPOUND ASSERTIONS
// ============================================================================

// Example 3.1: Combined Assertions
// -------------------------------

function assertNonNull<T>(value: T): asserts value is NonNullable<T> {
  if (value === null || value === undefined) {
    throw new Error("Value cannot be null or undefined");
  }
}

// ============================================================================
// SECTION 4: COMPLEX TYPE ASSERTIONS
// ============================================================================

// Example 4.1: Interface Assertions
// -----------------------------

interface Config {
  debug: boolean;
}

function isConfig(value: unknown): value is Config {
  return typeof value === "object" && value !== null && "debug" in value;
}

// ============================================================================
// SECTION 5: PRACTICAL USES
// ============================================================================

// Example 5.1: Runtime Validation
// ---------------------------

function validateStringArray(value: unknown): asserts value is string[] {
  if (!Array.isArray(value)) {
    throw new Error("Not an array");
  }
  if (!value.every(item => typeof item === "string")) {
    throw new Error("Not all items are strings");
  }
}

// ============================================================================
// PERFORMANCE
// ============================================================================

// Runtime assertions add overhead
// Use only in development

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 3.8+
// All targets

// ============================================================================
// SECURITY
// ============================================================================

// Runtime validation
// Input sanitization

// ============================================================================
// TESTING
// ============================================================================

// Test assertion functions
// Verify error handling

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 04_Flow_Sensitive_Types

console.log("\n=== Assertion Functions Complete ===");
console.log("Next: 07_Advanced_Control_Flow/07_Type_Safe_Error_Handling");