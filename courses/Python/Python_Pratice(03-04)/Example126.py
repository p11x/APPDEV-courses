# Example126.py
# Topic: Typed Collections (Python 3.9+)


# ============================================================
# Example 1: Built-in Generic Types
# ============================================================
print("=== Built-in Generics ===")

numbers: list[int] = [1, 2, 3, 4, 5]
print(f"Numbers: {numbers}")

scores: dict[str, int] = {"Alice": 95, "Bob": 87, "Charlie": 92}
print(f"Scores: {scores}")

unique_names: set[str] = {"apple", "banana", "cherry"}
print(f"Unique: {unique_names}")


# ============================================================
# Example 2: Tuple Types
# ============================================================
print("\n=== Tuple Types ===")

point: tuple[int, int] = (10, 20)
print(f"Point: {point}")

person: tuple[str, int, str] = ("Alice", 30, "NYC")
print(f"Person: {person}")

coords: tuple[int, ...] = (1, 2, 3, 4, 5)
print(f"Coords: {coords}")


# ============================================================
# Example 3: TypeAlias
# ============================================================
print("\n=== TypeAlias ===")

type Matrix = list[list[int]]
type UserDict = dict[str, int]
type Point3D = tuple[float, float, float]

def print_matrix(matrix: Matrix) -> None:
    for row in matrix:
        print(row)

m: Matrix = [[1, 2], [3, 4]]
print_matrix(m)

p: Point3D = (1.5, 2.5, 3.5)
print(f"3D Point: {p}")


# ============================================================
# Example 4: TypeVar Basics
# ============================================================
print("\n=== TypeVar ===")

from typing import TypeVar

T = TypeVar('T')

def first(items: list[T]) -> T | None:
    return items[0] if items else None

result_int = first([1, 2, 3])
result_str = first(["a", "b", "c"])
print(f"First int: {result_int}")
print(f"First str: {result_str}")


# ============================================================
# Example 5: Constrained TypeVar
# ============================================================
print("\n=== Constrained TypeVar ===")

from typing import TypeVar

Numeric = TypeVar('Numeric', int, float)

def sum_values(a: Numeric, b: Numeric) -> Numeric:
    return a + b

print(f"int sum: {sum_values(1, 2)}")
print(f"float sum: {sum_values(1.5, 2.5)}")


# ============================================================
# Example 6: Real-World: API Response Types
# ============================================================
print("\n=== Real-World Example ===")

from typing import TypeAlias

type UserId = int
type JsonDict = dict[str, any]

type UserResponse = dict[str, str | int | bool]
type UserList = list[UserResponse]

def get_users() -> UserList:
    return [
        {"id": 1, "name": "Alice", "active": True},
        {"id": 2, "name": "Bob", "active": False},
    ]

users = get_users()
for user in users:
    print(f"User: {user['name']}, Active: {user['active']}")
