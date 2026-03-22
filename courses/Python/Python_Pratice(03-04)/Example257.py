# Example257: Graph - BFS and DFS
from collections import deque

# Graph representation
graph = {
    'A': ['B', 'C'],
    'B': ['A', 'D', 'E'],
    'C': ['A', 'F'],
    'D': ['B'],
    'E': ['B', 'F'],
    'F': ['C', 'E']
}

def bfs(graph, start):
    """Breadth-first search."""
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

def dfs(graph, start):
    """Depth-first search."""
    visited = set()
    result = []
    def _dfs(node):
        visited.add(node)
        result.append(node)
        for neighbor in graph[node]:
            if neighbor not in visited:
                _dfs(neighbor)
    _dfs(start)
    return result

print("Graph Traversals:")
print(f"BFS from A: {bfs(graph, 'A')}")
print(f"DFS from A: {dfs(graph, 'A')}")

# Number of islands
def num_islands(grid):
    """Count number of islands."""
    if not grid:
        return 0
    rows, cols = len(grid), len(grid[0])
    visited = set()
    islands = 0
    
    def bfs(r, c):
        queue = deque([(r, c)])
        visited.add((r, c))
        while queue:
            row, col = queue.popleft()
            directions = [(1,0), (-1,0), (0,1), (0,-1)]
            for dr, dc in directions:
                nr, nc = row + dr, col + dc
                if 0 <= nr < rows and 0 <= nc < cols:
                    if (nr, nc) not in visited and grid[nr][nc] == '1':
                        visited.add((nr, nc))
                        queue.append((nr, nc))
    
    for r in range(rows):
        for c in range(cols):
            if grid[r][c] == '1' and (r, c) not in visited:
                islands += 1
                bfs(r, c)
    return islands

print("\nNumber of Islands:")
grid = [
    ['1', '1', '0', '0', '0'],
    ['1', '1', '0', '0', '0'],
    ['0', '0', '1', '0', '0'],
    ['0', '0', '0', '1', '1']
]
print(f"Grid:\n{grid}")
print(f"Islands: {num_islands(grid)}")

# Shortest path in unweighted graph
def shortest_path(graph, start, end):
    """BFS shortest path."""
    if start == end:
        return [start]
    visited = set([start])
    queue = deque([(start, [start])])
    while queue:
        node, path = queue.popleft()
        for neighbor in graph[node]:
            if neighbor == end:
                return path + [neighbor]
            if neighbor not in visited:
                visited.add(neighbor)
                queue.append((neighbor, path + [neighbor]))
    return []

print("\nShortest Path:")
print(f"A to F: {shortest_path(graph, 'A', 'F')}")

# Topological sort
def topological_sort(graph):
    """Kahn's algorithm for topological sort."""
    in_degree = {node: 0 for node in graph}
    for node in graph:
        for neighbor in graph[node]:
            in_degree[neighbor] += 1
    queue = deque([node for node in graph if in_degree[node] == 0])
    result = []
    while queue:
        node = queue.popleft()
        result.append(node)
        for neighbor in graph[node]:
            in_degree[neighbor] -= 1
            if in_degree[neighbor] == 0:
                queue.append(neighbor)
    return result

print("\nTopological Sort:")
dag = {
    'A': ['C'],
    'B': ['C', 'D'],
    'C': ['E'],
    'D': ['E'],
    'E': []
}
print(f"DAG: {topological_sort(dag)}")
