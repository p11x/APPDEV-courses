# Functions, Objects, and Arrays Deep Dive

## What You'll Learn

- Advanced function patterns and techniques
- Object-oriented programming in JavaScript
- Array methods and manipulation
- Common patterns and best practices

## Advanced Function Patterns

### Higher-Order Functions

Functions that accept or return other functions.

```javascript
// Function that returns a function
function multiplier(factor) {
    return (number) => number * factor;
}

const double = multiplier(2);
const triple = multiplier(3);

console.log(double(5)); // 10
console.log(triple(5)); // 15

// Function that accepts a function
function applyOperation(array, operation) {
    return array.map(operation);
}

const numbers = [1, 2, 3, 4, 5];
const squared = applyOperation(numbers, n => n * n);
// [1, 4, 9, 16, 25]
```

### Function Composition

```javascript
// Composing functions together
const compose = (...fns) => (x) => fns.reduceRight((acc, fn) => fn(acc), x);

const addOne = x => x + 1;
const double = x => x * 2;
const square = x => x * x;

const transform = compose(square, double, addOne);
console.log(transform(3)); // ((3 + 1) * 2)^2 = 64

// Pipe (left to right)
const pipe = (...fns) => (x) => fns.reduce((acc, fn) => fn(acc), x);

const transform2 = pipe(addOne, double, square);
console.log(transform2(3)); // 64
```

### Memoization

Caching function results for performance.

```javascript
// Simple memoization
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

// Fibonacci with memoization
const fibonacci = memoize(function fib(n) {
    if (n <= 1) return n;
    return fib(n - 1) + fib(n - 2);
});

console.log(fibonacci(50)); // Fast due to memoization
```

### Currying

Transforming a function with multiple arguments into a sequence of functions.

```javascript
// Manual currying
function add(a) {
    return function(b) {
        return function(c) {
            return a + b + c;
        };
    };
}

console.log(add(1)(2)(3)); // 6

// Generic curry function
function curry(fn) {
    return function curried(...args) {
        if (args.length >= fn.length) {
            return fn.apply(this, args);
        }
        return function(...moreArgs) {
            return curried.apply(this, args.concat(moreArgs));
        };
    };
}

const addThree = curry((a, b, c) => a + b + c);
console.log(addThree(1)(2)(3)); // 6
console.log(addThree(1, 2)(3)); // 6
console.log(addThree(1, 2, 3)); // 6
```

## Object Patterns

### Object Methods and `this`

```javascript
const calculator = {
    value: 0,
    
    add(n) {
        this.value += n;
        return this; // Enable chaining
    },
    
    subtract(n) {
        this.value -= n;
        return this;
    },
    
    multiply(n) {
        this.value *= n;
        return this;
    },
    
    getResult() {
        return this.value;
    }
};

// Method chaining
const result = calculator
    .add(5)
    .multiply(3)
    .subtract(2)
    .getResult();

console.log(result); // 13
```

### Prototypal Inheritance

```javascript
// Constructor function
function Person(name, age) {
    this.name = name;
    this.age = age;
}

// Adding methods to prototype
Person.prototype.greet = function() {
    return `Hello, ${this.name}!`;
};

Person.prototype.getInfo = function() {
    return `${this.name} is ${this.age} years old.`;
};

// Inheritance
function Employee(name, age, company) {
    Person.call(this, name, age);
    this.company = company;
}

// Set up prototype chain
Employee.prototype = Object.create(Person.prototype);
Employee.prototype.constructor = Employee;

Employee.prototype.work = function() {
    return `${this.name} works at ${this.company}.`;
};

const emp = new Employee('Alice', 30, 'TechCorp');
console.log(emp.greet()); // Hello, Alice!
console.log(emp.work()); // Alice works at TechCorp.
```

### Object Property Descriptors

```javascript
const user = {};

// Define property with descriptor
Object.defineProperty(user, 'name', {
    value: 'Alice',
    writable: false,      // Cannot be changed
    enumerable: true,     // Shows in for...in loops
    configurable: false   // Cannot be deleted or reconfigured
});

user.name = 'Bob'; // Silently fails in non-strict mode
console.log(user.name); // Alice

// Define getter/setter
Object.defineProperty(user, 'age', {
    get: function() {
        return this._age;
    },
    set: function(value) {
        if (value < 0) throw new Error('Age cannot be negative');
        this._age = value;
    },
    enumerable: true
});

user.age = 25;
console.log(user.age); // 25
```

### Object.freeze, Object.seal

```javascript
// Object.freeze - completely immutable
const frozen = Object.freeze({
    name: 'Alice',
    address: { city: 'NYC' }
});

frozen.name = 'Bob'; // Silently fails
frozen.address.city = 'LA'; // Works (shallow freeze)
console.log(frozen.name); // Alice
console.log(frozen.address.city); // LA

// Object.seal - can't add/remove properties, but can modify
const sealed = Object.seal({
    name: 'Alice',
    age: 30
});

sealed.name = 'Bob'; // Works
sealed.email = 'alice@example.com'; // Fails
delete sealed.age; // Fails
console.log(sealed); // { name: 'Bob', age: 30 }
```

## Array Methods Deep Dive

### Transformation Methods

```javascript
const numbers = [1, 2, 3, 4, 5];

// map - transform each element
const doubled = numbers.map(n => n * 2);
// [2, 4, 6, 8, 10]

// flatMap - map then flatten
const sentences = ['Hello World', 'Foo Bar'];
const words = sentences.flatMap(s => s.split(' '));
// ['Hello', 'World', 'Foo', 'Bar']

// reduce - accumulate values
const sum = numbers.reduce((acc, n) => acc + n, 0);
// 15

// reduce with object
const grouped = ['apple', 'banana', 'avocado', 'cherry'].reduce((acc, fruit) => {
    const firstLetter = fruit[0];
    acc[firstLetter] = acc[firstLetter] || [];
    acc[firstLetter].push(fruit);
    return acc;
}, {});
// { a: ['apple', 'avocado'], b: ['banana'], c: ['cherry'] }
```

### Filtering Methods

```javascript
const numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

// filter - keep elements that pass test
const evens = numbers.filter(n => n % 2 === 0);
// [2, 4, 6, 8, 10]

// find - get first matching element
const firstEven = numbers.find(n => n % 2 === 0);
// 2

// findIndex - get index of first matching element
const firstEvenIndex = numbers.findIndex(n => n % 2 === 0);
// 1

// some - test if any element passes
const hasEven = numbers.some(n => n % 2 === 0);
// true

// every - test if all elements pass
const allPositive = numbers.every(n => n > 0);
// true
```

### Sorting

```javascript
// Default sort (lexicographic)
const fruits = ['banana', 'apple', 'cherry'];
fruits.sort();
// ['apple', 'banana', 'cherry']

// Numeric sort
const numbers = [10, 5, 40, 25, 1];
numbers.sort((a, b) => a - b); // Ascending
// [1, 5, 10, 25, 40]

numbers.sort((a, b) => b - a); // Descending
// [40, 25, 10, 5, 1]

// Sort objects
const users = [
    { name: 'Alice', age: 30 },
    { name: 'Bob', age: 25 },
    { name: 'Charlie', age: 35 }
];

users.sort((a, b) => a.age - b.age);
// Sorted by age ascending

// Stable sort preserves order of equal elements
const items = [
    { id: 1, priority: 'high' },
    { id: 2, priority: 'low' },
    { id: 3, priority: 'high' },
    { id: 4, priority: 'low' }
];

items.sort((a, b) => {
    const priorityOrder = { high: 0, medium: 1, low: 2 };
    return priorityOrder[a.priority] - priorityOrder[b.priority];
});
// Items with same priority maintain original order
```

### Array.from and Array.of

```javascript
// Array.from - create array from iterable
const letters = Array.from('hello');
// ['h', 'e', 'l', 'l', 'o']

// Array.from with mapping function
const squares = Array.from({ length: 5 }, (_, i) => i * i);
// [0, 1, 4, 9, 16]

// Create array from Set
const unique = Array.from(new Set([1, 2, 2, 3, 3, 3]));
// [1, 2, 3]

// Array.of - create array from arguments
const arr = Array.of(1, 2, 3);
// [1, 2, 3]

// Difference from Array constructor
const arr2 = Array(3); // [empty × 3] (length 3)
const arr3 = Array.of(3); // [3]
```

## Common Patterns

### Destructuring in Loops

```javascript
const users = [
    { name: 'Alice', age: 30 },
    { name: 'Bob', age: 25 },
    { name: 'Charlie', age: 35 }
];

// Destructuring in for...of
for (const { name, age } of users) {
    console.log(`${name} is ${age} years old.`);
}

// With index
for (const [index, { name }] of users.entries()) {
    console.log(`${index + 1}. ${name}`);
}
```

### Object Transformation

```javascript
const user = {
    firstName: 'John',
    lastName: 'Doe',
    age: 30,
    email: 'john@example.com'
};

// Transform to different structure
const formatted = {
    fullName: `${user.firstName} ${user.lastName}`,
    contact: user.email,
    isAdult: user.age >= 18
};

// Pick specific properties
const { firstName, lastName, ...rest } = user;
const nameOnly = { firstName, lastName };

// Omit specific properties
const { age, ...userWithoutAge } = user;
```

### Array Grouping

```javascript
// Group by property
const transactions = [
    { type: 'income', amount: 100 },
    { type: 'expense', amount: 50 },
    { type: 'income', amount: 200 },
    { type: 'expense', amount: 75 }
];

const grouped = transactions.reduce((acc, transaction) => {
    const key = transaction.type;
    acc[key] = acc[key] || [];
    acc[key].push(transaction);
    return acc;
}, {});

// { income: [...], expense: [...] }

// Calculate totals by group
const totals = transactions.reduce((acc, { type, amount }) => {
    acc[type] = (acc[type] || 0) + amount;
    return acc;
}, {});

// { income: 300, expense: 125 }
```

## Troubleshooting Common Issues

### `this` Context Lost

```javascript
// Problem: 'this' is undefined in callback
const obj = {
    name: 'Alice',
    greet: function() {
        setTimeout(function() {
            console.log(`Hello, ${this.name}`); // undefined
        }, 100);
    }
};

// Solution 1: Arrow function
const obj2 = {
    name: 'Alice',
    greet: function() {
        setTimeout(() => {
            console.log(`Hello, ${this.name}`); // Alice
        }, 100);
    }
};

// Solution 2: Bind
const obj3 = {
    name: 'Alice',
    greet: function() {
        setTimeout(function() {
            console.log(`Hello, ${this.name}`);
        }.bind(this), 100);
    }
};
```

### Array Reference Issues

```javascript
// Problem: Modifying original array
const original = [1, 2, 3];
const reference = original;
reference.push(4);
console.log(original); // [1, 2, 3, 4] - original modified!

// Solution: Create copy
const copy1 = [...original];
const copy2 = original.slice();
const copy3 = Array.from(original);

// Deep copy for nested structures
const nested = [[1, 2], [3, 4]];
const deepCopy = JSON.parse(JSON.stringify(nested));
```

### Sorting Stability

```javascript
// Problem: Unstable sort in older browsers
const items = [
    { id: 1, priority: 'high' },
    { id: 2, priority: 'low' },
    { id: 3, priority: 'high' }
];

// Solution: Include secondary sort criterion
items.sort((a, b) => {
    const priorityOrder = { high: 0, medium: 1, low: 2 };
    const priorityDiff = priorityOrder[a.priority] - priorityOrder[b.priority];
    
    if (priorityDiff !== 0) return priorityDiff;
    return a.id - b.id; // Secondary sort by id
});
```

## Best Practices Checklist

- [ ] Use arrow functions for callbacks to preserve `this`
- [ ] Use method chaining for fluent APIs
- [ ] Implement memoization for expensive computations
- [ ] Use `Object.freeze` for immutable data
- [ ] Prefer `map`, `filter`, `reduce` over manual loops
- [ ] Use destructuring for cleaner code
- [ ] Always provide initial value for `reduce`
- [ ] Use `Array.from` for creating arrays from iterables
- [ ] Implement stable sorting with secondary criteria
- [ ] Create copies when modifying arrays/objects

## Performance Optimization Tips

- `for` loops are faster than `forEach`, `map`, `filter`
- Avoid creating unnecessary arrays with `map`/`filter` chains
- Use `Object.keys()`, `Object.values()`, `Object.entries()` efficiently
- Cache array length in loops for performance
- Use `Set` for unique value operations
- Prefer `Object.assign` over spread for large objects

## Cross-References

- See [ES6+ Features](./01-es6-plus-features.md) for arrow functions and destructuring
- See [Computer Science Basics](../03-computer-science-basics/) for data structures
- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for linting rules
- See [Performance Optimization](../06-performance-optimization/) for optimization techniques

## Next Steps

Now that you understand advanced JavaScript patterns, let's explore computer science fundamentals. Continue to [Computer Science Basics](../03-computer-science-basics/).