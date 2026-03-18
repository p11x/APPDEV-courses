# Recursion

## What You'll Learn

- Understanding recursion
- Base cases and recursive cases
- Common recursive problems

## Prerequisites

- Completed `06-trees-and-graphs.md`

## What Is Recursion

Recursion is when a function calls itself. It breaks a problem into smaller subproblems.

Think of looking through a nested box: open the outer box, find another box inside, open that one, and so on until you find what's at the center.

## Components

Every recursive function needs:
1. **Base case** - When to stop (no more recursion)
2. **Recursive case** - Call itself with smaller problem

## Examples

### Factorial

```python
def factorial(n: int) -> int:
    """Calculate n factorial."""
    # Base case
    if n <= 1:
        return 1
    
    # Recursive case
    return n * factorial(n - 1)

# factorial(5) = 5 * factorial(4)
#               = 5 * 4 * factorial(3)
#               = 5 * 4 * 3 * factorial(2)
#               = 5 * 4 * 3 * 2 * factorial(1)
#               = 5 * 4 * 3 * 2 * 1 = 120
```

### Fibonacci

```python
def fibonacci(n: int) -> int:
    """Calculate nth Fibonacci number."""
    if n <= 1:
        return n
    
    return fibonacci(n - 1) + fibonacci(n - 2)

# Very inefficient - O(2^n)
# Use memoization for efficiency
```

### Fibonacci with Memoization

```python
from functools import lru_cache

@lru_cache(maxsize=None)
def fibonacci_memo(n: int) -> int:
    """Fibonacci with memoization - O(n)."""
    if n <= 1:
        return n
    return fibonacci_memo(n - 1) + fibonacci_memo(n - 2)
```

### Sum of List

```python
def sum_list(items: list[int]) -> int:
    """Sum all elements in a list recursively."""
    if not items:
        return 0
    
    return items[0] + sum_list(items[1:])

# sum_list([1,2,3]) = 1 + sum_list([2,3])
#                    = 1 + 2 + sum_list([3])
#                    = 1 + 2 + 3 + sum_list([])
#                    = 1 + 2 + 3 + 0 = 6
```

### Reverse String

```python
def reverse_string(s: str) -> str:
    """Reverse a string recursively."""
    if len(s) <= 1:
        return s
    
    return s[-1] + reverse_string(s[:-1])
```

### Tree Traversal

```python
from dataclasses import dataclass
from typing import Optional

@dataclass
class TreeNode:
    value: int
    left: Optional["TreeNode"] = None
    right: Optional["TreeNode"] = None

def sum_tree(node: Optional[TreeNode]) -> int:
    """Sum all values in a tree."""
    if not node:
        return 0
    
    return node.value + sum_tree(node.left) + sum_tree(node.right)
```

## Tail Recursion

```python
def factorial_tail(n: int, accumulator: int = 1) -> int:
    """Tail-recursive factorial."""
    if n <= 1:
        return accumulator
    
    return factorial_tail(n - 1, n * accumulator)
```

## When to Use Recursion

- Tree/graph traversal
- Divide and conquer algorithms
- Problems with recursive structure

## Summary

- Recursion breaks problems into smaller parts
- Always have a base case
- Consider memoization for efficiency

## Next Steps

Continue to `08-dynamic-programming.md`.
