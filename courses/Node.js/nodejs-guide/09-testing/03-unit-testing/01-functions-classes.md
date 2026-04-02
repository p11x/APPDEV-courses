# Unit Testing Functions, Classes, and Async Code

## What You'll Learn

- Unit testing pure functions
- Unit testing classes and methods
- Unit testing async functions
- Unit testing error handling
- Mocking dependencies

## Testing Pure Functions

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';

// Source: src/utils/math.js
export function calculateTotal(items, taxRate = 0) {
    const subtotal = items.reduce((sum, item) => sum + item.price * item.quantity, 0);
    const tax = subtotal * taxRate;
    return Math.round((subtotal + tax) * 100) / 100;
}

export function formatCurrency(amount, currency = 'USD') {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency,
    }).format(amount);
}

// Tests: test/utils/math.test.js
describe('calculateTotal', () => {
    test('calculates subtotal without tax', () => {
        const items = [
            { price: 10, quantity: 2 },
            { price: 5, quantity: 3 },
        ];
        assert.strictEqual(calculateTotal(items), 35);
    });

    test('calculates total with tax', () => {
        const items = [{ price: 100, quantity: 1 }];
        assert.strictEqual(calculateTotal(items, 0.1), 110);
    });

    test('returns 0 for empty items', () => {
        assert.strictEqual(calculateTotal([]), 0);
    });

    test('handles zero price items', () => {
        const items = [{ price: 0, quantity: 5 }];
        assert.strictEqual(calculateTotal(items), 0);
    });
});

describe('formatCurrency', () => {
    test('formats USD currency', () => {
        assert.strictEqual(formatCurrency(1234.56), '$1,234.56');
    });

    test('formats EUR currency', () => {
        const result = formatCurrency(1234.56, 'EUR');
        assert.ok(result.includes('1,234.56'));
    });

    test('handles zero', () => {
        assert.strictEqual(formatCurrency(0), '$0.00');
    });
});
```

## Testing Classes

```javascript
// Source: src/services/user.service.js
export class UserService {
    constructor(repository, emailService) {
        this.repository = repository;
        this.emailService = emailService;
    }

    async createUser(data) {
        const existing = await this.repository.findByEmail(data.email);
        if (existing) throw new Error('Email already exists');

        const user = await this.repository.create(data);
        await this.emailService.sendWelcome(user.email);
        return user;
    }

    async updateUser(id, data) {
        const user = await this.repository.findById(id);
        if (!user) throw new Error('User not found');
        return this.repository.update(id, data);
    }
}

// Tests
describe('UserService', () => {
    let service;
    let mockRepo;
    let mockEmail;

    beforeEach(() => {
        mockRepo = {
            findByEmail: async () => null,
            create: async (data) => ({ id: 1, ...data }),
            findById: async (id) => ({ id, name: 'Test' }),
            update: async (id, data) => ({ id, ...data }),
        };
        mockEmail = {
            sendWelcome: async () => true,
        };
        service = new UserService(mockRepo, mockEmail);
    });

    describe('createUser', () => {
        test('creates user when email not taken', async () => {
            const user = await service.createUser({
                name: 'Alice',
                email: 'alice@test.com',
            });

            assert.equal(user.name, 'Alice');
            assert.ok(mockEmail.sendWelcome.called !== false);
        });

        test('throws when email exists', async () => {
            mockRepo.findByEmail = async () => ({ id: 1 });

            await assert.rejects(
                () => service.createUser({ email: 'taken@test.com' }),
                { message: 'Email already exists' }
            );
        });
    });

    describe('updateUser', () => {
        test('updates existing user', async () => {
            const result = await service.updateUser(1, { name: 'Updated' });
            assert.equal(result.name, 'Updated');
        });

        test('throws when user not found', async () => {
            mockRepo.findById = async () => null;

            await assert.rejects(
                () => service.updateUser(999, { name: 'X' }),
                { message: 'User not found' }
            );
        });
    });
});
```

## Testing Async Code

```javascript
describe('Async Operations', () => {
    // Testing async/await
    test('resolves with data', async () => {
        const result = await fetchData(1);
        assert.equal(result.id, 1);
    });

    // Testing rejections
    test('rejects with error', async () => {
        await assert.rejects(
            () => fetchData(-1),
            { message: 'Invalid ID' }
        );
    });

    // Testing with timeout
    test('completes within timeout', async () => {
        const result = await Promise.race([
            slowOperation(),
            new Promise((_, reject) =>
                setTimeout(() => reject(new Error('Timeout')), 5000)
            ),
        ]);
        assert.ok(result);
    });

    // Testing concurrent operations
    test('handles concurrent requests', async () => {
        const promises = [fetchData(1), fetchData(2), fetchData(3)];
        const results = await Promise.all(promises);

        assert.equal(results.length, 3);
        results.forEach(r => assert.ok(r.id));
    });

    // Testing settled promises
    test('handles mixed results', async () => {
        const results = await Promise.allSettled([
            fetchData(1),
            fetchData(-1),
            fetchData(2),
        ]);

        assert.equal(results[0].status, 'fulfilled');
        assert.equal(results[1].status, 'rejected');
        assert.equal(results[2].status, 'fulfilled');
    });
});
```

## Testing Error Handling

```javascript
describe('Error Handling', () => {
    test('handles synchronous errors', () => {
        assert.throws(
            () => { throw new Error('Sync error'); },
            { message: 'Sync error' }
        );
    });

    test('handles async errors', async () => {
        await assert.rejects(
            async () => { throw new Error('Async error'); },
            { message: 'Async error' }
        );
    });

    test('handles custom error types', async () => {
        class ValidationError extends Error {
            constructor(field, message) {
                super(message);
                this.field = field;
                this.name = 'ValidationError';
            }
        }

        await assert.rejects(
            () => validateUser({ email: '' }),
            (err) => {
                assert.equal(err.name, 'ValidationError');
                assert.equal(err.field, 'email');
                return true;
            }
        );
    });

    test('handles specific error codes', async () => {
        await assert.rejects(
            () => fetchFromApi('/invalid'),
            (err) => {
                assert.equal(err.code, 'NOT_FOUND');
                assert.equal(err.status, 404);
                return true;
            }
        );
    });
});
```

## Best Practices Checklist

- [ ] Test one concept per test
- [ ] Use descriptive test names
- [ ] Mock external dependencies
- [ ] Test both success and failure paths
- [ ] Test edge cases (empty, null, boundary)
- [ ] Keep tests fast (< 100ms for unit tests)
- [ ] Use beforeEach for common setup

## Cross-References

- See [Mocking](../node-test-runner/03-mocking.md) for mock patterns
- See [Async Testing](../08-async-testing/01-promises-async.md) for async patterns
- See [Error Testing](./02-error-edge-cases.md) for edge cases

## Next Steps

Continue to [Integration Testing](../04-integration-testing/01-rest-api.md).
