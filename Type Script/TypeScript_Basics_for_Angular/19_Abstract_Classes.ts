/**
 * Abstract Classes in TypeScript
 * 
 * Abstract classes define blueprints for other classes. They can have abstract
 * methods (without implementation) that must be implemented by subclasses.
 * 
 * Angular Connection: Used for:
 * - Base component classes
 * - Shared functionality patterns
 * - Framework extensibility
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== ABSTRACT CLASSES ==========\n");

// ============================================
// ABSTRACT CLASS
// ============================================

// Abstract class - can't be instantiated directly
abstract class AnimalBase {
    // Abstract property - must be implemented
    abstract name: string;
    abstract species: string;
    
    // Regular method with implementation
    move(): void {
        console.log(`   ${this.name} is moving`);
    }
    
    // Abstract method - no implementation
    abstract makeSound(): string;
}

// Concrete class implementing abstract
class DogClass extends AnimalBase {
    name: string;
    species: string = 'Canine';
    breed: string;
    
    constructor(name: string, breed: string) {
        super();
        this.name = name;
        this.breed = breed;
    }
    
    // Must implement abstract method
    makeSound(): string {
        return 'Woof!';
    }
    
    // Can add additional methods
    fetch(): string {
        return `${this.name} fetches the ball`;
    }
}

console.log("1. Abstract class:");
const dogClass = new DogClass('Buddy', 'Labrador');
console.log("   name:", dogClass.name);
console.log("   makeSound():", dogClass.makeSound());
dogClass.move();

// const animal = new AnimalBase();  // Error! Can't instantiate

// ============================================
// ABSTRACT WITH IMPLEMENTED METHODS
// ============================================

abstract class BaseComponent {
    protected isLoading: boolean = false;
    protected error: string = '';
    
    // Implemented method
    protected log(message: string): void {
        console.log(`   [Component] ${message}`);
    }
    
    // Abstract methods to implement
    abstract ngOnInit(): void;
    abstract render(): void;
}

class UserList extends BaseComponent {
    users: string[] = [];
    
    ngOnInit(): void {
        this.log('UserList initialized');
        this.users = ['Alice', 'Bob'];
    }
    
    render(): void {
        this.log(`Rendering ${this.users.length} users`);
    }
    
    loadUsers(): void {
        this.isLoading = true;
        this.log('Loading users...');
    }
}

console.log("\n2. Abstract with methods:");
const userList = new UserList();
userList.ngOnInit();
userList.render();
userList.loadUsers();

// ============================================
// ABSTRACT FACTORY PATTERN
// ============================================

abstract class Notification {
    abstract send(message: string): void;
    abstract getType(): string;
}

class EmailNotification extends Notification {
    send(message: string): void {
        console.log(`   [Email] Sending: ${message}`);
    }
    
    getType(): string {
        return 'email';
    }
}

class SmsNotification extends Notification {
    send(message: string): void {
        console.log(`   [SMS] Sending: ${message}`);
    }
    
    getType(): string {
        return 'sms';
    }
}

// Factory
class NotificationFactory {
    static create(type: 'email' | 'sms'): Notification {
        if (type === 'email') {
            return new EmailNotification();
        }
        return new SmsNotification();
    }
}

console.log("\n3. Factory pattern:");
const email = NotificationFactory.create('email');
email.send('Welcome!');
const sms = NotificationFactory.create('sms');
sms.send('Your code is 123');

// ============================================
// ANGULAR EXAMPLE
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Base component (similar to Angular's)
abstract class AngularComponentBase {
    protected title: string = '';
    protected template: string = '';
    protected inputs: Record<string, unknown> = {};
    
    abstract ngOnInit(): void;
    abstract ngOnDestroy(): void;
    
    protected logLifecycle(hook: string): void {
        console.log(`   ${this.constructor.name}: ${hook}`);
    }
    
    // Template method pattern
    init(): void {
        this.logLifecycle('ngOnInit');
        this.ngOnInit();
    }
    
    destroy(): void {
        this.logLifecycle('ngOnDestroy');
        this.ngOnDestroy();
    }
}

class DashboardComponent extends AngularComponentBase {
    data: string[] = [];
    
    ngOnInit(): void {
        this.title = 'Dashboard';
        this.data = ['Item 1', 'Item 2'];
        console.log(`   Dashboard loaded with ${this.data.length} items`);
    }
    
    ngOnDestroy(): void {
        console.log('   Cleaning up dashboard...');
    }
}

class ProfileComponent extends AngularComponentBase {
    user: { name: string } | null = null;
    
    ngOnInit(): void {
        this.title = 'Profile';
        this.user = { name: 'John' };
    }
    
    ngOnDestroy(): void {
        console.log('   Saving profile state...');
    }
}

console.log("Base component pattern:");
const dashboard = new DashboardComponent();
dashboard.init();
dashboard.destroy();

console.log("\nProfile component:");
const profile = new ProfileComponent();
profile.init();
profile.destroy();

console.log("\n========== SUMMARY ==========");
console.log("Abstract Classes:");
console.log("- Use 'abstract' keyword");
console.log("- Can't be instantiated directly");
console.log("- Can have abstract methods (no body)");
console.log("- Must implement abstract members");
console.log("- Can have implemented methods too");
console.log("\nUse Cases:");
console.log("- Base classes with common functionality");
console.log("- Enforcing implementation contracts");
console.log("- Factory pattern implementations");
console.log("\nAngular Usage:");
console.log("- Base component patterns");
console.log("- Shared lifecycle hooks");
console.log("- Template method patterns");
console.log("\nvs Interface:");
console.log("- Abstract: has implementation + contract");
console.log("- Interface: only contract");
console.log("================================\n");
