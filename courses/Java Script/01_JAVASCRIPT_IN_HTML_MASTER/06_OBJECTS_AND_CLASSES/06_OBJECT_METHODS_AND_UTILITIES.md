# JavaScript Object Methods and Utilities: Complete Mastery Guide

JavaScript provides a rich set of built-in methods for object manipulation, including property enumeration, merging, freezing, and sealing. Understanding these utilities is essential for efficient data transformation, immutability patterns, and robust application development. This comprehensive guide covers Object.assign, Object.keys/values/entries, Object.freeze/seal, spread operator, and modern ES2024+ features.

---

## Table of Contents

1. [Object.keys(), values(), and entries()](#objectkeys-values-and-entries)
2. [Object.assign()](#objectassign)
3. [Object.create()](#objectcreate)
4. [Object.freeze() and Object.seal()](#objectfreeze-and-objectseal)
5. [Spread Operator and Object Restructuring](#spread-operator-and-object-restructuring)
6. [Modern Object Utilities](#modern-object-utilities)
7. [Professional Use Cases](#professional-use-cases)
8. [Key Takeaways](#key-takeaways)
9. [Common Pitfalls](#common-pitfalls)
10. [Related Files](#related-files)

---

## Object.keys(), values(), and entries()

These methods provide different views of an object's properties for iteration and transformation.

### Object.keys()

Returns an array of the object's own enumerable property keys.

```javascript
// Object.keys() basics
const user = {
    name: 'Alice',
    email: 'alice@example.com',
    age: 30
};

const keys = Object.keys(user);
console.log(keys);  // ['name', 'email', 'age']

// Using keys for iteration
keys.forEach(key => {
    console.log(`${key}: ${user[key]}`);
});
// name: Alice
// email: alice@example.com
// age: 30

// Non-enumerable properties are excluded
const obj = { a: 1 };
Object.defineProperty(obj, 'b', {
    value: 2,
    enumerable: false
});

console.log(Object.keys(obj));  // ['a'] - 'b' is non-enumerable
```

### Object.values()

Returns an array of the object's own enumerable property values.

```javascript
// Object.values() basics
const user = {
    name: 'Alice',
    email: 'alice@example.com',
    age: 30
};

const values = Object.values(user);
console.log(values);  // ['Alice', 'alice@example.com', 30]

// Using with for...of
for (const value of Object.values(user)) {
    console.log(value);
}
// Alice
// alice@example.com
// 30
```

### Object.entries()

Returns an array of key-value pairs as [key, value] arrays.

```javascript
// Object.entries() basics
const user = {
    name: 'Alice',
    age: 30
};

const entries = Object.entries(user);
console.log(entries);  // [['name', 'Alice'], ['age', 30]]

// Converting to Map
const map = new Map(Object.entries(user));
console.log(map.get('name'));  // 'Alice'

// Using for iteration
for (const [key, value] of Object.entries(user)) {
    console.log(`${key}: ${value}`);
}

// Creating object from entries
const data = [['a', 1], ['b', 2]];
const obj = Object.fromEntries(data);
console.log(obj);  // { a: 1, b: 2 }
```

### Iteration Patterns

```javascript
// Advanced iteration patterns
const product = {
    name: 'Laptop',
    price: 999,
    category: 'Electronics',
    inStock: true
};

// Filter entries
const filtered = Object.entries(product)
    .filter(([key, value]) => typeof value === 'number');
console.log(Object.fromEntries(filtered));  // { price: 999 }

// Transform values
const uppercased = Object.fromEntries(
    Object.entries(product).map(([key, value]) => 
        [key, typeof value === 'string' ? value.toUpperCase() : value]
    )
);
console.log(uppercased);
// { name: 'LAPTOP', price: 999, category: 'ELECTRONICS', inStock: true }

// Pick specific keys
const pick = (obj, keys) => Object.fromEntries(
    Object.entries(obj).filter(([key]) => keys.includes(key))
);

console.log(pick(product, ['name', 'price']));  // { name: 'Laptop', price: 999 }

// Omit specific keys
const omit = (obj, keys) => Object.fromEntries(
    Object.entries(obj).filter(([key]) => !keys.includes(key))
);

console.log(omit(product, ['inStock']));  // { name: 'Laptop', price: 999, category: 'Electronics' }
```

---

## Object.assign()

Merges source objects into a target object, returning the modified target.

### Basic Usage

```javascript
// Object.assign() basics
const target = { a: 1 };
const source = { b: 2, c: 3 };

const result = Object.assign(target, source);
console.log(result);        // { a: 1, b: 2, c: 3 }
console.log(target === result);  // true - same object

// Multiple sources
const merged = Object.assign({}, { a: 1 }, { b: 2 }, { a: 3 });
console.log(merged);  // { a: 3, b: 2 } - later values override earlier
```

### Merging Objects

```javascript
// Deep merge alternative
const defaults = {
    theme: 'light',
    language: 'en',
    timeout: 5000,
    retries: 3
};

const userConfig = {
    theme: 'dark',
    timeout: 10000
};

// Shallow merge
const shallowMerged = Object.assign({}, defaults, userConfig);
console.log(shallowMerged);
// { theme: 'dark', language: 'en', timeout: 10000, retries: 3 }

// Deep merge function
function deepMerge(target, ...sources) {
    for (const source of sources) {
        for (const key of Object.keys(source)) {
            const sourceValue = source[key];
            const targetValue = target[key];
            
            if (sourceValue && typeof sourceValue === 'object' && !Array.isArray(sourceValue) &&
                targetValue && typeof targetValue === 'object' && !Array.isArray(targetValue)) {
                target[key] = deepMerge({}, targetValue, sourceValue);
            } else {
                target[key] = sourceValue;
            }
        }
    }
    return target;
}

const config = deepMerge({}, defaults, userConfig);
console.log(config);  // { theme: 'dark', language: 'en', timeout: 10000, retries: 3 }
```

### Cloning Objects

```javascript
// Cloning with Object.assign
const original = { a: 1, b: { c: 2 } };

// Shallow clone
const shallowClone = Object.assign({}, original);
console.log(shallowClone);     // { a: 1, b: { c: 2 } }
console.log(shallowClone.b === original.b);  // true - same reference!

// Proper shallow clone with spread
const clone = { ...original };
console.log(clone);            // { a: 1, b: { c: 2 } }
console.log(clone.b === original.b);  // true - still same reference

// Deep clone options
// Method 1: structuredClone (modern, handles circular)
const obj = { a: 1, b: { c: 2 } };
const deep1 = structuredClone(obj);

// Method 2: JSON round-trip (loses functions, symbols, circular)
const deep2 = JSON.parse(JSON.stringify(obj));

// Method 3: Custom recursive clone
function deepClone(obj) {
    if (obj === null || typeof obj !== 'object') return obj;
    if (obj instanceof Date) return new Date(obj.getTime());
    if (obj instanceof Array) return obj.map(item => deepClone(item));
    
    const cloned = Object.create(null);
    for (const key of Object.keys(obj)) {
        cloned[key] = deepClone(obj[key]);
    }
    return cloned;
}

const deep3 = deepClone(obj);
console.log(deep3.b === obj.b);  // false
```

### Assign with Property Transformations

```javascript
// Transform during assignment
const transform = Object.assign(
    {},
    { a: 1, b: 2 },
    { b: 3, c: 4 },  // b overwritten
    { ...['d', 'e', 'f'].reduce((acc, v, i) => ({ ...acc, [v]: i }), {}) }
);

console.log(transform);  // { a: 1, b: 3, c: 4, d: 0, e: 1, f: 2 }

// Merging with custom key mapping
const source = { firstName: 'John', lastName: 'Doe' };
const mapped = Object.assign(
    {},
    { fullName: `${source.firstName} ${source.lastName}` }
);
console.log(mapped);  // { fullName: 'John Doe' }
```

---

## Object.create()

Creates a new object with a specified prototype and optional own properties.

### Creating Objects with Custom Prototypes

```javascript
// Object.create() with prototype
const animalProto = {
    speak() {
        return `${this.name} makes a sound`;
    },
    move() {
        return `${this.name} moves`;
    }
};

const dog = Object.create(animalProto);
dog.name = 'Rex';
dog.breed = 'Labrador';

console.log(dog.speak());  // 'Rex makes a sound'
console.log(dog.move());   // 'Rex moves'
console.log(Object.getPrototypeOf(dog) === animalProto);  // true

// Check own vs inherited properties
console.log(dog.hasOwnProperty('name'));     // true
console.log(dog.hasOwnProperty('speak'));    // false - on prototype
```

### Creating Objects with Null Prototype

```javascript
// Null prototype for pure dictionary
const dict = Object.create(null);
dict['key'] = 'value';
dict[1] = 'one';

console.log(Object.keys(dict));  // ['key', '1']
console.log(dict.toString);      // undefined - no prototype methods
console.log(dict.constructor);  // undefined

// Safe from prototype pollution
const polluted = Object.create({});
polluted['__proto__'] = { isAdmin: true };
console.log(polluted.isAdmin);  // true - prototype pollution!

const safe = Object.create(null);
safe['__proto__'] = { isAdmin: true };
console.log(safe.isAdmin);      // undefined - can't pollute
```

### Object.create() with Descriptors

```javascript
// Define own properties with descriptors
const person = Object.create(null, {
    name: {
        value: 'Alice',
        writable: true,
        enumerable: true,
        configurable: true
    },
    age: {
        value: 30,
        writable: false,
        enumerable: true,
        configurable: false
    },
    // Computed property not possible with defineProperty
});

console.log(person.name);  // 'Alice'
console.log(person.age);   // 30

person.name = 'Bob';       // Works - writable
person.age = 35;           // Fails silently (strict: throws)

// Cannot delete non-configurable property
delete person.age;          // Fails
```

---

## Object.freeze() and Object.seal()

These methods provide different levels of object protection.

### Object.freeze()

Prevents all modifications to an object: no property additions, deletions, or value changes.

```javascript
// Object.freeze() basics
const frozen = Object.freeze({ a: 1, b: 2 });

// Cannot modify existing properties
frozen.a = 3;  // Silently fails (strict: throws)
console.log(frozen.a);  // 1

// Cannot add new properties
frozen.c = 3;  // Silently fails
console.log(frozen.c);  // undefined

// Cannot delete properties
delete frozen.a;  // Silently fails
console.log(frozen.a);  // 1

// Check frozen status
console.log(Object.isFrozen(frozen));  // true
```

### Object.seal()

Allows modifying existing properties but prevents additions and deletions.

```javascript
// Object.seal() basics
const sealed = Object.seal({ a: 1, b: 2 });

// Can modify existing properties
sealed.a = 3;
console.log(sealed.a);  // 3

// Cannot add new properties
sealed.c = 3;  // Silently fails
console.log(sealed.c);  // undefined

// Cannot delete existing properties
delete sealed.a;  // Silently fails
console.log(sealed.a);  // 3

// Check sealed status
console.log(Object.isSealed(sealed));  // true
```

### Deep Freeze and Seal

```javascript
// Deep freeze function
function deepFreeze(obj) {
    const freeze = Object.freeze;
    
    Object.keys(obj).forEach(key => {
        const value = obj[key];
        if (value && typeof value === 'object') {
            deepFreeze(value);
        }
    });
    
    return Object.freeze(obj);
}

const nested = {
    a: 1,
    b: {
        c: 2,
        d: { e: 3 }
    }
};

deepFreeze(nested);
nested.a = 100;               // Fails
nested.b.c = 200;             // Fails - nested is also frozen
nested.b.d.e = 300;           // Fails - deeply frozen

console.log(nested.a);        // 1
console.log(nested.b.c);      // 2

// Deep seal function
function deepSeal(obj) {
    Object.keys(obj).forEach(key => {
        const value = obj[key];
        if (value && typeof value === 'object') {
            deepSeal(value);
        }
    });
    
    return Object.seal(obj);
}
```

### Preventing Extension

```javascript
// Object.preventExtensions()
const nonExtensible = { a: 1 };
Object.preventExtensions(nonExtensible);

nonExtensible.b = 2;  // Silently fails
console.log(nonExtensible.b);  // undefined

console.log(Object.isExtensible(nonExtensible));  // false

// Different protection levels
const obj = { a: 1 };

// Each adds protection incrementally
Object.preventExtensions(obj);  // No new properties
Object.seal(obj);               // No new/delete, values writable
Object.freeze(obj);            // Full immutability
```

---

## Spread Operator and Object Restructuring

The spread operator (...) provides a modern syntax for object manipulation.

### Spread in Object Literals

```javascript
// Spread operator basics
const obj1 = { a: 1, b: 2 };
const obj2 = { ...obj1 };
console.log(obj2);  // { a: 1, b: 2 }

const obj3 = { ...obj1, c: 3 };
console.log(obj3);  // { a: 1, b: 2, c: 3 }

const obj4 = { ...obj1, a: 100 };  // Override
console.log(obj4);  // { a: 100, b: 2 }

// Multiple spreads
const base = { x: 1 };
const added = { y: 2 };
const final = { ...base, ...added, z: 3 };
console.log(final);  // { x: 1, y: 2, z: 3 }
```

### Conditional Spread

```javascript
// Conditional spread
const condition = false;
const obj = {
    a: 1,
    ...(condition && { b: 2 }),  // Only spreads if truthy
    c: 3
};
console.log(obj);  // { a: 1, c: 3 }

const condition2 = true;
const obj2 = {
    a: 1,
    ...(condition2 && { b: 2 }),
    c: 3
};
console.log(obj2);  // { a: 1, b: 2, c: 3 }

// Filter with spread
const filter = ['a', 'c'];
const data = { a: 1, b: 2, c: 3, d: 4 };
const filtered = Object.fromEntries(
    Object.entries(data).filter(([key]) => filter.includes(key))
);
console.log(filtered);  // { a: 1, c: 3 }
```

### Rest Properties

```javascript
// Rest in destructuring
const obj = { a: 1, b: 2, c: 3, d: 4 };
const { a, ...rest } = obj;
console.log(a);    // 1
console.log(rest); // { b: 2, c: 3, d: 4 }

// Omit function using rest
function omit(obj, keys) {
    const { [keys[0]]: _, ...rest } = obj;  // Remove first key
    return keys.length > 1 ? omit(rest, keys.slice(1)) : rest;
}

console.log(omit({ a: 1, b: 2, c: 3 }, ['b']));  // { a: 1, c: 3 }

// Pick function
function pick(obj, keys) {
    return Object.fromEntries(
        keys.map(key => [key, obj[key]]).filter(([, v]) => v !== undefined)
    );
}

console.log(pick({ a: 1, b: 2, c: 3 }, ['a', 'c']));  // { a: 1, c: 3 }
```

### Function Arguments

```javascript
// Spread in function calls
const arr = [1, 2, 3];
console.log(Math.max(...arr));  // 3
console.log(Math.min(...arr));  // 1

// Rest parameters
function sum(...numbers) {
    return numbers.reduce((a, b) => a + b, 0);
}

console.log(sum(1, 2, 3, 4));  // 10

// Combining rest and spread
function greet(greeting, ...names) {
    return names.map(name => `${greeting}, ${name}!`);
}

console.log(greet('Hello', 'Alice', 'Bob', 'Carol'));  // ['Hello, Alice!', ...]
```

---

## Modern Object Utilities

ES2022+ introduced new object utilities that simplify common operations.

### Object.hasOwn() (ES2022)

```javascript
// Object.hasOwn() - safer than hasOwnProperty
const obj = { a: 1 };
Object.prototype.b = 2;  // Dangerous - pollutes all objects

console.log(obj.hasOwnProperty('a'));   // true
console.log(obj.hasOwnProperty('b'));   // true - from prototype!

console.log(Object.hasOwn(obj, 'a'));  // true
console.log(Object.hasOwn(obj, 'b'));  // false - only own properties
```

### Object.entries() and Object.values() Improvements

```javascript
// Working with Symbol keys
const sym = Symbol('a');
const obj = { [sym]: 1, b: 2 };

console.log(Object.keys(obj));    // ['b'] - excludes Symbol
console.log(Object.values(obj));  // [2] - excludes Symbol
console.log(Object.entries(obj)); // [['b', 2]] - excludes Symbol

// Getting Symbol keys
console.log(Object.getOwnPropertySymbols(obj));  // [Symbol(a)]

// All own properties including Symbols
const allProps = [...Object.keys(obj), ...Object.getOwnPropertySymbols(obj)];
console.log(allProps);  // [Symbol(a), 'b']
```

### Object.fromEntries()

```javascript
// Converting Map to object
const map = new Map([
    ['a', 1],
    ['b', 2],
    ['c', 3]
]);

const obj = Object.fromEntries(map);
console.log(obj);  // { a: 1, b: 2, c: 3 }

// Converting URLSearchParams
const params = new URLSearchParams('name=Alice&age=30');
const paramsObj = Object.fromEntries(params);
console.log(paramsObj);  // { name: 'Alice', age: '30' }

// Transformations
const data = [['a', 1], ['b', 2], ['c', 3]];
const doubled = Object.fromEntries(
    data.map(([k, v]) => [k, v * 2])
);
console.log(doubled);  // { a: 2, b: 4, c: 6 }
```

### Object.groupBy() and Object.groupByToMap() (ES2024)

```javascript
// Object.groupBy for grouping
const items = [
    { name: 'Apple', category: 'fruit' },
    { name: 'Carrot', category: 'vegetable' },
    { name: 'Banana', category: 'fruit' }
];

const grouped = Object.groupBy(items, item => item.category);
console.log(grouped);
// { fruit: [{name: 'Apple',...}, {name: 'Banana',...}], vegetable: [...] }

// Object.groupByToMap
const groupedMap = Object.groupByToMap(items, item => item.category);
console.log(groupedMap.get('fruit'));  // Array of fruits
```

---

## Professional Use Cases

### Use Case 1: Configuration Manager

```javascript
// Configuration with defaults and deep freeze
class ConfigManager {
    #config;
    #defaults;
    #frozen;
    
    constructor(defaults = {}) {
        this.#defaults = defaults;
        this.#config = { ...defaults };
        this.#frozen = false;
    }
    
    set(key, value) {
        if (this.#frozen) {
            throw new Error('Config is frozen');
        }
        
        const keys = key.split('.');
        let current = this.#config;
        
        for (let i = 0; i < keys.length - 1; i++) {
            if (!current[keys[i]]) {
                current[keys[i]] = {};
            }
            current = current[keys[i]];
        }
        
        current[keys[keys.length - 1]] = value;
        return this;
    }
    
    get(key) {
        const keys = key.split('.');
        let current = this.#config;
        
        for (const k of keys) {
            if (current === undefined) return undefined;
            current = current[k];
        }
        
        return current;
    }
    
    reset() {
        this.#config = { ...this.#defaults };
        this.#frozen = false;
        return this;
    }
    
    freeze() {
        this.#frozen = true;
        this.#config = Object.freeze(this.#config);
        return this;
    }
    
    toJSON() {
        return { ...this.#config };
    }
}

const config = new ConfigManager({
    app: {
        name: 'MyApp',
        version: '1.0.0'
    },
    api: {
        timeout: 5000
    }
});

config.set('app.name', 'NewApp').set('api.timeout', 10000);
console.log(config.get('app.name'));  // 'NewApp'
config.freeze();
console.log(Object.isFrozen(config.get('app')));  // true
```

### Use Case 2: State Immutability

```javascript
// Immutable state updates
function updateState(state, path, value) {
    const keys = path.split('.');
    const newState = { ...state };
    let current = newState;
    
    for (let i = 0; i < keys.length - 1; i++) {
        current[keys[i]] = { ...current[keys[i]] };
        current = current[keys[i]];
    }
    
    current[keys[keys.length - 1]] = value;
    return Object.freeze(newState);
}

let state = Object.freeze({
    user: { name: 'Alice', settings: { theme: 'dark' } },
    items: []
});

state = updateState(state, 'user.settings.theme', 'light');
console.log(state.user.settings.theme);  // 'light'
console.log(Object.isFrozen(state.user));  // true

// Array updates
state = Object.freeze({
    ...state,
    items: Object.freeze([...state.items, { id: 1 }])
});
console.log(state.items.length);  // 1
```

### Use Case 3: Property-Based Transformation

```javascript
// Transform object properties
function transformObject(obj, transformers) {
    const result = {};
    
    for (const [key, value] of Object.entries(obj)) {
        const transformer = transformers[key];
        result[key] = transformer 
            ? transformer(value) 
            : value;
    }
    
    return result;
}

const data = {
    name: '  Alice  ',
    age: 30,
    email: 'ALICE@EXAMPLE.COM',
    price: 100
};

const transformed = transformObject(data, {
    name: v => v.trim(),
    age: v => v + 1,
    email: v => v.toLowerCase(),
    price: v => Number(v.toFixed(2))
});

console.log(transformed);
// { name: 'Alice', age: 31, email: 'alice@example.com', price: 100 }
```

### Use Case 4: Deep Comparison

```javascript
// Deep object comparison
function deepEqual(a, b) {
    if (a === b) return true;
    
    if (typeof a !== 'object' || a === null ||
        typeof b !== 'object' || b === null) {
        return false;
    }
    
    const keysA = Object.keys(a);
    const keysB = Object.keys(b);
    
    if (keysA.length !== keysB.length) return false;
    
    for (const key of keysA) {
        if (!keysB.includes(key)) return false;
        if (!deepEqual(a[key], b[key])) return false;
    }
    
    return true;
}

// Usage
const obj1 = { a: { b: 1 } };
const obj2 = { a: { b: 1 } };
const obj3 = { a: { b: 2 } };

console.log(deepEqual(obj1, obj2));  // true
console.log(deepEqual(obj1, obj3));  // false
```

---

## Key Takeaways

1. **Object.keys/values/entries** return own enumerable properties
2. **Object.assign()** merges objects, modifying the target
3. **Spread operator** creates new objects, preserving immutability
4. **Object.freeze()** prevents all modifications
5. **Object.seal()** prevents add/delete, allows modifications
6. **Object.hasOwn()** is safer than hasOwnProperty
7. **Object.fromEntries()** converts entries to objects
8. **Deep freeze** requires recursive freezing of nested objects

---

## Common Pitfalls

1. **Shallow clone with assign**: Nested objects remain shared references
2. **Forgetting enumerable default**: Object.defineProperty defaults to false
3. **Not freezing deeply**: Only first level is frozen
4. **Using hasOwnProperty**: Can be overridden, use Object.hasOwn()
5. **JSON.stringify loses functions**: Only serializes JSON-compatible data
6. **Mutating frozen objects**: Silently fails in non-strict mode
7. **Circular references in JSON**: Causes errors
8. **Confusing seal and freeze**: Different protection levels

---

## Related Files

- **01_OBJECT_LITERALS_AND_CREATION.md**: Object literal syntax
- **02_PROTOTYPES_DEEP_DIVE.md**: Prototype chain
- **03_CONSTRUCTOR_FUNCTIONS.md**: Constructor patterns
- **05_GETTERS_SETTERS_AND_PROPERTIES.md**: Property descriptors
- **07_INHERITANCE_PATTERNS.md**: Inheritance patterns
- **08_OBJECT_SECURITY_PATTERNS.md**: Object protection