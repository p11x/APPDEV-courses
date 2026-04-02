# Test Parallelization and Environment Management

## What You'll Learn

- Test parallelization strategies
- Test environment setup and teardown
- Test data management
- Test reporting and failure analysis

## Test Parallelization

```javascript
// package.json scripts
{
    "scripts": {
        "test": "node --test test/**/*.test.js",
        "test:parallel": "node --test --test-parallel test/**/*.test.js",
        "test:coverage": "node --test --experimental-test-coverage test/**/*.test.js",
        "test:watch": "node --test --watch test/**/*.test.js"
    }
}
```

```javascript
// Jest parallel configuration
// jest.config.js
export default {
    maxWorkers: '50%',
    testSequencer: './test/custom-sequencer.js',
    projects: [
        {
            displayName: 'unit',
            testMatch: ['<rootDir>/test/unit/**/*.test.js'],
        },
        {
            displayName: 'integration',
            testMatch: ['<rootDir>/test/integration/**/*.test.js'],
            maxWorkers: 2, // Fewer workers for integration tests
        },
    ],
};
```

## Test Environment Setup

```javascript
// test/setup.js
import { before, after, beforeEach, afterEach } from 'node:test';
import { Pool } from 'pg';
import { createClient } from 'redis';

let pool;
let redis;

before(async () => {
    // Start test dependencies
    pool = new Pool({ connectionString: process.env.TEST_DATABASE_URL });
    redis = createClient({ url: process.env.TEST_REDIS_URL });
    await redis.connect();

    // Run migrations
    await runMigrations(pool);

    // Set global test context
    globalThis.testPool = pool;
    globalThis.testRedis = redis;
});

after(async () => {
    await pool?.end();
    await redis?.quit();
});

beforeEach(async () => {
    // Clean data between tests
    await pool.query('DELETE FROM orders');
    await pool.query('DELETE FROM users');
});
```

## Test Data Management

```javascript
// test/factories/index.js
import { faker } from '@faker-js/faker';
import bcrypt from 'bcrypt';

export class TestFactory {
    static async createUser(overrides = {}) {
        const user = {
            name: faker.person.fullName(),
            email: faker.internet.email().toLowerCase(),
            passwordHash: await bcrypt.hash('TestPass123!', 4),
            role: 'user',
            ...overrides,
        };

        const { rows } = await globalThis.testPool.query(
            'INSERT INTO users (name, email, password_hash, role) VALUES ($1, $2, $3, $4) RETURNING *',
            [user.name, user.email, user.passwordHash, user.role]
        );

        return rows[0];
    }

    static async createProduct(overrides = {}) {
        const product = {
            name: faker.commerce.productName(),
            price: parseFloat(faker.commerce.price()),
            category: faker.commerce.department(),
            ...overrides,
        };

        const { rows } = await globalThis.testPool.query(
            'INSERT INTO products (name, price, category) VALUES ($1, $2, $3) RETURNING *',
            [product.name, product.price, product.category]
        );

        return rows[0];
    }

    static async seedDatabase() {
        const users = [];
        for (let i = 0; i < 5; i++) {
            users.push(await this.createUser({ email: `user${i}@test.com` }));
        }
        return { users };
    }
}
```

## Test Failure Analysis

```javascript
// test/helpers/failure-analyzer.js
class TestFailureAnalyzer {
    constructor() {
        this.failures = [];
    }

    record(testName, error, context = {}) {
        this.failures.push({
            test: testName,
            error: error.message,
            stack: error.stack,
            context,
            timestamp: new Date().toISOString(),
        });
    }

    analyze() {
        const byCategory = {};
        for (const failure of this.failures) {
            const category = this.categorize(failure.error);
            byCategory[category] = (byCategory[category] || 0) + 1;
        }

        return {
            total: this.failures.length,
            byCategory,
            failures: this.failures,
        };
    }

    categorize(error) {
        if (error.includes('timeout')) return 'timeout';
        if (error.includes('ECONNREFUSED')) return 'connection';
        if (error.includes('assertion')) return 'assertion';
        return 'unknown';
    }

    report() {
        const analysis = this.analyze();
        console.log('\n=== Test Failure Report ===');
        console.log(`Total failures: ${analysis.total}`);
        for (const [category, count] of Object.entries(analysis.byCategory)) {
            console.log(`  ${category}: ${count}`);
        }
    }
}
```

## CI/CD Test Pipeline

```yaml
# .github/workflows/test.yml
name: Tests

on: [push, pull_request]

jobs:
  unit:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '20' }
      - run: npm ci
      - run: npm run test:unit -- --coverage
      - uses: codecov/codecov-action@v3

  integration:
    runs-on: ubuntu-latest
    needs: unit
    services:
      postgres:
        image: postgres:15
        env:
          POSTGRES_DB: test_db
          POSTGRES_USER: test
          POSTGRES_PASSWORD: test
        ports: ['5432:5432']
        options: --health-cmd pg_isready
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '20' }
      - run: npm ci
      - run: npm run test:integration
        env:
          TEST_DATABASE_URL: postgresql://test:test@localhost:5432/test_db

  e2e:
    runs-on: ubuntu-latest
    needs: integration
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '20' }
      - run: npm ci
      - run: npx playwright install --with-deps
      - run: npm run test:e2e
```

## Common Mistakes

- Not parallelizing tests (slow CI)
- Not cleaning up test data between runs
- Not analyzing failure patterns
- Not separating unit and integration test environments

## Cross-References

- See [CI/CD](./01-ci-cd-integration.md) for pipeline setup
- See [Testing Tools](../12-testing-tools/01-factories-utilities.md) for utilities
- See [Testing Production](../11-testing-production/01-canary-feature-flags.md) for prod testing

## Next Steps

Continue to [Testing Production: A/B Testing](../11-testing-production/02-ab-testing-live.md).
