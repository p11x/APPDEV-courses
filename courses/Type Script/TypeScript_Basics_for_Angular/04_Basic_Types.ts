/**
 * Basic Types in TypeScript
 * 
 * This file demonstrates the fundamental types available in TypeScript.
 * Understanding these basic types is essential for Angular development,
 * as they form the foundation for all type annotations you'll use.
 * 
 * Angular Connection: Basic types are used throughout Angular for:
 * - Component property types
 * - Service method return types
 * - Function parameters in services
 * - Template variable types
 */

// Make this a module to avoid global scope conflicts
export {}

// ============================================
// STRING TYPE
// ============================================
// Used for text data
// In Angular: Component titles, user names, route paths, template strings

let userName: string = "John Doe";
let greeting: string = "Hello, welcome to TypeScript!";
let templateString: string = `User: ${userName}`; // Template literals work  too

console.log("String examples:");
console.log("  userName:", userName);
console.log("  greeting:", greeting);
console.log("  templateString:", templateString);

// ============================================
// NUMBER TYPE
// ============================================
// Used for all numeric values (integers and decimals)
// In Angular: Prices, quantities, coordinates, indices

let age: number = 25;
let price: number = 19.99;
let negative: number = -100;
let hex: number = 0xFF;  // Hexadecimal is also a number
let binary: number = 0b1010;  // Binary notation

console.log("\nNumber examples:");
console.log("  age:", age);
console.log("  price:", price);
console.log("  negative:", negative);
console.log("  hex:", hex);
console.log("  binary:", binary);

// ============================================
// BOOLEAN TYPE
// ============================================
// Used for true/false values
// In Angular: Toggle states, form validity, loading flags

let isActive: boolean = true;
let isLoggedIn: boolean = false;
let hasPermission: boolean = Boolean(1);  // Can use Boolean() conversion

console.log("\nBoolean examples:");
console.log("  isActive:", isActive);
console.log("  isLoggedIn:", isLoggedIn);
console.log("  hasPermission:", hasPermission);

// ============================================
// ANY TYPE
// ============================================
// Disables type checking - use sparingly!
// In Angular: Sometimes needed when working with third-party libraries
// or when migrating from JavaScript

let anything: any = "Hello";
anything = 42;  // No error - any type allows any value
anything = true;
anything = { name: "Object", value: 100 };

// Any array can hold anything
let mixedArray: any[] = ["string", 123, true, { key: "value" }];

console.log("\nAny type examples:");
console.log("  anything:", anything);
console.log("  mixedArray:", mixedArray);

// WARNING: Avoid 'any' when possible - it defeats TypeScript's type safety

// ============================================
// VOID TYPE
// ============================================
// Represents absence of a return value
// In Angular: Event handlers, callback functions that don't return values

function logMessage(message: string): void {
    console.log("  Logging:", message);
    // This function doesn't return anything
}

let result: void;  // Can only be undefined or null for void variables
result = undefined;  // Valid
// result = "something";  // Error!

console.log("\nVoid type examples:");
logMessage("This is a void function");

// ============================================
// NEVER TYPE
// ============================================
// Represents values that never occur
// In Angular: Functions that always throw errors or have infinite loops

function throwError(message: string): never {
    throw new Error(message);  // Function never returns
}

function infiniteLoop(): never {
    while (true) {
        // This loop never ends
    }
}

// This function returns never because it always throws
function alwaysThrows(): never {
    throw new Error("This function always throws");
}

console.log("\nNever type examples:");
try {
    // Uncomment to see the error
    // throwError("This is a test error");
    console.log("  (throwError commented out to prevent crash)");
} catch (e) {
    console.log("  Caught the error:", e);
}

// ============================================
// ARRAYS
// ============================================
// Typed arrays in TypeScript
// In Angular: Lists of items, form controls, HTTP responses

let numbers: number[] = [1, 2, 3, 4, 5];
let strings: string[] = ["apple", "banana", "orange"];
let booleans: boolean[] = [true, false, true];

// Alternative syntax using generic notation
let numbersAlt: Array<number> = [1, 2, 3];

console.log("\nArray examples:");
console.log("  numbers:", numbers);
console.log("  strings:", strings);
console.log("  booleans:", booleans);

// ============================================
// ENUM (Preview - covered in detail later)
// ============================================
// Named constants
// In Angular: Application states, user roles, route definitions

enum Role {
    Admin = "ADMIN",
    User = "USER",
    Guest = "GUEST"
}

let userRole: Role = Role.User;

console.log("\nEnum example:");
console.log("  userRole:", userRole);

// ============================================
// SUMMARY
// ============================================
console.log("\n========== SUMMARY ==========");
console.log("Basic types in TypeScript:");
console.log("- string: Text values");
console.log("- number: Numeric values (int, float, hex, binary)");
console.log("- boolean: true/false values");
console.log("- any: Opt-out of type checking (avoid when possible)");
console.log("- void: No return value");
console.log("- never: Never returns (always throws or infinite loop)");
console.log("- Arrays: Typed lists of values");
console.log("================================\n");
