# Example140.py
# Topic: Contextlib Module — nullcontext and Common Mistakes

from contextlib import contextmanager, suppress, nullcontext

# nullcontext: a context manager that does nothing
# Useful when you need a context manager conditionally

# Example 1: Basic nullcontext
with nullcontext():
    print("Inside nullcontext")

with nullcontext() as value:
    print("Value is: " + str(value))

# Example 2: Using nullcontext conditionally
def get_context(enable):
    if enable:
        return nullcontext()
    else:
        return nullcontext()

with get_context(True):
    print("With context enabled")

with get_context(False):
    print("With context disabled")

# Example 3: nullcontext vs suppress
# nullcontext does nothing, suppress ignores errors
with nullcontext():
    print("nullcontext: normal execution")

with suppress(ValueError):
    print("suppress: suppressed ValueError")

# Example 4: nullcontext for optional resource
class Database:
    def connect(self):
        print("Connecting to database")
    def disconnect(self):
        print("Disconnecting from database")

db = None
use_db = False

# Without nullcontext, you'd need if/else
context = Database() if use_db else nullcontext()

with context as c:
    if c is not None:
        c.connect()
    print("Working with database or not")

# Common Mistake 1: Forgetting yield in @contextmanager
@contextmanager
def bad_no_yield():
    print("Entering")
    # Forgot to yield!
    print("Exiting")

try:
    with bad_no_yield():
        print("Inside")
except Exception as e:
    print("Error: " + str(e))

# Correct version
@contextmanager
def good_with_yield():
    print("Entering")
    yield
    print("Exiting")

with good_with_yield():
    print("Inside")

# Common Mistake 2: Not using try/finally
@contextmanager
def bad_no_try():
    f = open("Python_Pratice/test.txt", "w")
    yield f
    # Forgot to close!
    # File stays open if exception occurs!

# Correct version
@contextmanager
def good_with_try():
    f = open("Python_Pratice/test.txt", "w")
    try:
        yield f
    finally:
        f.close()

with good_with_try() as f:
    f.write("Safe!")

# Common Mistake 3: Not handling exceptions in @contextmanager
@contextmanager
def bad_no_except():
    print("Starting")
    yield
    # Exception is not handled!

try:
    with bad_no_except():
        raise ValueError("Oops")
except ValueError:
    print("Exception propagated")

# Correct version
@contextmanager
def good_with_except():
    print("Starting")
    try:
        yield
    except ValueError as e:
        print("Caught: " + str(e))

with good_with_except():
    raise ValueError("Handled")

# Common Mistake 4: Wrong order of try/except/finally
@contextmanager
def bad_order():
    try:
        print("Try")
        yield
    except:
        print("Except")
        # Missing finally!

with bad_order():
    print("Inside")

print("---")

# Common Mistake 5: Using @contextmanager without importing
# (would cause NameError)
try:
    # This would fail if @contextmanager not imported
    pass
except:
    print("Need to import contextmanager")

# Common Mistake 6: Returning non-None from @contextmanager incorrectly
@contextmanager
def bad_return():
    yield "value"  # This is fine actually
    
with bad_return() as v:
    print(v)

# Common Mistake 7: Using return instead of yield
@contextmanager
def bad_return_instead_of_yield():
    print("Entering")
    return  # This breaks the context manager!
    print("This never runs")

# This would fail
# with bad_return_instead_of_yield():
#     pass

# Correct way
@contextmanager
def good_return():
    print("Entering")
    yield
    print("Exiting")

with good_return():
    print("Inside")

# Summary
print("=== Summary ===")
print("1. Always use yield in @contextmanager")
print("2. Always use try/finally for cleanup")
print("3. Handle exceptions if needed")
print("4. Use correct order: try/except/finally")
print("5. Import contextlib modules properly")
print("6. Use yield, not return")
print("7. nullcontext is useful for optional contexts")

print("All examples done!")
