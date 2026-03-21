# Promise Basics

## What You'll Learn

- What a Promise is and its states
- Creating Promises manually
- Using .then() and .catch()
- Handling success and failure

## What is a Promise?

A **Promise** is an object that represents a value that may be available now, or in the future, or never. It's JavaScript's way of handling asynchronous operations more elegantly than callbacks.

### Promise States

A Promise can be in one of three states:
- **Pending**: Initial state - the operation hasn't completed yet
- **Fulfilled (Resolved)**: The operation completed successfully
- **Rejected**: The operation failed

## Creating Promises

### Using the Promise Constructor

```javascript
// creating-promises.js - Creating Promises

// Create a new Promise
const myPromise = new Promise((resolve, reject) => {
  // This function runs immediately when Promise is created
  
  // Do some async work
  setTimeout(() => {
    const success = true;  // Simulate success or failure
    
    if (success) {
      // Resolve the promise with a value
      resolve('Operation completed successfully!');
    } else {
      // Reject the promise with an error
      reject(new Error('Operation failed!'));
    }
  }, 1000);
});

console.log('Promise created, waiting...');

myPromise
  .then((result) => {
    console.log('Success:', result);
  })
  .catch((error) => {
    console.error('Error:', error.message);
  });
```

### Promise States Example

```javascript
// promise-states.js - Understanding Promise states

// Create promises in different states
const resolvedPromise = Promise.resolve('Success value');
const rejectedPromise = Promise.reject(new Error('Failure reason'));

// Pending promise
const pendingPromise = new Promise((resolve, reject) => {
  // This stays pending until we call resolve or reject
  setTimeout(() => resolve('Done!'), 1000);
});

console.log('Resolved:', resolvedPromise);
console.log('Rejected:', rejectedPromise);
console.log('Pending:', pendingPromise);

// Check them after they settle
setTimeout(() => {
  console.log('\nAfter timeout:');
  console.log('Pending now:', pendingPromise);
}, 2000);
```

## Using .then() and .catch()

### Basic .then()

```javascript
// then-catch.js - Using then and catch

// Create a promise that resolves after 1 second
const fetchData = () => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      resolve({ id: 1, name: 'Alice' });
    }, 1000);
  });
};

console.log('Fetching data...');

// .then() runs when the promise resolves
fetchData()
  .then((data) => {
    console.log('Received:', data);
    return data.name;  // Return value becomes next promise's result
  })
  .then((name) => {
    console.log('Name:', name);
  });
```

### Handling Errors with .catch()

```javascript
// error-handling.js - Error handling with Promises

const failingOperation = () => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      reject(new Error('Something went wrong!'));
    }, 1000);
  });
};

failingOperation()
  .then((result) => {
    console.log('Success:', result);
  })
  .catch((error) => {
    console.error('Caught error:', error.message);
  });
```

### Chaining .then() Calls

```javascript
// chaining.js - Chaining promises

const step1 = () => {
  return new Promise((resolve) => {
    setTimeout(() => {
      console.log('Step 1 complete');
      resolve(1);
    }, 500);
  });
};

const step2 = (num) => {
  return new Promise((resolve) => {
    setTimeout(() => {
      console.log('Step 2 complete, received:', num);
      resolve(num + 10);
    }, 500);
  });
};

const step3 = (num) => {
  return new Promise((resolve) => {
    setTimeout(() => {
      console.log('Step 3 complete, received:', num);
      resolve(num * 2);
    }, 500);
  });
};

// Chain the steps
step1()
  .then(step2)
  .then(step3)
  .then((finalResult) => {
    console.log('Final result:', finalResult);
  });
```

## Code Example: Complete Promise Demo

```javascript
// promise-demo.js - Complete Promise demonstration

console.log('=== Promise Demo ===\n');

// ─────────────────────────────────────────
// 1. Creating a Promise
// ─────────────────────────────────────────
console.log('1. Creating a Promise:');

function delay(ms) {
  return new Promise((resolve) => {
    setTimeout(() => resolve(ms), ms);
  });
}

delay(100).then((ms) => {
  console.log(`   Waited ${ms} milliseconds`);
});

// ─────────────────────────────────────────
// 2. Promise with success and failure
// ─────────────────────────────────────────
console.log('\n2. Success and Failure:');

function getUser(id) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (id > 0) {
        resolve({ id, name: `User ${id}` });
      } else {
        reject(new Error('Invalid user ID'));
      }
    }, 500);
  });
}

getUser(1)
  .then((user) => console.log('   User:', user))
  .catch((err) => console.error('   Error:', err.message));

getUser(-1)
  .then((user) => console.log('   User:', user))
  .catch((err) => console.error('   Error:', err.message));

// ─────────────────────────────────────────
// 3. Chaining
// ─────────────────────────────────────────
console.log('\n3. Chaining Promises:');

getUser(1)
  .then((user) => {
    console.log('   Got user:', user.name);
    return user.name.toUpperCase();  // Transform data
  })
  .then((upperName) => {
    console.log('   Uppercased:', upperName);
    return upperName.length;  // Return new value
  })
  .then((length) => {
    console.log('   Name length:', length);
  })
  .catch((err) => console.error('   Error:', err.message));

// ─────────────────────────────────────────
// 4. Finally - runs regardless of success/failure
// ─────────────────────────────────────────
console.log('\n4. Finally:');

getUser(1)
  .then((user) => {
    console.log('   User:', user.name);
    // throw new Error('Oops!');  // Uncomment to test error
  })
  .catch((err) => {
    console.error('   Caught:', err.message);
  })
  .finally(() => {
    console.log('   Finally: cleanup complete!');
  });
```

## Converting Callback Code to Promises

```javascript
// promisify.js - Converting callbacks to Promises

import { readFile } from 'fs';

// Callback-based (older way)
function readFileCallback(filename, callback) {
  readFile(filename, 'utf8', callback);
}

// Promise-based (modern way)
function readFilePromise(filename) {
  return new Promise((resolve, reject) => {
    readFile(filename, 'utf8', (err, data) => {
      if (err) reject(err);
      else resolve(data);
    });
  });
}

// Using the promise version
readFilePromise('package.json')
  .then((content) => console.log('File size:', content.length))
  .catch((err) => console.error('Error:', err.message));
```

## Common Mistakes

### Mistake 1: Forgetting to Return

```javascript
// WRONG - promise chain breaks
fetchData()
  .then((data) => {
    process(data);  // Forgot to return!
  })
  .then((result) => {
    console.log(result);  // undefined!
  });

// CORRECT - return the value
fetchData()
  .then((data) => {
    return process(data);  // Return the promise
  })
  .then((result) => {
    console.log(result);  // Now works!
  });
```

### Mistake 2: Not Handling Rejections

```javascript
// WRONG - unhandled rejection warning
fetchData()
  .then((data) => console.log(data));
// If fetchData rejects, you'll get a warning!

// CORRECT - always catch
fetchData()
  .then((data) => console.log(data))
  .catch((err) => console.error(err));
```

### Mistake 3: Creating Promises for Sync Code

```javascript
// UNNECESSARY - don't wrap sync code in promises
const promise = new Promise((resolve) => {
  resolve(1 + 1);  // This is synchronous anyway!
});

// Just do:
const result = 1 + 1;
```

## Try It Yourself

### Exercise 1: Create a Promise
Create a function that returns a Promise and resolves after a random delay.

### Exercise 2: Chain Promises
Create multiple functions that return Promises and chain them together.

### Exercise 3: Error Handling
Add error handling to your Promise chain with .catch().

## Next Steps

You understand Promise basics. Let's learn about Promise chaining and error propagation. Continue to [Promise Chaining](./02-promise-chaining.md).
