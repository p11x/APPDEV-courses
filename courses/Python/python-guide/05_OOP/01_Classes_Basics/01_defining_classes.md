# Defining Classes

## What You'll Learn

- Creating classes with the class keyword
- Understanding instances and objects
- Defining attributes and methods
- The __init__ constructor

## Prerequisites

- Read [03_dictionaries.md](../../04_Data_Structures/01_Built_In/03_dictionaries.md) first

## Creating Classes

Classes are blueprints for creating objects in Python.

```python
# defining_classes.py

class Person:
    """A class representing a person."""
    
    def __init__(self, name: str, age: int) -> None:
        """Initialize a Person instance."""
        self.name = name
        self.age = age
    
    def greet(self) -> str:
        """Return a greeting message."""
        return f"Hello, my name is {self.name}"


# Create instances
person1 = Person("Alice", 30)
person2 = Person("Bob", 25)

print(person1.greet())
print(person2.greet())
```

## Class Attributes vs Instance Attributes

```python
# class_attributes.py

class Dog:
    # Class attribute - shared by all instances
    species = "Canis familiaris"
    
    def __init__(self, name: str, breed: str) -> None:
        # Instance attributes - unique to each instance
        self.name = name
        self.breed = breed
    
    def __str__(self) -> str:
        return f"{self.name} is a {self.breed}"


# Both dogs share the species class attribute
buddy = Dog("Buddy", "Golden Retriever")
max_dog = Dog("Max", "German Shepherd")

print(buddy.species)
print(max_dog.species)
print(buddy)
print(max_dog)
```

## Annotated Full Example

```python
# classes_demo.py
"""Complete demonstration of class definition."""

from typing import Optional


class BankAccount:
    """Represents a bank account."""
    
    # Class variable for all accounts
    bank_name: str = "Python Bank"
    
    def __init__(self, account_holder: str, initial_balance: float = 0.0) -> None:
        """Initialize a new bank account."""
        self.account_holder = account_holder
        self.balance = initial_balance
        self.transaction_count = 0
    
    def deposit(self, amount: float) -> bool:
        """Add money to the account."""
        if amount > 0:
            self.balance += amount
            self.transaction_count += 1
            return True
        return False
    
    def withdraw(self, amount: float) -> bool:
        """Remove money from the account."""
        if 0 < amount <= self.balance:
            self.balance -= amount
            self.transaction_count += 1
            return True
        return False
    
    def __str__(self) -> str:
        """Return string representation."""
        return f"{self.account_holder}: ${self.balance:.2f}"


def main() -> None:
    account = BankAccount("Alice", 1000)
    print(account)
    
    account.deposit(500)
    print(f"After deposit: {account}")
    
    account.withdraw(200)
    print(f"After withdrawal: {account}")


if __name__ == "__main__":
    main()
```

## Summary

- Creating classes with the class keyword
- Understanding instances and objects
- Defining attributes and methods

## Next Steps

Continue to **[02_instance_and_class_vars.md](./02_instance_and_class_vars.md)**
