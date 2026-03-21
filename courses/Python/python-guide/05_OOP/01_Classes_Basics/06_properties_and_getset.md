# Properties and Getters/Setters

## What You'll Learn

- Using @property decorator
- Creating computed properties
- Setter and deleter properties
- When to use properties

## Prerequisites

- Read [05_dunder_methods.md](./05_dunder_methods.md) first

## The @property Decorator

Properties provide controlled access to attributes.

```python
# property_demo.py

class Circle:
    def __init__(self, radius: float) -> None:
        self._radius = radius
    
    @property
    def radius(self) -> float:
        return self._radius
    
    @radius.setter
    def radius(self, value: float) -> None:
        if value < 0:
            raise ValueError("Radius cannot be negative")
        self._radius = value
    
    @property
    def area(self) -> float:
        import math
        return math.pi * self._radius ** 2


c = Circle(5)
print(c.radius)
print(c.area)
c.radius = 10
print(c.area)
```

## Computed Properties

Properties can compute values on the fly.

```python
# computed_properties.py

class Rectangle:
    def __init__(self, width: float, height: float) -> None:
        self.width = width
        self.height = height
    
    @property
    def area(self) -> float:
        return self.width * self.height
    
    @property
    def perimeter(self) -> float:
        return 2 * (self.width + self.height)
    
    @property
    def is_square(self) -> bool:
        return self.width == self.height


r = Rectangle(5, 5)
print(f"Area: {r.area}")
print(f"Is square: {r.is_square}")
```

## Annotated Full Example

```python
# properties_demo.py
"""Complete demonstration of properties."""

import math


class Temperature:
    """Temperature with Celsius/Fahrenheit conversion."""
    
    def __init__(self, celsius: float = 0) -> None:
        self._celsius = celsius
    
    @property
    def celsius(self) -> float:
        return self._celsius
    
    @celsius.setter
    def celsius(self, value: float) -> None:
        self._celsius = value
    
    @property
    def fahrenheit(self) -> float:
        return self._celsius * 9/5 + 32
    
    @fahrenheit.setter
    def fahrenheit(self, value: float) -> None:
        self._celsius = (value - 32) * 5/9
    
    @property
    def kelvin(self) -> float:
        return self._celsius + 273.15
    
    def __str__(self) -> str:
        return f"{self._celsius:.1f}C"


def main() -> None:
    temp = Temperature(25)
    print(f"Celsius: {temp.celsius}")
    print(f"Fahrenheit: {temp.fahrenheit}")
    print(f"Kelvin: {temp.kelvin}")
    
    temp.fahrenheit = 212
    print(f"After setting F to 212: {temp}")


if __name__ == "__main__":
    main()
```

## Summary

- Using @property decorator
- Creating computed properties
- Setter and deleter properties

## Next Steps

Continue to **[07_class_and_static_methods.md](./07_class_and_static_methods.md)**
