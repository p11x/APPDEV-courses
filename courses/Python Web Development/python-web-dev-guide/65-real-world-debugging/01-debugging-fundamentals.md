# Debugging Fundamentals

## What You'll Learn

- Understanding bugs
- Debugging tools
- Systematic debugging approach

## Prerequisites

- Basic Python knowledge

## What Are Bugs

Bugs are errors in code that cause unexpected behavior. They can be syntax errors, runtime errors, or logic errors.

Think of debugging like being a detective - you need to find clues and piece together what went wrong.

## Debugging Tools

```python
# Using print for debugging
def divide(a: int, b: int) -> float:
    print(f"Dividing {a} by {b}")  # Debug print
    result = a / b
    print(f"Result: {result}")  # Debug print
    return result
```

## Using pdb

```python
import pdb

def buggy_function(x: int) -> int:
    result = x * 2
    pdb.set_trace()  # Breakpoint
    return result + 10
```

## Summary

- Debug systematically
- Use tools like pdb
- Start with simple checks

## Next Steps

Continue to `02-using-debugger.md`.
