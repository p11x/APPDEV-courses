/**
 * Getters and Setters in TypeScript
 * 
 * Accessor methods provide controlled access to class properties.
 * They allow you to add logic when getting or setting values.
 * 
 * Angular Connection: Used for:
 * - Computed properties in components
 * - Encapsulation in services
 * - Validation before setting values
 * - Caching computed values
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== GETTERS AND SETTERS ==========\n");

// ============================================
// BASIC GETTER
// ============================================

class Circle {
    radius: number;
    
    constructor(radius: number) {
        this.radius = radius;
    }
    
    // Getter - accessed like a property
    get diameter(): number {
        return this.radius * 2;
    }
    
    get area(): number {
        return Math.PI * this.radius * this.radius;
    }
}

let circle = new Circle(5);

console.log("1. Basic getters:");
console.log("   radius:", circle.radius);
console.log("   diameter:", circle.diameter);
console.log("   area:", circle.area);

// ============================================
// GETTER AND SETTER
// ============================================

class Temperature {
    private _celsius: number = 0;
    
    // Getter for celsius
    get celsius(): number {
        return this._celsius;
    }
    
    // Setter for celsius with validation
    set celsius(value: number) {
        if (value < -273.15) {
            console.log("   Invalid temperature!");
            return;
        }
        this._celsius = value;
    }
    
    // Getter for fahrenheit (computed)
    get fahrenheit(): number {
        return (this._celsius * 9/5) + 32;
    }
    
    set fahrenheit(value: number) {
        this._celsius = (value - 32) * 5/9;
    }
}

let temp = new Temperature();
temp.celsius = 25;

console.log("\n2. Getter and setter:");
console.log("   celsius:", temp.celsius);
console.log("   fahrenheit:", temp.fahrenheit);

temp.celsius = -300;  // Invalid
console.log("   After invalid set:", temp.celsius);

// ============================================
// SETTER WITH VALIDATION
// ============================================

class UserAccount {
    private _username: string = '';
    private _email: string = '';
    
    get username(): string {
        return this._username;
    }
    
    set username(value: string) {
        if (value.length < 3) {
            console.log("   Username too short!");
            return;
        }
        this._username = value;
    }
    
    get email(): string {
        return this._email;
    }
    
    set email(value: string) {
        if (!value.includes('@')) {
            console.log("   Invalid email!");
            return;
        }
        this._email = value;
    }
}

console.log("\n3. Validation in setters:");
const userAcc = new UserAccount();
userAcc.username = 'ab';     // Too short
userAcc.username = 'john';   // Valid
console.log("   username:", userAcc.username);

userAcc.email = 'invalid';  // Invalid
userAcc.email = 'a@b.com';  // Valid
console.log("   email:", userAcc.email);

// ============================================
// READONLY WITH GETTER
// ============================================

class ProductClass {
    constructor(
        public name: string,
        private _price: number
    ) {}
    
    // Read-only property using getter
    get price(): number {
        return this._price;
    }
    
    // Can't set - no setter defined
}

let prod = new ProductClass('Laptop', 999);

console.log("\n4. Readonly with getter:");
console.log("   name:", prod.name);
console.log("   price:", prod.price);
// prod.price = 100;  // Error - no setter

// ============================================
// COMPUTED PROPERTIES
// ============================================

class ShoppingCart {
    private items: { name: string; price: number; quantity: number }[] = [];
    
    addItem(name: string, price: number, quantity: number = 1): void {
        this.items.push({ name, price, quantity });
    }
    
    // Computed property
    get totalItems(): number {
        return this.items.reduce((sum, item) => sum + item.quantity, 0);
    }
    
    get totalPrice(): number {
        return this.items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    }
}

let cart = new ShoppingCart();
cart.addItem('Laptop', 999, 1);
cart.addItem('Mouse', 29, 2);
cart.addItem('Keyboard', 79, 1);

console.log("\n5. Computed properties:");
console.log("   totalItems:", cart.totalItems);
console.log("   totalPrice:", cart.totalPrice);

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Component with computed properties
class UserListComponent {
    users: { name: string; active: boolean }[] = [
        { name: 'Alice', active: true },
        { name: 'Bob', active: false },
        { name: 'Charlie', active: true }
    ];
    
    // Computed from users array
    get userCount(): number {
        return this.users.length;
    }
    
    get activeUserCount(): number {
        return this.users.filter(u => u.active).length;
    }
    
    get inactiveUserCount(): number {
        return this.users.filter(u => !u.active).length;
    }
    
    get hasUsers(): boolean {
        return this.users.length > 0;
    }
}

console.log("Component computed properties:");
const userComp = new UserListComponent();
console.log("   userCount:", userComp.userCount);
console.log("   activeUserCount:", userComp.activeUserCount);
console.log("   inactiveUserCount:", userComp.inactiveUserCount);
console.log("   hasUsers:", userComp.hasUsers);

// Service with encapsulation
class ConfigService {
    private _config: { theme: string; language: string; apiUrl: string } | null = null;
    
    get config(): { theme: string; language: string; apiUrl: string } | null {
        return this._config;
    }
    
    set config(value: { theme: string; language: string; apiUrl: string }) {
        // Validation before setting
        if (!value.theme || !value.language || !value.apiUrl) {
            console.log("   Invalid config!");
            return;
        }
        this._config = value;
    }
    
    get isConfigured(): boolean {
        return this._config !== null;
    }
}

console.log("\nService encapsulation:");
const configSvc = new ConfigService();
console.log("   isConfigured:", configSvc.isConfigured);
configSvc.config = { theme: 'dark', language: 'en', apiUrl: 'https://api.example.com' };
console.log("   isConfigured:", configSvc.isConfigured);

// Form control example
class FormControlField {
    private _value: string = '';
    private _errors: string[] = [];
    
    get value(): string {
        return this._value;
    }
    
    set value(val: string) {
        this._value = val;
        this.validate();
    }
    
    get errors(): string[] {
        return this._errors;
    }
    
    get isValid(): boolean {
        return this._errors.length === 0;
    }
    
    private validate(): void {
        this._errors = [];
        if (this._value.length < 3) {
            this._errors.push('Minimum 3 characters required');
        }
        if (!this._value.includes('@') && !this._value.includes('.')) {
            this._errors.push('Invalid format');
        }
    }
}

console.log("\nForm control validation:");
const field = new FormControlField();
field.value = 'ab';
console.log("   value: 'ab', errors:", field.errors, ", isValid:", field.isValid);
field.value = 'valid@email.com';
console.log("   value: 'valid@email.com', errors:", field.errors, ", isValid:", field.isValid);

console.log("\n========== SUMMARY ==========");
console.log("Getters and Setters:");
console.log("- Use 'get' and 'set' keywords");
console.log("- Accessed like properties, not methods");
console.log("- Add validation in setters");
console.log("- Computed values in getters");
console.log("\nBenefits:");
console.log("- Encapsulation of internal state");
console.log("- Validation before setting values");
console.log("- Computed/cached properties");
console.log("- Readonly properties (no setter)");
console.log("\nAngular Usage:");
console.log("- Computed component properties");
console.log("- Service configuration");
console.log("- Form validation");
console.log("- Caching expensive computations");
console.log("================================\n");
