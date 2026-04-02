# Test Coverage

## Overview

Test coverage measures how much code is exercised by tests.

## Coverage Setup

### Configuration

```python
# Example 1: Coverage configuration
# pyproject.toml
"""
[tool.coverage.run]
source = ["app"]
omit = ["tests/*", "*/migrations/*"]
branch = true

[tool.coverage.report]
precision = 2
show_missing = true
fail_under = 80
"""

# Run coverage
# pytest --cov=app --cov-report=html --cov-report=term
```

### Coverage Reports

```python
# Example 2: Reading coverage reports
"""
Coverage Report:

Name                    Stmts   Miss Branch BrPart   Cover
----------------------------------------------------------
app/main.py               20      2      4      1  87.50%
app/routers/users.py      45      5     10      2  85.45%
app/services/user.py      60      8     15      3  85.33%
----------------------------------------------------------
TOTAL                    125     15     29      6  86.15%
"""
```

## Coverage Targets

| Code Type | Target |
|-----------|--------|
| Critical | 95%+ |
| Core Logic | 90%+ |
| API Endpoints | 85%+ |
| Infrastructure | 70%+ |

## Summary

Test coverage helps identify untested code.

## Next Steps

Continue learning about:
- [Test Parameterization](./07_test_parameterization.md)
- [Test Assertions](./08_test_assertions.md)
