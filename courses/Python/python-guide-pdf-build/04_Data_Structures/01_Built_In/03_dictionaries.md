# Dictionaries

## What You'll Learn

- Creating and accessing dictionaries
- Dictionary methods
- Dictionary comprehensions
- Merging dictionaries (Python 3.9+)
- defaultdict, OrderedDict, Counter

## Prerequisites

- Read [02_tuples_and_sets.md](./02_tuples_and_sets.md) first

## Creating Dictionaries

```python
# Basic dictionary
user: dict[str, int] = {"Alice": 25, "Bob": 30}

# Using dict()
data: dict[str, int] = dict(Alice=25, Bob=30)

# Accessing values
age: int = user["Alice"]  # 25
age = user.get("Alice", 0)  # 25 (with default)

# Safe access
if "Alice" in user:
    age = user["Alice"]
```

## Dictionary Methods

```python
d: dict[str, int] = {"a": 1, "b": 2}

# Get keys, values, items
keys: list = list(d.keys())   # ['a', 'b']
values: list = list(d.values())  # [1, 2]
items: list = list(d.items())  # [('a', 1), ('b', 2)]

# Update
d.update({"c": 3})  # {'a': 1, 'b': 2, 'c': 3}
d.pop("a")          # Removes 'a': returns 1

# Default value
d.setdefault("d", 4)  # Sets if not exists
```

## Dictionary Comprehensions

```python
# Create squares
squares: dict[int, int] = {x: x**2 for x in range(5)}
# {0: 0, 1: 1, 2: 4, 3: 9, 4: 16}

# From two lists
keys: list[str] = ["a", "b", "c"]
values: list[int] = [1, 2, 3]
d: dict[str, int] = {k: v for k, v in zip(keys, values)}
# {'a': 1, 'b': 2, 'c': 3}
```

## Merging Dictionaries (Python 3.9+)

```python
# Using | operator
d1: dict[str, int] = {"a": 1, "b": 2}
d2: dict[str, int] = {"b": 3, "c": 4}
merged: dict[str, int] = d1 | d2
# {'a': 1, 'b': 3, 'c': 4} (b overwritten)

# Using |=
d1 |= d2
```

## collections Module

```python
from collections import defaultdict, OrderedDict, Counter

# defaultdict - provides default values
dd: defaultdict[str, int] = defaultdict(int)
dd["missing"]  # Returns 0, doesn't error

# OrderedDict - remembers insertion order (Python 3.7+ dicts do too)
od: OrderedDict[str, int] = OrderedDict()
od["a"] = 1
od["b"] = 2

# Counter - counts occurrences
c: Counter[str] = Counter(["a", "b", "a", "c", "a"])
# Counter({'a': 3, 'b': 1, 'c': 1})
```

## Summary

- Dictionaries store key-value pairs
- Use `.get()` for safe access
- Dictionary comprehensions for creation
- `|` operator for merging (Python 3.9+)
- `defaultdict`, `Counter` from collections

## Next Steps

Continue to **[04_Data_Structures/02_Advanced/01_dataclasses.md](../02_Advanced/01_dataclasses.md)**
