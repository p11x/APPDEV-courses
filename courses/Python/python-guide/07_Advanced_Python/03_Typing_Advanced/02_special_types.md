# Special Types

## What You'll Learn

- Any
- Union
- Literal
- Final
- ClassVar
- Never, Self (Python 3.11+)

## Prerequisites

- Read [01_generics.md](./01_generics.md) first

## Special Types

```python
from typing import Any, Union, Literal, Final

# Any - any type
def anything(x: Any) -> Any:
    return x

# Union - multiple types
def int_or_str(x: int | str) -> str:
    return str(x)

# Literal - specific values
def status(s: Literal["pending", "done"]) -> None:
    pass

# Final - constant
MAX_SIZE: Final = 100

# ClassVar - class not instance
class Counter:
    count: ClassVar[int] = 0
```

## Summary

- **Any**: Dynamic type
- **Union**: Multiple possible types
- **Literal**: Specific allowed values
- **Final**: Constants

## Next Steps

Continue to **[03_overload_and_protocols.md](./03_overload_and_protocols.md)**
