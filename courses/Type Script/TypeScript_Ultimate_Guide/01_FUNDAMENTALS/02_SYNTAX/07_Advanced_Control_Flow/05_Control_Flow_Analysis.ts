/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 07
 * Topic: Control_Flow_Analysis
 * Purpose: Deep dive into TypeScript control flow analysis
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 * Version: TS 3.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Control Flow Analysis - Comprehensive Guide
 * ============================================
 * 
 * 📚 WHAT: How TypeScript analyzes code flow for type narrowing
 * 💡 WHY: Enables precise type inference without annotations
 * 🔧 HOW: Statement scanning, branch analysis, type guards
 */

// ============================================================================
// SECTION 1: BRANCH ANALYSIS
// ============================================================================

// Example 1.1: If-Else Branch Narrowing
// ----------------------------------

function branchNarrow(value: string | number | null): void {
  if (value === null) {
    console.log("null");
  } else if (typeof value === "string") {
    console.log(value.toUpperCase());
  } else {
    console.log(value.toFixed(2));
  }
}

// ============================================================================
// SECTION 2: ASSERTION ANALYSIS
// ============================================================================

// Example 2.1: Function Assertions
// ---------------------------------

function assertString(value: unknown): asserts value is string {
  if (typeof value !== "string") {
    throw new Error("Not a string");
  }
}

// ============================================================================
// SECTION 3: CONDITIONAL TYPE FLOW
// ============================================================================

// Example 3.1: Ternary Type Narrowing
// --------------------------------

function ternaryNarrow(value: string | number): string {
  return typeof value === "string" ? value.toUpperCase() : String(value);
}

// ============================================================================
// SECTION 4: ALIASED TYPES
// ============================================================================

// Example 4.1: Type Alias Narrowing
// --------------------------------

type Maybe<T> = T | null;

function aliasNarrow(value: Maybe<string>): void {
  if (value !== null) {
    console.log(value.toUpperCase());
  }
}

// ============================================================================
// SECTION 5: COMPLEX FLOW SCENARIOS
// ============================================================================

// Example 5.1: Short-circuit Evaluation
// -----------------------------------

function shortCircuit(value: string | null): void {
  value && console.log(value.length);
}

// ============================================================================
// PERFORMANCE
// ============================================================================

// Flow analysis runs at compile time
// No runtime impact

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 3.0+
// All targets

// ============================================================================
// SECURITY
// ============================================================================

// Improves type safety
// Reduces assertions

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 04_Flow_Sensitive_Types

console.log("\n=== Control Flow Analysis Complete ===");
console.log("Next: 07_Advanced_Control_Flow/06_Assertion_Functions");