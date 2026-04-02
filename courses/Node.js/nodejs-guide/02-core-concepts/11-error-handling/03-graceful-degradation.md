# Graceful Degradation Patterns

## What You'll Learn

- Feature flags for degradation
- Partial response patterns
- Timeout-based degradation
- Monitoring degraded states

## Feature Flags

```javascript
class FeatureFlags {
    constructor(flags = {}) {
        this.flags = flags;
    }

    isEnabled(feature) {
        return this.flags[feature]?.enabled ?? false;
    }

    getValue(feature, defaultValue) {
        return this.flags[feature]?.value ?? defaultValue;
    }
}

const flags = new FeatureFlags({
    'new-search': { enabled: true, value: 'v2' },
    'ai-recommendations': { enabled: false },
});

// Use in code
app.get('/search', async (req, res) => {
    if (flags.isEnabled('new-search')) {
        return searchV2(req.query);
    }
    return searchV1(req.query);
});
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

## Partial Response

```javascript
app.get('/dashboard', async (req, res) => {
    const results = await Promise.allSettled([
        getUserData(req.userId),
        getRecentOrders(req.userId),
        getRecommendations(req.userId),
        getNotifications(req.userId),
    ]);

    res.json({
        user: results[0].status === 'fulfilled' ? results[0].value : null,
        orders: results[1].status === 'fulfilled' ? results[1].value : [],
        recommendations: results[2].status === 'fulfilled' ? results[2].value : [],
        notifications: results[3].status === 'fulfilled' ? results[3].value : [],
        _degraded: results.some(r => r.status === 'rejected'),
    });
});
```

## Best Practices Checklist

- [ ] Use feature flags to disable features gracefully
- [ ] Set timeouts for all external calls
- [ ] Return partial responses when possible
- [ ] Indicate degraded state in responses
- [ ] Monitor degradation frequency

## Cross-References

- See [Error Propagation](./01-error-propagation.md) for error classes
- See [Recovery Strategies](./02-recovery-strategies.md) for retry patterns
- See [Caching](../13-caching-strategies/01-in-memory-caching.md) for fallback cache

## Next Steps

Continue to [Performance Optimization](../12-performance-optimization/01-cpu-memory-optimization.md) for optimization.
