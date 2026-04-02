# Type Hints and Annotations

## What You'll Learn

- Python 3.12+ type hints syntax
- Variable annotations
- Why type hints help beginners
- How to use `reveal_type()` for debugging

## Prerequisites

- Read [02_data_types.md](./02_data_types.md) first

## What Are Type Hints?

Type hints are **optional annotations** that tell you what type a variable, function parameter, or return value should be. They're like labels on boxes — they don't change the behavior, but they help you understand the code.

### Without Type Hints

```python
# What types are these?
name = "Alice"
age = 25
price = 19.99
items = ["apple", "banana"]
```

### With Type Hints

```python
# Now we know the types!
name: str = "Alice"
age: int = 25
price: float = 19.99
items: list[str] = ["apple", "banana"]
```

## Variable Annotations Syntax

The syntax for type hints on variables is:

```python
variable_name: Type = value
```

### Basic Examples

```python
# Simple types
count: int = 0
name: str = "Alice"
price: float = 9.99
is_valid: bool = True

# Container types (Python 3.9+)
# Instead of from typing import List, we can use built-in generics
names: list[str] = ["Alice", "Bob"]     # List of strings
ages: dict[str, int] = {"Alice": 25}    # Dict with string keys, int values
unique_ids: set[int] = {1, 2, 3}        # Set of integers

# Tuple types
point: tuple[int, int] = (10, 20)       # Tuple of 2 ints
coordinates: tuple[float, float, float] = (1.5, 2.5, 3.5)  # 3 floats
```

## Why Type Hints Help Beginners

### 1. Self-Documenting Code

```python
# Without hints - what does this function do?
def process(data):
    return data["value"] * 2

# With hints - much clearer!
def process(data: dict[str, int]) -> int:
    return data["value"] * 2
```

### 2. Better IDE Support

- **Autocomplete**: IDE knows what methods are available
- **Error detection**: Catch mistakes before running
- **Refactoring**: Safely rename and change code

### 3. Catches Bugs Early

Tools like **mypy** can find errors:

```python
name: str = "Alice"
age: int = 25

# Oops! We accidentally used the wrong type
result: str = age  # mypy would catch this!
```

### 4. Clearer Function Signatures

```python
# What does this return? What does it need?
def calculate(a, b, c):
    ...

# Much clearer with hints!
def calculate(a: float, b: float, c: float) -> float:
    ...
```

## Advanced Type Hints

### Optional Types

Use `X | None` (Python 3.10+) or `Optional[X]` (older):

```python
# Python 3.10+ syntax (recommended)
name: str | None = None
age: int | None = None

# Equivalent older syntax
from typing import Optional
name: Optional[str] = None
```

### Union Types

A value can be one of several types:

```python
# Can be either int or float
number: int | float = 42
number = 3.14  # Also valid
```

### Literal Types

Restrict to specific values:

```python
# Only these three strings are allowed
status: Literal["pending", "approved", "rejected"] = "pending"

# Only these integers
direction: Literal[0, 1, 2, 3] = 0
```

### Final Values

Mark values that shouldn't change:

```python
from typing import Final

MAX_SIZE: Final = 100
VERSION: Final = "1.0.0"
```

## The `reveal_type()` Function

When you're learning or debugging, use `reveal_type()` to see what type Python infers:

```python
# reveal_type.py
# Shows what type Python thinks a variable has

def main() -> None:
    # Let's see what types Python infers
    
    x = 42
    reveal_type(x)  # mypy will show: Revealed type is "int"
    
    y = "hello"
    reveal_type(y)  # mypy will show: Revealed type is "str"
    
    z = [1, 2, 3]
    reveal_type(z)  # mypy will show: Revealed type is "list[int]"
    
    # Type hints + reveal_type together
    name: str = "Alice"
    reveal_type(name)  # Shows: Revealed type is "str"


if __name__ == "__main__":
    main()
```

### Running reveal_type

You need a type checker like **mypy** to see the output:

```bash
pip install mypy
mypy reveal_type.py
```

Output:
```
reveal_type.py:5: note: Revealed type is "int"
reveal_type.py:8: note: Revealed type is "str"
reveal_type.py:11: note: Revealed type is "list[int]"
reveal_type.py:16: note: Revealed type is "str"
```

## `x: int = 5` vs `x = 5` — What's the Difference?

### Without Type Hint

```python
x = 5  # Python infers x is an int
```

Python sees `5` (an integer) and automatically makes `x` an integer type.

### With Type Hint

```python
x: int = 5  # We tell Python x is an int
```

Same result at runtime! The type hint doesn't change how Python runs the code. But now:
- IDEs know `x` is an integer
- Type checkers can verify you're using it correctly
- Other developers know your intent

### Runtime vs Static

| Aspect | Type Hint | Without Hint |
|--------|-----------|--------------|
| Affects runtime? | No | No |
| Checked at runtime? | No (by default) | No |
| Checked by mypy? | Yes | No |
| IDE autocomplete? | Yes | Limited |

## Annotated Example: Fully Typed Program

```python
# type_hints_demo.py
# Demonstrates comprehensive type hints

from typing import Optional


def main() -> None:
    # Variable annotations with basic types
    user_name: str = "Alice"
    user_age: int = 25
    account_balance: float = 1250.50
    is_active: bool = True
    nickname: Optional[str] = None  # Can be str or None
    
    # Container types - using built-in generics (Python 3.9+)
    user_ids: list[int] = [1, 2, 3]
    user_scores: dict[str, int] = {"Alice": 95, "Bob": 87}
    unique_tags: set[str] = {"python", "coding"}
    
    # Tuple with specific length and types
    point: tuple[int, int] = (10, 20)
    rgb_color: tuple[int, int, int] = (255, 128, 0)
    
    # Print all values with annotations
    print(f"Name: {user_name} (type: {type(user_name).__name__})")
    print(f"Age: {user_age} (type: {type(user_age).__name__})")
    print(f"Balance: {account_balance} (type: {type(account_balance).__name__})")
    print(f"Active: {is_active} (type: {type(is_active).__name__})")
    print(f"Nickname: {nickname} (type: {type(nickname).__name__ if nickname else 'NoneType'})")
    
    print(f"IDs: {user_ids}")
    print(f"Scores: {user_scores}")
    print(f"Tags: {unique_tags}")
    print(f"Point: {point}")
    print(f"Color: {rgb_color}")
    
    # Using reveal_type to see inferred types
    # (Install mypy and run: mypy type_hints_demo.py)
    reveal_type(user_name)
    reveal_type(user_ids)


# Demonstrate function type hints
def greet_user(name: str, times: int = 1) -> str:
    """Greet a user multiple times.
    
    Args:
        name: The user's name
        times: How many times to greet (default 1)
    
    Returns:
        The greeting message
    """
    return "Hello, " + name + "!" * times


# Function with complex return type
def get_user_data() -> dict[str, str | int | bool]:
    """Get user data as a dictionary."""
    return {
        "name": "Alice",
        "age": 25,
        "active": True
    }


if __name__ == "__main__":
    main()
    print(f"\nGreeting: {greet_user('Bob', 3)}")
    print(f"User data: {get_user_data()}")
```

## Summary

- **Type hints** are optional annotations: `x: int = 5`
- **Don't change runtime behavior** — they're for developers and tools
- **Help beginners** by making code self-documenting
- **Use `reveal_type()`** with mypy to debug types
- **Python 3.10+** supports `int | float` syntax for unions
- **Python 3.9+** supports built-in generics: `list[int]`, `dict[str, int]`

## Next Steps

Now let's dive into **operators** in **[01_Foundations/03_Operators/01_arithmetic_operators.md](../03_Operators/01_arithmetic_operators.md)** — learn how to perform calculations in Python.
