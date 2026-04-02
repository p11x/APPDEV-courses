# Return Values

## What You'll Learn

- Understanding how functions return values
- Working with None return values
- Multiple return values with tuples
- Early returns and guard clauses

## Prerequisites

- Read [03_parameters_and_arguments.md](./03_parameters_and_arguments.md) first

## Returning Values

Functions can return values using the `return` statement. When a return statement is executed, the function terminates immediately and passes the value back to the caller.

```python
# return_values.py

def greet(name: str) -> str:
    """Return a greeting message."""
    return f"Hello, {name}!"

def calculate_total(prices: list[float]) -> float:
    """Calculate total with tax."""
    subtotal = sum(prices)
    tax = subtotal * 0.08
    return subtotal + tax
```

## Returning None

When no return statement is provided, or return is used without a value, the function returns `None`.

```python
# none_returns.py

def log_message(message: str) -> None:
    """Log a message - returns None."""
    print(f"LOG: {message}")
    # Implicitly returns None

def process_data(data: list[int]) -> None:
    """Process data in place."""
    for i in range(len(data)):
        data[i] *= 2
    # No return statement
```

## Multiple Return Values

Python functions can return multiple values as tuples, which can be unpacked by the caller.

```python
# multiple_returns.py

def divide_and_remainder(a: int, b: int) -> tuple[int, int]:
    """Return quotient and remainder."""
    quotient = a // b
    remainder = a % b
    return quotient, remainder

def get_stats(numbers: list[float]) -> tuple[float, float, float]:
    """Return min, max, and average."""
    return min(numbers), max(numbers), sum(numbers) / len(numbers)

# Usage
q, r = divide_and_remainder(17, 5)  # q=3, r=2
min_val, max_val, avg = get_stats([1, 2, 3, 4, 5])
```

## Annotated Full Example

```python
# return_values_demo.py
"""Demonstrates various return value patterns."""

from typing import Optional


def find_first_negative(numbers: list[int]) -> Optional[int]:
    """Return first negative number or None if none exists."""
    for num in numbers:
        if num < 0:
            return num
    return None


def parse_credentials(username: str, password: str) -> tuple[bool, str]:
    """Validate credentials, return success status and message."""
    if not username:
        return False, "Username is required"
    if len(password) < 8:
        return False, "Password must be at least 8 characters"
    return True, "Credentials valid"


def main() -> None:
    # Single return value
    message = greet("Alice")
    print(message)
    
    # Multiple return values
    quotient, remainder = divide_and_remainder(17, 5)
    print(f"17 ÷ 5 = {quotient} remainder {remainder}")
    
    # Optional return value
    result = find_first_negative([1, 2, -3, 4, 5])
    if result is not None:
        print(f"First negative: {result}")
    else:
        print("No negative numbers found")
    
    # Tuple unpacking with status
    success, msg = parse_credentials("alice", "password123")
    print(f"Success: {success}, Message: {msg}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding how functions return values
- Working with None return values
- Multiple return values with tuples

## Next Steps

Continue to **[05_args_and_kwargs.md](./05_args_and_kwargs.md)**
