# Example103.py
# Topic: Tuples - Basics

# This file demonstrates tuples in Python.


# ============================================================
# Example 1: Creating Tuples
# ============================================================
print("=== Creating Tuples ===")

# Empty tuple
empty = ()
print(f"Empty: {empty}")

# Single element (needs comma!)
single = (1,)
print(f"Single: {single}, type: {type(single)}")

# Multiple elements
point = (10, 20)
colors = ("red", "green", "blue")

# Without parentheses (packing)
packed = 1, 2, 3
print(f"Packed: {packed}")

# From iterable
from_list = tuple([1, 2, 3])
from_range = tuple(range(5))
print(f"From list: {from_list}, From range: {from_range}")


# ============================================================
# Example 2: Tuple Indexing
# ============================================================
print("\n=== Tuple Indexing ===")

point = (10, 20, 30)

# Positive
print(f"First: {point[0]}")
print(f"Second: {point[1]}")

# Negative
print(f"Last: {point[-1]}")
print(f"Second to last: {point[-2]}")


# ============================================================
# Example 3: Tuple Unpacking
# ============================================================
print("\n=== Tuple Unpacking ===")

# Basic unpacking
coordinates = (10, 20, 30)
x, y, z = coordinates
print(f"x={x}, y={y}, z={z}")

# Swap values
a, b = 1, 2
a, b = b, a
print(f"Swapped: a={a}, b={b}")

# Extended unpacking
first, *middle, last = [1, 2, 3, 4, 5]
print(f"first={first}, middle={middle}, last={last}")


# ============================================================
# Example 4: Tuple Methods
# ============================================================
print("\n=== Tuple Methods ===")

my_tuple = (1, 2, 3, 2, 4, 2)

# count()
print(f"Count of 2: {my_tuple.count(2)}")

# index()
print(f"Index of 3: {my_tuple.index(3)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Tuples")
print("=" * 50)
print("""
TUPLES:
- Immutable, ordered sequences
- Created with () or packing
- Single element needs trailing comma

INDEXING:
- Same as lists [0], [-1]
- Immutable - can't assign!

UNPACKING:
- x, y = tuple
- Swap: a, b = b, a
- Extended: first, *rest, last = tuple

METHODS:
- count(item)
- index(item)
""")
