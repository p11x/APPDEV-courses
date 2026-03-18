# @overload and Protocols

## What You'll Learn

- @overload for multiple signatures
- Protocol for structural typing
- Combining Protocol + Generic

## Prerequisites

- Read [02_special_types.md](./02_special_types.md) first

## @overload

```python
from typing import overload

@overload
def process(x: int) -> int: ...

@overload
def process(x: str) -> str: ...

def process(x: int | str) -> int | str:
    if isinstance(x, int):
        return x * 2
    return x.upper()
```

## Protocol with Generic

```python
from typing import Protocol, TypeVar

T = TypeVar('T')

class Container(Protocol[T]):
    def get(self) -> T: ...
    def set(self, value: T) -> None: ...
```

## Summary

- **@overload**: Multiple function signatures
- **Protocol + Generic**: Powerful typing

## Next Steps

This concludes Advanced Typing. Move to **[08_Projects_and_Practices/01_Testing/01_unittest_basics.md](../08_Projects_and_Practices/01_Testing/01_unittest_basics.md)**
