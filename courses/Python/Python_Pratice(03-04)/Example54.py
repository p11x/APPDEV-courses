# Example54.py
# Topic: Closures and Scope - Edge Cases and Pitfalls

# This file demonstrates common pitfalls with closures and scope,
# especially the classic closure-in-loop problem.


# ============================================================
# Pitfall: Closures in Loops
# ============================================================
print("=== Pitfall: Closures in Loops ===")

# PROBLEM: All lambdas capture the same variable
functions = []
for i in range(3):
    functions.append(lambda: i)

# All return the same value!
print("Wrong (all capture loop variable):")
for f in functions:
    print(f"  Function returns: {f()}")  # All return 2!

# SOLUTION 1: Default argument captures value
functions_correct = []
for i in range(3):
    functions_correct.append(lambda x=i: x)

print("\nCorrect (using default argument):")
for f in functions_correct:
    print(f"  Function returns: {f()}")  # Returns 0, 1, 2

# SOLUTION 2: Use closure factory
def make_index_func(idx):
    def func():
        return idx
    return func

functions_factory = [make_index_func(i) for i in range(3)]

print("\nCorrect (using factory):")
for f in functions_factory:
    print(f"  Function returns: {f()}")


# ============================================================
# Pitfall: Mutable Default Arguments (Different Issue)
# ============================================================
print("\n=== Related: Mutable Default Arguments ===")

# This is a different but related pitfall
def bad_function(items=[]):
    items.append("default")
    return items

# This mutates the default!
r1 = bad_function()
r2 = bad_function()
print(f"Result 1: {r1}")  # ['default', 'default']
print(f"Result 2: {r2}")  # Same list!

# CORRECT: Use None
def good_function(items=None):
    if items is None:
        items = []
    items.append("default")
    return items

r1 = good_function()
r2 = good_function()
print(f"Correct 1: {r1}")  # ['default']
print(f"Correct 2: {r2}")  # ['default']


# ============================================================
# Real-world Example 1: Loop with Callbacks
# ============================================================
print("\n=== Real-world: Loop with Callbacks ===")

class EventManager:
    def __init__(self):
        self.handlers = {}
    
    def on(self, event_name):
        """Decorator to register event handlers."""
        def decorator(func):
            if event_name not in self.handlers:
                self.handlers[event_name] = []
            # Use default argument to capture current event_name
            def wrapper(*args, **kwargs):
                return func(*args, **kwargs)
            self.handlers[event_name].append(wrapper)
            return wrapper
        return decorator
    
    def trigger(self, event_name, *args, **kwargs):
        results = []
        if event_name in self.handlers:
            for handler in self.handlers[event_name]:
                results.append(handler(*args, **kwargs))
        return results

events = EventManager()

# Register handlers for multiple events - WORKS CORRECTLY
@events.on("user.created")
def handle_user_created(data):
    return f"Created user: {data['name']}"

@events.on("user.created")
def send_welcome_email(data):
    return f"Welcome email sent to {data['email']}"

@events.on("user.deleted")
def handle_user_deleted(data):
    return f"Deleted user: {data['name']}"

# Trigger events
results = events.trigger("user.created", {"name": "Alice", "email": "alice@example.com"})
print("User created handlers:")
for r in results:
    print(f"  {r}")


# ============================================================
# Real-world Example 2: List Comprehension with Closures
# ============================================================
print("\n=== Real-world: List Comprehension with Closures ===")

# Create functions that capture values from comprehension
multipliers = [lambda x, i=i: x * i for i in range(1, 6)]

print("Multipliers (1x to 5x):")
for i, m in enumerate(multipliers, 1):
    print(f"  {i}x of 10 = {m(10)}")


# ============================================================
# Real-world Example 3: Dynamic Button Handlers
# ============================================================
print("\n=== Real-world: Dynamic Button Handlers ===")

class Button:
    def __init__(self, label, action):
        self.label = label
        self.action = action
    
    def click(self):
        return self.action()

# Create buttons with closures - using default arg to capture
buttons = []
for i in range(1, 4):
    btn = Button(
        f"Button {i}",
        lambda i=i: f"Action {i} executed!"  # Default arg captures i
    )
    buttons.append(btn)

for btn in buttons:
    print(f"{btn.label}: {btn.click()}")


# ============================================================
# Real-world Example 4: Async Callbacks (Simulated)
# ============================================================
print("\n=== Real-world: Async Callbacks ===")

class AsyncRunner:
    def __init__(self):
        self.callbacks = []
    
    def register(self, callback_id, callback):
        # Use default argument to capture callback_id
        def wrapper(result, cid=callback_id):
            return callback(result, cid)
        self.callbacks.append(wrapper)
    
    def execute(self):
        results = []
        for cb in self.callbacks:
            results.append(cb(f"Result for callback"))
        return results

runner = AsyncRunner()

# Register callbacks
runner.register("callback_1", lambda result, cid: f"{cid}: {result} processed")
runner.register("callback_2", lambda result, cid: f"{cid}: {result} logged")
runner.register("callback_3", lambda result, cid: f"{cid}: {result} saved")

results = runner.execute()
for r in results:
    print(f"  {r}")


# ============================================================
# Real-world Example 5: Timeout/Expire Function
# ============================================================
print("\n=== Real-world: Function with Expiration ===")

import time

def make_ expiration_handler(ttl_seconds):
    """Create a function that expires after TTL."""
    created_at = time.time()
    call_count = [0]
    
    def func(*args, **kwargs):
        current = time.time()
        
        # Check expiration
        if current - created_at > ttl_seconds:
            raise TimeoutError(f"Function expired after {ttl_seconds}s")
        
        call_count[0] += 1
        return f"Called {call_count[0]} times in valid period"
    
    def is_expired():
        return time.time() - created_at > ttl_seconds
    
    def time_remaining():
        return max(0, ttl_seconds - (time.time() - created_at))
    
    return func, is_expired, time_remaining

# Create a 2-second expiration handler
func, is_expired, remaining = make_ expiration_handler(2)

# First call - should work
print(f"Call 1: {func()}")

# Wait a bit
time.sleep(1)
print(f"Remaining: {remaining():.1f}s")

# Second call - should still work
print(f"Call 2: {func()}")

# Wait for expiration
time.sleep(1.5)

# Should be expired now
if is_expired():
    print("Function has expired!")
    try:
        func()
    except TimeoutError as e:
        print(f"  Error: {e}")


# ============================================================
# Real-world Example 6: Retry with Closure
# ============================================================
print("\n=== Real-world: Retry Handler ===")

def make_retry_handler(max_retries, delay):
    """Create a function with retry logic."""
    attempts = [0]
    
    def execute(func):
        attempts[0] += 1
        
        if attempts[0] > max_retries:
            raise RuntimeError(f"Max retries ({max_retries}) exceeded")
        
        try:
            return func()
        except Exception as e:
            if attempts[0] < max_retries:
                print(f"  Attempt {attempts[0]} failed, retrying in {delay}s...")
                time.sleep(delay)
                return execute(func)  # Recursive retry
            raise
    
    def get_attempts():
        return attempts[0]
    
    def reset():
        attempts[0] = 0
    
    return execute, get_attempts, reset

retry, get_attempts, reset_retry = make_retry_handler(max_retries=3, delay=0.1)

# Simulate a failing function
call_count = [0]

def unreliable_function():
    call_count[0] += 1
    if call_count[0] < 3:
        raise ValueError("Temporary failure")
    return "Success!"

try:
    result = retry(unreliable_function)
    print(f"Result: {result}")
    print(f"Total attempts: {get_attempts()}")
except RuntimeError as e:
    print(f"Error: {e}")


# ============================================================
# Summary: Key Points
# ============================================================
print("\n" + "=" * 50)
print("KEY POINTS - CLOSURES IN LOOPS:")
print("=" * 50)
print("""
1. PROBLEM: Closures in loops capture the loop variable,
   not its current value

2. SOLUTIONS:
   a) Use default argument: lambda x=i: x
   b) Use closure factory: def make_func(i): return lambda: i
   c) Use list comprehension with default args

3. RELATED: Mutable default arguments are a different pitfall
   - Use None as default and create new list/dict inside

4. BEST PRACTICE: When in doubt about closure variables,
   use default arguments to capture values explicitly
""")
