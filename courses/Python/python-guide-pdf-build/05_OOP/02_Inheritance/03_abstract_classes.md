# Abstract Classes

## What You'll Learn

- abc module
- ABC base class
- @abstractmethod decorator
- Enforcing contracts

## Prerequisites

- Read [02_multiple_inheritance.md](./02_multiple_inheritance.md) first

## Abstract Base Class

```python
from abc import ABC, abstractmethod

class Shape(ABC):
    @abstractmethod
    def area(self) -> float:
        pass
    
    @abstractmethod
    def perimeter(self) -> float:
        pass


class Circle(Shape):
    def __init__(self, radius: float) -> None:
        self.radius = radius
    
    def area(self) -> float:
        return 3.14 * self.radius ** 2
    
    def perimeter(self) -> float:
        return 2 * 3.14 * self.radius


# shape = Shape()  # ERROR! Can't instantiate abstract class
circle = Circle(5)  # OK
```

## Abstract Property

```python
from abc import ABC, abstractproperty

class Shape(ABC):
    @abstractproperty
    def area(self) -> float:
        pass
```

## Summary

- Use **ABC** as base class
- Use **@abstractmethod** for methods
- Abstract classes enforce implementation

## Next Steps

This concludes Inheritance. Move to **[05_OOP/03_Modern_OOP/01_protocols.md](../03_Modern_OOP/01_protocols.md)**
