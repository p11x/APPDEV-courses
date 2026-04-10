/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 07
 * Topic: Flow_Sensitive_Types
 * Purpose: Understanding type flow analysis and narrowing
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 * Version: TS 3.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Flow Sensitive Types - Comprehensive Guide
 * =======================================
 * 
 * 📚 WHAT: Type narrowing based on control flow analysis
 * 💡 WHY: Enables type safety without explicit type assertions
 * 🔧 HOW: Type guards, conditional logic, assignment analysis
 */

// ============================================================================
// SECTION 1: TYPE GUARD NARROWING
// ============================================================================

// Example 1.1: typeof Narrowing
// --------------------------

function narrowBytypeof(value: string | number): number {
  if (typeof value === "string") {
    return value.length;
  }
  return value * 2;
}

// Example 1.2: instanceof Narrowing
// ---------------------------

class Animal { speak(): void { } }
class Dog extends Animal { breed: string; }

function handleAnimal(animal: Animal | string): void {
  if (animal instanceof Animal) {
    animal.speak();
  } else {
    console.log(animal.toUpperCase());
  }
}

// ============================================================================
// SECTION 2: IN OPERATOR NARROWING
// ============================================================================

// Example 2.1: Property Check Narrowing
// ---------------------------------

interface Fish { swim(): void; }
interface Bird { fly(): void; }

function moveAnimal(animal: Fish | Bird): void {
  if ("swim" in animal) {
    animal.swim();
  } else {
    animal.fly();
  }
}

// ============================================================================
// SECTION 3: TRUTHINESS NARROWING
// ============================================================================

// Example 3.1: Null/Undefined Narrowing
// -------------------------------

function processValue(value: string | null): number {
  if (value) {
    return value.length;
  }
  return 0;
}

// ============================================================================
// SECTION 4: ASSIGNMENT NARROWING
// ============================================================================

// Example 4.1: Variable Reassignment
// ------------------------------

function assignNarrowing(value: string | number): string {
  if (typeof value === "string") {
    return value;
  }
  value = String(value);
  return value;
}

// ============================================================================
// SECTION 5: CUSTOM TYPE GUARDS
// ============================================================================

// Example 5.1: is Type Guard
// ------------------------

interface Config {
  debug?: boolean;
}

function isDebug(config: Config): config is { debug: boolean } {
  return config.debug !== undefined;
}

function handleConfig(config: Config): void {
  if (isDebug(config)) {
    console.log(config.debug);
  }
}

// ============================================================================
// PERFORMANCE
// ============================================================================

// Flow analysis is compile-time only
// No runtime overhead
// Improves with better TS version

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 3.0+ for full support
// All targets

// ============================================================================
// SECURITY
// ============================================================================

// Reduces type assertion errors
// Improves type safety

// ============================================================================
// TESTING
// ============================================================================

// Test all code paths
// Verify type narrowing

// ============================================================================
// DEBUGGING
// ============================================================================

// Use hover for types
// Check inferred types

// ============================================================================
// ALTERNATIVE
// ============================================================================

// Use explicit type assertions
// Use type guards library

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 01_Pattern_Matching, 05_Control_Flow_Analysis

console.log("\n=== Flow Sensitive Types Complete ===");
console.log("Next: 07_Advanced_Control_Flow/05_Control_Flow_Analysis");