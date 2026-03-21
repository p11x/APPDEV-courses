# Example134.py
# Topic: Writing Context Managers — Creating Useful Context Managers

import os
import time

# Context manager for opening and reading a file
class OpenFile:
    def __init__(self, filename, mode="r"):
        self.filename = filename
        self.mode = mode
        self.file = None
    
    def __enter__(self):
        self.file = open(self.filename, self.mode)
        return self.file
    
    def __exit__(self, *args):
        if self.file:
            self.file.close()

# Using OpenFile context manager
with OpenFile("Python_Pratice/test.txt", "r") as f:
    content = f.read()
    print(content)

# Context manager for changing directory
class ChangeDirectory:
    def __init__(self, new_dir):
        self.new_dir = new_dir
        self.old_dir = None
    
    def __enter__(self):
        self.old_dir = os.getcwd()
        os.chdir(self.new_dir)
        return self.new_dir
    
    def __exit__(self, *args):
        os.chdir(self.old_dir)

# Using ChangeDirectory (be careful with this!)
# print(os.getcwd())
# with ChangeDirectory(".."):
#     print(os.getcwd())
# print(os.getcwd())

# Context manager for timing code blocks
class Timer:
    def __init__(self, name="Block"):
        self.name = name
        self.start = None
        self.end = None
    
    def __enter__(self):
        self.start = time.time()
        return self
    
    def __exit__(self, *args):
        self.end = time.time()
        elapsed = self.end - self.start
        print(self.name + " took " + str(elapsed) + " seconds")

with Timer("Quick sort"):
    sorted(range(10000))

with Timer("Bubble sort"):
    data = list(range(100))
    for i in range(len(data)):
        for j in range(i + 1, len(data)):
            if data[j] < data[i]:
                data[i], data[j] = data[j], data[i]

# Context manager for retry logic
class Retry:
    def __init__(self, max_attempts=3, delay=1):
        self.max_attempts = max_attempts
        self.delay = delay
        self.attempts = 0
    
    def __enter__(self):
        self.attempts = self.attempts + 1
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None and self.attempts < self.max_attempts:
            print("Attempt " + str(self.attempts) + " failed, retrying...")
            time.sleep(self.delay)
            return True
        return False

attempt_count = 0

def failing_function():
    global attempt_count
    attempt_count = attempt_count + 1
    if attempt_count < 3:
        raise ValueError("Temporary failure")

attempt_count = 0

# Using Retry
# with Retry(max_attempts=3, delay=0.1) as r:
#     failing_function()

# Context manager for counting
class CountCalls:
    def __init__(self):
        self.count = 0
    
    def __enter__(self):
        self.count = self.count + 1
        return self
    
    def __exit__(self, *args):
        print("This block was called " + str(self.count) + " times")

with CountCalls() as c:
    print("First call")

with CountCalls() as c:
    print("Second call")

# Context manager for logging
class LogExecution:
    def __init__(self, func_name):
        self.func_name = func_name
        self.start_time = None
    
    def __enter__(self):
        self.start_time = time.time()
        print(">>> Entering: " + self.func_name)
        return self
    
    def __exit__(self, *args):
        elapsed = time.time() - self.start_time
        print("<<< Exiting: " + self.func_name + " (" + str(elapsed) + "s)")

with LogExecution("my_function"):
    result = 0
    for i in range(1000):
        result = result + i

# Context manager for environment variables
class SetEnv:
    def __init__(self, key, value):
        self.key = key
        self.value = value
        self.old_value = None
    
    def __enter__(self):
        self.old_value = os.environ.get(self.key)
        os.environ[self.key] = self.value
        return self
    
    def __exit__(self, *args):
        if self.old_value is None:
            del os.environ[self.key]
        else:
            os.environ[self.key] = self.old_value

# Using SetEnv
print(os.environ.get("MY_VAR", "Not set"))
with SetEnv("MY_VAR", "Hello World"):
    print(os.environ.get("MY_VAR"))
print(os.environ.get("MY_VAR", "Not set"))

print("All examples done!")
