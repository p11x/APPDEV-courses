# Raising Exceptions

## What You'll Learn

- How to raise exceptions with `raise`
- Creating custom exception classes
- Using `raise from` for chained exceptions
- Python 3.11+ exception features

## Prerequisites

- Read [01_try_except_basics.md](./01_try_except_basics.md) first

## Raising Exceptions

You can raise exceptions manually to indicate errors:

```python
# Basic raise
raise ValueError("Invalid value!")
```

### Syntax

```python
raise ExceptionType("Error message")
```

### Raising Built-in Exceptions

```python
# Raise different exceptions
raise ValueError("Number must be positive")
raise TypeError("Expected string, got int")
raise KeyError("Name not found")
raise RuntimeError("Something went wrong")
```

## Custom Exception Classes

Create your own exceptions for your application:

```python
# Custom exception - inherit from Exception
class InvalidAgeError(Exception):
    """Raised when age is invalid."""
    pass


def set_age(age: int) -> None:
    if age < 0:
        raise InvalidAgeError("Age cannot be negative")
    if age > 150:
        raise InvalidAgeError("Age is unrealistic")
    
    print(f"Age set to {age}")


# Use it
try:
    set_age(-5)
except InvalidAgeError as e:
    print(f"Error: {e}")
```

### Best Practice: Add Details

```python
class ValidationError(Exception):
    """Custom exception for validation errors.
    
    Attributes:
        field: The field that failed validation
        value: The invalid value
    """
    
    def __init__(self, message: str, field: str, value: object) -> None:
        super().__init__(message)
        self.field = field
        self.value = value


def validate_username(username: str) -> None:
    if not username:
        raise ValidationError(
            "Username cannot be empty",
            field="username",
            value=username
        )
    if len(username) < 3:
        raise ValidationError(
            "Username too short",
            field="username", 
            value=username
        )


try:
    validate_username("")
except ValidationError as e:
    print(f"Error in {e.field}: {e}")  # Error in username: Username cannot be empty
```

## Raise From

Use `raise ... from ...` to chain exceptions:

```python
try:
    # Some operation that fails
    data: dict = {}
    value: str = data["missing_key"]  # Raises KeyError
    
except KeyError as e:
    # Wrap the original error with context
    raise RuntimeError("Failed to get data") from e
```

### Why Chain Exceptions?

- Shows the **original cause** of the error
- Preserves the **traceback**
- Helps with **debugging**

## Python 3.11+ Features

### Adding Notes to Exceptions

Python 3.11 added `add_note()` to add context:

```python
try:
    result: int = int("not a number")
    
except ValueError as e:
    e.add_note("This happened in the user input parsing")
    e.add_note("User was trying to convert input on line 42")
    raise
```

### ExceptionGroup and except*

Python 3.11 introduced `ExceptionGroup` for handling multiple exceptions at once (see next file for details):

```python
# Handle multiple exceptions
try:
    # ... some code ...
    
except* ValueError as eg:
    for e in eg.exceptions:
        print(f"Value error: {e}")
        
except* TypeError as eg:
    for e in eg.exceptions:
        print(f"Type error: {e}")
```

## Annotated Example: Validation Library

```python
# validation.py
# Demonstrates raising custom exceptions

from typing import Any


class ValidationError(Exception):
    """Raised when validation fails.
    
    Attributes:
        field: The field name that failed
        message: Description of the error
    """
    
    def __init__(self, message: str, field: str) -> None:
        super().__init__(message)
        self.field = field
        self.message = message
    
    def __str__(self) -> str:
        return f"Validation error for '{self.field}': {self.message}"


def validate_positive(value: float, field_name: str) -> None:
    """Validate that a number is positive.
    
    Args:
        value: The number to check
        field_name: Name of the field (for error message)
    
    Raises:
        ValidationError: If value is not positive
    """
    if value <= 0:
        raise ValidationError(
            f"{field_name} must be positive, got {value}",
            field=field_name
        )


def validate_not_empty(value: str, field_name: str) -> None:
    """Validate that a string is not empty.
    
    Args:
        value: The string to check
        field_name: Name of the field
    
    Raises:
        ValidationError: If value is empty
    """
    if not value or not value.strip():
        raise ValidationError(
            f"{field_name} cannot be empty",
            field=field_name
        )


def validate_range(value: int, field_name: str, min_val: int, max_val: int) -> None:
    """Validate that a number is within range.
    
    Args:
        value: The number to check
        field_name: Name of the field
        min_val: Minimum allowed value
        max_val: Maximum allowed value
    
    Raises:
        ValidationError: If value is out of range
    """
    if not (min_val <= value <= max_val):
        raise ValidationError(
            f"{field_name} must be between {min_val} and {max_val}, got {value}",
            field=field_name
        )


def validate_user(data: dict[str, Any]) -> None:
    """Validate user data.
    
    Args:
        data: User data dictionary
    
    Raises:
        ValidationError: If any validation fails
    """
    # Validate name
    validate_not_empty(data.get("name", ""), "name")
    
    # Validate age
    age: int = data.get("age", 0)
    validate_positive(age, "age")
    validate_range(age, "age", 0, 150)
    
    # Validate email (simple check)
    email: str = data.get("email", "")
    if "@" not in email:
        raise ValidationError(
            f"Invalid email format: {email}",
            field="email"
        )


def main() -> None:
    # Test valid data
    print("=== Valid User ===")
    valid_user: dict[str, Any] = {
        "name": "Alice",
        "age": 25,
        "email": "alice@example.com"
    }
    
    try:
        validate_user(valid_user)
        print("User is valid!")
    
    except ValidationError as e:
        print(f"Validation failed: {e}")
    
    # Test invalid data
    print("\n=== Invalid User ===")
    invalid_user: dict[str, Any] = {
        "name": "",
        "age": -5,
        "email": "invalid-email"
    }
    
    try:
        validate_user(invalid_user)
    
    except ValidationError as e:
        print(f"Validation failed: {e}")
        print(f"  Field: {e.field}")
    
    # Test with exception chaining (Python 3.11+)
    print("\n=== Exception Chaining ===")
    try:
        # Simulate code that raises an exception
        raise KeyError("missing_key")
    
    except KeyError as original:
        # Wrap it with context
        raise ValidationError("Failed due to missing data") from original


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
=== Valid User ===
User is valid!

=== Invalid User ===
Validation failed: Validation error for 'name': name cannot be empty
  Field: name

=== Exception Chaining ===
Traceback (most recent call last):
  File "validation.py", line 142, in main
    raise KeyError("missing_key")
KeyError: 'missing_key'

The above exception was the direct cause of the following exception:
Traceback (most recent call last):
  ValidationError: Failed due to missing data
```

## Common Mistakes

### ❌ Raising Wrong Exception Type

```python
# WRONG - don't use strings as exceptions
raise "Something went wrong"  # SyntaxError!

# CORRECT - raise exception instances
raise ValueError("Something went wrong")
```

### ❌ Not Re-raising Exceptions

```python
# WRONG - catching and not re-raising loses the error
try:
    risky_operation()
except Exception:
    print("Error!")
    # Error is now lost!

# CORRECT - re-raise to preserve error
try:
    risky_operation()
except Exception:
    print("Error!")
    raise  # Re-raise the exception
```

### ❌ Using Exceptions for Control Flow

```python
# WRONG - don't use exceptions for normal logic
try:
    result = data["key"]
except KeyError:
    result = "default"  # Not recommended!

# CORRECT - use .get() method
result = data.get("key", "default")  # Clean and efficient
```

## Summary

- **`raise`**: Manually throw an exception
- **Custom exceptions**: Create classes inheriting from `Exception`
- **`raise from`**: Chain exceptions to show cause
- **`add_note()`**: Add context (Python 3.11+)
- **Best practice**: Use specific exception types and meaningful messages

## Next Steps

Now let's learn about **exception groups** in **[03_exception_groups.md](./03_exception_groups.md)**.
