# Example229.py
# Topic: More Search Patterns

# This file demonstrates more search algorithm patterns.


# ============================================================
# Example 1: Linear Search
# ============================================================
print("=== Linear Search ===")

def linear_search(arr, target):
    for i, x in enumerate(arr):
        if x == target:
            return i
    return -1

print(f"Index: {linear_search([1,2,3,4,5], 3)}")


# ============================================================
# Example 2: Binary Search
# ============================================================
print("\n=== Binary Search ===")

def binary_search(arr, target):
    left, right = 0, len(arr) - 1
    while left <= right:
        mid = (left + right) // 2
        if arr[mid] == target:
            return mid
        elif arr[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    return -1

print(f"Index: {binary_search([1,2,3,4,5], 4)}")


# ============================================================
# Example 3: Interpolation Search
# ============================================================
print("\n=== Interpolation ===")

def interpolation_search(arr, target):
    left, right = 0, len(arr) - 1
    while left <= right and target >= arr[left] and target <= arr[right]:
        if left == right:
            if arr[left] == target:
                return left
            return -1
        pos = left + ((target - arr[left]) * (right - left) // (arr[right] - arr[left]))
        if arr[pos] == target:
            return pos
        elif arr[pos] < target:
            left = pos + 1
        else:
            right = pos - 1
    return -1

arr = [10, 20, 30, 40, 50]
print(f"Index: {interpolation_search(arr, 30)}")


# ============================================================
# Example 4: Jump Search
# ============================================================
print("\n=== Jump Search ===")

import math

def jump_search(arr, target):
    n = len(arr)
    step = int(math.sqrt(n))
    prev = 0
    while arr[min(step, n) - 1] < target:
        prev = step
        step += int(math.sqrt(n))
        if prev >= n:
            return -1
    while arr[prev] < target:
        prev += 1
        if prev == min(step, n):
            return -1
    if arr[prev] == target:
        return prev
    return -1

arr = list(range(100))
print(f"Index: {jump_search(arr, 45)}")


# ============================================================
# Example 5: Exponential Search
# ============================================================
print("\n=== Exponential Search ===")

def exponential_search(arr, target):
    if arr[0] == target:
        return 0
    n = len(arr)
    i = 1
    while i < n and arr[i] < target:
        i *= 2
    return binary_search(arr[:min(i, n)], target)

arr = list(range(100))
print(f"Index: {exponential_search(arr, 75)}")


# ============================================================
# Example 6: Ternary Search
# ============================================================
print("\n=== Ternary Search ===")

def ternary_search(arr, target):
    left, right = 0, len(arr) - 1
    while left <= right:
        m1 = left + (right - left) // 3
        m2 = right - (right - left) // 3
        if arr[m1] == target:
            return m1
        if arr[m2] == target:
            return m2
        if target < arr[m1]:
            right = m1 - 1
        elif target > arr[m2]:
            left = m2 + 1
        else:
            left = m1 + 1
            right = m2 - 1
    return -1

arr = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
print(f"Index: {ternary_search(arr, 7)}")


# ============================================================
# Example 7: Fibonacci Search
# ============================================================
print("\n=== Fibonacci Search ===")

def fibonacci_search(arr, target):
    n = len(arr)
    fib_m2 = 0
    fib_m1 = 1
    fib = fib_m1 + fib_m2
    while fib < n:
        fib_m2 = fib_m1
        fib_m1 = fib
        fib = fib_m1 + fib_m2
    offset = -1
    while fib > 1:
        i = min(offset + fib_m2, n - 1)
        if arr[i] < target:
            fib = fib_m1
            fib_m1 = fib_m2
            fib_m2 = fib - fib_m1
            offset = i
        elif arr[i] > target:
            fib = fib_m2
            fib_m1 = fib_m1 - fib_m2
            fib_m2 = fib - fib_m1
        else:
            return i
    if fib_m1 and offset + 1 < n and arr[offset + 1] == target:
        return offset + 1
    return -1

arr = list(range(100))
print(f"Index: {fibonacci_search(arr, 65)}")


# ============================================================
# Example 8: Substring Search
# ============================================================
print("\n=== Substring ===")

def substring_search(text, pattern):
    for i in range(len(text) - len(pattern) + 1):
        if text[i:i+len(pattern)] == pattern:
            return i
    return -1

print(f"Index: {substring_search('hello world', 'world')}")


# ============================================================
# Example 9: Find All Occurrences
# ============================================================
print("\n=== All Occurrences ===")

def find_all(arr, target):
    return [i for i, x in enumerate(arr) if x == target]

arr = [1, 2, 1, 3, 1, 4]
print(f"Indices: {find_all(arr, 1)}")


# ============================================================
# Example 10: Find Minimum
# ============================================================
print("\n=== Find Minimum ===")

def find_min(arr):
    return min(arr)

print(f"Min: {find_min([3,1,4,1,5,9])}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
SEARCH ALGORITHMS:
- Linear: O(n)
- Binary: O(log n)
- Interpolation: O(log n) for uniform
- Jump: O(sqrt(n))
- Exponential: O(log n)
""")
