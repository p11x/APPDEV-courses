# Dataclasses

## What You'll Learn

- Using @dataclass decorator
- field() for customization
- __post_init__ for validation
- frozen=True for immutability
- slots=True for memory efficiency

## Prerequisites

- Read [03_dictionaries.md](./03_dictionaries.md) first

## Basic Dataclass

```python
from dataclasses import dataclass

@dataclass
class User:
    name: str
    age: int
    email: str

# Create instance
user = User("Alice", 25, "alice@example.com")
print(user.name, user.age, user.email)
```

## Field Customization

```python
from dataclasses import dataclass, field

@dataclass
class Product:
    name: str
    price: float
    quantity: int = 0  # Default value
    tags: list[str] = field(default_factory=list)  # Mutable default
```

## __post_init__

```python
from dataclasses import dataclass

@dataclass
class Rectangle:
    width: float
    height: float
    
    @property
    def area(self) -> float:
        return self.width * self.height
```

## Frozen (Immutable)

```python
from dataclasses import dataclass

@dataclass(frozen=True)
class Point:
    x: int
    y: int

p = Point(1, 2)
# p.x = 3  # Raises FrozenInstanceError!
```

## Annotated Example

```python
# dataclass_demo.py

from dataclasses import dataclass, field


@dataclass
class User:
    name: str
    age: int
    email: str
    active: bool = True


@dataclass
class Product:
    name: str
    price: float
    quantity: int = 0
    tags: list[str] = field(default_factory=list)


def main() -> None:
    user = User("Alice", 25, "alice@example.com")
    print(user)
    
    product = Product("Laptop", 999.99, quantity=5)
    product.tags.append("electronics")
    print(product)


if __name__ == "__main__":
    main()
```

## Summary

- **@dataclass**: Auto-generates __init__, __repr__, __eq__
- **field()**: Customize individual fields
- **frozen=True**: Make immutable
- **slots=True**: Memory efficient (Python 3.10+)

## Next Steps

Continue to **[02_typed_collections.md](./02_typed_collections.md)**
