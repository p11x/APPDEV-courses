# Protocols

## What You'll Learn

- typing.Protocol for structural subtyping
- @runtime_checkable
- Protocol vs ABC
- Duck typing philosophy

## Prerequisites

- Read [03_abstract_classes.md](./03_abstract_classes.md) first

## Protocol

```python
from typing import Protocol

class Drawable(Protocol):
    def draw(self) -> None: ...


class Circle:
    def draw(self) -> None:
        print("Drawing circle")


def render(shape: Drawable) -> None:
    shape.draw()

# Works even without inheritance!
circle = Circle()
render(circle)  # Works!
```

## runtime_checkable

```python
from typing import Protocol, runtime_checkable

@runtime_checkable
class Drawable(Protocol):
    def draw(self) -> None: ...

obj = Circle()
isinstance(obj, Drawable)  # True if has draw()
```

## Summary

- **Protocol**: Structural subtyping (duck typing with type hints)
- No inheritance needed - just implement the interface
- **@runtime_checkable**: Check at runtime with isinstance

## Next Steps

Continue to **[02_slots_and_performance.md](./02_slots_and_performance.md)**
