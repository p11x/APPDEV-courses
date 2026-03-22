# Example214.py
# Topic: deque Advanced Patterns

# This file demonstrates advanced deque patterns for queue and sliding window operations.


# ============================================================
# Example 1: Sliding Window Average
# ============================================================
print("=== Sliding Window ===")

from collections import deque

def moving_avg(data, k):
    d = deque(maxlen=k)
    result = []
    for x in data:
        d.append(x)
        if len(d) == k:
            result.append(sum(d) / k)
    return result

data = [1, 2, 3, 4, 5, 6]
print(f"Avg: {moving_avg(data, 3)}")


# ============================================================
# Example 2: Recent Items
# ============================================================
print("\n=== Recent Items ===")

recent = deque(maxlen=5)
for i in range(7):
    recent.append(f"item_{i}")
print(f"Recent: {list(recent)}")


# ============================================================
# Example 3: Rotate Deque
# ============================================================
print("\n=== Rotate ===")

d = deque([1, 2, 3, 4, 5])
d.rotate(2)
print(f"Rotate 2: {list(d)}")

d.rotate(-1)
print(f"Rotate -1: {list(d)}")


# ============================================================
# Example 4: Deque as Stack
# ============================================================
print("\n=== Stack ===")

stack = deque()
stack.append(1)
stack.append(2)
stack.append(3)
print(f"Pop: {stack.pop()}")
print(f"Pop: {stack.pop()}")


# ============================================================
# Example 5: Deque as Queue
# ============================================================
print("\n=== Queue ===")

queue = deque()
queue.append(1)
queue.append(2)
queue.append(3)
print(f"Popleft: {queue.popleft()}")
print(f"Popleft: {queue.popleft()}")


# ============================================================
# Example 6: Max Value in Window
# ============================================================
print("\n=== Max in Window ===")

def max_in_window(arr, k):
    result = []
    dq = deque()
    for i, num in enumerate(arr):
        while dq and dq[-1][0] < num:
            dq.pop()
        dq.append((num, i))
        if dq[0][1] <= i - k:
            dq.popleft()
        if i >= k - 1:
            result.append(dq[0][0])
    return result

nums = [1, 3, -1, -3, 5, 3, 6, 7]
print(f"Max: {max_in_window(nums, 3)}")


# ============================================================
# Example 7: Palindrome Check
# ============================================================
print("\n=== Palindrome ===")

def is_palindrome(s):
    dq = deque(s)
    while len(dq) > 1:
        if dq.popleft() != dq.pop():
            return False
    return True

print(f"'racecar': {is_palindrome('racecar')}")
print(f"'hello': {is_palindrome('hello')}")


# ============================================================
# Example 8: Append Both Sides
# ============================================================
print("\n=== Append Both Sides ===")

d = deque([1, 2, 3])
d.appendleft(0)
d.append(4)
print(f"Both: {list(d)}")


# ============================================================
# Example 9: Extend Both Sides
# ============================================================
print("\n=== Extend ===")

d = deque([3])
d.extendleft([2, 1])
d.extend([4, 5])
print(f"Extended: {list(d)}")


# ============================================================
# Example 10: BFS with Deque
# ============================================================
print("\n=== BFS ===")

from collections import deque

graph = {"A": ["B", "C"], "B": ["D"], "C": []}
queue = deque(["A"])
visited = set()

while queue:
    node = queue.popleft()
    print(f"Visit: {node}")
    for neighbor in graph.get(node, []):
        if neighbor not in visited:
            visited.add(neighbor)
            queue.append(neighbor)


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
DEQUE ADVANCED:
- Sliding window: deque(maxlen=k)
- Rotate: d.rotate(n)
- Max in window: monotonic deque
- Palindrome check
""")
