# Instance and Class Variables

## What You'll Learn

- Understanding instance variables
- Understanding class variables
- Mutable class variable pitfalls
- Best practices for variable organization

## Prerequisites

- Read [01_defining_classes.md](./01_defining_classes.md) first

## Instance Variables

Instance variables are defined inside __init__ and are unique to each instance.

```python
# instance_variables.py

class Car:
    def __init__(self, make: str, model: str) -> None:
        # Instance variables
        self.make = make
        self.model = model
        self.mileage = 0
    
    def drive(self, miles: int) -> None:
        self.mileage += miles


car1 = Car("Toyota", "Camry")
car2 = Car("Honda", "Accord")

car1.drive(100)
print(f"Car1 mileage: {car1.mileage}")
print(f"Car2 mileage: {car2.mileage}")  # Independent
```

## Class Variables

Class variables are shared across all instances of a class.

```python
# class_variables.py

class Employee:
    # Class variable
    company_name: str = "Tech Corp"
    
    def __init__(self, name: str, salary: float) -> None:
        # Instance variables
        self.name = name
        self.salary = salary


emp1 = Employee("Alice", 50000)
emp2 = Employee("Bob", 60000)

print(Employee.company_name)
print(emp1.company_name)
print(emp2.company_name)
```

## Mutable Class Variable Pitfall

Be careful with mutable class variables - they can cause unexpected behavior.

```python
# mutable_pitfall.py

class Team:
    # Problem: mutable default value
    members: list[str] = []
    
    def __init__(self, name: str) -> None:
        self.name = name
        # Should be: self.members = []
    
    def add_member(self, name: str) -> None:
        self.members.append(name)


team1 = Team("Alpha")
team2 = Team("Beta")

team1.add_member("Alice")
team2.add_member("Bob")

print(team1.members)  # ['Alice', 'Bob'] - BUG!
print(team2.members)  # ['Alice', 'Bob'] - BUG!
```

## Annotated Full Example

```python
# instance_class_vars_demo.py
"""Complete demonstration of instance and class variables."""

from typing import Optional


class Restaurant:
    """A restaurant with instance and class variables."""
    
    # Class variables
    cuisine_type: str = "Italian"
    total_served: int = 0
    
    def __init__(self, name: str, rating: float = 0.0) -> None:
        """Initialize restaurant instance."""
        self.name = name
        self.rating = rating
        self.menu_items: dict[str, float] = {}
        Restaurant.total_served += 1
    
    def add_item(self, item: str, price: float) -> None:
        """Add item to menu."""
        self.menu_items[item] = price
    
    def __str__(self) -> str:
        return f"{self.name} ({self.cuisine_type}) - Rating: {self.rating}"


def main() -> None:
    r1 = Restaurant("Luigi's", 4.5)
    r2 = Restaurant("Mario's", 4.2)
    
    r1.add_item("Pizza", 12.99)
    r2.add_item("Pasta", 10.99)
    
    print(r1)
    print(r2)
    print(f"Total restaurants: {Restaurant.total_served}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding instance variables
- Understanding class variables
- Mutable class variable pitfalls

## Next Steps

Continue to **[03_methods_and_self.md](./03_methods_and_self.md)**
