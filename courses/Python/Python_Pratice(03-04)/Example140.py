# Example140.py
# Topic: Working with Named Tuples


# ============================================================
# Example 1: Basic Named Tuple
# ============================================================
print("=== Basic Named Tuple ===")

from collections import namedtuple

Point = namedtuple("Point", ["x", "y"])
p = Point(10, 20)

print(f"Point: {p}")
print(f"p.x: {p.x}")
print(f"p.y: {p.y}")
print(f"p[0]: {p[0]}")


# ============================================================
# Example 2: Named Tuple with Defaults
# ============================================================
print("\n=== Defaults ===")

from collections import namedtuple

Point3D = namedtuple("Point3D", ["x", "y", "z"], defaults=[0, 0, 0])
p1 = Point3D(1, 2)
p2 = Point3D(1, 2, 3)

print(f"p1 (defaults): {p1}")
print(f"p2: {p2}")


# ============================================================
# Example 3: _fields and _asdict
# ============================================================
print("\n=== Methods ===")

from collections import namedtuple

Person = namedtuple("Person", ["name", "age", "city"])

print(f"_fields: {Person._fields}")

p = Person("Alice", 30, "NYC")
print(f"_asdict: {p._asdict()}")
print(f"_replace: {p._replace(age=31)}")


# ============================================================
# Example 4: Named Tuple Inheritance
# ============================================================
print("\n=== Inheritance ===")

from collections import namedtuple

Shape = namedtuple("Shape", ["color"])
Rectangle = namedtuple("Rectangle", Shape._fields + ["width", "height"])

r = Rectangle("red", 10, 5)
print(f"Rectangle: {r}")
print(f"Color: {r.color}")


# ============================================================
# Example 5: Coordinate Systems
# ============================================================
print("\n=== Real-World: Coordinates ===")

from collections import namedtuple

GeoPoint = namedtuple("GeoPoint", ["lat", "lon", "alt"])
Location = namedtuple("Location", ["name", "geo"])

locations = [
    Location("NYC", GeoPoint(40.71, -74.00, 10)),
    Location("LA", GeoPoint(34.05, -118.24, 71)),
]

for loc in locations:
    print(f"{loc.name}: {loc.geo.lat}, {loc.geo.lon}")


# ============================================================
# Example 6: Employee Record
# ============================================================
print("\n=== Real-World: Employee ===")

from collections import namedtuple

Employee = namedtuple("Employee", ["id", "name", "department", "salary"])

employees = [
    Employee(1, "Alice", "IT", 75000),
    Employee(2, "Bob", "HR", 65000),
    Employee(3, "Charlie", "IT", 80000),
]

it_employees = [e for e in employees if e.department == "IT"]
print(f"IT Employees: {it_employees}")

total_salary = sum(e.salary for e in employees)
print(f"Total salary: ${total_salary:,}")
