/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: Variadic_Tuple_Types
 * Purpose: Variadic tuple types in TypeScript
 * Difficulty: expert
 * UseCase: web, backend
 */

/**
 * Variadic Tuple Types - Comprehensive Guide
 * ======================================
 * 
 * 📚 WHAT: Spreading types in tuple types
 * 💡 WHERE: Advanced TypeScript type manipulation
 * 🔧 HOW: Variadic spread, tuple concatenation
 */

// ============================================================================
// SECTION 1: VARIADIC TUPLE BASICS
// ============================================================================

// Example 1.1: Basic Variadic Spread
// -----------------------

type Flatten<T extends any[]> = T[number];

type StringTuple = string[];
type Flattened = Flatten<StringTuple>;
// string

// Example 1.2: Spread in Tuples
// ---------------------------------

type Prepend<T extends any[], E> = [E, ...T];

type Before = ["b", "c"];
type After = Prepend<Before, "a">;
// ["a", "b", "c"]

// ============================================================================
// SECTION 2: TUPLE CONCATENATION
// ============================================================================

// Example 2.1: Concatenate Tuples
// ---------------------------------

type Concat<T extends any[], U extends any[]> = [...T, ...U];

type First = ["a", "b"];
type Second = ["c", "d"];
type Combined = Concat<First, Second>;
// ["a", "b", "c", "d"]

// ============================================================================
// SECTION 3: REAL-WORLD EXAMPLES
// ============================================================================

// Example 3.1: Parameter List
// ---------------------------------

type Params<T extends any[]> = T;

type MyParams = Params<[string, number, boolean]>;
// [string, number, boolean]

// Example 3.2: Return Type Builder
// ---------------------------------

function createTuple<T extends any[]>(...args: T): T {
  return args;
}

const tuple = createTuple(1, "a", true);
// (string | number | boolean)[]

console.log("\n=== Variadic Tuple Types Complete ===");