# Example254: Dynamic Programming - 2D Grid Problems
# Unique paths
def unique_paths(m, n):
    """Unique paths in m x n grid."""
    dp = [[1] * n for _ in range(m)]
    for i in range(1, m):
        for j in range(1, n):
            dp[i][j] = dp[i-1][j] + dp[i][j-1]
    return dp[m-1][n-1]

print("DP 2D Grid - Unique paths:")
print(f"3x7 grid: {unique_paths(3, 7)}")

# With obstacles
def unique_paths_with_obstacles(grid):
    """Unique paths with obstacles."""
    m, n = len(grid), len(grid[0])
    dp = [[0] * n for _ in range(m)]
    dp[0][0] = 1 if grid[0][0] == 0 else 0
    for i in range(m):
        for j in range(n):
            if grid[i][j] == 1:
                dp[i][j] = 0
                continue
            if i > 0:
                dp[i][j] += dp[i-1][j]
            if j > 0:
                dp[i][j] += dp[i][j-1]
    return dp[m-1][n-1]

grid = [
    [0, 0, 0],
    [0, 1, 0],
    [0, 0, 0]
]
print(f"With obstacles: {unique_paths_with_obstacles(grid)}")

# Minimum path sum
def min_path_sum(grid):
    """Minimum path sum from top-left to bottom-right."""
    m, n = len(grid), len(grid[0])
    dp = [[0] * n for _ in range(m)]
    dp[0][0] = grid[0][0]
    for i in range(1, m):
        dp[i][0] = dp[i-1][0] + grid[i][0]
    for j in range(1, n):
        dp[0][j] = dp[0][j-1] + grid[0][j]
    for i in range(1, m):
        for j in range(1, n):
            dp[i][j] = min(dp[i-1][j], dp[i][j-1]) + grid[i][j]
    return dp[m-1][n-1]

grid = [
    [1, 3, 1],
    [1, 5, 1],
    [4, 2, 1]
]
print(f"Min path sum: {min_path_sum(grid)}")

# Longest common subsequence
def lcs(s1, s2):
    """Longest common subsequence length."""
    m, n = len(s1), len(s2)
    dp = [[0] * (n + 1) for _ in range(m + 1)]
    for i in range(1, m + 1):
        for j in range(1, n + 1):
            if s1[i-1] == s2[j-1]:
                dp[i][j] = dp[i-1][j-1] + 1
            else:
                dp[i][j] = max(dp[i-1][j], dp[i][j-1])
    return dp[m][n]

print("\nLCS:")
s1, s2 = "ABCDGH", "AEDFHR"
print(f"'{s1}' vs '{s2}': {lcs(s1, s2)}")

# Edit distance
def edit_distance(s1, s2):
    """Minimum edits to convert s1 to s2."""
    m, n = len(s1), len(s2)
    dp = [[0] * (n + 1) for _ in range(m + 1)]
    for i in range(m + 1):
        dp[i][0] = i
    for j in range(n + 1):
        dp[0][j] = j
    for i in range(1, m + 1):
        for j in range(1, n + 1):
            if s1[i-1] == s2[j-1]:
                dp[i][j] = dp[i-1][j-1]
            else:
                dp[i][j] = 1 + min(dp[i-1][j], dp[i][j-1], dp[i-1][j-1])
    return dp[m][n]

print("\nEdit distance:")
print(f"'horse' to 'ros': {edit_distance('horse', 'ros')}")
