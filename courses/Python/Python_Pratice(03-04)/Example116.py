# Example116.py
# Topic: Deque Advanced Operations

# Advanced deque patterns and use cases.


# ============================================================
# Example 1: Deque Basics Review
# ============================================================
print("=== Deque Basics ===")

from collections import deque

dq = deque([1, 2, 3])

# Both ends
dq.appendleft(0)
dq.append(4)
print(f"After: {dq}")

# Pop both ends
left = dq.popleft()
right = dq.pop()
print(f"Left: {left}, Right: {right}, Remaining: {dq}")


# ============================================================
# Example 2: Bounded Queue
# ============================================================
print("\n=== Bounded Queue ===")

from collections import deque

# Maxlen - fixed size
dq = deque(maxlen=3)
dq.append(1)
dq.append(2)
dq.append(3)
print(f"Full: {dq}")

dq.append(4)  # Auto-removes oldest
print(f"After overflow: {dq}")


# ============================================================
# Example 3: Deque for Sliding Window
# ============================================================
print("\n=== Sliding Window ===")

from collections import deque

def sliding_window(data, k):
    result = []
    window = deque()
    
    for i, num in enumerate(data):
        # Remove indices outside window
        while window and window[0] <= i - k:
            window.popleft()
        
        # Remove smaller elements
        while window and data[window[-1]] <= num:
            window.pop()
        
        window.append(i)
        
        if i >= k - 1:
            result.append(data[window[0]])
    
    return result

nums = [1, 3, -1, -3, 5, 3, 6, 7]
print(f"Window size 3: {sliding_window(nums, 3)}")


# ============================================================
# Example 4: Deque for Rotation
# ============================================================
print("\n=== Rotation ===")

from collections import deque

dq = deque([1, 2, 3, 4, 5])

# Rotate right
dq.rotate(2)
print(f"Rotate 2 right: {dq}")

# Rotate left
dq.rotate(-1)
print(f"Rotate 1 left: {dq}")


# ============================================================
# Example 5: Queue Implementation
# ============================================================
print("\n=== Queue ===")

from collections import deque
import time

class Queue:
    def __init__(self):
        self.items = deque()
    
    def enqueue(self, item):
        self.items.append(item)
    
    def dequeue(self):
        return self.items.popleft() if self.items else None
    
    def is_empty(self):
        return len(self.items) == 0

q = Queue()
q.enqueue("A")
q.enqueue("B")
q.enqueue("C")

while not q.is_empty():
    print(f"Dequeue: {q.dequeue()}")
