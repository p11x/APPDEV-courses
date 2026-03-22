# Example93.py
# Topic: Common Mistakes with Decorators

# This file shows common mistakes when writing decorators.


# ============================================================
# Example 1: Forgetting to Return
# ============================================================
print("=== Mistake 1: Forgot to Return ===")

# BAD: No return statement
def bad_decorator(func):
    def wrapper(*args, **kwargs):
        print("Before")
        func(*args, **kwargs)  # Forgot to return!
        print("After")

@bad_decorator
def add(a, b):
    return a + b

result = add(3, 5)
print(f"Result: {result}")  # None!

# GOOD: Return the result
def good_decorator(func):
    def wrapper(*args, **kwargs):
        print("Before")
        result = func(*args, **kwargs)
        print("After")
        return result
    return wrapper

@good_decorator
def add2(a, b):
    return a + b

result = add2(3, 5)
print(f"Result: {result}")


# ============================================================
# Example 2: Not Preserving Metadata
# ============================================================
print("\n=== Mistake 2: Not Preserving Metadata ===")

# BAD: No @wraps
def bad_decorator(func):
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@bad_decorator
def my_func():
    """My documentation."""
    pass

print(f"Name (bad): {my_func.__name__}")  # Shows wrapper!

# GOOD: Use @wraps
from functools import wraps

def good_decorator(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@good_decorator
def my_func2():
    """My documentation."""
    pass

print(f"Name (good): {my_func2.__name__}")  # Shows my_func2!


# ============================================================
# Example 3: Not Handling Arguments
# ============================================================
print("\n=== Mistake 3: Not Handling Arguments ===")

# BAD: Doesn't accept *args, **kwargs
def bad_decorator(func):
    def wrapper():
        return func()  # Only works with no args!
    return wrapper

@bad_decorator
def add_bad(a, b):
    return a + b

# add_bad(3, 5)  # Would fail!

# GOOD: Accept *args, **kwargs
def good_decorator(func):
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@good_decorator
def add_good(a, b):
    return a + b

print(f"Result: {add_good(3, 5)}")  # Works!


# ============================================================
# Example 4: Decorator Order Matters
# ============================================================
print("\n=== Mistake 4: Wrong Decorator Order ===")

def decorator_a(func):
    def wrapper(*args, **kwargs):
        print("A: Before")
        result = func(*args, **kwargs)
        print("A: After")
        return result
    return wrapper

def decorator_b(func):
    def wrapper(*args, **kwargs):
        print("B: Before")
        result = func(*args, **kwargs)
        print("B: After")
        return result
    return wrapper

# Order 1: A then B
@decorator_a
@decorator_b
def func1():
    print("   func1 executing")
    return "Result1"

# Order 2: B then A
@decorator_b
@decorator_a
def func2():
    print("   func2 executing")
    return "Result2"

print("Order: @decorator_a then @decorator_b:")
result1 = func1()

print("\nOrder: @decorator_b then @decorator_a:")
result2 = func2()


# ============================================================
# Example 5: Mutable State in Decorator
# ============================================================
print("\n=== Mistake 5: Mutable State ===")

# BAD: Using mutable default argument
def bad_counter(func):
    count = [0]  # Mutable default!
    def wrapper(*args, **kwargs):
        count[0] += 1
        print(f"Called {count[0]} times")
        return func(*args, **kwargs)
    return wrapper

@bad_counter
def example1():
    pass

example1()
example1()
example1()

# GOOD: Use closure or function attribute
def good_counter(func):
    def wrapper(*args, **kwargs):
        wrapper.calls += 1
        print(f"Called {wrapper.calls} times")
        return func(*args, **kwargs)
    wrapper.calls = 0
    return wrapper

@good_counter
def example2():
    pass

example2()
example2()
example2()


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMMON MISTAKES: Decorators")
print("=" * 50)
print("""
AVOID:
1. Forgetting to return the result
2. Not using @wraps to preserve metadata
3. Not accepting *args, **kwargs
4. Wrong decorator order
5. Mutable state in decorator

REMEMBER:
- Always return something from wrapper
- Use @wraps from functools
- Accept *args, **kwargs
- Order matters - bottom to top
- Use closure for state
""")
