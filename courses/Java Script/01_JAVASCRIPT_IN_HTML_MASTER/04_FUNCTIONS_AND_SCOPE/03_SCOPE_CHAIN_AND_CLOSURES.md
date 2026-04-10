# 📝 Scope Chain and Closures

## 📋 Table of Contents

1. [Overview](#overview)
2. [Understanding Scope](#understanding-scope)
3. [Scope Chain](#scope-chain)
4. [Closures](#closures)
5. [Closure Patterns](#closure-patterns)
6. [Memory Management](#memory-management)
7. [Practical Use Cases](#practical-use-cases)
8. [Common Pitfalls](#common-pitfalls)
9. [Key Takeaways](#key-takeaways)

---

## Overview

Closures are one of the most powerful and often misunderstood concepts in JavaScript. A closure is created when a function "remembers" its lexical scope even when executed outside that scope. Understanding closures is essential for mastering JavaScript, as they form the foundation for many patterns including data privacy, function factories, callbacks, and partial application.

This comprehensive guide explores the scope chain, closure creation, practical patterns, memory implications, and professional use cases. By the end, you'll have a deep understanding of how functions interact with their surrounding scope and how to leverage closures effectively.

---

## Understanding Scope

### What is Scope?

Scope determines where variables and functions are accessible in your code. JavaScript has three types of scope:

```javascript
// students/01_scopeTypes.js

// GLOBAL SCOPE
// Variables declared outside any function
const globalVar = 'I am global';

function globalAccess() {
    console.log(globalVar);  // ✅ Accessible
}
globalAccess();
console.log(globalVar);  // ✅ Accessible

// FUNCTION SCOPE (Local)
function myFunction() {
    const functionVar = 'I am local to function';
    console.log(functionVar);  // ✅ Accessible inside
    
    function inner() {
        console.log(functionVar);  // ✅ Accessible in nested function
    }
    inner();
}
// console.log(functionVar);  // ❌ ReferenceError

// BLOCK SCOPE (let/const)
if (true) {
    const blockVar = 'I am block scoped';
    let anotherBlock = 'also block scoped';
    console.log(blockVar);  // ✅ Accessible inside block
}
// console.log(blockVar);  // ❌ ReferenceError
```

### Global vs Function Scope

```javascript
// students/02_globalVsFunction.js

// Global pollution problem
let counter = 0;  // Global - can be modified anywhere

function increment() {
    counter++;
    return counter;
}

function decrement() {
    counter--;
    return counter;
}

console.log(increment());  // 1
console.log(decrement());  // 0
// Any code can modify counter - dangerous!

// Solution: IIFE for private scope
const counterModule = (function() {
    let counter = 0;  // Private - only accessible within IIFE
    
    return {
        increment: function() {
            return ++counter;
        },
        decrement: function() {
            return --counter;
        },
        getValue: function() {
            return counter;
        }
    };
})();

console.log(counterModule.increment());  // 1
console.log(counterModule.increment());  // 2
console.log(counterModule.getValue());   // 2
// console.log(counter);  // ❌ ReferenceError - private!
```

### Lexical vs Dynamic Scope

JavaScript uses lexical (static) scope - scope is determined by where functions are defined:

```javascript
// students/03_lexicalScope.js

// Lexical scope: determined by where function is written
const x = 10;

function outer() {
    const x = 20;
    
    function inner() {
        console.log(x);  // 20 - uses outer function's x
    }
    
    inner();
}

outer();  // 20
```

---

## Scope Chain

### How the Scope Chain Works

Every execution context has a reference to its outer (parent) scope, forming a chain:

```javascript
// students/04_scopeChain.js

// Scope chain visualization
const globalVar = 'global';

function outer() {
    const outerVar = 'outer';
    
    function middle() {
        const middleVar = 'middle';
        
        function inner() {
            const innerVar = 'inner';
            
            // Accesses variables from entire scope chain:
            console.log(innerVar);   // 'inner' - local
            console.log(middleVar);  // 'middle' - from middle()
            console.log(outerVar);   // 'outer' - from outer()
            console.log(globalVar);  // 'global' - from global
        }
        
        inner();
    }
    
    middle();
}

outer();

// Scope chain: inner -> middle -> outer -> global
```

### Chain Lookup

Variables are resolved by walking up the scope chain until found:

```javascript
// students/05_chainLookup.js

// Variable lookup
const a = 1;

function first() {
    const b = 2;
    
    function second() {
        const c = 3;
        
        function third() {
            // Looks for 'a' up the chain:
            // 1. local scope - not found
            // 2. second() - not found
            // 3. first() - not found
            // 4. global - FOUND: 1
            console.log(a, b, c);
        }
        
        third();
    }
    
    second();
}

first();  // 1 2 3
```

### Block Scope and the Scope Chain

`let` and `const` create block scope, affecting the scope chain:

```javascript
// students/06_blockScope.js

// Block scope behavior
const global = 'global';

{
    const block = 'block';
    console.log(global);  // ✓ Found in outer
    console.log(block);    // ✓ Found in block
}

{
    // Different block, different scope
    const block = 'different block';
    console.log(global);  // ✓
    console.log(block);   // ✓ - different variable
}

// console.log(block);  // ❌ Not accessible

// Loop with block scope
for (let i = 0; i < 3; i++) {
    setTimeout(() => console.log(i), 0);
}
// Output: 0, 1, 2 (each iteration gets new i)

// Contrast with var (function scope)
for (var j = 0; j < 3; j++) {
    setTimeout(() => console.log(j), 0);
}
// Output: 3, 3, 3 (all share same j)
```

---

## Closures

### What is a Closure?

A closure is a function that has access to variables from its outer (enclosing) scope, even after the outer function has returned:

```javascript
// students/07_closureBasics.js

// Basic closure
function createGreeter() {
    const greeting = 'Hello';  // Variable in outer scope
    
    return function(name) {    // This function "closes over" greeting
        return `${greeting}, ${name}!`;
    };
}

const greeter = createGreeter();  // Returns the inner function
console.log(greeter('Alice'));    // 'Hello, Alice!'
console.log(greeter('Bob'));      // 'Hello, Bob!'

// The inner function "remembers" greeting even after createGreeter ended
```

### Closure in Action

```javascript
// students/08_closureInAction.js

// Closure preserves the outer variable's state
function createCounter() {
    let count = 0;  // Initial state
    
    return {
        increment: function() {
            return ++count;
        },
        decrement: function() {
            return --count;
        },
        getCount: function() {
            return count;
        }
    };
}

const counter1 = createCounter();
const counter2 = createCounter();

console.log(counter1.increment());  // 1
console.log(counter1.increment());  // 2
console.log(counter2.increment());  // 1 - independent instance!

// Each call to createCounter() creates a new closure
// with its own independent count variable
```

### Multiple Closures

```javascript
// students/09_multipleClosures.js

// Multiple functions sharing same closure
function createMathOperations(base) {
    return {
        add: function(n) {
            return base + n;
        },
        subtract: function(n) {
            return base - n;
        },
        multiply: function(n) {
            return base * n;
        },
        divide: function(n) {
            if (n === 0) throw new Error('Cannot divide by zero');
            return base / n;
        },
        getBase: function() {
            return base;
        }
    };
}

const ops = createMathOperations(10);
console.log(ops.add(5));        // 15
console.log(ops.multiply(3));   // 30
console.log(ops.divide(2));     // 5

// Another instance with different base
const ops2 = createMathOperations(100);
console.log(ops2.add(50));     // 150
```

---

## Closure Patterns

### 1. Data Privacy (Module Pattern)

```javascript
// students/10_modulePattern.js

// Private variables with closures
const BankAccount = (function() {
    let balance = 0;
    let transactions = [];
    
    function logTransaction(type, amount) {
        transactions.push({
            type,
            amount,
            timestamp: new Date(),
            balance: balance
        });
    }
    
    return {
        deposit: function(amount) {
            if (amount <= 0) {
                throw new Error('Deposit amount must be positive');
            }
            balance += amount;
            logTransaction('deposit', amount);
            return balance;
        },
        
        withdraw: function(amount) {
            if (amount <= 0) {
                throw new Error('Withdrawal amount must be positive');
            }
            if (amount > balance) {
                throw new Error('Insufficient funds');
            }
            balance -= amount;
            logTransaction('withdrawal', amount);
            return balance;
        },
        
        getBalance: function() {
            return balance;
        },
        
        getTransactions: function() {
            return [...transactions];  // Return copy for privacy
        }
    };
})();

BankAccount.deposit(1000);
BankAccount.withdraw(250);
console.log(BankAccount.getBalance());  // 750
console.log(BankAccount.getTransactions());
// [{ type: 'deposit', amount: 1000, ... }, { type: 'withdrawal', amount: 250, ... }]
// balance is not directly accessible!
```

### 2. Function Factory

```javascript
// students/11_functionFactory.js

// Creating specialized functions
function createMultiplier(factor) {
    return function(number) {
        return number * factor;
    };
}

const double = createMultiplier(2);
const triple = createMultiplier(3);
const timesTen = createMultiplier(10);

console.log(double(5));     // 10
console.log(triple(5));     // 15
console.log(timesTen(5));   // 50

// Practical: Custom formatters
function createFormatter(prefix, suffix) {
    return function(text) {
        return `${prefix}${text}${suffix}`;
    };
}

const bold = createFormatter('<b>', '</b>');
const italic = createFormatter('<i>', '</i>');
const quote = createFormatter('"', '"');

console.log(bold('Hello'));       // <b>Hello</b>
console.log(italic('World'));     // <i>World</i>

// Factory for validation
function createValidator(rules) {
    return function(value) {
        const errors = [];
        
        for (const [rule, test, message] of rules) {
            if (!test(value)) {
                errors.push(message);
            }
        }
        
        return {
            valid: errors.length === 0,
            errors
        };
    };
}

const emailValidator = createValidator([
    ['required', v => v && v.length > 0, 'Email is required'],
    ['format', v => v.includes('@'), 'Invalid email format'],
    ['length', v => v.length <= 255, 'Email too long']
]);

console.log(emailValidator('test@example.com'));
// { valid: true, errors: [] }
console.log(emailValidator('invalid'));
// { valid: false, errors: ['Invalid email format'] }
```

### 3. Event Handlers with State

```javascript
// students/12_eventHandlers.js

// Event handler with private state
function createClickTracker(elementId) {
    let clickCount = 0;
    let lastClickTime = null;
    
    const element = document.getElementById(elementId);
    
    function handleClick(event) {
        clickCount++;
        const now = Date.now();
        
        if (lastClickTime && (now - lastClickTime) < 300) {
            console.log('Double click detected!');
        }
        
        lastClickTime = now;
        console.log(`Click ${clickCount}: ${now}`);
    }
    
    if (element) {
        element.addEventListener('click', handleClick);
    }
    
    return {
        getClickCount: () => clickCount,
        reset: () => {
            clickCount = 0;
            lastClickTime = null;
        }
    };
}

// Image lazy loader with closure
function createLazyLoader() {
    const loadedImages = new Set();
    
    return function(imageElement) {
        if (loadedImages.has(imageElement.src)) return;
        
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src;
                    loadedImages.add(img.src);
                    observer.unobserve(img);
                }
            });
        });
        
        observer.observe(imageElement);
    };
}
```

### 4. Currying

```javascript
// students/13_currying.js

// Curried functions using closures
function curriedAdd(a) {
    return function(b) {
        return a + b;
    };
}

const add5 = curriedAdd(5);
console.log(add5(10));  // 15
console.log(add5(20));  // 25

// Multi-parameter currying
function curry(fn) {
    return function(...args) {
        if (args.length >= fn.length) {
            return fn.apply(this, args);
        }
        return function(...moreArgs) {
            return fn.apply(this, [...args, ...moreArgs]);
        };
    };
}

function add(a, b, c) {
    return a + b + c;
}

const curriedAdd2 = curry(add);
console.log(curriedAdd2(1)(2)(3));  // 6
console.log(curriedAdd2(1, 2)(3)); // 6
console.log(curriedAdd2(1)(2, 3));  // 6
```

### 5. Partial Application

```javascript
// students/14_partialApplication.js

// Partial application with closures
function partial(fn, ...presetArgs) {
    return function(...laterArgs) {
        return fn(...presetArgs, ...laterArgs);
    };
}

function formatCurrency(locale, currency, value) {
    return new Intl.NumberFormat(locale, {
        style: 'currency',
        currency
    }).format(value);
}

const formatUSD = partial(formatCurrency, 'en-US', 'USD');
const formatEUR = partial(formatCurrency, 'de-DE', 'EUR');

console.log(formatUSD(1000));  // $1,000.00
console.log(formatEUR(1000));  // 1.000,00 €

// Async partial application
function partialAsync(fn, ...presetArgs) {
    return async function(...laterArgs) {
        return await fn(...presetArgs, ...laterArgs);
    };
}

async function fetchData(baseUrl, endpoint, options) {
    return { data: `${baseUrl}${endpoint}`, options };
}

const fetchUsers = partialAsync(fetchData, 'https://api.example.com', '/users');
const fetchWithAuth = partial(fetchUsers, { auth: 'token123' });
```

---

## Memory Management

### Closure Memory Considerations

Closures keep their outer scope in memory, which can cause memory leaks if not managed properly:

```javascript
// students/15_memoryConsiderations.js

// ❌ Memory leak: retaining unnecessary references
function createLeakyHandler() {
    const largeData = new Array(1000000).fill('data');
    const element = document.getElementById('leaky');
    
    element.addEventListener('click', function() {
        // This closure keeps largeData in memory!
        console.log(largeData[0]);
    });
}

// ✅ Better: Only capture what you need
function createOptimizedHandler() {
    const element = document.getElementById('optimized');
    
    // Extract only necessary data before creating handler
    const neededValue = 'important data';
    
    element.addEventListener('click', function() {
        // Only captures neededValue
        console.log(neededValue);
    });
}

// Memory cleanup pattern
function createManagedComponent() {
    const state = { count: 0 };
    let cleanup = null;
    
    function setup() {
        // Setup logic
        cleanup = () => {
            // Cleanup logic
            state.count = 0;
        };
    }
    
    function destroy() {
        if (cleanup) {
            cleanup();
            cleanup = null;
        }
    }
    
    return { setup, destroy };
}
```

### WeakMap for Private Data

```javascript
// students/16_weakMap.js

// Using WeakMap for memory-efficient private data
const privateData = new WeakMap();

class User {
    constructor(name, email) {
        this.name = name;
        this.email = email;
        
        // Private data in WeakMap
        privateData.set(this, {
            lastLogin: null,
            loginAttempts: 0,
            preferences: {}
        });
    }
    
    login() {
        const data = privateData.get(this);
        data.lastLogin = new Date();
        data.loginAttempts++;
    }
    
    getLastLogin() {
        return privateData.get(this).lastLogin;
    }
    
    getLoginAttempts() {
        return privateData.get(this).loginAttempts;
    }
}

const user = new User('Alice', 'alice@example.com');
user.login();
console.log(user.getLastLogin());
console.log(user.getLoginAttempts());
// user reference can be garbage collected, WeakMap auto-cleans
```

---

## Practical Use Cases

### 1. Caching/Memoization

```javascript
// students/17_memoization.js

// Memoization with closures
function memoize(fn) {
    const cache = new Map();
    
    return function(...args) {
        const key = JSON.stringify(args);
        
        if (cache.has(key)) {
            return cache.get(key);
        }
        
        const result = fn.apply(this, args);
        cache.set(key, result);
        return result;
    };
}

function fibonacci(n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

const memoizedFib = memoize(fibonacci);
console.log(memoizedFib(40));  // Fast - cached
console.log(memoizedFib(40));  // Even faster - from cache
```

### 2. Throttling and Debouncing

```javascript
// students/18_throttleDebounce.js

// Debounce: Wait until calls stop
function debounce(fn, delay) {
    let timeoutId;
    
    return function(...args) {
        clearTimeout(timeoutId);
        
        timeoutId = setTimeout(() => {
            fn.apply(this, args);
        }, delay);
    };
}

// Throttle: Limit call frequency
function throttle(fn, limit) {
    let inThrottle = false;
    
    return function(...args) {
        if (!inThrottle) {
            fn.apply(this, args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    };
}

// Usage
const handleScroll = debounce(() => {
    console.log('Scroll stopped');
}, 250);

const handleResize = throttle(() => {
    console.log('Resized');
}, 100);
```

### 3. Async Patterns

```javascript
// students/19_asyncClosures.js

// Promise-based retry with closure
function createRetryHandler(maxRetries = 3, delay = 1000) {
    return async function(fn) {
        let lastError;
        
        for (let attempt = 1; attempt <= maxRetries; attempt++) {
            try {
                return await fn();
            } catch (error) {
                lastError = error;
                if (attempt < maxRetries) {
                    await new Promise(r => setTimeout(r, delay * attempt));
                }
            }
        }
        
        throw lastError;
    };
}

const retry = createRetryHandler(3, 500);
retry(async () => {
    // Operation that might fail
    return await fetch('/api/data');
});
```

---

## Common Pitfalls

### 1. Loop Variable Capture

```javascript
// students/20_pitfallLoop.js

// ❌ WRONG: Loop variable captured by all closures
for (var i = 0; i < 3; i++) {
    setTimeout(() => console.log(i), 0);
}
// Output: 3, 3, 3 (all reference same i)

// ✅ CORRECT: Use let (block scope)
for (let i = 0; i < 3; i++) {
    setTimeout(() => console.log(i), 0);
}
// Output: 0, 1, 2 (each iteration has own i)

// Alternative: IIFE to capture value
for (var j = 0; j < 3; j++) {
    (function(index) {
        setTimeout(() => console.log(index), 0);
    })(j);
}
// Output: 0, 1, 2
```

### 2. Accidental Shared State

```javascript
// students/21_pitfallShared.js

// ❌ WRONG: Using var in loop creates shared closure
function createFunctions() {
    const functions = [];
    
    for (var i = 0; i < 3; i++) {
        functions.push(function() {
            return i;
        });
    }
    
    return functions;
}

const fns = createFunctions();
console.log(fns[0]());  // 3 - wrong!
console.log(fns[1]());  // 3 - wrong!
console.log(fns[2]());  // 3 - wrong!

// ✅ CORRECT: Use let or capture in closure
function createFunctionsFixed() {
    const functions = [];
    
    for (let i = 0; i < 3; i++) {
        functions.push(function() {
            return i;
        });
    }
    
    return functions;
}

const fnsFixed = createFunctionsFixed();
console.log(fnsFixed[0]());  // 0 - correct!
console.log(fnsFixed[1]());  // 1 - correct!
console.log(fnsFixed[2]());  // 2 - correct!
```

### 3. Forgetting to Return Function

```javascript
// students/22_pitfallReturn.js

// ❌ WRONG: Forgetting return in function factory
function createMultiplier(factor) {
    function multiply(n) {  // Missing return!
        return n * factor;
    }
}

const double = createMultiplier(2);
console.log(typeof double);  // 'function' but not multiply!
console.log(double(5));     // undefined

// ✅ CORRECT: Return the inner function
function createMultiplierFixed(factor) {
    return function multiply(n) {
        return n * factor;
    };
}

const doubleFixed = createMultiplierFixed(2);
console.log(doubleFixed(5));  // 10
```

---

## Key Takeaways

1. **Scope Chain**: JavaScript resolves variables by walking up from local to global scope.

2. **Closures**: Functions retain access to their outer scope even after the outer function returns.

3. **Module Pattern**: Use IIFE + closure to create private state and public APIs.

4. **Memory**: Closures keep their scope in memory - be mindful of what you capture.

5. **Loop Issues**: Use `let` instead of `var` for block-scoped loop variables in closures.

6. **Function Factories**: Create specialized functions by closing over different parameters.

7. **Practical Applications**: Memoization, debouncing, throttling, currying, and partial application all rely on closures.

---

## Related Files

- [01_FUNCTION_DECLARATIONS_EXPRESSIONS.md](./01_FUNCTION_DECLARATIONS_EXPRESSIONS.md) - IIFE and function types
- [04_ARROW_FUNCTIONS_MASTER.md](./04_ARROW_FUNCTIONS_MASTER.md) - Arrow function closure behavior
- [05_FUNCTION_CONTEXT_THIS.md](./05_FUNCTION_CONTEXT_THIS.md) - How closures relate to `this`
- [07_FUNCTION_MEMORIZATION_AND_OPTIMIZATION.md](./07_FUNCTION_MEMORIZATION_AND_OPTIMIZATION.md) - Memoization patterns