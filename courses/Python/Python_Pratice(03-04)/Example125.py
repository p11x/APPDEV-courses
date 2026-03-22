# Example125.py
# Topic: Dataclasses - Advanced Features

from dataclasses import dataclass, field
from typing import Optional


# ============================================================
# Example 1: __post_init__ for Validation
# ============================================================
print("=== __post_init__ Validation ===")

@dataclass
class Rectangle:
    width: float
    height: float
    
    def __post_init__(self):
        if self.width <= 0:
            raise ValueError("Width must be positive")
        if self.height <= 0:
            raise ValueError("Height must be positive")
    
    @property
    def area(self) -> float:
        return self.width * self.height

rect = Rectangle(5, 10)
print(f"Area: {rect.area}")

# rect2 = Rectangle(-5, 10)  # Raises ValueError


# ============================================================
# Example 2: Computed Fields with __post_init__
# ============================================================
print("\n=== Computed Fields ===")

@dataclass
class Employee:
    first_name: str
    last_name: str
    hourly_rate: float
    
    full_name: str = field(init=False)
    
    def __post_init__(self):
        self.full_name = f"{self.first_name} {self.last_name}"

emp = Employee("John", "Doe", 25.50)
print(f"Full name: {emp.full_name}")
print(f"Daily rate: ${emp.hourly_rate * 8:.2f}")


# ============================================================
# Example 3: Field Metadata
# ============================================================
print("\n=== Field Metadata ===")

from dataclasses import dataclass, field
from typing import Literal

@dataclass
class Config:
    env: Literal["dev", "prod", "test"]
    debug: bool = False
    max_connections: int = field(default=10, ge=1, le=100)

config = Config("dev", debug=True)
print(f"Env: {config.env}, Debug: {config.debug}")


# ============================================================
# Example 4: slots=True for Memory Efficiency
# ============================================================
print("\n=== Slots for Memory ===")

@dataclass(slots=True)
class Point3D:
    x: float
    y: float
    z: float

p = Point3D(1, 2, 3)
print(f"Point: {p}")


# ============================================================
# Example 5: Optional Fields
# ============================================================
print("\n=== Optional Fields ===")

@dataclass
class User:
    username: str
    email: str
    bio: Optional[str] = None
    avatar_url: Optional[str] = None

u1 = User("alice", "alice@example.com")
u2 = User("bob", "bob@example.com", bio="Developer")
print(f"u1: {u1}")
print(f"u2: {u2}")


# ============================================================
# Example 6: Real-World: Order Processing
# ============================================================
print("\n=== Real-World: Order ===")

@dataclass
class OrderItem:
    product_id: str
    quantity: int
    unit_price: float

@dataclass
class Order:
    order_id: str
    customer_id: str
    items: list[OrderItem]
    shipping_cost: float = 5.99
    
    def total(self) -> float:
        return sum(i.quantity * i.unit_price for i in self.items) + self.shipping_cost

order = Order(
    "ORD-001",
    "CUST-123",
    [
        OrderItem("PROD-1", 2, 19.99),
        OrderItem("PROD-2", 1, 49.99),
    ]
)
print(f"Order total: ${order.total():.2f}")
