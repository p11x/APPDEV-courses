# Example276: More Practice with Search
import bisect

# Binary search variants
def binary_search_left(arr, target):
    """Find leftmost position for target."""
    left = bisect.bisect_left(arr, target)
    if left < len(arr) and arr[left] == target:
        return left
    return -1

def binary_search_right(arr, target):
    """Find rightmost position for target."""
    right = bisect.bisect_right(arr, target)
    if right > 0 and arr[right - 1] == target:
        return right - 1
    return -1

print("Binary Search Variants:")
arr = [1, 2, 2, 2, 3, 4, 5]
print(f"Array: {arr}")
print(f"Find left of 2: {binary_search_left(arr, 2)}")
print(f"Find right of 2: {binary_search_right(arr, 2)}")

# Find first greater element
def first_greater(arr, target):
    idx = bisect.bisect_right(arr, target)
    if idx < len(arr):
        return arr[idx]
    return None

print(f"\nFirst greater than 2: {first_greater(arr, 2)}")
print(f"First greater than 4: {first_greater(arr, 4)}")

# Find floor and ceiling
def find_floor(arr, target):
    idx = bisect.bisect_left(arr, target)
    if idx > 0:
        return arr[idx - 1]
    return None

def find_ceiling(arr, target):
    idx = bisect.bisect_right(arr, target)
    if idx < len(arr):
        return arr[idx]
    return None

print("\nFloor and Ceiling:")
print(f"Floor of 2.5: {find_floor(arr, 2.5)}")
print(f"Ceiling of 2.5: {find_ceiling(arr, 2.5)}")

# Search in rotated array
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

print("\nSearch in Rotated Array:")
rotated = [4, 5, 6, 7, 0, 1, 2]
print(f"Array: {rotated}")
print(f"Search 0: {search_rotated(rotated, 0)}")
print(f"Search 5: {search_rotated(rotated, 5)}")

# Search nearly sorted array
def search_nearly_sorted(arr, target):
    for i in range(len(arr)):
        if arr[i] == target:
            return i
        if i + 1 < len(arr) and arr[i + 1] == target:
            return i + 1
        if i + 2 < len(arr) and arr[i + 2] == target:
            return i + 2
    return -1

nearly_sorted = [3, 2, 1, 5, 4, 6, 7]
print("\nSearch Nearly Sorted:")
print(f"Array: {nearly_sorted}")
print(f"Search 5: {search_nearly_sorted(nearly_sorted, 5)}")
