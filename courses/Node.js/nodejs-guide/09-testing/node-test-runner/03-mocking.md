# Mocking in Tests

## What You'll Learn

- Creating mocks
- Mocking functions
- Restoring mocks

## Using Mock Functions

```javascript
// mock.js - Creating mocks

import { mock } from 'node:test';

// Create mock function
const myMock = mock.fn((x) => x * 2);

console.log(myMock(5)); // 10
console.log(myMock.mock.calls); // Call history
```

## Mocking Methods

```javascript
// mock-method.js - Mocking object methods

const obj = {
  greet(name) {
    return `Hello, ${name}`;
  }
};

mock.method(obj, 'greet', (name) => `Hi, ${name}`);

console.log(obj.greet('World')); // Hi, World

// Restore original
obj.greet.restore();
```

## Code Example

```javascript
// mock-demo.js - Mocking demonstration

import { mock, test } from 'node:test';
import assert from 'node:assert';

test('mock function', () => {
  const fn = mock.fn((x) => x + 1);
  
  assert.strictEqual(fn(1), 2);
  assert.strictEqual(fn(2), 3);
  
  // Check calls
  assert.strictEqual(fn.mock.calls.length, 2);
});

test('mock method', () => {
  const obj = { getValue: () => 42 };
  
  mock.method(obj, 'getValue', () => 100);
  
  assert.strictEqual(obj.getValue(), 100);
  
  obj.getValue.restore();
  
  assert.strictEqual(obj.getValue(), 42);
});
```

## Try It Yourself

### Exercise 1: Create Mocks
Create mock functions in tests.

### Exercise 2: Mock Methods
Mock methods on objects.
