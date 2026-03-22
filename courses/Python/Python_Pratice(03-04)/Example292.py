# Example292: Error Handling Best Practices
def divide(a, b):
    try:
        result = a / b
    except ZeroDivisionError:
        return "Cannot divide by zero"
    except TypeError:
        return "Invalid types"
    else:
        print("Division successful")
        return result
    finally:
        print("Division attempt completed")

print("Error Handling:")
print(divide(10, 2))
print(divide(10, 0))

# Custom exceptions
class ValidationError(Exception):
    def __init__(self, field, message):
        self.field = field
        self.message = message
        super().__init__(f"{field}: {message}")

def validate_age(age):
    if age < 0:
        raise ValidationError("age", "Must be positive")
    if age > 150:
        raise ValidationError("age", "Must be reasonable")
    return "Valid"

print("\nCustom Exception:")
try:
    validate_age(-5)
except ValidationError as e:
    print(f"Error: {e.field} - {e.message}")

# Exception chaining
def inner():
    raise ValueError("Inner error")

def outer():
    try:
        inner()
    except ValueError as e:
        raise RuntimeError("Outer error") from e

print("\nException Chaining:")
try:
    outer()
except RuntimeError as e:
    print(f"Caught: {e}")
    print(f"Original: {e.__cause__}")

# Context manager for exception handling
from contextlib import contextmanager

@contextmanager
def handle_errors(func):
    try:
        yield func()
    except Exception as e:
        print(f"Handled: {type(e).__name__}")
        return None

print("\nContext Manager for Errors:")
with handle_errors(lambda: 10/2) as result:
    print(f"Result: {result}")
