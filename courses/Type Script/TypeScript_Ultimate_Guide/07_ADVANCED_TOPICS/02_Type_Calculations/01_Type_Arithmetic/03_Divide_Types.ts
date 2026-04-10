/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 02_Type_Calculations
 * Concept: 01_Type_Arithmetic
 * Topic: 03_Divide_Types
 * Purpose: Implement type-level division
 * Difficulty: expert
 * UseCase: type-level-programming
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Type-level division uses recursive subtraction to compute
 * the quotient of two numeric literal types at compile time.
 */

type BuildTuple<N extends number, T extends any[] = []> = 
  T["length"] extends N ? T : BuildTuple<N, [...T, any]>;

type Subtract<A extends number, B extends number> = 
  BuildTuple<A> extends [...BuildTuple<B>, ...infer Rest] ? Rest["length"] : 0;

type Divide<A extends number, B extends number, Acc extends number = 0> =
  A extends 0 ? Acc : Divide<Subtract<A, B>, B, Increment<Acc>>;

type Increment<N extends number> = [...BuildTuple<N>, any]["length"];

type IsDivisible<A extends number, B extends number> = 
  A extends 0 ? true : Subtract<A, B> extends 0 ? true : Subtract<A, B> extends infer R ? R extends 0 ? false : IsDivisible<R, B> : false;

type Mod<A extends number, B extends number> = 
  A extends 0 ? 0 : Subtract<A, B> extends 0 ? 0 : Subtract<A, B> extends infer R ? Mod<R, B> : never;

type TestDiv1 = Divide<12, 3>;
type TestDiv2 = Divide<10, 2>;
type TestDiv3 = Divide<100, 10>;

type TestMod = Mod<10, 3>;
type TestMod2 = Mod<7, 2>;

type IsEven<N extends number> = Mod<N, 2> extends 0 ? true : false;
type IsOdd<N extends number> = Mod<N, 2> extends 1 ? true : false;

type TestEven = IsEven<4>;
type TestOdd = IsOdd<5>;

console.log("\n=== Type Division Demo ===");
type D1 = Divide<15, 3>;
type D2 = Divide<20, 4>;
type M1 = Mod<10, 3>;
type E1 = IsEven<6>;
console.log("Type-level division works!");

/**
 * CROSS-REFERENCE:
 * - 01_Add_Types.ts - Addition
 * - 02_Multiply_Types.ts - Multiplication
 */