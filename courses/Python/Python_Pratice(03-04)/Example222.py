# Example222.py
# Topic: namedtuple Advanced

# This file demonstrates advanced namedtuple patterns.


# ============================================================
# Example 1: Basic namedtuple
# ============================================================
print("=== Basic namedtuple ===")

from collections import namedtuple

Point = namedtuple("Point", ["x", "y"])
p = Point(10, 20)
print(f"Point: {p}")
print(f"x: {p.x}, y: {p.y}")


# ============================================================
# Example 2: Access by Index
# ============================================================
print("\n=== Access by Index ===")

Point = namedtuple("Point", ["x", "y"])
p = Point(10, 20)
print(f"Index 0: {p[0]}")
print(f"Index 1: {p[1]}")


# ============================================================
# Example 3: _asdict
# ============================================================
print("\n=== _asdict ===")

Point = namedtuple("Point", ["x", "y"])
p = Point(10, 20)
print(f"Dict: {p._asdict()}")


# ============================================================
# Example 4: _replace
# ============================================================
print("\n=== _replace ===")

Point = namedtuple("Point", ["x", "y"])
p1 = Point(10, 20)
p2 = p1._replace(x=100)
print(f"Original: {p1}")
print(f"New: {p2}")


# ============================================================
# Example 5: _fields
# ============================================================
print("\n=== _fields ===")

Point = namedtuple("Point", ["x", "y"])
print(f"Fields: {Point._fields}")


# ============================================================
# Example 6: Defaults
# ============================================================
print("\n=== Defaults ===")

from collections import namedtuple

Point = namedtuple("Point", ["x", "y"], defaults=[0, 0])
p = Point(10)
print(f"Point: {p}")


# ============================================================
# Example 7: Inheritance
# ============================================================
print("\n=== Inheritance ===")

from collections import namedtuple

class Point(namedtuple("Point", ["x", "y"])):
    def __str__(self):
        return f"({self.x}, {self.y})"

p = Point(10, 20)
print(f"Custom: {p}")


# ============================================================
# Example 8: Multiple Types
# ============================================================
print("\n=== Multiple Types ===")

from collections import namedtuple

Person = namedtuple("Person", ["name", "age", "city"])
p = Person("Alice", 30, "NYC")
print(f"Person: {p}")


# ============================================================
# Example 9: Tuple Conversion
# ============================================================
print("\n=== Tuple Conversion ===")

Point = namedtuple("Point", ["x", "y"])
p = Point(10, 20)
print(f"Tuple: {tuple(p)}")


# ============================================================
# Example 10: Make
# ============================================================
print("\n=== _make ===")

Point = namedtuple("Point", ["x", "y"])
coords = (10, 20)
p = Point._make(coords)
print(f"Made: {p}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
NAMEDTUPLE:
- _asdict(): to dict
- _replace(): new with changes
- _fields: field names
- _make(): from iterable
""")
