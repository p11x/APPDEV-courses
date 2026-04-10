# JavaScript Data Structures Patterns: Complete Mastery Guide

Professional JavaScript development requires understanding common patterns for creating robust, maintainable data structures. This guide covers factory patterns, collection patterns, data structure selection, and implementation patterns used in production applications.

---

## Table of Contents

1. [Factory Patterns](#factory-patterns)
2. [Collection Patterns](#collection-patterns)
3. [Data Structure Selection](#data-structure-selection)
4. [Advanced Patterns](#advanced-patterns)
5. [Professional Implementations](#professional-implementations)
6. [Key Takeaways](#key-takeaways)
7. [Common Pitfalls](#common-pitfalls)
8. [Related Files](#related-files)

---

## Factory Patterns

Factory patterns provide flexible object creation with encapsulation and privacy.

### Simple Factory

Basic factory pattern for creating similar objects.

```javascript
// Simple factory for creating users
function createUser(name, email, role = 'user') {
    return {
        name,
        email,
        role,
        createdAt: new Date(),
        isActive: true,
        
        validate() {
            return this.name && this.email.includes('@');
        },
        
        serialize() {
            return JSON.stringify({
                name: this.name,
                email: this.email,
                role: this.role
            });
        }
    };
}

// Factory with validation
function createValidatedUser(data) {
    const errors = [];
    
    if (!data.name || data.name.length < 2) {
        errors.push('Name must be at least 2 characters');
    }
    
    if (!data.email || !data.email.includes('@')) {
        errors.push('Valid email is required');
    }
    
    if (errors.length > 0) {
        throw new Error(errors.join(', '));
    }
    
    return createUser(data.name, data.email, data.role);
}

// Usage
const user = createUser('Alice', 'alice@example.com', 'admin');
const json = user.serialize();
console.log(json);  // {"name":"Alice","email":"alice@example.com","role":"admin"}
```

### Factory with Private State

Creating objects with truly private variables.

```javascript
// Factory with closure-based privacy
function createSecureWallet(initialBalance = 0) {
    let _balance = initialBalance;
    let _transactions = [];
    
    const wallet = {
        get balance() {
            return _balance;
        },
        
        deposit(amount) {
            if (amount <= 0) {
                throw new Error('Deposit amount must be positive');
            }
            _balance += amount;
            _transactions.push({ type: 'deposit', amount, date: new Date() });
            return _balance;
        },
        
        withdraw(amount) {
            if (amount <= 0) {
                throw new Error('Withdrawal amount must be positive');
            }
            if (amount > _balance) {
                throw new Error('Insufficient funds');
            }
            _balance -= amount;
            _transactions.push({ type: 'withdraw', amount, date: new Date() });
            return _balance;
        },
        
        getTransactions() {
            return _transactions.map(t => ({ ...t }));
        },
        
        getTransactionCount() {
            return _transactions.length;
        }
    };
    
    return Object.freeze(wallet);
}

// Usage
const wallet = createSecureWallet(1000);
console.log(wallet.balance);  // 1000
wallet.deposit(500);
wallet.withdraw(200);
console.log(wallet.balance);  // 1300
console.log(wallet.getTransactions().length);  // 2

// _balance is not accessible
console.log(wallet._balance);  // undefined
```

### Factory Composition

Composing factories for complex object creation.

```javascript
// Base factory for common properties
function createBaseEntity(id) {
    return {
        id,
        createdAt: new Date(),
        updatedAt: new Date()
    };
}

// Mixin factory
function withTimestamps(entity) {
    return {
        ...entity,
        getAge() {
            return Date.now() - entity.createdAt.getTime();
        },
        touch() {
            entity.updatedAt = new Date();
            return entity;
        }
    };
}

// Mixin for event support
function withEvents(entity) {
    const events = {};
    
    return {
        ...entity,
        on(event, handler) {
            if (!events[event]) events[event] = [];
            events[event].push(handler);
        },
        emit(event, ...args) {
            if (events[event]) {
                events[event].forEach(h => h(...args));
            }
        },
        off(event, handler) {
            if (events[event]) {
                events[event] = events[event].filter(h => h !== handler);
            }
        }
    };
}

// Composed factory
function createTodoItem(title) {
    let _completed = false;
    let _priority = 'normal';
    
    const item = createBaseEntity(Date.now().toString());
    
    const entity = {
        ...item,
        
        get title() {
            return title;
        },
        
        get completed() {
            return _completed;
        },
        
        set priority(value) {
            if (['low', 'normal', 'high'].includes(value)) {
                _priority = value;
            }
        },
        
        get priority() {
            return _priority;
        },
        
        toggle() {
            _completed = !_completed;
            return this;
        }
    };
    
    return withTimestamps(withEvents(entity));
}

// Usage
const todo = createTodoItem('Learn JavaScript');
todo.priority = 'high';
todo.on('change', () => console.log('Changed!'));
todo.toggle();
console.log(todo.completed);  // true
```

### Builder Pattern

Step-by-step object construction with fluent API.

```javascript
// Builder for complex objects
class QueryBuilder {
    constructor() {
        this.query = {
            select: [],
            from: null,
            where: [],
            orderBy: [],
            limit: null,
            offset: null
        };
    }
    
    select(...columns) {
        this.query.select = columns;
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
    
    whereIn(column, values) {
        this.query.where.push(`${column} IN (${values.map(v => `'${v}'`).join(', ')})`);
        return this;
    }
    
    orderBy(column, direction = 'ASC') {
        this.query.orderBy.push(`${column} ${direction}`);
        return this;
    }
    
    limit(count) {
        this.query.limit = count;
        return this;
    }
    
    offset(count) {
        this.query.offset = count;
        return this;
    }
    
    build() {
        const parts = [];
        
        if (this.query.select.length > 0) {
            parts.push(`SELECT ${this.query.select.join(', ')}`);
        } else {
            parts.push('SELECT *');
        }
        
        if (this.query.from) {
            parts.push(`FROM ${this.query.from}`);
        }
        
        if (this.query.where.length > 0) {
            parts.push(`WHERE ${this.query.where.join(' AND ')}`);
        }
        
        if (this.query.orderBy.length > 0) {
            parts.push(`ORDER BY ${this.query.orderBy.join(', ')}`);
        }
        
        if (this.query.limit) {
            parts.push(`LIMIT ${this.query.limit}`);
        }
        
        if (this.query.offset) {
            parts.push(`OFFSET ${this.query.offset}`);
        }
        
        return parts.join(' ');
    }
}

// Usage
const query = new QueryBuilder()
    .select('id', 'name', 'email')
    .from('users')
    .where('active = true')
    .whereIn('role', ['admin', 'moderator'])
    .orderBy('created_at', 'DESC')
    .limit(10)
    .build();

console.log(query);
// SELECT id, name, email FROM users WHERE active = true AND role IN ('admin', 'moderator') ORDER BY created_at DESC LIMIT 10
```

---

## Collection Patterns

Managing collections of related data using JavaScript patterns.

### Collection Manager

Centralized collection with CRUD operations.

```javascript
class Collection {
    constructor(name) {
        this.name = name;
        this.items = new Map();
        this.observers = [];
    }
    
    add(item) {
        if (!item.id) {
            item.id = this.generateId();
        }
        
        this.items.set(item.id, item);
        this.notify('add', item);
        
        return item;
    }
    
    get(id) {
        return this.items.get(id);
    }
    
    update(id, updates) {
        const item = this.items.get(id);
        
        if (!item) {
            throw new Error(`Item ${id} not found`);
        }
        
        const updated = { ...item, ...updates };
        this.items.set(id, updated);
        this.notify('update', updated);
        
        return updated;
    }
    
    delete(id) {
        const item = this.items.get(id);
        
        if (!item) {
            return false;
        }
        
        this.items.delete(id);
        this.notify('delete', item);
        
        return true;
    }
    
    find(predicate) {
        return [...this.items.values()].filter(predicate);
    }
    
    findOne(predicate) {
        return [...this.items.values()].find(predicate) || null;
    }
    
    findById(id) {
        return this.items.get(id);
    }
    
    all() {
        return [...this.items.values()];
    }
    
    clear() {
        this.items.clear();
        this.notify('clear');
    }
    
    get size() {
        return this.items.size;
    }
    
    on(event, handler) {
        if (!this.observers[event]) {
            this.observers[event] = [];
        }
        this.observers[event].push(handler);
    }
    
    notify(event, data) {
        if (this.observers[event]) {
            this.observers[event].forEach(handler => handler(data));
        }
    }
    
    generateId() {
        return `${this.name}_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
    }
}

// Usage
const users = new Collection('users');
const admins = users.all().filter(u => u.role === 'admin');

users.add({ name: 'Alice', role: 'admin' });
users.add({ name: 'Bob', role: 'user' });

users.on('add', user => console.log(`Added: ${user.name}`));
users.add({ name: 'Charlie', role: 'moderator' });

console.log(users.findById(users.all()[0].id));
```

### Lazy Collection

Deferring computation until needed.

```javascript
class LazyCollection {
    constructor(source) {
        this.source = source;
        this.cache = null;
        this.computed = false;
    }
    
    _ensureComputed() {
        if (!this.computed) {
            this.cache = [...this.source];
            this.computed = true;
        }
    }
    
    map(fn) {
        this._ensureComputed();
        return new LazyCollection(this.cache.map(fn));
    }
    
    filter(fn) {
        this._ensureComputed();
        return new LazyCollection(this.cache.filter(fn));
    }
    
    take(n) {
        this._ensureComputed();
        return new LazyCollection(this.cache.slice(0, n));
    }
    
    skip(n) {
        this._ensureComputed();
        return new LazyCollection(this.cache.slice(n));
    }
    
    toArray() {
        this._ensureComputed();
        return [...this.cache];
    }
    
    [Symbol.iterator]() {
        this._ensureComputed();
        return this.cache[Symbol.iterator]();
    }
}

// Usage with lazy evaluation
function* hugeGenerator() {
    for (let i = 0; i < 1000000; i++) {
        yield i;
    }
}

const lazy = new LazyCollection(hugeGenerator());
// No computation yet!

// Computation happens only when needed
const result = lazy
    .filter(x => x % 2 === 0)
    .map(x => x * 2)
    .take(5)
    .toArray();

console.log(result);  // [0, 4, 8, 12, 16]
```

### Observable Collection

Collections with change notifications.

```javascript
class ObservableMap extends Map {
    constructor() {
        super();
        this.listeners = new Map();
    }
    
    set(key, value) {
        const oldValue = super.get(key);
        super.set(key, value);
        this._emit('set', { key, value, oldValue });
        return this;
    }
    
    delete(key) {
        const oldValue = super.get(key);
        const result = super.delete(key);
        if (result) {
            this._emit('delete', { key, oldValue });
        }
        return result;
    }
    
    clear() {
        const oldEntries = [...super.entries()];
        super.clear();
        this._emit('clear', { oldEntries });
    }
    
    on(event, handler) {
        if (!this.listeners.has(event)) {
            this.listeners.set(event, []);
        }
        this.listeners.get(event).push(handler);
    }
    
    off(event, handler) {
        if (this.listeners.has(event)) {
            const handlers = this.listeners.get(event);
            const index = handlers.indexOf(handler);
            if (index !== -1) {
                handlers.splice(index, 1);
            }
        }
    }
    
    _emit(event, data) {
        if (this.listeners.has(event)) {
            this.listeners.get(event).forEach(handler => handler(data));
        }
    }
}

// Usage
const store = new ObservableMap();

store.on('set', ({ key, value }) => {
    console.log(`Set ${key} = ${value}`);
});

store.set('theme', 'dark');
store.set('fontSize', 16);
// Set theme = dark
// Set fontSize = 16
```

---

## Data Structure Selection

Choosing the right data structure for the problem.

### Decision Matrix

| Use Case | Recommended | Avoid |
|----------|-----------|-------|
| Unique values, membership test | Set | Array |
| Key-value, any key type | Map | Object |
| Key-value, string keys only | Object | Map |
| Sorted iteration | Map | Object |
| Sequential access Array | Array | - |
| Stack (LIFO) | Array push/pop | Shift operations |
| Queue (FIFO) | Array with pointer | shift() |
| Caching | Map (LRU) | Object |

### Selection Examples

```javascript
// Case: User roles lookup - use Map
const userRoles = new Map();
userRoles.set('alice', 'admin');
userRoles.set('bob', 'user');
userRoles.has('alice');  // true - O(1)

// Case: Unique tags - use Set
const tags = new Set(['javascript', 'es6', 'javascript', 'web']);
[...tags];  // ['javascript', 'es6', 'web']

// Case: Configuration - use Object
const config = {
    theme: 'dark',
    fontSize: 16,
    language: 'en'
};

// Case: Sorted cache - use Map
const cache = new Map();
cache.set('oldest', 'value1');
cache.set('new', 'value2');
// Maintains insertion order

// Case: Document attributes - use Object
const document = {
    title: 'My Document',
    author: 'Alice',
    metadata: {
        created: new Date(),
        modified: new Date()
    }
};

// Case: Multiple values per key - use Map of Sets
const userGroups = new Map();
userGroups.set('alice', new Set(['admin', 'users']));
userGroups.set('bob', new Set(['users']));
```

### Migration Patterns

Converting between structures when needs change.

```javascript
// Object to Map - when you need non-string keys
function objectToMap(obj) {
    return new Map(Object.entries(obj));
}

// Map to Object - when you need simple serialization
function mapToObject(map) {
    return Object.fromEntries(map);
}

// Array to Set - for unique values
function arrayToSet(array) {
    return new Set(array);
}

// Set to Array - for ordered operations
function setToArray(set) {
    return [...set];
}

// Array to Map - for indexed access
function arrayToMap(array) {
    return new Map(array.map((item, index) => [index, item]));
}

// Object to Record - type-safe records
function objectToRecord(obj) {
    return Object.fromEntries(
        Object.entries(obj).map(([key, value]) => [key, value])
    );
}
```

---

## Advanced Patterns

Complex patterns for professional applications.

### Composite Pattern

Treating individual and composite objects uniformly.

```javascript
// Component interface
class Component {
    render() {
        throw new Error('Must implement render');
    }
}

// Leaf component
class TextNode extends Component {
    constructor(text) {
        super();
        this.text = text;
    }
    
    render() {
        return document.createTextNode(this.text);
    }
}

// Composite component
class Element extends Component {
    constructor(tag) {
        super();
        this.tag = tag;
        this.children = [];
        this.attributes = new Map();
    }
    
    add(child) {
        this.children.push(child);
        return this;
    }
    
    remove(child) {
        const index = this.children.indexOf(child);
        if (index !== -1) {
            this.children.splice(index, 1);
        }
        return this;
    }
    
    setAttribute(name, value) {
        this.attributes.set(name, value);
        return this;
    }
    
    render() {
        const element = document.createElement(this.tag);
        
        for (const [name, value] of this.attributes) {
            element.setAttribute(name, value);
        }
        
        for (const child of this.children) {
            element.appendChild(child.render());
        }
        
        return element;
    }
}

// Usage
const card = new Element('div')
    .setAttribute('class', 'card')
    .add(
        new Element('h1')
            .setAttribute('class', 'title')
            .add(new TextNode('Welcome'))
    )
    .add(
        new Element('p')
            .add(new TextNode('This is a card component'))
    );
```

### Flyweight Pattern

Sharing common data efficiently.

```javascript
// Flyweight factory
class FlyweightFactory {
    constructor() {
        this.flyweights = new Map();
    }
    
    getFlyweight(key, createFn) {
        if (!this.flyweights.has(key)) {
            this.flyweights.set(key, createFn());
        }
        return this.flyweights.get(key);
    }
    
    get count() {
        return this.flyweights.size;
    }
}

// Example: Shared styles
const styleFactory = new FlyweightFactory();

function getTextStyle(bold) {
    return styleFactory.getFlyweight(`bold_${bold}`, () => ({
        fontWeight: bold ? 'bold' : 'normal',
        fontSize: '16px',
        fontFamily: 'Arial, sans-serif'
    }));
}

// Example: Shared formatters
const formatterFactory = new FlyweightFactory();

function getDateFormatter(locale) {
    return formatterFactory.getFlyweight(`date_${locale}`, () => {
        return new Intl.DateTimeFormat(locale, {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
    });
}

// Usage
const formatUS = getDateFormatter('en-US');
const formatUK = getDateFormatter('en-GB');
console.log(formatUS.format(new Date()));  // "April 3, 2026"
console.log(formatUK.format(new Date()));  // "3 April 2026"
console.log(formatterFactory.count); // 2
```

### Command Pattern

Encapsulating operations as objects.

```javascript
// Command interface
class Command {
    execute() {
        throw new Error('Must implement execute');
    }
    
    undo() {
        throw new Error('Must implement undo');
    }
}

// Concrete commands
class SetPropertyCommand extends Command {
    constructor(target, property, newValue) {
        super();
        this.target = target;
        this.property = property;
        this.newValue = newValue;
        this.oldValue = target[property];
    }
    
    execute() {
        this.target[this.property] = this.newValue;
    }
    
    undo() {
        this.target[this.property] = this.oldValue;
    }
}

class CompositeCommand extends Command {
    constructor(commands) {
        super();
        this.commands = commands;
    }
    
    execute() {
        for (const command of this.commands) {
            command.execute();
        }
    }
    
    undo() {
        for (const command of [...this.commands].reverse()) {
            command.undo();
        }
    }
}

// Command manager
class CommandManager {
    constructor() {
        this.history = [];
        this.redoStack = [];
    }
    
    execute(command) {
        command.execute();
        this.history.push(command);
        this.redoStack = [];
    }
    
    undo() {
        const command = this.history.pop();
        if (command) {
            command.undo();
            this.redoStack.push(command);
        }
    }
    
    redo() {
        const command = this.redoStack.pop();
        if (command) {
            command.execute();
            this.history.push(command);
        }
    }
    
    canUndo() {
        return this.history.length > 0;
    }
    
    canRedo() {
        return this.redoStack.length > 0;
    }
}

// Usage
const settings = { theme: 'light', fontSize: 16 };
const manager = new CommandManager();

manager.execute(new SetPropertyCommand(settings, 'theme', 'dark'));
console.log(settings.theme);  // 'dark'

manager.undo();
console.log(settings.theme);  // 'light'

manager.redo();
console.log(settings.theme);  // 'dark'
```

### Iterator Pattern

Uniform traversal of different structures.

```javascript
class DirectoryIterator {
    constructor(structure) {
        this.stack = [structure];
        this.path = [structure.name];
    }
    
    [Symbol.iterator]() {
        return this;
    }
    
    next() {
        while (this.stack.length > 0) {
            const current = this.stack[this.stack.length - 1];
            
            if (typeof current === 'object' && !Array.isArray(current)) {
                const entries = Object.entries(current);
                const [key, value] = entries.pop();
                this.stack.push(value);
                this.path.push(key);
                
                if (typeof value === 'object') {
                    return {
                        done: false,
                        value: { key, value, path: [...this.path] }
                    };
                }
            } else {
                this.stack.pop();
                this.path.pop();
            }
        }
        
        return { done: true };
    }
}

// Tree iterator
class TreeNode {
    constructor(value, children = []) {
        this.value = value;
        this.children = children;
    }
    
    *[Symbol.iterator]() {
        yield this.value;
        
        for (const child of this.children) {
            yield* child[Symbol.iterator]();
        }
    }
    
    // Depth-first
    *depthFirst() {
        const stack = [this];
        
        while (stack.length > 0) {
            const node = stack.pop();
            yield node.value;
            
            for (let i = node.children.length - 1; i >= 0; i--) {
                stack.push(node.children[i]);
            }
        }
    }
    
    // Breadth-first
    *breadthFirst() {
        const queue = [this];
        
        while (queue.length > 0) {
            const node = queue.shift();
            yield node.value;
            
            queue.push(...node.children);
        }
    }
}

// Usage
const tree = new TreeNode('root', [
    new TreeNode('a', [
        new TreeNode('a1'),
        new TreeNode('a2')
    ]),
    new TreeNode('b')
]);

for (const node of tree.depthFirst()) {
    console.log(node);  // root, a, a1, a2, b
}
```

---

## Professional Implementations

### State Management

```javascript
class Store {
    constructor(initialState = {}) {
        this.state = { ...initialState };
        this.reducers = [];
        this.listeners = [];
        
        this.dispatch = this.dispatch.bind(this);
    }
    
    subscribe(listener) {
        this.listeners.push(listener);
        
        return () => {
            const index = this.listeners.indexOf(listener);
            if (index !== -1) {
                this.listeners.splice(index, 1);
            }
        };
    }
    
    getState() {
        return { ...this.state };
    }
    
    addReducer(reducer) {
        this.reducers.push(reducer);
    }
    
    dispatch(action) {
        console.log('Dispatching:', action);
        
        let hasChanged = false;
        
        for (const reducer of this.reducers) {
            const newState = reducer(this.state, action);
            
            if (newState !== this.state) {
                this.state = newState;
                hasChanged = true;
            }
        }
        
        if (hasChanged) {
            for (const listener of this.listeners) {
                listener(this.state);
            }
        }
        
        return action;
    }
}

// Reducer
function appReducer(state, action) {
    switch (action.type) {
        case 'SET_USER':
            return { ...state, user: action.payload };
        case 'SET_THEME':
            return { ...state, theme: action.payload };
        case 'UPDATE_SETTINGS':
            return { ...state, settings: { ...state.settings, ...action.payload } };
        default:
            return state;
    }
}

// Usage
const store = new Store({
    user: null,
    theme: 'light',
    settings: { fontSize: 16 }
});

store.addReducer(appReducer);

store.subscribe(state => console.log('State changed:', state));
store.dispatch({ type: 'SET_USER', payload: { name: 'Alice' } });
store.dispatch({ type: 'SET_THEME', payload: 'dark' });
```

### Cache Manager

```javascript
class CacheManager {
    constructor(options = {}) {
        this.maxAge = options.maxAge || 3600000; // 1 hour
        this.maxSize = options.maxSize || 100;
        this.cache = new Map();
    }
    
    set(key, value, maxAge = this.maxAge) {
        const entry = {
            value,
            expires: Date.now() + maxAge
        };
        
        // Evict if at capacity
        if (this.cache.size >= this.maxSize && !this.cache.has(key)) {
            const firstKey = this.cache.keys().next().value;
            this.cache.delete(firstKey);
        }
        
        this.cache.set(key, entry);
    }
    
    get(key) {
        const entry = this.cache.get(key);
        
        if (!entry) return null;
        
        if (Date.now() > entry.expires) {
            this.cache.delete(key);
            return null;
        }
        
        return entry.value;
    }
    
    has(key) {
        return this.get(key) !== null;
    }
    
    delete(key) {
        return this.cache.delete(key);
    }
    
    clear() {
        this.cache.clear();
    }
    
    prune() {
        const now = Date.now();
        
        for (const [key, entry] of this.cache) {
            if (now > entry.expires) {
                this.cache.delete(key);
            }
        }
    }
    
    get size() {
        return this.cache.size;
    }
}

// Usage
const cache = new CacheManager({ maxAge: 5000, maxSize: 3 });
cache.set('user', { name: 'Alice' });
console.log(cache.get('user'));  // { name: 'Alice' }
setTimeout(() => {
    console.log(cache.get('user'));  // null - expired
}, 6000);
```

---

## Key Takeaways

1. **Factory patterns encapsulate creation**: Control instantiation, enable testing
2. **Private state via closures**: True privacy, not just convention
3. **Builder for complex construction**: Fluent APIs, step-by-step
4. **Collection patterns unify CRUD**: Consistent interfaces
5. **Choose data structure wisely**: Map vs Set, Object vs Map
6. **Observable patterns**: Reactive data flow
7. **Command pattern enables undo**: Operations as objects
8. **Flyweight conserves memory**: Shared common state

---

## Common Pitfalls

1. **Mutating shared state in factories**: Always return new objects
2. **Memory leaks in collections**: Clean up observers
3. **Choosing wrong structure**: Consider operations needed
4. **Circular references**: Break cycles for JSON serialization
5. **Performance at scale**: Profile before optimizing
6. **Over-engineering**: Use appropriate complexity
7. **Not handling errors**: Validate inputs
8. **Ignoring edge cases**: Empty collections, single items

---

## Related Files

- **01_ARRAYS_MASTER.md**: Array construction and methods
- **02_OBJECTS_AND_PROPERTIES.md**: Object creation patterns
- **03_MAPS_AND_SETS.md**: Map/Set factory patterns
- **04_DATA_STRUCTURES_ALGORITHMS.md**: Algorithm patterns
- **06_MEMORY_MANAGEMENT_DATA_STRUCTURES.md**: Memory patterns