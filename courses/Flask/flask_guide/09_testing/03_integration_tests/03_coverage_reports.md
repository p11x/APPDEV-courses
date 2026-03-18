<!-- FILE: 09_testing/03_integration_tests/03_coverage_reports.md -->

## Overview

Test coverage measures how much of your code is executed by your test suite. This file covers generating coverage reports with pytest-cov, interpreting the results, and improving test coverage.

## Prerequisites

- pytest installed
- Basic testing setup

## Core Concepts

### What is Coverage?

Coverage percentage = (Lines executed by tests) / (Total lines of code) * 100

### Installation

```bash
pip install pytest-cov
```

## Code Walkthrough

### Generating Coverage Reports

```bash
# Run tests with coverage
pytest --cov=app

# Generate HTML report
pytest --cov=app --cov-report=html

# Generate XML report (for CI/CD)
pytest --cov=app --cov-report=xml

# Show missing lines
pytest --cov=app --cov-report=term-missing
```

### Coverage Report Example

```bash
$ pytest --cov=app --cov-report=term-missing
============================= test session starts ==============================
collected 5 items

tests/test_routes.py ..                                                  [ 40%]
tests/test_models.py ...                                                 [ 100%]

---------- coverage: platform linux, python 3.12.0-final-0 ----------
Name                    Stmts   Miss  Cover
--------------------------------------------------
app/__init__.py            10      0   100%
app/models.py              50      8    84%
app/routes.py              30      0   100%
--------------------------------------------------
TOTAL                     90      8    91%
```

### Configuring Coverage in pyproject.toml or setup.cfg

```toml
# pyproject.toml
[tool.coverage.run]
source = ["app"]
omit = [
    "app/__init__.py",
    "*/migrations/*"
]

[tool.coverage.report]
exclude_lines = [
    "pragma: no cover",
    "def __repr__",
    "if self\.debug",
    "if settings\.DEBUG",
    "if settings\.TESTING",
]
```

### Interpreting Coverage Results

- **Statements (Stmts)**: Number of executable lines
- **Miss**: Number of lines not executed by tests
- **Cover**: Percentage of lines covered

### What Constitutes Good Coverage?

- 80%+ is generally considered good for most projects
- 90%+ is excellent
- 100% is often unnecessary and can lead to brittle tests

### Strategies to Improve Coverage

1. **Test edge cases** — Test boundary conditions and error paths
2. **Test exception handling** — Force exceptions to test catch blocks
3. **Test all branches** — Ensure both sides of if/else are tested
4. **Test loops** — Test zero, one, and multiple iterations
5. **Test private methods indirectly** — Through public methods that use them

## Common Mistakes

❌ **Aiming for 100% coverage at all costs**
```python
# WRONG — Testing trivial getters/setters just to increase coverage
def test_get_id(self):
    assert user.get_id() == 1  # Adds little value
```

✅ **Correct — Focus on meaningful tests**
```python
# CORRECT — Test behavior, not just lines
def test_user_can_login(self):
    # Test the actual login process
    response = client.post('/login', data={'username': 'test', 'password': 'secret'})
    assert response.status_code == 200
```

� **Ignoring complex logic**
```python
# WRONG — Skipping hard-to-test functions
# Instead, refactor to make them testable
```

✅ **Correct — Refactor for testability**
```python
# CORRECT — Break down complex functions into smaller, testable units
```

## Quick Reference

| Command | Description |
|---------|-------------|
| `pytest --cov=app` | Generate coverage report |
| `pytest --cov=app --cov-report=html` | Generate HTML report |
| `pytest --cov=app --cov-report=term-missing` | Show missing lines |
| `pytest --cov=app --cov-fail-under=80` | Fail if coverage < 80% |

## Next Steps

You have completed the testing chapter. Continue to [01_environment_variables.md](../10_deployment/01_production_config/01_environment_variables.md) to learn about production configuration.