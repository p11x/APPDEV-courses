# Classes and Objects

## What You'll Learn

- The class keyword and __init__
- self parameter
- Instance vs class attributes
- Real example: BankAccount class

## Prerequisites

- Read [03_stacks_queues_heaps.md](../../04_Data_Structures/03_Algorithms_With_Python/03_stacks_queues_heaps.md) first

## Creating a Class

```python
class Dog:
    """A simple Dog class."""
    
    def __init__(self, name: str, breed: str) -> None:
        """Initialize the dog."""
        self.name = name
        self.breed = breed
    
    def bark(self) -> None:
        """Make the dog bark."""
        print(f"{self.name} says Woof!")


# Create instance
dog = Dog("Buddy", "Golden Retriever")
dog.bark()
```

## Instance vs Class Attributes

```python
class Dog:
    species: str = "Canis familiaris"  # Class attribute
    
    def __init__(self, name: str) -> None:
        self.name = name  # Instance attribute


dog1 = Dog("Buddy")
dog2 = Dog("Max")

print(dog1.species)  # Canis familiaris (shared!)
print(dog2.species)  # Canis familiaris (same!)
```

## Annotated Example: BankAccount

```python
# bank_account.py


class BankAccount:
    """A simple bank account class."""
    
    # Class attribute - shared by all accounts
    bank_name: str = "Python Bank"
    
    def __init__(self, account_holder: str, initial_balance: float = 0.0) -> None:
        """Initialize account.
        
        Args:
            account_holder: Name of the account holder
            initial_balance: Starting balance (default 0)
        """
        self.holder = account_holder
        self.balance = initial_balance
    
    def deposit(self, amount: float) -> None:
        """Add money to account."""
        if amount > 0:
            self.balance += amount
            print(f"Deposited ${amount:.2f}. New balance: ${self.balance:.2f}")
        else:
            print("Invalid deposit amount")
    
    def withdraw(self, amount: float) -> None:
        """Remove money from account."""
        if amount > 0 and amount <= self.balance:
            self.balance -= amount
            print(f"Withdrew ${amount:.2f}. New balance: ${self.balance:.2f}")
        else:
            print("Invalid withdrawal amount")
    
    def get_balance(self) -> float:
        """Get current balance."""
        return self.balance
    
    def __repr__(self) -> str:
        """String representation."""
        return f"BankAccount('{self.holder}', {self.balance:.2f})"


def main() -> None:
    # Create account
    account = BankAccount("Alice", 1000.0)
    print(account)
    
    # Deposit
    account.deposit(500.0)
    
    # Withdraw
    account.withdraw(200.0)
    
    # Check balance
    print(f"Current balance: ${account.get_balance():.2f}")


if __name__ == "__main__":
    main()
```

## Summary

- **class**: Defines a blueprint
- **__init__**: Constructor method
- **self**: Reference to current instance
- **Instance attributes**: Per-object data
- **Class attributes**: Shared by all objects

## Next Steps

Continue to **[02_methods.md](./02_methods.md)**
