# Example104.py
# Topic: Sets - Basics

# This file demonstrates sets in Python.


# ============================================================
# Example 1: Creating Sets
# ============================================================
print("=== Creating Sets ===")

# Empty set (not {})
empty = set()
print(f"Empty: {empty}")

# With items
fruits = {"apple", "banana", "cherry"}
print(f"Fruits: {fruits}")

# From iterable
from_list = set([1, 2, 3, 2, 1])
print(f"From list: {from_list}")  # Duplicates removed!

# Set comprehension
squares = {x ** 2 for x in range(5)}
print(f"Squares: {squares}")


# ============================================================
# Example 2: Set Operations
# ============================================================
print("\n=== Set Operations ===")

a = {1, 2, 3, 4}
b = {3, 4, 5, 6}

# Union
print(f"Union: {a | b}")
print(f"Union method: {a.union(b)}")

# Intersection
print(f"Intersection: {a & b}")
print(f"Intersection method: {a.intersection(b)}")

# Difference
print(f"Difference (a-b): {a - b}")
print(f"Difference method: {a.difference(b)}")

# Symmetric difference
print(f"Symmetric diff: {a ^ b}")


# ============================================================
# Example 3: Set Methods
# ============================================================
print("\n=== Set Methods ===")

fruits = {"apple", "banana", "cherry"}

# Add
fruits.add("orange")
print(f"After add: {fruits}")

# Remove (raises KeyError if not found)
fruits.remove("banana")
print(f"After remove: {fruits}")

# Discard (no error if not found)
fruits.discard("grape")
print(f"After discard: {fruits}")

# Pop (removes and returns random)
item = fruits.pop()
print(f"Popped: {item}, Remaining: {fruits}")


# ============================================================
# Example 4: Membership and Subsets
# ============================================================
print("\n=== Membership ===")

a = {1, 2, 3}
b = {1, 2, 3, 4, 5}

# Membership
print(f"1 in a: {1 in a}")
print(f"10 in a: {10 in a}")

# Subset
print(f"a <= b: {a <= b}")  # a is subset of b
print(f"a < b: {a < b}")    # proper subset

# Superset
print(f"b >= a: {b >= a}")  # b is superset of a


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Sets")
print("=" * 50)
print("""
SETS:
- Unordered, unique elements
- Created with {}
- set() for empty

OPERATIONS:
- | or union() - all elements
- & or intersection() - common
- - or difference() - in a not b
- ^ or symmetric_difference() - not in both

METHODS:
- add(item) - add element
- remove(item) - remove (KeyError if missing)
- discard(item) - remove (no error)
- pop() - remove and return random

MEMBERSHIP:
- item in set - O(1) lookup
- <= subset, >= superset
""")
