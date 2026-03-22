# Example187.py
# Topic: List Comprehensions - Basic Creation

# This file demonstrates basic list comprehension syntax and usage
# for creating lists from iterables in a concise, readable way.


# ============================================================
# Example 1: Basic Number Squares
# ============================================================
print("=== Basic Number Squares ===")

squares = [x ** 2 for x in range(5)]
print(f"Squares: {squares}")    # [0, 1, 4, 9, 16]


# ============================================================
# Example 2: Basic Number Cubes
# ============================================================
print("\n=== Basic Number Cubes ===")

cubes = [x ** 3 for x in range(6)]
print(f"Cubes: {cubes}")    # [0, 1, 8, 27, 64, 125]


# ============================================================
# Example 3: From String to List
# ============================================================
print("\n=== String to List ===")

word = "python"
letters = [char for char in word]
print(f"Letters: {letters}")    # ['p', 'y', 't', 'h', 'o', 'n']


# ============================================================
# Example 4: Using Existing List
# ============================================================
print("\n=== From Existing List ===")

numbers = [1, 2, 3, 4, 5]
doubled = [x * 2 for x in numbers]
print(f"Doubled: {doubled}")    # [2, 4, 6, 8, 10]


# ============================================================
# Example 5: With Range
# ============================================================
print("\n=== With Range ===")

evens = [x for x in range(10) if x % 2 == 0]
print(f"Evens: {evens}")    # [0, 2, 4, 6, 8]


# ============================================================
# Example 6: Float Operations
# ============================================================
print("\n=== Float Operations ===")

temps = [0, 20, 37, 100]
celsius = [(t * 9/5) + 32 for t in temps]
print(f"Fahrenheit: {celsius}")    # [32.0, 68.0, 98.6, 212.0]


# ============================================================
# Example 7: String Transforms
# ============================================================
print("\n=== String Transforms ===")

words = ["hello", "world", "python"]
upper = [word.upper() for word in words]
print(f"Upper: {upper}")    # ['HELLO', 'WORLD', 'PYTHON']


# ============================================================
# Example 8: Tuple to List
# ============================================================
print("\n=== Tuple to List ===")

coords = (10, 20, 30, 40)
pairs = [(x, x*2) for x in coords]
print(f"Pairs: {pairs}")    # [(10, 20), (20, 40), (30, 60), (40, 80)]


# ============================================================
# Example 9: Boolean List
# ============================================================
print("\n=== Boolean List ===")

numbers = [1, 2, 3, 4, 5]
is_positive = [x > 0 for x in numbers]
print(f"Is positive: {is_positive}")    # [True, True, True, True, True]


# ============================================================
# Example 10: Nested List Flatten
# ============================================================
print("\n=== Nested List Flatten ===")

matrix = [[1, 2], [3, 4], [5, 6]]
flat = [num for row in matrix for num in row]
print(f"Flat: {flat}")    # [1, 2, 3, 4, 5, 6]


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
LIST COMPREHENSION BASIC:
[x for x in iterable]
[x**2 for x in range(5)]
[transform for item in list]

KEY POINTS:
- More readable than for loops
- Faster than append in loops
- Can transform, filter, flatten
""")
