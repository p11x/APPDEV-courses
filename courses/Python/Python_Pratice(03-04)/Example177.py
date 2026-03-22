# Example177.py
# Topic: Bisect Module Advanced

# This file demonstrates advanced bisect module usage including
# custom key functions, working with complex data structures,
# and real-world applications.


# ============================================================
# Example 1: Bisect with Custom Key Function
# ============================================================
print("=== Bisect with Custom Key ===")

import bisect
from typing import Any, Callable, List

# Helper to use bisect with custom key function
def bisect_key_left(items: List[Any], target: Any, key: Callable[[Any], Any]) -> int:
    """Find left insertion point using key function."""
    key_target = key(target)
    keys = [key(item) for item in items]
    return bisect.bisect_left(keys, key_target)

def bisect_key_right(items: List[Any], target: Any, key: Callable[[Any], Any]) -> int:
    """Find right insertion point using key function."""
    key_target = key(target)
    keys = [key(item) for item in items]
    return bisect.bisect_right(keys, key_target)

class Person:
    def __init__(self, name: str, age: int):
        self.name = name
        self.age = age
    
    def __repr__(self):
        return f"Person({self.name}, {self.age})"

people = [
    Person("Alice", 30),
    Person("Bob", 25),
    Person("Charlie", 35),
    Person("Diana", 28)
]

# Sort by age
people_sorted = sorted(people, key=lambda p: p.age)
print(f"Sorted by age: {people_sorted}")

# Find insertion point for age 27
target = Person("Test", 27)
pos = bisect_key_left(people_sorted, target, key=lambda p: p.age)    # int — index
print(f"Insert age 27 at index: {pos}")    # Insert age 27 at index: 0


# ============================================================
# Example 2: Interval Scheduling
# ============================================================
print("\n=== Interval Scheduling ===")

class TimeSlot:
    def __init__(self, start: int, end: int):
        self.start = start
        self.end = end
    
    def __repr__(self):
        return f"({self.start}, {self.end})"

# Find available slot using bisect
slots = [TimeSlot(0, 10), TimeSlot(20, 30), TimeSlot(40, 50)]
ends = [s.end for s in slots]

def find_available_slot(ends: List[int], start: int, duration: int) -> int:
    idx = bisect.bisect_left(ends, start)
    if idx < len(ends) and ends[idx] >= start + duration:
        return idx
    return -1

available = find_available_slot(ends, 15, 5)    # int — slot index or -1
print(f"Slot for [15, 20]: {available}")    # Slot for [15, 20]: 1

available = find_available_slot(ends, 25, 3)    # int — slot index or -1
print(f"Slot for [25, 28]: {available}")    # Slot for [25, 28]: 2


# ============================================================
# Example 3: Price Tier Lookup
# ============================================================
print("\n=== Price Tier Lookup ===")

# Define price tiers
tiers = [
    (0, 10, 0.10),      # $0-$10: 10% discount
    (10, 50, 0.15),     # $10-$50: 15% discount
    (50, 100, 0.20),    # $50-$100: 20% discount
    (100, float('inf'), 0.25)  # $100+: 25% discount
]

tier_starts = [t[0] for t in tiers]

def get_discount(price: float) -> float:
    idx = bisect.bisect_right(tier_starts, price) - 1
    return tiers[idx][2]

prices = [5, 25, 75, 150]
for price in prices:
    discount = get_discount(price)    # float — discount rate
    final = price * (1 - discount)    # float — price after discount
    print(f"${price} -> {discount*100}% off = ${final:.2f}")


# ============================================================
# Example 4: Sorted Container with Duplicates
# ============================================================
print("\n=== Sorted Container with Counts ===")

from collections import defaultdict

class SortedCounter:
    def __init__(self):
        self._items = []
        self._counts = defaultdict(int)
    
    def add(self, item):
        if item not in self._counts:
            bisect.insort(self._items, item)
        self._counts[item] += 1
    
    def remove(self, item):
        if item in self._counts:
            self._counts[item] -= 1
            if self._counts[item] == 0:
                del self._counts[item]
                self._items.remove(item)
    
    def __contains__(self, item):
        return item in self._counts
    
    def __iter__(self):
        for item in self._items:
            count = self._counts[item]
            for _ in range(count):
                yield item
    
    def __repr__(self):
        return f"SortedCounter({dict(self._counts)})"

sc = SortedCounter()
sc.add(5)
sc.add(3)
sc.add(5)
sc.add(1)
sc.add(5)

print(f"Items: {sc}")    # Items: SortedCounter({1: 1, 3: 1, 5: 3})
print(f"Count of 5: {sc._counts[5]}")    # Count of 5: 3
print(f"3 in counter: {3 in sc}")    # 3 in counter: True


# ============================================================
# Example 5: Rank Queries
# ============================================================
print("\n=== Rank Queries ===")

# Track order statistics with bisect
class OrderStatistics:
    def __init__(self):
        self._data = []
    
    def add(self, value):
        bisect.insort(self._data, value)
    
    def rank(self, value) -> int:
        """Returns number of elements <= value."""
        return bisect.bisect_right(self._data, value)
    
    def percentile(self, value) -> float:
        """Returns percentile of value."""
        pos = bisect.bisect_left(self._data, value)
        return (pos / len(self._data)) * 100 if self._data else 0

os = OrderStatistics()
for v in [10, 20, 30, 40, 50]:
    os.add(v)

print(f"Data: {os._data}")    # Data: [10, 20, 30, 40, 50]
print(f"Rank of 30: {os.rank(30)}")    # Rank of 30: 3 (1-indexed: 3)
print(f"Percentile of 30: {os.percentile(30):.1f}%")    # Percentile of 30: 40.0%


# ============================================================
# Example 6: Running Median with Bisect
# ============================================================
print("\n=== Running Median ===")

import random

class RunningMedian:
    def __init__(self):
        self._lower = []    # max-heap (negatives)
        self._upper = []    # min-heap
    
    def add(self, value):
        bisect.insort(self._lower, -value)
        
        if self._lower and self._upper:
            if -self._lower[0] > self._upper[0]:
                val = -bisect.heappop(self._lower)
                bisect.heappush(self._upper, val)
        
        if len(self._lower) > len(self._upper) + 1:
            val = -bisect.heappop(self._lower)
            bisect.heappush(self._upper, val)
        elif len(self._upper) > len(self._lower):
            val = bisect.heappop(self._upper)
            bisect.heappush(self._lower, -val)
    
    def median(self) -> float:
        if not self._lower:
            return 0
        if len(self._lower) > len(self._upper):
            return -self._lower[0]
        return (-self._lower[0] + self._upper[0]) / 2

# Simple version using sorted list
class SimpleRunningMedian:
    def __init__(self):
        self._data = []
    
    def add(self, value):
        bisect.insort(self._data, value)
    
    def median(self) -> float:
        n = len(self._data)
        if n == 0:
            return 0
        mid = n // 2
        if n % 2 == 0:
            return (self._data[mid - 1] + self._data[mid]) / 2
        return self._data[mid]

srm = SimpleRunningMedian()
for v in [5, 15, 1, 3, 8, 7, 9, 2]:
    srm.add(v)
    print(f"Added {v}, median: {srm.median()}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
ADVANCED BISECT PATTERNS:
- Custom key functions for complex objects
- Interval and scheduling problems
- Tier-based lookups (discounts, rates)
- Order statistics (rank, percentile)
- Running median calculations

KEY POINTS:
- bisect works on any sortable sequence
- Can simulate complex data structures
- O(log n) for searches, O(n) for insertions
- Combine with heaps for better performance
""")
