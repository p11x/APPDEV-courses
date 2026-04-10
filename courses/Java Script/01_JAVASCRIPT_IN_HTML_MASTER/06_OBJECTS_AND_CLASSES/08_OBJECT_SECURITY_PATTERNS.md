# JavaScript Object Security Patterns: Complete Mastery Guide

Object security is critical for building robust JavaScript applications. Understanding how to protect objects from mutation, prevent prototype pollution, and implement secure patterns is essential for writing safe, maintainable code. This comprehensive guide covers Object.freeze, Object.seal, property protection, preventing object modification, and security best practices.

---

## Table of Contents

1. [Understanding Object Mutability](#understanding-object-mutability)
2. [Object.freeze() Deep Dive](#objectfreeze-deep-dive)
3. [Object.seal() and Object.preventExtensions()](#objectseal-and-objectpreventextensions)
4. [Property Descriptors for Protection](#property-descriptors-for-protection)
5. [Private Fields and Encapsulation](#private-fields-and-encapsulation)
6. [Prototype Pollution Prevention](#prototype-pollution-prevention)
7. [Deep Protection Patterns](#deep-protection-patterns)
8. [Professional Use Cases](#professional-use-cases)
9. [Key Takeaways](#key-takeaways)
10. [Common Pitfalls](#common-pilfalls)
11. [Related Files](#related-files)

---

## Understanding Object Mutability

JavaScript objects are mutable by default, which can lead to accidental modifications and security vulnerabilities.

### Default Mutability

```javascript
// Objects are mutable by default
const user = { name: 'Alice' };
user.name = 'Bob';     // Allowed
user.age = 30;         // Allowed
delete user.name;      // Allowed
console.log(user);     // { age: 30 }

// Comparison with primitives
let name = 'Alice';
name = 'Bob';          // Reassignment, not mutation - primitives are immutable
```

### Reference Behavior Implications

```javascript
// Reference behavior can cause unintended mutations
const original = { name: 'Alice', settings: { theme: 'dark' } };

// Shallow copy - nested objects still share reference
const copy = { ...original };
copy.name = 'Bob';
copy.settings.theme = 'light';

console.log(original.name);        // 'Alice' - unchanged
console.log(original.settings.theme);  // 'light' - MUTATED!

// Correct deep copy
const trueCopy = structuredClone(original);
trueCopy.settings.theme = 'blue';
console.log(original.settings.theme);  // 'light' - unchanged
```

### Mutation Detection

```javascript
// Detecting mutations
function createImmutable(data) {
    const original = JSON.stringify(data);
    
    return new Proxy(data, {
        set(target, property, value) {
            console.error(`Mutation attempt: setting ${property} to ${value}`);
            return false;  // Reject mutation
        },
        deleteProperty(target, property) {
            console.error(`Mutation attempt: deleting ${property}`);
            return false;
        }
    });
}

const safe = createImmutable({ a: 1 });
try {
    safe.a = 2;  // Will log error
} catch (e) {
    console.log('Mutation blocked');
}
```

---

## Object.freeze() Deep Dive

Object.freeze() provides the strongest level of object protection, preventing all modifications.

### Basic freeze() Usage

```javascript
// Object.freeze() - complete immutability
const frozen = Object.freeze({ a: 1, b: 2 });

// Cannot modify existing properties
frozen.a = 3;           // Silently fails in non-strict, throws in strict
console.log(frozen.a); // 1

// Cannot add new properties
frozen.c = 4;          // Silently fails
console.log(frozen.c); // undefined

// Cannot delete properties
delete frozen.a;       // Silently fails
console.log(frozen.a); // 1

// Verify frozen status
console.log(Object.isFrozen(frozen));  // true
```

### What freeze() Prevents

```javascript
// What freeze() prevents
const config = Object.freeze({
    apiUrl: 'https://api.example.com',
    timeout: 5000
});

// These operations are prevented:
config.apiUrl = 'other';       // Value change - blocked
config.newProp = 'value';      // Property addition - blocked
delete config.apiUrl;          // Property deletion - blocked

// But nested objects are NOT frozen!
const nested = Object.freeze({
    settings: { theme: 'dark' }  // Only top-level is frozen
});

nested.settings.theme = 'light';  // Allowed - nested not frozen!
console.log(nested.settings.theme);  // 'light'

// strict mode throws
'use strict';
try {
    frozen.a = 3;
} catch (e) {
    console.log('TypeError:', e.message);  // Cannot assign to read only property
}
```

### Freeze Check Patterns

```javascript
// Checking if objects are frozen
const obj1 = { a: 1 };
const obj2 = Object.freeze({ a: 1 });

console.log(Object.isExtensible(obj1));  // true
console.log(Object.isExtensible(obj2));  // false
console.log(Object.isSealed(obj1));     // false
console.log(Object.isSealed(obj2));     // true
console.log(Object.isFrozen(obj1));     // false
console.log(Object.isFrozen(obj2));     // true
```

---

## Object.seal() and Object.preventExtensions()

These methods provide intermediate levels of protection.

### Object.seal()

```javascript
// Object.seal() - prevents add/delete, allows modifications
const sealed = Object.seal({ a: 1, b: 2 });

// Can modify existing properties
sealed.a = 100;
console.log(sealed.a);  // 100

// Cannot add new properties
sealed.c = 3;           // Silently fails
console.log(sealed.c);  // undefined

// Cannot delete properties
delete sealed.a;       // Silently fails
console.log(sealed.a);  // 100

// Verify sealed status
console.log(Object.isSealed(sealed));  // true
```

### Object.preventExtensions()

```javascript
// Object.preventExtensions() - only prevents additions
const nonExtensible = { a: 1 };
Object.preventExtensions(nonExtensible);

// Can modify existing properties
nonExtensible.a = 2;
console.log(nonExtensible.a);  // 2

// Cannot add new properties
nonExtensible.b = 3;
console.log(nonExtensible.b); // undefined

// Can delete existing properties
delete nonExtensible.a;
console.log(nonExtensible.a); // undefined

// Verify
console.log(Object.isExtensible(nonExtensible));  // false
```

### Protection Level Comparison

```javascript
// Compare protection levels
const obj = { a: 1 };

console.log('\n--- Original ---');
console.log('isExtensible:', Object.isExtensible(obj));
console.log('isSealed:', Object.isSealed(obj));
console.log('isFrozen:', Object.isFrozen(obj));

Object.preventExtensions(obj);
console.log('\n--- After preventExtensions ---');
console.log('isExtensible:', Object.isExtensible(obj));
console.log('isSealed:', Object.isSealed(obj));
console.log('isFrozen:', Object.isFrozen(obj));

Object.seal(obj);
console.log('\n--- After seal ---');
console.log('isExtensible:', Object.isExtensible(obj));
console.log('isSealed:', Object.isSealed(obj));
console.log('isFrozen:', Object.isFrozen(obj));

Object.freeze(obj);
console.log('\n--- After freeze ---');
console.log('isExtensible:', Object.isExtensible(obj));
console.log('isSealed:', Object.isSealed(obj));
console.log('isFrozen:', Object.isFrozen(obj));
```

---

## Property Descriptors for Protection

Property descriptors provide fine-grained control over individual properties.

### Read-Only Properties

```javascript
// Creating read-only properties
const user = {};

Object.defineProperty(user, 'id', {
    value: 1,
    writable: false,     // Cannot change
    enumerable: true,
    configurable: false // Cannot delete or reconfigure
});

console.log(user.id);   // 1
user.id = 2;            // Silently fails (strict: throws)
console.log(user.id);   // 1
```

### Non-Configurable Properties

```javascript
// Non-configurable properties cannot be deleted or reconfigured
const config = {
    env: 'production'
};

Object.defineProperty(config, 'env', {
    configurable: false  // Lock property
});

// Cannot reconfigure
try {
    Object.defineProperty(config, 'env', { value: 'development' });
} catch (e) {
    console.log('Cannot reconfigure non-configurable property');
}

// Cannot delete
delete config.env;
console.log(config.env);  // 'production'
```

### Non-Enumerable Properties

```javascript
// Non-enumerable properties hidden from iteration
const user = {
    name: 'Alice'
};

Object.defineProperty(user, 'ssn', {
    value: '123-45-6789',
    enumerable: false,  // Hidden from Object.keys(), for...in
    configurable: false
});

console.log(Object.keys(user));   // ['name'] - ssn hidden
console.log(user.ssn);            // '123-45-6789' - accessible

for (const key in user) {
    console.log(key);  // 'name' - ssn not iterated
}
```

### Complete Property Protection

```javascript
// Fully protected property
function createProtectedProperty(obj, key, value) {
    Object.defineProperty(obj, key, {
        value: value,
        writable: false,
        enumerable: true,
        configurable: false
    });
    return obj;
}

const user = createProtectedProperty({}, 'id', 1);
user.id = 2;         // Fails
delete user.id;      // Fails
console.log(user.id);  // 1
```

---

## Private Fields and Encapsulation

ES2022 private fields (#) provide true encapsulation for classes.

### Private Fields

```javascript
// Private class fields
class BankAccount {
    #balance;
    #transactions = [];
    
    constructor(initialBalance = 0) {
        if (initialBalance < 0) {
            throw new Error('Initial balance cannot be negative');
        }
        this.#balance = initialBalance;
    }
    
    get balance() {
        return this.#balance;
    }
    
    deposit(amount) {
        if (amount <= 0) throw new Error('Amount must be positive');
        this.#balance += amount;
        this.#transactions.push({ type: 'deposit', amount });
        return this.#balance;
    }
    
    withdraw(amount) {
        if (amount > this.#balance) throw new Error('Insufficient funds');
        this.#balance -= amount;
        this.#transactions.push({ type: 'withdraw', amount });
        return this.#balance;
    }
}

const account = new BankAccount(1000);
console.log(account.balance);  // 1000

// Cannot access private fields directly
// console.log(account.#balance);  // SyntaxError: Private field
```

### Private Methods

```javascript
// Private methods
class SecureProcessor {
    #validateInput(data) {
        if (!data || typeof data !== 'object') {
            throw new Error('Invalid input');
        }
        return true;
    }
    
    #sanitize(data) {
        return { ...data };  // Create copy
    }
    
    #process(data) {
        return { ...data, processed: true };
    }
    
    process(input) {
        this.#validateInput(input);
        const sanitized = this.#sanitize(input);
        return this.#process(sanitized);
    }
}

const processor = new SecureProcessor();
console.log(processor.process({ value: 123 }));  // { value: 123, processed: true }

// Cannot call private methods
// processor.#validateInput({});  // SyntaxError
```

### Private Getters

```javascript
// Private getters for computed values
class Config {
    #data = {};
    
    constructor(config) {
        this.#data = config;
    }
    
    get #parsed() {
        return Object.freeze(JSON.parse(JSON.stringify(this.#data)));
    }
    
    get safeCopy() {
        return this.#parsed;
    }
    
    update(key, value) {
        this.#data[key] = value;
    }
}

const config = new Config({ theme: 'dark', debug: true });
const copy = config.safeCopy;
copy.theme = 'light';  // Fails - copy is frozen
console.log(copy.theme);  // 'dark'
```

### Static Private Fields

```javascript
// Static private fields
class Singleton {
    static #instance = null;
    #data = {};
    
    constructor() {
        if (Singleton.#instance) {
            throw new Error('Use getInstance() to get the instance');
        }
    }
    
    static getInstance() {
        if (!Singleton.#instance) {
            Singleton.#instance = new Singleton();
        }
        return Singleton.#instance;
    }
    
    setData(key, value) {
        this.#data[key] = value;
    }
    
    getData(key) {
        return this.#data[key];
    }
}

const instance1 = Singleton.getInstance();
const instance2 = Singleton.getInstance();
console.log(instance1 === instance2);  // true
```

---

## Prototype Pollution Prevention

Prototype pollution is a serious security vulnerability where attackers inject properties into Object.prototype, affecting all objects.

### Understanding Prototype Pollution

```javascript
// Dangerous vulnerable merge
function vulnerableMerge(target, source) {
    for (const key in source) {
        if (source[key] && typeof source[key] === 'object') {
            target[key] = target[key] || {};
            vulnerableMerge(target[key], source[key]);
        } else {
            target[key] = source[key];
        }
    }
    return target;
}

// Attack payload
const user = { name: 'Alice' };
vulnerableMerge(user, JSON.parse('{"__proto__": {"isAdmin": true}}'));

console.log(user.isAdmin);  // undefined - not directly on user

// But now ALL objects have isAdmin!
console.log({}.isAdmin);    // true - prototype polluted!
```

### Safe Merge Implementation

```javascript
// Safe merge - blocks prototype pollution
function safeMerge(target, source) {
    const forbidden = ['__proto__', 'constructor', 'prototype'];
    
    for (const key of Object.keys(source)) {
        if (forbidden.includes(key)) {
            console.warn(`Blocked dangerous key: ${key}`);
            continue;
        }
        
        if (source[key] && typeof source[key] === 'object') {
            if (!target[key] || typeof target[key] !== 'object') {
                target[key] = {};
            }
            target[key] = safeMerge(target[key], source[key]);
        } else {
            target[key] = source[key];
        }
    }
    
    return target;
}

// Test the fix
const user = { name: 'Alice' };
safeMerge(user, JSON.parse('{"__proto__": {"isAdmin": true}}'));

console.log(user.isAdmin);  // undefined - safe
console.log({}.isAdmin);    // undefined - not polluted
```

### Creating Objects Without Prototypes

```javascript
// Object.create(null) - no prototype chain
const safeObject = Object.create(null);
safeObject.key = 'value';

// No inherited properties or methods
console.log(safeObject.toString);     // undefined
console.log(safeObject.valueOf);       // undefined
safeObject['__proto__'] = 'pollution';
console.log(safeObject.__proto__);     // 'pollution' - just a property, not prototype

// Safer than using {}
const regular = {};
regular['__proto__'] = { evil: true };
console.log({}.evil);    // true - prototype polluted!

// Verify
console.log(Object.getPrototypeOf(safeObject));  // null
console.log(Object.getPrototypeOf(regular));    // Object.prototype
```

### Validating Object Keys

```javascript
// Key validation pattern
function createSecureObject(data, allowedKeys = []) {
    const allowed = new Set(allowedKeys);
    const result = Object.create(null);
    
    for (const [key, value] of Object.entries(data)) {
        if (key === '__proto__' || key === 'constructor' || key === 'prototype') {
            console.warn(`Rejecting dangerous key: ${key}`);
            continue;
        }
        
        if (allowed.size > 0 && !allowed.has(key)) {
            console.warn(`Rejecting unknown key: ${key}`);
            continue;
        }
        
        result[key] = value;
    }
    
    return Object.freeze(result);
}

const config = createSecureObject(
    { theme: 'dark', timeout: 5000, __proto__: { evil: true } },
    ['theme', 'timeout']
);

console.log(config.theme);    // 'dark'
console.log(config.evil);      // undefined - protected
console.log(Object.keys(config));  // ['theme', 'timeout']
```

---

## Deep Protection Patterns

Protecting nested objects requires recursive approaches.

### Deep Freeze

```javascript
// Recursive deep freeze
function deepFreeze(obj) {
    if (obj === null || typeof obj !== 'object') {
        return obj;
    }
    
    if (Array.isArray(obj)) {
        obj.forEach(item => deepFreeze(item));
        return Object.freeze(obj);
    }
    
    Object.keys(obj).forEach(key => {
        deepFreeze(obj[key]);
    });
    
    return Object.freeze(obj);
}

// Usage
const config = {
    api: {
        baseUrl: 'https://api.example.com',
        endpoints: {
            users: '/users',
            posts: '/posts'
        }
    },
    features: {
        darkMode: true,
        beta: false
    }
};

deepFreeze(config);

// Cannot modify nested objects
config.api.baseUrl = 'other';       // Fails
config.api.endpoints.users = '/u';  // Fails
config.features.darkMode = false;   // Fails

console.log(config.api.baseUrl);    // 'https://api.example.com'
```

### Deep Seal

```javascript
// Deep seal with configurable options
function deepSeal(obj, options = {}) {
    const { allowNewProps = false } = options;
    
    if (obj === null || typeof obj !== 'object') {
        return obj;
    }
    
    Object.keys(obj).forEach(key => {
        deepSeal(obj[key], options);
    });
    
    if (allowNewProps) {
        return Object.seal(obj);
    }
    
    return Object.freeze(obj);
}

const data = {
    users: [
        { name: 'Alice', settings: { theme: 'dark' } },
        { name: 'Bob', settings: { theme: 'light' } }
    ],
    metadata: { count: 2 }
};

deepSeal(data);

// Cannot delete existing
delete data.metadata;    // Fails
delete data.users[0];    // Fails

// Can still modify existing values
data.metadata.count = 3;  // Works
data.users[0].name = 'Carol';  // Works - nested not frozen, only sealed

console.log(data);
```

### Immutable Update Pattern

```javascript
// Immutable updates with deep freeze
function immutableSet(obj, path, value) {
    const keys = path.split('.');
    const clone = structuredClone ? structuredClone(obj) : JSON.parse(JSON.stringify(obj));
    
    let current = clone;
    for (let i = 0; i < keys.length - 1; i++) {
        current[keys[i]] = current[keys[i]] || {};
        current = current[keys[i]];
    }
    
    current[keys[keys.length - 1]] = value;
    return Object.freeze(clone);
}

// Usage
const state = Object.freeze({
    user: { name: 'Alice', settings: { theme: 'dark' } },
    items: []
});

const newState = immutableSet(state, 'user.settings.theme', 'light');

console.log(state.user.settings.theme);  // 'dark' - unchanged
console.log(newState.user.settings.theme); // 'light' - new state
console.log(state === newState);         // false - new object
```

### Validation with Protection

```javascript
// Protected config with validation
function createValidatedConfig(schema, defaults) {
    const data = { ...defaults };
    
    const handler = {
        set(target, property, value) {
            const validator = schema[property];
            
            if (!validator) {
                throw new Error(`Unknown property: ${property}`);
            }
            
            if (!validator.validate(value)) {
                throw new Error(validator.message || `Invalid value for ${property}`);
            }
            
            target[property] = validator.transform ? validator.transform(value) : value;
            return true;
        }
    };
    
    return new Proxy(data, handler);
}

const configSchema = {
    port: {
        validate: (v) => typeof v === 'number' && v > 0 && v < 65536,
        transform: (v) => Math.floor(v)
    },
    timeout: {
        validate: (v) => typeof v === 'number' && v > 0,
        message: 'Timeout must be positive'
    },
    mode: {
        validate: (v) => ['development', 'production', 'test'].includes(v)
    }
};

const config = createValidatedConfig(configSchema, {
    port: 3000,
    timeout: 5000,
    mode: 'development'
});

config.port = 8080;
console.log(config.port);  // 8080

config.mode = 'invalid';    // Error: Invalid value for mode
```

---

## Professional Use Cases

### Use Case 1: Secure Configuration Store

```javascript
// Secure configuration with deep protection
class SecureConfig {
    #frozenConfig;
    
    constructor(schema, defaults) {
        this.#frozenConfig = Object.freeze(this.#validateAndBuild(schema, defaults));
    }
    
    #validateAndBuild(schema, defaults) {
        const config = {};
        
        for (const [key, defaultValue] of Object.entries(defaults)) {
            const validator = schema[key];
            
            if (!validator) {
                config[key] = defaultValue;
                continue;
            }
            
            const valid = validator.validate(defaultValue);
            if (!valid) {
                throw new Error(`Invalid default value for ${key}`);
            }
            
            config[key] = validator.transform 
                ? validator.transform(defaultValue) 
                : defaultValue;
        }
        
        return config;
    }
    
    get(key) {
        if (!(key in this.#frozenConfig)) {
            throw new Error(`Unknown config key: ${key}`);
        }
        return this.#frozenConfig[key];
    }
    
    getAll() {
        return { ...this.#frozenConfig };
    }
    
    isFrozen() {
        return Object.isFrozen(this.#frozenConfig);
    }
}

const config = new SecureConfig(
    {
        apiUrl: {
            validate: (v) => typeof v === 'string' && v.startsWith('https://')
        },
        maxRetries: {
            validate: (v) => typeof v === 'number' && v >= 0 && v <= 10,
            transform: (v) => Math.floor(v)
        },
        debug: {
            validate: (v) => typeof v === 'boolean'
        }
    },
    {
        apiUrl: 'https://api.example.com',
        maxRetries: 3,
        debug: false
    }
);

console.log(config.get('apiUrl'));   // 'https://api.example.com'
console.log(config.isFrozen());      // true

try {
    config.get('unknown');           // Error
} catch (e) {
    console.log('Blocked:', e.message);
}
```

### Use Case 2: Protected State Management

```javascript
// Immutable state store with protection
class ImmutableStore {
    #state;
    #history = [];
    #maxHistory;
    
    constructor(initialState = {}, maxHistory = 50) {
        this.#maxHistory = maxHistory;
        this.#state = Object.freeze(this.#deepClone(initialState));
    }
    
    #deepClone(obj) {
        return structuredClone 
            ? structuredClone(obj) 
            : JSON.parse(JSON.stringify(obj));
    }
    
    #deepFreeze(obj) {
        if (obj === null || typeof obj !== 'object') return obj;
        
        if (Array.isArray(obj)) {
            return Object.freeze(obj.map(item => this.#deepFreeze(item)));
        }
        
        const frozen = {};
        for (const [key, value] of Object.entries(obj)) {
            frozen[key] = this.#deepFreeze(value);
        }
        
        return Object.freeze(frozen);
    }
    
    getState() {
        return this.#state;
    }
    
    setState(updater) {
        if (typeof updater !== 'function' && typeof updater !== 'object') {
            throw new Error('Updater must be function or object');
        }
        
        const previousState = this.#state;
        const newStateData = typeof updater === 'function' 
            ? updater(this.#state)
            : { ...this.#state, ...updater };
        
        if (JSON.stringify(previousState) === JSON.stringify(newStateData)) {
            return this;
        }
        
        this.#history.push(Object.freeze(previousState));
        if (this.#history.length > this.#maxHistory) {
            this.#history.shift();
        }
        
        this.#state = this.#deepFreeze(newStateData);
        return this;
    }
    
    undo() {
        if (this.#history.length === 0) return null;
        
        const previous = this.#history.pop();
        this.#state = previous;
        return this;
    }
    
    canUndo() {
        return this.#history.length > 0;
    }
}

// Usage
const store = new ImmutableStore({ user: { name: 'Alice', age: 30 } });

store.setState({ theme: 'dark' });
console.log(store.getState());  // { user: {...}, theme: 'dark' }

// Direct mutation attempts fail silently
const state = store.getState();
state.theme = 'light';  // Fails - frozen
console.log(store.getState().theme);  // 'dark'
```

### Use Case 3: API Response Sanitization

```javascript
// Sanitize API responses
function sanitizeResponse(data, options = {}) {
    const {
        removeNull = true,
        removeUndefined = true,
        maxDepth = 10,
        allowedTypes = ['string', 'number', 'boolean', 'object', 'array'],
        stripFunctions = true,
        freezeResult = true
    } = options;
    
    function sanitize(value, depth = 0) {
        if (depth > maxDepth) {
            return '[max depth exceeded]';
        }
        
        if (value === null) {
            return removeNull ? undefined : value;
        }
        
        if (value === undefined) {
            return removeUndefined ? undefined : value;
        }
        
        const type = typeof value;
        
        if (!allowedTypes.includes(type)) {
            return stripFunctions && type === 'function' ? undefined : value;
        }
        
        if (type === 'object') {
            if (Array.isArray(value)) {
                return value.map(item => sanitize(item, depth + 1));
            }
            
            const result = {};
            for (const [key, val] of Object.entries(value)) {
                if (key === '__proto__' || key === 'constructor' || key === 'prototype') {
                    continue;
                }
                result[key] = sanitize(val, depth + 1);
            }
            
            return result;
        }
        
        return value;
    }
    
    const result = sanitize(data);
    return freezeResult ? Object.freeze(result) : result;
}

// Usage
const response = {
    user: {
        name: 'Alice',
        email: 'alice@example.com',
        password: 'secret123',
        __proto__: { isAdmin: true }
    },
    meta: {
        timestamp: new Date(),
        callback: function() {}
    }
};

const sanitized = sanitizeResponse(response, {
    stripFunctions: true,
    freezeResult: true
});

console.log(sanitized.user.password);  // undefined - stripped
console.log(sanitized.meta.callback);   // undefined - function stripped
console.log(sanitized.user.isAdmin);    // undefined - proto pollution prevented
console.log(Object.isFrozen(sanitized));  // true
```

---

## Key Takeaways

1. **Object.freeze()** provides strongest protection - prevents all mutations
2. **Object.seal()** prevents add/delete but allows modifications
3. **Object.preventExtensions()** only prevents adding new properties
4. **Property descriptors** provide fine-grained control
5. **Private fields (#)** give true encapsulation in classes
6. **Prototype pollution** can be prevented with key validation
7. **Deep protection** requires recursive application
8. **Immutability** enables predictable state management

---

## Common Pitfalls

1. **Shallow freeze only**: Nested objects remain mutable
2. **Forgetting strict mode**: Silent failures instead of errors
3. **Confusing seal and freeze**: Different protection levels
4. **Not validating object keys**: Leads to prototype pollution
5. **Private fields are hard**: Requires class syntax
6. **Frozen objects can't be unsealed**: Permanent protection
7. **Mutation attempts return silently**: No errors without strict mode
8. **JSON.parse of untrusted input**: Can cause prototype pollution

---

## Related Files

- **01_OBJECT_LITERALS_AND_CREATION.md**: Object creation patterns
- **02_PROTOTYPES_DEEP_DIVE.md**: Prototype chain internals
- **04_ECMASCRIPT_CLASSES_SYNTAX.md**: Class syntax and private fields
- **05_GETTERS_SETTERS_AND_PROPERTIES.md**: Property descriptors
- **06_OBJECT_METHODS_AND_UTILITIES.md**: Object methods
- **07_INHERITANCE_PATTERNS.md**: Inheritance patterns