# 🔁 Recursion and Dynamic Programming

> 7 solved challenges covering recursion and DP patterns.

## 🎯 What You'll Learn

- Fibonacci — recursion → memoization → bottom-up DP
- Climbing Stairs — classic DP with state transitions
- House Robber — DP with decision at each step
- Coin Change — unbounded knapsack DP pattern
- Longest Common Subsequence — 2D DP table
- Binary Search — recursive vs iterative
- Power Set — recursive backtracking pattern

## 📦 Prerequisites

- Completion of [01_array_and_string_patterns.md](./01_array_and_string_patterns.md)
- Understanding of recursion basics

---

## 1. Fibonacci — Recursion → Memoization → Bottom-Up DP

> **Problem:** Calculate the nth Fibonacci number.

> **Constraints:**
> - 0 <= n <= 30

### 🧠 Approach
Show three approaches: naive recursion (exponential), memoization (top-down DP), and bottom-up DP (iterative).

### ✅ Solution
```python
def fibonacci_naive(n: int) -> int:
    """
    Naive recursive approach - O(2^n) time, O(n) space (call stack).
    
    Args:
        n: Position in Fibonacci sequence
        
    Returns:
        nth Fibonacci number
    """
    if n < 2:
        return n
    return fibonacci_naive(n - 1) + fibonacci_naive(n - 2)


def fibonacci_memo(n: int, memo: dict = None) -> int:
    """
    Memoization (top-down DP) - O(n) time, O(n) space.
    
    Args:
        n: Position in Fibonacci sequence
        memo: Cache for computed values
        
    Returns:
        nth Fibonacci number
    """
    if memo is None:
        memo = {}
    
    if n in memo:
        return memo[n]
    
    if n < 2:
        result = n
    else:
        result = fibonacci_memo(n - 1, memo) + fibonacci_memo(n - 2, memo)
    
    memo[n] = result
    return result


def fibonacci_bottom_up(n: int) -> int:
    """
    Bottom-up DP - O(n) time, O(1) space.
    
    Args:
        n: Position in Fibonacci sequence
        
    Returns:
        nth Fibonacci number
    """
    if n < 2:
        return n
    
    a, b = 0, 1  # F(0), F(1)
    for _ in range(2, n + 1):
        a, b = b, a + b  # Shift forward
    
    return b


# Usage showing evolution
print("Naive (slow for n>30):", fibonacci_naive(10))
print("Memoization:", fibonacci_memo(10))
print("Bottom-up:", fibonacci_bottom_up(10))
```

### ⏱️ Complexity
- Naive: Time O(2^n), Space O(n) (call stack)
- Memoization: Time O(n), Space O(n) (memo + call stack)
- Bottom-up: Time O(n), Space O(1)

### 💡 Pattern Recognition
**Dynamic Programming Evolution:** 
1. Identify overlapping subproblems (fib(n) depends on fib(n-1) and fib(n-2))
2. Naive recursion solves same subproblems repeatedly
3. Memoization caches results to avoid recomputation
4. Bottom-up builds solution iteratively from base cases

### 🔄 Variations
1. **Tribonacci** - F(n) = F(n-1) + F(n-2) + F(n-3)
2. **Climbing Stairs** - Exactly this pattern
3. **House Robber** - Similar decision at each step

### ⚡ Python Trick
Bottom-up approach uses tuple assignment for clean state transition:
```python
a, b = b, a + b  # Simultaneous update
```

### 📊 DP Table Visualization (Bottom-up)
```
n:  0  1  2  3  4  5  6
dp: 0  1  1  2  3  5  8
```

---

## 2. Climbing Stairs — Classic DP

> **Problem:** You are climbing a staircase. It takes n steps to reach the top. Each time you can climb 1 or 2 steps. In how many distinct ways can you climb to the top?

> **Constraints:**
> - 1 <= n <= 45

### 🧠 Approach
This is Fibonacci in disguise! To reach step n, you could have come from step n-1 (1 step) or n-2 (2 steps).

### ✅ Solution
```python
def climb_stairs(n: int) -> int:
    """
    Count distinct ways to climb n steps taking 1 or 2 steps at a time.
    
    Args:
        n: Number of steps
        
    Returns:
        Number of distinct ways
    """
    if n <= 2:
        return n
    
    # dp[i] = ways to reach step i
    # dp[i] = dp[i-1] + dp[i-2]
    
    one_step_before, two_steps_before = 1, 2  # dp[1], dp[2]
    
    for i in range(3, n + 1):
        current = one_step_before + two_steps_before
        two_steps_before = one_step_before
        one_step_before = current
    
    return one_step_before
```

### ⏱️ Complexity
- Time: O(n) - single loop
- Space: O(1) - only two variables needed

### 💡 Pattern Recognition
**State Transition Pattern:** 
- State: dp[i] = number of ways to reach step i
- Transition: dp[i] = dp[i-1] + dp[i-2]
- Base cases: dp[1] = 1, dp[2] = 2

### 🔄 Variations
1. **Climbing Stairs with Variable Steps** - Can take 1, 2, or 3 steps
2. **Jump Game** - Minimum jumps to reach end
3. **Decode Ways** - Number of ways to decode a message

### ⚡ Python Trick
We only need the previous two states, not the entire DP array. This is called "space optimization."

### 📊 State Evolution (n=5)
```
Step: 0  1  2  3  4  5
Ways: 1  1  2  3  5  8
```

---

## 3. House Robber — DP with Decision

> **Problem:** Given an array representing amount of money in each house, return the maximum amount you can rob without alerting police (can't rob adjacent houses).

> **Constraints:**
> - 1 <= nums.length <= 100
> - 0 <= nums[i] <= 400

### 🧠 Approach
At each house i, decide: rob it (can't rob i-1) or skip it (can rob i-1).

### ✅ Solution
```python
def rob(nums: list[int]) -> int:
    """
    Maximum amount that can be robbed without robbing adjacent houses.
    
    Args:
        nums: Money in each house
        
    Returns:
        Maximum amount
    """
    if not nums:
        return 0
    if len(nums) == 1:
        return nums[0]
    
    # dp[i] = max amount from houses[0..i]
    # dp[i] = max(dp[i-1], dp[i-2] + nums[i])
    
    two_houses_back, one_house_back = 0, nums[0]  # dp[-1]=0, dp[0]=nums[0]
    
    for i in range(1, len(nums)):
        current = max(one_house_back, two_houses_back + nums[i])
        two_houses_back = one_house_back
        one_house_back = current
    
    return one_house_back
```

### ⏱️ Complexity
- Time: O(n) - single pass
- Space: O(1) - only two variables

### 💡 Pattern Recognition
**Decision DP Pattern:** 
- State: dp[i] = max amount from first i houses
- Transition: dp[i] = max(dp[i-1], dp[i-2] + nums[i])
  - dp[i-1]: don't rob house i
  - dp[i-2] + nums[i]: rob house i (so can't rob i-1)
- Base cases: dp[0] = nums[0], dp[1] = max(nums[0], nums[1])

### 🔄 Variations
1. **House Robber II** - Houses arranged in circle (break into two cases)
2. **House Robber III** - Binary tree version (tree DP)
3. **Delete and Earn** - Similar but with points for taking all instances

### ⚡ Python Trick
Initialize with two_houses_back = 0 to handle the dp[-1] case cleanly.

### 📊 Decision Flow (nums = [2,7,9,3,1])
```
House: 0   1   2   3   4
Money: 2   7   9   3   1
dp:    2   7  11  12  12
        ^   ^   ^   ^   ^
        |   |   |   |   |
    take2 skip7 take9+2 skip3 take9+3
```

---

## 4. Coin Change — Unbounded Knapsack

> **Problem:** Given coins of different denominations and a total amount, return fewest coins needed to make up that amount. Return -1 if not possible.

> **Constraints:**
> - 1 <= coins.length <= 12
> - 1 <= coins[i] <= 2^31 - 1
> - 0 <= amount <= 10^4

### 🧠 Approach
Unbounded knapsack: each coin can be used unlimited times. dp[i] = min coins to make amount i.

### ✅ Solution
```python
def coin_change(coins: list[int], amount: int) -> int:
    """
    Fewest coins needed to make up amount.
    
    Args:
        coins: Available coin denominations
        amount: Target amount
        
    Returns:
        Fewest coins needed, or -1 if impossible
    """
    # dp[i] = fewest coins to make amount i
    # Initialize with infinity (unreachable)
    dp = [float('inf')] * (amount + 1)
    dp[0] = 0  # Base case: 0 coins needed for amount 0
    
    for i in range(1, amount + 1):
        for coin in coins:
            if i - coin >= 0:
                dp[i] = min(dp[i], dp[i - coin] + 1)
    
    return dp[amount] if dp[amount] != float('inf') else -1
```

### ⏱️ Complexity
- Time: O(amount * len(coins)) - nested loops
- Space: O(amount) - DP array

### 💡 Pattern Recognition
**Unbounded Knapsack Pattern:** 
- State: dp[i] = min coins for amount i
- Transition: dp[i] = min(dp[i], dp[i - coin] + 1) for each coin
- Think of it as: to make amount i, try using each coin and see what's left

### 🔄 Variations
1. **Coin Change 2** - Number of combinations that make up amount
2. **Partition Equal Subset Sum** - Can we split into two equal sum subsets?
3. **Target Sum** - Ways to assign +/- to reach target

### ⚡ Python Trick
Using `float('inf')` as initial value lets us use `min()` naturally. Check for `!= float('inf')` at the end to detect impossibility.

### 📊 DP Table (coins=[1,2,5], amount=11)
```
Amount: 0  1  2  3  4  5  6  7  8  9 10 11
Coins:  0  1  1  2  2  1  2  2  3  3  2  3
```

---

## 5. Longest Common Subsequence — 2D DP

> **Problem:** Given two strings text1 and text2, return the length of their longest common subsequence.

> **Constraints:**
> - 1 <= text1.length, text2.length <= 1000
> - text1 and text2 consist of lowercase English letters.

### 🧠 Approach
Classic 2D DP: dp[i][j] = LCS of text1[0..i-1] and text2[0..j-1].

### ✅ Solution
```python
def longest_common_subsequence(text1: str, text2: str) -> int:
    """
    Length of longest common subsequence.
    
    Args:
        text1: First string
        text2: Second string
        
    Returns:
        Length of LCS
    """
    m, n = len(text1), len(text2)
    
    # dp[i][j] = LCS of text1[:i] and text2[:j]
    dp = [[0] * (n + 1) for _ in range(m + 1)]
    
    for i in range(1, m + 1):
        for j in range(1, n + 1):
            if text1[i - 1] == text2[j - 1]:
                # Characters match - extend LCS
                dp[i][j] = dp[i - 1][j - 1] + 1
            else:
                # Characters don't match - take max of excluding one
                dp[i][j] = max(dp[i - 1][j], dp[i][j - 1])
    
    return dp[m][n]
```

### ⏱️ Complexity
- Time: O(m * n) - fill 2D table
- Space: O(m * n) - DP table

### 💡 Pattern Recognition
**2D DP Table Pattern:** 
- State: dp[i][j] = solution for prefixes of length i and j
- Transition: 
  - If match: dp[i][j] = dp[i-1][j-1] + 1
  - If no match: dp[i][j] = max(dp[i-1][j], dp[i][j-1])
- Fill table row by row or column by column

### 🔄 Variations
1. **Longest Common Substring** - Consecutive match required
2. **Edit Distance** - Minimum operations to convert one string to another
3. **Interleaving String** - Can s3 be formed by interleaving s1 and s2?

### ⚡ Python Trick
We can optimize space to O(min(m,n)) by only keeping current and previous row, but full table is clearer for learning.

### 📊 DP Table Visualization (text1="abcde", text2="ace")
```
    ""  a  c  e
""  0  0  0  0
a   0  1  1  1
b   0  1  1  1
c   0  1  2  2
d   0  1  2  2
e   0  1  2  3
```

---

## 6. Binary Search — Recursive vs Iterative

> **Problem:** Given a sorted array of integers, return index of target if found, otherwise -1.

> **Constraints:**
> - Array is sorted in ascending order

### 🧠 Approach
Show both recursive and iterative implementations, highlighting the off-by-one trap.

### ✅ Solution
```python
def binary_search_recursive(arr: list[int], target: int) -> int:
    """
    Recursive binary search.
    
    Args:
        arr: Sorted array
        target: Value to find
        
    Returns:
        Index of target or -1 if not found
    """
    def search(left: int, right: int) -> int:
        if left > right:
            return -1
        
        mid = left + (right - left) // 2  # Prevents overflow
        
        if arr[mid] == target:
            return mid
        elif arr[mid] < target:
            return search(mid + 1, right)
        else:
            return search(left, mid - 1)
    
    return search(0, len(arr) - 1)


def binary_search_iterative(arr: list[int], target: int) -> int:
    """
    Iterative binary search.
    
    Args:
        arr: Sorted array
        target: Value to find
        
    Returns:
        Index of target or -1 if not found
    """
    left, right = 0, len(arr) - 1
    
    while left <= right:
        mid = left + (right - left) // 2  # Critical: left + (right-left)//2
        
        if arr[mid] == target:
            return mid
        elif arr[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    
    return -1
```

### ⏱️ Complexity
- Time: O(log n) - halve search space each time
- Space: 
  - Recursive: O(log n) (call stack)
  - Iterative: O(1)

### 💡 Pattern Recognition
**Binary Search Pattern:** 
- Maintain search boundaries [left, right]
- Compute mid = left + (right - left) // 2
- If arr[mid] == target: found
- If arr[mid] < target: search right half (left = mid + 1)
- If arr[mid] > target: search left half (right = mid - 1)

### 🔄 Variations
1. **First/Last Position of Element** - Find boundaries
2. **Search in Rotated Sorted Array** - Handle rotation
3. **Find Peak Element** - Find local maximum

### ⚡ Python Trick
Use `left + (right - left) // 2` instead of `(left + right) // 2` to prevent integer overflow in other languages (good habit in Python too).

### 🚫 Off-by-One Trap
Common mistakes:
- `while left < right` (misses case when left == right)
- `right = mid` instead of `right = mid - 1` (infinite loop)
- `left = mid` instead of `left = mid + 1` (infinite loop)

---

## 7. Power Set — Recursive Backtracking

> **Problem:** Given an integer array `nums` of unique elements, return all possible subsets (the power set).

> **Constraints:**
> - 1 <= nums.length <= 10
> - -10 <= nums[i] <= 10

### 🧠 Approach
For each element, decide: include it or exclude it. Build subsets recursively.

### ✅ Solution
```python
def subsets(nums: list[int]) -> list[list[int]]:
    """
    Return all possible subsets (power set).
    
    Args:
        nums: List of unique integers
        
    Returns:
        List of all subsets
    """
    result = []
    
    def backtrack(start: int, current: list[int]):
        # Add current subset to result
        result.append(current.copy())
        
        # Try adding each remaining element
        for i in range(start, len(nums)):
            # Choose
            current.append(nums[i])
            # Explore
            backtrack(i + 1, current)
            # Un-choose (backtrack)
            current.pop()
    
    backtrack(0, [])
    return result
```

### ⏱️ Complexity
- Time: O(n * 2^n) - generate all subsets, each copy takes O(n)
- Space: O(n * 2^n) - store all subsets

### 💡 Pattern Recognition
**Backtracking Pattern:** 
- Make a choice
- Recursively explore further
- Unmake the choice (backtrack)
- Collect results at appropriate times

### 🔄 Variations
1. **Subsets II** - Handle duplicates in input
2. **Combination Sum** - Find combinations that sum to target
3. **Permutations** - Find all permutations of input

### ⚡ Python Trick
Use `current.copy()` when adding to result to avoid storing references to the same list that gets modified later.

### 📊 Decision Tree (nums = [1,2,3])
```
                    []
                 /     \
               [1]      []
              /  \     /  \
            [1,2] [1] [2]  []
           /    \    |    \
        [1,2,3][1,3][2,3][2]
```

---

## Summary

You've learned 7 essential DP/recursion patterns:

✅ **Fibonacci Evolution** — naive → memoization → bottom-up  
✅ **State Transition** — dp[i] depends on previous states  
✅ **Decision DP** — choose or skip at each step  
✅ **Unbounded Knapsack** — unlimited item usage  
✅ **2D DP Table** — dp[i][j] for two sequences  
✅ **Binary Search** — halve search space each time  
✅ **Backtracking** — build solution incrementally  

---

## ➡️ Next Steps

Continue to [03_trees_graphs_and_linked_lists.md](./03_trees_graphs_and_linked_lists.md) to learn tree, graph, and linked list patterns.

---

## 🔗 Further Reading

- [Dynamic Programming](https://www.geeksforgeeks.org/dynamic-programming/)
- [Recursion](https://realpython.com/python-recursion/)
- [LeetCode DP Tag](https://leetcode.com/tag/dynamic-programming/)
