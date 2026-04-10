# 🔀 If-Else Conditional Statements

## 📋 Table of Contents

1. [Overview](#overview)
2. [Basic If-Else Statements](#basic-if-else-statements)
3. [Ternary Operators](#ternary-operators)
4. [Switch Statements](#switch-statements)
5. [Truthy and Falsy Values](#truthy-and-falsy-values)
6. [Advanced Patterns](#advanced-patterns)
7. [Best Practices](#best-practices)
8. [Common Pitfalls](#common-pitfalls)
9. [Performance Considerations](#performance-considerations)
10. [Key Takeaways](#key-takeaways)

---

## Overview

Conditional statements form the backbone of decision-making in JavaScript programs. They enable your code to execute different paths based on varying conditions, making applications dynamic and responsive to different states and inputs. Mastering if-else statements, ternary operators, switch statements, and understanding truthy/falsy values is essential for writing robust JavaScript applications.

This comprehensive guide covers all aspects of conditional statements in JavaScript, from basic syntax to advanced patterns and performance optimization techniques.

---

## Basic If-Else Statements

### Simple If Statement

The simplest form of conditional execution uses the `if` keyword to execute code only when a specified condition evaluates to true.

```javascript
// File: basic-if-statement.js
// Description: Simple if statement usage

const userAge = 18;

// Basic if statement
if (userAge >= 18) {
    console.log('User is an adult');
}
// Output: "User is an adult"
```

### If-Else Structure

The `if-else` structure provides an alternative path when the condition is false.

```javascript
// File: if-else-structure.js
// Description: If-else conditional structure

const temperature = 25;

if (temperature > 30) {
    console.log('It's hot outside');
} else if (temperature > 20) {
    console.log('Pleasant weather');
} else if (temperature > 10) {
    console.log('Cool weather');
} else {
    console.log('Cold weather');
}
// Output: "Pleasant weather"
```

### Nested If Statements

You can nest if statements within each other for more complex decision-making.

```javascript
// File: nested-if-statements.js
// Description: Nested conditional checks

const user = {
    name: 'John',
    age: 25,
    isMember: true,
    subscriptionType: 'premium'
};

if (user.isMember) {
    if (user.subscriptionType === 'premium') {
        console.log('Full access granted');
    } else {
        console.log('Basic access granted');
    }
} else {
    if (user.age >= 18) {
        console.log('Limited access granted');
    } else {
        console.log('Restricted access');
    }
}
// Output: "Full access granted"
```

### Professional Use Case: User Authentication

```javascript
// File: user-authentication.js
// Description: Production-ready authentication flow

class AuthService {
    constructor() {
        this.maxLoginAttempts = 3;
        this.lockoutDuration = 15 * 60 * 1000; // 15 minutes
    }

    authenticate(username, password, userRecord) {
        // Check if account is locked
        if (userRecord.lockedUntil && Date.now() < userRecord.lockedUntil) {
            const remainingTime = Math.ceil(
                (userRecord.lockedUntil - Date.now()) / 1000
            );
            return {
                success: false,
                error: `Account locked. Try again in ${remainingTime} seconds`
            };
        }

        // Validate credentials
        if (!this.verifyPassword(password, userRecord.passwordHash)) {
            return this.handleFailedAttempt(userRecord);
        }

        // Check password expiry
        if (this.isPasswordExpired(userRecord.passwordChangedAt)) {
            return {
                success: false,
                requiresPasswordChange: true,
                message: 'Password expired. Please update your password.'
            };
        }

        // Successful authentication
        return this.handleSuccessfulLogin(userRecord);
    }

    verifyPassword(inputPassword, storedHash) {
        // In production, use proper password hashing library
        return inputPassword === storedHash;
    }

    handleFailedAttempt(userRecord) {
        userRecord.failedAttempts = (userRecord.failedAttempts || 0) + 1;
        
        if (userRecord.failedAttempts >= this.maxLoginAttempts) {
            userRecord.lockedUntil = Date.now() + this.lockoutDuration;
            return {
                success: false,
                error: 'Too many failed attempts. Account locked.'
            };
        }

        const attemptsRemaining = this.maxLoginAttempts - userRecord.failedAttempts;
        return {
            success: false,
            error: `Invalid credentials. ${attemptsRemaining} attempts remaining.`
        };
    }

    handleSuccessfulLogin(userRecord) {
        userRecord.failedAttempts = 0;
        userRecord.lockedUntil = null;
        userRecord.lastLogin = Date.now();
        
        return {
            success: true,
            user: {
                id: userRecord.id,
                username: userRecord.username,
                role: userRecord.role
            }
        };
    }

    isPasswordExpired(changedAt) {
        const ninetyDays = 90 * 24 * 60 * 60 * 1000;
        return Date.now() - changedAt > ninetyDays;
    }
}

// Usage example
const authService = new AuthService();
const userRecord = {
    id: 1,
    username: 'john_doe',
    passwordHash: 'password123',
    passwordChangedAt: Date.now() - 30 * 24 * 60 * 60 * 1000,
    role: 'admin',
    failedAttempts: 0,
    lockedUntil: null
};

const result = authService.authenticate('john_doe', 'password123', userRecord);
console.log(result);
```

---

## Ternary Operators

### Basic Ternary Syntax

The ternary operator provides a concise way to write simple conditional expressions.

```javascript
// File: basic-ternary.js
// Description: Ternary operator basics

// Syntax: condition ? valueIfTrue : valueIfFalse
const age = 20;
const status = age >= 18 ? 'adult' : 'minor';
// Output: "adult"
```

### Chained Ternary Operators

Multiple ternary operators can be chained for complex conditional logic.

```javascript
// File: chained-ternary.js
// Description: Chained ternary operators

const score = 85;

const grade = score >= 90 ? 'A' 
           : score >= 80 ? 'B' 
           : score >= 70 ? 'C' 
           : score >= 60 ? 'D' 
           : 'F';

console.log(grade);
// Output: "B"
```

### Ternary with Function Calls

```javascript
// File: ternary-with-functions.js
// Description: Ternary operator with function calls

function getDiscount(memberType) {
    return memberType === 'premium' ? 0.20 
         : memberType === 'gold' ? 0.15 
         : memberType === 'silver' ? 0.10 
         : 0;
}

console.log(getDiscount('premium')); // 0.20
console.log(getDiscount('gold'));    // 0.15
console.log(getDiscount('basic'));  // 0
```

### Professional Use Case: Conditional Rendering

```javascript
// File: conditional-rendering.js
// Description: UI component with conditional rendering

class ButtonComponent {
    constructor(props) {
        this.variant = props.variant || 'primary';
        this.disabled = props.disabled || false;
        this.loading = props.loading || false;
    }

    render() {
        const baseClasses = 'btn';
        const variantClasses = {
            primary: 'btn-primary',
            secondary: 'btn-secondary',
            danger: 'btn-danger',
            success: 'btn-success'
        };
        
        const classes = [
            baseClasses,
            variantClasses[this.variant] || variantClasses.primary,
            this.disabled ? 'btn-disabled' : '',
            this.loading ? 'btn-loading' : ''
        ].filter(Boolean).join(' ');

        const content = this.loading 
            ? '<span class="spinner"></span> Loading...' 
            : this.disabled 
                ? this.props.children 
                : this.props.children;

        return `<button class="${classes}" ${this.disabled ? 'disabled' : ''}>
            ${content}
        </button>`;
    }
}

// Usage
const button = new ButtonComponent({
    variant: 'primary',
    disabled: false,
    loading: true,
    children: 'Submit'
});

console.log(button.render());
```

---

## Switch Statements

### Basic Switch Syntax

The `switch` statement provides a cleaner way to handle multiple conditional branches.

```javascript
// File: basic-switch.js
// Description: Switch statement basics

const day = new Date().getDay();
let dayName;

switch (day) {
    case 0:
        dayName = 'Sunday';
        break;
    case 1:
        dayName = 'Monday';
        break;
    case 2:
        dayName = 'Tuesday';
        break;
    case 3:
        dayName = 'Wednesday';
        break;
    case 4:
        dayName = 'Thursday';
        break;
    case 5:
        dayName = 'Friday';
        break;
    case 6:
        dayName = 'Saturday';
        break;
    default:
        dayName = 'Unknown';
}

console.log(dayName);
```

### Switch with Fall-Through

Fall-through allows multiple cases to share the same code block.

```javascript
// File: switch-fallthrough.js
// Description: Switch fall-through behavior

const month = new Date().getMonth() + 1;
let season;

switch (month) {
    case 12:
    case 1:
    case 2:
        season = 'Winter';
        break;
    case 3:
    case 4:
    case 5:
        season = 'Spring';
        break;
    case 6:
    case 7:
    case 8:
        season = 'Summer';
        break;
    case 9:
    case 10:
    case 11:
        season = 'Fall';
        break;
}

console.log(season);
```

### Switch Expression (ES2024+)

Modern JavaScript supports switch expressions that return values.

```javascript
// File: switch-expression.js
// Description: Switch expression (ES2024+)

const getStatusMessage = (status) => {
    return switch (status) {
        case 'pending' -> 'Waiting for processing'
        case 'active' -> 'Currently running'
        case 'completed' -> 'Finished successfully'
        case 'failed' -> 'Operation failed'
        case 'cancelled' -> 'Operation cancelled'
        default -> 'Unknown status'
    };
};

console.log(getStatusMessage('active')); // "Currently running"
```

### Professional Use Case: State Machine

```javascript
// File: state-machine.js
// Description: Order processing state machine

class OrderProcessor {
    constructor() {
        this.state = 'pending';
    }

    process(order) {
        switch (this.state) {
            case 'pending':
                if (this.validateOrder(order)) {
                    this.state = 'confirmed';
                    return { success: true, message: 'Order confirmed' };
                }
                return { success: false, message: 'Invalid order' };

            case 'confirmed':
                if (this.checkInventory(order)) {
                    this.state = 'processing';
                    return { success: true, message: 'Order processing' };
                }
                this.state = 'pending';
                return { success: false, message: 'Out of stock' };

            case 'processing':
                if (this.processPayment(order)) {
                    this.state = 'shipped';
                    return { success: true, message: 'Order shipped' };
                }
                return { success: false, message: 'Payment failed' };

            case 'shipped':
                if (this.confirmDelivery(order)) {
                    this.state = 'delivered';
                    return { success: true, message: 'Order delivered' };
                }
                return { success: false, message: 'Delivery pending' };

            default:
                return { success: false, message: 'Invalid state' };
        }
    }

    validateOrder(order) {
        return order && order.items && order.items.length > 0;
    }

    checkInventory(order) {
        return true; // Simplified
    }

    processPayment(order) {
        return true; // Simplified
    }

    confirmDelivery(order) {
        return true; // Simplified
    }
}
```

---

## Truthy and Falsy Values

### Understanding Falsy Values

JavaScript has specific values that evaluate to false in boolean contexts:

```javascript
// File: falsy-values.js
// Description: Falsy values in JavaScript

const falsyValues = [
    false,
    0,
    -0,
    0n,        // BigInt zero
    '',        // Empty string
    null,
    undefined,
    NaN
];

// Check each value
falsyValues.forEach(value => {
    console.log(`${value} -> ${Boolean(value)}`);
});
// All output: false
```

### Truthy Values

All values that are not falsy are truthy:

```javascript
// File: truthy-values.js
// Description: Truthy values

const truthyValues = [
    true,
    1,
    -1,
    1n,
    '0',           // Non-empty string
    'false',       // String "false" is truthy!
    [],            // Empty array
    {},            // Empty object
    function() {}  // Empty function
];

truthyValues.forEach(value => {
    console.log(`${value} -> ${Boolean(value)}`);
});
// All output: true
```

### Practical Application: Default Values

```javascript
// File: default-values.js
// Description: Using truthy/falsy for defaults

function createUser(options = {}) {
    // Using || for default values (legacy approach)
    const name = options.name || 'Anonymous';
    
    // Using ?? for nullish coalescing (better)
    const role = options.role ?? 'guest';
    
    // Combining with ternary
    const isActive = options.isActive !== undefined 
        ? options.isActive 
        : true;

    return { name, role, isActive };
}

// Usage
const user1 = createUser({ name: 'John' });
console.log(user1); // { name: 'John', role: 'guest', isActive: true }

const user2 = createUser({ name: 'Jane', role: 'admin', isActive: false });
console.log(user2); // { name: 'Jane', role: 'admin', isActive: false }
```

### Security Consideration: Falsy Value Pitfalls

```javascript
// File: security-pitfalls.js
// Description: Security issues with truthy/falsy

class PaymentProcessor {
    processPayment(amount) {
        // VULNERABLE: 0 is falsy, would allow free purchase!
        if (!amount) {
            throw new Error('Invalid amount');
        }
        
        // SECURE: Use explicit comparison
        if (typeof amount !== 'number' || amount <= 0) {
            throw new Error('Invalid amount');
        }

        // Process payment...
        return { success: true, amount };
    }
}

// Better approach using nullish coalescing
function processAmount(amount) {
    // Only use ?? when 0 is a valid value
    const value = amount ?? 0;
    return value * 1.1; // Add tax
}

console.log(processAmount(0));    // 0 (with ?? 0 is treated as valid)
console.log(processAmount(null)); // 0 (with ?? uses default)
```

---

## Advanced Patterns

### Early Return Pattern

```javascript
// File: early-return.js
// Description: Early return for cleaner code

function processRegistration(userData) {
    // Validation checks
    if (!userData) {
        return { success: false, error: 'User data required' };
    }

    if (!userData.email) {
        return { success: false, error: 'Email required' };
    }

    if (!this.isValidEmail(userData.email)) {
        return { success: false, error: 'Invalid email format' };
    }

    if (!userData.password || userData.password.length < 8) {
        return { success: false, error: 'Password too short' };
    }

    // Check if user exists
    if (this.userExists(userData.email)) {
        return { success: false, error: 'User already exists' };
    }

    // All validations passed - proceed
    return this.createUser(userData);
}
```

### Guard Clauses

```javascript
// File: guard-clauses.js
// Description: Guard clause pattern

function calculateOrderTotal(order, user) {
    // Guard clauses first
    if (!order || !order.items || order.items.length === 0) {
        return 0;
    }

    if (!user) {
        throw new Error('User required');
    }

    // Calculate base total
    let total = order.items.reduce((sum, item) => {
        return sum + (item.price * item.quantity);
    }, 0);

    // Apply discounts
    if (user.isPremium) {
        total *= 0.9; // 10% discount
    }

    if (user.rewardsPoints >= 1000) {
        total -= 50; // $50 off
    }

    // Apply shipping
    if (total < 50) {
        total += 10; // Shipping cost
    }

    return total;
}
```

---

## Best Practices

1. **Use explicit comparisons** (`===` instead of `==`)
2. **Prefer early returns** to reduce nesting
3. **Use switch for multiple conditions** on the same variable
4. **Avoid deeply nested if statements** - refactor with early returns
5. **Be careful with falsy values** in security-critical code
6. **Use ternary for simple conditions only** - use if-else for complex logic

---

## Common Pitfalls

| Pitfall | Problem | Solution |
|---------|---------|----------|
| Using `==` | Type coercion causes unexpected results | Always use `===` |
| Forgetting `break` | Fall-through causes bugs | Add break statements |
| Treating "0" as falsy | String "0" evaluates to true | Use explicit checks |
| Nested conditionals | Deep nesting hard to read | Use early returns |
| Comparing NaN | `NaN === NaN` is false | Use `Number.isNaN()` |

---

## Performance Considerations

1. **Order matters**: Place most likely conditions first in if-else chains
2. **Switch vs if-else**: Switch is typically faster for many conditions
3. **Short-circuit evaluation**: `&&` and `||` stop evaluating once result is determined
4. **Avoid unnecessary evaluations**: Check expensive conditions last

---

## Key Takeaways

- Use `if-else` for complex conditional logic
- Ternary operators are ideal for simple, one-line conditions
- Switch statements excel when comparing one value against multiple options
- Understand truthy/falsy values to avoid subtle bugs
- Prioritize code readability and maintainability
- Always use strict equality (`===`) for comparisons
- Implement early returns to keep code flat and readable

---

## Related Files

- [02_LOOPS_MASTER](./02_LOOPS_MASTER.md) - Control flow with loops
- [03_LOGICAL_OPERATORS_MASTER](./03_LOGICAL_OPERATORS_MASTER.md) - &&, ||, ! operators
- [04_JUMP_STATEMENTS_ADVANCED](./04_JUMP_STATEMENTS_ADVANCED.md) - break, continue, return
- [05_PATTERN_MATCHING](./05_PATTERN_MATCHING.md) - Modern pattern matching

---

*Last updated: 2026-04-03*