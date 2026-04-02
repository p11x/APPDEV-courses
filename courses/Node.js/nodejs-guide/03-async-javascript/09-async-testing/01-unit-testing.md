# Unit Testing Async Functions

## What You'll Learn

- Testing async functions with node:test
- Testing promise rejections
- Mocking async operations
- Testing async error cases

## Basic Async Testing

```javascript
import { describe, it, mock } from 'node:test';
import assert from 'node:assert/strict';

// Function to test
async function fetchUser(id) {
    const response = await fetch(`/api/users/${id}`);
    if (!response.ok) throw new Error('User not found');
    return response.json();
}

describe('fetchUser', () => {
    it('should return user data', async () => {
        // Mock fetch
        const mockFetch = mock.fn(() => Promise.resolve({
            ok: true,
            json: () => Promise.resolve({ id: 1, name: 'Alice' }),
        }));
        global.fetch = mockFetch;

        const user = await fetchUser(1);
        assert.equal(user.name, 'Alice');
        assert.equal(mockFetch.mock.calls.length, 1);
    });

    it('should throw on not found', async () => {
        global.fetch = mock.fn(() => Promise.resolve({ ok: false }));

        await assert.rejects(
            () => fetchUser(999),
            { message: 'User not found' }
        );
    });
});
```

## Testing Promise Combinators

```javascript
describe('Promise combinators', () => {
    it('Promise.all should resolve all', async () => {
        const results = await Promise.all([
            Promise.resolve(1),
            Promise.resolve(2),
            Promise.resolve(3),
        ]);
        assert.deepEqual(results, [1, 2, 3]);
    });

    it('Promise.all should reject on first failure', async () => {
        await assert.rejects(
            () => Promise.all([
                Promise.resolve(1),
                Promise.reject(new Error('fail')),
                Promise.resolve(3),
            ]),
            { message: 'fail' }
        );
    });

    it('Promise.allSettled should return all results', async () => {
        const results = await Promise.allSettled([
            Promise.resolve(1),
            Promise.reject(new Error('fail')),
        ]);

        assert.equal(results[0].status, 'fulfilled');
        assert.equal(results[0].value, 1);
        assert.equal(results[1].status, 'rejected');
        assert.equal(results[1].reason.message, 'fail');
    });
});
```

## Mocking Async Operations

```javascript
describe('with mocked dependencies', () => {
    it('should call database and process result', async () => {
        const mockDb = {
            query: mock.fn(() => Promise.resolve({
                rows: [{ id: 1, name: 'Alice' }]
            })),
        };

        const users = await getUsers(mockDb);

        assert.equal(users.length, 1);
        assert.equal(mockDb.query.mock.calls.length, 1);
        assert.ok(mockDb.query.mock.calls[0].arguments[0].includes('SELECT'));
    });

    it('should handle database errors', async () => {
        const mockDb = {
            query: mock.fn(() => Promise.reject(new Error('Connection failed'))),
        };

        await assert.rejects(
            () => getUsers(mockDb),
            { message: 'Connection failed' }
        );
    });
});
```

## Best Practices Checklist

- [ ] Always make test functions async
- [ ] Use assert.rejects for error testing
- [ ] Mock external async dependencies
- [ ] Test both success and error cases
- [ ] Clean up mocks in afterEach

## Cross-References

- See [Integration Testing](./02-integration-testing.md) for API testing
- See [Mocking Strategies](./03-mocking-strategies.md) for advanced mocking
- See [Error Handling](../07-async-error-handling/01-error-propagation.md) for errors

## Next Steps

Continue to [Integration Testing](./02-integration-testing.md) for API testing.
