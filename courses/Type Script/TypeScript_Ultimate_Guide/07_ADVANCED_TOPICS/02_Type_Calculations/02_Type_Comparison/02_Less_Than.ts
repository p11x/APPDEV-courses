/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 02_Type_Calculations
 * Concept: 02_Type_Comparison
 * Topic: 02_Less_Than
 * Purpose: Implement type-level less-than comparison
 * Difficulty: expert
 * UseCase: type-level-programming
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Type-level less-than compares two numeric literal types
 * and returns a boolean indicating if the first is less.
 */

type BuildTuple<N extends number, T extends any[] = []> = 
  T["length"] extends N ? T : BuildTuple<N, [...T, any]>;

type LessThan<A extends number, B extends number> = 
  A extends B ? false : A extends 0 ? true : LessThan<Subtract<A, 1>, Subtract<B, 1>>;

type Subtract<A extends number, B extends number> = 
  BuildTuple<A> extends [...BuildTuple<B>, ...infer Rest] ? Rest["length"] : 0;

type Lt<A extends number, B extends number> = 
  A extends B ? false : [BuildTuple<A>, BuildTuple<B>] extends [infer A, infer B] 
    ? B extends [...A, ...any] ? true : false 
    : false;

type TestLt1 = LessThan<3, 5>;
type TestLt2 = LessThan<5, 3>;
type TestLt3 = LessThan<5, 5>;
type TestLt4 = LessThan<50, 100>;

type Min<A extends number, B extends number> = LessThan<A, B> extends true ? A : B;

type TestMin = Min<10, 20>;
type TestMin2 = Min<50, 30>;

type LessOrEqual<A extends number, B extends number> = 
  A extends B ? true : LessThan<A, B>;

type TestLe = LessOrEqual<5, 5>;

console.log("\n=== Type Less Than Demo ===");
type R1 = LessThan<5, 10>;
type R2 = LessThan<10, 5>;
type R3 = Min<15, 25>;
console.log("Type-level less-than works!");

/**
 * CROSS-REFERENCE:
 * - 01_Greater_Than.ts - Greater than comparison
 * - 03_Equal_Types.ts - Equality comparison
 */