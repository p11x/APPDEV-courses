/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 02_Type_Metaprogramming
 * Topic: 01_Type_Generation
 * Purpose: Learn compile-time type generation techniques
 * Difficulty: advanced
 * UseCase: type-safe-apis
 * Version: TS 4.1+
 * Compatibility: All TypeScript targets
 * Performance: Compile-time only, no runtime impact
 * Security: Types are erased at compile time
 */

/**
 * WHAT: Type generation creates new types programmatically at compile time.
 * TypeScript's type system is Turing complete, allowing complex computations.
 */

// ============================================
// SECTION 1: MAPPED TYPES FOR TYPE GENERATION
// ============================================

type Prepend<T extends any[], U> = [U, ...T];
type Append<T extends any[], U> = [...T, U];

type Primitive = string | number | boolean | null | undefined;

type DeepPartial<T> = {
  [P in keyof T]?: T[P] extends object ? DeepPartial<T[P]> : T[P];
};

type Nullable<T> = {
  [P in keyof T]: T[P] | null;
};

interface User {
  id: number;
  name: string;
  profile: { bio: string; avatar: string };
  settings: { theme: string; notifications: boolean };
}

type PartialUser = DeepPartial<User>;
type NullableUser = Nullable<User>;

// ============================================
// SECTION 2: CONDITIONAL TYPE GENERATION
// ============================================

type Filter<T, U> = T extends U ? T : never;

type StringKeys<T> = {
  [K in keyof T]: T[K] extends string ? K : never;
}[keyof T];

type OnlyStrings<T> = {
  [K in StringKeys<T>]: T[K];
};

type ExtractStrings<T> = OnlyStrings<T>;

interface MixedData {
  id: number;
  name: string;
  age: number;
  email: string;
  active: boolean;
}

type StringProps = ExtractStrings<MixedData>;
// Result: { name: string; email: string }

// ============================================
// SECTION 3: INFER AND RECURSIVE TYPES
// ============================================

type ReturnTypeOf<T> = T extends (...args: any[]) => infer R ? R : never;

type ParametersOf<T> = T extends (...args: infer P) => any ? P : never;

type UnwrapPromise<T> = T extends Promise<infer U> ? U : T;

type ElementType<T extends any[]> = T extends (infer U)[] ? U : never;

async function fetchUser(): Promise<{ id: number; name: string }> {
  return { id: 1, name: "Alice" };
}

type FetchResult = ReturnTypeOf<typeof fetchUser>;
type FetchParams = ParametersOf<typeof fetchUser>;

// ============================================
// SECTION 4: TEMPLATE LITERAL TYPE GENERATION
// ============================================

type EventName<T extends string> = `on${Capitalize<T>}`;

type EventHandlers<T extends string> = {
  [K in EventName<T>]: (data: K extends `on${infer U}` ? Uncapitalize<U> : never) => void;
};

type HttpMethod = "get" | "post" | "put" | "delete";
type Endpoint = "users" | "posts" | "comments";

type RouteKey<T extends HttpMethod, E extends Endpoint> = `${T}:/${E}`;

type Routes = {
  [K in RouteKey<HttpMethod, Endpoint>]: any;
};

type RouteKeys = RouteKey<"get", "users">;

// ============================================
// SECTION 5: TYPE FACTORIES
// ============================================

type Builder<T> = {
  [P in keyof T]?: Builder<T[P]>;
} & {
  (): T;
};

function typeBuilder<T>(): Builder<T> {
  return new Proxy({}, {
    get: () => typeBuilder(),
    apply: () => ({} as T)
  }) as any;
}

type OptionalProps<T, K extends keyof T> = Omit<T, K> & Partial<Pick<T, K>>;

type RequiredProps<T, K extends keyof T> = Omit<T, K> & { [P in K]-?: T[P] };

type Overwrite<T, U> = { [P in Exclude<keyof T, keyof U>]: T[P] } & U;

/**
 * PERFORMANCE:
 * - Type generation happens at compile time
 * - Complex recursive types may slow compilation
 * - Use type inference to reduce complexity
 * 
 * COMPATIBILITY:
 * - Template literal types: TS 4.1+
 * - Recursive inference: TS 4.1+
 * - Mapped types: TS 2.1+
 * 
 * SECURITY:
 * - Types are compile-time only, no runtime exposure
 * - No sensitive data in type definitions
 * 
 * TESTING:
 * - Use type assertions to verify type generation
 * - Create test types to validate transforms
 * 
 * DEBUGGING:
 * - Use `Extract` to inspect generated types
 * - Hover over types in IDE for inspection
 * 
 * ALTERNATIVE:
 * - External type generation tools
 * - Build-time code generation
 * 
 * CROSS-REFERENCE:
 * - 02_Type_Transformation.ts - Type manipulation
 * - 03_Compile_Time_Evaluation.ts - CTFE
 * - 01_Type_Generation.ts - This file
 */

console.log("\n=== Type Generation Demo ===");
type Test1 = Prepend<[number, string], boolean>;
type Test2 = Append<[number], string>;

console.log("Type generation complete!");

// Verify at compile time
const _partial: PartialUser = { profile: { bio: "Hi" } };
const _nullable: NullableUser = { name: null };