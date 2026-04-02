# ES6+ Features for Node.js Development

## What You'll Learn

- Modern JavaScript features essential for Node.js
- Arrow functions, destructuring, and spread operators
- Template literals and string manipulation
- Classes and inheritance patterns

## ES6+ Features Overview

ES6 (ECMAScript 2015) and later versions introduced many features that are fundamental to modern Node.js development.

## Arrow Functions

Arrow functions provide concise syntax and lexical `this` binding.

### Basic Syntax

```javascript
// Traditional function
function add(a, b) {
    return a + b;
}

// Arrow function (implicit return)
const add = (a, b) => a + b;

// Arrow function with body
const multiply = (a, b) => {
    const result = a * b;
    return result;
};

// Single parameter (parentheses optional)
const double = n => n * 2;

// No parameters
const greet = () => 'Hello!';
```

### Lexical `this` Binding

Arrow functions don't have their own `this` context - they inherit from parent scope.

```javascript
class Timer {
    constructor() {
        this.seconds = 0;
        
        // Traditional function - 'this' is undefined
        setInterval(function() {
            this.seconds++; // Error: Cannot read property 'seconds' of undefined
        }, 1000);
        
        // Arrow function - 'this' is bound to Timer instance
        setInterval(() => {
            this.seconds++; // Works correctly
        }, 1000);
    }
}

// Callback example
const user = {
    name: 'Alice',
    sayHello: function() {
        // 'this' refers to user object
        setTimeout(() => {
            console.log(`Hello, ${this.name}!`); // 'Alice'
        }, 100);
    }
};
```

### When NOT to Use Arrow Functions

```javascript
// Don't use as object methods (lose 'this' context)
const person = {
    name: 'Bob',
    greet: () => {
        console.log(`Hi, ${this.name}`); // 'this' is undefined/global
    }
};

// Use traditional function for object methods
const person2 = {
    name: 'Bob',
    greet: function() {
        console.log(`Hi, ${this.name}`); // Works correctly
    }
};

// Don't use as constructors
const User = (name) => {
    this.name = name; // Error: not a constructor
};
```

## Destructuring

Destructuring allows extracting values from arrays and objects into distinct variables.

### Object Destructuring

```javascript
const user = {
    name: 'Alice',
    age: 30,
    email: 'alice@example.com',
    address: {
        city: 'New York',
        country: 'USA'
    }
};

// Basic destructuring
const { name, age, email } = user;
console.log(name); // 'Alice'

// Renaming variables
const { name: userName, age: userAge } = user;
console.log(userName); // 'Alice'

// Default values
const { phone = 'N/A' } = user;
console.log(phone); // 'N/A'

// Nested destructuring
const { address: { city, country } } = user;
console.log(city); // 'New York'

// Rest operator in destructuring
const { name, ...rest } = user;
console.log(rest); // { age: 30, email: '...', address: {...} }
```

### Array Destructuring

```javascript
const colors = ['red', 'green', 'blue', 'yellow'];

// Basic array destructuring
const [first, second] = colors;
console.log(first); // 'red'
console.log(second); // 'green'

// Skipping elements
const [, , third] = colors;
console.log(third); // 'blue'

// Rest operator with arrays
const [primary, ...others] = colors;
console.log(others); // ['green', 'blue', 'yellow']

// Swapping variables
let a = 1, b = 2;
[a, b] = [b, a];
console.log(a, b); // 2, 1

// Destructuring with functions
function getCoordinates() {
    return [40.7128, -74.0060];
}

const [latitude, longitude] = getCoordinates();
```

### Destructuring in Function Parameters

```javascript
// Object parameter destructuring
function createUser({ name, email, age = 25 }) {
    return {
        id: Math.random().toString(36).substr(2, 9),
        name,
        email,
        age,
        createdAt: new Date()
    };
}

const user = createUser({
    name: 'Alice',
    email: 'alice@example.com'
});

// Array parameter destructuring
function processPoint([x, y]) {
    return { x, y, distance: Math.sqrt(x*x + y*y) };
}

const point = processPoint([3, 4]);
console.log(point.distance); // 5
```

## Spread Operator

The spread operator (`...`) expands iterables into individual elements.

### Spread with Arrays

```javascript
// Copying arrays
const original = [1, 2, 3];
const copy = [...original];

// Merging arrays
const arr1 = [1, 2, 3];
const arr2 = [4, 5, 6];
const merged = [...arr1, ...arr2]; // [1, 2, 3, 4, 5, 6]

// Adding elements
const withExtra = [...original, 4, 5]; // [1, 2, 3, 4, 5]

// Converting iterable to array
const str = 'hello';
const chars = [...str]; // ['h', 'e', 'l', 'l', 'o']

// Using with Math functions
const numbers = [5, 2, 8, 1, 9];
const max = Math.max(...numbers); // 9
const min = Math.min(...numbers); // 1
```

### Spread with Objects

```javascript
// Copying objects
const original = { a: 1, b: 2 };
const copy = { ...original };

// Merging objects
const defaults = { timeout: 5000, retries: 3 };
const userSettings = { timeout: 10000 };
const config = { ...defaults, ...userSettings };
// { timeout: 10000, retries: 3 }

// Adding properties
const user = { name: 'Alice', age: 30 };
const userWithRole = { ...user, role: 'admin' };

// Overwriting properties
const updated = { ...user, age: 31 };

// Shallow copy only (nested objects still reference original)
const original2 = { nested: { value: 1 } };
const copy2 = { ...original2 };
copy2.nested.value = 2;
console.log(original2.nested.value); // 2 (still references original)
```

### Rest Parameters

```javascript
// Rest parameters collect arguments into array
function sum(...numbers) {
    return numbers.reduce((total, num) => total + num, 0);
}

sum(1, 2, 3, 4); // 10

// Mixed parameters
function logFirstAndRest(first, ...rest) {
    console.log('First:', first);
    console.log('Rest:', rest);
}

logFirstAndRest(1, 2, 3, 4);
// First: 1
// Rest: [2, 3, 4]
```

## Template Literals

Template literals provide enhanced string creation with interpolation and multi-line support.

### String Interpolation

```javascript
const name = 'Alice';
const age = 30;

// Basic interpolation
const greeting = `Hello, ${name}!`;
console.log(greeting); // 'Hello, Alice!'

// Expression interpolation
const message = `${name} is ${age} years old and will be ${age + 1} next year.`;

// Function calls in interpolation
const items = ['apple', 'banana', 'orange'];
const list = `Items: ${items.join(', ')}`;

// Conditional expressions
const status = 'active';
const badge = `Status: ${status === 'active' ? '✓' : '✗'}`;
```

### Multi-line Strings

```javascript
// Traditional multi-line (error in ES5)
const html = '<div>\n' +
             '  <h1>Title</h1>\n' +
             '</div>';

// Template literal multi-line
const html = `
<div>
  <h1>Title</h1>
</div>
`;

// SQL query example
const query = `
SELECT *
FROM users
WHERE age > ${minAge}
  AND status = '${status}'
ORDER BY created_at DESC
LIMIT ${limit}
`;
```

### Tagged Templates

```javascript
// Custom template tag function
function highlight(strings, ...values) {
    return strings.reduce((result, str, i) => {
        const value = values[i] ? `<mark>${values[i]}</mark>` : '';
        return result + str + value;
    }, '');
}

const name = 'Alice';
const score = 95;
const message = highlight`Student ${name} scored ${score}%`;
// "Student <mark>Alice</mark> scored <mark>95</mark>%"

// Tagged template for SQL escaping
function sql(strings, ...values) {
    const escaped = values.map(val => 
        typeof val === 'string' ? `'${val.replace(/'/g, "''")}'` : val
    );
    return strings.reduce((query, str, i) => 
        query + str + (escaped[i] || ''), '');
}

const userInput = "O'Reilly";
const safeQuery = sql`SELECT * FROM authors WHERE name = ${userInput}`;
// "SELECT * FROM authors WHERE name = 'O''Reilly'"
```

## Classes and Inheritance

ES6 classes provide syntactic sugar for prototype-based inheritance.

### Basic Class Syntax

```javascript
class User {
    // Constructor method
    constructor(name, email) {
        this.name = name;
        this.email = email;
        this.createdAt = new Date();
    }
    
    // Instance method
    greet() {
        return `Hello, ${this.name}!`;
    }
    
    // Get property
    get info() {
        return `${this.name} (${this.email})`;
    }
    
    // Set property
    set updateEmail(newEmail) {
        if (!newEmail.includes('@')) {
            throw new Error('Invalid email');
        }
        this.email = newEmail;
    }
    
    // Static method (called on class, not instance)
    static createAnonymous() {
        return new User('Anonymous', 'anonymous@example.com');
    }
}

// Usage
const user = new User('Alice', 'alice@example.com');
console.log(user.greet()); // 'Hello, Alice!'
console.log(user.info); // 'Alice (alice@example.com)'
user.updateEmail = 'new@example.com';

const anon = User.createAnonymous();
```

### Inheritance with `extends`

```javascript
class Animal {
    constructor(name) {
        this.name = name;
    }
    
    speak() {
        return `${this.name} makes a sound.`;
    }
}

class Dog extends Animal {
    constructor(name, breed) {
        super(name); // Call parent constructor
        this.breed = breed;
    }
    
    speak() {
        return `${this.name} barks!`;
    }
    
    fetch() {
        return `${this.name} fetches the ball!`;
    }
}

class Cat extends Animal {
    speak() {
        return `${this.name} meows!`;
    }
    
    purr() {
        return `${this.name} purrs...`;
    }
}

// Usage
const dog = new Dog('Rex', 'German Shepherd');
console.log(dog.speak()); // 'Rex barks!'
console.log(dog.fetch()); // 'Rex fetches the ball!'

const cat = new Cat('Whiskers');
console.log(cat.speak()); // 'Whiskers meows!'
console.log(cat.purr()); // 'Whiskers purrs...'
```

### Private Fields and Methods

```javascript
class BankAccount {
    // Private fields
    #balance;
    #accountNumber;
    
    constructor(initialBalance) {
        this.#balance = initialBalance;
        this.#accountNumber = Math.random().toString(36).substr(2, 12);
    }
    
    // Public method accessing private field
    deposit(amount) {
        if (amount <= 0) throw new Error('Invalid amount');
        this.#balance += amount;
        return this.#balance;
    }
    
    withdraw(amount) {
        if (amount > this.#balance) throw new Error('Insufficient funds');
        this.#balance -= amount;
        return this.#balance;
    }
    
    get balance() {
        return this.#balance;
    }
    
    // Private method
    #validateAmount(amount) {
        return amount > 0 && amount <= this.#balance;
    }
}

const account = new BankAccount(100);
account.deposit(50); // 150
// account.#balance; // SyntaxError: Private field
// account.#validateAmount(50); // SyntaxError: Private method
```

## Troubleshooting Common Issues

### Arrow Function `this` Context

```javascript
// Problem: Arrow function in object method
const obj = {
    value: 42,
    getValue: () => this.value // undefined
};

// Solution: Use traditional function
const obj2 = {
    value: 42,
    getValue: function() { return this.value; }
};
```

### Destructuring with Missing Properties

```javascript
// Problem: Undefined when property doesn't exist
const { missing } = {};
console.log(missing); // undefined

// Solution: Provide default values
const { missing = 'default' } = {};
console.log(missing); // 'default'
```

### Spread with Nested Objects

```javascript
// Problem: Shallow copy doesn't clone nested objects
const original = { nested: { value: 1 } };
const copy = { ...original };
copy.nested.value = 2; // Changes original

// Solution: Deep clone
const deepCopy = JSON.parse(JSON.stringify(original));
// Or use structuredClone (modern)
const deepCopy2 = structuredClone(original);
```

## Best Practices Checklist

- [ ] Use arrow functions for callbacks and functional programming
- [ ] Use traditional functions for object methods and constructors
- [ ] Leverage destructuring for cleaner function parameters
- [ ] Use spread operator for immutable array/object operations
- [ ] Prefer template literals over string concatenation
- [ ] Use tagged templates for safe string processing
- [ ] Implement classes with proper encapsulation
- [ ] Use private fields for sensitive data
- [ ] Always call `super()` in derived class constructors

## Performance Optimization Tips

- Arrow functions are slightly faster than traditional functions
- Destructuring can be slower than direct property access
- Spread operator creates new arrays/objects (memory overhead)
- Template literals are faster than string concatenation
- Class methods are optimized by modern JavaScript engines

## Cross-References

- See [Computer Science Basics](../03-computer-science-basics/) for data structure fundamentals
- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for linting rules
- See [Node.js Installation](../05-nodejs-installation/) for environment setup

## Next Steps

Now that you understand ES6+ features, let's dive deeper into functions, objects, and arrays. Continue to [Functions, Objects, and Arrays Deep Dive](./02-functions-objects-arrays.md).