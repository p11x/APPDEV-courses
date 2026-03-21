# Example135.py
# Topic: Writing Context Managers — Handling Exceptions

# __exit__ receives three parameters about the exception:
# - exc_type: the exception class (e.g., ValueError)
# - exc_val: the exception instance (the actual error)
# - exc_tb: the traceback object

# Returning True suppresses the exception (it won't propagate)
# Returning False (or None) lets the exception propagate

# Example 1: Silently handling exceptions
class Suppressor:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            print("Caught: " + str(exc_val))
        return True  # Suppress the exception

with Suppressor():
    raise ValueError("This will be suppressed")

print("Continuing after suppression")

# Example 2: Not suppressing (letting exceptions propagate)
class Propagator:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            print("Exception occurred: " + str(exc_val))
        return False  # Don't suppress

try:
    with Propagator():
        raise ValueError("This will propagate")
except ValueError:
    print("Exception was caught outside")

# Example 3: Conditional handling
class ConditionalHandler:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is ValueError:
            print("Handling ValueError specifically: " + str(exc_val))
            return True  # Suppress only ValueError
        elif exc_type is TypeError:
            print("Handling TypeError: " + str(exc_val))
            return False  # Let TypeError propagate
        return False

with ConditionalHandler():
    raise ValueError("Value error")

try:
    with ConditionalHandler():
        raise TypeError("Type error")
except TypeError:
    print("TypeError propagated successfully")

# Example 4: Cleanup always runs
class CleanupManager:
    def __enter__(self):
        print("Setting up...")
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        print("Cleaning up...")
        print("Exception type: " + str(exc_type))
        return False

try:
    with CleanupManager():
        raise RuntimeError("Error during work")
except RuntimeError:
    print("Caught the runtime error")

# Example 5: Selective cleanup based on exception
class SelectiveCleanup:
    def __init__(self):
        self.resources = []
    
    def add_resource(self, name):
        self.resources.append(name)
        return self
    
    def __enter__(self):
        print("Opening: " + str(self.resources))
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            print("Error occurred! Cleaning up...")
            for r in reversed(self.resources):
                print("Closing: " + r)
        else:
            print("Success! Normal cleanup")
            for r in reversed(self.resources):
                print("Closing: " + r)
        return False

with SelectiveCleanup() as m:
    m.add_resource("file1").add_resource("file2").add_resource("file3")

print("---")

try:
    with SelectiveCleanup() as m:
        m.add_resource("file1").add_resource("file2")
        raise IOError("File not found")
except IOError:
    print("Caught IOError")

# Example 6: Rethrowing with context
class ExceptionWrapper:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            new_exc = RuntimeError("Wrapped: " + str(exc_val))
            raise new_exc from exc_val
        return False

try:
    with ExceptionWrapper():
        raise ValueError("Original error")
except RuntimeError as e:
    print("Got wrapped exception: " + str(e))
    print("Original cause: " + str(e.__cause__))

# Example 7: Context manager that always succeeds (cleanup runs)
class AlwaysClean:
    def __enter__(self):
        return self
    
    def __exit__(self, *args):
        print("Always runs cleanup")
        return False

try:
    with AlwaysClean():
        print("Before error")
        raise ZeroDivisionError("divide by zero")
except ZeroDivisionError:
    print("Caught the error outside")

print("After error - this runs!")

print("All examples done!")
