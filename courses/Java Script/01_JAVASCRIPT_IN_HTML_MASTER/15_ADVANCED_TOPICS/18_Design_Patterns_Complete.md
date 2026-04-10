# 🎨 Design Patterns Complete Guide

## 📋 Overview

Design patterns are reusable solutions to commonly occurring problems in software design. Understanding these patterns helps write more maintainable, scalable, and professional JavaScript code.

---

## 🎯 Why Design Patterns Matter

| Benefit | Description |
|---------|-------------|
| **Reusability** | Proven solutions can be reused |
| **Communication** | Common vocabulary for teams |
| **Best Practices** | Learn from experienced developers |
| **Maintainability** | Easier to modify and extend |

---

## 🎯 Creational Patterns

### 1. Factory Pattern

Creates objects without specifying exact class.

```javascript
// Factory function
function createUser(type, data) {
    switch(type) {
        case 'admin':
            return new AdminUser(data);
        case 'guest':
            return new GuestUser(data);
        case 'premium':
            return new PremiumUser(data);
        default:
            throw new Error('Unknown user type');
    }
}

// Usage
const user = createUser('admin', { name: 'John', permissions: ['all'] });
```

### 2. Singleton Pattern

Ensures only one instance exists.

```javascript
class Database {
    constructor() {
        if (Database.instance) {
            return Database.instance;
        }
        this.connection = this.connect();
        Database.instance = this;
    }
    
    connect() {
        return 'Connected to database';
    }
}

const db1 = new Database();
const db2 = new Database();
console.log(db1 === db2); // true - same instance
```

### 3. Builder Pattern

Constructs complex objects step by step.

```javascript
class QueryBuilder {
    constructor() {
        this.query = { select: [], from: '', where: [], orderBy: [] };
    }
    
    select(...fields) {
        this.query.select = fields;
        return this;
    }
    
    from(table) {
        this.query.from = table;
        return this;
    }
    
    where(condition) {
        this.query.where.push(condition);
        return this;
    }
    
    orderBy(field, direction = 'ASC') {
        this.query.orderBy.push({ field, direction });
        return this;
    }
    
    build() {
        return `SELECT ${this.query.select.join(', ')} FROM ${this.query.from}`;
    }
}

// Usage
const query = new QueryBuilder()
    .select('name', 'email')
    .from('users')
    .where('active = true')
    .orderBy('name')
    .build();
```

---

## 🎯 Structural Patterns

### 4. Module Pattern

Encapsulates code and creates private scope.

```javascript
const CounterModule = (function() {
    // Private variables
    let count = 0;
    
    // Private function
    function increment() {
        count++;
    }
    
    // Public API
    return {
        getCount: () => count,
        increment: () => increment(),
        reset: () => count = 0
    };
})();

CounterModule.increment();
CounterModule.increment();
console.log(CounterModule.getCount()); // 2
// count is not accessible directly
```

### 5. Decorator Pattern

Adds behavior dynamically.

```javascript
function withLogging(fn) {
    return function(...args) {
        console.log(`Calling ${fn.name} with`, args);
        const result = fn.apply(this, args);
        console.log(`Result:`, result);
        return result;
    };
}

function calculate(a, b) {
    return a + b;
}

const loggedCalculate = withLogging(calculate);
loggedCalculate(2, 3);
```

### 6. Composite Pattern

Treats individual and composite objects uniformly.

```javascript
class Component {
    render() { }
}

class Leaf extends Component {
    constructor(content) {
        super();
        this.content = content;
    }
    
    render() {
        return `<div>${this.content}</div>`;
    }
}

class Container extends Component {
    constructor() {
        super();
        this.children = [];
    }
    
    add(component) {
        this.children.push(component);
        return this;
    }
    
    render() {
        return `<div>${this.children.map(c => c.render()).join('')}</div>`;
    }
}

// Usage
const page = new Container()
    .add(new Leaf('Header'))
    .add(new Container()
        .add(new Leaf('Content 1'))
        .add(new Leaf('Content 2')))
    .add(new Leaf('Footer'));
```

---

## 🎯 Behavioral Patterns

### 7. Observer Pattern

Defines one-to-many dependency.

```javascript
class EventEmitter {
    constructor() {
        this.events = {};
    }
    
    on(event, callback) {
        if (!this.events[event]) {
            this.events[event] = [];
        }
        this.events[event].push(callback);
    }
    
    emit(event, ...args) {
        if (this.events[event]) {
            this.events[event].forEach(cb => cb(...args));
        }
    }
    
    off(event, callback) {
        if (this.events[event]) {
            this.events[event] = this.events[event].filter(cb => cb !== callback);
        }
    }
}

// Usage
const emitter = new EventEmitter();

emitter.on('message', (data) => {
    console.log('Handler 1:', data);
});

emitter.on('message', (data) => {
    console.log('Handler 2:', data);
});

emitter.emit('message', 'Hello!'); // Both handlers called
```

### 8. Strategy Pattern

Defines interchangeable algorithms.

```javascript
class PaymentProcessor {
    constructor(strategy) {
        this.strategy = strategy;
    }
    
    setStrategy(strategy) {
        this.strategy = strategy;
    }
    
    process(amount) {
        return this.strategy.pay(amount);
    }
}

class CreditCardPayment {
    pay(amount) {
        return `Paid ${amount} via Credit Card`;
    }
}

class PayPalPayment {
    pay(amount) {
        return `Paid ${amount} via PayPal`;
    }
}

class CryptoPayment {
    pay(amount) {
        return `Paid ${amount} via Crypto`;
    }
}

// Usage
const processor = new PaymentProcessor(new CreditCardPayment());
console.log(processor.process(100)); // Credit Card

processor.setStrategy(new PayPalPayment());
console.log(processor.process(100)); // PayPal
```

### 9. State Pattern

Allows object to alter behavior based on state.

```javascript
class Order {
    constructor() {
        this.state = 'pending';
    }
    
    setState(state) {
        this.state = state;
    }
    
    getStatus() {
        switch(this.state) {
            case 'pending':
                return 'Order is being processed';
            case 'shipped':
                return 'Order has been shipped';
            case 'delivered':
                return 'Order delivered';
            case 'cancelled':
                return 'Order cancelled';
            default:
                return 'Unknown state';
        }
    }
    
    // State-specific behavior
    ship() {
        if (this.state === 'pending') {
            this.state = 'shipped';
            return 'Order shipped!';
        }
        return 'Cannot ship in current state';
    }
    
    deliver() {
        if (this.state === 'shipped') {
            this.state = 'delivered';
            return 'Order delivered!';
        }
        return 'Cannot deliver in current state';
    }
}
```

### 10. Dependency Injection

Provides dependencies from external source.

```javascript
// Without DI - tight coupling
class UserService {
    constructor() {
        this.db = new Database(); // Hard dependency
    }
}

// With DI - loose coupling
class UserService {
    constructor(database) {
        this.db = database;
    }
    
    getUser(id) {
        return this.db.findById(id);
    }
}

// Inject dependency
const db = new Database();
const userService = new UserService(db);
```

---

## 🎯 Modern JavaScript Patterns

### Module Pattern (ES6)

```javascript
// math.js
export const add = (a, b) => a + b;
export const subtract = (a, b) => a - b;

// app.js
import { add, subtract } from './math.js';
```

### Class Factory Pattern

```javascript
function createClass(BaseClass) {
    return class Extended extends BaseClass {
        newMethod() {
            return 'Extended!';
        }
    };
}
```

### Mixin Pattern

```javascript
const withLogger = {
    log(message) {
        console.log(`[LOG]: ${message}`);
    }
};

const withValidator = {
    validate(value) {
        return value !== null && value !== undefined;
    }
};

class User {
    constructor(name) {
        this.name = name;
    }
}

Object.assign(User.prototype, withLogger, withValidator);

const user = new User('John');
user.log('User created'); // [LOG]: User created
console.log(user.validate(user.name)); // true
```

---

## 🔗 Related Topics

- [07_Functions_Complete_Guide.md](../02_JAVASCRIPT_SYNTAX_AND_BASICS/07_Functions_Complete_Guide.md)
- [04_Variables_Deep_Dive.md](../02_JAVASCRIPT_SYNTAX_AND_BASICS/04_Variables_Deep_Dive.md)

---

**Next: [SOLID Principles JavaScript](./19_SOLID_Principles_JavaScript.md)**