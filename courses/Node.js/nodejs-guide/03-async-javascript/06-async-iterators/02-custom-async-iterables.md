# Custom Async Iterable Creation

## What You'll Learn

- Creating async iterables from scratch
- Paginated API as async iterable
- Database cursor as async iterable
- WebSocket message stream

## Paginated API Iterable

```javascript
async function* paginatedAPI(baseUrl, options = {}) {
    const { pageSize = 20, maxPages = Infinity } = options;
    let page = 1;
    let totalFetched = 0;

    while (page <= maxPages) {
        const url = `${baseUrl}?page=${page}&limit=${pageSize}`;
        const response = await fetch(url);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }

        const { data, total } = await response.json();

        for (const item of data) {
            yield item;
            totalFetched++;

            if (totalFetched >= total) return;
        }

        page++;
    }
}

// Usage
for await (const user of paginatedAPI('/api/users', { pageSize: 50 })) {
    console.log(user.name);
}
```

## Database Cursor Iterable

```javascript
async function* dbCursor(pool, query, params = [], batchSize = 100) {
    let offset = 0;
    let hasMore = true;

    while (hasMore) {
        const { rows } = await pool.query(
            `${query} LIMIT $${params.length + 1} OFFSET $${params.length + 2}`,
            [...params, batchSize, offset]
        );

        if (rows.length === 0) {
            hasMore = false;
            break;
        }

        for (const row of rows) {
            yield row;
        }

        offset += rows.length;
        hasMore = rows.length === batchSize;
    }
}

// Usage
for await (const user of dbCursor(pool, 'SELECT * FROM users WHERE active = true', [])) {
    await processUser(user);
}
```

## Rate-Limited Iterable

```javascript
async function* rateLimited(items, fn, { concurrency = 5, delayMs = 100 } = {}) {
    const queue = [...items];
    let running = 0;
    const results = [];

    while (queue.length > 0 || running > 0) {
        while (running < concurrency && queue.length > 0) {
            const item = queue.shift();
            running++;

            fn(item)
                .then(result => results.push({ item, result }))
                .catch(error => results.push({ item, error }))
                .finally(() => running--);

            if (delayMs > 0) {
                await new Promise(r => setTimeout(r, delayMs));
            }
        }

        // Yield completed results
        while (results.length > 0) {
            yield results.shift();
        }

        if (running > 0) {
            await new Promise(r => setTimeout(r, 10));
        }
    }
}

// Usage
for await (const { item, result, error } of rateLimited(
    userIds,
    id => getUser(id),
    { concurrency: 3, delayMs: 50 }
)) {
    if (error) console.error(`Failed: ${item}`, error);
    else console.log(`Got: ${result.name}`);
}
```

## Best Practices Checklist

- [ ] Handle errors within async generators
- [ ] Implement proper cleanup on early break
- [ ] Use batching for database/API operations
- [ ] Consider rate limiting for external APIs
- [ ] Test async iterables with early termination

## Cross-References

- See [for-await-of](./01-for-await-of.md) for iteration basics
- See [Memory Considerations](./03-async-iterator-memory.md) for memory
- See [Concurrency Control](../08-concurrency-control/01-rate-limiting.md) for rate limiting

## Next Steps

Continue to [Async Iterator Memory](./03-async-iterator-memory.md) for memory management.
