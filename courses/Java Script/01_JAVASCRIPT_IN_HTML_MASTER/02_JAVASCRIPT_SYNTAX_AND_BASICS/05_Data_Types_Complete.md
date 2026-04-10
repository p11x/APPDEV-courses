# JavaScript Data Types Complete Guide

This guide provides an in-depth exploration of JavaScript data types, covering both primitive and reference types, type checking methods, type conversion techniques, and practical real-world examples. Understanding data types is fundamental to mastering JavaScript and building robust applications.

---

## 🎯 Introduction to Data Types

JavaScript is a dynamically typed language, meaning variables do not require explicit type declarations. The interpreter determines the type at runtime based on the assigned value. This flexibility makes JavaScript approachable but also requires careful type management to avoid unexpected behavior.

JavaScript data types are broadly categorized into two groups:

- **Primitive Types**: Immutable, value-based types stored directly in memory
- **Reference Types**: Complex types stored as references (pointers) to memory locations

Understanding the distinction between these types is crucial because it affects how values are assigned, compared, and passed between functions.

---

## 🔢 Primitive Types

Primitive types represent single, immutable values. When you assign a primitive value to a variable, the actual value is stored. Each primitive type serves a specific purpose in JavaScript applications.

### String

A string is a sequence of characters used to represent text. Strings are enclosed in single quotes (`'`), double quotes (`"`), or backticks (`` ` ``) for template literals.

```javascript
// Different ways to create strings
const singleQuotes = 'Hello, World!';
const doubleQuotes = "Hello, World!";
const templateLiteral = `Hello, World!`;

// Template literals allow interpolation
const name = 'Alice';
const greeting = `Hello, ${name}!`; // "Hello, Alice!"

// Common string methods
const message = 'JavaScript is awesome';
console.log(message.length); // 21
console.log(message.toUpperCase()); // "JAVASCRIPT IS AWESOME"
console.log(message.includes('awesome')); // true
console.log(message.split(' ')); // ["JavaScript", "is", "awesome"]
```

**Real-World Example: User Input Validation**

```javascript
function validateEmail(email) {
    // Check if email contains @ and has valid structure
    const atIndex = email.indexOf('@');
    const dotIndex = email.lastIndexOf('.');
    
    if (atIndex === -1 || dotIndex === -1) {
        return false;
    }
    
    if (atIndex < 1 || dotIndex < atIndex + 2 || dotIndex === email.length - 1) {
        return false;
    }
    
    return true;
}

const userEmail = 'user@example.com';
if (validateEmail(userEmail)) {
    console.log('Email is valid');
} else {
    console.log('Please enter a valid email address');
}
```

### Number

The number type represents both integers and floating-point numbers. JavaScript uses 64-bit floating-point representation (IEEE 754), following international IEEE 754 standard. There is no separate integer type.

```javascript
// Integer and floating-point numbers
const integer = 42;
const floatingPoint = 3.14159;
const negative = -17;

// Special values
const infinity = Infinity;
const negativeInfinity = -Infinity;
const notANumber = NaN; // Represents invalid numeric operations

// Numeric operations
console.log(10 + 5);       // 15
console.log(10 - 5);       // 5
console.log(10 * 5);       // 50
console.log(10 / 5);       // 2
console.log(10 % 3);        // 1 (modulo)
console.log(10 ** 2);      // 100 (exponentiation)

// Precision issues
console.log(0.1 + 0.2);    // 0.30000000000000004
console.log((0.1 + 0.2).toFixed(2)); // "0.30"

// Number methods
const num = 42.567;
console.log(Number.isInteger(42));    // true
console.log(Number.isNaN(NaN));    // true
console.log(num.toFixed(1));       // "42.6"
parseInt('42');                     // 42
parseFloat('3.14');                 // 3.14
```

**Real-World Example: Currency Calculator**

```javascript
function calculateTotalWithTax(price, taxRate) {
    // Round to avoid floating-point precision issues
    const tax = Math.round(price * taxRate * 100) / 100;
    const total = Math.round((price + tax) * 100) / 100;
    
    return {
        subtotal: price,
        tax: tax,
        total: total,
        formatted: `$${total.toFixed(2)}`
    };
}

const itemPrice = 29.99;
const result = calculateTotalWithTax(itemPrice, 0.08);
console.log(result.formatted); // "$32.39"
```

### Boolean

Boolean represents logical entities with two possible values: `true` or `false`. Booleans are essential for conditional logic and decision-making in JavaScript.

```javascript
// Boolean values
const isActive = true;
const isDisabled = false;

// Comparison operators return booleans
console.log(10 > 5);         // true
console.log(10 === 10);       // true (strict equality)
console.log(10 !== 5);       // true
console.log('hello' === 'hello'); // true

// Logical operators
console.log(true && false);    // false
console.log(true || false);    // true
console.log(!true);          // false

// Truthy and falsy values (evaluate to boolean in conditions)
// Falsy values: false, 0, '', null, undefined, NaN
// Truthy values: Everything else (including '0', [], {})

const userName = '';
if (!userName) {
    console.log('Please enter your name');
}
```

**Real-World Example: User Permissions**

```javascript
function checkPermission(user, action) {
    const permissions = {
        read: true,
        write: false,
        delete: false,
        admin: false
    };
    
    return permissions[action] && user.isActive;
}

const currentUser = { name: 'Alice', isActive: true };
const canRead = checkPermission(currentUser, 'read');
const canDelete = checkPermission(currentUser, 'delete');

if (canRead) {
    console.log('User can access the document');
}

if (!canDelete) {
    console.log('User does not have delete permission');
}
```

### Undefined

Undefined represents a variable that has been declared but not assigned a value. It is the default value for uninitialized variables.

```javascript
// Variables with no assigned value
let message;
console.log(message); // undefined

// Accessing non-existent object properties
const person = { name: 'Alice' };
console.log(person.age); // undefined

// Function parameters that are not passed
function greet(name) {
    console.log(`Hello, ${name}`);
}
greet(); // "Hello, undefined"

// Distinguishing undefined from other values
console.log(typeof undefined); // "undefined"
console.log(undefined === undefined); // true
```

**Real-World Example: Optional Function Parameters**

```javascript
function createUser(name, age, role = 'user') {
    // Provide default values for undefined parameters
    return {
        name: name || 'Anonymous',
        age: age || 0,
        role: role,
        createdAt: new Date()
    };
}

const user1 = createUser('Alice', 25, 'admin');
const user2 = createUser('Bob'); // Uses defaults for age and role
const user3 = createUser(); // All defaults

console.log(user1); // { name: 'Alice', age: 25, role: 'admin', ... }
console.log(user3); // { name: 'Anonymous', age: 0, role: 'user', ... }
```

### Null

Null represents an intentional absence of any value. It is often assigned to variables to indicate that they intentionally contain no object or value.

```javascript
// Explicitly setting a variable to null
let currentUser = null;
console.log(currentUser); // null

// Checking for null
console.log(null === null); // true

// Null vs undefined
let a;
let b = null;

console.log(a === b); // false (different types)
console.log(typeof null); // "object" (historical bug in JavaScript)
```

**Real-World Example: Database Connection**

```javascript
class DatabaseConnection {
    constructor() {
        this.connection = null;
    }
    
    async connect() {
        // Simulate connection
        this.connection = {
            host: 'localhost',
            database: 'myapp',
            connected: true
        };
        console.log('Connected to database');
    }
    
    disconnect() {
        this.connection = null;
        console.log('Disconnected from database');
    }
    
    query(sql) {
        if (!this.connection) {
            throw new Error('No database connection');
        }
        console.log(`Executing: ${sql}`);
        return [];
    }
}

const db = new DatabaseConnection();
db.connect();
db.disconnect();
```

### Symbol

Symbols are unique, immutable identifiers created with the `Symbol()` function. Each symbol is guaranteed to be unique, making them useful for creating unique object properties.

```javascript
// Creating symbols
const symbol1 = Symbol('description');
const symbol2 = Symbol('description');

console.log(symbol1 === symbol2); // false (unique!)
console.log(typeof symbol1);   // "symbol"

// Using symbols as object keys
const user = {
    name: 'Alice',
    [Symbol('id')]: 12345
};

const idSymbol = Symbol('id');
console.log(user[idSymbol]); // 12345
```

**Real-World Example: Unique Event Identifiers**

```javascript
const Events = {
    USER_LOGIN: Symbol('user login'),
    USER_LOGOUT: Symbol('user logout'),
    DATA_LOADED: Symbol('data loaded'),
    ERROR_OCCURRED: Symbol('error occurred')
};

class EventEmitter {
    constructor() {
        this.listeners = new Map();
    }
    
    on(event, callback) {
        if (!this.listeners.has(event)) {
            this.listeners.set(event, []);
        }
        this.listeners.get(event).push(callback);
    }
    
    emit(event, data) {
        const callbacks = this.listeners.get(event);
        if (callbacks) {
            callbacks.forEach(callback => callback(data));
        }
    }
}

const emitter = new EventEmitter();
emitter.on(Events.USER_LOGIN, (user) => {
    console.log(`User ${user.name} logged in`);
});

emitter.emit(Events.USER_LOGIN, { name: 'Alice' });
```

### BigInt

BigInt represent integers larger than the safe integer limit (2^53 - 1). BigInt values are created by appending `n` to an integer literal or using the `BigInt()` function.

```javascript
// Creating BigInt values
const bigNumber = 9007199254740993n; // Too large for regular number
const fromFunction = BigInt('9007199254740993');

// BigInt operations
const a = 100n + 50n;        // 150n
const b = 50n * 3n;          // 150n
const c = 10n ** 2n;        // 100n

// Comparison
console.log(10n === BigInt(10)); // false (different types)
console.log(10n == 10);         // true (loose equality)

// Division (truncated)
console.log(25n / 5n);          // 5n
// console.log(25n / 5);       // Error: cannot mix BigInt and number
```

**Real-World Example: Financial Calculations**

```javascript
// Using BigInt for precise integer calculations (like currency in cents)
function calculateTotal(items) {
    let total = 0n;
    
    for (const item of items) {
        const priceInCents = BigInt(item.price * 100);
        const quantity = BigInt(item.quantity);
        total += priceInCents * quantity;
    }
    
    return total;
}

function formatBigIntAsCurrency(bigIntCents) {
    const cents = bigIntCents % 100n;
    const dollars = bigIntCents / 100n;
    return `$${dollars}.${cents.toString().padStart(2, '0')}`;
}

const cart = [
    { name: 'Widget', price: 19.99, quantity: 3 },
    { name: 'Gadget', price: 149.99, quantity: 1 }
];

const totalCents = calculateTotal(cart);
console.log(formatBigIntAsCurrency(totalCents)); // "$229.96"
```

---

## 📋 Reference Types

Reference types are complex types that store references (pointers) to values in memory. They can hold multiple values and complex data structures.

### Object

Objects are collections of key-value pairs where keys are strings (or Symbols) and values can be any type. Objects are created using curly braces `{}` or the `Object()` constructor.

```javascript
// Creating objects
const person = {
    name: 'Alice',
    age: 30,
    isActive: true,
    address: {
        city: 'New York',
        country: 'USA'
    },
    greet() {
        return `Hello, I'm ${this.name}`;
    }
};

// Accessing properties
console.log(person.name);        // "Alice" (dot notation)
console.log(person['age']);     // 30 (bracket notation)

// Modifying objects
person.email = 'alice@example.com';
person['phone'] = '555-1234';

// Object methods
console.log(Object.keys(person));     // ["name", "age", "isActive", "address", ...]
console.log(Object.values(person)); // ["Alice", 30, true, ...]
console.log(Object.entries(person)); // [["name", "Alice"], ["age", 30], ...]

// Destructuring
const { name, age } = person;
console.log(name); // "Alice"
```

**Real-World Example: User Profile**

```javascript
function createUserProfile(userData) {
    return {
        id: userData.id || crypto.randomUUID(),
        username: userData.username,
        email: userData.email,
        profile: {
            firstName: userData.firstName || '',
            lastName: userData.lastName || '',
            bio: userData.bio || '',
            avatar: userData.avatar || 'default.png'
        },
        settings: {
            theme: userData.theme || 'light',
            notifications: userData.notifications ?? true,
            language: userData.language || 'en'
        },
        createdAt: new Date(),
        lastLogin: null,
        
        updateLastLogin() {
            this.lastLogin = new Date();
        },
        
        getFullName() {
            return `${this.profile.firstName} ${this.profile.lastName}`.trim();
        }
    };
}

const userData = {
    username: 'alice123',
    email: 'alice@example.com',
    firstName: 'Alice',
    lastName: 'Smith'
};

const profile = createUserProfile(userData);
profile.updateLastLogin();
console.log(profile.getFullName()); // "Alice Smith"
```

### Array

Arrays are ordered collections of values, indexed by numeric indices starting from 0. Arrays in JavaScript are dynamic and can grow or shrink. They can hold values of any type.

```javascript
// Creating arrays
const fruits = ['apple', 'banana', 'orange'];
const mixed = [1, 'hello', true, { name: 'Alice' }];
const numbers = new Array(1, 2, 3);

// Accessing elements
console.log(fruits[0]);    // "apple"
console.log(fruits.length); // 3

// Array methods
fruits.push('grape');     // Add to end
fruits.pop();             // Remove from end
fruits.unshift('mango');  // Add to beginning
fruits.shift();          // Remove from beginning

// Iterating
fruits.forEach((fruit, index) => {
    console.log(`${index}: ${fruit}`);
});

// Transformation
const upperFruits = fruits.map(fruit => fruit.toUpperCase());
const shortFruits = fruits.filter(fruit => fruit.length > 5);

// Finding
const found = fruits.find(fruit => fruit.startsWith('a'));
const index = fruits.findIndex(fruit => fruit === 'banana');

// Array with holes
const sparse = [1, , 3]; // Index 1 is empty
console.log(1 in sparse); // false

// Multidimensional arrays
const matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];
console.log(matrix[1][1]); // 5
```

**Real-World Example: Todo List**

```javascript
class TodoList {
    constructor() {
        this.todos = [];
    }
    
    addTodo(title, priority = 'medium') {
        const todo = {
            id: Date.now(),
            title: title,
            completed: false,
            priority: priority,
            createdAt: new Date()
        };
        this.todos.push(todo);
        return todo;
    }
    
    completeTodo(id) {
        const todo = this.todos.find(t => t.id === id);
        if (todo) {
            todo.completed = true;
            todo.completedAt = new Date();
        }
    }
    
    deleteTodo(id) {
        const index = this.todos.findIndex(t => t.id === id);
        if (index !== -1) {
            this.todos.splice(index, 1);
        }
    }
    
    getPendingTodos() {
        return this.todos.filter(t => !t.completed);
    }
    
    getCompletedTodos() {
        return this.todos.filter(t => t.completed);
    }
    
    sortByPriority() {
        const priorityOrder = { high: 3, medium: 2, low: 1 };
        this.todos.sort((a, b) => 
            priorityOrder[b.priority] - priorityOrder[a.priority]
        );
    }
}

const todoList = new TodoList();
todoList.addTodo('Learn JavaScript', 'high');
todoList.addTodo('Build a project', 'medium');
todoList.addTodo('Read documentation', 'low');

const pendingTodos = todoList.getPendingTodos();
console.log(pendingTodos.length); // 3

todoList.sortByPriority();
console.log(todoList.todos[0].title); // "Learn JavaScript"
```

### Function

Functions are first-class objects in JavaScript, meaning they can be assigned to variables, passed as arguments, and returned from other functions. Functions can be declared in multiple ways.

```javascript
// Function declaration
function greet(name) {
    return `Hello, ${name}!`;
}

// Function expression
const greetExpr = function(name) {
    return `Hello, ${name}!`;
};

// Arrow function
const greetArrow = (name) => `Hello, ${name}!`;

// Arrow function with multiple statements
const calculate = (a, b) => {
    const sum = a + b;
    const product = a * b;
    return { sum, product };
};

// Immediately Invoked Function Expression (IIFE)
(function() {
    console.log('Runs immediately');
})();

// Higher-order function (takes function as argument)
function withLogging(fn) {
    return function(...args) {
        console.log(`Calling ${fn.name} with:`, args);
        return fn(...args);
    };
}

const loggedGreet = withLogging(greet);
loggedGreet('Alice');
```

**Real-World Example: Form Validation**

```javascript
function createValidator(rules) {
    return function validate(formData) {
        const errors = {};
        
        for (const [field, ruleSet] of Object.entries(rules)) {
            const value = formData[field];
            
            for (const rule of ruleSet) {
                const error = rule(field, value);
                if (error) {
                    errors[field] = error;
                    break;
                }
            }
        }
        
        return {
            isValid: Object.keys(errors).length === 0,
            errors: errors
        };
    };
}

// Validation rules
const rules = {
    email: [
        (field, value) => !value ? `${field} is required` : null,
        (field, value) => !value.includes('@') ? 'Invalid email format' : null
    ],
    password: [
        (field, value) => !value ? `${field} is required` : null,
        (field, value) => value.length < 8 ? 'Password must be at least 8 characters' : null,
        (field, value) => !/[A-Z]/.test(value) ? 'Password must contain an uppercase letter' : null
    ],
    age: [
        (field, value) => !value ? `${field} is required` : null,
        (field, value) => value < 18 ? 'Must be at least 18 years old' : null
    ]
};

const validateForm = createValidator(rules);
const formData = {
    email: 'invalid-email',
    password: 'short',
    age: 16
};

const result = validateForm(formData);
console.log(result.isValid); // false
console.log(result.errors); // { email: 'Invalid email format', password: '...', age: '...' }
```

---

## 🔍 Type Checking

JavaScript provides multiple ways to check the type of a value. Understanding these methods helps ensure type safety in your applications.

### typeof Operator

The `typeof` operator returns a string indicating the type of the operand.

```javascript
// Basic type checking
console.log(typeof 'hello');     // "string"
console.log(typeof 42);          // "number"
console.log(typeof true);       // "boolean"
console.log(typeof undefined); // "undefined"
console.log(typeof null);       // "object" (historical bug!)
console.log(typeof Symbol('id')); // "symbol"
console.log(typeof 10n);        // "bigint"
console.log(typeof {});        // "object"
console.log(typeof []);        // "object"
console.log(typeof function() {}); // "function"
```

**Usage Guidelines**

```javascript
// Safe type checking function
function getType(value) {
    if (value === null) return 'null';
    if (value === undefined) return 'undefined';
    
    const type = typeof value;
    if (type === 'object') {
        if (Array.isArray(value)) return 'array';
        if (value instanceof Date) return 'date';
        if (value instanceof Error) return 'error';
        if (value instanceof RegExp) return 'regexp';
    }
    return type;
}
```

### instanceof Operator

The `instanceof` operator checks if an object is an instance of a particular class or constructor.

```javascript
// instanceof checks
const arr = [];
console.log(arr instanceof Array);       // true
console.log(arr instanceof Object);     // true

const date = new Date();
console.log(date instanceof Date);       // true

// Custom classes
class User {}
const user = new User();
console.log(user instanceof User);      // true

// Prototype chain
console.log(user instanceof Object);    // true
```

### Array.isArray()

The `Array.isArray()` method is the reliable way to check if a value is an array.

```javascript
// Array checking
console.log(Array.isArray([]));        // true
console.log(Array.isArray([1, 2, 3])); // true
console.log(Array.isArray({}));       // false
console.log(Array.isArray('hello')); // false
console.log(Array.isArray(undefined)); // false
```

**Real-World Example: Type-Safe Data Processing**

```javascript
function processData(data) {
    if (data === null || data === undefined) {
        console.log('No data provided');
        return;
    }
    
    if (Array.isArray(data)) {
        return data.map(item => processItem(item));
    }
    
    if (typeof data === 'object') {
        return processObject(data);
    }
    
    if (typeof data === 'string') {
        return data.toUpperCase();
    }
    
    if (typeof data === 'number') {
        return data * 2;
    }
    
    console.log('Unknown data type');
    return data;
}

function processItem(item) {
    return { ...item, processed: true };
}

function processObject(obj) {
    return { ...obj, processed: true };
}
```

---

## ↔️ Type Conversion

JavaScript performs type conversion both implicitly (automatically) and explicitly (manually).

### Implicit Conversion

JavaScript automatically converts types in certain operations, following specific rules.

```javascript
// String concatenation with +
console.log('hello' + 5);        // "hello5"
console.log(5 + '5');           // "55"
console.log(true + '5');       // "true5"

// Numeric conversion
console.log('5' - 2);          // 3
console.log('5' * 2);          // 10
console.log('5' / 2);          // 2.5
console.log('5' % 2);         // 1

// Boolean conversion (truthy/falsy)
console.log(Boolean(''));     // false
console.log(Boolean('hello')); // true
console.log(Boolean(0));      // false
console.log(Boolean(42));    // true

// Comparisons with conversion
console.log('5' == 5);        // true (loose equality)
console.log('5' === 5);       // false (strict equality)
console.log(null == undefined); // true
console.log(null === undefined); // false
```

### Explicit Conversion

Use explicit conversion functions for clear, intentional type changes.

```javascript
// To String
String(42);              // "42"
(42).toString();         // "42"
'' + 42;                 // "42"

// To Number
Number('42');            // 42
parseInt('42');           // 42
parseFloat('3.14');       // 3.14
+'42';                   // 42

// To Boolean
Boolean(42);             // true
!!42;                    // true
Boolean('');            // false

// To Array
Array.from('hello');     // ["h", "e", "l", "l", "o"]
[...'hello'];            // ["h", "e", "l", "l", "o"]

// To Object
Object('hello');         // String {"hello"}
Object(42);              // Number {42}
```

**Real-World Example: Form Input Processing**

```javascript
function processFormInput(input) {
    // Convert string input to appropriate type
    if (input === '' || input === null || input === undefined) {
        return null;
    }
    
    // Check for specific format
    const trimmed = input.trim();
    
    // Check for boolean-like values
    if (['true', 'false', '1', '0', 'yes', 'no'].includes(trimmed.toLowerCase())) {
        return trimmed.toLowerCase() === 'true' || 
               trimmed === '1' || 
               trimmed.toLowerCase() === 'yes';
    }
    
    // Check for number
    const num = Number(trimmed);
    if (!isNaN(num) && trimmed !== '') {
        return num;
    }
    
    // Return string otherwise
    return trimmed;
}

// Usage examples
console.log(processFormInput('  42  '));       // 42
console.log(processFormInput('  true  '));    // true
console.log(processFormInput('yes'));         // true
console.log(processFormInput('hello world')); // "hello world"
console.log(processFormInput(''));           // null
```

---

## 📊 Practice Exercises

Test your understanding of JavaScript data types with these exercises.

### Exercise 1: Type Identification

```javascript
// What is the typeof each value?

// 1.
let x;
console.log(typeof x);

// 2.
let y = null;
console.log(typeof y);

// 3.
const z = [];
console.log(typeof z);

// 4.
const w = Symbol('key');
console.log(typeof w);

// 5.
const v = 10n;
console.log(typeof v);

// Answers:
// 1. undefined
// 2. object (bug in JavaScript)
// 3. object
// 4. symbol
// 5. bigint
```

### Exercise 2: Fix the Bugs

```javascript
// Find and fix the type-related bugs

// Bug 1: Floating point precision
const price1 = 19.99;
const price2 = 5.00;
const total = price1 + price2;
console.log(total); // Expected: 24.99, Got: 24.990000000000003

// Fix:
const fixedTotal = Math.round((price1 + price2) * 100) / 100;
console.log(fixedTotal); // 24.99

// Bug 2: Array comparison
const arr1 = [1, 2, 3];
const arr2 = [1, 2, 3];
console.log(arr1 === arr2); // Expected: true, Got: false

// Fix: Compare elements instead
const isEqual = arr1.length === arr2.length && 
                arr1.every((val, idx) => val === arr2[idx]);
console.log(isEqual); // true
```

### Exercise 3: Build a Type Checker

```javascript
// Create a function that returns detailed type information

function getDetailedType(value) {
    if (value === null) {
        return { type: 'null', category: 'primitive' };
    }
    
    if (value === undefined) {
        return { type: 'undefined', category: 'primitive' };
    }
    
    const basicType = typeof value;
    
    if (basicType === 'object') {
        if (Array.isArray(value)) {
            return { type: 'array', category: 'reference' };
        }
        if (value instanceof Date) {
            return { type: 'date', category: 'reference' };
        }
        if (value instanceof Error) {
            return { type: 'error', category: 'reference' };
        }
        return { type: 'object', category: 'reference' };
    }
    
    if (basicType === 'function') {
        return { type: 'function', category: 'reference' };
    }
    
    return { type: basicType, category: 'primitive' };
}

// Test the function
console.log(getDetailedType(42));
console.log(getDetailedType('hello'));
console.log(getDetailedType([]));
console.log(getDetailedType(new Date()));
console.log(getDetailedType(null));
```

---

## 📋 Summary Comparison Table

| Type | Category | Created By | Mutable | typeof Result |
|------|----------|------------|---------|---------------|
| string | Primitive | 'text', "text", `text` | No | "string" |
| number | Primitive | 42, 3.14 | No | "number" |
| boolean | Primitive | true, false | No | "boolean" |
| undefined | Primitive | let x; | No | "undefined" |
| null | Primitive | null | No | "object" |
| symbol | Primitive | Symbol() | No | "symbol" |
| bigint | Primitive | 42n | No | "bigint" |
| object | Reference | {}, new Object() | Yes | "object" |
| array | Reference | [], new Array() | Yes | "object" |
| function | Reference | function(){}, () => {} | Yes | "function" |

### Key Differences

**Primitive Types:**

- Stored by value
- Immutable (cannot be changed)
- Compared by value
- Copied entirely when assigned

**Reference Types:**

- Stored by reference (pointer to memory location)
- Mutable (can be changed)
- Compared by reference (identity)
- Copy creates reference to same object

### Type Conversion Quick Reference

| From | To String | To Number | To Boolean |
|------|----------|----------|-----------|
| "" | - | 0 | false |
| "5" | - | 5 | true |
| "hello" | - | NaN | true |
| 0 | "0" | - | false |
| 1 | "1" | - | true |
| null | "null" | 0 | false |
| undefined | "undefined" | NaN | false |
| [] | "" | 0 | true |
| {} | "[object Object]" | NaN | true |

---

## 📚 Related Topics

- [04 Variables Deep Dive](./04_Variables_Deep_Dive.md) - Learn about variable declarations and scoping
- **Upcoming:** 06 Operators Complete - Master arithmetic, comparison, and logical operators
- **Upcoming:** 07 Control Flow - Learn about conditionals, loops, and error handling
- **Upcoming:** 08 Functions Deep Dive - Explore functions, closures, and the call stack

---

## 📖 Further Reading

- [MDN: Working with Objects](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Working_with_Objects)
- [MDN: Indexed Collections](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Indexed_collections)
- [MDN: Details of the Object Model](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Details_of_the_Object_Model)

---

*Last Updated: 2026-04-03*