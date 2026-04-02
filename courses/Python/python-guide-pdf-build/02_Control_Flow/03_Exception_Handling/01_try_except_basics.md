# Try and Except Basics

## What You'll Learn

- How to use `try`/`except` to handle errors
- Catching specific exceptions vs bare except
- The `else` and `finally` blocks
- Exception hierarchy

## Prerequisites

- Read [03_comprehensions.md](../02_Loops/03_comprehensions.md) first

## What Are Exceptions?

**Exceptions** are errors that occur during program execution. When something goes wrong, Python raises an exception:

```python
# This causes a ZeroDivisionError
result: int = 10 / 0  # Error!
```

Without handling, the program crashes. With `try`/`except`, we can handle errors gracefully.

## Basic Try/Except

```python
# Handle the error
try:
    result: float = 10 / 0
except ZeroDivisionError:
    print("Can't divide by zero!")
```

### Syntax

```python
try:
    # Code that might cause an error
    risky_code()
except ExceptionType:
    # Code that runs if error occurs
    handle_error()
```

## Catching Specific Exceptions

Always catch specific exceptions when possible:

```python
# ❌ WRONG - bare except catches everything
try:
    result = 10 / 0
except:
    print("Something went wrong")

# ✅ CORRECT - catch specific exception
try:
    result = 10 / 0
except ZeroDivisionError:
    print("Can't divide by zero!")
```

### Common Exceptions

| Exception | When It Occurs |
|-----------|---------------|
| `ZeroDivisionError` | Dividing by zero |
| `ValueError` | Wrong type value (int("abc")) |
| `TypeError` | Wrong operation type ("abc" + 5) |
| `IndexError` | List index out of range |
| `KeyError` | Dictionary key not found |
| `FileNotFoundError` | File doesn't exist |
| `NameError` | Variable not defined |

## Multiple Except Blocks

```python
try:
    value: int = int(input("Enter a number: "))
    result: float = 10 / value
except ValueError:
    print("That's not a valid number!")
except ZeroDivisionError:
    print("You can't divide by zero!")
```

## The Else Block

The `else` block runs only if **no exception** occurred:

```python
try:
    number: int = int(input("Enter a number: "))
    result: int = 10 / number
except ValueError:
    print("Invalid input")
except ZeroDivisionError:
    print("Can't divide by zero")
else:
    # Only runs if no exception
    print(f"Result: {result}")
```

## The Finally Block

The `finally` block **always runs**, whether there was an exception or not:

```python
try:
    file = open("data.txt", "r")
    content: str = file.read()
except FileNotFoundError:
    print("File not found")
finally:
    # This ALWAYS runs - even if there's an error!
    print("Cleaning up...")
    # file.close() would go here
```

### When to Use Finally

- **Closing files**
- **Releasing resources**
- **Cleaning up** regardless of success or failure

## Exception Hierarchy

Exceptions form a hierarchy:

```
BaseException
├── Exception
│   ├── ZeroDivisionError
│   ├── ValueError
│   ├── TypeError
│   ├── LookupError
│   │   ├── IndexError
│   │   └── KeyError
│   └── OSError
│       └── FileNotFoundError
```

Catching a parent exception also catches children:

```python
# This catches FileNotFoundError AND all other OSError subclasses
try:
    file = open("missing.txt")
except OSError:
    print("File error!")
```

## Annotated Example: Safe Calculator

```python
# safe_calculator.py
# Demonstrates exception handling

def safe_divide(a: float, b: float) -> str:
    """Divide two numbers safely.
    
    Args:
        a: Numerator
        b: Denominator
    
    Returns:
        Result or error message
    """
    try:
        # Try to perform division
        result: float = a / b
        return f"Result: {result}"
    
    except ZeroDivisionError:
        # Handle specific error
        return "Error: Cannot divide by zero"
    
    except TypeError:
        # Handle type errors (e.g., None + 1)
        return "Error: Invalid types for division"
    
    # Optional: else block runs if no exception
    else:
        print("Division successful!")
    
    # Optional: finally runs always
    finally:
        print("Division operation attempted")


def get_integer(prompt: str) -> int:
    """Get an integer from user with validation.
    
    Args:
        prompt: What to ask the user
    
    Returns:
        The valid integer
    """
    while True:
        try:
            # Try to convert input to integer
            value: int = int(input(prompt))
            return value
        
        except ValueError:
            # Input wasn't a valid integer
            print("Please enter a valid integer.")
        
        # No finally needed here - loop will retry


def access_list_safely(items: list[str], index: int) -> str:
    """Access list item with error handling.
    
    Args:
        items: List of strings
        index: Index to access
    
    Returns:
        Item at index or error message
    """
    try:
        return f"Item: {items[index]}"
    
    except IndexError:
        # Index out of bounds
        return f"Error: Index {index} out of range (0-{len(items)-1})"
    
    except TypeError:
        # Index wasn't an integer
        return "Error: Index must be an integer"


def main() -> None:
    print("=== Safe Calculator ===")
    print(safe_divide(10, 2))
    print(safe_divide(10, 0))
    print(safe_divide("10", 2))
    
    print("\n=== Safe List Access ===")
    fruits: list[str] = ["apple", "banana", "cherry"]
    print(access_list_safely(fruits, 1))
    print(access_list_safely(fruits, 10))
    print(access_list_safely(fruits, "one"))
    
    print("\n=== Get Integer ===")
    age: int = get_integer("Enter your age: ")
    print(f"You are {age} years old")


# Run the program
if __name__ == "__main__":
    main()
```

### Sample Output

```
=== Safe Calculator ===
Result: 5.0
Error: Cannot divide by zero
Error: Invalid types for division
Division operation attempted
Division operation attempted
Division operation attempted

=== Safe List Access ===
Item: banana
Error: Index 10 out of range (0-2)
Error: Index must be an integer

=== Get Integer ===
Enter your age: twenty
Please enter a valid integer.
Enter your age: 25
You are 25 years old
```

## Common Mistakes

### ❌ Using Bare Except

```python
# WRONG - catches everything including KeyboardInterrupt
try:
    risky_code()
except:
    handle_error()

# CORRECT - catch specific exceptions
try:
    risky_code()
except ValueError:
    handle_value_error()
except ZeroDivisionError:
    handle_zero_division()
```

### ❌ Swallowing Exceptions

```python
# WRONG - silently ignoring errors
try:
    do_something()
except Exception:
    pass  # Bad! We don't know what went wrong

# CORRECT - at least log the error
try:
    do_something()
except Exception as e:
    print(f"Error: {e}")
```

### ❌ Not Using Finally for Cleanup

```python
# WRONG - file might not close if error occurs
file = open("data.txt")
process(file)
file.close()  # Never runs if process() fails!

# CORRECT - use finally
try:
    file = open("data.txt")
    process(file)
finally:
    file.close()  # Always runs!
```

## Summary

- **`try`/`except`**: Handle errors gracefully
- **Catch specific exceptions**: `except ZeroDivisionError:`
- **Multiple except blocks**: Handle different errors differently
- **`else`**: Runs if no exception
- **`finally`**: Always runs (for cleanup)
- **Avoid bare `except`**: Always catch specific exceptions

## Next Steps

Now let's learn about **raising exceptions** in **[02_raising_exceptions.md](./02_raising_exceptions.md)**.
