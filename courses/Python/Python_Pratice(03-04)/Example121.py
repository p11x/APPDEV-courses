# Example121.py
# Topic: List Comprehensions Deep Dive

# Advanced list comprehension patterns.


# ============================================================
# Example 1: Basic Comprehensions
# ============================================================
print("=== Basic ===")

# List
squares = [x**2 for x in range(5)]
print(f"Squares: {squares}")

# Set
unique = {x % 3 for x in range(10)}
print(f"Unique mods: {unique}")

# Dict
cube = {x: x**3 for x in range(3)}
print(f"Cube: {cube}")


# ============================================================
# Example 2: With Condition
# ============================================================
print("\n=== With Condition ===")

# Filter
evens = [x for x in range(10) if x % 2 == 0]
print(f"Evens: {evens}")

# Multiple conditions
fizzbuzz = ["Fizz" if x % 3 == 0 else "Buzz" if x % 5 == 0 else str(x) for x in range(1, 11)]
print(f"FizzBuzz: {fizzbuzz}")


# ============================================================
# Example 3: Nested Comprehensions
# ============================================================
print("\n=== Nested ===")

# Flatten
matrix = [[1, 2], [3, 4], [5, 6]]
flat = [num for row in matrix for num in row]
print(f"Flat: {flat}")

# 2D
grid = [[i*j for j in range(3)] for i in range(3)]
print(f"Grid: {grid}")


# ============================================================
# Example 4: With Multiple Sources
# ====================================
print("\n=== Multiple Sources ===")

# Two lists
pairs = [(x, y) for x in range(3) for y in range(3)]
print(f"Pairs: {pairs}")

# With condition
pairs = [(x, y) for x in range(3) for y in range(3) if x != y]
print(f"Pairs (x!=y): {pairs}")


# ============================================================
# Example 5: Set and Dict Comprehensions
# ============================================================
print("\n=== Set/Dict ===")

# Set from string
unique_chars = {c.lower() for c in "Hello World"}
print(f"Unique chars: {unique_chars}")

# Dict from two lists
keys = ['a', 'b', 'c']
values = [1, 2, 3]
d = {k: v for k, v in zip(keys, values)}
print(f"Dict: {d}")
