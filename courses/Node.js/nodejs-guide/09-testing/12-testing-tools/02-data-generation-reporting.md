# Test Data Generation, Reporting, and Visualization

## What You'll Learn

- Test data generation with Faker
- Test report generation
- Test visualization and dashboards
- Test logging and debugging tools

## Test Data Generation with Faker

```javascript
import { faker } from '@faker-js/faker';

class DataGenerator {
    static user(overrides = {}) {
        return {
            id: faker.string.uuid(),
            name: faker.person.fullName(),
            email: faker.internet.email().toLowerCase(),
            password: 'TestPass123!',
            role: faker.helpers.arrayElement(['user', 'admin', 'moderator']),
            createdAt: faker.date.past(),
            ...overrides,
        };
    }

    static users(count, overrides = {}) {
        return Array.from({ length: count }, (_, i) =>
            this.user({ email: `user${i}@test.com`, ...overrides })
        );
    }

    static product(overrides = {}) {
        return {
            id: faker.string.uuid(),
            name: faker.commerce.productName(),
            description: faker.commerce.productDescription(),
            price: parseFloat(faker.commerce.price({ min: 1, max: 1000 })),
            category: faker.commerce.department(),
            inStock: faker.datatype.boolean(),
            ...overrides,
        };
    }

    static order(userId, overrides = {}) {
        return {
            id: faker.string.uuid(),
            userId,
            items: Array.from({ length: faker.number.int({ min: 1, max: 5 }) }, () => ({
                productId: faker.string.uuid(),
                quantity: faker.number.int({ min: 1, max: 10 }),
                price: parseFloat(faker.commerce.price()),
            })),
            status: faker.helpers.arrayElement(['pending', 'confirmed', 'shipped', 'delivered']),
            createdAt: faker.date.recent(),
            ...overrides,
        };
    }

    static apiResponse(data, overrides = {}) {
        return {
            data,
            pagination: {
                page: 1,
                limit: 20,
                total: Array.isArray(data) ? data.length : 1,
            },
            timestamp: new Date().toISOString(),
            ...overrides,
        };
    }
}

// Usage in tests
const testUser = DataGenerator.user();
const testUsers = DataGenerator.users(10);
const testProducts = Array.from({ length: 50 }, () => DataGenerator.product());
```

## Test Report Generation

```javascript
// test/reporters/json-reporter.js
import { writeFileSync } from 'node:fs';

class JSONReporter {
    constructor() {
        this.results = [];
    }

    onTestComplete(test) {
        this.results.push({
            name: test.name,
            suite: test.suite,
            status: test.status, // 'passed', 'failed', 'skipped'
            duration: test.duration,
            error: test.error?.message,
            timestamp: new Date().toISOString(),
        });
    }

    generate(outputPath) {
        const report = {
            timestamp: new Date().toISOString(),
            summary: {
                total: this.results.length,
                passed: this.results.filter(r => r.status === 'passed').length,
                failed: this.results.filter(r => r.status === 'failed').length,
                skipped: this.results.filter(r => r.status === 'skipped').length,
                duration: this.results.reduce((s, r) => s + (r.duration || 0), 0),
            },
            results: this.results,
        };

        writeFileSync(outputPath, JSON.stringify(report, null, 2));
        return report;
    }

    printSummary() {
        const { summary } = this.generate();
        console.log('\n=== Test Summary ===');
        console.log(`Total: ${summary.total}`);
        console.log(`Passed: ${summary.passed}`);
        console.log(`Failed: ${summary.failed}`);
        console.log(`Skipped: ${summary.skipped}`);
        console.log(`Duration: ${(summary.duration / 1000).toFixed(1)}s`);

        if (summary.failed > 0) {
            console.log('\nFailed Tests:');
            this.results
                .filter(r => r.status === 'failed')
                .forEach(r => console.log(`  - ${r.name}: ${r.error}`));
        }
    }
}
```

## Test Debugging Tools

```javascript
// test/helpers/debug.js
export function debugTest(label, value) {
    if (process.env.DEBUG_TESTS) {
        console.log(`[DEBUG] ${label}:`, JSON.stringify(value, null, 2));
    }
}

export function logTestDuration(label) {
    const start = performance.now();
    return {
        end() {
            const elapsed = performance.now() - start;
            if (process.env.DEBUG_TESTS) {
                console.log(`[TIMING] ${label}: ${elapsed.toFixed(1)}ms`);
            }
            return elapsed;
        },
    };
}

export function captureConsole() {
    const logs = [];
    const original = { ...console };

    console.log = (...args) => logs.push({ level: 'log', args });
    console.error = (...args) => logs.push({ level: 'error', args });
    console.warn = (...args) => logs.push({ level: 'warn', args });

    return {
        logs,
        restore() {
            Object.assign(console, original);
        },
    };
}

export function retryTest(fn, attempts = 3, delay = 100) {
    return async (...args) => {
        for (let i = 0; i < attempts; i++) {
            try {
                return await fn(...args);
            } catch (err) {
                if (i === attempts - 1) throw err;
                await new Promise(r => setTimeout(r, delay));
            }
        }
    };
}
```

## Common Mistakes

- Not generating realistic test data
- Not reporting test results in CI
- Not debugging flaky tests
- Not capturing console output in tests

## Cross-References

- See [Factories](./01-factories-utilities.md) for test utilities
- See [CI/CD](../10-testing-automation/01-ci-cd-integration.md) for automation
- See [Testing Production](../11-testing-production/01-canary-feature-flags.md) for prod testing
