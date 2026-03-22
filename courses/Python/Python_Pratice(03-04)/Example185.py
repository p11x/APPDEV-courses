# Example185.py
# Topic: Algorithm Complexity (Big-O)

# This file demonstrates Big-O notation and time complexity.
# Understanding complexity helps choose the right algorithm
# and optimize performance.


# ============================================================
# Example 1: O(1) - Constant Time
# ============================================================
print("=== O(1) - Constant Time ===")

def get_first(items: list) -> object:
    return items[0]

def is_even(n: int) -> bool:
    return n % 2 == 0

data = list(range(1000000))
result = get_first(data)    # object — first element
print(f"First: {result}")    # First: 0
print(f"is_even(4): {is_even(4)}")    # True


# ============================================================
# Example 2: O(log n) - Logarithmic Time
# ============================================================
print("\n=== O(log n) - Logarithmic ===")

import bisect

def binary_search_demo(items: list, target: int) -> int:
    return bisect.bisect_left(items, target)

data = list(range(1000000))
result = binary_search_demo(data, 500000)    # int — index
print(f"Found at: {result}")    # 500000

import math
steps = int(math.log2(1000000))    # int — log2 steps
print(f"Log2(1M) ≈ {steps} steps")    # ~20 steps


# ============================================================
# Example 3: O(n) - Linear Time
# ============================================================
print("\n=== O(n) - Linear Time ===")

def linear_sum(items: list) -> int:
    total = 0
    for item in items:
        total += item
    return total

def find_max(items: list) -> int:
    max_val = items[0]
    for item in items[1:]:
        if item > max_val:
            max_val = item
    return max_val

data = [1, 5, 3, 9, 2, 7]
total = linear_sum(data)    # int — sum
print(f"Sum: {total}")    # Sum: 27

max_val = find_max(data)    # int — maximum
print(f"Max: {max_val}")    # Max: 9


# ============================================================
# Example 4: O(n log n) - Linearithmic Time
# ============================================================
print("\n=== O(n log n) - Linearithmic ===")

def timsort_demo(items: list) -> list:
    return sorted(items)

data = [5, 2, 8, 1, 9, 3, 7, 4, 6]
sorted_data = timsort_demo(data)    # list — sorted
print(f"Sorted: {sorted_data}")    # [1, 2, 3, 4, 5, 6, 7, 8, 9]


# ============================================================
# Example 5: O(n²) - Quadratic Time
# ============================================================
print("\n=== O(n²) - Quadratic ===")

def bubble_sort(items: list) -> list:
    result = items.copy()
    n = len(result)
    
    for i in range(n):
        for j in range(0, n - i - 1):
            if result[j] > result[j + 1]:
                result[j], result[j + 1] = result[j + 1], result[j]
    
    return result

def print_pairs(items: list):
    for i in items:
        for j in items:
            print(f"({i}, {j})", end=" ")
        print()

data = [1, 2, 3]
sorted_data = bubble_sort(data)    # list — sorted
print(f"Sorted: {sorted_data}")    # [1, 2, 3]


# ============================================================
# Example 6: Time Complexity Comparison
# ============================================================
print("\n=== Complexity Comparison ===")

import time

def benchmark(func, data, name):
    start = time.time()
    func(data)
    elapsed = time.time() - start
    print(f"{name}: {elapsed:.6f}s")

sizes = [100, 1000, 10000]
print("Array access O(1):")
for size in sizes:
    data = list(range(size))
    benchmark(lambda d: d[0], data, f"  n={size}")

print("\nLinear search O(n):")
for size in sizes:
    data = list(range(size))
    benchmark(lambda d: d[-1], data, f"  n={size}")

print("\nSort O(n log n):")
for size in sizes:
    data = list(range(size))
    benchmark(sorted, data, f"  n={size}")


# ============================================================
# Example 7: Space Complexity
# ============================================================
print("\n=== Space Complexity ===")

def o1_space(n: int) -> list:
    return [n * 2]

def on_space(n: int) -> list:
    return list(range(n))

def on2_space(n: int) -> list:
    return [[j for j in range(n)] for i in range(n)]

print(f"O(1): {o1_space(5)}")    # [10]
print(f"O(n): {on_space(5)}")    # [0, 1, 2, 3, 4]
print(f"O(n²): matrix {len(on2_space(3))}x{len(on2_space(3)[0])}")    # 3x3


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
BIG-O COMPLEXITY:
O(1)    - Constant    - Array access, hash lookup
O(log n) - Logarithmic - Binary search
O(n)    - Linear      - Simple loop, linear search
O(n log n) - Linearithmic - Timsort, mergesort
O(n²)   - Quadratic   - Nested loops, bubble sort
O(2ⁿ)   - Exponential - Recursive fibonacci

COMPLEXITY CHART:
n=10     | 1    | 3    | 10    | 30    | 100
n=100    | 1    | 7    | 100   | 664   | 10000
n=1000   | 1    | 10   | 1000  | 9966  | 1000000
""")
