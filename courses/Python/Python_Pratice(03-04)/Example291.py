# Example291: Type Hints
from typing import List, Dict, Tuple, Optional, Union

def greet(name: str) -> str:
    return f"Hello, {name}!"

print("Type Hints:")
print(greet("Alice"))

# Complex types
def process_data(items: List[int]) -> Dict[str, int]:
    return {
        "sum": sum(items),
        "count": len(items),
        "avg": sum(items) // len(items)
    }

print("\nComplex types:")
result = process_data([1, 2, 3, 4, 5])
print(result)

# Optional and Union
def find_user(user_id: int) -> Optional[str]:
    users = {1: "Alice", 2: "Bob"}
    return users.get(user_id)

print("\nOptional:")
print(find_user(1))
print(find_user(3))

# Type aliases
Vector = List[float]
Matrix = List[Vector]

def dot_product(v1: Vector, v2: Vector) -> float:
    return sum(a * b for a, b in zip(v1, v2))

print("\nType aliases:")
v1 = [1.0, 2.0, 3.0]
v2 = [4.0, 5.0, 6.0]
print(f"Dot product: {dot_product(v1, v2)}")

# Generics
from typing import TypeVar, Generic

T = TypeVar('T')

class Box(Generic[T]):
    def __init__(self, value: T):
        self.value = value
    
    def get(self) -> T:
        return self.value

print("\nGenerics:")
box = Box(42)
print(f"Box value: {box.get()}")
