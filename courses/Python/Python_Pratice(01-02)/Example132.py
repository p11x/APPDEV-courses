# Example132.py
# Topic: Built-in Context Managers — Common Mistakes

import tempfile
import threading
import os

# Mistake 1: Not using 'with' for files
# This can leave files open and cause data loss
print("=== Mistake 1: No 'with' statement ===")
f = open("Python_Pratice/test.txt", "w")
f.write("Data")
# If exception happens here, file stays open!
f.close()

# Correct way: always use 'with'
with open("Python_Pratice/test.txt", "w") as f:
    f.write("Data")
print("File handled correctly")

# Mistake 2: Wrong file mode
print("\n=== Mistake 2: Wrong file mode ===")
# Trying to read from a write-only file
try:
    with open("Python_Pratice/test.txt", "w") as f:
        content = f.read()
except:
    print("Error: cannot read from 'w' mode")

# Correct: use 'r+' or 'w+' for read and write
with open("Python_Pratice/test.txt", "r+") as f:
    f.write("Data")
print("Used r+ mode correctly")

# Mistake 3: Forgetting to close Lock
print("\n=== Mistake 3: Lock issues ===")
lock = threading.Lock()
lock.acquire()
print("Lock acquired")
# Forgot to release! This causes deadlock
lock.release()
print("Lock released")

# Correct way: use 'with'
with lock:
    print("Lock automatically released")

# Mistake 4: Using Lock in the wrong scope
print("\n=== Mistake 4: Lock in wrong scope ===")
counter = 0

def increment_bad():
    global counter
    local_lock = threading.Lock()
    with local_lock:
        counter = counter + 1

# Each call creates a new lock - no synchronization!
for i in range(100):
    increment_bad()

print(counter)

# Correct: use a shared lock
counter = 0
shared_lock = threading.Lock()

def increment_good():
    global counter
    with shared_lock:
        counter = counter + 1

for i in range(100):
    increment_good()

print(counter)

# Mistake 5: Not handling exceptions in context manager
print("\n=== Mistake 5: Exception handling ===")
class BadManager:
    def __enter__(self):
        return self
    def __exit__(self, *args):
        pass

try:
    with BadManager() as m:
        raise ValueError("Oops!")
except:
    print("Exception was swallowed (no handling)")

# Correct: let exceptions propagate or handle explicitly
class GoodManager:
    def __enter__(self):
        return self
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            print("Exception caught: " + str(exc_val))
        return False

try:
    with GoodManager() as m:
        raise ValueError("Oops!")
except:
    print("Exception propagated correctly")

# Wrong order in multiple context managers
print("\n=== Mistake 6: Order matters ===")
with open("Python_Pratice/file1.txt", "w") as f1, open("Python_Pratice/file2.txt", "w") as f2:
    f1.write("First")
    f2.write("Second")
    print("Both opened")

# file2 closes first, then file1 (reverse order)
# This is usually fine, but be aware
os.unlink("Python_Pratice/file1.txt")
os.unlink("Python_Pratice/file2.txt")

# Mistake 7: Forgetting tempfile cleanup
print("\n=== Mistake 7: Temp file cleanup ===")
# BAD: leaving temp files around
temp_path = tempfile.mktemp()
with open(temp_path, "w") as f:
    f.write("Temp data")
print("Created temp file: " + temp_path)
# Forgot to delete!
os.unlink(temp_path)
print("Manually deleted")

# GOOD: use TemporaryDirectory (auto-cleanup)
with tempfile.TemporaryDirectory() as tmpdir:
    path = tmpdir + "/file.txt"
    with open(path, "w") as f:
        f.write("Auto-cleaned")
print("Auto-cleanup works")

# Mistake 8: Returning values from __enter__ incorrectly
print("\n=== Mistake 8: __enter__ return value ===")
class Wrapper:
    def __init__(self, value):
        self.value = value
    def __enter__(self):
        return self.value  # Returns the value, not self
    def __exit__(self, *args):
        pass

with Wrapper("test") as w:
    print(w)
    # Can't access Wrapper methods here!

# Better: return self AND the value
class BetterWrapper:
    def __init__(self, value):
        self.value = value
    def __enter__(self):
        return self  # Return self for methods
    def __exit__(self, *args):
        pass

with BetterWrapper("test") as w:
    print(w.value)

# Mistake 9: Using binary mode for text
print("\n=== Mistake 9: Binary vs Text ===")
try:
    with open("Python_Pratice/test.txt", "wb") as f:
        f.write("Hello")  # Must be bytes!
except TypeError:
    print("Error: need bytes, not string")

# Correct: use bytes or text mode
with open("Python_Pratice/test.txt", "wb") as f:
    f.write(b"Hello")

with open("Python_Pratice/test.txt", "w") as f:
    f.write("Hello")

print("Fixed!")

# Best practices summary
print("\n=== Best Practices ===")
print("1. Always use 'with' for files and locks")
print("2. Choose the correct mode (r/w/a/r+/w+)")
print("3. Handle exceptions properly in __exit__")
print("4. Use TemporaryDirectory over mktemp")
print("5. Return self from __enter__ when needed")
print("6. Use binary mode (b) for non-text files")
print("7. Keep locks at the right scope")

print("\nAll tests passed!")
