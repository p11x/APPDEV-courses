# Testing Automation and CI/CD Integration

## What You'll Learn

- CI/CD pipeline configuration
- Test parallelization
- Test environment management
- Test reporting
- Test failure analysis

## GitHub Actions CI/CD

```yaml
# .github/workflows/test.yml
name: Tests

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  unit-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'

      - run: npm ci
      - run: npm run test:unit -- --coverage

      - name: Upload coverage
        uses: codecov/codecov-action@v3
        with:
          file: ./coverage/lcov.info

  integration-tests:
    runs-on: ubuntu-latest
    needs: unit-tests
    services:
      postgres:
        image: postgres:15
        env:
          POSTGRES_DB: test_db
          POSTGRES_USER: test_user
          POSTGRES_PASSWORD: test_pass
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5
        ports:
          - 5432:5432

      redis:
        image: redis:7
        options: >-
          --health-cmd "redis-cli ping"
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5
        ports:
          - 6379:6379

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'

      - run: npm ci
      - run: npm run test:integration
        env:
          DATABASE_URL: postgresql://test_user:test_pass@localhost:5432/test_db
          REDIS_URL: redis://localhost:6379

  e2e-tests:
    runs-on: ubuntu-latest
    needs: integration-tests
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'

      - run: npm ci
      - run: npx playwright install --with-deps

      - run: npm run test:e2e
        env:
          CI: true

      - name: Upload test results
        if: always()
        uses: actions/upload-artifact@v4
        with:
          name: playwright-report
          path: playwright-report/

  security-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: npm audit --audit-level=high
      - run: npm run test:security
```

## Test Parallelization

```javascript
// jest.config.js — parallel execution
export default {
    maxWorkers: '50%', // Use half of available CPUs
    testSequencer: './test/custom-sequencer.js',
};

// For node:test — use --test-parallel
// package.json
{
    "scripts": {
        "test:parallel": "node --test --test-parallel test/**/*.test.js",
        "test:sequential": "node --test test/**/*.test.js"
    }
}

// Worker-based test splitting
// scripts/split-tests.js
import fs from 'node:fs';
import path from 'node:path';

function splitTests(testDir, numWorkers) {
    const files = fs.readdirSync(testDir)
        .filter(f => f.endsWith('.test.js'))
        .map(f => path.join(testDir, f));

    const chunks = Array.from({ length: numWorkers }, () => []);
    files.forEach((file, i) => {
        chunks[i % numWorkers].push(file);
    });

    return chunks;
}
```

## Test Reporting

```javascript
// jest-junit for CI reporting
// jest.config.js
export default {
    reporters: [
        'default',
        ['jest-junit', {
            outputDirectory: 'test-results',
            outputName: 'junit.xml',
            classNameTemplate: '{classname}',
            titleTemplate: '{title}',
        }],
    ],
};

// Custom reporter
class CustomReporter {
    onRunComplete(_, results) {
        const summary = {
            total: results.numTotalTests,
            passed: results.numPassedTests,
            failed: results.numFailedTests,
            skipped: results.numPendingTests,
            duration: results.testResults.reduce((sum, r) => sum + r.perfStats.runtime, 0),
        };

        console.log('\n=== Test Summary ===');
        console.log(`Total: ${summary.total}`);
        console.log(`Passed: ${summary.passed}`);
        console.log(`Failed: ${summary.failed}`);
        console.log(`Duration: ${(summary.duration / 1000).toFixed(1)}s`);

        if (results.numFailedTests > 0) {
            console.log('\nFailed Tests:');
            results.testResults.forEach(result => {
                result.testResults
                    .filter(t => t.status === 'failed')
                    .forEach(t => {
                        console.log(`  - ${t.fullName}`);
                        console.log(`    ${t.failureMessages[0]?.split('\n')[0]}`);
                    });
            });
        }
    }
}
```

## Test Environment Management

```javascript
// test/setup.js
import { before, after } from 'node:test';

before(async () => {
    // Start test database
    await startTestDatabase();

    // Run migrations
    await runMigrations();

    // Seed test data
    await seedTestData();
});

after(async () => {
    // Cleanup
    await dropTestDatabase();
    await stopTestDatabase();
});

// Environment-specific configuration
function getTestConfig() {
    return {
        database: {
            url: process.env.TEST_DATABASE_URL || 'postgresql://localhost:5432/test_db',
        },
        redis: {
            url: process.env.TEST_REDIS_URL || 'redis://localhost:6379/1',
        },
        jwt: {
            secret: 'test-secret-not-for-production',
        },
    };
}
```

## Best Practices Checklist

- [ ] Run unit tests first (fast feedback)
- [ ] Parallelize independent tests
- [ ] Generate JUnit XML reports for CI
- [ ] Upload coverage to Codecov/Coveralls
- [ ] Cache node_modules in CI
- [ ] Use test databases in CI
- [ ] Capture test artifacts on failure
- [ ] Set test timeout budgets

## Cross-References

- See [Production Testing](../11-testing-production/01-canary-feature-flags.md) for prod testing
- See [Testing Fundamentals](../01-testing-fundamentals/01-testing-pyramid-architecture.md) for principles
- See [Performance Testing](../09-security-performance/01-security-testing.md) for load tests

## Next Steps

Continue to [Testing in Production](../11-testing-production/01-canary-feature-flags.md).
