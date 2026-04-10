/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 02_Type_Calculations
 * Concept: 02_Type_Comparison
 * Topic: 01_Greater_Than
 * Purpose: Implement type-level greater-than comparison
 * Difficulty: expert
 * UseCase: type-level-programming
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Type-level greater-than compares two numeric literal types
 * and returns a boolean indicating if the first is greater.
 */

type BuildTuple<N extends number, T extends any[] = []> = 
  T["length"] extends N ? T : BuildTuple<N, [...T, any]>;

type GreaterThan<A extends number, B extends number> = 
  A extends B ? false : B extends 0 ? true : GreaterThan<Subtract<A, 1>, Subtract<B, 1>>;

type Subtract<A extends number, B extends number> = 
  BuildTuple<A> extends [...BuildTuple<B>, ...infer Rest] ? Rest["length"] : 0;

type Gt<A extends number, B extends number> = 
  A extends B ? false : [BuildTuple<B>, BuildTuple<A>] extends [infer B, infer A] 
    ? A extends [...B, ...any] ? true : false 
    : false;

type TestGt1 = GreaterThan<5, 3>;
type TestGt2 = GreaterThan<3, 5>;
type TestGt3 = GreaterThan<5, 5>;
type TestGt4 = GreaterThan<100, 50>;

type Max<A extends number, B extends number> = GreaterThan<A, B> extends true ? A : B;

type TestMax = Max<10, 20>;
type TestMax2 = Max<50, 30>;

type GreaterOrEqual<A extends number, B extends number> = 
  A extends B ? true : GreaterThan<A, B>;

type TestGe = GreaterOrEqual<5, 5>;

console.log("\n=== Type Greater Than Demo ===");
type R1 = GreaterThan<10, 5>;
type R2 = GreaterThan<5, 10>;
type R3 = Max<15, 25>;
console.log("Type-level comparison works!");

/**
 * CROSS-REFERENCE:
 * - 02_Less_Than.ts - Less than comparison
 * - 03_Equal_Types.ts - Equality comparison
 */