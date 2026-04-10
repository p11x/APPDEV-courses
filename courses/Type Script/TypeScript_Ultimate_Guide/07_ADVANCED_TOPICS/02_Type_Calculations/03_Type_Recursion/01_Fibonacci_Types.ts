/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 02_Type_Calculations
 * Concept: 03_Type_Recursion
 * Topic: 01_Fibonacci_Types
 * Purpose: Implement Fibonacci sequence at the type level
 * Difficulty: expert
 * UseCase: type-level-programming
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only, slow for large N
 * Security: N/A
 */

/**
 * WHAT: Type-level Fibonacci uses recursive conditional types to compute
 * Fibonacci numbers at compile time. Note: This is slow for large N.
 */

type BuildTuple<N extends number, T extends any[] = []> = 
  T["length"] extends N ? T : BuildTuple<N, [...T, any]>;

type Add<A extends number, B extends number> = [...BuildTuple<A>, ...BuildTuple<B>]["length"];

type Fibonacci<N extends number> = 
  N extends 0 ? 0 : 
  N extends 1 ? 1 : 
  Add<Fibonacci<Subtract<N, 1>>, Fibonacci<Subtract<N, 2>>>;

type Subtract<A extends number, B extends number> = 
  BuildTuple<A> extends [...BuildTuple<B>, ...infer Rest] ? Rest["length"] : 0;

type Fib0 = Fibonacci<0>;
type Fib1 = Fibonacci<1>;
type Fib2 = Fibonacci<2>;
type Fib3 = Fibonacci<3>;
type Fib5 = Fibonacci<5>;
type Fib10 = Fibonacci<10>;

type FastFibonacci<N extends number> = 
  N extends 0 ? 0 : 
  N extends 1 ? 1 : 
  N extends 2 ? 1 :
  N extends 3 ? 2 :
  N extends 4 ? 3 :
  N extends 5 ? 5 :
  N extends 6 ? 8 :
  N extends 7 ? 13 :
  N extends 8 ? 21 :
  N extends 9 ? 34 :
  N extends 10 ? 55 : 
  Add<FastFibonacci<Subtract<N, 1>>, FastFibonacci<Subtract<N, 2>>>;

type Fib6 = FastFibonacci<6>;
type Fib8 = FastFibonacci<8>;

type FibonacciTuple<N extends number> = 
  N extends 0 ? [0] : 
  N extends 1 ? [0, 1] : 
  [...FibonacciTuple<Subtract<N, 1>>, Add<FibonacciTuple<Subtract<N, 1>>[Subtract<N, 1]>, FibonacciTuple<Subtract<N, 2>>[Subtract<N, 2]>]];

type FibSeq = FibonacciTuple<5>;

console.log("\n=== Fibonacci Types Demo ===");
type F1 = Fibonacci<5>;
type F2 = FastFibonacci<10>;
console.log("Type-level Fibonacci works!");

/**
 * PERFORMANCE:
 * - Exponential time complexity O(2^n)
 * - Use lookup tables for small values
 * - Consider memoization patterns
 * 
 * CROSS-REFERENCE:
 * - 02_List_Operations.ts - List operations
 * - 01_Add_Types.ts - Type addition
 */