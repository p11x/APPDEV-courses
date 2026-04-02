# Async Iterator Memory Considerations

## What You'll Learn

- Memory-efficient async iteration
- Chunking and batching strategies
- Backpressure with async iterators
- Cleanup and resource management

## Memory-Efficient Patterns

```javascript
// BAD: Load all into memory
const allUsers = [];
for await (const user of fetchAllUsers()) {
    allUsers.push(user); // Loads everything into memory
}

// GOOD: Process one at a time
for await (const user of fetchAllUsers()) {
    await processUser(user); // Memory: O(1)
}

// GOOD: Process in batches
const batch = [];
for await (const user of fetchAllUsers()) {
    batch.push(user);
    if (batch.length >= 100) {
        await processBatch(batch);
        batch.length = 0; // Clear batch
    }
}
if (batch.length > 0) await processBatch(batch);
```

## Early Termination and Cleanup

```javascript
async function* withCleanup(generator, cleanup) {
    try {
        yield* generator;
    } finally {
        await cleanup();
    }
}

// Usage
async function* dbCursor(pool, query) {
    const client = await pool.connect();
    try {
        const result = await client.query(new Cursor(query));
        for (const row of result.rows) {
            yield row;
        }
    } finally {
        client.release(); // Always release connection
    }
}

// Early break triggers cleanup
for await (const row of dbCursor(pool, 'SELECT * FROM large_table')) {
    if (row.id === 100) break; // Connection is released
}
```

## Best Practices Checklist

- [ ] Process items one at a time or in small batches
- [ ] Implement cleanup in finally blocks
- [ ] Use early termination when appropriate
- [ ] Monitor memory usage during iteration
- [ ] Handle backpressure for stream-based iterators

## Cross-References

- See [for-await-of](./01-for-await-of.md) for iteration basics
- See [Custom Iterables](./02-custom-async-iterables.md) for creation
- See [Streams](../../02-core-concepts/05-stream-architecture/01-readable-writable-streams.md) for streaming

## Next Steps

Continue to [Async Error Handling](../07-async-error-handling/01-error-propagation.md) for error patterns.
