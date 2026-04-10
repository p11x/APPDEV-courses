/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 02_Type_Calculations
 * Concept: 02_Type_Comparison
 * Topic: 03_Equal_Types
 * Purpose: Implement type-level equality checking
 * Difficulty: expert
 * UseCase: type-level-programming
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Type-level equality uses structural comparison to determine
 * if two types are equal at compile time.
 */

type Equals<X, Y> = 
  (<T>() => T extends X ? 1 : 2) extends (<T>() => T extends Y ? 1 : 2) ? true : false;

type IsEqual<T, U> = 
  [T] extends [U] ? [U] extends [T] ? true : false : false;

type StrictEquals<T extends number, U extends number> = 
  [BuildTuple<T>, BuildTuple<U>] extends [infer A, infer B] 
    ? A extends B ? (B extends A ? true : false) : false 
    : false;

type BuildTuple<N extends number, T extends any[] = []> = 
  T["length"] extends N ? T : BuildTuple<N, [...T, any]>;

type TestEq1 = Equals<number, number>;
type TestEq2 = Equals<number, string>;
type TestEq3 = Equals<1, 1>;
type TestEq4 = Equals<1, 2>;

type StrictEq<T extends number, U extends number> = 
  T extends U ? (U extends T ? true : false) : false;

type TestStrict1 = StrictEq<5, 5>;
type TestStrict2 = StrictEq<5, 6>;

type IfEquals<T, U, Then, Else> = Equals<T, U> extends true ? Then : Else;

type Result = IfEquals<number, number, "equal", "not equal">;

console.log("\n=== Type Equality Demo ===");
type E1 = Equals<1, 1>;
type E2 = Equals<1, 2>;
type E3 = IsEqual<string, string>;
type E4 = IfEquals<5, 5, "yes", "no");
console.log("Type-level equality works!");

/**
 * CROSS-REFERENCE:
 * - 01_Greater_Than.ts - Greater than
 * - 02_Less_Than.ts - Less than
 */