# 🔄 Loops Mastery

## 📋 Table of Contents

1. [Overview](#overview)
2. [For Loops](#for-loops)
3. [While Loops](#while-loops)
4. [Do-While Loops](#do-while-loops)
5. [For...of and For...in](#forof-and-forin)
6. [break and continue](#break-and-continue)
7. [Loop Patterns](#loop-patterns)
8. [When to Use Each Type](#when-to-use-each-type)
9. [Performance Considerations](#performance-considerations)
10. [Key Takeaways](#key-takeaways)

---

## Overview

Loops are fundamental constructs in JavaScript that allow you to execute a block of code repeatedly. Understanding the different types of loops and when to use each one is crucial for writing efficient, maintainable code. This guide covers for loops, while loops, do-while loops, and modern iteration methods.

---

## For Loops

### Traditional For Loop

The classic for loop provides complete control over iteration:

```javascript
// File: traditional-for-loop.js
// Description: Classic for loop syntax

for (let i = 0; i < 5; i++) {
    console.log(`Iteration ${i}`);
}
// Output: 0, 1, 2, 3, 4
```

### For Loop with Step

Control the iteration step for different patterns:

```javascript
// File: for-loop-steps.js
// Description: For loop with custom step

// Even numbers (0, 2, 4, 6, 8)
for (let i = 0; i < 10; i += 2) {
    console.log(i);
}

// Reverse (10, 8, 6, 4, 2)
for (let i = 10; i > 0; i -= 2) {
    console.log(i);
}

// Power of 2 (1, 2, 4, 8, 16)
for (let i = 1; i <= 16; i *= 2) {
    console.log(i);
}
```

### For Loop with Multiple Terms

Handle complex iteration patterns:

```javascript
// File: for-multiple-terms.js
// Description: Multiple loop variables

// Iterate two arrays simultaneously
const names = ['Alice', 'Bob', 'Charlie'];
const ages = [25, 30, 35];

for (let i = 0; i < names.length; i++) {
    console.log(`${names[i]} is ${ages[i]} years old`);
}

// Nested loops for matrices
const matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];

for (let row = 0; row < matrix.length; row++) {
    for (let col = 0; col < matrix[row].length; col++) {
        console.log(`Matrix[${row}][${col}] = ${matrix[row][col]}`);
    }
}
```

### Professional Use Case: Pagination

```javascript
// File: pagination.js
// Description: Paginated data fetching

class Paginator {
    constructor(data, pageSize = 10) {
        this.data = data;
        this.pageSize = pageSize;
    }

    getPage(pageNumber) {
        const startIndex = (pageNumber - 1) * this.pageSize;
        const endIndex = startIndex + this.pageSize;
        
        return {
            page: pageNumber,
            items: this.data.slice(startIndex, endIndex),
            totalItems: this.data.length,
            totalPages: Math.ceil(this.data.length / this.pageSize),
            hasNext: endIndex < this.data.length,
            hasPrevious: pageNumber > 1
        };
    }

    getAllPages() {
        const pages = [];
        const totalPages = Math.ceil(this.data.length / this.pageSize);
        
        for (let page = 1; page <= totalPages; page++) {
            pages.push(this.getPage(page));
        }
        
        return pages;
    }
}

// Usage
const items = Array.from({ length: 35 }, (_, i) => `Item ${i + 1}`);
const paginator = new Paginator(items, 10);

console.log(paginator.getPage(2));
// { page: 2, items: ["Item 11"..."Item 20"], totalItems: 35, totalPages: 4, ... }
```

---

## While Loops

### Basic While Loop

The while loop executes as long as the condition is true:

```javascript
// File: while-basic.js
// Description: Basic while loop

let count = 0;

while (count < 5) {
    console.log(count);
    count++;
}
// Output: 0, 1, 2, 3, 4
```

### While with Array Processing

Process array elements with while:

```javascript
// File: while-array.js
// Description: While loop with arrays

const items = ['apple', 'banana', 'cherry'];
let index = 0;

while (index < items.length) {
    console.log(items[index]);
    index++;
}
// Output: "apple", "banana", "cherry"
```

### While with Condition Checking

```javascript
// File: while-conditions.js
// Description: While with condition checks

class EventQueue {
    constructor() {
        this.queue = [];
    }

    enqueue(event) {
        this.queue.push(event);
    }

    processEvent() {
        while (this.queue.length > 0 && !this.isPaused) {
            const event = this.queue.shift();
            this.handleEvent(event);
        }
    }

    handleEvent(event) {
        console.log(`Processing: ${event.type}`);
    }

    pause() {
        this.isPaused = true;
    }

    resume() {
        this.isPaused = false;
        this.processEvent();
    }
}
```

### Professional Use Case: Retry Logic

```javascript
// File: retry-logic.js
// Description: Retry mechanism with while loop

class RetryHandler {
    constructor(options = {}) {
        this.maxRetries = options.maxRetries || 3;
        this.delay = options.delay || 1000;
        this.exponentialBackoff = options.exponentialBackoff || true;
    }

    async executeWithRetry(fn, context = 'operation') {
        let attempts = 0;
        let lastError = null;

        while (attempts < this.maxRetries) {
            try {
                return await fn();
            } catch (error) {
                lastError = error;
                attempts++;
                
                if (attempts < this.maxRetries) {
                    const delay = this.exponentialBackoff 
                        ? this.delay * Math.pow(2, attempts - 1)
                        : this.delay;
                    
                    console.log(`Retry ${attempts}/${this.maxRetries} for ${context} after ${delay}ms`);
                    await this.sleep(delay);
                }
            }
        }

        throw new Error(
            `Failed after ${this.maxRetries} attempts: ${lastError?.message}`
        );
    }

    sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}

// Usage
const retryHandler = new RetryHandler({ maxRetries: 3, delay: 500 });

async function fetchData() {
    const random = Math.random();
    if (random < 0.7) {
        throw new Error('Network error');
    }
    return { data: 'Success!' };
}

retryHandler.executeWithRetry(fetchData, 'fetch-user')
    .then(result => console.log(result))
    .catch(error => console.error(error));
```

---

## Do-While Loops

### Basic Do-While

The do-while loop executes at least once, then checks the condition:

```javascript
// File: basic-do-while.js
// Description: Do-while loop

let count = 0;

do {
    console.log(count);
    count++;
} while (count < 5);

// Output: 0, 1, 2, 3, 4
```

### Do-While for User Input

```javascript
// File: do-while-input.js
// Description: Do-while for validation

class InputValidator {
    constructor() {
        this.input = '';
    }

    async promptForInput(promptText) {
        do {
            this.input = await this.getUserInput(promptText);
            
            if (!this.input) {
                console.log('Input cannot be empty. Please try again.');
            } else if (this.input.length < 3) {
                console.log('Input must be at least 3 characters.');
            }
        } while (!this.input || this.input.length < 3);
        
        return this.input;
    }

    getUserInput(promptText) {
        return Promise.resolve(promptText);
    }
}
```

### Processing Until Condition Met

```javascript
// File: do-until-condition.js
// Description: Do-while until condition

class BatchProcessor {
    constructor() {
        this.queue = [];
        this.processed = [];
    }

    addToQueue(items) {
        this.queue.push(...items);
    }

    processBatch(batchSize = 10) {
        let processedCount = 0;

        do {
            const item = this.queue.shift();
            if (item) {
                this.processed.push(item);
                processedCount++;
            }
        } while (
            processedCount < batchSize && 
            this.queue.length > 0
        );

        return {
            processed: processedCount,
            remaining: this.queue.length
        };
    }
}
```

---

## For...of and For...in

### For...of (Iterables)

For...of iterates over iterable objects:

```javascript
// File: for-of-basic.js
// Description: For...of loop

const fruits = ['apple', 'banana', 'cherry'];

for (const fruit of fruits) {
    console.log(fruit);
}
// Output: "apple", "banana", "cherry"

// With index
for (const [index, fruit] of fruits.entries()) {
    console.log(`${index}: ${fruit}`);
}
// Output: "0: apple", "1: banana", "2: cherry"
```

### For...of with Objects (Using Object.entries)

```javascript
// File: for-of-objects.js
// Description: For...of with objects

const user = {
    name: 'John',
    age: 30,
    role: 'developer'
};

// Iterate over entries
for (const [key, value] of Object.entries(user)) {
    console.log(`${key}: ${value}`);
}

// Iterate over keys
for (const key of Object.keys(user)) {
    console.log(key);
}

// Iterate over values
for (const value of Object.values(user)) {
    console.log(value);
}
```

### For...in (Object Properties)

For...in iterates over enumerable properties:

```javascript
// File: for-in-basic.js
// Description: For...in loop

const user = {
    name: 'John',
    age: 30,
    role: 'developer'
};

for (const property in user) {
    console.log(`${property}: ${user[property]}`);
}
// Output: "name: John", "age: 30", "role: developer"
```

### Professional Use Case: Object Deep Clone

```javascript
// File: object-clone.js
// Description: Deep clone using for...in

function deepClone(obj) {
    if (obj === null || typeof obj !== 'object') {
        return obj;
    }

    if (Array.isArray(obj)) {
        const cloneArray = [];
        for (const item of obj) {
            cloneArray.push(deepClone(item));
        }
        return cloneArray;
    }

    const cloneObj = {};
    for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
            cloneObj[key] = deepClone(obj[key]);
        }
    }
    return cloneObj;
}

// Usage
const original = {
    name: 'John',
    address: {
        city: 'New York',
        zip: '10001'
    },
    hobbies: ['reading', 'coding']
};

const clone = deepClone(original);
clone.address.city = 'Boston';
clone.hobbies.push('gaming');

console.log(original.address.city); // "New York" (unchanged)
console.log(clone.address.city);        // "Boston"
console.log(original.hobbies);        // ["reading", "coding"]
console.log(clone.hobbies);          // ["reading", "coding", "gaming"]
```

---

## break and continue

### break Statement

The break statement exits the loop entirely:

```javascript
// File: break-statement.js
// Description: Break usage

const numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

for (let i = 0; i < numbers.length; i++) {
    if (numbers[i] === 6) {
        console.log('Found 6, stopping...');
        break;
    }
    console.log(numbers[i]);
}
// Output: 1, 2, 3, 4, 5, "Found 6, stopping..."
```

### continue Statement

The continue statement skips to the next iteration:

```javascript
// File: continue-statement.js
// Description: Continue usage

const numbers = [1, 2, 3, 4, 5];

for (let i = 0; i < numbers.length; i++) {
    if (numbers[i] % 2 === 0) {
        continue; // Skip even numbers
    }
    console.log(numbers[i]);
}
// Output: 1, 3, 5
```

### break with Nested Loops

```javascript
// File: break-nested.js
// Description: Break in nested loops

const matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];

outerLoop:
for (let row = 0; row < matrix.length; row++) {
    for (let col = 0; col < matrix[row].length; col++) {
        if (matrix[row][col] === 5) {
            console.log(`Found 5 at [${row}][${col}]`);
            break outerLoop;
        }
    }
}
// Output: "Found 5 at [1][1]"
```

### continue with Labels

```javascript
// File: continue-labels.js
// Description: Continue with labels

const grid = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];

evenRows:
for (let row = 0; row < grid.length; row++) {
    for (let col = 0; col < grid[row].length; col++) {
        if (row % 2 === 0) {
            continue evenRows;
        }
        console.log(grid[row][col]);
    }
}
// Output: 4, 5, 6, 7, 8, 9 (skips row 0)
```

---

## Loop Patterns

### Map Pattern

Transform each element:

```javascript
// File: map-pattern.js
// Description: Transform loop pattern

const numbers = [1, 2, 3, 4, 5];

// Using for...of
const doubled = [];
for (const num of numbers) {
    doubled.push(num * 2);
}
console.log(doubled); // [2, 4, 6, 8, 10]

// Using for loop
const squared = [];
for (let i = 0; i < numbers.length; i++) {
    squared.push(numbers[i] ** 2);
}
console.log(squared); // [1, 4, 9, 16, 25]
```

### Filter Pattern

Select elements matching criteria:

```javascript
// File: filter-pattern.js
// Description: Filter loop pattern

const numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

// Even numbers
const even = [];
for (const num of numbers) {
    if (num % 2 === 0) {
        even.push(num);
    }
}
console.log(even); // [2, 4, 6, 8, 10]

// Greater than 5
const greaterThanFive = [];
for (const num of numbers) {
    if (num > 5) {
        greaterThanFive.push(num);
    }
}
console.log(greaterThanFive); // [6, 7, 8, 9, 10]
```

### Reduce Pattern

Accumulate a single value:

```javascript
// File: reduce-pattern.js
// Description: Reduce loop pattern

const numbers = [1, 2, 3, 4, 5];

// Sum
let sum = 0;
for (const num of numbers) {
    sum += num;
}
console.log(sum); // 15

// Product
let product = 1;
for (const num of numbers) {
    product *= num;
}
console.log(product); // 120
```

### Find Pattern

Locate first matching element:

```javascript
// File: find-pattern.js
// Description: Find loop pattern

const users = [
    { id: 1, name: 'Alice', active: true },
    { id: 2, name: 'Bob', active: false },
    { id: 3, name: 'Charlie', active: true }
];

// Find first active user
let activeUser = null;
for (const user of users) {
    if (user.active) {
        activeUser = user;
        break;
    }
}
console.log(activeUser); // { id: 1, name: 'Alice', active: true }
```

### Professional Use Case: Event Emitter

```javascript
// File: event-emitter.js
// Description: Custom event emitter

class EventEmitter {
    constructor() {
        this.events = {};
    }

    on(event, listener) {
        if (!this.events[event]) {
            this.events[event] = [];
        }
        this.events[event].push(listener);
    }

    off(event, listenerToRemove) {
        if (!this.events[event]) return;

        this.events[event] = this.events[event].filter(
            listener => listener !== listenerToRemove
        );
    }

    emit(event, ...args) {
        if (!this.events[event]) return;

        for (const listener of this.events[event]) {
            listener(...args);
        }
    }

    once(event, listener) {
        const onceWrapper = (...args) => {
            listener(...args);
            this.off(event, onceWrapper);
        };
        this.on(event, onceWrapper);
    }
}

// Usage
const emitter = new EventEmitter();

emitter.on('greet', (name) => {
    console.log(`Hello, ${name}!`);
});

emitter.emit('greet', 'World'); // "Hello, World!"
emitter.emit('greet', 'JavaScript'); // "Hello, JavaScript!"
```

---

## When to Use Each Type

| Loop Type | Use Case | Example |
|----------|----------|----------|
| `for` | Known iteration count | Loop 10 times |
| `while` | Unknown count, condition-based | Process until queue empty |
| `do-while` | Must execute at least once | User input validation |
| `for...of` | Iterate arrays/iterables | Process array elements |
| `for...in` | Iterate object properties | Process key-value pairs |

---

## Performance Considerations

```javascript
// File: performance.js
// Description: Loop performance tips

const largeArray = Array.from({ length: 100000 }, (_, i) => i);

// BAD: Array.length re-evaluated each iteration
for (let i = 0; i < largeArray.length; i++) {
    // Access largeArray.length every iteration
}

// GOOD: Cache length
for (let i = 0, len = largeArray.length; i < len; i++) {
    // Length optimized
}

// BEST: for...of (built-in optimization)
for (const item of largeArray) {
    // Modern and optimized
}

// Avoid creating functions inside loops
// BAD
for (let i = 0; i < 1000; i++) {
    setTimeout(() => console.log(i), i);
}

// GOOD
for (let i = 0; i < 1000; i++) {
    const item = i; // Capture value
    setTimeout(() => console.log(item), i);
}
```

---

## Key Takeaways

- Use `for` loops when you know the iteration count
- Use `while` loops when the condition is dynamic
- Use `do-while` when code must execute at least once
- Prefer `for...of` for array iteration in modern JavaScript
- Use `for...in` carefully with objects (checks prototype chain)
- Cache array length in performance-critical loops
- Use `break` to exit early, `continue` to skip iterations

---

## Related Files

- [01_IF_ELSE_CONDITIONAL_STATEMENTS](./01_IF_ELSE_CONDITIONAL_STATEMENTS.md) - Conditional logic
- [03_LOGICAL_OPERATORS_MASTER](./03_LOGICAL_OPERATORS_MASTER.md) - Logical operators
- [04_JUMP_STATEMENTS_ADVANCED](./04_JUMP_STATEMENTS_ADVANCED.md) - Advanced jump control
- [06_CONTROL_FLOW_EXAMPLES](./06_CONTROL_FLOW_EXAMPLES.md) - Real-world examples

---

*Last updated: 2026-04-03*