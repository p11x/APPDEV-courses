# Example131.py
# Topic: Advanced Data Structure Patterns


# ============================================================
# Example 1: Stacks (LIFO)
# ============================================================
print("=== Stack (LIFO) ===")

stack = []

stack.append(1)
stack.append(2)
stack.append(3)
print(f"Push 1, 2, 3: {stack}")

print(f"Pop: {stack.pop()}")
print(f"Pop: {stack.pop()}")
print(f"Remaining: {stack}")


# ============================================================
# Example 2: Queue (FIFO)
# ============================================================
print("\n=== Queue (FIFO) ===")

from collections import deque

queue = deque()
queue.append(1)
queue.append(2)
queue.append(3)
print(f"Add 1, 2, 3: {list(queue)}")

print(f"Popleft: {queue.popleft()}")
print(f"Popleft: {queue.popleft()}")
print(f"Remaining: {list(queue)}")


# ============================================================
# Example 3: Priority Queue
# ============================================================
print("\n=== Priority Queue ===")

import heapq

class PriorityQueue:
    def __init__(self):
        self._heap = []
    
    def push(self, item, priority):
        heapq.heappush(self._heap, (priority, item))
    
    def pop(self):
        if self._heap:
            return heapq.heappop(self._heap)[1]
        return None

pq = PriorityQueue()
pq.push("task A", 2)
pq.push("task B", 1)
pq.push("task C", 3)

print(f"Pop: {pq.pop()}")
print(f"Pop: {pq.pop()}")
print(f"Pop: {pq.pop()}")


# ============================================================
# Example 4: Circular Buffer
# ============================================================
print("\n=== Circular Buffer ===")

class CircularBuffer:
    def __init__(self, capacity):
        self.capacity = capacity
        self.buffer = [None] * capacity
        self.head = 0
        self.tail = 0
        self.size = 0
    
    def append(self, item):
        self.buffer[self.tail] = item
        self.tail = (self.tail + 1) % self.capacity
        if self.size < self.capacity:
            self.size += 1
        else:
            self.head = (self.head + 1) % self.capacity
    
    def get_all(self):
        if self.size == 0:
            return []
        if self.tail > self.head:
            return self.buffer[self.head:self.tail]
        return self.buffer[self.head:] + self.buffer[:self.tail]

cb = CircularBuffer(3)
for i in [1, 2, 3]:
    cb.append(i)
print(f"After 1,2,3: {cb.get_all()}")

cb.append(4)
print(f"After 4 (circular): {cb.get_all()}")


# ============================================================
# Example 5:LRU Cache (OrderedDict)
# ============================================================
print("\n=== LRU Cache ===")

from collections import OrderedDict

class LRUCache:
    def __init__(self, capacity):
        self.cache = OrderedDict()
        self.capacity = capacity
    
    def get(self, key):
        if key in self.cache:
            self.cache.move_to_end(key)
            return self.cache[key]
        return None
    
    def put(self, key, value):
        if key in self.cache:
            self.cache.move_to_end(key)
        self.cache[key] = value
        if len(self.cache) > self.capacity:
            self.cache.popitem(last=False)

cache = LRUCache(3)
cache.put("a", 1)
cache.put("b", 2)
cache.put("c", 3)
print(f"Get a: {cache.get('a')}")
cache.put("d", 4)
print(f"Get b (should be None): {cache.get('b')}")


# ============================================================
# Example 6: Graph Adjacency List
# ============================================================
print("\n=== Graph (Adjacency List) ===")

class Graph:
    def __init__(self):
        self.adj = {}
    
    def add_edge(self, u, v):
        if u not in self.adj:
            self.adj[u] = []
        if v not in self.adj:
            self.adj[v] = []
        self.adj[u].append(v)
        self.adj[v].append(u)
    
    def bfs(self, start):
        visited = {start}
        queue = [start]
        while queue:
            node = queue.pop(0)
            print(node, end=" ")
            for neighbor in self.adj.get(node, []):
                if neighbor not in visited:
                    visited.add(neighbor)
                    queue.append(neighbor)

g = Graph()
g.add_edge("A", "B")
g.add_edge("A", "C")
g.add_edge("B", "D")
g.add_edge("C", "D")
print("BFS from A:", end=" ")
g.bfs("A")


# ============================================================
# Example 7: Real-World: Event Bus Pattern
# ============================================================
print("\n=== Real-World: Event Bus ===")

from collections import defaultdict

class EventBus:
    def __init__(self):
        self.listeners = defaultdict(list)
    
    def subscribe(self, event, callback):
        self.listeners[event].append(callback)
    
    def publish(self, event, *args):
        for callback in self.listeners[event]:
            callback(*args)

bus = EventBus()
bus.subscribe("order_placed", lambda order: print(f"Order received: {order}"))
bus.subscribe("order_placed", lambda order: print(f"Processing: {order}"))
bus.publish("order_placed", "Order #123")
