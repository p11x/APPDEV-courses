# JavaScript Objects and Properties: Complete Mastery Guide

Objects are the foundational data structure in JavaScript, serving as containers for key-value pairs and the basis for classes, modules, and complex data structures. Mastery of objects is essential for any JavaScript developer. This guide covers object creation, property manipulation, property descriptors, dynamic properties, and professional patterns.

---

## Table of Contents

1. [Object Fundamentals](#object-fundamentals)
2. [Object Creation Methods](#object-creation-methods)
3. [Property Manipulation](#property-manipulation)
4. [Property Descriptors](#property-descriptors)
5. [Dynamic Properties](#dynamic-properties)
6. [Object Methods and Operations](#object-methods-and-operations)
7. [Professional Patterns](#professional-patterns)
8. [Key Takeaways](#key-takeaways)
9. [Common Pitfalls](#common-pitfalls)
10. [Related Files](#related-files)

---

## Object Fundamentals

Objects in JavaScript are collections of properties, where each property is a key-value pair. Properties can be added, modified, or deleted at runtime. Objects serve as the backbone for JSON data, class definitions, and many design patterns.

### Understanding Object Structure

Every JavaScript object is essentially a hash map with string or Symbol keys. Unlike arrays, objects are optimized for key-based access rather than sequential iteration.

```javascript
// Basic object creation
const person = {
    name: 'Alice',
    age: 30,
    isActive: true
};

// Accessing properties
console.log(person.name);     // 'Alice' - dot notation
console.log(person['age']); // 30 - bracket notation
console.log(person.job);    // undefined - non-existent property

// Property assignment
person.job = 'Engineer';
person['location'] = 'New York';
console.log(person);
// { name: 'Alice', age: 30, isActive: true, job: 'Engineer', location: 'New York' }
```

### Object Type Behavior

Objects are reference types, meaning variables hold references to the object in memory, not the object itself. Understanding this distinction is crucial for avoiding subtle bugs.

```javascript
// Reference type behavior
const original = { value: 1 };
const reference = original;
reference.value = 2;
console.log(original.value);  // 2 - original is modified!

// Correct copying
const copy = { ...original };  // spread operator
const copy2 = Object.assign({}, original);
const copy3 = structuredClone(original);  // deep copy

// Object comparison
const a = { x: 1 };
const b = { x: 1 };
console.log(a === b);  // false - different references!
console.log(a == b);   // false

// Use explicit comparison for values
function objectsEqual(obj1, obj2) {
    return JSON.stringify(obj1) === JSON.stringify(obj2);
}
```

---

## Object Creation Methods

JavaScript provides multiple ways to create objects, each suited to different scenarios.

### Object Literal

The most common and recommended approach for most use cases.

```javascript
// Basic literal
const empty = {};
const withProps = { a: 1, b: 2 };

// Computed property names
const key = 'dynamicKey';
const obj = {
    [key]: 'value',
    [`prefix_${key}`]: 'prefixed value'
};

// Shorthand property names
const name = 'Alice';
const age = 30;
const shorthand = { name, age };  // { name: 'Alice', age: 30 }

// Shorthand method names
const calculator = {
    add(a, b) {
        return a + b;
    },
    subtract(a, b) {
        return a - b;
    }
};
```

### Object Constructor

The Object constructor can be used explicitly but is generally not recommended for simple object creation.

```javascript
// Object constructor
const usingNew = new Object();
const withProps = new Object({ a: 1, b: 2 });

// Difference from literal
const literal = { a: 1 };
const constructor = new Object({ a: 1 });
console.log(literal.constructor === constructor.constructor);  // true

// Object.create for prototypal inheritance
const proto = {
    greet() {
        return `Hello, I'm ${this.name}`;
    }
};

const child = Object.create(proto);
child.name = 'Alice';
console.log(child.greet());  // "Hello, I'm Alice"
console.log(child.__proto__ === proto);  // true
```

### Factory Functions

Factory functions provide a flexible pattern for creating objects with private state.

```javascript
// Factory function for creating objects
function createPerson(name, age) {
    let _privateAge = age;  // Private variable
    
    return {
        get name() {
            return name;
        },
        set name(value) {
            name = value;
        },
        get age() {
            return _privateAge;
        },
        set age(value) {
            if (value >= 0) {
                _privateAge = value;
            }
        },
        introduce() {
            return `Hi, I'm ${name} and I'm ${_privateAge}`;
        }
    };
}

const alice = createPerson('Alice', 30);
console.log(alice.introduce());  // "Hi, I'm Alice and I'm 30"
alice.age = 25;
console.log(alice.age);  // 25
console.log(alice._privateAge);  // undefined - not accessible
```

### Classes (ES6+)

Classes provide a cleaner syntax for creating objects with shared behavior.

```javascript
class Person {
    static species = 'Homo sapiens';
    
    constructor(name, age) {
        this._name = name;
        this._age = age;
        this._createdAt = new Date();
    }
    
    get name() {
        return this._name;
    }
    
    set name(value) {
        if (value && typeof value === 'string') {
            this._name = value;
        }
    }
    
    get age() {
        return this._age;
    }
    
    set age(value) {
        if (value >= 0 && Number.isInteger(value)) {
            this._age = value;
        }
    }
    
    greet() {
        return `Hello, I'm ${this._name}`;
    }
    
    static create(name, age) {
        return new Person(name, age);
    }
}

const bob = new Person('Bob', 25);
console.log(bob.greet());  // "Hello, I'm Bob"
console.log(Person.species);  // 'Homo sapiens'
```

---

## Property Manipulation

JavaScript provides extensive capabilities for working with object properties.

### Adding and Modifying Properties

Properties can be added or modified using various syntaxes.

```javascript
const obj = {};

// Adding properties
obj.name = 'Alice';
obj['age'] = 30;

// Adding multiple properties
Object.assign(obj, {
    city: 'NYC',
    status: 'active'
});

// Using Object.defineProperty for more control
Object.defineProperty(obj, 'id', {
    value: '12345',
    writable: false,      // Cannot be modified
    enumerable: true,    // Shows in for...in
    configurable: false  // Cannot be deleted
});
```

### Property Descriptors

Every property has a descriptor that defines its behavior. Understanding descriptors is crucial for creating robust objects.

```javascript
const obj = { value: 10 };

// Get property descriptor
const descriptor = Object.getOwnPropertyDescriptor(obj, 'value');
console.log(descriptor);
// {
//   value: 10,
//   writable: true,
//   enumerable: true,
//   configurable: true
// }

// Define with full descriptor
Object.defineProperty(obj, 'readOnly', {
    value: 'cannot change',
    writable: false,
    enumerable: true,
    configurable: true
});

// Define multiple properties
Object.defineProperties(obj, {
    property1: {
        value: 1,
        writable: true
    },
    property2: {
        get() {
            return this.property1 * 2;
        }
    },
    property3: {
        value: 'hidden',
        enumerable: false
    }
});
```

### Getters and Setters

Getters and setters provide computed properties with validation logic.

```javascript
const temperature = {
    _celsius: 0,
    
    get celsius() {
        return this._celsius;
    },
    
    set celsius(value) {
        if (typeof value !== 'number') {
            throw new TypeError('Temperature must be a number');
        }
        this._celsius = value;
    },
    
    get fahrenheit() {
        return this._celsius * 9/5 + 32;
    },
    
    set fahrenheit(value) {
        this._celsius = (value - 32) * 5/9;
    }
};

temperature.celsius = 25;
console.log(temperature.celsius);    // 25
console.log(temperature.fahrenheit); // 77
temperature.fahrenheit = 100;
console.log(temperature.celsius);    // 37.777...
```

### Property Enumeration

You can control which properties appear in loops and Object.keys() results.

```javascript
const obj = {
    visible: 1,
    hidden: 2
};

// Hide a property
Object.defineProperty(obj, 'hidden', {
    value: 2,
    enumerable: false
});

console.log(Object.keys(obj));     // ['visible'] - hidden not included
console.log(Object.values(obj)); // [1]
console.log(Object.entries(obj)); // [['visible', 1]]

// Get all properties including non-enumerable
console.log(Object.getOwnPropertyNames(obj)); // ['visible', 'hidden']

// Check property existence
console.log('visible' in obj);           // true
console.log(obj.hasOwnProperty('visible')); // true
console.log(Object.hasOwn(obj, 'visible')); // true (ES2022+)
```

---

## Property Descriptors Deep Dive

Property descriptors control every aspect of property behavior.

### Writable Descriptor

Controls whether the property value can be changed.

```javascript
const obj = {};

// Non-writable property
Object.defineProperty(obj, 'constant', {
    value: 'fixed',
    writable: false
});

try {
    obj.constant = 'changed';  // Throws in strict mode
} catch (e) {
    console.log('Cannot modify constant property');
}

// In non-strict mode, assignment is silently ignored
obj.constant = 'changed';
console.log(obj.constant);  // 'fixed'
```

### Configurable Descriptor

Controls whether the property descriptor can be changed and the property can be deleted.

```javascript
const obj = { value: 1 };

Object.defineProperty(obj, 'value', {
    value: 1,
    writable: true,
    enumerable: true,
    configurable: false  // Cannot delete or change descriptor
});

// Cannot delete
delete obj.value;  // Silently ignored
console.log(obj.value);  // 1

// Can change writable
Object.defineProperty(obj, 'value', {
    writable: false  // Now value is read-only
});
```

### Accessor Descriptors

Getters and setters use accessor descriptors rather than data descriptors.

```javascript
const user = {
    _password: 'secret123',
    
    get password() {
        return this._password;
    },
    
    set password(value) {
        if (value.length < 8) {
            throw new Error('Password must be at least 8 characters');
        }
        this._password = value;
    }
};

// Get accessor descriptor
const descriptor = Object.getOwnPropertyDescriptor(user, 'password');
console.log(descriptor);
// {
//   get: [Function: get password],
//   set: [Function: set password],
//   enumerable: true,
//   configurable: true
// }
```

---

## Dynamic Properties

JavaScript allows dynamic property manipulation at runtime.

### Computed Property Names

Properties can be computed from expressions.

```javascript
const prefix = 'user';
const suffix = 'Id';

const user1 = {
    [`${prefix}Name`]: 'Alice',
    [`${prefix}${suffix}`]: 123
};

console.log(user1.userName);   // 'Alice'
console.log(user1.userId);    // 123

// Dynamic key creation in loops
const fields = ['name', 'email', 'phone'];
const data = { name: 'Alice', email: 'alice@example.com', phone: '555-1234' };

const selected = {};
fields.forEach(field => {
    selected[field] = data[field];
});
console.log(selected); // { name: 'Alice', email: 'alice@example.com', phone: '555-1234' }
```

### Symbol Keys

Symbol properties do not appear in most enumeration operations, useful for private-style properties.

```javascript
const unique = Symbol('description');

const obj = {
    [unique]: 'private data',
    public: 'visible'
};

console.log(Object.keys(obj));       // ['public']
console.log(Object.getOwnPropertySymbols(obj)); // [Symbol(description)]

// Use Symbol.for for global symbols
const globalSymbol = Symbol.for('appKey');
const obj2 = {
    [globalSymbol]: 'global value'
};
console.log(Symbol.for('appKey')); // Returns same symbol if exists

// Well-known symbols
const iterable = {
    [Symbol.iterator]() {
        let step = 0;
        return {
            next() {
                step++;
                if (step <= 3) {
                    return { value: step, done: false };
                }
                return { value: undefined, done: true };
            }
        };
    }
};

for (const value of iterable) {
    console.log(value);  // 1, 2, 3
}
```

### Prototype Manipulation

Objects can inherit from other objects via prototypes.

```javascript
const animal = {
    speak() {
        return 'Sound';
    }
};

const dog = Object.create(animal);
dog.breed = 'Golden Retriever';

console.log(dog.speak());      // 'Sound' - from prototype
console.log(dog.hasOwnProperty('speak')); // false - from prototype
console.log(animal.isPrototypeOf(dog)); // true

// Change prototype
const cat = { breed: 'Persian' };
Object.setPrototypeOf(dog, cat);
console.log(dog.breed);  // 'Golden Retriever' - own property wins
```

---

## Object Methods and Operations

JavaScript objects have extensive built-in methods for manipulation and querying.

### Object.keys, Values, Entries

These methods return enumerable properties as arrays.

```javascript
const obj = { a: 1, b: 2, c: 3 };

console.log(Object.keys(obj));   // ['a', 'b', 'c']
console.log(Object.values(obj)); // [1, 2, 3]
console.log(Object.entries(obj)); // [['a', 1], ['b', 2], ['c', 3]]

// Iterate with entries
for (const [key, value] of Object.entries(obj)) {
    console.log(`${key}: ${value}`);
}

// Convert back to object
const pairs = [['a', 1], ['b', 2], ['c', 3]];
const restored = Object.fromEntries(pairs);
console.log(restored);  // { a: 1, b: 2, c: 3 }
```

### Object.assign and Spread

Merge and copy objects efficiently.

```javascript
const defaults = { theme: 'light', language: 'en' };
const userPrefs = { theme: 'dark' };

// Merge objects
const config = Object.assign({}, defaults, userPrefs);
console.log(config);  // { theme: 'dark', language: 'en' }

// Spread operator
const config2 = { ...defaults, ...userPrefs };

// Handling nested objects
const base = { settings: { theme: 'light' } };
const override = { settings: { theme: 'dark' } };
const merged = { 
    ...base, 
    settings: { ...base.settings, ...override.settings } 
};
```

### Object.freeze and seal

Prevent modifications to objects.

```javascript
const frozen = Object.freeze({ value: 1 });
const sealed = Object.seal({ value: 1 });

// Frozen - cannot add, modify, or delete
try {
    frozen.value = 2;  // Ignored or throws
    frozen.newProp = 3;  // Ignored or throws
    delete frozen.value; // Ignored or throws
} catch (e) {
    console.log('Cannot modify frozen object');
}

// Check freeze/seal state
console.log(Object.isFrozen(frozen));  // true
console.log(Object.isSealed(sealed));  // true

// Seal - can modify existing, cannot add or delete
sealed.value = 2;  // Works
sealed.newProp = 3;  // Ignored
delete sealed.value; // Ignored
```

### Object.hasOwn and in

Check property existence.

```javascript
const obj = { a: 1 };
console.log(Object.hasOwn(obj, 'a'));  // true (ES2022+)
console.log(obj.hasOwnProperty('a'));    // true

// Using 'in' operator (includes prototype chain)
const proto = { inherited: 1 };
const child = Object.create(proto);
console.log('inherited' in child);       // true - checks prototype
console.log(child.hasOwnProperty('inherited')); // false - own only

// Modern approach
console.log(Object.hasOwn(child, 'inherited')); // false
```

---

## Professional Patterns

These patterns are used in production JavaScript applications.

### Deep Merge

Combining nested objects without losing data.

```javascript
function deepMerge(target, ...sources) {
    if (!sources.length) return target;
    const source = sources.shift();
    
    if (isObject(target) && isObject(source)) {
        for (const key in source) {
            if (isObject(source[key])) {
                if (!target[key]) {
                    Object.assign(target, { [key]: {} });
                }
                deepMerge(target[key], source[key]);
            } else {
                Object.assign(target, { [key]: source[key] });
            }
        }
    }
    
    return deepMerge(target, ...sources);
}

function isObject(obj) {
    return obj != null && typeof obj === 'object';
}

// Usage
const defaults = { a: 1, b: { c: 2 } };
const override = { b: { d: 3 }, e: 4 };
console.log(deepMerge(defaults, override));
// { a: 1, b: { c: 2, d: 3 }, e: 4 }
```

### Object Validation

Validating object structure and values.

```javascript
function validate(schema, data) {
    const errors = [];
    
    for (const [field, rules] of Object.entries(schema)) {
        const value = data[field];
        
        if (rules.required && value === undefined) {
            errors.push(`${field} is required`);
            continue;
        }
        
        if (value !== undefined) {
            if (rules.type && typeof value !== rules.type) {
                errors.push(`${field} must be ${rules.type}`);
            }
            
            if (rules.min !== undefined && value < rules.min) {
                errors.push(`${field} must be at least ${rules.min}`);
            }
            
            if (rules.max !== undefined && value > rules.max) {
                errors.push(`${field} must be at most ${rules.max}`);
            }
            
            if (rules.pattern && !rules.pattern.test(value)) {
                errors.push(`${field} has invalid format`);
            }
        }
    }
    
    return {
        valid: errors.length === 0,
        errors
    };
}

// Usage
const schema = {
    name: { required: true, type: 'string' },
    age: { required: false, type: 'number', min: 0, max: 150 },
    email: { required: true, pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/ }
};

const data = { name: 'Alice', age: -5 };
console.log(validate(schema, data));
// { valid: false, errors: ['age must be at least 0'] }
```

### Event Bus Pattern

Using objects for event-driven architecture.

```javascript
class EventBus {
    constructor() {
        this.listeners = {};
    }
    
    on(event, callback) {
        if (!this.listeners[event]) {
            this.listeners[event] = [];
        }
        this.listeners[event].push(callback);
        
        return () => this.off(event, callback);
    }
    
    off(event, callback) {
        if (!this.listeners[event]) return;
        this.listeners[event] = this.listeners[event]
            .filter(cb => cb !== callback);
    }
    
    emit(event, ...args) {
        if (!this.listeners[event]) return;
        this.listeners[event].forEach(cb => cb(...args));
    }
    
    once(event, callback) {
        const wrapper = (...args) => {
            callback(...args);
            this.off(event, wrapper);
        };
        return this.on(event, wrapper);
    }
}

// Usage
const events = new EventBus();

events.on('user:login', (user) => {
    console.log(`${user.name} logged in`);
});

events.on('user:login', (user) => {
    console.log(`Session started for ${user.name}`);
});

events.emit('user:login', { name: 'Alice' });
// Alice logged in
// Session started for Alice
```

### Memoization Cache

Caching expensive computations.

```javascript
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

// Usage - fibonacci
function fibonacci(n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

const memoizedFibonacci = memoize(fibonacci);
console.log(memoizedFibonacci(40));  // Much faster than uncached
```

### Data Transform Pipeline

Processing object data through transformations.

```javascript
function pipe(obj, ...fns) {
    return fns.reduce((acc, fn) => fn(acc), obj);
}

const transform = pipe(
    { rawAmount: '100.50', currency: 'USD', date: '2024-01-15' },
    // Step 1: Parse numbers
    data => ({
        ...data,
        amount: parseFloat(data.rawAmount)
    }),
    // Step 2: Format date
    data => ({
        ...data,
        formattedDate: new Date(data.date).toLocaleDateString()
    }),
    // Step 3: Add formatted amount
    data => ({
        ...data,
        displayAmount: `${data.currency} ${data.amount.toFixed(2)}`
    }),
    // Step 4: Filter unused fields
    ({ displayAmount, formattedDate }) => ({
        amount: displayAmount,
        date: formattedDate })
);

console.log(transform);
// { amount: 'USD 100.50', date: '1/15/2024' }
```

---

## Key Takeaways

1. **Objects are reference types**: Variables hold references, not copies
2. **Use Object.freeze() for immutability**: Prevents accidental modifications
3. **Property descriptors control behavior**: writable, enumerable, configurable
4. **Classes provide clean syntax**: Use for creating multiple similar objects
5. **Factory functions enable privacy**: Create objects with private state
6. **Symbol keys are non-enumerable**: Useful for private properties
7. **Object methods enable manipulation**: keys(), values(), entries(), assign()
8. **Deep merge nested objects**: Use recursive approach or libraries

---

## Common Pitfalls

1. **Mutating shared references**: Always copy before modifying
2. **Forgetting enumerable defaults**: All properties are enumerable by default
3. **Confusing in and hasOwnProperty**: Different prototype chain behavior
4. **Not using deep freeze**: Only freezes first level
5. **Circular references in JSON**: Cannot serialize
6. **Shadowing prototype methods**: Creates own properties that hide prototype methods
7. **Using arrays as keys**: Converts to string '1,2,3'
8. **Modifying sealed objects**: Property values can still be modified

---

## Related Files

- **01_ARRAYS_MASTER.md**: Arrays share object-like behavior with forEach, map, filter
- **03_MAPS_AND_SETS.md**: Map provides better key-value storage for complex keys
- **04_DATA_STRUCTURES_ALGORITHMS.md**: Object-based algorithm implementations
- **05_JAVASCRIPT_DATA_STRUCTURES_PATTERNS.md**: Professional object patterns
- **06_MEMORY_MANAGEMENT_DATA_STRUCTURES.md**: Memory management for objects