# JavaScript ECMAScript Classes Syntax: Complete Mastery Guide

ES6 class syntax provides a cleaner, more familiar approach to object-oriented programming in JavaScript while building on the underlying prototype-based system. This comprehensive guide covers class declarations, expressions, inheritance with extends, super keyword, static methods, private fields, and modern ES2024+ features. Understanding class syntax is essential for building robust JavaScript applications with modern frameworks like React, Vue, and Angular.

---

## Table of Contents

1. [Class Declarations](#class-declarations)
2. [Class Expressions](#class-expressions)
3. [Constructor and Instance Properties](#constructor-and-instance-properties)
4. [Methods and Getters/Setters](#methods-and-getterssetters)
5. [Static Methods and Properties](#static-methods-and-properties)
6. [Private Fields and Methods](#private-fields-and-methods)
7. [Inheritance with extends](#inheritance-with-extends)
8. [Super Keyword](#super-keyword)
9. [Key Takeaways](#key-takeaways)
10. [Common Pitfalls](#common-pitfalls)
11. [Related Files](#related-files)

---

## Class Declarations

Class declarations define blueprints for creating objects with shared behavior through prototype methods.

### Basic Class Declaration

```javascript
// Basic class declaration
class User {
    // Constructor - called when creating new instance
    constructor(name, email) {
        this.name = name;
        this.email = email;
        this.isActive = true;
        this.createdAt = new Date();
    }
    
    // Instance method
    greet() {
        return `Hello, I'm ${this.name}`;
    }
    
    // Method returning new object state
    deactivate() {
        this.isActive = false;
        return this;
    }
}

// Creating instances
const user1 = new User('Alice', 'alice@example.com');
const user2 = new User('Bob', 'bob@example.com');

console.log(user1.greet());  // 'Hello, I'm Alice'
console.log(user2.greet());  // 'Hello, I'm Bob'

// Verify class
console.log(user1 instanceof User);  // true
console.log(typeof User);  // 'function'
```

### Class vs Function Constructor

Classes have strict mode semantics and different behavior than traditional function constructors.

```javascript
// Class behavior differences
class Counter {
    constructor(count) {
        this.count = count;
    }
    
    increment() {
        return ++this.count;
    }
}

// Class methods are non-enumerable
const descriptor = Object.getOwnPropertyDescriptor(Counter.prototype, 'increment');
console.log(descriptor.enumerable);  // false

// Strict mode enforcement - calling without new throws error
try {
    const bad = Counter(5);
} catch (e) {
    console.log('Error:', e.message);  // Class constructor Counter cannot be invoked without 'new'
}

// Proper instantiation
const counter = new Counter(0);
console.log(counter.increment());  // 1
```

### Class Name and Hoisting

Classes follow different hoisting rules than function declarations.

```javascript
// Function declarations are hoisted
const func = createFunction();
function createFunction() {
    return 'function';
}

// Class declarations are not hoisted
// const instance = new MyClass();  // ReferenceError: Cannot access 'MyClass' before initialization

class MyClass {
    constructor(value) {
        this.value = value;
    }
}

// After declaration, works normally
const instance = new MyClass(10);
console.log(instance.value);  // 10
```

---

## Class Expressions

Class expressions provide flexibility for dynamic class creation and singleton patterns.

### Basic Class Expression

```javascript
// Basic class expression
const Animal = class {
    constructor(name) {
        this.name = name;
    }
    
    speak() {
        return `${this.name} makes a sound`;
    }
};

const animal = new Animal('Rex');
console.loganimal.speak());  // 'Rex makes a sound'

// Named class expression
const User = class UserClass {
    constructor(name) {
        this.name = name;
    }
    
    getClassName() {
        return UserClass.name;
    }
};

const user = new User('Alice');
console.log(user.getClassName());  // 'UserClass'
```

### Dynamic Class Creation

```javascript
// Dynamic class based on configuration
function createClass(config) {
    const { name, properties, methods } = config;
    
    return class {
        constructor(...args) {
            properties.forEach((prop, i) => {
                this[prop] = args[i];
            });
        }
        
        // Add methods
        ...Object.entries(methods).reduce((acc, [key, fn]) => ({
            ...acc,
            [key]: fn
        }), {})
    };
}

const Point = createClass({
    name: 'Point',
    properties: ['x', 'y'],
    methods: {
        distance() {
            return Math.sqrt(this.x ** 2 + this.y ** 2);
        },
        toString() {
            return `(${this.x}, ${this.y})`;
        }
    }
});

const point = new Point(3, 4);
console.log(point.distance());  // 5
console.log(point.toString());  // (3, 4)
```

### Singleton Pattern with Class Expression

```javascript
// Singleton using class expression
const Database = new class {
    constructor() {
        this.connected = false;
        this.connection = null;
    }
    
    connect(config = {}) {
        console.log('Connecting to database...');
        this.connection = config;
        this.connected = true;
        return this;
    }
    
    disconnect() {
        console.log('Disconnecting...');
        this.connected = false;
        this.connection = null;
    }
    
    getConnection() {
        return this.connection;
    }
};

// Usage - already instantiated
Database.connect({ host: 'localhost', port: 5432 });
console.log(Database.connected);  // true

// Cannot create new instances
// const db2 = new Database();  // Error
```

---

## Constructor and Instance Properties

The constructor method serves as the initialization point for new instances.

### Constructor Properties

```javascript
// Comprehensive constructor
class User {
    constructor(name, email, role = 'user') {
        // Validate required parameters
        if (!name || typeof name !== 'string') {
            throw new Error('Name is required');
        }
        
        if (!email || !email.includes('@')) {
            throw new Error('Valid email is required');
        }
        
        // Initialize properties
        this.name = name;
        this.email = email;
        this.role = role;
        this.isActive = true;
        this.createdAt = new Date();
        this.lastLogin = null;
        
        // Generate unique ID
        this.id = this.constructor.generateId();
    }
    
    // Static method for ID generation
    static generateId() {
        return `user-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
    }
}

const user = new User('Alice', 'alice@example.com', 'admin');
console.log(user.id);        // 'user-...'
console.log(user.name);     // 'Alice'
console.log(user.role);     // 'admin'
console.log(user instanceof User);  // true
```

### Instance Property Initializers

ES2022 allows instance field declarations outside the constructor.

```javascript
// Instance field declarations
class Counter {
    // Public fields
    count = 0;
    name = 'Counter';
    
    // Fields with initial values
    lastUpdated = new Date();
    
    // Constructor
    constructor(initialCount = 0) {
        if (initialCount < 0) {
            throw new Error('Count cannot be negative');
        }
        this.count = initialCount;
    }
    
    increment() {
        this.count++;
        this.lastUpdated = new Date();
        return this.count;
    }
}

const counter = new Counter(10);
console.log(counter.count);       // 10
console.log(counter.lastUpdated);  // Date object
```

---

## Methods and Getters/Setters

Classes support various method types including regular methods, getters, setters, and static methods.

### Instance Methods

```javascript
// Instance methods
class Calculator {
    constructor(initial = 0) {
        this.value = initial;
        this.history = [];
    }
    
    add(n) {
        this.history.push({ op: 'add', value: n, result: this.value + n });
        this.value += n;
        return this;
    }
    
    subtract(n) {
        this.value -= n;
        return this;
    }
    
    multiply(n) {
        this.value *= n;
        return this;
    }
    
    getResult() {
        return this.value;
    }
    
    getHistory() {
        return [...this.history];
    }
    
    reset() {
        this.value = 0;
        this.history = [];
        return this;
    }
}

const calc = new Calculator(10);
calc.add(5).multiply(2).subtract(10);
console.log(calc.getResult());  // 20
console.log(calc.getHistory());  // [{op: 'add', value: 5, result: 15}, ...]
```

### Getters and Setters

```javascript
// Getters and setters
class User {
    constructor(name, email) {
        this._name = name;
        this._email = email;
    }
    
    // Getter
    get name() {
        return this._name;
    }
    
    // Setter
    set name(value) {
        if (!value || typeof value !== 'string') {
            throw new Error('Name must be a non-empty string');
        }
        this._name = value.trim();
    }
    
    get email() {
        return this._email;
    }
    
    set email(value) {
        if (!value.includes('@')) {
            throw new Error('Invalid email format');
        }
        this._email = value.toLowerCase();
    }
    
    // Computed property
    get displayName() {
        return `${this._name} <${this._email}>`;
    }
    
    // Read-only property
    get createdAt() {
        return this._createdAt;
    }
}

const user = new User('Alice', 'Alice@EXAMPLE.COM');
console.log(user.name);          // 'Alice'
console.log(user.email);         // 'alice@example.com'
console.log(user.displayName);   // 'Alice <alice@example.com>'

user.name = '  Bob  ';  // Trimmed automatically
console.log(user.name);  // 'Bob'
```

### Static Methods and Properties

Static methods belong to the class itself, not instances.

```javascript
// Static methods
class MathUtils {
    static readonly PI = 3.14159;
    
    static degreesToRadians(degrees) {
        return degrees * (Math.PI / 180);
    }
    
    static radiansToDegrees(radians) {
        return radians * (180 / Math.PI);
    }
    
    static clamp(value, min, max) {
        return Math.max(min, Math.min(max, value));
    }
    
    static randomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }
    
    // Static private field (ES2024+)
    static #cache = new Map();
    
    static cache(key, value) {
        if (value !== undefined) {
            this.#cache.set(key, value);
            return value;
        }
        return this.#cache.get(key);
    }
}

console.log(MathUtils.degreesToRadians(90));  // ~1.57
console.log(MathUtils.clamp(5, 0, 10));  // 5
console.log(MathUtils.clamp(-5, 0, 10));  // 0
```

---

## Private Fields and Methods

ES2022 introduced true private fields using the # prefix, providing encapsulation beyond closures.

### Private Fields

```javascript
// Private fields
class BankAccount {
    // Private fields
    #balance;
    #transactions;
    #accountNumber;
    
    constructor(initialBalance = 0) {
        if (initialBalance < 0) {
            throw new Error('Initial balance cannot be negative');
        }
        this.#balance = initialBalance;
        this.#transactions = [];
        this.#accountNumber = this.#generateAccountNumber();
    }
    
    #generateAccountNumber() {
        return 'ACCT-' + Date.now().toString(36).toUpperCase();
    }
    
    // Public getters
    get balance() {
        return this.#balance;
    }
    
    get accountNumber() {
        return this.#accountNumber;
    }
    
    get transactions() {
        return [...this.#transactions];
    }
    
    // Public methods
    deposit(amount) {
        if (amount <= 0) {
            throw new Error('Deposit amount must be positive');
        }
        this.#balance += amount;
        this.#transactions.push({
            type: 'deposit',
            amount,
            date: new Date()
        });
        return this;
    }
    
    withdraw(amount) {
        if (amount <= 0) {
            throw new Error('Withdrawal amount must be positive');
        }
        if (amount > this.#balance) {
            throw new Error('Insufficient funds');
        }
        this.#balance -= amount;
        this.#transactions.push({
            type: 'withdraw',
            amount,
            date: new Date()
        });
        return this;
    }
}

const account = new BankAccount(1000);
console.log(account.balance);       // 1000
console.log(account.accountNumber);  // 'ACCT-...'

account.deposit(500);
console.log(account.balance);     // 1500

// Cannot access private fields directly
// console.log(account.#balance);  // SyntaxError: Private field
```

### Private Methods

```javascript
// Private methods
class Encoder {
    #data = [];
    
    add(value) {
        if (this.#validate(value)) {
            this.#data.push(value);
            return true;
        }
        return false;
    }
    
    #validate(value) {
        if (typeof value === 'string') {
            return value.length > 0;
        }
        if (typeof value === 'number') {
            return value > 0;
        }
        return value !== null;
    }
    
    #process(item) {
        return { value: item, encoded: true };
    }
    
    encodeAll() {
        return this.#data.map(item => this.#process(item));
    }
}

const encoder = new Encoder();
encoder.add('hello');
encoder.add(42);
console.log(encoder.encodeAll());  // [{value: 'hello', encoded: true}, ...]
```

### Private Static Fields and Methods

```javascript
// Private static members
class Config {
    static #defaults = {
        timeout: 5000,
        retries: 3,
        debug: false
    };
    
    static #validateConfig(config) {
        return config && typeof config === 'object';
    }
    
    static getDefaults() {
        return { ...this.#defaults };
    }
    
    static merge(config) {
        if (!this.#validateConfig(config)) {
            return this.getDefaults();
        }
        return { ...this.#defaults, ...config };
    }
}

console.log(Config.getDefaults());  // { timeout: 5000, retries: 3, debug: false }
console.log(Config.merge({ timeout: 10000 }));  // { timeout: 10000, retries: 3, debug: false }
```

---

## Inheritance with extends

Class inheritance uses the extends keyword to create subclass relationships.

### Basic Inheritance

```javascript
// Basic class inheritance
class Animal {
    constructor(name) {
        this.name = name;
        this.isAlive = true;
    }
    
    speak() {
        return `${this.name} makes a sound`;
    }
    
    move() {
        return `${this.name} moves`;
    }
    
    die() {
        this.isAlive = false;
        return `${this.name} has died`;
    }
}

class Dog extends Animal {
    constructor(name, breed) {
        super(name);  // Call parent constructor
        this.breed = breed;
    }
    
    // Override parent method
    speak() {
        return `${this.name} barks`;
    }
    
    // New method specific to Dog
    fetch() {
        return `${this.name} fetches the ball`;
    }
}

const dog = new Dog('Rex', 'German Shepherd');
console.log(dog.name);         // 'Rex'
console.log(dog.breed);       // 'German Shepherd'
console.log(dog.speak());      // 'Rex barks'
console.log(dog.fetch());      // 'Rex fetches the ball'
console.log(dog.move());        // 'Rex moves'
```

### Multi-Level Inheritance

```javascript
// Multi-level inheritance
class LivingThing {
    constructor(name) {
        this.name = name;
        this.createdAt = new Date();
    }
    
    breathe() {
        return `${this.name} breathes`;
    }
}

class Animal extends LivingThing {
    constructor(name, species) {
        super(name);
        this.species = species;
        this.isAlive = true;
    }
    
    move() {
        return `${this.name} moves`;
    }
}

class Mammal extends Animal {
    constructor(name, species, furColor) {
        super(name, species);
        this.furColor = furColor;
        this.warmBlooded = true;
    }
    
    regulateTemperature() {
        return `${this.name} maintains body temperature`;
    }
}

class Dog extends Mammal {
    constructor(name, breed, furColor) {
        super(name, 'Canis familiaris', furColor);
        this.breed = breed;
    }
    
    speak() {
        return `${this.name} barks`;
    }
}

const dog = new Dog('Rex', 'Labrador', 'golden');
console.log(dog.name);                  // 'Rex'
console.log(dog.breed);               // 'Labrador'
console.log(dog.breathe());           // 'Rex breathes'
console.log(dog.regulateTemperature()); // 'Rex maintains body temperature'
console.log(dog.speak());            // 'Rex barks'
```

### Multiple Levels with super

```javascript
// Calling super methods
class Shape {
    constructor(color) {
        this.color = color;
    }
    
    describe() {
        return `A ${this.color} shape`;
    }
}

class Circle extends Shape {
    constructor(color, radius) {
        super(color);
        this.radius = radius;
    }
    
    describe() {
        return `${super.describe()} with radius ${this.radius}`;
    }
    
    area() {
        return Math.PI * this.radius ** 2;
    }
}

class ColoredCircle extends Circle {
    constructor(color, radius, pattern) {
        super(color, radius);
        this.pattern = pattern;
    }
    
    describe() {
        return `${super.describe()} and ${this.pattern} pattern`;
    }
}

const circle = new ColoredCircle('red', 5, 'striped');
console.log(circle.describe());  // 'A red shape with radius 5 and striped pattern'
console.log(circle.area());    // 78.54...
```

---

## Key Takeaways

1. **Class declarations are not hoisted**: Must be declared before use
2. **Constructor runs on new**: Initializes instance properties
3. **Getters compute values**: No storage, computed on access
4. **Private fields use #**: True encapsulation, ES2022+
5. **Extends creates inheritance**: Prototype chain is automatic
6. **Super must be called before this**: In constructors
7. **Static methods belong to class**: Not instances
8. **Classes execute in strict mode**: Automatic enforcement

---

## Common Pitfalls

1. **Forgetting to call super**: Must call in constructors
2. **Calling super after this**: Must call before accessing this
3. **Private field name typos**: Different names create new fields
4. **Static methods aren't inherited**: Each class has its own
5. **Getters without setters**: No write attempted will fail silently
6. **Returning primitives from constructor**: Ignored, returns this
7. **Extending built-in types**: Requires modern approaches
8. **Confusing extends with implements**: Different features

---

## Related Files

- **01_OBJECT_LITERALS_AND_CREATION.md**: Object literal syntax and patterns
- **02_PROTOTYPES_DEEP_DIVE.md**: Prototype chain internals
- **03_CONSTRUCTOR_FUNCTIONS.md**: Constructor patterns
- **05_GETTERS_SETTERS_AND_PROPERTIES.md**: Property descriptors
- **07_INHERITANCE_PATTERNS.md**: Classical and composition
- **08_OBJECT_SECURITY_PATTERNS.md**: Object protection