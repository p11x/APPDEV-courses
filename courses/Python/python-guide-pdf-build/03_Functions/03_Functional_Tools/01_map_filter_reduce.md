# map(), filter(), and reduce()

## What You'll Learn

- Using map() for transformations
- Using filter() for selection
- Using functools.reduce() for aggregation
- When to use comprehensions vs functional tools

## Prerequisites

- Read [03_decorators.md](../02_Advanced_Functions/03_decorators.md) first

## map() — Transform Each Item

`map()` applies a function to every item in an iterable:

```python
numbers: list[int] = [1, 2, 3, 4, 5]

# Double each number
doubled: list[int] = list(map(lambda x: x * 2, numbers))
print(doubled)  # [2, 4, 6, 8, 10]
```

## filter() — Select Items

`filter()` keeps items that satisfy a condition:

```python
numbers: list[int] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Keep only even numbers
evens: list[int] = list(filter(lambda x: x % 2 == 0, numbers))
print(evens)  # [2, 4, 6, 8, 10]
```

## reduce() — Aggregate Items

`reduce()` combines all items into a single value:

```python
from functools import reduce

numbers: list[int] = [1, 2, 3, 4, 5]

# Sum all numbers
total: int = reduce(lambda a, b: a + b, numbers)
print(total)  # 15
```

## Annotated Example

```python
# functional_demo.py
# Demonstrates map, filter, and reduce

from functools import reduce


def main() -> None:
    # --- map() ---
    print("=== map() ===")
    
    numbers: list[int] = [1, 2, 3, 4, 5]
    
    # Square each number
    squared: list[int] = list(map(lambda x: x ** 2, numbers))
    print(f"Squared: {squared}")
    
    # Convert to strings
    as_strings: list[str] = list(map(str, numbers))
    print(f"As strings: {as_strings}")
    
    # --- filter() ---
    print("\n=== filter() ===")
    
    # Keep even numbers
    evens: list[int] = list(filter(lambda x: x % 2 == 0, numbers))
    print(f"Evens: {evens}")
    
    # Keep numbers > 3
    greater_than_3: list[int] = list(filter(lambda x: x > 3, numbers))
    print(f"Greater than 3: {greater_than_3}")
    
    # --- reduce() ---
    print("\n=== reduce() ===")
    
    # Sum
    total: int = reduce(lambda a, b: a + b, numbers)
    print(f"Sum: {total}")
    
    # Product
    product: int = reduce(lambda a, b: a * b, numbers)
    print(f"Product: {product}")
    
    # Max
    max_val: int = reduce(lambda a, b: a if a > b else b, numbers)
    print(f"Max: {max_val}")
    
    # --- Combining ---
    print("\n=== Combining ===")
    
    # Filter evens, then square
    result: list[int] = list(
        map(lambda x: x ** 2, 
            filter(lambda x: x % 2 == 0, numbers))
    )
    print(f"Evens squared: {result}")
    
    # --- vs Comprehensions ---
    print("\n=== Comprehensions ===")
    
    # map equivalent
    comp_squared: list[int] = [x ** 2 for x in numbers]
    print(f"Comprehension squared: {comp_squared}")
    
    # filter equivalent
    comp_evens: list[int] = [x for x in numbers if x % 2 == 0]
    print(f"Comprehension evens: {comp_evens}")


if __name__ == "__main__":
    main()
```

## Summary

- **map()**: Transform each item
- **filter()**: Select items by condition
- **reduce()**: Aggregate into single value
- Use comprehensions when simpler

## Next Steps

Now let's continue with the remaining folders efficiently.
