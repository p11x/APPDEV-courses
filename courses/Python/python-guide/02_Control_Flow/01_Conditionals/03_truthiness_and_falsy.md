# Truthiness and Falsiness

## What You'll Learn

- What values Python treats as False (falsy)
- What values Python treats as True (truthy)
- Why `if my_list:` is better than `if len(my_list) > 0:`
- Common pitfalls and best practices

## Prerequisites

- Read [02_match_statements.md](./02_match_statements.md) first

## What Is Truthiness?

In Python, every value can be evaluated as either **truthy** or **falsy** in a boolean context (like an `if` statement). You don't always need to explicitly compare to `True` or `False`.

## Falsy Values

These values are treated as `False` in conditions:

| Value | Type | Description |
|-------|------|-------------|
| `False` | bool | The boolean False |
| `None` | NoneType | The absence of a value |
| `0` | int | Zero |
| `0.0` | float | Zero float |
| `""` | str | Empty string |
| `[]` | list | Empty list |
| `()` | tuple | Empty tuple |
| `{}` | dict | Empty dictionary |
| `set()` | set | Empty set |
| `range(0)` | range | Empty range |

## Truthy Values

Everything else is truthy! These are treated as `True`:

```python
# These are all truthy
True
1
-1
0.1
"hello"
["item"]
(1, 2)
{"key": "value"}
{1, 2, 3}
range(1)
```

## Examples

### Checking Empty Collections

```python
# Instead of this:
my_list: list[int] = [1, 2, 3]
if len(my_list) > 0:
    print("List has items")

# Do this (Pythonic!):
my_list: list[int] = [1, 2, 3]
if my_list:
    print("List has items")

# Same for other collections
my_dict: dict[str, int] = {"a": 1}
if my_dict:
    print("Dict has items")

my_str: str = "hello"
if my_str:
    print("String is not empty")
```

### Checking None

```python
# Check if value is not None
value: int | None = 5

if value is not None:
    print(f"Value is {value}")

# Or use the fact that None is falsy
value: int | None = 5
if value:
    print(f"Value is truthy: {value}")
```

### Numeric Examples

```python
# Zero is falsy
if 0:
    print("This won't print")

# Any non-zero number is truthy
if -1:
    print("This will print (negative is still truthy!)")

if 42:
    print("This will print")
```

## The bool() Function

You can convert any value to boolean explicitly:

```python
# Explicit conversion
result: bool = bool(0)       # False
result: bool = bool(1)      # True
result: bool = bool([])    # False
result: bool = bool([1])    # True
result: bool = bool("")     # False
result: bool = bool("hi")   # True
result: bool = bool(None)   # False
```

## Common Patterns

### Default Value Pattern

```python
# Without truthiness
name: str | None = None
if name is None:
    name = "Guest"
print(f"Hello, {name}")

# With truthiness (simpler!)
name: str | None = None
name = name or "Guest"  # If name is falsy, use "Guest"
print(f"Hello, {name}")
```

### Short-Circuit with `and`

```python
# Only print if list is not empty
my_list: list[str] = ["item"]
if my_list:
    print(my_list[0])

# Using and (short-circuit)
my_list: list[str] = ["item"]
my_list and print(my_list[0])
```

### Default Dict Value

```python
# Get value or default
config: dict[str, str] = {"theme": "dark"}
theme: str = config.get("theme", "light")  # Returns "dark"

# Without get (using truthiness)
theme: str = config["theme"] if "theme" in config else "light"
```

## Annotated Example: Processing User Input

```python
# truthiness_demo.py
# Demonstrates truthiness in real scenarios

from typing import Any


def process_input(user_input: str | None) -> str:
    """Process user input with truthiness checks.
    
    Args:
        user_input: User's input string or None
    
    Returns:
        Processed result
    """
    # Check if input exists (None is falsy)
    # This is cleaner than: if user_input is not None
    if user_input:
        # Strip whitespace and convert to lowercase
        cleaned: str = user_input.strip().lower()
        return f"Processing: '{cleaned}'"
    else:
        # None or empty string
        return "No input provided"


def check_items(items: list[Any]) -> None:
    """Check if items list has content."""
    # ❌ WRONG - verbose way
    if len(items) > 0:
        print(f"Items: {items}")
    
    # ✅ CORRECT - Pythonic way (truthiness)
    if items:
        print(f"Items: {items}")
    else:
        print("No items")


def validate_number(number: int | None) -> str:
    """Validate that a number is provided and non-zero."""
    # Check if number was provided
    if not number:
        return "No number provided or number is zero"
    
    # If we get here, number is truthy (non-zero)
    return f"Valid number: {number}"


def main() -> None:
    # Test process_input
    print("=== Input Processing ===")
    result1: str = process_input("  Hello World  ")
    result2: str = process_input("")
    result3: str = process_input(None)
    
    print(result1)
    print(result2)
    print(result3)
    
    # Test check_items
    print("\n=== Item Checking ===")
    check_items([1, 2, 3])
    check_items([])
    
    # Test validate_number
    print("\n=== Number Validation ===")
    print(validate_number(42))
    print(validate_number(0))
    print(validate_number(None))
    
    # Truthiness table
    print("\n=== Truthiness Examples ===")
    values: list[Any] = [
        True, False,        # Booleans
        1, 0, -1,          # Integers
        1.0, 0.0,          # Floats
        "hi", "",          # Strings
        [1, 2], [],        # Lists
        {"a": 1}, {},      # Dictionaries
        None,              # None
    ]
    
    for val in values:
        # Show value and its boolean interpretation
        print(f"{repr(val):15} → {bool(val)}")


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
=== Input Processing ===
Processing: 'hello world'
No input provided
No input provided

=== Item Checking ===
Items: [1, 2, 3]
No items

=== Number Validation ===
Valid number: 42
No number provided or number is zero
No number provided or number is zero

=== Truthiness Examples ===
True             → True
False            → False
1                → True
0                → False
-1               → True
1.0              → True
0.0              → False
'hi'             → True
''               → False
[1, 2]           → True
[]               → False
{'a': 1}         → True
{}               → False
None             → False
```

## Common Mistakes

### ❌ Checking for Empty String Incorrectly

```python
# WRONG - empty string is falsy, but so is None
name: str = ""
if name == "":
    print("No name")  # Works but verbose

# CORRECT - just use truthiness
name: str = ""
if not name:
    print("No name")  # Simpler!
```

### ❌ Confusing `False` and `None`

```python
# Be careful - 0 and False are both falsy
value: int = 0
if value:  # False, but value could be 0 legitimately!
    print("Value is truthy")
else:
    print("Value is falsy (could be 0 or None)")

# Better to check explicitly
value: int | None = None
if value is None:
    print("Value is None")
elif value == 0:
    print("Value is zero")
else:
    print(f"Value is {value}")
```

## Summary

- **Falsy values**: `0`, `""`, `[]`, `{}`, `()`, `set()`, `None`, `False`, `range(0)`
- **Truthy values**: Everything else (non-zero, non-empty, non-None)
- **Use truthiness**: `if my_list:` instead of `if len(my_list) > 0:`
- **Be explicit** when you need to distinguish between `0` and `None`

## Next Steps

Now let's learn about **for loops** in **[02_Control_Flow/02_Loops/01_for_loops.md](../02_Loops/01_for_loops.md)**.
