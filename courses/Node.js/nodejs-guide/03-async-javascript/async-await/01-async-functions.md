# Async Functions

## What You'll Learn

- What async/await is and why it's useful
- How to define async functions
- Using await to wait for Promises
- Return values from async functions

## What is Async/Await?

**Async/await** is syntactic sugar built on top of Promises. It makes asynchronous code look and behave more like synchronous code, making it easier to read and write.

## Defining Async Functions

### The async Keyword

```javascript
// async-function.js - Defining async functions

// Regular function
function normalFunction() {
  return 'Hello';
}

// Async function - returns a Promise automatically
async function asyncFunction() {
  return 'Hello';
}

// Both can be called the same way
console.log(normalFunction());   // 'Hello'
console.log(asyncFunction());    // Promise { 'Hello' }

// But with async, we can use await inside
async function greet() {
  return 'Hello, World!';
}

greet().then((message) => {
  console.log(message);  // 'Hello, World!'
});
```

### Arrow Functions as Async

```javascript
// async-arrow.js - Arrow functions with async

// Arrow function with async
const fetchData = async () => {
  return { id: 1, name: 'Alice' };
};

// Can also use await inside arrow async functions
const getUser = async (id) => {
  const response = await fetch(`/api/users/${id}`);
  return response.json();
};
```

## Using Await

### Waiting for Promises

```javascript
// await-basic.js - Using await

// A function that returns a promise
function delay(ms) {
  return new Promise((resolve) => {
    setTimeout(() => resolve('Done!'), ms);
  });
}

// Using async/await
async function main() {
  console.log('Starting...');
  
  // await pauses execution until promise resolves
  const result = await delay(1000);
  
  console.log(result);  // 'Done!'
  console.log('Finished!');
}

main();
```

### Multiple Awaits

```javascript
// multiple-await.js - Multiple await statements

function step1() {
  return new Promise((resolve) => 
    setTimeout(() => { console.log('Step 1'); resolve(1); }, 500)
  );
}

function step2() {
  return new Promise((resolve) => 
    setTimeout(() => { console.log('Step 2'); resolve(2); }, 500)
  );
}

function step3() {
  return new Promise((resolve) => 
    setTimeout(() => { console.log('Step 3'); resolve(3); }, 500)
  );
}

// Sequential execution - each waits for previous
async function runSequential() {
  const result1 = await step1();
  const result2 = await step2();
  const result3 = await step3();
  
  console.log('All done:', result1 + result2 + result3);
}

runSequential();
```

## Return Values

### Returning from Async Functions

```javascript
// async-return.js - Return values from async functions

// Async function that returns a value
async function getUser() {
  return { id: 1, name: 'Alice' };
}

// Calling it - returns a Promise!
getUser().then((user) => {
  console.log(user);  // { id: 1, name: 'Alice' }
});

// Or use await
async function main() {
  const user = await getUser();
  console.log(user);
}

main();
```

## Code Example: Complete Async Demo

```javascript
// async-demo.js - Complete async/await demonstration

console.log('=== Async/Await Demo ===\n');

// Helper functions
function delay(ms, value) {
  return new Promise((resolve) => 
    setTimeout(() => resolve(value), ms)
  );
}

function fetchUser(id) {
  return delay(100, { id, name: `User ${id}` });
}

function fetchPosts(userId) {
  return delay(100, [{ title: 'Post 1' }, { title: 'Post 2' }]);
}

// ─────────────────────────────────────────
// Example 1: Basic async/await
// ─────────────────────────────────────────
console.log('1. Basic Async/Await:');

async function basic() {
  const result = await delay(200, 'Hello after 200ms');
  console.log('   Result:', result);
}

basic();

// ─────────────────────────────────────────
// Example 2: Async with multiple awaits
// ─────────────────────────────────────────
console.log('\n2. Multiple Awaits:');

async function multi() {
  console.log('   Starting...');
  const a = await delay(100, 'first');
  const b = await delay(100, 'second');
  const c = await delay(100, 'third');
  console.log('   All:', a, b, c);
}

multi();

// ─────────────────────────────────────────
// Example 3: Async with error handling
// ─────────────────────────────────────────
console.log('\n3. Async with Errors:');

async function withErrors() {
  try {
    const user = await fetchUser(1);
    console.log('   User:', user.name);
  } catch (error) {
    console.log('   Error:', error.message);
  }
}

withErrors();

// ─────────────────────────────────────────
// Example 4: Parallel execution
// ─────────────────────────────────────────
console.log('\n4. Parallel Execution:');

async function parallel() {
  console.log('   Starting all at once...');
  
  // Start all at once with Promise.all
  const [user, posts] = await Promise.all([
    fetchUser(1),
    fetchPosts(1)
  ]);
  
  console.log('   User:', user.name);
  console.log('   Posts:', posts.length);
}

parallel();

// ─────────────────────────────────────────
// Example 5: Returning async results
// ─────────────────────────────────────────
console.log('\n5. Return Values:');

async function getFullData() {
  const user = await fetchUser(1);
  const posts = await fetchPosts(user.id);
  
  return {
    user,
    posts,
    fetchedAt: new Date().toISOString()
  };
}

// Calling the function
getFullData().then((data) => {
  console.log('   Got data:', data);
});
```

## Common Mistakes

### Mistake 1: Forgetting await

```javascript
// WRONG - doesn't wait for promise
async function bad() {
  const data = fetchUser(1);  // Returns Promise, not data!
  console.log(data);  // Promise { <pending> }
}

// CORRECT - use await
async function good() {
  const data = await fetchUser(1);
  console.log(data);  // { id: 1, name: 'User 1' }
}
```

### Mistake 2: Using await Outside async

```javascript
// WRONG - can't use await at top level (except modules)
const data = await fetchUser(1);  // SyntaxError!

// CORRECT - wrap in async function
async function main() {
  const data = await fetchUser(1);
  console.log(data);
}

main();

// In ES Modules (files with "type": "module"), you can use top-level await
```

### Mistake 3: Not Handling Errors

```javascript
// WRONG - unhandled rejection
async function risky() {
  const data = await Promise.reject(new Error('Oops!'));
  console.log(data);  // Never runs
}

// CORRECT - use try/catch
async function safe() {
  try {
    const data = await Promise.reject(new Error('Oops!'));
    console.log(data);
  } catch (error) {
    console.log('Caught:', error.message);
  }
}

safe();
```

## Try It Yourself

### Exercise 1: Convert to Async
Take a Promise chain and convert it to use async/await.

### Exercise 2: Multiple API Calls
Create async functions that fetch user data and their posts, then combine them.

### Exercise 3: Error Handling
Add try/catch to handle errors in your async functions.

## Next Steps

Now you understand async functions. Let's learn about error handling with try/catch. Continue to [Error Handling](./02-error-handling.md).
