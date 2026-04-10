/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 03_Type_Utilities
 * Concept: 02_Custom_Utilities
 * Topic: 01_Merge_Types
 * Purpose: Implement custom type merging utilities
 * Difficulty: intermediate
 * UseCase: type-transformations
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Merge types combines multiple types into one, with later types
 * taking precedence over earlier ones for overlapping properties.
 */

type Merge<T, U> = {
  [P in keyof (T & U)]: P extends keyof U ? U[P] : P extends keyof T ? T[P] : never;
};

type DeepMerge<T, U> = {
  [P in keyof (T & U)]: P extends keyof U 
    ? U[P] extends object 
      ? P extends keyof T 
        ? T[P] extends object 
          ? DeepMerge<T[P], U[P]> 
          : U[P] 
        : U[P] 
      : U[P] 
    : P extends keyof T 
      ? T[P] 
      : never;
};

interface Config {
  debug: boolean;
  timeout: number;
  retry: number;
}

interface Override {
  debug: boolean;
  timeout: number;
}

type MergedConfig = Merge<Config, Override>;
type DeepMerged = DeepMerge<Config, Override>;

type Assign<T extends any[], Acc = {}> = 
  T extends [infer F, ...infer R] ? Assign<R, Acc & F> : Acc;

type Combined = Assign<[Config, Override, { debug: true }]>;

type Overwrite<T, U> = { [P in Exclude<keyof T, keyof U>]: T[P] } & U;

type MergeAll<T extends any[]> = T extends [infer F, ...infer R] ? F & MergeAll<R> : {};

type FlatMerge<T extends any[]> = {
  [K in keyof T[number]]: K extends keyof T[number] ? T[number][K] : never;
};

console.log("\n=== Merge Types Demo ===");
type M = Merge<{ a: 1 }, { b: 2 }>;
type D = DeepMerge<{ x: { a: 1 } }, { x: { b: 2 } }>;
console.log("Merge types works!");

/**
 * CROSS-REFERENCE:
 * - 02_Diff_Types.ts - Type difference
 * - 03_Omit_Deep.ts - Deep omit
 */