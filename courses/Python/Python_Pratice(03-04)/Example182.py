# Example182.py
# Topic: Queue Implementation

# This file demonstrates queue data structure implementation.
# Queue is FIFO (First In, First Out) - like a line at a store.
# Operations: enqueue, dequeue, peek, is_empty.


# ============================================================
# Example 1: Basic Queue with deque
# ============================================================
print("=== Basic Queue with deque ===")

from collections import deque

queue = deque()    # deque — efficient queue

queue.append(1)    # Enqueue 1
queue.append(2)    # Enqueue 2
queue.append(3)    # Enqueue 3
print(f"After enqueues: {queue}")    # deque([1, 2, 3])

front = queue.popleft()    # int — dequeue front
print(f"Dequeued: {front}")    # Dequeued: 1
print(f"After dequeue: {queue}")    # deque([2, 3])


# ============================================================
# Example 2: Queue Class Implementation
# ============================================================
print("\n=== Queue Class ===")

class Queue:
    def __init__(self):
        self._items = deque()
    
    def enqueue(self, item):
        self._items.append(item)
    
    def dequeue(self):
        if self.is_empty():
            raise IndexError("dequeue from empty queue")
        return self._items.popleft()
    
    def peek(self):
        if self.is_empty():
            raise IndexError("peek from empty queue")
        return self._items[0]
    
    def is_empty(self) -> bool:
        return len(self._items) == 0
    
    def size(self) -> int:
        return len(self._items)
    
    def __repr__(self):
        return f"Queue({list(self._items)})"

q = Queue()
q.enqueue("A")
q.enqueue("B")
q.enqueue("C")

print(f"Queue: {q}")    # Queue(['A', 'B', 'C'])
print(f"Peek: {q.peek()}")    # Peek: A
print(f"Size: {q.size()}")    # Size: 3

q.dequeue()    # Remove front
print(f"After dequeue: {q}")    # Queue(['B', 'C'])


# ============================================================
# Example 3: Task Scheduling
# ============================================================
print("\n=== Task Scheduler ===")

class Task:
    def __init__(self, name: str, priority: int = 0):
        self.name = name
        self.priority = priority
    
    def __repr__(self):
        return f"Task({self.name}, {self.priority})"

class TaskScheduler:
    def __init__(self):
        self._queue = Queue()
    
    def add_task(self, name: str, priority: int = 0):
        task = Task(name, priority)
        self._queue.enqueue(task)
    
    def execute_next(self):
        if self._queue.is_empty():
            return None
        task = self._queue.dequeue()
        print(f"Executing: {task.name}")
        return task
    
    def pending_count(self) -> int:
        return self._queue.size()

scheduler = TaskScheduler()
scheduler.add_task("Backup", 2)
scheduler.add_task("Email", 1)
scheduler.add_task("Report", 3)

print(f"Pending: {scheduler.pending_count()}")    # Pending: 3
scheduler.execute_next()    # Execute: Backup
scheduler.execute_next()    # Execute: Email
print(f"Remaining: {scheduler.pending_count()}")    # Remaining: 1


# ============================================================
# Example 4: BFS (Breadth-First Search)
# ============================================================
print("\n=== BFS Traversal ===")

from collections import deque

def bfs(graph: dict, start: str) -> list:
    visited = set()
    queue = Queue()
    result = []
    
    queue.enqueue(start)
    visited.add(start)
    
    while not queue.is_empty():
        node = queue.dequeue()
        result.append(node)
        
        for neighbor in graph.get(node, []):
            if neighbor not in visited:
                visited.add(neighbor)
                queue.enqueue(neighbor)
    
    return result

graph = {
    "A": ["B", "C"],
    "B": ["D", "E"],
    "C": ["F"],
    "D": [],
    "E": ["F"],
    "F": []
}

path = bfs(graph, "A")    # list — BFS traversal
print(f"BFS: {path}")    # BFS: ['A', 'B', 'C', 'D', 'E', 'F']


# ============================================================
# Example 5: Print Queue Simulation
# ============================================================
print("\n=== Print Queue ===")

class PrintJob:
    def __init__(self, name: str, pages: int):
        self.name = name
        self.pages = pages
    
    def __repr__(self):
        return f"Job({self.name}, {self.pages} pages)"

class PrintQueue:
    def __init__(self):
        self._queue = Queue()
    
    def add_job(self, name: str, pages: int):
        job = PrintJob(name, pages)
        self._queue.enqueue(job)
        print(f"Added: {job}")
    
    def print_next(self):
        if self._queue.is_empty():
            print("No jobs in queue")
            return
        job = self._queue.dequeue()
        print(f"Printing: {job}")
    
    def queue_size(self) -> int:
        return self._queue.size()

printer = PrintQueue()
printer.add_job("Document1", 5)
printer.add_job("Spreadsheet", 10)
printer.add_job("Photo", 2)

print(f"\nQueue size: {printer.queue_size()}")    # Queue size: 3
printer.print_next()    # Print: Document1
printer.print_next()    # Print: Spreadsheet
print(f"Remaining: {printer.queue_size()}")    # Remaining: 1


# ============================================================
# Example 6: Producer-Consumer Pattern
# ============================================================
print("\n=== Producer-Consumer ===")

import time
import random

class MessageQueue:
    def __init__(self):
        self._queue = Queue()
    
    def put(self, message: str):
        self._queue.enqueue(message)
    
    def get(self) -> str:
        if self._queue.is_empty():
            return None
        return self._queue.dequeue()

queue = MessageQueue()

def producer():
    for i in range(3):
        queue.put(f"Message {i}")
        time.sleep(0.1)

def consumer():
    while True:
        msg = queue.get()
        if msg:
            print(f"Got: {msg}")
        time.sleep(0.15)

producer()
print("---")
while not queue._queue.is_empty():
    msg = queue.get()
    print(f"Got: {msg}")


# ============================================================
# Example 7: Deque-based Double-Ended Queue
# ============================================================
print("\n=== Deque Operations ===")

from collections import deque

d = deque([1, 2, 3])

# Both ends
d.appendleft(0)    # Add to front
d.append(4)    # Add to back
print(f"After appends: {d}")    # deque([0, 1, 2, 3, 4])

front = d.popleft()    # int — from front
back = d.pop()    # int — from back
print(f"Front: {front}, Back: {back}")    # Front: 0, Back: 4
print(f"Remaining: {d}")    # deque([1, 2, 3])


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
QUEUE (FIFO):
- enqueue(item): Add to back
- dequeue(): Remove from front
- peek(): View front without removing
- is_empty(): Check if empty
- size(): Get number of elements

USES:
- Task scheduling
- BFS traversal
- Print queues
- Message passing
- Producer-consumer patterns

COLLECTIONS.DEQUE:
- O(1) for append/popleft
- Supports both ends
- Thread-safe variants available
""")
