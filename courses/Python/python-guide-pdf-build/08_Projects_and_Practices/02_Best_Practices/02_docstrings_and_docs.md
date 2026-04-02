# Docstrings

## What You'll Learn

- Google style
- NumPy style
- reStructuredText
- __doc__ attribute

## Prerequisites

- Read [01_pep8_and_style.md](./01_pep8_and_style.md) first

## Google Style

```python
def func(arg1, arg2):
    """Summary line.
    
    Args:
        arg1: Description
        arg2: Description
    
    Returns:
        Description
    """
    pass
```

## NumPy Style

```python
def func(arg1, arg2):
    """
    Summary line.
    
    Parameters
    ----------
    arg1 : type
        Description
    """
    pass
```

## Summary

- **Docstring**: Documentation for functions/classes
- **Styles**: Google, NumPy, reStructuredText

## Next Steps

Continue to **[03_logging_and_debugging.md](./03_logging_and_debugging.md)**
