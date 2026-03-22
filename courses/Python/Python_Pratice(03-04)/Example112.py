# Example112.py
# Topic: Advanced Dictionary Operations

# Advanced dictionary patterns and operations.


# ============================================================
# Example 1: Dictionary Methods Deep Dive
# ============================================================
print("=== Dictionary Methods ===")

d = {"a": 1, "b": 2, "c": 3}

# keys(), values(), items()
print(f"Keys: {list(d.keys())}")
print(f"Values: {list(d.values())}")
print(f"Items: {list(d.items())}")

# Iterate
for key in d:
    print(f"Key: {key}")

for k, v in d.items():
    print(f"{k}: {v}")


# ============================================================
# Example 2: Dictionary Update Patterns
# ============================================================
print("\n=== Update Patterns ===")

# setdefault
d = {}
d.setdefault("a", []).append(1)
d.setdefault("a", []).append(2)
print(f"setdefault: {d}")

# Using defaultdict
from collections import defaultdict
dd = defaultdict(list)
dd["a"].extend([1, 2, 3])
print(f"defaultdict: {dict(dd)}")


# ============================================================
# Example 3: Dictionary Comprehension Patterns
# ============================================================
print("\n=== Comprehensions ===")

# Basic
squares = {x: x**2 for x in range(5)}
print(f"Squares: {squares}")

# From two lists
keys = ["a", "b", "c"]
values = [1, 2, 3]
combined = {k: v for k, v in zip(keys, values)}
print(f"Combined: {combined}")

# Filtering
d = {"a": 1, "b": 2, "c": 3, "d": 4}
filtered = {k: v for k, v in d.items() if v > 2}
print(f"Filtered: {filtered}")


# ============================================================
# Example 4: Nested Dictionaries
# ============================================================
print("\n=== Nested Dicts ===")

# Students by grade
students = {
    "A": {"Alice": 95, "Bob": 88},
    "B": {"Charlie": 92, "Diana": 78}
}

print(f"Alice grade: {students['A']['Alice']}")

# Add new
students["C"] = {"Eve": 85}
print(f"Added: {students}")


# ============================================================
# Example 5: Dictionary Sorting
# ============================================================
print("\n=== Sorting ===")

d = {"banana": 3, "apple": 1, "cherry": 2}

# Sort by keys
print(f"By key: {sorted(d.keys())}")

# Sort by values
print(f"By value: {sorted(d.items(), key=lambda x: x[1])}")

# Reverse
print(f"Reverse: {sorted(d.items(), key=lambda x: x[1], reverse=True)}")
