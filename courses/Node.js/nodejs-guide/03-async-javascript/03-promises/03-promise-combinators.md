# Promise Combinators

## What You'll Learn

- Promise.all() - wait for all promises
- Promise.allSettled() - wait for all to settle
- Promise.race() - first one to settle wins
- Promise.any() - first one to fulfill wins
- When to use each combinator

## Promise Combinators Overview

Promise combinators are methods that work with multiple Promises at once. They help you coordinate parallel operations.

## Promise.all()

Waits for **all** promises to resolve. If **any** rejects, the whole thing rejects.

```javascript
// promise-all.js - Promise.all() example

function delay(ms, value) {
  return new Promise((resolve) => setTimeout(() => resolve(value), ms));
}

// Run multiple promises in parallel
Promise.all([
  delay(100, 'first'),
  delay(200, 'second'),
  delay(150, 'third')
]).then((results) => {
  console.log('All done!', results);
  // Results: ['first', 'second', 'third']
});
```

### Handling Errors with Promise.all()

```javascript
// all-with-error.js - Promise.all() error handling

function delay(ms, value, shouldFail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (shouldFail) {
        reject(new Error(`Failed: ${value}`));
      } else {
        resolve(value);
      }
    }, ms);
  });
}

// Success case
Promise.all([
  delay(100, 'one'),
  delay(200, 'two'),
  delay(150, 'three')
]).then((results) => {
  console.log('Success:', results);
}).catch((err) => {
  console.log('Error:', err.message);  // Won't run
});

// Failure case - one rejects, all fail
Promise.all([
  delay(100, 'one'),
  delay(200, 'two'),
  delay(150, 'ERROR', true)  // This will reject
]).then((results) => {
  console.log('Success:', results);  // Won't run
}).catch((err) => {
  console.log('Error:', err.message);  // "Failed: ERROR"
});
```

## Promise.allSettled()

Waits for **all** promises to settle (either resolve or reject). Never rejects!

```javascript
// promise-allsettled.js - Promise.allSettled()

function delay(ms, value, shouldFail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (shouldFail) {
        reject(new Error(`Failed: ${value}`));
      } else {
        resolve(value);
      }
    }, ms);
  });
}

// All settle - even if some fail
Promise.allSettled([
  delay(100, 'first'),
  delay(200, 'second'),
  delay(150, 'ERROR', true)
]).then((results) => {
  console.log('All settled:');
  results.forEach((result, index) => {
    if (result.status === 'fulfilled') {
      console.log(`  ${index}: ${result.value}`);
    } else {
      console.log(`  ${index}: ${result.reason.message}`);
    }
  });
});
```

Use `allSettled()` when you need results from all operations regardless of failures.

## Promise.race()

The first promise to settle (resolve or reject) wins the race!

```javascript
// promise-race.js - Promise.race()

function delay(ms, value) {
  return new Promise((resolve) => setTimeout(() => resolve(value), ms));
}

function failDelay(ms, value) {
  return new Promise((resolve, reject) => 
    setTimeout(() => reject(new Error(value)), ms)
  );
}

// Race - fastest wins
Promise.race([
  delay(100, 'fast'),
  delay(200, 'slow'),
  delay(300, 'slower')
]).then((result) => {
  console.log('Winner:', result);  // 'fast'
});

// Race with a reject - if it settles first, it wins
Promise.race([
  failDelay(100, 'fast fail'),  // Rejects in 100ms
  delay(200, 'slow success')     // Resolves in 200ms
]).then((result) => {
  console.log('Winner:', result);  // Won't run - already failed!
}).catch((err) => {
  console.log('Race error:', err.message);  // 'fast fail'
});
```

Use `race()` to implement timeouts:

```javascript
// timeout-example.js - Using race for timeout

function fetchData() {
  return new Promise((resolve) => {
    // Imagine this is a very slow network request
    setTimeout(() => resolve('Data!'), 5000);
  });
}

function timeout(ms) {
  return new Promise((_, reject) => 
    setTimeout(() => reject(new Error('Timeout!')), ms)
  );
}

// Race data fetch against timeout
Promise.race([
  fetchData(),
  timeout(2000)  // 2 second timeout
])
  .then((result) => {
    console.log('Got data:', result);
  })
  .catch((err) => {
    console.log('Failed:', err.message);  // Will timeout
  });
```

## Promise.any()

The first promise to **fulfill** (resolve) wins. Ignores rejections!

```javascript
// promise-any.js - Promise.any()

function delay(ms, value, shouldFail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (shouldFail) {
        reject(new Error(`Failed: ${value}`));
      } else {
        resolve(value);
      }
    }, ms);
  });
}

// First success wins
Promise.any([
  delay(300, 'slow'),
  delay(100, 'fast'),
  delay(200, 'medium')
]).then((result) => {
  console.log('First success:', result);  // 'fast'
});

// Ignores rejections until all fail
Promise.any([
  delay(100, 'a', true),   // Rejects
  delay(100, 'b', true),   // Rejects
  delay(100, 'c', true)    // Rejects
]).then((result) => {
  console.log('Success:', result);  // Won't run
}).catch((err) => {
  console.log('All failed:', err.errors.map(e => e.message));
});
```

## Code Example: Complete Combinator Demo

```javascript
// combinator-demo.js - Complete demonstration

console.log('=== Promise Combinators Demo ===\n');

// Helper to create delayed promises
function createPromise(ms, value, fail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (fail) reject(new Error(value));
      else resolve(value);
    }, ms);
  });
}

// ─────────────────────────────────────────
// 1. Promise.all() - all must succeed
// ─────────────────────────────────────────
console.log('1. Promise.all():');

Promise.all([
  createPromise(100, 'one'),
  createPromise(200, 'two'),
  createPromise(150, 'three')
]).then((results) => {
  console.log('   Results:', results);
});

// ─────────────────────────────────────────
// 2. Promise.allSettled() - wait for all
// ─────────────────────────────────────────
console.log('\n2. Promise.allSettled():');

Promise.allSettled([
  createPromise(100, 'success'),
  createPromise(200, 'error', true),
  createPromise(150, 'also success')
]).then((results) => {
  results.forEach((r, i) => {
    const status = r.status === 'fulfilled' ? '✓' : '✗';
    const value = r.status === 'fulfilled' ? r.value : r.reason.message;
    console.log(`   ${i}: ${status} ${value}`);
  });
});

// ─────────────────────────────────────────
// 3. Promise.race() - first to settle
// ─────────────────────────────────────────
console.log('\n3. Promise.race():');

Promise.race([
  createPromise(300, 'slow'),
  createPromise(100, 'fast'),
  createPromise(200, 'medium')
]).then((result) => {
  console.log('   Winner:', result);
});

// ─────────────────────────────────────────
// 4. Promise.any() - first to fulfill
// ─────────────────────────────────────────
console.log('\n4. Promise.any():');

Promise.any([
  createPromise(100, 'error', true),
  createPromise(200, 'success!'),
  createPromise(300, 'error', true)
]).then((result) => {
  console.log('   First success:', result);
});
```

## When to Use Each Combinator

| Combinator | Use When |
|------------|----------|
| `Promise.all()` | All operations must succeed; fail fast on any error |
| `Promise.allSettled()` | Need results from all, even if some fail |
| `Promise.race()` | Need the first result (or implement timeouts) |
| `Promise.any()` | Need at least one success; ignore failures |

## Common Mistakes

### Mistake 1: Using all() When You Need allSettled()

```javascript
// WRONG - one failure causes entire all() to fail
Promise.all([
  fetch('/api/users'),
  fetch('/api/posts'),  // Might fail
  fetch('/api/comments')
]).then(([users, posts, comments]) => {
  // Won't run if any request fails!
});

// CORRECT - use allSettled() for multiple requests
const results = await Promise.allSettled([
  fetch('/api/users'),
  fetch('/api/posts'),
  fetch('/api/comments')
]);
// Handle each result individually
```

### Mistake 2: Confusing race() and any()

```javascript
// race() - first to SETTLE (resolve or reject)
// any() - first to FULFILL (only resolve)

Promise.race([
  Promise.reject(new Error('fast fail')),  // Settles first!
  Promise.resolve('slow success')
]).catch(err => console.log('race error'));  // Gets the rejection!

Promise.any([
  Promise.reject(new Error('fast fail')),
  Promise.resolve('slow success')
]).then(result => console.log('any success'));  // Gets the success!
```

## Try It Yourself

### Exercise 1: Multiple API Calls
Use Promise.all() to fetch multiple API endpoints simultaneously and combine results.

### Exercise 2: Timeout Pattern
Use Promise.race() to implement a timeout for a slow operation.

### Exercise 3: Partial Failure
Use Promise.allSettled() to handle a scenario where some operations may fail.

## Next Steps

You now know about Promises and their combinators. Let's move to async/await, which is the modern way to write asynchronous code. Continue to [Async Functions](./03-async-javascript/async-await/01-async-functions.md).
