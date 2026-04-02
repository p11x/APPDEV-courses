# Callback Hell and Solutions

## What You'll Learn

- What callback hell (pyramid of doom) is
- Why deeply nested callbacks are problematic
- Techniques to avoid callback hell
- Introduction to Promises as a better alternative

## What is Callback Hell?

**Callback hell** (also called the "pyramid of doom") happens when you have many nested callbacks, making the code hard to read and maintain.

### Visual Example

```javascript
// callback-hell.js - The pyramid of doom

// This is callback hell - hard to read and maintain!
getData((err, data1) => {
  if (err) {
    console.error(err);
    return;
  }
  
  processData(data1, (err, data2) => {
    if (err) {
      console.error(err);
      return;
    }
    
    saveData(data2, (err, data3) => {
      if (err) {
        console.error(err);
        return;
      }
      
      sendNotification(data3, (err, result) => {
        if (err) {
          console.error(err);
          return;
        }
        
        console.log('Final result:', result);
      });
    });
  });
});
```

## Problems with Callback Hell

1. **Hard to read**: Code flows diagonally, not linearly
2. **Hard to maintain**: Adding new steps is awkward
3. **Error handling重复**: Same error handling repeated
4. **Debugging困难**: Hard to trace errors
5. **Scope confusion**: Variables from outer scopes can cause issues

## Solutions to Callback Hell

### Solution 1: Named Functions

Extract callbacks into named functions:

```javascript
// named-functions.js - Avoiding callback hell with named functions

// Define each step as a named function
function getDataAndProcess(err, data) {
  if (err) {
    console.error('Error getting data:', err);
    return;
  }
  
  console.log('Got data:', data);
  processData(data, saveDataAndNotify);
}

function saveDataAndNotify(err, processedData) {
  if (err) {
    console.error('Error processing:', err);
    return;
  }
  
  console.log('Processed:', processedData);
  saveData(processedData, sendNotification);
}

function sendNotification(err, savedData) {
  if (err) {
    console.error('Error saving:', err);
    return;
  }
  
  console.log('Saved:', savedData);
  notifyUser(savedData, finish);
}

function finish(err, result) {
  if (err) {
    console.error('Error notifying:', err);
    return;
  }
  
  console.log('Complete!', result);
}

// Start the chain
getData(getDataAndProcess);
```

### Solution 2: Modularize Code

Split into separate modules/files:

```javascript
// modular.js - Separate concerns into modules

// Step 1: getUser.js
async function getUser(userId) {
  return new Promise((resolve, reject) => {
    // Simulated database call
    setTimeout(() => {
      resolve({ id: userId, name: 'Alice' });
    }, 100);
  });
}

// Step 2: getOrders.js
async function getOrders(userId) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      resolve([{ id: 1, item: 'Book' }]);
    }, 100);
  });
}

// Main - uses async/await (see next section!)
async function main() {
  const user = await getUser(1);
  const orders = await getOrders(user.id);
  console.log(user, orders);
}

main();
```

### Solution 3: Use Promises (Recommended)

Promises provide a cleaner way to handle async operations:

```javascript
// promise-solution.js - Using Promises instead of callbacks

// Convert callback-based to Promise-based
function fetchUser(userId) {
  return new Promise((resolve, reject) => {
    // Simulate async operation
    setTimeout(() => {
      if (userId > 0) {
        resolve({ id: userId, name: `User ${userId}` });
      } else {
        reject(new Error('Invalid user ID'));
      }
    }, 100);
  });
}

// Chain Promises - much cleaner!
fetchUser(1)
  .then(user => {
    console.log('Got user:', user.name);
    return fetchUser(2);  // Chain next operation
  })
  .then(user2 => {
    console.log('Got user 2:', user2.name);
    return fetchUser(3);
  })
  .then(user3 => {
    console.log('Got user 3:', user3.name);
  })
  .catch(error => {
    console.error('Error:', error.message);
  });
```

## Code Example: Transforming Callback Code

```javascript
// transform-callbacks.js - Before and after

// ═══════════════════════════════════════
// BEFORE: Callback Hell
// ═══════════════════════════════════════
console.log('=== BEFORE: Callback Hell ===');

function callbackStyle(callback) {
  setTimeout(() => {
    callback(null, 'Step 1');
  }, 100);
}

function callbackStyle2(data, callback) {
  setTimeout(() => {
    callback(null, data + ' -> Step 2');
  }, 100);
}

function callbackStyle3(data, callback) {
  setTimeout(() => {
    callback(null, data + ' -> Step 3');
  }, 100);
}

// Callback hell
callbackStyle((err, result1) => {
  if (err) { console.error(err); return; }
  
  callbackStyle2(result1, (err, result2) => {
    if (err) { console.error(err); return; }
    
    callbackStyle3(result2, (err, result3) => {
      if (err) { console.error(err); return; }
      
      console.log('Final:', result3);
    });
  });
});

// ═══════════════════════════════════════
// AFTER: Promise Chain
// ═══════════════════════════════════════
console.log('\n=== AFTER: Promise Chain ===');

// Wrap in Promise
function promiseStyle() {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve('Step 1');
    }, 100);
  });
}

function promiseStyle2(data) {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(data + ' -> Step 2');
    }, 100);
  });
}

function promiseStyle3(data) {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(data + ' -> Step 3');
    }, 100);
  });
}

// Clean promise chain
promiseStyle()
  .then(result => promiseStyle2(result))
  .then(result => promiseStyle3(result))
  .then(result => console.log('Final:', result))
  .catch(err => console.error(err));
```

## Async/Await (Even Better!)

Modern JavaScript has async/await which makes async code look synchronous:

```javascript
// async-await.js - Using async/await

// Convert to async functions
async function main() {
  try {
    const result1 = await promiseStyle();
    console.log(result1);
    
    const result2 = await promiseStyle2(result1);
    console.log(result2);
    
    const result3 = await promiseStyle3(result2);
    console.log('Final:', result3);
    
  } catch (error) {
    console.error('Error:', error.message);
  }
}

main();
```

## Common Mistakes

### Mistake 1: Mixing Callbacks and Promises

```javascript
// WRONG - mixing paradigms
async function bad() {
  const result = await someCallbackFunction(); // Won't work!
}

// CORRECT - convert callbacks to promises first
function promisified() {
  return new Promise((resolve, reject) => {
    someCallbackFunction((err, data) => {
      if (err) reject(err);
      else resolve(data);
    });
  });
}
```

### Mistake 2: Not Handling Errors in Chains

```javascript
// WRONG - no error handling
fetchData()
  .then(result => process(result))
  .then(final => console.log(final));
// If error occurs, it's silently ignored!

// CORRECT - always catch errors
fetchData()
  .then(result => process(result))
  .then(final => console.log(final))
  .catch(error => console.error('Error:', error));
```

## Try It Yourself

### Exercise 1: Identify Callback Hell
Find callback hell in existing code and refactor to use named functions.

### Exercise 2: Convert to Promises
Take a callback-based function and convert it to return a Promise.

### Exercise 3: Use Async/Await
Rewrite Promise chains to use async/await syntax.

## Next Steps

Callbacks are the foundation of async JavaScript, but Promises offer a cleaner approach. Continue to [Promise Basics](../promises/01-promise-basics.md) to learn more about Promises.
