# Automated Testing in CI

## What You'll Learn
- Running tests in CI
- Test matrices
- Coverage reports

## Prerequisites
- Completed GitHub Actions

## Test Matrix

```yaml
jobs:
  test:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        python-version: ['3.10', '3.11', '3.12']
        django-version: ['4.2', '5.0']
    steps:
    - uses: actions/checkout@v3
    - name: Set up Python ${{ matrix.python-version }}
      uses: actions/setup-python@v4
      with:
        python-version: ${{ matrix.python-version }}
    - name: Install dependencies
      run: |
        pip install pytest
        pip install django==${{ matrix.django-version }}
    - name: Run tests
      run: pytest
```

## Summary
- Test on multiple Python versions
- Generate coverage reports
- Block PRs on failures

## Next Steps
→ Continue to `04-deployment-strategies.md`
