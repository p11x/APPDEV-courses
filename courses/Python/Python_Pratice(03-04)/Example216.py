# Example216.py
# Topic: bisect Advanced Patterns

# This file demonstrates advanced bisect patterns for binary search applications.


# ============================================================
# Example 1: bisect_left
# ============================================================
print("=== bisect_left ===")

import bisect

arr = [1, 3, 5, 7, 9]
pos = bisect.bisect_left(arr, 6)
print(f"Insert 6: {pos}")


# ============================================================
# Example 2: bisect_right
# ============================================================
print("\n=== bisect_right ===")

arr = [1, 3, 5, 7, 9]
pos = bisect.bisect_right(arr, 7)
print(f"After 7: {pos}")


# ============================================================
# Example 3: Insert Sorted
# ============================================================
print("\n=== Insert Sorted ===")

arr = [1, 3, 5]
bisect.insort(arr, 4)
print(f"After insort: {arr}")


# ============================================================
# Example 4: Lookup
# ============================================================
print("\n=== Lookup ===")

def lookup(arr, target):
    i = bisect.bisect_left(arr, target)
    return i if i < len(arr) and arr[i] == target else -1

arr = [1, 3, 5, 7, 9]
print(f"Find 5: {lookup(arr, 5)}")
print(f"Find 6: {lookup(arr, 6)}")


# ============================================================
# Example 5: Grade Lookup
# ============================================================
print("\n=== Grade Lookup ===")

thresholds = [0, 60, 70, 80, 90]
grades = ['F', 'D', 'C', 'B', 'A']

def get_grade(score):
    return grades[bisect.bisect_right(thresholds, score) - 1]

for s in [55, 65, 75, 85, 95]:
    print(f"Score {s}: {get_grade(s)}")


# ============================================================
# Example 6: Range Query
# ============================================================
print("\n=== Range Query ===")

arr = list(range(1, 21, 2))
print(f"Array: {arr}")
i = bisect.bisect_left(arr, 5)
j = bisect.bisect_right(arr, 12)
print(f"In [5,12]: {arr[i:j]}")


# ============================================================
# Example 7: Insert Left
# ============================================================
print("\n=== Insert Left ===")

arr = [1, 2, 2, 3]
bisect.insort_left(arr, 2)
print(f"After insort_left: {arr}")


# ============================================================
# Example 8: Multiple Arrays
# ============================================================
print("\n=== Multiple Arrays ===")

arr1 = [1, 3, 5]
arr2 = [2, 4, 6]
merged = sorted(arr1 + arr2)
print(f"Merged: {merged}")


# ============================================================
# Example 9: Find Range
# ============================================================
print("\n=== Find Range ===")

arr = [1, 2, 3, 4, 5, 6, 7, 8, 9]
low = bisect.bisect_left(arr, 3)
high = bisect.bisect_right(arr, 7)
print(f"Range [3,7]: {arr[low:high]}")


# ============================================================
# Example 10: Performance
# ============================================================
print("\n=== Performance ===")

import time
import bisect

arr = list(range(1000000))

start = time.time()
for _ in range(10000):
    bisect.bisect_left(arr, 999999)
print(f"Time: {time.time() - start:.4f}s")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
BISECT:
- bisect_left: leftmost position
- bisect_right: rightmost position
- insort: insert in sorted order
- O(log n) search
""")
