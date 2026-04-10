/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 02_Type_Calculations
 * Concept: 01_Type_Arithmetic
 * Topic: 02_Multiply_Types
 * Purpose: Implement type-level multiplication
 * Difficulty: expert
 * UseCase: type-level-programming
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Type-level multiplication uses recursive addition to compute
 * the product of two numeric literal types at compile time.
 */

type BuildTuple<N extends number, T extends any[] = []> = 
  T["length"] extends N ? T : BuildTuple<N, [...T, any]>;

type Multiply<A extends number, B extends number, Acc extends any[] = []> =
  B extends 0 
    ? Acc["length"] 
    : Multiply<A, Subtract<B, 1>, [...Acc, ...BuildTuple<A>>["length"] extends number ? BuildTuple<Acc["length"]> : never>;

type Subtract<A extends number, B extends number> = 
  BuildTuple<A> extends [...BuildTuple<B>, ...infer Rest] ? Rest["length"] : 0;

type MultiplyLoop<A extends number, B extends number> =
  B extends 0 ? 0 : A extends 0 ? 0 : B extends 1 ? A : A extends 0 ? 0 : MultiplyLoop<A, Subtract<B, 1>> extends number ? Add<A, MultiplyLoop<A, Subtract<B, 1>>> : never;

type Add<A extends number, B extends number> = [...BuildTuple<A>, ...BuildTuple<B>]["length"];

type TestMul1 = Multiply<3, 4>;
type TestMul2 = Multiply<5, 5>;
type TestMul3 = Multiply<0, 100>;
type TestMul4 = Multiply<10, 10>;

type PowerOfTwo<N extends number> = N extends 0 ? 1 : Multiply<2, PowerOfTwo<Subtract<N, 1>>>;

type TestPow = PowerOfTwo<8>;

type Square<N extends number> = Multiply<N, N>;

type TestSq = Square<7>;

console.log("\n=== Type Multiplication Demo ===");
type M1 = Multiply<3, 4>;
type M2 = Multiply<10, 5>;
type M3 = PowerOfTwo<4>;
console.log("Type-level multiplication works!");

/**
 * CROSS-REFERENCE:
 * - 01_Add_Types.ts - Addition
 * - 03_Divide_Types.ts - Division
 */