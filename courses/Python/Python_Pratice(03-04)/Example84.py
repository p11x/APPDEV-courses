# Example84.py
# Topic: Testing Pure Functions

# This file demonstrates how easy it is to test pure functions.


# ============================================================
# Example 1: Simple Unit Tests
# ============================================================
print("=== Testing Pure Functions ===")

# Pure functions are easy to test
def add(a, b):
    return a + b

def multiply(a, b):
    return a * b

def divide(a, b):
    if b == 0:
        raise ValueError("Division by zero")
    return a / b

# Simple assertions
assert add(2, 3) == 5
assert add(-1, 1) == 0
assert add(0, 0) == 0

assert multiply(3, 4) == 12
assert multiply(0, 100) == 0
assert multiply(-2, 3) == -6

assert divide(10, 2) == 5
assert divide(7, 2) == 3.5

print("All tests passed!")


# ============================================================
# Example 2: Property-Based Testing
# ============================================================
print("\n=== Property-Based Testing ===")

# Test properties that should always hold
def add(a, b):
    return a + b

def subtract(a, b):
    return a - b

# Property: a + b - b == a
for a in range(-10, 10):
    for b in range(-10, 10):
        if b != 0:  # Avoid issues
            assert add(subtract(a, b), b) == a

# Property: a * b / b == a (for non-zero)
for a in range(-5, 5):
    for b in range(-5, 5):
        if b != 0:
            assert (a * b) / b == a

print("Property-based tests passed!")


# ============================================================
# Example 3: Testing Data Transformations
# ============================================================
print("\n=== Testing Data Transformations ===")

def transform_user(data):
    """Pure transformation."""
    return {
        'name': data.get('name', '').strip().title(),
        'email': data.get('email', '').strip().lower(),
        'age': data.get('age', 0)
    }

# Test various cases
test_cases = [
    ({'name': 'john', 'email': 'JOHN@EXAMPLE.COM', 'age': 25},
     {'name': 'John', 'email': 'john@example.com', 'age': 25}),
    ({'name': '  jane  ', 'email': 'JANE@TEST.ORG', 'age': 30},
     {'name': 'Jane', 'email': 'jane@test.org', 'age': 30}),
    ({}, {'name': '', 'email': '', 'age': 0}),
]

for input_data, expected in test_cases:
    result = transform_user(input_data)
    assert result == expected, f"Failed: {input_data}"

print("Data transformation tests passed!")


# ============================================================
# Example 4: Testing with Edge Cases
# ============================================================
print("\n=== Testing Edge Cases ===")

def calculate_discount(price, discount_percent):
    """Pure: apply discount."""
    if discount_percent < 0 or discount_percent > 100:
        raise ValueError("Invalid discount")
    return price * (1 - discount_percent / 100)

# Edge cases
assert calculate_discount(100, 0) == 100
assert calculate_discount(100, 100) == 0
assert calculate_discount(100, 50) == 50

# Error cases
try:
    calculate_discount(100, -10)
    assert False, "Should raise error"
except ValueError:
    pass

try:
    calculate_discount(100, 150)
    assert False, "Should raise error"
except ValueError:
    pass

print("Edge case tests passed!")


# ============================================================
# Example 5: Memoization Testing
# ============================================================
print("\n=== Testing with Memoization ===")

from functools import lru_cache

@lru_cache(maxsize=10)
def fibonacci(n):
    """Pure function - can be cached."""
    if n < 2:
        return n
    return fibonacci(n-1) + fibonacci(n-2)

# First call - computes
result1 = fibonacci(30)

# Second call - from cache
result2 = fibonacci(30)

# Cache info
info = fibonacci.cache_info()
print(f"Result: {result1}")
print(f"Cache hits: {info.hits}")
print(f"Cache misses: {info.misses}")
print(f"Results equal: {result1 == result2}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("TESTING PURE FUNCTIONS")
print("=" * 50)
print("""
Benefits for testing:

1. Simple assertions:
   - No setup needed
   - No mocking needed

2. Property-based testing:
   - Test many random inputs
   - Verify mathematical properties

3. Edge cases:
   - Easy to test boundary conditions
   - No hidden state

4. Memoization:
   - Pure functions can be cached
   - Cache can be inspected

Remember:
- Pure = Predictable = Testable
""")
