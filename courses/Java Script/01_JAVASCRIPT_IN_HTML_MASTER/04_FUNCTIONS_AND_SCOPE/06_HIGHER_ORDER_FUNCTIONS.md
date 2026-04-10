# 📝 Higher Order Functions

## 📋 Table of Contents

1. [Overview](#overview)
2. [Understanding Higher Order Functions](#understanding-higher-order-functions)
3. [Map Method](#map-method)
4. [Filter Method](#filter-method)
5. [Reduce Method](#reduce-method)
6. [Function Composition](#function-composition)
7. [Callbacks](#callbacks)
8. [Professional Use Cases](#professional-use-cases)
9. [Common Pitfalls](#common-pitfalls)
10. [Key Takeaways](#key-takeaways)

---

## Overview

Higher order functions are functions that take other functions as arguments or return functions as results. They are a fundamental concept in functional programming and are extensively used in modern JavaScript for array manipulation, event handling, asynchronous operations, and code organization. The array methods `map`, `filter`, and `reduce` are the most commonly used higher order functions.

Understanding higher order functions enables you to write more declarative, concise, and maintainable code. Instead of imperatively describing each step, you describe the transformations you want to apply. This guide covers essential higher order functions, function composition patterns, and production-ready examples.

---

## Understanding Higher Order Functions

### What Makes a Function "Higher Order"?

```javascript
// students/01_higherOrderBasics.js

// Higher order function: takes function as parameter
function mapArray(arr, transformFn) {
    const result = [];
    for (const item of arr) {
        result.push(transformFn(item));
    }
    return result;
}

const numbers = [1, 2, 3, 4, 5];
const doubled = mapArray(numbers, n => n * 2);
console.log(doubled);  // [2, 4, 6, 8, 10]

// Higher order function: returns a function
function createMultiplier(factor) {
    return function(number) {
        return number * factor;
    };
}

const double = createMultiplier(2);
const triple = createMultiplier(3);

console.log(double(5));   // 10
console.log(triple(5));   // 15

// Combined: takes function, returns function
function createTransformer(transformFn, options = {}) {
    const { verbose = false } = options;
    
    return function(input) {
        const result = transformFn(input);
        if (verbose) {
            console.log(`Transformed ${input} to ${result}`);
        }
        return result;
    };
}

const loggingDouble = createTransformer(n => n * 2, { verbose: true });
loggingDouble(5);  // Logs and returns 10
```

### Functions as First-Class Citizens

In JavaScript, functions are values that can be assigned, passed, and returned:

```javascript
// students/02_firstClass.js

// Assign function to variable
const add = function(a, b) {
    return a + b;
};

// Store in array
const operations = [
    (x) => x + 1,
    (x) => x * 2,
    (x) => x - 3
];

// Pass as argument
function applyOperations(value, ops) {
    return ops.reduce((acc, op) => op(acc), value);
}

console.log(applyOperations(5, operations));  // 9: ((5+1)*2)-3

// Return from function
function createLogger(prefix) {
    return function(message) {
        console.log(`[${prefix}] ${message}`);
    };
}

const infoLogger = createLogger('INFO');
const errorLogger = createLogger('ERROR');

infoLogger('Application started');
errorLogger('Connection failed');
```

---

## Map Method

### Transforming Arrays

The `map` method creates a new array by applying a function to each element:

```javascript
// students/03_mapBasics.js

const numbers = [1, 2, 3, 4, 5];

// Basic transformation
const doubled = numbers.map(n => n * 2);
console.log(doubled);  // [2, 4, 6, 8, 10]

// Transform to objects
const users = [
    { firstName: 'John', lastName: 'Doe' },
    { firstName: 'Jane', lastName: 'Smith' }
];

const fullNames = users.map(user => `${user.firstName} ${user.lastName}`);
console.log(fullNames);  // ['John Doe', 'Jane Smith']

// Transform structure
const products = [
    { name: 'Laptop', price: 999 },
    { name: 'Mouse', price: 29 }
];

const withTax = products.map(p => ({
    ...p,
    priceWithTax: p.price * 1.1
}));

console.log(withTax);
// [{ name: 'Laptop', price: 999, priceWithTax: 1098.9 }, ...]
```

### Advanced Map Patterns

```javascript
// students/04_mapAdvanced.js

// Map with index
const letters = ['a', 'b', 'c'];
const indexed = letters.map((letter, index) => `${index}: ${letter}`);
console.log(indexed);  // ['0: a', '1: b', '2: c']

// FlatMap for nested transformations
const sentences = ['hello world', 'foo bar'];
const words = sentences.flatMap(sentence => sentence.split(' '));
console.log(words);  // ['hello', 'world', 'foo', 'bar']

// Chaining map with other methods
const data = [1, 2, 3, 4, 5];

const result = data
    .map(x => x * 2)
    .map(x => x + 1)
    .filter(x => x > 3);

console.log(result);  // [5, 7, 9, 11]

// Map with complex objects
const employees = [
    { name: 'Alice', department: 'Engineering', salary: 90000 },
    { name: 'Bob', department: 'Marketing', salary: 70000 },
    { name: 'Charlie', department: 'Engineering', salary: 95000 }
];

const summary = employees.map(emp => ({
    name: emp.name,
    department: emp.department,
    formattedSalary: new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
    }).format(emp.salary),
    tax: emp.salary * 0.3
}));

console.log(summary);
```

---

## Filter Method

### Selecting Elements

The `filter` method creates a new array with elements that pass a test:

```javascript
// students/05_filterBasics.js

const numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

// Basic filtering
const evens = numbers.filter(n => n % 2 === 0);
console.log(evens);  // [2, 4, 6, 8, 10]

// Filter objects
const products = [
    { name: 'Laptop', price: 999, inStock: true },
    { name: 'Mouse', price: 29, inStock: true },
    { name: 'Keyboard', price: 149, inStock: false }
];

const availableProducts = products.filter(p => p.inStock);
console.log(availableProducts);  // Laptop, Mouse

const affordableProducts = products.filter(p => p.price < 100);
console.log(affordableProducts);  // Mouse
```

### Advanced Filter Patterns

```javascript
// students/06_filterAdvanced.js

// Filter with multiple conditions
const users = [
    { name: 'Alice', age: 25, active: true },
    { name: 'Bob', age: 17, active: true },
    { name: 'Charlie', age: 30, active: false }
];

const activeAdults = users.filter(u => u.active && u.age >= 18);
console.log(activeAdults);  // Alice

// Filter unique values
const withDuplicates = [1, 2, 2, 3, 3, 3, 4];
const unique = withDuplicates.filter((value, index, arr) => arr.indexOf(value) === index);
console.log(unique);  // [1, 2, 3, 4]

// Or use Set
const uniqueViaSet = [...new Set(withDuplicates)];
console.log(uniqueViaSet);  // [1, 2, 3, 4]

// Filter by property existence
const data = [
    { id: 1, metadata: { active: true } },
    { id: 2, metadata: null },
    { id: 3, metadata: { active: false } }
];

const withMetadata = data.filter(d => d.metadata && d.metadata.active);
console.log(withMetadata);  // [{ id: 1, ... }]

// Complex object filtering
const orders = [
    { id: 1, items: ['a', 'b'], total: 100, status: 'completed' },
    { id: 2, items: ['c'], total: 50, status: 'pending' },
    { id: 3, items: ['d', 'e', 'f'], total: 200, status: 'completed' }
];

const validOrders = orders.filter(o => 
    o.status === 'completed' && 
    o.total >= 100 && 
    o.items.length >= 2
);

console.log(validOrders);  // [order 1, order 3]
```

---

## Reduce Method

### Accumulating Values

The `reduce` method reduces an array to a single value:

```javascript
// students/07_reduceBasics.js

const numbers = [1, 2, 3, 4, 5];

// Sum all numbers
const sum = numbers.reduce((acc, n) => acc + n, 0);
console.log(sum);  // 15

// Find maximum
const max = numbers.reduce((acc, n) => n > acc ? n : acc, numbers[0]);
console.log(max);  // 5

// Count occurrences
const fruits = ['apple', 'banana', 'apple', 'orange', 'banana', 'apple'];
const counts = fruits.reduce((acc, fruit) => {
    acc[fruit] = (acc[fruit] || 0) + 1;
    return acc;
}, {});

console.log(counts);  // { apple: 3, banana: 2, orange: 1 }

// Group by property
const users = [
    { name: 'Alice', department: 'Engineering' },
    { name: 'Bob', department: 'Marketing' },
    { name: 'Charlie', department: 'Engineering' }
];

const byDepartment = users.reduce((acc, user) => {
    if (!acc[user.department]) {
        acc[user.department] = [];
    }
    acc[user.department].push(user.name);
    return acc;
}, {});

console.log(byDepartment);
// { Engineering: ['Alice', 'Charlie'], Marketing: ['Bob'] }
```

### Advanced Reduce Patterns

```javascript
// students/08_reduceAdvanced.js

// Flatten array
const nested = [[1, 2], [3, 4], [5, 6]];
const flat = nested.reduce((acc, arr) => [...acc, ...arr], []);
console.log(flat);  // [1, 2, 3, 4, 5, 6]

// Or use flat()
console.log(nested.flat());  // [1, 2, 3, 4, 5, 6]

// Compose functions (right to left)
const compose = (...fns) => 
    (initial) => fns.reduceRight((acc, fn) => fn(acc), initial);

const add1 = x => x + 1;
const double = x => x * 2;
const square = x => x * x;

const result = compose(square, double, add1)(5);
console.log(result);  // square(double(add1(5))) = square(12) = 144

// Pipe functions (left to right)
const pipe = (...fns) => 
    (initial) => fns.reduce((acc, fn) => fn(acc), initial);

const pipedResult = pipe(add1, double, square)(5);
console.log(pipedResult);  // ((5+1)*2)^2 = 144

// Running totals
const transactions = [100, -50, 200, -30, 150];
const runningBalances = transactions.reduce((acc, amount, index) => {
    const balance = (acc[index - 1] || 0) + amount;
    return [...acc, balance];
}, []);

console.log(runningBalances);  // [100, 50, 250, 220, 370]
```

### Map and Filter with Reduce

```javascript
// students/09_reduce替代.js

// Implementing map with reduce
const mapWithReduce = (arr, fn) => 
    arr.reduce((acc, val, i) => [...acc, fn(val, i)], []);

console.log(mapWithReduce([1, 2, 3], n => n * 2));  // [2, 4, 6]

// Implementing filter with reduce
const filterWithReduce = (arr, predicate) => 
    arr.reduce((acc, val) => 
        predicate(val) ? [...acc, val] : acc, []);

console.log(filterWithReduce([1, 2, 3, 4], n => n > 2));  // [3, 4]

// Combined map and filter in one pass
const processData = (arr, filterFn, mapFn) => 
    arr.reduce((acc, val) => {
        if (filterFn(val)) {
            return [...acc, mapFn(val)];
        }
        return acc;
    }, []);

console.log(processData(
    [1, 2, 3, 4, 5],
    n => n > 2,
    n => n * 10
));  // [30, 40, 50]
```

---

## Function Composition

### Composing Functions

Function composition combines small, reusable functions into more complex operations:

```javascript
// students/10_composition.js

// Basic composition
const compose = (...fns) => 
    (value) => fns.reduceRight((acc, fn) => fn(acc), value);

const pipe = (...fns) => 
    (value) => fns.reduce((acc, fn) => fn(acc), value);

// Simple transformations
const trim = s => s.trim();
const upperCase = s => s.toUpperCase();
const addExclamation = s => `${s}!`;

// Compose: right to left
const format = compose(addExclamation, upperCase, trim);
console.log(format('  hello  '));  // "HELLO!"

// Pipe: left to right (more readable)
const formatPipe = pipe(trim, upperCase, addExclamation);
console.log(formatPipe('  hello  '));  // "HELLO!"

// Chaining with object
class FluentString {
    constructor(str) {
        this.str = str;
    }
    
    then(...fns) {
        return fns.reduce((acc, fn) => fn(acc), this.str);
    }
    
    trim() {
        return new FluentString(this.str.trim());
    }
    
    upperCase() {
        return new FluentString(this.str.toUpperCase());
    }
    
    addExclamation() {
        return new FluentString(this.str + '!');
    }
    
    value() {
        return this.str;
    }
}

console.log(
    new FluentString('  hello  ')
        .trim()
        .upperCase()
        .addExclamation()
        .value()
);  // "HELLO!"
```

### Composition Utilities

```javascript
// students/11_compositionUtils.js

// Partial application for composition
const partial = (fn, ...presetArgs) => 
    (...laterArgs) => fn(...presetArgs, ...laterArgs);

// Curried functions for composition
const prop = (key) => (obj) => obj[key];
const map = (fn) => (arr) => arr.map(fn);
const filter = (pred) => (arr) => arr.filter(pred);
const reduce = (fn, init) => (arr) => arr.reduce(fn, init);

// Compose with object methods
const getUsers = pipe(
    () => fetch('/api/users').then(r => r.json()),
    filter(u => u.active),
    map(u => ({ id: u.id, name: u.name }))
);

// Real-world: Data pipeline
const processUserData = pipe(
    // Step 1: Fetch and parse
    (raw) => JSON.parse(raw),
    
    // Step 2: Filter valid users
    filter(user => user.email && user.name),
    
    // Step 3: Normalize data
    map(user => ({
        ...user,
        name: user.name.trim(),
        email: user.email.toLowerCase()
    })),
    
    // Step 4: Group by domain
    reduce((acc, user) => {
        const domain = user.email.split('@')[1];
        if (!acc[domain]) acc[domain] = [];
        acc[domain].push(user);
        return acc;
    }, {})
);
```

---

## Callbacks

### Callback Patterns

Callbacks are functions passed as arguments to be executed later:

```javascript
// students/12_callbacks.js

// Synchronous callback
function forEach(arr, callback) {
    for (let i = 0; i < arr.length; i++) {
        callback(arr[i], i, arr);
    }
}

forEach(['a', 'b', 'c'], (item, index) => {
    console.log(`${index}: ${item}`);
});

// Asynchronous callback
function fetchData(url, callback) {
    fetch(url)
        .then(response => response.json())
        .then(data => callback(null, data))
        .catch(error => callback(error, null));
}

fetchData('https://api.example.com/data', (error, data) => {
    if (error) {
        console.error('Error:', error);
        return;
    }
    console.log('Data:', data);
});
```

### Callback Error Handling

```javascript
// students/13_callbackError.js

// Error-first callback pattern
function readFile(path, callback) {
    // Simulate async operation
    setTimeout(() => {
        if (path.includes('error')) {
            callback(new Error('File not found'), null);
            return;
        }
        callback(null, { content: 'File content', path });
    }, 100);
}

readFile('/data/file.txt', (error, data) => {
    if (error) {
        console.error('Failed to read:', error.message);
        return;
    }
    console.log('Content:', data.content);
});

readFile('/error/file.txt', (error, data) => {
    if (error) {
        console.error('Failed to read:', error.message);
        return;
    }
    console.log('Content:', data.content);
});

// Promise-based alternative (preferred)
function readFilePromise(path) {
    return new Promise((resolve, reject) => {
        if (path.includes('error')) {
            reject(new Error('File not found'));
            return;
        }
        resolve({ content: 'File content', path });
    });
}

readFilePromise('/data/file.txt')
    .then(data => console.log('Content:', data.content))
    .catch(error => console.error('Error:', error.message));
```

### Callback Management

```javascript
// students/14_callbackManagement.js

// Abort controller for cancellable operations
function fetchWithTimeout(url, timeout = 5000) {
    const controller = new AbortController();
    const id = setTimeout(() => controller.abort(), timeout);
    
    return fetch(url, { signal: controller.signal })
        .finally(() => clearTimeout(id));
}

// Event emitter pattern
class EventEmitter {
    constructor() {
        this.events = {};
    }
    
    on(event, callback) {
        if (!this.events[event]) {
            this.events[event] = [];
        }
        this.events[event].push(callback);
        return () => this.off(event, callback);
    }
    
    off(event, callback) {
        if (!this.events[event]) return;
        this.events[event] = this.events[event].filter(cb => cb !== callback);
    }
    
    emit(event, ...args) {
        if (!this.events[event]) return;
        this.events[event].forEach(cb => cb(...args));
    }
}

const emitter = new EventEmitter();
const unsub = emitter.on('data', (data) => console.log('Data:', data));

emitter.emit('data', { message: 'Hello' });
emitter.emit('data', { message: 'World' });
unsub();  // Unsubscribe
emitter.emit('data', { message: 'After unsubscribe' });  // Not logged
```

---

## Professional Use Cases

### 1. Data Pipeline

```javascript
// students/15_dataPipeline.js

// Comprehensive data processing pipeline
const processOrders = pipe(
    // Step 1: Parse raw data
    (raw) => JSON.parse(raw),
    
    // Step 2: Validate and filter
    filter(order => order.id && order.total > 0),
    
    // Step 3: Normalize data
    map(order => ({
        id: order.id,
        items: order.items || [],
        total: Number(order.total),
        currency: order.currency || 'USD',
        date: new Date(order.date || Date.now()),
        status: order.status || 'pending'
    })),
    
    // Step 4: Add computed fields
    map(order => ({
        ...order,
        itemCount: order.items.length,
        formattedTotal: new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: order.currency
        }).format(order.total),
        isHighValue: order.total > 500
    })),
    
    // Step 5: Group by status
    reduce((acc, order) => {
        if (!acc[order.status]) {
            acc[order.status] = { orders: [], total: 0 };
        }
        acc[order.status].orders.push(order);
        acc[order.status].total += order.total;
        return acc;
    }, {})
);

// Usage
const rawOrders = JSON.stringify([
    { id: '1', total: 150, items: [1, 2], status: 'completed' },
    { id: '2', total: 750, items: [1, 2, 3, 4], status: 'completed' },
    { id: '3', total: 50, status: 'pending' }
]);

const result = processOrders(rawOrders);
console.log(result);
```

### 2. Validation Pipeline

```javascript
// students/16_validation.js

// Validation composition
const validate = (rules) => (data) => {
    const errors = [];
    
    for (const [field, validators] of Object.entries(rules)) {
        const value = data[field];
        
        for (const { test, message } of validators) {
            if (!test(value, data)) {
                errors.push({ field, message });
            }
        }
    }
    
    return {
        valid: errors.length === 0,
        errors
    };
};

// Validation rules
const userValidator = validate({
    name: [
        { test: v => v && v.length > 0, message: 'Name is required' },
        { test: v => v.length <= 100, message: 'Name too long' }
    ],
    email: [
        { test: v => v && v.includes('@'), message: 'Invalid email format' }
    ],
    age: [
        { test: v => v >= 18, message: 'Must be 18 or older' },
        { test: v => v <= 150, message: 'Invalid age' }
    ],
    password: [
        { test: v => v && v.length >= 8, message: 'Password too short' },
        { test: v => /[A-Z]/.test(v), message: 'Need uppercase letter' },
        { test: v => /[0-9]/.test(v), message: 'Need number' }
    ]
});

// Usage
const userData = {
    name: 'Alice',
    email: 'alice@example.com',
    age: 25,
    password: 'Password123'
};

console.log(userValidator(userData));
// { valid: true, errors: [] }

const invalidData = {
    name: '',
    email: 'invalid',
    age: 15,
    password: 'abc'
};

console.log(userValidator(invalidData));
// { valid: false, errors: [...] }
```

### 3. Event Processing

```javascript
// students/17_eventProcessing.js

// Event stream processing
class EventProcessor {
    constructor() {
        this.handlers = [];
    }
    
    use(middleware) {
        this.handlers.push(middleware);
        return this;
    }
    
    process(event) {
        return this.handlers.reduce(
            (promise, handler) => promise.then(e => handler(e)),
            Promise.resolve(event)
        );
    }
}

// Middleware functions
const logger = async (event) => {
    console.log(`[${event.type}] Processing...`);
    return event;
};

const validator = async (event) => {
    if (!event.type || !event.data) {
        throw new Error('Invalid event structure');
    }
    return event;
};

const enricher = async (event) => ({
    ...event,
    timestamp: event.timestamp || new Date().toISOString(),
    source: 'event-processor'
});

const persister = async (event) => {
    // Save to database
    console.log('Persisting event:', event.id);
    return event;
};

// Pipeline setup
const processor = new EventProcessor()
    .use(logger)
    .use(validator)
    .use(enricher)
    .use(persister);

// Process events
processor.process({ type: 'USER_CREATED', data: { name: 'Alice' } })
    .then(result => console.log('Processed:', result));
```

---

## Common Pitfalls

### 1. Not Returning from Map/Filter

```javascript
// students/18_pitfallReturn.js

// ❌ WRONG: Forgot return
const doubled = numbers.map(n => n * 2);
const doubledBad = numbers.map(function(n) {
    n * 2;  // No return!
});
console.log(doubledBad);  // [undefined, undefined, undefined]

// ✅ CORRECT: Return value
const doubledGood = numbers.map(n => n * 2);
console.log(doubledGood);  // [2, 4, 6, 8, 10]

// Filter same issue
const evensBad = numbers.filter(n => {
    n % 2 === 0;  // No return!
});
console.log(evensBad);  // []
```

### 2. Mutating in Reduce

```javascript
// students/19_pitfallReduce.js

// ❌ WRONG: Mutating accumulator
const badReduce = [1, 2, 3].reduce((acc, n) => {
    acc.push(n * 2);
    return acc;
}, []);
// This is actually fine, but mutation in general should be avoided

// ❌ More problematic: mutating original array
const data = [{ a: 1 }, { a: 2 }, { a: 3 }];
const badMap = data.map(item => {
    item.b = item.a * 2;  // Mutating original!
    return item;
});
console.log(data);  // Original is now mutated!

// ✅ CORRECT: Create new objects
const goodMap = data.map(item => ({
    ...item,
    b: item.a * 2
}));
console.log(data);  // Original unchanged
```

### 3. Not Handling Empty Arrays

```javascript
// students/20_pitfallEmpty.js

// ❌ WRONG: Not providing initial value
const empty = [];
const sum = empty.reduce((acc, n) => acc + n);  // TypeError!

// ✅ CORRECT: Always provide initial value
const sumWithInitial = empty.reduce((acc, n) => acc + n, 0);
console.log(sumWithInitial);  // 0

// Filter can return empty - handle it
const result = [1, 2, 3]
    .filter(n => n > 10)
    .map(n => n.toString());

console.log(result);  // [] - empty, not error

// Use optional chaining for safety
const processed = (numbers?.reduce || (() => 0))(...) 
// Not ideal, just handle properly
```

---

## Key Takeaways

1. **Map**: Transforms each element - always returns new array of same length.

2. **Filter**: Selects elements that pass a test - returns subset, may be shorter.

3. **Reduce**: Accumulates to single value - always provide initial value.

4. **Function Composition**: Combine small functions - use `pipe` or `compose`.

5. **Callbacks**: Functions passed as arguments - use error-first pattern for async.

6. **Chaining**: Chain methods for readable data transformations.

7. **Immutability**: Never mutate original array - always create new arrays.

8. **Performance**: For very large arrays, consider for-loops. For most cases, map/filter/reduce are fine.

---

## Related Files

- [02_FUNCTION_PARAMETERS_AND_ARGUMENTS.md](./02_FUNCTION_PARAMETERS_AND_ARGUMENTS.md) - Rest/spread parameters
- [03_SCOPE_CHAIN_AND_CLOSURES.md](./03_SCOPE_CHAIN_AND_CLOSURES.md) - Closures in callbacks
- [04_ARROW_FUNCTIONS_MASTER.md](./04_ARROW_FUNCTIONS_MASTER.md) - Arrow functions for callbacks
- [07_FUNCTION_MEMORIZATION_AND_OPTIMIZATION.md](./07_FUNCTION_MEMORIZATION_AND_OPTIMIZATION.md) - Performance optimization