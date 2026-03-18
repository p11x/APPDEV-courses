# Dynamic Programming

## What You'll Learn

- Memoization
- Tabulation
- Solving DP problems

## Prerequisites

- Completed `07-recursion.md`

## What Is Dynamic Programming

Dynamic programming (DP) solves problems by breaking them into overlapping subproblems and storing results to avoid recomputation.

Think of it like a spreadsheet: instead of calculating the same cell multiple times, you calculate it once and reference it.

## When to Use DP

- Problem has optimal substructure
- Problem has overlapping subproblems
- Need to find optimal solution

## Memoization (Top-Down)

```python
from functools import lru_cache

@lru_cache(maxsize=None)
def fibonacci(n: int) -> int:
    """Fibonacci with memoization."""
    if n <= 1:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)
```

## Tabulation (Bottom-Up)

```python
def fibonacci_tab(n: int) -> int:
    """Fibonacci with tabulation."""
    if n <= 1:
        return n
    
    dp = [0] * (n + 1)
    dp[1] = 1
    
    for i in range(2, n + 1):
        dp[i] = dp[i - 1] + dp[i - 2]
    
    return dp[n]

def fibonacci_optimized(n: int) -> int:
    """Fibonacci with O(1) space."""
    if n <= 1:
        return n
    
    prev, curr = 0, 1
    
    for _ in range(2, n + 1):
        prev, curr = curr, prev + curr
    
    return curr
```

## Classic DP Problems

### Climbing Stairs

```python
def climb_stairs(n: int) -> int:
    """Number of ways to climb n stairs."""
    if n <= 2:
        return n
    
    dp = [0] * (n + 1)
    dp[1] = 1
    dp[2] = 2
    
    for i in range(3, n + 1):
        dp[i] = dp[i - 1] + dp[i - 2]
    
    return dp[n]
```

### Coin Change

```python
def coin_change(coins: list[int], amount: int) -> int:
    """Minimum coins needed to make amount."""
    dp = [float('inf')] * (amount + 1)
    dp[0] = 0
    
    for coin in coins:
        for i in range(coin, amount + 1):
            dp[i] = min(dp[i], dp[i - coin] + 1)
    
    return dp[amount] if dp[amount] != float('inf') else -1
```

### Longest Common Subsequence

```python
def lcs(s1: str, s2: str) -> int:
    """Length of longest common subsequence."""
    m, n = len(s1), len(s2)
    dp = [[0] * (n + 1) for _ in range(m + 1)]
    
    for i in range(1, m + 1):
        for j in range(1, n + 1):
            if s1[i - 1] == s2[j - 1]:
                dp[i][j] = dp[i - 1][j - 1] + 1
            else:
                dp[i][j] = max(dp[i - 1][j], dp[i][j - 1])
    
    return dp[m][n]
```

### 0/1 Knapsack

```python
def knapsack(weights: list[int], values: list[int], capacity: int) -> int:
    """Maximum value with given capacity."""
    n = len(weights)
    dp = [[0] * (capacity + 1) for _ in range(n + 1)]
    
    for i in range(1, n + 1):
        for w in range(capacity + 1):
            if weights[i - 1] <= w:
                dp[i][w] = max(
                    dp[i - 1][w],
                    dp[i - 1][w - weights[i - 1]] + values[i - 1]
                )
            else:
                dp[i][w] = dp[i - 1][w]
    
    return dp[n][capacity]
```

## Summary

- Memoization: recursive + cache
- Tabulation: iterative + table
- DP reduces time from exponential to polynomial

## Next Steps

Continue to `09-greedy-algorithms.md`.
