# Error Handling with Async/Await

## What You'll Learn

- Using try/catch for error handling
- The finally block
- Re-throwing errors
- Best practices for error handling

## Try/Catch with Async/Await

When using async/await, errors are handled using traditional try/catch blocks, making error handling look synchronous.

### Basic Try/Catch

```javascript
// try-catch-basic.js - Basic error handling

function maybeFail(shouldFail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (shouldFail) {
        reject(new Error('Operation failed!'));
      } else {
        resolve('Success!');
      }
    }, 500);
  });
}

async function handleSuccess() {
  try {
    const result = await maybeFail(false);
    console.log('Result:', result);
  } catch (error) {
    console.error('Caught error:', error.message);
  }
}

async function handleFailure() {
  try {
    const result = await maybeFail(true);
    console.log('Result:', result);
  } catch (error) {
    console.error('Caught error:', error.message);
  }
}

handleSuccess();
handleFailure();
```

### Multiple Awaits with Single Try/Catch

```javascript
// multiple-await-catch.js - Handling multiple awaits

function delay(ms, value, fail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (fail) reject(new Error(value));
      else resolve(value);
    }, ms);
  });
}

async function processAll() {
  try {
    // All these run, but if any fails, we catch it
    const a = await delay(100, 'first');
    const b = await delay(100, 'second');
    const c = await delay(100, 'third');
    
    console.log('All:', a, b, c);
  } catch (error) {
    console.error('Error occurred:', error.message);
  }
}

processAll();
```

## The Finally Block

The `finally` block runs regardless of whether the operation succeeded or failed. It's perfect for cleanup tasks.

### Using Finally

```javascript
// finally-example.js - Using finally

function fetchData(shouldFail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (shouldFail) reject(new Error('Fetch failed'));
      else resolve({ data: 'some data' });
    }, 500);
  });
}

async function getData() {
  console.log('Starting fetch...');
  
  try {
    const data = await fetchData(false);
    console.log('Got data:', data);
  } catch (error) {
    console.error('Error:', error.message);
  } finally {
    // This always runs - for cleanup
    console.log('Cleanup: Closing connections, etc.');
  }
}

getData();
```

## Re-throwing Errors

Sometimes you want to catch an error, do something, then re-throw it.

```javascript
// rethrow.js - Re-throwing errors

async function fetchUser(id) {
  if (id <= 0) {
    throw new Error('Invalid ID');
  }
  
  return { id, name: `User ${id}` };
}

async function getUserWithLogging(id) {
  try {
    const user = await fetchUser(id);
    console.log('Got user:', user);
    return user;
  } catch (error) {
    console.log('Logging error:', error.message);
    // Re-throw so caller can also handle it
    throw error;
  }
}

// Handle at the top level
getUserWithLogging(-1)
  .catch((err) => console.log('Top level caught:', err.message));
```

## Code Example: Complete Error Handling Demo

```javascript
// error-handling-demo.js - Complete demonstration

console.log('=== Async Error Handling Demo ===\n');

// Helper to simulate operations
function operation(name, ms, shouldFail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (shouldFail) {
        reject(new Error(`${name} failed`));
      } else {
        resolve(`${name} success`);
      }
    }, ms);
  });
}

// ─────────────────────────────────────────
// 1. Basic try/catch
// ─────────────────────────────────────────
console.log('1. Basic Try/Catch:');

async function basic() {
  try {
    const result = await operation('Task', 100);
    console.log('   Result:', result);
  } catch (error) {
    console.log('   Error:', error.message);
  }
}

basic();

// ─────────────────────────────────────────
// 2. Try/catch with failure
// ─────────────────────────────────────────
console.log('\n2. Catch on Failure:');

async function catchFailure() {
  try {
    await operation('Task', 100, true);
  } catch (error) {
    console.log('   Caught:', error.message);
  }
}

catchFailure();

// ─────────────────────────────────────────
// 3. Finally block
// ─────────────────────────────────────────
console.log('\n3. Finally Block:');

async function withFinally() {
  let loading = true;
  
  try {
    await operation('Load', 100);
    console.log('   Loaded successfully');
  } catch (error) {
    console.log('   Error:', error.message);
  } finally {
    loading = false;
    console.log('   Finally: loading =', loading);
  }
}

withFinally();

// ─────────────────────────────────────────
// 4. Multiple operations with single catch
// ─────────────────────────────────────────
console.log('\n4. Multiple Operations:');

async function multiple() {
  try {
    await operation('Step 1', 100);
    await operation('Step 2', 100);
    await operation('Step 3', 100, true);  // This fails
    await operation('Step 4', 100);  // Never runs
  } catch (error) {
    console.log('   Caught at:', error.message);
  }
}

multiple();

// ─────────────────────────────────────────
// 5. Rethrowing
// ─────────────────────────────────────────
console.log('\n5. Rethrowing:');

async function outer() {
  try {
    await inner();
  } catch (error) {
    console.log('   Outer caught:', error.message);
  }
}

async function inner() {
  try {
    await operation('Inner task', 100, true);
  } catch (error) {
    console.log('   Inner caught:', error.message);
    throw error;  // Rethrow
  }
}

outer();
```

## Best Practices

### 1. Catch Specific Errors

```javascript
// Specific errors
async function handleErrors() {
  try {
    await riskyOperation();
  } catch (error) {
    if (error.code === 'NOT_FOUND') {
      console.log('Resource not found');
    } else if (error.code === 'UNAUTHORIZED') {
      console.log('Please log in');
    } else {
      console.log('Unknown error:', error);
    }
  }
}
```

### 2. Don't Catch Everything Blindly

```javascript
// BAD - catching everything
async function bad() {
  try {
    await something();
  } catch (e) {
    // Swallowing the error - bad!
  }
}

// GOOD - specific handling or rethrow
async function good() {
  try {
    await something();
  } catch (error) {
    console.error('Failed:', error.message);
    throw error;  // Or handle specifically
  }
}
```

### 3. Use Finally for Cleanup

```javascript
// Good cleanup pattern
async function withCleanup() {
  const connection = await connect();
  
  try {
    return await process(connection);
  } finally {
    await connection.close();  // Always cleanup
  }
}
```

## Common Mistakes

### Mistake 1: Not Using Try/Catch

```javascript
// WRONG - unhandled rejection
async function bad() {
  await Promise.reject(new Error('Oops'));
}

// CORRECT
async function good() {
  try {
    await Promise.reject(new Error('Oops'));
  } catch (error) {
    console.log('Caught:', error.message);
  }
}
```

### Mistake 2: Forgetting Await in Catch

```javascript
// WRONG - promise not awaited
async function bad() {
  try {
    await fetchData();
  } catch (error) {
    // handleError returns a promise but we're not awaiting it!
    handleError(error);  // Missing await!
  }
}

// CORRECT
async function good() {
  try {
    await fetchData();
  } catch (error) {
    await handleError(error);  // Await the async handler
  }
}
```

### Mistake 3: Nested Try/Catch When Not Needed

```javascript
// Unnecessary nesting
async function nested() {
  try {
    try {
      await step1();
    } catch (e) {
      // handle step1 error
    }
    
    try {
      await step2();
    } catch (e) {
      // handle step2 error
    }
  } catch (e) {
    // outer catch
  }
}

// Better - single try/catch if appropriate
async function better() {
  try {
    await step1();
    await step2();
  } catch (error) {
    // handle all errors
  }
}
```

## Try It Yourself

### Exercise 1: Error Handling Practice
Create a function that fetches data and handles network errors, parsing errors, and other errors differently.

### Exercise 2: Cleanup with Finally
Add a finally block that always runs cleanup code, regardless of success or failure.

### Exercise 3: Retry Logic
Implement retry logic that catches errors and retries the operation up to N times.

## Next Steps

Now you understand error handling with async/await. Let's learn about top-level await - using await outside of async functions in ES Modules. Continue to [Top-Level Await](./03-top-level-await.md).
