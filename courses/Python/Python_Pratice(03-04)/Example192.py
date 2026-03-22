# Example192.py
# Topic: Dict Comprehensions - Inversion & Merging

# This file demonstrates dict inversion, merging, and advanced dict patterns.


# ============================================================
# Example 1: Invert Dict
# ============================================================
print("=== Invert Dict ===")

d = {"a": 1, "b": 2, "c": 3}
inverted = {v: k for k, v in d.items()}
print(f"Inverted: {inverted}")    # {1: 'a', 2: 'b', 3: 'c'}


# ============================================================
# Example 2: Invert with Collision Handling
# ============================================================
print("\n=== Invert with List ===")

d = {"a": 1, "b": 2, "c": 1}
inverted = {}
for k, v in d.items():
    inverted.setdefault(v, []).append(k)
print(f"Inverted: {inverted}")    # {1: ['a', 'c'], 2: ['b']}


# ============================================================
# Example 3: Merge Two Dicts
# ============================================================
print("\n=== Merge Two Dicts ===")

d1 = {"a": 1, "b": 2}
d2 = {"b": 3, "c": 4}
merged = {**d1, **d2}
print(f"Merged: {merged}")    # {'a': 1, 'b': 3, 'c': 4}


# ============================================================
# Example 4: Dict Merge (Python 3.9+)
# ============================================================
print("\n=== Dict Merge | ===")

d1 = {"a": 1, "b": 2}
d2 = {"b": 3, "c": 4}
merged = d1 | d2
print(f"Merged: {merged}")    # {'a': 1, 'b': 3, 'c': 4}


# ============================================================
# Example 5: Update in Place |=
# ============================================================
print("\n=== Update In Place ===")

d1 = {"a": 1, "b": 2}
d1 |= {"b": 3, "c": 4}
print(f"Updated: {d1}")    # {'a': 1, 'b': 3, 'c': 4}


# ============================================================
# Example 6: Merge with Conflict Resolution
# ============================================================
print("\n=== Merge with Resolution ===")

d1 = {"a": 1, "b": 2}
d2 = {"b": 3, "c": 4}
merged = {**d1, **d2}
resolved = {k: d2.get(k, d1.get(k)) for k in set(d1) | set(d2)}
print(f"Resolved: {resolved}")    # {'a': 1, 'b': 3, 'c': 4}


# ============================================================
# Example 7: Combine Counts
# ============================================================
print("\n=== Combine Counts ===")

from collections import Counter

c1 = Counter({"a": 1, "b": 2})
c2 = Counter({"b": 1, "c": 3})
combined = c1 + c2
print(f"Combined: {combined}")    # Counter({'b': 3, 'c': 3, 'a': 1})


# ============================================================
# Example 8: Dict of Counters
# ============================================================
print("\n=== Dict of Counters ===")

from collections import defaultdict

data = [("apple", "red"), ("banana", "yellow"), ("cherry", "red")]
grouped = defaultdict(list)
for fruit, color in data:
    grouped[color].append(fruit)
print(f"Grouped: {dict(grouped)}")    # {'red': ['apple', 'cherry'], 'yellow': ['banana']}


# ============================================================
# Example 9: Dict Diff
# ============================================================
print("\n=== Dict Difference ===")

d1 = {"a": 1, "b": 2, "c": 3}
d2 = {"b": 2, "c": 4, "d": 5}
diff = {k: (d1.get(k), d2.get(k)) for k in set(d1) | set(d2)}
print(f"Diff: {diff}")    # {'a': (1, None), 'b': (2, 2), 'c': (3, 4), 'd': (None, 5)}


# ============================================================
# Example 10: Nested Dict Update
# ============================================================
print("\n=== Nested Dict Update ===")

base = {"a": {"x": 1}}
update = {"a": {"y": 2}, "b": 3}
result = {**base, **{"a": {**base["a"], **update["a"]}, ** {k: v for k, v in update.items() if k != "a"}}}
print(f"Result: {result}")    # {'a': {'x': 1, 'y': 2}, 'b': 3}


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
INVERT & MERGE:
- Invert: {v: k for k, v in d.items()}
- Merge: {**d1, **d2} or d1 | d2
- Update: d1 |= d2

USE CASES:
- Value-to-key mapping
- Combining counters
- Grouping data
""")
