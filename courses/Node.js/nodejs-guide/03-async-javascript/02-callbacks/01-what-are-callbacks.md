# What are Callbacks?

## What You'll Learn

- What callbacks are and why they're used
- The error-first callback pattern
- How Node.js uses callbacks for async operations
- Writing your own callback-based functions

## Understanding Callbacks

A **callback** is a function that's passed as an argument to another function and is executed after some operation completes. Callbacks are Node.js's fundamental way of handling asynchronous operations.

### Why Do We Need Callbacks?

In JavaScript, code can run synchronously (line by line, in order) or asynchronously (not waiting for previous operations to finish). Callbacks let us specify what should happen after an async operation completes.

```javascript
// sync-vs-async.js - Synchronous vs Asynchronous

// Synchronous - runs immediately
console.log('1. Start');
console.log('2. Process');
console.log('3. End');

// Asynchronous with callback
console.log('1. Start');

// setTimeout is a built-in function that takes a callback
// It waits 1 second, then runs the callback
setTimeout(() => {
  console.log('2. This runs after 1 second');
}, 1000);

console.log('3. End (callback not done yet!)');
```

## Callback Basics

### Creating a Callback Function

```javascript
// basic-callback.js - Basic callback usage

// A function that accepts a callback
function greet(name, callback) {
  const message = `Hello, ${name}!`;
  // Call the callback with the result
  callback(message);
}

// Define the callback function
function displayGreeting(message) {
  console.log(message);
}

// Pass the callback to the function
greet('Alice', displayGreeting);

// Or use an inline/arrow callback
greet('Bob', (message) => {
  console.log(message);
});
```

### Callback with Error Handling (Error-First Pattern)

Node.js uses a special pattern called **error-first callbacks**. The first parameter of the callback is always an error object (or null/undefined if no error):

```javascript
// error-first.js - Error-first callback pattern

import { readFile } from 'fs';

// Node.js style: callback(error, result)
// If error exists, handle it
// If no error, use the result

// Using the callback-based readFile (not promises)
readFile('nonexistent.txt', 'utf8', (err, data) => {
  if (err) {
    console.error('Error:', err.message);
    return;
  }
  
  console.log('File content:', data);
});

// Successful read
readFile('package.json', 'utf8', (err, data) => {
  if (err) {
    console.error('Error:', err.message);
    return;
  }
  
  console.log('File size:', data.length, 'bytes');
});
```

## Writing Callback-Based Functions

### Creating Your Own Async Callback Function

```javascript
// custom-callback.js - Creating callback-based functions

// Simulate an async operation (like reading a file)
function fetchUserData(userId, callback) {
  // Simulate async operation with setTimeout
  setTimeout(() => {
    // Check for "errors"
    if (!userId) {
      callback(new Error('User ID is required'), null);
      return;
    }
    
    // Simulate successful result
    const user = {
      id: userId,
      name: `User ${userId}`,
      email: `user${userId}@example.com`
    };
    
    // Call callback with no error and the result
    callback(null, user);
  }, 1000);
}

// Use the function
console.log('Fetching user...');

fetchUserData(1, (err, user) => {
  if (err) {
    console.error('Error:', err.message);
    return;
  }
  
  console.log('User found:', user);
});

fetchUserData(null, (err, user) => {
  if (err) {
    console.error('Error:', err.message);
    return;
  }
  
  console.log('User:', user);
});
```

## Code Example: Complete Callback Demo

```javascript
// callback-demo.js - Complete callback demonstration

import { readFile } from 'fs';

// ─────────────────────────────────────────
// Example 1: Simple callback
// ─────────────────────────────────────────
console.log('=== Example 1: Simple Callback ===');

function add(a, b, callback) {
  const result = a + b;
  callback(result);
}

add(5, 3, (result) => {
  console.log('5 + 3 =', result);
});

// ─────────────────────────────────────────
// Example 2: Error-first callback (simulated)
// ─────────────────────────────────────────
console.log('\n=== Example 2: Error-First Callback ===');

function divide(a, b, callback) {
  if (b === 0) {
    // Error case: first argument is the error
    callback(new Error('Cannot divide by zero'), null);
    return;
  }
  
  // Success case: first argument is null
  callback(null, a / b);
}

divide(10, 2, (err, result) => {
  if (err) {
    console.log('Error:', err.message);
    return;
  }
  console.log('10 / 2 =', result);
});

divide(10, 0, (err, result) => {
  if (err) {
    console.log('Error:', err.message);
    return;
  }
  console.log('Result:', result);
});

// ─────────────────────────────────────────
// Example 3: fs.readFile with callback
// ─────────────────────────────────────────
console.log('\n=== Example 3: fs.readFile Callback ===');

readFile('package.json', 'utf8', (err, data) => {
  if (err) {
    console.log('Error reading file:', err.message);
    return;
  }
  
  console.log('package.json content length:', data.length, 'chars');
});

// ─────────────────────────────────────────
// Example 4: Nested callbacks (async operations)
// ─────────────────────────────────────────
console.log('\n=== Example 4: Sequential Operations ===');

function stepOne(callback) {
  setTimeout(() => {
    console.log('Step 1 complete');
    callback(null, 'result-1');
  }, 500);
}

function stepTwo(prevResult, callback) {
  setTimeout(() => {
    console.log('Step 2 complete, got:', prevResult);
    callback(null, 'result-2');
  }, 500);
}

function stepThree(prevResult, callback) {
  setTimeout(() => {
    console.log('Step 3 complete, got:', prevResult);
    callback(null, 'result-3');
  }, 500);
}

// Chain the operations
stepOne((err, result1) => {
  if (err) {
    console.log('Error:', err);
    return;
  }
  
  stepTwo(result1, (err, result2) => {
    if (err) {
      console.log('Error:', err);
      return;
    }
    
    stepThree(result2, (err, result3) => {
      if (err) {
        console.log('Error:', err);
        return;
      }
      
      console.log('All steps complete, final result:', result3);
    });
  });
});
```

## How It Works

1. **Function accepts callback**: `function fetchData(callback)`
2. **Async operation happens**: Inside, we do something async (like setTimeout, reading a file)
3. **Callback invoked**: When done, we call `callback(error, result)`
4. **Callback executes**: The code we passed runs with the result

## Common Mistakes

### Mistake 1: Not Handling Errors

```javascript
// WRONG - ignoring errors
readFile('file.txt', 'utf8', (err, data) => {
  console.log(data);  // Will crash if error!
});

// CORRECT - always check for errors
readFile('file.txt', 'utf8', (err, data) => {
  if (err) {
    console.error('Error:', err.message);
    return;
  }
  console.log(data);
});
```

### Mistake 2: Forgetting the Callback

```javascript
// WRONG - callback never called
function getData(callback) {
  // Forgot to call callback!
  const data = 'something';
}

// CORRECT - always call callback
function getData(callback) {
  const data = 'something';
  callback(null, data);
}
```

### Mistake 3: Callback Hell (Nesting Too Deep)

```javascript
// BAD - deeply nested callbacks (callback hell)
doSomething((err, result) => {
  if (err) return;
  doSomethingElse((err, result) => {
    if (err) return;
    andAnotherThing((err, result) => {
      // This gets hard to read!
    });
  });
});
```

We'll learn how to avoid this with Promises and async/await.

## Try It Yourself

### Exercise 1: Callback Basics
Create a function that takes a name and a callback, and greets the name using the callback.

### Exercise 2: Error Handling
Create a function that validates input and uses error-first callbacks.

### Exercise 3: Async Simulation
Create a function that simulates fetching data from an API using callbacks.

## Next Steps

Now you understand callbacks. Let's explore the problems that can arise from excessive callback nesting. Continue to [Callback Hell](./02-callback-hell.md).
