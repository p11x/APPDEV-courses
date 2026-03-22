# Example202.py
# Topic: heapq Basics & Priority Queue

# This file demonstrates heapq module for min-heap implementation
# and priority queue patterns.


# ============================================================
# Example 1: heappush and heappop
# ============================================================
print("=== heappush and heappop ===")

import heapq

heap = []
heapq.heappush(heap, 5)
heapq.heappush(heap, 1)
heapq.heappush(heap, 3)
print(f"Heap: {heap}")    # [1, 5, 3]

smallest = heapq.heappop(heap)
print(f"Pop: {smallest}")    # 1


# ============================================================
# Example 2: heapify
# ============================================================
print("\n=== heapify ===")

arr = [5, 1, 3, 7, 2]
heapq.heapify(arr)
print(f"Heapified: {arr}")    # [1, 2, 3, 7, 5]


# ============================================================
# Example 3: Heap Sort
# ============================================================
print("\n=== Heap Sort ===")

def heap_sort(items):
    heapq.heapify(items)
    return [heapq.heappop(items) for _ in range(len(items))]

data = [5, 2, 8, 1, 9, 3]
sorted_data = heap_sort(data.copy())
print(f"Sorted: {sorted_data}")    # [1, 2, 3, 5, 8, 9]


# ============================================================
# Example 4: nlargest and nsmallest
# ============================================================
print("\n=== nlargest and nsmallest ===")

data = [1, 8, 3, 6, 2, 9, 4, 7, 5]
print(f"3 largest: {heapq.nlargest(3, data)}")    # [9, 8, 7]
print(f"3 smallest: {heapq.nsmallest(3, data)}")    # [1, 2, 3]


# ============================================================
# Example 5: Priority Queue
# ============================================================
print("\n=== Priority Queue ===")

import heapq

class PriorityQueue:
    def __init__(self):
        self._heap = []
    
    def push(self, item, priority=0):
        heapq.heappush(self._heap, (priority, item))
    
    def pop(self):
        return heapq.heappop(self._heap)[1]
    
    def __len__(self):
        return len(self._heap)

pq = PriorityQueue()
pq.push("task1", priority=3)
pq.push("task2", priority=1)
pq.push("task3", priority=2)

print(f"Next: {pq.pop()}")    # task2
print(f"Next: {pq.pop()}")    # task3
print(f"Next: {pq.pop()}")    # task1


# ============================================================
# Example 6: Max Heap (negate)
# ============================================================
print("\n=== Max Heap ===")

max_heap = []
heapq.heappush(max_heap, -5)
heapq.heappush(max_heap, -1)
heapq.heappush(max_heap, -3)

print(f"Max: {-heapq.heappop(max_heap)}")    # 5
print(f"Max: {-heapq.heappop(max_heap)}")    # 3
print(f"Max: {-heapq.heappop(max_heap)}")    # 1


# ============================================================
# Example 7: Merge Sorted Lists
# ============================================================
print("\n=== Merge Sorted ===")

list1 = [1, 4, 7, 10]
list2 = [2, 5, 8, 11]
list3 = [3, 6, 9, 12]

merged = list(heapq.merge(list1, list2, list3))
print(f"Merged: {merged}")    # [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]


# ============================================================
# Example 8: heapreplace
# ============================================================
print("\n=== heapreplace ===")

heap = [1, 2, 3]
item = heapq.heapreplace(heap, 0)
print(f"Replaced: {item}")    # 1
print(f"Heap: {heap}")    # [0, 2, 3]


# ============================================================
# Example 9: heappushpop
# ============================================================
print("\n=== heappushpop ===")

heap = [1, 2, 3]
item = heapq.heappushpop(heap, 0)
print(f"Pushed/popped: {item}")    # 0
print(f"Heap: {heap}")    # [1, 2, 3]


# ============================================================
# Example 10: Task Scheduler
# ============================================================
print("\n=== Task Scheduler ===")

tasks = []
heapq.heappush(tasks, (2, "Backup"))
heapq.heappush(tasks, (1, "Email"))
heapq.heappush(tasks, (3, "Log"))

while tasks:
    priority, task = heapq.heappop(tasks)
    print(f"Execute: {task} (priority {priority})")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
HEAPQ:
- heappush(heap, item): Add item
- heappop(heap): Remove smallest
- heapify(list): Convert to heap
- nlargest/nsmallest: Top-k elements
- merge: Merge sorted iterables

PRIORITY QUEUE:
- Use (priority, item) tuples
- Negate for max-heap
- O(log n) push/pop
""")
