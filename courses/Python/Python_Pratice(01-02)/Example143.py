# Example143.py
# Topic: Multiple Context Managers — Common Mistakes

import os

# Common Mistake 1: Wrong order of context managers
# The last one entered is the first one exited (LIFO)

class Resource:
    def __init__(self, name):
        self.name = name
    def __enter__(self):
        print("Enter: " + self.name)
        return self
    def __exit__(self, *args):
        print("Exit: " + self.name)

print("=== Wrong order - should be reversed ===")
# WRONG: Assuming left-to-right exit
# with Resource("A"), Resource("B"), Resource("C"):
#     pass

# CORRECT: Remember LIFO order
with Resource("First"), Resource("Second"), Resource("Third"):
    print("Inside")

print("=== Correct: Third exits first ===")

# Common Mistake 2: Forgetting that files must exist for comma syntax
print("\n=== Mistake 2: File not found ===")

# This will fail if file doesn't exist
try:
    with open("nonexistent.txt", "r") as f1, open("another.txt", "r") as f2:
        pass
except FileNotFoundError:
    print("Error: File not found!")

# Fix: Check or create files first
open("test_file.txt", "w").write("test")
with open("test_file.txt", "r") as f:
    print("File exists: " + f.read())
os.unlink("test_file.txt")

# Common Mistake 3: Mixing different context manager types incorrectly
print("\n=== Mistake 3: Mixing types incorrectly ===")

from contextlib import contextmanager

@contextmanager
def my_context(name):
    print("Enter: " + name)
    yield
    print("Exit: " + name)

# This works but can be confusing
with my_context("context"), open("Python_Pratice/test.txt", "r") as f:
    print("Mixed: " + f.read())

# Common Mistake 4: Not handling exceptions in nested contexts
print("\n=== Mistake 4: Exception in nested context ===")

class SafeResource:
    def __init__(self, name):
        self.name = name
    def __enter__(self):
        print("Enter: " + self.name)
        return self
    def __exit__(self, exc_type, exc_val, exc_tb):
        print("Exit: " + self.name + " (exc: " + str(exc_type) + ")")
        return False

try:
    with SafeResource("Outer"):
        with SafeResource("Inner"):
            raise ValueError("Error!")
except ValueError:
    print("Caught outside")

# Common Mistake 5: Using wrong variable scope
print("\n=== Mistake 5: Variable scope ===")

# WRONG: Variable used outside context
# with open("test.txt", "w") as f:
#     f.write("test")
# print(f.read())  # f is closed!

# CORRECT: Read content inside context
with open("Python_Pratice/test.txt", "w") as f:
    f.write("test")
    
with open("Python_Pratice/test.txt", "r") as f:
    content = f.read()
print("Content: " + content)

# Common Mistake 6: Forgetting cleanup in dynamic contexts
print("\n=== Mistake 6: Forgetting dynamic cleanup ===")

from contextlib import ExitStack

def bad_dynamic():
    # This doesn't clean up properly
    files = []
    for i in range(3):
        f = open("temp" + str(i) + ".txt", "w")
        files.append(f)
    # Forgot to close!

# Fix with ExitStack
def good_dynamic():
    with ExitStack() as stack:
        files = []
        for i in range(3):
            f = stack.enter_context(open("temp" + str(i) + ".txt", "w"))
            files.append(f)
        # Automatically cleaned up!

good_dynamic()

# Cleanup any leftover files
for i in range(3):
    try:
        os.unlink("temp" + str(i) + ".txt")
    except:
        pass

# Common Mistake 7: Not understanding ExitStack callback order
print("\n=== Mistake 7: Callback order ===")

with ExitStack() as stack:
    stack.callback(lambda: print("Callback 1"))
    stack.callback(lambda: print("Callback 2"))
    stack.callback(lambda: print("Callback 3"))

print("Callbacks run in reverse order!")

# Common Mistake 8: Trying to reuse context managers
print("\n=== Mistake 8: Reusing context managers ===")

class Reusable:
    def __enter__(self):
        print("Enter")
        return self
    def __exit__(self, *args):
        print("Exit")

r = Reusable()

# First use
with r:
    print("First use")

# Second use - might fail or behave unexpectedly
try:
    with r:
        print("Second use")
except Exception as e:
    print("Error: " + str(e))

# Common Mistake 9: Using with statement incorrectly
print("\n=== Mistake 9: Using with incorrectly ===")

# WRONG: No colon after with
# with open("test.txt", "w")
#     pass

# CORRECT:
with open("Python_Pratice/test.txt", "w") as f:
    pass

# Common Mistake 10: Not handling None from context manager
print("\n=== Mistake 10: None from context manager ===")

class ReturnsNone:
    def __enter__(self):
        return None
    def __exit__(self, *args):
        pass

with ReturnsNone() as val:
    if val is None:
        print("Got None - that's valid but be careful!")

# Best practices
print("\n=== Best Practices ===")
print("1. Remember LIFO order for exits")
print("2. Check files exist before opening")
print("3. Keep variable scope in mind")
print("4. Use ExitStack for dynamic contexts")
print("5. Don't reuse context managers")
print("6. Handle None return values")
print("7. Use callbacks for cleanup")

print("All examples done!")
