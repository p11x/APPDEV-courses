# 🎯 SOLID Principles

> The 5 principles that separate good code from great code.

## 🎯 What You'll Learn

- S — Single Responsibility Principle
- O — Open/Closed Principle
- L — Liskov Substitution Principle
- I — Interface Segregation Principle
- D — Dependency Inversion Principle
- How to refactor messy code step by step

## 📦 Prerequisites

- Completion of [05_OOP/03_Modern_OOP/01_protocols.md](../../05_OOP/03_Modern_OOP/01_protocols.md)
- Understanding of classes and inheritance
- Basic knowledge of Python typing

---

## Overview

SOLID is a set of 5 design principles that help you write maintainable, flexible, and scalable code:

| Principle | Meaning |
|-----------|---------|
| **S** | Single Responsibility — one class, one job |
| **O** | Open/Closed — open for extension, closed for modification |
| **L** | Liskov Substitution — subclasses must be substitutable |
| **I** | Interface Segregation — small interfaces over fat ones |
| **D** | Dependency Inversion — depend on abstractions, not concretions |

---

## S — Single Responsibility

> A class should have only one reason to change.

### ❌ Violation: God Class

```python
# BAD: This class does too many things
class UserManager:
    """Manages users — but does too much!"""
    
    def __init__(self):
        self.users = []
    
    def create_user(self, name: str, email: str) -> None:
        # Creates user
        user = {"name": name, "email": email}
        self.users.append(user)
        # Also sends welcome email!
        self._send_welcome_email(email)
        # Also logs the action!
        self._log_action(f"Created user {name}")
        # Also saves to database!
        self._save_to_database(user)
    
    def _send_welcome_email(self, email: str) -> None:
        # Email logic here
        print(f"Sending welcome email to {email}")
    
    def _log_action(self, action: str) -> None:
        # Logging logic here
        print(f"LOG: {action}")
    
    def _save_to_database(self, user: dict) -> None:
        # Database logic here
        print(f"Saving {user} to database")
    
    def generate_report(self) -> str:
        # Reporting logic — what is this doing here?!
        return "User report..."
    
    def send_newsletter(self, subject: str) -> None:
        # Newsletter logic — also not user's job!
        print(f"Sending newsletter: {subject}")
```

### ✅ Fixed: Separate Responsibilities

```python
# GOOD: Each class has one job

class UserRepository:
    """Just handles database operations."""
    
    def save(self, user: dict) -> None:
        print(f"Saving {user} to database")
    
    def find_by_email(self, email: str) -> dict | None:
        print(f"Finding user by email: {email}")
        return None


class EmailService:
    """Just handles emails."""
    
    def send_welcome(self, email: str) -> None:
        print(f"Sending welcome email to {email}")
    
    def send_newsletter(self, recipients: list[str], subject: str) -> None:
        print(f"Sending newsletter to {len(recipients)} recipients")


class Logger:
    """Just handles logging."""
    
    def log(self, message: str) -> None:
        print(f"LOG: {message}")


class UserService:
    """Coordinates user operations."""
    
    def __init__(
        self,
        repository: UserRepository,
        email_service: EmailService,
        logger: Logger
    ):
        # Dependencies injected — each has one job
        self.repository = repository
        self.email_service = email_service
        self.logger = logger
    
    def create_user(self, name: str, email: str) -> None:
        user = {"name": name, "email": email}
        
        self.repository.save(user)
        self.email_service.send_welcome(email)
        self.logger.log(f"Created user {name}")
```

### 💡 Key Point

Each class now:
- Does ONE thing
- Has ONE reason to change
- Is easy to test in isolation

---

## O — Open/Closed

> Classes should be open for extension but closed for modification.

### ❌ Violation: Adding New Types Requires Changing Existing Code

```python
# BAD: Adding new payment types requires modifying this class
class PaymentProcessor:
    """Processes payments — but needs modification for each new type!"""
    
    def process_payment(self, amount: float, payment_type: str) -> str:
        if payment_type == "credit_card":
            # Process credit card
            return "Processed credit card payment"
        elif payment_type == "paypal":
            # Process PayPal
            return "Processed PayPal payment"
        elif payment_type == "crypto":
            # Process crypto
            return "Processed crypto payment"
        # Need to add more elif branches for new payment types!
        else:
            raise ValueError(f"Unknown payment type: {payment_type}")
```

### ✅ Fixed: Use Strategy Pattern

```python
from abc import ABC, abstractmethod
from typing import Protocol


class PaymentMethod(Protocol):
    """Protocol for payment methods — any class with process() works."""
    
    def process(self, amount: float) -> str:
        ...


class CreditCardPayment:
    """Credit card implementation."""
    
    def process(self, amount: float) -> str:
        return f"Processed ${amount:.2f} via Credit Card"


class PayPalPayment:
    """PayPal implementation."""
    
    def process(self, amount: float) -> str:
        return f"Processed ${amount:.2f} via PayPal"


class CryptoPayment:
    """Crypto implementation."""
    
    def process(self, amount: float) -> str:
        return f"Processed ${amount:.2f} via Crypto"


class PaymentProcessor:
    """Works with ANY payment method without modification!"""
    
    def process_payment(self, amount: float, method: PaymentMethod) -> str:
        return method.process(amount)


# Usage — add new payment types WITHOUT modifying PaymentProcessor!
processor = PaymentProcessor()

# Add Bitcoin later — just create a new class, don't modify this file!
# class BitcoinPayment:
#     def process(self, amount: float) -> str:
#         return f"Processed {amount} BTC"

result = processor.process_payment(99.99, CreditCardPayment())
print(result)  # Processed $99.99 via Credit Card
```

### 💡 Key Point

- Add new features by **creating new classes**
- Don't modify existing working code
- Use Protocol for duck typing

---

## L — Liskov Substitution

> Subclasses must be substitutable for their parent class.

### ❌ Violation: Square Inheriting Rectangle

```python
# BAD: Square IS-A Rectangle, but behaves differently!
class Rectangle:
    """A rectangle with width and height."""
    
    def __init__(self, width: float, height: float):
        self._width = width
        self._height = height
    
    @property
    def width(self) -> float:
        return self._width
    
    @width.setter
    def width(self, value: float) -> None:
        self._width = value
    
    @property
    def height(self) -> float:
        return self._height
    
    @height.setter
    def height(self, value: float) -> None:
        self._height = value
    
    def area(self) -> float:
        return self._width * self._height


class Square(Rectangle):
    """A square — but breaks Rectangle's contract!"""
    
    def __init__(self, side: float):
        super().__init__(side, side)
    
    @Rectangle.width.setter
    def width(self, value: float) -> None:
        # Setting width ALSO changes height — violates Rectangle!
        self._width = value
        self._height = value
    
    @Rectangle.height.setter
    def height(self, value: float) -> None:
        self._width = value
        self._height = value


# This breaks!
rect = Rectangle(5, 4)
rect.width = 6
print(f"Rectangle area: {rect.area()}")  # 6 * 4 = 24 ✓

square = Square(5)
square.width = 6  # Also changes height to 6!
print(f"Square area: {square.area()}")    # 6 * 6 = 36 ✗ (should be 30!)
```

### ✅ Fixed: Use Composition or Separate Hierarchy

```python
# Option 1: Don't inherit — use composition
class Square:
    """Square as its own class, not a Rectangle."""
    
    def __init__(self, side: float):
        self.side = side
    
    def area(self) -> float:
        return self.side ** 2


# Option 2: Separate hierarchies with Protocol
class Shape(Protocol):
    """Any shape with area."""
    
    def area(self) -> float:
        ...


class Rectangle:
    def __init__(self, width: float, height: float):
        self.width = width
        self.height = height
    
    def area(self) -> float:
        return self.width * self.height


class Square:
    def __init__(self, side: float):
        self.side = side
    
    def area(self) -> float:
        return self.side ** 2


# Both can be used wherever Shape is expected
def calculate_total_area(shapes: list[Shape]) -> float:
    return sum(shape.area() for shape in shapes)
```

---

## I — Interface Segregation

> Many small, focused interfaces are better than one large interface.

### ❌ Violation: Fat Interface

```python
# BAD: One interface with too many methods
class Machine(Protocol):
    """A machine that can do everything — but most machines can't!"""
    
    def print(self, document: str) -> None:
        ...
    
    def scan(self, document: str) -> None:
        ...
    
    def fax(self, document: str) -> None:
        ...


class OldPrinter:
    """An old printer — can't scan or fax, but must implement these!"""
    
    def print(self, document: str) -> None:
        print(f"Printing: {document}")
    
    def scan(self, document: str) -> None:
        raise NotImplementedError("Can't scan!")
    
    def fax(self, document: str) -> None:
        raise NotImplementedError("Can't fax!")
```

### ✅ Fixed: Small, Focused Interfaces

```python
from typing import Protocol


class Printer(Protocol):
    """Just printing."""
    def print(self, document: str) -> None: ...


class Scanner(Protocol):
    """Just scanning."""
    def scan(self, document: str) -> None: ...


class FaxMachine(Protocol):
    """Just faxing."""
    def fax(self, document: str) -> None: ...


class OldPrinter:
    """Implements ONLY what it can do."""
    
    def print(self, document: str) -> None:
        print(f"Printing: {document}")


class MultiFunctionPrinter:
    """Implements multiple interfaces."""
    
    def print(self, document: str) -> None:
        print(f"Printing: {document}")
    
    def scan(self, document: str) -> None:
        print(f"Scanning: {document}")
    
    def fax(self, document: str) -> None:
        print(f"Faxing: {document}")


# Usage
def print_document(printer: Printer, doc: str) -> None:
    printer.print(doc)

# Both work!
print_document(OldPrinter(), "hello")
print_document(MultiFunctionPrinter(), "hello")
```

---

## D — Dependency Inversion

> Depend on abstractions, not concretions.

### ❌ Violation: Hard-Coded Dependency

```python
# BAD: Hard-codes a specific database
class UserService:
    """Service that needs a database."""
    
    def __init__(self):
        # Directly creates concrete implementation!
        self.db = SQLiteDatabase("users.db")
    
    def create_user(self, name: str) -> None:
        self.db.insert({"name": name})


class SQLiteDatabase:
    """Specific database implementation."""
    
    def __init__(self, db_path: str):
        self.db_path = db_path
    
    def insert(self, data: dict) -> None:
        print(f"Inserting {data} into {self.db_path}")
```

### ✅ Fixed: Inject Dependencies

```python
from typing import Protocol


class Database(Protocol):
    """Abstract database protocol."""
    
    def insert(self, data: dict) -> None:
        ...


class SQLiteDatabase:
    """SQLite implementation."""
    
    def __init__(self, db_path: str):
        self.db_path = db_path
    
    def insert(self, data: dict) -> None:
        print(f"Inserting {data} into SQLite: {self.db_path}")


class PostgreSQLDatabase:
    """PostgreSQL implementation."""
    
    def __init__(self, connection_string: str):
        self.conn_string = connection_string
    
    def insert(self, data: dict) -> None:
        print(f"Inserting {data} into PostgreSQL: {self.conn_string}")


class UserService:
    """Depends on abstraction, not concretion!"""
    
    def __init__(self, db: Database):  # Accepts ANY database
        self.db = db
    
    def create_user(self, name: str) -> None:
        self.db.insert({"name": name})


# Usage — inject whatever database you need!
service = UserService(SQLiteDatabase("users.db"))
service.create_user("Alice")

# Easy to switch to PostgreSQL later!
service2 = UserService(PostgreSQLDatabase("postgresql://localhost/users"))
service2.create_user("Bob")
```

---

## Full Refactoring Example

### Before: Messy 80-Line Class

```python
# ❌ BEFORE: Everything in one class
class OrderProcessor:
    """Processes orders — doing way too much!"""
    
    def __init__(self):
        self.orders = []
        self.db_connection = "localhost:5432"
        self.email_host = "smtp.example.com"
        self.logger = MyLogger()
    
    def process_order(self, order_data: dict) -> str:
        # Validate
        if not order_data.get("items"):
            return "Error: No items"
        
        # Calculate total
        total = sum(item["price"] * item["quantity"] for item in order_data["items"])
        
        # Save to database
        self.logger.log(f"Saving order: {order_data}")
        
        # Send confirmation email
        print(f"Sending email to {order_data['email']}")
        
        # Generate invoice
        invoice = f"Invoice for {order_data['email']}: ${total}"
        
        # Update inventory
        for item in order_data["items"]:
            print(f"Updating inventory: {item['name']}")
        
        return invoice
```

### After: SOLID Principles Applied

```python
# ✅ AFTER: Clean, separated, testable

# Protocol for dependency injection
class Database(Protocol):
    def save_order(self, order: dict) -> None: ...

class EmailService(Protocol):
    def send_confirmation(self, email: str, invoice: str) -> None: ...

class InventoryService(Protocol):
    def update_stock(self, items: list[dict]) -> None: ...


# Clean classes with single responsibility
class OrderValidator:
    """Validates orders."""
    
    def validate(self, order_data: dict) -> None:
        if not order_data.get("items"):
            raise ValueError("Order must have items")


class PriceCalculator:
    """Calculates order prices."""
    
    def calculate(self, items: list[dict]) -> float:
        return sum(item["price"] * item["quantity"] for item in items)


class InvoiceGenerator:
    """Generates invoices."""
    
    def generate(self, email: str, total: float) -> str:
        return f"Invoice for {email}: ${total}"


class OrderService:
    """Orchestrates the order process — but delegates to specialists!"""
    
    def __init__(
        self,
        db: Database,
        email: EmailService,
        inventory: InventoryService
    ):
        self.validator = OrderValidator()
        self.calculator = PriceCalculator()
        self.invoice_gen = InvoiceGenerator()
        self.db = db
        self.email = email
        self.inventory = inventory
    
    def process_order(self, order_data: dict) -> str:
        # Validate
        self.validator.validate(order_data)
        
        # Calculate
        total = self.calculator.calculate(order_data["items"])
        
        # Generate invoice
        invoice = self.invoice_gen.generate(order_data["email"], total)
        
        # Delegate to dependencies
        self.db.save_order(order_data)
        self.email.send_confirmation(order_data["email"], invoice)
        self.inventory.update_stock(order_data["items"])
        
        return invoice
```

---

## Summary

✅ **S — Single Responsibility** — one class, one job

✅ **O — Open/Closed** — extend without modifying

✅ **L — Liskov Substitution** — subclasses work like parent

✅ **I — Interface Segregation** — small, focused interfaces

✅ **D — Dependency Inversion** — inject abstractions

---

## ➡️ Next Steps

Continue to [02_creational_patterns.md](./02_creational_patterns.md) to learn creational design patterns.

---

## 🔗 Further Reading

- [SOLID Principles (Robert C. Martin)](https://en.wikipedia.org/wiki/SOLID)
- [Python Design Patterns](https://refactoring.guru/design-patterns/python)
- [Architecture Patterns with Python](https://www.oreilly.com/library/view/architecture-patterns-with/9781492034544/)
