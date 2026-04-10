# 🔣 Logical Operators Mastery

## 📋 Table of Contents

1. [Overview](#overview)
2. [AND Operator (`&&`)](#and-operator-)
3. [OR Operator (`||`)](#or-operator-)
4. [NOT Operator (`!`)](#not-operator-)
5. [Nullish Coalescing (`??`)](#nullish-coalescing-)
6. [Short-Circuit Evaluation](#short-circuit-evaluation)
7. [Practical Use Cases](#practical-use-cases)
8. [Common Mistakes](#common-mistakes)
9. [Performance Considerations](#performance-considerations)
10. [Key Takeaways](#key-takeaways)

---

## Overview

Logical operators are the backbone of conditional logic in JavaScript. The AND (`&&`), OR (`||`), and NOT (`!`) operators enable you to combine boolean values and make complex decisions. Understanding short-circuit evaluation and proper usage patterns is essential for writing efficient JavaScript code.

---

## AND Operator (`&&`)

### Basic AND Operations

```javascript
// File: and-basic.js
// Description: AND operator basics

console.log(true && true);   // true
console.log(true && false);  // false
console.log(false && true);  // false
console.log(false && false); // false
```

### AND with Non-Boolean Values

The AND operator returns one of its operands:

```javascript
// File: and-non-boolean.js
// Description: AND with non-boolean values

console.log(1 && 2);    // 2 (last truthy value)
console.log(1 && 0);    // 0
console.log('a' && 'b'); // 'b'
console.log('' && 'b');  // '' (first falsy value)
console.log(null && 2); // null
```

### Chaining AND Operators

```javascript
// File: and-chaining.js
// Description: Chaining AND operators

const user = {
    name: 'John',
    age: 25,
    isActive: true,
    role: 'admin'
};

// All conditions must be true
const hasAccess = 
    user.isActive && 
    user.role === 'admin' && 
    user.age >= 18;

console.log(hasAccess); // true
```

### Professional Use Case: Permission System

```javascript
// File: permission-system.js
// Description: Permission checking with AND

class PermissionManager {
    constructor() {
        this.permissions = {
            admin: ['read', 'write', 'delete', 'manage'],
            editor: ['read', 'write'],
            viewer: ['read']
        };
    }

    hasPermission(user, action) {
        // All conditions must be met
        return (
            user &&
            user.isActive &&
            user.role &&
            this.permissions[user.role]?.includes(action)
        );
    }

    canPerformAction(user, requiredActions) {
        // User needs ALL specified permissions
        return requiredActions.every(
            action => this.hasPermission(user, action)
        );
    }
}

// Usage
const permissionManager = new PermissionManager();

const adminUser = {
    name: 'John',
    role: 'admin',
    isActive: true,
    age: 30
};

console.log(permissionManager.hasPermission(adminUser, 'delete')); // true
console.log(permissionManager.hasPermission(adminUser, 'manage')); // true
console.log(permissionManager.canPerformAction(adminUser, ['read', 'write', 'delete'])); // true
```

---

## OR Operator (`||`)

### Basic OR Operations

```javascript
// File: or-basic.js
// Description: OR operator basics

console.log(true || true);   // true
console.log(true || false);  // true
console.log(false || true); // true
console.log(false || false); // false
```

### OR with Non-Boolean Values

OR returns the first truthy value or the last value:

```javascript
// File: or-non-boolean.js
// Description: OR with non-boolean values

console.log(1 || 2);    // 1 (first truthy)
console.log(0 || 2);    // 2 (0 is falsy)
console.log('a' || 'b'); // 'a' (first truthy)
console.log('' || 'b');  // 'b'
console.log(null || 0);  // 0
```

### Default Value Pattern

The OR operator is commonly used for default values:

```javascript
// File: or-defaults.js
// Description: Default value pattern

function createUser(options = {}) {
    // Using || for defaults (careful with 0)
    const name = options.name || 'Guest';
    const age = options.age ?? 0;  // Only null/undefined
    const role = options.role || 'user';
    
    return { name, age, role };
}

console.log(createUser({})); 
// { name: 'Guest', age: 0, role: 'user' }

console.log(createUser({ name: 'John', age: 0, role: 'admin' })); 
// { name: 'John', age: 0, role: 'admin' }
```

### Chaining OR for Multiple Options

```javascript
// File: or-chaining.js
// Description: Chaining OR operators

function getConfig(config) {
    return (
        config?.environment?.apiUrl ||
        config?.apiUrl ||
        config?.defaultApiUrl ||
        'https://api.default.com'
    );
}

console.log(getConfig({})); // 'https://api.default.com'
console.log(getConfig({ apiUrl: 'custom.com' })); // 'custom.com'
console.log(getConfig({ environment: { apiUrl: 'env.com' } })); // 'env.com'
```

### Professional Use Case: Feature Flags

```javascript
// File: feature-flags.js
// Description: Feature flag system with OR

class FeatureFlagManager {
    constructor() {
        this.defaultFlags = {
            darkMode: true,
            notifications: true,
            betaFeatures: false,
            newUI: false
        };
    }

    getFlag(flagName, featureFlags = {}) {
        return (
            featureFlags[flagName] ?? 
            this.defaultFlags[flagName]
        );
    }

    isEnabled(flagName, featureFlags = {}) {
        return !!this.getFlag(flagName, featureFlags);
    }

    getAllFlags(featureFlags = {}) {
        return {
            ...this.defaultFlags,
            ...featureFlags
        };
    }
}

const flagManager = new FeatureFlagManager();

// User-specific overrides
const userFlags = {
    darkMode: false,
    betaFeatures: true
};

console.log(flagManager.getAllFlags(userFlags));
// { darkMode: false, notifications: true, betaFeatures: true, newUI: false }
```

---

## NOT Operator (`!`)

### Basic NOT Operations

```javascript
// File: not-basic.js
// Description: NOT operator basics

console.log(!true);   // false
console.log(!false);  // true
console.log(!!true); // true (double negation)
```

### NOT with Truthy/Falsy

```javascript
// File: not-truthy.js
// Description: NOT with truthy values

console.log(!0);        // true (0 is falsy)
console.log(!1);        // false (1 is truthy)
console.log(!'');      // true (empty string is falsy)
console.log(!'hello');  // false
console.log(!null);     // true (null is falsy)
console.log(!{});       // false (object is always truthy)
```

### Double Negation Pattern

Convert any value to boolean:

```javascript
// File: double-negation.js
// Description: Double negation

function toBoolean(value) {
    return !!value;
}

console.log(toBoolean(1));       // true
console.log(toBoolean(0));       // false
console.log(toBoolean(''));      // false
console.log(toBoolean('hello')); // true
console.log(toBoolean(null));   // false
console.log(toBoolean({}));    // true
```

### Professional Use Case: Toggle Components

```javascript
// File: toggle-component.js
// Description: Toggle component with NOT

class ToggleButton {
    constructor(initialState = false) {
        this.state = initialState;
    }

    toggle() {
        this.state = !this.state;
        return this.state;
    }

    enable() {
        this.state = true;
    }

    disable() {
        this.state = false;
    }

    isEnabled() {
        return !!this.state;
    }

    render() {
        const baseClass = 'toggle-button';
        const stateClass = this.state ? 'active' : 'inactive';
        return `<button class="${baseClass} ${stateClass}">
            ${this.state ? 'ON' : 'OFF'}
        </button>`;
    }
}

// Usage
const button = new ToggleButton(true);
console.log(button.toggle()); // false
console.log(button.render());
```

---

## Nullish Coalescing (`??`)

### Difference from OR

The nullish coalescing operator only considers `null` and `undefined` as nullish:

```javascript
// File: nullish-vs-or.js
// Description: ?? vs || comparison

console.log(0 ?? 'default');   // 0 (0 is not nullish)
console.log('' ?? 'default'); // '' (empty string is not nullish)
console.log(false ?? 'default'); // false

console.log(0 || 'default');   // 'default'
console.log('' || 'default');  // 'default'
console.log(false || 'default'); // 'default'
```

### When to Use Nullish Coalescing

```javascript
// File: nullish-usage.js
// Description: Nullish coalescing practical use

function processValue(value) {
    // Use ?? when 0, '', false are valid values
    const processed = value ?? 'no value provided';
    return processed;
}

console.log(processValue(0));     // 0 (valid)
console.log(processValue(''));    // '' (valid)
console.log(processValue(false)); // false (valid)
console.log(processValue(null)); // 'no value provided'
console.log(processValue(undefined)); // 'no value provided'
```

### Professional Use Case: Configuration

```javascript
// File: config-handler.js
// Description: Configuration handler

class ConfigHandler {
    constructor(defaults = {}) {
        this.defaults = defaults;
    }

    get(path, userConfig = {}) {
        // Navigate nested path
        const value = path.split('.').reduce(
            (obj, key) => obj?.[key],
            userConfig
        );
        
        // Return value if not nullish, otherwise use default
        const defaultValue = path.split('.').reduce(
            (obj, key) => obj?.[key],
            this.defaults
        );
        
        return value ?? defaultValue;
    }

    getNumber(path, userConfig = {}) {
        const value = this.get(path, userConfig);
        return typeof value === 'number' ? value : 0;
    }

    getBoolean(path, userConfig = {}) {
        const value = this.get(path, userConfig);
        return !!value;
    }
}

// Usage
const config = new ConfigHandler({
    app: {
        port: 3000,
        debug: false,
        name: 'MyApp'
    }
});

console.log(config.get('app.port'));         // 3000
console.log(config.get('app.unknown'));       // undefined, falls back to default
console.log(config.getBoolean('app.debug')); // false (preserved)
```

---

## Short-Circuit Evaluation

### AND Short-Circuit

With AND, if the first operand is falsy, the second is never evaluated:

```javascript
// File: short-circuit-and.js
// Description: AND short-circuit evaluation

let counter = 0;

function first() {
    counter++;
    return false; // Falsy
}

function second() {
    counter++;
    return true;
}

// First is evaluated, returns false immediately
const result = first() && second();

console.log(counter); // 1 (second was never called)
console.log(result);  // false
```

### OR Short-Circuit

With OR, if the first operand is truthy, the second is never evaluated:

```javascript
// File: short-circuit-or.js
// Description: OR short-circuit evaluation

let counter = 0;

function first() {
    counter++;
    return true; // Truthy
}

function second() {
    counter++;
    return "evaluated";
}

// First is evaluated, returns true immediately
const result = first() || second();

console.log(counter); // 1 (second was never called)
console.log(result);  // true
```

### Performance Implications

Use short-circuit for expensive operations:

```javascript
// File: short-circuit-performance.js
// Description: Performance optimization

let expensiveCalls = 0;

function expensiveCheck() {
    expensiveCalls++;
    return false;
}

// BAD: Always calls expensive function
if (false && expensiveCheck()) {
    console.log('Never runs');
}

// GOOD: Short-circuits immediately
if (false && expensiveCheck()) {
    console.log('Never runs');
}

console.log(expensiveCalls); // 0 (not called)
```

### Safety Checking Pattern

```javascript
// File: safety-check.js
// Description: Safety checks with short-circuit

const user = null;

// Safe property access
const city = user?.address?.city ?? 'Unknown';

// This won't throw because second part is never evaluated
const result = user && user.name && user.name.toUpperCase();

console.log(city);   // 'Unknown'
console.log(result);  // null (not undefined)
```

---

## Practical Use Cases

### 1. Conditional Function Execution

```javascript
// File: conditional-execution.js
// Description: Conditionally execute functions

function executeIfValid(value, fn) {
    return value && fn(value);
}

const result = executeIfValid(5, x => x * 2);
console.log(result); // 10

const nullResult = executeIfValid(null, x => x * 2);
console.log(nullResult); // null
```

### 2. Multiple Fallbacks

```javascript
// File: multiple-fallbacks.js
// Description: Multiple fallback values

function getApiUrl(config) {
    return (
        config?.environment?.apiUrl ||
        config?.apiUrl ||
        config?.defaultApiUrl ||
        process.env.API_URL ||
        'https://api.example.com'
    );
}
```

### 3. Chained Conditions

```javascript
// File: chained-conditions.js
// Description: Complex condition chains

function canUpgrade(user) {
    return (
        user &&
        user.isActive &&
        !user.isLocked &&
        user.subscription === 'premium' &&
        user.usageHours < user.allowedHours
    );
}
```

### 4. Guard Clauses

```javascript
// File: guard-clauses.js
// Description: Guard clause pattern

function processOrder(order) {
    // Guard clauses
    if (!order || !order.items?.length) return { error: 'Empty order' };
    if (!order.user) return { error: 'User required' };
    if (order.status !== 'pending') return { error: 'Invalid status' };
    
    // Main logic
    return { success: true, order };
}
```

### 5. Required Field Validation

```javascript
// File: field-validation.js
// Description: Required field validation

function validate(obj, requiredFields) {
    const missing = requiredFields.filter(
        field => !obj[field]
    );
    
    if (missing.length > 0) {
        return { valid: false, missing };
    }
    
    return { valid: true };
}
```

---

## Common Mistakes

| Mistake | Problem | Solution |
|--------|---------|----------|
| Using `\|\|` for 0 | 0 gets replaced | Use `??` |
| Forgetting short-circuit | Expensive functions called | Use logical operators |
| Comparing with `!==` | Confusion with NOT | Use explicit boolean conversion |
| Chaining too many | Hard to read | Extract to named variables |
| NOT vs `!==` | != vs !== | Use strict inequality |

---

## Performance Considerations

1. **Short-circuit is fast**: JavaScript stops evaluating once outcome is determined
2. **Prefer simple operations**: Complex chains can be harder to optimize
3. **Avoid redundant checks**: Don't check the same condition twice
4. **Use `??` for numeric defaults**: Avoids replacing valid 0

---

## Key Takeaways

- `&&` returns the last value if all are truthy, first falsy otherwise
- `||` returns first truthy value or last value
- `!` converts to boolean and negates
- `??` only treats `null` and `undefined` as nullish
- Short-circuit evaluation saves performance
- Use logical operators to avoid explicit if statements

---

## Related Files

- [01_IF_ELSE_CONDITIONAL_STATEMENTS](./01_IF_ELSE_CONDITIONAL_STATEMENTS.md) - Conditional logic
- [02_LOOPS_MASTER](./02_LOOPS_MASTER.md) - Loop patterns
- [04_JUMP_STATEMENTS_ADVANCED](./04_JUMP_STATEMENTS_ADVANCED.md) - Jump statements
- [05_PATTERN_MATCHING](./05_PATTERN_MATCHING.md) - Modern alternatives

---

*Last updated: 2026-04-03*