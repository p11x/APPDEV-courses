# *args and **kwargs

## What You'll Learn

- Understanding variable positional arguments with *args
- Working with keyword arguments using **kwargs
- Unpacking iterables into function calls
- Combining regular parameters with *args and **kwargs

## Prerequisites

- Read [04_return_values.md](./04_return_values.md) first

## Understanding *args

The `*args` parameter allows a function to accept any number of positional arguments. These arguments are packed into a tuple.

```python
# args_demo.py

def sum_all(*args: int) -> int:
    """Sum all provided arguments."""
    total = 0
    for num in args:
        total += num
    return total

# Usage
result = sum_all(1, 2, 3, 4, 5)  # Returns 15
print(result)
```

## Understanding **kwargs

The `**kwargs` parameter allows a function to accept any number of keyword arguments. These are packed into a dictionary.

```python
# kwargs_demo.py

def create_user(**kwargs: str) -> dict[str, str]:
    """Create a user dictionary from keyword arguments."""
    return {
        "username": kwargs.get("username", "anonymous"),
        "email": kwargs.get("email", "no@email.com"),
        "age": kwargs.get("age", "unknown")
    }

# Usage
user = create_user(username="alice", email="alice@example.com", age="25")
print(user)
```

## Combining All Parameter Types

You can use regular parameters, *args, and **kwargs together in any order (with specific rules).

```python
# combined_params.py

def func(required: str, *args: int, **kwargs: str) -> None:
    """Demonstrates all parameter types."""
    print(f"Required: {required}")
    print(f"Args: {args}")
    print(f"Kwargs: {kwargs}")

# Usage
func("hello", 1, 2, 3, name="Alice", age="30")
```

## Annotated Full Example

```python
# args_kwargs_demo.py
"""Complete demonstration of *args and **kwargs."""

from typing import Any


def print_all(prefix: str, *args: Any, **kwargs: Any) -> None:
    """Print all arguments with a prefix."""
    print(f"{prefix}:")
    
    # Print positional arguments
    for i, arg in enumerate(args):
        print(f"  Arg {i}: {arg}")
    
    # Print keyword arguments
    for key, value in kwargs.items():
        print(f"  {key}: {value}")


def configure_server(host: str, port: int = 80, *extra_options: str, **config: Any) -> dict[str, Any]:
    """Build server configuration from various inputs."""
    config["host"] = host
    config["port"] = port
    
    # Add extra options as list
    if extra_options:
        config["options"] = list(extra_options)
    
    return config


def main() -> None:
    # Using *args
    print_all("Numbers", 1, 2, 3, 4, 5)
    
    # Using **kwargs
    print_all("User Info", name="Alice", role="Admin")
    
    # Combined
    config = configure_server("localhost", 8080, "debug", "ssl", 
                               workers=4, timeout=30)
    print(f"Server config: {config}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding variable positional arguments with *args
- Working with keyword arguments using **kwargs
- Unpacking iterables into function calls

## Next Steps

Continue to **[06_docstrings_and_annotations.md](./06_docstrings_and_annotations.md)**
