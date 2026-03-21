# Python Interview Patterns

## What You'll Learn

- Pythonic idioms for interviews
- Common patterns and tricks
- Efficient data structures
- Type hints in interviews

## Prerequisites

- Read [08_design_a_notification_system.md](../../20_System_Design_and_Architecture/03_Architecture_Projects/08_system_design_interview_guide.md) first

## Pythonic Idioms

```python
# pythonic_idioms.py

# Swap values
a, b = 1, 2
a, b = b, a

# List comprehension
squares = [x**2 for x in range(10)]

# Dictionary comprehension
word_lengths = {word: len(word) for word in ["hello", "world"]}

# Using enumerate
for i, val in enumerate(items):
    print(f"{i}: {val}")

# Using zip
for name, age in zip(names, ages):
    print(f"{name}: {age}")
```

## Defaultdict Tricks

```python
# defaultdict_tricks.py

from collections import defaultdict, Counter

# Counting
counter = Counter(["a", "b", "a", "c", "a"])
print(counter)  # Counter({'a': 3, 'b': 1, 'c': 1})

# Grouping
groups = defaultdict(list)
for word in ["cat", "dog", "car", "door"]:
    groups[len(word)].append(word)
print(dict(groups))  # {3: ['cat', 'car', 'dog'], 4: ['door']}
```

## Annotated Full Example

```python
# interview_patterns_demo.py
"""Common Python interview patterns."""

from collections import defaultdict, Counter
from typing import List


def two_sum(nums: List[int], target: int) -> List[int]:
    """Find indices of two numbers that add to target."""
    seen = {}
    for i, num in enumerate(nums):
        complement = target - num
        if complement in seen:
            return [seen[complement], i]
        seen[num] = i
    return []


def count_characters(s: str) -> dict:
    """Count character frequencies."""
    return dict(Counter(s))


def group_by_length(words: List[str]) -> dict:
    """Group words by their length."""
    groups = defaultdict(list)
    for word in words:
        groups[len(word)].append(word)
    return dict(groups)


def main() -> None:
    # Two sum
    print(f"Two sum: {two_sum([2, 7, 11, 15], 9)}")
    
    # Count characters
    print(f"Char count: {count_characters('hello')}")
    
    # Group by length
    print(f"Grouped: {group_by_length(['cat', 'dog', 'car', 'door'])}")


if __name__ == "__main__":
    main()
```

## Summary

- Pythonic idioms for efficiency
- Defaultdict for grouping
- Counter for counting

## Next Steps

Continue to **[02_array_and_string_problems.md](./02_array_and_string_problems.md)**
