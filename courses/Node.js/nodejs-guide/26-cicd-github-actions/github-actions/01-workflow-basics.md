# Workflow Basics

## What You'll Learn

- What GitHub Actions is and how CI/CD works
- How to create a workflow file in `.github/workflows/`
- How to define triggers, jobs, and steps
- How to use the `uses` and `run` keywords
- How to test your workflow locally and on GitHub

## What Is CI/CD?

**CI** (Continuous Integration): automatically test and validate code on every push.
**CD** (Continuous Deployment): automatically deploy code after tests pass.

```
git push → GitHub Actions → Run tests → Build → Deploy
```

## Workflow File Structure

```yaml
# .github/workflows/ci.yml

name: CI

# When to run this workflow
on:
  push:
    branches: [main]       # Run on push to main
  pull_request:
    branches: [main]       # Run on PRs targeting main

# Jobs to run
jobs:
  test:
    # Environment to run on
    runs-on: ubuntu-latest

    # Steps within the job
    steps:
      # Step 1: Check out the code
      - name: Checkout code
        uses: actions/checkout@v4

      # Step 2: Set up Node.js
      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20'

      # Step 3: Install dependencies
      - name: Install dependencies
        run: npm ci  # ci = clean install (uses package-lock.json)

      # Step 4: Run tests
      - name: Run tests
        run: npm test
```

## Workflow Anatomy

### Triggers (`on`)

```yaml
on:
  push:
    branches: [main, develop]     # Specific branches
    paths: ['src/**']             # Only when these files change
  pull_request:
    branches: [main]
  schedule:
    - cron: '0 0 * * 1'          # Every Monday at midnight
  workflow_dispatch:               # Manual trigger (button in GitHub UI)
    inputs:
      environment:
        description: 'Deploy environment'
        required: true
        default: 'staging'
```

### Jobs

```yaml
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - run: echo "Testing..."

  deploy:
    runs-on: ubuntu-latest
    needs: test                   # Wait for 'test' to complete first
    if: github.ref == 'refs/heads/main'  # Only deploy from main
    steps:
      - run: echo "Deploying..."
```

### Steps

```yaml
steps:
  # Uses an existing action from the marketplace
  - name: Checkout
    uses: actions/checkout@v4

  # Runs a shell command
  - name: Run script
    run: |
      echo "Hello"
      npm test

  # With environment variables
  - name: Build
    run: npm run build
    env:
      NODE_ENV: production
```

## Complete CI Workflow

```yaml
# .github/workflows/ci.yml

name: CI Pipeline

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
          cache: 'npm'             # Cache node_modules for faster runs
      - run: npm ci
      - run: npm run lint

  test:
    runs-on: ubuntu-latest
    needs: lint                    # Run after lint passes
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
      - run: npm ci
      - run: npm test

  build:
    runs-on: ubuntu-latest
    needs: test                    # Run after tests pass
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
      - run: npm ci
      - run: npm run build

      # Upload build artifacts for later jobs
      - name: Upload build
        uses: actions/upload-artifact@v4
        with:
          name: build-output
          path: dist/
          retention-days: 7
```

## Environment Variables

```yaml
jobs:
  test:
    runs-on: ubuntu-latest

    # Job-level environment variables
    env:
      NODE_ENV: test
      DATABASE_URL: postgresql://localhost/test

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
      - run: npm ci

      # Step-level environment variables
      - name: Run tests
        run: npm test
        env:
          API_KEY: ${{ secrets.API_KEY }}  # From GitHub Secrets
```

## How It Works

### GitHub Actions Architecture

```
GitHub (stores workflow files)
    │
    │ Detects trigger (push, PR, schedule)
    │
    ▼
GitHub-hosted runner (Ubuntu VM)
    │
    ├── Checkout code
    ├── Setup Node.js
    ├── Install dependencies
    ├── Run tests
    └── Report results back to GitHub
```

### Workflow Run Lifecycle

```
Push to main
  → Workflow triggered
  → Job: lint (queued → in_progress → completed)
  → Job: test (queued → in_progress → completed)
  → Job: build (queued → in_progress → completed)
  → All jobs complete → Workflow passed ✓
```

## Common Mistakes

### Mistake 1: Using npm install Instead of npm ci

```yaml
# WRONG — npm install may update package-lock.json
- run: npm install

# CORRECT — npm ci installs exact versions from package-lock.json
- run: npm ci
```

### Mistake 2: Not Caching Dependencies

```yaml
# WRONG — installs all dependencies every run (slow)
- uses: actions/setup-node@v4
  with:
    node-version: '20'

# CORRECT — cache node_modules
- uses: actions/setup-node@v4
  with:
    node-version: '20'
    cache: 'npm'  # Caches ~/.npm and node_modules
```

### Mistake 3: Committing Secrets

```yaml
# WRONG — secrets in the workflow file
env:
  API_KEY: 'my-secret-key-123'

# CORRECT — use GitHub Secrets
env:
  API_KEY: ${{ secrets.API_KEY }}
# Set secrets in: GitHub repo → Settings → Secrets → Actions
```

## Try It Yourself

### Exercise 1: Create a Workflow

Create `.github/workflows/ci.yml` that runs `npm test` on every push. Push to GitHub and verify the workflow runs.

### Exercise 2: Add Linting

Add a lint step before the test step. Make the workflow fail if linting fails.

### Exercise 3: Matrix (Preview)

Try adding a matrix to test on Node.js 18, 20, and 22:

```yaml
strategy:
  matrix:
    node-version: [18, 20, 22]
```

## Next Steps

You can create GitHub Actions workflows. For a full test pipeline with matrix, continue to [Test Pipeline](./02-test-pipeline.md).
