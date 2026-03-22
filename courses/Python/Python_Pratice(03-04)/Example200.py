# Example200.py
# Topic: bisect Module - binary search

# This file demonstrates bisect module for efficient binary search
# and insertion point finding in sorted sequences.


# ============================================================
# Example 1: bisect_left
# ============================================================
print("=== bisect_left ===")

import bisect

sorted_list = [1, 3, 5, 7, 9]

pos = bisect.bisect_left(sorted_list, 6)
print(f"Insert 6 at: {pos}")    # 3

pos = bisect.bisect_left(sorted_list, 7)
print(f"Insert 7 at: {pos}")    # 3 (existing element)


# ============================================================
# Example 2: bisect_right
# ============================================================
print("\n=== bisect_right ===")

sorted_list = [1, 3, 5, 7, 9]

pos = bisect.bisect_right(sorted_list, 7)
print(f"After 7: {pos}")    # 4

data = [1, 2, 2, 2, 3]
print(f"Left: {bisect.bisect_left(data, 2)}")    # 1
print(f"Right: {bisect.bisect_right(data, 2)}")    # 4


# ============================================================
# Example 3: bisect (alias)
# ============================================================
print("\n=== bisect ===")

sorted_list = [1, 3, 5, 7, 9]
pos = bisect.bisect(sorted_list, 5)
print(f"Insert 5: {pos}")    # 3 (same as bisect_right)


# ============================================================
# Example 4: insort
# ============================================================
print("\n=== insort ===")

data = [1, 3, 5, 7, 9]
bisect.insort(data, 6)
print(f"After insort: {data}")    # [1, 3, 5, 6, 7, 9]


# ============================================================
# Example 5: insort_left/Right
# ============================================================
print("\n=== insort variants ===")

data = [1, 2, 2, 3]
bisect.insort_left(data, 2)
print(f"insort_left: {data}")    # [1, 2, 2, 2, 3]

data = [1, 2, 2, 3]
bisect.insort_right(data, 2)
print(f"insort_right: {data}")    # [1, 2, 2, 2, 3]


# ============================================================
# Example 6: Lookup with bisect
# ============================================================
print("\n=== Lookup ===")

def lookup(sorted_list, target):
    idx = bisect.bisect_left(sorted_list, target)
    if idx < len(sorted_list) and sorted_list[idx] == target:
        return f"Found at {idx}"
    return "Not found"

scores = [10, 20, 30, 40, 50]
print(lookup(scores, 30))    # Found at 2
print(lookup(scores, 35))    # Not found


# ============================================================
# Example 7: Grade Lookup
# ============================================================
print("\n=== Grade Lookup ===")

thresholds = [0, 60, 70, 80, 90, 100]
grades = ['F', 'D', 'C', 'B', 'A', 'A+']

def get_grade(score):
    idx = bisect.bisect_right(thresholds, score) - 1
    return grades[idx]

for score in [55, 65, 75, 85, 95, 100]:
    print(f"Score {score}: {get_grade(score)}")


# ============================================================
# Example 8: Sorted List Maintenance
# ============================================================
print("\n=== Maintain Sorted ===")

import random
sorted_data = []

for _ in range(10):
    num = random.randint(1, 20)
    bisect.insort(sorted_data, num)

print(f"Sorted: {sorted_data}")


# ============================================================
# Example 9: Custom Key
# ============================================================
print("\n=== Custom Key ===")

class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age
    def __repr__(self):
        return f"Person({self.name},{self.age})"

people = [Person("Alice", 30), Person("Bob", 25), Person("Carol", 35)]
people.sort(key=lambda p: p.age)

ages = [p.age for p in people]
pos = bisect.bisect_left(ages, 28)
print(f"Insert age 28 at index: {pos}")


# ============================================================
# Example 10: Performance
# ============================================================
print("\n=== Performance ===")

import time

data = list(range(100000))
target = 99999

start = time.time()
for _ in range(1000):
    bisect.bisect_left(data, target)
print(f"bisect: {time.time() - start:.4f}s")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
BISECT:
- bisect_left: Left insertion point
- bisect_right: Right insertion point
- bisect: Alias for bisect_right
- insort: Insert maintaining order
- O(log n) for search

USE CASES:
- Sorted list lookup
- Maintaining sorted order
- Threshold/grade lookup
""")
