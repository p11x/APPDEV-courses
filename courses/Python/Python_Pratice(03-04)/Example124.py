# Example124.py
# Topic: Dataclasses - Basic Usage

from dataclasses import dataclass


# ============================================================
# Example 1: Basic Dataclass
# ============================================================
print("=== Basic Dataclass ===")

@dataclass
class User:
    name: str
    age: int
    email: str

user = User("Alice", 25, "alice@example.com")
print(f"User: {user}")
print(f"Name: {user.name}, Age: {user.age}")


# ============================================================
# Example 2: Default Values
# ============================================================
print("\n=== Default Values ===")

@dataclass
class Product:
    name: str
    price: float
    quantity: int = 0

p1 = Product("Laptop", 999.99)
p2 = Product("Mouse", 29.99, 10)
print(f"p1: {p1}")
print(f"p2: {p2}")


# ============================================================
# Example 3: Mutable Default with field()
# ============================================================
print("\n=== Mutable Default ===")

from dataclasses import dataclass, field

@dataclass
class Book:
    title: str
    author: str
    tags: list[str] = field(default_factory=list)

book = Book("1984", "Orwell")
book.tags.append("dystopian")
book.tags.append("classic")
print(f"Book: {book}")
print(f"Tags: {book.tags}")


# ============================================================
# Example 4: Frozen (Immutable) Dataclass
# ============================================================
print("\n=== Frozen Dataclass ===")

@dataclass(frozen=True)
class Point:
    x: int
    y: int

p = Point(10, 20)
print(f"Point: {p}")
# p.x = 30  # Would raise FrozenInstanceError


# ============================================================
# Example 5: Comparison
# ============================================================
print("\n=== Auto-Generated Methods ===")

@dataclass
class Person:
    name: str
    age: int

p1 = Person("Alice", 30)
p2 = Person("Alice", 30)
p3 = Person("Bob", 25)

print(f"p1 == p2: {p1 == p2}")  # True
print(f"p1 == p3: {p1 == p3}")  # False


# ============================================================
# Example 6: Real-World: Inventory Item
# ============================================================
print("\n=== Real-World Example ===")

@dataclass
class InventoryItem:
    name: str
    unit_price: float
    quantity_on_hand: int = 0
    
    def total_value(self) -> float:
        return self.unit_price * self.quantity_on_hand

item = InventoryItem("Widget", 9.99, 50)
print(f"Item: {item.name}, Total Value: ${item.total_value():.2f}")
