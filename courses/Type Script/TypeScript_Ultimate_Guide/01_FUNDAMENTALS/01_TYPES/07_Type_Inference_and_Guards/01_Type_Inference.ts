/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Type_Inference_and_Guards
 * Purpose: Understanding type inference and type guards
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Type Inference - Comprehensive Guide
 * ======================================
 * 
 * 📚 WHAT: How TypeScript infers types automatically
 * 💡 WHY: Reduces boilerplate while maintaining type safety
 * 🔧 HOW: Context, inference from usage, best common type
 */

// ============================================================================
// SECTION 1: VARIABLE TYPE INFERENCE
// ============================================================================

// Example 1.1: Basic Inference from Value
// ----------------------------------

// TypeScript infers from the assigned value
let a = "hello";       // inferred as string
let b = 42;            // inferred as number
let c = true;          // inferred as boolean
let d = [1, 2, 3];    // inferred as number[]
let e = { x: 1 };     // inferred as { x: number }

console.log("Variable Inference:", typeof a, typeof b, typeof c);

// Example 1.2: Contextual Inference
// -----------------------------

// In array methods, type is inferred from context
const numbers = [1, 2, 3];
const doubled = numbers.map(n => n * 2); // n is number

const strings = ["a", "b", "c"];
const upper = strings.map(s => s.toUpperCase()); // s is string

// Example 1.3: Best Common Type
// -------------------------

// When multiple values, TypeScript finds the common type
const values = [1, "hello", true];
// inferred as (string | number | boolean)[]

const points = [
  { x: 1, y: 2 },
  { x: 3, y: 4 }
];
// inferred as { x: number; y: number }[]

// ============================================================================
// SECTION 2: FUNCTION TYPE INFERENCE
// ============================================================================

// Example 2.1: Return Type Inference
// -----------------------------

// TypeScript infers return type from return statements
function add(a: number, b: number) {
  return a + b;
}
// Return type is inferred as number

function getUser() {
  return { id: 1, name: "John" };
}
// Return type is inferred as { id: number; name: string }

// Example 2.2: Contextual Function Types
// ---------------------------------

interface Handler {
  (event: MouseEvent): void;
}

// TypeScript infers parameter types
const handler: Handler = (e) => {
  console.log(e.clientX, e.clientY);
};

// Example 2.3: Arrow Function Inference
// ---------------------------------

// Return type is inferred
const createUser = (name: string) => ({ name, createdAt: new Date() });
// Type: (name: string) => { name: string; createdAt: Date }

// ============================================================================
// SECTION 3: CONTEXTUAL TYPING
// ============================================================================

// Example 3.1: Callback Context
// -------------------------

type Callback<T> = (item: T) => void;

function forEach<T>(items: T[], callback: Callback<T>): void {
  items.forEach(callback);
}

const numbers = [1, 2, 3];
forEach(numbers, (n) => console.log(n)); // n is number

// Example 3.2: Object Method Context
// ------------------------------

const config = {
  debug: true,
  log: (msg: string) => console.log(msg)
};

// Method is typed with correct 'this' context

// Example 3.3: Event Handler Context
// -----------------------------

const button = {
  onClick: (handler: (e: MouseEvent) => void) => {},
  click: () => handler({ clientX: 0, clientY: 0 } as MouseEvent)
};

button.onClick((e) => {
  console.log(e.clientX);
});

// ============================================================================
// SECTION 4: TYPE GUARDS
// ============================================================================

// Example 4.1: typeof Type Guards
// ---------------------------

function process(value: string | number): string {
  if (typeof value === "string") {
    return value.toUpperCase();
  } else {
    return value.toFixed(2);
  }
}

console.log("typeof guard:", process("hello"));
console.log("typeof guard:", process(42));

// Example 4.2: instanceof Type Guards
// -------------------------------

class Dog {
  bark() { return "Woof!"; }
}

class Cat {
  meow() { return "Meow!"; }
}

function makeSound(animal: Dog | Cat): string {
  if (animal instanceof Dog) {
    return animal.bark();
  } else {
    return animal.meow();
  }
}

// Example 4.3: in Operator Type Guard
// -------------------------------

interface Fish {
  swim(): void;
}

interface Bird {
  fly(): void;
}

function moveAnimal(animal: Fish | Bird): void {
  if ("swim" in animal) {
    animal.swim();
  } else {
    animal.fly();
  }
}

// ============================================================================
// SECTION 5: CUSTOM TYPE GUARDS
// ============================================================================

// Example 5.1: Type Predicate Functions
// ---------------------------------

interface User {
  name: string;
  role: string;
}

function isUser(value: unknown): value is User {
  return (
    typeof value === "object" &&
    value !== null &&
    "name" in value &&
    "role" in value &&
    typeof (value as any).name === "string" &&
    typeof (value as any).role === "string"
  );
}

function process(data: unknown): void {
  if (isUser(data)) {
    console.log(data.name, data.role); // TypeScript knows it's User
  }
}

// Example 5.2: Type Guard with Generics
// -------------------------------

function isArray<T>(value: unknown): value is T[] {
  return Array.isArray(value);
}

function handleData(data: unknown): void {
  if (isArray<string>(data)) {
    console.log(data.join(", "));
  }
}

// Example 5.3: Equality Narrowing
// ---------------------------

function equalityNarrow(value: string | number | null): void {
  if (value === null) {
    console.log("null value");
  } else if (typeof value === "string" && value.length > 0) {
    console.log("non-empty string");
  } else if (typeof value === "number" && value > 0) {
    console.log("positive number");
  }
}

// ============================================================================
// SECTION 6: DISCRIMINATED UNIONS
// ============================================================================

// Example 6.1: Basic Discriminated Union
// ------------------------------------

type Result<T> = 
  | { status: "success"; data: T }
  | { status: "error"; error: Error };

function handleResult<T>(result: Result<T>): void {
  if (result.status === "success") {
    console.log(result.data); // TypeScript knows it's T
  } else {
    console.log(result.error.message);
  }
}

// Example 6.2: Switch Statement Narrowing
// -----------------------------------

type Shape = 
  | { kind: "circle"; radius: number }
  | { kind: "rectangle"; width: number; height: number };

function getArea(shape: Shape): number {
  switch (shape.kind) {
    case "circle":
      return Math.PI * shape.radius ** 2;
    case "rectangle":
      return shape.width * shape.height;
  }
}

// Example 6.3: Exhaustiveness Checking
// ---------------------------------

type Color = "red" | "green" | "blue";

function getColorName(color: Color): string {
  switch (color) {
    case "red": return "Red";
    case "green": return "Green";
    case "blue": return "Blue";
    default:
      const _exhaustive: never = color;
      return _exhaustive;
  }
}

// ============================================================================
// SECTION 7: INFERENCE IN GENERICS
// ============================================================================

// Example 7.1: Generic Type Inference
// -------------------------------

function identity<T>(value: T): T {
  return value;
}

const num = identity(42);  // inferred as number
const str = identity("hi"); // inferred as string

// Example 7.2: Multi-Parameter Inference
// ---------------------------------

function pair<K, V>(key: K, value: V): { key: K; value: V } {
  return { key, value };
}

const p1 = pair("x", 1);    // { key: string; value: number }
const p2 = pair(1, "x");    // { key: number; value: string }

// Example 7.3: Inference from Return Type
// ---------------------------------

function createUser(name: string): { name: string; id: number } {
  return { name, id: Math.random() };
}

const user = createUser("John"); // type inferred

console.log("\n=== Type Inference and Guards Complete ===");
console.log("Next: FUNDAMENTALS/SYNTAX/01_Variables_and_Declarations");