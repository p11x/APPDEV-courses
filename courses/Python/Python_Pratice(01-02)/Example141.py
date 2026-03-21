# Example141.py
# Topic: Multiple Context Managers — Stacked/Comma Syntax Basics

# Python allows multiple context managers in a single 'with' statement
# They can be stacked (nested) or written on the same line

# Method 1: Stacked (nested) context managers
class Resource:
    def __init__(self, name):
        self.name = name
    def __enter__(self):
        print("Entering: " + self.name)
        return self
    def __exit__(self, *args):
        print("Exiting: " + self.name)

with Resource("A"):
    with Resource("B"):
        print("Inside both resources")

# Method 2: Comma-separated on single line
with Resource("X") as x, Resource("Y") as y:
    print("Inside X and Y")
    print("X: " + x.name)
    print("Y: " + y.name)

# Method 3: Multiple files at once
import os

# Create test files
with open("Python_Pratice/file1.txt", "w") as f1:
    f1.write("Content 1")
with open("Python_Pratice/file2.txt", "w") as f2:
    f2.write("Content 2")

# Read both files together
with open("Python_Pratice/file1.txt", "r") as f1, open("Python_Pratice/file2.txt", "r") as f2:
    content1 = f1.read()
    content2 = f2.read()
    print("File 1: " + content1)
    print("File 2: " + content2)

# Copy content from one file to another
with open("Python_Pratice/file1.txt", "r") as source, open("Python_Pratice/file3.txt", "w") as dest:
    content = source.read()
    dest.write(content.upper())

print("Copied and uppercase!")

# Cleanup
os.unlink("Python_Pratice/file1.txt")
os.unlink("Python_Pratice/file2.txt")
os.unlink("Python_Pratice/file3.txt")

# Multiple context managers with different types
import tempfile

# Using open() and tempfile together
temp_file = tempfile.NamedTemporaryFile(mode="w", delete=False)
temp_file.write("Temp content")
temp_file.close()

with open(temp_file.name, "r") as f, tempfile.TemporaryDirectory() as tmpdir:
    content = f.read()
    print("Read from: " + temp_file.name)
    print("Content: " + content)
    print("Temp dir: " + tmpdir)

os.unlink(temp_file.name)

# Using threading.Lock with other context managers
import threading

lock = threading.Lock()

with lock:
    with open("Python_Pratice/test.txt", "r") as f:
        data = f.read()
        print("Data protected by lock: " + str(len(data)) + " chars")

# Mixed: class-based and function-based context managers
from contextlib import contextmanager

@contextmanager
def timer(name):
    import time
    start = time.time()
    print("Starting: " + name)
    yield
    print("Finished: " + name + " in " + str(time.time() - start) + "s")

with timer("operation"), open("Python_Pratice/test.txt", "r") as f:
    lines = f.readlines()
    print("Read " + str(len(lines)) + " lines")

# Order matters for cleanup
print("=== Cleanup order ===")

class Tracker:
    def __init__(self, name):
        self.name = name
    def __enter__(self):
        print("Enter: " + self.name)
        return self
    def __exit__(self, *args):
        print("Exit: " + self.name)

# Entered first, exits last (LIFO - Last In First Out)
with Tracker("First"), Tracker("Second"), Tracker("Third"):
    print("Inside all three")

print("All examples done!")
