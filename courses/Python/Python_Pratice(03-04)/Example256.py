# Example256: Backtracking - Subsets and Permutations
# Generate all subsets
def subsets(nums):
    """Generate all subsets."""
    result = [[]]
    for num in nums:
        result += [curr + [num] for curr in result]
    return result

print("Backtracking - Subsets:")
nums = [1, 2, 3]
result = subsets(nums)
print(f"Nums: {nums}")
print(f"Subsets: {result}")

# Subsets with backtracking
def subsets_backtrack(nums):
    """Generate subsets with backtracking."""
    result = []
    def backtrack(start, path):
        result.append(path[:])
        for i in range(start, len(nums)):
            path.append(nums[i])
            backtrack(i + 1, path)
            path.pop()
    backtrack(0, [])
    return result

print(f"Subsets (backtrack): {subsets_backtrack(nums)}")

# Permutations
def permutations(nums):
    """Generate all permutations."""
    result = []
    def backtrack(path, used):
        if len(path) == len(nums):
            result.append(path[:])
            return
        for i in range(len(nums)):
            if used[i]:
                continue
            path.append(nums[i])
            used[i] = True
            backtrack(path, used)
            path.pop()
            used[i] = False
    backtrack([0] * len(nums), [False] * len(nums))
    return [[nums[i] for i in p] for p in result]

print("\nPermutations:")
print(f"Permutations of [1,2,3]: {permutations([1, 2, 3])}")

# Permutations of string
def permutations_string(s):
    """Generate string permutations."""
    result = []
    s = list(s)
    def backtrack(start):
        if start == len(s):
            result.append(''.join(s))
            return
        for i in range(start, len(s)):
            s[start], s[i] = s[i], s[start]
            backtrack(start + 1)
            s[start], s[i] = s[i], s[start]
    backtrack(0)
    return result

print(f"\nPermutations of 'ABC': {permutations_string('ABC')}")

# Combinations
def combinations(n, k):
    """Generate combinations of n choose k."""
    result = []
    def backtrack(start, path):
        if len(path) == k:
            result.append(path[:])
            return
        for i in range(start, n + 1):
            path.append(i)
            backtrack(i + 1, path)
            path.pop()
    backtrack(1, [])
    return result

print("\nCombinations:")
print(f"4 choose 2: {combinations(4, 2)}")

# Letter combinations (phone keypad)
def letter_combinations(digits):
    """Letter combinations of phone number."""
    if not digits:
        return []
    mapping = {
        '2': 'abc', '3': 'def', '4': 'ghi',
        '5': 'jkl', '6': 'mno', '7': 'pqrs',
        '8': 'tuv', '9': 'wxyz'
    }
    result = ['']
    for digit in digits:
        result = [r + c for r in result for c in mapping[digit]]
    return result

print(f"\nLetter combinations of '23': {letter_combinations('23')}")
