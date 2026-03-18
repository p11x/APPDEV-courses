# Data Model

## What You'll Learn

- Python's data model
- Dunder methods
- Customizing behavior

## Prerequisites

- Completed `06-imports-and-modules.md`

## Dunder Methods

```python
class Vector:
    def __init__(self, x: float, y: float):
        self.x = x
        self.y = y
    
    def __add__(self, other: "Vector") -> "Vector":
        return Vector(self.x + other.x, self.y + other.y)
    
    def __repr__(self) -> str:
        return f"Vector({self.x}, {self.y})"
```

## Summary

- Dunder methods customize behavior
- __init__, __repr__, __str__, etc.

## Next Steps

Continue to `08-iterators-and-generators.md`.
