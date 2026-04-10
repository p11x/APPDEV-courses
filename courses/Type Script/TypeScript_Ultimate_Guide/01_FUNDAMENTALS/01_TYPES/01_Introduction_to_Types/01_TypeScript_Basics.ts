/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Introduction_to_Types
 * Purpose: Master the foundational concepts of TypeScript's type system
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * TypeScript Basics - A Comprehensive Introduction
 * ============================================
 * 
 * This file provides a thorough introduction to TypeScript's fundamental type system,
 * covering everything from basic type annotations to type inference.
 * 
 * 📚 WHAT: Understanding TypeScript's type system fundamentals
 * 💡 WHY: TypeScript adds static typing to JavaScript for better developer experience
 * 🔧 HOW: Type annotations, type inference, and type safety mechanisms
 */

// ============================================================================
// SECTION 1: WHAT IS TYPESCRIPT?
// ============================================================================

/**
 * TypeScript is a strongly typed programming language that builds on JavaScript.
 * 
 * Key Differentiators from JavaScript:
 * 1. Static Type Checking - Types are checked at compile-time, not runtime
 * 2. Type Annotations - Optional explicit type declarations
 * 3. Advanced Type System - Union types, generics, conditional types, and more
 * 4. IDE Support - Enhanced autocompletion, refactoring, and error prevention
 * 5. Modern Features - Supports latest ECMAScript features plus TypeScript-specific additions
 * 
 * Compilation Process:
 * TypeScript (.ts) → TypeScript Compiler (tsc) → JavaScript (.js)
 */

// ============================================================================
// SECTION 2: BASIC TYPE ANNOTATIONS
// ============================================================================

/**
 * TypeScript provides multiple ways to specify types:
 * 
 * 1. Explicit Type Annotations
 * 2. Type Inference
 * 3. Generic Types
 * 4. Union Types
 */

// Example 2.1: Basic Type Annotations
// ----------------------------------

// Primitive types with explicit annotations
let userName: string = "John Doe";
let userAge: number = 30;
let isActive: boolean = true;
let userId: symbol = Symbol("user-id");

console.log("Basic Type Annotations:");
console.log(`Name: ${userName}, Age: ${userAge}, Active: ${isActive}`);

// Arrays with type annotations
let scores: number[] = [95, 87, 92, 88];
let names: Array<string> = ["Alice", "Bob", "Charlie"];
let mixed: (string | number)[] = [1, "two", 3, "four"];

// Objects with type annotations
interface User {
  id: number;
  name: string;
  email: string;
  age?: number; // Optional property
  readonly createdAt: Date; // Read-only property
}

let user: User = {
  id: 1,
  name: "John Doe",
  email: "john@example.com",
  createdAt: new Date()
};

console.log("\nObject Type Annotation:");
console.log(JSON.stringify(user, null, 2));

// Example 2.2: Function Type Annotations
// ------------------------------------

// Function with parameter and return types
function greetUser(name: string, greeting?: string): string {
  return `${greeting || "Hello"}, ${name}!`;
}

// Arrow function with types
const addNumbers = (a: number, b: number): number => a + b;

// Function with complex return type
function getUserInfo(): User {
  return {
    id: 2,
    name: "Jane Doe",
    email: "jane@example.com",
    createdAt: new Date()
  };
}

console.log("\nFunction Types:");
console.log(greetUser("Alice", "Hi"));
console.log(`Sum: ${addNumbers(5, 3)}`);

// Example 2.3: Generic Function Types
// ----------------------------------

function identity<T>(value: T): T {
  return value;
}

function createPair<K, V>(key: K, value: V): { key: K; value: V } {
  return { key, value };
}

console.log("\nGeneric Function Types:");
console.log(identity<string>("Hello TypeScript"));
console.log(createPair<string, number>("score", 100));

// ============================================================================
// SECTION 3: TYPE INFERENCE
// ============================================================================

/**
 * TypeScript automatically infers types when no explicit annotation is provided.
 * 
 * Inference Rules:
 * 1. Variable initialization → infers from value
 * 2. Function return → infers from return statements
 * 3. Default parameter → infers from default value
 * 4. Type cannot be inferred → defaults to 'any'
 */

// Example 3.1: Variable Type Inference
// ------------------------------------

// TypeScript infers these types automatically
let inferredString = "Hello"; // inferred as string
let inferredNumber = 42; // inferred as number
let inferredBoolean = true; // inferred as boolean
let inferredArray = [1, 2, 3]; // inferred as number[]
let inferredObject = { name: "Test" }; // inferred as { name: string }

// Challenge: Be careful with mutable variables
let mutableArray = [1, 2, 3];
mutableArray.push(4); // TypeScript knows it's number[]
// mutableArray = "not an array"; // Error: Type 'string' is not assignable to type 'number[]'

console.log("\nType Inference Examples:");
console.log(`Inferred string: ${inferredString} (type: ${typeof inferredString})`);
console.log(`Inferred array: ${inferredArray}`);

// Example 3.2: Function Return Type Inference
// -------------------------------------------

// TypeScript infers return type from the function body
function calculateArea(radius: number) {
  return Math.PI * radius * radius;
}

// TypeScript infers return type as number
function getUser() {
  return {
    id: 1,
    name: "Test User",
    email: "test@example.com"
  };
}

console.log("\nReturn Type Inference:");
console.log(`Circle area: ${calculateArea(5)}`);
console.log(`User: ${JSON.stringify(getUser())}`);

// Example 3.3: Contextual Typing
// ----------------------------

// TypeScript uses context to infer types
const numbers = [1, 2, 3, 4, 5];

// TypeScript knows 'n' is number based on the array type
const doubled = numbers.map((n) => n * 2);
console.log(`Doubled: ${doubled}`);

// Event handler contextual typing
type ClickHandler = (event: MouseEvent) => void;
const handleClick: ClickHandler = (event) => {
  console.log(`Clicked at ${event.clientX}, ${event.clientY}`);
};

// ============================================================================
// SECTION 4: THE 'ANY', 'UNKNOWN', AND 'VOID' TYPES
// ============================================================================

/**
 * Special TypeScript Types:
 * 
 * any - Bypasses type checking (avoid when possible)
 * unknown - Type-safe 'any' - must be narrowed before use
 * void - Represents absence of value (mainly for function returns)
 * never - Represents values that never occur
 * undefined/null - Represents missing values
 */

// Example 4.1: The 'any' Type
// --------------------------

let dynamicValue: any = "Hello";
dynamicValue = 42;
dynamicValue = true;
dynamicValue = { some: "object" };

// Dangerous: any bypasses ALL type checking
let someString: string = dynamicValue; // No error!
let someNumber: number = dynamicValue; // No error!

console.log("\n'any' Type Examples:");
console.log(`Dynamic value: ${dynamicValue}`);
console.log(`String assignment: ${someString}`);

// Example 4.2: The 'unknown' Type
// -----------------------------

// Unknown requires type narrowing before use
function processUnknown(value: unknown): void {
  if (typeof value === "string") {
    console.log(`String length: ${value.length}`);
  } else if (typeof value === "number") {
    console.log(`Number value: ${value}`);
  } else if (Array.isArray(value)) {
    console.log(`Array length: ${value.length}`);
  }
}

// Type narrowing with type guards
function isString(value: unknown): value is string {
  return typeof value === "string";
}

console.log("\n'unknown' Type Examples:");
processUnknown("Hello TypeScript");
processUnknown(42);

// Example 4.3: The 'void' Type
// ---------------------------

function logMessage(message: string): void {
  console.log(`Log: ${message}`);
  // void means this function doesn't return a meaningful value
}

function processAndReturn(): void {
  // Can use return; but not return <value>;
  console.log("Processing...");
  return; // This is allowed
}

// Example 4.4: The 'never' Type
// ---------------------------

// Function that throws always returns never
function throwError(message: string): never {
  throw new Error(message);
}

// Function with infinite loop returns never
function infiniteLoop(): never {
  while (true) {
    // Infinite execution
  }
}

// TypeScript infers never for exhaustiveness checking
function exhaustiveCheck(value: "a" | "b"): string {
  switch (value) {
    case "a":
      return "A";
    case "b":
      return "B";
    default:
      // If we add a new case but forget it here, TypeScript error
      const _exhaustive: never = value;
      return _exhaustive;
  }
}

console.log("\n'never' Type Examples:");
try {
  throwError("This is an error");
} catch (e) {
  console.log(`Caught: ${e}`);
}

// ============================================================================
// SECTION 5: TYPE ALIASES
// ============================================================================

/**
 * Type aliases create custom names for types, improving code readability.
 */

// Example 5.1: Basic Type Aliases
// ------------------------------

type UserID = number;
type UserName = string;
type UserEmail = string;
type UserStatus = "active" | "inactive" | "pending";

interface TypedUser {
  id: UserID;
  name: UserName;
  email: UserEmail;
  status: UserStatus;
}

function createUser(name: UserName, email: UserEmail): TypedUser {
  return {
    id: Date.now(),
    name,
    email,
    status: "active"
  };
}

// Example 5.2: Complex Type Aliases
// --------------------------------

type Point = {
  x: number;
  y: number;
};

type Line = {
  start: Point;
  end: Point;
};

type Shape = Circle | Rectangle | Triangle;

interface Circle {
  type: "circle";
  center: Point;
  radius: number;
}

interface Rectangle {
  type: "rectangle";
  topLeft: Point;
  width: number;
  height: number;
}

interface Triangle {
  type: "triangle";
  vertices: [Point, Point, Point];
}

// Example 5.3: Function Type Aliases
// ----------------------------------

type MathOperation = (a: number, b: number) => number;
type Callback<T> = (error: Error | null, result: T) => void;
type EventHandler<T extends Event = Event> = (event: T) => void;

const add: MathOperation = (a, b) => a + b;
const subtract: MathOperation = (a, b) => a - b;

// ============================================================================
// SECTION 6: INTERFACES VS TYPE ALIASES
// ============================================================================

/**
 * When to use Interface vs Type Alias:
 * 
 * Interface:
 * - Defining object shapes
 * - Class implementation
 * - Extending other interfaces
 * - Declaration merging
 * 
 * Type Alias:
 * - Union types
 * - Tuple types
 * - Function types
 * - Conditional types
 * - Mapped types
 */

// Example 6.1: Interface for Object Shapes
// ---------------------------------------

interface Person {
  name: string;
  age: number;
  greet(): void;
}

class PersonImpl implements Person {
  name: string;
  age: number;

  constructor(name: string, age: number) {
    this.name = name;
    this.age = age;
  }

  greet(): void {
    console.log(`Hello, I'm ${this.name}`);
  }
}

// Example 6.2: Type Alias for Unions
// --------------------------------

type StringOrNumber = string | number;
type CallbackResult = SuccessResult | ErrorResult;

interface SuccessResult {
  status: "success";
  data: unknown;
}

interface ErrorResult {
  status: "error";
  error: Error;
}

// Example 6.3: Declaration Merging (Interfaces only)
// -------------------------------------------------

interface Config {
  apiUrl: string;
}

interface Config {
  timeout: number;
}

// After merging:
const config: Config = {
  apiUrl: "https://api.example.com",
  timeout: 5000
};

// ============================================================================
// SECTION 7: COMMON MISTAKES AND PITFALLS
// ============================================================================

/**
 * ⚠️ COMMON ISSUE 1: Using 'any' too liberally
 * -------------------------------------------
 * Problem: 'any' defeats the purpose of TypeScript
 * Solution: Use 'unknown' and proper type narrowing
 */

function badPractice(value: any): void {
  // This defeats type safety
  console.log(value.someProperty); // No error, but dangerous!
}

function goodPractice(value: unknown): void {
  // Proper type narrowing
  if (value && typeof value === "object" && "someProperty" in value) {
    console.log((value as { someProperty: string }).someProperty);
  }
}

/**
 * ⚠️ COMMON ISSUE 2: Not handling null/undefined
 * --------------------------------------------
 * Problem: TypeScript strict null checks
 * Solution: Use optional chaining and nullish coalescing
 */

interface OptionalUser {
  name: string;
  address?: {
    city: string;
    zip: string;
  };
}

function printCity(user: OptionalUser): void {
  // Bad: Could crash if address is undefined
  // console.log(user.address.city);
  
  // Good: Optional chaining
  console.log(user.address?.city ?? "Unknown");
}

/**
 * ⚠️ COMMON ISSUE 3: Overly complex types
 * ---------------------------------------
 * Problem: Type aliases nested too deep
 * Solution: Break into smaller, reusable types
 */

// Bad: Overly complex
type ComplexType = {
  data: {
    items: Array<{
      id: number;
      name: string;
      metadata: {
        created: Date;
        modified: Date;
        tags: string[];
      };
    }>;
  };
};

// Good: Broken down
type ItemMetadata = {
  created: Date;
  modified: Date;
  tags: string[];
};

type Item = {
  id: number;
  name: string;
  metadata: ItemMetadata;
};

type ItemList = {
  items: Item[];
};

type GoodComplexType = {
  data: ItemList;
};

// ============================================================================
// SECTION 8: PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * TypeScript Runtime Performance:
 * 
 * 1. Type annotations have NO runtime impact (they're compiled away)
 * 2. Use 'const' assertions for literal types when performance matters
 * 3. Avoid excessive generics in hot paths
 * 4. Consider using 'satisfies' for type checking without widening
 */

// Example 8.1: Const Assertions
// ---------------------------

// Without const - type is string[]
const fruits = ["apple", "banana"];

// With const - type is readonly ["apple", "banana"]
const fruitsLiteral = ["apple", "banana"] as const;

// Example 8.2: Satisfies Operator
// ------------------------------

// Type is inferred but also validates against the type
const config = {
  apiUrl: "https://api.example.com",
  timeout: 5000
} satisfies {
  apiUrl: string;
  timeout: number;
};

// ============================================================================
// SECTION 9: COMPATIBILITY MATRIX
// ============================================================================

/**
 * Browser Support:
 * - TypeScript compiles to ES3+ JavaScript
 * - Target version configured in tsconfig.json
 * 
 * Node.js Support:
 * - Full ESNext support available
 * - Check node --version for capabilities
 * 
 * Framework Considerations:
 * - React: Works with all modern React versions
 * - Angular: Native TypeScript support
 * - Vue: Full TypeScript support withvue-tsc
 */

console.log("\n=== TypeScript Basics Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/02_Primitive_Types");