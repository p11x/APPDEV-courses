# Function Basics

## What You'll Learn

- How to define functions with `def`
- Parameters and return values
- The difference between `print` and `return`
- Default arguments and keyword arguments

## Prerequisites

- Read [03_exception_groups.md](../02_Control_Flow/03_Exception_Handling/03_exception_groups.md) first

## What Is a Function?

A function is a **reusable block of code** that performs a specific task. Instead of writing the same code multiple times, you define it once and call it when needed.

### Function Call Flow

```
┌─────────────────────────────────────────┐
│              CALLING A FUNCTION          │
├─────────────────────────────────────────┤
│                                         │
│   main()                                │
│       │                                 │
│       ▼                                 │
│   greet("Alice")  ◄─── Call function   │
│       │                                 │
│       ▼                                 │
│   ┌─────────────────────┐              │
│   │  Function executes  │              │
│   │  with "Alice"       │              │
│   └─────────────────────┘              │
│       │                                 │
│       ▼                                 │
│   Returns "Hello, Alice!"              │
│       │                                 │
│       ▼                                 │
│   Print the result                     │
│                                         │
└─────────────────────────────────────────┘
```

## Defining Functions

```python
# Simple function
def greet() -> None:
    print("Hello, World!")


# Call the function
greet()  # Output: Hello, World!
```

### Syntax

```python
def function_name(parameters) -> return_type:
    """Docstring - description of what function does"""
    # Function body
    # ...
    return value  # Optional
```

## Parameters and Arguments

### Parameters

Parameters are variables in the function definition:

```python
# Parameter: name
def greet(name: str) -> None:
    print(f"Hello, {name}!")
```

### Arguments

Arguments are the actual values passed when calling:

```python
# "Alice" is the argument
greet("Alice")  # Output: Hello, Alice!
```

## Return Values

### The Return Statement

The `return` statement sends a value back to the caller:

```python
def add(a: int, b: int) -> int:
    return a + b


# Use the returned value
result: int = add(5, 3)
print(result)  # 8
```

### Return Without Value

```python
def greet(name: str) -> None:
    if not name:
        return  # Exit early, returns None
    
    print(f"Hello, {name}!")
```

## Print vs Return

This is a common source of confusion:

### ❌ Using Print (Wrong)

```python
def add_with_print(a: int, b: int) -> None:
    print(a + b)  # Prints but doesn't return!


result: None = add_with_print(5, 3)
print(result)  # None - we lost the answer!
```

### ✅ Using Return (Correct)

```python
def add_with_return(a: int, b: int) -> int:
    return a + b


result: int = add_with_return(5, 3)
print(result)  # 8 - we got the answer!
```

## Default Arguments

Provide default values for parameters:

```python
def greet(name: str = "World") -> None:
    print(f"Hello, {name}!")


greet()           # Hello, World!
greet("Alice")    # Hello, Alice!
```

### Important Rules

```python
# ❌ WRONG - default after non-default
def invalid(a: int = 1, b: int) -> None:
    pass

# ✅ CORRECT - defaults come after non-defaults
def valid(a: int, b: int = 2) -> None:
    pass
```

## Keyword Arguments

Use argument names to specify which parameter:

```python
def greet(greeting: str, name: str) -> None:
    print(f"{greeting}, {name}!")


# Positional arguments
greet("Hello", "Alice")  # Hello, Alice!

# Keyword arguments (order doesn't matter!)
greet(name="Bob", greeting="Hi")  # Hi, Bob!
```

### Mixing Positional and Keyword

```python
def create_user(name: str, age: int, city: str = "NYC") -> None:
    print(f"{name}, {age}, {city}")


# Valid combinations
create_user("Alice", 25)  # Positional + default
create_user("Bob", 30, "LA")  # Positional + explicit default
create_user("Charlie", age=35)  # Keyword for non-default
create_user("Diana", city="SF", age=28)  # All keywords (any order)
```

## Annotated Example: Calculator

```python
# calculator.py
# Demonstrates function fundamentals

def add(a: float, b: float) -> float:
    """Add two numbers.
    
    Args:
        a: First number
        b: Second number
    
    Returns:
        Sum of a and b
    """
    return a + b


def subtract(a: float, b: float) -> float:
    """Subtract b from a."""
    return a - b


def multiply(a: float, b: float) -> float:
    """Multiply two numbers."""
    return a * b


def divide(a: float, b: float) -> float | None:
    """Divide a by b.
    
    Args:
        a: Numerator
        b: Denominator
    
    Returns:
        Result of division, or None if division by zero
    """
    if b == 0:
        print("Cannot divide by zero!")
        return None
    return a / b


def calculate(a: float, b: float, operation: str = "add") -> float | None:
    """Perform a calculation on two numbers.
    
    Args:
        a: First number
        b: Second number
        operation: One of "add", "subtract", "multiply", "divide"
    
    Returns:
        Result of the operation
    """
    match operation:
        case "add":
            return add(a, b)
        case "subtract":
            return subtract(a, b)
        case "multiply":
            return multiply(a, b)
        case "divide":
            return divide(a, b)
        case _:
            print(f"Unknown operation: {operation}")
            return None


def main() -> None:
    # Using basic functions
    print("=== Basic Operations ===")
    sum_result: float = add(10, 5)
    print(f"10 + 5 = {sum_result}")
    
    print(f"10 - 5 = {subtract(10, 5)}")
    print(f"10 * 5 = {multiply(10, 5)}")
    print(f"10 / 5 = {divide(10, 5)}")
    
    # Using default argument
    print("\n=== Default Arguments ===")
    print(divide(10, 2))
    print(divide(10, 0))  # Handles division by zero
    
    # Using keyword arguments
    print("\n=== Keyword Arguments ===")
    result: float | None = calculate(
        a=100,
        b=20,
        operation="divide"
    )
    print(f"100 / 20 = {result}")
    
    # Using the operation parameter
    print("\n=== Different Operations ===")
    print(f"Add: {calculate(10, 5, 'add')}")
    print(f"Subtract: {calculate(10, 5, 'subtract')}")
    print(f"Multiply: {calculate(10, 5, 'multiply')}")
    print(f"Divide: {calculate(10, 5, 'divide')}")
    print(f"Unknown: {calculate(10, 5, 'mod')}")


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
=== Basic Operations ===
10 + 5 = 15.0
10 - 5 = 5.0
10 * 5 = 50.0
10 / 5 = 2.0

=== Default Arguments ===
Cannot divide by zero!
None
2.0

=== Keyword Arguments ===
100 / 20 = 5.0

=== Different Operations ===
Add: 15.0
Subtract: 5.0
Multiply: 50.0
Divide: 2.0
Unknown: None
```

## Summary

- **`def`**: Defines a function
- **Parameters**: Variables in the function definition
- **Arguments**: Values passed when calling
- **`return`**: Sends a value back (vs `print` which displays)
- **Default arguments**: `def f(x=5)`
- **Keyword arguments**: `f(name="Alice")`

## Next Steps

Now let's learn about **`*args` and `**kwargs`** in **[02_args_and_kwargs.md](./02_args_and_kwargs.md)**.
