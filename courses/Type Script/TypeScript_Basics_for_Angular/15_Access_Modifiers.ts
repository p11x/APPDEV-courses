/**
 * Access Modifiers in TypeScript
 * 
 * Access modifiers control visibility of class members (properties and methods).
 * TypeScript supports: public, private, and protected.
 * 
 * Angular Connection: Essential for Angular architecture:
 * - Services: private properties for internal state
 * - Components: public for template access, private for logic
 * - Inheritance patterns with protected members
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== ACCESS MODIFIERS ==========\n");

// ============================================
// PUBLIC (DEFAULT)
// ============================================

// Public members are accessible everywhere
class PublicMember {
    public name: string;  // Explicitly public
    
    constructor(name: string) {
        this.name = name;
    }
    
    public greet(): string {
        return `Hello, ${this.name}`;
    }
}

let pub = new PublicMember('John');
console.log("1. Public access:");
console.log("   name:", pub.name);           // Accessible
console.log("   greet():", pub.greet());    // Accessible

// ============================================
// PRIVATE
// ============================================

// Private members only accessible within the class
class PrivateMember {
    private secret: string;
    public name: string;
    
    constructor(name: string, secret: string) {
        this.name = name;
        this.secret = secret;
    }
    
    public revealSecret(): string {
        // Can access private within class
        return `Secret: ${this.secret}`;
    }
}

let priv = new PrivateMember('John', 'MyPassword123');

// console.log(priv.secret);  // Error! Private
console.log("\n2. Private access:");
console.log("   name:", priv.name);              // Accessible
console.log("   revealSecret():", priv.revealSecret());  // OK - within class

// ============================================
// PROTECTED
// ============================================

// Protected members accessible within class and subclasses
class Animal {
    protected name: string;
    
    constructor(name: string) {
        this.name = name;
    }
    
    protected makeSound(): string {
        return 'Some sound';
    }
    
    public getName(): string {
        return this.name;
    }
}

class Dog extends Animal {
    private breed: string;
    
    constructor(name: string, breed: string) {
        super(name);
        this.breed = breed;
    }
    
    public bark(): string {
        // Can access protected name from parent
        return `${this.name} says Woof!`;
    }
    
    public getInfo(): string {
        // Can access protected makeSound from parent
        return `${this.name} (${this.breed}) - ${this.makeSound()}`;
    }
}

console.log("\n3. Protected access:");
const dog = new Dog('Buddy', 'Labrador');
console.log("   getName():", dog.getName());       // OK - public method
// console.log(dog.name);                         // Error! Protected
console.log("   bark():", dog.bark());              // OK
console.log("   getInfo():", dog.getInfo());        // OK

// ============================================
// PARAMETER PROPERTIES WITH ACCESS MODIFIERS
// ============================================

// Shorthand to declare and initialize properties
class Employee {
    // All in one line with access modifiers
    constructor(
        public name: string,
        private salary: number,
        protected department: string
    ) {}
    
    public getSalaryInfo(): string {
        // Private accessible within class
        return `$${this.salary}`;
    }
}

let emp = new Employee('Alice', 75000, 'Engineering');

console.log("\n4. Parameter properties:");
console.log("   name:", emp.name);              // Public - OK
// console.log(emp.salary);                    // Private - Error
console.log("   getSalaryInfo():", emp.getSalaryInfo());
// console.log(emp.department);                 // Protected - Error

// ============================================
// READONLY WITH ACCESS MODIFIERS
// ============================================

class Config {
    constructor(
        public readonly apiUrl: string,
        private readonly apiKey: string,
        protected readonly timeout: number
    ) {}
    
    public getConfig(): string {
        return `API: ${this.apiUrl}, Key: ${this.apiKey}, Timeout: ${this.timeout}`;
    }
}

let config = new Config('https://api.example.com', 'key123', 5000);

console.log("\n5. Readonly with access modifiers:");
console.log("   apiUrl:", config.apiUrl);
// config.apiUrl = 'different';  // Error! Readonly
console.log("   getConfig():", config.getConfig());

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Angular Service Example
class UserServiceExample {
    // Public - accessible from components
    public users: { name: string; email: string }[] = [];
    
    // Private - internal only
    private cache: Map<string, unknown> = new Map();
    
    // Protected - for subclass access
    protected apiUrl: string = 'https://api.example.com';
    
    constructor() {
        this.loadInitialData();
    }
    
    private loadInitialData(): void {
        // Private method - internal logic
        this.users = [
            { name: 'John', email: 'john@example.com' },
            { name: 'Jane', email: 'jane@example.com' }
        ];
    }
    
    public getUsers(): { name: string; email: string }[] {
        return this.users;
    }
    
    public addUser(user: { name: string; email: string }): void {
        this.users.push(user);
    }
}

console.log("Angular Service:");
const userServiceEx = new UserServiceExample();
console.log("   getUsers():", userServiceEx.getUsers());
userServiceEx.addUser({ name: 'Bob', email: 'bob@example.com' });
console.log("   After add:", userServiceEx.getUsers());

// Angular Component Example
class ButtonComponent {
    // Public - accessed in template
    public label: string = 'Click Me';
    public isDisabled: boolean = false;
    
    // Private - internal logic only
    private clickCount: number = 0;
    
    // Protected - for child components
    protected buttonId: string = 'btn-001';
    
    // Called from template or parent
    public onClick(): void {
        this.clickCount++;
        console.log(`   Button clicked ${this.clickCount} times`);
    }
    
    public getClickCount(): number {
        return this.clickCount;
    }
}

console.log("\nAngular Component:");
const button = new ButtonComponent();
button.onClick();
button.onClick();
console.log("   Click count:", button.getClickCount());

// Inheritance Example
class PrimaryButtonComponent extends ButtonComponent {
    constructor() {
        super();
        this.label = 'Primary Button';
    }
    
    // Override parent method
    public onClick(): void {
        console.log("   Primary button handling");
        super.onClick();
    }
}

console.log("\nInheritance:");
const primaryBtn = new PrimaryButtonComponent();
primaryBtn.onClick();

console.log("\n========== SUMMARY ==========");
console.log("Access Modifiers:");
console.log("- public: Accessible everywhere (default)");
console.log("- private: Accessible only within class");
console.log("- protected: Accessible in class and subclasses");
console.log("\nCombined with readonly:");
console.log("- readonly public: Can read, can't modify");
console.log("- readonly private: Class-only, can't modify");
console.log("\nAngular Usage:");
console.log("- public: Template bindings, @Input/@Output");
console.log("- private: Internal service state, helpers");
console.log("- protected: For extension (child components)");
console.log("\nParameter Properties:");
console.log("- constructor(public prop: Type) {}");
console.log("- Automatically declares and initializes");
console.log("================================\n");
