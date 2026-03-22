# Example167.py
# Topic: Dataclasses Advanced - Deep Dive


# ============================================================
# Example 1: Dataclass with __post_init__ Validation
# ============================================================
print("=== __post_init__ Validation ===")

from dataclasses import dataclass, field

@dataclass
class Rectangle:
    width: float
    height: float
    
    def __post_init__(self):
        if self.width <= 0:
            raise ValueError(f"Width must be positive, got {self.width}")
        if self.height <= 0:
            raise ValueError(f"Height must be positive, got {self.height}")
    
    @property
    def area(self) -> float:
        return self.width * self.height
    
    @property
    def perimeter(self) -> float:
        return 2 * (self.width + self.height)

rect = Rectangle(5, 10)
print(f"Rectangle: {rect}")
print(f"Area: {rect.area}")
print(f"Perimeter: {rect.perimeter}")

try:
    invalid = Rectangle(-5, 10)
except ValueError as e:
    print(f"Error: {e}")


# ============================================================
# Example 2: Dataclass with Computed Fields
# ============================================================
print("\n=== Computed Fields ===")

from dataclasses import dataclass, field

@dataclass
class Employee:
    first_name: str
    last_name: str
    hourly_rate: float
    hours_worked: float = 0.0
    
    @property
    def full_name(self) -> str:
        return f"{self.first_name} {self.last_name}"
    
    @property
    def total_pay(self) -> float:
        return self.hourly_rate * self.hours_worked

emp = Employee("John", "Doe", 25.50, 40)
print(f"Employee: {emp.full_name}")
print(f"Hourly rate: ${emp.hourly_rate:.2f}")
print(f"Hours worked: {emp.hours_worked}")
print(f"Total pay: ${emp.total_pay:.2f}")


# ============================================================
# Example 3: Dataclass with field() Customization
# ============================================================
print("\n=== Field Customization ===")

from dataclasses import dataclass, field
from typing import List

@dataclass
class Product:
    name: str
    price: float
    quantity: int = 0
    tags: List[str] = field(default_factory=list)
    _id: int = field(default=0, repr=False)
    
    def __post_init__(self):
        if self._id == 0:
            Product._next_id += 1
            self._id = Product._next_id
    
    _next_id = 0

p1 = Product("Laptop", 999.99, quantity=5)
p2 = Product("Mouse", 29.99, tags=["electronics", "peripheral"])
p3 = Product("Keyboard", 79.99, tags=["electronics"])

print(f"p1: {p1}")
print(f"p2: {p2}")
print(f"p3: {p3}")


# ============================================================
# Example 4: Frozen Dataclass (Immutable)
# ============================================================
print("\n=== Frozen Dataclass ===")

from dataclasses import dataclass

@dataclass(frozen=True)
class Point:
    x: int
    y: int
    
    def distance_from_origin(self) -> float:
        return (self.x ** 2 + self.y ** 2) ** 0.5

p = Point(3, 4)
print(f"Point: {p}")
print(f"Distance: {p.distance_from_origin()}")

try:
    p.x = 10
except Exception as e:
    print(f"Error (expected): {type(e).__name__}")


# ============================================================
# Example 5: Slots in Dataclass
# ============================================================
print("\n=== Slots Dataclass ===")

@dataclass(slots=True)
class Person:
    name: str
    age: int
    email: str

person = Person("Alice", 30, "alice@example.com")
print(f"Person: {person}")
print(f"Name: {person.name}")

try:
    person.new_attr = "test"
except AttributeError as e:
    print(f"Slots prevent new attributes: {e}")


# ============================================================
# Example 6: Dataclass Inheritance
# ============================================================
print("\n=== Dataclass Inheritance ===")

from dataclasses import dataclass, field

@dataclass
class Animal:
    name: str
    species: str

@dataclass
class Dog(Animal):
    breed: str = "Unknown"
    bark_volume: int = 5

@dataclass
class Cat(Animal):
    fur_color: str = "Unknown"
    indoor: bool = True

dog = Dog("Buddy", "Canis familiaris", "Labrador", 8)
cat = Cat("Whiskers", "Felis catus", "Orange", False)

print(f"Dog: {dog}")
print(f"Cat: {cat}")


# ============================================================
# Example 7: Dataclass with Custom __eq__
# ============================================================
print("\n=== Custom __eq__ ===")

from dataclasses import dataclass, field

@dataclass
class Version:
    major: int
    minor: int
    patch: int = 0
    
    def __eq__(self, other):
        if not isinstance(other, Version):
            return NotImplemented
        return (self.major, self.minor, self.patch) == (other.major, other.minor, other.patch)
    
    def __lt__(self, other):
        if not isinstance(other, Version):
            return NotImplemented
        return (self.major, self.minor, self.patch) < (other.major, other.minor, other.patch)

v1 = Version(1, 0, 0)
v2 = Version(1, 0, 0)
v3 = Version(2, 0, 0)

print(f"v1 == v2: {v1 == v2}")
print(f"v1 < v3: {v1 < v3}")


# ============================================================
# Example 8: Real-World: Order System with Dataclasses
# ============================================================
print("\n=== Real-World: Order System ===")

from dataclasses import dataclass, field
from datetime import datetime
from typing import List
from enum import Enum

class OrderStatus(Enum):
    PENDING = "pending"
    CONFIRMED = "confirmed"
    SHIPPED = "shipped"
    DELIVERED = "delivered"
    CANCELLED = "cancelled"

@dataclass
class OrderItem:
    product_id: str
    product_name: str
    quantity: int
    unit_price: float
    
    @property
    def total(self) -> float:
        return self.quantity * self.unit_price

@dataclass
class Order:
    order_id: str
    customer_id: str
    items: List[OrderItem] = field(default_factory=list)
    status: OrderStatus = OrderStatus.PENDING
    created_at: datetime = field(default_factory=datetime.now)
    
    @property
    def total(self) -> float:
        return sum(item.total for item in self.items)
    
    def add_item(self, item: OrderItem) -> None:
        self.items.append(item)

order = Order("ORD-001", "CUST-123")
order.add_item(OrderItem("P001", "Laptop", 1, 999.99))
order.add_item(OrderItem("P002", "Mouse", 2, 29.99))
order.status = OrderStatus.CONFIRMED

print(f"Order ID: {order.order_id}")
print(f"Status: {order.status.value}")
print(f"Total: ${order.total:.2f}")
print(f"Created: {order.created_at}")
