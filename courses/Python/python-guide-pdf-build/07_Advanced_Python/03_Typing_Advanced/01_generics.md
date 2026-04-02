# Generics

## What You'll Learn

- TypeVar
- Generic[T]
- ParamSpec (Python 3.10+)
- TypeVarTuple (Python 3.11+)

## Prerequisites

- Read [03_async_generators_context_managers.md](./03_async_generators_context_managers.md) first

## TypeVar

```python
from typing import TypeVar

T = TypeVar('T')

def first(items: list[T]) -> T | None:
    return items[0] if items else None
```

## Generic

```python
from typing import Generic, TypeVar

T = TypeVar('T')

class Stack(Generic[T]):
    def __init__(self) -> None:
        self.items: list[T] = []
    
    def push(self, item: T) -> None:
        self.items.append(item)
    
    def pop(self) -> T | None:
        return self.items.pop() if self.items else None
```

## Summary

- **TypeVar**: Type variable
- **Generic[T]**: Generic class

## Next Steps

Continue to **[02_special_types.md](./02_special_types.md)**
