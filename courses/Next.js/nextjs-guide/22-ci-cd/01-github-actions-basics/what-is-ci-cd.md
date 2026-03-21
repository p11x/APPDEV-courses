# What Is CI/CD

## What You'll Learn
- Understanding CI/CD concepts
- Benefits of automation
- GitHub Actions basics
- Building your first workflow

## Prerequisites
- Basic git knowledge
- Understanding of deployments
- A GitHub repository

## Concept Explained Simply

CI/CD stands for Continuous Integration/Continuous Deployment. It's a way to automatically test and deploy your code whenever you make changes. Instead of manually running tests and deploying, computers do it for you.

Think of it like an automatic quality control system in a factory: every time a product comes off the line (code is pushed), it's automatically tested. If it passes, it gets packaged and shipped (deployed).

## CI (Continuous Integration)

Every time you push code:
1. Clone the repository
2. Install dependencies  
3. Run linters
4. Run tests
5. Build the project
6. Report results

## CD (Continuous Deployment)

After CI passes:
1. Deploy to staging
2. Run integration tests
3. Deploy to production

## GitHub Actions

GitHub Actions is GitHub's built-in CI/CD tool. It runs workflows defined in YAML files in your repository.

```yaml
# .github/workflows/test.yml
name: Test

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
      - run: npm ci
      - run: npm test
```

## Complete Workflow

```yaml
# .github/workflows/ci-cd.yml
name: CI/CD Pipeline

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
      - run: npm ci
      - run: npm run lint
      - run: npm run typecheck
      - run: npm test -- --coverage

  build:
    needs: test
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
      - run: npm ci
      - run: npm run build
```

## Common Mistakes

### Not Caching Dependencies

```typescript
// WRONG - Downloads dependencies every time
steps:
  - run: npm ci

// CORRECT - Cache node_modules
steps:
  - uses: actions/setup-node@v4
    with:
      cache: 'npm'
  - run: npm ci
```

### Running Tests on Every Push

```typescript
// WRONG - Tests also run for docs-only changes
on: [push]

// CORRECT - Only test relevant paths
on:
  push:
    paths:
      - '**.ts'
      - '**.tsx'
      - 'package.json'
```

## Summary

- CI automatically tests code on every change
- CD automatically deploys after tests pass
- GitHub Actions is built into GitHub
- Workflows are defined in YAML files
- Always cache dependencies for speed
