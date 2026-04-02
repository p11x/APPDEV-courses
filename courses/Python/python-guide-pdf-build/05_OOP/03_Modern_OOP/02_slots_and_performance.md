# __slots__ and Performance

## What You'll Learn

- __slots__ reduces memory
- slots=True in dataclasses
- When to use __slots__

## Prerequisites

- Read [01_protocols.md](./01_protocols.md) first

## Using __slots__

```python
class WithoutSlots:
    def __init__(self, name: str) -> None:
        self.name = name


class WithSlots:
    __slots__ = ('name',)
    
    def __init__(self, name: str) -> None:
        self.name = name


# With slots uses less memory!
import sys
print(sys.getsizeof(WithoutSlots("test")))  # More
print(sys.getsizeof(WithSlots("test")))    # Less
```

## In Dataclasses

```python
from dataclasses import dataclass

@dataclass(slots=True)
class User:
    name: str
    age: int
```

## Summary

- **__slots__**: Prevents __dict__, saves memory
- **slots=True**: In dataclasses (Python 3.10+)

## Next Steps

Continue to **[03_descriptors.md](./03_descriptors.md)**
