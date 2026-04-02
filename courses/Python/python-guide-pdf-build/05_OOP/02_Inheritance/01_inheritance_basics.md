# Inheritance Basics

## What You'll Learn

- Single inheritance
- super() for parent class access
- Method overriding
- isinstance() and issubclass()

## Prerequisites

- Read [03_dunder_methods.md](./03_dunder_methods.md) first

## Basic Inheritance

```python
class Animal:
    def __init__(self, name: str) -> None:
        self.name = name
    
    def speak(self) -> str:
        raise NotImplementedError


class Dog(Animal):  # Inherits from Animal
    def speak(self) -> str:
        return "Woof!"


class Cat(Animal):
    def speak(self) -> str:
        return "Meow!"
```

## super() Function

```python
class Animal:
    def __init__(self, name: str) -> None:
        self.name = name


class Dog(Animal):
    def __init__(self, name: str, breed: str) -> None:
        super().__init__(name)  # Call parent __init__
        self.breed = breed
```

## Method Overriding

```python
class Animal:
    def speak(self) -> str:
        return "Some sound"


class Dog(Animal):
    def speak(self) -> str:  # Override
        return "Woof!"
```

## isinstance and issubclass

```python
dog = Dog("Buddy")

isinstance(dog, Dog)      # True
isinstance(dig, Animal)    # True
issubclass(Dog, Animal)   # True
```

## Summary

- **Inheritance**: class Child(Parent)
- **super()**: Call parent methods
- **Override**: Redefine method in child
- **isinstance**: Check object type

## Next Steps

Continue to **[02_multiple_inheritance.md](./02_multiple_inheritance.md)**
