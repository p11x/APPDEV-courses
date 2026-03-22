# Example98.py
# Topic: Lists - Stacks and Queues

# This file demonstrates using lists as stacks and queues.


# ============================================================
# Example 1: Stack (LIFO)
# ============================================================
print("=== Stack (LIFO) ===")

stack = []

# Push (append)
stack.append(1)
stack.append(2)
stack.append(3)
print(f"After pushes: {stack}")

# Pop
item = stack.pop()
print(f"Popped: {item}, Stack: {item}")

# Peek (without popping)
print(f"Top: {stack[-1]}")

# Check empty
while stack:
    print(f"Popping: {stack.pop()}")
    
print(f"Empty: {not stack}")


# ============================================================
# Example 2: Queue (FIFO)
# ============================================================
print("\n=== Queue (FIFO) ===")

from collections import deque

queue = deque()

# Enqueue (append)
queue.append(1)
queue.append(2)
queue.append(3)
print(f"After enqueues: {queue}")

# Dequeue (popleft)
item = queue.popleft()
print(f"Dequeued: {item}, Queue: {queue}")

# Peek
print(f"Front: {queue[0]}")

# Process all
while queue:
    print(f"Processing: {queue.popleft()}")
    
print(f"Empty: {not queue}")


# ============================================================
# Example 3: List as Queue (Not Recommended)
# ============================================================
print("\n=== List as Queue (Not Recommended) ===")

# Using list as queue (slow!)
queue = [1, 2, 3]

# This is slow for large lists!
item = queue.pop(0)  # Must shift all elements
print(f"Popped: {item}, Queue: {queue}")

# Use deque instead for queues!


# ============================================================
# Example 4: Bounded Stack (with maxsize)
# ============================================================
print("\n=== Bounded Stack ===")

from collections import deque

# Max size stack
stack = deque(maxlen=3)

stack.append(1)
stack.append(2)
stack.append(3)
print(f"Full stack: {stack}")

stack.append(4)  # Automatically removes oldest
print(f"After overflow: {stack}")


# ============================================================
# Example 5: Priority Queue
# ============================================================
print("\n=== Priority Queue ===")

import heapq

# heapq implements min-heap
heap = []

# Push (with priority)
heapq.heappush(heap, (2, "task B"))
heapq.heappush(heap, (1, "task A"))
heapq.heappush(heap, (3, "task C"))
print(f"Heap: {heap}")

# Pop (returns smallest priority)
while heap:
    priority, task = heapq.heappop(heap)
    print(f"Processing: {task} (priority {priority})")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Stacks and Queues")
print("=" * 50)
print("""
STACK (LIFO):
- Use list with append() and pop()
- Perfect for: undo, function calls, expression evaluation

QUEUE (FIFO):
- Use collections.deque with append() and popleft()
- NOT use list.pop(0) - slow!

BOUNDED:
- deque with maxlen parameter
- Automatically evicts oldest item

PRIORITY QUEUE:
- Use heapq (min-heap)
- Push (priority, item) tuples
- heappop() returns lowest priority first
""")
