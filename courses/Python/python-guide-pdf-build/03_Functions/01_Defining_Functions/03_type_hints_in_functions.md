# Type Hints in Functions

## What You'll Learn

- Parameter type hints
- Return type hints
- Union types with `|` (Python 3.10+)
- `Optional` types
- Fully typed function examples

## Prerequisites

- Read [02_args_and_kwargs.md](./02_args_and_kwargs.md) first

## Parameter Type Hints

Type hints for parameters make functions self-documenting:

```python
# With type hints
def greet(name: str) -> str:
    return f"Hello, {name}!"


# Without type hints (same functionality)
def greet(name):
    return f"Hello, {name}!"
```

### Multiple Parameters

```python
def add(a: int, b: int) -> int:
    return a + b


def create_user(name: str, age: int, active: bool = True) -> dict[str, Any]:
    return {
        "name": name,
        "age": age,
        "active": active
    }
```

## Return Type Hints

The `->` specifies what the function returns:

```python
# Returns an integer
def add(a: int, b: int) -> int:
    return a + b


# Returns None (nothing)
def print_hello() -> None:
    print("Hello!")


# Returns a string
def format_name(first: str, last: str) -> str:
    return f"{last}, {first}"
```

## Union Types (Python 3.10+)

A parameter or return can be multiple types:

```python
# Using | operator (Python 3.10+)
def process(value: int | float) -> int | float:
    return value * 2


# Works with integers
result: int | float = process(5)    # 10
result = process(3.14)              # 6.28
```

### Optional Types

When something can be a type or `None`:

```python
# Python 3.10+ syntax
def greet(name: str | None) -> str:
    if name is None:
        return "Hello, Guest!"
    return f"Hello, {name}!"


# Older syntax (still works)
from typing import Optional


def greet(name: Optional[str]) -> str:
    ...
```

## Complex Type Examples

### Lists and Dictionaries

```python
# Python 3.9+ syntax (built-in generics)
def sum_list(numbers: list[int]) -> int:
    return sum(numbers)


def get_value(data: dict[str, int], key: str) -> int | None:
    return data.get(key)


# With nested types
def process_matrix(matrix: list[list[int]]) -> list[int]:
    return [sum(row) for row in matrix]
```

### Tuples

```python
# Tuple with specific length and types
def get_point() -> tuple[int, int]:
    return (10, 20)


def get_person() -> tuple[str, int, bool]:
    return ("Alice", 25, True)
```

### Callables

```python
from typing import Callable


def apply_func(func: Callable[[int], int], value: int) -> int:
    """Apply a function to a value."""
    return func(value)


# Usage
result: int = apply_func(lambda x: x ** 2, 5)  # 25
```

## Annotated Example: Fully Typed Function Library

```python
# typed_functions.py
# Demonstrates comprehensive type hints

from typing import Any, Optional


# --- Basic Types ---

def add(a: int, b: int) -> int:
    """Add two integers."""
    return a + b


def divide(a: float, b: float) -> float | None:
    """Divide two numbers, returns None if divisor is zero."""
    if b == 0:
        return None
    return a / b


# --- Collection Types ---

def find_max(numbers: list[float]) -> float | None:
    """Find the maximum value in a list."""
    if not numbers:
        return None
    return max(numbers)


def merge_dicts(dict1: dict[str, Any], dict2: dict[str, Any]) -> dict[str, Any]:
    """Merge two dictionaries."""
    return {**dict1, **dict2}


# --- Optional Types ---

def get_user(user_id: int, default_name: str = "Guest") -> str:
    """Get user name by ID, or default if not found."""
    # Simulate database lookup
    users: dict[int, str] = {1: "Alice", 2: "Bob"}
    return users.get(user_id, default_name)


def parse_int(value: str) -> int | None:
    """Try to parse string as integer, return None if invalid."""
    try:
        return int(value)
    except ValueError:
        return None


# --- Complex Types ---

def process_items(
    items: list[dict[str, Any]],
    filter_key: str,
    filter_value: Any
) -> list[dict[str, Any]]:
    """Filter a list of dictionaries.
    
    Args:
        items: List of dictionaries to filter
        filter_key: Key to filter by
        filter_value: Value to filter for
    
    Returns:
        Filtered list
    """
    return [
        item for item in items
        if item.get(filter_key) == filter_value
    ]


def apply_to_each(
    func: Callable[[int], int],
    numbers: list[int]
) -> list[int]:
    """Apply a function to each number in a list."""
    return [func(n) for n in numbers]


# --- Full Example: Data Processor ---

from dataclasses import dataclass


@dataclass
class User:
    """Represents a user."""
    id: int
    name: str
    email: str
    age: int


def create_user(
    name: str,
    email: str,
    age: int,
    id: Optional[int] = None
) -> User:
    """Create a new user.
    
    Args:
        name: User's name
        email: User's email
        age: User's age
        id: Optional user ID (auto-generated if not provided)
    
    Returns:
        User instance
    """
    if id is None:
        # Simulate auto-generated ID
        id = hash(email) % 10000
    
    return User(id=id, name=name, email=email, age=age)


def validate_user(user: User) -> list[str]:
    """Validate user data.
    
    Args:
        user: User to validate
    
    Returns:
        List of validation errors (empty if valid)
    """
    errors: list[str] = []
    
    if not user.name or len(user.name) < 2:
        errors.append("Name must be at least 2 characters")
    
    if "@" not in user.email:
        errors.append("Invalid email format")
    
    if user.age < 0 or user.age > 150:
        errors.append("Age must be between 0 and 150")
    
    return errors


def main() -> None:
    # Test basic functions
    print("=== Basic Functions ===")
    print(f"add(3, 5) = {add(3, 5)}")
    print(f"divide(10, 2) = {divide(10, 2)}")
    print(f"divide(10, 0) = {divide(10, 0)}")
    
    # Test collection functions
    print("\n=== Collection Functions ===")
    print(f"max([1, 5, 3]) = {find_max([1, 5, 3])}")
    print(f"max([]) = {find_max([])}")
    
    # Test optional types
    print("\n=== Optional Types ===")
    print(f"get_user(1) = {get_user(1)}")
    print(f"get_user(999) = {get_user(999)}")
    print(f"parse_int('42') = {parse_int('42')}")
    print(f"parse_int('abc') = {parse_int('abc')}")
    
    # Test User class
    print("\n=== User Management ===")
    user: User = create_user("Alice", "alice@example.com", 25)
    print(f"Created user: {user}")
    
    errors: list[str] = validate_user(user)
    if errors:
        print(f"Validation errors: {errors}")
    else:
        print("User is valid!")


if __name__ == "__main__":
    main()
```

### Output

```
=== Basic Functions ===
add(3, 5) = 8
divide(10, 2) = 5.0
divide(10, 0) = None

=== Collection Functions ===
max([1, 5, 3]) = 5.0
max([]) = None

=== Optional Types ===
get_user(1) = Alice
get_user(999) = Guest
parse_int('42') = 42
parse_int('abc') = None

=== User Management ===
Created user: User(id=..., name='Alice', email='alice@example.com', age=25)
User is valid!
```

## Summary

- **Parameter hints**: `def f(x: int):`
- **Return hints**: `-> int`
- **Union types**: `int | float` (Python 3.10+)
- **`Optional`**: `str | None` or `Optional[str]`
- **Complex types**: `list[int]`, `dict[str, Any]`, `tuple[int, str]`

## Next Steps

Now let's learn about **lambda functions** in **[02_Advanced_Functions/01_lambda_functions.md](../02_Advanced_Functions/01_lambda_functions.md)**.
