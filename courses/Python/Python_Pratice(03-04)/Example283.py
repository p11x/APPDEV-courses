# Example283: Context Managers
from contextlib import contextmanager

# Basic context manager
class FileManager:
    def __init__(self, filename, mode):
        self.filename = filename
        self.mode = mode
        self.file = None
    
    def __enter__(self):
        self.file = open(self.filename, self.mode)
        return self.file
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if self.file:
            self.file.close()
        return False

print("Basic Context Manager:")
with FileManager('test.txt', 'w') as f:
    f.write("Hello from context manager!")

with FileManager('test.txt', 'r') as f:
    print(f"Content: {f.read()}")

# Using contextmanager decorator
@contextmanager
def timer():
    import time
    start = time.time()
    yield
    end = time.time()
    print(f"Elapsed: {end - start:.4f}s")

print("\nTimer:")
with timer():
    sum(range(1000000))

# Multiple resources
@contextmanager
def managed_resource(name):
    print(f"Acquiring {name}")
    yield f"{name}_connection"
    print(f"Releasing {name}")

print("\nManaged Resource:")
with managed_resource("database") as conn:
    print(f"Using {conn}")

# closing context
from contextlib import closing
import urllib.request

print("\nUsing closing:")
with closing(urllib.request.urlopen("https://example.com")) as page:
    print(f"Status: {page.status}")
