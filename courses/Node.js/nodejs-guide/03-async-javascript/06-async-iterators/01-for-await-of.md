# Async Iterators and for-await-of

## What You'll Learn

- Async iterator protocol
- for-await-of loops
- Creating custom async iterables
- Stream integration with async iterators

## Async Iterator Protocol

```javascript
// An async iterable implements [Symbol.asyncIterator]()
// Each call to next() returns a Promise<{ value, done }>

const asyncRange = {
    from: 1,
    to: 5,

    [Symbol.asyncIterator]() {
        let current = this.from;
        const last = this.to;

        return {
            async next() {
                if (current <= last) {
                    await new Promise(resolve => setTimeout(resolve, 100));
                    return { done: false, value: current++ };
                }
                return { done: true };
            }
        };
    }
};

// Using for-await-of
for await (const num of asyncRange) {
    console.log(num); // 1, 2, 3, 4, 5 (100ms between each)
}
```

## Async Generator Functions

```javascript
// Async generators use async function* syntax
async function* fetchPaginated(url) {
    let page = 1;
    let hasMore = true;

    while (hasMore) {
        const response = await fetch(`${url}?page=${page}`);
        const data = await response.json();

        for (const item of data.items) {
            yield item;
        }

        hasMore = data.hasMore;
        page++;
    }
}

// Usage
for await (const user of fetchPaginated('/api/users')) {
    console.log(user);
}

// Collect all items
const users = [];
for await (const user of fetchPaginated('/api/users')) {
    users.push(user);
}
```

## Stream Integration

```javascript
import { createReadStream } from 'node:fs';
import { createInterface } from 'node:readline';

// Read file line by line with async iterator
async function* readLines(filePath) {
    const stream = createReadStream(filePath, { encoding: 'utf-8' });
    const rl = createInterface({ input: stream, crlfDelay: Infinity });

    for await (const line of rl) {
        yield line;
    }
}

// Usage
for await (const line of readLines('./large-file.txt')) {
    console.log(line);
}

// Node.js streams are async iterable
import { createReadStream } from 'node:fs';

const stream = createReadStream('./data.txt', { encoding: 'utf-8' });
for await (const chunk of stream) {
    console.log(`Chunk: ${chunk.length} bytes`);
}
```

## Async Generator with Error Handling

```javascript
async function* resilientFetch(urls) {
    for (const url of urls) {
        try {
            const response = await fetch(url);
            yield { url, data: await response.json() };
        } catch (err) {
            yield { url, error: err.message };
        }
    }
}

// Usage
for await (const result of resilientFetch(['/api/a', '/api/b', '/api/c'])) {
    if (result.error) {
        console.error(`Failed: ${result.url} — ${result.error}`);
    } else {
        console.log(`OK: ${result.url}`, result.data);
    }
}
```

## Best Practices Checklist

- [ ] Use for-await-of for async iteration
- [ ] Use async generators for paginated data
- [ ] Handle errors within async iterators
- [ ] Be aware of memory when collecting all results
- [ ] Use Node.js streams as async iterables

## Cross-References

- See [Custom Async Iterables](./02-custom-async-iterables.md) for creation patterns
- See [Memory Considerations](./03-async-iterator-memory.md) for memory management
- See [Event Patterns](../05-event-based-patterns/01-observer-pubsub.md) for events

## Next Steps

Continue to [Custom Async Iterables](./02-custom-async-iterables.md) for creation patterns.
