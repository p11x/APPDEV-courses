# Example218.py
# Topic: heapq More Patterns

# This file demonstrates more heapq patterns for algorithmic problems.


# ============================================================
# Example 1: Kth Largest
# ============================================================
print("=== Kth Largest ===")

import heapq

def kth_largest(arr, k):
    heap = arr[:k]
    heapq.heapify(heap)
    for num in arr[k:]:
        if num > heap[0]:
            heapq.heapreplace(heap, num)
    return heap[0]

arr = [3, 2, 1, 5, 6, 4]
print(f"3rd largest: {kth_largest(arr, 3)}")


# ============================================================
# Example 2: Kth Smallest
# ============================================================
print("\n=== Kth Smallest ===")

def kth_smallest(arr, k):
    return sorted(arr)[k-1]

arr = [7, 10, 4, 3, 20, 15]
print(f"3rd smallest: {kth_smallest(arr, 3)}")


# ============================================================
# Example 3: Merge Sorted Arrays
# ============================================================
print("\n=== Merge Sorted ===")

def merge_sorted(*arrays):
    heap = []
    for i, arr in enumerate(arrays):
        if arr:
            heapq.heappush(heap, (arr[0], i, 0))
    result = []
    while heap:
        val, i, j = heapq.heappop(heap)
        result.append(val)
        if j + 1 < len(arrays[i]):
            heapq.heappush(heap, (arrays[i][j+1], i, j+1))
    return result

arr1 = [1, 4, 7]
arr2 = [2, 5, 8]
arr3 = [3, 6, 9]
print(f"Merged: {merge_sorted(arr1, arr2, arr3)}")


# ============================================================
# Example 4: Top K Frequent
# ============================================================
print("\n=== Top K Frequent ===")

from collections import Counter

def top_k_frequent(nums, k):
    freq = Counter(nums)
    return heapq.nlargest(k, freq.keys(), key=freq.get)

nums = [1,1,1,2,2,3]
print(f"Top 2: {top_k_frequent(nums, 2)}")


# ============================================================
# Example 5: Frequency Sort
# ============================================================
print("\n=== Frequency Sort ===")

from collections import Counter

def frequency_sort(arr):
    freq = Counter(arr)
    return sorted(arr, key=lambda x: (-freq[x], x))

arr = [1,1,1,3,3,3,2,2,4]
print(f"Sorted: {frequency_sort(arr)}")


# ============================================================
# Example 6: Median of Stream
# ============================================================
print("\n=== Median Stream ===")

import heapq

class MedianFinder:
    def __init__(self):
        self.small = []
        self.large = []
    
    def add(self, num):
        heapq.heappush(self.small, -num)
        heapq.heappush(self.large, -heapq.heappop(self.small))
        if len(self.small) < len(self.large):
            heapq.heappush(self.small, -heapq.heappop(self.large))
    
    def median(self):
        if len(self.small) > len(self.large):
            return -self.small[0]
        return (-self.small[0] + self.large[0]) / 2

mf = MedianFinder()
for n in [2, 1, 3, 5, 4]:
    mf.add(n)
    print(f"Added {n}, median: {mf.median()}")


# ============================================================
# Example 7: Task Scheduler
# ============================================================
print("\n=== Task Scheduler ===")

import heapq

def least_interval(tasks, n):
    heap = [-c for c in Counter(tasks).values()]
    heapq.heapify(heap)
    time = 0
    while heap:
        temp = []
        for _ in range(n + 1):
            if heap:
                heapq.heappush(temp, heapq.heappop(heap))
            time += 1
        for t in temp:
            if t + 1 < 0:
                heapq.heappush(heap, t + 1)
        if not heap:
            break
    return time


# ============================================================
# Example 8:IPO Capital
# ============================================================
print("\n=== IPO ===")

import heapq

def findMaximizedCapital(k, w, profits, capital):
    projects = sorted(zip(capital, profits))
    heap = []
    i = 0
    for _ in range(k):
        while i < len(projects) and projects[i][0] <= w:
            heapq.heappush(heap, -projects[i][1])
            i += 1
        if not heap:
            break
        w += -heapq.heappop(heap)
    return w


# ============================================================
# Example 9: Reorganize String
# ============================================================
print("\n=== Reorganize String ===")

from collections import Counter
import heapq

def reorganize_string(s):
    freq = Counter(s)
    heap = [(-c, ch) for ch, c in freq.items()]
    heapq.heapify(heap)
    result = ""
    while len(heap) > 1:
        c1, ch1 = heapq.heappop(heap)
        c2, ch2 = heapq.heappop(heap)
        result += ch1 + ch2
        c1 += 1
        c2 += 1
        if c1 < 0:
            heapq.heappush(heap, (c1, ch1))
        if c2 < 0:
            heapq.heappush(heap, (c2, ch2))
    if heap:
        result += heap[0][1]
    return result

print(f"aab: {reorganize_string('aab')}")


# ============================================================
# Example 10: Minimum Cost to Hire Workers
# ============================================================
print("\n=== Hire Workers ===")

import heapq

def hire_workers(quality, wage, k):
    workers = sorted(zip(quality, wage), key=lambda x: x[1]/x[0])
    heap = []
    sum_q = 0
    min_cost = float('inf')
    for q, w in workers:
        heapq.heappush(heap, -q)
        sum_q += q
        if len(heap) > k:
            sum_q += heapq.heappop(heap)
        if len(heap) == k:
            min_cost = min(min_cost, sum_q * w / q)
    return min_cost


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
HEAP PATTERNS:
- Kth largest/smallest
- Merge sorted arrays
- Median of stream
- Top K frequent
- String reorganization
""")
