/**
 * Union and Intersection Types in TypeScript
 * 
 * Union types: A value can be one of several types (|)
 * Intersection types: A value must be all of several types (&)
 * 
 * Angular Connection: Used for:
 * - Flexible component inputs
 * - API response types
 * - Form field types
 * - Error handling
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== UNION AND INTERSECTION TYPES ==========\n");

// ============================================
// UNION TYPES
// ============================================

// String OR number
let stringOrNumber: string | number;
stringOrNumber = 'hello';  // OK
stringOrNumber = 42;       // OK
// stringOrNumber = true;  // Error!

console.log("1. Union types:");
console.log("   stringOrNumber:", stringOrNumber);

// Function parameter with union
function processValue(value: string | number): void {
    if (typeof value === 'string') {
        console.log("   String value:", value.toUpperCase());
    } else {
        console.log("   Number value:", value * 2);
    }
}

processValue('hello');
processValue(42);

// ============================================
// LITERAL UNION TYPES
// ============================================

type Direction = 'north' | 'south' | 'east' | 'west';

function move(direction: Direction): void {
    console.log("\n2. Literal union types:");
    console.log(`   Moving ${direction}`);
}

move('north');
move('south');
// move('northeast');  // Error!

// ============================================
// OPTIONAL WITH UNION
// ============================================

type MaybeString = string | undefined;
type MaybeNumber = number | null;

let name: MaybeString = 'John';
name = undefined;

let count: MaybeNumber = 100;
count = null;

console.log("\n3. Optional union:");
console.log("   name:", name);
console.log("   count:", count);

// ============================================
// INTERSECTION TYPES
// ============================================

interface Person {
    name: string;
    age: number;
}

interface Employee {
    employeeId: string;
    department: string;
}

// Must be BOTH Person AND Employee
type PersonEmployee = Person & Employee;

const personEmployee: PersonEmployee = {
    name: 'John',
    age: 30,
    employeeId: 'EMP001',
    department: 'Engineering'
};

console.log("\n4. Intersection types:");
console.log("   personEmployee:", personEmployee);

// ============================================
// COMBINING UNION AND INTERSECTION
// ============================================

interface A {
    a: string;
}

interface B {
    b: number;
}

interface C {
    c: boolean;
}

// Union of intersections
type Combined = (A & B) | (B & C);

const combined1: Combined = { a: 'hello', b: 42 };
const combined2: Combined = { b: 100, c: true };

console.log("\n5. Complex combination:");
console.log("   combined1:", combined1);
console.log("   combined2:", combined2);

// ============================================
// TYPE GUARDS
// ============================================

function isString(value: unknown): value is string {
    return typeof value === 'string';
}

function isNumber(value: unknown): value is number {
    return typeof value === 'number';
}

function process(value: string | number): void {
    if (isString(value)) {
        console.log("\n6. Type guards:");
        console.log("   String:", value.toUpperCase());
    } else if (isNumber(value)) {
        console.log("   Number:", value.toFixed(2));
    }
}

process('hello');
process(42.5);

// ============================================
// DISCRIMINATED UNIONS
// ============================================

interface SuccessResponse {
    status: 'success';
    data: string[];
}

interface ErrorResponse {
    status: 'error';
    error: string;
}

interface LoadingResponse {
    status: 'loading';
}

type ApiResponse = SuccessResponse | ErrorResponse | LoadingResponse;

function handleResponse(response: ApiResponse): void {
    console.log("\n7. Discriminated unions:");
    switch (response.status) {
        case 'success':
            console.log("   Data:", response.data);
            break;
        case 'error':
            console.log("   Error:", response.error);
            break;
        case 'loading':
            console.log("   Loading...");
            break;
    }
}

handleResponse({ status: 'success', data: ['item1', 'item2'] });
handleResponse({ status: 'error', error: 'Not found' });
handleResponse({ status: 'loading' });

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Flexible component input
interface BaseInput {
    label: string;
    required?: boolean;
}

interface TextInput extends BaseInput {
    type: 'text' | 'email' | 'password';
    placeholder?: string;
}

interface SelectInput extends BaseInput {
    type: 'select';
    options: { value: string; label: string }[];
}

type FormInput = TextInput | SelectInput;

function renderInput(input: FormInput): void {
    console.log("Form input:");
    console.log("   label:", input.label);
    
    if (input.type === 'select') {
        console.log("   options:", input.options);
    } else {
        console.log("   type:", input.type);
    }
}

renderInput({ label: 'Name', type: 'text', placeholder: 'Enter name' });
renderInput({ 
    label: 'Country', 
    type: 'select', 
    options: [{ value: 'us', label: 'USA' }] 
});

// Route parameters
type RouteParam = string | number;

function navigate(path: string, params: RouteParam | RouteParam[]): void {
    console.log("\nRoute navigation:");
    if (Array.isArray(params)) {
        console.log(`   ${path}/${params.join('/')}`);
    } else {
        console.log(`   ${path}/${params}`);
    }
}

navigate('/users', 123);
navigate('/products', ['category', 'electronics']);

// Error handling
interface HttpError {
    status: number;
    message: string;
}

type Result<T> = 
    | { success: true; data: T }
    | { success: false; error: HttpError };

function handleResult<T>(result: Result<T>): void {
    console.log("\nResult handling:");
    if (result.success) {
        console.log("   Success:", result.data);
    } else {
        console.log("   Error:", result.error.message);
    }
}

handleResult<string>({ success: true, data: 'Hello' });
handleResult<string>({ 
    success: false, 
    error: { status: 404, message: 'Not found' } 
});

// Form control value
type FormControlValue<T> = T | null | undefined;

let formValue: FormControlValue<string> = 'initial';
formValue = null;
formValue = undefined;

console.log("\nForm control:");
console.log("   value:", formValue);

console.log("\n========== SUMMARY ==========");
console.log("Union Types (|):");
console.log("- Value can be one of multiple types");
console.log("- Use '|' to separate types");
console.log("- Literal unions for specific values");
console.log("- Type guards for narrowing");
console.log("\nIntersection Types (&):");
console.log("- Value must be all types combined");
console.log("- Use '&' to combine types");
console.log("- Useful for mixing interfaces");
console.log("\nAngular Usage:");
console.log("- Flexible component inputs");
console.log("- API response types");
console.log("- Form field types");
console.log("- Route parameters");
console.log("\nDiscriminated Unions:");
console.log("- Common property for switching");
console.log("- Type-safe conditional logic");
console.log("================================\n");
