# Example91.py
# Topic: Decorator Anatomy - How Decorators Work

# This file demonstrates how decorators work under the hood.


# ============================================================
# Example 1: What is a Decorator?
# ============================================================
print("=== What is a Decorator? ===")

# A decorator is a function that takes a function and returns a new function
def my_decorator(func):
    def wrapper():
        print("Before function call")
        func()
        print("After function call")
    return wrapper

@my_decorator
def say_hello():
    print("Hello!")

# This is equivalent to:
# say_hello = my_decorator(say_hello)

say_hello()


# ============================================================
# Example 2: Decorator Syntax
# ============================================================
print("\n=== Decorator Syntax ===")

def simple_decorator(func):
    def wrapper(*args, **kwargs):
        print(f"Calling {func.__name__}")
        return func(*args, **kwargs)
    return wrapper

# Using @ syntax
@simple_decorator
def add(a, b):
    return a + b

# Equivalent to:
# def add(a, b): return a + b
# add = simple_decorator(add)

result = add(3, 5)
print(f"Result: {result}")


# ============================================================
# Example 3: Decorator with Arguments
# ============================================================
print("\n=== Decorator with Arguments ===")

def decorator_with_args(times):
    def decorator(func):
        def wrapper(*args, **kwargs):
            for _ in range(times):
                result = func(*args, **kwargs)
            return result
        return wrapper
    return decorator

@decorator_with_args(3)
def greet():
    print("Hello!")

greet()


# ============================================================
# Example 4: How @ Works Step by Step
# ============================================================
print("\n=== Step by Step ===")

def trace_decorator(func):
    print(f"1. Starting decorator for {func.__name__}")
    
    def wrapper(*args, **kwargs):
        print(f"2. Before calling {func.__name__}")
        result = func(*args, **kwargs)
        print(f"3. After calling {func.__name__}")
        return result
    
    print(f"4. Returning wrapper for {func.__name__}")
    return wrapper

# Step 1: define function
def example():
    print("   Inside example function")
    return 42

print("Before decorator:")
result = example()
print(f"Result: {result}\n")

# Step 2: apply decorator
print("Applying @trace_decorator:")
example = trace_decorator(example)

print("\nCalling decorated function:")
result = example()
print(f"Result: {result}")


# ============================================================
# Example 5: Multiple Decorators
# ============================================================
print("\n=== Multiple Decorators ===")

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

# Applied from bottom to top
@decorator_a
@decorator_b
def process():
    print("   Processing")
    return "Done"

result = process()
print(f"Result: {result}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Decorator Anatomy")
print("=" * 50)
print("""
DECORATOR ANATOMY:

1. A decorator is a function that:
   - Takes a function as argument
   - Returns a new function (wrapper)

2. Syntax:
   @decorator
   def func(): pass
   # Equivalent to:
   func = decorator(func)

3. Decorator with args:
   @decorator(arg)
   def func(): pass
   # Creates decorator factory

4. Multiple decorators:
   @A
   @B
   def func(): pass
   # Equivalent to: func = A(B(func))
""")
