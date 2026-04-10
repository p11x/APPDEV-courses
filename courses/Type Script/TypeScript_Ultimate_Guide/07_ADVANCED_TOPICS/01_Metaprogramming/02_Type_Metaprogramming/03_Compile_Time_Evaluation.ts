/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 02_Type_Metaprogramming
 * Topic: 03_Compile_Time_Evaluation
 * Purpose: Explore compile-time type-level computations
 * Difficulty: expert
 * UseCase: advanced-type-engineering
 * Version: TS 4.7+
 * Performance: Varies by complexity
 * Security: No runtime concerns
 */

/**
 * WHAT: Compile-time evaluation (CTFE) uses TypeScript's type system to perform
 * computations at compile time, enabling powerful type-level programming.
 */

type Equals<X, Y> = (<T>() => T extends X ? 1 : 2) extends (<T>() => T extends Y ? 1 : 2) ? true : false;

type AssertEqual<T, U> = Equals<T, U> extends true ? T : never;

type If<Condition extends boolean, Then, Else> = Condition extends true ? Then : Else;

type IsNever<T> = [T] extends [never] ? true : false;

type IsAny<T> = 0 extends (1 & T) ? true : false;

type IsEqual<T, U> = (<G>() => G extends T ? 1 : 2) extends (<G>() => G extends U ? 1 : 2) ? true : false;

type TupleLength<T extends any[]> = T["length"];

type Reverse<T extends any[]> = T extends [infer F, ...infer R] ? [...Reverse<R>, F] : [];

type Head<T extends any[]> = T extends [infer H, ...any[]] ? H : never;

type Tail<T extends any[]> = T extends [any, ...infer R] ? R : never;

type DropFirst<T extends any[], N extends number = 1> = 
  N extends 0 ? T : T extends [any, ...infer R] ? DropFirst<R, N extends 1 ? 0 : Subtract<N, 1>> : never;

type Subtract<M extends number, N extends number> = 
  M extends N ? 0 : M extends 0 ? -N : M extends 1 ? (N extends 1 ? 0 : Subtract<M, Subtract<N, 1>>) : never;

type GreaterThan<A extends number, B extends number> = 
  A extends B ? false : (B extends A ? false : true);

type LessThan<A extends number, B extends number> = 
  Equals<A, B> extends true ? false : GreaterThan<B, A>;

type Includes<T extends readonly any[], U> = U extends T[number] ? true : false;

type Unique<T extends any[], Acc extends any[] = []> = 
  T extends [infer F, ...infer R] ? 
    F extends Acc[number] ? Unique<R, Acc> : Unique<R, [...Acc, F]> : Acc;

type Flatten<T> = T extends any[] ? T[number] : T;

type UnionToTuple<U> = UnionToIntersection<U extends any ? (f: (x: U) => void) => void : never> extends (f: infer F) => void 
  ? F extends (x: infer X) => void ? X : never 
  : never;

type AllKeys<T> = T extends any ? keyof T : never;

type MergeAll<T extends any[]> = { [K in AllKeys<T>]: Flatten<{ [I in keyof T]: K extends keyof T[I] ? T[I][K] : never }> };

type RequireKeys<T, K extends keyof T> = T & { [P in K]-?: T[P] };

type OptionalKeys<T> = { [K in keyof T]: T[K] extends Required<T>[K] ? never : K }[keyof T];

type RequiredKeys<T> = { [K in keyof T]: T[K] extends Optional<T>[K] ? K : never }[keyof T];

console.log("\n=== Compile-Time Evaluation Demo ===");
type Check1 = AssertEqual<Head<[1, 2, 3]>, 1>;
type Check2 = AssertEqual<TupleLength<[1, 2, 3]>, 3>;
type Check3 = Unique<[1, 2, 2, 3, 1, 3]>;
console.log("CTFE compiled successfully!");

console.log("\n=== Type Equality ===");
type Eq1 = Equals<number, number>;
type Eq2 = Equals<number, string>;
console.log("Equals<number, number>:", Eq1);
console.log("Equals<number, string>:", Eq2);