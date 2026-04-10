# 📝 Function Parameters and Arguments

## 📋 Table of Contents

1. [Overview](#overview)
2. [Rest Parameters](#rest-parameters)
3. [Spread Operator](#spread-operator)
4. [Default Parameters](#default-parameters)
5. [Destructuring in Parameters](#destructuring-in-parameters)
6. [Arguments Object](#arguments-object)
7. [Parameter Validation](#parameter-validation)
8. [Professional Use Cases](#professional-use-cases)
9. [Common Pitfalls](#common-pitfalls)
10. [Key Takeaways](#key-takeaways)

---

## Overview

JavaScript provides powerful mechanisms for handling function parameters that go beyond simple positional arguments. Modern JavaScript (ES6+) offers rest parameters, spread operators, default parameters, and destructuringassignment in parameters. These features enable flexible function signatures, variadic functions, and elegant handling of optional parameters. This comprehensive guide explores each technique with production-ready examples and professional use cases.

Understanding these parameter patterns is essential for writing clean, maintainable JavaScript code. They form the foundation for many functional programming patterns and are heavily used in modern frameworks and libraries. By mastering these techniques, you'll be able to create more flexible and expressive APIs.

---

## Rest Parameters

### Basic Rest Parameter Syntax

Rest parameters allow a function to accept an indefinite number of arguments as an array:

```javascript
// students/01_restParameters.js

// Basic rest parameter
function sum(...numbers) {
    return numbers.reduce((total, n) => total + n, 0);
}

console.log(sum(1, 2));           // 3
console.log(sum(1, 2, 3, 4, 5));   // 15

// Finding maximum using rest parameters
function findMax(...numbers) {
    if (numbers.length === 0) return undefined;
    return Math.max(...numbers);
}

console.log(findMax(3, 1, 4, 1, 5, 9, 2, 6));  // 9

// Collecting all arguments after required ones
function greet(name, ...greetings) {
    return greetings.map(greeting => `${greeting}, ${name}!`);
}

console.log(greet('Alice', 'Hello', 'Hi', 'Hey'));
// ['Hello, Alice!', 'Hi, Alice!', 'Hey, Alice!']
```

### Rest Parameters with Other Parameters

```javascript
// students/02_restWithParams.js

// Rest must be the last parameter
function createUser(name, age, ...roles) {
    return {
        name,
        age,
        roles: roles.length > 0 ? roles : ['user']
    };
}

console.log(createUser('Alice', 30, 'admin', 'editor'));
// { name: 'Alice', age: 30, roles: ['admin', 'editor'] }

console.log(createUser('Bob', 25));
// { name: 'Bob', age: 25, roles: ['user'] }

// Error: Rest parameter must be last
// function bad(a, ...rest, b) {}  // SyntaxError

// Combining required and rest parameters
function processTransaction(id, amount, ...tags) {
    return {
        id,
        amount,
        tags,
        timestamp: new Date(),
        metadata: {
            tagsCount: tags.length
        }
    };
}

console.log(processTransaction('TXN-001', 100, 'sale', 'online', 'usd'));
// { id: 'TXN-001', amount: 100, tags: ['sale', 'online', 'usd'], ... }
```

### Practical Rest Parameter Patterns

```javascript
// students/03_restPatterns.js

// Variadic Logger
const Logger = {
    log: function(level, ...messages) {
        const timestamp = new Date().toISOString();
        messages.forEach(msg => {
            console.log(`[${timestamp}] [${level.toUpperCase()}]:`, msg);
        });
    },
    
    debug: function(...messages) {
        this.log('debug', ...messages);
    },
    
    info: function(...messages) {
        this.log('info', ...messages);
    },
    
    error: function(...messages) {
        this.log('error', ...messages);
    }
};

Logger.info('User logged in', 'Session started', 'IP: 192.168.1.1');

// Merge multiple objects
function mergeObjects(...objects) {
    return Object.assign({}, ...objects);
}

const base = { a: 1 };
const config = { b: 2 };
const overrides = { c: 3 };

console.log(mergeObjects(base, config, overrides));
// { a: 1, b: 2, c: 3 }

// Union of arrays
function unionArrays(...arrays) {
    return [...new Set(arrays.flat())];
}

console.log(unionArrays([1, 2, 3], [2, 3, 4], [3, 4, 5]));
// [1, 2, 3, 4, 5]
```

---

## Spread Operator

### Spread in Function Calls

The spread operator (`...`) expands an array or iterable into individual arguments:

```javascript
// students/04_spreadInCalls.js

// Apply spread to function arguments
const numbers = [1, 2, 3, 4, 5];
console.log(Math.max(...numbers));  // 5
console.log(Math.min(...numbers));  // 1

// Push multiple elements from array
const arr = [1, 2, 3];
const toAdd = [4, 5];
arr.push(...toAdd);
console.log(arr);  // [1, 2, 3, 4, 5]

// Clone and extend array
const original = [1, 2, 3];
const extended = [...original, 4, 5];
console.log(extended);  // [1, 2, 3, 4, 5]
console.log(original); // [1, 2, 3] - unchanged

// String to array of characters
const letters = ..."hello";
console.log(letters);  // ['h', 'e', 'l', 'l', 'o']
```

### Spread for Function Composition

```javascript
// students/05_spreadComposition.js

// Pipe function results
function pipe(value, ...fns) {
    return fns.reduce((acc, fn) => fn(acc), value);
}

const result = pipe(
    5,
    x => x * 2,
    x => x + 1,
    x => x ** 2
);
console.log(result);  // 121: ((5 * 2) + 1) ** 2

// Compose functions (right to left)
function compose(...fns) {
    return function(x) {
        return fns.reduceRight((acc, fn) => fn(acc), x);
    };
}

const process = compose(
    x => x.toString(),
    x => x * 10,
    x => x + 5
);
console.log(process(5));  // '100'

// Conditional function application
function maybeApply(value, ...operations) {
    let result = value;
    for (const [condition, operation] of operations) {
        if (condition) {
            result = operation(result);
        }
    }
    return result;
}

const data = { value: 10 };
const processed = maybeApply(
    data,
    [data.value > 5, x => ({ ...x, large: true })],
    [data.value > 100, x => ({ ...x, huge: true })]
);
console.log(processed);  // { value: 10, large: true }
```

### Spread vs Rest: Key Distinction

```javascript
// students/06_spreadVsRest.js

// REST: Collects multiple elements into an array
function collect(...args) {
    console.log('Rest collects:', args);
}
collect(1, 2, 3);  // [1, 2, 3]

// SPREAD: Expands array into individual elements
const arr = [1, 2, 3];
console.log('Spread expands:', ...arr);  // 1 2 3

// Practical: Dynamic function invocation
function execute(fn, ...args) {
    return fn(...args);  // Spread args into individual parameters
}

const add = (a, b) => a + b;
const greet = (name, greeting) => `${greeting}, ${name}!`;

console.log(execute(add, 2, 3));           // 5
console.log(execute(greet, 'Alice', 'Hello'));  // 'Hello, Alice!'
```

---

## Default Parameters

### Basic Default Parameters

```javascript
// students/07_defaultParams.js

// Default value for missing parameter
function greet(name = 'Guest') {
    return `Hello, ${name}!`;
}

console.log(greet('Alice'));  // 'Hello, Alice!'
console.log(greet());      // 'Hello, Guest!'

// Multiple defaults
function createUser(name = 'Unknown', role = 'user', active = true) {
    return { name, role, active, createdAt: new Date() };
}

console.log(createUser('Bob', 'admin'));
// { name: 'Bob', role: 'admin', active: true, ... }

console.log(createUser());
// { name: 'Unknown', role: 'user', active: true, ... }

// Default with expressions
function calculatePrice(basePrice, taxRate = 0.1, discount = basePrice * 0.05) {
    return basePrice + (basePrice * taxRate) - discount;
}

console.log(calculatePrice(100));  // 105 (100 + 10 - 5)
console.log(calculatePrice(100, 0.2));  // 115 (100 + 20 - 5)
```

### Default Parameters and undefined

```javascript
// students/08_undefinedDefaults.js

// undefined triggers default, null does not
function test(x = 'default') {
    return x;
}

console.log(test());        // 'default'
console.log(test(undefined));  // 'default'
console.log(test(null));     // null
console.log(test(0));       // 0
console.log(test(''));      // ''

// Practical: Optional configuration
function fetchData(url, options = {}) {
    const {
        method = 'GET',
        headers = {},
        timeout = 5000,
        retries = 3
    } = options;
    
    return { url, method, headers, timeout, retries };
}

console.log(fetchData('/api/users'));
console.log(fetchData('/api/users', { method: 'POST', timeout: 10000 }));
```

### Complex Default Expressions

```javascript
// students/09_complexDefaults.js

// Default with side effects (not recommended in production)
let counter = 0;
function getId(prefix = `ID-${++counter}`) {
    return prefix;
}

console.log(getId());  // 'ID-1'
console.log(getId());  // 'ID-2'

// Better: Factory function for unique IDs
function createIdGenerator() {
    let counter = 0;
    return function(prefix = 'ID') {
        return `${prefix}-${++counter}`;
    };
}

const getUserId = createIdGenerator();
console.log(getUserId());  // 'ID-1'
console.log(getUserId('USER'));  // 'USER-2'

// Date-based defaults
function scheduleEvent(title, date = new Date()) {
    return {
        title,
        date: date.toISOString(),
        timestamp: date.getTime()
    };
}

console.log(scheduleEvent('Meeting'));
console.log(scheduleEvent('Party', new Date('2025-01-01')));

// Deep copy default
function cloneDefault(obj = {}) {
    return JSON.parse(JSON.stringify(obj));
}

const defaultConfig = { theme: 'light', language: 'en' };
function getConfig(custom = {}) {
    return { ...cloneDefault(defaultConfig), ...custom };
}

console.log(getConfig({ language: 'es' }));
// { theme: 'light', language: 'es' }
```

---

## Destructuring in Parameters

### Object Destructuring

```javascript
// students/10_objectDestructuring.js

// Basic destructuring
function getFullName({ firstName, lastName }) {
    return `${firstName} ${lastName}`;
}

console.log(getFullName({ firstName: 'John', lastName: 'Doe' }));
// 'John Doe'

// With defaults
function createPoint({ x = 0, y = 0, label = 'origin' } = {}) {
    return { x, y, label };
}

console.log(createPoint({ x: 10, y: 20, label: 'A' }));
// { x: 10, y: 20, label: 'A' }
console.log(createPoint());
// { x: 0, y: 0, label: 'origin' }

// Renaming variables
function processUser({ name: userName, age: userAge }) {
    return `${userName} is ${userAge} years old`;
}

console.log(processUser({ name: 'Alice', age: 30 }));
// 'Alice is 30 years old'
```

### Array Destructuring

```javascript
// students/11_arrayDestructuring.js

// Basic array destructuring
function getCoordinates([x, y, z]) {
    return { x, y, z };
}

console.log(getCoordinates([1, 2, 3]));  // { x: 1, y: 2, z: 3 }

// With rest parameters
function getFirstAndRest([first, ...rest]) {
    return { first, rest };
}

console.log(getFirstAndRest([1, 2, 3, 4, 5]));
// { first: 1, rest: [2, 3, 4, 5] }

// Skip elements
function getSecondAndThird([, second, third]) {
    return { second, third };
}

console.log(getSecondAndThird([1, 2, 3, 4, 5]));
// { second: 2, third: 3 }

// Combined with rest
function processPairs([first, second, ...pairs]) {
    return { first, second, pairs };
}

console.log(processPairs(['a', 'b', 'c', 'd', 'e']));
// { first: 'a', second: 'b', pairs: ['c', 'd', 'e'] }
```

### Mixed Destructuring

```javascript
// students/12_mixedDestructuring.js

// Nested destructuring
function processOrder({
    customer: { name, email },
    items: [{ product: firstProduct }, ...otherItems],
    shipping: { address: { city, country } }
}) {
    return {
        customerName: name,
        customerEmail: email,
        firstProduct,
        otherItemsCount: otherItemsCount,
        shippingCity: city,
        shippingCountry: country
    };
}

const order = {
    customer: { name: 'Alice', email: 'alice@example.com' },
    items: [{ product: 'Laptop' }, { product: 'Mouse' }, { product: 'Keyboard' }],
    shipping: { address: { city: 'NYC', country: 'USA' } }
};

console.log(processOrder(order));
// { customerName: 'Alice', customerEmail: 'alice@example.com', ... }

// Complex type extraction
function processConfig({
    server: {
        host = 'localhost',
        port = 3000,
        secure = false
    },
    database: {
        connection: { max = 10, timeout = 5000 } = {}
    } = {},
    features: [useCache = true, useLogs = false, ...restFeatures] = []
}) {
    return {
        server: { host, port, secure },
        database: { max, timeout },
        features: { useCache, useLogs, restFeatures }
    };
}

const config = {
    server: { host: 'api.example.com', port: 8080 },
    database: { connection: { max: 50 } },
    features: [false, true, 'experimental']
};

console.log(processConfig(config));
```

---

## Arguments Object

### Legacy Arguments (Pre-ES6)

```javascript
// students/13_argumentsObject.js

// Arguments object (function declarations only)
function sumAll() {
    let total = 0;
    for (let i = 0; i < arguments.length; i++) {
        total += arguments[i];
    }
    return total;
}

console.log(sumAll(1, 2, 3, 4, 5));  // 15

// With default parameters (arguments.callee deprecated)
function factorial(n) {
    if (n <= 1) return 1;
    return n * arguments.callee(n - 1);
}

console.log(factorial(5));  // 120

// ⚠️ Arrow functions don't have arguments
// const arrowSum = () => arguments;  // ReferenceError
```

### Converting Arguments to Array

```javascript
// students/14_argumentsToArray.js

// Modern: Use rest parameters instead
function modernArgs(...args) {
    return args;
}

// Old way: Convert arguments to array
function legacyArgs() {
    return Array.prototype.slice.call(arguments);
}

console.log(legacyArgs(1, 2, 3));  // [1, 2, 3]

// More efficient conversion
function efficientArgs() {
    return [...arguments];
}

console.log(efficientArgs(1, 2, 3));  // [1, 2, 3]

// Practical: Generic argument processing
function processAllTypes() {
    const args = [...arguments];
    return {
        numbers: args.filter(n => typeof n === 'number'),
        strings: args.filter(s => typeof s === 'string'),
        objects: args.filter(o => typeof o === 'object' && o !== null),
        others: args.filter(x => 
            typeof x !== 'number' && 
            typeof x !== 'string' && 
            (typeof x !== 'object' || x === null)
        )
    };
}

console.log(processAllTypes(1, 'hello', { a: 1 }, 42, 'world', null, [1,2]));
// { numbers: [1, 42], strings: ['hello', 'world'], objects: [...], others: [null] }
```

---

## Parameter Validation

### Input Validation Patterns

```javascript
// students/15_validation.js

// Type checking
function validateParams(fn) {
    return function(...args) {
        console.log('Validating arguments...');
        return fn(...args);
    };
}

function add(a, b) {
    if (typeof a !== 'number' || typeof b !== 'number') {
        throw new TypeError('Arguments must be numbers');
    }
    if (isNaN(a) || isNaN(b)) {
        throw new RangeError('Arguments cannot be NaN');
    }
    return a + b;
}

console.log(add(2, 3));  // 5

// Parameter constraints with validation
function createUser(name, age, email) {
    if (typeof name !== 'string' || name.trim().length === 0) {
        throw new Error('Invalid name');
    }
    if (typeof age !== 'number' || age < 0 || age > 150) {
        throw new Error('Invalid age');
    }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        throw new Error('Invalid email');
    }
    
    return { name: name.trim(), age, email };
}

console.log(createUser('Alice', 30, 'alice@example.com'));
// { name: 'Alice', age: 30, email: 'alice@example.com' }
```

### Defensive Programming

```javascript
// students/16_defensive.js

// Optional parameter handling
function processWithDefaults(options = {}) {
    const {
        required = true,
        timeout = 5000,
        retries = 3,
        onError = () => {}
    } = options;
    
    return { required, timeout, retries, onError };
}

// Type coercion utilities
function toNumber(value, defaultValue = 0) {
    const num = Number(value);
    return isNaN(num) ? defaultValue : num;
}

function toArray(value) {
    return Array.isArray(value) ? value : [value];
}

function toString(value, defaultValue = '') {
    return value == null ? defaultValue : String(value);
}

console.log(toNumber('42'));    // 42
console.log(toNumber('abc', 0)); // 0
console.log(toArray('x'));    // ['x']
console.log(toArray([1,2])); // [1,2]
```

---

## Professional Use Cases

### 1. API Handler Pattern

```javascript
// students/17_apiHandler.js

function createApiHandler(handlers) {
    return function route(req, res) {
        const { method, path } = req;
        const handlerKey = `${method}:${path}`;
        
        const handler = handlers[handlerKey];
        
        if (!handler) {
            res.status(404).json({ error: 'Not found' });
            return;
        }
        
        try {
            const { params = {}, query = {}, body = {} } = req;
            
            const result = handler({ params, query, body });
            res.json(result);
        } catch (error) {
            res.status(400).json({ error: error.message });
        }
    };
}

const api = createApiHandler({
    'GET:/users': ({ query }) => {
        const { limit = 10, offset = 0 } = query;
        return { users: [], total: 0 };
    },
    'POST:/users': ({ body }) => {
        const { name, email } = body;
        return { id: 1, name, email };
    }
});
```

### 2. Function Builder Pattern

```javascript
// students/18_functionBuilder.js

function createQueryBuilder(table) {
    let conditions = [];
    let selections = ['*'];
    let limitCount;
    let offsetCount;
    
    return {
        select: function(...fields) {
            selections = fields.length > 0 ? fields : ['*'];
            return this;
        },
        
        where: function(field, operator, value) {
            conditions.push({ field, operator, value });
            return this;
        },
        
        limit: function(n) {
            limitCount = n;
            return this;
        },
        
        offset: function(n) {
            offsetCount = n;
            return this;
        },
        
        toSQL: function() {
            let sql = `SELECT ${selections.join(', ')} FROM ${table}`;
            
            if (conditions.length > 0) {
                const whereClauses = conditions.map(c => 
                    `${c.field} ${c.operator} ${c.value}`
                );
                sql += ` WHERE ${whereClauses.join(' AND ')}`;
            }
            
            if (limitCount !== undefined) {
                sql += ` LIMIT ${limitCount}`;
            }
            
            if (offsetCount !== undefined) {
                sql += ` OFFSET ${offsetCount}`;
            }
            
            return sql;
        }
    };
}

const query = createQueryBuilder('users')
    .select('id', 'name', 'email')
    .where('active', '=', 'true')
    .where('role', '=', 'admin')
    .limit(10)
    .offset(0);

console.log(query.toSQL());
// SELECT id, name, email FROM users WHERE active = 'true' AND role = 'admin' LIMIT 10 OFFSET 0
```

### 3. Event Handler with Options

```javascript
// students/19_eventHandler.js

function createEventHandler(options = {}) {
    const {
        enableLogging = false,
        errorHandler = console.error,
        beforeHook = () => {},
        afterHook = () => {}
    } = options;
    
    return function handle(event) {
        if (enableLogging) {
            console.log('Event received:', event);
        }
        
        beforeHook(event);
        
        try {
            const result = processEvent(event);
            afterHook(event, result);
            return result;
        } catch (error) {
            errorHandler(error, event);
            throw error;
        }
    };
}

function processEvent(event) {
    return { processed: true, event };
}

const sensitiveHandler = createEventHandler({
    enableLogging: false,
    errorHandler: (err) => {
        // Send to error tracking service
        console.error('Error:', err.message);
    }
});
```

---

## Common Pitfalls

### 1. Rest Parameter Position

```javascript
// students/20_pitfallRestOrder.js

// ❌ WRONG: Rest must be last
// function bad(a, ...rest, b) {}  // SyntaxError

// ✅ CORRECT: Rest is last parameter
function correct(a, ...rest) {
    return { first: a, rest };
}
```

### 2. Spread Creates Shallow Copy

```javascript
// students/21_pitfallSpreads.js

// ❌ WRONG: Spread only shallow copies
const original = { nested: { value: 1 } };
const copy = { ...original };
copy.nested.value = 2;
console.log(original.nested.value);  // 2 - modified!

// ✅ CORRECT: Deep clone when nested
const deepCopy = JSON.parse(JSON.stringify(original));
deepCopy.nested.value = 2;
console.log(original.nested.value);  // 1 - unchanged
```

### 3. Default Expression Evaluation

```javascript
// students/22_pitfallDefaults.js

// ❌ WRONG: Default evaluated on each call
let id = 0;
function createUserBad(name = `user-${++id}`) {
    return { name };
}
createUserBad();  // id: 1
createUserBad();  // id: 2 - id changes even when name provided!

// ✅ CORRECT: Factory pattern
function createIdGenerator() {
    let id = 0;
    return function(name) {
        return { name: name || `user-${++id}` };
    };
}
const gen = createIdGenerator();
console.log(gen());         // user-1
console.log(gen('Custom'));  // Custom - no side effect
```

---

## Key Takeaways

1. **Rest Parameters** (`...args`): Collect multiple arguments into an array. Must be the last parameter.

2. **Spread Operator** (`...array`): Expands an array into individual arguments. Used when calling functions.

3. **Default Parameters**: Provide fallback values. `undefined` triggers default, `null` does not.

4. **Destructuring**: Extract values from objects/arrays directly in parameter list. Supports defaults, renaming, nested structures.

5. **Arguments Object**: Legacy feature in regular functions. Prefer rest parameters in modern JavaScript.

6. **Validation**: Always validate parameters in production code. Use defensive programming patterns.

---

## Related Files

- [01_FUNCTION_DECLARATIONS_EXPRESSIONS.md](./01_FUNCTION_DECLARATIONS_EXPRESSIONS.md) - Function definition patterns
- [06_HIGHER_ORDER_FUNCTIONS.md](./06_HIGHER_ORDER_FUNCTIONS.md) - map, filter, reduce
- [04_ARROW_FUNCTIONS_MASTER.md](./04_ARROW_FUNCTIONS_MASTER.md) - Arrow function parameter handling