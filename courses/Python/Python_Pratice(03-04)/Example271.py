# Example271: Queue Implementations and Patterns
from collections import deque

# Basic queue implementation
class Queue:
    def __init__(self):
        self.items = []
    
    def enqueue(self, item):
        self.items.append(item)
    
    def dequeue(self):
        if self.is_empty():
            return None
        return self.items.pop(0)
    
    def peek(self):
        if self.is_empty():
            return None
        return self.items[0]
    
    def is_empty(self):
        return len(self.items) == 0
    
    def size(self):
        return len(self.items)

print("Queue Implementation:")
q = Queue()
q.enqueue(1)
q.enqueue(2)
q.enqueue(3)
print(f"Peek: {q.peek()}")
print(f"Dequeue: {q.dequeue()}")
print(f"Size: {q.size()}")

# Circular queue
class CircularQueue:
    def __init__(self, capacity):
        self.capacity = capacity
        self.queue = [None] * capacity
        self.front = self.rear = -1
    
    def is_full(self):
        return (self.rear + 1) % self.capacity == self.front
    
    def is_empty(self):
        return self.front == -1
    
    def enqueue(self, item):
        if self.is_full():
            return False
        if self.is_empty():
            self.front = self.rear = 0
        else:
            self.rear = (self.rear + 1) % self.capacity
        self.queue[self.rear] = item
        return True
    
    def dequeue(self):
        if self.is_empty():
            return None
        item = self.queue[self.front]
        if self.front == self.rear:
            self.front = self.rear = -1
        else:
            self.front = (self.front + 1) % self.capacity
        return item

print("\nCircular Queue:")
cq = CircularQueue(5)
for i in range(5):
    cq.enqueue(i + 1)
print(f"Full: {cq.is_full()}")
print(f"Dequeue: {cq.dequeue()}")
cq.enqueue(6)
print(f"After adding 6: {cq.queue}")

# Deque patterns - palindrome check
print("\nDeque palindrome check:")
def is_palindrome_deque(s):
    d = deque(s)
    while len(d) > 1:
        if d.popleft() != d.pop():
            return False
    return True

print(f"'racecar': {is_palindrome_deque('racecar')}")
print(f"'hello': {is_palindrome_deque('hello')}")

# Recent elements with deque
print("\nRecent elements (max 3):")
recent = deque(maxlen=3)
recent.append(1)
recent.append(2)
recent.append(3)
print(f"After 1,2,3: {list(recent)}")
recent.append(4)
print(f"After 4: {list(recent)}")

# BFS with queue
print("\nBFS traversal:")
from collections import deque
graph = {
    'A': ['B', 'C'],
    'B': ['A', 'D'],
    'C': ['A', 'F'],
    'D': ['B'],
    'E': ['F'],
    'F': ['C', 'E']
}

def bfs_traversal(graph, start):
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

print(f"BFS from A: {bfs_traversal(graph, 'A')}")
