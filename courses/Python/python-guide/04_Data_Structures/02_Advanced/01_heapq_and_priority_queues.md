# Heapq and Priority Queues

## What You'll Learn

- Understanding the heap invariant
- Using heapq for min-heap operations
- Finding nlargest and nsmallest efficiently
- Implementing priority queues

## Prerequisites

- Read [07_deque_and_defaultdict.md](../01_Built_In/07_deque_and_defaultdict.md) first

## Understanding Heaps

A heap is a binary tree where parent nodes are always smaller (min-heap) or larger (max-heap) than their children.

```python
# heap_basics.py

import heapq

# Create a min-heap
heap: list[int] = []
heapq.heappush(heap, 5)
heapq.heappush(heap, 3)
heapq.heappush(heap, 7)
heapq.heappush(heap, 1)

# heap is now [1, 3, 7, 5]
print(f"Heap: {heap}")

# Pop smallest
smallest = heapq.heappop(heap)  # 1
print(f"Popped: {smallest}, Heap: {heap}")
```

## Using heapq Functions

```python
# heapq_functions.py

import heapq

# heapify - convert list to heap in place
data = [5, 3, 7, 1, 9, 2]
heapq.heapify(data)
print(f"Heapified: {data}")

# nlargest and nsmallest
numbers = [1, 5, 2, 8, 3, 9, 1, 4]
print(f"3 largest: {heapq.nlargest(3, numbers)}")
print(f"3 smallest: {heapq.nsmallest(3, numbers)}")

# Merge sorted iterables
list1 = [1, 4, 7]
list2 = [2, 5, 8]
list3 = [3, 6, 9]
merged = list(heapq.merge(list1, list2, list3))
print(f"Merged: {merged}")
```

## Implementing Priority Queue

```python
# priority_queue.py

import heapq
from dataclasses import dataclass, field
from typing import Any


@dataclass(order=True)
class PriorityItem:
    priority: int
    item: Any = field(compare=False)


class PriorityQueue:
    def __init__(self) -> None:
        self._heap: list[PriorityItem] = []
    
    def push(self, priority: int, item: Any) -> None:
        heapq.heappush(self._heap, PriorityItem(priority, item))
    
    def pop(self) -> Any:
        return heapq.heappop(self._heap).item
    
    def is_empty(self) -> bool:
        return len(self._heap) == 0
```

## Annotated Full Example

```python
# heapq_demo.py
"""Complete demonstration of heapq and priority queues."""

import heapq
from dataclasses import dataclass


def main() -> None:
    # Basic heap operations
    heap = []
    for num in [5, 3, 7, 1, 9]:
        heapq.heappush(heap, num)
    print(f"Heap: {heap}")
    
    # Pop elements in order
    print("Popping:")
    while heap:
        print(heapq.heappop(heap), end=" ")
    print()
    
    # Using nlargest/nsmallest
    numbers = [1, 5, 2, 8, 3, 9, 1, 4]
    print(f"\n3 largest: {heapq.nlargest(3, numbers)}")
    print(f"3 smallest: {heapq.nsmallest(3, numbers)}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding the heap invariant
- Using heapq for min-heap operations
- Finding nlargest and nsmallest efficiently

## Next Steps

Continue to **[02_linked_lists.md](./02_linked_lists.md)**
