# Example95.py
# Topic: Comprehensive Review - Decorator Anatomy

# This file provides a comprehensive review of decorator anatomy.


# ============================================================
# Example 1: Complete Decorator Template
# ============================================================
print("=== Complete Decorator Template ===")

from functools import wraps

def proper_decorator(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        # Before
        print(f"Calling {func.__name__}")
        
        # Execute
        result = func(*args, **kwargs)
        
        # After
        print(f"{func.__name__} returned {result}")
        
        return result
    return wrapper

@proper_decorator
def add(a, b):
    """Adds two numbers."""
    return a + b

result = add(3, 5)
print(f"Result: {result}")
print(f"Function name: {add.__name__}")
print(f"Docstring: {add.__doc__}")


# ============================================================
# Example 2: Decorator with Arguments
# ============================================================
print("\n=== Decorator with Arguments ===")

from functools import wraps

def repeat(times):
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            results = []
            for _ in range(times):
                results.append(func(*args, **kwargs))
            return results
        return wrapper
    return decorator

@repeat(times=3)
def greet(name):
    return f"Hello, {name}!"

result = greet("World")
print(f"Result: {result}")


# ============================================================
# Example 3: Stacked Decorators
# ============================================================
print("\n=== Stacked Decorators ===")

from functools import wraps

def decorator_a(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print("A: Before")
        result = func(*args, **kwargs)
        print("A: After")
        return result
    return wrapper

def decorator_b(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print("B: Before")
        result = func(*args, **kwargs)
        print("B: After")
        return result
    return wrapper

@decorator_a
@decorator_b
def process():
    print("   Processing")
    return "Done"

result = process()
print(f"Result: {result}")


# ============================================================
# Example 4: Class Decorators
# ============================================================
print("\n=== Class Decorators ===")

from functools import wraps

def debug_methods(cls):
    for name, method in cls.__dict__.items():
        if callable(method) and not name.startswith('_'):
            setattr(cls, name, debug(method))
    return cls

def debug(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print(f"Calling {func.__name__}")
        result = func(*args, **kwargs)
        print(f"{func.__name__} returned {result}")
        return result
    return wrapper

@debug_methods
class Calculator:
    def add(self, a, b):
        return a + b
    
    def multiply(self, a, b):
        return a * b

calc = Calculator()
calc.add(3, 5)
calc.multiply(4, 6)


# ============================================================
# Example 5: Summary
# ============================================================
print("\n=== Summary ===")

# Key points
print("""
DECORATOR ANATOMY SUMMARY:

1. Basic structure:
   def decorator(func):
       @wraps(func)
       def wrapper(*args, **kwargs):
           # Do something before
           result = func(*args, **kwargs)
           # Do something after
           return result
       return wrapper

2. With arguments:
   def decorator(arg):
       def decorator(func):
           @wraps(func)
           def wrapper(*args, **kwargs):
               return func(*args, **kwargs)
           return wrapper
       return decorator

3. Stacking:
   @A
   @B
   def func(): pass
   # Equivalent to: func = A(B(func))

4. Always use @wraps to preserve metadata!

5. Common mistakes:
   - Forgetting to return
   - Not using @wraps
   - Not accepting *args, **kwargs
   - Wrong decorator order
""")


# ============================================================
# Final Summary
# ============================================================
print("\n" + "=" * 50)
print("COMPREHENSIVE SUMMARY: Decorator Anatomy")
print("=" * 50)
print("""
DECORATOR ANATOMY:

1. What is a decorator?
   - A function that wraps another function
   - Adds functionality without modifying original

2. How it works:
   @decorator
   def func(): pass
   # Equivalent to:
   func = decorator(func)

3. Key components:
   - Original function (func)
   - Wrapper function (inner)
   - Return wrapper

4. functools.wraps:
   - Preserves __name__, __doc__, etc.
   - Always use @wraps!

5. Decorator with args:
   - Need factory function
   - def decorator(args):
       def decorator(func):
           ...

6. Stacking:
   - Applied bottom to top
   - A(B(func)) = @A then @B
""")
