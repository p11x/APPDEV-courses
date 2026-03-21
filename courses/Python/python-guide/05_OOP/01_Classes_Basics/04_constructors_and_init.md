# Constructors and __init__

## What You'll Learn

- Understanding __init__ constructor
- The __new__ method
- Initialization patterns
- Multiple constructors with class methods

## Prerequisites

- Read [03_methods_and_self.md](./03_methods_and_self.md) first

## The __init__ Constructor

`__init__` is called after an object is created to initialize its attributes.

```python
# init_demo.py

class Person:
    def __init__(self, name: str, age: int) -> None:
        print("__init__ called")
        self.name = name
        self.age = age
    
    def __str__(self) -> str:
        return f"{self.name}, {self.age} years old"


person = Person("Alice", 30)  # __init__ called automatically
print(person)
```

## The __new__ Method

`__new__` creates the instance, `__init__` initializes it.

```python
# new_demo.py

class Singleton:
    _instance = None
    
    def __new__(cls) -> "Singleton":
        if cls._instance is None:
            cls._instance = super().__new__(cls)
        return cls._instance
    
    def __init__(self) -> None:
        if not hasattr(self, "initialized"):
            self.initialized = True
            print("Singleton instance created")


s1 = Singleton()
s2 = Singleton()
print(s1 is s2)  # True - same instance
```

## Multiple Constructors with Class Methods

Use class methods to create alternative constructors.

```python
# multiple_constructors.py

class Rectangle:
    def __init__(self, width: float, height: float) -> None:
        self.width = width
        self.height = height
    
    @classmethod
    def from_square(cls, side: float) -> "Rectangle":
        """Create a square."""
        return cls(side, side)
    
    @classmethod
    def from_area(cls, area: float, aspect_ratio: float = 1.0) -> "Rectangle":
        """Create rectangle from area and aspect ratio."""
        height = (area / aspect_ratio) ** 0.5
        width = area / height
        return cls(width, height)
    
    def area(self) -> float:
        return self.width * self.height


# Different ways to create rectangles
r1 = Rectangle(5, 3)
r2 = Rectangle.from_square(4)
r3 = Rectangle.from_area(24, 2)
```

## Annotated Full Example

```python
# constructors_demo.py
"""Complete demonstration of constructors."""

from datetime import datetime
from typing import Optional


class Person:
    def __init__(self, name: str, birth_year: int) -> None:
        self.name = name
        self.birth_year = birth_year
    
    @property
    def age(self) -> int:
        return datetime.now().year - self.birth_year
    
    @classmethod
    def from_dict(cls, data: dict) -> "Person":
        """Create Person from dictionary."""
        return cls(data["name"], data["birth_year"])
    
    def to_dict(self) -> dict:
        """Convert Person to dictionary."""
        return {"name": self.name, "birth_year": self.birth_year}
    
    def __str__(self) -> str:
        return f"{self.name} ({self.age} years old)"


def main() -> None:
    p1 = Person("Alice", 1990)
    print(p1)
    
    p2 = Person.from_dict({"name": "Bob", "birth_year": 1985})
    print(p2)
    print(p2.to_dict())


if __name__ == "__main__":
    main()
```

## Summary

- Understanding __init__ constructor
- The __new__ method
- Multiple constructors with class methods

## Next Steps

Continue to **[05_dunder_methods.md](./05_dunder_methods.md)**
