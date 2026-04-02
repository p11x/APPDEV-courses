# ⏱️ Big-O for Python

> Time and space complexity for Python's built-in operations.

## 🎯 What You'll Learn

- Big-O intuition refresher (no math — just patterns)
- Complete Big-O table for Python built-ins
- Space complexity basics
- Interview patterns and optimization techniques
- 10 interview problems showing naive vs optimised solutions

## 📦 Prerequisites

- Completion of [01_python_gotchas_and_traps.md](./01_python_gotchas_and_traps.md)
- Understanding of basic data structures (list, dict, set)

---

## Big-O Intuition Refresher

Think of Big-O as answering: **"How much slower does it get when I double the input?"**

| Complexity | Meaning | Example |
|------------|---------|---------|
| **O(1)** | Constant time — same speed regardless of size | dict lookup, list append |
| **O(log n)** | Logarithmic — slows down very slowly | binary search |
| **O(n)** | Linear — doubles time when input doubles | list search, loop once |
| **O(n log n)** | Linearithmic — a bit worse than linear | sort(), sorted() |
| **O(n²)** | Quadratic — 4x slower when input doubles | nested loops |
| **O(2ⁿ)** | Exponential — doubles input squares time | naive recursion |

### Visual Intuition

```
Input size:    10    100    1,000    10,000
O(1):          1     1       1         1
O(log n):      3     6      10        13
O(n):         10   100    1,000    10,000
O(n log n):   30   600   10,000   130,000
O(n²):       100 10,000 1,000,000 100,000,000
```

---

## Complete Big-O Table for Python Built-ins

### List Operations
| Operation | Average Case | Worst Case | Notes |
|-----------|--------------|------------|-------|
| `list[i]` | O(1) | O(1) | Indexing |
| `list.append(x)` | O(1) | O(1)* | Amortized |
| `list.insert(i, x)` | O(n) | O(n) | Shifts elements |
| `list.pop()` | O(1) | O(1) | From end |
| `list.pop(i)` | O(n) | O(n) | Shifts elements |
| `x in list` | O(n) | O(n) | Linear search |
| `list.index(x)` | O(n) | O(n) | Linear search |
| `list.count(x)` | O(n) | O(n) | Linear search |
| `list.sort()` | O(n log n) | O(n log n) | Timsort |
| `list.reverse()` | O(n) | O(n) | In-place |
| `list.extend(list)` | O(k) | O(k) | k = len(list) |

*Amortized means occasional O(n) when resizing, but average O(1)

### Dict Operations
| Operation | Average Case | Worst Case | Notes |
|-----------|--------------|------------|-------|
| `d[key]` | O(1) | O(n) | Hash table lookup |
| `d[key] = value` | O(1) | O(n) | Hash table insert |
| `del d[key]` | O(1) | O(n) | Hash table delete |
| `k in d` | O(1) | O(n) | Key lookup |
| `d.get(k, default)` | O(1) | O(n) | Same as lookup |
| `d.copy()` | O(n) | O(n) | Shallow copy |
| `d.update(other)` | O(len(other)) | O(len(other)) |  |

### Set Operations
| Operation | Average Case | Worst Case | Notes |
|-----------|--------------|------------|-------|
| `x in s` | O(1) | O(n) | Hash table lookup |
| `s.add(x)` | O(1) | O(n) | Hash table insert |
| `s.remove(x)` | O(1) | O(n) | Hash table delete |
| `s.discard(x)` | O(1) | O(n) | No error if missing |
| `s.pop()` | O(1) | O(n) | Remove arbitrary element |
| `s.clear()` | O(n) | O(n) | Removes all elements |
| `s1 & s2` | O(min(len(s1), len(s2))) | O(min(len(s1), len(s2))) | Intersection |
| `s1 | s2` | O(len(s1) + len(s2)) | O(len(s1) + len(s2)) | Union |

### Deque Operations (collections.deque)
| Operation | Average Case | Worst Case | Notes |
|-----------|--------------|------------|-------|
| `append(x)` | O(1) | O(1) | Right side |
| `appendleft(x)` | O(1) | O(1) | Left side |
| `pop()` | O(1) | O(1) | Right side |
| `popleft()` | O(1) | O(1) | Left side |
| `rotate(n)` | O(min(n, len(d))) | O(min(n, len(d))) |  |
| `x in d` | O(n) | O(n) | No random access! |

### Heapq Operations
| Operation | Average Case | Worst Case | Notes |
|-----------|--------------|------------|-------|
| `heappush(heap, item)` | O(log n) | O(log n) |  |
| `heappop(heap)` | O(log n) | O(log n) |  |
| `heapify(list)` | O(n) | O(n) | Build heap from list |
| `nlargest(n, iterable)` | O(k log n) | O(k log n) | k = n |
| `nsmallest(n, iterable)` | O(k log n) | O(k log n) | k = n |

### String Operations
| Operation | Average Case | Worst Case | Notes |
|-----------|--------------|------------|-------|
| `len(s)` | O(1) | O(1) |  |
| `s[i]` | O(1) | O(1) | Indexing |
| `s + t` | O(len(s) + len(t)) | O(len(s) + len(t)) | Creates new string |
| `s % args` | O(len(s)) | O(len(s)) | Old formatting |
| `s.format(*args)` | O(len(s)) | O(len(s)) | New formatting |
| `f"..."` | O(len(string)) | O(len(string)) | f-strings |
| `x in s` | O(n) | O(n) | Substring search |
| `s.find(x)` | O(n) | O(n) | Substring search |
| `s.replace(old, new)` | O(n) | O(n) |  |
| `s.split(sep)` | O(n) | O(n) |  |
| `s.join(list)` | O(n) | O(n) | n = total length |

### Sorting and Searching
| Operation | Average Case | Worst Case | Notes |
|-----------|--------------|------------|-------|
| `sorted(iterable)` | O(n log n) | O(n log n) | Returns new list |
| `list.sort()` | O(n log n) | O(n log n) | In-place |
| `min(iterable)` | O(n) | O(n) |  |
| `max(iterable)` | O(n) | O(n) |  |
| `bisect_left(list, x)` | O(log n) | O(log n) | Requires sorted list |
| `bisect_right(list, x)` | O(log n) | O(log n) | Requires sorted list |

---

## Space Complexity Basics

| Type | Description | Example |
|------|-------------|---------|
| **O(1)** | Constant space | Fixed number of variables |
| **O(n)** | Linear space | Proportional to input size |
| **O(n²)** | Quadratic space | Matrix or nested structures |

### In-Place vs Creating New

```python
# In-place - O(1) extra space
def reverse_in_place(lst):
    left, right = 0, len(lst) - 1
    while left < right:
        lst[left], lst[right] = lst[right], lst[left]
        left += 1
        right -= 1

# Creates new - O(n) extra space
def reverse_new(lst):
    return lst[::-1]  # Creates reversed copy
```

---

## Interview Pattern: Always State Complexity

> Before writing code, state the expected time and space complexity.

### Example Interview Question
> "Find the first duplicate in an array of integers."

### Good Answer
> "I can solve this in O(n) time and O(n) space using a set to track seen elements. First, I'll iterate through the array, adding each element to a set. If I encounter an element already in the set, that's the first duplicate. If no duplicates found, return -1."

### Then Write Code
```python
def first_duplicate(arr):
    seen = set()
    for num in arr:
        if num in seen:
            return num
        seen.add(num)
    return -1
# Time: O(n), Space: O(n)
```

---

## 10 Interview Problems: Naive vs Optimised

### 1. Two Sum — Find pair that adds to target

#### ❌ Naive O(n²) Solution
```python
def two_sum(nums, target):
    for i in range(len(nums)):
        for j in range(i + 1, len(nums)):
            if nums[i] + nums[j] == target:
                return [i, j]
    return []
```

#### ✅ Optimised O(n) Solution
```python
def two_sum(nums, target):
    seen = {}  # value -> index
    for i, num in enumerate(nums):
        complement = target - num
        if complement in seen:
            return [seen[complement], i]
        seen[num] = i
    return []
```
**Pattern:** Hash map for complement lookup  
**💡 Explanation:** Store each number's index as we go. For each number, check if we've seen its complement.

---

### 2. Reverse String

#### ❌ Naive O(n²) Solution (string concatenation in loop)
```python
def reverse_string(s):
    result = ""
    for char in s:
        result = char + result  # O(n) each time!
    return result
```

#### ✅ Optimised O(n) Solution
```python
def reverse_string(s):
    return s[::-1]  # Pythonic slicing
# OR
def reverse_string(s):
    return ''.join(reversed(s))
```
**Pattern:** Use built-in operations  
**💡 Explanation:** String concatenation in loop creates new string each time. Slicing is O(n).

---

### 3. FizzBuzz

#### ❌ Naive Approach (not really slower, but verbose)
```python
def fizzbuzz(n):
    result = []
    for i in range(1, n + 1):
        if i % 3 == 0 and i % 5 == 0:
            result.append("FizzBuzz")
        elif i % 3 == 0:
            result.append("Fizz")
        elif i % 5 == 0:
            result.append("Buzz")
        else:
            result.append(str(i))
    return result
```

#### ✅ Optimised Approach (same O(n), cleaner)
```python
def fizzbuzz(n):
    result = []
    for i in range(1, n + 1):
        output = ""
        if i % 3 == 0:
            output += "Fizz"
        if i % 5 == 0:
            output += "Buzz"
        result.append(output or str(i))
    return result
```
**Pattern:** Build output incrementally  
**💡 Explanation:** Avoids nested conditionals by building string incrementally.

---

### 4. Palindrome Check

#### ❌ Naive O(n²) Solution (inefficient reversal)
```python
def is_palindrome(s):
    reversed_s = ""
    for char in s:
        reversed_s = char + reversed_s  # O(n²)!
    return s == reversed_s
```

#### ✅ Optimised O(n) Solution
```python
def is_palindrome(s):
    left, right = 0, len(s) - 1
    while left < right:
        if s[left] != s[right]:
            return False
        left += 1
        right -= 1
    return True
```
**Pattern:** Two-pointer technique  
**💡 Explanation:** Compare characters from both ends moving inward.

---

### 5. Maximum Subarray Sum (Kadane's Algorithm)

#### ❌ Naive O(n³) Solution
```python
def max_subarray(arr):
    max_sum = float('-inf')
    for i in range(len(arr)):
        for j in range(i, len(arr)):
            current_sum = 0
            for k in range(i, j + 1):
                current_sum += arr[k]
            max_sum = max(max_sum, current_sum)
    return max_sum
```

#### ✅ Optimised O(n) Solution
```python
def max_subarray(arr):
    max_current = max_global = arr[0]
    for num in arr[1:]:
        max_current = max(num, max_current + num)
        max_global = max(max_global, max_current)
    return max_global
```
**Pattern:** Dynamic programming - keep running maximum  
**💡 Explanation:** At each position, either start new subarray or extend previous.

---

### 6. Merge Two Sorted Lists

#### ❌ Naive O((n+m) log(n+m)) Solution
```python
def merge_sorted(list1, list2):
    combined = list1 + list2
    combined.sort()
    return combined
```

#### ✅ Optimised O(n+m) Solution
```python
def merge_sorted(list1, list2):
    result = []
    i = j = 0
    
    while i < len(list1) and j < len(list2):
        if list1[i] < list2[j]:
            result.append(list1[i])
            i += 1
        else:
            result.append(list2[j])
            j += 1
    
    # Add remaining elements
    result.extend(list1[i:])
    result.extend(list2[j:])
    
    return result
```
**Pattern:** Two-pointer merge  
**💡 Explanation:** Take smallest element from front of each list.

---

### 7. Valid Parentheses

#### ❌ Naive O(n²) Solution (repeated replacement)
```python
def is_valid(s):
    while '()' in s or '[]' in s or '{}' in s:
        s = s.replace('()', '').replace('[]', '').replace('{}', '')
    return s == ''
```

#### ✅ Optimised O(n) Solution
```python
def is_valid(s):
    stack = []
    mapping = {')': '(', ']': '[', '}': '{'}
    
    for char in s:
        if char in mapping:  # Closing bracket
            if stack and stack[-1] == mapping[char]:
                stack.pop()
            else:
                return False
        else:  # Opening bracket
            stack.append(char)
    
    return not stack
```
**Pattern:** Stack for matching pairs  
**💡 Explanation:** Push opening brackets, pop when matching closing found.

---

### 8. Contains Duplicate

#### ❌ Naive O(n²) Solution
```python
def contains_duplicate(nums):
    for i in range(len(nums)):
        for j in range(i + 1, len(nums)):
            if nums[i] == nums[j]:
                return True
    return False
```

#### ✅ Optimised O(n) Solution
```python
def contains_duplicate(nums):
    return len(nums) != len(set(nums))
# OR
def contains_duplicate(nums):
    seen = set()
    for num in nums:
        if num in seen:
            return True
        seen.add(num)
    return False
```
**Pattern:** Set for deduplication  
**💡 Explanation:** Set eliminates duplicates in O(n) average time.

---

### 9. Single Number (every element appears twice except one)

#### ❌ Naive O(n²) Solution
```python
def single_number(nums):
    for i in range(len(nums)):
        count = 0
        for num in nums:
            if num == nums[i]:
                count += 1
        if count == 1:
            return nums[i]
    return -1
```

#### ✅ Optimised O(n) Solution
```python
def single_number(nums):
    result = 0
    for num in nums:
        result ^= num  # XOR cancels pairs
    return result
```
**Pattern:** Bit manipulation - XOR properties  
**💡 Explanation:** a XOR a = 0, a XOR 0 = a, XOR is commutative and associative.

---

### 10. Intersection of Two Arrays

#### ❌ Naive O(n²) Solution
```python
def intersection(nums1, nums2):
    result = []
    for num in nums1:
        if num in nums2 and num not in result:
            result.append(num)
    return result
```

#### ✅ Optimised O(n+m) Solution
```python
def intersection(nums1, nums2):
    set1 = set(nums1)
    set2 = set(nums2)
    return list(set1 & set2)
# OR for preserving order of nums1:
def intersection(nums1, nums2):
    set2 = set(nums2)
    seen = set()
    result = []
    for num in nums1:
        if num in set2 and num not in seen:
            result.append(num)
            seen.add(num)
    return result
```
**Pattern:** Set intersection  
**💡 Explanation:** Convert to sets, use set intersection operation.

---

## Summary

✅ **O(1)** — dict/set lookup, list append/pop end

✅ **O(n)** — list search, string search, loop once

✅ **O(n log n)** — sort(), sorted(), heap operations

✅ **O(n²)** — nested loops, naive string concatenation

✅ **Space** — distinguish between auxiliary and input space

✅ **Pattern recognition** — hash maps, two pointers, stacks, sets

✅ **Always state complexity** — before and after solution

---

## ➡️ Next Steps

Continue to [03_common_interview_questions.md](./03_common_interview_questions.md) to learn the 25 most common Python interview questions.

---

## 🔗 Further Reading

- [Big-O Cheat Sheet](https://www.bigocheatsheet.com/)
- [Interview Cake - Python](https://www.interviewcake.com/article/python)
- [LeetCode Explore](https://leetcode.com/explore/)
