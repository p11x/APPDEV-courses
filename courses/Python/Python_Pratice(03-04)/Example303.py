# Example303: Named Tuples Deep Dive
from collections import namedtuple

# Basic
print("Named Tuples:")
Point = namedtuple('Point', ['x', 'y'])
p = Point(10, 20)
print(f"Point: {p}")
print(f"x: {p.x}, y: {p.y}")

# Methods
print("\nMethods:")
print(f"_asdict: {p._asdict()}")
print(f"_fields: {Point._fields}")

# Replace
p2 = p._replace(x=30)
print(f"Replaced: {p2}")

# Make
p3 = Point._make([40, 50])
print(f"Made: {p3}")

# Defaults (Python 3.7+)
print("\nDefaults:")
from typing import Optional
