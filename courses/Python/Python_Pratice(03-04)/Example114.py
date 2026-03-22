# Example114.py
# Topic: Tuple Advanced Operations

# Advanced tuple operations and patterns.


# ============================================================
# Example 1: Tuple Unpacking
# ============================================================
print("=== Tuple Unpacking ===")

# Basic
point = (10, 20, 30)
x, y, z = point
print(f"x={x}, y={y}, z={z}")

# Extended
first, *middle, last = (1, 2, 3, 4, 5)
print(f"first={first}, middle={middle}, last={last}")

# Swap
a, b = 1, 2
a, b = b, a
print(f"Swapped: a={a}, b={b}")


# ============================================================
# Example 2: Named Tuples Deep Dive
# ============================================================
print("\n=== Named Tuples ===")

from collections import namedtuple

# Basic
Point = namedtuple("Point", ["x", "y"])
p = Point(10, 20)
print(f"x={p.x}, y={p.y}")

# With defaults
Point3D = namedtuple("Point3D", ["x", "y", "z"], defaults=[0, 0])
p3 = Point3D(5, 10)
print(f"z default: {p3.z}")

# _fields and _asdict
print(f"Fields: {Point._fields}")
print(f"As dict: {p._asdict()}")


# ============================================================
# Example 3: Tuple as Dictionary Keys
# ============================================================
print("\n=== Tuple as Keys ===")

# Points as keys
points = {}
points[(0, 0)] = "Origin"
points[(1, 0)] = "Unit X"
points[(0, 1)] = "Unit Y"
print(f"Points: {points}")

# Distance lookup
distances = {
    ("A", "B"): 10,
    ("B", "C"): 15,
    ("A", "C"): 20
}


# ============================================================
# Example 4: Named Tuples Methods
# ============================================================
print("\n=== Named Tuple Methods ===")

from collections import namedtuple

Person = namedtuple("Person", ["name", "age", "city"])

# _make - create from iterable
data = ["Alice", 30, "NYC"]
person = Person._make(data)
print(f"From _make: {person}")

# _replace - create copy with changes
alice = Person("Alice", 30, "NYC")
bob = alice._replace(name="Bob", age=25)
print(f"Original: {alice}")
print(f"Replaced: {bob}")


# ============================================================
# Example 5: Tuple Methods
# ============================================================
print("\n=== Tuple Methods ===")

t = (1, 2, 3, 2, 1)

# count
print(f"Count of 2: {t.count(2)}")

# index
print(f"Index of 3: {t.index(3)}")
