# Example142.py
# Topic: Multiple Context Managers — ExitStack Usage

# ExitStack is used when you need to manage multiple context managers dynamically
# It allows you to push context managers at runtime

from contextlib import ExitStack

# Basic ExitStack usage - push files
with ExitStack() as stack:
    # Push context managers onto the stack
    f1 = stack.enter_context(open("Python_Pratice/test.txt", "r"))
    f2 = stack.enter_context(open("Python_Pratice/output.txt", "w"))
    
    # Use the resources
    content = f1.read()
    f2.write(content.upper())
    
print("Files processed")

# ExitStack with callback (cleanup function)
import os

def cleanup_file(filename):
    print("Cleaning up: " + filename)
    if os.path.exists(filename):
        os.unlink(filename)

with ExitStack() as stack:
    # Push cleanup callbacks
    stack.callback(cleanup_file, "temp1.txt")
    stack.callback(cleanup_file, "temp2.txt")
    
    # Create temp files
    open("temp1.txt", "w").write("temp1")
    open("temp2.txt", "w").write("temp2")
    
    print("Working with temp files")

print("After context - temp files cleaned up")

# ExitStack with context manager class
class Resource:
    def __init__(self, name):
        self.name = name
    def __enter__(self):
        print("Acquire: " + self.name)
        return self
    def __exit__(self, *args):
        print("Release: " + self.name)

with ExitStack() as stack:
    # Push context managers using enter_context
    stack.enter_context(Resource("Database"))
    stack.enter_context(Resource("File"))
    stack.enter_context(Resource("Network"))
    
    print("Using all resources")

# ExitStack with unknown number of resources
def process_files(filenames):
    with ExitStack() as stack:
        files = []
        for name in filenames:
            try:
                f = stack.enter_context(open(name, "r"))
                files.append(f)
            except FileNotFoundError:
                print("Skipping: " + name)
        
        # Process all files
        for f in files:
            print("Processing: " + f.name)

# Create test files
open("data1.txt", "w").write("data1")
open("data2.txt", "w").write("data2")

process_files(["data1.txt", "data2.txt", "missing.txt"])

# Cleanup
os.unlink("data1.txt")
os.unlink("data2.txt")

# ExitStack with enter_context
with ExitStack() as stack:
    # Use enter_context to properly enter a context manager
    resource = stack.enter_context(Resource("Managed"))
    
    print("Using entered context")

# ExitStack with conditional context managers
def get_contexts(enable_extra=False):
    with ExitStack() as stack:
        # Always open main file
        main = stack.enter_context(open("Python_Pratice/test.txt", "r"))
        
        # Conditionally open extra file
        if enable_extra:
            extra = stack.enter_context(open("Python_Pratice/output.txt", "r"))
        
        # Process
        print("Main file open")
        
        # pop_all transfers control, resources stay active
        stack.pop_all()

print("Without extra:")
get_contexts(enable_extra=False)

print("With extra:")
get_contexts(enable_extra=True)

# ExitStack for cleanup on exception
try:
    with ExitStack() as stack:
        # Setup resources
        stack.enter_context(Resource("Res0"))
        stack.enter_context(Resource("Res1"))
        stack.enter_context(Resource("Res2"))
        
        # Simulate work that might fail
        raise ValueError("Something went wrong!")
except ValueError:
    print("Exception caught - cleanup still ran")

# Combining ExitStack with suppress
from contextlib import suppress

with ExitStack() as stack:
    # Push a suppressor for specific exceptions
    stack.push(suppress(FileNotFoundError))
    
    # This won't fail even if file doesn't exist
    f = open("nonexistent.txt", "r")
    print("File opened (or not)")

print("Continuing...")

# Using ExitStack with push (alternative to enter_context)
with ExitStack() as stack:
    # push takes a context manager directly
    stack.push(open("Python_Pratice/test.txt", "r"))
    
print("Pushed and cleaned up")

# Using callback with arguments
with ExitStack() as stack:
    def cleanup(a, b, c=None):
        print("Cleanup: " + str(a) + " " + str(b) + " " + str(c))
    
    stack.callback(cleanup, "arg1", "arg2", c="arg3")

print("All examples done!")
