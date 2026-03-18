# Functions and Modules

## What You'll Learn
- Defining and calling functions
- Function parameters and return types
- Lambda functions
- Organizing code with modules
- Importing and using external packages
- The Python standard library

## Prerequisites
- Completed Python Syntax Refresher
- Understanding of basic Python types

## Functions

A **function** is a reusable block of code that performs a specific task. Functions are fundamental to writing clean, maintainable code.

### Defining Functions

```python
def greet(name: str) -> str:
    """Return a greeting message.
    
    Args:
        name: The name to greet
        
    Returns:
        A greeting string
    """
    return f"Hello, {name}!"

# Calling the function
message: str = greet("Alice")
print(message)  # Hello, Alice!
```

🔍 **Function Components:**

1. `def greet(name: str) -> str:` — Function definition with parameter type hint and return type
2. `"""docstring"""` — Documentation explaining what the function does
3. `return` — Sends a value back to the caller

### Parameters and Arguments

```python
# Default parameters
def greet(name: str, greeting: str = "Hello") -> str:
    return f"{greeting}, {name}!"

print(greet("Alice"))              # Hello, Alice!
print(greet("Bob", "Hi"))          # Hi, Bob!


# Multiple default parameters
def create_user(
    username: str,
    email: str,
    active: bool = True,
    role: str = "user"
) -> dict[str, str | bool]:
    return {
        "username": username,
        "email": email,
        "active": active,
        "role": role
    }


# Keyword arguments (specify which param by name)
user: dict[str, str | bool] = create_user(
    email="alice@example.com",
    username="alice",
    role="admin"
)
```

### Variable Arguments

```python
# *args - variable number of positional arguments
def sum_all(*args: float) -> float:
    """Sum all provided numbers."""
    total: float = 0
    for num in args:
        total += num
    return total

result: float = sum_all(1, 2, 3, 4, 5)  # 15


# **kwargs - variable number of keyword arguments
def create_user_info(**kwargs: str) -> dict[str, str]:
    """Create a user info dictionary from keyword arguments."""
    return kwargs

info: dict[str, str] = create_user_info(
    name="Alice",
    email="alice@example.com",
    city="NYC"
)
# {'name': 'Alice', 'email': 'alice@example.com', 'city': 'NYC'}


# Combining all types
def flexible_function(
    required: str,           # Must be provided
    *args: float,            # Optional positional
    default: str = "value",  # Optional with default
    **kwargs: str            # Optional keyword
) -> None:
    print(f"Required: {required}")
    print(f"Args: {args}")
    print(f"Default: {default}")
    print(f"Kwargs: {kwargs}")
```

## Lambda Functions

**Lambda functions** are small, anonymous functions defined in a single line. They're useful when you need a simple function for a short period.

```python
# Regular function
def add_one(x: int) -> int:
    return x + 1

# Lambda equivalent
add_one_lambda: callable = lambda x: x + 1

# Using with built-in functions
numbers: list[int] = [1, 2, 3, 4, 5]
squared: list[int] = list(map(lambda x: x**2, numbers))
# [1, 4, 9, 16, 25]

# Using with sorted
names: list[str] = ["Charlie", "Alice", "Bob"]
sorted_by_length: list[str] = sorted(names, key=lambda name: len(name))
# ['Bob', 'Alice', 'Charlie']

# Using with filter
even_numbers: list[int] = list(filter(lambda x: x % 2 == 0, numbers))
# [2, 4]

# List comprehension equivalent (often preferred)
squared_comp: list[int] = [x**2 for x in numbers]
```

🔍 **Lambda Syntax:**

1. `lambda` — Keyword to define anonymous function
2. `x: int` — Parameters (can have type hints but usually omitted)
3. `x + 1` — Expression that gets returned

## Modules

A **module** is a Python file containing code that can be imported. Modules help organize code into reusable components.

### Creating a Module

Create a file called `utils.py`:

```python
# utils.py

def format_name(first: str, last: str) -> str:
    """Format a full name."""
    return f"{first} {last}".title()

def validate_email(email: str) -> bool:
    """Check if email looks valid."""
    return "@" in email and "." in email.split("@")[-1]

# Constants
MAX_NAME_LENGTH: int = 50
DEFAULT_ROLE: str = "user"
```

### Importing Modules

```python
# Import the entire module
import utils

name: str = utils.format_name("john", "doe")
is_valid: bool = utils.validate_email("john@example.com")

# Import specific items (recommended)
from utils import format_name, validate_email

name = format_name("john", "doe")
is_valid = validate_email("john@example.com")

# Import with alias
from utils import format_name as fn, validate_email as ve

# Import everything (generally avoid)
from utils import *
```

### The `if __name__ == "__main__"` Pattern

This allows a module to run as a script when called directly, but not when imported:

```python
# my_module.py

def process_data(data: list[int]) -> list[int]:
    """Process a list of numbers."""
    return [x * 2 for x in data]

def main() -> None:
    """Run the module as a script."""
    sample_data: list[int] = [1, 2, 3, 4, 5]
    result: list[int] = process_data(sample_data)
    print(f"Processed: {result}")

if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `if __name__ == "__main__":` — This is True only when the file is run directly, not when imported
2. `main()` — Calls the main function
3. When imported as a module, `main()` won't run automatically

## The Standard Library

Python comes with "batteries included" — a rich standard library for common tasks:

### Working with Files

```python
from pathlib import Path

# Write to file
output_path: Path = Path("output.txt")
output_path.write_text("Hello, World!")

# Read from file
content: str = output_path.read_text()
print(content)

# Read lines
lines: list[str] = output_path.read_text().splitlines()
```

### JSON Handling

```python
import json

# Convert Python to JSON
data: dict[str, str | int] = {"name": "Alice", "age": 25}
json_string: str = json.dumps(data, indent=2)
# '{"name": "Alice", "age": 25}'

# Convert JSON to Python
parsed: dict[str, str | int] = json.loads(json_string)

# Read/Write JSON files
json.dump(data, open("data.json", "w"))
loaded: dict = json.load(open("data.json"))
```

### Date and Time

```python
from datetime import datetime, timedelta

# Current time
now: datetime = datetime.now()
print(f"Current time: {now}")

# Parse string to datetime
parsed_date: datetime = datetime.fromisoformat("2024-01-15 10:30:00")

# Format datetime to string
formatted: str = now.strftime("%Y-%m-%d %H:%M:%S")
# "2024-01-15 14:30:00"

# Time arithmetic
future: datetime = now + timedelta(days=7)
past: datetime = now - timedelta(hours=2)
```

### Random Numbers

```python
import random

# Random integer
dice_roll: int = random.randint(1, 6)

# Random float between 0 and 1
random_float: float = random.random()

# Choose random item
colors: list[str] = ["red", "green", "blue"]
chosen: str = random.choice(colors)

# Shuffle list
numbers: list[int] = [1, 2, 3, 4, 5]
random.shuffle(numbers)

# Random sample
sample: list[int] = random.sample(range(1, 50), 6)  # Lottery numbers
```

## Organizing Projects

A typical Python web project structure:

```
my_project/
├── app/
│   ├── __init__.py      # Makes app a package
│   ├── main.py          # Main application code
│   ├── models.py        # Database models
│   ├── routes.py        # URL routes
│   └── utils.py         # Helper functions
├── tests/
│   ├── __init__.py
│   └── test_main.py
├── venv/                # Virtual environment
├── requirements.txt     # Dependencies
└── README.md
```

### The `__init__.py` File

This file makes a directory a Python package:

```python
# app/__init__.py
from .main import app
from .routes import router
```

## Third-Party Packages

Install packages from PyPI using pip:

```bash
pip install requests      # HTTP client
pip install flask        # Web framework
pip install fastapi      # API framework
pip install sqlalchemy   # Database ORM
```

### Using Installed Packages

```python
# Using requests for HTTP
import requests

response: requests.Response = requests.get("https://api.example.com/data")
if response.status_code == 200:
    data: dict = response.json()
    print(data)

# Using Flask
from flask import Flask

app = Flask(__name__)

@app.route("/")
def hello() -> str:
    return "Hello, World!"
```

## Summary
- **Functions** encapsulate reusable code with parameters and return values
- Use **type hints** on all function parameters and return types
- **Lambda functions** are anonymous, single-expression functions
- **Modules** are Python files that can be imported
- Use `if __name__ == "__main__"` to run code only when script is executed directly
- Python's **standard library** provides powerful built-in modules
- Use **pip** to install third-party packages

## Next Steps
→ Continue to `03-virtual-environments.md` to learn how to manage project dependencies with virtual environments.
