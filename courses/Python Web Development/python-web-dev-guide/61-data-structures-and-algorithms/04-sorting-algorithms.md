# Sorting Algorithms

## What You'll Learn

- Common sorting algorithms
- When to use each
- Python's sorted function

## Prerequisites

- Completed `03-hash-tables-and-dictionaries.md`

## Bubble Sort - O(n²)

Simple but inefficient:

```python
def bubble_sort(items: list[int]) -> list[int]:
    """Bubble sort implementation."""
    result = items.copy()
    n = len(result)
    
    for i in range(n):
        swapped = False
        for j in range(0, n - i - 1):
            if result[j] > result[j + 1]:
                result[j], result[j + 1] = result[j + 1], result[j]
                swapped = True
        if not swapped:
            break
    
    return result
```

## Quick Sort - O(n log n) average

Efficient divide and conquer:

```python
def quick_sort(items: list[int]) -> list[int]:
    """Quick sort implementation."""
    if len(items) <= 1:
        return items.copy()
    
    pivot = items[len(items) // 2]
    left = [x for x in items if x < pivot]
    middle = [x for x in items if x == pivot]
    right = [x for x in items if x > pivot]
    
    return quick_sort(left) + middle + quick_sort(right)
```

## Merge Sort - O(n log n) guaranteed

Stable and consistent:

```python
def merge_sort(items: list[int]) -> list[int]:
    """Merge sort implementation."""
    if len(items) <= 1:
        return items
    
    mid = len(items) // 2
    left = merge_sort(items[:mid])
    right = merge_sort(items[mid:])
    
    return merge(left, right)

def merge(left: list[int], right: list[int]) -> list[int]:
    """Merge two sorted lists."""
    result = []
    i = j = 0
    
    while i < len(left) and j < len(right):
        if left[i] <= right[j]:
            result.append(left[i])
            i += 1
        else:
            result.append(right[j])
            j += 1
    
    result.extend(left[i:])
    result.extend(right[j:])
    return result
```

## Python's sorted

In practice, use Python's built-in:

```python
# Simple sorting
numbers = [5, 2, 8, 1, 9]
sorted_numbers = sorted(numbers)  # Returns new list
numbers.sort()  # Sorts in place

# With key
users = [{"name": "Bob", "age": 30}, {"name": "Alice", "age": 25}]
by_name = sorted(users, key=lambda x: x["name"])
by_age = sorted(users, key=lambda x: x["age"])

# Reverse
descending = sorted(numbers, reverse=True)

# Stable sort
# Python's sort is stable - maintains relative order of equal elements
```

## When to Use What

| Algorithm | Time | Space | Best For |
|-----------|------|-------|----------|
| Bubble | O(n²) | O(1) | Educational |
| Quick | O(n log n) | O(log n) | General purpose |
| Merge | O(n log n) | O(n) | Stability needed |
| Timsort | O(n log n) | O(n) | Python's default |

## Summary

- Quick sort and merge sort are efficient for general use
- Python's sorted uses Timsort
- Use built-in functions in practice

## Next Steps

Continue to `05-searching-algorithms.md`.
