# Example153.py
# Topic: Context Managers (With Statement)


# ============================================================
# Example 1: Basic With Statement
# ============================================================
print("=== Basic With Statement ===")

with open("test.txt", "w") as f:
    f.write("Hello, World!")

print("File written successfully")

with open("test.txt", "r") as f:
    content: str = f.read()
    print(f"Content: {content}")


# ============================================================
# Example 2: Multiple Context Managers
# ============================================================
print("\n=== Multiple Context Managers ===")

with open("file1.txt", "w") as f1, open("file2.txt", "w") as f2:
    f1.write("Content 1")
    f2.write("Content 2")

print("Files written")


# ============================================================
# Example 3: Writing Custom Context Manager
# ============================================================
print("\n=== Custom Context Manager ===")

class Timer:
    def __enter__(self):
        import time
        self.start = time.time()
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        import time
        self.end = time.time()
        print(f"Elapsed time: {self.end - self.start:.4f} seconds")

with Timer() as t:
    sum(range(1000000))


# ============================================================
# Example 4: Context Manager for Database
# ============================================================
print("\n=== Database Context Manager ===")

class DatabaseConnection:
    def __init__(self, db_name: str):
        self.db_name = db_name
        self.connected = False
    
    def __enter__(self):
        print(f"Connecting to {self.db_name}...")
        self.connected = True
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        print(f"Disconnecting from {self.db_name}...")
        self.connected = False
    
    def query(self, sql: str):
        print(f"Executing: {sql}")

with DatabaseConnection("users.db") as db:
    db.query("SELECT * FROM users")
    db.query("INSERT INTO users VALUES (1, 'Alice')")


# ============================================================
# Example 5: Using contextlib
# ============================================================
print("\n=== contextlib ===")

from contextlib import contextmanager

@contextmanager
def timer_context():
    import time
    start = time.time()
    yield
    end = time.time()
    print(f"Time: {end - start:.4f}s")

with timer_context():
    sum(range(1000000))


# ============================================================
# Example 6: Closing Context Manager
# ============================================================
print("\n=== closing ===")

from contextlib import closing
import urllib.request

# with closing(urllib.request.urlopen("https://example.com")) as page:
#     print(page.read()[:100])

print("(URL fetching skipped)")


# ============================================================
# Example 7: Real-World: File Processing
# ============================================================
print("\n=== Real-World: File Processing ===")

import csv

with open("data.csv", "w", newline="") as f:
    writer = csv.writer(f)
    writer.writerow(["Name", "Age", "City"])
    writer.writerow(["Alice", "30", "NYC"])
    writer.writerow(["Bob", "25", "LA"])

with open("data.csv", "r") as f:
    reader = csv.reader(f)
    for row in reader:
        print(row)


# ============================================================
# Example 8: Suppress Exceptions
# ============================================================
print("\n=== suppress ===")

from contextlib import suppress

result = None

with suppress(FileNotFoundError):
    with open("nonexistent.txt", "r") as f:
        result = f.read()

print(f"Result: {result}")
print("Program continued")
