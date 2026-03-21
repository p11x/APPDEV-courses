# Example70.py
# Topic: Exception Handling — Exception Hierarchy

# Exceptions form a hierarchy
# Catching a parent catches all children

# === Hierarchy ===
# BaseException
# └── Exception
#     ├── ZeroDivisionError
#     ├── ValueError
#     ├── TypeError
#     ├── LookupError
#     │   ├── IndexError
#     │   └── KeyError
#     └── OSError
#         └── FileNotFoundError

# === Catching parent catches children ===

# This catches ZeroDivisionError
try:
    result = 10 / 0
except ArithmeticError:
    print("Caught arithmetic error")

# This catches FileNotFoundError
# try:
#     f = open("missing.txt")
# except OSError:
#     print("Caught OS error")

# === LookupError catches both IndexError and KeyError ===
items = [1, 2, 3]

try:
    value = items[10]
except LookupError:
    print("Caught lookup error (IndexError is child of LookupError)")

data = {"key": "value"}
try:
    value = data["missing"]
except LookupError:
    print("Caught lookup error (KeyError is child of LookupError)")

# === Catching Exception catches everything ===
try:
    result = 10 / 0
except Exception:
    print("Caught by Exception")

try:
    x = int("hello")
except Exception:
    print("Caught by Exception")

# === Specific to general - order matters! ===
def handle_error(value):
    try:
        result = int(value) / 0
    except ZeroDivisionError:
        return "Specific: Zero division"
    except ArithmeticError:
        return "General: Math error"
    except Exception:
        return "General: Something else"

# ZeroDivisionError is caught first

# === Using hierarchy wisely ===

# Good: catch specific
try:
    result = 10 / 0
except ZeroDivisionError:
    print("Specific handling")

# Good: catch group
try:
    # Some lookup operation
    data = {}
    value = data["missing"]
except LookupError:
    print("Handles both IndexError and KeyError")

# === Example: catching file errors ===
# This catches FileNotFoundError AND other OS errors
# try:
#     f = open("file.txt")
# except OSError:
#     print("OS error occurred")

# === Custom exceptions (inherit from Exception) ===
class MyError(Exception):
    pass

try:
    raise MyError("Custom error!")
except MyError:
    print("Caught custom error")

# Also caught by Exception
try:
    raise MyError("Custom error!")
except Exception:
    print("Caught by Exception")
