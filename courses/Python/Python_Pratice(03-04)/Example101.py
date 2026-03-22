# Example101.py
# Topic: Lists In Depth - Advanced List Operations

# This file demonstrates advanced list operations and patterns.


# ============================================================
# Example 1: Advanced List Creation
# ============================================================
print("=== Advanced List Creation ===")

# List from range
numbers = list(range(10))
print(f"From range: {numbers}")

# List from string
chars = list("hello")
print(f"From string: {chars}")

# List from dict keys/values
d = {'a': 1, 'b': 2}
keys = list(d)
values = list(d.values())
items = list(d.items())
print(f"Keys: {keys}, Values: {values}, Items: {items}")

# List comprehension with condition
evens = [x for x in range(10) if x % 2 == 0]
print(f"Evens: {evens}")

# Nested list comprehension
matrix = [[i * j for j in range(3)] for i in range(3)]
print(f"Matrix: {matrix}")


# ============================================================
# Example 2: Advanced Slicing
# ============================================================
print("\n=== Advanced Slicing ===")

numbers = list(range(10))

# Assign to slice
numbers[2:5] = [20, 30, 40]
print(f"After assign: {numbers}")

# Delete slice
numbers = list(range(10))
del numbers[2:5]
print(f"After delete: {numbers}")

# Insert at position
numbers = [1, 2, 3, 4, 5]
numbers[2:2] = [99, 99]
print(f"After insert: {numbers}")

# Negative step
numbers = [1, 2, 3, 4, 5]
print(f"Reverse: {numbers[::-1]}")
print(f"Every other: {numbers[::2]}")


# ============================================================
# Example 3: List Methods Deep Dive
# ============================================================
print("\n=== List Methods Deep Dive ===")

my_list = [1, 2, 3, 2, 4, 2]

# index() with start and end
idx = my_list.index(2, 1, 4)  # Find 2 between index 1 and 4
print(f"Index of 2 (from 1): {idx}")

# count()
print(f"Count of 2: {my_list.count(2)}")

# copy() - shallow copy
original = [1, 2, [3, 4]]
copied = original.copy()
copied[2].append(5)
print(f"Original after copy modify: {original}")  # Shared reference!

# deep copy
import copy
original = [1, 2, [3, 4]]
deep_copied = copy.deepcopy(original)
deep_copied[2].append(5)
print(f"Original after deep copy: {original}")


# ============================================================
# Example 4: In-Place Operations
# ============================================================
print("\n=== In-Place Operations ===")

# sort() options
numbers = [3, 1, 4, 1, 5, 9, 2, 6]
numbers.sort(reverse=True)
print(f"Sorted reverse: {numbers}")

# sorted() - creates new list
numbers = [3, 1, 4, 1, 5]
new_list = sorted(numbers)
print(f"Original: {numbers}, Sorted: {new_list}")

# reverse() vs reversed()
numbers = [1, 2, 3]
numbers.reverse()
print(f"In-place reverse: {numbers}")

numbers = [1, 2, 3]
reversed_list = list(reversed(numbers))
print(f"Reversed: {reversed_list}, Original: {numbers}")


# ============================================================
# Example 5: Performance Considerations
# ============================================================
print("\n=== Performance ===")

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
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Lists In Depth")
print("=" * 50)
print("""
ADVANCED CREATION:
- list(range(n))
- list(string)
- list comprehension

ADVANCED SLICING:
- Assign to slice: list[2:5] = [a, b, c]
- Delete slice: del list[2:5]
- Insert at position: list[2:2] = [a, b]

METHODS:
- index(item, start, end)
- count(item)
- copy() - shallow
- deepcopy() - for nested

PERFORMANCE:
- append() - O(1)
- insert(0) - O(n)
- pop() - O(1)
- pop(0) - O(n)
""")
