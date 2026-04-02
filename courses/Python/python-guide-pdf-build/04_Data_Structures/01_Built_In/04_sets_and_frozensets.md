# Sets and Frozensets

## What You'll Learn

- Understanding set operations (union, intersection, difference)
- Set comprehension and methods
- Using frozenset for immutable sets
- Performance characteristics of sets

## Prerequisites

- Read [03_dicts_in_depth.md](./03_dicts_in_depth.md) first

## Set Operations

Sets support mathematical set operations: union, intersection, difference, and symmetric difference.

```python
# set_operations.py

set1: set[int] = {1, 2, 3, 4}
set2: set[int] = {3, 4, 5, 6}

# Union - all elements from both sets
union: set[int] = set1 | set2
print(f"Union: {union}")

# Intersection - common elements
intersection: set[int] = set1 & set2
print(f"Intersection: {intersection}")

# Difference - elements in set1 but not in set2
difference: set[int] = set1 - set2
print(f"Difference: {difference}")

# Symmetric difference - elements in either but not both
sym_diff: set[int] = set1 ^ set2
print(f"Symmetric difference: {sym_diff}")
```

## Set Methods and Comprehensions

```python
# set_methods.py

fruits: set[str] = {"apple", "banana", "cherry"}

# Add and remove
fruits.add("date")
fruits.remove("banana")  # KeyError if not found
fruits.discard("grape")  # No error if not found

# Set comprehension
even_squares: set[int] = {x**2 for x in range(10) if x % 2 == 0}
print(even_squares)

# Check membership (O(1) operation)
has_apple: bool = "apple" in fruits
```

## Frozensets

Frozensets are immutable versions of sets - they can be used as dictionary keys or set elements.

```python
# frozenset_demo.py

# Create frozenset
immutable_set: frozenset[int] = frozenset([1, 2, 3])
print(immutable_set)

# Frozenset as dictionary key
sets_dict: dict[frozenset[int], str] = {
    frozenset([1, 2]): "set1",
    frozenset([3, 4]): "set2"
}

# Set of frozensets
set_of_frozen: set[frozenset[int]] = {
    frozenset([1, 2]),
    frozenset([3, 4])
}
```

## Annotated Full Example

```python
# sets_frozensets_demo.py
"""Complete demonstration of sets and frozensets."""

from typing import Set, FrozenSet


def find_common_elements(list1: list, list2: list) -> Set:
    """Find common elements between two lists."""
    return set(list1) & set(list2)


def main() -> None:
    # Basic set operations
    a = {1, 2, 3, 4}
    b = {3, 4, 5, 6}
    
    print(f"Set A: {a}")
    print(f"Set B: {b}")
    print(f"Union: {a | b}")
    print(f"Intersection: {a & b}")
    print(f"Difference (A-B): {a - b}")
    
    # Using frozenset
    frozen: FrozenSet[int] = frozenset([1, 2, 3])
    print(f"\nFrozenset: {frozen}")
    
    # Frozenset as dict key
    mapping: dict[FrozenSet, str] = {
        frozenset(["a", "b"]): "AB",
        frozenset(["c", "d"]): "CD"
    }
    print(f"Frozen mapping: {mapping}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding set operations
- Set comprehension and methods
- Using frozenset for immutable sets

## Next Steps

Continue to **[05_strings_as_sequences.md](./05_strings_as_sequences.md)**
