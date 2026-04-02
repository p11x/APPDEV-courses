# 🧬 Python 3.12 Type System Upgrades

## 🎯 What You'll Learn

- PEP 695's new type alias syntax with the `type` keyword
- New generic function and class syntax without TypeVar boilerplate
- `@override` decorator for intentional method overrides
- `@dataclass(slots=True)` for automatic memory optimization
- `Required[]` and `NotRequired[]` for precise TypedDict fields

## 📦 Prerequisites

- Understanding of Python type hints from earlier guides
- Familiarity with generics (e.g., `List[int]`, `Dict[str, int]`)

---

## PEP 695: A New Type Alias Syntax

Python 3.12 introduces a cleaner way to create type aliases using the `type` keyword:

### Old Way: TypeVar for Type Aliases

```python
from typing import TypeVar

# Traditional approach - works but verbose
Vector = list[float]  # This is just a variable, not a true type alias
Point = tuple[float, float]  # Same here - just a regular assignment
```

### New Way: `type` Keyword (PEP 695)

```python
# Python 3.12+ - clean, explicit type aliases
type Vector = list[float]  # True type alias with 'type' keyword
type Point = tuple[float, float]  # Clear and readable

def scale(v: Vector, factor: float) -> Vector:
    """Multiply each element by a factor."""
    return [x * factor for x in v]

# Using the new type alias
result: Vector = scale([1.0, 2.0, 3.0], 2.0)
```

### 💡 Line-by-Line Breakdown

```python
type Vector = list[float]           # New type alias syntax - creates a true type
type Point = tuple[float, float]    # Another type alias for coordinates

def scale(v: Vector, factor: float) -> Vector:  # Use Vector like any type
    return [x * factor for x in v]  # Multiply each element in the vector

result: Vector = scale([1.0, 2.0, 3.0], 2.0)  # Annotate variables with type aliases
```

---

## New Generic Syntax Without TypeVar

PEP 695 allows defining generic functions and classes without the old TypeVar boilerplate:

### Old Way: TypeVar Boilerplate

```python
from typing import TypeVar, Generic

T = TypeVar('T')

def first(lst: list[T]) -> T:
    """Return the first element of a list."""
    return lst[0]

class Stack(Generic[T]):
    """A simple stack generic over type T."""
    def __init__(self) -> None:
        self._items: list[T] = []
    
    def push(self, item: T) -> None:
        self._items.append(item)
    
    def pop(self) -> T:
        return self._items.pop()
```

### New Way: Simple Generic Syntax (Python 3.12+)

```python
# Generic function - no TypeVar needed!
def first[T](lst: list[T]) -> T:
    """Return the first element of a list."""
    return lst[0]

# Generic class - clean and simple
class Stack[T]:
    """A simple stack generic over type T."""
    
    def __init__(self) -> None:
        self._items: list[T] = []  # Can annotate internal variables
    
    def push(self, item: T) -> None:
        self._items.append(item)
    
    def pop(self) -> T:
        return self._items.pop()
    
    def peek(self) -> T:
        return self._items[-1]

# Using the new generic class
int_stack: Stack[int] = Stack()
int_stack.push(42)
int_stack.push(100)
print(int_stack.pop())  # 100
```

### 💡 Line-by-Line Breakdown

```python
def first[T](lst: list[T]) -> T:      # [T] makes this function generic
    return lst[0]                      # Return type uses T

class Stack[T]:                       # [T] after class name makes it generic
    def __init__(self) -> None:
        self._items: list[T] = []     # Internal storage with type annotation
    
    def push(self, item: T) -> None:  # T works in method signatures too
        self._items.append(item)
    
    def pop(self) -> T:
        return self._items.pop()
    
    def peek(self) -> T:
        return self._items[-1]

int_stack: Stack[int] = Stack()      # Specify the type parameter explicitly
int_stack.push(42)
int_stack.push(100)
print(int_stack.pop())                # 100
```

---

## Using `@override` Decorator

Python 3.12 adds `@override` from `typing` to mark intentional method overrides:

```python
from typing import override

class Animal:
    """Base animal class."""
    def speak(self) -> str:
        raise NotImplementedError

class Dog(Animal):
    @override  # This decorator says "I'm intentionally overriding a parent method"
    def speak(self) -> str:
        return "Woof!"

class Cat(Animal):
    @override
    def speak(self) -> str:
        return "Meow!"

# If you make a mistake, Python will tell you
class Fish(Animal):
    @override
    def swim(self) -> str:  # ERROR: swim doesn't override anything in Animal!
        return "Fish swims"
```

### 💡 Line-by-Line Breakdown

```python
from typing import override   # Import the decorator from typing

class Animal:                 # Base class with abstract-like method
    def speak(self) -> str:  # Base method that raises error if not overridden
        raise NotImplementedError

class Dog(Animal):            # Dog inherits from Animal
    @override                #Decorator marking intentional override
    def speak(self) -> str:   # Override the speak method
        return "Woof!"

class Cat(Animal):
    @override
    def speak(self) -> str:
        return "Meow!"

class Fish(Animal):
    @override
    def swim(self) -> str:   # This would cause an error - swim doesn't exist in Animal
        return "Fish swims"
```

### Why Use `@override`?

1. **Catches typos**: If you misspell the method name, Python warns you
2. **Documents intent**: Shows other developers this override is intentional
3. **Refactoring safety**: Warns if parent class method is removed or renamed

---

## `@dataclass(slots=True)`

Python 3.10+ allows `@dataclass` to automatically generate `__slots__`:

```python
from dataclasses import dataclass

@dataclass(slots=True)
class Point:
    """A point with automatic __slots__ for memory efficiency."""
    x: float
    y: float
    label: str = "origin"

# The class automatically has __slots__ = ['x', 'y', 'label']
p = Point(1.0, 2.0, "marker")

# Access attributes normally
print(p.x, p.y, p.label)

# Try to add new attributes - this will fail!
try:
    p.z = 3.0  # AttributeError: 'Point' object has no attribute 'z'
except AttributeError as e:
    print(f"Error: {e}")
```

### 💡 Line-by-Line Breakdown

```python
from dataclasses import dataclass  # Import dataclass decorator

@dataclass(slots=True)             # slots=True generates __slots__ automatically
class Point:
    x: float                       # These become slots, not __dict__ entries
    y: float
    label: str = "origin"         # Default value still works with slots

p = Point(1.0, 2.0, "marker")     # Create instance normally

print(p.x, p.y, p.label)          # Access attributes the normal way

try:
    p.z = 3.0                      # Can't add new attributes - slots restrict this
except AttributeError as e:
    print(f"Error: {e}")           # This will fail as expected
```

### Memory Comparison

```python
from dataclasses import dataclass

@dataclass
class RegularPoint:
    x: float
    y: float

@dataclass(slots=True)
class SlottedPoint:
    x: float
    y: float

import sys

regular = RegularPoint(1.0, 2.0)
slotted = SlottedPoint(1.0, 2.0)

# Slotted objects use less memory
print(f"Regular: {sys.getsizeof(regular)} bytes + dict")
print(f"Slotted: {sys.getsizeof(slotted)} bytes (no dict)")
```

---

## TypedDict with Required and NotRequired

Python 3.11+ adds `Required[]` and `NotRequired[]` for precise field requirements:

```python
from typing import TypedDict, NotRequired, Required

class User(TypedDict):
    # These fields are required (default)
    name: Required[str]
    email: Required[str]
    # These fields are optional
    age: NotRequired[int]
    bio: NotRequired[str]
    avatar_url: NotRequired[str]

# Valid - only required fields
user1: User = {"name": "Alice", "email": "alice@example.com"}

# Valid - all fields
user2: User = {
    "name": "Bob",
    "email": "bob@example.com",
    "age": 30,
    "bio": "Hello!",
    "avatar_url": "https://example.com/bob.png"
}

# Invalid - missing required field
try:
    user3: User = {"name": "Charlie"}  # Missing 'email' - Type error!
except Exception as e:
    print(f"Error: {e}")
```

### 💡 Line-by-Line Breakdown

```python
from typing import TypedDict, NotRequired, Required  # Import the new types

class User(TypedDict):
    name: Required[str]        # This field MUST be present
    email: Required[str]      # This field MUST be present
    age: NotRequired[int]     # This field is optional
    bio: NotRequired[str]     # This field is optional
    avatar_url: NotRequired[str]  # This field is optional

user1: User = {"name": "Alice", "email": "alice@example.com"}  # Valid - required only
user2: User = {"name": "Bob", "email": "bob@example.com", "age": 30, "bio": "Hello!", "avatar_url": "https://example.com/bob.png"}  # Valid - all fields

try:
    user3: User = {"name": "Charlie"}  # Invalid - email is required!
except Exception as e:
    print(f"Error: {e}")
```

---

## Old vs New: Side-by-Side Comparison

| Feature | Old Way (Python 3.9-3.11) | New Way (Python 3.12+) |
|---------|--------------------------|------------------------|
| Type Alias | `Vector = List[float]` | `type Vector = list[float]` |
| Generic Function | `def f[T](x: T): ...` | Same in 3.12+ |
| Generic Class | `class Stack[T]: ...` | Same in 3.12+ |
| Override Check | No built-in | `@override` decorator |
| Dataclass Slots | Manual `__slots__` | `@dataclass(slots=True)` |

---

## ✅ Summary

- PEP 695 introduces `type` keyword for cleaner type aliases
- Generic functions and classes now use simple `[T]` syntax without TypeVar
- `@override` decorator catches accidental method overrides
- `@dataclass(slots=True)` automatically generates `__slots__` for memory efficiency
- `Required[]` and `NotRequired[]` provide precise TypedDict field control

## ➡️ Next Steps

Continue to [03_new_stdlib_additions.md](./03_new_stdlib_additions.md) to learn about new standard library features in Python 3.12 including tomllib, pathlib.walk(), f-string improvements, and more.

## 🔗 Further Reading

- [PEP 695: Type Parameter Syntax](https://peps.python.org/pep-0695/)
- [PEP 673: TypeAlias](https://peps.python.org/pep-0693/) (for Self type)
- [dataclass — Data classes](https://docs.python.org/3.12/library/dataclasses.html)
- [typing — Support for type hints](https://docs.python.org/3.12/library/typing.html)
