# Node.js Test Runner Basics

## What You'll Learn

- Using node:test
- Writing tests with describe and it
- Running tests

## Built-in Test Runner

Node.js v18+ has a built-in test runner:

```javascript
// test/example.test.js - Basic test

import { test, describe } from 'node:test';
import assert from 'node:assert';

describe('My Tests', () => {
  test('should pass', () => {
    assert.strictEqual(1 + 1, 2);
  });
  
  test('should fail', () => {
    assert.strictEqual(1 + 1, 3);  // Will fail
  });
});
```

## Running Tests

```bash
node --test
node --test filename.test.js
```

## Code Example

```javascript
// math.test.js - Test example

import { test, describe } from 'node:test';
import assert from 'node:assert';

function add(a, b) {
  return a + b;
}

function multiply(a, b) {
  return a * b;
}

describe('Math functions', () => {
  test('add returns correct sum', () => {
    assert.strictEqual(add(2, 3), 5);
  });
  
  test('multiply returns correct product', () => {
    assert.strictEqual(multiply(3, 4), 12);
  });
});
```

Run with:
```bash
node --test math.test.js
```

## Try It Yourself

### Exercise 1: Write Tests
Write tests for a function.

### Exercise 2: Run Tests
Run tests with the node test runner.
