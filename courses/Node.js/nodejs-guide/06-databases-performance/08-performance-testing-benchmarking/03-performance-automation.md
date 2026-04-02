# Performance Testing Automation

## What You'll Learn

- CI/CD performance testing integration
- Automated benchmark comparison
- Performance gate enforcement
- Report generation
- Alert integration

## CI/CD Performance Gate

```javascript
// performance-test.js
import autocannon from 'autocannon';
import fs from 'node:fs';

class PerformanceGate {
    constructor(config) {
        this.baseUrl = config.baseUrl;
        this.thresholds = config.thresholds;
        this.baselinePath = config.baselinePath || './performance-baseline.json';
    }

    async loadBaseline() {
        try {
            return JSON.parse(fs.readFileSync(this.baselinePath, 'utf-8'));
        } catch {
            return null;
        }
    }

    async saveBaseline(results) {
        fs.writeFileSync(this.baselinePath, JSON.stringify(results, null, 2));
    }

    async runEndpointTest(endpoint, options = {}) {
        const result = await autocannon({
            url: `${this.baseUrl}${endpoint}`,
            connections: options.connections || 50,
            duration: options.duration || 15,
        });

        return {
            endpoint,
            throughput: result.throughput.average,
            latency: {
                avg: result.latency.average,
                p95: result.latency.p95,
                p99: result.latency.p99,
            },
            errors: result.errors,
        };
    }

    async runAllTests() {
        const results = {};

        for (const endpoint of this.thresholds.endpoints) {
            results[endpoint.path] = await this.runEndpointTest(endpoint.path, endpoint.options);
        }

        return results;
    }

    async evaluate(results) {
        const baseline = await this.loadBaseline();
        const violations = [];

        for (const [endpoint, result] of Object.entries(results)) {
            const config = this.thresholds.endpoints.find(e => e.path === endpoint);
            if (!config) continue;

            // Check absolute thresholds
            if (config.maxLatencyP95 && result.latency.p95 > config.maxLatencyP95) {
                violations.push({
                    endpoint,
                    type: 'absolute',
                    metric: 'latency.p95',
                    value: result.latency.p95,
                    threshold: config.maxLatencyP95,
                });
            }

            if (config.minThroughput && result.throughput < config.minThroughput) {
                violations.push({
                    endpoint,
                    type: 'absolute',
                    metric: 'throughput',
                    value: result.throughput,
                    threshold: config.minThroughput,
                });
            }

            // Compare with baseline
            if (baseline?.[endpoint]) {
                const base = baseline[endpoint];
                const latencyIncrease = ((result.latency.avg - base.latency.avg) / base.latency.avg) * 100;

                if (latencyIncrease > this.thresholds.maxRegressionPercent) {
                    violations.push({
                        endpoint,
                        type: 'regression',
                        metric: 'latency.avg',
                        baseline: base.latency.avg,
                        current: result.latency.avg,
                        change: +latencyIncrease.toFixed(1),
                        threshold: this.thresholds.maxRegressionPercent,
                    });
                }
            }
        }

        return {
            passed: violations.length === 0,
            violations,
            results,
        };
    }

    async runAndEvaluate() {
        const results = await this.runAllTests();
        const evaluation = await this.evaluate(results);

        if (evaluation.passed) {
            console.log('Performance gate PASSED');
            await this.saveBaseline(results);
        } else {
            console.error('Performance gate FAILED');
            for (const v of evaluation.violations) {
                console.error(`  ${v.endpoint}: ${v.metric} ${v.type === 'regression' ? 'regressed by ' + v.change + '%' : 'exceeded threshold'}`);
            }
        }

        return evaluation;
    }
}

// Configuration
const gate = new PerformanceGate({
    baseUrl: process.env.TEST_URL || 'http://localhost:3000',
    thresholds: {
        maxRegressionPercent: 20,
        endpoints: [
            { path: '/api/users', options: { connections: 50, duration: 15 }, maxLatencyP95: 200, minThroughput: 500 },
            { path: '/api/products', options: { connections: 50, duration: 15 }, maxLatencyP95: 300, minThroughput: 300 },
            { path: '/api/orders', options: { connections: 30, duration: 15 }, maxLatencyP95: 500, minThroughput: 200 },
        ],
    },
});

const result = await gate.runAndEvaluate();
process.exit(result.passed ? 0 : 1);
```

## GitHub Actions Integration

```yaml
# .github/workflows/performance.yml
name: Performance Tests

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  performance:
    runs-on: ubuntu-latest

    services:
      postgres:
        image: postgres:15
        env:
          POSTGRES_PASSWORD: test
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5

    steps:
      - uses: actions/checkout@v4

      - uses: actions/setup-node@v4
        with:
          node-version: '20'

      - run: npm ci
      - run: npm run build

      - name: Start application
        run: npm start &
        env:
          DATABASE_URL: postgresql://postgres:test@localhost:5432/test

      - name: Wait for server
        run: npx wait-on http://localhost:3000/health

      - name: Run performance tests
        run: node performance-test.js
        env:
          TEST_URL: http://localhost:3000

      - name: Upload results
        if: always()
        uses: actions/upload-artifact@v4
        with:
          name: performance-results
          path: performance-baseline.json
```

## Best Practices Checklist

- [ ] Run performance tests in CI/CD pipeline
- [ ] Set performance gates that block deployment
- [ ] Store baselines in version control
- [ ] Generate human-readable performance reports
- [ ] Alert on performance regressions > 20%
- [ ] Run tests against production-like data
- [ ] Schedule nightly full performance suites

## Cross-References

- See [Load Testing](./01-load-testing.md) for testing setup
- See [Regression Detection](../03-performance-monitoring-analysis/04-regression-detection.md) for detection
- See [Stress Testing](./02-stress-capacity-testing.md) for capacity planning

## Next Steps

Continue to [Modern Database Technologies](../09-modern-database-technologies/01-nosql-patterns.md) for database types.
