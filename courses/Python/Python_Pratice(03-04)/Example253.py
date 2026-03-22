# Example253: Dynamic Programming - Fibonacci Variations
# Memoization approach
def fib_memo(n, memo={}):
    """Fibonacci with memoization."""
    if n in memo:
        return memo[n]
    if n <= 1:
        return n
    memo[n] = fib_memo(n - 1, memo) + fib_memo(n - 2, memo)
    return memo[n]

print("Dynamic Programming - Fibonacci:")
print(f"Fib(30): {fib_memo(30)}")

# Bottom-up approach
def fib_bottom_up(n):
    """Fibonacci bottom-up (tabulation)."""
    if n <= 1:
        return n
    dp = [0] * (n + 1)
    dp[1] = 1
    for i in range(2, n + 1):
        dp[i] = dp[i - 1] + dp[i - 2]
    return dp[n]

print(f"Fib(50): {fib_bottom_up(50)}")

# Space-optimized
def fib_optimized(n):
    """Fibonacci with O(1) space."""
    if n <= 1:
        return n
    prev, curr = 0, 1
    for _ in range(2, n + 1):
        prev, curr = curr, prev + curr
    return curr

print(f"Fib(100): {fib_optimized(100)}")

# Climbing stairs problem
def climb_stairs(n):
    """Ways to climb n stairs (1 or 2 steps)."""
    if n <= 2:
        return n
    prev, curr = 1, 2
    for _ in range(3, n + 1):
        prev, curr = curr, prev + curr
    return curr

print("\nClimbing stairs:")
for n in [1, 2, 3, 4, 5]:
    print(f"  {n} stairs: {climb_stairs(n)} ways")

# Minimum cost climbing
def min_cost_climbing(cost):
    """Min cost to reach top."""
    n = len(cost)
    if n == 0:
        return 0
    if n == 1:
        return cost[0]
    prev, curr = cost[0], cost[1]
    for i in range(2, n):
        prev, curr = curr, min(prev, curr) + cost[i]
    return min(prev, curr)

print("\nMin cost climbing:")
cost = [10, 15, 20, 25, 30]
print(f"Cost: {cost}")
print(f"Min cost: {min_cost_climbing(cost)}")

# House robber
def rob(nums):
    """Max money without robbing adjacent houses."""
    if not nums:
        return 0
    if len(nums) == 1:
        return nums[0]
    prev, curr = nums[0], max(nums[0], nums[1])
    for i in range(2, len(nums)):
        prev, curr = curr, max(curr, prev + nums[i])
    return curr

print("\nHouse robber:")
houses = [2, 7, 9, 3, 1]
print(f"Houses: {houses}")
print(f"Max rob: {rob(houses)}")
