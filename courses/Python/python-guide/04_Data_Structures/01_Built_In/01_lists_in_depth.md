# Lists In Depth

## What You'll Learn

- Creating lists with various methods
- Understanding indexing and slicing
- Mastering all list methods
- Using lists as stacks and queues

## Prerequisites

- Read [07_functional_patterns.md](../../03_Functions/03_Functional_Tools/07_functional_patterns.md) first

## Creating Lists

Lists can be created in multiple ways in Python.

```python
# list_creation.py

# Empty list
empty: list = []

# With initial values
fruits: list[str] = ["apple", "banana", "cherry"]

# List from iterable
numbers: list[int] = list(range(10))

# List comprehension
squares: list[int] = [x ** 2 for x in range(10)]

# Type-hinted list
matrix: list[list[int]] = [[1, 2], [3, 4]]
```

## Indexing and Slicing

Python lists support powerful indexing and slicing operations.

```python
# indexing_slicing.py

numbers: list[int] = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]

# Positive indexing (0-based)
first: int = numbers[0]   # 0
last: int = numbers[-1]   # 9

# Slicing [start:end:step]
subset: list[int] = numbers[2:7]    # [2, 3, 4, 5, 6]
first_three: list[int] = numbers[:3]  # [0, 1, 2]
last_three: list[int] = numbers[-3:]   # [7, 8, 9]

# With step
evens: list[int] = numbers[::2]     # [0, 2, 4, 6, 8]
reversed_list: list[int] = numbers[::-1]  # [9, 8, 7, 6, 5, 4, 3, 2, 1, 0]
```

## List Methods

Python lists have many built-in methods for manipulation.

```python
# list_methods.py

my_list: list[int] = [3, 1, 4, 1, 5, 9, 2, 6]

# Adding elements
my_list.append(5)         # Add to end
my_list.insert(0, 0)       # Insert at index
my_list.extend([7, 8])     # Add multiple

# Removing elements
my_list.pop()             # Remove and return last
my_list.remove(1)         # Remove first occurrence of 1

# Finding elements
index: int = my_list.index(5)  # Index of first 5
count: int = my_list.count(5)  # Count of 5s

# Sorting
my_list.sort()             # In-place sort
sorted_list: list[int] = sorted(my_list)  # New sorted list
```

## Annotated Full Example

```python
# lists_in_depth_demo.py
"""Comprehensive demonstration of list operations."""

from typing import List


def process_shopping_cart(items: List[str], prices: List[float]) -> dict:
    """Process shopping cart with list operations."""
    # Sort by price descending
    combined = list(zip(items, prices))
    combined.sort(key=lambda x: x[1], reverse=True)
    
    # Get top 3 items
    top_3 = combined[:3]
    
    # Calculate total
    total = sum(prices)
    
    return {
        "items": items,
        "prices": prices,
        "top_3_by_price": top_3,
        "total": total,
        "item_count": len(items)
    }


def main() -> None:
    cart = ["apple", "banana", "cherry", "date"]
    prices = [0.50, 0.25, 2.00, 1.50]
    
    result = process_shopping_cart(cart, prices)
    print(f"Cart: {result['items']}")
    print(f"Prices: {result['prices']}")
    print(f"Top 3: {result['top_3_by_price']}")
    print(f"Total: ${result['total']:.2f}")
    print(f"Items: {result['item_count']}")


if __name__ == "__main__":
    main()
```

## Summary

- Creating lists with various methods
- Understanding indexing and slicing
- Mastering all list methods

## Next Steps

Continue to **[02_tuples_and_namedtuples.md](./02_tuples_and_namedtuples.md)**
