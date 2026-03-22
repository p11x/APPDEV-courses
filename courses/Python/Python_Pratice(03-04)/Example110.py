# Example110.py
# Topic: Deque and Defaultdict

# This file demonstrates deque and defaultdict from collections.


# ============================================================
# Example 1: Deque
# ============================================================
print("=== Deque ===")

from collections import deque

# Create deque
dq = deque([1, 2, 3])
print(f"Initial: {dq}")

# O(1) operations
dq.appendleft(0)  # Add to left
dq.append(4)       # Add to right
print(f"After appends: {dq}")

left = dq.popleft()  # Remove from left
right = dq.pop()      # Remove from right
print(f"Pop left: {left}, Pop right: {right}, Remaining: {dq}")


# ============================================================
# Example 2: Bounded Deque
# ============================================================
print("\n=== Bounded Deque ===")

# Maxlen - automatically removes old items
dq = deque(maxlen=3)
dq.append(1)
dq.append(2)
dq.append(3)
print(f"Full: {dq}")

dq.append(4)  # Removes oldest
print(f"After overflow: {dq}")


# ============================================================
# Example 3: Defaultdict
# ============================================================
print("\n=== Defaultdict ===")

from collections import defaultdict

# Auto-initialize missing keys
dd = defaultdict(int)
dd["apple"] += 1
dd["apple"] += 1
dd["banana"] += 1
print(f"Counts: {dict(dd)}")

# With list
items = defaultdict(list)
items["fruits"].append("apple")
items["fruits"].append("banana")
items["vegetables"].append("carrot")
print(f"By category: {dict(items)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
DEQUE:
- Double-ended queue
- O(1) append/pop from both ends
- deque(maxlen=N) for bounded queue
- appendleft(), popleft()

DEFAULTDICT:
- Auto-initializes missing keys
- defaultdict(int) - default 0
- defaultdict(list) - default []
- defaultdict(lambda: default) - custom
""")
