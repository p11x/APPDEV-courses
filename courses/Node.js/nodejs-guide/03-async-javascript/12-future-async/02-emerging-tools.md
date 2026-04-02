# Emerging Async Tools and Frameworks

## What You'll Learn

- Modern async libraries and frameworks
- Effect-TS for structured concurrency
- p-limit, p-map for concurrency control
- Async state management patterns

## Modern Async Libraries

```bash
# p-limit: Concurrency limiting
npm install p-limit

# p-map: Parallel map with concurrency
npm install p-map

# p-retry: Retry with backoff
npm install p-retry

# p-timeout: Promise timeout
npm install p-timeout
```

```javascript
import pLimit from 'p-limit';
import pMap from 'p-map';
import pRetry from 'p-retry';

// p-limit: Control concurrency
const limit = pLimit(5);
const results = await Promise.all(
    urls.map(url => limit(() => fetch(url)))
);

// p-map: Parallel map with concurrency
const users = await pMap(userIds, async (id) => {
    return getUser(id);
}, { concurrency: 10 });

// p-retry: Retry with exponential backoff
const data = await pRetry(
    () => fetchExternalAPI(),
    { retries: 3, factor: 2 }
);
```

## Effect-TS Pattern

```javascript
// Effect-TS: Structured concurrency and error handling
// (Conceptual — actual API is more complex)

const program = Effect.gen(function* () {
    const user = yield* Effect.tryPromise(() => getUser(id));
    const orders = yield* Effect.tryPromise(() => getOrders(user.id));
    return { user, orders };
});

const result = await Effect.runPromise(program);
```

## Best Practices Checklist

- [ ] Use p-limit for simple concurrency control
- [ ] Use p-map for parallel mapping with limits
- [ ] Evaluate Effect-TS for complex async workflows
- [ ] Keep async dependencies minimal
- [ ] Monitor new TC39 proposals for native alternatives

## Cross-References

- See [Upcoming Features](./01-upcoming-features.md) for language features
- See [Concurrency Control](../08-concurrency-control/01-rate-limiting.md) for rate limiting
- See [Ecosystem Integration](../13-ecosystem-integration/01-frameworks.md) for frameworks

## Next Steps

Continue to [Ecosystem Integration](../13-ecosystem-integration/01-frameworks.md) for framework patterns.
