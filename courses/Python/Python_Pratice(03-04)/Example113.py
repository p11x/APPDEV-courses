# Example113.py
# Topic: Set Operations Deep Dive

# Advanced set operations and use cases.


# ============================================================
# Example 1: Set Methods
# ============================================================
print("=== Set Methods ===")

a = {1, 2, 3, 4}
b = {3, 4, 5, 6}

# Union
print(f"Union: {a.union(b)}")
print(f"a | b: {a | b}")

# Intersection
print(f"Intersection: {a.intersection(b)}")
print(f"a & b: {a & b}")

# Difference
print(f"Difference: {a.difference(b)}")
print(f"a - b: {a - b}")

# Symmetric difference
print(f"Symmetric: {a.symmetric_difference(b)}")
print(f"a ^ b: {a ^ b}")


# ============================================================
# Example 2: Set Comparison
# ============================================================
print("\n=== Comparison ===")

a = {1, 2, 3}
b = {1, 2, 3, 4}
c = {1, 2, 3}

print(f"a == c: {a == c}")
print(f"a <= b: {a <= b}")  # subset
print(f"a < b: {a < b}")    # proper subset
print(f"b >= a: {b >= a}")  # superset
print(f"b > a: {b > a}")    # proper superset


# ============================================================
# Example 3: Set Operations In-Place
# ============================================================
print("\n=== In-Place Operations ===")

a = {1, 2, 3}
b = {2, 3, 4}

a.update(b)  # union
print(f"update: {a}")

a = {1, 2, 3}
a.intersection_update(b)
print(f"intersection_update: {a}")

a = {1, 2, 3}
a.difference_update(b)
print(f"difference_update: {a}")

a = {1, 2, 3}
a.symmetric_difference_update(b)
print(f"symmetric_difference_update: {a}")


# ============================================================
# Example 4: Practical Set Uses
# ============================================================
print("\n=== Practical Uses ===")

# Find duplicates
items = [1, 2, 3, 2, 1, 4, 5, 3]
duplicates = set(items) ^ set(items)  # Not this way
# Better:
seen = set()
duplicates = set()
for item in items:
    if item in seen:
        duplicates.add(item)
    seen.add(item)
print(f"Duplicates: {duplicates}")

# Common elements
list1 = [1, 2, 3, 4]
list2 = [3, 4, 5, 6]
common = set(list1) & set(list2)
print(f"Common: {common}")


# ============================================================
# Example 5: Frozenset Deep Dive
# ============================================================
print("\n=== Frozenset ===")

# Immutable set
fs = frozenset([1, 2, 3])
print(f"Frozenset: {fs}")

# Can be dict key
mapping = {frozenset([1, 2]): "a", frozenset([3, 4]): "b"}
print(f"Mapping: {mapping}")

# Set of frozensets
s = {frozenset([1, 2]), frozenset([3, 4])}
print(f"Set of frozensets: {s}")
