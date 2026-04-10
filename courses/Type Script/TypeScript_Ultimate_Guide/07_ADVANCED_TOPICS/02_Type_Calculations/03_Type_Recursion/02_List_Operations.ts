/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 02_Type_Calculations
 * Concept: 03_Type_Recursion
 * Topic: 02_List_Operations
 * Purpose: Implement list operations at type level
 * Difficulty: expert
 * UseCase: type-level-programming
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Type-level list operations manipulate tuple types using
 * recursive conditional types for transformations.
 */

type Prepend<T extends any[], U> = [U, ...T];

type Append<T extends any[], U> = [...T, U];

type Head<T extends any[]> = T extends [infer H, ...any[]] ? H : never;

type Tail<T extends any[]> = T extends [any, ...infer R] ? R : never;

type Last<T extends any[]> = T extends [...any[], infer L] ? L : never;

type Initial<T extends any[]> = T extends [...infer I, any] ? I : never;

type Length<T extends any[]> = T["length"];

type Reverse<T extends any[]> = T extends [infer F, ...infer R] ? [...Reverse<R>, F] : [];

type Concat<T extends any[], U extends any[]> = [...T, ...U];

type Drop<N extends number, T extends any[], R extends any[] = []> = 
  R["length"] extends N ? T : T extends [infer F, ...infer Rest] ? Drop<N, Rest, [...R, F]> : R;

type Take<N extends number, T extends any[], R extends any[] = []> = 
  R["length"] extends N ? R : T extends [infer F, ...infer Rest] ? Take<N, Rest, [...R, F]> : R;

type Flatten<T extends any[]> = T extends [infer F, ...infer R] ? 
  F extends any[] ? [...Flatten<F>, ...Flatten<R>] : [F, ...Flatten<R>] : [];

type Unique<T extends any[], Acc extends any[] = []> = 
  T extends [infer F, ...infer R] ? 
    F extends Acc[number] ? Unique<R, Acc> : Unique<R, [...Acc, F]> : Acc;

type Includes<T extends any[], U> = U extends T[number] ? true : false;

type Filter<T extends any[], U> = T extends [infer F, ...infer R] ? 
  F extends U ? [F, ...Filter<R, U>] : Filter<R, U> : [];

type Map<T extends any[], F> = T extends [infer H, ...infer R] ? [F[H], ...Map<R, F>] : [];

type Zip<T extends any[], U extends any[]> = 
  T extends [infer HT, ...infer RT] ? 
    U extends [infer HU, ...infer RU] ? 
      [[HT, HU], ...Zip<RT, RU>] : [] : [];

type IndexOf<T extends any[], U> = 
  T extends [infer H, ...infer R] ? 
    H extends U ? 0 : IndexOf<R, U> extends number ? Add<IndexOf<R, U>, 1> : -1 : -1;

type Add<A extends number, B extends number> = [...BuildTuple<A>, ...BuildTuple<B>]["length"];

type BuildTuple<N extends number, T extends any[] = []> = 
  T["length"] extends N ? T : BuildTuple<N, [...T, any]>;

type TestList = [1, 2, 3, 4, 5];
type TestHead = Head<TestList>;
type TestTail = Tail<TestList>;
type TestLast = Last<TestList>;
type TestRev = Reverse<TestList>;
type TestConcat = Concat<[1, 2], [3, 4]>;
type TestTake = Take<3, TestList>;
type TestDrop = Drop<2, TestList>;
type TestFlat = Flatten<[[1, 2], [3, 4]]>;
type TestUnique = Unique<[1, 2, 1, 3, 2]>;

console.log("\n=== List Operations Demo ===");
type H = Head<[1, 2, 3]>;
type L = Last<[1, 2, 3]>;
type R = Reverse<[1, 2, 3]>;
console.log("Type-level list operations work!");

/**
 * PERFORMANCE:
 * - Linear time for most operations
 * - Recursion depth can impact compile time
 * 
 * CROSS-REFERENCE:
 * - 01_Fibonacci_Types.ts - Recursive types
 * - 01_Type_Arithmetic/01_Add_Types.ts - Addition
 */