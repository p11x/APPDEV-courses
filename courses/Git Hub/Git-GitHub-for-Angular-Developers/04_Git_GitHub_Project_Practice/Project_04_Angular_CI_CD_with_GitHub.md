# Project 4: Angular CI/CD with GitHub Actions

## Project Overview

This project teaches continuous integration and deployment (CI/CD) for Angular applications using GitHub Actions. Students will automate testing, building, and deployment.

### Learning Objectives

- Create GitHub Actions workflows
- Set up automated testing
- Configure production builds
- Deploy to GitHub Pages
- Use secrets and environment variables

### Prerequisites

- GitHub account
- Completed Projects 1-3

---

## Step 1: Create Workflow Directory

### Task 1.1: Create Directory Structure

```bash
# Create .github/workflows directory
mkdir -p .github/workflows

# Verify structure
ls -la .github/
```

---

## Step 2: Create Basic Build Workflow

### Task 2.1: Create CI Workflow

Create `.github/workflows/angular-ci.yml`:

```yaml
name: Angular CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

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

    - name: Lint
      run: npm run lint

    - name: Build
      run: npm run build -- --configuration production
```

### Task 2.2: Test Workflow

```bash
# Commit and push
git add .
git commit -m "ci: add GitHub Actions workflow"
git push origin main
```

On GitHub:
1. Go to "Actions" tab
2. See workflow running
3. View build logs

---

## Step 3: Add Testing to Workflow

### Task 3.1: Update Workflow with Tests

Update `.github/workflows/angular-ci.yml`:

```yaml
name: Angular CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

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

    - name: Install dependencies
      run: npm ci

    - name: Lint
      run: npm run lint

    - name: Run tests
      run: npm test -- --no-watch --browsers=ChromeHeadless

    - name: Build
      run: npm run build -- --configuration production
```

### Task 3.2: Add Chrome Setup

```yaml
    - name: Setup Chrome
      uses: browser-actions/setup-chrome@latest
      with:
        chrome-version: stable
```

---

## Step 4: Deploy to GitHub Pages

### Task 4.1: Create Deploy Workflow

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy to GitHub Pages

on:
  push:
    branches: [main]

permissions:
  contents: read
  pages: write
  id-token: write

concurrency:
  group: "pages"
  cancel-in-progress: false

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout
        uses: actions/checkout@v3

      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '18'
          cache: 'npm'

      - name: Install
        run: npm ci

      - name: Build
        run: npm run build -- --configuration production --base-href "/${{ github.repository }}/"

      - name: Upload artifact
        uses: actions/upload-pages-artifact@v1
        with:
          path: dist/angular-ci-demo

  deploy:
    needs: build
    runs-on: ubuntu-latest
    environment:
      name: github-pages
      url: ${{ steps.deployment.outputs.page_url }}
    steps:
      - name: Deploy to GitHub Pages
        id: deployment
        uses: actions/deploy-pages@v1
```

### Task 4.2: Enable GitHub Pages

On GitHub:
1. Go to Settings → Pages
2. Select "Deploy from a branch"
3. Select branch: "gh-pages"
4. Save

---

## Step 5: Use Secrets

### Task 5.1: Add Firebase Secrets

1. Go to Repository Settings → Secrets → Actions
2. Add secrets:
   - `FIREBASE_TOKEN`: Your Firebase token
   - `API_KEY`: Your API key

### Task 5.2: Create Firebase Deploy Workflow

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
          cache: 'npm'

      - name: Install
        run: npm ci

      - name: Build
        run: npm run build -- --configuration production

      - name: Deploy to Firebase
        uses: w9jds/firebase-action@v2
        with:
          args: deploy --only hosting
        env:
          FIREBASE_TOKEN: ${{ secrets.FIREBASE_TOKEN }}
```

---

## Step 6: Matrix Builds

### Task 6.1: Test Multiple Environments

Create `.github/workflows/matrix.yml`:

```yaml
name: Matrix Build

on: [push, pull_request]

jobs:
  build:
    strategy:
      matrix:
        node-version: [16, 18, 20]
        Angular-version: [15, 16, 17]
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Use Node.js ${{ matrix.node-version }}
      uses: actions/setup-node@v3
      with:
        node-version: ${{ matrix.node-version }}
    
    - name: Install
      run: npm ci
    
    - name: Build
      run: npm run build
```

---

## Step 7: Branch Protection with Status Checks

### Task 7.1: Configure Protected Branch

On GitHub:
1. Go to Settings → Branches
2. Add rule for "main"
3. Require status checks to pass before merging
4. Select your workflow as required check

---

## Exercise Requirements

### Exercise 1: Basic CI

- [ ] Create CI workflow
- [ ] Push to trigger workflow
- [ ] View workflow run
- [ ] Verify build passes

### Exercise 2: Add Testing

- [ ] Add test step to workflow
- [ ] Verify tests run on push

### Exercise 3: Deploy

- [ ] Configure GitHub Pages
- [ ] Create deploy workflow
- [ ] Deploy on push

### Exercise 4: Advanced

- [ ] Add secrets
- [ ] Create matrix build
- [ ] Configure branch protection

---

## Assessment Criteria

| Criteria | Points |
|----------|--------|
| Workflow created | 15 |
| CI passes on push | 15 |
| Testing included | 20 |
| Deployment configured | 20 |
| Secrets used | 10 |
| Branch protection | 10 |
| Documentation | 10 |

---

## Common CI/CD Patterns

### Cache Dependencies

```yaml
- name: Cache npm
  uses: actions/cache@v3
  with:
    path: ~/.npm
    key: ${{ runner.os }}-npm-${{ hashFiles('**/package-lock.json') }}
```

### Fail Fast

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

---

## Summary

This project teaches:

- GitHub Actions basics
- CI workflow creation
- Automated testing
- GitHub Pages deployment
- Using secrets
- Matrix builds
- Branch protection

Your Angular app now has automated CI/CD!

---

**Course Complete!** 

You have completed the Git & GitHub learning curriculum for Angular developers!

---

**Previous Project**: [GitHub Project Management](./Project_03_GitHub_Project_Management.md)
