# CI/CD Testing Pipeline

## Overview

Running tests in CI/CD ensures code quality and prevents bugs from reaching production. This guide covers setting up automated testing pipelines using GitHub Actions.

## Prerequisites

- GitHub repository
- Basic CI/CD knowledge

## Core Concepts

### GitHub Actions Workflow

```yaml
# File: .github/workflows/test.yml

name: Test Pipeline

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main, develop]

jobs:
  unit-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Run unit tests
        run: npm run test:unit
      
      - name: Upload coverage
        uses: codecov/codecov-action@v3

  e2e-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Install Playwright browsers
        run: npx playwright install --with-deps
      
      - name: Run E2E tests
        run: npx playwright test
      
      - name: Upload test results
        uses: actions/upload-artifact@v3
        if: always()
        with:
          name: playwright-report
          path: playwright-report/
```

## Key Takeaways

- Run unit tests on every PR
- Run E2E tests before merging
- Upload coverage reports
- Cache dependencies for faster builds

## What's Next

This concludes the Testing section. Continue to [JWT Authentication Flow](/11-real-world-projects/01-auth-system/01-jwt-authentication-flow.md) to learn about implementing authentication in React applications.