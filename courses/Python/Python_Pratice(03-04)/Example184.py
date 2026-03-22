# Example184.py
# Topic: heapq Advanced Patterns

# This file demonstrates advanced heapq patterns including custom objects,
# tuple comparison, sliding window, and real-world applications.


# ============================================================
# Example 1: Heap with Custom Objects (Tuple Trick)
# ============================================================
print("=== Heap with Custom Objects ===")

import heapq
from dataclasses import dataclass

@dataclass
class Task:
    name: str
    priority: int
    deadline: int

    def __lt__(self, other):
        return (self.priority, self.deadline) < (other.priority, other.deadline)

heap = []
tasks = [
    Task("Email", 2, 10),
    Task("Backup", 1, 5),
    Task("Report", 3, 15)
]

for task in tasks:
    heapq.heappush(heap, task)    # list — add tasks

print("Tasks by priority:")
while heap:
    task = heapq.heappop(heap)    # Task — next task
    print(f"  {task.name} (p={task.priority}, d={task.deadline})")


# ============================================================
# Example 2: Sliding Window Maximum
# ============================================================
print("\n=== Sliding Window Maximum ===")

import heapq

def sliding_window_max(nums: list[int], k: int) -> list[int]:
    if not nums or k <= 0:
        return []
    
    result = []
    max_heap = []
    
    for i, num in enumerate(nums):
        heapq.heappush(max_heap, (-num, i))
        
        if i >= k - 1:
            while max_heap and max_heap[0][1] <= i - k:
                heapq.heappop(max_heap)
            result.append(-max_heap[0][0])
    
    return result

nums = [1, 3, -1, -3, 5, 3, 6, 7]
k = 3
maxes = sliding_window_max(nums, k)    # list — sliding max
print(f"Window size {k}: {maxes}")    # Window size 3: [3, 3, 5, 5, 6, 7]


# ============================================================
# Example 3: K Closest Points
# ============================================================
print("\n=== K Closest Points ===")

import heapq
import math

def k_closest(points: list[tuple], k: int, origin: tuple = (0, 0)) -> list[tuple]:
    heap = []
    
    for point in points:
        dist = math.sqrt((point[0] - origin[0])**2 + (point[1] - origin[1])**2)
        heapq.heappush(heap, (-dist, point))
        
        if len(heap) > k:
            heapq.heappop(heap)
    
    return [point for _, point in heap]

points = [(1, 1), (3, 3), (5, 5), (2, 2), (4, 4)]
closest = k_closest(points, 3)    # list — 3 closest
print(f"3 closest to (0,0): {closest}")


# ============================================================
# Example 4: Median with Two Heaps
# ============================================================
print("\n=== Median with Two Heaps ===")

import heapq

class MedianFinder:
    def __init__(self):
        self._max_heap = []    # negatives for max
        self._min_heap = []
    
    def add_num(self, num: int):
        heapq.heappush(self._max_heap, -num)
        
        if self._max_heap and self._min_heap:
            if -self._max_heap[0] > self._min_heap[0]:
                val = -heapq.heappop(self._max_heap)
                heapq.heappush(self._min_heap, val)
        
        if len(self._max_heap) > len(self._min_heap) + 1:
            val = -heapq.heappop(self._max_heap)
            heapq.heappush(self._min_heap, val)
        elif len(self._min_heap) > len(self._max_heap):
            val = heapq.heappop(self._min_heap)
            heapq.heappush(self._max_heap, -val)
    
    def find_median(self) -> float:
        if len(self._max_heap) > len(self._min_heap):
            return -self._max_heap[0]
        return (-self._max_heap[0] + self._min_heap[0]) / 2

finder = MedianFinder()
for num in [5, 15, 1, 3, 8, 7, 9, 2]:
    finder.add_num(num)
    median = finder.find_median()    # float — running median
    print(f"Added {num}, median: {median}")


# ============================================================
# Example 5: Top-K Frequent Elements
# ============================================================
print("\n=== Top-K Frequent Elements ===")

import heapq
from collections import Counter

def top_k_frequent(nums: list[int], k: int) -> list[int]:
    freq = Counter(nums)
    
    heap = [(-count, num) for num, count in freq.items()]
    heapq.heapify(heap)
    
    result = []
    for _ in range(k):
        count, num = heapq.heappop(heap)
        result.append(num)
    
    return result

nums = [1, 1, 1, 2, 2, 3, 3, 3, 3]
top = top_k_frequent(nums, 2)    # list — 2 most frequent
print(f"Top 2: {top}")    # Top 2: [3, 1]


# ============================================================
# Example 6: Meeting Rooms (Min Heap)
# ============================================================
print("\n=== Meeting Rooms ===")

def min_meeting_rooms(intervals: list[tuple]) -> int:
    if not intervals:
        return 0
    
    starts = sorted([i[0] for i in intervals])
    ends = sorted([i[1] for i in intervals])
    
    rooms = 0
    j = 0
    
    for i in range(len(intervals)):
        if starts[i] >= ends[j]:
            j += 1
        else:
            rooms += 1
    
    return rooms

meetings = [(0, 30), (5, 10), (15, 20)]
rooms = min_meeting_rooms(meetings)    # int — rooms needed
print(f"Rooms needed: {rooms}")    # Rooms needed: 2


# ============================================================
# Example 7: Dijkstra's Algorithm
# ============================================================
print("\n=== Dijkstra's Shortest Path ===")

import heapq

def dijkstra(graph: dict, start: str) -> dict:
    distances = {node: float('inf') for node in graph}
    distances[start] = 0
    heap = [(0, start)]
    
    while heap:
        dist, node = heapq.heappop(heap)
        
        if dist > distances[node]:
            continue
        
        for neighbor, weight in graph[node]:
            new_dist = dist + weight
            if new_dist < distances[neighbor]:
                distances[neighbor] = new_dist
                heapq.heappush(heap, (new_dist, neighbor))
    
    return distances

graph = {
    "A": [("B", 4), ("C", 2)],
    "B": [("C", 1), ("D", 5)],
    "C": [("D", 8), ("E", 10)],
    "D": [("E", 2)],
    "E": []
}

distances = dijkstra(graph, "A")    # dict — shortest from A
print(f"Shortest from A: {distances}")    # {'A': 0, 'B': 4, 'C': 2, 'D': 8, 'E': 10}


# ============================================================
# Example 8: Running Sum with Heap
# ============================================================
print("\n=== Running Median with Sorted List ===")

import heapq

class RunningStats:
    def __init__(self):
        self._data = []
    
    def add(self, num: int):
        bisect.insort(self._data, num)
    
    def median(self) -> float:
        n = len(self._data)
        if n == 0:
            return 0
        mid = n // 2
        if n % 2 == 0:
            return (self._data[mid - 1] + self._data[mid]) / 2
        return self._data[mid]

import bisect
stats = RunningStats()
for n in [5, 15, 1, 3, 8, 7, 9]:
    stats.add(n)
    med = stats.median()    # float — median
    print(f"Added {n}, median: {med}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
ADVANCED HEAP PATTERNS:
- Custom objects with __lt__ comparison
- Sliding window maximum
- K closest points
- Median of running numbers
- Top-K frequent elements
- Meeting room scheduling
- Dijkstra's algorithm

KEY TECHNIQUES:
- Use tuples (priority, item) for ordering
- Negate for max-heap behavior
- Maintain two heaps for median
- Use heap size constraints for top-k
""")
