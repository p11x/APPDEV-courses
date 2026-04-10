/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: Functions_and_Methods
 * Purpose: Understanding functions and methods in TypeScript
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Functions and Methods - Comprehensive Guide
 * ============================================
 * 
 * 📚 WHAT: Function declarations, signatures, and method types
 * 💡 WHY: Core building blocks of TypeScript applications
 * 🔧 HOW: Type annotations, overloads, generics, this typing
 */

// ============================================================================
// SECTION 1: FUNCTION DECLARATIONS
// ============================================================================

// Example 1.1: Basic Function with Types
// -----------------------------------

function greet(name: string): string {
  return `Hello, ${name}!`;
}

// Function with multiple parameters
function add(a: number, b: number): number {
  return a + b;
}

// Function with no return value
function log(message: string): void {
  console.log(message);
}

// Example 1.2: Function with Optional Parameters
// ------------------------------------------

function greetOptional(name: string, greeting?: string): string {
  return greeting ? `${greeting}, ${name}!` : `Hello, ${name}!`;
}

console.log(greetOptional("John"));
console.log(greetOptional("John", "Hi"));

// Example 1.3: Default Parameter Types
// ---------------------------------

function greetDefault(name: string, greeting: string = "Hello"): string {
  return `${greeting}, ${name}!`;
}

// ============================================================================
// SECTION 2: ARROW FUNCTIONS
// ============================================================================

// Example 2.1: Basic Arrow Functions
// ------------------------------

const addNumbers = (a: number, b: number): number => a + b;

const greetArrow = (name: string): string => `Hello, ${name}!`;

const logValue = (value: unknown): void => console.log(value);

// Example 2.2: Arrow Function with Block Body
// ---------------------------------------

const processAndLog = (value: number): number => {
  const result = value * 2;
  console.log(result);
  return result;
};

// Example 2.3: Arrow Functions with Type Inference
// --------------------------------------------

// Return type can be inferred
const double = (n: number) => n * 2;

// Parameter type can be inferred from context
const numbers = [1, 2, 3];
numbers.map(n => n * 2); // n is number

// ============================================================================
// SECTION 3: FUNCTION TYPES
// ============================================================================

// Example 3.1: Function Type Aliases
// ------------------------------

type AddFunction = (a: number, b: number) => number;
type StringHandler = (message: string) => void;
type EventCallback<T> = (data: T) => void;

const add: AddFunction = (a, b) => a + b;
const handle: StringHandler = (msg) => console.log(msg);

// Example 3.2: Interface Method Types
// -------------------------------

interface Calculator {
  add(a: number, b: number): number;
  subtract(a: number, b: number): number;
  calculate(operation: string, a: number, b: number): number;
}

const calculator: Calculator = {
  add: (a, b) => a + b,
  subtract: (a, b) => a - b,
  calculate: (op, a, b) => {
    switch (op) {
      case "+": return a + b;
      case "-": return a - b;
      default: return 0;
    }
  }
};

// Example 3.3: Function Parameter Types
// ---------------------------------

function processItems(
  items: string[],
  processor: (item: string) => string
): string[] {
  return items.map(processor);
}

const processed = processItems(["a", "b", "c"], (s) => s.toUpperCase());

// ============================================================================
// SECTION 4: FUNCTION OVERLOADS
// ============================================================================

// Example 4.1: Overload Signatures
// ---------------------------

function format(value: string): string;
function format(value: number, precision: number): string;
function format(value: string | number, precision?: number): string {
  if (typeof value === "number") {
    return value.toFixed(precision ?? 2);
  }
  return value.toString();
}

console.log(format("hello"));
console.log(format(3.14159, 2));
console.log(format(3.14159));

// Example 4.2: Method Overloads
// -------------------------

class StringBuilder {
  private value: string = "";
  
  add(text: string): this;
  add(lines: string[]): this;
  add(textOrLines: string | string[]): this {
    if (Array.isArray(textOrLines)) {
      this.value += textOrLines.join("");
    } else {
      this.value += textOrLines;
    }
    return this;
  }
  
  build(): string {
    return this.value;
  }
}

// ============================================================================
// SECTION 5: REST PARAMETERS AND ARGUMENTS
// ============================================================================

// Example 5.1: Rest Parameters
// -----------------------

function sum(...numbers: number[]): number {
  return numbers.reduce((acc, n) => acc + n, 0);
}

console.log("Sum:", sum(1, 2, 3, 4, 5));

// Example 5.2: Rest with Type Annotation
// ----------------------------------

function concat(separator: string, ...parts: string[]): string {
  return parts.join(separator);
}

console.log("Concat:", concat("-", "a", "b", "c"));

// ============================================================================
// SECTION 6: THIS TYPING
// ============================================================================

// Example 6.1: Explicit this Parameter
// ---------------------------------

interface Counter {
  count: number;
  increment(this: Counter): void;
  decrement(this: Counter): void;
}

const counter: Counter = {
  count: 0,
  increment() { this.count++; },
  decrement() { this.count--; }
};

// Example 6.2: this in Arrow Functions
// -------------------------------

class Timer {
  seconds: number = 0;
  
  // Arrow function - 'this' is lexically bound
  start = () => {
    setInterval(() => {
      this.seconds++;
      console.log(this.seconds);
    }, 1000);
  };
}

// ============================================================================
// SECTION 7: GENERIC FUNCTIONS
// ============================================================================

// Example 7.1: Basic Generic Function
// -------------------------------

function identity<T>(value: T): T {
  return value;
}

console.log(identity<string>("hello"));
console.log(identity<number>(42));

// Example 7.2: Generic Constraints
// ---------------------------

function getProperty<T, K extends keyof T>(obj: T, key: K): T[K] {
  return obj[key];
}

const user = { name: "John", age: 30 };
console.log(getProperty(user, "name"));

// Example 7.3: Generic with Default
// -----------------------------

function createPair<K, V = string>(key: K, value?: V): { key: K; value: V } {
  return { key, value: value as V };
}

console.log(createPair("id"));
console.log(createPair("id", 123));

console.log("\n=== Functions and Methods Complete ===");
console.log("Next: FUNDAMENTALS/SYNTAX/03_Classes_and_OOP");