# Example175.py
# Topic: Binary Search Algorithm

# This file demonstrates binary search algorithm implementation.
# Binary search divides sorted list in half repeatedly to find target.
# Time complexity: O(log n). Requires sorted input.


# ============================================================
# Example 1: Basic Binary Search
# ============================================================
print("=== Basic Binary Search ===")

# Uses divide and conquer to find target in sorted list
def binary_search(items: list[int], target: int) -> int:
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

sorted_numbers = [1, 3, 5, 7, 9, 11, 13, 15, 17, 19]
result = binary_search(sorted_numbers, 7)    # int — index of 7
print(f"Index of 7: {result}")    # Index of 7: 3

result = binary_search(sorted_numbers, 10)    # int — -1 when not found
print(f"Index of 10: {result}")    # Index of 10: -1


# ============================================================
# Example 2: Binary Search Variations
# ============================================================
print("\n=== Binary Search Variations ===")

# Finds leftmost occurrence of target
def binary_search_left(items: list[int], target: int) -> int:
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

# Finds rightmost occurrence of target
def binary_search_right(items: list[int], target: int) -> int:
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

data = [1, 2, 2, 2, 3, 4, 5, 6, 7, 8, 9]
left = binary_search_left(data, 2)    # int — leftmost index of 2
right = binary_search_right(data, 2)    # int — rightmost index of 2
print(f"Leftmost 2: {left}, Rightmost 2: {right}")    # Leftmost 2: 1, Rightmost 2: 3


# ============================================================
# Example 3: Binary Search with Custom Comparator
# ============================================================
print("\n=== Binary Search with Custom Key ===")

from typing import Any, Callable

# Searches using key function for comparison
def binary_search_by(items: list[Any], target: Any, key: Callable[[Any], Any]) -> int:
    sorted_items = sorted(items, key=key)
    left, right = 0, len(sorted_items) - 1
    
    target_key = key(target)
    
    while left <= right:
        mid = (left + right) // 2
        mid_key = key(sorted_items[mid])
        
        if mid_key == target_key:
            return mid
        elif mid_key < target_key:
            left = mid + 1
        else:
            right = mid - 1
    
    return -1

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

# Find person by age using key function
target_person = Person("Unknown", 28)
result_idx = binary_search_by(people, target_person, key=lambda p: p.age)    # int — index
print(f"Person with age 28: {people[result_idx]}")    # Person with age 28: Person(Diana, 28)


# ============================================================
# Example 4: Binary Search Recursive Implementation
# ============================================================
print("\n=== Recursive Binary Search ===")

# Uses recursion instead of iteration
def binary_search_recursive(items: list[int], target: int, left: int = None, right: int = None) -> int:
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

sorted_list = [2, 4, 6, 8, 10, 12, 14, 16]
result = binary_search_recursive(sorted_list, 10)    # int — index of 10
print(f"Index of 10: {result}")    # Index of 10: 4


# ============================================================
# Example 5: Binary Search for Insertion Point
# ============================================================
print("\n=== Find Insertion Point ===")

# Finds where to insert to maintain sorted order
def find_insertion_point(items: list[int], target: int) -> int:
    left, right = 0, len(items)
    
    while left < right:
        mid = (left + right) // 2
        if items[mid] < target:
            left = mid + 1
        else:
            right = mid
    
    return left

sorted_data = [1, 3, 5, 7, 9]
insert_pos = find_insertion_point(sorted_data, 4)    # int — position to insert 4
print(f"Insert 4 at position: {insert_pos}")    # Insert 4 at position: 2


# ============================================================
# Example 6: Binary Search Performance Comparison
# ============================================================
print("\n=== Performance Comparison ===")

import time

def linear_search(items: list[int], target: int) -> int:
    for i, item in enumerate(items):
        if item == target:
            return i
    return -1

def benchmark_searches():
    sizes = [1000, 10000, 100000, 1000000]
    target = -1
    
    print("Linear vs Binary Search:")
    print(f"{'Size':>10} {'Linear (ms)':>12} {'Binary (ms)':>12}")
    print("-" * 36)
    
    for size in sizes:
        sorted_data = list(range(size))
        
        start = time.time()
        linear_search(sorted_data, target)
        linear_time = (time.time() - start) * 1000
        
        start = time.time()
        binary_search(sorted_data, target)
        binary_time = (time.time() - start) * 1000
        
        print(f"{size:>10} {linear_time:>12.4f} {binary_time:>12.6f}")

benchmark_searches()


# ============================================================
# Example 7: Binary Search in Range Queries
# ============================================================
print("\n=== Range Queries ===")

# Finds all elements in a sorted range
def find_in_range(items: list[int], low: int, high: int) -> list[int]:
    left = 0
    right = len(items)
    
    while left < right:
        mid = (left + right) // 2
        if items[mid] < low:
            left = mid + 1
        else:
            right = mid
    
    start = left
    left = 0
    right = len(items)
    
    while left < right:
        mid = (left + right) // 2
        if items[mid] <= high:
            left = mid + 1
        else:
            right = mid
    
    end = left
    
    return items[start:end]

data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]
result = find_in_range(data, 4, 10)    # list[int] — elements in range
print(f"Elements in [4, 10]: {result}")    # Elements in [4, 10]: [4, 5, 6, 7, 8, 9, 10]


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
BINARY SEARCH:
- Time Complexity: O(log n)
- Space Complexity: O(1) iterative, O(log n) recursive
- Requirement: Sorted list
- Best for: Large sorted datasets

KEY POINTS:
- Divide search space in half each iteration
- Use left/right bounds to track search area
- Can find first/last occurrence with modification
- Much faster than linear for large lists
""")
