/**
 * Objects and Type Aliases in TypeScript
 * 
 * This file demonstrates object types and type aliases for creating
 * reusable type definitions.
 * 
 * Angular Connection: Used extensively for:
 * - Data models and DTOs
 * - Component input/output types
 * - Service response types
 * - Configuration objects
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== OBJECTS AND TYPE ALIASES ==========\n");

// ============================================
// TYPE ALIASES
// ============================================

// Create a reusable type alias
type UserId = number;
type UserName = string;
type UserEmail = string;

// Use the alias
let userId: UserId = 123;
let userName: UserName = 'John';

console.log("1. Type aliases:");
console.log("   userId:", userId);
console.log("   userName:", userName);

// ============================================
// OBJECT TYPE ANNOTATIONS
// ============================================

// Inline object type
let person: { name: string; age: number } = {
    name: 'Alice',
    age: 30
};

console.log("\n2. Inline object type:");
console.log("   person:", person);

// ============================================
// TYPE ALIAS FOR OBJECTS
// ============================================

// Define object shape with type alias
type User = {
    id: number;
    name: string;
    email: string;
    isActive: boolean;
};

// Use the type alias
let user: User = {
    id: 1,
    name: 'John Doe',
    email: 'john@example.com',
    isActive: true
};

console.log("\n3. Type alias for objects:");
console.log("   user:", user);

// ============================================
// OPTIONAL PROPERTIES
// ============================================

type Product = {
    id: number;
    name: string;
    price: number;
    description?: string;  // Optional property
    category?: string;    // Optional property
};

let product1: Product = {
    id: 1,
    name: 'Laptop',
    price: 999
};

let product2: Product = {
    id: 2,
    name: 'Phone',
    price: 699,
    description: 'Smartphone',
    category: 'Electronics'
};

console.log("\n4. Optional properties:");
console.log("   product1:", product1);
console.log("   product2:", product2);

// ============================================
// READONLY PROPERTIES
// ============================================

type Config = {
    readonly apiUrl: string;   // Cannot be modified
    readonly apiKey: string;
    timeout: number;            // Can be modified
};

let config: Config = {
    apiUrl: 'https://api.example.com',
    apiKey: 'secret-key-123',
    timeout: 5000
};

// config.apiUrl = 'different';  // Error! Readonly
config.timeout = 10000;  // OK

console.log("\n5. Readonly properties:");
console.log("   config:", config);

// ============================================
// TYPE ALIASES WITH METHODS
// ============================================

type Calculator = {
    (a: number, b: number): number;  // Method signature
    name: string;                      // Property
};

let add: Calculator = (a, b) => a + b;
add.name = 'Addition';

console.log("\n6. Type alias with method:");
console.log("   add(5, 3):", add(5, 3));

// ============================================
// NESTED OBJECT TYPES
// ============================================

type Address = {
    street: string;
    city: string;
    country: string;
    zipCode?: string;  // Optional
};

type Employee = {
    id: number;
    name: string;
    department: string;
    address: Address;
};

let employee: Employee = {
    id: 1,
    name: 'Jane Smith',
    department: 'Engineering',
    address: {
        street: '123 Main St',
        city: 'San Francisco',
        country: 'USA',
        zipCode: '94102'
    }
};

console.log("\n7. Nested object types:");
console.log("   employee:", employee);

// ============================================
// EXTENDING TYPES (INTERSECTION)
// ============================================

type BaseEntity = {
    id: number;
    createdAt: Date;
    updatedAt: Date;
};

type UserEntity = BaseEntity & {
    username: string;
    email: string;
};

let userEntity: UserEntity = {
    id: 1,
    createdAt: new Date(),
    updatedAt: new Date(),
    username: 'johndoe',
    email: 'john@example.com'
};

console.log("\n8. Type intersection:");
console.log("   userEntity:", userEntity);

// ============================================
// TYPE ALIAS VS INTERFACE (COMMON QUESTION)
// ============================================

// Type alias
type TypeUser = {
    name: string;
    age: number;
};

// Interface (more on this later)
interface InterfaceUser {
    name: string;
    age: number;
}

// Both work similarly, but interfaces can be extended
// Use type for:
 // - Primitives: type Name = string
// - Unions: type Status = 'active' | 'inactive'
// - Tuples: type Pair = [number, number]
// Use interface for:
// - Object shapes (especially in Angular)
// - Class contracts
// - When you need to extend/merge

console.log("\n9. Type vs Interface:");
console.log("   Type alias and interface are similar for objects");
console.log("   Use interface for object shapes in Angular");

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// API Response type
type ApiResponse<T> = {
    data: T;
    status: number;
    message: string;
};

// User model for Angular
interface AngularUser {
    id: number;
    username: string;
    email: string;
    roles: string[];
    profile?: {
        avatar: string;
        bio: string;
    };
}

let apiResponse: ApiResponse<AngularUser[]> = {
    data: [
        {
            id: 1,
            username: 'john',
            email: 'john@example.com',
            roles: ['user'],
            profile: {
                avatar: '/avatars/1.jpg',
                bio: 'Software developer'
            }
        }
    ],
    status: 200,
    message: 'Success'
};

console.log("API Response:");
console.log("   apiResponse:", apiResponse);

// Component inputs
interface ComponentInputs {
    title: string;
    items: string[];
    onItemClick?: (item: string) => void;
    isLoading?: boolean;
}

function initializeComponent(inputs: ComponentInputs): void {
    console.log(`   Component: ${inputs.title}`);
    console.log(`   Items: ${inputs.items.length}`);
    console.log(`   Loading: ${inputs.isLoading ?? false}`);
}

console.log("\nComponent inputs:");
initializeComponent({
    title: 'User List',
    items: ['User1', 'User2']
});

console.log("\n========== SUMMARY ==========");
console.log("Type Aliases:");
console.log("- Create reusable type definitions with 'type'");
console.log("- Use for objects, primitives, unions, tuples");
console.log("- Can be extended with intersection (&)");
console.log("\nObject Types:");
console.log("- { prop: type } syntax");
console.log("- Optional properties with ?");
console.log("- Readonly properties with readonly");
console.log("- Nested objects supported");
console.log("\nWhen to use:");
console.log("- Use type for primitives and unions");
console.log("- Use interface for object shapes in Angular");
console.log("================================\n");
