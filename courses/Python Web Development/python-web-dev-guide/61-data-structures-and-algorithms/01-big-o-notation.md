# Big O Notation

## What You'll Learn

- Understanding algorithmic complexity
- Time and space complexity
- Common complexities

## Prerequisites

- Basic Python knowledge

## What Is Big O

Big O notation describes how the runtime of an algorithm grows as input size increases. It's crucial for writing efficient web applications.

Think of it like this: If you have 10 items, algorithm A takes 10 seconds. But if you have 100 items, does it take 100 seconds (linear) or just 1 second (logarithmic)? Big O tells you this.

## Common Complexities

| Complexity | Name | Example |
|------------|------|---------|
| O(1) | Constant | Array index access |
| O(log n) | Logarithmic | Binary search |
| O(n) | Linear | Simple loop |
| O(n log n) | Linearithmic | Merge sort |
| O(n²) | Quadratic | Nested loops |
| O(2ⁿ) | Exponential | Recursive Fibonacci |

## Examples

### O(1) - Constant

```python
def get_first_element(items: list) -> int:
    """Always takes the same time."""
    return items[0]  # Single operation regardless of size
```

### O(n) - Linear

```python
def find_max(items: list[int]) -> int:
    """Time grows with input size."""
    max_value = items[0]
    for item in items:  # Loops through all items
        if item > max_value:
            max_value = item
    return max_value
```

### O(n²) - Quadratic

```python
def find_duplicates(items: list) -> list:
    """Nested loops - time grows quadratically."""
    duplicates = []
    for i in range(len(items)):
        for j in range(i + 1, len(items)):
            if items[i] == items[j]:
                duplicates.append(items[i])
    return duplicates
```

### O(log n) - Logarithmic

```python
def binary_search(items: list[int], target: int) -> int | None:
    """Binary search - cuts problem in half each time."""
    left, right = 0, len(items) - 1
    
    while left <= right:
        mid = (left + right) // 2
        if items[mid] == target:
            return mid
        elif items[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    
    return None
```

## Why It Matters

In web development:
- Database queries - O(log n) for indexed queries vs O(n) for scans
- API response time - O(n) loops are fine for small data, bad for millions
- Sorting - O(n log n) is best you can do for comparison sorts
- Caching - Understanding complexity helps decide what to cache

## Summary

- Big O describes how runtime scales with input
- O(1) is best, O(2ⁿ) is worst
- Know the complexity of your code

## Next Steps

Continue to `02-basic-data-structures.md`.
