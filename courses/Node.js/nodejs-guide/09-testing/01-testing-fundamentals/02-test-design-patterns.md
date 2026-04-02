# Test Design Patterns and Code Quality

## What You'll Learn

- Test design patterns (Arrange-Act-Assert, Given-When-Then)
- Test naming conventions and organization
- Test code quality standards
- Test documentation and standards
- Testing anti-patterns to avoid

## AAA Pattern (Arrange-Act-Assert)

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';

describe('UserService', () => {
    describe('createUser', () => {
        test('should create user with valid data', async () => {
            // ARRANGE: Set up test data and dependencies
            const mockRepo = {
                findByEmail: async () => null,
                create: async (data) => ({ id: 1, ...data }),
            };
            const service = new UserService(mockRepo);
            const userData = {
                name: 'Alice',
                email: 'alice@example.com',
            };

            // ACT: Execute the code under test
            const result = await service.createUser(userData);

            // ASSERT: Verify the expected outcome
            assert.equal(result.name, 'Alice');
            assert.equal(result.email, 'alice@example.com');
            assert.ok(result.id);
        });

        test('should throw when email already exists', async () => {
            // ARRANGE
            const mockRepo = {
                findByEmail: async () => ({ id: 1, email: 'taken@example.com' }),
            };
            const service = new UserService(mockRepo);

            // ACT & ASSERT
            await assert.rejects(
                () => service.createUser({ email: 'taken@example.com' }),
                { message: 'Email already exists' }
            );
        });
    });
});
```

## Given-When-Then (BDD Style)

```javascript
describe('Shopping Cart', () => {
    describe('Given an empty cart', () => {
        let cart;

        beforeEach(() => {
            cart = new ShoppingCart();
        });

        test('When item is added, Then cart has one item', () => {
            cart.addItem({ id: 1, name: 'Widget', price: 9.99 });

            assert.equal(cart.itemCount, 1);
            assert.equal(cart.total, 9.99);
        });

        test('When checkout is attempted, Then error is thrown', () => {
            assert.throws(
                () => cart.checkout(),
                { message: 'Cart is empty' }
            );
        });
    });

    describe('Given a cart with items', () => {
        let cart;

        beforeEach(() => {
            cart = new ShoppingCart();
            cart.addItem({ id: 1, name: 'Widget', price: 9.99 });
            cart.addItem({ id: 2, name: 'Gadget', price: 19.99 });
        });

        test('When item is removed, Then total updates', () => {
            cart.removeItem(1);

            assert.equal(cart.itemCount, 1);
            assert.equal(cart.total, 19.99);
        });

        test('When discount is applied, Then total reflects discount', () => {
            cart.applyDiscount(10); // 10% off

            assert.equal(cart.total, 26.99); // (9.99 + 19.99) * 0.9
        });
    });
});
```

## Test Naming Conventions

```javascript
// GOOD: Descriptive, follows pattern
describe('OrderService', () => {
    describe('createOrder', () => {
        test('should create order when inventory is available', () => {});
        test('should throw InsufficientInventory when stock is low', () => {});
        test('should deduct inventory after successful order', () => {});
    });

    describe('cancelOrder', () => {
        test('should restore inventory when order is cancelled', () => {});
        test('should throw when order is already shipped', () => {});
        test('should process refund for paid orders', () => {});
    });
});

// BAD: Vague names
describe('OrderService', () => {
    test('works', () => {});
    test('test1', () => {});
    test('should handle stuff', () => {});
});
```

## Testing Anti-Patterns

```javascript
// ANTI-PATTERN 1: Testing implementation details
// BAD: Testing internal method calls
test('should call internal method', () => {
    const spy = jest.spyOn(service, '_internalMethod');
    service.doSomething();
    expect(spy).toHaveBeenCalled(); // Fragile — breaks on refactor
});

// GOOD: Test behavior, not implementation
test('should produce correct result', () => {
    const result = service.doSomething();
    assert.equal(result.status, 'success');
});

// ANTI-PATTERN 2: Too many assertions
// BAD: One test checks everything
test('user operations', async () => {
    const user = await create({ name: 'A' });
    assert.equal(user.name, 'A');
    const updated = await update(user.id, { name: 'B' });
    assert.equal(updated.name, 'B');
    const deleted = await delete(user.id);
    assert.ok(deleted);
});

// GOOD: Separate tests for each operation
test('should create user', async () => { /* ... */ });
test('should update user', async () => { /* ... */ });
test('should delete user', async () => { /* ... */ });

// ANTI-PATTERN 3: Test interdependence
// BAD: Test B depends on Test A's result
let createdUserId;
test('A: create user', async () => {
    createdUserId = (await create({ name: 'A' })).id;
});
test('B: update user', async () => {
    await update(createdUserId, { name: 'B' }); // Fails if A fails
});

// GOOD: Each test is independent
test('should update user', async () => {
    const user = await create({ name: 'A' }); // Setup in test
    const updated = await update(user.id, { name: 'B' });
    assert.equal(updated.name, 'B');
});
```

## Test Organization Structure

```
test/
├── unit/
│   ├── services/
│   │   ├── user.service.test.js
│   │   └── order.service.test.js
│   ├── utils/
│   │   └── validation.test.js
│   └── middleware/
│       └── auth.test.js
├── integration/
│   ├── api/
│   │   ├── users.api.test.js
│   │   └── orders.api.test.js
│   └── database/
│       └── user.repository.test.js
├── e2e/
│   ├── user-journey.test.js
│   └── checkout-flow.test.js
├── fixtures/
│   ├── users.json
│   └── orders.json
├── helpers/
│   ├── test-db.js
│   ├── test-app.js
│   └── factories.js
└── setup.js
```

## Common Mistakes

- Testing implementation instead of behavior
- Interdependent tests that fail together
- Too many assertions in one test
- Vague test names that don't describe intent

## Cross-References

- See [Testing Pyramid](./01-testing-pyramid-architecture.md) for architecture
- See [Unit Testing](../03-unit-testing/01-functions-classes.md) for patterns
- See [CI/CD](../10-testing-automation/01-ci-cd-integration.md) for automation

## Next Steps

Continue to [Testing Frameworks: Mocha and Sinon](../02-testing-frameworks/02-mocha-sinon.md).
