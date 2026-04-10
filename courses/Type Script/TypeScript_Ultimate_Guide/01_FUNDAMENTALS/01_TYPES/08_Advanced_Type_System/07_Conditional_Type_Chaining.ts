/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 08_Advanced_Type_System
 * Topic: 07_Conditional_Type_Chaining
 * Purpose: Chaining conditional types for complex type transformations
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe conditional logic
 */

/**
 * Conditional Type Chaining - Complex Type Logic
 * =============================================
 * 
 * 📚 WHAT: Combining multiple conditional types for complex type logic
 * 💡 WHY: Enables sophisticated type-level computation and transformation
 * 🔧 HOW: Nested conditionals, distributive conditionals, and infer chains
 */

// ============================================================================
// SECTION 1: BASIC CONDITIONAL CHAINING
// ============================================================================

// Example 1.1: Simple conditional chain
type Result<T> = T extends string 
  ? "string"
  : T extends number 
    ? "number"
    : T extends boolean 
      ? "boolean"
      : "unknown";

// Example 1.2: Nested conditional with multiple branches
type Category<T> = T extends string 
  ? StringCategory<T>
  : T extends number 
    ? NumberCategory<T>
    : T extends object 
      ? ObjectCategory<T>
      : NeverCategory;

type StringCategory<T extends string> = T extends `${infer U}@${string}` 
  ? { kind: "email"; localPart: U }
  : { kind: "string" };

type NumberCategory<T extends number> = T extends 0 
  ? { kind: "zero" }
  : T extends positive 
    ? { kind: "positive" }
  : { kind: "negative" };

type positive = infer U extends number ? U : never;
type negative = infer U extends number ? U : never;

type ObjectCategory<T extends object> = T extends Array<infer U> 
  ? { kind: "array"; elementType: U }
  : { kind: "object" };

type NeverCategory = { kind: "never" };

// ============================================================================
// SECTION 2: DISTRIBUTIVE CONDITIONAL TYPES
// ============================================================================

// Example 2.1: Distributive over unions
type Flatten<T> = T extends any 
  ? T extends Array<infer U> ? U : T 
  : never;

type FlattenResult = Flatten<string[] | number[] | boolean[]>;
// string | number | boolean

// Example 2.2: Non-distributive version
type NonDistributiveFlatten<T> = [T] extends [Array<infer U>] ? U : T;

type NonDistResult = NonDistributiveFlatten<string[] | number[]>;
// string[] | number[]

// ============================================================================
// SECTION 3: INFER IN CONDITIONALS
// ============================================================================

// Example 3.1: Multiple infer positions
type FirstTwo<T extends unknown[]> = T extends [infer F, infer S, ...unknown[]] 
  ? [F, S] 
  : never;

type FirstTwoResult = FirstTwo<[1, 2, 3]>; // [1, 2]
type FirstTwoEmpty = FirstTwo<[1]>; // never

// Example 3.2: Nested infer
type DeepReturn<T> = T extends (...args: infer A) => infer R 
  ? R extends (...args: infer B) => infer S 
    ? S 
    : R 
  : never;

type NestedFunc = (a: string) => (b: number) => boolean;
type DeepReturnResult = DeepReturn<NestedFunc>; // boolean

// ============================================================================
// SECTION 4: CONDITIONAL TYPE COMPOSITION
// ============================================================================

// Example 4.1: Composing multiple conditionals
type Transform<T> = 
  T extends string ? Trim<T> :
  T extends number ? Square<T> :
  T extends boolean ? BooleanToString<T> :
  T;

type Trim<T extends string> = T;
type Square<T extends number> = T;
type BooleanToString<T extends boolean> = T extends true ? "true" : "false";

// Example 4.2: Chain of transformations
type ProcessValue<T> = 
  T extends null | undefined ? never :
  T extends string ? StringProcessor<T> :
  T extends number ? NumberProcessor<T> :
  T extends boolean ? BooleanProcessor<T> :
  never;

type StringProcessor<T extends string> = T extends `${infer U}` ? U : never;
type NumberProcessor<T extends number> = T;
type BooleanProcessor<T extends boolean> = T;

// ============================================================================
// SECTION 5: CONDITIONAL WITH CONSTRAINTS
// ============================================================================

// Example 5.1: Constrained conditional
type ExtractArrayType<T> = T extends Array<infer U> ? U : never;

type ExtractResult = ExtractArrayType<string[]>; // string

// Example 5.2: Multiple constraints
type Validate<T> = T extends string 
  ? T extends "" 
    ? { valid: false; reason: "empty" }
    : { valid: true; value: T }
  : T extends number 
    ? T extends 0 
      ? { valid: false; reason: "zero" }
      : { valid: true; value: T }
    : { valid: false; reason: "invalid type" };

type ValidatedString = Validate<"hello">; // { valid: true; value: "hello" }
type ValidatedEmpty = Validate<"">; // { valid: false; reason: "empty" }
type ValidatedNumber = Validate<5>; // { valid: true; value: 5 }
type ValidatedZero = Validate<0>; // { valid: false; reason: "zero" }

// ============================================================================
// SECTION 6: RECURSIVE CONDITIONAL TYPES
// ============================================================================

// Example 6.1: Recursive type processing
type DeepReadonly<T> = {
  readonly [P in keyof T]: T[P] extends object ? DeepReadonly<T[P]> : T[P];
};

interface Nested {
  a: { b: { c: string } };
  d: number;
}

type DeepReadonlyResult = DeepReadonly<Nested>;

// Example 6.2: Recursive union processing
type UnionToArray<T> = T extends unknown ? [T] : never;
type ArrayToUnion<T extends unknown[]> = T[number];

type UnionMembers = UnionToArray<string | number | boolean>;
// [string] | [number] | [boolean]

// ============================================================================
// SECTION 7: PRACTICAL CONDITIONAL PATTERNS
// ============================================================================

// Example 7.1: Type-safe event handling
type EventType<T extends string> = T extends `${infer U}:${string}` ? U : T;

type Event = EventType<"click">; // "click"
type NamespacedEvent = EventType<"namespace:action">; // "namespace"

// Example 7.2: Type-safe reducer
type ActionType<T> = T extends { type: infer U } ? U : never;

type AnyAction = 
  | { type: "increment"; amount: number }
  | { type: "decrement"; amount: number }
  | { type: "reset" };

type ExtractedType = ActionType<AnyAction>; // "increment" | "decrement" | "reset"

// Example 7.3: Conditional type for error handling
type AsyncResult<T, E = Error> = 
  T extends Promise<infer U> 
    ? U extends { ok: true; data: infer D } 
      ? { ok: true; data: D }
      : { ok: false; error: E }
    : never;

type SuccessResponse = AsyncResult<Promise<{ ok: true; data: string }>>;
type ErrorResponse = AsyncResult<Promise<{ ok: false }>>;

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Conditional type chaining increases compilation time, especially
 * with recursive types. Each conditional evaluation is O(1) but chaining
 * multiplies complexity. TypeScript has recursion limits (~1000 levels).
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Conditional types require TypeScript 2.8+. Distributive
 * conditionals and infer support have improved over versions. Recursive
 * conditionals require TypeScript 4.1+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Conditional types provide compile-time type checking. Use
 * them to validate data structures and prevent invalid data from reaching
 * runtime. Conditional type errors fail compilation, preventing releases.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test each branch of conditional types. Verify union distribution
 * works correctly. Test edge cases with never and any types.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: TypeScript errors show which branch failed. Use intermediate
 * types to debug complex conditionals. Hover over types in IDE for
 * expanded view.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Manual type mapping: More verbose but explicit
 * - Type libraries: ts-toolbelt provides optimized versions
 * - Runtime validation: More flexible but less type-safe
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 10_Infer_Type_Patterns.ts: Advanced inference patterns
 * - 09_Assertion_Types.ts: Type assertions with conditionals
 * - 05_Recursive_Type_Definitions.ts: Recursive conditional types
 */

console.log("=== Conditional Type Chaining Complete ===");
