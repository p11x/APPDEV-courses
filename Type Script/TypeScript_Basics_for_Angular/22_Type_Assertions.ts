/**
 * Type Assertions in TypeScript
 * 
 * Type assertions tell TypeScript to treat a value as a specific type.
 * They're like type casting in other languages.
 * 
 * Angular Connection: Used for:
 * - Working with第三方 library types
 * - DOM manipulation
 * - API response handling
 * - Any/unknown type handling
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== TYPE ASSERTIONS ==========\n");

// ============================================
// AS SYNTAX
// ============================================

// Value of unknown type
let unknownValue: unknown = 'Hello TypeScript';

// Assert as string
let stringValue: string = unknownValue as string;
console.log("1. As syntax:");
console.log("   unknownValue as string:", stringValue);
console.log("   typeof:", typeof stringValue);

// Assert as number
let anyValue: any = 42;
let numberValue = anyValue as number;
console.log("   anyValue as number:", numberValue);

// ============================================
// ANGLE BRACKET SYNTAX
// ============================================

let unknownStr: unknown = 'TypeScript';
// Note: Can't use with JSX (React), prefer 'as' syntax
let strValue = <string>unknownStr;

console.log("\n2. Angle bracket syntax:");
console.log("   <string>unknownStr:", strValue);

// ============================================
// NARROWING WITH ASSERTIONS
// ============================================

function processValue(value: unknown): void {
    // Check and assert type
    if (typeof value === 'string') {
        // TypeScript narrows to string
        console.log("\n3. Type narrowing:");
        console.log("   String value:", value.toUpperCase());
    }
    
    if (typeof value === 'number') {
        console.log("   Number value:", value * 2);
    }
}

processValue('hello');
processValue(42);

// ============================================
// ASSERTING OBJECT TYPES
// ============================================

interface UserObj {
    name: string;
    age: number;
}

const data: unknown = {
    name: 'John',
    age: 30
};

// Assert as User
const user = data as UserObj;
console.log("\n4. Object assertion:");
console.log("   user.name:", user.name);
console.log("   user.age:", user.age);

// ============================================
// ASSERTING ARRAY TYPES
// ============================================

const arrayData: unknown = [1, 2, 3, 4, 5];
const numbersArr = arrayData as number[];

console.log("\n5. Array assertion:");
console.log("   numbersArr:", numbersArr);
console.log("   sum:", numbersArr.reduce((a, b) => a + b, 0));

// ============================================
// ASSERTING TO CONST (CONST ASSERTIONS)
// ============================================

// Makes values readonly and literal types
let colors = ['red', 'green', 'blue'] as const;
// Now colors is readonly and type is: readonly ['red', 'green', 'blue']

console.log("\n6. Const assertion:");
console.log("   colors:", colors);
// colors.push('yellow');  // Error! - readonly

// ============================================
// NON-NULL ASSERTION
// ============================================

interface MaybeUser {
    name?: string;
}

const maybeUser: MaybeUser = { name: 'John' };
// Assert not null/undefined
const userName = maybeUser.name!;

console.log("\n7. Non-null assertion:");
console.log("   userName:", userName);

// ============================================
// TYPE GUARDS
// ============================================

function isString(value: unknown): value is string {
    return typeof value === 'string';
}

function isNumber(value: unknown): value is number {
    return typeof value === 'number';
}

function processUnknown(value: unknown): void {
    if (isString(value)) {
        console.log("\n8. Type guard:");
        console.log("   Uppercase:", value.toUpperCase());
    } else if (isNumber(value)) {
        console.log("   Doubled:", value * 2);
    }
}

processUnknown('hello');
processUnknown(100);

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// DOM element handling
// In real Angular, use @ViewChild with proper types
function handleElement(element: unknown): void {
    const el = element as HTMLElement;
    console.log("DOM element:");
    console.log("   tagName:", el.tagName);
    console.log("   textContent:", el.textContent);
}

// Simulated DOM element
const mockElement = {
    tagName: 'DIV',
    textContent: 'Hello World'
};
handleElement(mockElement);

// API response handling
interface ApiData {
    users: { id: number; name: string }[];
}

const response: unknown = {
    users: [
        { id: 1, name: 'Alice' },
        { id: 2, name: 'Bob' }
    ]
};

console.log("\nAPI response:");
const apiData = response as ApiData;
console.log("   users:", apiData.users);

// Working with any
function parseJson<T>(json: string): T {
    return JSON.parse(json) as T;
}

const userJson = '{"id":1,"name":"John"}';
const parsed = parseJson<{ id: number; name: string }>(userJson);

console.log("\nJSON parsing:");
console.log("   parsed:", parsed);

// Asserting union types
type StringOrNumber = string | number;

function handleUnion(value: StringOrNumber): void {
    if (typeof value === 'string') {
        console.log("\nUnion handling:");
        console.log("   String:", value.length);
    } else {
        console.log("   Number:", value.toFixed(2));
    }
}

handleUnion('hello');
handleUnion(42.123);

console.log("\n========== SUMMARY ==========");
console.log("Type Assertions:");
console.log("- 'as' syntax: value as Type");
console.log("- Angle brackets: <Type>value");
console.log("- Non-null: value!");
console.log("- Const: as const");
console.log("\nBest Practices:");
console.log("- Use assertions sparingly");
console.log("- Prefer type guards when possible");
console.log("- Be careful with 'any' - avoid if possible");
console.log("\nAngular Usage:");
console.log("- DOM element types");
console.log("- API response typing");
console.log("- Working with external libraries");
console.log("- JSON parsing");
console.log("================================\n");
