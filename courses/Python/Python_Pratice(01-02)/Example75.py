# Example75.py
# Topic: Exception Handling — Python 3.11+ Features

# Python 3.11 added new exception features

# === Adding Notes to Exceptions ===
# Use add_note() to add context to exceptions

try:
    result = int("not a number")
except ValueError as e:
    # Add notes with context
    e.add_note("This happened in user input parsing")
    e.add_note("User was trying to convert input on line 42")
    raise

# Note: The above will re-raise the exception

# === Simpler example without re-raising ===
def parse_number(value):
    try:
        return int(value)
    except ValueError as e:
        e.add_note("Input was: " + str(value))
        raise


# This shows the note in traceback in Python 3.11+

# === ExceptionGroup and except* (Python 3.11+) ===

# ExceptionGroup handles multiple exceptions at once

# Example without ExceptionGroup:
def process_items(items):
    results = []
    for item in items:
        try:
            results.append(int(item))
        except ValueError as e:
            results.append(str(e))
    return results


items = ["1", "2", "abc", "4"]
print(process_items(items))

# Note: ExceptionGroup/except* is for handling multiple raised exceptions
# This is more advanced - for now understand the concept

# === Version check example ===
import sys

print("Python version: " + sys.version)

# Check if add_note is available
if hasattr(ValueError, "add_note"):
    print("add_note is available!")
else:
    print("add_note not available (Python < 3.11)")

# === Practical use of notes ===
def validate_age(age):
    if age < 0:
        e = ValueError("Age cannot be negative")
        e.add_note("Field: age")
        e.add_note("Input: " + str(age))
        raise e
    return age


try:
    validate_age(-5)
except ValueError as e:
    print("Validation error: " + str(e))
    if hasattr(e, "add_note"):
        print("Notes: " + str(e.__notes__))
