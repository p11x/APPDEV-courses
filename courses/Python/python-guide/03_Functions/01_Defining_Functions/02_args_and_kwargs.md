# *args and **kwargs

## What You'll Learn

- Using `*args` for variable positional arguments
- Using `**kwargs` for variable keyword arguments
- Positional-only parameters (`/`)
- Keyword-only parameters (`*`)
- Python 3.12 parameter syntax

## Prerequisites

- Read [01_function_basics.md](./01_function_basics.md) first

## What Are *args and **kwargs?

These special parameters allow functions to accept **any number** of arguments:

| Parameter | What It Does | Becomes |
|-----------|--------------|---------|
| `*args` | Collects extra positional arguments | Tuple |
| `**kwargs` | Collects extra keyword arguments | Dictionary |

## Using *args

`*args` collects extra positional arguments into a tuple:

```python
def print_all(*args) -> None:
    """Print all arguments."""
    for arg in args:
        print(arg)


print_all(1, 2, 3)      # 1, 2, 3
print_all("a", "b")     # a, b
print_all(1, "hello", True)  # 1, hello, True
```

### With Regular Parameters

```python
def greet(greeting: str, *names: str) -> None:
    """Greet multiple people."""
    for name in names:
        print(f"{greeting}, {name}!")


greet("Hello", "Alice", "Bob", "Charlie")
# Hello, Alice!
# Hello, Bob!
# Hello, Charlie!
```

## Using **kwargs

`**kwargs` collects extra keyword arguments into a dictionary:

```python
def print_info(**kwargs) -> None:
    """Print all keyword arguments."""
    for key, value in kwargs.items():
        print(f"{key}: {value}")


print_info(name="Alice", age=25)
# name: Alice
# age: 25
```

### With Regular Parameters

```python
def create_user(username: str, **info: str) -> None:
    """Create a user with extra info."""
    print(f"Username: {username}")
    for key, value in info.items():
        print(f"  {key}: {value}")


create_user("alice", email="alice@example.com", city="NYC")
# Username: alice
#   email: alice@example.com
#   city: NYC
```

## Combining Both

You can use `*args` and `**kwargs` together:

```python
def flex_func(*args: int, **kwargs: int) -> None:
    print(f"Positional: {args}")
    print(f"Keyword: {kwargs}")


flex_func(1, 2, 3, a=4, b=5)
# Positional: (1, 2, 3)
# Keyword: {'a': 4, 'b': 5}
```

## Positional-Only Parameters (Python 3.8+)

Use `/` to mark parameters as positional-only:

```python
def pos_only(a: int, b: int, /, c: int) -> None:
    """a and b are positional-only, c can be positional or keyword."""
    print(f"a={a}, b={b}, c={c}")


pos_only(1, 2, 3)      # OK - all positional
pos_only(1, 2, c=3)    # OK - c as keyword
# pos_only(1, b=2, c=3)  # ERROR! b is positional-only
```

## Keyword-Only Parameters (Python 3+)

Use `*` to mark parameters as keyword-only:

```python
def kw_only(a: int, *, b: int, c: int) -> None:
    """b and c must be passed as keywords."""
    print(f"a={a}, b={b}, c={c}")


kw_only(1, b=2, c=3)  # OK
# kw_only(1, 2, 3)     # ERROR! b and c are keyword-only
```

## Python 3.12+ Modern Parameter Syntax

Python 3.12 allows combining all parameter types:

```python
def modern_func(
    pos_or_kw: int,           # Can be positional or keyword
    /,                        # Everything before is positional-only
    *args: int,              # Extra positional args
    keyword_only: int,        # Must be keyword
    **kwargs: int            # Extra keyword args
) -> None:
    pass
```

## Annotated Example: Flexible Logger

```python
# flexible_logger.py
# Demonstrates *args and **kwargs in a logging context

from datetime import datetime
from typing import Any


def log(
    message: str,
    level: str = "INFO",
    *tags: str,
    **details: Any
) -> None:
    """Log a message with optional tags and details.
    
    Args:
        message: The log message
        level: Log level (DEBUG, INFO, WARNING, ERROR)
        *tags: Additional tags for the message
        **details: Additional key-value details
    """
    # Build the log line
    timestamp: str = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    
    # Format: [TIMESTAMP] LEVEL: message
    log_line: str = f"[{timestamp}] {level}: {message}"
    
    # Add tags if present
    if tags:
        tags_str: str = " ".join(f"#{tag}" for tag in tags)
        log_line += f" {tags_str}"
    
    # Print the main log line
    print(log_line)
    
    # Print details if present
    if details:
        print("  Details:")
        for key, value in details.items():
            print(f"    {key}: {value}")


def log_error(message: str, *args: Any, **kwargs: Any) -> None:
    """Log an error with automatic ERROR level."""
    # Pass all arguments to log with ERROR level
    log(message, "ERROR", *args, **kwargs)


def log_debug(message: str, *args: Any, **kwargs: Any) -> None:
    """Log a debug message."""
    log(message, "DEBUG", *args, **kwargs)


def main() -> None:
    # Basic logging
    print("=== Basic Logging ===")
    log("Application started")
    log("User logged in", "WARNING")
    log("Payment processed", level="INFO")
    
    # With tags
    print("\n=== With Tags ===")
    log("Performance issue detected", "WARNING", "performance", "urgent")
    
    # With details
    print("\n=== With Details ===")
    log("User created", details={"username": "alice", "user_id": 123})
    
    # With both
    print("\n=== With Tags and Details ===")
    log(
        "Database query slow",
        "DEBUG",
        "database", "performance",
        query="SELECT * FROM users",
        duration_ms=2500,
        table="users"
    )
    
    # Using helper functions
    print("\n=== Using Helper Functions ===")
    log_error("Failed to connect to database", error_code=500)
    log_debug("Cache hit", key="user:123")


# Run the program
if __name__ == "__main__":
    main()
```

### Sample Output

```
=== Basic Logging ===
[2024-01-15 10:30:45] INFO: Application started
[2024-01-15 10:30:45] WARNING: User logged in
[2024-01-15 10:30:45] INFO: Payment processed

=== With Tags ===
[2024-01-15 10:30:45] WARNING: Performance issue detected #performance #urgent

=== With Details ===
[2024-01-15 10:30:45] INFO: User created
  Details:
    username: alice
    user_id: 123

=== With Tags and Details ===
[2024-01-15 10:30:45] DEBUG: Database query slow #database #performance
  Details:
    query: SELECT * FROM users
    duration_ms: 2500
    table: users

=== Using Helper Functions ===
[2024-01-15 10:30:45] ERROR: Failed to connect to database
  Details:
    error_code: 500
[2024-01-15 10:30:45] DEBUG: Cache hit
  Details:
    key: user:123
```

## Common Mistakes

### ❌ Forgetting to Unpack

```python
# WRONG - passes a tuple, not multiple arguments
numbers: tuple[int, ...] = (1, 2, 3)
print_all(numbers)  # (1, 2, 3) - one argument!

# CORRECT - unpack with *
print_all(*numbers)  # 1, 2, 3 - three arguments!
```

### ❌ Confusing *args and **kwargs

```python
# *args - positional arguments (becomes tuple)
# **kwargs - keyword arguments (becomes dictionary)

def f(*args, **kwargs):
    print(args)    # Tuple of positional
    print(kwargs)  # Dict of keyword
```

## Summary

- **`*args`**: Collects extra positional arguments into a tuple
- **`**kwargs`**: Collects extra keyword arguments into a dictionary
- **`/`**: Positional-only parameters (Python 3.8+)
- **`*`**: Keyword-only parameters
- **Can combine**: All parameter types in one function

## Next Steps

Now let's learn about **type hints in functions** in **[03_type_hints_in_functions.md](./03_type_hints_in_functions.md)**.
