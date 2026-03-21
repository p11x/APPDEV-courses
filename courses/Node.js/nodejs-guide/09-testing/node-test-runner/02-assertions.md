# Using Assertions

## What You'll Learn

- node:assert module
- Common assertions

## Importing Assertions

```javascript
import assert from 'node:assert';
```

## Common Assertions

```javascript
// assertions.js - Common assertions

import assert from 'node:assert';

// Strict equality
assert.strictEqual(1, 1);

// Deep equality
assert.deepStrictEqual({ a: 1 }, { a: 1 });

// Throws
assert.throws(() => {
  throw new Error('Error!');
});

// Fail
assert.fail('This failed');
```

## Code Example

```javascript
// assert-demo.js - Assertion examples

import assert from 'node:assert';

describe('Assertions', () => {
  test('strictEqual', () => {
    assert.strictEqual(1, 1);
  });
  
  test('deepStrictEqual', () => {
    assert.deepStrictEqual({ a: 1 }, { a: 1 });
  });
  
  test('notStrictEqual', () => {
    assert.notStrictEqual(1, 2);
  });
  
  test('throws', () => {
    assert.throws(() => {
      throw new Error('Error');
    });
  });
});
```

## Try It Yourself

### Exercise 1: Use Assertions
Write tests using different assertions.

### Exercise 2: Test Throws
Test that functions throw errors correctly.
