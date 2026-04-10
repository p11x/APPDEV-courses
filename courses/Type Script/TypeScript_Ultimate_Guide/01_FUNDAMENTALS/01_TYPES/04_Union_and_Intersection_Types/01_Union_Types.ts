/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Union_and_Intersection_Types
 * Purpose: Understanding union types in TypeScript
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Union Types - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Union types represent values that can be one of several types
 * 💡 WHY: Enables flexible type handling and type-safe operations
 * 🔧 HOW: Pipe notation, type narrowing, discriminated unions
 */

// ============================================================================
// SECTION 1: BASIC UNION TYPES
// ============================================================================

/**
 * Union types use the pipe (|) to indicate "one of" types.
 */

// Example 1.1: Simple Union Types
// ---------------------------

// String or number
let id: string | number = "abc123";
id = 123; // Also valid

// Boolean or string
let flag: boolean | string = true;
flag = "enabled";

// Multiple primitive types
let value: string | number | boolean = "hello";
value = 42;
value = false;

console.log("Basic Union Types:", { id, flag, value });

// Example 1.2: Union Type Arrays
// --------------------------

// Array of strings or numbers
let mixedArray: (string | number)[] = [1, "two", 3, "four"];
let stringOrNumberArray: Array<string | number> = ["a", 1, "b", 2];

console.log("Union Arrays:", mixedArray);

// Example 1.3: Union with Null/Undefined
// ----------------------------------

// String or null
let name: string | null = "John";
name = null;

// String, null, or undefined
let optionalName: string | null | undefined = "John";
optionalName = null;
optionalName = undefined;

console.log("Nullable:", { name, optionalName });

// ============================================================================
// SECTION 2: UNION TYPE NARROWING
// ============================================================================

/**
 * Type narrowing reduces union to a specific type.
 */

// Example 2.1: typeof Type Guards
// ---------------------------

function processValue(value: string | number): string {
  if (typeof value === "string") {
    // TypeScript knows value is string here
    return value.toUpperCase();
  } else {
    // TypeScript knows value is number here
    return value.toFixed(2);
  }
}

console.log("typeof Narrowing:", processValue("hello"));
console.log("typeof Narrowing:", processValue(42.567));

// Example 2.2: Equality Type Guards
// ------------------------------

function handleInput(input: string | number): void {
  if (input === "") {
    console.log("Empty string");
  } else if (typeof input === "number") {
    console.log(`Number: ${input * 2}`);
  } else {
    console.log(`String: ${input}`);
  }
}

// Example 2.3: Array.isArray Type Guard
// ----------------------------------

function processArrayOrString(input: string | string[]): string {
  if (Array.isArray(input)) {
    return input.join(", ");
  }
  return input;
}

console.log("Array.isArray:", processArrayOrString(["a", "b", "c"]));
console.log("Array.isArray:", processArrayOrString("hello"));

// ============================================================================
// SECTION 3: DISCRIMINATED UNIONS
// ============================================================================

/**
 * Discriminated unions use a common property to identify types.
 */

// Example 3.1: Basic Discriminated Union
// ------------------------------------

type Shape = 
  | { kind: "circle"; radius: number }
  | { kind: "rectangle"; width: number; height: number }
  | { kind: "triangle"; base: number; height: number };

function getArea(shape: Shape): number {
  switch (shape.kind) {
    case "circle":
      return Math.PI * shape.radius ** 2;
    case "rectangle":
      return shape.width * shape.height;
    case "triangle":
      return 0.5 * shape.base * shape.height;
  }
}

console.log("\nDiscriminated Union:");
console.log(`Circle area: ${getArea({ kind: "circle", radius: 5 }).toFixed(2)}`);
console.log(`Rectangle area: ${getArea({ kind: "rectangle", width: 10, height: 5 })}`);
console.log(`Triangle area: ${getArea({ kind: "triangle", base: 8, height: 4 })}`);

// Example 3.2: Async Result Type
// --------------------------

type AsyncData<T> = 
  | { status: "loading" }
  | { status: "success"; data: T }
  | { status: "error"; error: Error };

function handleAsyncData<T>(result: AsyncData<T>): string {
  switch (result.status) {
    case "loading":
      return "Loading...";
    case "success":
      return `Data: ${JSON.stringify(result.data)}`;
    case "error":
      return `Error: ${result.error.message}`;
  }
}

// ============================================================================
// SECTION 4: UNION TYPE IN FUNCTIONS
// ============================================================================

// Example 4.1: Function Parameter Unions
// ----------------------------------

function format(value: string | number | boolean): string {
  if (typeof value === "string") {
    return value.toUpperCase();
  } else if (typeof value === "number") {
    return value.toString();
  } else {
    return value ? "Yes" : "No";
  }
}

console.log("\nFunction Parameter Unions:");
console.log(format("hello"));
console.log(format(42));
console.log(format(true));

// Example 4.2: Function Return Type Unions
// ------------------------------------

function parseInput(input: string): string | number {
  const num = parseInt(input, 10);
  if (!isNaN(num)) {
    return num;
  }
  return input;
}

// Example 4.3: Generic with Union Constraints
// -------------------------------------

function firstElement<T extends string | any[]>(arr: T): T extends (infer U)[] ? U : T extends string ? string : never {
  return arr[0];
}

console.log(firstElement(["a", "b", "c"]));
console.log(firstElement("hello"));

// ============================================================================
// SECTION 5: UNION TYPES IN INTERFACES
// ============================================================================

// Example 5.1: Property with Multiple Types
// -------------------------------------

interface User {
  id: number;
  name: string;
  // Can be string or null
  email: string | null;
  // Can be string or Date
  createdAt: string | Date;
}

const user1: User = {
  id: 1,
  name: "John",
  email: "john@example.com",
  createdAt: new Date()
};

const user2: User = {
  id: 2,
  name: "Jane",
  email: null,
  createdAt: "2024-01-01"
};

// Example 5.2: Method with Multiple Types
// ----------------------------------

interface Parser {
  parse(input: string): string | number | object;
  validate(input: string): boolean | Error;
}

// ============================================================================
// SECTION 6: LITERAL UNION TYPES
// ============================================================================

// Example 6.1: String Literal Unions
// ------------------------------

type Status = "pending" | "active" | "completed" | "failed";
type Priority = "low" | "medium" | "high" | "critical";
type HttpMethod = "GET" | "POST" | "PUT" | "DELETE" | "PATCH";

function setStatus(status: Status): void {
  console.log(`Status set to: ${status}`);
}

setStatus("active");
// setStatus("unknown"); // Error - not in union

// Example 6.2: Number Literal Unions
// -----------------------------

type ErrorCode = 400 | 401 | 403 | 404 | 500;

function handleError(code: ErrorCode): string {
  switch (code) {
    case 400: return "Bad Request";
    case 401: return "Unauthorized";
    case 403: return "Forbidden";
    case 404: return "Not Found";
    case 500: return "Internal Server Error";
  }
}

// Example 6.3: Boolean Literal Unions
// -------------------------------

type ToggleState = true | false;

const isEnabled: ToggleState = true;

// ============================================================================
// SECTION 7: UTILITY TYPES WITH UNIONS
// ============================================================================

// Example 7.1: Exclude and Extract
// ---------------------------

type Colors = "red" | "green" | "blue" | "yellow" | "purple";

type PrimaryColors = Extract<Colors, "red" | "green" | "blue">;
type NonPrimary = Exclude<Colors, "red" | "green" | "blue">;

// Example 7.2: NonNullable
// ---------------------

type MaybeNull = string | null | undefined;
type NotNull = NonNullable<MaybeNull>; // string

// Example 7.3: ReturnType with Unions
// -------------------------------

type Handler = (() => string) | (() => number);

type Return = ReturnType<Handler>; // string | number

// ============================================================================
// SECTION 8: COMMON PITFALLS
// ============================================================================

/**
 * ⚠️ COMMON ISSUE 1: Accessing methods not on all union types
 * ----------------------------------------------------------
 */

function fixUnionMethods(): void {
  let value: string | number = "hello";
  
  // This is safe due to type narrowing
  if (typeof value === "string") {
    console.log(value.toUpperCase());
  }
  
  // Or use type assertion
  (value as string).toUpperCase();
}

/**
 * ⚠️ COMMON ISSUE 2: Default parameter with union
 * ---------------------------------------------
 */

function fixDefaultParams(param: string | number = "default"): void {
  console.log(param);
}

/**
 * ⚠️ COMMON ISSUE 3: Union with too many types
 * ------------------------------------------
 */

function fixLargeUnions(): void {
  // Instead of many unions, use discriminated union
  type Result<T> = 
    | { success: true; data: T }
    | { success: false; error: string };
}

console.log("\n=== Union Types Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/04_Union_and_Intersection_Types (Intersection)");