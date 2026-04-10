# 🔧 Debugging Control Flow

## 📋 Table of Contents

1. [Overview](#overview)
2. [Common Pitfalls](#common-pitfalls)
3. [Debugging Techniques](#debugging-techniques)
4. [Error Patterns](#error-patterns)
5. [Problem Solving](#problem-solving)
6. [Testing Strategies](#testing-strategies)
7. [Best Practices](#best-practices)
8. [Key Takeaways](#key-takeaways)

---

## Overview

Debugging control flow issues is one of the most common tasks in JavaScript development. This guide covers common pitfalls, debugging techniques, and systematic approaches to identify and fix control flow problems in your code.

---

## Common Pitfalls

### 1. Forgetting break in Switch

```javascript
// File: pitfall-missing-break.js
// Description: Missing break statement

const day = 'Tuesday';

switch (day) {
    case 'Monday':
        console.log('Start of week');
    case 'Tuesday':
        console.log('Second day');
    case 'Wednesday':
        console.log('Middle of week');
    default:
        console.log('Another day');
}
// Output: "Second day", "Middle of week", "Another day"
// Bug: Missing break statements!
```

**Solution**: Always add break statements.

### 2. Using == Instead of ===

```javascript
// File: pitfall-equality.js
// Description: Loose equality issues

console.log(0 == false);   // true (unintended!)
console.log('' == false);  // true (unintended!)
console.log('0' == 0);     // true (unintended!)

// Solution: Always use strict equality
console.log(0 === false);  // false (correct)
console.log('' === false); // false (correct)
```

### 3. Accidental Global Variables

```javascript
// File: pitfall-global.js
// Description: Creating globals accidentally

function calculate() {
    result = 10;  // Missing const/let/var!
    // Creates global variable in non-strict mode
}

calculate();
console.log(result); // 10
```

**Solution**: Use strict mode and declare variables properly.

### 4. Loop Reference Issues

```javascript
// File: pitfall-loop-reference.js
// Description: Loop closure problem

// Bug: All functions reference same i
const functions = [];
for (var i = 0; i < 3; i++) {
    functions.push(() => i);
}

console.log(functions[0]()); // 3 (should be 0!)
console.log(functions[1]()); // 3 (should be 1!)

// Solution: Use let or IIFE
const functions2 = [];
for (let i = 0; i < 3; i++) {
    functions2.push(() => i);
}

console.log(functions2[0]()); // 0
console.log(functions2[1]()); // 1
```

### 5. Infinite Loops

```javascript
// File: pitfall-infinite-loop.js
// Description: Infinite loop

let i = 0;
while (i < 10) {
    console.log(i);
    // Bug: i never increments
    // i++ missing!
}

// Solution: Always ensure loop progresses
let j = 0;
while (j < 10) {
    console.log(j);
    j++; // Make sure to increment
}
```

---

## Debugging Techniques

### 1. Logging with Conditional Breakpoints

```javascript
// File: debugging-logging.js
// Description: Debug logging

function findItem(array, target) {
    for (let i = 0; i < array.length; i++) {
        console.log(`Checking index ${i}: ${array[i]}`);
        
        if (array[i] === target) {
            console.log(`Found at index ${i}`);
            return i;
        }
    }
    return -1;
}

findItem(['a', 'b', 'c'], 'b');
// Logs: "Checking index 0: a", "Checking index 1: b", Found at index 1
```

### 2. Using Debugger Statements

```javascript
// File: debugging-debugger.js
// Description: Using debugger

function processData(data) {
    console.log('Before processing');
    debugger; // Browser will pause here
    console.log('After processing');
    
    return data.map(x => x * 2);
}
```

### 3. Logging State Changes

```javascript
// File: debugging-state.js
// Description: State change logging

class StateManager {
    constructor() {
        this.state = 'idle';
    }

    setState(newState) {
        console.log(`State: ${this.state} -> ${newState}`);
        this.state = newState;
    }
}

const manager = new StateManager();
manager.setState('loading');
manager.setState('ready');
```

### 4. Try-Catch with Stack Traces

```javascript
// File: debugging-stack.js
// Description: Stack trace logging

function wrapper() {
    return inner();
}

function inner() {
    try {
        throw new Error('Something went wrong');
    } catch (e) {
        console.log('Stack trace:');
        console.log(e.stack);
        throw e;
    }
}
```

### 5. Debugging Logical Operators

```javascript
// File: debugging-logical.js
// Description: Debug logical operators

// Log the value of each operand
const value = user?.name;
console.log('Value:', value);

if (value && value.length > 0) {
    console.log('Name is valid');
}
```

---

## Error Patterns

### 1. Incorrect Condition Order

```javascript
// File: error-condition-order.js
// Description: Wrong order of conditions

// Bug: Expensive check first
if (expensiveCheck() || cheapCheck()) {
    // Do something
}

// Solution: Put cheap check first
if (cheapCheck() || expensiveCheck()) {
    // Do something
}
```

### 2. Missing Default Case

```javascript
// File: error-missing-default.js
// Description: Missing default case

switch (status) {
    case 'active':
        return 'Active';
    case 'pending':
        return 'Pending';
    // Bug: No default handling unknown status
}

// Solution: Always add default
switch (status) {
    case 'active':
        return 'Active';
    case 'pending':
        return 'Pending';
    default:
        console.log('Unknown status:', status);
        return 'Unknown';
}
```

### 3. Off-by-One Errors

```javascript
// File: error-off-by-one.js
// Description: Off-by-one errors

const array = [1, 2, 3];

// Bug: Wrong comparison
for (let i = 0; i <= array.length; i++) {
    console.log(array[i]); // undefined at the end!
}

// Solution: Use < instead of <=
for (let i = 0; i < array.length; i++) {
    console.log(array[i]); // Correct!
}
```

### 4. Incorrect Loop Boundaries

```javascript
// File: error-boundaries.js
// Description: Loop boundary issues

const data = [
    { name: 'Alice', active: true },
    { name: 'Bob', active: false }
];

// Bug: Filtering in wrong order
const result = data.filter(d => d.active);
const first = result[0]; // Returns Alice

// Correct approach
const firstActive = data.find(d => d.active);
```

### 5. Misunderstanding Truthy/Falsy

```javascript
// File: error-truthy-falsy.js
// Description: Truthy/falsy misunderstanding

const config = {
    debug: false,
    timeout: 0
};

// Bug: Using || for defaults
const debug = config.debug || true; // true (correct!)
const timeout = config.timeout || 3000; // 3000 (wrong, 0 is valid!)

// Solution: Use ??
const timeout = config.timeout ?? 3000; // 0 (correct!)
```

---

## Problem Solving

### Systematic Debugging Approach

```javascript
// File: debugging-systematic.js
// Description: Systematic debugging

function debugControlFlow(data, operations) {
    console.log('=== Starting Debug ===');
    console.log('Initial data:', data);
    
    let result = data;
    
    for (const [index, operation] of operations.entries()) {
        console.log(`\nStep ${index + 1}:`, operation.name);
        console.log('Input:', result);
        
        try {
            result = operation.fn(result);
            console.log('Output:', result);
        } catch (error) {
            console.log('Error:', error.message);
            console.log('Step failed at:', index);
            return { success: false, error: error.message, step: index };
        }
    }
    
    console.log('\n=== Debug Complete ===');
    return { success: true, result };
}

// Usage
debugControlFlow(
    [1, 2, 3],
    [
        { name: 'Filter even', fn: arr => arr.filter(x => x % 2 === 0) },
        { name: 'Double', fn: arr => arr.map(x => x * 2) },
        { name: 'Sum', fn: arr => arr.reduce((a, b) => a + b, 0) }
    ]
);
```

### Using Console.table for Arrays

```javascript
// File: debugging-table.js
// Description: Console.table for debugging

const users = [
    { name: 'Alice', role: 'admin', active: true },
    { name: 'Bob', role: 'user', active: false },
    { name: 'Charlie', role: 'editor', active: true }
];

console.table(users);

// Filter and table
console.table(users.filter(u => u.active));
```

---

## Testing Strategies

### Testing Control Flow

```javascript
// File: testing-control-flow.js
// Description: Testing control flow

describe('Control Flow', () => {
    describe('if-else', () => {
        test('executes if branch', () => {
            const result = processValue(10, {
                whenPositive: x => x * 2,
                whenNegative: x => x / 2
            });
            expect(result).toBe(20);
        });

        test('executes else branch', () => {
            const result = processValue(-10, {
                whenPositive: x => x * 2,
                whenNegative: x => x / 2
            });
            expect(result).toBe(-5);
        });
    });

    describe('loops', () => {
        test('correctly counts iterations', () => {
            const iterations = [];
            for (let i = 0; i < 3; i++) {
                iterations.push(i);
            }
            expect(iterations).toEqual([0, 1, 2]);
        });

        test('handles empty array', () => {
            let count = 0;
            for (const item of []) {
                count++;
            }
            expect(count).toBe(0);
        });
    });

    describe('break/continue', () => {
        test('break exits early', () => {
            const result = [1, 2, 3, 4, 5].find(x => x > 3);
            expect(result).toBe(4);
        });

        test('continue skips iterations', () => {
            const result = [1, 2, 3, 4, 5]
                .filter(x => x % 2 === 0)
                .map(x => x * 2);
            expect(result).toEqual([4, 8]);
        });
    });
});
```

---

## Best Practices

1. **Use strict mode** at the top of all files
2. **Always use** strict equality (`===`)
3. **Declare loop variables** with `let` instead of `var`
4. **Add default cases** to switch statements
5. **Log important state changes** during development

---

## Key Takeaways

- Understand common pitfalls to avoid them
- Use systematic debugging approaches
- Log state changes during development
- Write tests for control flow logic

---

## Related Files

- [01_IF_ELSE_CONDITIONAL_STATEMENTS](./01_IF_ELSE_CONDITIONAL_STATEMENTS.md)
- [02_LOOPS_MASTER](./02_LOOPS_MASTER.md)
- [03_LOGICAL_OPERATORS_MASTER](./03_LOGICAL_OPERATORS_MASTER.md)
- [04_JUMP_STATEMENTS_ADVANCED](./04_JUMP_STATEMENTS_ADVANCED.md)
- [06_CONTROL_FLOW_EXAMPLES](./06_CONTROL_FLOW_EXAMPLES.md)

---

*Last updated: 2026-04-03*