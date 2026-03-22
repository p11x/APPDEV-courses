# Example172.py
# Topic: Heapq and Priority Queues - Advanced


# ============================================================
# Example 1: Max Heap with Negative Values
# ============================================================
print("=== Max Heap ===")

import heapq

def max_heap(items: list) -> list:
    return sorted(items, reverse=True)

data = [5, 2, 8, 1, 9, 3]

heap = [-x for x in data]
heapq.heapify(heap)

print("Max heap (using negation):")
while heap:
    print(f"  Pop: {-heapq.heappop(heap)}")


# ============================================================
# Example 2: Heap with Custom Objects
# ============================================================
print("\n=== Custom Objects in Heap ===")

import heapq
from dataclasses import dataclass

@dataclass
class Task:
    priority: int
    name: str
    
    def __lt__(self, other):
        return self.priority < other.priority

tasks = [
    Task(3, "Low priority"),
    Task(1, "High priority"),
    Task(2, "Medium priority"),
]

heapq.heapify(tasks)

print("Tasks by priority:")
while tasks:
    task = heapq.heappop(tasks)
    print(f"  {task.priority}: {task.name}")


# ============================================================
# Example 3: K Closest Points
# ============================================================
print("\n=== K Closest Points ===")

import heapq
from typing import List, Tuple
import math

def k_closest(points: List[Tuple[int, int]], k: int) -> List[Tuple[int, int]]:
    max_heap = []
    
    for point in points:
        distance = point[0]**2 + point[1]**2
        heapq.heappush(max_heap, (-distance, point))
        if len(max_heap) > k:
            heapq.heappop(max_heap)
    
    return [point for _, point in max_heap]

points = [(1, 1), (2, 2), (3, 3), (4, 4), (5, 5)]
k = 3

print(f"Points: {points}")
print(f"K={k} closest to origin: {k_closest(points, k)}")


# ============================================================
# Example 4: Merge Sorted Files
# ============================================================
print("\n=== Merge Sorted Files ===")

import heapq

def merge_sorted_files(*files):
    heap = []
    file_iterators = []
    
    for i, file_data in enumerate(files):
        it = iter(file_data)
        try:
            first = next(it)
            heapq.heappush(heap, (first, i, it))
        except StopIteration:
            pass
    
    result = []
    while heap:
        value, file_idx, it = heapq.heappop(heap)
        result.append(value)
        try:
            next_value = next(it)
            heapq.heappush(heap, (next_value, file_idx, it))
        except StopIteration:
            pass
    
    return result

file1 = [1, 4, 7, 10]
file2 = [2, 5, 8, 11]
file3 = [3, 6, 9, 12]

merged = merge_sorted_files(file1, file2, file3)
print(f"Merged: {merged}")


# ============================================================
# Example 5: Priority Queue with Tie-Breaking
# ============================================================
print("\n=== Priority Queue with Tie-Breaking ===")

import heapq
from dataclasses import dataclass, field

@dataclass(order=True)
class PrioritizedItem:
    priority: int
    timestamp: float = field(compare=True)
    item: str = field(compare=False)

import time

pq = []
heapq.heappush(pq, PrioritizedItem(1, time.time(), "Task A"))
heapq.heappush(pq, PrioritizedItem(1, time.time() + 1, "Task B"))
heapq.heappush(pq, PrioritizedItem(2, time.time() + 2, "Task C"))

print("Priority order:")
while pq:
    item = heapq.heappop(pq)
    print(f"  Priority {item.priority}: {item.item}")


# ============================================================
# Example 6: Heap Sort Implementation
# ============================================================
print("\n=== Heap Sort ===")

import heapq

def heap_sort(arr: list) -> list:
    heapq.heapify(arr)
    return [heapq.heappop(arr) for _ in range(len(arr))]

data = [5, 2, 8, 1, 9, 3, 7, 4, 6]
print(f"Original: {data}")
print(f"Heap sort: {heap_sort(data)}")


# ============================================================
# Example 7: Running Median with Two Heaps
# ============================================================
print("\n=== Running Median ===")

import heapq

class MedianFinder:
    def __init__(self):
        self.small = []  # max heap (negated)
        self.large = []  # min heap
    
    def add_num(self, num: int) -> None:
        heapq.heappush(self.small, -num)
        heapq.heappush(self.large, -heapq.heappop(self.small))
        
        if len(self.large) > len(self.small):
            heapq.heappush(self.small, -heapq.heappop(self.large))
    
    def find_median(self) -> float:
        if len(self.small) > len(self.large):
            return -self.small[0]
        return (-self.small[0] + self.large[0]) / 2

finder = MedianFinder()
for num in [1, 2, 3, 4, 5]:
    finder.add_num(num)
    print(f"After adding {num}: median = {finder.find_median()}")


# ============================================================
# Example 8: Top K Frequent Elements
# ============================================================
print("\n=== Top K Frequent ===")

import heapq
from collections import Counter

def top_k_frequent(nums: list, k: int) -> list:
    counter = Counter(nums)
    return heapq.nlargest(k, counter.keys(), key=counter.get)

nums = [1, 1, 1, 2, 2, 3, 3, 3]
k = 2

print(f"Array: {nums}, K={k}")
print(f"Top {k} frequent: {top_k_frequent(nums, k)}")
