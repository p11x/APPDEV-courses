# Promise Debugging Techniques

## What You'll Learn

- Detecting unhandled rejections
- Tracing promise chains
- Common async error scenarios
- Production debugging strategies

## Unhandled Rejection Detection

```javascript
// Global handler
process.on('unhandledRejection', (reason, promise) => {
    console.error('Unhandled Rejection:', reason);
    console.error('Promise:', promise);
});

// Development: fail fast
if (process.env.NODE_ENV !== 'production') {
    process.on('unhandledRejection', (reason) => {
        console.error('UNHANDLED:', reason);
        process.exit(1);
    });
}
```

## Promise Chain Tracing

```javascript
function trace(label) {
    return (value) => {
        console.log(`[${label}]`, value);
        return value;
    };
}

fetchUser(id)
    .then(trace('user'))
    .then(user => getOrders(user.id))
    .then(trace('orders'))
    .catch(err => console.error('Chain error:', err));
```

## Common Error Scenarios

```javascript
// 1. Missing return in .then()
getUser(id)
    .then(user => { getOrders(user.id); }) // Missing return!
    .then(orders => console.log(orders)); // undefined

// 2. Empty .catch()
fetchData().then(process).catch(() => {}); // Swallows errors

// 3. Not awaiting async operations
async function handler() {
    saveToDatabase(data); // Missing await!
}

// 4. Mixing callbacks and promises
fs.readFile('file.txt', (err, data) => {
    return process(data); // Returns promise from callback — lost!
});
```

## Best Practices Checklist

- [ ] Handle unhandledRejection events
- [ ] Add trace logging during development
- [ ] Always return in .then() handlers
- [ ] Never create empty .catch() blocks
- [ ] Await all async operations

## Cross-References

- See [Async Stack Traces](./01-async-stack-traces.md) for stack traces
- See [Promise Debugging](../03-promises/04-promise-debugging.md) for Promise specifics
- See [Error Handling](../07-async-error-handling/01-error-propagation.md) for error patterns

## Next Steps

Continue to [Async Performance](../11-async-performance/01-memory-cpu-patterns.md) for optimization.
