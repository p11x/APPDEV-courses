# Lint and Typecheck in CI

## What You'll Learn
- Add lint to CI pipeline
- Type checking
- Fail on errors

## Prerequisites
- GitHub Actions workflow

## Do I Need This Right Now?
Linting and type checking catch errors before they reach production.

## Adding Lint

```yaml
name: CI

on: [push]

jobs:
  lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
      - run: npm ci
      - run: npm run lint
```

## Adding TypeCheck

```yaml
jobs:
  typecheck:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
      - run: npm ci
      - run: npm run typecheck
```

## Summary
- Add lint job to catch code issues
- Add typecheck job for TypeScript
- Both should fail pipeline if errors found

## Next Steps
- [running-tests-in-ci.md](./running-tests-in-ci.md) — Running tests
