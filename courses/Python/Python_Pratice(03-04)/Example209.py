# Example209.py
# Topic: Advanced Dict Comprehensions - Nested & Complex

# This file demonstrates advanced dict comprehension patterns with nested structures.


# ============================================================
# Example 1: Nested Dict from Lists
# ============================================================
print("=== Nested Dict ===")

names = ["Alice", "Bob", "Carol"]
ages = [25, 30, 35]
nested = {name: {"age": age, "index": i} for i, (name, age) in enumerate(zip(names, ages))}
print(f"Nested: {nested}")


# ============================================================
# Example 2: Dict with List Values
# ============================================================
print("\n=== Dict of Lists ===")

strings = ["apple", "banana", "apricot", "blueberry"]
grouped = {s[0]: [w for w in strings if w[0] == s[0]] for s in set(s[0] for s in strings)}
print(f"Grouped: {grouped}")


# ============================================================
# Example 3: Dict Comprehension with defaultdict
# ============================================================
print("\n=== Defaultdict Pattern ===")

from collections import defaultdict
data = [("a", 1), ("b", 2), ("a", 3)]
d = defaultdict(list)
for k, v in data:
    d[k].append(v)
print(f"Grouped: {dict(d)}")


# ============================================================
# Example 4: Dict with Conditions
# ============================================================
print("\n=== Dict with Condition ===")

d = {x: x**2 for x in range(10) if x % 2 == 0}
print(f"Even squares: {d}")


# ============================================================
# Example 5: Swap Keys and Values
# ============================================================
print("\n=== Swap Keys Values ===")

d = {"a": 1, "b": 2, "c": 3}
swapped = {v: k for k, v in d.items()}
print(f"Swapped: {swapped}")


# ============================================================
# Example 6: Dict of Counters
# ============================================================
print("\n=== Dict of Counters ===")

from collections import Counter
data = ["a", "b", "a", "c", "b", "a"]
d = {k: v for k, v in Counter(data).items() if v > 1}
print(f"Counts > 1: {d}")


# ============================================================
# Example 7: Nested Dict Comprehension
# ============================================================
print("\n=== Nested Dict Comp ===")

d = {i: {j: i*j for j in range(3)} for i in range(2)}
print(f"Nested: {d}")


# ============================================================
# Example 8: Dict from Tuples with Transform
# ============================================================
print("\n=== Transform Tuples ===")

data = [("a", 1), ("b", 2), ("c", 3)]
d = {k.upper(): v * 10 for k, v in data}
print(f"Transformed: {d}")


# ============================================================
# Example 9: Merge Dicts with Comprehension
# ============================================================
print("\n=== Merge Dicts ===")

d1 = {"a": 1, "b": 2}
d2 = {"b": 3, "c": 4}
merged = {k: d2.get(k, d1.get(k)) for k in set(d1) | set(d2)}
print(f"Merged: {merged}")


# ============================================================
# Example 10: Dict with Complex Values
# ============================================================
print("\n=== Complex Values ===")

numbers = range(1, 6)
d = {x: {"square": x**2, "cube": x**3, "factors": [i for i in range(1, x+1) if x % i == 0]} for x in numbers}
print(f"Complex: {d}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
ADVANCED DICT:
- Nested dict comprehensions
- Swap keys and values
- Merge with conditions
- Complex value structures
""")
