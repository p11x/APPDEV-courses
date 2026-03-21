# Example127.py
# Topic: Context Managers — How They Work (__enter__/__exit__)

# Understanding the context manager protocol

# === What is a context manager? ===
# Objects that implement __enter__ and __exit__

class MyContext:
    def __enter__(self):
        print("__enter__ called")
        return self  # Value assigned to 'as' variable
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        print("__exit__ called")
        print("Exception type: " + str(exc_type))
        return False  # Don't suppress exceptions

# Using our context manager
with MyContext() as obj:
    print("Inside with block")
# __exit__ called automatically

print("\n=== How it works ===")

# === __enter__ is called at start ===
class Tracker:
    def __init__(self, name):
        self.name = name
    
    def __enter__(self):
        print("Starting: " + self.name)
        return self
    
    def __exit__(self, *args):
        print("Ending: " + self.name)
        return False

with Tracker("First"):
    print("  Doing work...")

with Tracker("Second"):
    print("  More work...")

# === __exit__ receives exception info ===
class ErrorHandler:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            print("Exception occurred: " + str(exc_val))
            return True  # Suppress exception
        return False

# With exception
with ErrorHandler():
    x = 1 / 0  # ZeroDivisionError

print("Continuing after exception...")

# === Return value of __exit__ matters ===
class Cleanup:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        print("Cleanup performed")
        return False  # Don't suppress

# Exception propagates
try:
    with Cleanup():
        raise ValueError("Test error")
except ValueError as e:
    print("Caught: " + str(e))

# === Building a useful context manager ===
import time

class Timer:
    def __enter__(self):
        self.start = time.time()
        return self
    
    def __exit__(self, *args):
        elapsed = time.time() - self.start
        print("Elapsed time: " + str(elapsed) + " seconds")

with Timer():
    time.sleep(0.1)
    total = sum(range(10000))

print("\n=== Real-world: File-like context ===")
class LogWriter:
    def __init__(self, filename):
        self.filename = filename
        self.file = None
    
    def __enter__(self):
        self.file = open(self.filename, "w")
        self.file.write("Log started\n")
        return self
    
    def __exit__(self, *args):
        if self.file:
            self.file.write("Log ended\n")
            self.file.close()
        return False
    
    def write(self, msg):
        if self.file:
            self.file.write(msg + "\n")

with LogWriter("log.txt") as log:
    log.write("Event 1")
    log.write("Event 2")
# File automatically closed

# Read and verify
with open("log.txt", "r") as f:
    print("Log contents:")
    print(f.read())

# === Context manager with state ===
class Counter:
    def __init__(self):
        self.count = 0
    
    def __enter__(self):
        self.count += 1
        print("Entered (count=" + str(self.count) + ")")
        return self
    
    def __exit__(self, *args):
        self.count -= 1
        print("Exited (count=" + str(self.count) + ")")
        return False

with Counter() as c:
    print("Inside: " + str(c.count))

print("\n=== Summary ===")
print("__enter__: Called when entering with block")
print("__exit__: Called when exiting with block")
print("__exit__ receives exception info if any")
print("Return True from __exit__ to suppress exceptions")
