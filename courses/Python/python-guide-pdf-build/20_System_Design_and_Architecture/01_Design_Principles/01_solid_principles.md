# SOLID Principles

## What You'll Learn

- Single Responsibility Principle
- Open/Closed Principle
- Liskov Substitution Principle
- Interface Segregation Principle
- Dependency Inversion Principle

## Prerequisites

- Read [08_design_patterns_in_python.md](../../05_OOP/03_Modern_OOP/08_design_patterns.md) first

## Single Responsibility Principle (SRP)

A class should have only one reason to change.

```python
# srp_demo.py

# Bad - multiple responsibilities
class User:
    def __init__(self, name: str, email: str) -> None:
        self.name = name
        self.email = email
    
    def save(self) -> None:
        # Save to database
        pass
    
    def send_email(self) -> None:
        # Send email
        pass

# Good - separate responsibilities
class User:
    def __init__(self, name: str, email: str) -> None:
        self.name = name
        self.email = email


class UserRepository:
    def save(self, user: User) -> None:
        pass


class EmailService:
    def send(self, user: User) -> None:
        pass
```

## Open/Closed Principle (OCP)

Open for extension, closed for modification.

```python
# ocp_demo.py

from abc import ABC, abstractmethod


class Shape(ABC):
    @abstractmethod
    def area(self) -> float:
        pass


class Circle(Shape):
    def __init__(self, radius: float) -> None:
        self.radius = radius
    
    def area(self) -> float:
        import math
        return math.pi * self.radius ** 2


class Rectangle(Shape):
    def __init__(self, width: float, height: float) -> None:
        self.width = width
        self.height = height
    
    def area(self) -> float:
        return self.width * self.height


# Add new shapes without modifying existing code
class Triangle(Shape):
    def __init__(self, base: float, height: float) -> None:
        self.base = base
        self.height = height
    
    def area(self) -> float:
        return 0.5 * self.base * self.height
```

## Annotated Full Example

```python
# solid_demo.py
"""Complete demonstration of SOLID principles."""

from abc import ABC, abstractmethod
from typing import List


# ISP - Interface Segregation
class Printer:
    @abstractmethod
    def print(self, document: str) -> None:
        pass


class Scanner:
    @abstractmethod
    def scan(self, document: str) -> None:
        pass


# DIP - Dependency Inversion
class Document:
    def __init__(self, content: str) -> None:
        self.content = content


class MyPrinter(Printer):
    def print(self, document: str) -> None:
        print(f"Printing: {document}")


class PrintService:
    def __init__(self, printer: Printer) -> None:
        self.printer = printer
    
    def print_document(self, doc: Document) -> None:
        self.printer.print(doc.content)


def main() -> None:
    printer = MyPrinter()
    service = PrintService(printer)
    service.print_document(Document("Hello, World!"))


if __name__ == "__main__":
    main()
```

## Summary

- Single Responsibility - one reason to change
- Open/Closed - extend without modifying
- Liskov Substitution - interchangeable subtypes
- Interface Segregation - specific interfaces
- Dependency Inversion - depend on abstractions

## Next Steps

Continue to **[02_dry_yagni_kiss.md](./02_dry_yagni_kiss.md)**
