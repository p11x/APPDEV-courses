/**
 * Type Inference in TypeScript
 * 
 * TypeScript can automatically determine the type of a variable based on
 * its initial value. This is called type inference.
 * 
 * Angular Connection: TypeScript infers types throughout Angular code:
 * - Variable declarations with initial values
 * - Function return types from return statements
 * - Array types from initial values
 * - Object property types
 */

// Make this a module to avoid global scope conflicts
export {}

console.log("========== TYPE INFERENCE ==========\n");

// ============================================
// VARIABLE TYPE INFERENCE
// ============================================
// When you assign a value, TypeScript infers the type

let inferredString = "Hello";    // TypeScript infers: string
let inferredNumber = 42;         // TypeScript infers: number
let inferredBoolean = true;     // TypeScript infers: boolean
let inferredArray = [1, 2, 3];  // TypeScript infers: number[]

console.log("Variable type inference:");
console.log("  inferredString:", inferredString, "- Type:", typeof inferredString);
console.log("  inferredNumber:", inferredNumber, "- Type:", typeof inferredNumber);
console.log("  inferredBoolean:", inferredBoolean, "- Type:", typeof inferredBoolean);
console.log("  inferredArray:", inferredArray);

// Once inferred, the type is locked (type safety)
let inferred = "text";
// inferred = 123;  // Error! TypeScript knows it's a string

// ============================================
// FUNCTION RETURN TYPE INFERENCE
// ============================================
// TypeScript infers return types from return statements

function add(a: number, b: number) {
    return a + b;  // TypeScript infers: number return type
}

function greet(name: string) {
    return "Hello, " + name;  // TypeScript infers: string return type
}

function getFirstItem(items: string[]) {
    return items[0];  // TypeScript infers: string | undefined
}

console.log("\nFunction return type inference:");
console.log("  add(5, 3):", add(5, 3));
console.log("  greet('World'):", greet('World'));
console.log("  getFirstItem(['a', 'b']):", getFirstItem(['a', 'b']));

// ============================================
// ARRAY TYPE INFERENCE
// ============================================
// TypeScript infers array types from initial values

let fruits = ["apple", "banana", "orange"];  // Type: string[]
let numbersList = [1, 2, 3, 4, 5];               // Type: number[]
let mixed = [1, "two", 3];                    // Type: (string | number)[]

console.log("\nArray type inference:");
console.log("  fruits:", fruits, "- Type: string[]");
console.log("  numbersList:", numbersList, "- Type: number[]");
console.log("  mixed:", mixed);

// ============================================
// OBJECT TYPE INFERENCE
// ============================================
// TypeScript infers object property types

let person = {
    name: "John",
    age: 30,
    isEmployed: true
};
// TypeScript infers: { name: string; age: number; isEmployed: boolean }

console.log("\nObject type inference:");
console.log("  person:", person);

// ============================================
// WHEN EXPLICIT ANNOTATIONS ARE NEEDED
// ============================================

// 1. Variable declared without initialization
let notInitialized: string;  // Must use annotation - no value to infer from
notInitialized = "Now I have a value";

console.log("\nExplicit annotation needed:");
console.log("  notInitialized:", notInitialized);

// 2. Function parameters need explicit types (cannot be inferred)
function processUser(name: string, age: number) {
    return `${name} is ${age} years old`;
}
console.log("  processUser('Alice', 25):", processUser("Alice", 25));

// 3. When you want a different type than inferred
let inferredAsNumber = 42;
// let forceAsString: string = inferredAsNumber; // Error if types don't match

// 4. When inferring would be 'any' (avoid 'any')
// Bad: let anything = getUnknownValue();  // Inferred as any
// Good: let specific: string = getUnknownValue();  // Explicit type

// ============================================
// BEST PRACTICES
// ============================================

// Use inference when type is obvious from value
let count = 0;                    // Clearly a number
let message = "Hello";            // Clearly a string
let items: string[] = [];         // Need annotation for empty array
let config = {                    // Object literal - inference works well
    apiUrl: "https://api.example.com",
    timeout: 5000
};

// Use explicit annotations when:
// - No initial value
// - Type isn't obvious
// - You want different type than inferred

console.log("\n========== SUMMARY ==========");
console.log("Type Inference:");
console.log("- TypeScript automatically determines types from values");
console.log("- Works with variables, functions, arrays, and objects");
console.log("- Use explicit annotations when:");
console.log("  * No initial value");
console.log("  * Type isn't obvious from context");
console.log("  * You want to ensure specific type");
console.log("- Let TypeScript infer when type is clear");
console.log("================================\n");
