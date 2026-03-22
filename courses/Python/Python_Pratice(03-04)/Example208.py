# Example208.py
# Topic: Advanced List Comprehensions - Walrus & Conditional Expressions

# This file demonstrates the walrus operator and complex conditional expressions.


# ============================================================
# Example 1: Walrus Operator (Python 3.8+)
# ============================================================
print("=== Walrus Operator ===")

numbers = [1, 5, 3, 8, 2]
squared = [y := x**2 for x in numbers if x > 2]
print(f"Squared: {squared}")


# ============================================================
# Example 2: Nested Conditional
# ============================================================
print("\n=== Nested Conditional ===")

numbers = [1, 2, 3, 4, 5, 6]
result = ["small" if x < 3 else "medium" if x < 5 else "large" for x in numbers]
print(f"Result: {result}")


# ============================================================
# Example 3: Multiple Transforms
# ============================================================
print("\n=== Multiple Transforms ===")

data = [1, 2, 3, 4, 5]
transformed = [x * 2 + 1 for x in data if x % 2 == 0]
print(f"Transformed: {transformed}")


# ============================================================
# Example 4: Comprehension with zip
# ============================================================
print("\n=== With Zip ===")

names = ["Alice", "Bob", "Carol"]
ages = [25, 30, 35]
combined = [f"{n} is {a}" for n, a in zip(names, ages)]
print(f"Combined: {combined}")


# ============================================================
# Example 5: Cartesian Product
# ============================================================
print("\n=== Cartesian Product ===")

A = [1, 2]
B = [3, 4]
product = [(a, b) for a in A for b in B]
print(f"Product: {product}")


# ============================================================
# Example 6: If-Else with Transform
# ============================================================
print("\n=== If-Else Transform ===")

numbers = [1, 2, 3, 4, 5]
result = [x**2 if x % 2 == 0 else x*2 for x in numbers]
print(f"Result: {result}")


# ============================================================
# Example 7: Comprehension with enumerate
# ============================================================
print("\n=== With Enumerate ===")

words = ["apple", "banana", "cherry"]
indexed = [(i, w) for i, w in enumerate(words)]
print(f"Indexed: {indexed}")


# ============================================================
# Example 8: Comprehension with filter and map
# ============================================================
print("\n=== Filter and Map ===")

numbers = range(10)
result = list(map(lambda x: x*2, filter(lambda x: x > 3, numbers)))
print(f"Filtered mapped: {result}")


# ============================================================
# Example 9: Dict from Two Lists
# ============================================================
print("\n=== Dict from Lists ===")

keys = ["a", "b", "c"]
vals = [1, 2, 3]
d = {k: v for k, v in zip(keys, vals)}
print(f"Dict: {d}")


# ============================================================
# Example 10: Generator with Condition
# ============================================================
print("\n=== Conditional Generator ===")

gen = (x for x in range(10) if x % 2 == 0)
print(f"Gen: {list(gen)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
COMPLEX COMPREHENSIONS:
- Walrus: (y := expr for x in list)
- Multiple transforms: x*2+1
- Cartesian: [x for a in A for b in B]
- Conditional: expr if cond else expr
""")
