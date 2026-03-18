# Dunder Methods

## What You'll Learn

- __str__ and __repr__
- __len__, __eq__, __lt__
- __add__, __contains__
- __iter__, __next__

## Prerequisites

- Read [02_methods.md](./02_methods.md) first

## __str__ and __repr__

```python
class Person:
    def __init__(self, name: str, age: int) -> None:
        self.name = name
        self.age = age
    
    def __str__(self) -> str:
        return f"{self.name}, {self.age}"
    
    def __repr__(self) -> str:
        return f"Person('{self.name}', {self.age})"

p = Person("Alice", 25)
print(str(p))    # Alice, 25
print(repr(p))   # Person('Alice', 25)
```

## Comparison Methods

```python
class Person:
    def __init__(self, name: str, age: int) -> None:
        self.name = name
        self.age = age
    
    def __eq__(self, other: object) -> bool:
        if not isinstance(other, Person):
            return NotImplemented
        return self.age == other.age
    
    def __lt__(self, other: "Person") -> bool:
        return self.age < other.age
```

## Arithmetic Methods

```python
class Vector:
    def __init__(self, x: int, y: int) -> None:
        self.x = x
        self.y = y
    
    def __add__(self, other: "Vector") -> "Vector":
        return Vector(self.x + other.x, self.y + other.y)
    
    def __repr__(self) -> str:
        return f"Vector({self.x}, {self.y})"

v1 = Vector(1, 2)
v2 = Vector(3, 4)
print(v1 + v2)  # Vector(4, 6)
```

## Summary

- **__str__**: Human-readable string
- **__repr__**: Developer representation
- **__eq__**, **__lt__**: Comparison
- **__add__**, etc.: Arithmetic operations
- **__iter__**, **__next__**: Iteration

## Next Steps

This concludes Classes Basics. Continue to **[05_OOP/02_Inheritance/01_inheritance_basics.md](../02_Inheritance/01_inheritance_basics.md)**
