# Example72.py
# Topic: Exception Handling — Raising Exceptions

# You can raise exceptions manually to indicate errors

# === Basic raise syntax ===
# raise ExceptionType("Error message")

# === Raising built-in exceptions ===

# Raise ValueError
try:
    raise ValueError("Invalid value!")
except ValueError as e:
    print("Caught: " + str(e))

# Raise TypeError
try:
    raise TypeError("Expected string, got int")
except TypeError as e:
    print("Caught: " + str(e))

# Raise KeyError
try:
    raise KeyError("Name not found")
except KeyError as e:
    print("Caught: " + str(e))

# Raise RuntimeError
try:
    raise RuntimeError("Something went wrong")
except RuntimeError as e:
    print("Caught: " + str(e))

# === Raising in functions ===
def divide(a, b):
    if b == 0:
        raise ValueError("Cannot divide by zero!")
    return a / b

try:
    result = divide(10, 0)
except ValueError as e:
    print("Error: " + str(e))

# === Raising with conditions ===
def set_age(age):
    if age < 0:
        raise ValueError("Age cannot be negative!")
    if age > 150:
        raise ValueError("Age is unrealistic!")
    print("Age set to: " + str(age))

try:
    set_age(-5)
except ValueError as e:
    print("Error: " + str(e))

try:
    set_age(200)
except ValueError as e:
    print("Error: " + str(e))

set_age(25)  # This works

# === Raising in validation ===
def validate_positive(number):
    if number <= 0:
        raise ValueError("Number must be positive!")
    return number

try:
    validate_positive(-1)
except ValueError as e:
    print("Validation failed: " + str(e))

validate_positive(10)  # Works
