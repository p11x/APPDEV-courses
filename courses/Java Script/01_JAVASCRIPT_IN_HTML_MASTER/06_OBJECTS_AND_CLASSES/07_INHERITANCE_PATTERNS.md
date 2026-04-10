# JavaScript Inheritance Patterns: Complete Mastery Guide

JavaScript provides multiple patterns for implementing inheritance, from classical prototypal inheritance to modern class-based inheritance and composition. Understanding these patterns enables you to build flexible, maintainable object hierarchies while choosing the right approach for each situation. This comprehensive guide covers classical inheritance, prototypal inheritance, ES6 class inheritance, mixins, and composition over inheritance.

---

## Table of Contents

1. [Classical Inheritance Patterns](#classical-inheritance-patterns)
2. [Prototypal Inheritance](#prototypal-inheritance)
3. [ES6 Class Inheritance](#es6-class-inheritance)
4. [Mixin Pattern](#mixin-pattern)
5. [Composition Over Inheritance](#composition-over-inheritance)
6. [Hybrid Patterns](#hybrid-patterns)
7. [Professional Use Cases](#professional-use-cases)
8. [Key Takeaways](#key-takeaways)
9. [Common Pitfalls](#common-pitfalls)
10. [Related Files](#related-files)

---

## Classical Inheritance Patterns

Classical JavaScript inheritance uses constructor functions and prototype chaining to establish parent-child relationships between objects.

### Constructor Chaining

```javascript
// Constructor function inheritance
function Animal(name) {
    this.name = name;
    this.isAlive = true;
}

Animal.prototype.speak = function() {
    return `${this.name} makes a sound`;
};

Animal.prototype.move = function() {
    return `${this.name} moves`;
};

function Dog(name, breed) {
    // Call parent constructor with current context
    Animal.call(this, name);
    this.breed = breed;
}

// Set up prototype chain
Dog.prototype = Object.create(Animal.prototype);
Dog.prototype.constructor = Dog;

// Override parent method
Dog.prototype.speak = function() {
    return `${this.name} barks`;
};

// Add new method
Dog.prototype.fetch = function() {
    return `${this.name} fetches the ball`;
};

const dog = new Dog('Rex', 'German Shepherd');

console.log(dog.name);           // 'Rex'
console.log(dog.breed);          // 'German Shepherd'
console.log(dog.speak());        // 'Rex barks'
console.log(dog.move());         // 'Rex moves'
console.log(dog.fetch());        // 'Rex fetches the ball'
console.log(dog instanceof Dog);      // true
console.log(dog instanceof Animal);   // true
```

### Multi-Level Classical Inheritance

```javascript
// Multi-level inheritance chain
function LivingThing(name) {
    this.name = name;
    this.createdAt = new Date();
}

LivingThing.prototype.exists = function() {
    return this.name !== undefined;
};

function Animal(name, species) {
    LivingThing.call(this, name);
    this.species = species;
    this.isAlive = true;
}

Animal.prototype = Object.create(LivingThing.prototype);
Animal.prototype.constructor = Animal;

Animal.prototype.breathe = function() {
    return `${this.name} breathes`;
};

Animal.prototype.die = function() {
    this.isAlive = false;
    return `${this.name} has died`;
};

function Mammal(name, species, furColor) {
    Animal.call(this, name, species);
    this.furColor = furColor;
    this.warmBlooded = true;
}

Mammal.prototype = Object.create(Animal.prototype);
Mammal.prototype.constructor = Mammal;

Mammal.prototype.sustainHeat = function() {
    return `${this.name} maintains body heat`;
};

function Dog(name, breed, furColor) {
    Mammal.call(this, name, 'Canis familiaris', furColor);
    this.breed = breed;
}

Dog.prototype = Object.create(Mammal.prototype);
Dog.prototype.constructor = Dog;

Dog.prototype.speak = function() {
    return `${this.name} barks`;
};

const dog = new Dog('Rex', 'Labrador', 'golden');

console.log(dog.name);               // 'Rex'
console.log(dog.breathe());          // 'Rex breathes'
console.log(dog.sustainHeat());      // 'Rex maintains body heat'
console.log(dog.speak());            // 'Rex barks'
console.log(dog.die());              // 'Rex has died'
```

### instanceof Implementation

```javascript
// Understanding instanceof
function InstanceCheck() {}

const instance = new InstanceCheck();

console.log(instance instanceof InstanceCheck);           // true
console.log(instance instanceof Object);                  // true
console.log(instance instanceof Array);                   // false

// Custom instanceof-like function
function isInstanceOf(instance, Constructor) {
    if (instance === null) return false;
    if (typeof instance !== 'object') return false;
    
    let proto = Object.getPrototypeOf(instance);
    while (proto !== null) {
        if (proto === Constructor.prototype) return true;
        proto = Object.getPrototypeOf(proto);
    }
    
    return false;
}

function Parent() {}
function Child() {}
Child.prototype = Object.create(Parent.prototype);

const child = new Child();
console.log(isInstanceOf(child, Child));   // true
console.log(isInstanceOf(child, Parent));  // true
console.log(isInstanceOf(child, Object));  // true
```

---

## Prototypal Inheritance

Prototypal inheritance uses Object.create() to establish prototype chains directly, without constructor functions.

### Basic Prototypal Inheritance

```javascript
// Direct prototypal inheritance
const animal = {
    species: 'Unknown',
    
    speak() {
        return `${this.name} makes a sound`;
    },
    
    getInfo() {
        return {
            name: this.name,
            species: this.species
        };
    }
};

const dog = Object.create(animal);
dog.name = 'Rex';
dog.breed = 'Labrador';

console.log(dog.speak());  // 'Rex makes a sound'
console.log(dog.getInfo());  // { name: 'Rex', species: 'Unknown' }

console.log(Object.getPrototypeOf(dog) === animal);  // true
console.log(dog.hasOwnProperty('name'));             // true
console.log(dog.hasOwnProperty('species'));         // false - from prototype
```

### Factory with Prototypal Inheritance

```javascript
// Factory pattern with prototype inheritance
function createAnimal(prototype, props) {
    const obj = Object.create(prototype);
    Object.assign(obj, props);
    return Object.freeze(obj);
}

const birdPrototype = {
    fly() {
        return `${this.name} flies`;
    },
    speak() {
        return `${this.name} chirps`;
    }
};

const eagle = createAnimal(birdPrototype, {
    name: 'Eagle',
    wingspan: '2m',
    diet: 'carnivore'
});

console.log(eagle.fly());        // 'Eagle flies'
console.log(eagle.wingspan);    // '2m'
console.log(eagle.name);        // 'Eagle'
```

### Deep Prototype Chains

```javascript
// Creating complex prototype chains
const entity = {
    getId() {
        return this.id;
    },
    getCreatedAt() {
        return this.createdAt;
    }
};

const identifiable = Object.create(entity);
identifiable.getName = function() {
    return this.name;
};

const timestampable = Object.create(entity);
timestampable.getUpdatedAt = function() {
    return this.updatedAt;
};

const userable = Object.create(identifiable);
Object.assign(userable, {
    getEmail() { return this.email; },
    getRole() { return this.role; }
});

const user = Object.create(userable);
Object.assign(user, {
    id: 1,
    name: 'Alice',
    email: 'alice@example.com',
    role: 'admin',
    createdAt: new Date(),
    updatedAt: new Date()
});

console.log(user.getId());         // 1
console.log(user.getName());       // 'Alice'
console.log(user.getEmail());      // 'alice@example.com'
```

---

## ES6 Class Inheritance

ES6 class syntax provides a cleaner approach to inheritance while using the underlying prototype system.

### Basic Class Inheritance

```javascript
// Class inheritance
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
        super(name);  // Must call super before accessing this
        this.breed = breed;
    }
    
    // Override parent method
    speak() {
        return `${this.name} barks`;
    }
    
    // New method
    fetch(item) {
        return `${this.name} fetches ${item}`;
    }
}

const dog = new Dog('Rex', 'German Shepherd');
console.log(dog.name);      // 'Rex'
console.log(dog.breed);    // 'German Shepherd'
console.log(dog.speak());  // 'Rex barks'
console.log(dog.fetch('ball'));  // 'Rex fetches ball'
```

### Using super in Methods

```javascript
// super in various contexts
class Shape {
    constructor(color) {
        this.color = color;
    }
    
    describe() {
        return `A ${this.color} shape`;
    }
    
    get area() {
        return 0;
    }
}

class Circle extends Shape {
    constructor(color, radius) {
        super(color);
        this.radius = radius;
    }
    
    // Call parent method and extend
    describe() {
        return `${super.describe()} with radius ${this.radius}`;
    }
    
    get area() {
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
console.log(circle.area);       // 78.54...
```

### Static Inheritance

```javascript
// Static methods are inherited
class Base {
    static defaultConfig = { timeout: 5000 };
    
    static createDefault() {
        return new this();
    }
    
    static getType() {
        return 'Base';
    }
}

class Derived extends Base {
    static getType() {
        return 'Derived';
    }
}

console.log(Derived.defaultConfig);  // { timeout: 5000 } - inherited
console.log(Derived.createDefault() instanceof Derived);  // true
console.log(Derived.getType());      // 'Derived' - can be overridden
```

---

## Mixin Pattern

Mixins provide a way to compose behavior from multiple sources without traditional inheritance.

### Basic Mixin

```javascript
// Basic mixin function
function mixin(target, source) {
    for (const key of Object.keys(source)) {
        if (typeof source[key] === 'function') {
            target.prototype[key] = source[key];
        }
    }
    return target;
}

// Mixins
const canSwim = {
    swim() {
        return `${this.name} swims`;
    }
};

const canFly = {
    fly() {
        return `${this.name} flies`;
    }
};

const canHunt = {
    hunt(target) {
        return `${this.name} hunts ${target}`;
    }
};

// Apply mixins to class
class Duck {
    constructor(name) {
        this.name = name;
    }
}

mixin(Duck, canSwim);
mixin(Duck, canFly);

const duck = new Duck('Donald');
console.log(duck.swim());  // 'Donald swims'
console.log(duck.fly());   // 'Donald flies'
```

### Class Mixins with Composition

```javascript
// Class-based mixin pattern
const withLogging = (superclass) => class extends superclass {
    log(message) {
        console.log(`[${this.constructor.name}] ${message}`);
    }
};

const withValidation = (superclass) => class extends superclass {
    validate(data) {
        if (!data) throw new Error('Data required');
        return true;
    }
};

const withTimestamp = (superclass) => class extends superclass {
    constructor(...args) {
        super(...args);
        this.createdAt = new Date();
        this.updatedAt = new Date();
    }
    
    touch() {
        this.updatedAt = new Date();
    }
};

class BaseModel {
    constructor(data) {
        this.data = data;
    }
}

class User extends withTimestamp(withLogging(withValidation(BaseModel))) {
    constructor(data) {
        super(data);
        this.validate(data);
    }
    
    save() {
        this.log('Saving user...');
        this.touch();
        this.log('User saved');
    }
}

const user = new User({ name: 'Alice', email: 'alice@example.com' });
console.log(user.createdAt);  // Date
user.save();                  // Logs to console
```

### Multiple Mixin Application

```javascript
// Composing multiple mixins
const composed = (...mixins) => superclass => {
    return mixins.reduce((acc, mixin) => mixin(acc), superclass);
};

const withCache = (superclass) => class extends superclass {
    #cache = new Map();
    
    getCache(key) {
        return this.#cache.get(key);
    }
    
    setCache(key, value) {
        this.#cache.set(key, value);
    }
    
    clearCache() {
        this.#cache.clear();
    }
};

const withPersistence = (superclass) => class extends superclass {
    save() {
        console.log('Saving to storage...');
    }
    
    load() {
        console.log('Loading from storage...');
    }
};

const withEvents = (superclass) => class extends superclass {
    #events = {};
    
    on(event, handler) {
        this.#events[event] = this.#events[event] || [];
        this.#events[event].push(handler);
    }
    
    emit(event, data) {
        if (this.#events[event]) {
            this.#events[event].forEach(h => h(data));
        }
    }
};

class DataService extends composed(
    withCache,
    withPersistence,
    withEvents
)(Object) {
    fetchData(id) {
        const cached = this.getCache(id);
        if (cached) return cached;
        
        const data = { id, value: 'data' };
        this.setCache(id, data);
        return data;
    }
}

const service = new DataService();
console.log(service.fetchData(1));  // { id: 1, value: 'data' }
```

---

## Composition Over Inheritance

Composition creates flexible designs by combining objects rather than establishing inheritance hierarchies.

### Basic Composition

```javascript
// Composition pattern
class Engine {
    constructor(type, horsepower) {
        this.type = type;
        this.horsepower = horsepower;
    }
    
    start() {
        return `Engine ${this.type} starting...`;
    }
    
    getInfo() {
        return `${this.type} with ${this.horsepower} HP`;
    }
}

class Wheels {
    constructor(count, size) {
        this.count = count;
        this.size = size;
    }
    
    getInfo() {
        return `${this.count}x${this.size}" wheels`;
    }
}

// Composition over inheritance
class Car {
    constructor(engine, wheels) {
        this.engine = engine;
        this.wheels = wheels;
        this.isRunning = false;
    }
    
    start() {
        this.isRunning = true;
        return this.engine.start();
    }
    
    getDescription() {
        return `${this.engine.getInfo()}, ${this.wheels.getInfo()}`;
    }
}

const engine = new Engine('V8', 450);
const wheels = new Wheels(4, 18);
const car = new Car(engine, wheels);

console.log(car.start());           // 'Engine V8 starting...'
console.log(car.getDescription());  // 'V8 with 450 HP, 4x18" wheels'
```

### Behavior Composition

```javascript
// Composable behaviors
const createLogger = (prefix) => ({
    log: (...args) => console.log(`[${prefix}]`, ...args),
    error: (...args) => console.error(`[${prefix} ERROR]`, ...args),
    warn: (...args) => console.warn(`[${prefix} WARN]`, ...args)
});

const createCache = () => {
    const cache = new Map();
    return {
        get: (key) => cache.get(key),
        set: (key, value) => cache.set(key, value),
        clear: () => cache.clear()
    };
};

const createValidator = (schema) => ({
    validate: (data) => {
        for (const [key, type] of Object.entries(schema)) {
            if (typeof data[key] !== type) {
                return false;
            }
        }
        return true;
    }
});

function createService(config) {
    return {
        logger: createLogger(config.name),
        cache: createCache(),
        validator: createValidator(config.schema),
        
        process(data) {
            if (!this.validator.validate(data)) {
                this.logger.error('Invalid data', data);
                return null;
            }
            
            const cached = this.cache.get(data.id);
            if (cached) {
                this.logger.log('Cache hit');
                return cached;
            }
            
            const result = { ...data, processed: true };
            this.cache.set(data.id, result);
            this.logger.log('Processed', data.id);
            
            return result;
        }
    };
}

const service = createService({
    name: 'UserService',
    schema: { id: 'string', name: 'string' }
});

service.process({ id: '1', name: 'Alice' });
service.process({ id: '1', name: 'Alice' });  // Cache hit
```

### Strategy Pattern

```javascript
// Strategy pattern with composition
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

class CreditCardStrategy {
    constructor(cardNumber, cvv) {
        this.cardNumber = cardNumber;
        this.cvv = cvv;
    }
    
    pay(amount) {
        return `Paid $${amount} with Credit Card ${this.cardNumber.slice(-4)}`;
    }
}

class PayPalStrategy {
    constructor(email) {
        this.email = email;
    }
    
    pay(amount) {
        return `Paid $${amount} via PayPal account ${this.email}`;
    }
}

class CryptoStrategy {
    constructor(wallet) {
        this.wallet = wallet;
    }
    
    pay(amount) {
        return `Paid $${amount} in cryptocurrency to ${this.wallet}`;
    }
}

const processor = new PaymentProcessor(
    new CreditCardStrategy('1234567890123456', '123')
);

console.log(processor.process(100));  // Paid $100 with Credit Card 3456

processor.setStrategy(new PayPalStrategy('user@example.com'));
console.log(processor.process(50));     // Paid $50 via PayPal account user@example.com
```

### Delegation Pattern

```javascript
// Delegation composition
class Printer {
    print(document) {
        console.log(`Printing: ${document}`);
    }
}

class Scanner {
    scan() {
        return 'Scanned document';
    }
}

class Fax {
    send(number, message) {
        console.log(`Sending fax to ${number}: ${message}`);
    }
}

// All-in-one device using composition
class MultiFunctionDevice {
    constructor() {
        this.printer = new Printer();
        this.scanner = new Scanner();
        this.fax = new Fax();
    }
    
    // Delegation to components
    print = (...args) => this.printer.print(...args);
    scan = (...args) => this.scanner.scan(...args);
    fax = (...args) => this.fax.send(...args);
    
    // Combined functionality
    copy() {
        const document = this.scan();
        this.print(document);
        return 'Copy complete';
    }
}

const device = new MultiFunctionDevice();
device.print('Document');
console.log(device.scan());  // 'Scanned document'
console.log(device.copy());  // 'Copy complete'
```

---

## Hybrid Patterns

Combining inheritance and composition often provides the most flexible designs.

### Inheritance with Composition

```javascript
// Mixing inheritance and composition
class BaseRepository {
    constructor(storage) {
        this.storage = storage;
        this.cache = new Map();
    }
    
    async findById(id) {
        const cached = this.cache.get(id);
        if (cached) return cached;
        
        const data = await this.storage.get(id);
        this.cache.set(id, data);
        return data;
    }
    
    async save(entity) {
        await this.storage.set(entity.id, entity);
        this.cache.set(entity.id, entity);
    }
}

class UserRepository extends BaseRepository {
    constructor(storage) {
        super(storage);
    }
    
    async findByEmail(email) {
        const users = await this.storage.query({ email });
        return users[0];
    }
}

class AuditLogger {
    log(action, data) {
        console.log(`[AUDIT] ${action}:`, data);
    }
}

// Extended with composition
class AuditedUserRepository extends UserRepository {
    constructor(storage, logger) {
        super(storage);
        this.logger = logger;
    }
    
    async save(entity) {
        this.logger.log('save', entity);
        return super.save(entity);
    }
    
    async findById(id) {
        this.logger.log('findById', id);
        return super.findById(id);
    }
}
```

---

## Professional Use Cases

### Use Case 1: Plugin System

```javascript
// Plugin architecture with composition
class PluginHost {
    constructor() {
        this.plugins = new Map();
    }
    
    register(name, plugin) {
        if (this.plugins.has(name)) {
            throw new Error(`Plugin ${name} already registered`);
        }
        
        if (plugin.install) {
            plugin.install(this);
        }
        
        this.plugins.set(name, plugin);
    }
    
    unregister(name) {
        const plugin = this.plugins.get(name);
        if (plugin && plugin.uninstall) {
            plugin.uninstall(this);
        }
        this.plugins.delete(name);
    }
    
    getPlugin(name) {
        return this.plugins.get(name);
    }
}

// Base plugin interface
const createPlugin = (name, methods) => ({
    name,
    ...methods
});

// Example plugins
const loggingPlugin = createPlugin('logging', {
    install(host) {
        host.logger = console;
    },
    uninstall(host) {
        delete host.logger;
    }
});

const cachePlugin = createPlugin('cache', {
    install(host) {
        host.cache = new Map();
    },
    get(key) {
        return this.host?.cache?.get(key);
    },
    set(key, value) {
        this.host?.cache?.set(key, value);
    }
});

const host = new PluginHost();
host.register('logging', loggingPlugin);
host.register('cache', cachePlugin);
```

### Use Case 2: Entity Component System

```javascript
// Entity-Component-System pattern
class Entity {
    #components = new Map();
    
    addComponent(name, component) {
        this.#components.set(name, component);
        return this;
    }
    
    removeComponent(name) {
        this.#components.delete(name);
        return this;
    }
    
    getComponent(name) {
        return this.#components.get(name);
    }
    
    hasComponent(name) {
        return this.#components.has(name);
    }
}

// Components
const Position = (x, y) => ({ x, y, type: 'position' });
const Velocity = (vx, vy) => ({ vx, vy, type: 'velocity' });
const Renderable = (color) => ({ color, type: 'renderable' });
const Health = (hp) => ({ hp, type: 'health' });

// Systems
const MovementSystem = (entities) => {
    entities.forEach(entity => {
        const pos = entity.getComponent('position');
        const vel = entity.getComponent('velocity');
        
        if (pos && vel) {
            pos.x += vel.vx;
            pos.y += vel.vy;
        }
    });
};

const RenderSystem = (entities) => {
    entities.forEach(entity => {
        const pos = entity.getComponent('position');
        const render = entity.getComponent('renderable');
        
        if (pos && render) {
            console.log(`Rendering at (${pos.x}, ${pos.y}) with color ${render.color}`);
        }
    });
};

// Usage
const player = new Entity()
    .addComponent('position', Position(0, 0))
    .addComponent('velocity', Velocity(1, 1))
    .addComponent('renderable', Renderable('blue'))
    .addComponent('health', Health(100));

const enemies = [player];
MovementSystem(enemies);
RenderSystem(enemies);
```

### Use Case 3: State Machine

```javascript
// Composition-based state machine
class StateMachine {
    #currentState;
    #transitions = new Map();
    #handlers = {};
    
    constructor(initialState) {
        this.#currentState = initialState;
    }
    
    addTransition(from, event, to, handler) {
        const key = `${from}:${event}`;
        this.#transitions.set(key, { to, handler });
    }
    
    addHandler(state, enter, exit) {
        this.#handlers[state] = { enter, exit };
    }
    
    transition(event) {
        const key = `${this.#currentState}:${event}`;
        const transition = this.#transitions.get(key);
        
        if (!transition) {
            throw new Error(`No transition from ${this.#currentState} on ${event}`);
        }
        
        const handler = this.#handlers[this.#currentState];
        if (handler?.exit) {
            handler.exit();
        }
        
        this.#currentState = transition.to;
        
        const newHandler = this.#handlers[this.#currentState];
        if (newHandler?.enter) {
            newHandler.enter();
        }
        
        if (transition.handler) {
            transition.handler();
        }
        
        return this.#currentState;
    }
    
    getState() {
        return this.#currentState;
    }
}

// Usage
const orderMachine = new StateMachine('draft');

orderMachine.addHandler('draft', 
    () => console.log('Order created'),
    () => console.log('Order cancelled')
);

orderMachine.addHandler('confirmed',
    () => console.log('Payment confirmed'),
    () => console.log('Unconfirming payment')
);

orderMachine.addHandler('shipped',
    () => console.log('Package shipped'),
    () => console.log('Un-shipping')
);

orderMachine.addTransition('draft', 'confirm', 'confirmed');
orderMachine.addTransition('confirmed', 'ship', 'shipped');
orderMachine.addTransition('shipped', 'deliver', 'delivered');

orderMachine.transition('confirm');  // 'Payment confirmed'
orderMachine.transition('ship');     // 'Package shipped'
console.log(orderMachine.getState());  // 'shipped'
```

---

## Key Takeaways

1. **Classical inheritance** uses constructor functions and prototype chains
2. **Prototypal inheritance** uses Object.create() directly
3. **Class inheritance** provides cleaner syntax with extends
4. **Mixins** compose behavior without single inheritance
5. **Composition** creates flexible, loosely-coupled designs
6. **Favor composition** over deep inheritance hierarchies
7. **Hybrid approaches** combine best of both patterns
8. **Strategy pattern** enables runtime behavior switching

---

## Common Pitfalls

1. **Deep inheritance chains**: Creates fragile, hard-to-maintain code
2. **Diamond problem**: Multiple paths to same parent cause ambiguity
3. **Forgetting super()**: Classes require calling parent constructor
4. **Tight coupling**: Inheritance creates strong coupling between classes
5. **Rigid hierarchies**: Hard to modify behavior at runtime
6. **Overusing mixins**: Can cause name conflicts and complexity
7. **Not using composition**: Miss flexibility benefits
8. **Ignoring composition for inheritance**: When behavior varies

---

## Related Files

- **01_OBJECT_LITERALS_AND_CREATION.md**: Object creation patterns
- **02_PROTOTYPES_DEEP_DIVE.md**: Prototype chain internals
- **03_CONSTRUCTOR_FUNCTIONS.md**: Constructor patterns
- **04_ECMASCRIPT_CLASSES_SYNTAX.md**: Class syntax
- **05_GETTERS_SETTERS_AND_PROPERTIES.md**: Property descriptors
- **08_OBJECT_SECURITY_PATTERNS.md**: Object protection