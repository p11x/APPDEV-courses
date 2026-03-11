/**
 * Arrays and Tuples in TypeScript
 * 
 * This file demonstrates typed arrays and tuples in TypeScript.
 * 
 * Angular Connection: Arrays and tuples are used extensively for:
 * - Lists of items (ngFor directive)
 * - FormControl arrays
 * - Route parameter arrays
 * - API response data
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== ARRAYS AND TUPLES ==========\n");

// ============================================
// TYPED ARRAYS
// ============================================

// Array of numbers
let numbers: number[] = [1, 2, 3, 4, 5];

// Array of strings
let fruits: string[] = ['apple', 'banana', 'orange'];

// Array of booleans
let flags: boolean[] = [true, false, true];

// Generic array syntax (alternative)
let scores: Array<number> = [100, 95, 87];

console.log("1. Typed arrays:");
console.log("   numbers:", numbers);
console.log("   fruits:", fruits);
console.log("   flags:", flags);
console.log("   scores:", scores);

// ============================================
// ARRAY METHODS WITH TYPES
// ============================================

let items: string[] = ['first', 'second', 'third'];

// Push - add item to end
items.push('fourth');

// Pop - remove last item
const removed = items.pop();

// Map - transform each item
let upperItems: string[] = items.map((item: string) => item.toUpperCase());

// Filter - keep matching items
let filtered: string[] = items.filter((item: string) => item.length > 5);

// Find - get first match
let found: string | undefined = items.find((item: string) => item.startsWith('f'));

// Reduce - accumulate to single value
let combined: string = items.reduce((acc: string, item: string) => acc + item, '');

console.log("\n2. Array methods:");
console.log("   items:", items);
console.log("   upperItems:", upperItems);
console.log("   filtered (length > 5):", filtered);
console.log("   found:", found);
console.log("   combined:", combined);

// ============================================
// ARRAY TYPES IN FUNCTIONS
// ============================================

function sumArray(numbers: number[]): number {
    return numbers.reduce((total, n) => total + n, 0);
}

function findIndex(items: string[], searchItem: string): number {
    return items.indexOf(searchItem);
}

function filterByPrefix(items: string[], prefix: string): string[] {
    return items.filter(item => item.startsWith(prefix));
}

console.log("\n3. Functions with arrays:");
console.log("   sumArray([1,2,3,4,5]):", sumArray([1, 2, 3, 4, 5]));
console.log("   findIndex(['a','b','c'], 'b'):", findIndex(['a', 'b', 'c'], 'b'));
console.log("   filterByPrefix(['foo','bar','fox'], 'f'):", filterByPrefix(['foo', 'bar', 'fox'], 'f'));

// ============================================
// TUPLES
// ============================================

// Tuple - fixed-length array with specific types
let person: [string, number] = ['John', 30];
let coordinates: [number, number] = [10.5, 20.3];
let rgb: [number, number, number] = [255, 128, 0];

// Accessing tuple elements
const name = person[0];  // 'John'
const age = person[1];    // 30

console.log("\n4. Tuples:");
console.log("   person:", person);
console.log("   coordinates:", coordinates);
console.log("   rgb:", rgb);
console.log("   name:", name, ", age:", age);

// Tuple with optional elements (TypeScript 4.0+)
let optionalTuple: [string, number?] = ['hello'];
optionalTuple = ['world', 42];  // Both are valid

console.log("   optionalTuple:", optionalTuple);

// ============================================
// TUPLES IN FUNCTIONS
// ============================================

// Function returning a tuple
function getMinMax(numbers: number[]): [number, number] {
    return [Math.min(...numbers), Math.max(...numbers)];
}

// Function taking tuple parameter
function displayPoint(point: [number, number]): void {
    console.log(`   Point: (${point[0]}, ${point[1]})`);
}

const minMax = getMinMax([5, 2, 8, 1, 9]);

console.log("\n5. Tuple functions:");
console.log("   getMinMax([5,2,8,1,9]):", minMax);
displayPoint([15, 30]);

// ============================================
// READONLY ARRAYS
// ============================================

// Cannot modify the array
const readonlyArray: readonly string[] = ['a', 'b', 'c'];
// readonlyArray.push('d');  // Error!
// readonlyArray[0] = 'x';  // Error!

// Readonly array type
function processReadonly(items: readonly number[]): number {
    // Cannot modify items
    return items.reduce((a, b) => a + b, 0);
}

console.log("\n6. Readonly arrays:");
console.log("   readonlyArray:", readonlyArray);
console.log("   processReadonly([1,2,3]):", processReadonly([1, 2, 3]));

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// User list - common in Angular
interface User {
    id: number;
    name: string;
}

let users: User[] = [
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' }
];

// Adding to array (common in Angular)
users.push({ id: 3, name: 'Charlie' });

console.log("User list:");
console.log("   users:", users);

// Route parameters as tuple
type RouteParams = [string, string?];  // path, id (optional)

function navigate(route: RouteParams): void {
    console.log(`   Navigating to: ${route[0]}`, route[1] ? `/${route[1]}` : '');
}

navigate(['/users']);
navigate(['/users', '123']);

// Form array example
interface FormField {
    name: string;
    value: string;
}

let formFields: FormField[] = [
    { name: 'username', value: '' },
    { name: 'email', value: '' }
];

console.log("\nForm fields:");
console.log("   formFields:", formFields);

// HTTP response handling
interface ApiResponse<T> {
    data: T;
    status: number;
}

let responses: ApiResponse<string[]>[] = [
    { data: ['item1', 'item2'], status: 200 },
    { data: ['item3'], status: 201 }
];

console.log("\nAPI responses:");
console.log("   responses:", responses);

console.log("\n========== SUMMARY ==========");
console.log("Arrays:");
console.log("- Type[] or Array<Type> syntax");
console.log("- All elements must match the type");
console.log("- Array methods (map, filter, reduce) preserve types");
console.log("- Use readonly for immutable arrays");
console.log("\nTuples:");
console.log("- Fixed-length arrays with specific types");
console.log("- [type1, type2, ...] syntax");
console.log("- Each position has specific type");
console.log("- Useful for return values and pairs");
console.log("\nAngular usage:");
console.log("- Arrays for ngFor loops");
console.log("- Tuples for route parameters");
console.log("- Readonly for inputs");
console.log("================================\n");
