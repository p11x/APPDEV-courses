# Example220.py
# Topic: Queue Applications

# This file demonstrates more queue applications including BFS patterns.


# ============================================================
# Example 1: BFS Template
# ============================================================
print("=== BFS Template ===")

from collections import deque

def bfs(graph, start):
    visited = set([start])
    queue = deque([start])
    result = []
    while queue:
        node = queue.popleft()
        result.append(node)
        for neighbor in graph[node]:
            if neighbor not in visited:
                visited.add(neighbor)
                queue.append(neighbor)
    return result

graph = {"A": ["B", "C"], "B": ["D", "E"], "C": ["F"]}
print(f"BFS: {bfs(graph, 'A')}")


# ============================================================
# Example 2: Level Order Traversal
# ============================================================
print("\n=== Level Order ===")

from collections import deque

class TreeNode:
    def __init__(self, val, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right

root = TreeNode(1, TreeNode(2, TreeNode(4), TreeNode(5)), TreeNode(3))

def level_order(root):
    if not root:
        return []
    queue = deque([root])
    result = []
    while queue:
        level = []
        for _ in range(len(queue)):
            node = queue.popleft()
            level.append(node.val)
            if node.left: queue.append(node.left)
            if node.right: queue.append(node.right)
        result.append(level)
    return result

print(f"Levels: {level_order(root)}")


# ============================================================
# Example 3: Shortest Path
# ============================================================
print("\n=== Shortest Path ===")

from collections import deque

def shortest_path(graph, start, end):
    queue = deque([(start, [start])])
    visited = {start}
    while queue:
        node, path = queue.popleft()
        if node == end:
            return path
        for neighbor in graph[node]:
            if neighbor not in visited:
                visited.add(neighbor)
                queue.append((neighbor, path + [neighbor]))
    return None

graph = {"A": ["B", "C"], "B": ["D"], "C": ["D"]}
print(f"Path A->D: {shortest_path(graph, 'A', 'D')}")


# ============================================================
# Example 4: Number of Islands
# ============================================================
print("\n=== Number of Islands ===")

from collections import deque

def num_islands(grid):
    if not grid:
        return 0
    count = 0
    for i in range(len(grid)):
        for j in range(len(grid[0])):
            if grid[i][j] == '1':
                count += 1
                bfs_island(grid, i, j)
    return count

def bfs_island(grid, i, j):
    queue = deque([(i, j)])
    grid[i][j] = '0'
    while queue:
        r, c = queue.popleft()
        for dr, dc in [(1,0), (-1,0), (0,1), (0,-1)]:
            nr, nc = r + dr, c + dc
            if 0 <= nr < len(grid) and 0 <= nc < len(grid[0]) and grid[nr][nc] == '1':
                grid[nr][nc] = '0'
                queue.append((nr, nc))

grid = [["1","1","1"],["0","1","0"],["1","1","1"]]
print(f"Islands: {num_islands(grid)}")


# ============================================================
# Example 5: Rotting Oranges
# ============================================================
print("\n=== Rotting Oranges ===")

from collections import deque

def oranges_rotting(grid):
    queue = deque()
    fresh = 0
    for i in range(len(grid)):
        for j in range(len(grid[0])):
            if grid[i][j] == 2:
                queue.append((i, j, 0))
            elif grid[i][j] == 1:
                fresh += 1
    minutes = 0
    while queue:
        i, j, t = queue.popleft()
        minutes = max(minutes, t)
        for dr, dc in [(1,0), (-1,0), (0,1), (0,-1)]:
            nr, nc = i + dr, j + dc
            if 0 <= nr < len(grid) and 0 <= nc < len(grid[0]) and grid[nr][nc] == 1:
                grid[nr][nc] = 2
                fresh -= 1
                queue.append((nr, nc, t + 1))
    return minutes if fresh == 0 else -1


# ============================================================
# Example 6: Open Lock
# ============================================================
print("\n=== Open Lock ===")

from collections import deque

def open_lock(deadends, target):
    queue = deque([("0000", 0)])
    visited = set(deadends)
    while queue:
        node, steps = queue.popleft()
        if node == target:
            return steps
        for i in range(4):
            for d in [-1, 1]:
                new = node[:i] + str((int(node[i]) + d) % 10) + node[i+1:]
                if new not in visited:
                    visited.add(new)
                    queue.append((new, steps + 1))
    return -1


# ============================================================
# Example 7: Clone Graph
# ============================================================
print("\n=== Clone Graph ===")

from collections import deque

class Node:
    def __init__(self, val=0, neighbors=None):
        self.val = val
        self.neighbors = neighbors if neighbors else []

def clone_graph(node):
    if not node:
        return None
    queue = deque([node])
    cloned = {node: Node(node.val)}
    while queue:
        curr = queue.popleft()
        for neighbor in curr.neighbors:
            if neighbor not in cloned:
                cloned[neighbor] = Node(neighbor.val)
                queue.append(neighbor)
            cloned[curr].neighbors.append(cloned[neighbor])
    return cloned[node]


# ============================================================
# Example 8: Perfect Squares
# ============================================================
print("\n=== Perfect Squares ===")

from collections import deque

def num_squares(n):
    queue = deque([(n, 0)])
    visited = set()
    while queue:
        num, steps = queue.popleft()
        for i in range(1, int(num**0.5) + 1):
            next_num = num - i*i
            if next_num == 0:
                return steps + 1
            if next_num not in visited:
                visited.add(next_num)
                queue.append((next_num, steps + 1))


# ============================================================
# Example 9: Maze Shortest Path
# ============================================================
print("\n=== Maze Path ===")

from collections import deque

def shortest_path_maze(maze, start, end):
    queue = deque([(start[0], start[1], 0)])
    visited = {start}
    while queue:
        r, c, steps = queue.popleft()
        if (r, c) == end:
            return steps
        for dr, dc in [(1,0), (-1,0), (0,1), (0,-1)]:
            nr, nc = r + dr, c + dc
            if 0 <= nr < len(maze) and 0 <= nc < len(maze[0]) and (nr, nc) not in visited and maze[nr][nc] == 0:
                visited.add((nr, nc))
                queue.append((nr, nc, steps + 1))
    return -1


# ============================================================
# Example 10: Queue Reconstruction
# ============================================================
print("\n=== Reconstruct Queue ===")

def reconstruct_queue(people):
    people.sort(key=lambda x: (-x[0], x[1]))
    result = []
    for p in people:
        result.insert(p[1], p)
    return result


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
QUEUE/BFS:
- Level order traversal
- Shortest path
- Graph traversal
- Matrix BFS
""")
