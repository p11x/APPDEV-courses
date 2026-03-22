# Example210.py
# Topic: Advanced Set Operations

# This file demonstrates advanced set operations and patterns.


# ============================================================
# Example 1: Set Union
# ============================================================
print("=== Set Union ===")

A = {1, 2, 3}
B = {3, 4, 5}
print(f"Union: {A | B}")
print(f"Union: {A.union(B)}")


# ============================================================
# Example 2: Set Intersection
# ============================================================
print("\n=== Intersection ===")

A = {1, 2, 3}
B = {2, 3, 4}
print(f"Intersection: {A & B}")


# ============================================================
# Example 3: Set Difference
# ============================================================
print("\n=== Difference ===")

A = {1, 2, 3, 4}
B = {3, 4, 5}
print(f"A-B: {A - B}")
print(f"B-A: {B - A}")


# ============================================================
# Example 4: Symmetric Difference
# ============================================================
print("\n=== Symmetric Difference ===")

A = {1, 2, 3}
B = {2, 3, 4}
print(f"Sym Diff: {A ^ B}")


# ============================================================
# Example 5: Set Comprehension with Condition
# ============================================================
print("\n=== Set Comp ===")

squares = {x**2 for x in range(10)}
print(f"Squares: {squares}")


# ============================================================
# Example 6: Set Methods
# ============================================================
print("\n=== Set Methods ===")

A = {1, 2, 3}
print(f"Add: {A.add(4)}")
print(f"After add: {A}")
print(f"Remove: {A.discard(2)}")
print(f"After discard: {A}")


# ============================================================
# Example 7: Frozen Set
# ============================================================
print("\n=== Frozen Set ===")

fs = frozenset([1, 2, 3])
print(f"Frozen: {fs}")
print(f"Hashable: {hash(fs)}")


# ============================================================
# Example 8: Set Subset Superset
# ============================================================
print("\n=== Subset Superset ===")

A = {1, 2, 3, 4}
B = {2, 3}
print(f"B subset A: {B.issubset(A)}")
print(f"A superset B: {A.issuperset(B)}")


# ============================================================
# Example 9: Set Operations with Update
# ============================================================
print("\n=== Update Operations ===")

A = {1, 2}
A.update({3, 4})
print(f"Update union: {A}")

A = {1, 2, 3}
A.intersection_update({2, 3, 4})
print(f"Update intersection: {A}")


# ============================================================
# Example 10: Set Disjoint
# ============================================================
print("\n=== Disjoint ===")

A = {1, 2, 3}
B = {4, 5, 6}
C = {3, 4, 5}
print(f"A,B disjoint: {A.isdisjoint(B)}")
print(f"A,C disjoint: {A.isdisjoint(C)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
SET OPERATIONS:
- Union: A | B or A.union(B)
- Intersection: A & B
- Difference: A - B
- Symmetric: A ^ B
- Frozen: immutable set
""")
