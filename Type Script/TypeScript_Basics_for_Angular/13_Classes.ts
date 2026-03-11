/**
 * Classes in TypeScript
 * 
 * Classes are the foundation of Angular components and services.
 * Understanding classes is essential for Angular development.
 * 
 * Angular Connection: Angular uses classes for:
 * - Components (@Component decorator)
 * - Services (@Injectable decorator)
 * - Pipes (@Pipe decorator)
 * - Directives (@Directive decorator)
 * - Guards (CanActivate, etc.)
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== CLASSES ==========\n");

// ============================================
// BASIC CLASS
// ============================================

class Person {
    // Properties
    name: string;
    age: number;
    
    // Constructor
    constructor(name: string, age: number) {
        this.name = name;
        this.age = age;
    }
    
    // Method
    greet(): string {
        return `Hello, my name is ${this.name} and I'm ${this.age} years old.`;
    }
}

// Create instance
let person = new Person('John', 30);

console.log("1. Basic class:");
console.log("   person:", person);
console.log("   greet():", person.greet());

// ============================================
// CLASS WITH METHODS
// ============================================

class Calculator {
    // Properties
    lastResult: number = 0;
    
    // Methods
    add(a: number, b: number): number {
        this.lastResult = a + b;
        return this.lastResult;
    }
    
    subtract(a: number, b: number): number {
        this.lastResult = a - b;
        return this.lastResult;
    }
    
    getLastResult(): number {
        return this.lastResult;
    }
}

let calc = new Calculator();

console.log("\n2. Class with methods:");
console.log("   add(10, 5):", calc.add(10, 5));
console.log("   subtract(10, 5):", calc.subtract(10, 5));
console.log("   getLastResult():", calc.getLastResult());

// ============================================
// CLASS WITH PROPERTY INITIALIZATION
// ============================================

class Counter {
    // Initialize with default value
    count: number = 0;
    name: string = 'Counter';
    
    increment(): void {
        this.count++;
    }
    
    decrement(): void {
        this.count--;
    }
    
    reset(): void {
        this.count = 0;
    }
}

let counter = new Counter();

console.log("\n3. Property initialization:");
console.log("   Initial count:", counter.count);
counter.increment();
counter.increment();
console.log("   After 2 increments:", counter.count);
counter.reset();
console.log("   After reset:", counter.count);

// ============================================
// CLASS AS ANGULAR COMPONENT (SIMPLIFIED)
// ============================================

// In Angular, components are classes with @Component decorator
// This is a simplified example
class UserComponent {
    // Component properties (like @Input)
    title: string = 'User List';
    users: { name: string; email: string }[] = [];
    isLoading: boolean = false;
    
    // Component methods (like event handlers)
    ngOnInit(): void {
        console.log("   Component initialized");
        this.loadUsers();
    }
    
    loadUsers(): void {
        this.isLoading = true;
        // Simulate API call
        setTimeout(() => {
            this.users = [
                { name: 'Alice', email: 'alice@example.com' },
                { name: 'Bob', email: 'bob@example.com' }
            ];
            this.isLoading = false;
            console.log("   Users loaded");
        }, 100);
    }
    
    onUserClick(user: { name: string; email: string }): void {
        console.log(`   User clicked: ${user.name}`);
    }
}

console.log("\n4. Angular component simulation:");
const component = new UserComponent();
component.ngOnInit();

// ============================================
// CLASS AS ANGULAR SERVICE (SIMPLIFIED)
// ============================================

// In Angular, services are classes with @Injectable decorator
class DataService {
    // Service properties
    private apiUrl: string = 'https://api.example.com';
    private data: string[] = [];
    
    // Service methods
    getData(): string[] {
        return this.data;
    }
    
    setData(data: string[]): void {
        this.data = data;
    }
    
    fetchData(): Promise<string[]> {
        // Simulate HTTP call
        return new Promise(resolve => {
            setTimeout(() => {
                const newData = ['item1', 'item2', 'item3'];
                this.data = newData;
                resolve(newData);
            }, 100);
        });
    }
}

console.log("\n5. Angular service simulation:");
const dataService = new DataService();
dataService.fetchData().then(data => {
    console.log("   Data fetched:", data);
});

// ============================================
// STATIC PROPERTIES AND METHODS
// ============================================

class AppConfig {
    static readonly APP_NAME: string = 'MyAngularApp';
    static readonly VERSION: string = '1.0.0';
    static readonly API_URL: string = 'https://api.example.com';
    
    static getAppInfo(): string {
        return `${this.APP_NAME} v${this.VERSION}`;
    }
}

console.log("\n6. Static members:");
console.log("   APP_NAME:", AppConfig.APP_NAME);
console.log("   getAppInfo():", AppConfig.getAppInfo());

// ============================================
// GETTERS AND SETTERS
// ============================================

class BankAccount {
    private _balance: number = 0;
    private _accountHolder: string = '';
    
    // Getter
    get balance(): number {
        return this._balance;
    }
    
    // Setter
    set balance(value: number) {
        if (value >= 0) {
            this._balance = value;
        }
    }
    
    get accountHolder(): string {
        return this._accountHolder;
    }
    
    set accountHolder(name: string) {
        this._accountHolder = name;
    }
    
    deposit(amount: number): void {
        this._balance += amount;
    }
    
    withdraw(amount: number): void {
        if (amount <= this._balance) {
            this._balance -= amount;
        }
    }
}

let account = new BankAccount();
account.accountHolder = 'John Doe';
account.balance = 1000;
account.deposit(500);

console.log("\n7. Getters and setters:");
console.log("   Account holder:", account.accountHolder);
console.log("   Balance:", account.balance);
account.withdraw(200);
console.log("   After withdraw:", account.balance);

console.log("\n========== SUMMARY ==========");
console.log("Classes:");
console.log("- Blueprint for creating objects");
console.log("- Have properties (data) and methods (behavior)");
console.log("- Use 'constructor' for initialization");
console.log("- Can have static members (accessed on class)");
console.log("- Can have getters/setters for controlled access");
console.log("\nAngular Usage:");
console.log("- Components = classes with @Component");
console.log("- Services = classes with @Injectable");
console.log("- Pipes = classes with @Pipe");
console.log("- Directives = classes with @Directive");
console.log("- Guards = classes with @Injectable");
console.log("\nClass structure:");
console.log("- Properties: data/state");
console.log("- Constructor: initialization");
console.log("- Methods: behavior/actions");
console.log("- Getters/Setters: controlled access");
console.log("================================\n");
