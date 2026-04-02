# Promise Debugging and Troubleshooting

## What You'll Learn

- Debugging unhandled promise rejections
- Tracing promise chains
- Common promise pitfalls and solutions
- Production promise monitoring

## Unhandled Rejection Detection

```javascript
// Global handler for unhandled rejections
process.on('unhandledRejection', (reason, promise) => {
    console.error('Unhandled Rejection at:', promise);
    console.error('Reason:', reason);
    // In production: send to error tracking service
});

// Detect unhandled rejections in development
if (process.env.NODE_ENV === 'development') {
    process.on('unhandledRejection', (reason) => {
        console.error('UNHANDLED REJECTION:', reason);
        process.exit(1); // Fail fast in development
    });
}
```

## Common Pitfalls

### Pitfall 1: Forgetting to Return

```javascript
// BUG: Not returning in .then()
getUser(id)
    .then(user => {
        getOrders(user.id); // Missing return!
    })
    .then(orders => {
        console.log(orders); // undefined!
    });

// FIX: Always return in .then()
getUser(id)
    .then(user => {
        return getOrders(user.id); // Return the promise
    })
    .then(orders => {
        console.log(orders); // Works!
    });
```

### Pitfall 2: Swallowing Errors

```javascript
// BUG: Empty .catch() swallows errors
fetchData()
    .then(processData)
    .catch(() => {}); // Error silently lost!

// FIX: Always handle or re-throw errors
fetchData()
    .then(processData)
    .catch(err => {
        console.error('Fetch failed:', err.message);
        throw err; // Re-throw if needed
    });
```

### Pitfall 3: Creating Promises Unnecessarily

```javascript
// BAD: Wrapping already-promise value
async function getUser(id) {
    return new Promise((resolve, reject) => {
        db.query('SELECT * FROM users WHERE id = ?', [id])
            .then(resolve)
            .catch(reject);
    });
}

// GOOD: Just use the existing promise
async function getUser(id) {
    return db.query('SELECT * FROM users WHERE id = ?', [id]);
}
```

### Pitfall 4: Not Using Promise.all

```javascript
// BAD: Sequential when parallel is possible
const user = await getUser(id);
const orders = await getOrders(id);
const recommendations = await getRecommendations(id);
// Total: T_user + T_orders + T_recommendations

// GOOD: Parallel execution
const [user, orders, recommendations] = await Promise.all([
    getUser(id),
    getOrders(id),
    getRecommendations(id),
]);
// Total: max(T_user, T_orders, T_recommendations)
```

## Promise Chain Tracing

```javascript
// Debug helper: trace promise chain
function trace(label) {
    return (value) => {
        console.log(`[${label}]`, value);
        return value;
    };
}

function traceError(label) {
    return (err) => {
        console.error(`[${label} ERROR]`, err.message);
        throw err;
    };
}

// Usage
fetchUser(id)
    .then(trace('user'))
    .then(user => getOrders(user.id))
    .then(trace('orders'))
    .catch(traceError('fetchUser'));
```

## Production Monitoring

```javascript
class PromiseMonitor {
    constructor() {
        this.pending = new Set();
        this.completed = 0;
        this.failed = 0;
    }

    wrap(promise, label = 'unknown') {
        this.pending.add(label);

        return promise
            .then(value => {
                this.pending.delete(label);
                this.completed++;
                return value;
            })
            .catch(err => {
                this.pending.delete(label);
                this.failed++;
                throw err;
            });
    }

    getStats() {
        return {
            pending: this.pending.size,
            completed: this.completed,
            failed: this.failed,
            pendingLabels: [...this.pending],
        };
    }
}

const monitor = new PromiseMonitor();

// Wrap promises
const data = await monitor.wrap(fetchData(), 'fetchData');
console.log(monitor.getStats());
// { pending: 0, completed: 1, failed: 0, pendingLabels: [] }
```

## Best Practices Checklist

- [ ] Always return in .then() handlers
- [ ] Never create empty .catch() blocks
- [ ] Use Promise.all for parallel operations
- [ ] Add trace logging during debugging
- [ ] Monitor pending promises in production
- [ ] Handle unhandledRejection events

## Cross-References

- See [Promise Basics](./01-promise-basics.md) for Promise fundamentals
- See [Promise Chaining](./02-promise-chaining.md) for chaining patterns
- See [Promise Combinators](./03-promise-combinators.md) for combinators
- See [Async Debugging](../10-async-debugging/01-async-stack-traces.md) for debugging

## Next Steps

Continue to [Promise Performance](./05-promise-performance.md) for optimization.
