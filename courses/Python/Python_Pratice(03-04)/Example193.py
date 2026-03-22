# Example193.py
# Topic: Set Comprehensions - Basic & Operations

# This file demonstrates set comprehension syntax and set operations.


# ============================================================
# Example 1: Basic Set from Range
# ============================================================
print("=== Basic Set from Range ===")

squares = {x**2 for x in range(5)}
print(f"Squares: {squares}")    # {0, 1, 4, 9, 16}


# ============================================================
# Example 2: From String
# ============================================================
print("\n=== From String ===")

word = "hello"
unique = {char for char in word}
print(f"Unique chars: {unique}")    # {'h', 'e', 'l', 'o'}


# ============================================================
# Example 3: From List with Duplicates
# ============================================================
print("\n=== From List ===")

numbers = [1, 2, 2, 3, 3, 3, 4]
unique = {x for x in numbers}
print(f"Unique: {unique}")    # {1, 2, 3, 4}


# ============================================================
# Example 4: With Condition
# ============================================================
print("\n=== With Condition ===")

numbers = range(10)
evens = {x for x in numbers if x % 2 == 0}
print(f"Evens: {evens}")    # {0, 2, 4, 6, 8}


# ============================================================
# Example 5: Transform
# ============================================================
print("\n=== Transform ===")

words = ["hello", "world", "python"]
lengths = {len(w) for w in words}
print(f"Lengths: {lengths}")    # {5, 6}


# ============================================================
# Example 6: Set Union
# ============================================================
print("\n=== Union ===")

a = {1, 2, 3}
b = {3, 4, 5}
union = a | b
print(f"Union: {union}")    # {1, 2, 3, 4, 5}


# ============================================================
# Example 7: Set Intersection
# ============================================================
print("\n=== Intersection ===")

a = {1, 2, 3}
b = {2, 3, 4}
intersection = a & b
print(f"Intersection: {intersection}")    # {2, 3}


# ============================================================
# Example 8: Set Difference
# ============================================================
print("\n=== Difference ===")

a = {1, 2, 3, 4}
b = {3, 4, 5}
diff = a - b
print(f"Difference: {diff}")    # {1, 2}


# ============================================================
# Example 9: Symmetric Difference
# ============================================================
print("\n=== Symmetric Difference ===")

a = {1, 2, 3}
b = {2, 3, 4}
sym_diff = a ^ b
print(f"Symmetric diff: {sym_diff}")    # {1, 4}


# ============================================================
# Example 10: Set Comprehension with Union
# ============================================================
print("\n=== Comprehension with Set Op ===")

a = {1, 2, 3}
b = {3, 4, 5}
combined = {x for x in a | b if x > 2}
print(f"Combined: {combined}")    # {3, 4, 5}


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
SET COMPREHENSION:
{x for x in iterable}
{x for x in iterable if condition}

SET OPERATIONS:
- Union: a | b
- Intersection: a & b
- Difference: a - b
- Symmetric: a ^ b
""")
