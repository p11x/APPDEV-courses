/**
 * Generics in TypeScript
 * 
 * Generics allow creating reusable components that work with multiple types.
 * Angular heavily uses generics for services, HTTP, and more.
 * 
 * Angular Connection: Used extensively for:
 * - HttpClient responses (Observable<T>)
 * - Service methods
 * - Form controls
 * - Dependency injection
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== GENERICS ==========\n");

// ============================================
// GENERIC FUNCTIONS
// ============================================

// Generic function - works with any type
function identity<T>(value: T): T {
    return value;
}

console.log("1. Generic functions:");
console.log("   identity<string>('hello'):", identity<string>('hello'));
console.log("   identity<number>(42):", identity<number>(42));
console.log("   identity<boolean>(true):", identity<boolean>(true));

// Type inference - TypeScript figures out the type
console.log("   identity('inferred'):", identity('inferred'));

// ============================================
// GENERIC INTERFACES
// ============================================

interface ApiResponse<T> {
    data: T;
    status: number;
    message: string;
}

interface User {
    id: number;
    name: string;
}

console.log("\n2. Generic interfaces:");
const userResponse: ApiResponse<User> = {
    data: { id: 1, name: 'John' },
    status: 200,
    message: 'Success'
};
console.log("   userResponse:", userResponse);

const usersResponse: ApiResponse<User[]> = {
    data: [
        { id: 1, name: 'John' },
        { id: 2, name: 'Jane' }
    ],
    status: 200,
    message: 'Success'
};
console.log("   usersResponse:", usersResponse);

// ============================================
// GENERIC CLASSES
// ============================================

class Container<T> {
    private value: T;
    
    constructor(value: T) {
        this.value = value;
    }
    
    getValue(): T {
        return this.value;
    }
    
    setValue(value: T): void {
        this.value = value;
    }
}

console.log("\n3. Generic classes:");
const stringContainer = new Container<string>('Hello');
console.log("   stringContainer.getValue():", stringContainer.getValue());

const numberContainer = new Container<number>(42);
console.log("   numberContainer.getValue():", numberContainer.getValue());

// ============================================
// GENERIC CONSTRAINTS
// ============================================

// Constraint: T must have a length property
interface HasLength {
    length: number;
}

function logLength<T extends HasLength>(item: T): void {
    console.log(`   Length: ${item.length}`);
}

console.log("\n4. Generic constraints:");
logLength('hello');           // String has length
logLength([1, 2, 3]);        // Array has length
logLength({ length: 10 });   // Object with length

// Constraint: T must be subtype of another type
class BaseClass {
    baseMethod(): void {
        console.log('   Base method');
    }
}

class DerivedClass extends BaseClass {
    derivedMethod(): void {
        console.log('   Derived method');
    }
}

function callBaseMethod<T extends BaseClass>(obj: T): void {
    obj.baseMethod();
}

console.log("\n5. Type constraint:");
callBaseMethod(new DerivedClass());

// ============================================
// GENERIC WITH DEFAULT
// ============================================

interface Result<T = string> {
    success: boolean;
    data?: T;
    error?: string;
}

console.log("\n6. Generic defaults:");
const stringResult: Result = {
    success: true,
    data: 'Hello'
};

const numberResult: Result<number> = {
    success: true,
    data: 42
};

console.log("   stringResult:", stringResult);
console.log("   numberResult:", numberResult);

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Generic service
interface Repository<T> {
    findAll(): Promise<T[]>;
    findById(id: number): Promise<T | null>;
    create(item: Omit<T, 'id'>): Promise<T>;
    update(id: number, item: Partial<T>): Promise<T>;
    delete(id: number): Promise<void>;
}

interface ProductItem {
    id: number;
    name: string;
    price: number;
}

// Simulated generic repository
class ProductRepository implements Repository<ProductItem> {
    private products: ProductItem[] = [
        { id: 1, name: 'Laptop', price: 999 },
        { id: 2, name: 'Phone', price: 699 }
    ];
    
    async findAll(): Promise<ProductItem[]> {
        return this.products;
    }
    
    async findById(id: number): Promise<ProductItem | null> {
        return this.products.find(p => p.id === id) ?? null;
    }
    
    async create(item: Omit<ProductItem, 'id'>): Promise<ProductItem> {
        const newProduct = { ...item, id: this.products.length + 1 };
        this.products.push(newProduct);
        return newProduct;
    }
    
    async update(id: number, item: Partial<ProductItem>): Promise<ProductItem> {
        const index = this.products.findIndex(p => p.id === id);
        if (index >= 0) {
            this.products[index] = { ...this.products[index], ...item };
            return this.products[index];
        }
        throw new Error('Not found');
    }
    
    async delete(id: number): Promise<void> {
        this.products = this.products.filter(p => p.id !== id);
    }
}

console.log("Generic Repository:");
const productRepo = new ProductRepository();
productRepo.findAll().then(products => {
    console.log('   All products:', products);
});

// HTTP Response type (Angular-style)
interface HttpResponse<T> {
    data: T;
    status: number;
    statusText: string;
}

function handleResponse<T>(response: HttpResponse<T>): void {
    console.log(`   Status: ${response.status}`);
    console.log(`   Data:`, response.data);
}

console.log("\nHTTP Response handling:");
const response: HttpResponse<string[]> = {
    data: ['item1', 'item2'],
    status: 200,
    statusText: 'OK'
};
handleResponse(response);

// Generic form control
interface FormControlValue<T> {
    value: T;
    valid: boolean;
    errors: string[];
}

function createFormControl<T>(initialValue: T): FormControlValue<T> {
    return {
        value: initialValue,
        valid: true,
        errors: []
    };
}

console.log("\nForm control:");
const stringControl = createFormControl('Hello');
const numberControl = createFormControl(42);
console.log('   stringControl:', stringControl);
console.log('   numberControl:', numberControl);

console.log("\n========== SUMMARY ==========");
console.log("Generics:");
console.log("- Create reusable components with <T>");
console.log("- Type inference works automatically");
console.log("- Can constrain with 'extends'");
console.log("- Can have default types");
console.log("\nAngular Usage:");
console.log("- HttpClient: Observable<T>");
console.log("- Services: Repository<T>");
console.log("- Forms: FormControl<T>");
console.log("- DI: Injector.get<T>()");
console.log("\nBenefits:");
console.log("- Type safety with flexibility");
console.log("- No 'any' type coercion");
console.log("- Reusable code");
console.log("================================\n");
