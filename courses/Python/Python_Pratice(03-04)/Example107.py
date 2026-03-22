# Example107.py
# Topic: Named Tuples and Frozensets

# This file demonstrates named tuples and frozensets.


# ============================================================
# Example 1: Named Tuples
# ============================================================
print("=== Named Tuples ===")

from collections import namedtuple

# Define namedtuple
Point = namedtuple("Point", ["x", "y"])
Person = namedtuple("Person", ["name", "age"])

# Create instances
p = Point(10, 20)
person = Person("Alice", 30)

# Access by name or index
print(f"Point: x={p.x}, y={p.y}")
print(f"Point[0]: {p[0]}, Point[1]: {p[1]}")
print(f"Person: {person.name}, age {person.age}")


# ============================================================
# Example 2: Named Tuples with Defaults
# ============================================================
print("\n=== Defaults ===")

# With defaults
Point3D = namedtuple("Point3D", ["x", "y", "z"], defaults=[0, 0])
p = Point3D(5, 10)
print(f"Default z: {p.z}")


# ============================================================
# Example 3: Frozensets
# ============================================================
print("\n=== Frozensets ===")

# Create frozenset
frozen = frozenset([1, 2, 3, 2, 1])
print(f"Frozenset: {frozen}")

# As dictionary key
mapping = {
    frozenset([1, 2]): "set1",
    frozenset([3, 4]): "set2"
}
print(f"Mapping: {mapping}")

# Frozensets are immutable
# frozen.add(4)  # AttributeError!


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
NAMEDTUPLES:
- namedtuple("Name", ["field1", "field2"])
- Access by name or index
- defaults parameter for defaults

FROZENSETS:
- frozenset([1, 2, 3])
- Immutable - can be dict keys
- Used for set operations requiring immutability
""")
