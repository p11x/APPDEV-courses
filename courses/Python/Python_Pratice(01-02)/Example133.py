# Example133.py
# Topic: Writing Context Managers — Basic __enter__/__exit__ Protocol

# A context manager is a class that implements __enter__ and __exit__
# These methods are called automatically when using 'with'

# Basic context manager structure
class SimpleManager:
    def __enter__(self):
        # Called when entering the 'with' block
        # Return value is assigned to the 'as' variable
        print("Entering the context")
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        # Called when exiting the 'with' block
        # Always called, even if an exception occurs
        print("Exiting the context")
        return False

# Using the basic context manager
with SimpleManager() as m:
    print("Inside the with block")

# __enter__ can return any value
class ReturnsValue:
    def __enter__(self):
        return "Hello from __enter__"
    
    def __exit__(self, *args):
        pass

with ReturnsValue() as value:
    print(value)

# __enter__ can return self for method access
class Calculator:
    def __enter__(self):
        self.result = 0
        return self
    
    def add(self, x):
        self.result = self.result + x
        return self
    
    def __exit__(self, *args):
        print("Final result: " + str(self.result))

with Calculator() as calc:
    calc.add(5).add(10).add(3)

# __exit__ receives exception information
class ExceptionTracker:
    def __enter__(self):
        print("Starting")
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            print("Exception occurred: " + str(exc_val))
            print("Exception type: " + str(exc_type))
        else:
            print("No exception")
        return False

with ExceptionTracker():
    print("Normal execution")

print("---")

try:
    with ExceptionTracker():
        raise ValueError("Something went wrong")
except:
    print("Exception was handled")

print("---")

# Real-world: Timer context manager
import time

class Timer:
    def __enter__(self):
        self.start = time.time()
        return self
    
    def __exit__(self, *args):
        self.end = time.time()
        elapsed = self.end - self.start
        print("Elapsed time: " + str(elapsed) + " seconds")

with Timer():
    sum(range(1000000))

with Timer():
    result = 0
    for i in range(10000):
        result = result + i

print("All examples done!")
