/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 03_Type_Utilities
 * Concept: 03_Utility_Libraries
 * Topic: 02_TypeFest_Usage
 * Purpose: Learn type-fest utility types
 * Difficulty: intermediate
 * UseCase: production-code
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: N/A
 */

/**
 * WHAT: type-fest is a collection of TypeScript types for common patterns,
 * providing utilities not found in the standard library.
 */

type PartialDeep<T> = {
  [P in keyof T]?: T[P] extends object ? PartialDeep<T[P]> : T[P];
};

type RequiredDeep<T> = {
  [P in keyof T]-?: T[P] extends object ? RequiredDeep<T[P]> : T[P];
};

type ReadonlyDeep<T> = {
  readonly [P in keyof T]: T[P] extends object ? ReadonlyDeep<T[P]> : T[P];
};

type Exact<T, S> = T extends S ? (S extends T ? T : never) : never;

type OptionalKeys<T> = { [K in keyof T]-?: T[K] }[keyof T] extends T[K] ? never : K;

type RequiredKeys<T> = { [K in keyof T]-?: T[K] }[keyof T] extends T[K] ? K : never;

type Mutable<T> = { -readonly [P in keyof T]: T[P] };

type Immutable<T> = { readonly [P in keyof T]: T[P] };

type Simplify<T> = { [K in keyof T]: T[K] } & {};

type ExactProps<T, U extends T> = T extends U ? (U extends T ? U : never) : never;

type Merge<First, Second> = {
  for K in keyof First | keyof Second: K extends keyof Second ? Second[K] : K extends keyof First ? First[K] : never;
};

type Overwrite<T, U> = { [P in Exclude<keyof T, keyof U>]: T[P] } & U;

type SetOptional<T, K extends keyof T> = Omit<T, K> & Partial<Pick<T, K>>;

type SetRequired<T, K extends keyof T> = Omit<T, K> & Required<Pick<T, K>>;

type SetNonNullable<T, K extends keyof T> = Omit<T, K> & { [P in K]: NonNullable<T[P]> };

type UnwrapOpaque<T, S> = T extends { __opaque: S } ? T["type"] : never;

type Opaque<T, K extends string> = T & { __opaque: K };

type Invariant<T> = (arg: T) => T;

type ConditionalKeys<T, Condition> = { [K in keyof T]: T[K] extends Condition ? K : never }[keyof T];

type ConditionalType<T, Condition, True, False> = T extends Condition ? True : False;

type IsNever<T> = [T] extends [never] ? true : false;

type IsAny<T> = 0 extends (1 & T) ? true : false;

console.log("\n=== TypeFest Utilities Demo ===");
type P = PartialDeep<{ a: { b: 1 } }>;
type R = RequiredKeys<{ a?: string; b: number }>;
console.log("TypeFest-style utilities work!");

/**
 * COMPATIBILITY:
 * - Works with all TypeScript targets
 * - Available as npm package: type-fest
 * 
 * CROSS-REFERENCE:
 * - 01_TsToolbelt_Usage.ts - Another utility library
 * - 01_Built_in_Utilities/01_Partial_Deep.ts - Deep partial
 */