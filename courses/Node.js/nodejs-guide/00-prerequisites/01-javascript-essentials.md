# JavaScript Essentials

## What You'll Learn

- Core JavaScript concepts needed before learning Node.js
- Variables, functions, and scope
- Objects, arrays, and destructuring
- Arrow functions and template literals
- Error handling basics

## Variables and Scope

```js
// Modern JavaScript uses const and let — never var

// const: cannot be reassigned (use by default)
const appName = 'My App';
const maxRetries = 3;

// let: can be reassigned (use when value changes)
let requestCount = 0;
requestCount++;  // OK — let allows reassignment

// var: OLD — function-scoped, hoisted, avoid entirely
// var leaked = 'do not use';  // BAD
```

## Functions

```js
// Function declaration — hoisted (can be called before definition)
function greet(name) {
  return `Hello, ${name}!`;
}

// Arrow function — concise, not hoisted, lexically binds `this`
const add = (a, b) => a + b;

// Arrow function with body
const calculate = (x, y) => {
  const sum = x + y;
  return sum * 2;
};

// Default parameters
const createUser = (name, role = 'user') => ({ name, role });

// Rest parameters
const sum = (...numbers) => numbers.reduce((a, b) => a + b, 0);
sum(1, 2, 3, 4);  // 10
```

## Objects and Destructuring

```js
// Object shorthand
const name = 'Alice';
const user = { name, role: 'admin' };  // { name: 'Alice', role: 'admin' }

// Destructuring
const { name: userName, role } = user;
console.log(userName);  // 'Alice'
console.log(role);       // 'admin'

// Nested destructuring
const config = { db: { host: 'localhost', port: 5432 } };
const { db: { host, port } } = config;

// Spread operator
const defaults = { timeout: 5000, retries: 3 };
const overrides = { timeout: 10000 };
const config2 = { ...defaults, ...overrides };  // { timeout: 10000, retries: 3 }
```

## Arrays and Iteration

```js
// Array methods — these are essential for Node.js
const numbers = [1, 2, 3, 4, 5];

// map: transform each element
const doubled = numbers.map((n) => n * 2);  // [2, 4, 6, 8, 10]

// filter: keep elements that pass a test
const evens = numbers.filter((n) => n % 2 === 0);  // [2, 4]

// reduce: combine elements into a single value
const total = numbers.reduce((sum, n) => sum + n, 0);  // 15

// find: get first matching element
const found = numbers.find((n) => n > 3);  // 4

// some/every: test conditions
const hasEven = numbers.some((n) => n % 2 === 0);   // true
const allPositive = numbers.every((n) => n > 0);     // true

// for...of: iterate over values
for (const num of numbers) {
  console.log(num);
}
```

## Template Literals

```js
// Template literals use backticks — essential for Node.js
const user = 'Alice';
const count = 5;

// String interpolation
console.log(`${user} has ${count} items`);

// Multi-line strings
const html = `
  <div>
    <h1>${user}</h1>
    <p>Items: ${count}</p>
  </div>
`;

// Tagged templates (advanced)
const sql = (strings, ...values) => {
  // Used by database libraries for safe query building
  return strings.reduce((result, str, i) => {
    return result + str + (values[i] ?? '');
  }, '');
};

const query = sql`SELECT * FROM users WHERE name = ${user}`;
```

## Error Handling

```js
// try/catch — essential for async code
try {
  const data = JSON.parse('invalid json');
} catch (err) {
  console.error('Parse error:', err.message);
}

// Throwing errors
function divide(a, b) {
  if (b === 0) throw new Error('Cannot divide by zero');
  return a / b;
}

// Custom error classes
class ValidationError extends Error {
  constructor(field, message) {
    super(message);
    this.name = 'ValidationError';
    this.field = field;
  }
}

// finally — always runs, even if error is thrown
try {
  const resource = acquireResource();
  useResource(resource);
} catch (err) {
  console.error(err);
} finally {
  releaseResource();  // Always runs
}
```

## Async/Await Preview

```js
// Promises — the foundation of async Node.js
const fetchData = () => {
  return new Promise((resolve, reject) => {
    setTimeout(() => resolve({ data: 'hello' }), 1000);
  });
};

// async/await — cleaner syntax for promises
async function loadData() {
  try {
    const result = await fetchData();
    console.log(result.data);
  } catch (err) {
    console.error('Failed:', err.message);
  }
}

// Parallel execution with Promise.all
const [users, posts] = await Promise.all([
  fetchUsers(),
  fetchPosts(),
]);
```

## Modules (ESM)

```js
// math.js — export functions
export const add = (a, b) => a + b;
export const subtract = (a, b) => a - b;
export default function multiply(a, b) { return a * b; }

// app.js — import functions
import multiply, { add, subtract } from './math.js';
```

## Common Mistakes

### Mistake 1: Using var

```js
// WRONG — var is function-scoped and hoisted
for (var i = 0; i < 3; i++) {
  setTimeout(() => console.log(i), 100);  // Prints 3, 3, 3 (not 0, 1, 2)
}

// CORRECT — let is block-scoped
for (let i = 0; i < 3; i++) {
  setTimeout(() => console.log(i), 100);  // Prints 0, 1, 2
}
```

### Mistake 2: Not Handling Async Errors

```js
// WRONG — unhandled promise rejection crashes Node.js
async function load() {
  const data = await fetchData();  // If this rejects, crash!
}

// CORRECT — always handle async errors
async function load() {
  try {
    const data = await fetchData();
  } catch (err) {
    console.error('Load failed:', err.message);
  }
}
```

### Mistake 3: Mutating const Objects

```js
// WRONG — trying to reassign a const
const user = { name: 'Alice' };
user = { name: 'Bob' };  // TypeError: Assignment to constant variable

// CORRECT — const prevents reassignment, not mutation
const user = { name: 'Alice' };
user.name = 'Bob';  // OK — modifying property, not reassigning
```

## Next Steps

Now that you know the JavaScript fundamentals, let's learn the command line basics needed for Node.js development. Continue to [Command Line Basics](./02-command-line-basics.md).
