# Async Function Composition and Patterns

## What You'll Learn

- Composing async functions
- Error boundary patterns
- Async function testing
- Common async/await pitfalls

## Async Function Composition

```javascript
// Compose async operations
function pipe(...fns) {
    return (input) => fns.reduce(
        (promise, fn) => promise.then(fn),
        Promise.resolve(input)
    );
}

// Usage
const processUser = pipe(
    fetchUser,
    validateUser,
    enrichUser,
    saveUser,
);

const result = await processUser(userId);

// Async compose (right to left)
function compose(...fns) {
    return (input) => fns.reduceRight(
        (promise, fn) => promise.then(fn),
        Promise.resolve(input)
    );
}
```

## Error Boundaries

```javascript
// Wrap async operations with error boundaries
class AsyncBoundary {
    constructor(onError, fallback) {
        this.onError = onError;
        this.fallback = fallback;
    }

    async run(fn) {
        try {
            return await fn();
        } catch (err) {
            this.onError(err);
            if (this.fallback !== undefined) {
                return this.fallback;
            }
            throw err;
        }
    }
}

// Usage
const boundary = new AsyncBoundary(
    (err) => logger.error('Operation failed:', err),
    null, // fallback value
);

const data = await boundary.run(() => fetchExternalAPI());
```

## Promise.allSettled for Partial Success

```javascript
// Handle mixed success/failure gracefully
async function fetchDashboard(userId) {
    const results = await Promise.allSettled([
        getUser(userId),
        getOrders(userId),
        getRecommendations(userId),
        getNotifications(userId),
    ]);

    return {
        user: results[0].status === 'fulfilled' ? results[0].value : null,
        orders: results[1].status === 'fulfilled' ? results[1].value : [],
        recommendations: results[2].status === 'fulfilled' ? results[2].value : [],
        notifications: results[3].status === 'fulfilled' ? results[3].value : [],
        _errors: results
            .filter(r => r.status === 'rejected')
            .map(r => r.reason.message),
    };
}
```

## Common Pitfalls

```javascript
// Pitfall 1: forEach with async/await
// BAD: Doesn't wait for async callbacks
items.forEach(async (item) => {
    await processItem(item); // Fires and forgets!
});

// GOOD: Use for...of
for (const item of items) {
    await processItem(item); // Waits for each
}

// GOOD: Parallel with Promise.all
await Promise.all(items.map(item => processItem(item)));

// Pitfall 2: Missing await
// BAD: Promise is not awaited
function handler(req, res) {
    saveToDatabase(req.body); // Missing await!
    res.json({ success: true });
}

// GOOD: Await the async operation
async function handler(req, res) {
    await saveToDatabase(req.body);
    res.json({ success: true });
}

// Pitfall 3: Async in synchronous callbacks
// BAD: Map returns array of promises, not values
const users = userIds.map(async id => await getUser(id));
console.log(users); // [Promise, Promise, Promise]

// GOOD: Await all promises
const users = await Promise.all(userIds.map(id => getUser(id)));
```

## Testing Async Functions

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';

describe('Async functions', () => {
    it('should resolve with data', async () => {
        const result = await fetchData(1);
        assert.ok(result);
    });

    it('should reject on error', async () => {
        await assert.rejects(
            () => fetchData(-1),
            { message: 'Invalid ID' }
        );
    });

    it('should handle timeout', async () => {
        const result = await Promise.race([
            slowOperation(),
            new Promise((_, reject) =>
                setTimeout(() => reject(new Error('Timeout')), 1000)
            ),
        ]);
        assert.ok(result);
    });
});
```

## Best Practices Checklist

- [ ] Use for...of instead of forEach with async
- [ ] Always await async operations
- [ ] Use Promise.allSettled for partial success
- [ ] Wrap critical async operations in error boundaries
- [ ] Test both success and error cases for async functions
- [ ] Use try/catch for error handling in async functions

## Cross-References

- See [Async Functions](./01-async-functions.md) for async basics
- See [Error Handling](./02-error-handling.md) for try/catch patterns
- See [Top-Level Await](./03-top-level-await.md) for module-level async
- See [Async Error Handling](../07-async-error-handling/01-error-propagation.md) for error patterns

## Next Steps

Continue to [Event-Based Patterns](../05-event-based-patterns/01-observer-pubsub.md) for event-driven async.
