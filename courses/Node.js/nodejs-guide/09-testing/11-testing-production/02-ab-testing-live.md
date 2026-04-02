# A/B Testing and Live Testing in Production

## What You'll Learn

- A/B testing implementation
- Live testing patterns
- Production debugging
- Test rollback strategies

## A/B Testing Implementation

```javascript
import { createHash } from 'node:crypto';

class ABTest {
    constructor(redis) {
        this.redis = redis;
    }

    async assignVariant(testId, userId) {
        const hash = createHash('md5')
            .update(`${testId}:${userId}`)
            .digest('hex');
        const bucket = parseInt(hash.slice(0, 8), 16) % 100;

        const config = await this.getConfig(testId);
        let cumulative = 0;

        for (const variant of config.variants) {
            cumulative += variant.weight;
            if (bucket < cumulative) {
                return variant;
            }
        }

        return config.variants[0];
    }

    async getConfig(testId) {
        const config = await this.redis.get(`ab-test:${testId}`);
        return JSON.parse(config);
    }

    async trackConversion(testId, userId, variantId, event) {
        await this.redis.hIncrBy(
            `ab-test:${testId}:results`,
            `${variantId}:${event}`,
            1
        );
    }

    async getResults(testId) {
        const results = await this.redis.hGetAll(`ab-test:${testId}:results`);
        const summary = {};

        for (const [key, count] of Object.entries(results)) {
            const [variant, event] = key.split(':');
            if (!summary[variant]) summary[variant] = {};
            summary[variant][event] = parseInt(count);
        }

        return summary;
    }

    statisticalSignificance(control, variant) {
        const n1 = control.visitors;
        const n2 = variant.visitors;
        const p1 = control.conversions / n1;
        const p2 = variant.conversions / n2;
        const p = (control.conversions + variant.conversions) / (n1 + n2);
        const se = Math.sqrt(p * (1 - p) * (1/n1 + 1/n2));
        const z = (p2 - p1) / se;
        return Math.abs(z) > 1.96; // 95% confidence
    }
}

// Usage in application
app.get('/checkout', async (req, res) => {
    const abTest = new ABTest(redis);
    const variant = await abTest.assignVariant('checkout-redesign', req.user.id);

    await abTest.trackConversion('checkout-redesign', req.user.id, variant.id, 'page_view');

    if (variant.id === 'control') {
        res.render('checkout-v1');
    } else {
        res.render('checkout-v2');
    }
});
```

## Live Testing with Feature Flags

```javascript
class FeatureFlag {
    constructor(redis) {
        this.redis = redis;
    }

    async isEnabled(flag, context = {}) {
        const config = await this.redis.get(`feature:${flag}`);
        if (!config) return false;

        const { enabled, percentage, users, roles } = JSON.parse(config);

        if (!enabled) return false;
        if (users?.includes(context.userId)) return true;
        if (roles?.includes(context.role)) return true;

        if (percentage !== undefined) {
            const hash = createHash('md5')
                .update(`${context.userId}:${flag}`)
                .digest('hex');
            return parseInt(hash.slice(0, 8), 16) % 100 < percentage;
        }

        return false;
    }
}

// Test with feature flags
describe('New Checkout Flow', () => {
    let flags;

    before(async () => {
        flags = new FeatureFlag(redis);
    });

    test('new checkout enabled for test users', async () => {
        const enabled = await flags.isEnabled('new-checkout', {
            userId: 'test-user-1',
            role: 'beta',
        });

        if (enabled) {
            const result = await checkout.newFlow(cart);
            assert.ok(result.success);
        } else {
            const result = await checkout.oldFlow(cart);
            assert.ok(result.success);
        }
    });
});
```

## Production Debugging

```javascript
// Production-safe test endpoint
app.get('/debug/test-flow', async (req, res) => {
    // Only available in staging or with admin role
    if (process.env.NODE_ENV === 'production' && req.user?.role !== 'admin') {
        return res.status(404).json({ error: 'Not found' });
    }

    const results = {
        database: 'unknown',
        redis: 'unknown',
        auth: 'unknown',
    };

    try {
        await pool.query('SELECT 1');
        results.database = 'ok';
    } catch (err) {
        results.database = err.message;
    }

    try {
        await redis.ping();
        results.redis = 'ok';
    } catch (err) {
        results.redis = err.message;
    }

    try {
        const token = jwt.sign({ test: true }, process.env.JWT_SECRET, { expiresIn: '1s' });
        jwt.verify(token, process.env.JWT_SECRET);
        results.auth = 'ok';
    } catch (err) {
        results.auth = err.message;
    }

    res.json(results);
});
```

## Common Mistakes

- Not measuring statistical significance in A/B tests
- Not testing feature flags in both states
- Not having rollback strategies
- Exposing debug endpoints in production

## Cross-References

- See [Canary Testing](./01-canary-feature-flags.md) for gradual rollouts
- See [Testing Automation](../10-testing-automation/01-ci-cd-integration.md) for CI/CD
- See [Monitoring](../../08-authentication/10-authentication-monitoring/01-monitoring-metrics.md) for observability

## Next Steps

Continue to [Testing Tools: Data Generation](../12-testing-tools/02-data-generation-reporting.md).
