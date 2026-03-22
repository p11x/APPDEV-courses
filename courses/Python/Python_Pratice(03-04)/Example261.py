# Example261: Union-Find (Disjoint Set)
class UnionFind:
    def __init__(self, n):
        self.parent = list(range(n))
        self.rank = [0] * n
    
    def find(self, x):
        if self.parent[x] != x:
            self.parent[x] = self.find(self.parent[x])
        return self.parent[x]
    
    def union(self, x, y):
        px, py = self.find(x), self.find(y)
        if px == py:
            return False
        if self.rank[px] < self.rank[py]:
            px, py = py, px
        self.parent[py] = px
        if self.rank[px] == self.rank[py]:
            self.rank[px] += 1
        return True

print("Union-Find (Disjoint Set):")
uf = UnionFind(5)
uf.union(0, 1)
uf.union(1, 2)
print(f"0 and 2 connected: {uf.find(0) == uf.find(2)}")
print(f"0 and 3 connected: {uf.find(0) == uf.find(3)}")

# Number of islands with Union-Find
def num_islands_uf(grid):
    """Count islands using Union-Find."""
    if not grid:
        return 0
    rows, cols = len(grid), len(grid[0])
    uf = UnionFind(rows * cols)
    directions = [(1,0), (-1,0), (0,1), (0,-1)]
    
    def idx(r, c):
        return r * cols + c
    
    for r in range(rows):
        for c in range(cols):
            if grid[r][c] == '1':
                for dr, dc in directions:
                    nr, nc = r + dr, c + dc
                    if 0 <= nr < rows and 0 <= nc < cols and grid[nr][nc] == '1':
                        uf.union(idx(r, c), idx(nr, nc))
    
    roots = set()
    for r in range(rows):
        for c in range(cols):
            if grid[r][c] == '1':
                roots.add(uf.find(idx(r, c)))
    return len(roots)

print("\nNumber of Islands (Union-Find):")
grid = [
    ['1', '1', '0', '0'],
    ['1', '1', '0', '0'],
    ['0', '0', '1', '0'],
    ['0', '0', '0', '1']
]
print(f"Grid:\n{grid}")
print(f"Islands: {num_islands_uf(grid)}")

# Graph connected components
def count_components(n, edges):
    """Count connected components."""
    uf = UnionFind(n)
    for u, v in edges:
        uf.union(u, v)
    return len(set(uf.find(i) for i in range(n)))

print("\nConnected components:")
n = 5
edges = [(0,1), (1,2), (3,4)]
print(f"Nodes: {n}, Edges: {edges}")
print(f"Components: {count_components(n, edges)}")
