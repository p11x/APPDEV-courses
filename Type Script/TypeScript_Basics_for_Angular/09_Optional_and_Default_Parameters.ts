/**
 * Optional and Default Parameters in TypeScript
 * 
 * This file demonstrates how to make parameters optional and how to 
 * provide default values for parameters in TypeScript functions.
 * 
 * Angular Connection: Used extensively in Angular for:
 * - Component @Input properties (optional inputs)
 * - Service method parameters
 * - Route parameters
 * - Form control configurations
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== OPTIONAL AND DEFAULT PARAMETERS ==========\n");

// ============================================
// OPTIONAL PARAMETERS (?)
// ============================================

// Use ? after parameter name to make it optional
// Optional parameters must come after required parameters
function greet(name: string, greeting?: string): string {
    if (greeting) {
        return `${greeting}, ${name}!`;
    }
    return `Hello, ${name}!`;
}

// Optional parameter with default
function describePerson(name: string, age?: number): string {
    if (age !== undefined) {
        return `${name} is ${age} years old`;
    }
    return `${name}`;
}

console.log("1. Optional parameters:");
console.log("   greet('Alice'):", greet('Alice'));
console.log("   greet('Alice', 'Hi'):", greet('Alice', 'Hi'));
console.log("   describePerson('Bob'):", describePerson('Bob'));
console.log("   describePerson('Charlie', 30):", describePerson('Charlie', 30));

// ============================================
// DEFAULT PARAMETERS
// ============================================

// Provide default value using =
function createUser(name: string, role: string = 'user'): string {
    return `Created user: ${name} with role: ${role}`;
}

// Default can be any expression
function calculateTotal(price: number, tax: number = price * 0.1): number {
    return price + tax;
}

// Default parameter with optional parameter
function sendMessage(message: string, recipient: string = 'everyone', priority: string = 'normal'): string {
    return `Message: "${message}" to ${recipient} (${priority})`;
}

console.log("\n2. Default parameters:");
console.log("   createUser('John'):", createUser('John'));
console.log("   createUser('Admin', 'admin'):", createUser('Admin', 'admin'));
console.log("   calculateTotal(100):", calculateTotal(100));
console.log("   calculateTotal(100, 20):", calculateTotal(100, 20));
console.log("   sendMessage('Hello'):", sendMessage('Hello'));
console.log("   sendMessage('Urgent', 'Team', 'high'):", sendMessage('Urgent', 'Team', 'high'));

// ============================================
// OPTIONAL VS DEFAULT - WHEN TO USE EACH
// ============================================

console.log("\n3. Optional vs Default:");

// OPTIONAL (?): When the parameter might not be provided at all
// - Use when caller might omit the parameter entirely
// - Parameter becomes type | undefined
function processForm(data: string, validate?: boolean): void {
    const shouldValidate = validate ?? true;
    console.log(`   Processing "${data}", validate: ${shouldValidate}`);
}

// DEFAULT (=): When you want a specific default value
// - Use when you want a meaningful default
// - Caller can still pass undefined to use default
function configure(options: { timeout?: number; retries?: number } = {}): void {
    const timeout = options.timeout ?? 5000;
    const retries = options.retries ?? 3;
    console.log(`   Config: timeout=${timeout}ms, retries=${retries}`);
}

console.log("Optional (?):");
processForm('some data');
processForm('some data', false);

console.log("\nDefault (=):");
configure();
configure({ timeout: 10000 });
configure({ retries: 5 });

// ============================================
// REST PARAMETERS
// ============================================

// Collect remaining arguments into array
function sum(...numbers: number[]): number {
    return numbers.reduce((total, n) => total + n, 0);
}

function greetAll(greeting: string, ...names: string[]): string {
    return `${greeting}, ${names.join(', ')}!`;
}

console.log("\n4. Rest parameters:");
console.log("   sum(1, 2, 3, 4, 5):", sum(1, 2, 3, 4, 5));
console.log("   greetAll('Hello', 'A', 'B', 'C'):", greetAll('Hello', 'A', 'B', 'C'));

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Component Input - optional property
interface UserCardInputs {
    name: string;
    age?: number;           // Optional - might not always be provided
    showAvatar?: boolean;   // Optional - defaults to true if not specified
}

// Service method with defaults
class NotificationService {
    send(message: string, type: 'success' | 'error' | 'info' = 'info', duration: number = 3000): void {
        console.log(`   [${type.toUpperCase()}] ${message} (${duration}ms)`);
    }
}

// Using the service
const notifier = new NotificationService();
console.log("NotificationService:");
notifier.send('Operation successful');
notifier.send('Error occurred', 'error');
notifier.send('Warning', 'info', 5000);

// Route configuration with defaults
interface RouteConfig {
    path: string;
    component?: string;    // Optional - might be redirect
    redirectTo?: string;    // Optional - for redirects
    data?: object;         // Optional - route data
}

function configureRoute(config: RouteConfig): void {
    console.log(`   Route: ${config.path}`);
    if (config.component) {
        console.log(`     -> Component: ${config.component}`);
    }
    if (config.redirectTo) {
        console.log(`     -> Redirect to: ${config.redirectTo}`);
    }
}

console.log("\nRoute configuration:");
configureRoute({ path: '/home', component: 'HomeComponent' });
configureRoute({ path: '/old', redirectTo: '/new' });

// Form control configuration with defaults
interface FormControlConfig {
    value?: string;
    disabled?: boolean;
    validators?: string[];
}

function createFormControl(config: FormControlConfig = {}): void {
    const initialValue = config.value ?? '';
    const isDisabled = config.disabled ?? false;
    const validationRules = config.validators ?? [];
    console.log(`   FormControl: value="${initialValue}", disabled=${isDisabled}, validators=[${validationRules.join(', ')}]`);
}

console.log("\nForm control:");
createFormControl();
createFormControl({ value: 'Initial', disabled: true });
createFormControl({ validators: ['required', 'email'] });

console.log("\n========== SUMMARY ==========");
console.log("Optional Parameters (?:):");
console.log("- Use ? after parameter name");
console.log("- Parameter becomes type | undefined");
console.log("- Must come after required parameters");
console.log("- Use when parameter might not be provided");
console.log("\nDefault Parameters (=):");
console.log("- Use = value after parameter name");
console.log("- Parameter has a meaningful default");
console.log("- Caller can override with specific value");
console.log("- More explicit than optional parameters");
console.log("\nRest Parameters (...):");
console.log("- Collect multiple arguments into array");
console.log("- Must be last parameter");
console.log("- Useful for variadic functions");
console.log("================================\n");
