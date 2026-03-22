# Example154.py
# Topic: Advanced Pattern Matching


# ============================================================
# Example 1: Matching Sequences
# ============================================================
print("=== Matching Sequences ===")

data: list = [1, 2, 3]

match data:
    case []:
        print("Empty list")
    case [1, 2, 3]:
        print("Exact match: [1, 2, 3]")
    case [first, second]:
        print(f"Two elements: {first}, {second}")
    case [first, *rest]:
        print(f"First: {first}, Rest: {rest}")


# ============================================================
# Example 2: Matching Tuples
# ============================================================
print("\n=== Matching Tuples ===")

point: tuple = (10, 20, 30)

match point:
    case (0, 0, 0):
        print("Origin")
    case (x, y):
        print(f"2D point: {x}, {y}")
    case (x, y, z):
        print(f"3D point: {x}, {y}, {z}")


# ============================================================
# Example 3: Matching Mappings
# ============================================================
print("\n=== Matching Mappings ===")

user: dict = {"name": "Alice", "age": 30}

match user:
    case {"name": name}:
        print(f"User name: {name}")
    case {"name": name, "age": age}:
        print(f"User: {name}, age: {age}")
    case _:
        print("Unknown format")


# ============================================================
# Example 4: Matching with **rest
# ============================================================
print("\n=== Mapping with **rest ===")

config: dict = {"host": "localhost", "port": 8080, "debug": True}

match config:
    case {"host": host, **rest}:
        print(f"Host: {host}")
        print(f"Other settings: {rest}")


# ============================================================
# Example 5: Guard Clauses with Patterns
# ============================================================
print("\n=== Guard Clauses ===")

number: int = 15

match number:
    case x if x < 0:
        print("Negative")
    case x if x % 2 == 0:
        print(f"Even: {x}")
    case x if x % 3 == 0:
        print(f"Divisible by 3: {x}")
    case x:
        print(f"Other: {x}")


# ============================================================
# Example 6: Class Patterns
# ============================================================
print("\n=== Class Patterns ===")

from dataclasses import dataclass

@dataclass
class Point:
    x: int
    y: int

p1 = Point(0, 0)
p2 = Point(10, 20)

match p1:
    case Point(x=0, y=0):
        print("Origin point")
    case Point(x=0):
        print(f"On Y-axis at y={p1.y}")
    case Point(y=0):
        print(f"On X-axis at x={p1.x}")
    case Point(x, y):
        print(f"Point at ({x}, {y})")


# ============================================================
# Example 7: Nested Patterns
# ============================================================
print("\n=== Nested Patterns ===")

data = {"user": {"name": "Alice", "age": 30}, "active": True}

match data:
    case {"user": {"name": name, "age": age}, "active": True}:
        print(f"Active user: {name}, {age}")
    case {"user": {"name": name}}:
        print(f"User: {name}")


# ============================================================
# Example 8: Real-World: HTTP Request Handling
# ============================================================
print("\n=== Real-World: HTTP Request ===")

def handle_request(request: dict) -> str:
    match request:
        case {"method": "GET", "path": path}:
            return f"GET {path}"
        case {"method": "POST", "path": path, "body": body}:
            return f"POST {path} with body"
        case {"method": "PUT", "path": path, "body": body}:
            return f"PUT {path} with body"
        case {"method": "DELETE", "path": path}:
            return f"DELETE {path}"
        case _:
            return "Unknown request"

requests: list[dict] = [
    {"method": "GET", "path": "/users"},
    {"method": "POST", "path": "/users", "body": {"name": "Alice"}},
    {"method": "DELETE", "path": "/users/1"},
    {"method": "PATCH", "path": "/users"},
]

for req in requests:
    print(handle_request(req))
