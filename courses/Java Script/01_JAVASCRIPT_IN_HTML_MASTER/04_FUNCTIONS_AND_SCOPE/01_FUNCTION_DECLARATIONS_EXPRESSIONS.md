# 📝 Function Declarations vs Expressions

## 📋 Table of Contents

1. [Overview](#overview)
2. [Function Declarations](#function-declarations)
3. [Function Expressions](#function-expressions)
4. [Hoisting Behavior](#hoisting-behavior)
5. [IIFE (Immediately Invoked Function Expressions)](#iife-immediately-invoked-function-expressions)
6. [Function Types Comparison](#function-types-comparison)
7. [Professional Use Cases](#professional-use-cases)
8. [Common Pitfalls](#common-pitfalls)
9. [Key Takeaways](#key-takeaways)

---

## Overview

Understanding the difference between function declarations and function expressions is fundamental to mastering JavaScript. This knowledge impacts code organization, hoisting behavior, closure creation, and overall program architecture. In this comprehensive guide, we'll explore every aspect of these two primary function definition patterns, including their unique characteristics, use cases, and the critical concept of IIFE (Immediately Invoked Function Expressions).

The distinction between declarations and expressions affects when and how functions can be used in your code. This becomes especially important when working with callbacks, event handlers, module patterns, and asynchronous programming. By the end of this guide, you'll have a thorough understanding of when to use each approach and why.

---

## Function Declarations

### Basic Syntax and Characteristics

A function declaration (also known as a function statement) defines a named function that can be called anywhere in the scope where it was defined. Function declarations are one of the most common ways to create functions in JavaScript and form the backbone of many applications.

```javascript
// students/01_functionDeclarations.js

// Basic function declaration
function calculateGrade(score) {
    if (score >= 90) return 'A';
    if (score >= 80) return 'B';
    if (score >= 70) return 'C';
    if (score >= 60) return 'D';
    return 'F';
}

// Multiple parameters
function calculateWeightedGrade(score, weight) {
    return score * weight;
}

// Function with default parameters (ES6+)
function greetUser(name = 'Guest', greeting = 'Hello') {
    return `${greeting}, ${name}!`;
}

// Function returning multiple values via object
function getStats(numbers) {
    const sum = numbers.reduce((a, b) => a + b, 0);
    const average = sum / numbers.length;
    const min = Math.min(...numbers);
    const max = Math.max(...numbers);
    
    return { sum, average, min, max };
}

// Usage examples
console.log(calculateGrade(85));    // 'B'
console.log(calculateWeightedGrade(85, 0.4));  // 34
console.log(greetUser('Alice'));    // 'Hello, Alice!'
console.log(greetUser('Bob', 'Hi')); // 'Hi, Bob!'
console.log(getStats([85, 90, 78, 92, 88]));
// { sum: 433, average: 86.6, min: 78, max: 92 }
```

### Recursive Function Declarations

Function declarations can reference themselves, making them ideal for recursive algorithms:

```javascript
// students/02_recursiveDeclarations.js

// Factorial calculation
function factorial(n) {
    if (n <= 1) return 1;
    return n * factorial(n - 1);
}

// Fibonacci sequence
function fibonacci(n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

// Binary search (sorted array)
function binarySearch(arr, target) {
    return binarySearchHelper(arr, target, 0, arr.length - 1);
}

function binarySearchHelper(arr, target, left, right) {
    if (left > right) return -1;
    
    const mid = Math.floor((left + right) / 2);
    
    if (arr[mid] === target) return mid;
    if (arr[mid] < target) return binarySearchHelper(arr, target, mid + 1, right);
    return binarySearchHelper(arr, target, left, mid - 1);
}

// Flatten nested arrays
function flattenArray(arr) {
    const result = [];
    
    for (const item of arr) {
        if (Array.isArray(item)) {
            result.push(...flattenArray(item));
        } else {
            result.push(item);
        }
    }
    
    return result;
}

// Usage
console.log(factorial(5));        // 120
console.log(fibonacci(10));       // 55
console.log(binarySearch([1, 2, 3, 4, 5, 6, 7, 8, 9], 5));  // 4
console.log(flattenArray([1, [2, [3, 4]], 5]));  // [1, 2, 3, 4, 5]
```

---

## Function Expressions

### Anonymous Function Expressions

A function expression assigns a function to a variable. The function can be anonymous (unnamed) or named:

```javascript
// students/03_functionExpressions.js

// Anonymous function expression
const calculateGrade = function(score) {
    if (score >= 90) return 'A';
    if (score >= 80) return 'B';
    if (score >= 70) return 'C';
    if (score >= 60) return 'D';
    return 'F';
};

console.log(calculateGrade(85));  // 'B'

// Using with array methods
const numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

const evens = numbers.filter(function(n) {
    return n % 2 === 0;
});

const doubled = numbers.map(function(n) {
    return n * 2;
});

const sum = numbers.reduce(function(acc, n) {
    return acc + n;
}, 0);

console.log(evens);    // [2, 4, 6, 8, 10]
console.log(doubled);  // [2, 4, 6, 8, 10, 12, 14, 16, 18, 20]
console.log(sum);     // 55
```

### Named Function Expressions

Named function expressions provide internal name reference:

```javascript
// students/04_namedFunctionExpressions.js

// Named function expression for recursion
const factorial = function fact(n) {
    if (n <= 1) return 1;
    return n * fact(n - 1);
};

console.log(factorial(5));  // 120

// Error-stack friendly (named for debugging)
const deepClone = function cloneObject(obj) {
    if (obj === null || typeof obj !== 'object') return obj;
    if (Array.isArray(obj)) return obj.map(cloneObject);
    
    const cloned = {};
    for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
            cloned[key] = cloneObject(obj[key]);
        }
    }
    return cloned;
};

const original = { a: 1, b: { c: 2 } };
const copy = deepClone(original);
console.log(copy);  // { a: 1, b: { c: 2 } }
console.log(original !== copy);  // true
```

### Function Expressions as Callbacks

Function expressions are commonly used as callbacks:

```javascript
// students/05_callbackPatterns.js

// Event handler as function expression
const handleClick = function(event) {
    console.log('Button clicked:', event.target.textContent);
};

// setTimeout with function expression
setTimeout(function() {
    console.log('This runs after 1 second');
}, 1000);

// Custom forEach with callback
function forEach(arr, callback) {
    for (let i = 0; i < arr.length; i++) {
        callback(arr[i], i, arr);
    }
}

forEach(['a', 'b', 'c'], function(item, index) {
    console.log(`${index}: ${item}`);
});

// Promise-based async function expression
const fetchData = function(url) {
    return fetch(url)
        .then(function(response) {
            return response.json();
        })
        .then(function(data) {
            console.log(data);
        })
        .catch(function(error) {
            console.error('Error:', error);
        });
};
```

---

## Hoisting Behavior

### Critical Difference

The key difference between function declarations and expressions lies in hoisting behavior:

```javascript
// students/06_hoisting.js

// ✅ Function declarations are fully hoisted
console.log(add(2, 3));  // 5 - Works!

function add(a, b) {
    return a + b;
}

// ❌ Function expressions are NOT fully hoisted
// TypeError: greet is not a function
// console.log(greet('World'));  // Would fail!

const greet = function(name) {
    return `Hello, ${name}!`;
};

console.log(greet('World'));  // Works when called after definition
```

### Hoisting Demonstration

```javascript
// students/07_hoistingComparison.js

// Variable hoisting with function declaration
console.log(typeof sayHello);  // 'function'

// Function declaration is fully hoisted
function sayHello() {
    return 'Hello!';
}

// Variable declaration is hoisted, but assignment is not
console.log(typeof sayGoodbye);  // 'undefined'
// sayGoodbye();  // TypeError if called

var sayGoodbye = function() {
    return 'Goodbye!';
};

console.log(typeof sayGoodbye);  // 'function' after assignment
```

### Practical Hoisting Patterns

```javascript
// students/08_hoistingPatterns.js

// Pattern: Conditional function definition
const createHandler = function(mode) {
    if (mode === 'production') {
        return function handleProduction(event) {
            console.log('Production handler:', event);
        };
    } else {
        return function handleDevelopment(event) {
            console.log('Dev handler:', event, '(verbose logging)');
        };
    }
};

const handler = createHandler('development');
handler({ type: 'click' });

// Pattern: Feature detection with function expressions
const detectGeolocation = function() {
    if ('geolocation' in navigator) {
        return function getPosition() {
            return new Promise((resolve, reject) => {
                navigator.geolocation.getCurrentPosition(resolve, reject);
            });
        };
    } else {
        return function getPosition() {
            return Promise.reject(new Error('Geolocation not supported'));
        };
    }
};

const geo = detectGeolocation();
console.log(typeof geo.getPosition);  // 'function'
```

---

## IIFE (Immediately Invoked Function Expressions)

### Basic IIFE Syntax

An IIFE is a function that executes immediately upon definition:

```javascript
// students/09_iife.js

// Basic IIFE
(function() {
    console.log('IIFE executed immediately!');
})();

// With return value
const result = (function() {
    const privateVar = 'secret';
    return privateVar;
})();

console.log(result);  // 'secret'

// Arrow function IIFE
((message) => {
    console.log(message);
})('Arrow IIFE executed!');
```

### IIFE for Private Scope

```javascript
// students/10_iifePrivacy.js

// Creating private scope
const counter = (function() {
    let count = 0;
    
    return {
        increment: function() {
            return ++count;
        },
        decrement: function() {
            return --count;
        },
        getValue: function() {
            return count;
        }
    };
})();

console.log(counter.increment());  // 1
console.log(counter.increment());  // 2
console.log(counter.getValue());   // 2
console.log(count);  // ReferenceError: count is not defined
```

### Module Pattern with IIFE

```javascript
// students/11_modulePattern.js

// Modern module pattern
const UserManager = (function() {
    const users = new Map();
    
    function validateEmail(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }
    
    return {
        addUser: function(id, email, name) {
            if (!validateEmail(email)) {
                throw new Error('Invalid email format');
            }
            users.set(id, { id, email, name, createdAt: new Date() });
            return true;
        },
        
        getUser: function(id) {
            return users.get(id);
        },
        
        removeUser: function(id) {
            return users.delete(id);
        },
        
        getAllUsers: function() {
            return Array.from(users.values());
        },
        
        getCount: function() {
            return users.size;
        }
    };
})();

// Usage
UserManager.addUser(1, 'alice@example.com', 'Alice');
UserManager.addUser(2, 'bob@example.com', 'Bob');
console.log(UserManager.getAllUsers());
// [{ id: 1, email: 'alice@example.com', name: 'Alice', ... }, ...]
console.log(typeof users);  // undefined - private
```

### IIFE with Parameters

```javascript
// students/12_iifeWithParams.js

// IIFE with parameters
const config = (function(dbConfig, appEnv) {
    return {
        host: dbConfig.host,
        port: dbConfig.port,
        env: appEnv,
        isProduction: appEnv === 'production',
        connectionString: `postgres://${dbConfig.user}:${dbConfig.pass}@${dbConfig.host}:${dbConfig.port}/${dbConfig.name}`
    };
})(
    { host: 'localhost', port: 5432, user: 'admin', pass: 'secret', name: 'mydb' },
    'development'
);

console.log(config);
// { host: 'localhost', port: 5432, env: 'development', ... }
```

---

## Function Types Comparison

### Comparison Table

| Aspect | Function Declaration | Function Expression |
|--------|-------------------|-------------------|
| Syntax | `function name() {}` | `const name = function() {}` |
| Hoisting | Fully hoisted | Only variable hoisted |
| Name | Has intrinsic name | Optional (named expression) |
| Use in recursion | Easy | Requires named expression |
| As callbacks | Indirect | Direct |
| Timing | Can call before definition | Must call after definition |

### When to Use Each

```javascript
// students/13_whenToUse.js

// Use FUNCTION DECLARATIONS for:
// - Standalone utility functions
// - Functions that need to be called before definition (due to hoisting)
// - Recursive functions
// - Public API functions

function findMax(arr) {
    return Math.max(...arr);
}

function deepEqual(a, b) {
    if (a === b) return true;
    if (a == null || b == null) return false;
    if (typeof a !== typeof b) return false;
    
    if (typeof a === 'object') {
        const keysA = Object.keys(a);
        const keysB = Object.keys(b);
        if (keysA.length !== keysB.length) return false;
        
        for (const key of keysA) {
            if (!deepEqual(a[key], b[key])) return false;
        }
        return true;
    }
    
    return false;
}

// Use FUNCTION EXPRESSIONS for:
// - Callbacks and event handlers
// - Conditional function creation
// - Module patterns
// - Private helper functions

const validateInput = function(value, rules) {
    for (const [pattern, message] of Object.entries(rules)) {
        if (!new RegExp(pattern).test(value)) {
            return { valid: false, error: message };
        }
    }
    return { valid: true };
};

const handleFormSubmit = function(event) {
    event.preventDefault();
    const formData = new FormData(event.target);
    console.log(Object.fromEntries(formData));
};

const debounce = function(fn, delay) {
    let timeoutId;
    return function(...args) {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => fn.apply(this, args), delay);
    };
};
```

---

## Professional Use Cases

### 1. Event-Driven Architecture

```javascript
// students/14_eventSystem.js

// Event emitter using function expressions
const EventEmitter = function() {
    const events = {};
    
    return {
        on: function(event, callback) {
            if (!events[event]) events[event] = [];
            events[event].push(callback);
            return () => this.off(event, callback);
        },
        
        off: function(event, callback) {
            if (!events[event]) return;
            events[event] = events[event].filter(cb => cb !== callback);
        },
        
        emit: function(event, data) {
            if (!events[event]) return;
            events[event].forEach(cb => cb(data));
        }
    };
};

const emitter = EventEmitter();

const unsub1 = emitter.on('user:login', function(user) {
    console.log('User logged in:', user.name);
});

const unsub2 = emitter.on('user:login', function(user) {
    console.log('Analytics:Login', user.id);
});

emitter.emit('user:login', { id: 1, name: 'Alice' });
// User logged in: Alice
// Analytics:Login 1
```

### 2. Factory Pattern

```javascript
// students/15_factoryPattern.js

// Product factory using IIFE
const ProductFactory = (function() {
    const products = [];
    let nextId = 1;
    
    function validateProduct(product) {
        if (!product.name || product.name.trim() === '') {
            throw new Error('Product name is required');
        }
        if (typeof product.price !== 'number' || product.price < 0) {
            throw new Error('Invalid price');
        }
        return true;
    }
    
    return {
        create: function(name, price, category) {
            const product = {
                id: nextId++,
                name,
                price,
                category,
                createdAt: new Date()
            };
            
            validateProduct(product);
            products.push(product);
            return product;
        },
        
        findById: function(id) {
            return products.find(p => p.id === id);
        },
        
        findByCategory: function(category) {
            return products.filter(p => p.category === category);
        },
        
        update: function(id, updates) {
            const index = products.findIndex(p => p.id === id);
            if (index === -1) throw new Error('Product not found');
            
            const updated = { ...products[index], ...updates };
            validateProduct(updated);
            products[index] = updated;
            return updated;
        },
        
        delete: function(id) {
            const index = products.findIndex(p => p.id === id);
            if (index === -1) return false;
            products.splice(index, 1);
            return true;
        }
    };
})();

// Usage
const p1 = ProductFactory.create('Laptop', 999, 'electronics');
const p2 = ProductFactory.create('Book', 19.99, 'books');
console.log(ProductFactory.findByCategory('electronics'));
```

### 3. Currying and Function Composition

```javascript
// students/16_currying.js

// Curried function expressions
const curriedMap = function(fn) {
    return function(arr) {
        return arr.map(fn);
    };
};

const curriedFilter = function(predicate) {
    return function(arr) {
        return arr.filter(predicate);
    };
};

const curriedReduce = function(reducer, initial) {
    return function(arr) {
        return arr.reduce(reducer, initial);
    };
};

// Composed utility
const processNumbers = function(arr) {
    const mapDouble = curriedMap(n => n * 2);
    const filterEvens = curriedFilter(n => n % 2 === 0);
    const sum = curriedReduce((a, b) => a + b, 0);
    
    return sum(filterEvens(mapDouble(arr)));
};

console.log(processNumbers([1, 2, 3, 4, 5, 6]));  // 24
```

---

## Common Pitfalls

### 1. Hoisting Confusion

```javascript
// students/17_pitfallHoisting.js

// ❌ WRONG: Calling function expression before definition
try {
    console.log(multiply(2, 3));  // TypeError
} catch (e) {
    console.log('Error:', e.message);
}

const multiply = function(a, b) {
    return a * b;
};

// ✅ CORRECT: Using function declaration or calling after definition
console.log(divide(10, 2));  // 5

function divide(a, b) {
    return a / b;
}
```

### 2. Variable Shadowing

```javascript
// students/18_pitfallShadowing.js

// ❌ WRONG: Accidental variable shadowing
function createCounter() {
    var count = 0;
    
    return {
        increment: function() {
            var count = 100;  // Shadows outer count
            return ++count;  // Returns 101, not 1!
        }
    };
}

// ✅ CORRECT: No shadowing
function createCounterFixed() {
    var count = 0;
    
    return {
        increment: function() {
            return ++count;
        }
    };
}
```

### 3. Forgetting Return Value

```javascript
// students/19_pitfallReturn.js

// ❌ WRONG: Forgetting return statement
const calculateTotal = function(items) {
    items.reduce((sum, item) => sum + item.price, 0);
    // No return! Returns undefined
};

// ✅ CORRECT: Return the calculated value
const calculateTotalFixed = function(items) {
    return items.reduce((sum, item) => sum + item.price, 0);
};
```

---

## Key Takeaways

1. **Function Declarations** are hoisted entirely and can be called before definition. Use for utility functions and recursive algorithms.

2. **Function Expressions** (assigned to variables) are NOT fully hoisted - only the variable declaration is hoisted. Use for callbacks, event handlers, and conditional definitions.

3. **IIFE** creates immediate execution and private scope. Essential for the module pattern and avoiding global namespace pollution.

4. **Named Function Expressions** allow internal recursion while maintaining expression semantics. Useful for debugging (shows name in stack traces).

5. **Choose the Right Pattern**: Declarations for public APIs, expressions for callbacks/handlers, IIFE for modules and private state.

---

## Related Files

- [02_FUNCTION_PARAMETERS_AND_ARGUMENTS.md](./02_FUNCTION_PARAMETERS_AND_ARGUMENTS.md) - Advanced parameter handling
- [04_ARROW_FUNCTIONS_MASTER.md](./04_ARROW_FUNCTIONS_MASTER.md) - Arrow function syntax and this binding
- [03_SCOPE_CHAIN_AND_CLOSURES.md](./03_SCOPE_CHAIN_AND_CLOSURES.md) - Scope and closure patterns