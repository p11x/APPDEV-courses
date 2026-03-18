# Searching Algorithms

## What You'll Learn

- Linear search
- Binary search with bisect module
- Big-O intuition

## Prerequisites

- Read [03_collections_module.md](./03_collections_module.md) first

## Linear Search

Check each element one by one:

```python
def linear_search(items: list[int], target: int) -> int:
    """Linear search - O(n)."""
    for i, item in enumerate(items):
        if item == target:
            return i
    return -1

# Example
numbers = [4, 2, 7, 1, 9, 5]
print(linear_search(numbers, 7))  # 2
```

## Binary Search

Requires sorted list - divide and conquer:

```python
def binary_search(items: list[int], target: int) -> int:
    """Binary search - O(log n)."""
    left, right = 0, len(items) - 1
    
    while left <= right:
        mid = (left + right) // 2
        if items[mid] == target:
            return mid
        elif items[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    
    return -1

# Example
sorted_numbers = [1, 3, 5, 7, 9, 11, 13]
print(binary_search(sorted_numbers, 7))  # 3
```

## Using bisect Module

```python
import bisect

sorted_list = [1, 3, 5, 7, 9]

# Find insertion point
pos = bisect.bisect_left(sorted_list, 6)
print(pos)  # 3 (where 6 would be inserted)

# Insert while maintaining order
bisect.insort(sorted_list, 6)
print(sorted_list)  # [1, 3, 5, 6, 7, 9]
```

## Big-O Intuition

| Algorithm | Time | When to Use |
|-----------|------|-------------|
| O(1) | Constant | Array access |
| O(log n) | Logarithmic | Binary search |
| O(n) | Linear | Simple search |
| O(n log n) | Linearithmic | Efficient sorting |
| O(n²) | Quadratic | Nested loops |

## Summary

- **Linear search**: O(n) - check each item
- **Binary search**: O(log n) - requires sorted list
- **bisect**: Python's binary search module

## Next Steps

Continue to **[02_sorting.md](./02_sorting.md)**
