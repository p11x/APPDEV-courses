# Example85.py
# Topic: Comprehensive Review - Pure Functions

# This file provides a comprehensive review of pure functions.


# ============================================================
# Example 1: Pure Function Characteristics
# ============================================================
print("=== Pure Function Characteristics ===")

# Characteristics of pure functions:
# 1. Same input → Same output
def add(a, b):
    return a + b

# Always returns same result for same input
print(f"add(2, 3) = {add(2, 3)}")
print(f"add(2, 3) = {add(2, 3)}")

# 2. No side effects
def greet(name):
    return f"Hello, {name}!"  # Just returns, no side effects


# ============================================================
# Example 2: Impure vs Pure
# ============================================================
print("\n=== Impure vs Pure ===")

# IMPURE
counter = 0
def increment_impure():
    global counter
    counter += 1
    return counter

# PURE
def increment_pure(counter):
    return counter + 1

# Test
print(f"Impure: {increment_impure()}")
print(f"Impure: {increment_impure()}")

counter = 0
print(f"Pure: {increment_pure(counter)}")
print(f"Pure: {increment_pure(counter)}")


# ============================================================
# Example 3: Modifying Arguments
# ============================================================
print("\n=== Modifying Arguments ===")

# IMPURE: Modifies list
def add_to_list_impure(lst, item):
    lst.append(item)
    return lst

# PURE: Returns new list
def add_to_list_pure(lst, item):
    return lst + [item]

# Test
lst1 = [1, 2, 3]
result1 = add_to_list_impure(lst1, 4)
print(f"Impure: lst={lst1}, result={result1}")

lst2 = [1, 2, 3]
result2 = add_to_list_pure(lst2, 4)
print(f"Pure: lst={lst2}, result={result2}")


# ============================================================
# Example 4: Pure in Classes
# ============================================================
print("\n=== Pure in Classes ===")

class Calculator:
    def __init__(self, value=0):
        self.value = value
    
    # Impure: depends on internal state
    def add_impure(self, n):
        self.value += n
        return self.value
    
    # Pure: returns new value
    def add_pure(self, n):
        return self.value + n

calc = Calculator(10)
print(f"Impure: {calc.add_impure(5)}")
print(f"Impure: {calc.add_impure(5)}")

calc2 = Calculator(10)
print(f"Pure: {calc2.add_pure(5)}")
print(f"Pure: {calc2.add_pure(5)}")


# ============================================================
# Example 5: Composing Pure Functions
# ============================================================
print("\n=== Composing Pure Functions ===")

def double(x):
    return x * 2

def increment(x):
    return x + 1

def square(x):
    return x * x

# Compose: square(increment(double(5)))
result = square(increment(double(5)))
print(f"square(increment(double(5))) = {result}")

# Using reduce to compose
from functools import reduce

def compose(*functions):
    """Compose functions right to left."""
    return lambda x: reduce(lambda acc, f: f(acc), reversed(functions), x)

f = compose(square, increment, double)
print(f"composed(5) = {f(5)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMPREHENSIVE SUMMARY: PURE FUNCTIONS")
print("=" * 50)
print("""
PURE FUNCTIONS:

Characteristics:
- Same input → Same output
- No side effects
- Don't modify arguments

Benefits:
- Easy to test
- Easy to reason about
- Cacheable
- Parallelizable
- Composable

Remember:
- Avoid global variables
- Don't modify arguments
- Separate I/O from logic
- Return new objects
""")
