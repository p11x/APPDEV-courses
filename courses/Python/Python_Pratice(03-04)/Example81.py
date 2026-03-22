# Example81.py
# Topic: Pure Functions - Basics

# This file demonstrates pure functions in Python.


# ============================================================
# Example 1: What is a Pure Function?
# ============================================================
print("=== Pure Functions ===")

# PURE: Same input always gives same output, no side effects
def add(a, b):
    """Pure function: no side effects."""
    return a + b

def multiply(a, b):
    """Pure function: no side effects."""
    return a * b

def square(x):
    """Pure function."""
    return x * x

# Test purity
print(f"add(2, 3) = {add(2, 3)}")
print(f"add(2, 3) = {add(2, 3)}")  # Same result always
print(f"multiply(4, 5) = {multiply(4, 5)}")
print(f"square(3) = {square(3)}")


# ============================================================
# Example 2: Impure Functions (Side Effects)
# ============================================================
print("\n=== Impure Functions ===")

# IMPURE: Has side effects or depends on external state

# Side effect: printing
def greet_impure(name):
    print(f"Hello, {name}!")  # Side effect: prints
    return f"Hello, {name}!"

# Modifies global state
counter = 0
def increment_impure():
    global counter
    counter += 1  # Side effect: modifies global
    return counter

# Depends on external state
base_price = 100
def get_price_impure():
    return base_price  # Depends on external variable

# Modifies input
def add_item_impure(items, item):
    items.append(item)  # Side effect: modifies input
    return items


# ============================================================
# Example 3: Pure vs Impure Comparison
# ============================================================
print("\n=== Pure vs Impure ===")

# Pure version
def calculate_total_pure(items):
    """Pure: doesn't modify items."""
    return sum(item['price'] * item['qty'] for item in items)

# Impure version
def calculate_total_impure(items):
    """Impure: modifies the input list."""
    for item in items:
        item['total'] = item['price'] * item['qty']
    return items

# Test
items = [{'price': 10, 'qty': 2}, {'price': 5, 'qty': 3}]
total = calculate_total_pure(items)
print(f"Pure total: {total}")
print(f"Original items unchanged: {items}")

# Impure - notice the original is modified
items2 = [{'price': 10, 'qty': 2}, {'price': 5, 'qty': 3}]
calculate_total_impure(items2)
print(f"Impure - items modified: {items2}")


# ============================================================
# Example 4: Benefits of Pure Functions
# ============================================================
print("\n=== Benefits of Pure Functions ===")

# 1. Testable
def calculate_tax_pure(amount, rate):
    """Easy to test."""
    return amount * rate

assert calculate_tax_pure(100, 0.1) == 10
assert calculate_tax_pure(200, 0.1) == 20
print("Tests passed!")

# 2. Predictable
def double_pure(x):
    return x * 2

# Always same result
print(f"double(5) = {double_pure(5)}")
print(f"double(5) = {double_pure(5)}")

# 3. Cacheable (can use memoization)
from functools import lru_cache

@lru_cache(maxsize=10)
def fibonacci_cached(n):
    """Pure functions can be cached effectively."""
    if n < 2:
        return n
    return fibonacci_cached(n-1) + fibonacci_cached(n-2)

print(f"fibonacci(30) = {fibonacci_cached(30)}")


# ============================================================
# Example 5: Pure Function Patterns
# ============================================================
print("\n=== Pure Function Patterns ===")

# Return new objects instead of modifying
def add_element_pure(lst, element):
    """Pure: returns new list."""
    return lst + [element]

def update_dict_pure(d, key, value):
    """Pure: returns new dict."""
    return {**d, key: value}

def increment_pure(n):
    """Pure: returns new value."""
    return n + 1

# Test
original = [1, 2, 3]
new = add_element_pure(original, 4)
print(f"Original: {original}")
print(f"New: {new}")

d = {'a': 1}
new_d = update_dict_pure(d, 'b', 2)
print(f"Original dict: {d}")
print(f"New dict: {new_d}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Pure Functions")
print("=" * 50)
print("""
PURE FUNCTIONS:
- Same input → Same output
- No side effects (no I/O, no modifying state)
- Don't modify their arguments

BENEFITS:
- Easy to test
- Predictable
- Cacheable
- Parallelizable
- Composable
""")
