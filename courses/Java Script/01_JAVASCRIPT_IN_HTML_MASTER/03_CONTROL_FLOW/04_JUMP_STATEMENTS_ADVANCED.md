# 🚀 Jump Statements Advanced

## 📋 Table of Contents

1. [Overview](#overview)
2. [break Statement](#break-statement)
3. [continue Statement](#continue-statement)
4. [return Statement](#return-statement)
5. [Labels in Nested Loops](#labels-in-nested-loops)
6. [throw Statement](#throw-statement)
7. [Professional Use Cases](#professional-use-cases)
8. [Best Practices](#best-practices)
9. [Performance Considerations](#performance-considerations)
10. [Key Takeaways](#key-takeaways)

---

## Overview

Jump statements control the flow of execution within functions and loops. The `break`, `continue`, `return`, and `throw` statements allow you to exit loops, skip iterations, return values from functions, and handle exceptional situations. Understanding these statements is crucial for writing controlled, predictable JavaScript code.

---

## break Statement

### Basic break Usage

The `break` statement immediately exits the nearest loop or switch:

```javascript
// File: break-basic.js
// Description: Basic break statement

const numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

for (let i = 0; i < numbers.length; i++) {
    if (numbers[i] === 6) {
        console.log('Found 6, breaking...');
        break;
    }
    console.log(numbers[i]);
}
// Output: 1, 2, 3, 4, 5, "Found 6, breaking..."
```

### break in While Loop

```javascript
// File: break-while.js
// Description: Break in while loop

let count = 0;

while (true) {
    count++;
    const random = Math.floor(Math.random() * 10);
    
    if (random === 7) {
        console.log(`Found 7 after ${count} attempts`);
        break;
    }
}
```

### break in Switch

```javascript
// File: break-switch.js
// Description: Break in switch statement

function getDayType(day) {
    switch (day) {
        case 'saturday':
        case 'sunday':
            return 'weekend';
        case 'monday':
        case 'tuesday':
        case 'wednesday':
        case 'thursday':
        case 'friday':
            return 'weekday';
        default:
            return 'invalid';
    }
}

console.log(getDayType('saturday')); // "weekend"
```

### Professional Use Case: Search Algorithm

```javascript
// File: search-algorithm.js
// Description: Linear search with break

class SearchEngine {
    constructor(data) {
        this.data = data;
    }

    findFirst(predicate) {
        for (let i = 0; i < this.data.length; i++) {
            if (predicate(this.data[i])) {
                return { found: true, index: i, item: this.data[i] };
            }
        }
        return { found: false, index: -1 };
    }

    findAll(predicate) {
        const results = [];
        
        for (let i = 0; i < this.data.length; i++) {
            if (predicate(this.data[i])) {
                results.push({ index: i, item: this.data[i] });
            }
        }
        
        return results;
    }

    findWithLimit(predicate, limit) {
        const results = [];
        
        for (let i = 0; i < this.data.length; i++) {
            if (predicate(this.data[i])) {
                results.push({ index: i, item: this.data[i] });
                
                if (results.length >= limit) {
                    break;
                }
            }
        }
        
        return results;
    }
}

const users = [
    { id: 1, name: 'Alice', active: true },
    { id: 2, name: 'Bob', active: false },
    { id: 3, name: 'Charlie', active: true },
    { id: 4, name: 'Diana', active: true }
];

const engine = new SearchEngine(users);
console.log(engine.findFirst(u => u.active)); 
```

---

## continue Statement

### Basic continue Usage

```javascript
// File: continue-basic.js
// Description: Basic continue statement

const numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

for (let i = 0; i < numbers.length; i++) {
    if (numbers[i] % 2 === 0) {
        continue;
    }
    console.log(numbers[i]);
}
// Output: 1, 3, 5, 7, 9
```

### continue with Filtering

```javascript
// File: continue-filtering.js
// Description: Filter with continue

const users = [
    { name: 'Alice', role: 'admin', active: true },
    { name: 'Bob', role: 'user', active: false },
    { name: 'Charlie', role: 'user', active: true },
    { name: 'Diana', role: 'guest', active: true }
];

const activeAdmins = [];

for (const user of users) {
    if (!user.active) continue;
    if (user.role !== 'admin') continue;
    
    activeAdmins.push(user);
}

console.log(activeAdmins);
```

### Professional Use Case: Validation Pipeline

```javascript
// File: validation-pipeline.js
// Description: Validation with continue

class ValidationPipeline {
    constructor() {
        this.rules = [];
    }

    addRule(validator, message) {
        this.rules.push({ validator, message });
    }

    validate(data) {
        const errors = [];

        for (const rule of this.rules) {
            if (!rule.validator(data)) {
                continue;
            }
            errors.push(rule.message);
        }

        return {
            valid: errors.length === 0,
            errors
        };
    }
}

const validator = new ValidationPipeline();
validator.addRule(d => d.email?.includes('@'), 'Valid email required');
validator.addRule(d => d.password?.length >= 8, 'Password must be at least 8 characters');

console.log(validator.validate({ email: 'test@example.com', password: '12345678' }));
```

---

## return Statement

### Basic return Usage

```javascript
// File: return-basic.js
// Description: Basic return statements

function add(a, b) {
    return a + b;
}

function findUser(users, id) {
    for (const user of users) {
        if (user.id === id) {
            return user;
        }
    }
    return null;
}

function divide(a, b) {
    if (b === 0) {
        return { success: false, error: 'Cannot divide by zero' };
    }
    return { success: true, result: a / b };
}

console.log(add(2, 3));         // 5
console.log(divide(10, 2));    // { success: true, result: 5 }
console.log(divide(10, 0));   // { success: false, error: "Cannot divide by zero" }
```

### return with Objects

```javascript
// File: return-objects.js
// Description: Returning objects

class ResultWrapper {
    static success(data, message = 'Success') {
        return {
            success: true,
            data,
            message
        };
    }

    static error(error, code = 'ERROR') {
        return {
            success: false,
            error,
            code
        };
    }
}

function processData(input) {
    if (!input) {
        return ResultWrapper.error('Input required', 'INVALID_INPUT');
    }
    
    return ResultWrapper.success({ processed: input.toUpperCase() });
}

console.log(processData('hello'));
console.log(processData(null));
```

### return in Arrow Functions

```javascript
// File: return-arrow.js
// Description: Return in arrow functions

const add = (a, b) => a + b;
const multiply = (a, b) => a * b;
const max = (a, b) => a > b ? a : b;

const getUser = (users, id) => {
    const user = users.find(u => u.id === id);
    return user || null;
};

console.log(add(2, 3));     // 5
console.log(multiply(2, 3)); // 6
console.log(max(2, 3));     // 3
```

---

## Labels in Nested Loops

### Label Syntax

```javascript
// File: label-basic.js
// Description: Label syntax for loops

outerLoop: for (let i = 0; i < 3; i++) {
    for (let j = 0; j < 3; j++) {
        console.log(`i=${i}, j=${j}`);
        if (j === 1) {
            break outerLoop;
        }
    }
}
// Output: i=0,j=0, i=0,j=1 (stops entirely)
```

### continue with Labels

```javascript
// File: continue-label.js
// Description: Continue with labels

outerLoop: for (let i = 0; i < 3; i++) {
    innerLoop: for (let j = 0; j < 3; j++) {
        if (i === 1) {
            continue outerLoop;
        }
        console.log(`i=${i}, j=${j}`);
    }
}
// Only processes i=0 completely
```

### Professional Use Case: Multi-dimensional Search

```javascript
// File: multi-dim-search.js
// Description: Search in 2D matrix

function findInMatrix(matrix, target) {
    let found = null;
    
    searchLoop: for (let row = 0; row < matrix.length; row++) {
        for (let col = 0; col < matrix[row].length; col++) {
            if (matrix[row][col] === target) {
                found = { row, col };
                break searchLoop;
            }
        }
    }
    
    return found;
}

const matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];

console.log(findInMatrix(matrix, 5)); // { row: 1, col: 1 }
console.log(findInMatrix(matrix, 9)); // { row: 2, col: 2 }
console.log(findInMatrix(matrix, 0)); // null
```

---

## throw Statement

### Basic throw Usage

```javascript
// File: throw-basic.js
// Description: Throwing exceptions

function divide(a, b) {
    if (b === 0) {
        throw new Error('Cannot divide by zero');
    }
    return a / b;
}

try {
    divide(10, 0);
} catch (e) {
    console.log(e.message); // "Cannot divide by zero"
}
```

### Custom Error Classes

```javascript
// File: custom-errors.js
// Description: Custom error types

class ValidationError extends Error {
    constructor(field, message) {
        super(message);
        this.name = 'ValidationError';
        this.field = field;
    }
}

class NotFoundError extends Error {
    constructor(resource) {
        super(`${resource} not found`);
        this.name = 'NotFoundError';
    }
}

function getUser(id) {
    const user = null; // Simulate DB lookup
    
    if (!user) {
        throw new NotFoundError('User');
    }
    
    return user;
}

try {
    getUser(123);
} catch (e) {
    if (e instanceof NotFoundError) {
        console.log('Handling not found:', e.message);
    } else {
        throw e;
    }
}
```

### Professional Use Case: Error Handling

```javascript
// File: error-handling.js
// Description: Professional error handling

class ApiError extends Error {
    constructor(message, statusCode, code) {
        super(message);
        this.name = 'ApiError';
        this.statusCode = statusCode;
        this.code = code;
    }
}

async function fetchUser(id) {
    try {
        const response = await fetch(`/api/users/${id}`);
        
        if (!response.ok) {
            if (response.status === 404) {
                throw new ApiError('User not found', 404, 'USER_NOT_FOUND');
            }
            throw new ApiError('Server error', response.status, 'SERVER_ERROR');
        }
        
        return response.json();
    } catch (e) {
        if (e instanceof ApiError) {
            throw e;
        }
        throw new ApiError('Network error', 0, 'NETWORK_ERROR');
    }
}
```

---

## Professional Use Cases

### 1. State Machine

```javascript
// File: state-machine.js
// Description: State machine with jump statements

class StateMachine {
    constructor() {
        this.state = 'idle';
    }

    transition(event) {
        switch (this.state) {
            case 'idle':
                if (event === 'start') {
                    this.state = 'running';
                    return { action: 'Start processing' };
                }
                break;
                
            case 'running':
                if (event === 'pause') {
                    this.state = 'paused';
                    return { action: 'Pause processing' };
                }
                if (event === 'complete') {
                    this.state = 'completed';
                    return { action: 'Complete processing' };
                }
                break;
                
            case 'paused':
                if (event === 'resume') {
                    this.state = 'running';
                    return { action: 'Resume processing' };
                }
                if (event === 'cancel') {
                    this.state = 'idle';
                    return { action: 'Cancel processing' };
                }
                break;
                
            case 'completed':
                if (event === 'reset') {
                    this.state = 'idle';
                    return { action: 'Reset state' };
                }
                break;
        }
        
        return { error: `Invalid event ${event} for state ${this.state}` };
    }
}

const machine = new StateMachine();
console.log(machine.transition('start'));
console.log(machine.transition('complete'));
```

### 2. Parser with break

```javascript
// File: parser.js
// Description: Token parser

class TokenParser {
    parse(tokens) {
        const result = [];
        
        for (let i = 0; i < tokens.length; i++) {
            const token = tokens[i];
            
            if (token.type === 'SKIP') {
                continue;
            }
            
            if (token.type === 'EOF') {
                break;
            }
            
            result.push(this.transform	token));
        }
        
        return result;
    }

    transformToken(token) {
        return { ...token, parsed: true };
    }
}
```

---

## Best Practices

1. **Use early returns** to reduce unnecessary computation
2. **Avoid complex nested structures** - extract to functions
3. **Use descriptive labels** for nested loops
4. **Throw meaningful errors** with context
5. **Always handle async errors** with try-catch

---

## Performance Considerations

- **break/continue** are faster than flag-based approaches
- **Early return** avoids unnecessary calculations
- **Label break** is more efficient than nested flag checks

---

## Key Takeaways

- `break` exits the current loop/switch immediately
- `continue` skips to the next iteration
- `return` exits the function entirely
- Labels allow breaking/continuing outer loops
- Use these statements to write clean, controlled code

---

## Related Files

- [01_IF_ELSE_CONDITIONAL_STATEMENTS](./01_IF_ELSE_CONDITIONAL_STATEMENTS.md)
- [02_LOOPS_MASTER](./02_LOOPS_MASTER.md)
- [03_LOGICAL_OPERATORS_MASTER](./03_LOGICAL_OPERATORS_MASTER.md)
- [07_DEBUGGING_CONTROL_FLOW](./07_DEBUGGING_CONTROL_FLOW.md)

---

*Last updated: 2026-04-03*