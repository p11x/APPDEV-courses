# Type Parameter Syntax (PEP 695)

## What You'll Learn

- New generic type syntax
- Type aliases
- Generic functions
- Generic classes

## Prerequisites

- Read [08_migration_guide_312_to_313.md](../../13_Cutting_Edge_Python/02_Python_313_Preview/07_migration_guide_312_to_313.md) first

## New Generic Syntax (Python 3.12+)

```python
# type_params.py

# New syntax (Python 3.12+)
type Point = tuple[float, float]

# Generic function
def first[T](items: list[T]) -> T:
    return items[0]

# Generic class
class Stack[T]:
    def __init__(self) -> None:
        self._items: list[T] = []
    
    def push(self, item: T) -> None:
        self._items.append(item)
    
    def pop(self) -> T:
        return self._items.pop()


# Usage
int_stack: Stack[int] = Stack()
int_stack.push(1)
```

## Type Aliases

```python
# type_aliases.py

# Simple alias
type Matrix = list[list[float]]

# Complex alias
type Callback[T] = callable[[T], None]

# Using in functions
def process(items: list[int], callback: Callback[int]) -> None:
    for item in items:
        callback(item)
```

## Annotated Full Example

```python
# type_parameter_demo.py
"""Complete demonstration of new type parameter syntax."""


# Generic function
def wrap[T](value: T) -> list[T]:
    return [value]


def pair[T, U](a: T, b: U) -> tuple[T, U]:
    return (a, b)


# Generic class
class Container[T]:
    def __init__(self, value: T) -> None:
        self.value = value
    
    def get(self) -> T:
        return self.value


def main() -> None:
    # Using generic function
    print(wrap(42))
    print(wrap("hello"))
    
    # Using pair
    print(pair(1, "a"))
    
    # Using generic class
    c = Container(100)
    print(c.get())


if __name__ == "__main__":
    main()
```

## Summary

- New generic type syntax (PEP 695)
- Type aliases
- Generic functions

## Next Steps

Continue to **[02_f_string_improvements.md](./02_f_string_improvements.md)**
