/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 02_Type_Calculations
 * Concept: 01_Type_Arithmetic
 * Topic: 01_Add_Types
 * Purpose: Implement type-level addition
 * Difficulty: expert
 * UseCase: type-level-programming
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Type-level addition uses recursive conditional types to compute
 * the sum of two numeric literal types at compile time.
 */

type Digit = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9;

type Add<A extends number, B extends number> = 
  [...BuildTuple<A>, ...BuildTuple<B>]["length"];

type BuildTuple<N extends number, T extends any[] = []> = 
  T["length"] extends N ? T : BuildTuple<N, [...T, any]>;

type Test1 = Add<2, 3>;
type Test2 = Add<0, 5>;
type Test3 = Add<100, 200>;

type AddPositive<A extends number, B extends number> = 
  A extends 0 ? B : B extends 0 ? A : Add<A, B>;

type AddArrays<A extends any[], B extends any[]> = [...A, ...B]["length"];

type Increment<N extends number> = Add<N, 1>;
type Decrement<N extends number> = N extends 0 ? 0 : BuildTuple<N> extends [...infer H, any] ? H["length"] : 0;

type TestInc = Increment<5>;
type TestDec = Decrement<5>;

type AddStrings<A extends string, B extends string> = 
  `${A}${B}` extends `${infer N extends number}` ? N : never;

type Sum<A extends number, B extends number, Acc extends number = 0> =
  A extends 0 ? Add<Acc, B> : Sum<Subtract<A, 1>, B, Increment<Acc>>;

type Subtract<A extends number, B extends number> = 
  BuildTuple<A> extends [...BuildTuple<B>, ...infer Rest] ? Rest["length"] : 0;

type TestSub = Subtract<10, 3>;

console.log("\n=== Type Addition Demo ===");
type R1 = Add<2, 3>;
type R2 = Add<100, 200>;
type R3 = Increment<10>;
type R4 = Decrement<10>;
console.log("Type-level addition works!");

/**
 * PERFORMANCE:
 * - Recursion depth affects compilation time
 * - Large numbers slow down type inference
 * 
 * COMPATIBILITY:
 * - TS 4.0+ for infer in constraints
 * - Works with all targets
 * 
 * CROSS-REFERENCE:
 * - 02_Multiply_Types.ts - Multiplication
 * - 03_Divide_Types.ts - Division
 */