# Dictionaries In Depth

## What You'll Learn

- CRUD operations on dictionaries
- Dictionary iteration patterns
- Dictionary comprehensions
- Using merge operators (| and |=)

## Prerequisites

- Read [02_tuples_and_namedtuples.md](./02_tuples_and_namedtuples.md) first

## Dictionary CRUD Operations

Create, Read, Update, Delete operations on dictionaries.

```python
# dict_crud.py

# Create
person: dict[str, str] = {"name": "Alice", "age": "30"}

# Read
name: str = person["name"]           # KeyError if missing!
name_safe: str = person.get("name")    # None if missing
name_default: str = person.get("city", "Unknown")

# Update
person["age"] = 31                     # Update existing
person["city"] = "NYC"                 # Add new

# Delete
del person["age"]                      # KeyError if missing
removed: str = person.pop("city")       # Returns value, None if missing
```

## Dictionary Iteration

Multiple ways to iterate over dictionaries.

```python
# dict_iteration.py

data: dict[str, int] = {"a": 1, "b": 2, "c": 3}

# Iterate over keys
for key in data:
    print(key)

# Iterate over values
for value in data.values():
    print(value)

# Iterate over key-value pairs
for key, value in data.items():
    print(f"{key}: {value}")

# Dictionary comprehension
squares: dict[int, int] = {k: k**2 for k in range(5)}
print(squares)
```

## Dictionary Merge Operators (Python 3.9+)

Python 3.9 introduced merge (|) and update (|=) operators.

```python
# dict_merge.py

# Merge dictionaries
dict1: dict[str, int] = {"a": 1, "b": 2}
dict2: dict[str, int] = {"b": 3, "c": 4}

# Merge - dict2 overwrites dict1 for conflicts
merged: dict[str, int] = dict1 | dict2
print(f"Merged: {merged}")

# Update in-place
dict1 |= dict2
print(f"Updated: {dict1}")
```

## Annotated Full Example

```python
# dicts_in_depth_demo.py
"""Complete demonstration of dictionary operations."""

from typing import Dict


def word_frequency(text: str) -> Dict[str, int]:
    """Count word frequencies in text."""
    words = text.lower().split()
    freq: Dict[str, int] = {}
    
    for word in words:
        freq[word] = freq.get(word, 0) + 1
    
    return freq


def main() -> None:
    # Basic operations
    inventory: Dict[str, int] = {"apples": 5, "bananas": 3}
    inventory["oranges"] = 10
    print(f"Inventory: {inventory}")
    
    # Merge operator (Python 3.9+)
    additional = {"bananas": 2, "grapes": 15}
    inventory |= additional
    print(f"After merge: {inventory}")
    
    # Word frequency
    text = "the quick brown fox jumps over the lazy dog the fox"
    freq = word_frequency(text)
    print(f"Word frequencies: {freq}")
    
    # Sorted by frequency
    sorted_words = sorted(freq.items(), key=lambda x: x[1], reverse=True)
    print(f"Most common: {sorted_words[:3]}")


if __name__ == "__main__":
    main()
```

## Summary

- CRUD operations on dictionaries
- Dictionary iteration patterns
- Dictionary comprehensions

## Next Steps

Continue to **[04_sets_and_frozensets.md](./04_sets_and_frozensets.md)**
