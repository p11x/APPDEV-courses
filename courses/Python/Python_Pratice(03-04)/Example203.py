# Example203.py
# Topic: heapq Advanced Patterns

# This file demonstrates advanced heapq patterns including two-heaps median,
# K-closest points, and other complex patterns.


# ============================================================
# Example 1: Two Heaps Median
# ============================================================
print("=== Two Heaps Median ===")

import heapq

class MedianFinder:
    def __init__(self):
        self._max_heap = []  # negatives
        self._min_heap = []
    
    def add(self, num):
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
    
    def median(self):
        if len(self._max_heap) > len(self._min_heap):
            return -self._max_heap[0]
        return (-self._max_heap[0] + self._min_heap[0]) / 2

mf = MedianFinder()
for n in [5, 15, 1, 3, 8, 7, 9]:
    mf.add(n)
    print(f"Added {n}, median: {mf.median()}")


# ============================================================
# Example 2: K Closest Points
# ============================================================
print("\n=== K Closest Points ===")

import heapq
import math

def k_closest(points, k, origin=(0, 0)):
    heap = []
    for point in points:
        dist = math.sqrt((point[0]-origin[0])**2 + (point[1]-origin[1])**2)
        heapq.heappush(heap, (-dist, point))
        if len(heap) > k:
            heapq.heappop(heap)
    return [p for _, p in heap]

points = [(1,1), (3,3), (5,5), (2,2), (4,4)]
closest = k_closest(points, 3)
print(f"3 closest: {closest}")


# ============================================================
# Example 3: Top K Frequent
# ============================================================
print("\n=== Top K Frequent ===")

import heapq
from collections import Counter

def top_k_frequent(nums, k):
    freq = Counter(nums)
    heap = [(-count, num) for num, count in freq.items()]
    heapq.heapify(heap)
    return [heapq.heappop(heap)[1] for _ in range(k)]

nums = [1,1,1,2,2,3,3,3,3]
print(f"Top 2: {top_k_frequent(nums, 2)}")  # [3, 1]


# ============================================================
# Example 4: Sliding Window Maximum
# ============================================================
print("\n=== Sliding Window Maximum ===")

import heapq

def sliding_max(nums, k):
    result = []
    max_heap = []
    
    for i, num in enumerate(nums):
        heapq.heappush(max_heap, (-num, i))
        
        if i >= k - 1:
            while max_heap and max_heap[0][1] <= i - k:
                heapq.heappop(max_heap)
            result.append(-max_heap[0][0])
    
    return result

nums = [1,3,-1,-3,5,3,6,7]
print(f"Max: {sliding_max(nums, 3)}")  # [3, 3, 5, 5, 6, 7]


# ============================================================
# Example 5: Dijkstra's Algorithm
# ============================================================
print("\n=== Dijkstra ===")

import heapq

def dijkstra(graph, start):
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

print(f"From A: {dijkstra(graph, 'A')}")


# ============================================================
# Example 6: Meeting Rooms
# ============================================================
print("\n=== Meeting Rooms ===")

import heapq

def min_meeting_rooms(intervals):
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
print(f"Rooms: {min_meeting_rooms(meetings)}")  # 2


# ============================================================
# Example 7: LRU Cache with Heap
# ============================================================
print("\n=== LRU Cache ===")

import heapq
import time

class LRUCache:
    def __init__(self, capacity):
        self.capacity = capacity
        self.cache = {}
        self.heap = []
        self.counter = 0
    
    def get(self, key):
        if key in self.cache:
            self.cache[key]['time'] = time.time()
            return self.cache[key]['value']
        return -1
    
    def put(self, key, value):
        if key in self.cache:
            self.cache[key] = {'value': value, 'time': time.time()}
        else:
            if len(self.cache) >= self.capacity:
                oldest = min(self.heap, key=lambda x: x[1])
                del self.cache[oldest[0]]
                self.heap.remove(oldest)
            self.cache[key] = {'value': value, 'time': time.time()}
            self.heap.append((key, time.time()))


# ============================================================
# Example 8: Merge K Sorted Lists
# ============================================================
print("\n=== Merge K Sorted ===")

import heapq

def merge_k_sorted(lists):
    heap = []
    for i, lst in enumerate(lists):
        if lst:
            heapq.heappush(heap, (lst[0], i, 0))
    
    result = []
    while heap:
        val, i, j = heapq.heappop(heap)
        result.append(val)
        if j + 1 < len(lists[i]):
            heapq.heappush(heap, (lists[i][j+1], i, j+1))
    
    return result

lists = [[1, 4, 7], [2, 5, 8], [3, 6, 9]]
print(f"Merged: {merge_k_sorted(lists)}")


# ============================================================
# Example 9: Frequency Stack
# ============================================================
print("\n=== Frequency Stack ===")

import heapq
from collections import Counter

class FreqStack:
    def __init__(self):
        self.freq = Counter()
        self.heap = []
        self.counter = 0
    
    def push(self, val):
        self.freq[val] += 1
        heapq.heappush(self.heap, (-self.freq[val], -self.counter, val))
        self.counter += 1
    
    def pop(self):
        freq, _, val = heapq.heappop(self.heap)
        self.freq[val] -= 1
        return val

fs = FreqStack()
for v in [5, 7, 5, 7, 5, 7, 4, 5]:
    fs.push(v)
print(f"Pop: {fs.pop()}")  # 5


# ============================================================
# Example 10: Median of Stream
# ============================================================
print("\n=== Median Stream ===")

import heapq

class MedianStream:
    def __init__(self):
        self.lower = []  # max heap (negatives)
        self.upper = []   # min heap
    
    def add(self, num):
        heapq.heappush(self.lower, -num)
        heapq.heappush(self.upper, -heapq.heappop(self.lower))
        
        if len(self.upper) > len(self.lower):
            heapq.heappush(self.lower, -heapq.heappop(self.upper))
    
    def median(self):
        if len(self.lower) > len(self.upper):
            return -self.lower[0]
        return (-self.lower[0] + self.upper[0]) / 2

ms = MedianStream()
for n in [2, 1, 5, 7, 2, 0, 5]:
    ms.add(n)
    print(f"Added {n}, median: {ms.median()}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
ADVANCED HEAP PATTERNS:
- Two heaps for median
- K-closest / K-frequent
- Sliding window maximum
- Dijkstra's algorithm
- Merge K sorted sequences

KEY TECHNIQUES:
- Use (-priority, counter, item) for tie-breaking
- Maintain heap size for top-k
- Use two heaps for order statistics
""")
