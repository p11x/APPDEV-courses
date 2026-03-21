# Example136.py
# Topic: Writing Context Managers — Common Mistakes

# Mistake 1: Not returning self from __enter__
class BadReturn:
    def __enter__(self):
        return "some value"  # Should return self
    
    def __exit__(self, *args):
        pass

with BadReturn() as m:
    # Can't access methods here!
    pass

# Correct: return self
class GoodReturn:
    def __init__(self):
        self.value = 100
    
    def __enter__(self):
        return self
    
    def __exit__(self, *args):
        print("Cleaned up")

with GoodReturn() as m:
    print(m.value)

# Mistake 2: Not handling exceptions in __exit__
class BadHandler:
    def __enter__(self):
        print("Entering")
        return self
    
    def __exit__(self, *args):
        print("Exiting")  # Doesn't handle the exception

try:
    with BadHandler():
        raise ValueError("Oops!")
except:
    print("Exception propagated")

# Correct: handle or propagate
class GoodHandler:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            print("Handled: " + str(exc_val))
        return True  # Suppress the exception

with GoodHandler():
    raise ValueError("Caught inside")

# Mistake 3: Forgetting to close resources
class BadResource:
    def __init__(self):
        self.file = None
    
    def __enter__(self):
        self.file = open("Python_Pratice/test.txt", "r")
        return self
    
    def __exit__(self, *args):
        # Forgot to close!
        pass

with BadResource() as r:
    data = r.file.read()
    print(data)
# File is still open!

# Correct: always close
class GoodResource:
    def __init__(self):
        self.file = None
    
    def __enter__(self):
        self.file = open("Python_Pratice/test.txt", "r")
        return self
    
    def __exit__(self, *args):
        if self.file:
            self.file.close()

with GoodResource() as r:
    data = r.file.read()
    print(data)
# File is closed now

# Mistake 4: Raising exceptions in __enter__
class BadEnter:
    def __init__(self):
        self.initialized = False
    
    def __enter__(self):
        raise RuntimeError("Failed to initialize!")
    
    def __exit__(self, *args):
        pass

try:
    with BadEnter() as m:
        pass
except RuntimeError as e:
    print("Exception in __enter__: " + str(e))

# Mistake 5: Using mutable default arguments
class MutableDefault:
    def __init__(self, items=None):
        if items is None:
            items = []
        self.items = items
    
    def __enter__(self):
        self.items.append("item")
        return self
    
    def __exit__(self, *args):
        pass

# This works but is dangerous with mutable defaults
with MutableDefault() as m1:
    print(m1.items)

with MutableDefault() as m2:
    print(m2.items)

# Mistake 6: Not checking for None in __exit__
class BadNoneCheck:
    def __init__(self):
        self.resource = None
    
    def __enter__(self):
        self.resource = "acquired"
        return self
    
    def __exit__(self, *args):
        self.resource.close()  # AttributeError if None!

# Correct: check for None
class GoodNoneCheck:
    def __init__(self):
        self.resource = None
    
    def __enter__(self):
        self.resource = "acquired"
        return self
    
    def __exit__(self, *args):
        if self.resource is not None:
            print("Cleaning up: " + self.resource)

with GoodNoneCheck() as m:
    print(m.resource)

# Mistake 7: Wrong return value from __exit__
class WrongReturn:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        print("Exception: " + str(exc_type))
        return "True"  # Should be True/False, not string!

try:
    with WrongReturn():
        raise ValueError("Error")
except:
    print("Exception propagated anyway")

# Correct: return boolean
class CorrectReturn:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            print("Caught: " + str(exc_val))
            return True  # Suppress the exception
        return False

with CorrectReturn():
    raise ValueError("Suppressed")

print("Continuing...")

# Mistake 8: Forgetting exception parameters
class ForgottenParams:
    def __enter__(self):
        return self
    
    def __exit__(self):
        # Forgot to accept exc_type, exc_val, exc_tb!
        pass

try:
    with ForgottenParams():
        raise ValueError("Oops")
except:
    print("Exception was lost!")

# Correct: accept all three
class CorrectParams:
    def __enter__(self):
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        print("Exception type: " + str(exc_type))
        return False

try:
    with CorrectParams():
        raise ValueError("Caught")
except:
    print("Propagated correctly")

# Best practices summary
print("=== Best Practices ===")
print("1. Return self from __enter__ when needed")
print("2. Handle exceptions in __exit__ properly")
print("3. Always close resources in __exit__")
print("4. Don't raise exceptions in __enter__ (or handle them)")
print("5. Avoid mutable default arguments")
print("6. Check for None before using resources")
print("7. Return True/False from __exit__, not strings")
print("8. Accept all three exception parameters")

print("All examples done!")
