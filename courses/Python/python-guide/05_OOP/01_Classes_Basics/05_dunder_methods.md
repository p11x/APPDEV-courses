# Dunder Methods

## What You'll Learn

- Understanding dunder (double underscore) methods
- __str__ vs __repr__
- __eq__ and __hash__
- Arithmetic operator overloading

## Prerequisites

- Read [04_constructors_and_init.md](./04_constructors_and_init.md) first

## String Representation

`__str__` and `__repr__` provide string representations of objects.

```python
# string_repr.py

class Person:
    def __init__(self, name: str, age: int) -> None:
        self.name = name
        self.age = age
    
    def __str__(self) -> str:
        """Human-readable representation."""
        return f"{self.name}, {self.age}"
    
    def __repr__(self) -> str:
        """Developer representation."""
        return f"Person(name={self.name!r}, age={self.age!r})"


p = Person("Alice", 30)
print(str(p))      # Alice, 30
print(repr(p))    # Person(name='Alice', age=30)
```

## Equality and Hashing

Define how objects are compared and hashed.

```python
# equality_hash.py

class Point:
    def __init__(self, x: int, y: int) -> None:
        self.x = x
        self.y = y
    
    def __eq__(self, other: object) -> bool:
        if not isinstance(other, Point):
            return NotImplemented
        return self.x == other.x and self.y == other.y
    
    def __hash__(self) -> int:
        return hash((self.x, self.y))


p1 = Point(1, 2)
p2 = Point(1, 2)
print(p1 == p2)  # True
print(hash(p1) == hash(p2))  # True
```

## Arithmetic Operators

```python
# arithmetic_ops.py

class Vector:
    def __init__(self, x: float, y: float) -> None:
        self.x = x
        self.y = y
    
    def __add__(self, other: "Vector") -> "Vector":
        return Vector(self.x + other.x, self.y + other.y)
    
    def __mul__(self, scalar: float) -> "Vector":
        return Vector(self.x * scalar, self.y * scalar)
    
    def __repr__(self) -> str:
        return f"Vector({self.x}, {self.y})"


v1 = Vector(1, 2)
v2 = Vector(3, 4)
print(v1 + v2)  # Vector(4, 6)
print(v1 * 3)   # Vector(3, 6)
```

## Annotated Full Example

```python
# dunder_methods_demo.py
"""Complete demonstration of dunder methods."""

from typing import Any


class Money:
    def __init__(self, amount: float, currency: str = "USD") -> None:
        self.amount = amount
        self.currency = currency
    
    def __str__(self) -> str:
        return f"{self.currency} {self.amount:.2f}"
    
    def __repr__(self) -> str:
        return f"Money(amount={self.amount}, currency={self.currency!r})"
    
    def __eq__(self, other: object) -> bool:
        if not isinstance(other, Money):
            return NotImplemented
        return self.amount == other.amount and self.currency == other.currency
    
    def __add__(self, other: "Money") -> "Money":
        if self.currency != other.currency:
            raise ValueError("Cannot add different currencies")
        return Money(self.amount + other.amount, self.currency)
    
    def __mul__(self, multiplier: float) -> "Money":
        return Money(self.amount * multiplier, self.currency)


def main() -> None:
    m1 = Money(100, "USD")
    m2 = Money(50, "USD")
    
    print(f"m1: {m1}")
    print(f"m2: {m2}")
    print(f"m1 == m2: {m1 == m2}")
    print(f"m1 + m2: {m1 + m2}")
    print(f"m1 * 2: {m1 * 2}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding dunder methods
- __str__ vs __repr__
- __eq__ and __hash__

## Next Steps

Continue to **[06_properties_and_getset.md](./06_properties_and_getset.md)**
