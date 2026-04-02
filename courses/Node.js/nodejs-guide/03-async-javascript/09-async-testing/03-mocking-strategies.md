# Mocking Async Operations

## What You'll Learn

- Mocking async functions
- Mock timers for async testing
- Mocking HTTP requests
- Mock database operations

## Mock Functions

```javascript
import { mock } from 'node:test';

// Create mock function
const mockFn = mock.fn();
mockFn(1, 2);
assert.equal(mockFn.mock.calls.length, 1);
assert.deepEqual(mockFn.mock.calls[0].arguments, [1, 2]);

// Mock with return value
const mockDb = {
    query: mock.fn(() => Promise.resolve({ rows: [{ id: 1 }] })),
};

// Mock that throws
const mockFail = {
    query: mock.fn(() => Promise.reject(new Error('DB error'))),
};
```

## Mock Timers

```javascript
import { mock } from 'node:test';

describe('with mocked timers', () => {
    it('should timeout after delay', async () => {
        mock.timers.enable({ apis: ['setTimeout'] });

        let timedOut = false;
        const promise = new Promise((_, reject) => {
            setTimeout(() => {
                timedOut = true;
                reject(new Error('Timeout'));
            }, 5000);
        });

        // Fast-forward time
        mock.timers.tick(5000);

        await assert.rejects(() => promise, { message: 'Timeout' });
        assert.ok(timedOut);

        mock.timers.reset();
    });
});
```

## Mock Modules

```javascript
import { mock } from 'node:test';

// Mock entire module
mock.module('node:fs/promises', {
    namedExports: {
        readFile: mock.fn(() => Promise.resolve('mocked content')),
        writeFile: mock.fn(() => Promise.resolve()),
    },
});

// Tests use mocked module
import { readFile } from 'node:fs/promises';
const content = await readFile('any-file.txt');
assert.equal(content, 'mocked content');
```

## Best Practices Checklist

- [ ] Use mock.fn() for function mocking
- [ ] Use mock.timers for time-dependent tests
- [ ] Use mock.module() for module mocking
- [ ] Reset mocks in afterEach hooks
- [ ] Verify mock call arguments

## Cross-References

- See [Unit Testing](./01-unit-testing.md) for unit tests
- See [Integration Testing](./02-integration-testing.md) for API tests
- See [Async Debugging](../10-async-debugging/01-async-stack-traces.md) for debugging

## Next Steps

Continue to [Async Debugging](../10-async-debugging/01-async-stack-traces.md) for debugging.
