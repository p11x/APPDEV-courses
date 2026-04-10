/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Primitive_Types
 * Purpose: Advanced primitive type operations and edge cases
 * Difficulty: intermediate
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Advanced Primitive Types - Deep Dive
 * =====================================
 * 
 * 📚 WHAT: Complex operations and edge cases with primitive types
 * 💡 WHY: Understanding edge cases ensures robust code
 * 🔧 HOW: Type guards, type narrowing, special operations
 */

// ============================================================================
// SECTION 1: BIGINT TYPE
// ============================================================================

/**
 * BigInt represents integers of arbitrary precision.
 * Use for values beyond Number.MAX_SAFE_INTEGER.
 */

// Example 1.1: Basic BigInt
// -----------------------

// BigInt literal (n suffix)
let bigNumber: bigint = 9007199254740991n; // MAX_SAFE_INTEGER
let largerNumber: bigint = 9007199254740992n;

// BigInt function
let fromFunction: bigint = BigInt("12345678901234567890");

console.log("BigInt Types:");
console.log(`BigNumber: ${bigNumber}`);
console.log(`Larger: ${largerNumber}`);
console.log(`From function: ${fromFunction}`);

// Example 1.2: BigInt Operations
// ---------------------------

let bigA: bigint = 100n;
let bigB: bigint = 200n;

// Arithmetic operations
let sum = bigA + bigB; // 300n
let diff = bigB - bigA; // 100n
let product = bigA * bigB; // 20000n
let quotient = bigB / bigA; // 2n
let remainder = bigB % bigA; // 0n

// Comparison
let isGreater = bigB > bigA; // true

// Cannot mix BigInt with Number
// let mixed = bigA + 5; // Error

// Example 1.3: BigInt in Real-World Scenarios
// -----------------------------------------

interface Transaction {
  id: string;
  amount: bigint;
  timestamp: bigint;
}

function createTransaction(amount: bigint): Transaction {
  return {
    id: `tx_${Date.now()}`,
    amount,
    timestamp: BigInt(Date.now())
  };
}

// ============================================================================
// SECTION 2: ENUM TYPES (Const and Regular)
// ============================================================================

/**
 * Enums allow defining named constants.
 */

// Example 2.1: Numeric Enums
// -----------------------

enum Direction {
  Up,    // 0
  Down,  // 1
  Left,  // 2
  Right  // 3
}

enum Status {
  Pending = 1,
  Active = 2,
  Completed = 3,
  Failed = 4
}

console.log("Numeric Enums:");
console.log(`Direction.Up: ${Direction.Up}`);
console.log(`Direction[0]: ${Direction[0]}`);
console.log(`Status.Active: ${Status.Active}`);

// Example 2.2: String Enums
// ----------------------

enum HttpMethod {
  GET = "GET",
  POST = "POST",
  PUT = "PUT",
  DELETE = "DELETE"
}

enum LogLevel {
  Debug = "DEBUG",
  Info = "INFO",
  Warn = "WARN",
  Error = "ERROR"
}

console.log("\nString Enums:");
console.log(`HttpMethod.GET: ${HttpMethod.GET}`);
console.log(`LogLevel.Error: ${LogLevel.Error}`);

// Example 2.3: Const Enums (Compile-time)
// -----------------------------------

const enum Color {
  Red = "#FF0000",
  Green = "#00FF00",
  Blue = "#0000FF"
}

// Const enums are inlined at compile time
const color = Color.Red; // Inlined as "#FF0000"

// Example 2.4: Heterogeneous Enums (Mixed)
// -------------------------------------

enum Mixed {
  A = "A",
  B = 2,
  C = "C",
  D = 4
}

// ============================================================================
// SECTION 3: TUPLE TYPES
// ============================================================================

/**
 * Tuples represent fixed-size arrays with specific types.
 */

// Example 3.1: Basic Tuples
// ---------------------

// [string, number] represents a tuple with string and number
let coordinate: [number, number] = [10, 20];
let userInfo: [string, number, boolean] = ["John", 30, true];

console.log("Basic Tuples:");
console.log(`Coordinate: ${coordinate}`);
console.log(`User: ${userInfo}`);

// Accessing elements
let x = coordinate[0]; // 10
let y = coordinate[1]; // 20

// Example 3.2: Optional Tuple Elements
// ---------------------------------

// Last element optional
let optionalTuple: [string, number, boolean?] = ["test", 42];
let optionalFull: [string, number, boolean?] = ["test", 42, true];

// Rest elements
let restTuple: [string, ...number[]] = ["a", 1, 2, 3, 4];

// Example 3.3: Named Tuple Elements
// ------------------------------

type NamedPoint = [x: number, y: number];
let point: NamedPoint = [10, 20];

type HttpResponse = [
  statusCode: number,
  statusMessage: string,
  body: string,
  headers?: Record<string, string>
];

const response: HttpResponse = [200, "OK", '{"success": true}'];
console.log("\nNamed Tuple:", response[0], response[1]);

// ============================================================================
// SECTION 4: ARRAY TYPES ADVANCED
// ============================================================================

/**
 * Advanced array type operations and patterns.
 */

// Example 4.1: Array Type Variations
// ------------------------------

// Basic array
let numbers: number[] = [1, 2, 3];
let strings: string[] = ["a", "b", "c"];

// Generic array
let genericNumbers: Array<number> = [1, 2, 3];
let genericStrings: Array<string> = ["a", "b", "c"];

// Readonly arrays (cannot modify)
let readonlyNumbers: readonly number[] = [1, 2, 3];
let readonlyTyped: ReadonlyArray<number> = [1, 2, 3];

// Example 4.2: Array Methods with Types
// ---------------------------------

interface Product {
  name: string;
  price: number;
}

const products: Product[] = [
  { name: "Laptop", price: 999 },
  { name: "Phone", price: 699 },
  { name: "Tablet", price: 499 }
];

// Map returns correct type
const prices = products.map(p => p.price);
const names = products.map(p => p.name);

// Filter preserves type
const expensiveProducts = products.filter(p => p.price > 500);

// Reduce transforms to any type
const totalPrice = products.reduce((sum, p) => sum + p.price, 0);

// Find returns union type
const found = products.find(p => p.name === "Phone");
// found is Product | undefined

// Example 4.3: Array Type Guards
// --------------------------

function isNumberArray(arr: unknown): arr is number[] {
  return Array.isArray(arr) && arr.every(item => typeof item === "number");
}

function processArray(arr: unknown): void {
  if (isNumberArray(arr)) {
    // TypeScript knows arr is number[] here
    console.log("Sum:", arr.reduce((a, b) => a + b, 0));
  }
}

// ============================================================================
// SECTION 5: TYPE NARROWING
// ============================================================================

/**
 * Narrowing union types to more specific types.
 */

// Example 5.1: typeof Narrowing
// --------------------------

function processValue(value: string | number): string {
  if (typeof value === "string") {
    // value is string here
    return value.toUpperCase();
  } else {
    // value is number here
    return value.toFixed(2);
  }
}

// Example 5.2: Array.isArray Narrowing
// --------------------------------

function processArrayValue(value: string | string[]): string {
  if (Array.isArray(value)) {
    // value is string[] here
    return value.join(", ");
  } else {
    // value is string here
    return value;
  }
}

// Example 5.3: Instanceof Narrowing
// -----------------------------

class Dog {
  bark(): void { console.log("Woof!"); }
}

class Cat {
  meow(): void { console.log("Meow!"); }
}

function handlePet(pet: Dog | Cat): void {
  if (pet instanceof Dog) {
    pet.bark();
  } else {
    pet.meow();
  }
}

// Example 5.4: Property Presence Narrowing
// ------------------------------------

interface User {
  name: string;
  email?: string;
}

function greet(user: User): string {
  if (user.email) {
    // user has email here
    return `Hello ${user.name}, email: ${user.email}`;
  }
  return `Hello ${user.name}`;
}

// ============================================================================
// SECTION 6: DISCRIMINATED UNIONS
// ============================================================================

/**
 * Using a common property to narrow union types.
 */

// Example 6.1: Basic Discriminated Union
// -----------------------------------

type Shape = 
  | { kind: "circle"; radius: number }
  | { kind: "rectangle"; width: number; height: number }
  | { kind: "triangle"; base: number; height: number };

function calculateArea(shape: Shape): number {
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
console.log(`Circle: ${calculateArea({ kind: "circle", radius: 5 }).toFixed(2)}`);
console.log(`Rectangle: ${calculateArea({ kind: "rectangle", width: 10, height: 5 })}`);

// Example 6.2: Async Result Discriminated Union
// -----------------------------------------

type AsyncResult<T> = 
  | { status: "loading" }
  | { status: "success"; data: T }
  | { status: "error"; error: Error };

function handleResult<T>(result: AsyncResult<T>): string {
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
// SECTION 7: TYPE ASSERTIONS
// ============================================================================

/**
 * Type assertions tell TypeScript to treat a value as a specific type.
 */

// Example 7.1: As Syntax
// --------------------

// Get element from DOM - TypeScript doesn't know the type
const input = document.getElementById("username") as HTMLInputElement;
// Now we can access .value
console.log("\nType Assertion:", input?.value);

// Example 7.2: Angle Bracket Syntax
// ------------------------------

// Only available in .ts files, not .tsx
// let value = <string>someValue;

// Example 7.3: Non-null Assertion
// ---------------------------

function getLength(str: string | null): number {
  // Using non-null assertion
  return str!.length;
}

function getLengthSafe(str: string | null): number {
  // Safer approach
  return str?.length ?? 0;
}

// ============================================================================
// SECTION 8: CONST ASSERTIONS
// ============================================================================

/**
 * Const assertions make values immutable with literal types.
 */

// Example 8.1: As Const
// -------------------

// Without as const - type is string[]
const fruits = ["apple", "banana"];
// With as const - type is readonly ["apple", "banana"]
const fruitsLiteral = ["apple", "banana"] as const;

// Object as const
const config = {
  env: "production",
  port: 3000
} as const;
// Type: { readonly env: "production"; readonly port: 3000 }

// Example 8.2: Enforcing Literal Types
// ---------------------------------

type Method = "GET" | "POST" | "PUT" | "DELETE";

// Without const assertion - accepts any string
const method1: Method = "GET"; // OK

// With const - literal type inference
const method2 = "GET" as const;
// Type: "GET"

console.log("\nConst Assertion:");
console.log(fruitsLiteral);

// ============================================================================
// SECTION 9: UTILITY TYPES
// ============================================================================

/**
 * Built-in utility types for common type transformations.
 */

// Example 9.1: Partial and Required
// -------------------------------

interface User {
  name: string;
  email: string;
  age: number;
}

// All properties optional
type PartialUser = Partial<User>;
// { name?: string; email?: string; age?: number }

// All properties required (already required, but useful with Partial)
type RequiredUser = Required<PartialUser>;

// Example 9.2: Pick and Omit
// -----------------------

// Pick specific properties
type UserSummary = Pick<User, "name" | "email">;
// { name: string; email: string }

// Omit specific properties
type UserWithoutAge = Omit<User, "age">;
// { name: string; email: string }

// Example 9.3: Record and Readonly
// ---------------------------

// Create object type with specific key-value types
type UserRoles = Record<string, "admin" | "user" | "guest">;
const roles: UserRoles = {
  john: "admin",
  jane: "user"
};

// Make all properties readonly
type FrozenUser = Readonly<User>;
// { readonly name: string; ... }

// Example 9.4: Extract and Exclude
// -----------------------------

type Theme = "light" | "dark" | "auto";
// Extract specific type from union
type LightTheme = Extract<Theme, "light" | "dark">;
// Exclude specific type from union
type NonAuto = Exclude<Theme, "auto">;

// ============================================================================
// SECTION 10: PERFORMANCE AND MEMORY
// ============================================================================

/**
 * Performance considerations for primitive types:
 * - Numbers are 64-bit floats (double precision)
 * - Strings are immutable - use array join for concatenation
 * - BigInt has overhead - use only when needed
 * - Symbols are unique - creation has cost
 */

// Example: Efficient string building
function buildMessage(parts: string[]): string {
  return parts.join(" "); // More efficient than concatenation
}

console.log("\n=== Advanced Primitive Types Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/03_Type_Aliases_and_Interfaces");