# 🎯 Pattern Matching

## 📋 Table of Contents

1. [Overview](#overview)
2. [Switch Expressions (ES2024+)](#switch-expressions-es2024)
3. [Match Expressions](#match-expressions)
4. [Destructuring Patterns](#destructuring-patterns)
5. [Guard Patterns](#guard-patterns)
6. [Practical Use Cases](#practical-use-cases)
7. [Modern Alternatives to Switch](#modern-alternatives-to-switch)
8. [Performance Considerations](#performance-considerations)
9. [Key Takeaways](#key-takeaways)

---

## Overview

Pattern matching in JavaScript has evolved significantly with modern ES2024+ features. What once required verbose if-else chains or switch statements can now be expressed more elegantly using match expressions, destructuring patterns, and guard clauses. This guide covers modern pattern matching techniques that make your code more readable and maintainable.

---

## Switch Expressions (ES2024+)

### Basic Switch Expression

Switch expressions return values instead of just executing code:

```javascript
// File: switch-expression-basic.js
// Description: Basic switch expression syntax

const day = 'Monday';

const dayType = switch (day) {
    case 'Saturday', 'Sunday' -> 'weekend'
    case 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday' -> 'weekday'
    default -> 'invalid'
};

console.log(dayType); // "weekday"
```

### Switch Expression with Multiple Values

```javascript
// File: switch-multi-value.js
// Description: Switch with multiple matched values

const status = 'active';

const statusLabel = switch (status) {
    case 'active', 'pending', 'running' -> 'In Progress'
    case 'completed', 'done', 'success' -> 'Completed'
    case 'failed', 'error', 'cancelled' -> 'Failed'
    default -> 'Unknown'
};

console.log(statusLabel); // "In Progress"
```

### Switch Expression with Conditions

```javascript
// File: switch-conditions.js
// Description: Switch with conditional matching

const score = 85;

const grade = switch (true) {
    case score >= 90 -> 'A'
    case score >= 80 -> 'B'
    case score >= 70 -> 'C'
    case score >= 60 -> 'D'
    default -> 'F'
};

console.log(grade); // "B"
```

### Professional Use Case: HTTP Status Handler

```javascript
// File: http-status-handler.js
// Description: HTTP response handling with switch

function handleHttpStatus(status) {
    return switch (status) {
        case 200 -> { success: true, message: 'OK' }
        case 201 -> { success: true, message: 'Created' }
        case 204 -> { success: true, message: 'No Content' }
        case 400 -> { success: false, message: 'Bad Request' }
        case 401 -> { success: false, message: 'Unauthorized' }
        case 403 -> { success: false, message: 'Forbidden' }
        case 404 -> { success: false, message: 'Not Found' }
        case 500 -> { success: false, message: 'Internal Server Error' }
        default -> { success: false, message: 'Unknown Status' }
    };
}

console.log(handleHttpStatus(200));
console.log(handleHttpStatus(404));
```

---

## Match Expressions

### Basic Match Syntax

The `match` expression allows comprehensive pattern matching:

```javascript
// File: match-basic.js
// Description: Basic match expression

const result = 42;

const description = match (result) {
    0 -> 'zero',
    1 -> 'one',
    > 0 and < 10 -> 'single digit',
    >= 10 and <= 99 -> 'two digits',
    default -> 'large number'
};

console.log(description); // "two digits"
```

### Match with Object Patterns

```javascript
// File: match-object.js
// Description: Match with object patterns

const user = { name: 'John', role: 'admin', active: true };

const userInfo = match (user) {
    { role: 'admin' } -> 'Administrator access',
    { role: 'editor', active: true } -> 'Editor access',
    { active: false } -> 'Account inactive',
    default -> 'Standard access'
};

console.log(userInfo); // "Administrator access"
```

### Match with Array Patterns

```javascript
// File: match-array.js
// Description: Match with array patterns

const point = [10, 20];

const quadrant = match (point) {
    [0, 0] -> 'origin',
    [_, 0] -> 'on x-axis',
    [0, _] -> 'on y-axis',
    [> 0, > 0] -> 'Quadrant I',
    [< 0, > 0] -> 'Quadrant II',
    [< 0, < 0] -> 'Quadrant III',
    [> 0, < 0] -> 'Quadrant IV'
};

console.log(quadrant); // "Quadrant I"
```

### Professional Use Case: Message Router

```javascript
// File: message-router.js
// Description: Message type router with match

class MessageRouter {
    route(message) {
        return match (message) {
            { type: 'text', content: string } -> this.handleText(content),
            { type: 'image', url: string } -> this.handleImage(url),
            { type: 'video', url: string, duration: number } -> this.handleVideo(url, duration),
            { type: 'audio', url: string } -> this.handleAudio(url),
            { type: 'file', name: string } -> this.handleFile(name),
            default -> { error: 'Unknown message type' }
        };
    }

    handleText(content) {
        return { handler: 'text', content: content.toUpperCase() };
    }

    handleImage(url) {
        return { handler: 'image', url };
    }

    handleVideo(url, duration) {
        return { handler: 'video', url, duration };
    }

    handleAudio(url) {
        return { handler: 'audio', url };
    }

    handleFile(name) {
        return { handler: 'file', name };
    }
}

const router = new MessageRouter();
console.log(router.route({ type: 'text', content: 'hello world' }));
console.log(router.route({ type: 'image', url: 'img.jpg' }));
```

---

## Destructuring Patterns

### Array Destructuring with Match

```javascript
// File: destructuring-array.js
// Description: Array destructuring patterns

const coords = [100, 200];

const [x, y] = coords;
console.log(x, y); // 100, 200

// Rest pattern
const [first, ...rest] = [1, 2, 3, 4, 5];
console.log(first); // 1
console.log(rest); // [2, 3, 4, 5]
```

### Object Destructuring with Match

```javascript
// File: destructuring-object.js
// Description: Object destructuring patterns

const user = {
    name: 'John Doe',
    email: 'john@example.com',
    age: 30,
    address: {
        city: 'New York',
        country: 'USA'
    }
};

const { 
    name, 
    email, 
    address: { city } 
} = user;

console.log(name, email, city);
```

### Nested Destructuring

```javascript
// File: destructuring-nested.js
// Description: Nested destructuring patterns

const data = {
    users: [
        { name: 'Alice', scores: [90, 85, 95] },
        { name: 'Bob', scores: [70, 80, 75] }
    ]
};

const {
    users: [
        { name: firstName, scores: [firstScore] },
        secondUser
    ]
} = data;

console.log(firstName, firstScore);
```

### Professional Use Case: API Response Handler

```javascript
// File: api-response.js
// Description: API response with destructuring

function processApiResponse(response) {
    const {
        data: users,
        meta: { total, page, limit },
        errors = []
    } = response;

    if (errors.length > 0) {
        return { success: false, errors };
    }

    return {
        success: true,
        users,
        pagination: { total, page, limit },
        hasMore: page * limit < total
    };
}

const response = {
    data: [{ name: 'Alice' }, { name: 'Bob' }],
    meta: { total: 100, page: 1, limit: 10 }
};

console.log(processApiResponse(response));
```

---

## Guard Patterns

### Basic Guard Syntax

Guards add additional conditions to patterns:

```javascript
// File: guard-basic.js
// Description: Basic guard patterns

function classify(number) {
    if (number > 0) {
        if (number % 2 === 0) return 'positive even';
        return 'positive odd';
    }
    if (number < 0) {
        return 'negative';
    }
    return 'zero';
}

// Using match with guard
const classifyMatch = (number) => match (number) {
    n if n > 0 && n % 2 === 0 -> 'positive even',
    n if n > 0 -> 'positive odd',
    0 -> 'zero',
    n if n < 0 -> 'negative'
};

console.log(classify(5));     // "positive odd"
console.log(classifyMatch(5)); // "positive odd"
```

### Guard with Multiple Conditions

```javascript
// File: guard-multiple.js
// Description: Multiple guard conditions

function validateUser(user) {
    return match (user) {
        u if u.age >= 18 && u.age <= 65 && u.isActive -> 'valid adult',
        u if u.age < 18 -> 'minor',
        u if u.age > 65 -> 'senior',
        u if !u.isActive -> 'inactive account',
        default -> 'invalid'
    };
}

console.log(validateUser({ age: 30, isActive: true }));
console.log(validateUser({ age: 16, isActive: true }));
console.log(validateUser({ age: 70, isActive: true }));
```

### Professional Use Case: Shipping Calculator

```javascript
// File: shipping-calculator.js
// Description: Shipping cost with guard patterns

function calculateShipping(order) {
    const { weight, destination, express = false } = order;
    
    return match (order) {
        { weight: w, destination: 'international' } 
            if w <= 1 -> 10.00,
        { weight: w, destination: 'international' } 
            if w <= 5 -> 25.00,
        { weight: w, destination: 'international' } 
            -> 50.00 + (w - 5) * 5,
        { weight: w, destination: 'domestic' } 
            if w <= 1 -> 5.00,
        { weight: w, destination: 'domestic' } 
            if w <= 5 -> 10.00,
        { weight: w, destination: 'domestic' } 
            -> 15.00 + (w - 5) * 2,
        default -> { error: 'Invalid order' }
    };
}

console.log(calculateShipping({ weight: 2, destination: 'domestic' }));
console.log(calculateShipping({ weight: 10, destination: 'international' }));
```

---

## Practical Use Cases

### 1. Event Handler

```javascript
// File: event-handler.js
// Description: Event handling with pattern matching

class EventHandler {
    handle(event) {
        return match (event) {
            { type: 'click', target: 'button' } -> 'Button clicked',
            { type: 'click', target: 'link' } -> 'Link clicked',
            { type: 'submit', formId: string } -> `Form ${formId} submitted`,
            { type: 'keydown', key: 'Enter' } -> 'Enter pressed',
            { type: 'keydown', key: 'Escape' } -> 'Escape pressed',
            { type: 'mouseenter', target } -> `Entered ${target}`,
            { type: 'mouseleave', target } -> `Left ${target}`,
            default -> 'Unknown event'
        };
    }
}
```

### 2. Configuration Merger

```javascript
// File: config-merger.js
// Description: Merge configurations

function mergeConfig(defaults, user) {
    return {
        ...defaults,
        ...match (user) {
            { apiKey: string } -> { apiKey: user.apiKey },
            { theme: 'dark', accentColor: string } -> { 
                theme: 'dark', 
                accentColor: user.accentColor 
            },
            default -> user
        }
    };
}
```

### 3. Command Parser

```javascript
// File: command-parser.js
// Description: Parse commands

function parseCommand(input) {
    const parts = input.split(' ');
    const [command, ...args] = parts;
    
    return match (command) {
        'help' -> { action: 'showHelp' },
        'run' -> { action: 'run', script: args[0] },
        'build' -> { action: 'build', target: args[0] || 'all' },
        'test' -> { action: 'test', coverage: args.includes('--coverage') },
        'deploy' -> { action: 'deploy', environment: args[0] || 'production' },
        default -> { action: 'unknown', command }
    };
}
```

---

## Modern Alternatives to Switch

### 1. Object Map

```javascript
// File: object-map.js
// Description: Using object map instead of switch

const handlers = {
    home: () => 'Home page',
    about: () => 'About page',
    contact: () => 'Contact page',
    default: () => 'Not found'
};

function getPage(name) {
    return handlers[name] || handlers.default;
}

console.log(getPage('about'));
```

### 2. Map Object

```javascript
// File: map-object.js
// Description: Using Map for complex keys

const statusHandlers = new Map([
    ['active', () => 'Active user'],
    ['inactive', () => 'Inactive user'],
    ['pending', () => 'Pending approval'],
    ['suspended', () => 'Account suspended']
]);

function getStatusInfo(status) {
    return statusHandlers.get(status)?.() || 'Unknown status';
}
```

### 3. Function Map

```javascript
// File: function-map.js
// Description: Using functions in map

const actions = {
    increment: (n) => n + 1,
    decrement: (n) => n - 1,
    double: (n) => n * 2,
    square: (n) => n ** 2
};

function applyAction(value, action) {
    return actions[action]?.(value) ?? value;
}
```

---

## Performance Considerations

- **Switch expressions** are optimized by modern engines
- **Object maps** provide O(1) lookup
- **Pattern matching** compile to efficient code

---

## Key Takeaways

- Use switch expressions for value-based decisions
- Employ match for complex pattern matching
- Apply guards for conditional logic
- Prefer object maps over switch for static mappings

---

## Related Files

- [01_IF_ELSE_CONDITIONAL_STATEMENTS](./01_IF_ELSE_CONDITIONAL_STATEMENTS.md)
- [02_LOOPS_MASTER](./02_LOOPS_MASTER.md)
- [03_LOGICAL_OPERATORS_MASTER](./03_LOGICAL_OPERATORS_MASTER.md)
- [06_CONTROL_FLOW_EXAMPLES](./06_CONTROL_FLOW_EXAMPLES.md)

---

*Last updated: 2026-04-03*