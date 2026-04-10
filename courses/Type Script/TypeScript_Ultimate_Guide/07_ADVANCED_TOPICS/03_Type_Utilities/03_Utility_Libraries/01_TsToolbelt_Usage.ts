/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 03_Type_Utilities
 * Concept: 03_Utility_Libraries
 * Topic: 01_TsToolbelt_Usage
 * Purpose: Learn ts-toolbelt library utilities
 * Difficulty: advanced
 * UseCase: advanced-type-programming
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: ts-toolbelt is a type utility library providing advanced type
 * operations for complex TypeScript projects.
 */

type ObjectOf<T> = { [key: string]: T };

type Merge<Object extends object> = {
  [K in keyof Object]: Object[K];
};

type Compute<A extends any> = A extends Function ? A : { [K in keyof A]: A[K] };

type Intersection<A extends any, B extends any> = Compute<
  A extends any ? (A extends B ? A : never) : never
>;

type Unionize<A extends object> = A[keyof A];

type Values<A extends object> = A[keyof A];

type Optional<T extends object> = {
  [K in keyof T]?: T[K];
};

type Required<T extends object> = {
  [K in keyof T]-?: T[K];
};

type Readonly<T extends object> = {
  readonly [K in keyof T]: T[K];
};

type Writable<T extends object> = {
  -readonly [K in keyof T]: T[K];
};

type Nullable<T extends object> = {
  [K in keyof T]: T[K] | null;
};

type NonNullable<T> = T extends null | undefined ? never : T;

type Simplify<T> = T extends any ? { [K in keyof T]: T[K] } : never;

type Cast<X, Y> = X extends Y ? X : Y;

type Maybe<T, IsNullable extends boolean = false> = IsNullable extends true ? T | null : T | undefined | null;

type Head<T extends any[]> = T extends [infer H, ...any[]] ? H : never;

type Tail<T extends any[]> = T extends [any, ...infer R] ? R : [];

type Last<T extends any[]> = T extends [...any[], infer L] ? L : never;

type Length<T extends any[]> = T["length"];

type Append<T extends any[], U> = [...T, U];

type Prepend<T extends any[], U> = [U, ...T];

type Concat<T extends any[], U extends any[]> = [...T, ...U];

type Drop<N extends number, T extends any[], A extends any[] = []> = 
  A["length"] extends N ? T : T extends [infer H, ...infer R] ? Drop<N, R, [...A, H]> : [];

type Take<N extends number, T extends any[], A extends any[] = []> = 
  A["length"] extends N ? A : T extends [infer H, ...infer R] ? Take<N, R, [...A, H]> : [];

console.log("\n=== TsToolbelt Utilities Demo ===");
type M = Merge<{ a: 1 } & { b: 2 }>;
type H = Head<[1, 2, 3]>;
type L = Last<[1, 2, 3]>;
console.log("TsToolbelt-style utilities work!");

/**
 * COMPATIBILITY:
 * - Works with all TypeScript targets
 * - Available as npm package: ts-toolbelt
 * 
 * CROSS-REFERENCE:
 * - 02_TypeFest_Usage.ts - Another utility library
 * - 02_Custom_Utilities/01_Merge_Types.ts - Custom merge
 */