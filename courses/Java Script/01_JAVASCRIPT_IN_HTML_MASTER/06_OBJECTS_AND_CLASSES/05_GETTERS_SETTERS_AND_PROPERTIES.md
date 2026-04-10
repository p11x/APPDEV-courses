# JavaScript Getters, Setters, and Properties: Complete Mastery Guide

JavaScript's property system provides sophisticated control over property access and assignment through property descriptors, computed properties, getters, and setters. Understanding these features is essential for creating robust APIs, implementing data validation, building computed properties, and managing object behavior. This comprehensive guide covers property descriptors, accessor properties, computed properties, and professional patterns.

---

## Table of Contents

1. [Property Descriptors Deep Dive](#property-descriptors-deep-dive)
2. [Getters and Setters](#getters-and-setters)
3. [Computed Properties](#computed-properties)
4. [Accessor Property Patterns](#accessor-property-patterns)
5. [Property Enumerability and Configurability](#property-enumerability-and-configurability)
6. [Professional Use Cases](#professional-use-cases)
7. [Key Takeaways](#key-takeaways)
8. [Common Pitfalls](#common-pilfalls)
9. [Related Files](#related-files)

---

## Property Descriptors Deep Dive

Every JavaScript property has a descriptor that defines its behavior. Property descriptors include value, writable, enumerable, configurable, get, and set.

### Understanding Property Descriptors

```javascript
// Getting property descriptors
const obj = {
    name: 'Alice',
    age: 30
};

const descriptor = Object.getOwnPropertyDescriptor(obj, 'name');
console.log(descriptor);
// { value: 'Alice', writable: true, enumerable: true, configurable: true }
```

### Defining Properties with Descriptors

```javascript
// Object.defineProperty for fine-grained control
const user = {};

Object.defineProperty(user, 'name', {
    value: 'Alice',
    writable: true,
    enumerable: true,
    configurable: true
});

Object.defineProperty(user, 'age', {
    value: 30,
    writable: false,  // Cannot be changed
    enumerable: true,
    configurable: false  // Cannot be deleted or reconfigured
});

console.log(user.name);  // 'Alice'
user.name = 'Bob';       // Works - writable is true
console.log(user.name);  // 'Bob'

user.age = 35;           // Silently fails - writable is false
console.log(user.age);   // 30

delete user.name;        // Works - configurable is true
console.log(user.name); // undefined

delete user.age;        // Silently fails - configurable is false
console.log(user.age);  // 30
```

### Multiple Property Definitions

```javascript
// Object.defineProperties for multiple properties
const product = {};

Object.defineProperties(product, {
    name: {
        value: 'Laptop',
        writable: true,
        enumerable: true,
        configurable: true
    },
    price: {
        value: 999.99,
        writable: false,
        enumerable: true,
        configurable: false
    },
    sku: {
        value: 'SKU-001',
        writable: false,
        enumerable: false,  // Won't show in Object.keys()
        configurable: false
    }
});

console.log(Object.keys(product));  // ['name', 'price']
console.log(product.sku);          // 'SKU-001'
```

### Default Descriptor Values

```javascript
// Default values when not specified
const obj1 = {};
Object.defineProperty(obj1, 'implicit', 'value');

const implicitDesc = Object.getOwnPropertyDescriptor(obj1, 'implicit');
console.log(implicitDesc);
// { value: 'implicit', writable: false, enumerable: false, configurable: false }

// Contrast with literal property
const obj2 = { explicit: 'value' };
const explicitDesc = Object.getOwnPropertyDescriptor(obj2, 'explicit');
console.log(explicitDesc);
// { value: 'value', writable: true, enumerable: true, configurable: true }
```

---

## Getters and Setters

Getters and setters define accessor properties that execute functions when properties are accessed or assigned.

### Basic Getter and Setter

```javascript
// Object literal with getters and setters
const user = {
    _name: 'Alice',
    _email: 'alice@example.com',
    
    // Getter
    get name() {
        return this._name;
    },
    
    // Setter
    set name(value) {
        if (typeof value !== 'string' || value.trim().length === 0) {
            throw new Error('Name must be a non-empty string');
        }
        this._name = value.trim();
    },
    
    get email() {
        return this._email;
    },
    
    set email(value) {
        if (!value.includes('@')) {
            throw new Error('Invalid email format');
        }
        this._email = value.toLowerCase();
    },
    
    // Computed property
    get displayName() {
        return `${this._name} <${this._email}>`;
    }
};

console.log(user.name);      // 'Alice'
console.log(user.displayName); // 'Alice <alice@example.com>'

user.name = '  Bob  ';
console.log(user.name);      // 'Bob' - trimmed

user.email = 'BOB@EXAMPLE.COM';
console.log(user.email);     // 'bob@example.com' - lowercased
```

### Using Object.defineProperty for Accessors

```javascript
// Define accessor properties programmatically
const temperature = {};

Object.defineProperty(temperature, 'celsius', {
    get() {
        return this._celsius;
    },
    set(value) {
        if (typeof value !== 'number') {
            throw new Error('Temperature must be a number');
        }
        this._celsius = value;
    },
    enumerable: true,
    configurable: true
});

Object.defineProperty(temperature, 'fahrenheit', {
    get() {
        return (this._celsius * 9/5) + 32;
    },
    set(value) {
        this._celsius = (value - 32) * 5/9;
    },
    enumerable: true,
    configurable: true
});

temperature.celsius = 25;
console.log(temperature.celsius);    // 25
console.log(temperature.fahrenheit); // 77

temperature.fahrenheit = 212;
console.log(temperature.celsius);   // 100
console.log(temperature.fahrenheit); // 212
```

### Getters with Complex Computation

```javascript
// Computed properties with getters
const shoppingCart = {
    items: [],
    
    addItem(item) {
        const existing = this.items.find(i => i.id === item.id);
        if (existing) {
            existing.quantity += 1;
        } else {
            this.items.push({ ...item, quantity: 1 });
        }
    },
    
    get itemCount() {
        return this.items.reduce((sum, item) => sum + item.quantity, 0);
    },
    
    get totalPrice() {
        return this.items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    },
    
    get isEmpty() {
        return this.items.length === 0;
    },
    
    get formattedTotal() {
        return new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD'
        }).format(this.totalPrice);
    }
};

shoppingCart.addItem({ id: 1, name: 'Apple', price: 1.50 });
shoppingCart.addItem({ id: 2, name: 'Banana', price: 0.75 });
shoppingCart.addItem({ id: 1, name: 'Apple', price: 1.50 });  // Increases quantity

console.log(shoppingCart.itemCount);     // 3
console.log(shoppingCart.totalPrice);   // 3.75
console.log(shoppingCart.formattedTotal); // '$3.75'
```

---

## Computed Properties

Computed property names allow using expressions as property keys in object literals.

### Computed Property Keys

```javascript
// Basic computed properties
const prefix = 'user';
const id = 1;

const user = {
    [`${prefix}Id`]: id,
    [`${prefix}Name`]: 'Alice',
    [`get${prefix}Email`]() {
        return this.userEmail;
    }
};

console.log(user.userId);          // 1
console.log(user.userName);        // 'Alice'
console.log(user.getuserEmail()); // undefined - method doesn't exist yet
```

### Dynamic Property Creation

```javascript
// Dynamic property generation
function createObjectWithPrefix(prefix, data) {
    return Object.fromEntries(
        Object.entries(data).map(([key, value]) => 
            [`${prefix}_${key}`, value]
        )
    );
}

const data = { name: 'Product', price: 100, category: 'Electronics' };
const product = createObjectWithPrefix('prod', data);

console.log(product);
// { prod_name: 'Product', prod_price: 100, prod_category: 'Electronics' }
```

### Computed Keys in Classes

```javascript
// Computed property names in class
const methodName = 'calculateTotal';

class Order {
    constructor(items) {
        this.items = items;
        this.tax = 0.08;
    }
    
    [methodName]() {
        return this.items.reduce((sum, item) => sum + item.price, 0);
    }
    
    get totalWithTax() {
        return this[methodName]() * (1 + this.tax);
    }
}

const order = new Order([
    { name: 'Item 1', price: 50 },
    { name: 'Item 2', price: 30 }
]);

console.log(order.calculateTotal());  // 80
console.log(order.totalWithTax);      // 86.4
```

### Symbol Computed Properties

```javascript
// Using Symbols as computed keys
const privateData = Symbol('private');

const obj = {
    name: 'Alice',
    [privateData]: {
        ssn: '123-45-6789',
        creditScore: 750
    },
    
    getSSN() {
        return this[privateData].ssn;
    }
};

console.log(obj.name);        // 'Alice'
console.log(obj.getSSN());    // '123-45-6789'

// Symbols aren't included in Object.keys
console.log(Object.keys(obj));  // ['name']
console.log(Object.getOwnPropertySymbols(obj));  // [Symbol(private)]
```

---

## Accessor Property Patterns

Accessor properties enable validation, transformation, lazy evaluation, and proxy-like patterns.

### Validation with Setters

```javascript
// Comprehensive validation
class User {
    #data = {};
    
    constructor(data) {
        this.#data = { ...data };
    }
    
    get name() {
        return this.#data.name;
    }
    
    set name(value) {
        if (typeof value !== 'string') {
            throw new Error('Name must be a string');
        }
        if (value.length < 2) {
            throw new Error('Name must be at least 2 characters');
        }
        if (value.length > 50) {
            throw new Error('Name must be at most 50 characters');
        }
        this.#data.name = value.trim();
    }
    
    get age() {
        return this.#data.age;
    }
    
    set age(value) {
        if (typeof value !== 'number') {
            throw new Error('Age must be a number');
        }
        if (value < 0 || value > 150) {
            throw new Error('Age must be between 0 and 150');
        }
        this.#data.age = Math.floor(value);
    }
    
    get email() {
        return this.#data.email;
    }
    
    set email(value) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            throw new Error('Invalid email format');
        }
        this.#data.email = value.toLowerCase().trim();
    }
}

const user = new User({});
user.name = '  Alice  ';
user.age = 30.7;
user.email = 'ALICE@EXAMPLE.COM';

console.log(user.name);   // 'Alice'
console.log(user.age);    // 30
console.log(user.email);  // 'alice@example.com'
```

### Lazy Evaluation

```javascript
// Lazy computed properties
class HeavyComputation {
    constructor(data) {
        this._data = data;
        this._computed = null;
    }
    
    get expensiveResult() {
        if (this._computed === null) {
            console.log('Computing...');
            this._computed = this._performHeavyCalculation();
        }
        return this._computed;
    }
    
    _performHeavyCalculation() {
        // Simulate heavy computation
        let result = 0;
        for (let i = 0; i < 1000000; i++) {
            result += Math.sqrt(i);
        }
        return result;
    }
}

const calc = new HeavyComputation('data');
console.log('Before first access');
console.log(calc.expensiveResult);  // Logs 'Computing...'
console.log('After first access');
console.log(calc.expensiveResult);  // No recomputation
```

### Caching Getter

```javascript
// Caching pattern
class CacheableData {
    #cache = new Map();
    
    constructor(fetcher) {
        this.fetcher = fetcher;
    }
    
    getData(key) {
        if (this.#cache.has(key)) {
            return this.#cache.get(key);
        }
        
        const value = this.fetcher(key);
        this.#cache.set(key, value);
        return value;
    }
    
    invalidate(key) {
        this.#cache.delete(key);
    }
    
    clear() {
        this.#cache.clear();
    }
}

const cache = new CacheableData(key => {
    console.log(`Fetching ${key}...`);
    return { key, data: `Data for ${key}`, timestamp: Date.now() };
});

console.log('First access:');
console.log(cache.getData('user'));  // Logs 'Fetching user...'

console.log('\nSecond access:');
console.log(cache.getData('user'));  // No fetch
```

### Access Control Pattern

```javascript
// Role-based access control
class ProtectedUser {
    #privateData = {};
    #permissions = new Set();
    
    constructor(data, permissions = []) {
        this.#privateData = { ...data };
        this.#permissions = new Set(permissions);
    }
    
    get _data() {
        return this.#privateData;
    }
    
    canRead(field) {
        return this.#permissions.has('admin') || 
               this.#permissions.has(`read:${field}`);
    }
    
    canWrite(field) {
        return this.#permissions.has('admin') || 
               this.#permissions.has(`write:${field}`);
    }
    
    getField(field) {
        if (!this.canRead(field)) {
            throw new Error(`No permission to read ${field}`);
        }
        return this.#privateData[field];
    }
    
    setField(field, value) {
        if (!this.canWrite(field)) {
            throw new Error(`No permission to write ${field}`);
        }
        this.#privateData[field] = value;
    }
}

const user = new ProtectedUser(
    { ssn: '123-45-6789', email: 'test@example.com' },
    ['read:email', 'write:email']
);

console.log(user.getField('email'));  // 'test@example.com'
// console.log(user.getField('ssn')); // Error: No permission
```

---

## Property Enumerability and Configurability

These attributes control whether properties appear in iterations and whether they can be modified.

### Enumerable Properties

```javascript
// Enumerable properties in iteration
const obj = {
    a: 1,
    b: 2,
    c: 3
};

Object.defineProperty(obj, 'd', {
    value: 4,
    enumerable: false
});

console.log(Object.keys(obj));    // ['a', 'b', 'c']
console.log(Object.values(obj));  // [1, 2, 3]
console.log(Object.entries(obj)); // [['a', 1], ['b', 2], ['c', 3]]

// for...in skips non-enumerable
for (const key in obj) {
    console.log(key);  // a, b, c
}

// JSON.stringify also uses enumerable
console.log(JSON.stringify(obj));  // {"a":1,"b":2,"c":3}
```

### Non-Enumerable Methods

```javascript
// Hiding methods from iteration
const calculator = {
    _value: 0,
    
    add(n) {
        this._value += n;
        return this;
    },
    
    subtract(n) {
        this._value -= n;
        return this;
    },
    
    get value() {
        return this._value;
    }
};

// Make methods non-enumerable
Object.defineProperties(calculator, {
    add: { enumerable: false },
    subtract: { enumerable: false },
    value: { enumerable: false }
});

console.log(Object.keys(calculator));  // ['_value']
console.log(JSON.stringify(calculator));  // {"_value":0}
```

### Configurable Properties

```javascript
// Configurable controls
const obj = { a: 1 };

Object.defineProperty(obj, 'immutable', {
    value: 100,
    writable: false,
    enumerable: true,
    configurable: false
});

// Cannot delete or reconfigure non-configurable property
delete obj.immutable;  // Silently fails
console.log(obj.immutable);  // 100

Object.defineProperty(obj, 'immutable', {
    value: 200
});  // TypeError in strict mode
```

### Sealing Objects

```javascript
// Object.seal and Object.freeze
const sealed = { a: 1 };
Object.seal(sealed);

sealed.a = 2;       // Works - existing properties remain writable
sealed.b = 3;       // Silently fails - cannot add new properties
delete sealed.a;   // Silently fails - cannot delete

console.log(Object.isSealed(sealed));  // true
console.log(sealed);                   // { a: 2 }

const frozen = { x: 1 };
Object.freeze(frozen);

frozen.x = 2;       // Silently fails
frozen.y = 3;       // Silently fails

console.log(Object.isFrozen(frozen));  // true
```

---

## Professional Use Cases

### Use Case 1: Immutable Data Store

```javascript
// Immutable state management
class ImmutableStore {
    #state;
    #history = [];
    #maxHistory;
    
    constructor(initialState = {}, maxHistory = 50) {
        this.#state = Object.freeze({ ...initialState });
        this.#maxHistory = maxHistory;
    }
    
    get state() {
        return this.#state;
    }
    
    setState(updater) {
        const previousState = this.#state;
        
        const newState = typeof updater === 'function'
            ? updater(previousState)
            : { ...previousState, ...updater };
        
        if (this.#deepEqual(previousState, newState)) {
            return this;
        }
        
        // Save to history
        this.#history.push(Object.freeze(previousState));
        if (this.#history.length > this.#maxHistory) {
            this.#history.shift();
        }
        
        this.#state = Object.freeze(newState);
        return this;
    }
    
    #deepEqual(a, b) {
        return JSON.stringify(a) === JSON.stringify(b);
    }
    
    undo() {
        if (this.#history.length === 0) return null;
        const previous = this.#history.pop();
        this.#state = Object.freeze(previous);
        return this;
    }
}

const store = new ImmutableStore({ count: 0 });
store.setState({ count: 1 });
store.setState({ count: 2 });
console.log(store.state.count);  // 2

store.undo();
console.log(store.state.count);  // 1
```

### Use Case 2: Reactive Data Binding

```javascript
// Simple reactive data binding
class ReactiveObject {
    #data = {};
    #listeners = new Map();
    
    constructor(initialData = {}) {
        this.#data = { ...initialData };
    }
    
    #notify(property, value) {
        if (this.#listeners.has(property)) {
            this.#listeners.get(property).forEach(callback => callback(value));
        }
    }
    
    get(key) {
        return this.#data[key];
    }
    
    set(key, value) {
        if (this.#data[key] === value) return;
        
        const oldValue = this.#data[key];
        this.#data[key] = value;
        this.#notify(key, { oldValue, newValue: value });
    }
    
    on(key, callback) {
        if (!this.#listeners.has(key)) {
            this.#listeners.set(key, new Set());
        }
        this.#listeners.get(key).add(callback);
        
        return () => this.#listeners.get(key).delete(callback);
    }
}

const user = new ReactiveObject({ name: 'Alice', age: 30 });

const unsubscribe = user.on('name', ({ oldValue, newValue }) => {
    console.log(`Name changed from ${oldValue} to ${newValue}`);
});

user.set('name', 'Bob');  // Logs: Name changed from Alice to Bob
user.set('age', 31);     // No log for age
unsubscribe();
user.set('name', 'Carol');  // No log - unsubscribed
```

### Use Case 3: Virtual Properties

```javascript
// Virtual computed properties
class VirtualObject {
    #source = {};
    
    constructor(source) {
        this.#source = source;
    }
    
    get fullName() {
        return `${this.#source.firstName || ''} ${this.#source.lastName || ''}`.trim();
    }
    
    get initials() {
        const first = this.#source.firstName?.[0] || '';
        const last = this.#source.lastName?.[0] || '';
        return (first + last).toUpperCase();
    }
    
    get isComplete() {
        return !!(this.#source.firstName && this.#source.lastName && this.#source.email);
    }
    
    get ageCategory() {
        const age = this.#source.age;
        if (age < 18) return 'minor';
        if (age < 65) return 'adult';
        return 'senior';
    }
}

const user = new VirtualObject({
    firstName: 'John',
    lastName: 'Doe',
    email: 'john@example.com',
    age: 30
});

console.log(user.fullName);    // 'John Doe'
console.log(user.initials);    // 'JD'
console.log(user.isComplete);  // true
console.log(user.ageCategory); // 'adult'
```

### Use Case 4: Validation Schema

```javascript
// Property validation schema
class ValidatedObject {
    #validators = new Map();
    #data = {};
    
    addField(field, validator) {
        this.#validators.set(field, validator);
        return this;
    }
    
    setData(data) {
        const errors = {};
        
        for (const [field, validator] of this.#validators) {
            if (field in data) {
                const result = validator.validate(data[field], data);
                if (!result.valid) {
                    errors[field] = result.error;
                }
            } else if (validator.required) {
                errors[field] = `${field} is required`;
            }
        }
        
        if (Object.keys(errors).length > 0) {
            throw new Error(Object.values(errors).join(', '));
        }
        
        this.#data = { ...data };
        return this;
    }
    
    get(field) {
        return this.#data[field];
    }
}

const validator = new ValidatedObject()
    .addField('email', {
        required: true,
        validate: (value) => ({
            valid: /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value),
            error: 'Invalid email format'
        })
    })
    .addField('age', {
        required: true,
        validate: (value) => ({
            valid: typeof value === 'number' && value >= 0 && value <= 150,
            error: 'Age must be between 0 and 150'
        })
    })
    .addField('name', {
        required: true,
        validate: (value) => ({
            valid: typeof value === 'string' && value.length >= 2,
            error: 'Name must be at least 2 characters'
        })
    });

try {
    validator.setData({ email: 'invalid', age: 30, name: 'A' });
} catch (e) {
    console.log(e.message);  // 'Invalid email format, Name must be at least 2 characters'
}

validator.setData({ email: 'test@example.com', age: 30, name: 'Alice' });
console.log(validator.get('email'));  // 'test@example.com'
```

---

## Key Takeaways

1. **Property descriptors define behavior**: value, writable, enumerable, configurable
2. **Getters compute on access**: No storage, executed every access
3. **Setters validate on assignment**: Transform and validate before storage
4. **Computed property names**: Use expressions as property keys
5. **Non-enumerable hides from iteration**: Useful for methods
6. **Configurable controls modification**: false prevents deletion/reconfiguration
7. **Object.seal() prevents additions**: Existing properties remain modifiable
8. **Object.freeze() prevents all changes**: Full immutability

---

## Common Pitfalls

1. **Getters without storage**: Values recomputed every access
2. **Not returning this in setters**: Breaks method chaining
3. **Forgetting enumerable default**: false in defineProperty vs true in literals
4. **Confusing writable with configurable**: Different semantics
5. **Private fields vs closures**: Private fields are truly private
6. **Circular getters**: Infinite loops if getters reference each other
7. **JSON.stringify ignores functions**: Not serialized
8. **Freeze is shallow**: Nested objects remain mutable

---

## Related Files

- **01_OBJECT_LITERALS_AND_CREATION.md**: Object literal syntax
- **02_PROTOTYPES_DEEP_DIVE.md**: Prototype chain
- **03_CONSTRUCTOR_FUNCTIONS.md**: Constructor patterns
- **04_ECMASCRIPT_CLASSES_SYNTAX.md**: Class syntax
- **06_OBJECT_METHODS_AND_UTILITIES.md**: Object methods
- **08_OBJECT_SECURITY_PATTERNS.md**: Object protection