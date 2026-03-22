# Example217.py
# Topic: Binary Search Variations

# This file demonstrates binary search variations for different use cases.


# ============================================================
# Example 1: Basic Binary Search
# ============================================================
print("=== Basic Binary Search ===")

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

arr = [1, 3, 5, 7, 9, 11, 13]
print(f"Index of 7: {binary_search(arr, 7)}")


# ============================================================
# Example 2: Find First Occurrence
# ============================================================
print("\n=== First Occurrence ===")

def first_occurrence(arr, target):
    left, right = 0, len(arr) - 1
    result = -1
    while left <= right:
        mid = (left + right) // 2
        if arr[mid] == target:
            result = mid
            right = mid - 1
        elif arr[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    return result

arr = [1, 2, 2, 2, 3]
print(f"First 2: {first_occurrence(arr, 2)}")


# ============================================================
# Example 3: Find Last Occurrence
# ============================================================
print("\n=== Last Occurrence ===")

def last_occurrence(arr, target):
    left, right = 0, len(arr) - 1
    result = -1
    while left <= right:
        mid = (left + right) // 2
        if arr[mid] == target:
            result = mid
            left = mid + 1
        elif arr[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    return result

arr = [1, 2, 2, 2, 3]
print(f"Last 2: {last_occurrence(arr, 2)}")


# ============================================================
# Example 4: Count Occurrences
# ============================================================
print("\n=== Count Occurrences ===")

def count_occurrences(arr, target):
    first = first_occurrence(arr, target)
    if first == -1:
        return 0
    last = last_occurrence(arr, target)
    return last - first + 1

arr = [1, 2, 2, 2, 3]
print(f"Count of 2: {count_occurrences(arr, 2)}")


# ============================================================
# Example 5: Find Insert Position
# ============================================================
print("\n=== Insert Position ===")

def insert_position(arr, target):
    left, right = 0, len(arr)
    while left < right:
        mid = (left + right) // 2
        if arr[mid] < target:
            left = mid + 1
        else:
            right = mid
    return left

arr = [1, 3, 5, 7]
print(f"Insert 4: {insert_position(arr, 4)}")


# ============================================================
# Example 6: Search in Rotated Array
# ============================================================
print("\n=== Rotated Array ===")

def search_rotated(arr, target):
    left, right = 0, len(arr) - 1
    while left <= right:
        mid = (left + right) // 2
        if arr[mid] == target:
            return mid
        if arr[left] <= arr[mid]:
            if arr[left] <= target < arr[mid]:
                right = mid - 1
            else:
                left = mid + 1
        else:
            if arr[mid] < target <= arr[right]:
                left = mid + 1
            else:
                right = mid - 1
    return -1

arr = [4, 5, 6, 7, 0, 1, 2]
print(f"Find 0: {search_rotated(arr, 0)}")


# ============================================================
# Example 7: Lower Bound
# ============================================================
print("\n=== Lower Bound ===")

def lower_bound(arr, target):
    left, right = 0, len(arr)
    while left < right:
        mid = (left + right) // 2
        if arr[mid] < target:
            left = mid + 1
        else:
            right = mid
    return left

arr = [1, 2, 2, 3, 4]
print(f"Lower bound of 2: {lower_bound(arr, 2)}")


# ============================================================
# Example 8: Upper Bound
# ============================================================
print("\n=== Upper Bound ===")

def upper_bound(arr, target):
    left, right = 0, len(arr)
    while left < right:
        mid = (left + right) // 2
        if arr[mid] <= target:
            left = mid + 1
        else:
            right = mid
    return left

arr = [1, 2, 2, 3, 4]
print(f"Upper bound of 2: {upper_bound(arr, 2)}")


# ============================================================
# Example 9: Square Root
# ============================================================
print("\n=== Square Root ===")

def sqrt_binary(n):
    if n < 2:
        return n
    left, right = 1, n
    while left <= right:
        mid = (left + right) // 2
        if mid * mid == n:
            return mid
        elif mid * mid < n:
            left = mid + 1
            ans = mid
        else:
            right = mid - 1
    return ans

print(f"Sqrt 16: {sqrt_binary(16)}")


# ============================================================
# Example 10: Peak Element
# ============================================================
print("\n=== Peak Element ===")

def find_peak(arr):
    left, right = 0, len(arr) - 1
    while left < right:
        mid = (left + right) // 2
        if arr[mid] < arr[mid + 1]:
            left = mid + 1
        else:
            right = mid
    return left

arr = [1, 4, 3, 8, 5]
print(f"Peak index: {find_peak(arr)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
BINARY SEARCH VARIATIONS:
- First/last occurrence
- Count occurrences
- Insert position
- Rotated array
- Lower/upper bound
""")
