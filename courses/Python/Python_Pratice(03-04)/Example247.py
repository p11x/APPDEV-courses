# Example247: Named Tuple - Advanced Patterns
from collections import namedtuple

# Basic namedtuple
print("Basic namedtuple:")
Point = namedtuple('Point', ['x', 'y'])
p = Point(10, 20)
print(f"Point: {p}")
print(f"x: {p.x}, y: {p.y}")
print(f"As dict: {p._asdict()}")

# Replace (create modified copy)
print("\nReplace:")
p2 = p._replace(x=30)
print(f"Original: {p}")
print(f"Modified: {p2}")

# Fields and _fields
print("\nFields:")
print(f"Fields: {Point._fields}")

# Make (create from iterable)
print("\nMake:")
coords = [100, 200]
p3 = Point._make(coords)
print(f"From list: {p3}")

# Inheritance with namedtuple
print("\nExtended namedtuple:")
class Point3D(Point):
    pass

Point3D._fields = ('x', 'y', 'z')
p3d = Point3D(1, 2, 3)
print(f"3D Point: {p3d}")

# Practical: CSV-like data
print("\nEmployee record:")
Employee = namedtuple('Employee', ['name', 'dept', 'salary'])
employees = [
    Employee('Alice', 'IT', 70000),
    Employee('Bob', 'Sales', 60000),
    Employee('Charlie', 'IT', 80000)
]
for emp in employees:
    print(f"  {emp.name}: {emp.dept}, ${emp.salary}")

# Group by dept
from collections import defaultdict
by_dept = defaultdict(list)
for emp in employees:
    by_dept[emp.dept].append(emp)
print(f"\nBy department: {dict(by_dept)}")

# Sort by field
print("\nSort by salary:")
sorted_emp = sorted(employees, key=lambda e: e.salary, reverse=True)
for emp in sorted_emp:
    print(f"  {emp.name}: ${emp.salary}")

# _fields defaults (Python 3.7+)
print("\nWith defaults:")
Record = namedtuple('Record', ['id', 'name', 'status'], defaults=['active'])
r = Record(1, 'Test')
print(f"With default: {r}")
