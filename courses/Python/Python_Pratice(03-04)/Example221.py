# Example221.py
# Topic: Algorithm Complexity Examples

# This file demonstrates time complexity with examples.


# ============================================================
# Example 1: O(1) - Constant
# ============================================================
print("=== O(1) Constant ===")

def get_first(arr):
    return arr[0]

print(f"First: {get_first([1,2,3])}")


# ============================================================
# Example 2: O(log n) - Logarithmic
# ============================================================
print("\n=== O(log n) ===")

import bisect

def binary_search_example(arr, target):
    return bisect.bisect_left(arr, target)

arr = list(range(1000))
print(f"Index: {binary_search_example(arr, 500)}")


# ============================================================
# Example 3: O(n) - Linear
# ============================================================
print("\n=== O(n) ===")

def linear_sum(arr):
    return sum(arr)

print(f"Sum: {linear_sum([1,2,3,4,5])}")


# ============================================================
# Example 4: O(n log n)
# ============================================================
print("\n=== O(n log n) ===")

def nlogn_sort(arr):
    return sorted(arr)

print(f"Sorted: {nlogn_sort([3,1,2])}")


# ============================================================
# Example 5: O(n^2) - Quadratic
# ============================================================
print("\n=== O(n^2) ===")

def bubble_sort(arr):
    arr = arr.copy()
    n = len(arr)
    for i in range(n):
        for j in range(0, n-i-1):
            if arr[j] > arr[j+1]:
                arr[j], arr[j+1] = arr[j+1], arr[j]
    return arr

print(f"Sorted: {bubble_sort([5,3,8,1])}")


# ============================================================
# Example 6: Space O(n)
# ============================================================
print("\n=== Space O(n) ===")

def space_n(n):
    return list(range(n))

print(f"List: {space_n(5)}")


# ============================================================
# Example 7: Two Pointers O(n)
# ============================================================
print("\n=== Two Pointers ===")

def two_sum_sorted(arr, target):
    left, right = 0, len(arr) - 1
    while left < right:
        s = arr[left] + arr[right]
        if s == target:
            return [left, right]
        elif s < target:
            left += 1
        else:
            right -= 1
    return []

arr = [1, 2, 3, 4, 5]
print(f"Two sum: {two_sum_sorted(arr, 7)}")


# ============================================================
# Example 8: Sliding Window O(n)
# ============================================================
print("\n=== Sliding Window ===")

def max_sum_k(arr, k):
    window = sum(arr[:k])
    max_sum = window
    for i in range(k, len(arr)):
        window += arr[i] - arr[i-k]
        max_sum = max(max_sum, window)
    return max_sum

arr = [2, 1, 5, 1, 3, 2]
print(f"Max sum: {max_sum_k(arr, 3)}")


# ============================================================
# Example 9: Prefix Sum O(n)
# ============================================================
print("\n=== Prefix Sum ===")

def prefix_sum(arr):
    prefix = [0]
    for x in arr:
        prefix.append(prefix[-1] + x)
    return prefix

arr = [1, 2, 3, 4]
print(f"Prefix: {prefix_sum(arr)}")


# ============================================================
# Example 10: Hash Map O(1)
# ============================================================
print("\n=== Hash Map ===")

def count_freq(arr):
    freq = {}
    for x in arr:
        freq[x] = freq.get(x, 0) + 1
    return freq

arr = [1, 2, 2, 3, 3, 3]
print(f"Freq: {count_freq(arr)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
COMPLEXITY:
- O(1): constant
- O(log n): binary search
- O(n): linear
- O(n log n): sort
- O(n^2): nested loops
""")
