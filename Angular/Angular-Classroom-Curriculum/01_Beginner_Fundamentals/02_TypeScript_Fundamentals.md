# TypeScript Fundamentals for Angular

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Understand TypeScript's role in Angular development
- [ ] Write TypeScript code with types, interfaces, and generics
- [ ] Apply object-oriented programming concepts in TypeScript
- [ ] Use decorators and metadata annotation
- [ ] Debug TypeScript compilation errors

## Conceptual Explanation

**Visual Analogy**: Think of TypeScript as a **highly detailed blueprint** for a building. While JavaScript is like a rough sketch that anyone can interpret differently, TypeScript is a precise engineering blueprint with exact specifications. This prevents construction they errors before happen - just like catching bugs before runtime!

### What is TypeScript?

TypeScript is JavaScript with syntax for types. It helps developers catch errors early through type checking and makes code more maintainable.

```
┌─────────────────────────────────────────────────────────────┐
│                    TypeScript Compilation                    │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   TypeScript (.ts)    ──▶    TypeScript    ──▶   JavaScript │
│   Source Code                Compiler            (.js)      │
│                                                              │
│   ┌──────────────┐         ┌──────────────┐                 │
│   │ let x: number│  ──▶    │ Type Check  │  ──▶  let x = 5 │
│   │ x = "hello"  │ Error!  │  at compile │       (valid)   │
│   └──────────────┘         └──────────────┘                 │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Why Angular Uses TypeScript

1. **Type Safety**: Catches errors at compile time
2. **Better IDE Support**: IntelliSense, refactoring, navigation
3. **Maintainability**: Large teams can work on same codebase
4. **Modern Features**: Uses latest ECMAScript features

## Real-World Application Context

### Industry Benefits

- **Google**: Uses TypeScript extensively for Angular and internal tools
- **Microsoft**: TypeScript is used in VS Code, Office 365
- **Airbnb**: TypeScript improves code quality in large codebases

### TypeScript in Angular

Every Angular file uses TypeScript features:

```typescript
// Angular component uses decorators, types, generics
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  @Input() title: string = '';
  items: Item[] = [];
  
  ngOnInit(): void {
    this.loadItems();
  }
}
```

## Step-by-Step Walkthrough

### Setting Up TypeScript

#### Step 1: Understanding tsconfig.json

When you create an Angular project, TypeScript is pre-configured:

```json
{
  "compileOnSave": false,
  "compilerOptions": {
    "baseUrl": "./",
    "outDir": "./dist/out-tsc",
    "forceConsistentCasingInFileNames": true,
    "strict": true,
    "noImplicitOverride": true,
    "noPropertyAccessFromIndexSignature": true,
    "noImplicitReturns": true,
    "noFallthroughCasesInSwitch": true,
    "sourceMap": true,
    "declaration": false,
    "downlevelIteration": true,
    "experimentalDecorators": true,
    "moduleResolution": "node",
    "importHelpers": true,
    "target": "ES2022",
    "module": "ES2022",
    "useDefineForClassFields": false,
    "lib": ["ES2022", "dom"]
  },
  "angularCompilerOptions": {
    "enableI18nLegacyMessageIdFormat": false,
    "strictInjectionParameters": true,
    "strictInputAccessModifiers": true,
    "strictTemplates": true
  }
}
```

### Key TypeScript Concepts for Angular

#### 1. Basic Types

```typescript
// Primitive types
let name: string = 'Angular';
let version: number = 17;
let isAwesome: boolean = true;
let nothing: null = null;
let notDefined: undefined = undefined;

// Arrays
let versions: number[] = [16, 17, 18];
let frameworks: Array<string> = ['Angular', 'React', 'Vue'];

// Enums
enum HttpStatus {
  OK = 200,
  NotFound = 404,
  ServerError = 500
}

// Any, Void, Never
let anything: any = 'could be anything';
function log(message: string): void {
  console.log(message);
}
function throwError(): never {
  throw new Error('This never returns');
}
```

#### 2. Interfaces and Types

```typescript
// Interface - for object shapes
interface User {
  id: number;
  name: string;
  email: string;
  age?: number; // Optional property
  readonly createdAt: Date; // Cannot be modified
}

// Type - more flexible
type UserRole = 'admin' | 'user' | 'guest';

interface UserWithRole extends User {
  role: UserRole;
}

// Using interfaces
const user: User = {
  id: 1,
  name: 'John Doe',
  email: 'john@example.com',
  createdAt: new Date()
};

function greetUser(user: User): string {
  return `Hello, ${user.name}!`;
}
```

#### 3. Classes and OOP

```typescript
// Class with inheritance
abstract class Animal {
  constructor(public name: string) {}
  
  abstract makeSound(): void;
  
  move(): void {
    console.log(`${this.name} is moving`);
  }
}

class Dog extends Animal {
  makeSound(): void {
    console.log('Woof! Woof!');
  }
}

const dog = new Dog('Buddy');
dog.makeSound(); // Woof! Woof!
dog.move(); // Buddy is moving
```

#### 4. Generics

```typescript
// Generic function
function identity<T>(value: T): T {
  return value;
}

const num = identity<number>(42);
const str = identity<string>('Hello');

// Generic class
class Container<T> {
  private value: T;
  
  constructor(value: T) {
    this.value = value;
  }
  
  getValue(): T {
    return this.value;
  }
}

const stringContainer = new Container<string>('TypeScript');
const numberContainer = new Container<number>(100);

// Generic constraints
interface HasLength {
  length: number;
}

function logLength<T extends HasLength>(item: T): void {
  console.log(item.length);
}

logLength('Hello'); // 5
logLength([1, 2, 3]); // 3
logLength({ length: 10 }); // 10
```

#### 5. Decorators (Angular's Magic)

```typescript
// Decorator function
function Log(target: any, propertyKey: string, descriptor: PropertyDescriptor) {
  const originalMethod = descriptor.value;
  
  descriptor.value = function (...args: any[]) {
    console.log(`Calling ${propertyKey} with`, args);
    return originalMethod.apply(this, args);
  };
  
  return descriptor;
}

class Calculator {
  @Log
  add(a: number, b: number): number {
    return a + b;
  }
}

const calc = new Calculator();
calc.add(2, 3); // Logs: Calling add with [2, 3] -> Returns 5
```

#### 6. Union and Intersection Types

```typescript
// Union types
type StringOrNumber = string | number;
let value: StringOrNumber = 'hello';
value = 42; // Also valid

// Type narrowing
function processValue(val: string | number): string {
  if (typeof val === 'string') {
    return val.toUpperCase();
  }
  return val.toFixed(2);
}

// Intersection types
interface Named {
  name: string;
}

interface Aged {
  age: number;
}

type Person = Named & Aged;

const person: Person = {
  name: 'Alice',
  age: 30
};
```

#### 7. Utility Types

```typescript
interface User {
  id: number;
  name: string;
  email: string;
  password: string;
}

// Partial - all properties optional
type PartialUser = Partial<User>;

// Required - all properties required
type RequiredUser = Required<PartialUser>;

// Pick - select specific properties
type UserPreview = Pick<User, 'id' | 'name'>;

// Omit - exclude specific properties
type UserWithoutPassword = Omit<User, 'password'>;

// Readonly
type ReadonlyUser = Readonly<User>;

// Record
type UserDict = Record<string, User>;
```

## Code Examples

### Complete Angular Service Example

```typescript
// user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';

export interface User {
  id: number;
  name: string;
  email: string;
  role: 'admin' | 'user' | 'guest';
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://api.example.com/users';
  
  constructor(private http: HttpClient) {}
  
  getUsers(): Observable<ApiResponse<User[]>> {
    return this.http.get<ApiResponse<User[]>>(this.apiUrl)
      .pipe(
        catchError(this.handleError<ApiResponse<User[]>>('getUsers'))
      );
  }
  
  getUser(id: number): Observable<User | undefined> {
    return this.http.get<User>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError<User>(`getUser id=${id}`))
      );
  }
  
  createUser(user: Omit<User, 'id'>): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }
  
  private handleError<T>(operation = 'operation'): (error: any) => Observable<T> {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      return of(undefined as T);
    };
  }
}
```

## Best Practices

### 1. Enable Strict Mode

```json
// tsconfig.json
{
  "compilerOptions": {
    "strict": true,
    "noImplicitAny": true,
    "strictNullChecks": true,
    "strictPropertyInitialization": true
  }
}
```

### 2. Use Type Inference

```typescript
// Good: Let TypeScript infer when obvious
const numbers = [1, 2, 3];
const first = numbers[0];

// Specify types when not obvious
function process(data: string[]): Map<string, number> {
  // ...
}
```

### 3. Avoid `any`

```typescript
// Bad
function processAnything(data: any): any {
  return data;
}

// Good
function processString(data: string): string {
  return data.toUpperCase();
}
```

## Common Pitfalls and Debugging

### Pitfall 1: Type Errors with null/undefined

```typescript
// Problem
let user: User | null = getUser();
console.log(user.name); // Error: Object is possibly null

// Solution: Use optional chaining and nullish coalescing
console.log(user?.name ?? 'Unknown');
```

### Pitfall 2: Not Understanding `this` in Classes

```typescript
class Handler {
  value = 10;
  
  handle() {
    // Problem: 'this' loses context
    setTimeout(function() {
      console.log(this.value); // undefined
    }, 100);
    
    // Solution: Arrow function preserves 'this'
    setTimeout(() => {
      console.log(this.value); // 10
    }, 100);
  }
}
```

### Pitfall 3: Mutating Objects Marked as Readonly

```typescript
interface Config {
  readonly apiUrl: string;
  readonly maxRetries: number;
}

const config: Config = {
  apiUrl: 'https://api.example.com',
  maxRetries: 3
};

// Error: Cannot assign to 'apiUrl' because it is a read-only property
config.apiUrl = 'https://other.com';
```

## Hands-On Exercise

### Exercise 1.3: TypeScript Practice

**Objective**: Create a typed data layer for an Angular application

**Requirements**:
1. Create interfaces for: Product, Category, Order, Customer
2. Create a Service class with generic CRUD methods
3. Use proper TypeScript features (generics, unions, utility types)
4. Add proper error handling with typed errors

**Deliverable**: A TypeScript module with all interfaces and service

**Assessment Criteria**:
- [ ] At least 4 interfaces with proper typing
- [ ] Generic service with type constraints
- [ ] Proper use of union types
- [ ] At least one custom type alias
- [ ] No use of `any` type

## Extension Challenge

**Challenge**: Create a generic data transformer

```typescript
// Create a utility type that transforms API response to display format
interface ApiResponse<T> {
  data: T;
  metadata: {
    page: number;
    total: number;
  };
}

type TransformToDisplay<T> = {
  items: T[];
  totalPages: number;
  currentPage: number;
  hasNextPage: boolean;
};

// Write a function that transforms ApiResponse to TransformToDisplay
function transformResponse<T>(response: ApiResponse<T>): TransformToDisplay<T> {
  // Implementation here
}
```

## Summary

- TypeScript provides static typing for better code quality
- Interfaces define object shapes, types are more flexible
- Generics enable reusable, type-safe code
- Decorators power Angular's dependency injection and component system
- Always enable strict mode for best practices

## Suggested Reading

- [TypeScript Official Documentation](https://www.typescriptlang.org/docs/)
- "Programming TypeScript" by Boris Cherny
- "Effective TypeScript" by Dan Vanderkam

## Next Steps

In the next lecture, we'll cover Angular CLI and workspace creation.
