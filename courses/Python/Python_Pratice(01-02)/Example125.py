# Example125.py
# Topic: Context Managers — Basic With Statement

# Using with for automatic resource cleanup

# === Basic with statement ===
# File automatically closes after block
# Note: Create a test file first

# Writing to a file
with open("test.txt", "w") as f:
    f.write("Hello, world!")

# Reading from a file
with open("test.txt", "r") as f:
    data = f.read()
    print("File content: " + data)

# File is now closed automatically

# === Why use with? ===
# Without with - must manually close
file = open("test.txt", "r")
content = file.read()
file.close()  # Easy to forget!

# With with - auto close
with open("test.txt", "r") as f:
    content = f.read()
# Automatically closed!

# === with guarantees cleanup even with exceptions ===
# This works even if exception occurs
try:
    with open("test.txt", "r") as f:
        lines = f.readlines()
        # Even if error here, file still closes!
except Exception as e:
    print("Error: " + str(e))

# === with assigns result of __enter__ to variable ===
with open("test.txt", "r") as f:
    # f is the result of open().__enter__()
    print("File object: " + str(type(f)))

# === Practical: Working with temporary data ===
import os

# Create temp file
with open("temp_data.txt", "w") as f:
    f.write("Line 1\nLine 2\nLine 3")

# Read and process
with open("temp_data.txt", "r") as f:
    lines = f.readlines()
    print("Lines: " + str(len(lines)))

# Clean up
os.remove("temp_data.txt")

# === with for other contexts ===
# Many objects support context manager protocol

# === Context manager for locking (threading) ===
import threading

lock = threading.Lock()

with lock:
    # Critical section
    print("Inside lock")
# Lock automatically released

# === SimpleTimer context manager example ===
import time

class Timer:
    def __enter__(self):
        self.start = time.time()
        return self
    
    def __exit__(self, *args):
        self.end = time.time()
        print("Elapsed: " + str(self.end - self.start) + " seconds")

with Timer():
    # Do something that takes time
    total = 0
    for i in range(1000000):
        total += i
    print("Computed: " + str(total))

# === with for database (simulated) ===
# Real DB would use similar pattern

class DatabaseConnection:
    def __enter__(self):
        print("Connecting to database...")
        self.connected = True
        return self
    
    def __exit__(self, *args):
        print("Closing database connection...")
        self.connected = False
    
    def query(self, sql):
        print("Executing: " + sql)

# Using context manager
with DatabaseConnection() as db:
    db.query("SELECT * FROM users")
# Connection auto-closed

print("\n=== Summary ===")
print("with ensures resources are properly cleaned up")
print("Works with any object implementing __enter__ and __exit__")
print("Guarantees cleanup even if exceptions occur")
