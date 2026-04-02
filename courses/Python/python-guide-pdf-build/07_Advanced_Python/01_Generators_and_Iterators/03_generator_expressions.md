# Generator Expressions

## What You'll Learn

- Generator expression syntax
- Memory efficiency
- When to use generators vs list comprehensions
- Chaining generators

## Prerequisites

- Read [02_generator_functions.md](./02_generator_functions.md) first

## Generator Expression Basics

Generator expressions are like list comprehensions but return generators.

```python
# gen_expr.py

# List comprehension - creates full list
squares_list = [x**2 for x in range(10)]
print(squares_list)

# Generator expression - creates generator
squares_gen = (x**2 for x in range(10))
print(squares_gen)
print(list(squares_gen))
```

## Memory Efficiency

```python
# memory_efficiency.py

import sys

# List comprehension
large_list = [i**2 for i in range(1000000)]
print(f"List size: {sys.getsizeof(large_list)} bytes")

# Generator expression
large_gen = (i**2 for i in range(1000000))
print(f"Generator size: {sys.getsizeof(large_gen)} bytes")

# Both can iterate the same way
for i, val in enumerate(large_gen):
    if i > 5:
        break
    print(val)
```

## Annotated Full Example

```python
# generator_expressions_demo.py
"""Complete demonstration of generator expressions."""

import sys


def main() -> None:
    # Basic generator expression
    gen = (x * 2 for x in range(5))
    print(f"Generator: {list(gen)}")
    
    # With filter
    evens = (x for x in range(10) if x % 2 == 0)
    print(f"Evens: {list(evens)}")
    
    # Nested (flattened)
    nested = [[1, 2], [3, 4], [5, 6]]
    flat = (x for sublist in nested for x in sublist)
    print(f"Flattened: {list(flat)}")
    
    # Memory comparison
    list_size = sys.getsizeof([x**2 for x in range(10000)])
    gen_size = sys.getsizeof((x**2 for x in range(10000)))
    print(f"\nList: {list_size} bytes")
    print(f"Generator: {gen_size} bytes")


if __name__ == "__main__":
    main()
```

## Summary

- Generator expression syntax
- Memory efficiency
- When to use generators vs list comprehensions

## Next Steps

Continue to **[04_yield_from.md](./04_yield_from.md)**
