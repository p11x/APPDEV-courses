# Playwright CI

## What You'll Learn

- How to run Playwright in CI
- How to configure GitHub Actions
- How to handle test artifacts
- How to optimize CI performance

## GitHub Actions

```yaml
# .github/workflows/playwright.yml

name: Playwright Tests
on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: 20
          cache: 'npm'

      - run: npm ci
      - run: npx playwright install --with-deps

      - name: Run Playwright tests
        run: npx playwright test

      - uses: actions/upload-artifact@v4
        if: ${{ !cancelled() }}
        with:
          name: playwright-report
          path: playwright-report/
          retention-days: 30
```

## Sharding

```yaml
jobs:
  test:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        shard: [1, 2, 3, 4]
    steps:
      - run: npx playwright test --shard=${{ matrix.shard }}/${{ strategy.job-total }}
```

## Next Steps

For Testcontainers, continue to [Testcontainers Setup](../03-testcontainers-node/01-testcontainers-setup.md).
