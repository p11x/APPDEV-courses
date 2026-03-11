# GitHub Actions

## Topic Title
Automating Workflows with GitHub Actions

## Concept Explanation

GitHub Actions is a powerful automation platform that allows you to automate your software development workflows directly in GitHub. You can build, test, and deploy your Angular applications automatically.

### What are GitHub Actions?

GitHub Actions are:
- Automated workflows
- Triggered by events (push, PR, etc.)
- Run in virtual machines
- Define in YAML files

### Why Use GitHub Actions?

1. **CI/CD**: Automatically build and deploy
2. **Testing**: Run tests on every push
3. **Quality**: Enforce code standards
4. **Automation**: Reduce manual work

## Key Concepts

### Workflows

YAML files in `.github/workflows/`:

```yaml
name: Build and Test

on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '18'
```

### Events

Triggers for workflows:

```yaml
on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
  workflow_dispatch:  # Manual trigger
  schedule:
    - cron: '0 0 * * *'  # Daily
```

### Jobs

Groups of steps that run on same runner:

```yaml
jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
      - name: Checkout code
        uses: actions/checkout@v3
      
      - name: Build
        run: npm run build
```

### Runners

Virtual machines to run jobs:
- ubuntu-latest
- windows-latest
- macos-latest

## Angular CI/CD Example

### Basic Build Workflow

Create `.github/workflows/angular.yml`:

```yaml
name: Angular CI

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v3

    - name: Setup Node.js
      uses: actions/setup-node@v3
      with:
        node-version: '18'
        cache: 'npm'

    - name: Install dependencies
      run: npm ci

    - name: Build
      run: npm run build

    - name: Run tests
      run: npm test -- --no-watch --browsers=ChromeHeadless
```

### Deploy to GitHub Pages

```yaml
name: Deploy to GitHub Pages

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup Node.js
      uses: actions/setup-node@v3
      with:
        node-version: '18'
    
    - name: Install dependencies
      run: npm ci
    
    - name: Build Angular app
      run: npm run build -- --configuration production --base-href "/repo-name/"
    
    - name: Deploy
      uses: peaceiris/actions-gh-pages@v3
      with:
        github_token: ${{ secrets.GITHUB_TOKEN }}
        publish_dir: ./dist/repo-name
```

### Deploy to Firebase

```yaml
name: Deploy to Firebase

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup Node.js
      uses: actions/setup-node@v3
      with:
        node-version: '18'
    
    - name: Install and Build
      run: |
        npm ci
        npm run build -- --configuration production
    
    - name: Deploy to Firebase
      uses: w9jds/firebase-action@v2
      with:
        args: deploy --only hosting
      env:
        FIREBASE_TOKEN: ${{ secrets.FIREBASE_TOKEN }}
```

## Angular Testing Workflow

### Test with Chrome Headless

```yaml
name: Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup Node.js
      uses: actions/setup-node@v3
      with:
        node-version: '18'
        cache: 'npm'
    
    - name: Install
      run: npm ci
    
    - name: Lint
      run: npm run lint
    
    - name: Test
      run: npm test -- --no-watch --browsers=ChromeHeadless
    
    - name: Build
      run: npm run build
```

## Security and Secrets

### Using Secrets

```yaml
- name: Deploy
  run: npm run deploy
  env:
    API_TOKEN: ${{ secrets.API_TOKEN }}
```

### Setting Secrets

1. Go to Repository Settings
2. Click "Secrets and variables" → "Actions"
3. Add new secret

## Matrix Builds

```yaml
jobs:
  build:
    strategy:
      matrix:
        node-version: [16, 18, 20]
        Angular-version: [15, 16, 17]
    
    steps:
    - uses: actions/checkout@v3
    - name: Use Node.js ${{ matrix.node-version }}
      uses: actions/setup-node@v3
      with:
        node-version: ${{ matrix.node-version }}
    - name: Install
      run: npm ci
    - name: Test
      run: npm test
```

## Best Practices

### 1. Use Caching

```yaml
- name: Cache npm
  uses: actions/cache@v3
  with:
    path: ~/.npm
    key: ${{ runner.os }}-npm-${{ hashFiles('**/package-lock.json') }}
```

### 2. Run Fast First

```yaml
jobs:
  lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - run: npm ci
      - run: npm run lint

  test:
    needs: lint
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - run: npm ci
      - run: npm test
```

### 3. Fail Fast

```yaml
strategy:
  fail-fast: true  # Stop other jobs if one fails
```

## Common Actions

| Action | Use |
|--------|-----|
| actions/checkout | Checkout code |
| actions/setup-node | Setup Node.js |
| actions/cache | Cache dependencies |
| peaceiris/actions-gh-pages | Deploy to Pages |
| aws-actions/configure-aws-credentials | Configure AWS |

## Exercises for Students

### Exercise 1: Create Basic Workflow
1. Create `.github/workflows` directory
2. Add basic build workflow
3. Push and watch it run

### Exercise 2: Add Testing
1. Add test step to workflow
2. Configure ChromeHeadless
3. See tests run automatically

### Exercise 3: Deploy to Pages
1. Add GitHub Pages deployment
2. Enable Pages in settings
3. Deploy on push

## Summary

GitHub Actions automate development:

- **Workflows**: YAML files in `.github/workflows/`
- **Events**: Triggers like push, PR
- **Jobs**: Groups of steps
- **Steps**: Individual actions

For Angular:
- Build production apps
- Run tests automatically
- Deploy to hosting services
- Enforce code quality

Automate your Angular CI/CD with GitHub Actions!

---

**Next Lesson**: [Collaboration Workflows](./23_Collaboration_Workflows.md)

**Previous Lesson**: [Git Tags](./21_Git_Tags.md)
