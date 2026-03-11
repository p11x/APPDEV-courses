/**
 * Constructors in TypeScript
 * 
 * Constructors initialize objects when they are created. They are essential
 * for Angular components and services.
 * 
 * Angular Connection: Constructors are used for:
 * - Initializing component properties
 * - Dependency injection (services)
 * - Setting up subscriptions
 * - Initializing form controls
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== CONSTRUCTORS ==========\n");

// ============================================
// BASIC CONSTRUCTOR
// ============================================

class User {
    name: string;
    email: string;
    
    constructor(name: string, email: string) {
        this.name = name;
        this.email = email;
    }
}

let user = new User('John', 'john@example.com');

console.log("1. Basic constructor:");
console.log("   user:", user);

// ============================================
// CONSTRUCTOR WITH INITIALIZATION
// ============================================

class Product {
    id: number;
    name: string;
    price: number;
    inStock: boolean = true;  // Default value
    
    constructor(id: number, name: string, price: number) {
        this.id = id;
        this.name = name;
        this.price = price;
    }
    
    getInfo(): string {
        return `${this.name} - $${this.price}`;
    }
}

let product = new Product(1, 'Laptop', 999);

console.log("\n2. Constructor with defaults:");
console.log("   product:", product);
console.log("   getInfo():", product.getInfo());

// ============================================
// PARAMETER PROPERTIES (SHORTHAND)
// ============================================

// Shorthand: declare and initialize in constructor parameter
class Car {
    // Instead of: brand: string; constructor(b: string) { this.brand = b; }
    // Use: constructor(public brand: string) {}
    
    constructor(
        public brand: string,
        public model: string,
        public year: number
    ) {
        // No body needed - properties auto-initialized
    }
    
    getDescription(): string {
        return `${this.year} ${this.brand} ${this.model}`;
    }
}

let car = new Car('Toyota', 'Camry', 2023);

console.log("\n3. Parameter properties:");
console.log("   car:", car);
console.log("   getDescription():", car.getDescription());

// ============================================
// CONSTRUCTOR OVERLOADING (MULTIPLE SIGNATURES)
// ============================================

// TypeScript doesn't have true constructor overloading
// Instead, use optional parameters or union types

class Rectangle {
    width: number;
    height: number;
    
    // Single constructor with optional params
    constructor(width: number, height?: number) {
        this.width = width;
        // If height not provided, assume square
        this.height = height ?? width;
    }
    
    getArea(): number {
        return this.width * this.height;
    }
}

let rect1 = new Rectangle(10);       // Square
let rect2 = new Rectangle(10, 20);  // Rectangle

console.log("\n4. Constructor with optional params:");
console.log("   Rectangle(10):", rect1.getArea());
console.log("   Rectangle(10, 20):", rect2.getArea());

// ============================================
// CONSTRUCTOR WITH INTERFACE IMPLEMENTATION
// ============================================

interface Creatable {
    createdAt: Date;
    createdBy: string;
}

class Order implements Creatable {
    createdAt: Date;
    createdBy: string;
    orderId: string;
    total: number;
    
    constructor(orderId: string, total: number, createdBy: string) {
        this.orderId = orderId;
        this.total = total;
        this.createdBy = createdBy;
        this.createdAt = new Date();
    }
}

let order = new Order('ORD-001', 99.99, 'John');

console.log("\n5. Constructor with interface:");
console.log("   order:", order);

// ============================================
// ANGULAR COMPONENT CONSTRUCTOR
// ============================================

// In Angular, constructor is used for dependency injection
// and initializing the component

class AngularComponentDemo {
    title: string;
    items: string[];
    private _loading: boolean;
    
    // Constructor used for DI (services would be injected here)
    constructor(title: string = 'Default Title') {
        this.title = title;
        this.items = [];
        this._loading = false;
    }
    
    get loading(): boolean {
        return this._loading;
    }
    
    loadData(): void {
        this._loading = true;
        // Simulate data loading
        setTimeout(() => {
            this.items = ['Item 1', 'Item 2', 'Item 3'];
            this._loading = false;
        }, 100);
    }
}

console.log("\n6. Angular component constructor:");
const demo = new AngularComponentDemo('My Component');
console.log("   Initial title:", demo.title);
demo.loadData();

// ============================================
// ANGULAR SERVICE CONSTRUCTOR
// ============================================

// Services also use constructors for initialization
// and potentially for injecting other services

class HttpService {
    private baseUrl: string;
    
    constructor(baseUrl: string = 'https://api.example.com') {
        this.baseUrl = baseUrl;
    }
    
    getUrl(endpoint: string): string {
        return `${this.baseUrl}/${endpoint}`;
    }
}

class UserApiService {
    private http: HttpService;
    
    // Inject HttpService via constructor
    constructor() {
        this.http = new HttpService('https://users.api.com');
    }
    
    getUsersEndpoint(): string {
        return this.http.getUrl('users');
    }
}

console.log("\n7. Angular service constructor:");
const userApi = new UserApiService();
console.log("   Users endpoint:", userApi.getUsersEndpoint());

// ============================================
// CONSTRUCTOR WITH VALIDATION
// ============================================

class BankAccountClass {
    private _balance: number;
    
    constructor(initialBalance: number) {
        if (initialBalance < 0) {
            throw new Error('Initial balance cannot be negative');
        }
        this._balance = initialBalance;
    }
    
    get balance(): number {
        return this._balance;
    }
}

console.log("\n8. Constructor validation:");
try {
    const invalid = new BankAccountClass(-100);
} catch (error) {
    console.log("   Caught error:", (error as Error).message);
}

const valid = new BankAccountClass(1000);
console.log("   Valid account balance:", valid.balance);

console.log("\n========== SUMMARY ==========");
console.log("Constructors:");
console.log("- Special method called when creating instances");
console.log("- Use 'constructor' keyword");
console.log("- Initialize properties and setup state");
console.log("\nParameter Properties:");
console.log("- Shorthand: constructor(public prop: type) {}");
console.log("- Automatically declares and initializes");
console.log("\nAngular Usage:");
console.log("- Component constructor for DI");
console.log("- Service constructor for initialization");
console.log("- Initialize properties with defaults");
console.log("- Validation for invalid states");
console.log("\nBest Practices:");
console.log("- Keep constructors simple");
console.log("- Use for dependency injection only");
console.log("- Initialize in ngOnInit for complex setup");
console.log("================================\n");
