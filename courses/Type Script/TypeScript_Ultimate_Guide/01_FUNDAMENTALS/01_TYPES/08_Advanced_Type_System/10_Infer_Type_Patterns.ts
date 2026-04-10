/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 08_Advanced_Type_System
 * Topic: 10_Infer_Type_Patterns
 * Purpose: Using infer keyword for type extraction and pattern matching
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe type inference
 */

/**
 * Infer Type Patterns - Type-Level Pattern Matching
 * =================================================
 * 
 * 📚 WHAT: Using 'infer' keyword to extract types from other types
 * 💡 WHY: Enables creating flexible, composable type utilities
 * 🔧 HOW: Conditional types with infer for type extraction
 */

// ============================================================================
// SECTION 1: BASIC INFER PATTERNS
// ============================================================================

// Example 1.1: Extract array element type
type ArrayElement<T> = T extends (infer U)[] ? U : never;

type ElementOfStringArray = ArrayElement<string[]>; // string
type ElementOfArray = ArrayElement<string[]>; // string
type ElementOfReadonly = ArrayElement<readonly number[]>; // number

// Example 1.2: Extract function return type
type ReturnTypeOf<T> = T extends (...args: any[]) => infer R ? R : never;

function getData(): Promise<{ id: number }> {
  return Promise.resolve({ id: 1 });
}

type ReturnTypeResult = ReturnTypeOf<typeof getData>; // Promise<{ id: number }>

// Example 1.3: Extract function parameter types
type ParametersOf<T> = T extends (...args: infer P) => unknown ? P : never;

function process(a: string, b: number): void {}

type Params = ParametersOf<typeof process>; // [string, number]

// ============================================================================
// SECTION 2: MULTIPLE INFER POSITIONS
// ============================================================================

// Example 2.1: Multiple parameters
type FirstTwoParams<T> = T extends (
  infer A, 
  infer B, 
  ...unknown[]
) => unknown 
  ? [A, B] 
  : never;

type FirstTwoResult = FirstTwoParams<(a: string, b: number, c: boolean) => void>;
// [string, number]

// Example 2.2: Tuple inference
type Head<T extends unknown[]> = T extends [infer H, ...unknown[]] ? H : never;
type Tail<T extends unknown[]> = T extends [unknown, ...infer T] ? T : never;

type HeadResult = Head<[string, number, boolean]>; // string
type TailResult = Tail<[string, number, boolean]>; // [number, boolean]

// Example 2.3: Rest inference
type Rest<T extends unknown[]> = T extends [...unknown[], infer R] ? R : never;

type RestResult = Rest<[string, number, boolean]>; // boolean

// ============================================================================
// SECTION 3: NESTED INFER
// ============================================================================

// Example 3.1: Nested function types
type UnwrapPromise<T> = T extends Promise<infer U> ? U : never;

type DeepUnwrap = UnwrapPromise<Promise<Promise<string>>>; // Promise<string>

// Example 3.2: Recursive unwrapping
type DeepUnwrapPromise<T> = T extends Promise<infer U> 
  ? U extends Promise<unknown> 
    ? DeepUnwrapPromise<U> 
    : U 
  : T;

type DeepResult = DeepUnwrapPromise<Promise<Promise<string>>>; // string

// Example 3.3: Nested array types
type DeepArrayElement<T> = T extends (infer U)[] 
  ? U extends (infer V)[] 
    ? DeepArrayElement<V> 
    : U 
  : T;

type DeepElement = DeepArrayElement<string[][][]>; // string

// ============================================================================
// SECTION 4: INFER IN CONDITIONAL TYPES
// ============================================================================

// Example 4.1: Conditional with infer
type IsAny<T> = 0 extends (1 & T) ? true : false;

type IsAnyResult = IsAny<any>; // true
type IsStringResult = IsAny<string>; // false

// Example 4.2: Type that checks if T extends U
type IfExtends<T, U, True, False> = T extends U ? True : False;

type CheckExtends = IfExtends<string, string, "yes", "no">; // "yes"
type CheckExtends2 = IfExtends<number, string, "yes", "no">; // "no"

// Example 4.3: Distributive infer
type FlattenDistributive<T> = T extends unknown ? (T extends any[] ? T[number] : T) : never;

type FlattenResult2 = FlattenDistributive<string[] | number[] | boolean>;
// string | number | boolean

// ============================================================================
// SECTION 5: INFER WITH KEYOF
// ============================================================================

// Example 5.1: Get value type from key
type ValueOf<T, K extends keyof T> = T extends { [key in K]: infer V } ? V : never;

interface User {
  name: string;
  age: number;
}

type NameValue = ValueOf<User, "name">; // string

// Example 5.2: Mapped type with infer
type MappedValue<T> = {
  [K in keyof T]: T[K] extends infer V ? V : never;
};

type MappedResult = MappedValue<{ a: string; b: number }>;
// { a: string; b: number }

// ============================================================================
// SECTION 6: INFER IN CLASS TYPES
// ============================================================================

// Example 6.1: Instance type from constructor
type InstanceTypeOf<T> = T extends new (...args: any[]) => infer I ? I : never;

class User {
  constructor(public name: string) {}
}

type InstanceResult = InstanceTypeOf<typeof User>; // User

// Example 6.2: Constructor parameters
type ConstructorParametersOf<T> = T extends new (...args: infer P) => any ? P : never;

type ConstructorParams = ConstructorParametersOf<typeof User>; // [string]

// ============================================================================
// SECTION 7: ADVANCED INFER PATTERNS
// ============================================================================

// Example 7.1: Extract method from object
type MethodOf<T> = T extends { method(): infer R } ? R : never;

interface Service {
  method(): Promise<string>;
}

type MethodResult = MethodOf<Service>; // Promise<string>

// Example 7.2: Union to intersection (advanced)
type UnionToIntersection<U> = 
  (U extends unknown ? (x: U) => void) extends (x: infer I) => void ? I : never;

type Union = string | number;
type Intersection = UnionToIntersection<Union>; // string & number

// Example 7.3: Last element of tuple
type Last<T extends unknown[]> = 
  T extends [...unknown[], infer L] ? L : never;

type LastResult = Last<[string, number, boolean]>; // boolean

// ============================================================================
// SECTION 8: PRACTICAL INFER PATTERNS
// ============================================================================

// Example 8.1: Extract from discriminated union
type EventData<T extends string> = 
  | { type: T; data: infer D } 
  | { type: string };

type ExtractData<T extends string> = 
  EventData<T> extends { type: T; data: infer D } ? D : never;

type Extracted = ExtractData<"click">;

// Example 8.2: Parameter type from callback
type CallbackParameter<T> = 
  T extends (param: infer P) => void ? P : never;

function onClick(handler: (data: { x: number; y: number }) => void): void {}

type ClickParam = CallbackParameter<typeof onClick>; // { x: number; y: number }

// Example 8.3: Deep property type
type DeepPropertyType<T, P extends string> = 
  P extends `${infer K}.${infer R}`
    ? K extends keyof T
      ? DeepPropertyType<T[K], R>
      : never
    : P extends keyof T
      ? T[P]
      : never;

interface Config {
  database: {
    host: string;
    port: number;
  };
}

type DeepProp = DeepPropertyType<Config, "database.host">; // string

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Complex infer patterns can slow compilation. Each infer
 * position requires type traversal. Use intermediate types for complex
 * transformations. TypeScript has a recursion depth limit.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: The infer keyword requires TypeScript 2.8+. Many patterns
 * have been improved in TypeScript 3.x, 4.x, and 5.x. Check version-specific
 * capabilities when using advanced patterns.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Infer patterns provide compile-time type extraction without
 * runtime overhead. Use them to create type-safe utilities. Avoid
 * over-reliance on type assertions after inference.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test each infer position with various input types. Verify
 * nested infer works correctly. Test edge cases with never and any.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use IDE tooltips to see inferred types. Break complex patterns
 * into intermediate steps. Check TypeScript errors for inference failures.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Manual type extraction: More verbose but explicit
 * - Utility libraries: ts-toolbelt provides optimized versions
 * - Runtime solutions: For dynamic type scenarios
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 07_Conditional_Type_Chaining.ts: Conditional type inference
 * - 05_Recursive_Type_Definitions.ts: Recursive type inference
 * - 10_Type_Utilities: Built-in types using infer
 */

console.log("=== Infer Type Patterns Complete ===");
