# Exception Groups (Python 3.11+)

## What You'll Learn

- What ExceptionGroups are
- How to use `except*` to handle multiple exceptions
- Real-world example: collecting multiple validation errors
- When to use exception groups vs traditional handling

## Prerequisites

- Read [02_raising_exceptions.md](./02_raising_exceptions.md) first

## What Are ExceptionGroups?

Python 3.11 introduced **ExceptionGroup** — a way to handle multiple exceptions at once. This is useful when you run multiple tasks and want to collect all their errors.

### The Problem Before ExceptionGroups

```python
# Before: If one task fails, you lose info about others
def process_all(items: list[str]) -> None:
    for item in items:
        # If this fails, loop stops!
        process(item)
```

### The Solution

```python
# Now: Run all tasks and collect all errors
def process_all(items: list[str]) -> None:
    # Use asyncio.gather or similar to run all tasks
    # Then catch ExceptionGroup to handle multiple errors
```

## Using `except*`

The `except*` syntax handles exception groups:

```python
try:
    # Code that might raise multiple exceptions
    raise ExceptionGroup(
        "Multiple errors",
        [
            ValueError("Invalid value"),
            TypeError("Wrong type"),
        ]
    )

except* ValueError as eg:
    print(f"Value errors: {eg.exceptions}")

except* TypeError as eg:
    print(f"Type errors: {eg.exceptions}")
```

### Syntax

```python
try:
    # Code that might raise ExceptionGroup
    ...

except* SpecificException as eg:
    # Handle this type of exception
    for e in eg.exceptions:
        # Handle each exception
        ...
```

## Real-World Example: Validation

A common use case is **collecting multiple validation errors**:

```python
# validation_demo.py
# Demonstrates ExceptionGroup for collecting validation errors

from typing import Any


class ValidationError(Exception):
    """Exception for validation failures."""
    pass


def validate_field(value: Any, field_name: str, validators: list[dict[str, Any]]) -> None:
    """Validate a single field.
    
    Args:
        value: Value to validate
        field_name: Name of the field
        validators: List of validator rules
    
    Raises:
        ValidationError: If validation fails
    """
    errors: list[str] = []
    
    for rule in validators:
        rule_type: str = rule["type"]
        
        if rule_type == "required":
            if not value:
                errors.append(f"{field_name} is required")
        
        elif rule_type == "min_length":
            if len(str(value)) < rule["value"]:
                errors.append(
                    f"{field_name} must be at least {rule['value']} characters"
                )
        
        elif rule_type == "max_length":
            if len(str(value)) > rule["value"]:
                errors.append(
                    f"{field_name} must be at most {rule['value']} characters"
                )
        
        elif rule_type == "pattern":
            import re
            if not re.match(rule["value"], str(value)):
                errors.append(f"{field_name} has invalid format")
    
    if errors:
        raise ValidationError(f"{field_name}: {'; '.join(errors)}")


def validate_form(data: dict[str, Any]) -> dict[str, str]:
    """Validate an entire form, collecting all errors.
    
    Args:
        data: Form data to validate
    
    Returns:
        Dictionary of field_name: error_message
    
    Raises:
        ValidationError: If validation fails
    """
    errors: list[ValidationError] = []
    
    # Define validators for each field
    validators: dict[str, list[dict[str, Any]]] = {
        "username": [
            {"type": "required"},
            {"type": "min_length", "value": 3},
            {"type": "max_length", "value": 20},
        ],
        "email": [
            {"type": "required"},
            {"type": "pattern", "value": r"^[\w.-]+@[\w.-]+\.\w+$"},
        ],
        "age": [
            {"type": "required"},
        ],
    }
    
    # Validate each field
    for field, field_validators in validators.items():
        try:
            validate_field(data.get(field), field, field_validators)
        except ValidationError as e:
            errors.append(e)
    
    # If there are errors, raise as ExceptionGroup
    if errors:
        raise ExceptionGroup("Validation failed", errors)
    
    return {"status": "valid"}


def main() -> None:
    # Test with invalid data
    invalid_data: dict[str, Any] = {
        "username": "ab",      # Too short
        "email": "not-an-email",  # Invalid format
        "age": "",              # Required but empty
    }
    
    print("=== Form Validation ===")
    print(f"Testing data: {invalid_data}\n")
    
    try:
        result: dict[str, str] = validate_form(invalid_data)
        print(f"Result: {result}")
    
    except ExceptionGroup as eg:
        print(f"Found {len(eg.exceptions)} validation errors:\n")
        
        # Handle different types if needed
        # In this case, all are ValidationErrors
        for error in eg.exceptions:
            print(f"  • {error}")
    
    # Test with valid data
    print("\n=== Valid Data ===")
    valid_data: dict[str, Any] = {
        "username": "alice123",
        "email": "alice@example.com",
        "age": 25,
    }
    
    try:
        result = validate_form(valid_data)
        print(f"Result: {result}")
    
    except ExceptionGroup as eg:
        for error in eg.exceptions:
            print(f"Error: {error}")


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
=== Form Validation ===
Testing data: {'username': 'ab', 'email': 'not-an-email', 'age': ''}

Found 3 validation errors:

  • username: username must be at least 3 characters
  • email: email has invalid format
  • age: age is required

=== Valid Data ===
Result: {'status': 'valid'}
```

## When to Use ExceptionGroups

| Use ExceptionGroups When | Use Traditional Try/Except When |
|--------------------------|--------------------------------|
| Running multiple tasks | Single operation that might fail |
| Collecting all errors | Stopping on first error |
| Parallel/concurrent code | Sequential code |

## Annotated Example: Parallel Processing

```python
# parallel_processing.py
# Demonstrates ExceptionGroup in parallel processing context

from concurrent.futures import ThreadPoolExecutor
import time


def process_item(item: int) -> int:
    """Process a single item.
    
    Args:
        item: Number to process
    
    Returns:
        Processed result
    
    Raises:
        ValueError: If item is invalid
        RuntimeError: If item triggers some condition
    """
    # Simulate processing
    time.sleep(0.1)
    
    # Different errors for different items
    if item == 1:
        raise ValueError(f"Invalid value: {item}")
    if item == 3:
        raise RuntimeError(f"Processing failed for {item}")
    
    return item * 2


def process_all(items: list[int]) -> dict[str, Any]:
    """Process multiple items in parallel.
    
    Args:
        items: Items to process
    
    Returns:
        Dictionary with results and errors
    """
    results: list[int] = []
    errors: list[Exception] = []
    
    # Process in parallel
    with ThreadPoolExecutor() as executor:
        # This would normally use asyncio, but for demo:
        for item in items:
            try:
                result: int = process_item(item)
                results.append(result)
            except Exception as e:
                errors.append(e)
    
    return {
        "results": results,
        "errors": errors
    }


def main() -> None:
    items: list[int] = [0, 1, 2, 3, 4]
    
    print("=== Processing Items ===")
    print(f"Items: {items}\n")
    
    outcome: dict[str, Any] = process_all(items)
    
    print(f"Successful results: {outcome['results']}")
    print(f"Number of errors: {len(outcome['errors'])}")
    
    for i, error in enumerate(outcome['errors']):
        print(f"  Error {i+1}: {type(error).__name__}: {error}")


# Run the program
if __name__ == "__main__":
    main()
```

## Common Mistakes

### ❌ Not Handling ExceptionGroup

```python
# WRONG - treating as single exception
try:
    risky_code()
except Exception as e:
    print(e)  # Only shows top-level message!
```

### ❌ Forgetting to Iterate

```python
# WRONG - not accessing .exceptions
try:
    # ...
except* ValueError as eg:
    print(eg)  # Shows the group, not individual errors!

# CORRECT - iterate through exceptions
except* ValueError as eg:
    for e in eg.exceptions:
        print(f"Error: {e}")
```

## Summary

- **ExceptionGroup**: Groups multiple exceptions together
- **`except*`**: Handles specific exception types within a group
- **Use case**: Collecting multiple validation errors or handling parallel task failures
- **`eg.exceptions`**: Access the individual exceptions in the group

## Next Steps

Congratulations on completing the Control Flow section! Now let's move to **[03_Functions/01_Defining_Functions/01_function_basics.md](../03_Functions/01_Defining_Functions/01_function_basics.md)** to learn about functions.
