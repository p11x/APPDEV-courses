# 📝 Functions Complete Guide

## 📋 Overview

Functions are reusable blocks of code that perform specific tasks. They are fundamental to JavaScript programming and enable code organization, reusability, and abstraction.

---

## 🔨 Function Declarations

### Function Keyword

```javascript
// Basic function declaration
function greet(name) {
    return "Hello, " + name + "!";
}

// Function with multiple parameters
function add(a, b) {
    return a + b;
}

// Function with default parameters
function greet(name = "Guest") {
    return "Hello, " + name + "!";
}

console.log(greet("John"));  // "Hello, John!"
console.log(greet());        // "Hello, Guest!"
```

### Function Hoisting

```javascript
// Function declarations are hoisted
console.log(add(2, 3)); // 5 - works!

function add(a, b) {
    return a + b;
}
```

---

## 🎯 Function Expressions

### Anonymous Functions

```javascript
// Function expression (not hoisted)
const greet = function(name) {
    return "Hello, " + name + "!";
};

console.log(greet("John")); // "Hello, John!"
```

### Named Function Expressions

```javascript
const factorial = function fact(n) {
    if (n <= 1) return 1;
    return n * fact(n - 1);
};

console.log(factorial(5)); // 120
```

---

## ➡️ Arrow Functions

### Basic Syntax

```javascript
// Full arrow function
const add = (a, b) => {
    return a + b;
};

// Implicit return (single expression)
const add = (a, b) => a + b;

// Single parameter (parentheses optional)
const square = x => x * x;

// No parameters
const getRandom = () => Math.random();
```

### Arrow Functions and `this`

```javascript
// ❌ Regular function - 'this' varies
function Timer() {
    this.time = 0;
    setInterval(function() {
        this.time++; // 'this' is not Timer!
    }, 1000);
}

// ✅ Arrow function - 'this' is lexically bound
function Timer() {
    this.time = 0;
    setInterval(() => {
        this.time++; // 'this' is Timer
    }, 1000);
}
```

---

## 📦 Parameters

### Rest Parameters

```javascript
// Collect remaining arguments into array
function sum(...numbers) {
    return numbers.reduce((total, n) => total + n, 0);
}

console.log(sum(1, 2, 3, 4, 5)); // 15

function format(text, ...args) {
    args.forEach(arg => {
        text = text.replace("{}", arg);
    });
    return text;
}

console(format("Hello {} and {}", "John", "Jane")); // "Hello John and Jane"
```

### Arguments Object (Legacy)

```javascript
function showArgs() {
    console.log(arguments); // Array-like object
    console.log(arguments.length);
    console.log(arguments[0]);
}

showArgs(1, 2, 3); // Arguments {0: 1, 1: 2, 2: 3}
```

---

## 🔄 Return Values

### Multiple Returns

```javascript
function divide(a, b) {
    if (b === 0) {
        return { error: "Cannot divide by zero" };
    }
    return { result: a / b };
}

const output = divide(10, 2);
if (output.error) {
    console.error(output.error);
} else {
    console.log(output.result);
}
```

### Early Return Pattern

```javascript
function processUser(user) {
    // Early validation returns
    if (!user) return null;
    if (!user.name) return null;
    if (!user.email) return null;
    
    // Main logic
    return {
        name: user.name,
        email: user.email,
        validated: true
    };
}
```

---

## 🎯 Higher-Order Functions

### Functions as Arguments

```javascript
function calculate(a, b, operation) {
    return operation(a, b);
}

const add = (a, b) => a + b;
const multiply = (a, b) => a * b;

console.log(calculate(5, 3, add));      // 8
console.log(calculate(5, 3, multiply)); // 15
```

### Functions Returning Functions

```javascript
function multiplier(factor) {
    return function(number) {
        return number * factor;
    };
}

const double = multiplier(2);
const triple = multiplier(3);

console.log(double(5)); // 10
console.log(triple(5)); // 15
```

---

## 🔒 Closures

### What is a Closure?

```javascript
function createCounter() {
    let count = 0; // Private variable
    
    return {
        increment() { count++; return count; },
        decrement() { count--; return count; },
        getCount() { return count; }
    };
}

const counter = createCounter();
console.log(counter.increment()); // 1
console.log(counter.increment()); // 2
console.log(counter.getCount());   // 2
```

### Practical Closures

```javascript
// Private data
function createUser(name) {
    let _name = name; // Private
    
    return {
        getName() { return _name; },
        setName(newName) { _name = newName; }
    };
}

const user = createUser("John");
console.log(user.getName()); // "John"
user.setName("Jane");
console.log(user.getName()); // "Jane"
// user._name would be undefined - it's private!
```

---

## ⚡ Best Practices

### Naming Functions

```javascript
// ✅ Verb-based names
function calculateTotal() {}
function fetchUserData() {}
function validateEmail() {}

// ❌ Nouns
function total() {} // Unclear purpose
function user() {}  // Verb is better
```

### Single Responsibility

```javascript
// ❌ Multiple responsibilities
function processUser(user) {
    // validate
    // format
    // save
    // send email
}

// ✅ Single responsibility
function validateUser(user) {}
function formatUser(user) {}
function saveUser(user) {}
function sendWelcomeEmail(user) {}
```

---

## 🎯 Practice Exercise

### Callback-Based Event System

```javascript
class EventEmitter {
    constructor() {
        this.events = {};
    }
    
    on(event, callback) {
        if (!this.events[event]) {
            this.events[event] = [];
        }
        this.events[event].push(callback);
    }
    
    emit(event, ...args) {
        if (this.events[event]) {
            this.events[event].forEach(cb => cb(...args));
        }
    }
}

// Usage
const emitter = new EventEmitter();
emitter.on('message', (msg) => console.log('Received:', msg));
emitter.emit('message', 'Hello World!');
```

---

## 🔗 Related Topics

- [04_Variables_Deep_Dive.md](./04_Variables_Deep_Dive.md)
- [06_Control_Flow_Deep_Dive.md](./06_Control_Flow_Deep_Dive.md)

---

**Next: Learn about [Promises Complete Guide](../08_ASYNC_JAVASCRIPT/02_Promises_Complete_Guide.md)**