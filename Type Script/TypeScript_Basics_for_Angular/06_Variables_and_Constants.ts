/**
 * Variables and Constants in TypeScript
 * 
 * This file demonstrates the differences between let, const, and var
 * in TypeScript and JavaScript.
 * 
 * Angular Connection: Angular best practices recommend:
 * - Use 'const' for values that won't change (most cases)
 * - Use 'let' for values that will be reassigned
 * - Avoid 'var' completely in modern TypeScript
 */

// Make this a module to avoid global scope conflicts
export {}

console.log("========== VARIABLES AND CONSTANTS ==========\n");

// ============================================
// LET - Block-scoped variable
// ============================================
// 'let' is the modern way to declare variables that can be reassigned
// Block-scoped (only accessible within the block where defined)

let count = 0;
count = 5;  // Can reassign
console.log("Using 'let':");
console.log("  Initial value:", count);

// Example of block scoping
if (true) {
    let blockVariable = "I'm inside the block";
    console.log("  Inside block:", blockVariable);
}
// console.log(blockVariable);  // Error! Not accessible outside block

// ============================================
// CONST - Constants that cannot be reassigned
// ============================================
// Use 'const' for values that won't change after initialization
// Also block-scoped like 'let'

const appName = "My Angular App";
const maxRetries = 3;
// maxRetries = 5;  // Error! Cannot reassign a const

console.log("\nUsing 'const':");
console.log("  appName:", appName);
console.log("  maxRetries:", maxRetries);

// const with objects - the reference can't change, but properties can!
const user = {
    name: "John",
    age: 30
};

user.age = 31;  // This is OK - modifying property, not reassigning
// user = { name: "Jane" };  // Error! Cannot reassign

console.log("  Modified user:", user);

// ============================================
// VAR - Function-scoped (legacy, avoid in modern code)
// ============================================
// 'var' is the old JavaScript way (before ES6)
// Function-scoped, not block-scoped - can cause bugs!

var oldStyle = "I'm using var";

function testVar() {
    var functionScoped = "Only exists in this function";
    console.log("  Inside function:", functionScoped);
}
testVar();
// console.log(functionScoped);  // Error! Not accessible

// Problem with var - hoisting and function scope
console.log("\nvar issues (demonstration):");
var myVar = "value";
console.log("  After declaration:", myVar);

// This wouldn't happen with let/const - they'd give ReferenceError

// ============================================
// COMPARISON EXAMPLES
// ============================================

console.log("\n========== COMPARISON ==========\n");

// Example 1: Loop variable
console.log("1. Loop variable:");
for (let i = 0; i < 3; i++) {
    console.log("  i =", i);  // i is block-scoped
}
// console.log(i);  // Error! i doesn't exist outside the loop

// Example 2: Multiple declarations
let x = 10;
let y = 20;
console.log("2. Multiple declarations:");
console.log("  x =", x, ", y =", y);

// Example 3: Constant with objects
const config = {
    apiUrl: "https://api.example.com",
    timeout: 5000,
    retryCount: 3
};
console.log("3. Const with object:");
console.log("  config:", config);

// Modifying object properties (allowed)
config.timeout = 10000;
console.log("  After modification:", config);

// Example 4: Constants for enums (common pattern in Angular)
const enum Status {
    Pending = "PENDING",
    Active = "ACTIVE",
    Completed = "COMPLETED"
}
const currentStatus = Status.Active;
console.log("4. Status constant:", currentStatus);

// ============================================
// BEST PRACTICES FOR ANGULAR
// ============================================

// DO: Use const for almost everything
const API_BASE_URL = "https://api.example.com";
const DEFAULT_LANGUAGE = "en";
const MAX_UPLOAD_SIZE = 5 * 1024 * 1024;  // 5MB

// DO: Use let when value must change
let itemCount = 0;
itemCount = itemCount + 1;

// DON'T: Use var in modern TypeScript
// var legacy = "avoid this";

// DO: Use UPPER_SNAKE_CASE for constants
const CONSTANT_VALUE = "some value";

// Type annotations with const
const userName: string = "Alice";  // Explicit type with const
const userAge: number = 25;

console.log("\n========== SUMMARY ==========");
console.log("Variable declarations:");
console.log("- const: Cannot be reassigned, use for constants");
console.log("- let: Can be reassigned, block-scoped");
console.log("- var: Legacy, function-scoped, avoid in modern code");
console.log("- const with objects: reference is constant, properties can change");
console.log("\nAngular Best Practices:");
console.log("- Prefer const over let for immutable values");
console.log("- Use const for service injections, component configs");
console.log("- Use let only for counters, toggles that change");
console.log("- Never use var in modern Angular code");
console.log("================================\n");
