# Python Syntax Refresher

## What You'll Learn
- Python 3.11+ syntax fundamentals
- Variables and data types
- Control flow with if/elif/else and loops
- Data structures: lists, dictionaries, sets
- String formatting with f-strings
- Modern Python patterns

## Prerequisites
- Basic programming knowledge (any language)
- Python 3.11+ installed

## Variables and Data Types

### Creating Variables

In Python, you create variables by simply assigning a value:

```python
# String (text)
name: str = "Alice"

# Integer (whole number)
age: int = 25

# Float (decimal number)
height: float = 5.8

# Boolean (True/False)
is_student: bool = True

# None (absence of value)
middle_name: str | None = None
```

🔍 **Line-by-Line Breakdown:**

1. `name: str = "Alice"` — Creates a variable with type hint `str` (string). Type hints are optional but help documentation and IDEs.
2. `age: int = 25` — Integer type for whole numbers
3. `height: float = 5.8` — Float type for decimal numbers
4. `is_student: bool = True` — Boolean type (capital T/F in Python)
5. `middle_name: str | None = None` — Union type (Python 3.10+); can be string or None

### Type Annotations

Python 3.5+ supports type hints. While optional, they're highly recommended:

```python
# Without type hints (older style)
def greet(name):
    return f"Hello, {name}!"

# With type hints (modern style, recommended)
def greet(name: str) -> str:
    return f"Hello, {name}!"
```

## Data Structures

### Lists

Lists are ordered, mutable collections:

```python
# Creating a list
fruits: list[str] = ["apple", "banana", "cherry"]

# Accessing elements (0-indexed)
first_fruit: str = fruits[0]     # "apple"
last_fruit: str = fruits[-1]     # "cherry"

# Slicing
first_two: list[str] = fruits[:2]  # ["apple", "banana"]

# Modifying
fruits.append("orange")           # Add to end
fruits.insert(1, "mango")         # Insert at index 1
fruits.remove("banana")           # Remove by value
popped: str = fruits.pop()        # Remove and return last

# List comprehension (modern style)
squares: list[int] = [x**2 for x in range(10)]
# [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]
```

### Dictionaries

Dictionaries store key-value pairs:

```python
# Creating a dictionary
person: dict[str, str | int] = {
    "name": "Alice",
    "age": 25,
    "city": "New York"
}

# Accessing values
name: str = person["name"]           # "Alice"
age: int = person.get("age", 0)      # 25, with default

# Modifying
person["age"] = 26                   # Update value
person["email"] = "alice@example.com"  # Add new key-value
del person["city"]                   # Delete key

# Dictionary comprehension
squares_dict: dict[int, int] = {x: x**2 for x in range(5)}
# {0: 0, 1: 1, 2: 4, 3: 9, 4: 16}

# Iterating
for key, value in person.items():
    print(f"{key}: {value}")
```

### Sets

Sets are unordered collections of unique elements:

```python
# Creating a set
colors: set[str] = {"red", "green", "blue"}

# Adding/removing
colors.add("yellow")
colors.remove("green")       # Raises error if not found
colors.discard("purple")     # No error if not found

# Set operations
set1: set[int] = {1, 2, 3, 4}
set2: set[int] = {3, 4, 5, 6}

union: set[int] = set1 | set2        # {1, 2, 3, 4, 5, 6}
intersection: set[int] = set1 & set2  # {3, 4}
difference: set[int] = set1 - set2    # {1, 2}
```

## Control Flow

### If/Elif/Else

```python
def check_grade(score: int) -> str:
    """Return letter grade based on score."""
    if score >= 90:
        return "A"
    elif score >= 80:
        return "B"
    elif score >= 70:
        return "C"
    elif score >= 60:
        return "D"
    else:
        return "F"

# Modern match statement (Python 3.10+)
def check_grade_match(score: int) -> str:
    match score:
        case 90 | 91 | 92 | 93 | 94 | 95 | 96 | 97 | 98 | 99 | 100:
            return "A"
        case x if x >= 80:
            return "B"
        case x if x >= 70:
            return "C"
        case x if x >= 60:
            return "D"
        case _:
            return "F"
```

🔍 **Match Statement Breakdown:**

1. `match score:` — Starts pattern matching on `score`
2. `case 90 | 91 | ...` — Matches specific values (using `|` as "or")
3. `case x if x >= 80:` — Captures value in `x`, then checks condition
4. `case _:` — Wildcard case (matches anything, like `else`)

### For Loops

```python
# Basic loop
for i in range(5):
    print(i)  # 0, 1, 2, 3, 4

# Loop through list
fruits: list[str] = ["apple", "banana", "cherry"]
for fruit in fruits:
    print(fruit)

# Enumerate (get index and value)
for index, fruit in enumerate(fruits):
    print(f"{index}: {fruit}")

# With else (runs after loop completes)
for fruit in fruits:
    if fruit == "banana":
        print("Found banana!")
        break  # Exit loop early
else:
    print("Loop completed without break")
```

### While Loops

```python
count: int = 0
while count < 5:
    print(count)
    count += 1  # Must increment manually

# With break
while True:
    user_input: str = input("Enter 'quit' to exit: ")
    if user_input == "quit":
        break
```

## String Formatting

### F-Strings (Recommended)

F-strings are the modern, readable way to format strings:

```python
name: str = "Alice"
age: int = 25

# Simple interpolation
message: str = f"Hello, {name}!"
# "Hello, Alice!"

# Expressions inside f-strings
result: str = f"In 10 years, {name} will be {age + 10}"
# "In 10 years, Alice will be 35"

# Formatting
pi: float = 3.14159
formatted: str = f"Pi is approximately {pi:.2f}"
# "Pi is approximately 3.14"

# F-strings with dictionaries
person: dict[str, str | int] = {"name": "Bob", "age": 30}
greeting: str = f"Hi, I'm {person['name']}, age {person['age']}"
# "Hi, I'm Bob, age 30"
```

🔍 **F-String Formatting:**

1. `f"text {variable} text"` — Prefix with `f`, use `{}` to embed variables
2. `{pi:.2f}` — Format as float with 2 decimal places
3. `{value:>10}` — Right-align in 10 characters
4. `{value:05d}` — Pad integers with zeros

## Modern Python Patterns

### Walrus Operator (:=) - Python 3.8+

Assigns and returns a value in one expression:

```python
# Without walrus (repeating calculation)
if len(name := input("Enter name: ")) > 5:
    print(f"Long name: {name}")

# With walrus (calculate once)
if (length := len(name := input("Enter name: "))) > 5:
    print(f"Long name ({length} chars): {name}")
```

### Chained Comparisons

```python
# Instead of: if x > 0 and x < 10
if 0 < x < 10:
    print("x is between 0 and 10")

# Chained comparisons work with 'and'
if 0 < x < 10 < y < 20:
    print("x between 0-10, y between 10-20")
```

### Unpacking

```python
# Tuple unpacking
coordinates: tuple[int, int, int] = (10, 20, 30)
x, y, z = coordinates

# Extended unpacking
first, *middle, last = [1, 2, 3, 4, 5]
# first=1, middle=[2, 3, 4], last=5

# Swap without temp
a, b = b, a
```

### Dataclasses (Recommended for Data)

```python
from dataclasses import dataclass

@dataclass
class User:
    name: str
    email: str
    age: int = 0  # Default value
    
    def is_adult(self) -> bool:
        return self.age >= 18

# Creating instances
user: User = User(name="Alice", email="alice@example.com", age=25)
print(user)  # User(name='Alice', email='alice@example.com', age=25)
```

🔍 **Dataclass Benefits:**

1. Automatically generates `__init__`, `__repr__`, `__eq__`
2. Clean, readable way to define data containers
3. Type hints are first-class citizens
4. Great for API responses and data transfer objects

## Exception Handling

```python
def divide(a: float, b: float) -> float | None:
    """Divide two numbers, returns None on error."""
    try:
        result: float = a / b
        return result
    except ZeroDivisionError:
        print("Cannot divide by zero!")
        return None
    except TypeError:
        print("Both arguments must be numbers!")
        return None
    finally:
        print("This always runs")

# Raising exceptions
def validate_age(age: int) -> int:
    if age < 0:
        raise ValueError("Age cannot be negative")
    return age
```

## Summary
- Python uses **type hints** (`str`, `int`, etc.) for better code documentation
- **Lists** are ordered, mutable collections; **dicts** store key-value pairs
- Use **match statements** (Python 3.10+) instead of long if/elif chains
- Use **f-strings** for string formatting
- **Dataclasses** provide clean data structures with less boilerplate
- Always handle **exceptions** with try/except blocks

## Next Steps
→ Continue to `02-functions-and-modules.md` to learn about organizing code with functions and modules.
