/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 03_Type_Utilities
 * Concept: 02_Custom_Utilities
 * Topic: 02_Diff_Types
 * Purpose: Implement type difference utilities
 * Difficulty: intermediate
 * UseCase: type-transformations
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: Diff types calculate the difference between two types,
 * identifying properties that exist in one but not the other.
 */

type Diff<T, U> = {
  [P in Exclude<keyof T, keyof U>]: T[P];
};

type Intersection<T, U> = {
  [P in keyof (T & U)]: P extends keyof T ? T[P] : P extends keyof U ? U[P] : never;
};

type SymDiff<T, U> = Diff<T, U> & Diff<U, T>;

interface A {
  id: number;
  name: string;
  email: string;
}

interface B {
  id: number;
  name: string;
  age: number;
}

type AOnly = Diff<A, B>;
type BOnly = Diff<B, A>;
type Common = Intersection<A, B>;
type Both = SymDiff<A, B>;

type NestedDiff<T, U> = {
  [P in keyof T as P extends keyof U ? (U[P] extends T[P] ? never : P) : P]: T[P];
};

interface Config {
  apiUrl: string;
  timeout: number;
  retries: number;
  debug: boolean;
}

interface PartialConfig {
  apiUrl: string;
  timeout: number;
}

type NewConfig = Diff<Config, PartialConfig>;
type MissingConfig = Diff<PartialConfig, Config>;

console.log("\n=== Diff Types Demo ===");
type D1 = Diff<{ a: 1; b: 2 }, { a: 1 }>;
type I1 = Intersection<{ a: 1 }, { b: 2 }>;
console.log("Diff types work!");

/**
 * CROSS-REFERENCE:
 * - 01_Merge_Types.ts - Type merging
 * - 03_Omit_Deep.ts - Deep omit
 */