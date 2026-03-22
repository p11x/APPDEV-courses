# Example280: Dataclasses Advanced
from dataclasses import dataclass, field
from typing import List

@dataclass
class Person:
    name: str
    age: int
    email: str = ""

# Basic dataclass
print("Dataclass:")
p1 = Person("Alice", 30, "alice@example.com")
p2 = Person("Alice", 30, "alice@example.com")
print(f"p1: {p1}")
print(f"p1 == p2: {p1 == p2}")

# Dataclass with default factory
@dataclass
class Team:
    name: str
    members: List[str] = field(default_factory=list)

print("\nWith default factory:")
team = Team("Developers")
team.members.append("Alice")
team.members.append("Bob")
print(f"Team: {team}")

# Post-init processing
@dataclass
class Employee:
    name: str
    salary: float
    email: str = field(init=False)
    
    def __post_init__(self):
        self.email = f"{self.name.lower().replace(' ', '.')}@company.com"

print("\nPost-init:")
emp = Employee("John Doe", 50000)
print(f"Name: {emp.name}, Email: {emp.email}")

# Frozen dataclass (immutable)
@dataclass(frozen=True)
class Point:
    x: int
    y: int

print("\nFrozen:")
p = Point(1, 2)
# p.x = 3  # Error!

# Comparison control
@dataclass(order=True)
class Item:
    name: str
    price: float
    
    def __lt__(self, other):
        return self.price < other.price

print("\nOrdered:")
items = [Item("B", 20), Item("A", 10), Item("C", 30)]
print(f"Sorted: {sorted(items)}")
