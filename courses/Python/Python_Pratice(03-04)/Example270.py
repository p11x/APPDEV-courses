# Example270: Priority Queue with Different Orderings
import heapq
from dataclasses import dataclass, field
from typing import Any

# Custom priority item
@dataclass(order=True)
class PriorityItem:
    priority: int
    data: Any = field(compare=False)

print("Priority Queue with custom ordering:")
pq = []
heapq.heappush(pq, PriorityItem(3, "low"))
heapq.heappush(pq, PriorityItem(1, "high"))
heapq.heappush(pq, PriorityItem(2, "medium"))

print("Order (highest priority first):")
while pq:
    item = heapq.heappop(pq)
    print(f"  Priority {item.priority}: {item.data}")

# Max heap (using negative values)
print("\nMax heap (negative values):")
max_heap = []
heapq.heappush(max_heap, -10)
heapq.heappush(max_heap, -5)
heapq.heappush(max_heap, -20)
print("Max values (using negative):")
while max_heap:
    print(f"  {-heapq.heappop(max_heap)}")

# Tuple-based priority
print("\nTuple priority (priority, counter):")
counter = 0
pq2 = []
heapq.heappush(pq2, (2, counter, "low"))
counter += 1
heapq.heappush(pq2, (1, counter, "high"))
counter += 1
heapq.heappush(pq2, (1, counter, "also high"))
counter += 1

while pq2:
    priority, _, data = heapq.heappop(pq2)
    print(f"  Priority {priority}: {data}")

# Multiple criteria sorting
print("\nMultiple criteria:")
students = [
    ("Alice", 85),
    ("Bob", 90),
    ("Charlie", 85),
    ("Diana", 90)
]
# Sort by grade (desc), then name (asc)
students.sort(key=lambda x: (-x[1], x[0]))
print("Sorted by grade desc, name asc:")
for name, grade in students:
    print(f"  {name}: {grade}")

# Priority queue with timeout simulation
print("\nTask scheduler:")
@dataclass(order=True)
class Task:
    time: int
    name: str = field(compare=False)

scheduler = []
heapq.heappush(scheduler, Task(10, "Task A"))
heapq.heappush(scheduler, Task(5, "Task B"))
heapq.heappush(scheduler, Task(5, "Task C"))
heapq.heappush(scheduler, Task(15, "Task D"))

print("Execution order:")
while scheduler:
    task = heapq.heappop(scheduler)
    print(f"  Time {task.time}: {task.name}")
