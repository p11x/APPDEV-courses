/**
 * Functions in TypeScript
 * 
 * This file demonstrates function declarations with proper type annotations
 * for parameters and return types.
 * 
 * Angular Connection: Functions are used extensively in Angular:
 * - Component methods (event handlers, lifecycle hooks)
 * - Service methods (API calls, business logic)
 * - Pipe transform methods
 * - Form validation functions
 */

// Make this a module to avoid global scope conflicts
export {};

// ============================================
// FUNCTION WITH TYPE ANNOTATIONS
// ============================================

// Parameter types and return type explicitly annotated
function add(a: number, b: number): number {
    return a + b;
}

function greet(name: string): string {
    return "Hello, " + name + "!";
}

function isAdult(age: number): boolean {
    return age >= 18;
}

console.log("========== FUNCTION EXAMPLES ==========\n");

console.log("1. Basic function with types:");
console.log("   add(5, 3):", add(5, 3));
console.log("   greet('Alice'):", greet('Alice'));
console.log("   isAdult(25):", isAdult(25));

// ============================================
// VOID RETURN TYPE
// ============================================

// Functions that don't return anything use void
function logMessage(message: string): void {
    console.log("   Log:", message);
    // No return statement, or returns nothing
}

function updateCounter(counter: number): void {
    console.log("   Updating counter to:", counter);
    // Doesn't return anything useful
}

console.log("\n2. Void return type:");
logMessage("This is a log message");
updateCounter(10);

// ============================================
// MULTIPLE PARAMETER TYPES
// ============================================

// Function with different parameter types
function describePerson(name: string, age: number, isStudent: boolean): string {
    return `${name} is ${age} years old and is ${isStudent ? 'a student' : 'not a student'}`;
}

// Function that can accept different types using union (covered later)
function printValue(value: string | number): void {
    console.log("   Value:", value);
}

console.log("\n3. Multiple parameter types:");
console.log("   describePerson:", describePerson("John", 20, true));
printValue("hello");
printValue(42);

// ============================================
// FUNCTION TYPE ANNOTATIONS
// ============================================

// You can define types for functions
type MathOperation = (a: number, b: number) => number;

// Use the function type
let addOperation: MathOperation = function(a: number, b: number): number {
    return a + b;
};

let multiplyOperation: MathOperation = (a: number, b: number): number => a * b;

console.log("\n4. Function type annotations:");
console.log("   addOperation(4, 5):", addOperation(4, 5));
console.log("   multiplyOperation(4, 5):", multiplyOperation(4, 5));

// ============================================
// FUNCTION INTERFACES (more on this later)
// ============================================

interface Calculator {
    (a: number, b: number): number;
}

let subtract: Calculator = (a, b) => a - b;

console.log("\n5. Function interface:");
console.log("   subtract(10, 3):", subtract(10, 3));

// ============================================
// OPTIONAL PARAMETERS (preview)
// ============================================

// Parameters with ? are optional
function createUser(name: string, age?: number): string {
    if (age !== undefined) {
        return `${name} is ${age} years old`;
    }
    return `${name} (age unknown)`;
}

console.log("\n6. Optional parameters:");
console.log("   createUser('Alice'):", createUser('Alice'));
console.log("   createUser('Bob', 30):", createUser('Bob', 30));

// ============================================
// ARROW FUNCTIONS (preview)
// ============================================

const double = (n: number): number => n * 2;
const sayHello = (name: string): string => `Hello, ${name}!`;

console.log("\n7. Arrow functions:");
console.log("   double(7):", double(7));
console.log("   sayHello('World'):", sayHello('World'));

// ============================================
// ANGULAR-STYLE EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Simulating an Angular component method
class UserComponent {
    // Property
    userName: string = "Guest";
    
    // Method with proper typing
    getGreeting(): string {
        return `Welcome, ${this.userName}!`;
    }
    
    // Event handler (void return)
    onButtonClick(message: string): void {
        console.log("   Button clicked:", message);
    }
    
    // Method with multiple types
    processUserInput(input: string | number): void {
        console.log("   Processing:", input);
    }
}

const component = new UserComponent();
console.log("UserComponent methods:");
console.log("   getGreeting():", component.getGreeting());
component.onButtonClick("Save clicked");
component.processUserInput("some input");
component.processUserInput(123);

// Simulating an Angular service method
class DataService {
    // Method returning array
    getUsers(): string[] {
        return ["Alice", "Bob", "Charlie"];
    }
    
    // Method returning object
    getConfig(): { apiUrl: string; timeout: number } {
        return {
            apiUrl: "https://api.example.com",
            timeout: 5000
        };
    }
}

const dataService = new DataService();
console.log("\nDataService methods:");
console.log("   getUsers():", dataService.getUsers());
console.log("   getConfig():", dataService.getConfig());

console.log("\n========== SUMMARY ==========");
console.log("Functions in TypeScript:");
console.log("- Always specify parameter types");
console.log("- Always specify return type (especially void)");
console.log("- Use arrow functions for short functions");
console.log("- Function types can be named and reused");
console.log("- Angular uses typed functions throughout");
console.log("================================\n");
