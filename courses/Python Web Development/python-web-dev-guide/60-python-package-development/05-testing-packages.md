# Testing Packages

## What You'll Learn

- Testing strategies for packages
- Using tox for multiple environments
- CI/CD for packages

## Prerequisites

- Completed `04-versioning-and-releases.md`

## Package Testing

```python
# tests/test_package.py
import pytest
from my_package import __version__, function1

def test_version():
    """Test version is defined."""
    assert __version__ is not None

def test_function1():
    """Test function1 returns expected output."""
    result = function1("test")
    assert result == "Processed: test"

def test_function1_empty():
    """Test function1 with empty string."""
    result = function1("")
    assert result == "Processed: "
```

## Tox Configuration

```toml
# tox.ini
[tox]
envlist = py311,py312

[testenv]
deps =
    pytest
    pytest-cov
commands =
    pytest {posargs:tests/}

[testenv:lint]
deps =
    black
    flake8
    mypy
commands =
    black --check src/ tests/
    flake8 src/ tests/
    mypy src/

[testenv:build]
deps =
    build
commands =
    python -m build
```

## Running Tox

```bash
# Install tox
pip install tox

# Run all environments
tox

# Run specific environment
tox -e py311

# Run with coverage
tox -e py311 -- --cov=my_package
```

## CI/CD with GitHub Actions

```yaml
# .github/workflows/test.yml
name: Test

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        python-version: ['3.11', '3.12']
    
    steps:
      - uses: actions/checkout@v4
      - name: Set up Python ${{ matrix.python-version }}
        uses: actions/setup-python@v5
        with:
          python-version: ${{ matrix.python-version }}
      - name: Install dependencies
        run: |
          pip install -e ".[dev]"
      - name: Run tests
        run: pytest
      - name: Run linting
        run: |
          black --check src/
          flake8 src/
```

## Test Coverage

```toml
# pyproject.toml
[tool.coverage.run]
source = ["my_package"]
omit = ["*/tests/*"]

[tool.coverage.report]
exclude_lines = [
    "pragma: no cover",
    "def __repr__",
    "raise AssertionError",
]
```

## Summary

- Test across Python versions
- Use tox for automation
- Add CI/CD for quality

## Next Steps

Continue to `06-documentation.md`.
