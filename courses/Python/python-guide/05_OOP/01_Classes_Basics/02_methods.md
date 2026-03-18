# Methods

## What You'll Learn

- Instance methods
- Class methods with @classmethod
- Static methods with @staticmethod
- @property getter/setter/deleter

## Prerequisites

- Read [01_classes_and_objects.md](./01_classes_and_objects.md) first

## Instance Methods

```python
class Dog:
    def __init__(self, name: str) -> None:
        self.name = name
    
    def bark(self) -> None:  # Instance method
        print(f"{self.name} says Woof!")

dog = Dog("Buddy")
dog.bark()  # Instance method
```

## @classmethod

```python
class Dog:
    species: str = "Canis familiaris"
    
    @classmethod
    def create_puppy(cls, name: str) -> "Dog":
        """Factory method to create a puppy."""
        return cls(name)
```

## @staticmethod

```python
class Math:
    @staticmethod
    def add(a: int, b: int) -> int:
        """Static method - no self or cls needed."""
        return a + b
```

## @property

```python
class Circle:
    def __init__(self, radius: float) -> None:
        self._radius = radius
    
    @property
    def radius(self) -> float:
        return self._radius
    
    @radius.setter
    def radius(self, value: float) -> None:
        self._radius = value
```

## Summary

- **Instance methods**: First parameter is self
- **@classmethod**: First parameter is cls, can create instances
- **@staticmethod**: No self/cls, standalone function
- **@property**: Creates getter methods

## Next Steps

Continue to **[03_dunder_methods.md](./03_dunder_methods.md)**
