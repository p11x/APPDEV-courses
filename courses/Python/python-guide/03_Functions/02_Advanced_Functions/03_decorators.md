# Decorators

## What You'll Learn

- What decorators are and how they work
- Building decorators from scratch
- Using @wraps to preserve function metadata
- Common built-in decorators: @property, @staticmethod, @classmethod
- Python 3.12 @override decorator

## Prerequisites

- Read [02_closures_and_scope.md](./02_closures_and_scope.md) first

## What Is a Decorator?

A decorator is a **function that modifies another function**. It's like wrapping a gift — you add functionality without changing the original.

### Before Decorators

```python
def say_hello() -> None:
    print("Hello!")


# Manually adding timing
import time

def say_hello_timed() -> None:
    start: float = time.time()
    say_hello()
    end: float = time.time()
    print(f"Took {end - start:.4f} seconds")
```

### With Decorators

```python
import time
from functools import wraps


def timer(func: Callable) -> Callable:
    """Decorator that times function execution."""
    
    @wraps(func)  # Preserves original function's metadata
    def wrapper(*args, **kwargs):
        start: float = time.time()
        result = func(*args, **kwargs)
        end: float = time.time()
        print(f"{func.__name__} took {end - start:.4f} seconds")
        return result
    
    return wrapper


@timer
def say_hello() -> None:
    print("Hello!")


say_hello()
```

## Building a Decorator

```python
from functools import wraps
from typing import Callable


def my_decorator(func: Callable) -> Callable:
    """A basic decorator template."""
    
    @wraps(func)  # Preserves func's name and docstring
    def wrapper(*args, **kwargs):
        # Code before
        print("Before calling function")
        
        # Call the original function
        result = func(*args, **kwargs)
        
        # Code after
        print("After calling function")
        
        return result
    
    return wrapper


@my_decorator
def say_hello() -> None:
    """Says hello."""
    print("Hello!")


say_hello()
# Output:
# Before calling function
# Hello!
# After calling function
```

## Built-in Decorators

### @property

```python
class Circle:
    def __init__(self, radius: float) -> None:
        self._radius = radius
    
    @property
    def radius(self) -> float:
        """Get the radius."""
        return self._radius
    
    @property
    def diameter(self) -> float:
        """Calculate diameter (read-only)."""
        return self._radius * 2


circle = Circle(5)
print(circle.radius)    # 5
print(circle.diameter)  # 10
```

### @staticmethod

```python
class Math:
    @staticmethod
    def add(a: int, b: int) -> int:
        """Add two numbers."""
        return a + b


# Call without creating an instance
result: int = Math.add(5, 3)
```

### @classmethod

```python
class User:
    def __init__(self, name: str) -> None:
        self.name = name
    
    @classmethod
    def from_dict(cls, data: dict) -> "User":
        """Create User from dictionary."""
        return cls(data["name"])


# Create without __init__
user = User.from_dict({"name": "Alice"})
```

### @override (Python 3.12+)

```python
from typing import override


class Animal:
    def speak(self) -> str:
        raise NotImplementedError


class Dog(Animal):
    @override  # Ensures we're actually overriding a parent method
    def speak(self) -> str:
        return "Woof!"
```

## Annotated Example: Decorator Library

```python
# decorator_demo.py
# Demonstrates decorators in practice

from functools import wraps
from typing import Callable, Any


def debug(func: Callable) -> Callable:
    """Print function call details."""
    
    @wraps(func)
    def wrapper(*args, **kwargs) -> Any:
        print(f"Calling {func.__name__}")
        print(f"  Args: {args}")
        print(f"  Kwargs: {kwargs}")
        
        result = func(*args, **kwargs)
        
        print(f"  Result: {result}")
        return result
    
    return wrapper


def retry(max_attempts: int = 3):
    """Retry decorator - attempts function multiple times on failure."""
    
    def decorator(func: Callable) -> Callable:
        @wraps(func)
        def wrapper(*args, **kwargs) -> Any:
            for attempt in range(max_attempts):
                try:
                    return func(*args, **kwargs)
                except Exception as e:
                    if attempt == max_attempts - 1:
                        raise
                    print(f"Attempt {attempt + 1} failed: {e}, retrying...")
            return None  # Should not reach here
        return wrapper
    return decorator


def cache(func: Callable) -> Callable:
    """Simple caching decorator."""
    # Store results in a dictionary
    cache_data: dict = {}
    
    @wraps(func)
    def wrapper(*args) -> Any:
        if args in cache_data:
            print(f"Cache hit for {args}")
            return cache_data[args]
        
        result = func(*args)
        cache_data[args] = result
        return result
    
    return wrapper


@debug
def add(a: int, b: int) -> int:
    """Add two numbers."""
    return a + b


@cache
def expensive_operation(n: int) -> int:
    """Simulate expensive computation."""
    print(f"Computing for {n}...")
    return n * n


@retry(max_attempts=3)
def unreliable_function() -> str:
    """Sometimes fails, sometimes succeeds."""
    import random
    if random.random() < 0.7:
        raise RuntimeError("Failed!")
    return "Success!"


class Calculator:
    """Calculator class demonstrating built-in decorators."""
    
    def __init__(self, value: int = 0) -> None:
        self._value = value
    
    @property
    def value(self) -> int:
        """Get current value."""
        return self._value
    
    @value.setter
    def value(self, new_value: int) -> None:
        """Set value with validation."""
        if new_value < 0:
            raise ValueError("Value cannot be negative")
        self._value = new_value
    
    @staticmethod
    def add(a: int, b: int) -> int:
        """Static method - no instance needed."""
        return a + b
    
    @classmethod
    def from_string(cls, s: str) -> "Calculator":
        """Create Calculator from string."""
        return cls(int(s))


def main() -> None:
    print("=== Debug Decorator ===")
    result: int = add(5, 3)
    
    print("\n=== Cache Decorator ===")
    print(expensive_operation(10))
    print(expensive_operation(10))  # Cached!
    print(expensive_operation(20))
    
    print("\n=== Retry Decorator ===")
    for i in range(3):
        try:
            result: str = unreliable_function()
            print(f"Result: {result}")
        except RuntimeError as e:
            print(f"Final attempt failed: {e}")
    
    print("\n=== Built-in Decorators ===")
    calc = Calculator(10)
    print(f"Value: {calc.value}")
    
    calc.value = 20
    print(f"New value: {calc.value}")
    
    # Static method without instance
    print(f"Static add: {Calculator.add(3, 4)}")
    
    # Class method
    calc2 = Calculator.from_string("42")
    print(f"From string: {calc2.value}")


if __name__ == "__main__":
    main()
```

### Output

```
=== Debug Decorator ===
Calling add
  Args: (5, 3)
  Kwargs: {}
  Result: 8
  Result: 8

=== Cache Decorator ===
Computing for 10...
100
Cache hit for (10,)
100
Computing for 20...
400

=== Retry Decorator ===
Attempt 1 failed: Failed!, retrying...
Attempt 2 failed: Failed!, retrying...
Result: Success!

=== Built-in Decorators ===
Value: 10
New value: 20
Static add: 7
From string: 42
```

## Summary

- **Decorator**: Function that wraps another function
- **`@wraps`**: Preserves original function metadata
- **`@property`**: Creates getter methods
- **`@staticmethod`**: Creates methods that don't need self
- **`@classmethod`**: Creates methods that get class as first argument
- **`@override`**: Ensures method is overriding parent (Python 3.12+)

## Next Steps

Now let's learn about **functional tools** in **[03_Functional_Tools/01_map_filter_reduce.md](../03_Functional_Tools/01_map_filter_reduce.md)**.
