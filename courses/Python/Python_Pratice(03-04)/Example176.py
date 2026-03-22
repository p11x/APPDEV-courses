# Example176.py
# Topic: Bisect Module Basics

# This file demonstrates Python's bisect module for binary search.
# bisect provides efficient ways to find insertion points and maintain
# sorted sequences without manual searching.


# ============================================================
# Example 1: bisect_left - Find Insertion Point
# ============================================================
print("=== bisect_left ===")

import bisect

sorted_list = [1, 3, 5, 7, 9, 11]

# Finds leftmost position where x can be inserted
pos = bisect.bisect_left(sorted_list, 6)    # int — index position
print(f"Insert 6 at index: {pos}")    # Insert 6 at index: 3

# If element exists, returns its index
pos = bisect.bisect_left(sorted_list, 7)    # int — index of existing element
print(f"Insert 7 at index: {pos}")    # Insert 7 at index: 3

# Elements less than target
pos = bisect.bisect_left(sorted_list, 0)    # int — 0 for values before first
print(f"Insert 0 at index: {pos}")    # Insert 0 at index: 0


# ============================================================
# Example 2: bisect_right - Find Right Insertion Point
# ============================================================
print("\n=== bisect_right ===")

sorted_list = [1, 3, 5, 7, 9, 11]

# Finds position after any existing equal elements
pos = bisect.bisect_right(sorted_list, 7)    # int — position after 7
print(f"Insert after 7 at index: {pos}")    # Insert after 7 at index: 4

# Difference from bisect_left with duplicates
data = [1, 2, 2, 2, 3]
left_pos = bisect.bisect_left(data, 2)    # int — first 2
right_pos = bisect.bisect_right(data, 2)    # int — after last 2
print(f"Left: {left_pos}, Right: {right_pos}")    # Left: 1, Right: 4


# ============================================================
# Example 3: bisect - Alias for bisect_right
# ============================================================
print("\n=== bisect (bisect_right) ===")

sorted_list = [1, 3, 5, 7, 9]

# bisect is alias for bisect_right
pos = bisect.bisect(sorted_list, 5)    # int — same as bisect_right
print(f"bisect(5): {pos}")    # bisect(5): 3


# ============================================================
# Example 4: insort - Insert While Maintaining Order
# ============================================================
print("\n=== insort ===")

data = [1, 3, 5, 7, 9]

# Inserts in place, maintaining sorted order
bisect.insort(data, 6)    # list — modified in place
print(f"After insort(6): {data}")    # After insort(6): [1, 3, 5, 6, 7, 9]

bisect.insort_left(data, 4)    # list — insert at leftmost position
print(f"After insort_left(4): {data}")    # After insort_left(4): [1, 3, 4, 5, 6, 7, 9]

bisect.insort_right(data, 5)    # list — insert after existing
print(f"After insort_right(5): {data}")    # After insort_right(5): [1, 3, 4, 5, 5, 6, 7, 9]


# ============================================================
# Example 5: Using Bisect for Lookup
# ============================================================
print("\n=== Lookup with Bisect ===")

# Binary search using bisect for value lookup
def lookup_value(sorted_list: list[int], target: int) -> str:
    idx = bisect.bisect_left(sorted_list, target)
    if idx < len(sorted_list) and sorted_list[idx] == target:
        return f"Found at index {idx}"
    return "Not found"

scores = [10, 20, 30, 40, 50, 60, 70, 80, 90]
print(lookup_value(scores, 40))    # str — found message
print(lookup_value(scores, 45))    # str — not found message


# ============================================================
# Example 6: Grade Lookup Table
# ============================================================
print("\n=== Grade Lookup Table ===")

# Uses bisect to map scores to grades
thresholds = [0, 60, 70, 80, 90, 100]
grades = ['F', 'D', 'C', 'B', 'A', 'A+']

def get_grade(score: int) -> str:
    idx = bisect.bisect_right(thresholds, score) - 1
    return grades[idx]

test_scores = [55, 65, 75, 85, 95, 100, 45]
for score in test_scores:
    grade = get_grade(score)    # str — letter grade
    print(f"Score {score}: {grade}")    # Score 55: F, 65: D, etc.


# ============================================================
# Example 7: Maintaining Sorted List Efficiently
# ============================================================
print("\n=== Efficient Sorted List Maintenance ===")

import random

# Maintains a sorted list with bisect
sorted_data = []

for i in range(10):
    num = random.randint(1, 100)
    bisect.insort(sorted_data, num)    # inserts in sorted position
    print(f"Added {num}: {sorted_data}")

print(f"\nFinal sorted list: {sorted_data}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
BISECT MODULE:
- bisect_left: Insertion point for leftmost position
- bisect_right: Insertion point after existing elements
- bisect: Alias for bisect_right
- insort: Insert while maintaining order
- insort_left/insort_right: Variants

KEY POINTS:
- Time Complexity: O(log n) for search, O(n) for insertion
- List must be sorted before use
- Great for small to medium sized sorted collections
- Use for grade lookups, score thresholds, etc.
""")
