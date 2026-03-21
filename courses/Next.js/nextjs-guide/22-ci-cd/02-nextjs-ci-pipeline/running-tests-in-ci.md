# Running Tests in CI

## What You'll Learn
- Run tests in GitHub Actions
- Coverage reports
- Best practices

## Prerequisites
- Tests set up (Jest, Playwright)

## Do I Need This Right Now?
Tests in CI ensure you don't break existing functionality.

## Adding Tests

```yaml
name: CI

on: [push]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
      - run: npm ci
      - run: npm run test
      - run: npm run test:e2e
```

## Summary
- Add test jobs to workflow
- Run both unit and e2e tests
- Fail pipeline if tests fail
