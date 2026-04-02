# Async Stack Traces and Debugging

## What You'll Learn

- Understanding async stack traces
- Improving async error visibility
- Chrome DevTools for async debugging
- Production async debugging strategies

## Async Stack Traces

```javascript
// Traditional stack trace loses context across async boundaries
async function fetchUser(id) {
    return fetch(`/api/users/${id}`); // Error here
}

async function getUser(id) {
    return fetchUser(id);
}

try {
    await getUser(1);
} catch (err) {
    console.error(err.stack);
    // Only shows: Error at fetchUser
    // Doesn't show: getUser → fetchUser chain
}

// Node.js 12+ preserves async stack traces
// Shows full chain: getUser → fetchUser → Error
```

## Improving Error Context

```javascript
// Add context to async errors
class AsyncError extends Error {
    constructor(message, context = {}) {
        super(message);
        this.context = context;
        this.timestamp = Date.now();
    }
}

async function withContext(fn, context) {
    try {
        return await fn();
    } catch (err) {
        throw new AsyncError(err.message, {
            ...context,
            originalError: err,
            stack: err.stack,
        });
    }
}

// Usage
const user = await withContext(
    () => fetchUser(id),
    { operation: 'fetchUser', userId: id }
);
```

## Chrome DevTools

```bash
# Start with inspector
node --inspect your-app.js

# Open chrome://inspect
# Go to Sources tab
# Set breakpoints in async code
# Use "Async Stack Traces" checkbox
```

## Best Practices Checklist

- [ ] Use Node.js 12+ for async stack traces
- [ ] Add context to errors for debugging
- [ ] Use Chrome DevTools for local debugging
- [ ] Log async operation context in production
- [ ] Use --enable-source-maps for TypeScript

## Cross-References

- See [Promise Debugging](./02-promise-debugging.md) for Promise-specific debugging
- See [Error Handling](../07-async-error-handling/01-error-propagation.md) for error patterns
- See [Performance](../11-async-performance/01-memory-cpu-patterns.md) for profiling

## Next Steps

Continue to [Promise Debugging](./02-promise-debugging.md) for Promise debugging.
