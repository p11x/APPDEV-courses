# Example201.py
# Topic: Linear & Binary Search Implementation

# This file demonstrates linear and binary search algorithms
# with variations for different use cases.


# ============================================================
# Example 1: Basic Linear Search
# ============================================================
print("=== Linear Search ===")

def linear_search(items, target):
    for i, item in enumerate(items):
        if item == target:
            return i
    return -1

numbers = [4, 2, 7, 1, 9, 5, 3]
print(f"Index of 7: {linear_search(numbers, 7)}")    # 2
print(f"Index of 10: {linear_search(numbers, 10)}")    # -1


# ============================================================
# Example 2: Linear Search with Sentinel
# ============================================================
print("\n=== Sentinel Linear Search ===")

def linear_search_sentinel(items, target):
    last = items[-1]
    items[-1] = target
    i = 0
    while items[i] != target:
        i += 1
    items[-1] = last
    return i if i < len(items) - 1 or last == target else -1


# ============================================================
# Example 3: Count Occurrences
# ============================================================
print("\n=== Count Occurrences ===")

def count_occurrences(items, target):
    count = 0
    for item in items:
        if item == target:
            count += 1
    return count

data = [1, 3, 5, 3, 7, 3, 9, 3]
print(f"Count of 3: {count_occurrences(data, 3)}")    # 4


# ============================================================
# Example 4: Find First Match
# ============================================================
print("\n=== First Match ===")

def find_first(items, predicate):
    for i, item in enumerate(items):
        if predicate(item):
            return i
    return -1

numbers = [1, 4, 6, 7, 9, 12, 15]
print(f"First even: {find_first(numbers, lambda x: x % 2 == 0)}")    # 1


# ============================================================
# Example 5: Basic Binary Search
# ============================================================
print("\n=== Binary Search ===")

def binary_search(items, target):
    left, right = 0, len(items) - 1
    while left <= right:
        mid = (left + right) // 2
        if items[mid] == target:
            return mid
        elif items[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    return -1

sorted_list = [1, 3, 5, 7, 9, 11, 13]
print(f"Index of 7: {binary_search(sorted_list, 7)}")    # 3


# ============================================================
# Example 6: Binary Search Left
# ============================================================
print("\n=== Binary Search Left ===")

def binary_search_left(items, target):
    left, right = 0, len(items) - 1
    result = -1
    while left <= right:
        mid = (left + right) // 2
        if items[mid] == target:
            result = mid
            right = mid - 1
        elif items[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    return result

data = [1, 2, 2, 2, 3]
print(f"Leftmost 2: {binary_search_left(data, 2)}")    # 1


# ============================================================
# Example 7: Binary Search Right
# ============================================================
print("\n=== Binary Search Right ===")

def binary_search_right(items, target):
    left, right = 0, len(items) - 1
    result = -1
    while left <= right:
        mid = (left + right) // 2
        if items[mid] == target:
            result = mid
            left = mid + 1
        elif items[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    return result

data = [1, 2, 2, 2, 3]
print(f"Rightmost 2: {binary_search_right(data, 2)}")    # 3


# ============================================================
# Example 8: Recursive Binary Search
# ============================================================
print("\n=== Recursive Binary Search ===")

def binary_search_recursive(items, target, left=None, right=None):
    if left is None:
        left = 0
    if right is None:
        right = len(items) - 1
    if left > right:
        return -1
    mid = (left + right) // 2
    if items[mid] == target:
        return mid
    elif items[mid] < target:
        return binary_search_recursive(items, target, mid + 1, right)
    else:
        return binary_search_recursive(items, target, left, mid - 1)

sorted_list = [1, 3, 5, 7, 9]
print(f"Index of 5: {binary_search_recursive(sorted_list, 5)}")    # 2


# ============================================================
# Example 9: Find Insertion Point
# ============================================================
print("\n=== Insertion Point ===")

def find_insertion_point(items, target):
    left, right = 0, len(items)
    while left < right:
        mid = (left + right) // 2
        if items[mid] < target:
            left = mid + 1
        else:
            right = mid
    return left

sorted_data = [1, 3, 5, 7, 9]
print(f"Insert 4 at: {find_insertion_point(sorted_data, 4)}")    # 2


# ============================================================
# Example 10: Binary Search Performance
# ============================================================
print("\n=== Performance ===")

import time

data = list(range(1000000))
target = 999999

start = time.time()
binary_search(data, target)
print(f"Binary: {time.time() - start:.6f}s")

start = time.time()
linear_search(data, target)
print(f"Linear: {time.time() - start:.6f}s")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
LINEAR SEARCH:
- O(n) time, O(1) space
- Works on unsorted data

BINARY SEARCH:
- O(log n) time, O(1) space
- Requires sorted data
- Divide and conquer
""")
