/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: Conditional_and_Mapped_Types
 * Purpose: Conditional and mapped types in TypeScript
 * Difficulty: expert
 * UseCase: web, backend
 */

/**
 * Conditional Types - Comprehensive Guide
 * ===================================
 * 
 * 📚 WHAT: Type-level conditional logic
 * 💡 WHERE: Advanced TypeScript patterns
 * 🔧 HOW: Conditional type inference, distributive types
 */

// ============================================================================
// SECTION 1: BASIC CONDITIONAL TYPES
// ============================================================================

// Example 1.1: Simple Conditional
// -----------------------

type IsString<T> = T extends string ? true : false;

type Test1 = IsString<string>;  // true
type Test2 = IsString<number>; // false

// Example 1.2: Conditional with Union
// ---------------------------------

type ToArray<T> = T extends any ? T[] : never;

type UnionArray = ToArray<string | number>;
// string[] | number[] (distributes over union)

// ============================================================================
// SECTION 2: INFER KEYWORD
// ============================================================================

// Example 2.1: Return Type Inference
// -----------------------

type MyReturnType<T> = T extends (...args: any[]) => infer R ? R : never;

type F = () => string;
type R = MyReturnType<F>; // string

// Example 2.2: Parameter Inference
// ---------------------------------

type MyParameters<T> = T extends (...args: infer P) => any ? P : never;

type P = MyParameters<(a: string, b: number) => void>;
// [string, number]

// ============================================================================
// SECTION 3: MAPPED TYPES
// ============================================================================

// Example 3.1: Basic Mapped Type
// ---------------------------------

type KeysToUppercase<T> = {
  [K in keyof T]: string;
};

type Original = { name: string; age: number };
type Uppercased = KeysToUppercase<Original>;
// { name: string; age: string }

// Example 3.2: Mapped with Modifiers
// ---------------------------------

type MakeOptional<T> = {
  [K in keyof T]?: T[K];
};

type MakeReadonly<T> = {
  readonly [K in keyof T]: T[K];
};

// ============================================================================
// SECTION 4: KEY REMAPPING
// ============================================================================

// Example 4.1: Key Transformation
// ---------------------------------

type PrefixKeys<T, P extends string> = {
  [K in keyof T as `${P}${K & string}`]: T[K];
};

type Prefixed = PrefixKeys<{ a: number }, "get">;
// { geta: number }

// ============================================================================
// SECTION 5: PRACTICAL EXAMPLES
// ============================================================================

// Example 5.1: Extract Return Type
// -----------------------

type GetReturnType<T> = T extends (x: infer R) => any ? R : never;

// Example 5.2: Extract Parameter
// ---------------------------------

type GetParameter<T> = T extends (x: infer P) => any ? P : never;

console.log("\n=== Conditional and Mapped Types Complete ===");
console.log("Next: EXPERIMENTAL/EXPERIMENTAL_FEATURES/03_Recursive_and_Distributive_Types/01_Recursive_Types.ts");