# Example100.py
# Topic: Comprehensive Review - Lists

# This file provides a comprehensive review of Python lists.


# ============================================================
# Example 1: Creating Lists
# ============================================================
print("=== Creating Lists ===")

# Various ways to create lists
empty = []
with_items = [1, 2, 3]
from_range = list(range(5))
comprehension = [x ** 2 for x in range(5)]
nested = [[1, 2], [3, 4]]

print(f"Empty: {empty}")
print(f"With items: {with_items}")
print(f"From range: {from_range}")
print(f"Comprehension: {comprehension}")
print(f"Nested: {nested}")


# ============================================================
# Example 2: Indexing and Slicing
# ============================================================
print("\n=== Indexing and Slicing ===")

numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]

# Indexing
print(f"First: {numbers[0]}")
print(f"Last: {numbers[-1]}")

# Slicing
print(f"Subset [2:5]: {numbers[2:5]}")
print(f"First 3: {numbers[:3]}")
print(f"Last 3: {numbers[-3:]}")

# Step
print(f"Evens: {numbers[::2]}")
print(f"Odds: {numbers[1::2]}")
print(f"Reverse: {numbers[::-1]}")


# ============================================================
# Example 3: List Methods
# ============================================================
print("\n=== List Methods ===")

my_list = [3, 1, 4, 1, 5, 9, 2, 6]

# Adding
my_list.append(0)
print(f"After append: {my_list}")

my_list.insert(0, 0)
print(f"After insert: {my_list}")

my_list.extend([7, 8])
print(f"After extend: {my_list}")

# Removing
my_list.pop()
print(f"After pop: {my_list}")

my_list.remove(1)
print(f"After remove: {my_list}")

# Sorting
my_list.sort()
print(f"After sort: {my_list}")

my_list.reverse()
print(f"After reverse: {my_list}")


# ============================================================
# Example 4: List Comprehensions
# ============================================================
print("\n=== List Comprehensions ===")

# Basic
squares = [x ** 2 for x in range(5)]
print(f"Squares: {squares}")

# With condition
evens = [x for x in range(10) if x % 2 == 0]
print(f"Evens: {evens}")

# Nested
matrix = [[i * j for j in range(3)] for i in range(3)]
print(f"Matrix: {matrix}")

# With multiple sources
pairs = [(x, y) for x in [1, 2] for y in [3, 4]]
print(f"Pairs: {pairs}")


# ============================================================
# Example 5: Common Patterns
# ============================================================
print("\n=== Common Patterns ===")

# Sum
numbers = [1, 2, 3, 4, 5]
print(f"Sum: {sum(numbers)}")

# Min/Max
print(f"Min: {min(numbers)}")
print(f"Max: {max(numbers)}")

# All/Any
print(f"All positive: {all(x > 0 for x in numbers)}")
print(f"Any > 3: {any(x > 3 for x in numbers)}")

# Enumerate
for i, v in enumerate(['a', 'b', 'c']):
    print(f"Index {i}: {v}")

# Zip
names = ['Alice', 'Bob']
ages = [25, 30]
combined = list(zip(names, ages))
print(f"Zipped: {combined}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMPREHENSIVE SUMMARY: Lists")
print("=" * 50)
print("""
LISTS:
- Ordered, mutable sequences
- 0-indexed
- Can contain mixed types
- Dynamic size

CREATING:
- [] empty list
- [1, 2, 3] literal
- list(iterable)
- [x for x in items] comprehension

INDEXING:
- list[i] - get/set element
- list[-1] - last element

SLICING:
- list[start:end] - subset
- list[start:] - to end
- list[:end] - from start
- list[::step] - with step

METHODS:
- append, extend, insert - add
- pop, remove, clear - remove
- sort, reverse - order
- index, count, in - find

COMPREHENSIONS:
- [expr for x in items]
- [expr for x in items if cond]
""")
