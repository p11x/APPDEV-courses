# Example183.py
# Topic: Priority Queue / heapq Basics

# This file demonstrates heapq module for priority queue implementation.
# heapq implements a min-heap (smallest element at front).
# Used for priority scheduling, Dijkstra's algorithm, etc.


# ============================================================
# Example 1: Basic Heap Operations
# ============================================================
print("=== Basic Heap Operations ===")

import heapq

heap = []    # list — heap root

# Push elements
heapq.heappush(heap, 5)    # list — add 5 to heap
heapq.heappush(heap, 1)    # list — add 1 to heap
heapq.heappush(heap, 3)    # list — add 3 to heap
print(f"Heap: {heap}")    # Heap: [1, 5, 3]

# Pop smallest
smallest = heapq.heappop(heap)    # int — smallest element
print(f"Popped: {smallest}")    # Popped: 1
print(f"Heap after pop: {heap}")    # Heap after pop: [3, 5]


# ============================================================
# Example 2: heapify - Convert List to Heap
# ============================================================
print("\n=== heapify ===")

# Convert existing list to heap in place
arr = [5, 1, 3, 7, 2, 8, 4]
heapq.heapify(arr)    # list — now a valid heap
print(f"Heapified: {arr}")    # Heapified: [1, 2, 3, 5, 7, 8, 4]

# Pop all elements (sorted!)
sorted_list = []
while arr:
    sorted_list.append(heapq.heappop(arr))    # list — pop all
print(f"Sorted: {sorted_list}")    # Sorted: [1, 2, 3, 4, 5, 7, 8]


# ============================================================
# Example 3: Priority Queue Implementation
# ============================================================
print("\n=== Priority Queue ===")

import heapq
from dataclasses import dataclass, field
from typing import Any

@dataclass(order=True)
class PrioritizedItem:
    priority: int
    item: Any = field(compare=False)

class PriorityQueue:
    def __init__(self):
        self._heap = []
    
    def push(self, item: Any, priority: int = 0):
        heapq.heappush(self._heap, PrioritizedItem(priority, item))
    
    def pop(self) -> Any:
        return heapq.heappop(self._heap).item
    
    def peek(self) -> Any:
        return self._heap[0].item
    
    def is_empty(self) -> bool:
        return len(self._heap) == 0
    
    def size(self) -> int:
        return len(self._heap)

pq = PriorityQueue()
pq.push("task1", priority=3)
pq.push("task2", priority=1)
pq.push("task3", priority=2)

print(f"Size: {pq.size()}")    # Size: 3
print(f"Next: {pq.pop()}")    # Next: task2 (priority 1)
print(f"Next: {pq.pop()}")    # Next: task3 (priority 2)
print(f"Next: {pq.pop()}")    # Next: task1 (priority 3)


# ============================================================
# Example 4: Largest/Smallest Elements
# ============================================================
print("\n=== nlargest / nsmallest ===")

data = [1, 8, 3, 6, 2, 9, 4, 7, 5]

# Get 3 largest
largest_3 = heapq.nlargest(3, data)    # list — 3 largest
print(f"3 largest: {largest_3}")    # 3 largest: [9, 8, 7]

# Get 3 smallest
smallest_3 = heapq.nsmallest(3, data)    # list — 3 smallest
print(f"3 smallest: {smallest_3}")    # 3 smallest: [1, 2, 3]


# ============================================================
# Example 5: Merge Sorted Sequences
# ============================================================
print("\n=== heapq.merge ===")

list1 = [1, 4, 7, 10]
list2 = [2, 5, 8, 11]
list3 = [3, 6, 9, 12]

# Merge multiple sorted iterables
merged = list(heapq.merge(list1, list2, list3))    # list — merged
print(f"Merged: {merged}")    # Merged: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]


# ============================================================
# Example 6: Task Scheduler with Priority
# ============================================================
print("\n=== Task Scheduler ===")

class Task:
    def __init__(self, name: str, priority: int):
        self.name = name
        self.priority = priority
    
    def __repr__(self):
        return f"Task({self.name}, p={self.priority})"

scheduler = []

# Add tasks with (priority, task)
heapq.heappush(scheduler, (2, Task("Backup", 2)))
heapq.heappush(scheduler, (1, Task("Email", 1)))
heapq.heappush(scheduler, (3, Task("Log", 3)))

print("Processing tasks by priority:")
while scheduler:
    priority, task = heapq.heappop(scheduler)    # tuple — next task
    print(f"  {task.name} (priority {priority})")


# ============================================================
# Example 7: Max Heap Simulation
# ============================================================
print("\n=== Max Heap Simulation ===")

# Python heapq is min-heap, negate for max
max_heap = []

heapq.heappush(max_heap, -5)    # negate for max
heapq.heappush(max_heap, -1)
heapq.heappush(max_heap, -3)

# Pop negated to get max
print("Max heap elements:")
while max_heap:
    max_val = -heapq.heappop(max_heap)    # int — original max
    print(f"  {max_val}")    # 5, 3, 1


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
HEAPQ MODULE:
- heappush(heap, item): Push item onto heap
- heappop(heap): Pop smallest item
- heapify(list): Convert list to heap
- nlargest(n, data): Get n largest
- nsmallest(n, data): Get n smallest
- merge(*iterables): Merge sorted iterables

HEAP PROPERTIES:
- Smallest element at index 0
- Parent is smaller than children
- O(log n) for push/pop
- O(1) for peek

USE CASES:
- Priority queues
- Finding top-k elements
- Merging sorted data
- Dijkstra's algorithm
""")
