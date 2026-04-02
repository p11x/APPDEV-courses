# Stacks, Queues, and Heaps

## What You'll Learn

- Implement stack with list
- Implement queue with deque
- Implement min-heap with heapq

## Prerequisites

- Read [02_sorting.md](./02_sorting.md) first

## Stack (LIFO)

```python
# Stack using list
stack: list[int] = []

# Push
stack.append(1)
stack.append(2)
stack.append(3)

# Pop
top = stack.pop()  # 3
```

## Queue (FIFO)

```python
from collections import deque

# Queue using deque
queue: deque[int] = deque()

# Enqueue
queue.append(1)
queue.append(2)
queue.append(3)

# Dequeue
first = queue.popleft()  # 1
```

## Priority Queue (Min-Heap)

```python
import heapq

# Min-heap
heap: list[int] = []

# Push
heapq.heappush(heap, 5)
heapq.heappush(heap, 1)
heapq.heappush(heap, 3)

# Pop (gets smallest)
smallest = heapq.heappop(heap)  # 1

# Heapify
arr = [5, 1, 3]
heapq.heapify(arr)  # [1, 5, 3]
```

## Summary

- **Stack**: list.append() / list.pop()
- **Queue**: deque.append() / deque.popleft()
- **Priority Queue**: heapq.heappush() / heapq.heappop()

## Next Steps

This concludes Data Structures. Move to **[05_OOP/01_Classes_Basics/01_classes_and_objects.md](../05_OOP/01_Classes_Basics/01_classes_and_objects.md)**
