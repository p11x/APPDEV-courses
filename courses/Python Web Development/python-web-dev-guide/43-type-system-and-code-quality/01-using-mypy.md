# Using MyPy

## What You'll Learn
- Type checking fundamentals
- Configuring mypy
- Common type patterns
- Integration with CI/CD

## Prerequisites
- Python type hints knowledge

## Installation

```bash
pip install mypy
```

## Basic Usage

```bash
mypy your_module.py
mypy your_package/
```

## Configuration

```toml
# pyproject.toml
[tool.mypy]
python_version = "3.11"
warn_return_any = true
warn_unused_configs = true
disallow_untyped_defs = true
disallow_incomplete_defs = true

[[tool.mypy.overrides]]
module = "numpy.*"
ignore_missing_imports = true
```

## Type Patterns

```python
from typing import Union, Optional, List, Dict

def process(data: Dict[str, Optional[int]]) -> List[str]:
    """Process dictionary with optional integers."""
    result = []
    for key, value in data.items():
        if value is not None:
            result.append(f"{key}: {value * 2}")
    return result
```

## Summary

- MyPy provides static type checking
- Configure in pyproject.toml or mypy.ini
- Use strict mode for better type safety
- Add to CI/CD for automatic checking
