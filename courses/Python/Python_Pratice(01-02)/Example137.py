# Example137.py
# Topic: Contextlib Module — @contextmanager Decorator Basics

# The @contextmanager decorator lets you create context managers using generators
# Instead of writing a class with __enter__ and __exit__
# You write a generator function with a single yield

from contextlib import contextmanager

# Basic @contextmanager usage
@contextmanager
def simple_context():
    print("Entering")
    yield  # This is where the 'with' block runs
    print("Exiting")

with simple_context():
    print("Inside the context")

# @contextmanager can yield a value (assigned to 'as' variable)
@contextmanager
def timer():
    import time
    start = time.time()
    yield start  # The yielded value is what gets assigned
    end = time.time()
    print("Elapsed: " + str(end - start))

with timer() as t:
    print("Started at: " + str(t))
    result = sum(range(10000))

# Using try/finally with @contextmanager (required for cleanup!)
@contextmanager
def file_manager(filename, mode):
    f = open(filename, mode)
    try:
        yield f
    finally:
        f.close()
        print("File closed")

with file_manager("Python_Pratice/test.txt", "w") as f:
    f.write("Hello from contextmanager!")

# With file_manager using "r" mode
with file_manager("Python_Pratice/test.txt", "r") as f:
    content = f.read()
    print(content)

# Multiple yield values (using a dict or object)
@contextmanager
def managed_resource():
    resource = {"name": "myresource", "value": 100}
    try:
        yield resource
    finally:
        resource["value"] = 0
        print("Resource cleaned up")

with managed_resource() as r:
    print(r["name"])
    r["value"] = r["value"] + 50
    print(r["value"])

print(r["value"])

# Real-world: timing code blocks
@contextmanager
def timed_block(name):
    import time
    start = time.time()
    print(">>> Starting: " + name)
    try:
        yield
    finally:
        end = time.time()
        print("<<< Finished: " + name + " (" + str(end - start) + "s)")

with timed_block("Quick calculation"):
    x = sum(range(1000))

with timed_block("Slow calculation"):
    x = 0
    for i in range(10000):
        x = x + i

# Nested context managers with @contextmanager
@contextmanager
def outer():
    print("  Outer: Enter")
    yield
    print("  Outer: Exit")

@contextmanager
def inner():
    print("    Inner: Enter")
    yield
    print("    Inner: Exit")

with outer():
    with inner():
        print("    Inside both!")

# Context manager that tracks entry/exit count
@contextmanager
def counted(name):
    print("Enter: " + name)
    try:
        yield
    finally:
        print("Exit: " + name)

with counted("first"):
    with counted("second"):
        print("Inside")

print("All examples done!")
