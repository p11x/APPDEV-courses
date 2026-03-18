# Tuples and Sets

## What You'll Learn

- Tuple immutability and use cases
- Named tuples
- Sets: union, intersection, difference
- Frozenset

## Prerequisites

- Read [01_lists.md](./01_lists.md) first

## Tuples

Tuples are **immutable** ordered collections:

```python
# Create tuple
point: tuple[int, int] = (10, 20)
coordinates: tuple[float, float, float] = (1.5, 2.5, 3.5)

# Tuple unpacking
x, y = point  # x=10, y=20

# Named tuple
from collections import namedtuple
Point = namedtuple("Point", ["x", "y"])
p = Point(10, 20)
print(p.x, p.y)
```

## Sets

Sets are **unordered, unique** collections:

```python
# Create set
fruits: set[str] = {"apple", "banana", "cherry"}

# Add/remove
fruits.add("orange")
fruits.remove("banana")

# Set operations
a: set = {1, 2, 3}
b: set = {2, 3, 4}

union: set = a | b          # {1, 2, 3, 4}
intersection: set = a & b   # {2, 3}
difference: set = a - b     # {1}
symmetric_diff: set = a ^ b # {1, 4}

# Frozenset (immutable)
frozen: frozenset = frozenset([1, 2, 3])
```

## Summary

- **Tuple**: Immutable, ordered, indexed
- **Named tuple**: Tuple with named fields
- **Set**: Unordered, unique values
- **Frozenset**: Immutable set

## Next Steps

Continue to **[03_dictionaries.md](./03_dictionaries.md)**
