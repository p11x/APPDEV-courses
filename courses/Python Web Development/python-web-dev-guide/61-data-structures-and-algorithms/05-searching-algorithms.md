# Searching Algorithms

## What You'll Learn

- Linear search
- Binary search
- Graph search algorithms

## Prerequisites

- Completed `04-sorting-algorithms.md`

## Linear Search - O(n)

```python
def linear_search(items: list[int], target: int) -> int | None:
    """Linear search - check each element."""
    for i, item in enumerate(items):
        if item == target:
            return i
    return None
```

## Binary Search - O(log n)

Requires sorted array:

```python
def binary_search(items: list[int], target: int) -> int | None:
    """Binary search - divide and conquer."""
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

def binary_search_recursive(
    items: list[int],
    target: int,
    left: int = 0,
    right: int | None = None
) -> int | None:
    """Recursive binary search."""
    if right is None:
        right = len(items) - 1
    
    if left > right:
        return None
    
    mid = (left + right) // 2
    
    if items[mid] == target:
        return mid
    elif items[mid] < target:
        return binary_search_recursive(items, target, mid + 1, right)
    else:
        return binary_search_recursive(items, target, left, mid - 1)
```

## Binary Search with Python's bisect

```python
import bisect

def find_insert_position(items: list[int], target: int) -> int:
    """Find where to insert to maintain sorted order."""
    return bisect.bisect_left(items, target)

def find_all_positions(items: list[int], target: int) -> list[int]:
    """Find all positions where target appears."""
    pos = bisect.bisect_left(items, target)
    positions = []
    while pos < len(items) and items[pos] == target:
        positions.append(pos)
        pos += 1
    return positions
```

## Depth-First Search (DFS)

For graphs and trees:

```python
from collections import defaultdict

class Graph:
    def __init__(self):
        self._adjacency: dict[str, list[str]] = defaultdict(list)
    
    def add_edge(self, u: str, v: str) -> None:
        self._adjacency[u].append(v)
    
    def dfs(self, start: str) -> list[str]:
        """Depth-first search."""
        visited: set[str] = set()
        result: list[str] = []
        
        def _dfs(node: str) -> None:
            if node in visited:
                return
            
            visited.add(node)
            result.append(node)
            
            for neighbor in self._adjacency[node]:
                _dfs(neighbor)
        
        _dfs(start)
        return result
    
    def dfs_iterative(self, start: str) -> list[str]:
        """Iterative DFS using stack."""
        visited: set[str] = set()
        stack = [start]
        result = []
        
        while stack:
            node = stack.pop()
            if node in visited:
                continue
            
            visited.add(node)
            result.append(node)
            
            for neighbor in reversed(self._adjacency[node]):
                if neighbor not in visited:
                    stack.append(neighbor)
        
        return result
```

## Breadth-First Search (BFS)

```python
from collections import deque

class Graph:
    # ... same as above ...
    
    def bfs(self, start: str) -> list[str]:
        """Breadth-first search."""
        visited: set[str] = set([start])
        queue = deque([start])
        result = []
        
        while queue:
            node = queue.popleft()
            result.append(node)
            
            for neighbor in self._adjacency[node]:
                if neighbor not in visited:
                    visited.add(neighbor)
                    queue.append(neighbor)
        
        return result
```

## Summary

- Linear search for unsorted data
- Binary search for sorted data - O(log n)
- DFS uses stack, BFS uses queue

## Next Steps

Continue to `06-trees-and-graphs.md`.
