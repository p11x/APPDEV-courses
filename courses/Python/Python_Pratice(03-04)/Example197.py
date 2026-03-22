# Example197.py
# Topic: deque Basics & Advanced

# This file demonstrates deque (double-ended queue) operations
# for efficient appends/pops from both ends.


# ============================================================
# Example 1: Basic deque
# ============================================================
print("=== Basic deque ===")

from collections import deque

d = deque([1, 2, 3])
print(f"Deque: {d}")    # deque([1, 2, 3])


# ============================================================
# Example 2: Append and Pop
# ============================================================
print("\n=== Append and Pop ===")

d = deque([1, 2, 3])
d.append(4)
d.appendleft(0)
print(f"After: {d}")    # deque([0, 1, 2, 3, 4])

item = d.pop()
print(f"Pop: {item}")    # 4
item = d.popleft()
print(f"Popleft: {item}")    # 0


# ============================================================
# Example 3: Maxlen
# ============================================================
print("\n=== Maxlen ===")

d = deque(maxlen=3)
d.append(1)
d.append(2)
d.append(3)
d.append(4)  # 1 is removed
print(f"Fixed: {d}")    # deque([2, 3, 4], maxlen=3)


# ============================================================
# Example 4: Rotation
# ============================================================
print("\n=== Rotation ===")

d = deque([1, 2, 3, 4, 5])
d.rotate(2)
print(f"Rotate 2: {d}")    # deque([4, 5, 1, 2, 3])

d.rotate(-1)
print(f"Rotate -1: {d}")    # deque([5, 1, 2, 3, 4])


# ============================================================
# Example 5: Extend
# ============================================================
print("\n=== Extend ===")

d = deque([1, 2])
d.extend([3, 4])
print(f"Extend: {d}")    # deque([1, 2, 3, 4])

d.extendleft([0, -1])
print(f"Extendleft: {d}")    # deque([-1, 0, 1, 2, 3, 4])


# ============================================================
# Example 6: Window with deque
# ============================================================
print("\n=== Moving Window ===")

def moving_average(data, k):
    d = deque(maxlen=k)
    result = []
    for x in data:
        d.append(x)
        if len(d) == k:
            result.append(sum(d) / k)
    return result

data = [1, 2, 3, 4, 5]
avgs = moving_average(data, 3)
print(f"Moving avg: {avgs}")    # [2.0, 3.0, 4.0]


# ============================================================
# Example 7: Recent Items
# ============================================================
print("\n=== Recent Items ===")

recent = deque(maxlen=5)
recent.append("page1")
recent.append("page2")
recent.append("page3")
recent.append("page4")
recent.append("page5")
recent.append("page6")
print(f"Recent: {list(recent)}")    # ['page2', 'page3', 'page4', 'page5', 'page6']


# ============================================================
# Example 8: Stack with deque
# ============================================================
print("\n=== Stack ===")

stack = deque()
stack.append(1)
stack.append(2)
stack.append(3)
print(f"Pop: {stack.pop()}")    # 3
print(f"Pop: {stack.pop()}")    # 2


# ============================================================
# Example 9: BFS with deque
# ============================================================
print("\n=== BFS ===")

graph = {"A": ["B", "C"], "B": ["D"], "C": []}
queue = deque(["A"])
visited = set()

while queue:
    node = queue.popleft()
    if node not in visited:
        print(f"Visit: {node}")
        visited.add(node)
        queue.extend(graph.get(node, []))


# ============================================================
# Example 10: Clear
# ============================================================
print("\n=== Clear ===")

d = deque([1, 2, 3])
d.clear()
print(f"Cleared: {d}")    # deque([])


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
DEQUE:
- O(1) append/popleft from both ends
- deque(maxlen=N): Fixed size
- rotate(n): Rotate elements
- extend/extendleft

USE CASES:
- Queues and stacks
- Moving windows
- Recent history
- BFS traversal
""")
