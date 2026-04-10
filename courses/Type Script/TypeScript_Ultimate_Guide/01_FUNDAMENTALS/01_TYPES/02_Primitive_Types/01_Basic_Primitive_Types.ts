/**
 * Category: FUNDAMENTALS
 * Subcategory: TYPES
 * Concept: Primitive_Types
 * Purpose: Complete guide to TypeScript's primitive types
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Primitive Types - Comprehensive Guide
 * ======================================
 * 
 * 📚 WHAT: Understanding TypeScript's primitive data types
 * 💡 WHY: Primitives are the building blocks of all data in TypeScript
 * 🔧 HOW: Type annotations, type inference, and type operations
 */

// ============================================================================
// SECTION 1: NUMBER TYPE
// ============================================================================

/**
 * The number type represents both integers and floating-point values.
 * TypeScript uses the same number type for all numeric values.
 */

// Example 1.1: Basic Number Types
// ------------------------------

// Integer values
let positiveInt: number = 42;
let negativeInt: number = -17;

// Floating-point values
let float: number = 3.14159;
let scientific: number = 1.5e10; // 15000000000

// Hexadecimal (base 16)
let hex: number = 0xff; // 255
let hexLarge: number = 0x1a2b3c; // 1715004

// Binary (base 2)
let binary: number = 0b1010; // 10
let binaryLarge: number = 0b11111111; // 255

// Octal (base 8)
let octal: number = 0o755; // 493
let octalLarge: number = 0o777; // 511

// Special numeric values
let infinity: number = Infinity;
let negativeInfinity: number = -Infinity;
let notANumber: number = NaN;

console.log("Number Types:");
console.log(`Integers: ${positiveInt}, ${negativeInt}`);
console.log(`Floats: ${float}, ${scientific}`);
console.log(`Hex: ${hex} (0xff), ${hexLarge}`);
console.log(`Binary: ${binary} (0b1010), ${binaryLarge}`);
console.log(`Octal: ${octal} (0o755), ${octalLarge}`);
console.log(`Special: ${infinity}, ${negativeInfinity}, ${notANumber}`);

// Example 1.2: Number Type in Functions
// ------------------------------------

function calculateArea(radius: number): number {
  return Math.PI * radius * radius;
}

function calculateVolume(radius: number, height: number): number {
  return Math.PI * radius * radius * height;
}

// Function with multiple number parameters
function calculateBMI(weight: number, heightInMeters: number): number {
  return weight / (heightInMeters * heightInMeters);
}

console.log("\nNumber in Functions:");
console.log(`Area (r=5): ${calculateArea(5).toFixed(2)}`);
console.log(`Volume (r=3, h=10): ${calculateVolume(3, 10).toFixed(2)}`);
console.log(`BMI (70kg, 1.75m): ${calculateBMI(70, 1.75).toFixed(2)}`);

// Example 1.3: Number Type Arrays
// -----------------------------

let scores: number[] = [95, 87, 92, 78, 88];
let temperatures: Array<number> = [22.5, 23.1, 21.8, 24.0];

// Numeric operations on arrays
const sum = scores.reduce((acc, score) => acc + score, 0);
const average = sum / scores.length;
const maxScore = Math.max(...scores);
const minScore = Math.min(...scores);

console.log("\nNumber Arrays:");
console.log(`Scores: ${scores.join(", ")}`);
console.log(`Sum: ${sum}, Average: ${average.toFixed(2)}`);
console.log(`Max: ${maxScore}, Min: ${minScore}`);

// ============================================================================
// SECTION 2: STRING TYPE
// ============================================================================

/**
 * The string type represents textual data. TypeScript strings are immutable.
 */

// Example 2.1: Basic String Types
// -----------------------------

// Single quotes
let singleQuote: string = 'Hello World';

// Double quotes
let doubleQuote: string = "Hello World";

// Template literals (backticks)
let templateLiteral: string = `Hello World`;

// String with expressions
let name: string = "John";
let greeting: string = `Hello, ${name}!`;
let multiline: string = `
  This is a
  multiline
  string
`;

console.log("String Types:");
console.log(singleQuote);
console.log(doubleQuote);
console.log(greeting);
console.log(multiline);

// Example 2.2: String Methods and Properties
// ----------------------------------------

let message: string = "TypeScript is awesome!";

// Length
console.log(`\nString Length: ${message.length}`);

// Index access
console.log(`Character at index 0: ${message[0]}`);
console.log(`Character at index 10: ${message.charAt(10)}`);

// Substring operations
console.log(`Substring (0,10): ${message.substring(0, 10)}`);
console.log(`Slice (0, 10): ${message.slice(0, 10)}`);

// Search methods
console.log(`Index of "is": ${message.indexOf("is")}`);
console.log(`Includes "awesome": ${message.includes("awesome")}`);
console.log(`Starts with "Type": ${message.startsWith("Type")}`);
console.log(`Ends with "!": ${message.endsWith("!")}`);

// Case conversion
console.log(`Uppercase: ${message.toUpperCase()}`);
console.log(`Lowercase: ${message.toLowerCase()}`);

// Split and join
let words: string[] = message.split(" ");
console.log(`Split: ${JSON.stringify(words)}`);

// Replace
let replaced: string = message.replace("awesome", "amazing");
console.log(`Replaced: ${replaced}`);

// Example 2.3: String Type in Functions
// ----------------------------------

function formatName(firstName: string, lastName: string): string {
  return `${lastName}, ${firstName}`;
}

function createEmail(to: string, subject: string, body: string): string {
  return `To: ${to}\nSubject: ${subject}\n\n${body}`;
}

function capitalizeWords(text: string): string {
  return text
    .split(" ")
    .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
    .join(" ");
}

console.log("\nString in Functions:");
console.log(formatName("John", "Doe"));
console.log(capitalizeWords("hello world from typescript"));

// Example 2.4: Template Literal Types
// ---------------------------------

// Template literal types for type-level string manipulation
type Greeting = `Hello, ${string}!`;
let greeting: Greeting = "Hello, World!"; // Valid
// let invalidGreeting: Greeting = "Hi"; // Error: doesn't match pattern

type HttpMethod = "GET" | "POST" | "PUT" | "DELETE";
type Endpoint = `/api/${string}`;
type FullEndpoint = `${HttpMethod}:${Endpoint}`;

const getUsers: FullEndpoint = "GET:/api/users";
const createUser: FullEndpoint = "POST:/api/users";

// ============================================================================
// SECTION 3: BOOLEAN TYPE
// ============================================================================

/**
 * The boolean type represents logical values - true or false.
 */

// Example 3.1: Basic Boolean Types
// ------------------------------

let isActive: boolean = true;
let isComplete: boolean = false;
let hasPermission: boolean = true;

// Boolean from expressions
let isGreater: boolean = 10 > 5;
let isEqual: boolean = "hello" === "hello";
let isValid: boolean = (100 > 0) && (50 < 100);

console.log("Boolean Types:");
console.log(`isActive: ${isActive}`);
console.log(`isComplete: ${isComplete}`);
console.log(`isGreater: ${isGreater}`);

// Example 3.2: Boolean in Conditionals
// ----------------------------------

function checkAccess(isLoggedIn: boolean, hasRole: boolean): boolean {
  return isLoggedIn && hasRole;
}

function validateForm(formData: {
  name: string;
  email: string;
  age: number;
}): { isValid: boolean; errors: string[] } {
  const errors: string[] = [];
  
  if (!formData.name || formData.name.trim().length === 0) {
    errors.push("Name is required");
  }
  
  if (!formData.email || !formData.email.includes("@")) {
    errors.push("Valid email is required");
  }
  
  if (formData.age < 0 || formData.age > 150) {
    errors.push("Age must be between 0 and 150");
  }
  
  return {
    isValid: errors.length === 0,
    errors
  };
}

console.log("\nBoolean in Conditionals:");
console.log(checkAccess(true, true));
console.log(JSON.stringify(validateForm({ name: "John", email: "john@test.com", age: 30 })));

// Example 3.3: Boolean Type Guards
// -----------------------------

interface User {
  id: number;
  name: string;
  email?: string;
  isActive: boolean;
  role?: "admin" | "user" | "guest";
}

function isActiveUser(user: User): boolean {
  return user.isActive === true;
}

function isAdmin(user: User): user is User & { role: "admin" } {
  return user.role === "admin";
}

// ============================================================================
// SECTION 4: SYMBOL TYPE
// ============================================================================

/**
 * The symbol type represents unique identifier values.
 */

// Example 4.1: Basic Symbol Types
// -----------------------------

// Create symbols
const sym1: symbol = Symbol("description");
const sym2: symbol = Symbol("description");

// Symbols are unique even with same description
console.log("Symbol Uniqueness:");
console.log(`sym1 === sym2: ${sym1 === sym2}`);
console.log(`sym1 description: ${sym1.description}`);
console.log(`sym2 description: ${sym2.description}`);

// Example 4.2: Symbol as Object Keys
// -------------------------------

// Symbol as property key
const ageKey: symbol = Symbol("age");
const user = {
  name: "John",
  [ageKey]: 30
};

console.log("\nSymbol as Object Key:");
console.log(`Name: ${user.name}`);
console.log(`Age (via symbol): ${(user as any)[ageKey]}`);

// Example 4.3: Well-Known Symbols
// ---------------------------

// Symbol.iterator for custom iteration
const collection = {
  items: [1, 2, 3],
  [Symbol.iterator]() {
    let index = 0;
    return {
      next: () => {
        if (index < this.items.length) {
          return { value: this.items[index++], done: false };
        }
        return { value: undefined, done: true };
      }
    };
  }
};

console.log("\nSymbol.iterator:");
for (const item of collection) {
  console.log(item);
}

// Symbol for object string representation
const obj = {
  [Symbol.toStringTag]: "CustomObject"
};

// ============================================================================
// SECTION 5: UNDEFINED AND NULL
// ============================================================================

/**
 * undefined and null represent the absence of value.
 * With strictNullChecks, they must be explicitly handled.
 */

// Example 5.1: Basic Null Types
// ---------------------------

let undefinedValue: undefined = undefined;
let nullValue: null = null;

// Uninitialized (implicitly undefined)
let notAssigned: string | undefined;
// console.log(notAssigned); // Error in strict mode

console.log("Null Types:");
console.log(`undefined: ${undefinedValue}`);
console.log(`null: ${nullValue}`);

// Example 5.2: Nullable Types
// -----------------------

function findUser(id: number): { name: string } | null {
  // Simulating user lookup
  if (id === 1) {
    return { name: "John" };
  }
  return null;
}

function processUser(user: { name: string } | null): string {
  if (user === null) {
    return "User not found";
  }
  return `Hello, ${user.name}`;
}

// Optional chaining
interface Company {
  name: string;
  address?: {
    city: string;
    zip: string;
  };
}

const company: Company = {
  name: "Tech Corp"
};

const city = company.address?.city ?? "Unknown";
const zip = company.address?.zip ?? "00000";

console.log("\nNullable Types:");
console.log(processUser(findUser(1)));
console.log(processUser(findUser(999)));
console.log(`City: ${city}, Zip: ${zip}`);

// Example 5.3: Nullish Coalescing
// ---------------------------

// ?? operator - returns right side only if left is null/undefined
let value: string | null = null;
let result = value ?? "default";
console.log(`\nNullish coalescing: ${result}`);

// || operator - returns right side if left is falsy (including "", 0, false)
let falsyValue: string = "";
let orResult = falsyValue || "default";
console.log(`|| operator: ${orResult}`);

// ============================================================================
// SECTION 6: VOID, NEVER, AND OBJECT
// ============================================================================

/**
 * Additional primitive-like types in TypeScript.
 */

// Example 6.1: Void Type
// --------------------

function logMessage(message: string): void {
  console.log(`Log: ${message}`);
  // No return value
}

function processData(data: string): void {
  // Side effects only
  console.log(`Processing: ${data}`);
}

// Function returning void
const handler: (() => void) = () => {
  console.log("Handler called");
};

// Example 6.2: Never Type
// --------------------

// Function that never returns (throws error)
function error(message: string): never {
  throw new Error(message);
}

// Function with infinite loop
function infiniteLoop(): never {
  while (true) {
    // Never exits
  }
}

// Type narrowing to never (exhaustiveness check)
function exhaustive(value: "a" | "b" | "c"): string {
  switch (value) {
    case "a":
      return "A";
    case "b":
      return "B";
    case "c":
      return "C";
    default:
      const _exhaustive: never = value;
      return _exhaustive;
  }
}

// Example 6.3: Object Type
// ---------------------

// Non-primitive types (not string, number, boolean, symbol, null, undefined)
let obj: object = { name: "John" };
let arr: object = [1, 2, 3];
let func: object = function() {};

// Better to use specific object types
let betterObj: { name: string; age: number } = { name: "John", age: 30 };
let betterArr: number[] = [1, 2, 3];

// ============================================================================
// SECTION 7: TYPE CONVERSIONS AND COERCIONS
// ============================================================================

/**
 * Example 7.1: Number String Conversions
 * -----------------------------------
 */

// String to Number
let numStr = "42";
let parsedNum = Number(numStr); // 42
let intParsed = parseInt(numStr); // 42
let floatParsed = parseFloat("3.14"); // 3.14

// Number to String
let num = 42;
let strNum = String(num); // "42"
let toStr = num.toString(); // "42"
let withRadix = num.toString(16); // "2a" (hex)

// Example 7.2: Boolean Conversions
// ------------------------------

// Truthy/Falsy conversions
let truthy = Boolean("hello"); // true
let falsy = Boolean(""); // false
let numToBool = Boolean(1); // true
let zeroToBool = Boolean(0); // false

// Double negation (!!) for conversion
let boolFromStr = !!"test"; // true
let boolFromNum = !!0; // false

// ============================================================================
// SECTION 8: COMMON PITFALLS AND SOLUTIONS
// ============================================================================

/**
 * ⚠️ COMMON ISSUE 1: Implicit any in arithmetic
 * --------------------------------------------
 * Problem: Operations on uninitialized variables
 */

function fixArithmetic(): void {
  let x: number;
  // console.log(x + 5); // Error: Variable 'x' used before being assigned
  
  // Solution: Initialize
  let initialized = 0;
  console.log(initialized + 5); // OK
}

/**
 * ⚠️ COMMON ISSUE 2: String comparison pitfalls
 * -------------------------------------------
 */

function fixStringComparison(): void {
  // Case sensitivity
  let s1 = "Hello";
  let s2 = "hello";
  
  // === is case-sensitive
  console.log(s1 === s2); // false
  
  // For case-insensitive comparison
  console.log(s1.toLowerCase() === s2.toLowerCase()); // true
}

/**
 * ⚠️ COMMON ISSUE 3: NaN checking
 * ------------------------------
 */

function fixNaNChecking(): void {
  let result: number = parseInt("not a number");
  
  // NaN is not equal to itself!
  console.log(result === NaN); // false (wrong way)
  console.log(Number.isNaN(result)); // true (correct way)
  console.log(isNaN(result)); // true (also works)
}

// ============================================================================
// SECTION 9: PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance characteristics:
 * - Primitives are stored by value (not reference)
 * - String operations create new strings (immutable)
 * - Number operations are fast
 * - Symbol creation has slight overhead
 */

// Example: String builder pattern for performance
function buildLargeString(parts: string[]): string {
  // Bad for many concatenations: str += part
  // Good: Use array join
  return parts.join("");
}

console.log("\n=== Primitive Types Complete ===");
console.log("Next: FUNDAMENTALS/TYPES/03_Type_Aliases_and_Interfaces");