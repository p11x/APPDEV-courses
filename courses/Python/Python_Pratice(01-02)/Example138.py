# Example138.py
# Topic: Contextlib Module — @contextmanager for Useful Managers

from contextlib import contextmanager
import os

# Using @contextmanager for directory change
@contextmanager
def change_dir(new_dir):
    old_dir = os.getcwd()
    os.chdir(new_dir)
    try:
        yield
    finally:
        os.chdir(old_dir)

# Note: commenting out to avoid changing working directory
# with change_dir(".."):
#     print(os.getcwd())
# print(os.getcwd())

# Using @contextmanager for environment variables
@contextmanager
def set_env(key, value):
    old_value = os.environ.get(key)
    os.environ[key] = value
    try:
        yield
    finally:
        if old_value is None:
            del os.environ[key]
        else:
            os.environ[key] = old_value

print("Before:", os.environ.get("MY_VAR", "Not set"))
with set_env("MY_VAR", "Hello"):
    print("During:", os.environ.get("MY_VAR"))
print("After:", os.environ.get("MY_VAR", "Not set"))

# Using @contextmanager for temporary file
@contextmanager
def temp_file(mode="w", suffix=".txt"):
    import tempfile
    import os
    fd, path = tempfile.mkstemp(suffix=suffix)
    if "b" in mode:
        import os
        os.close(fd)
        f = open(path, mode)
    else:
        f = os.fdopen(fd, mode)
    try:
        yield f
    finally:
        f.close()
        os.unlink(path)

with temp_file("w") as f:
    f.write("Temporary content")

print("Temp file cleaned up")

# Using @contextmanager for retry logic
@contextmanager
def retry(max_attempts=3, delay=1):
    import time
    attempt = 0
    while attempt < max_attempts:
        try:
            yield attempt
            break
        except Exception as e:
            attempt = attempt + 1
            if attempt >= max_attempts:
                raise e
            print("Attempt " + str(attempt) + " failed, retrying...")
            time.sleep(delay)

# Simulating retry
# with retry(max_attempts=3, delay=0.1) as attempt:
#     if attempt < 2:
#         raise ValueError("Temporary failure")
#     print("Success on attempt: " + str(attempt))

# Using @contextmanager for logging
@contextmanager
def log_calls(func_name):
    print(">>> Calling: " + func_name)
    try:
        yield
    finally:
        print("<<< Finished: " + func_name)

with log_calls("my_function"):
    result = 0
    for i in range(100):
        result = result + i

# Using @contextmanager for suppressing specific exceptions
@contextmanager
def suppress_errors(*exception_types):
    try:
        yield
    except exception_types as e:
        print("Suppressed: " + str(e))

with suppress_errors(ZeroDivisionError, ValueError):
    x = 1 / 0

with suppress_errors(ZeroDivisionError, ValueError):
    raise ValueError("This is also suppressed")

print("Continuing after suppressed errors")

# Using @contextmanager for transaction-like behavior
@contextmanager
def transaction(db):
    print("Starting transaction")
    db.begin()
    try:
        yield db
        print("Committing transaction")
        db.commit()
    except Exception as e:
        print("Rolling back: " + str(e))
        db.rollback()
        raise

# Mock database class
class MockDB:
    def begin(self):
        print("DB: begin")
    def commit(self):
        print("DB: commit")
    def rollback(self):
        print("DB: rollback")

db = MockDB()
try:
    with transaction(db):
        print("Doing DB operations...")
        raise ValueError("Simulated error")
except ValueError:
    print("Error handled outside")

# Using @contextmanager for iteration timing
@contextmanager
def iter_timer():
    import time
    start = time.time()
    count = 0
    try:
        yield count
    finally:
        end = time.time()
        print("Processed " + str(count) + " items in " + str(end - start) + "s")

with iter_timer() as counter:
    for i in range(1000):
        counter = counter + 1

print("All examples done!")
