# Example139.py
# Topic: Contextlib Module — suppress Function

# The suppress function is a context manager that suppresses specified exceptions
# It ignores any exception of the given type(s) that occurs in the with block

from contextlib import suppress

# Basic suppress usage
with suppress(ValueError):
    raise ValueError("This is suppressed")

print("Continued after suppressing ValueError")

# Suppress multiple exception types
with suppress(ValueError, TypeError, ZeroDivisionError):
    x = 1 / 0

print("Continued after suppressing ZeroDivisionError")

# Suppress FileNotFoundError
with suppress(FileNotFoundError):
    f = open("nonexistent.txt", "r")

print("Continued after suppressing FileNotFoundError")

# Using suppress for optional operations
data = {"name": "Alice", "age": 30}

# Try to delete optional key
with suppress(KeyError):
    del data["missing_key"]

print(data)

# Using suppress in a loop
results = []
errors = []

items = [1, 2, "three", 4, "five", 6]

for item in items:
    with suppress(ValueError, TypeError):
        result = item * 2
        results.append(result)

print(results)

# Using suppress for cleanup that might fail
class Resource:
    def close(self):
        print("Closing resource")

resource = Resource()

# close() might fail but we don't care
with suppress(Exception):
    resource.close()

print("Done")

# Using suppress with conditional logic
def safe_divide(a, b):
    with suppress(ZeroDivisionError):
        return a / b
    return None

print(safe_divide(10, 2))
print(safe_divide(10, 0))

# Using suppress for attribute deletion
class Obj:
    def __init__(self):
        self.value = 100

obj = Obj()
print(obj.value)

with suppress(AttributeError):
    del obj.value

print("value" in dir(obj))

# Using suppress in async code (conceptual example)
# In async code, suppress can handle CancelledError

# Using suppress vs try/except
# This is equivalent:
try:
    raise ValueError("Test")
except ValueError:
    pass

print("---")

with suppress(ValueError):
    raise ValueError("Test")

print("Both work the same way")

# Using suppress with context manager stack
from contextlib import ExitStack

# With ExitStack, you can manage multiple context managers
with ExitStack() as stack:
    # Push context managers onto the stack
    stack.push(suppress(FileNotFoundError))
    
    # This won't fail even if file doesn't exist
    try:
        f = open("nonexistent.txt", "r")
    except FileNotFoundError:
        print("File not found (handled)")
    
    print("Continuing...")

# Using suppress for graceful degradation
def get_config(key, default=None):
    with suppress(KeyError, AttributeError):
        config = {"host": "localhost", "port": 8080}
        return config[key]
    return default

print(get_config("host"))
print(get_config("missing", "default"))

# Using suppress to ignore specific errors in data processing
import os

files = ["file1.txt", "file2.txt", "file3.txt"]

# Skip files that don't exist
for f in files:
    with suppress(FileNotFoundError):
        size = os.path.getsize(f)
        print(f + ": " + str(size) + " bytes")

print("Processed all available files")

# Using suppress in testing
def test_function():
    # Suppress expected exceptions during test
    with suppress(ZeroDivisionError):
        result = 1 / 0
    
    # Continue with assertions
    return True

result = test_function()
print("Test passed: " + str(result))

print("All examples done!")
