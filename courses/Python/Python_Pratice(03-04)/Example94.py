# Example94.py
# Topic: Real-World Decorator Examples

# This file provides practical real-world examples of decorators.


# ============================================================
# Example 1: Timing Decorator
# ============================================================
print("=== Real-world: Timing Decorator ===")

import time
from functools import wraps

def timer(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        start = time.perf_counter()
        result = func(*args, **kwargs)
        end = time.perf_counter()
        print(f"{func.__name__} took {end - start:.4f} seconds")
        return result
    return wrapper

@timer
def slow_function():
    time.sleep(0.1)
    return "Done"

result = slow_function()
print(f"Result: {result}")


# ============================================================
# Example 2: Retry Decorator
# ============================================================
print("\n=== Real-world: Retry Decorator ===")

from functools import wraps
import random

def retry(max_attempts=3, delay=1):
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            for attempt in range(max_attempts):
                try:
                    return func(*args, **kwargs)
                except Exception as e:
                    if attempt == max_attempts - 1:
                        raise
                    print(f"Attempt {attempt + 1} failed: {e}. Retrying...")
                    time.sleep(delay)
            return None
        return wrapper
    return decorator

@retry(max_attempts=3, delay=0.1)
def unstable_function():
    if random.random() < 0.7:
        raise ValueError("Random failure!")
    return "Success!"

try:
    result = unstable_function()
    print(f"Result: {result}")
except ValueError as e:
    print(f"Failed: {e}")


# ============================================================
# Example 3: Caching Decorator
# ============================================================
print("\n=== Real-world: Caching Decorator ===")

from functools import wraps

def memoize(func):
    cache = {}
    
    @wraps(func)
    def wrapper(*args):
        if args not in cache:
            cache[args] = func(*args)
        return cache[args]
    
    wrapper.cache = cache
    return wrapper

@memoize
def fibonacci(n):
    if n < 2:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)

print(f"Fibonacci(30): {fibonacci(30)}")
print(f"Cache size: {len(fibonacci.cache)}")


# ============================================================
# Example 4: Logging Decorator
# ============================================================
print("\n=== Real-world: Logging Decorator ===")

from functools import wraps
import logging

def logged(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print(f"Calling {func.__name__} with args={args}, kwargs={kwargs}")
        result = func(*args, **kwargs)
        print(f"{func.__name__} returned {result}")
        return result
    return wrapper

@logged
def calculate(a, b, operation='add'):
    if operation == 'add':
        return a + b
    elif operation == 'multiply':
        return a * b
    return a - b

result = calculate(5, 3, operation='multiply')
print(f"Final result: {result}")


# ============================================================
# Example 5: Validation Decorator
# ============================================================
print("\n=== Real-world: Validation Decorator ===")

from functools import wraps

def validate(**validators):
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            # Get function parameters
            import inspect
            sig = inspect.signature(func)
            bound = sig.bind(*args, **kwargs)
            bound.apply_defaults()
            
            # Validate
            for param_name, validator in validators.items():
                if param_name in bound.arguments:
                    value = bound.arguments[param_name]
                    if not validator(value):
                        raise ValueError(f"Validation failed for {param_name}={value}")
            
            return func(*args, **kwargs)
        return wrapper
    return decorator

@validate(age=lambda x: x >= 18, name=lambda x: len(x) >= 2)
def register(name, age):
    return f"Registered {name}, age {age}"

try:
    result = register("John", 25)
    print(f"Success: {result}")
except ValueError as e:
    print(f"Error: {e}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("REAL-WORLD DECORATOR EXAMPLES")
print("=" * 50)
print("""
Common real-world uses:
- @timer: Measure execution time
- @retry: Retry failed operations
- @memoize: Cache expensive computations
- @logged: Log function calls
- @validate: Validate inputs

Decorators are great for:
- Cross-cutting concerns
- Reusable functionality
- Code organization
- Separation of concerns
""")
