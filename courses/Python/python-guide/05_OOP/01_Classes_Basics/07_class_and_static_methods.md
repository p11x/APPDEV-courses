# Class and Static Methods

## What You'll Learn

- @classmethod decorators
- @staticmethod decorators
- When to use each type
- Factory patterns

## Prerequisites

- Read [06_properties_and_getset.md](./06_properties_and_getset.md) first

## @classmethod

Class methods receive the class as the first parameter and are used for factory methods.

```python
# classmethod_demo.py

class Person:
    def __init__(self, name: str, age: int) -> None:
        self.name = name
        self.age = age
    
    @classmethod
    def from_dict(cls, data: dict) -> "Person":
        """Create Person from dictionary."""
        return cls(data["name"], data["age"])
    
    @classmethod
    def from_string(cls, s: str) -> "Person":
        """Create Person from string 'name,age'."""
        name, age = s.split(",")
        return cls(name, int(age))


p1 = Person.from_dict({"name": "Alice", "age": 30})
p2 = Person.from_string("Bob,25")
```

## @staticmethod

Static methods don't receive any special first parameter and are used for utility functions.

```python
# staticmethod_demo.py

class StringHelper:
    @staticmethod
    def reverse(s: str) -> str:
        return s[::-1]
    
    @staticmethod
    def is_palindrome(s: str) -> bool:
        return s == s[::-1]
    
    @staticmethod
    def word_count(s: str) -> int:
        return len(s.split())


print(StringHelper.reverse("hello"))
print(StringHelper.is_palindrome("radar"))
print(StringHelper.word_count("Hello world"))
```

## Annotated Full Example

```python
# class_static_demo.py
"""Complete demonstration of class and static methods."""

from datetime import datetime


class Person:
    def __init__(self, name: str, birth_year: int) -> None:
        self.name = name
        self.birth_year = birth_year
    
    @property
    def age(self) -> int:
        return datetime.now().year - self.birth_year
    
    @classmethod
    def create_anonymous(cls, age: int) -> "Person":
        """Create person with generated name."""
        name = f"Person_{id(cls)}"
        birth_year = datetime.now().year - age
        return cls(name, birth_year)
    
    @staticmethod
    def validate_age(age: int) -> bool:
        """Validate age is reasonable."""
        return 0 <= age <= 150
    
    def __str__(self) -> str:
        return f"{self.name} ({self.age})"


def main() -> None:
    p1 = Person("Alice", 1990)
    print(p1)
    
    p2 = Person.create_anonymous(25)
    print(p2)
    
    print(f"Valid age 30: {Person.validate_age(30)}")
    print(f"Valid age 200: {Person.validate_age(200)}")


if __name__ == "__main__":
    main()
```

## Summary

- @classmethod decorators
- @staticmethod decorators
- Factory patterns

## Next Steps

Continue to **[01_single_inheritance.md](../02_Inheritance/01_single_inheritance.md)**
