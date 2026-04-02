# Test Pipeline

## What You'll Learn

- How to build a full test pipeline with GitHub Actions
- How to use matrix strategy for multi-version testing
- How to upload test coverage reports
- How to use service containers (database)
- How to cache dependencies for faster builds

## Matrix Strategy

```yaml
# .github/workflows/test.yml

name: Test Pipeline

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest

    # Matrix: run the same job with different configurations
    strategy:
      matrix:
        node-version: [18, 20, 22]    # Test on 3 Node.js versions
      fail-fast: false                  # Run all even if one fails

    steps:
      - uses: actions/checkout@v4

      - name: Setup Node.js ${{ matrix.node-version }}
        uses: actions/setup-node@v4
        with:
          node-version: ${{ matrix.node-version }}
          cache: 'npm'

      - run: npm ci
      - run: npm test

      # Upload coverage only for one version (e.g., Node 20)
      - name: Upload coverage
        if: matrix.node-version == 20
        uses: actions/upload-artifact@v4
        with:
          name: coverage-node20
          path: coverage/
```

## With Database Service

```yaml
# .github/workflows/test-with-db.yml

name: Test with Database

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest

    # Service containers — start alongside your tests
    services:
      postgres:
        image: postgres:16
        env:
          POSTGRES_USER: test
          POSTGRES_PASSWORD: test
          POSTGRES_DB: testdb
        ports:
          - 5432:5432
        # Wait for PostgreSQL to be ready
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5

      redis:
        image: redis:7-alpine
        ports:
          - 6379:6379
        options: >-
          --health-cmd "redis-cli ping"
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5

    env:
      DATABASE_URL: postgresql://test:test@localhost:5432/testdb
      REDIS_URL: redis://localhost:6379
      NODE_ENV: test

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'

      - run: npm ci

      # Run database migrations
      - name: Run migrations
        run: npx prisma migrate deploy

      # Seed the test database
      - name: Seed database
        run: npx prisma db seed

      # Run tests
      - name: Run tests
        run: npm test
```

## Coverage Reporting

```yaml
# .github/workflows/coverage.yml

name: Coverage

on: [push, pull_request]

jobs:
  coverage:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'

      - run: npm ci

      # Run tests with coverage
      - name: Run tests with coverage
        run: npm test -- --coverage

      # Upload coverage to Codecov
      - name: Upload to Codecov
        uses: codecov/codecov-action@v4
        with:
          token: ${{ secrets.CODECOV_TOKEN }}
          files: coverage/lcov-report/lcov.info
          fail_ci_if_error: true

      # Or upload as artifact
      - name: Upload coverage artifact
        uses: actions/upload-artifact@v4
        with:
          name: coverage-report
          path: coverage/
          retention-days: 14
```

## Lint + Test + Build Pipeline

```yaml
# .github/workflows/ci.yml

name: CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
      - run: npm ci
      - run: npm run lint

  typecheck:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
      - run: npm ci
      - run: npm run typecheck

  test:
    runs-on: ubuntu-latest
    needs: [lint, typecheck]          # Only test if lint and typecheck pass
    strategy:
      matrix:
        node-version: [18, 20, 22]
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: ${{ matrix.node-version }}
          cache: 'npm'
      - run: npm ci
      - run: npm test

  build:
    runs-on: ubuntu-latest
    needs: test
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
      - run: npm ci
      - run: npm run build
      - uses: actions/upload-artifact@v4
        with:
          name: build
          path: dist/
```

## How It Works

### Matrix Creates Multiple Jobs

```yaml
strategy:
  matrix:
    node-version: [18, 20, 22]
```

Creates 3 separate jobs:
- `test (node-version: 18)`
- `test (node-version: 20)`
- `test (node-version: 22)`

Each runs independently and in parallel.

### Service Containers

```yaml
services:
  postgres:
    image: postgres:16
    ports:
      - 5432:5432
```

GitHub Actions starts the container before your steps and stops it after. Your tests can connect to `localhost:5432`.

## Common Mistakes

### Mistake 1: Not Using fail-fast: false

```yaml
# WRONG — if Node 18 fails, Node 20 and 22 are cancelled
strategy:
  matrix:
    node-version: [18, 20, 22]

# CORRECT — let all versions run even if one fails
strategy:
  matrix:
    node-version: [18, 20, 22]
  fail-fast: false
```

### Mistake 2: Installing Dependencies Every Time

```yaml
# WRONG — slow: downloads packages every run
- run: npm install

# CORRECT — cache + ci for faster, deterministic installs
- uses: actions/setup-node@v4
  with:
    node-version: '20'
    cache: 'npm'
- run: npm ci
```

### Mistake 3: Missing Health Checks for Services

```yaml
# WRONG — tests start before database is ready
services:
  postgres:
    image: postgres:16
    # No health check — tests may fail with "connection refused"

# CORRECT — wait for health check
services:
  postgres:
    image: postgres:16
    options: >-
      --health-cmd pg_isready
      --health-interval 10s
      --health-timeout 5s
      --health-retries 5
```

## Try It Yourself

### Exercise 1: Multi-Version Matrix

Test your project on Node.js 18, 20, and 22. Verify all three pass.

### Exercise 2: Database Tests

Add a PostgreSQL service container. Run integration tests that query the database.

### Exercise 3: Coverage Badge

Upload coverage to Codecov. Add a coverage badge to your README.

## Next Steps

You have a full test pipeline. For publishing Docker images, continue to [Docker Publish](./03-docker-publish.md).
