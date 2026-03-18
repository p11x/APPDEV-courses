# GitHub Actions

## What You'll Learn
- Workflow files
- Running tests
- Deployment

## Prerequisites
- Completed CI/CD basics

## Basic Workflow

```yaml
# .github/workflows/ci.yml
name: CI

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
    
    - name: Set up Python
      uses: actions/setup-python@v4
      with:
        python-version: '3.11'
    
    - name: Install dependencies
      run: |
        pip install -r requirements.txt
    
    - name: Run tests
      run: |
        pytest
    
    - name: Upload coverage
      uses: codecov/codecov-action@v3
```

## Summary
- GitHub Actions is free for public repos
- Easy to get started
- Many pre-built actions

## Next Steps
→ Continue to `03-automated-testing.md`
