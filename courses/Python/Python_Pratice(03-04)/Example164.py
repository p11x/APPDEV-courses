# Example164.py
# Topic: Real-World Control Flow Patterns


# ============================================================
# Example 1: User Input Validation Loop
# ============================================================
print("=== Input Validation Loop ===")

def get_valid_email() -> str:
    while True:
        email = input("Enter email: ") if False else "test@example.com"
        if "@" in email and "." in email:
            return email
        print("Invalid email. Try again.")

email = get_valid_email()
print(f"Valid email: {email}")


# ============================================================
# Example 2: Menu-Driven Program
# ============================================================
print("=== Menu Program ===")

def main_menu():
    while True:
        print("\n1. Add Item")
        print("2. Remove Item")
        print("3. View Items")
        print("4. Exit")
        
        choice = input("Choice: ") if False else "3"
        
        match choice:
            case "1":
                print("Adding item...")
            case "2":
                print("Removing item...")
            case "3":
                print("Viewing items: [item1, item2]")
            case "4":
                print("Goodbye!")
                break
            case _:
                print("Invalid choice")

# main_menu()
print("(Menu skipped)")


# ============================================================
# Example 3: File Processing Pipeline
# ============================================================
print("=== File Pipeline ===")

def process_files(files: list[str]) -> dict:
    results = {"success": [], "failed": []}
    
    for file in files:
        try:
            if file.endswith(".txt"):
                results["success"].append(file)
            else:
                raise ValueError(f"Unsupported: {file}")
        except Exception as e:
            results["failed"].append((file, str(e)))
    
    return results

files = ["data1.txt", "data2.csv", "data3.txt", "data4.json"]
results = process_files(files)
print(f"Success: {results['success']}")
print(f"Failed: {results['failed']}")


# ============================================================
# Example 4: Rate Limiter Pattern
# ============================================================
print("=== Rate Limiter ===")

import time

class RateLimiter:
    def __init__(self, max_calls: int, period: float):
        self.max_calls = max_calls
        self.period = period
        self.calls = []
    
    def allow(self) -> bool:
        now = time.time()
        self.calls = [c for c in self.calls if now - c < self.period]
        
        if len(self.calls) < self.max_calls:
            self.calls.append(now)
            return True
        return False

limiter = RateLimiter(3, 10)

for i in range(5):
    result = limiter.allow()
    print(f"Request {i+1}: {'Allowed' if result else 'Denied'}")


# ============================================================
# Example 5: Observer Pattern
# ============================================================
print("=== Observer Pattern ===")

class EventObserver:
    def __init__(self):
        self.listeners = []
    
    def subscribe(self, callback):
        self.listeners.append(callback)
    
    def notify(self, event):
        for listener in self.listeners:
            listener(event)

def logger(event):
    print(f"LOG: {event}")

def analytics(event):
    print(f"ANALYTICS: {event}")

observer = EventObserver()
observer.subscribe(logger)
observer.subscribe(analytics)

observer.notify("User logged in")
observer.notify("Purchase made")


# ============================================================
# Example 6: Pipeline Pattern
# ============================================================
print("=== Pipeline Pattern ===")

def pipeline(*functions):
    def execute(value):
        result = value
        for func in functions:
            result = func(result)
        return result
    return execute

def add_5(x):
    return x + 5

def double(x):
    return x * 2

def subtract_3(x):
    return x - 3

pipe = pipeline(add_5, double, subtract_3)
print(f"Pipeline result: {pipe(10)}")


# ============================================================
# Example 7: Memoization Cache
# ============================================================
print("=== Memoization ===")

def memoize(func):
    cache = {}
    def wrapper(*args):
        if args not in cache:
            cache[args] = func(*args)
        return cache[args]
    return wrapper

@memoize
def fibonacci(n):
    if n < 2:
        return n
    return fibonacci(n-1) + fibonacci(n-2)

for i in range(10):
    print(f"fib({i}) = {fibonacci(i)}")


# ============================================================
# Example 8: Command Pattern
# ============================================================
print("=== Command Pattern ===")

class Command:
    def execute(self):
        raise NotImplementedError

class AddCommand(Command):
    def __init__(self, value):
        self.value = value
    
    def execute(self):
        return f"Added {self.value}"

class MultiplyCommand(Command):
    def __init__(self, value):
        self.value = value
    
    def execute(self):
        return f"Multiplied by {self.value}"

class CommandRunner:
    def __init__(self):
        self.commands = []
    
    def add_command(self, cmd):
        self.commands.append(cmd)
    
    def run_all(self):
        results = []
        for cmd in self.commands:
            results.append(cmd.execute())
        return results

runner = CommandRunner()
runner.add_command(AddCommand(5))
runner.add_command(MultiplyCommand(3))
runner.add_command(AddCommand(10))

for result in runner.run_all():
    print(result)
