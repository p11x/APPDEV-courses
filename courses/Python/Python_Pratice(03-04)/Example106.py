# Example106.py
# Topic: Dictionary Comprehensions and Merging

# This file demonstrates dictionary comprehensions and merging.


# ============================================================
# Example 1: Dictionary Comprehensions
# ============================================================
print("=== Dictionary Comprehensions ===")

# Basic
squares = {x: x ** 2 for x in range(5)}
print(f"Squares: {squares}")

# From two lists
keys = ["a", "b", "c"]
values = [1, 2, 3]
combined = {k: v for k, v in zip(keys, values)}
print(f"From zip: {combined}")

# With condition
numbers = {x: x ** 2 for x in range(10) if x % 2 == 0}
print(f"Even squares: {numbers}")


# ============================================================
# Example 2: Dictionary Merging (Python 3.9+)
# ============================================================
print("\n=== Dictionary Merging ===")

d1 = {"a": 1, "b": 2}
d2 = {"b": 3, "c": 4}

# | operator
merged = d1 | d2
print(f"Merged: {merged}")  # b overwritten

# |= operator
d1 |= d2
print(f"After |=: {d1}")


# ============================================================
# Example 3: Default Values
# ============================================================
print("\n=== Default Values ===")

d = {"a": 1}

# setdefault
d.setdefault("b", 2)
d.setdefault("a", 99)  # Not changed
print(f"After setdefault: {d}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
DICT COMPREHENSIONS:
- {k: v for k, v in zip(keys, values)}
- {x: x**2 for x in range(5)}

MERGING (3.9+):
- d1 | d2
- d1 |= d2
""")
