/**
 * Interfaces in TypeScript
 * 
 * Interfaces define contracts for objects. They are extensively used in Angular
 * for defining component inputs, service contracts, and data models.
 * 
 * Angular Connection: Interfaces are used for:
 * - @Input() and @Output() type definitions
 * - Service method contracts
 * - HTTP response models
 * - Form model definitions
 * - Route parameter types
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== INTERFACES ==========\n");

// ============================================
// BASIC INTERFACE
// ============================================

// Define an interface
interface User {
    id: number;
    name: string;
    email: string;
}

// Use the interface
let user: User = {
    id: 1,
    name: 'John Doe',
    email: 'john@example.com'
};

console.log("1. Basic interface:");
console.log("   user:", user);

// ============================================
// OPTIONAL PROPERTIES
// ============================================

interface Product {
    id: number;
    name: string;
    price: number;
    description?: string;  // Optional
    category?: string;    // Optional
}

let product1: Product = {
    id: 1,
    name: 'Laptop',
    price: 999
};

let product2: Product = {
    id: 2,
    name: 'Phone',
    price: 699,
    description: 'Smartphone'
};

console.log("\n2. Optional properties:");
console.log("   product1:", product1);
console.log("   product2:", product2);

// ============================================
// READONLY PROPERTIES
// ============================================

interface Config {
    readonly id: string;
    readonly apiKey: string;
    timeout: number;
}

let config: Config = {
    id: 'app-1',
    apiKey: 'secret123',
    timeout: 5000
};

// config.id = 'different';  // Error - readonly
config.timeout = 10000;  // OK

console.log("\n3. Readonly properties:");
console.log("   config:", config);

// ============================================
// METHOD SIGNATURES IN INTERFACES
// ============================================

interface Calculator {
    add(a: number, b: number): number;
    subtract(a: number, b: number): number;
    multiply(a: number, b: number): number;
}

let calculator: Calculator = {
    add: (a, b) => a + b,
    subtract: (a, b) => a - b,
    multiply: (a, b) => a * b
};

console.log("\n4. Method signatures:");
console.log("   add(10, 5):", calculator.add(10, 5));
console.log("   subtract(10, 5):", calculator.subtract(10, 5));
console.log("   multiply(10, 5):", calculator.multiply(10, 5));

// ============================================
// INTERFACE EXTENDING INTERFACES
// ============================================

interface Person {
    name: string;
    age: number;
}

interface Employee extends Person {
    employeeId: string;
    department: string;
}

let employee: Employee = {
    name: 'Alice',
    age: 30,
    employeeId: 'EMP001',
    department: 'Engineering'
};

console.log("\n5. Interface extending:");
console.log("   employee:", employee);

// ============================================
// INTERFACE WITH MULTIPLE EXTENDS
// ============================================

interface Named {
    name: string;
}

interface Aged {
    age: number;
}

interface PersonInfo extends Named, Aged {
    email: string;
}

let personInfo: PersonInfo = {
    name: 'Bob',
    age: 25,
    email: 'bob@example.com'
};

console.log("\n6. Multiple extends:");
console.log("   personInfo:", personInfo);

// ============================================
// CLASS IMPLEMENTING INTERFACE
// ============================================

interface Printable {
    print(): void;
    getContent(): string;
}

class Document implements Printable {
    private content: string;
    
    constructor(content: string) {
        this.content = content;
    }
    
    print(): void {
        console.log(`   Printing: ${this.content}`);
    }
    
    getContent(): string {
        return this.content;
    }
}

let doc = new Document('Hello World');

console.log("\n7. Class implementing interface:");
doc.print();
console.log("   getContent():", doc.getContent());

// ============================================
// INTERFACE FOR FUNCTION TYPES
// ============================================

interface StringProcessor {
    (input: string): string;
}

let toUpperCase: StringProcessor = (input) => input.toUpperCase();
let addPrefix: StringProcessor = (input) => `PREFIX: ${input}`;

console.log("\n8. Function type interface:");
console.log("   toUpperCase('hello'):", toUpperCase('hello'));
console.log("   addPrefix('world'):", addPrefix('world'));

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Component Input Interface
interface UserCardInputs {
    user: User;
    showAvatar?: boolean;
    onUserClick?: (user: User) => void;
}

// Service Contract Interface
interface UserService {
    getUsers(): Promise<User[]>;
    getUserById(id: number): Promise<User | null>;
    createUser(user: Omit<User, 'id'>): Promise<User>;
    updateUser(id: number, user: Partial<User>): Promise<User>;
    deleteUser(id: number): Promise<void>;
}

// Implementing the service (simulated)
class MockUserService implements UserService {
    private users: User[] = [
        { id: 1, name: 'Alice', email: 'alice@example.com' },
        { id: 2, name: 'Bob', email: 'bob@example.com' }
    ];
    
    async getUsers(): Promise<User[]> {
        return this.users;
    }
    
    async getUserById(id: number): Promise<User | null> {
        return this.users.find(u => u.id === id) ?? null;
    }
    
    async createUser(user: Omit<User, 'id'>): Promise<User> {
        const newUser = { ...user, id: this.users.length + 1 };
        this.users.push(newUser);
        return newUser;
    }
    
    async updateUser(id: number, user: Partial<User>): Promise<User> {
        const index = this.users.findIndex(u => u.id === id);
        if (index >= 0) {
            this.users[index] = { ...this.users[index], ...user };
            return this.users[index];
        }
        throw new Error('User not found');
    }
    
    async deleteUser(id: number): Promise<void> {
        this.users = this.users.filter(u => u.id !== id);
    }
}

const userService = new MockUserService();

console.log("UserService example:");
userService.getUsers().then(users => {
    console.log("   getUsers():", users);
});

userService.getUserById(1).then(user => {
    console.log("   getUserById(1):", user);
});

// Route parameters interface
interface RouteParams {
    id: string;
    action?: string;
}

function handleRoute(params: RouteParams): void {
    console.log(`   Route: /users/${params.id}`, params.action ? `/${params.action}` : '');
}

console.log("\nRoute handling:");
handleRoute({ id: '123' });
handleRoute({ id: '456', action: 'edit' });

// Form model interface
interface LoginForm {
    username: string;
    password: string;
    rememberMe?: boolean;
}

function submitForm(form: LoginForm): void {
    console.log("   Submitting form:", {
        username: form.username,
        password: '***',
        rememberMe: form.rememberMe ?? false
    });
}

console.log("\nForm submission:");
submitForm({ username: 'admin', password: 'password123' });

console.log("\n========== SUMMARY ==========");
console.log("Interfaces:");
console.log("- Define contracts for objects");
console.log("- Use 'interface' keyword");
console.log("- Can have optional properties (?)");
console.log("- Can have readonly properties");
console.log("- Can define method signatures");
console.log("- Can extend other interfaces");
console.log("- Classes can implement interfaces");
console.log("\nAngular Usage:");
console.log("- Component @Input types");
console.log("- Service method contracts");
console.log("- HTTP response models");
console.log("- Form model definitions");
console.log("- Prefer interfaces over type for objects");
console.log("================================\n");
