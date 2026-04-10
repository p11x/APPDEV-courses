/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: Operators_and_Expressions
 * Purpose: TypeScript operators and expressions
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Operators and Expressions - Comprehensive Guide
 * ================================================
 * 
 * 📚 WHAT: TypeScript operators and type operations
 * 💡 WHY: Core language features for manipulating values and types
 * 🔧 HOW: Arithmetic, logical, bitwise, type operators
 */

// ============================================================================
// SECTION 1: ARITHMETIC OPERATORS
// ============================================================================

// Example 1.1: Basic Arithmetic
// -----------------------

const sum = 10 + 5;   // 15
const diff = 10 - 5;  // 5
const product = 10 * 5; // 50
const quotient = 10 / 5; // 2
const remainder = 10 % 3; // 1
const power = 2 ** 3; // 8

// Example 1.2: Type-Safe Math
// -----------------------

function add(a: number, b: number): number {
  return a + b;
}

// Works with literal types
const literalSum = 1 as const + 2 as const; // 3

// ============================================================================
// SECTION 2: COMPARISON OPERATORS
// ============================================================================

// Example 2.1: Equality
// -----------------

// Strict equality (recommended)
const strictEqual = "5" === 5; // false
const strictNotEqual = "5" !== 5; // true

// Loose equality (avoid)
const looseEqual = "5" == 5; // true

// Example 2.2: Type-Safe Comparison
// ---------------------------

function compare(a: string | number, b: string | number): boolean {
  if (typeof a === "number" && typeof b === "number") {
    return a === b;
  }
  return false;
}

// ============================================================================
// SECTION 3: LOGICAL OPERATORS
// ============================================================================

// Example 3.1: Logical AND/OR
// -----------------------

const a = true && true;  // true
const b = true || false; // true

// Short-circuit evaluation
function getName(): string {
  throw new Error("Error");
}

const safe = false && getName(); // Returns false, doesn't call getName()

// Example 3.2: Nullish Coalescing
// ---------------------------

const nullValue: string | null = null;
const result = nullValue ?? "default"; // "default"

const zeroValue = 0;
const zeroResult = zeroValue ?? "default"; // 0 (not "default")

// Example 3.3: Optional Chaining
// ---------------------------

interface User {
  name: string;
  address?: {
    city: string;
  };
}

const user: User = { name: "John" };
const city = user.address?.city; // undefined, no error

// ============================================================================
// SECTION 4: TYPE OPERATORS
// ============================================================================

// Example 4.1: typeof Operator
// -----------------------

function getType(value: unknown): string {
  if (typeof value === "string") return "string";
  if (typeof value === "number") return "number";
  if (typeof value === "boolean") return "boolean";
  return "unknown";
}

// Example 4.2: instanceof Operator
// ---------------------------

class Dog { bark() {} }
class Cat { meow() {} }

function sound(animal: Dog | Cat): string {
  if (animal instanceof Dog) {
    return animal.bark();
  }
  return animal.meow();
}

// Example 4.3: keyof Operator
// -----------------------

interface User {
  id: number;
  name: string;
}

type UserKey = keyof User; // "id" | "name"

// Example 4.4: in Operator
// ---------------------

interface Fish { swim(): void; }
interface Bird { fly(): void; }

function move(animal: Fish | Bird): void {
  if ("swim" in animal) {
    animal.swim();
  }
}

// ============================================================================
// SECTION 5: TYPE ASSERTION OPERATORS
// ============================================================================

// Example 5.1: As Type Assertion
// --------------------------

const value: unknown = "hello";
const str: string = value as string;

// Example 5.2: Type Narrowing
// -----------------------

function process(value: string | number): void {
  if (typeof value === "string") {
    console.log(value.toUpperCase());
  }
}

// Example 5.3: Non-null Assertion
// ---------------------------

function getLength(str: string | null): number {
  return str!.length;
}

// ============================================================================
// SECTION 6: CONDITIONAL OPERATOR
// ============================================================================

// Example 6.1: Ternary Operator
// -----------------------

const age = 20;
const status = age >= 18 ? "adult" : "minor";

// Example 6.2: Type in Ternary
// -----------------------

function getMessage(isError: boolean): string {
  return isError ? "Error occurred" : "Success";
}

console.log("\n=== Operators and Expressions Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/01_Setup_and_Installation");