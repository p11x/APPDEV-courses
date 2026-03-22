# Example92.py
# Topic: functools.wraps - Preserving Function Metadata

# This file demonstrates why and how to use functools.wraps.


# ============================================================
# Example 1: Problem - Lost Metadata
# ============================================================
print("=== Problem: Lost Metadata ===")

def bad_decorator(func):
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@bad_decorator
def add(a, b):
    """Adds two numbers."""
    return a + b

# Problem: metadata is lost
print(f"Function name: {add.__name__}")  # Shows 'wrapper' instead of 'add'
print(f"Docstring: {add.__doc__}")       # Shows None instead of "Adds two numbers"


# ============================================================
# Example 2: Solution - functools.wraps
# ============================================================
print("\n=== Solution: functools.wraps ===")

from functools import wraps

def good_decorator(func):
    @wraps(func)  # Preserves original function's metadata
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@good_decorator
def multiply(a, b):
    """Multiplies two numbers."""
    return a * b

# Now metadata is preserved!
print(f"Function name: {multiply.__name__}")  # Shows 'multiply'
print(f"Docstring: {multiply.__doc__}")       # Shows "Multiplies two numbers"


# ============================================================
# Example 3: What wraps preserves
# ============================================================
print("\n=== What wraps preserves ===")

from functools import wraps

def my_decorator(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@my_decorator
def example_func(a, b):
    """This is the docstring."""
    return a + b

example_func.__annotations__ = {'a': int, 'b': int, 'return': int}

print(f"__name__: {example_func.__name__}")
print(f"__doc__: {example_func.__doc__}")
print(f"__module__: {example_func.__module__}")
print(f"__qualname__: {example_func.__qualname__}")
print(f"__annotations__: {example_func.__annotations__}")


# ============================================================
# Example 4: Why it matters - introspection
# ============================================================
print("\n=== Why it matters ===")

import inspect

def bad_decorator(func):
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

def good_decorator(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@bad_decorator
def bad_func():
    pass

@good_decorator
def good_func():
    """My function."""
    pass

# Using inspect
print(f"Bad func signature: {inspect.signature(bad_func)}")
print(f"Good func signature: {inspect.signature(good_func)}")


# ============================================================
# Example 5: Using wraps with args
# ============================================================
print("\n=== wraps with decorator arguments ===")

from functools import wraps

def decorator_with_args(times):
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            results = []
            for _ in range(times):
                results.append(func(*args, **kwargs))
            return results
        return wrapper
    return decorator

@decorator_with_args(3)
def get_value():
    """Returns a constant value."""
    return 42

print(f"Function name: {get_value.__name__}")
print(f"Docstring: {get_value.__doc__}")
print(f"Result: {get_value()}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: functools.wraps")
print("=" * 50)
print("""
WHY USE @wraps:
- Preserves original function metadata
- __name__, __doc__, __module__, __qualname__
- __annotations__
- Makes introspection work correctly

HOW TO USE:
from functools import wraps

def decorator(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

ALWAYS use @wraps in decorators!
""")
