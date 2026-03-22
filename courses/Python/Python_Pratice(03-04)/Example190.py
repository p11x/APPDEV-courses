# Example190.py
# Topic: Dict Comprehensions - Basic Creation

# This file demonstrates dictionary comprehension basics for creating
# dictionaries from iterables with key-value transformations.


# ============================================================
# Example 1: Basic Dict from Range
# ============================================================
print("=== Basic Dict from Range ===")

squares = {x: x**2 for x in range(5)}
print(f"Squares: {squares}")    # {0: 0, 1: 1, 2: 4, 3: 9, 4: 16}


# ============================================================
# Example 2: From Two Lists
# ============================================================
print("\n=== From Two Lists ===")

keys = ["a", "b", "c"]
values = [1, 2, 3]
d = {k: v for k, v in zip(keys, values)}
print(f"Dict: {d}")    # {'a': 1, 'b': 2, 'c': 3}


# ============================================================
# Example 3: From List of Tuples
# ============================================================
print("\n=== From List of Tuples ===")

pairs = [("a", 1), ("b", 2), ("c", 3)]
d = dict(pairs)
print(f"Dict: {d}")    # {'a': 1, 'b': 2, 'c': 3}


# ============================================================
# Example 4: String to Dict
# ============================================================
print("\n=== String to Dict ===")

word = "abc"
freq = {char: ord(char) for char in word}
print(f"Char codes: {freq}")    # {'a': 97, 'b': 98, 'c': 99}


# ============================================================
# Example 5: From Existing Dict
# ============================================================
print("\n=== From Existing Dict ===")

original = {"a": 1, "b": 2, "c": 3}
doubled = {k: v * 2 for k, v in original.items()}
print(f"Doubled: {doubled}")    # {'a': 2, 'b': 4, 'c': 6}


# ============================================================
# Example 6: Keys from Range
# ============================================================
print("\n=== Keys from Range ===")

d = {f"key_{i}": i * 10 for i in range(3)}
print(f"Named keys: {d}")    # {'key_0': 0, 'key_1': 10, 'key_2': 20}


# ============================================================
# Example 7: With enumerate
# ============================================================
print("\n=== With enumerate ===")

words = ["apple", "banana", "cherry"]
d = {word: idx for idx, word in enumerate(words)}
print(f"Word index: {d}")    # {'apple': 0, 'banana': 1, 'cherry': 2}


# ============================================================
# Example 8: Dict with Tuples as Values
# ============================================================
print("\n=== Tuple Values ===")

numbers = [1, 2, 3]
d = {x: (x, x**2, x**3) for x in numbers}
print(f"Tuple values: {d}")    # {1: (1, 1, 1), 2: (2, 4, 8), 3: (3, 9, 27)}


# ============================================================
# Example 9: Nested Dict
# ============================================================
print("\n=== Nested Dict ===")

d = {i: {j: i*j for j in range(3)} for i in range(2)}
print(f"Nested: {d}")    # {0: {0: 0, 1: 0, 2: 0}, 1: {0: 0, 1: 1, 2: 2}}


# ============================================================
# Example 10: From Set
# ============================================================
print("\n=== From Set ===")

chars = {"a", "b", "c"}
d = {c: c.upper() for c in chars}
print(f"From set: {d}")    # {'a': 'A', 'b': 'B', 'c': 'C'}


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
DICT COMPREHENSION:
{k: v for k, v in iterable}
{k: v for k, v in zip(keys, values)}
{k: v**2 for k, v in dict.items()}

KEY POINTS:
- Use curly braces {}
- Key must be hashable
- Can transform both keys and values
""")
