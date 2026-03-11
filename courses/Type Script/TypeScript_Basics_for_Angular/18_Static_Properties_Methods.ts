/**
 * Static Properties and Methods in TypeScript
 * 
 * Static members belong to the class itself, not instances.
 * They're accessed directly on the class, not on objects.
 * 
 * Angular Connection: Used for:
 * - Service singletons
 * - Shared utilities
 * - Configuration constants
 * - Factory methods
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== STATIC MEMBERS ==========\n");

// ============================================
// STATIC PROPERTIES
// ============================================

class MathUtils {
    // Static property - belongs to class
    static readonly PI: number = 3.14159;
    static version: string = '1.0.0';
    
    // Instance property
    value: number;
    
    constructor(value: number) {
        this.value = value;
    }
}

console.log("1. Static properties:");
console.log("   MathUtils.PI:", MathUtils.PI);
console.log("   MathUtils.version:", MathUtils.version);
// Access via instance also works but not recommended
const math = new MathUtils(10);
// console.log("   math.PI:", math.PI);  // Would show warning

// ============================================
// STATIC METHODS
// ============================================

class ArrayHelper {
    // Static method
    static sum(numbers: number[]): number {
        return numbers.reduce((a, b) => a + b, 0);
    }
    
    static average(numbers: number[]): number {
        if (numbers.length === 0) return 0;
        return this.sum(numbers) / numbers.length;
    }
    
    static max(numbers: number[]): number {
        return Math.max(...numbers);
    }
    
    static min(numbers: number[]): number {
        return Math.min(...numbers);
    }
    
    // Instance method (regular)
    capitalizeAll(strings: string[]): string[] {
        return strings.map(s => s.toUpperCase());
    }
}

console.log("\n2. Static methods:");
console.log("   sum([1,2,3,4,5]):", ArrayHelper.sum([1, 2, 3, 4, 5]));
console.log("   average([1,2,3]):", ArrayHelper.average([1, 2, 3]));
console.log("   max([3,1,2]):", ArrayHelper.max([3, 1, 2]));

// Instance method
const helper = new ArrayHelper();
console.log("   capitalizeAll(['a','b']):", helper.capitalizeAll(['a', 'b']));

// ============================================
// STATIC FACTORY METHODS
// ============================================

class UserClass {
    constructor(
        public name: string,
        public email: string
    ) {}
    
    // Static factory method
    static fromData(data: { name: string; email: string }): UserClass {
        return new UserClass(data.name, data.email);
    }
    
    static createGuest(): UserClass {
        return new UserClass('Guest', 'guest@example.com');
    }
    
    toString(): string {
        return `${this.name} (${this.email})`;
    }
}

console.log("\n3. Static factory methods:");
const user1 = UserClass.fromData({ name: 'John', email: 'john@example.com' });
const user2 = UserClass.createGuest();
console.log("   fromData:", user1.toString());
console.log("   createGuest:", user2.toString());

// ============================================
// STATIC WITH PRIVATE CONSTRUCTOR (SINGLETON)
// ============================================

class SingletonService {
    private static instance: SingletonService;
    
    // Private constructor
    private constructor(public name: string) {}
    
    // Static getter for instance
    static getInstance(): SingletonService {
        if (!SingletonService.instance) {
            SingletonService.instance = new SingletonService('SingletonInstance');
        }
        return SingletonService.instance;
    }
}

console.log("\n4. Singleton pattern:");
const s1 = SingletonService.getInstance();
const s2 = SingletonService.getInstance();
console.log("   Same instance:", s1 === s2);
console.log("   s1.name:", s1.name);
console.log("   s2.name:", s2.name);

// ============================================
// STATIC INITIALIZATION
// ============================================

class Config {
    // Not readonly - will be set in static block
    static API_URL: string;
    static MAX_RETRIES: number;
    static readonly TIMEOUT: number = 5000;
    
    // Static initializer block
    static {
        const env = 'production';
        
        if (env === 'production') {
            Config.API_URL = 'https://api.production.com';
            Config.MAX_RETRIES = 3;
        } else {
            Config.API_URL = 'https://api.dev.com';
            Config.MAX_RETRIES = 5;
        }
    }
}

console.log("\n5. Static initialization:");
console.log("   API_URL:", Config.API_URL);
console.log("   MAX_RETRIES:", Config.MAX_RETRIES);
console.log("   TIMEOUT:", Config.TIMEOUT);

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Service configuration
class ApiConstants {
    static readonly BASE_URL = 'https://api.example.com';
    static readonly VERSION = 'v1';
    
    static getEndpoint(path: string): string {
        return `${this.BASE_URL}/${this.VERSION}${path}`;
    }
}

console.log("API Constants:");
console.log("   BASE_URL:", ApiConstants.BASE_URL);
console.log("   getEndpoint('/users'):", ApiConstants.getEndpoint('/users'));

// Http status codes
class HttpStatus {
    static readonly OK = 200;
    static readonly CREATED = 201;
    static readonly BAD_REQUEST = 400;
    static readonly UNAUTHORIZED = 401;
    static readonly NOT_FOUND = 404;
    static readonly SERVER_ERROR = 500;
    
    static isSuccess(code: number): boolean {
        return code >= 200 && code < 300;
    }
    
    static isError(code: number): boolean {
        return code >= 400;
    }
}

console.log("\nHTTP Status:");
console.log("   OK:", HttpStatus.OK);
console.log("   isSuccess(200):", HttpStatus.isSuccess(200));
console.log("   isError(404):", HttpStatus.isError(404));

// Utility service with static methods
class DateUtils {
    static formatDate(date: Date): string {
        return date.toISOString().split('T')[0];
    }
    
    static formatTime(date: Date): string {
        return date.toTimeString().split(' ')[0];
    }
    
    static addDays(date: Date, days: number): Date {
        const result = new Date(date);
        result.setDate(result.getDate() + days);
        return result;
    }
}

console.log("\nDate utilities:");
console.log("   formatDate(new Date()):", DateUtils.formatDate(new Date()));
console.log("   addDays(new Date(), 7):", DateUtils.formatDate(DateUtils.addDays(new Date(), 7)));

console.log("\n========== SUMMARY ==========");
console.log("Static Members:");
console.log("- Use 'static' keyword");
console.log("- Belong to class, not instances");
console.log("- Accessed via ClassName.member");
console.log("- Can be readonly for constants");
console.log("\nUse Cases:");
console.log("- Factory methods");
console.log("- Singleton pattern");
console.log("- Utility functions");
console.log("- Constants and configuration");
console.log("\nAngular Usage:");
console.log("- Service singletons");
console.log("- API constants");
console.log("- HTTP status codes");
console.log("- Utility functions");
console.log("================================\n");
