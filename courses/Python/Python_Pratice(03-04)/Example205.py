# Example205.py
# Topic: Queue Implementation & BFS

# This file demonstrates queue data structure and BFS traversal patterns.


# ============================================================
# Example 1: Basic Queue with deque
# ============================================================
print("=== Basic Queue ===")

from collections import deque

queue = deque()
queue.append(1)
queue.append(2)
queue.append(3)
print(f"Enqueue 1,2,3: {queue}")

front = queue.popleft()
print(f"Dequeue: {front}")    # 1


# ============================================================
# Example 2: Queue Class
# ============================================================
print("\n=== Queue Class ===")

class Queue:
    def __init__(self):
        self._items = deque()
    
    def enqueue(self, item):
        self._items.append(item)
    
    def dequeue(self):
        return self._items.popleft() if self._items else None
    
    def peek(self):
        return self._items[0] if self._items else None
    
    def is_empty(self):
        return len(self._items) == 0

q = Queue()
q.enqueue("A")
q.enqueue("B")
print(f"Peek: {q.peek()}")    # A
print(f"Dequeue: {q.dequeue()}")    # A


# ============================================================
# Example 3: BFS Traversal
# ============================================================
print("\n=== BFS Traversal ===")

from collections import deque

graph = {
    "A": ["B", "C"],
    "B": ["D", "E"],
    "C": ["F"],
    "D": [],
    "E": ["F"],
    "F": []
}

def bfs(graph, start):
    visited = set()
    queue = deque([start])
    visited.add(start)
    result = []
    
    while queue:
        node = queue.popleft()
        result.append(node)
        for neighbor in graph.get(node, []):
            if neighbor not in visited:
                visited.add(neighbor)
                queue.append(neighbor)
    
    return result

print(f"BFS: {bfs(graph, 'A')}")    # ['A', 'B', 'C', 'D', 'E', 'F']


# ============================================================
# Example 4: BFS Shortest Path
# ============================================================
print("\n=== BFS Shortest Path ===")

from collections import deque

def shortest_path(graph, start, end):
    queue = deque([(start, [start])])
    visited = {start}
    
    while queue:
        node, path = queue.popleft()
        if node == end:
            return path
        for neighbor in graph.get(node, []):
            if neighbor not in visited:
                visited.add(neighbor)
                queue.append((neighbor, path + [neighbor]))
    return None

graph = {"A": ["B", "C"], "B": ["D"], "C": ["D"]}
print(f"Path: {shortest_path(graph, 'A', 'D')}")    # ['A', 'B', 'D'] or ['A', 'C', 'D']


# ============================================================
# Example 5: Level Order Traversal
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

print(f"Levels: {level_order(root)}")    # [[1], [2, 3], [4, 5]]


# ============================================================
# Example 6: Task Scheduling
# ============================================================
print("\n=== Task Scheduler ===")

class TaskScheduler:
    def __init__(self):
        self._queue = deque()
    
    def add_task(self, task):
        self._queue.append(task)
    
    def process_next(self):
        return self._queue.popleft() if self._queue else None
    
    def pending(self):
        return len(self._queue)

scheduler = TaskScheduler()
scheduler.add_task("Task 1")
scheduler.add_task("Task 2")
print(f"Pending: {scheduler.pending()}")    # 2
print(f"Processing: {scheduler.process_next()}")    # Task 1


# ============================================================
# Example 7: Print Queue
# ============================================================
print("\n=== Print Queue ===")

from collections import deque

class PrintQueue:
    def __init__(self):
        self._queue = deque()
    
    def add_job(self, doc):
        self._queue.append(doc)
    
    def print_next(self):
        return self._queue.popleft()

pq = PrintQueue()
pq.add_job("Doc1")
pq.add_job("Doc2")
print(f"Printing: {pq.print_next()}")    # Doc1


# ============================================================
# Example 8: Rotating Plate
# ============================================================
print("\n=== Rotating Plate ===")

from collections import deque

plate_queue = deque(range(1, 6))
print(f"Initial: {list(plate_queue)}")

for _ in range(2):
    plate = plate_queue.rotate(1)
print(f"After rotate: {list(plate_queue)}")


# ============================================================
# Example 9: Moving Average
# ============================================================
print("\n=== Moving Average ===")

from collections import deque

class MovingAverage:
    def __init__(self, k):
        self._window = deque(maxlen=k)
        self._sum = 0
    
    def add(self, val):
        if len(self._window) == self._window.maxlen:
            self._sum -= self._window[0]
        self._sum += val
        self._window.append(val)
        return self._sum / len(self._window)

ma = MovingAverage(3)
for v in [1, 2, 3, 4, 5]:
    print(f"Avg: {ma.add(v)}")    # 2.0, 3.0, 4.0


# ============================================================
# Example 10: BFS Matrix Traversal
# ============================================================
print("\n=== BFS Matrix ===")

from collections import deque

def bfs_matrix(matrix, start):
    if not matrix:
        return []
    rows, cols = len(matrix), len(matrix[0])
    visited = set()
    queue = deque([start])
    visited.add(start)
    result = []
    
    while queue:
        r, c = queue.popleft()
        result.append(matrix[r][c])
        
        for dr, dc in [(0,1), (1,0), (0,-1), (-1,0)]:
            nr, nc = r + dr, c + dc
            if 0 <= nr < rows and 0 <= nc < cols and (nr, nc) not in visited:
                visited.add((nr, nc))
                queue.append((nr, nc))
    
    return result

matrix = [[1,2,3],[4,5,6],[7,8,9]]
print(f"BFS: {bfs_matrix(matrix, (0,0))}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
QUEUE (FIFO):
- enqueue: Add to back
- dequeue: Remove from front
- O(1) operations with deque

BFS USES:
- Shortest path in unweighted
- Level order traversal
- Shortest path in grid
- Crawling/web scraping
""")
