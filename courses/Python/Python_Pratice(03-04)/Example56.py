# Example56.py
# Topic: Decorators - Basic Concepts and Syntax

# This file demonstrates the basics of decorators in Python,
# including how they work and how to create them.


# ============================================================
# Understanding Decorators - Without Decorator
# ============================================================
print("=== Without Decorator ===")

def simple_function():
    """A simple function."""
    return "Hello!"

# Calling it normally
result = simple_function()
print(f"Result: {result}")


# ============================================================
# Manual Wrapping (Before Decorators)
# ============================================================
print("\n=== Manual Wrapping ===")

def simple_function():
    return "Hello!"

def simple_function_with_logging():
    print("Calling simple_function...")
    result = simple_function()
    print("simple_function finished!")
    return result

result = simple_function_with_logging()
print(f"Result: {result}")


# ============================================================
# Basic Decorator
# ============================================================
print("\n=== Basic Decorator ===")

def my_decorator(func):
    """A basic decorator that wraps a function."""
    def wrapper():
        print("Before calling the function")
        func()
        print("After calling the function")
    return wrapper

@my_decorator
def say_hello():
    print("Hello!")

say_hello()


# ============================================================
# Decorator with Arguments
# ============================================================
print("\n=== Decorator with Arguments ===")

def repeat(times):
    """Decorator that repeats function execution."""
    def decorator(func):
        def wrapper(*args, **kwargs):
            for _ in range(times):
                result = func(*args, **kwargs)
            return result
        return wrapper
    return decorator

@repeat(times=3)
def greet(name):
    print(f"Hello, {name}!")

greet("Alice")


# ============================================================
# Preserving Function Metadata with @wraps
# ============================================================
print("\n=== Preserving Metadata with @wraps ===")

from functools import wraps

def my_decorator(func):
    @wraps(func)  # Preserves original function's metadata
    def wrapper(*args, **kwargs):
        print("Running decorated function")
        return func(*args, **kwargs)
    return wrapper

@my_decorator
def original_function():
    """This is the original function's docstring."""
    return "Original result"

print(f"Function name: {original_function.__name__}")
print(f"Function doc: {original_function.__doc__}")
result = original_function()
print(f"Result: {result}")


# ============================================================
# Real-life Example 1: Timer Decorator
# ============================================================
print("\n=== Real-life: Timer Decorator ===")

import time

def timer(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        start_time = time.time()
        result = func(*args, **kwargs)
        end_time = time.time()
        print(f"Function '{func.__name__}' took {end_time - start_time:.4f} seconds")
        return result
    return wrapper

@timer
def slow_function():
    time.sleep(0.5)
    return "Done!"

result = slow_function()
print(f"Result: {result}")


# ============================================================
# Real-life Example 2: Debug Decorator
# ============================================================
print("\n=== Real-life: Debug Decorator ===")

def debug(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print(f"Calling {func.__name__} with args={args}, kwargs={kwargs}")
        result = func(*args, **kwargs)
        print(f"{func.__name__} returned: {result}")
        return result
    return wrapper

@debug
def add(a, b):
    return a + b

result = add(5, 3)


# ============================================================
# Real-life Example 3: Validation Decorator
# ============================================================
print("\n=== Real-life: Validation Decorator ===")

def validate_positive(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        for arg_name, arg_value in kwargs.items():
            if isinstance(arg_value, (int, float)) and arg_value < 0:
                raise ValueError(f"Argument '{arg_name}' must be positive, got {arg_value}")
        return func(*args, **kwargs)
    return wrapper

@validate_positive
def calculate_area(width, height):
    return width * height

try:
    area = calculate_area(10, 5)
    print(f"Area: {area}")
    
    # This will raise an error
    area = calculate_area(-10, 5)
except ValueError as e:
    print(f"Error: {e}")


# ============================================================
# Real-life Example 4: Logging Decorator
# ============================================================
print("\n=== Real-life: Logging Decorator ===")

def log_calls(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print(f"[LOG] Calling {func.__name__}")
        try:
            result = func(*args, **kwargs)
            print(f"[LOG] {func.__name__} completed successfully")
            return result
        except Exception as e:
            print(f"[LOG] {func.__name__} raised {type(e).__name__}: {e}")
            raise
    return wrapper

@log_calls
def divide(a, b):
    return a / b

try:
    result = divide(10, 2)
    print(f"Result: {result}")
except:
    pass

try:
    result = divide(10, 0)
except:
    pass


# ============================================================
# Real-life Example 5: Authorization Decorator
# ============================================================
print("\n=== Real-life: Authorization Decorator ===")

def require_admin(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        user_role = kwargs.get('user_role', 'guest')
        if user_role != 'admin':
            raise PermissionError(f"Admin access required, got {user_role}")
        return func(*args, **kwargs)
    return wrapper

@require_admin
def delete_user(user_id, user_role='guest'):
    return f"User {user_id} deleted"

try:
    result = delete_user(123, user_role='admin')
    print(f"Result: {result}")
    
    result = delete_user(123, user_role='user')
except PermissionError as e:
    print(f"Error: {e}")


# ============================================================
# Real-life Example 6: Caching Decorator
# ============================================================
print("\n=== Real-life: Caching Decorator ===")

def cache(func):
    cache_data = {}
    
    @wraps(func)
    def wrapper(*args):
        if args in cache_data:
            print(f"Cache hit for {args}")
            return cache_data[args]
        result = func(*args)
        cache_data[args] = result
        return result
    return wrapper

@cache
def fibonacci(n):
    if n <= 1:
        return n
    return fibonacci(n-1) + fibonacci(n-2)

# First call - slow
print("Computing fibonacci(10)...")
result = fibonacci(10)
print(f"Result: {result}")

# Second call - fast (cached)
print("Computing fibonacci(10) again...")
result = fibonacci(10)
print(f"Result: {result}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("KEY TAKEAWAYS:")
print("=" * 50)
print("""
1. DECORATOR: A function that wraps another function to add functionality

2. SYNTAX: @decorator_name above function definition

3. @wraps: Preserves original function's metadata

4. COMMON USES:
   - Timing function execution
   - Debugging/logging
   - Validation
   - Authorization
   - Caching

5. DECORATOR WITH ARGUMENTS:
   def decorator_with_args(arg):
       def decorator(func):
           def wrapper(*args, **kwargs):
               # code
           return wrapper
       return decorator
""")
