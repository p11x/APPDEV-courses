# Creating Modules

## What You'll Learn

- Writing a module from scratch
- __all__ list
- Module docstrings
- sys.path

## Prerequisites

- Read [01_importing.md](./01_importing.md) first

## Creating a Module

```python
# mymodule.py
"""My module docstring."""

def add(a: int, b: int) -> int:
    """Add two numbers."""
    return a + b


__all__ = ["add"]  # Public API
```

## Using the Module

```python
import mymodule
print(mymodule.add(5, 3))  # 8
```

## sys.path

```python
import sys
sys.path.append("/path/to/module")
import mymodule
```

## Summary

- Module is just a .py file
- Use __all__ to define public API
- sys.path determines where Python looks

## Next Steps

Continue to **[03_standard_library_tour.md](./03_standard_library_tour.md)**
