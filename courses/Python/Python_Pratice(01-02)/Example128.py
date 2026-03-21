# Example128.py
# Topic: Context Managers — Common Mistakes

# Common mistakes when using context managers

# === MISTAKE 1: Using file after close ===

# WRONG - file closed after with block
with open("test.txt", "w") as f:
    f.write("Hello")

# This will fail!
# f.write("More")  # ValueError: I/O operation on closed file

# CORRECT - do everything inside with block
with open("test.txt", "w") as f:
    f.write("Hello")
    f.write(" More")  # Inside block - works!

# === MISTAKE 2: Forgetting exception handling ===

# WRONG - with doesn't catch exceptions automatically
# The exception still propagates!
try:
    with open("test.txt", "r") as f:
        data = f.read()
        result = 1 / 0  # Exception here
except ZeroDivisionError as e:
    print("Caught: " + str(e))

# with ensures cleanup happens, not exception handling

# === MISTAKE 3: Not returning self from __enter__ ===

# WRONG - __enter__ should return something useful
class BadContext:
    def __enter__(self):
        # Should return self or useful value
        return None  # Bad!
    
    def __exit__(self, *args):
        pass

# with BadContext() as ctx:
#     print(ctx)  # None!

# CORRECT
class GoodContext:
    def __enter__(self):
        return self
    
    def __exit__(self, *args):
        pass

with GoodContext() as ctx:
    print("Context: " + str(ctx))

# === MISTAKE 4: Not handling __exit__ properly ===

# WRONG - cleanup might not happen
class BadCleanup:
    def __enter__(self):
        self.file = open("test.txt", "w")
        return self
    
    def __exit__(self, *args):
        # Forgot to close!
        # self.file.close()
        pass

# CORRECT - always close resources
class GoodCleanup:
    def __enter__(self):
        self.file = open("test.txt", "w")
        return self
    
    def __exit__(self, *args):
        if self.file:
            self.file.close()

with GoodCleanup() as ctx:
    ctx.file.write("Test")

# === MISTAKE 5: Using wrong file mode ===

# WRONG - reading from write-only file
# with open("test.txt", "w") as f:
#     data = f.read()  # UnsupportedOperation

# CORRECT - use correct mode
with open("test.txt", "w") as f:
    f.write("Write mode")

with open("test.txt", "r") as f:
    data = f.read()
    print("Read: " + data)

# === MISTAKE 6: Forgetting to use with entirely ===

# WRONG - resource leak
file = open("test.txt", "w")
file.write("Content")
# Forgot to close - file stays open!

# File still open!
import sys
sys.exit(0)

# CORRECT - always use with or explicitly close
with open("test.txt", "w") as f:
    f.write("Content")
# Auto-closed

# === MISTAKE 7: Nested with errors ===

# WRONG - exception in inner with breaks outer
class Inner:
    def __enter__(self):
        return self
    def __exit__(self, *args):
        print("Inner cleanup")
        return False

class Outer:
    def __enter__(self):
        return self
    def __exit__(self, *args):
        print("Outer cleanup")
        return False

# Both still clean up even with exception
try:
    with Outer():
        with Inner():
            raise ValueError("Error!")
except ValueError:
    pass

# === MISTAKE 8: Not using context manager for network ===

# WRONG - connection might leak
import socket

# sock = socket.socket()
# sock.connect(("example.com", 80))
# # If error, socket never closes!

# CORRECT - use with if supported, or try/finally
try:
    with socket.socket() as sock:
        # sock.connect(("example.com", 80))
        print("Socket created (using context manager)")
except Exception as e:
    print("Error: " + str(e))

# === MISTAKE 9: Assuming __exit__ return value ===

# WRONG - returning wrong value suppresses exception
class BadHandler:
    def __enter__(self):
        return self
    def __exit__(self, exc_type, exc_val, exc_tb):
        return True  # Always suppresses!

try:
    with BadHandler():
        raise ValueError("This is suppressed!")
except ValueError:
    print("This shouldn't print!")

# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Always use with for resource management
# 2. Do all work inside the with block
# 3. __enter__ should return useful value (usually self)
# 4. __exit__ should clean up resources
# 5. Use correct file modes
# 6. with doesn't replace exception handling
# 7. Use return True in __exit__ only to suppress exceptions intentionally
