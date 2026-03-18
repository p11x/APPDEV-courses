# Type Hints and Modern Python

## What You'll Learn
- Advanced type hints (Union, Optional, Any)
- Creating custom types with TypeAlias
- Using dataclasses and Pydantic
- Pattern matching with match statements
- Modern iterators and generators
- Context managers

## Prerequisites
- Completed Python Syntax Refresher
- Understanding of functions and modules
- Familiarity with basic types

## Why Type Hints Matter

Type hints make your code:

1. **More readable** — You know what types to expect
2. **Easier to debug** — Tools catch errors before runtime
3. **Self-documenting** — Less need for comments explaining types
4. **Better IDE support** — Autocomplete works better

Web frameworks like FastAPI use type hints for automatic validation and documentation.

## Advanced Type Hints

### Union Types

When a value can be one of multiple types:

```python
# Before Python 3.10
from typing import Union

def process(value: Union[str, int]) -> str:
    return str(value)

# Python 3.10+ (preferred)
def process(value: str | int) -> str:
    return str(value)
```

🔍 **Union Type Explanation:**

1. `str | int` means the parameter can be either a string or an integer
2. The `|` syntax (Python 3.10+) is cleaner than `Union[str, int]`
3. Type checkers understand this and provide appropriate autocomplete

### Optional Types

When a value can be a type or None:

```python
# Before Python 3.10
from typing import Optional

def greet(name: Optional[str] = None) -> str:
    if name is None:
        return "Hello, stranger!"
    return f"Hello, {name}!"

# Python 3.10+ (preferred)
def greet(name: str | None = None) -> str:
    if name is None:
        return "Hello, stranger!"
    return f"Hello, {name}!"
```

### Any Type

When a value can be absolutely anything:

```python
from typing import Any

def debug_print(value: Any) -> None:
    print(f"Value: {value}, Type: {type(value)}")
```

⚠️ **Use sparingly** — `Any` defeats the purpose of type checking!

### Type Aliases

Create meaningful names for complex types:

```python
from typing import TypeAlias

# Complex type alias
UserDict: TypeAlias = dict[str, str | int | bool]
APIResponse: TypeAlias = dict[str, str | list[dict[str, Any]]]

def get_user(id: int) -> UserDict:
    return {"name": "Alice", "age": 25, "active": True}
```

### Callable Types

For functions as parameters or return values:

```python
from typing import Callable

# A function that takes an int and returns a string
Converter: TypeAlias = Callable[[int], str]

def apply_converter(value: int, converter: Converter) -> str:
    return converter(value)

def int_to_word(num: int) -> str:
    words = ["zero", "one", "two", "three", "four"]
    return words[num] if num < len(words) else "many"

result: str = apply_converter(3, int_to_word)  # "three"
```

## Dataclasses

Dataclasses are perfect for data containers:

```python
from dataclasses import dataclass, field
from datetime import datetime

@dataclass
class User:
    """A user in the system."""
    name: str
    email: str
    age: int = 0
    created_at: datetime = field(default_factory=datetime.now)
    is_active: bool = True
    
    def is_adult(self) -> bool:
        return self.age >= 18
    
    def to_dict(self) -> dict[str, Any]:
        return {
            "name": self.name,
            "email": self.email,
            "age": self.age,
            "is_adult": self.is_adult()
        }

# Creating instances
user: User = User(name="Alice", email="alice@example.com", age=25)
print(user)
# User(name='Alice', email='alice@example.com', age=25, created_at=datetime(...), is_active=True)

# Auto-generated __init__, __repr__, __eq__
print(user.is_adult())  # True
```

🔍 **Dataclass Features:**

1. `@dataclass` decorator — Auto-generates `__init__`, `__repr__`, `__eq__`
2. `field(default_factory=...)` — For mutable defaults or computed values
3. Type hints are first-class — Great for web frameworks

## Pydantic Models

**Pydantic** is a library for data validation using type hints. FastAPI uses it extensively:

```python
from pydantic import BaseModel, EmailStr, Field, validator
from datetime import datetime

class User(BaseModel):
    """User model with automatic validation."""
    name: str = Field(min_length=1, max_length=100)
    email: EmailStr  # Validates email format automatically
    age: int = Field(ge=0, le=150)
    created_at: datetime = Field(default_factory=datetime.now)
    
    @validator("name")
    def name_must_be_title_case(cls, v: str) -> str:
        if not v.istitle():
            raise ValueError("Name must be title case")
        return v

# Creating with validation
try:
    user: User = User(
        name="Alice Smith",
        email="alice@example.com",
        age=25
    )
    print(user)
    # name='Alice Smith' email='alice@example.com' age=25 created_at=datetime(...)
except Exception as e:
    print(f"Validation error: {e}")

# Creating from JSON
json_data: str = '{"name": "Bob", "email": "bob@test.com", "age": 30}'
user_from_json: User = User.model_validate_json(json_data)
```

🔍 **Pydantic Benefits:**

1. **Automatic validation** — Invalid data raises clear errors
2. **JSON parsing** — Convert JSON to Python objects seamlessly
3. **Serialization** — Convert back to JSON with `.model_dump_json()`
4. **Nested models** — Complex data structures validated automatically
5. **Performance** — Much faster than runtime type checking

## Pattern Matching (match statements)

Python 3.10+ introduced match statements:

```python
from typing import Any

def http_status(code: int) -> str:
    match code:
        case 200:
            return "OK"
        case 201:
            return "Created"
        case 204:
            return "No Content"
        case 301 | 302 | 307 | 308:
            return "Redirect"
        case 400:
            return "Bad Request"
        case 401 | 403:
            return "Unauthorized"
        case 404:
            return "Not Found"
        case 500:
            return "Server Error"
        case _:
            return "Unknown"

# With patterns
def process_response(response: dict[str, Any]) -> str:
    match response:
        case {"status": "success", "data": var}:
            return f"Success with data: {var}"
        case {"status": "error", "message": msg}:
            return f"Error: {msg}"
        case {"status": status}:
            return f"Unknown status: {status}"
        case _:
            return "Invalid response"
```

🔍 **Match Statement Patterns:**

1. `case 200:` — Literal match
2. `case 301 | 302 | 307:` — Multiple values (or pattern)
3. `case {"status": status}:` — Capture value into variable
4. `case _:` — Wildcard (matches everything)

## Generators and Iterators

### Generators

Generators create iterators lazily (one item at a time):

```python
def count_up_to(n: int) -> Generator[int, None, None]:
    """Yield numbers from 1 to n."""
    i: int = 1
    while i <= n:
        yield i
        i += 1

# Using the generator
counter: Generator[int, None, None] = count_up_to(5)
print(next(counter))  # 1
print(next(counter))  # 2
print(list(counter))  # [3, 4, 5] - exhausted

# More common: iterate directly
for num in count_up_to(3):
    print(num)  # 1, 2, 3
```

### Generator Expressions

Like list comprehensions but lazy:

```python
# List comprehension (creates entire list)
squares: list[int] = [x**2 for x in range(10)]

# Generator expression (lazy)
squares_gen: Generator[int, None, None] = (x**2 for x in range(10))

# Only computes when needed
print(next(squares_gen))  # 0
print(next(squares_gen))  # 1
```

### The itertools Module

```python
import itertools

# Infinite counter
counter: itertools.count = itertools.count(start=1, step=2)
print(next(counter))  # 1
print(next(counter))  # 3

# Cycle through sequence
cycler: itertools.cycle = itertools.cycle(["A", "B", "C"])
print([next(cycler) for _ in range(5)])  # ['A', 'B', 'C', 'A', 'B']

# Chain iterables
combined: list[int] = list(itertools.chain([1, 2], [3, 4], [5]))
# [1, 2, 3, 4, 5]

# Product (cartesian product)
grid: list[tuple[int, int]] = list(itertools.product([1, 2], [3, 4]))
# [(1, 3), (1, 4), (2, 3), (2, 4)]
```

## Context Managers

Context managers handle setup and teardown:

```python
# File handling (automatic cleanup)
with open("file.txt", "w") as f:
    f.write("Hello!")

# Custom context manager
from contextlib import contextmanager

@contextmanager
def timer(name: str):
    """Time a code block."""
    import time
    start = time.perf_counter()
    try:
        yield  # Code runs here
    finally:
        elapsed = time.perf_counter() - start
        print(f"{name} took {elapsed:.4f} seconds")

with timer("My operation"):
    # Do something
    sum(range(1000000))
```

## Modern Patterns in Web Development

### Enum Types

```python
from enum import Enum

class UserRole(Enum):
    ADMIN = "admin"
    MODERATOR = "moderator"
    USER = "user"
    GUEST = "guest"

def describe_role(role: UserRole) -> str:
    match role:
        case UserRole.ADMIN:
            return "Full access to everything"
        case UserRole.MODERATOR:
            return "Can moderate content"
        case UserRole.USER:
            return "Standard access"
        case UserRole.GUEST:
            return "Limited access"

print(describe_role(UserRole.ADMIN))
```

### TypedDict for Dictionary Structures

```python
from typing import TypedDict

class UserDict(TypedDict):
    name: str
    email: str
    age: int
    is_active: bool

def create_user(data: UserDict) -> UserDict:
    return {**data, "is_active": True}

user: UserDict = create_user({
    "name": "Alice",
    "email": "alice@example.com",
    "age": 25,
    "is_active": False
})
```

## Summary
- Use **type hints** throughout your code for better maintainability
- Use `str | None` for optional values (Python 3.10+)
- Use **dataclasses** for simple data containers
- Use **Pydantic** for validated data models (common in FastAPI)
- Use **match statements** for complex conditional logic
- Use **generators** for memory-efficient iteration
- Use **context managers** for resource management

## Next Steps
→ Continue to `../03-flask/01-flask-introduction.md` to start building web applications with Flask.
