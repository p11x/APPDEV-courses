# Methods and self

## What You'll Learn

- Understanding instance methods
- The self parameter
- Calling methods
- Method types: instance, class, static

## Prerequisites

- Read [02_instance_and_class_vars.md](./02_instance_and_class_vars.md) first

## Instance Methods

Instance methods operate on instances of a class and receive `self` as the first parameter.

```python
# instance_methods.py

class Rectangle:
    def __init__(self, width: float, height: float) -> None:
        self.width = width
        self.height = height
    
    def area(self) -> float:
        """Calculate the area."""
        return self.width * self.height
    
    def perimeter(self) -> float:
        """Calculate the perimeter."""
        return 2 * (self.width + self.height)
    
    def scale(self, factor: float) -> None:
        """Scale the rectangle by a factor."""
        self.width *= factor
        self.height *= factor


rect = Rectangle(5, 3)
print(f"Area: {rect.area()}")
print(f"Perimeter: {rect.perimeter()}")
rect.scale(2)
print(f"Scaled area: {rect.area()}")
```

## The self Parameter

`self` refers to the instance itself and allows access to attributes and other methods.

```python
# self_parameter.py

class Counter:
    def __init__(self, start: int = 0) -> None:
        self.count = start
    
    def increment(self) -> None:
        self.count += 1
    
    def decrement(self) -> None:
        self.count -= 1
    
    def reset(self) -> None:
        self.count = 0
    
    def get_count(self) -> int:
        return self.count


c = Counter(10)
c.increment()
c.increment()
print(c.get_count())
```

## Annotated Full Example

```python
# methods_demo.py
"""Complete demonstration of methods and self."""

from typing import Optional


class Stack:
    """A simple stack implementation."""
    
    def __init__(self) -> None:
        self._items: list = []
    
    def push(self, item: object) -> None:
        """Add item to top of stack."""
        self._items.append(item)
    
    def pop(self) -> Optional[object]:
        """Remove and return top item."""
        if self._items:
            return self._items.pop()
        return None
    
    def peek(self) -> Optional[object]:
        """View top item without removing."""
        if self._items:
            return self._items[-1]
        return None
    
    def is_empty(self) -> bool:
        """Check if stack is empty."""
        return len(self._items) == 0
    
    def __len__(self) -> int:
        return len(self._items)


def main() -> None:
    stack = Stack()
    stack.push(1)
    stack.push(2)
    stack.push(3)
    
    print(f"Stack length: {len(stack)}")
    print(f"Peek: {stack.peek()}")
    print(f"Pop: {stack.pop()}")
    print(f"Remaining: {stack._items}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding instance methods
- The self parameter
- Calling methods

## Next Steps

Continue to **[04_constructors_and_init.md](./04_constructors_and_init.md)**
