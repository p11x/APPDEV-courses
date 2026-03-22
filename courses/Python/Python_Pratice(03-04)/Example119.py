# Example119.py
# Topic: Data Structure Performance

# Performance characteristics of data structures.


# ============================================================
# Example 1: List Operations Complexity
# ============================================================
print("=== List Complexity ===")

import time

# Append vs Insert
n = 10000

start = time.perf_counter()
lst = []
for i in range(n):
    lst.append(i)
append_time = time.perf_counter() - start

start = time.perf_counter()
lst = []
for i in range(n):
    lst.insert(0, i)  # Slow!
insert_time = time.perf_counter() - start

print(f"Append: {append_time:.4f}s")
print(f"Insert(0): {insert_time:.4f}s")


# ============================================================
# Example 2: Set vs List Lookup
# ============================================================
print("\n=== Lookup Performance ===")

import time

n = 10000

# List lookup
lst = list(range(n))
start = time.perf_counter()
for _ in range(100):
    9999 in lst
list_time = time.perf_counter() - start

# Set lookup
s = set(range(n))
start = time.perf_counter()
for _ in range(100):
    9999 in s
set_time = time.perf_counter() - start

print(f"List: {list_time:.4f}s")
print(f"Set: {set_time:.4f}s")


# ============================================================
# Example 3: Dict Access
# ============================================================
print("\n=== Dict Performance ===")

import time

d = {i: i for i in range(10000)}

start = time.perf_counter()
for _ in range(10000):
    d[9999]
dict_time = time.perf_counter() - start

print(f"Dict access: {dict_time:.4f}s")


# ============================================================
# Example 4: When to Use What
# ============================================================
print("\n=== When to Use ===")

print("""
LIST:
- Need ordered sequence
- Need index access
- Need duplicates

TUPLE:
- Immutable data
- Dictionary keys
- Function returns

SET:
- Need unique elements
- Need fast membership test
- Need mathematical sets

DICT:
- Key-value mapping
- Fast lookup by key
- Count occurrences

DEQUE:
- Queue/stack operations
- Need O(1) from both ends
- Sliding window
""")


# ============================================================
# Example 5: Memory Usage
# ============================================================
print("\n=== Memory ===")

import sys

lst = [1, 2, 3, 4, 5]
tup = (1, 2, 3, 4, 5)
s = {1, 2, 3, 4, 5}
d = {i: i for i in range(5)}

print(f"List: {sys.getsizeof(lst)} bytes")
print(f"Tuple: {sys.getsizeof(tup)} bytes")
print(f"Set: {sys.getsizeof(s)} bytes")
print(f"Dict: {sys.getsizeof(d)} bytes")
