# Packaging Basics

## What You'll Learn

- Understanding Python packages
- Creating your first package
- Using pyproject.toml

## Prerequisites

- Basic Python knowledge

## What Is a Package

A Python package is a directory containing Python modules with an `__init__.py` file. It allows you to organize code and share it with others.

```
my_package/
├── __init__.py
├── module1.py
└── module2.py
```

## Modern Package Structure

```toml
# pyproject.toml
[build-system]
requires = ["setuptools>=61.0", "wheel"]
build-backend = "setuptools.build_meta"

[project]
name = "my-package"
version = "0.1.0"
description = "A short description"
readme = "README.md"
license = {text = "MIT"}
requires-python = ">=3.11"
authors = [
    {name = "Your Name", email = "you@example.com"}
]
dependencies = [
    "requests>=2.28.0",
    "click>=8.0.0",
]

[project.optional-dependencies]
dev = [
    "pytest>=7.0.0",
    "black>=23.0.0",
]

[project.scripts]
my-package = "my_package.cli:main"
```

## Creating Package Files

```python
# my_package/__init__.py
"""My package description."""

__version__ = "0.1.0"

from .module1 import function1
from .module2 import function2

__all__ = ["function1", "function2", "__version__"]
```

```python
# my_package/module1.py
"""Module 1 functionality."""

def function1(param: str) -> str:
    """Process the parameter."""
    return f"Processed: {param}"
```

## Installing and Testing

```bash
# Install in development mode
pip install -e .

# Install with dev dependencies
pip install -e ".[dev]"

# Build the package
python -m build

# Upload to PyPI
twine upload dist/*
```

## Summary

- Use pyproject.toml for modern packaging
- Structure packages with clear modules
- Test locally before publishing

## Next Steps

Continue to `02-setup-py-and-pyproject.md`.
