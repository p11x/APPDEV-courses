/**
 * Inheritance in TypeScript
 * 
 * Inheritance allows classes to extend other classes, reusing code.
 * Angular uses inheritance in component hierarchies.
 * 
 * Angular Connection: Used for:
 * - Base component classes
 * - Shared functionality in services
 * - Component lifecycle inheritance
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== INHERITANCE ==========\n");

// ============================================
// BASIC INHERITANCE
// ============================================

class Animal {
    name: string;
    
    constructor(name: string) {
        this.name = name;
    }
    
    makeSound(): string {
        return 'Some sound';
    }
    
    move(): void {
        console.log(`   ${this.name} is moving`);
    }
}

// Dog extends Animal
class Dog extends Animal {
    breed: string;
    
    constructor(name: string, breed: string) {
        super(name);  // Call parent constructor
        this.breed = breed;
    }
    
    // Override parent method
    makeSound(): string {
        return 'Woof!';
    }
    
    // New method specific to Dog
    fetch(): string {
        return `${this.name} fetches the ball`;
    }
}

console.log("1. Basic inheritance:");
const dog = new Dog('Buddy', 'Labrador');
console.log("   name:", dog.name);
console.log("   makeSound():", dog.makeSound());
dog.move();
console.log("   fetch():", dog.fetch());

// ============================================
// SUPER CALLS
// ============================================

class Cat extends Animal {
    indoor: boolean;
    
    constructor(name: string, indoor: boolean) {
        super(name);  // Must call super() before accessing this
        this.indoor = indoor;
    }
    
    makeSound(): string {
        // Call parent method then add more
        const parentSound = super.makeSound();
        return `${parentSound} Meow!`;
    }
    
    climb(): string {
        return `${this.name} climbs the tree`;
    }
}

console.log("\n2. Super calls:");
const cat = new Cat('Whiskers', true);
console.log("   makeSound():", cat.makeSound());
console.log("   climb():", cat.climb());

// ============================================
// MULTIPLE LEVELS OF INHERITANCE
// ============================================

class Vehicle {
    constructor(public brand: string) {}
    
    start(): string {
        return `${this.brand} is starting`;
    }
}

class Car extends Vehicle {
    constructor(brand: string, public model: string) {
        super(brand);
    }
    
    drive(): string {
        return `${this.brand} ${this.model} is driving`;
    }
}

class SportsCar extends Car {
    constructor(brand: string, model: string, public topSpeed: number) {
        super(brand, model);
    }
    
    drive(): string {
        return `${super.drive()} at ${this.topSpeed} mph!`;
    }
}

console.log("\n3. Multiple levels:");
const sportsCar = new SportsCar('Ferrari', 'F8', 200);
console.log("   start():", sportsCar.start());
console.log("   drive():", sportsCar.drive());

// ============================================
// PROTECTED MEMBERS IN INHERITANCE
// ============================================

class Shape {
    protected color: string;
    
    constructor(color: string) {
        this.color = color;
    }
    
    protected getColor(): string {
        return this.color;
    }
}

class RectangleClass extends Shape {
    private width: number;
    private height: number;
    
    constructor(color: string, width: number, height: number) {
        super(color);
        this.width = width;
        this.height = height;
    }
    
    getArea(): number {
        // Can access protected color via parent method
        return this.width * this.height;
    }
    
    describe(): string {
        // Can access protected color
        return `A ${this.color} rectangle of ${this.width}x${this.height}`;
    }
}

console.log("\n4. Protected members:");
const rect = new RectangleClass('red', 10, 5);
console.log("   getArea():", rect.getArea());
console.log("   describe():", rect.describe());

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Base component (simplified)
class BaseComponent {
    protected isLoading: boolean = false;
    protected errorMessage: string = '';
    
    protected setLoading(loading: boolean): void {
        this.isLoading = loading;
    }
    
    protected setError(message: string): void {
        this.errorMessage = message;
    }
    
    protected clearError(): void {
        this.errorMessage = '';
    }
    
    // Template method pattern
    protected showError(): void {
        if (this.errorMessage) {
            console.log(`   Error: ${this.errorMessage}`);
        }
    }
}

// Child component inheriting from base
class UserListComponentInherit extends BaseComponent {
    users: { name: string }[] = [];
    
    // Public method to trigger loading
    loadUsers(): void {
        this.setLoading(true);
        // Simulate API call
        setTimeout(() => {
            this.users = [
                { name: 'Alice' },
                { name: 'Bob' }
            ];
            this.setLoading(false);
        }, 100);
    }
    
    // Public method to show error
    displayError(): void {
        this.showError();  // Can call protected from subclass
    }
}

console.log("Base component inheritance:");
const userList = new UserListComponentInherit();
userList.loadUsers();
userList.displayError();

// Service inheritance example
class BaseService {
    protected apiUrl: string = 'https://api.example.com';
    protected timeout: number = 5000;
    
    protected getHeaders(): Record<string, string> {
        return {
            'Content-Type': 'application/json'
        };
    }
}

class UserApiServiceInherit extends BaseService {
    constructor() {
        super();
        this.apiUrl = 'https://users.api.com';  // Override protected property
    }
    
    getUsersEndpoint(): string {
        const headers = this.getHeaders();  // Can use protected method
        return `${this.apiUrl}/users`;
    }
}

console.log("\nService inheritance:");
const userApi = new UserApiServiceInherit();
console.log("   getUsersEndpoint():", userApi.getUsersEndpoint());

// Interface with inheritance
interface PersonBase {
    name: string;
    email: string;
}

interface EmployeeInterface extends PersonBase {
    employeeId: string;
    department: string;
}

interface ManagerInterface extends PersonBase {
    teamSize: number;
    department: string;
}

const employee: EmployeeInterface = {
    name: 'Alice',
    email: 'alice@company.com',
    employeeId: 'EMP001',
    department: 'Engineering'
};

console.log("\nInterface inheritance:");
console.log("   employee:", employee);

console.log("\n========== SUMMARY ==========");
console.log("Inheritance:");
console.log("- Use 'extends' keyword");
console.log("- Use 'super()' to call parent constructor");
console.log("- Use 'super.method()' to call parent methods");
console.log("- Override methods by redefining them");
console.log("- Protected members accessible in subclasses");
console.log("\nAngular Usage:");
console.log("- Base component classes");
console.log("- Shared service functionality");
console.log("- Interface inheritance for types");
console.log("\nBest Practices:");
console.log("- Prefer composition over inheritance");
console.log("- Use protected for overridable methods");
console.log("- Use interfaces for contracts");
console.log("================================\n");
