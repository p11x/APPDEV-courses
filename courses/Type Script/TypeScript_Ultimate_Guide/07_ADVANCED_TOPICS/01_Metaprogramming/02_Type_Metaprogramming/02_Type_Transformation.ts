/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 02_Type_Metaprogramming
 * Topic: 02_Type_Transformation
 * Purpose: Master type transformation patterns
 * Difficulty: advanced
 * UseCase: type-safe-apis
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only
 * Security: No runtime impact
 */

/**
 * WHAT: Type transformation converts one type into another using conditional types,
 * mapped types, and template literal types.
 */

type Writable<T> = { -readonly [P in keyof T]: T[P] };
type Readonly<T> = { readonly [P in keyof T]: T[P] };

interface Config {
  readonly apiUrl: string;
  readonly timeout: number;
  debug: boolean;
}

type MutableConfig = Writable<Config>;
type DeepWritable<T> = { -readonly [P in keyof T]: T[P] extends object ? DeepWritable<T[P]> : T[P] };

type Optional<T> = { [P in keyof T]?: T[P] };
type Required<T> = { [P in keyof T]-?: T[P] };

type Nullable<T> = { [P in keyof T]: T[P] | null };

type Pick<T, K extends keyof T> = { [P in K]: T[P] };
type Omit<T, K extends keyof T> = { [P in Exclude<keyof T, K>]: T[P] };

type Extract<T, U> = T extends U ? T : never;
type Exclude<T, U> = T extends U ? never : T;

type NonNullable<T> = T extends null | undefined ? never : T;

type ExtractKeys<T, U> = { [P in keyof T]: T[P] extends U ? P : never }[keyof T];
type KeysMatching<T, U> = Extract<keyof T, ExtractKeys<T, U>>;

type PartialDeep<T> = { [P in keyof T]?: T[P] extends object ? PartialDeep<T[P]> : T[P] };

type Merge<T, U> = { [P in keyof (T & U)]: P extends keyof U ? U[P] : P extends keyof T ? T[P] : never };

interface A { x: number; y: string; }
interface B { y: number; z: boolean; }
type Merged = Merge<A, B>;

type Overwrite<T, U> = { [P in Exclude<keyof T, keyof U>]: T[P] } & U;

type Diff<T, U> = { [P in Exclude<keyof (T & U), keyof U>]: T[P] };

type Intersection<T, U> = { [P in keyof (T & U)]: P extends keyof T ? T[P] : never };

type UnionToIntersection<U> = 
  (U extends any ? (k: U) => void : never) extends ((k: infer I) => void) ? I : never;

type Flatten<T> = T extends Array<infer U> ? U : T;

type FlattenAll<T> = T extends Array<infer U> ? FlattenAll<U> : T;

type Unwrap<T> = T extends { value: infer V } ? V : T;

console.log("\n=== Type Transformation Demo ===");
type M1 = Pick<A, "x">;
type M2 = Omit<A, "x">;
type M3 = Merge<A, B>;
console.log("Transformations compiled successfully!");