# Testing in Production: Canary, Feature Flags, and A/B Testing

## What You'll Learn

- Canary deployments and gradual rollouts
- Feature flag testing patterns
- A/B testing implementation
- Shadow testing and blue-green deployments
- Production monitoring and validation

## Feature Flag Testing

```javascript
class FeatureFlags {
    constructor(redis) {
        this.redis = redis;
    }

    async isEnabled(flag, context = {}) {
        const config = await this.redis.get(`feature:${flag}`);
        if (!config) return false;

        const { enabled, percentage, users, roles } = JSON.parse(config);

        if (!enabled) return false;

        // Check user-specific flags
        if (users?.includes(context.userId)) return true;

        // Check role-based flags
        if (roles?.includes(context.role)) return true;

        // Check percentage rollout
        if (percentage !== undefined) {
            const hash = createHash('md5')
                .update(`${context.userId}:${flag}`)
                .digest('hex');
            const bucket = parseInt(hash.slice(0, 8), 16) % 100;
            return bucket < percentage;
        }

        return false;
    }
}

// Testing with feature flags
describe('Feature: New Checkout Flow', () => {
    let flags;

    before(async () => {
        flags = new FeatureFlags(redis);
    });

    test('new checkout enabled for test users', async () => {
        const enabled = await flags.isEnabled('new-checkout', {
            userId: 'test-user-1',
            role: 'beta',
        });

        if (enabled) {
            // Test new flow
            const result = await checkout.newFlow(cart);
            assert.ok(result.success);
        } else {
            // Test old flow
            const result = await checkout.oldFlow(cart);
            assert.ok(result.success);
        }
    });

    test('gradual rollout at 10%', async () => {
        let enabledCount = 0;
        for (let i = 0; i < 1000; i++) {
            const enabled = await flags.isEnabled('new-checkout', {
                userId: `user-${i}`,
            });
            if (enabled) enabledCount++;
        }

        // Should be approximately 10% (±5% tolerance)
        assert.ok(enabledCount > 50 && enabledCount < 150,
            `Expected ~10%, got ${(enabledCount / 10).toFixed(1)}%`);
    });
});
```

## Canary Deployment Testing

```javascript
class CanaryDeployer {
    constructor(k8sClient, monitoring) {
        this.k8s = k8sClient;
        this.monitoring = monitoring;
    }

    async deployCanary(version, options = {}) {
        const canaryPercent = options.initialPercent || 5;
        const errorThreshold = options.errorThreshold || 0.01;
        const duration = options.duration || 300000; // 5 minutes

        // Deploy canary version
        await this.k8s.createDeployment(`app-canary-${version}`, {
            image: `app:${version}`,
            replicas: 1,
            labels: { version, track: 'canary' },
        });

        // Route percentage of traffic to canary
        await this.k8s.updateService('app', {
            canaryWeight: canaryPercent,
        });

        // Monitor for duration
        const startTime = Date.now();
        while (Date.now() - startTime < duration) {
            const metrics = await this.monitoring.getMetrics({
                version,
                since: startTime,
            });

            if (metrics.errorRate > errorThreshold) {
                console.error(`Canary error rate ${metrics.errorRate} exceeds threshold`);
                await this.rollbackCanary(version);
                return { success: false, reason: 'high_error_rate', metrics };
            }

            if (metrics.p95Latency > options.latencyThreshold) {
                console.error(`Canary p95 latency ${metrics.p95Latency}ms exceeds threshold`);
                await this.rollbackCanary(version);
                return { success: false, reason: 'high_latency', metrics };
            }

            await new Promise(r => setTimeout(r, 10000));
        }

        // Promote canary
        return { success: true, metrics: await this.monitoring.getMetrics({ version }) };
    }

    async rollbackCanary(version) {
        await this.k8s.deleteDeployment(`app-canary-${version}`);
        await this.k8s.updateService('app', { canaryWeight: 0 });
    }
}
```

## A/B Testing

```javascript
class ABTest {
    constructor(redis) {
        this.redis = redis;
    }

    async assignVariant(testId, userId) {
        const hash = createHash('md5')
            .update(`${testId}:${userId}`)
            .digest('hex');
        const bucket = parseInt(hash.slice(0, 8), 16) % 100;

        const testConfig = await this.redis.get(`ab-test:${testId}`);
        const config = JSON.parse(testConfig);

        // Find variant based on weight
        let cumulative = 0;
        for (const variant of config.variants) {
            cumulative += variant.weight;
            if (bucket < cumulative) {
                return variant;
            }
        }

        return config.variants[0]; // Fallback
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
}

// A/B test in application
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

// Test A/B assignments
describe('A/B Testing', () => {
    test('deterministic variant assignment', async () => {
        const abTest = new ABTest(redis);

        const v1 = await abTest.assignVariant('test-1', 'user-123');
        const v2 = await abTest.assignVariant('test-1', 'user-123');

        assert.equal(v1.id, v2.id); // Same user always gets same variant
    });

    test('distribution matches configured weights', async () => {
        const abTest = new ABTest(redis);
        const counts = {};

        for (let i = 0; i < 10000; i++) {
            const variant = await abTest.assignVariant('test-1', `user-${i}`);
            counts[variant.id] = (counts[variant.id] || 0) + 1;
        }

        // Control (50%) should be ~5000, Variant (50%) ~5000
        for (const [id, count] of Object.entries(counts)) {
            const percent = count / 100;
            assert.ok(percent > 45 && percent < 55, `${id}: ${percent}% expected ~50%`);
        }
    });
});
```

## Best Practices Checklist

- [ ] Use feature flags for gradual rollouts
- [ ] Monitor canary deployments continuously
- [ ] Set error rate and latency thresholds
- [ ] Make A/B test assignments deterministic
- [ ] Track conversion metrics
- [ ] Implement automatic rollback on failures
- [ ] Test feature flag edge cases

## Cross-References

- See [CI/CD](../10-testing-automation/01-ci-cd-integration.md) for automation
- See [Monitoring](../../08-authentication/10-authentication-monitoring/01-monitoring-metrics.md) for observability
- See [Deployment](../../10-deployment/) for deployment patterns

## Next Steps

Continue to [Testing Tools and Utilities](../12-testing-tools/01-factories-utilities.md).
