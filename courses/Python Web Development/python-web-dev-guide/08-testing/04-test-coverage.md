# Test Coverage

## What You'll Learn
- Measuring test coverage
- Increasing coverage

## Prerequisites
- Completed testing basics

## Coverage

```bash
pip install pytest-cov
```

Run with coverage:

```bash
pytest --cov=app tests/
```

View detailed report:

```bash
pytest --cov=app --cov-report=html tests/
```

## Summary
- Use pytest-cov for coverage
- Aim for 80%+ coverage
- Focus on critical paths
