# Promise Chaining and Error Propagation

## What You'll Learn

- How to chain Promises together
- Error propagation through Promise chains
- Returning values in chains
- Handling multiple errors

## Promise Chaining

Promise chaining allows you to execute multiple asynchronous operations in sequence, where each operation starts after the previous one completes.

### Basic Chain

```javascript
// basic-chain.js - Basic Promise chaining

function step1() {
  return new Promise((resolve) => {
    setTimeout(() => {
      console.log('Step 1 complete');
      resolve({ step: 1, data: 'from step 1' });
    }, 500);
  });
}

function step2(input) {
  return new Promise((resolve) => {
    setTimeout(() => {
      console.log('Step 2 complete, received:', input);
      resolve({ ...input, step: 2, data: 'from step 2' });
    }, 500);
  });
}

function step3(input) {
  return new Promise((resolve) => {
    setTimeout(() => {
      console.log('Step 3 complete, received:', input);
      resolve({ ...input, step: 3, final: true });
    }, 500);
  });
}

// Chain them together
step1()
  .then(step2)
  .then(step3)
  .then((finalResult) => {
    console.log('\nFinal result:', finalResult);
  });
```

## Returning Values in Chains

Each `.then()` can return:
- A value (becomes resolved value of next Promise)
- Another Promise (chain continues with that Promise)
- Nothing (next `.then()` receives undefined)

```javascript
// returning-values.js - Returning values in chains

function addOne(num) {
  return num + 1;
}

function multiplyByTwo(num) {
  return num * 2;
}

// Chain transformations
Promise.resolve(5)
  .then(addOne)    // Returns 6
  .then(addOne)    // Returns 7
  .then(multiplyByTwo)  // Returns 14
  .then((result) => {
    console.log('Final:', result);  // 14
  });

// Returning promises - chain waits for them
function asyncAddOne(num) {
  return new Promise((resolve) => {
    setTimeout(() => resolve(num + 1), 100);
  });
}

Promise.resolve(5)
  .then(asyncAddOne)  // Waits for promise
  .then(asyncAddOne)  // Waits for promise
  .then((result) => {
    console.log('Async final:', result);  // 7
  });
```

## Error Propagation

Errors in Promise chains propagate automatically. If any Promise rejects, the chain jumps to the nearest `.catch()`.

### Error Flow

```javascript
// error-propagation.js - Error propagation in chains

function succeed() {
  return Promise.resolve('Success!');
}

function fail() {
  return Promise.reject(new Error('Failed!'));
}

// Error propagates to first catch
fail()
  .then(() => console.log('This runs'))
  .then(() => console.log('This also runs'))
  .catch((err) => {
    console.log('Caught error:', err.message);
  });

// Success followed by error
succeed()
  .then(() => fail())
  .then(() => console.log('After fail - not reached'))
  .catch((err) => {
    console.log('Caught error from fail():', err.message);
  });
```

### Multiple Catch Blocks

```javascript
// multiple-catches.js - Multiple error handlers

function maybeFail(shouldFail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (shouldFail) {
        reject(new Error('Operation failed'));
      } else {
        resolve('Success');
      }
    }, 100);
  });
}

// Each catch only catches errors before it
maybeFail(false)
  .then(() => maybeFail(true))  // This fails
  .then(() => console.log('Not reached'))
  .catch((err) => {
    console.log('First catch:', err.message);
    // Can handle error here or rethrow
    throw err;  // Re-throw to propagate
  })
  .catch((err) => {
    console.log('Second catch:', err.message);
  });
```

## Code Example: Complete Chain Demo

```javascript
// chain-demo.js - Complete Promise chaining demonstration

console.log('=== Promise Chaining Demo ===\n');

// Simulated async operations
function fetchUser(id) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (id > 0) {
        resolve({ id, name: `User ${id}` });
      } else {
        reject(new Error('Invalid user ID'));
      }
    }, 200);
  });
}

function fetchPosts(userId) {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve([
        { id: 1, title: 'Post 1' },
        { id: 2, title: 'Post 2' }
      ]);
    }, 200);
  });
}

function fetchComments(postId) {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve([{ id: 1, text: 'Great post!' }]);
    }, 200);
  });
}

// ─────────────────────────────────────────
// Example 1: Successful chain
// ─────────────────────────────────────────
console.log('1. Successful Chain:');

fetchUser(1)
  .then((user) => {
    console.log('   User:', user.name);
    return fetchPosts(user.id);
  })
  .then((posts) => {
    console.log('   Posts:', posts.length);
    return fetchComments(posts[0].id);
  })
  .then((comments) => {
    console.log('   Comments:', comments.length);
    return comments;
  })
  .then((comments) => {
    console.log('   Final comment:', comments[0].text);
  });

// ─────────────────────────────────────────
// Example 2: Error handling
// ─────────────────────────────────────────
console.log('\n2. Error Handling:');

fetchUser(-1)  // This will fail
  .then((user) => {
    console.log('   User:', user.name);  // Won't run
    return fetchPosts(user.id);
  })
  .catch((err) => {
    console.log('   Error caught:', err.message);
    // Can recover by returning a new promise
    return { id: 0, name: 'Guest' };
  })
  .then((user) => {
    console.log('   Recovered user:', user.name);
  });

// ─────────────────────────────────────────
// Example 3: Finally block
// ─────────────────────────────────────────
console.log('\n3. Finally Block:');

fetchUser(1)
  .then((user) => {
    console.log('   Got user:', user.name);
  })
  .catch((err) => {
    console.log('   Error:', err.message);
  })
  .finally(() => {
    console.log('   Finally: Always runs for cleanup');
  });
```

## Handling Multiple Async Operations

### Parallel vs Sequential

```javascript
// parallel-vs-sequential.js - Comparing approaches

function delay(ms, value) {
  return new Promise((resolve) => setTimeout(() => resolve(value), ms));
}

// Sequential: each waits for the previous
console.log('Sequential:');
console.time('sequential');
delay(100, 1)
  .then((v) => delay(100, v + 1))
  .then((v) => delay(100, v + 1))
  .then((v) => {
    console.log('Result:', v);
    console.timeEnd('sequential');
  });

// Parallel: all start at once
console.log('\nParallel:');
console.time('parallel');
Promise.all([
  delay(100, 1),
  delay(100, 2),
  delay(100, 3)
]).then((results) => {
  console.log('Results:', results);
  console.timeEnd('parallel');
});
```

## Common Mistakes

### Mistake 1: Forgetting to Return

```javascript
// WRONG - breaks the chain
fetchData()
  .then((data) => {
    process(data);  // Forgot return!
  })
  .then((result) => {
    console.log(result);  // undefined
  });

// CORRECT
fetchData()
  .then((data) => {
    return process(data);  // Return!
  })
  .then((result) => {
    console.log(result);  // Now works
  });
```

### Mistake 2: Not Handling Errors

```javascript
// WRONG - unhandled rejection
doSomething()
  .then(success);

// CORRECT
doSomething()
  .then(success)
  .catch(handleError);
```

### Mistake 3: Throwing in .then()

```javascript
// Errors thrown in .then() become rejections
Promise.resolve(5)
  .then((value) => {
    throw new Error('Oops!');  // Same as rejecting!
  })
  .catch((err) => {
    console.log('Caught:', err.message);
  });
```

## Try It Yourself

### Exercise 1: User Posts Comments Chain
Create a chain that fetches a user, then their posts, then comments on a specific post.

### Exercise 2: Recovery from Error
Create a chain that catches an error and recovers with fallback data.

### Exercise 3: Finally Cleanup
Add a .finally() block that runs cleanup code regardless of success or failure.

## Next Steps

Now you understand Promise chaining. Let's learn about Promise combinators - methods for handling multiple Promises. Continue to [Promise Combinators](./03-promise-combinators.md).
