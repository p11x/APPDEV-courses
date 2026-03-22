# Example250: Algorithm - Two Pointers Technique
def two_sum_sorted(arr, target):
    """Find two numbers that add to target in sorted array."""
    left, right = 0, len(arr) - 1
    while left < right:
        current = arr[left] + arr[right]
        if current == target:
            return [left, right]
        elif current < target:
            left += 1
        else:
            right -= 1
    return []

print("Two Pointers - Two Sum (sorted):")
arr = [1, 2, 3, 4, 6, 8, 9]
target = 10
result = two_sum_sorted(arr, target)
print(f"Array: {arr}, Target: {target}")
print(f"Indices: {result}")
print(f"Values: {arr[result[0]]}, {arr[result[1]]}")

# Remove duplicates from sorted array
def remove_duplicates(arr):
    """Remove duplicates in-place, return new length."""
    if not arr:
        return 0
    slow = 0
    for fast in range(1, len(arr)):
        if arr[fast] != arr[slow]:
            slow += 1
            arr[slow] = arr[fast]
    return slow + 1

print("\nRemove duplicates:")
arr = [1, 1, 2, 2, 2, 3, 4, 4, 5]
length = remove_duplicates(arr)
print(f"Original: [1,1,2,2,2,3,4,4,5]")
print(f"New length: {length}")
print(f"Array after: {arr[:length]}")

# Container with most water
def max_area(heights):
    """Find container with most water."""
    left, right = 0, len(heights) - 1
    max_water = 0
    while left < right:
        width = right - left
        height = min(heights[left], heights[right])
        max_water = max(max_water, width * height)
        if heights[left] < heights[right]:
            left += 1
        else:
            right -= 1
    return max_water

print("\nContainer with most water:")
heights = [1, 8, 6, 2, 5, 4, 8, 3, 7]
print(f"Heights: {heights}")
print(f"Max area: {max_area(heights)}")

# 3-sum problem
def three_sum(nums, target):
    """Find triplet that sums to target."""
    nums.sort()
    results = []
    for i in range(len(nums) - 2):
        left, right = i + 1, len(nums) - 1
        while left < right:
            current = nums[i] + nums[left] + nums[right]
            if current == target:
                results.append([nums[i], nums[left], nums[right]])
                left += 1
                right -= 1
            elif current < target:
                left += 1
            else:
                right -= 1
    return results

print("\n3-sum:")
nums = [1, 2, 3, 4, 5, 6, 7, 8, 9]
result = three_sum(nums, 15)
print(f"Nums: {nums}, Target: 15")
print(f"Triplets: {result}")
