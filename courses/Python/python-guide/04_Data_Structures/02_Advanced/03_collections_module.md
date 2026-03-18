# Collections Module Deep Dive

## What You'll Learn

- deque for efficient queue operations
- Counter for counting occurrences
- defaultdict for automatic defaults
- ChainMap for multiple dictionaries
- namedtuple deep dive

## Prerequisites

- Read [02_typed_collections.md](./02_typed_collections.md) first

## deque — Double-Ended Queue

```python
from collections import deque

# Efficient for append/pop from both ends
dq: deque[int] = deque([1, 2, 3])
dq.appendleft(0)  # [0, 1, 2, 3]
dq.append(4)      # [0, 1, 2, 3, 4]
dq.popleft()       # [1, 2, 3, 4]
dq.pop()          # [1, 2, 3]
```

## Counter

```python
from collections import Counter

# Count occurrences
c: Counter[str] = Counter("hello world")
print(c)  # Counter({'l': 3, 'o': 2, 'h': 1, 'e': 1, ' ': 1, 'w': 1, 'r': 1, 'd': 1})

# Most common
print(c.most_common(2))  # [('l', 3), ('o', 2)]
```

## defaultdict

```python
from collections import defaultdict

# Auto-default for missing keys
dd: defaultdict[str, int] = defaultdict(int)
dd["missing"]  # Returns 0 instead of error

# With factory
dd_list: defaultdict[str, list] = defaultdict(list)
dd_list["key"].append(1)  # Works!
```

## ChainMap

```python
from collections import ChainMap

# Search multiple dictionaries
d1: dict[str, int] = {"a": 1, "b": 2}
d2: dict[str, int] = {"b": 3, "c": 4}

cm: ChainMap = ChainMap(d1, d2)
print(cm["a"])  # 1 (found in d1)
print(cm["b"])  # 2 (d1 takes precedence)
print(cm["c"])  # 4 (found in d2)
```

## Summary

- **deque**: Fast append/pop from both ends
- **Counter**: Count hashable objects
- **defaultdict**: Auto-default values
- **ChainMap**: Multiple dictionaries as one

## Next Steps

Continue to **[04_Data_Structures/03_Algorithms_With_Python/01_searching.md](../03_Algorithms_With_Python/01_searching.md)**
