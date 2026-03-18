# Variables Explained

## What You'll Learn

- What a variable is (the "box" analogy)
- How to name variables correctly (snake_case)
- Python 3.12+ type hints on variables
- The walrus operator (`:=`)
- How to annotate every line of code

## Prerequisites

- Read [03_your_first_script.md](../../01_Getting_Started/03_your_first_script.md) first

## What Is a Variable?

Think of a variable as a **labeled box** that stores information:

```
┌─────────────────────────────────────────────────────────────────┐
│                      VARIABLE AS A BOX                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   Variable Name          Contents (Value)                       │
│        │                        │                              │
│        ▼                        ▼                              │
│   ┌─────────────┐         ┌─────────────┐                     │
│   │    name     │  ─────►  │  "Alice"    │                     │
│   │  (label)    │         │  (string)   │                     │
│   └─────────────┘         └─────────────┘                     │
│                                                                 │
│   ┌─────────────┐         ┌─────────────┐                     │
│   │    age      │  ─────►  │     25     │                     │
│   │  (label)    │         │  (integer)  │                     │
│   └─────────────┘         └─────────────┘                     │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Creating Variables

```python
# Create a variable called 'name' with the value "Alice"
name: str = "Alice"

# Create a variable called 'age' with the value 25
age: int = 25

# Create a variable called 'height' with the value 5.6
height: float = 5.6

# Create a variable called 'is_student' with the value True
is_student: bool = True
```

### Reading Variables

```python
# Print the value stored in 'name'
print(name)  # Output: Alice

# Use in calculations
next_year_age: int = age + 1  # 26
```

### Changing Variables

```python
age: int = 25       # Initially 25
print(age)          # Output: 25

age: int = 26       # Now 26
print(age)          # Output: 26
```

## Variable Naming Rules

### ✅ Good Variable Names

| Good Name | Why It's Good |
|-----------|---------------|
| `name` | Short and clear |
| `user_name` | Describes the content |
| `age_in_years` | Descriptive |
| `total_price` | Clear purpose |
| `is_valid` | Boolean (yes/no) meaning |

### ❌ Bad Variable Names

| Bad Name | Problem |
|----------|---------|
| `n` | Too short, unclear |
| `x1, x2` | Not descriptive |
| `NAME` | Capital letters hard to read |
| `my-variable` | Hyphens aren't allowed |
| `123abc` | Can't start with numbers |
| `class` | `class` is a Python keyword |

### Keywords You Cannot Use

These words are reserved by Python and cannot be variable names:

```
and, as, assert, async, await, break, class, continue, def, del,
elif, else, except, False, finally, for, from, global, if, import,
in, is, lambda, None, nonlocal, not, or, pass, raise, return,
True, try, while, with, yield
```

## Python Naming Convention: snake_case

Python uses **snake_case** for variable names — all lowercase with underscores:

```python
# ✅ Good - snake_case
user_name = "Alice"
age_in_years = 25
total_price = 99.99
is_active = True

# ❌ Avoid - other styles
userName = "Alice"    # camelCase (used in JavaScript/Java)
UserName = "Alice"    # PascalCase (used for classes)
USER_NAME = "Alice"   # SCREAMING_SNAKE_CASE (used for constants)
```

## Type Hints (Python 3.12+)

Type hints tell readers (and tools) what **type** of data a variable should hold:

```python
# Without type hint - Python figures it out automatically
name = "Alice"      # Python knows this is a string

# With type hint - we explicitly say it's a string
name: str = "Alice" # Same thing, but we told Python it's a str
```

### Why Use Type Hints?

1. **Readability** — Others know what to expect
2. **IDE support** — Better autocomplete and error detection
3. **Documentation** — Acts as inline documentation
4. **Catches bugs** — Tools like mypy can find errors before runtime

### Type Hint Syntax

```python
# Variable name followed by colon, then type, then equals value
variable_name: Type = value

# Examples
count: int = 0
price: float = 19.99
message: str = "Hello"
is_enabled: bool = True
items: list = []          # A list (more on this later)
```

## The Walrus Operator (`:=`) — Python 3.8+

The **walrus operator** (`:=`) assigns and returns a value in one expression. It's named after the walrus (`:=` looks like walrus eyes and tusks).

### Without Walrus Operator

```python
# Calculate a value, then use it
result: int = len("hello")  # First assign
if result > 3:              # Then use
    print(f"Length is {result}")
```

### With Walrus Operator

```python
# Assign and use in the same expression
if (result := len("hello")) > 3:
    print(f"Length is {result}")
```

### A More Practical Example

```python
# Without walrus - calculate twice or use extra variable
name: str = input("Enter name: ")
if name:
    print(f"Hello, {name}")
    
# With walrus - assign and check in one line
if (name := input("Enter name: ")):
    print(f"Hello, {name}")
```

### Walrus in Loops

The walrus operator is especially useful in loops:

```python
# Read input until user types "quit"
while (command := input("Enter command (or 'quit' to exit): ")) != "quit":
    print(f"Processing: {command}")
```

This is cleaner than:
```python
command: str = ""
while command != "quit":
    command = input("Enter command (or 'quit' to exit): ")
    if command != "quit":
        print(f"Processing: {command}")
```

## Annotated Example: Complete Program

Here's a fully annotated program demonstrating variables:

```python
# variables_demo.py
# Demonstrates variables, type hints, and walrus operator

def main() -> None:
    # String variable - holds text
    user_name: str = "Alice"
    
    # Integer variable - holds whole numbers
    user_age: int = 25
    
    # Float variable - holds decimal numbers
    account_balance: float = 1250.50
    
    # Boolean variable - holds True or False
    is_premium_member: bool = True
    
    # Print all variables using f-string
    print(f"User: {user_name}")
    print(f"Age: {user_age}")
    print(f"Balance: ${account_balance}")
    print(f"Premium: {is_premium_member}")
    
    # Demonstrate walrus operator - calculate and store in one expression
    # This calculates len(user_name) and stores it in 'name_length' at the same time
    name_length: int = (length := len(user_name))
    print(f"Name has {length} characters")
    
    # Another walrus example - in a conditional
    if (first_name := input("Enter first name: ")):
        print(f"Hello, {first_name}!")
    
    # Variables can be updated
    user_age = user_age + 1  # Birthday! Now user_age is 26
    print(f"Happy birthday! You are now {user_age}")


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
User: Alice
Age: 25
Balance: $1250.5
Premium: True
Name has 5 characters
Enter first name: Bob
Hello, Bob!
Happy birthday! You are now 26
```

## Summary

- **Variables** are labeled containers that store data
- **Create** variables with `name = value`
- **Naming**: Use `snake_case` (lowercase with underscores)
- **Type hints** (`: Type`) make code clearer: `age: int = 25`
- **Walrus operator** (`:=`) assigns and returns in one expression
- **Variables can change** — just assign a new value

## Next Steps

Now let's explore different **data types** in **[02_data_types.md](./02_data_types.md)** — integers, floats, strings, booleans, and more.
