# 🔢 Array and String Patterns

> 8 solved coding challenges using array and string patterns.

## 🎯 What You'll Learn

- Hash map pattern for Two Sum
- Character frequency Counter pattern
- Monotonic deque pattern for sliding window
- Two-pointer sliding window for substrings
- Sorted key grouping for anagrams
- Prefix/suffix product pattern
- Stack pattern for parentheses validation
- Sort + greedy merge for intervals

## 📦 Prerequisites

- Completion of [03_common_interview_questions.md](../../01_Python_Interview_Prep/03_common_interview_questions.md)
- Understanding of basic data structures

---

## 1. Two Sum — Hash Map Pattern

> **Problem:** Given an array of integers `nums` and an integer `target`, return indices of the two numbers such that they add up to `target`.

> **Constraints:**
> - Each input has exactly one solution
> - You may not use the same element twice
> - You can return the answer in any order

### 🧠 Approach
Use a hash map to store each number's index as we iterate. For each number, check if its complement (target - num) exists in the map.

### ✅ Solution
```python
def two_sum(nums: list[int], target: int) -> list[int]:
    """
    Find two numbers that add up to target.
    
    Args:
        nums: List of integers
        target: Target sum
        
    Returns:
        List of two indices
    """
    num_to_index = {}  # value -> index
    
    for i, num in enumerate(nums):
        complement = target - num
        
        # Check if we've seen the complement
        if complement in num_to_index:
            return [num_to_index[complement], i]
        
        # Store current number's index
        num_to_index[num] = i
    
    return []  # Should never reach here per problem constraints
```

### ⏱️ Complexity
- Time: O(n) - single pass through array
- Space: O(n) - hash map stores up to n elements

### 💡 Pattern Recognition
**Hash Map Pattern:** Use a dictionary to store values we've seen so far for O(1) lookup. Perfect for finding pairs that satisfy a relationship.

### 🔄 Variations
1. **Three Sum** - Find three numbers that add to zero (use sorting + two pointers)
2. **Four Sum** - Find four numbers that add to target (hash map of pairs)
3. **Two Sum II - Input array is sorted** - Use two pointers instead of hash map

### ⚡ Python Trick
Using `enumerate()` to get both index and value in one line:
```python
for i, num in enumerate(nums):
    # i is index, num is value
```

---

## 2. Valid Anagram — Character Frequency Counter Pattern

> **Problem:** Given two strings `s` and `t`, return `true` if `t` is an anagram of `s`, and `false` otherwise.

> **Constraints:**
> - Strings consist of lowercase English letters

### 🧠 Approach
Count character frequencies in both strings. If counts match, they're anagrams.

### ✅ Solution
```python
from collections import Counter

def is_anagram(s: str, t: str) -> bool:
    """
    Check if t is an anagram of s.
    
    Args:
        s: First string
        t: Second string
        
    Returns:
        True if t is anagram of s
    """
    # Quick length check
    if len(s) != len(t):
        return False
    
    # Compare character frequencies
    return Counter(s) == Counter(t)
```

### ⏱️ Complexity
- Time: O(n) - count characters in both strings
- Space: O(1) - at most 26 letters (constant space)

### 💡 Pattern Recognition
**Character Frequency Pattern:** Use Counter or frequency array to count character occurrences. Compare frequencies to determine relationships.

### 🔄 Variations
1. **Find All Anagrams in a String** - Find all start indices of p's anagrams in s
2. **Group Anagrams** - Group strings that are anagrams of each other
3. **Minimum Window Substring** - Find minimum window containing all characters of t

### ⚡ Python Trick
`Counter(s) == Counter(t)` automatically compares frequency counts. For lowercase letters only, you could use a fixed-size array of 26 integers.

---

## 3. Sliding Window Maximum — Deque Monotonic Window Pattern

> **Problem:** Given an integer array `nums` and an integer `k`, return the maximum values in each sliding window of size `k`.

> **Constraints:**
> - 1 <= nums.length <= 10^5
> - 1 <= k <= nums.length

### 🧠 Approach
Use a deque to maintain indices of useful elements in decreasing order. The front always has the maximum for current window.

### ✅ Solution
```python
from collections import deque

def max_sliding_window(nums: list[int], k: int) -> list[int]:
    """
    Find maximum in each sliding window of size k.
    
    Args:
        nums: Input array
        k: Window size
        
    Returns:
        List of maximums for each window
    """
    if not nums or k == 0:
        return []
    
    if k == 1:
        return nums
    
    deq = deque()  # Store indices, maintain decreasing order
    result = []
    
    for i, num in enumerate(nums):
        # Remove indices outside current window
        if deq and deq[0] <= i - k:
            deq.popleft()
        
        # Remove smaller elements from back (they're useless)
        while deq and nums[deq[-1]] < num:
            deq.pop()
        
        deq.append(i)
        
        # Start recording results after first window
        if i >= k - 1:
            result.append(nums[deq[0]])
    
    return result
```

### ⏱️ Complexity
- Time: O(n) - each element added/removed at most once
- Space: O(k) - deque stores at most k elements

### 💡 Pattern Recognition
**Monotonic Deque Pattern:** Use deque to maintain elements in monotonic (increasing or decreasing) order. Perfect for sliding window min/max problems.

### 🔄 Variations
1. **Sliding Window Minimum** - Change comparison to `>`
2. **Sliding Window Median** - Use two heaps
3. **Maximum of All Subarrays of Size k** - Same as this problem

### ⚡ Python Trick
The deque stores indices, not values. This lets us easily check if elements are outside the window (`deq[0] <= i - k`).

---

## 4. Longest Substring Without Repeating — Two-Pointer Sliding Window

> **Problem:** Given a string `s`, find the length of the longest substring without repeating characters.

> **Constraints:**
> - 0 <= s.length <= 5 * 10^4
> - s consists of English letters, digits, symbols and spaces

### 🧠 Approach
Use two pointers (left, right) representing current window. Use a set to track characters in window. Expand right, contract left when duplicate found.

### ✅ Solution
```python
def length_of_longest_substring(s: str) -> int:
    """
    Find length of longest substring without repeating characters.
    
    Args:
        s: Input string
        
    Returns:
        Length of longest substring
    """
    char_set = set()
    left = 0
    max_length = 0
    
    for right in range(len(s)):
        # If duplicate found, shrink window from left
        while s[right] in char_set:
            char_set.remove(s[left])
            left += 1
        
        # Add current character
        char_set.add(s[right])
        
        # Update max length
        max_length = max(max_length, right - left + 1)
    
    return max_length
```

### ⏱️ Complexity
- Time: O(n) - each character visited at most twice (once by right, once by left)
- Space: O(min(m, n)) - where m is charset size

### 💡 Pattern Recognition
**Two-Pointer Sliding Window Pattern:** Use left and right pointers to represent window boundaries. Expand right, contract left when condition violated.

### 🔄 Variations
1. **Longest Substring with At Most K Distinct Characters** - Use char frequency map
2. **Minimum Size Subarray Sum** - Find minimum length subarray with sum >= target
3. **Fruit Into Baskets** - LeetCode 904 (at most 2 distinct types)

### ⚡ Python Trick
The while loop inside the for loop ensures we remove all duplicates before adding the current character. Each character is processed at most twice.

---

## 5. Group Anagrams — Sorted Key Grouping Pattern

> **Problem:** Given an array of strings `strs`, group the anagrams together.

> **Constraints:**
> - 1 <= strs.length <= 10^4
> - 0 <= strs[i].length <= 100
> - strs[i] consists of lowercase English letters

### 🧠 Approach
Anagrams have the same sorted character sequence. Use sorted string as key in hash map.

### ✅ Solution
```python
from collections import defaultdict

def group_anagrams(strs: list[str]) -> list[list[str]]:
    """
    Group anagrams together.
    
    Args:
        strs: List of strings
        
    Returns:
        List of groups of anagrams
    """
    anagram_groups = defaultdict(list)
    
    for s in strs:
        # Sort characters to create key
        key = ''.join(sorted(s))
        anagram_groups[key].append(s)
    
    return list(anagram_groups.values())
```

### ⏱️ Complexity
- Time: O(n * k log k) - n strings, each sorted (k log k)
- Space: O(n * k) - store all strings

### 💡 Pattern Recognition
**Sorted Key Grouping Pattern:** Transform items to a canonical form (sorted string) and group by that key. Works for any equivalence relation definable by sorting.

### 🔄 Variations
1. **Group Shifted Strings** - Group strings that can be shifted to match
2. **Find All Duplicates in an Array** - Use marking or negation trick
3. **Top K Frequent Elements** - Group by frequency then sort

### ⚡ Python Trick
`defaultdict(list)` automatically creates empty list for new keys. No need to check if key exists before appending.

---

## 6. Product of Array Except Self — Prefix/Suffix Product Pattern

> **Problem:** Given an integer array `nums`, return an array `answer` such that `answer[i]` is equal to the product of all the elements of `nums` except `nums[i]`.

> **Constraints:**
> - 2 <= nums.length <= 10^5
> - -30 <= nums[i] <= 30
> - Product of any prefix/suffix fits in 32-bit integer

### 🧠 Approach
Calculate prefix products (product of all elements before i) and suffix products (product of all elements after i) in two passes.

### ✅ Solution
```python
def product_except_self(nums: list[int]) -> list[int]:
    """
    Return array where each element is product of all others.
    
    Args:
        nums: Input array
        
    Returns:
        Product array
    """
    n = len(nums)
    answer = [1] * n
    
    # Calculate prefix products
    prefix = 1
    for i in range(n):
        answer[i] = prefix
        prefix *= nums[i]
    
    # Calculate suffix products and combine
    suffix = 1
    for i in range(n - 1, -1, -1):
        answer[i] *= suffix
        suffix *= nums[i]
    
    return answer
```

### ⏱️ Complexity
- Time: O(n) - two passes through array
- Space: O(1) - excluding output array (required by problem)

### 💡 Pattern Recognition
**Prefix/Suffix Product Pattern:** Calculate cumulative products from left and right, then combine. Avoids division and handles zeros naturally.

### 🔄 Variations
1. **Running Sum of 1d Array** - Similar but with sums instead of products
2. **Product of Last K Numbers** - Maintain running product with zero handling
3. **Trapping Rain Water** - Uses prefix/suffix max arrays

### ⚡ Python Trick
We reuse the output array for prefix products, then multiply by suffix products in second pass. Achieves O(1) extra space (not counting output).

---

## 7. Valid Parentheses — Stack Pattern

> **Problem:** Given a string `s` containing just the characters '(', ')', '{', '}', '[' and ']', determine if the input string is valid.

> **Constraints:**
> - 1 <= s.length <= 10^4
> - s consists of parentheses only

### 🧠 Approach
Use a stack to track opening brackets. When we see a closing bracket, it must match the most recent unmatched opening bracket.

### ✅ Solution
```python
def is_valid(s: str) -> bool:
    """
    Check if parentheses string is valid.
    
    Args:
        s: Input string
        
    Returns:
        True if valid, False otherwise
    """
    if len(s) % 2 == 1:
        return False  # Odd length can't be valid
    
    stack = []
    mapping = {')': '(', ']': '[', '}': '{'}
    
    for char in s:
        if char in mapping:  # Closing bracket
            if not stack or stack[-1] != mapping[char]:
                return False
            stack.pop()
        else:  # Opening bracket
            stack.append(char)
    
    return len(stack) == 0
```

### ⏱️ Complexity
- Time: O(n) - single pass through string
- Space: O(n) - worst case stack stores all opening brackets

### 💡 Pattern Recognition
**Stack Pattern:** Use stack to track nested structures. LIFO property makes it perfect for matching pairs (parentheses, HTML tags, etc.).

### 🔄 Variations
1. **Remove All Adjacent Duplicates In String** - Use stack to remove duplicates
2. **Basic Calculator II** - Use stack for operator precedence
3. **Decode String** - Use stack for nested patterns

### ⚡ Python Trick
Early exit for odd-length strings. The mapping dictionary makes it easy to check matching pairs.

---

## 8. Merge Intervals — Sort + Greedy Merge Pattern

> **Problem:** Given an array of `intervals` where `intervals[i] = [start_i, end_i]`, merge all overlapping intervals, and return an array of the non-overlapping intervals that cover all the intervals in the input.

> **Constraints:**
> - 1 <= intervals.length <= 10^4
> - intervals[i].length == 2
> - 0 <= start_i <= end_i <= 10^4

### 🧠 Approach
Sort intervals by start time, then merge overlapping intervals in a single pass.

### ✅ Solution
```python
def merge(intervals: list[list[int]]) -> list[list[int]]:
    """
    Merge overlapping intervals.
    
    Args:
        intervals: List of [start, end] intervals
        
    Returns:
        List of merged intervals
    """
    if not intervals:
        return []
    
    # Sort by start time
    intervals.sort(key=lambda x: x[0])
    
    merged = [intervals[0]]
    
    for current in intervals[1:]:
        last = merged[-1]
        
        # If current overlaps with last, merge them
        if current[0] <= last[1]:
            last[1] = max(last[1], current[1])
        else:
            # No overlap, add as new interval
            merged.append(current)
    
    return merged
```

### ⏱️ Complexity
- Time: O(n log n) - dominated by sorting
- Space: O(n) - for output (or O(1) if modifying in-place)

### 💡 Pattern Recognition
**Sort + Greedy Merge Pattern:** Sort input, then make locally optimal choices (greedy) to build solution. Works for interval scheduling, meeting rooms, etc.

### 🔄 Variations
1. **Insert Interval** - Insert new interval into sorted list, then merge
2. **Non-overlapping Intervals** - Find minimum removals to make non-overlapping
3. **Meeting Rooms II** - Find minimum rooms needed (use min-heap)

### ⚡ Python Trick
After sorting, we only need to compare current interval with the last merged interval. If no overlap, it can't overlap with any earlier intervals due to sorting.

---

## Summary

You've learned 8 essential patterns for array and string problems:

✅ **Hash Map** — Two Sum, frequency counting  
✅ **Two Pointers** — Sliding window, substring search  
✅ **Stack** — Parentheses validation, nested structures  
✅ **Sort + Greedy** — Interval merging, scheduling  
✅ **Prefix/Suffix** — Product except self, running sums  
✅ **Monotonic Deque** — Sliding window maximum/minimum  
✅ **Character Frequency** — Anagrams, character counting  
✅ **Sorted Key** — Grouping by equivalence class  

---

## ➡️ Next Steps

Continue to [02_recursion_and_dynamic_programming.md](./02_recursion_and_dynamic_programming.md) to learn recursion and DP patterns.

---

## 🔗 Further Reading

- [LeetCode Explore](https://leetcode.com/explore/)
- [Grokking the Coding Interview](https://www.educative.io/courses/grokking-the-coding-interview)
- [Blind 75 LeetCode Questions](https://leetcode.com/discuss/general-discussion/460599/blind-75-leetcode-questions)
