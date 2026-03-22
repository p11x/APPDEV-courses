# Example191.py
# Topic: Dict Comprehensions - Filtering & Transforming

# This file demonstrates dict comprehensions with conditional filtering
# and complex transformations.


# ============================================================
# Example 1: Filter by Value
# ============================================================
print("=== Filter by Value ===")

d = {"a": 1, "b": 5, "c": 3, "d": 7}
filtered = {k: v for k, v in d.items() if v > 3}
print(f"Filtered: {filtered}")    # {'b': 5, 'd': 7}


# ============================================================
# Example 2: Filter by Key
# ============================================================
print("\n=== Filter by Key ===")

d = {"apple": 5, "banana": 2, "cherry": 7, "date": 1}
filtered = {k: v for k in d.keys() if len(k) > 5}
print(f"Long names: {filtered}")    # {'banana': 2, 'cherry': 7}


# ============================================================
# Example 3: Transform Values
# ============================================================
print("\n=== Transform Values ===")

d = {"a": 1, "b": 2, "c": 3}
transformed = {k: v * 10 for k, v in d.items()}
print(f"Transformed: {transformed}")    # {'a': 10, 'b': 20, 'c': 30}


# ============================================================
# Example 4: Transform Keys
# ============================================================
print("\n=== Transform Keys ===")

d = {"a": 1, "b": 2, "c": 3}
transformed = {k.upper(): v for k, v in d.items()}
print(f"Upper keys: {transformed}")    # {'A': 1, 'B': 2, 'C': 3}


# ============================================================
# Example 5: Both Key and Value Transform
# ============================================================
print("\n=== Both Key and Value ===")

d = {"a": 1, "b": 2, "c": 3}
transformed = {k.upper(): v * v for k, v in d.items()}
print(f"Squared: {transformed}")    # {'A': 1, 'B': 4, 'C': 9}


# ============================================================
# Example 6: Conditional Value
# ============================================================
print("\n=== Conditional Value ===")

d = {"a": 1, "b": 2, "c": 3, "d": 4}
result = {k: ("even" if v % 2 == 0 else "odd") for k, v in d.items()}
print(f"Parity: {result}")    # {'a': 'odd', 'b': 'even', ...}


# ============================================================
# Example 7: Filter and Transform
# ============================================================
print("\n=== Filter and Transform ===")

d = {"apple": 5, "banana": 2, "cherry": 7}
result = {k.upper(): v * 2 for k, v in d.items() if v > 3}
print(f"Filtered doubled: {result}")    # {'APPLE': 10, 'CHERRY': 14}


# ============================================================
# Example 8: Complex Condition
# ============================================================
print("\n=== Complex Condition ===")

d = {"a": 10, "b": 25, "c": 15, "d": 30}
result = {k: v for k, v in d.items() if v % 5 == 0 and v > 10}
print(f"Divisible by 5 > 10: {result}")    # {'b': 25, 'd': 30}


# ============================================================
# Example 9: Default Values
# ============================================================
print("\n=== Default Values ===")

keys = ["a", "b", "c"]
d = {k: d.get(k, 0) + 1 for k in keys}
print(f"Default: {d}")    # {'a': 1, 'b': 1, 'c': 1}


# ============================================================
# Example 10: Dict from List of Dicts
# ============================================================
print("\n=== List of Dicts to Dict ===")

users = [{"id": 1, "name": "Alice"}, {"id": 2, "name": "Bob"}]
d = {u["id"]: u["name"] for u in users}
print(f"Users dict: {d}")    # {1: 'Alice', 2: 'Bob'}


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
FILTER & TRANSFORM:
{k: v for k, v in d.items() if cond}
{k: v*2 for k, v in d.items() if v > 0}
{k.upper(): v for k, v in d.items()}

KEYS:
- Filter with if after for
- Transform both key and value
- Conditional value with if-else
""")
