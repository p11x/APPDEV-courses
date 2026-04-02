# Graceful Degradation in Async Applications

## What You'll Learn

- Partial response patterns
- Timeout-based degradation
- Feature flags for async operations
- Production monitoring strategies

## Partial Response with Promise.allSettled

```javascript
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
        _degraded: results.some(r => r.status === 'rejected'),
        _errors: results
            .filter(r => r.status === 'rejected')
            .map(r => r.reason.message),
    };
}
```

## Timeout-Based Degradation

```javascript
function withTimeout(promise, ms, fallback) {
    return Promise.race([
        promise,
        new Promise(resolve => setTimeout(() => resolve(fallback), ms)),
    ]);
}

// Usage
const recommendations = await withTimeout(
    aiService.getRecommendations(user),
    500, // 500ms timeout
    defaultRecommendations, // Fallback
);
```

## Best Practices Checklist

- [ ] Use Promise.allSettled for partial responses
- [ ] Set timeouts for all external calls
- [ ] Return fallback values on timeout
- [ ] Indicate degraded state in responses
- [ ] Monitor degradation frequency in production

## Cross-References

- See [Error Propagation](./01-error-propagation.md) for error classes
- See [Error Boundaries](./02-error-boundaries.md) for retry patterns
- See [Concurrency Control](../08-concurrency-control/01-rate-limiting.md) for rate limiting

## Next Steps

Continue to [Concurrency Control](../08-concurrency-control/01-rate-limiting.md) for rate limiting.
