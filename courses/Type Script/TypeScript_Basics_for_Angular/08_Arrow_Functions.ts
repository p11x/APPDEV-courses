/**
 * Arrow Functions in TypeScript
 * 
 * Arrow functions provide a concise syntax and lexical 'this' binding.
 * They are extensively used in Angular for callbacks, event handlers,
 * and functional programming patterns.
 * 
 * Angular Connection: Arrow functions are used in:
 * - Template expressions
 * - Array operations (map, filter, reduce)
 * - Observables and subscriptions
 * - Event handlers
 * - HTTP callback handling
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== ARROW FUNCTIONS ==========\n");

// ============================================
// BASIC ARROW FUNCTION SYNTAX
// ============================================

// Traditional function
function addTraditional(a: number, b: number): number {
    return a + b;
}

// Arrow function equivalent
const addArrow = (a: number, b: number): number => {
    return a + b;
};

// Shortened arrow function (implicit return)
const addShort = (a: number, b: number) => a + b;

console.log("1. Basic arrow functions:");
console.log("   addTraditional(3, 4):", addTraditional(3, 4));
console.log("   addArrow(3, 4):", addArrow(3, 4));
console.log("   addShort(3, 4):", addShort(3, 4));

// ============================================
// SINGLE PARAMETER - PARENTHESES OPTIONAL
// ============================================

// With parentheses
const double1 = (n: number): number => n * 2;

// Without parentheses (only when single param)
const double2 = (n: number) => n * 2;

console.log("\n2. Single parameter:");
console.log("   double1(5):", double1(5));
console.log("   double2(5):", double2(5));

// ============================================
// NO PARAMETERS
// ============================================

const getRandomNumber = () => Math.random();

console.log("\n3. No parameters:");
console.log("   getRandomNumber():", getRandomNumber());

// ============================================
// MULTIPLE STATEMENTS NEED BRACES AND RETURN
// ============================================

const processUser = (name: string, age: number): string => {
    const greeting = `Hello, ${name}!`;
    const info = `You are ${age} years old.`;
    return `${greeting} ${info}`;
};

console.log("\n4. Multiple statements:");
console.log("   processUser('Alice', 25):", processUser('Alice', 25));

// ============================================
// 'THIS' BEHAVIOR - KEY DIFFERENCE
// ============================================

console.log("\n5. 'this' behavior:");

// Regular function - 'this' depends on how called
class RegularFunction {
    value: number = 0;
    
    // Using regular function method
    incrementRegular() {
        // Regular function inside method - 'this' could be lost in callback
        console.log("   Regular function 'this' context varies");
    }
}

// Arrow function - 'this' is lexically bound
class ArrowFunctionExample {
    value: number = 0;
    
    // Arrow function method - 'this' always refers to the instance
    incrementArrow = () => {
        this.value++;
        console.log("   Arrow function 'this' always refers to class instance");
    }
}

const regular = new RegularFunction();
regular.incrementRegular();

const arrow = new ArrowFunctionExample();
arrow.incrementArrow();

// ============================================
// PRACTICAL EXAMPLES
// ============================================

console.log("\n6. Practical examples:");

// Array methods with arrow functions
const numbers = [1, 2, 3, 4, 5];

// Map - transform each item
const doubled = numbers.map((n: number) => n * 2);
console.log("   numbers.map(n => n * 2):", doubled);

// Filter - keep items that pass test
const evens = numbers.filter((n: number) => n % 2 === 0);
console.log("   numbers.filter(n => n % 2 === 0):", evens);

// Reduce - accumulate to single value
const sum = numbers.reduce((acc: number, n: number) => acc + n, 0);
console.log("   numbers.reduce((acc, n) => acc + n, 0):", sum);

// Find - get first matching item
const firstBig = numbers.find((n: number) => n > 3);
console.log("   numbers.find(n => n > 3):", firstBig);

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Simulating Angular template expression
class ProductListComponent {
    products: string[] = ['Laptop', 'Phone', 'Tablet'];
    
    // Arrow function for template display
    getProductNames = (): string => {
        return this.products.join(', ');
    };
    
    // Arrow function for click handler
    onProductClick = (product: string): void => {
        console.log("   Product clicked:", product);
    };
}

const productList = new ProductListComponent();
console.log("ProductListComponent:");
console.log("   getProductNames():", productList.getProductNames());
productList.onProductClick('Phone');

// Simulating Observable subscription (common in Angular)
interface ApiResponse {
    data: string;
}

// Simulating HTTP callback pattern
const fetchData = (callback: (response: ApiResponse) => void): void => {
    // In real Angular, this would be HttpClient.get()
    callback({ data: "Sample data" });
};

console.log("\nSimulating HTTP callback:");
fetchData((response: ApiResponse) => {
    console.log("   Response received:", response.data);
});

console.log("\n========== SUMMARY ==========");
console.log("Arrow Functions:");
console.log("- Concise syntax: (params) => expression");
console.log("- Single param can omit parentheses");
console.log("- Lexical 'this' binding (key advantage)");
console.log("- Perfect for array methods (map, filter, reduce)");
console.log("- Used extensively in Angular templates & callbacks");
console.log("- Cannot be used as object constructors");
console.log("================================\n");
