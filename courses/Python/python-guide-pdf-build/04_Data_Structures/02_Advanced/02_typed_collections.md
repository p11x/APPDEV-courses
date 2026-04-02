# Typed Collections

## What You'll Learn

- Built-in generic syntax (Python 3.9+)
- list[int], dict[str, int], tuple[int, ...], set[str]
- TypeAlias (Python 3.10+)
- TypeVar basics

## Prerequisites

- Read [01_dataclasses.md](./01_dataclasses.md) first

## Built-in Generics (Python 3.9+)

```python
# Instead of typing.List, typing.Dict, etc.
# Use built-in generics directly

# List of integers
numbers: list[int] = [1, 2, 3]

# Dictionary with string keys and int values
scores: dict[str, int] = {"Alice": 95, "Bob": 87}

# Set of strings
unique_names: set[str] = {"Alice", "Bob"}

# Tuple of specific length
point: tuple[int, int] = (10, 20)

# Tuple of any length (ellipsis)
coords: tuple[int, ...] = (1, 2, 3, 4, 5)
```

## TypeAlias

Create meaningful type names:

```python
# Python 3.10+
type Matrix = list[list[int]]
type UserDict = dict[str, int]

# Use them
def process_matrix(matrix: Matrix) -> None:
    ...

# Alternative: using TypeAlias (more explicit)
from typing import TypeAlias

UserDict: TypeAlias = dict[str, int]
```

## TypeVar

Create generic types:

```python
from typing import TypeVar

T = TypeVar('T')

def first(items: list[T]) -> T | None:
    return items[0] if items else None

# Works with any type
result: int | None = first([1, 2, 3])
result: str | None = first(["a", "b", "c"])
```

## Summary

- Use built-in generics: `list[int]`, `dict[str, int]`
- `TypeAlias` for meaningful type names
- `TypeVar` for generic functions

## Next Steps

Continue to **[03_collections_module.md](./03_collections_module.md)**
