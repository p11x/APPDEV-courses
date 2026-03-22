# Example105.py
# Topic: Dictionaries - Basics

# This file demonstrates dictionaries in Python.


# ============================================================
# Example 1: Creating Dictionaries
# ============================================================
print("=== Creating Dictionaries ===")

# Empty dict
empty = {}
print(f"Empty: {empty}")

# With items
user = {"name": "Alice", "age": 30}
print(f"User: {user}")

# Using dict()
data = dict(name="Bob", age=25)
print(f"Using dict(): {data}")

# From sequence of pairs
pairs = [("a", 1), ("b", 2)]
from_pairs = dict(pairs)
print(f"From pairs: {from_pairs}")


# ============================================================
# Example 2: Accessing Values
# ============================================================
print("\n=== Accessing Values ===")

user = {"name": "Alice", "age": 30, "city": "NYC"}

# Direct access (raises KeyError if missing)
print(f"name: {user['name']}")

# get() method (returns None if missing)
print(f"get name: {user.get('name')}")
print(f"get missing: {user.get('missing')}")
print(f"get with default: {user.get('missing', 'N/A')}")

# Check existence
print(f"'name' in user: {'name' in user}")


# ============================================================
# Example 3: Modifying Dictionaries
# ============================================================
print("\n=== Modifying ===")

d = {"a": 1, "b": 2}

# Add/Update
d["c"] = 3
d.update({"d": 4, "e": 5})
print(f"After add: {d}")

# Delete
del d["a"]
removed = d.pop("b")
print(f"After delete: {d}, removed: {removed}")

# Clear
d.clear()
print(f"After clear: {d}")


# ============================================================
# Example 4: Dictionary Views
# ============================================================
print("\n=== Dictionary Views ===")

d = {"a": 1, "b": 2, "c": 3}

# Keys, values, items
print(f"Keys: {list(d.keys())}")
print(f"Values: {list(d.values())}")
print(f"Items: {list(d.items())}")

# Iterate
for key in d:
    print(f"Key: {key}")
    
for key, value in d.items():
    print(f"{key}: {value}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Dictionaries")
print("=" * 50)
print("""
DICTIONARIES:
- Key-value pairs
- Created with {} or dict()
- Keys must be hashable (immutable)

ACCESS:
- dict[key] - raises KeyError if missing
- dict.get(key) - returns None
- dict.get(key, default) - returns default

MODIFY:
- dict[key] = value
- dict.update({key: value})
- del dict[key]
- dict.pop(key)

VIEWS:
- dict.keys()
- dict.values()
- dict.items()
""")
