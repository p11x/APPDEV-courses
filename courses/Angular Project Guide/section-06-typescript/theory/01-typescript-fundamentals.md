# Section 6: TypeScript Fundamentals

## Introduction to TypeScript

TypeScript is a superset of JavaScript that adds static types. It's the language Angular uses! Think of it as JavaScript with training wheels - it helps catch errors before you run your code.

### What You'll Learn

- TypeScript basics and types
- Interfaces and types
- Classes
- Generics
- Type safety

---

## 6.1 Why TypeScript?

### JavaScript vs TypeScript

```javascript
// JavaScript - No type checking
function add(a, b) {
    return a + b;
}
add(1, 2);      // Works: 3
add("1", "2");  // Works: "12" (probably unintended!)
```

```typescript
// TypeScript - Type safety
function add(a: number, b: number): number {
    return a + b;
}
add(1, 2);        // Works: 3
add("1", "2");   // Error! Argument of type 'string' is not assignable
```

---

## 6.2 Basic Types

### Primitive Types
```typescript
// String
let name: string = "John";

// Number (integers and decimals)
let age: number = 25;
let price: number = 99.99;

// Boolean
let isActive: boolean = true;

// Arrays
let numbers: number[] = [1, 2, 3, 4, 5];
let names: Array<string> = ["John", "Jane", "Bob"];

// Any (avoid when possible)
let anything: any = "can be anything";
anything = 123;

// Void (no return value)
function logMessage(message: string): void {
    console.log(message);
}

// Null and Undefined
let nullValue: null = null;
let undefinedValue: undefined = undefined;
```

---

## 6.3 Interfaces

### Defining Interfaces
```typescript
interface Product {
    id: number;
    name: string;
    price: number;
    inStock: boolean;
    category?: string;  // Optional property
}

// Using the interface
const product: Product = {
    id: 1,
    name: "Laptop",
    price: 999.99,
    inStock: true
};
```

### Interface with Methods
```typescript
interface ProductService {
    getAll(): Promise<Product[]>;
    getById(id: number): Promise<Product | null>;
    create(product: Product): Promise<Product>;
    update(id: number, product: Partial<Product>): Promise<Product>;
    delete(id: number): Promise<void>;
}
```

---

## 6.4 Types

### Type Aliases
```typescript
type ID = number | string;
type Status = "pending" | "active" | "completed";
type ProductList = Product[];

// Using type aliases
let userId: ID = 123;
let userId2: ID = "abc123";

let orderStatus: Status = "pending";
```

### Union Types
```typescript
// Can be one of multiple types
let value: string | number;
value = "hello";  // OK
value = 123;      // OK

// With type guards
function getLength(value: string | number): number {
    if (typeof value === "string") {
        return value.length;
    }
    return value.toString().length;
}
```

### Intersection Types
```typescript
interface Name {
    firstName: string;
    lastName: string;
}

interface Contact {
    email: string;
    phone: string;
}

type Person = Name & Contact;

const person: Person = {
    firstName: "John",
    lastName: "Doe",
    email: "john@example.com",
    phone: "123-456-7890"
};
```

---

## 6.5 Classes

### Class Definition
```typescript
class Product {
    id: number;
    name: string;
    price: number;
    private description: string;
    protected category: string;

    constructor(id: number, name: string, price: number) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.description = "";
        this.category = "General";
    }

    // Method
    getFormattedPrice(): string {
        return `$${this.price.toFixed(2)}`;
    }

    // Getter
    getDescription(): string {
        return this.description;
    }

    // Setter
    setDescription(description: string): void {
        this.description = description;
    }
}

// Create instance
const laptop = new Product(1, "Laptop", 999.99);
console.log(laptop.getFormattedPrice()); // "$999.99"
```

### Inheritance
```typescript
class ElectronicProduct extends Product {
    warranty: number;

    constructor(id: number, name: string, price: number, warranty: number) {
        super(id, name, price);
        this.warranty = warranty;
    }

    getWarrantyInfo(): string {
        return `${this.warranty} months warranty`;
    }
}
```

---

## 6.6 Generics

### Generic Functions
```typescript
function identity<T>(value: T): T {
    return value;
}

const num = identity<number>(123);    // type: number
const str = identity<string>("hello"); // type: string
const auto = identity(123);           // type: inferred as number
```

### Generic Interfaces
```typescript
interface ApiResponse<T> {
    data: T;
    status: number;
    message: string;
    timestamp: Date;
}

interface Product {
    id: number;
    name: string;
}

const response: ApiResponse<Product[]> = {
    data: [{ id: 1, name: "Laptop" }],
    status: 200,
    message: "Success",
    timestamp: new Date()
};
```

---

## 6.7 Angular-Specific Types

### Component Types
```typescript
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-product-list',
    templateUrl: './product-list.component.html'
})
export class ProductListComponent implements OnInit {
    products: Product[] = [];
    loading: boolean = false;
    error: string | null = null;

    ngOnInit(): void {
        this.loadProducts();
    }

    loadProducts(): void {
        // Implementation
    }
}
```

### Service Types
```typescript
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ProductService {
    getProducts(): Observable<Product[]> {
        // Implementation
    }
}
```

---

## 6.8 TypeScript Configuration

### tsconfig.json
```json
{
    "compilerOptions": {
        "target": "ES2020",
        "module": "commonjs",
        "strict": true,
        "esModuleInterop": true,
        "skipLibCheck": true,
        "forceConsistentCasingInFileNames": true,
        "outDir": "./dist",
        "rootDir": "./src",
        "declaration": true,
        "sourceMap": true
    },
    "include": ["src/**/*"],
    "exclude": ["node_modules", "dist"]
}
```

---

## 6.9 Summary

### Key Takeaways

1. **TypeScript** adds static types to JavaScript
2. **Interfaces** define object shapes
3. **Types** create custom type aliases
4. **Classes** provide OOP features
5. **Generics** make code reusable with different types

### What's Next?

Now let's learn Angular! We'll create our first Angular application!
