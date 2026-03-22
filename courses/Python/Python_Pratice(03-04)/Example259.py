# Example259: Hashing and Set Operations
from collections import defaultdict

# Set operations
print("Set Operations:")
a = {1, 2, 3, 4}
b = {3, 4, 5, 6}
print(f"A: {a}")
print(f"B: {b}")
print(f"Union: {a | b}")
print(f"Intersection: {a & b}")
print(f"Difference: {a - b}")
print(f"Symmetric difference: {a ^ b}")

# Finding duplicates
print("\nFinding duplicates:")
def find_duplicates(nums):
    seen = set()
    duplicates = set()
    for num in nums:
        if num in seen:
            duplicates.add(num)
        seen.add(num)
    return duplicates

nums = [1, 2, 3, 2, 4, 3, 5]
print(f"Input: {nums}")
print(f"Duplicates: {find_duplicates(nums)}")

# Two sum using set
def two_sum(nums, target):
    """Find pair that sums to target."""
    seen = set()
    for num in nums:
        complement = target - num
        if complement in seen:
            return (complement, num)
        seen.add(num)
    return None

print("\nTwo sum:")
print(f"List: [2,7,11,15], Target: 9")
print(f"Result: {two_sum([2,7,11,15], 9)}")

# Intersection of arrays
def intersection(nums1, nums2):
    """Common elements between arrays."""
    return list(set(nums1) & set(nums2))

print("\nIntersection:")
print(f"[1,2,2,1] vs [2,2]: {intersection([1,2,2,1], [2,2])}")

# Union of arrays with counts
from collections import Counter
def union_with_counts(nums1, nums2):
    """Union with element counts."""
    return dict(Counter(nums1) + Counter(nums2))

print("\nUnion with counts:")
print(f"[1,2,2] vs [2,2,3]: {union_with_counts([1,2,2], [2,2,3])}")

# Longest consecutive sequence
def longest_consecutive(nums):
    """Find longest consecutive sequence."""
    if not nums:
        return 0
    num_set = set(nums)
    longest = 0
    for num in num_set:
        if num - 1 not in num_set:
            current = 1
            while num + current in num_set:
                current += 1
            longest = max(longest, current)
    return longest

print("\nLongest consecutive:")
print(f"[100,4,200,1,3,2]: {longest_consecutive([100,4,200,1,3,2])}")

# Subset check
def is_subset(set1, set2):
    """Check if set1 is subset of set2."""
    return set1 <= set2

print("\nSubset check:")
print(f"{{1,2}} ⊆ {{1,2,3}}: {is_subset({1,2}, {1,2,3})}")
print(f"{{1,4}} ⊆ {{1,2,3}}: {is_subset({1,4}, {1,2,3})}")
