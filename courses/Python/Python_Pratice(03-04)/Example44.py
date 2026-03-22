# Example44.py
# Topic: Avoiding Common Scope Pitfalls

# This file demonstrates common mistakes related to scope and how to avoid them.


# Pitfall 1: Modifying global variable without 'global' keyword
counter = 0

def increment_without_global():
    """This creates a LOCAL variable, doesn't modify global!"""
    counter = counter + 1  # This would cause UnboundLocalError!
    # To fix, use: global counter

# The above function would raise UnboundLocalError
# Let's demonstrate the correct way:

def increment_with_global():
    """Correct way to modify global variable."""
    global counter
    counter += 1

print("=== Pitfall 1: Global Variable Modification ===")  # === Pitfall 1: Global Variable Modification ===
print(f"Initial counter: {counter}")  # Initial counter: 0
increment_with_global()
print(f"After increment: {counter}")  # After increment: 1


# Pitfall 2: Mutable default argument gotcha
def add_item_bad(item, items=[]):
    """BAD: Default mutable argument persists between calls."""
    items.append(item)
    return items

print("\n=== Pitfall 2: Mutable Default Arguments ===")
result1 = add_item_bad("first")
result2 = add_item_bad("second")
print(f"Result 1: {result1}")  # ['first', 'second'] - unexpected!
print(f"Result 2: {result2}")  # ['first', 'second'] - same list!

# Correct approach:
def add_item_good(item, items=None):
    """GOOD: Use None as default and create new list."""
    if items is None:
        items = []
    items.append(item)
    return items

result1 = add_item_good("first")
result2 = add_item_good("second")
print(f"Result 1 (correct): {result1}")  # ['first']
print(f"Result 2 (correct): {result2}")  # ['second']


# Pitfall 3: Loop variable captured in closure
print("\n=== Pitfall 3: Loop Variable in Closure ===")

# Problematic code:
funcs = []
for i in range(3):
    funcs.append(lambda: i)  # Captures loop variable, not value

for f in funcs:
    print(f"Wrong: {f()}", end=" ")  # All return 2!

print()

# Correct approach - use default argument to capture value:
funcs_correct = []
for i in range(3):
    funcs_correct.append(lambda x=i: x)  # Capture current value

print("Correct:", end=" ")
for f in funcs_correct:
    print(f"{f()}", end=" ")  # Returns 0, 1, 2


# Pitfall 4: Python 3.8+ walrus operator with comprehension scope
print("\n=== Pitfall 4: Walrus Operator in Comprehension ===")

# In Python 3.8+, list comprehension has its own scope
# But walrus operator (:=) can access outer scope
result = [y for x in [[1, 2], [3, 4]] for y in x if (total := sum(x)) and total > 2]
print(f"Filtered with walrus: {result}")  # Filtered with walrus: [3, 4]


# Pitfall 5: Name masking/shadowing
print("\n=== Pitfall 5: Name Shadowing ===")  # (blank line)

def outer_function():
    """Outer function."""
    message = "outer"
    
    def inner_function():
        """Inner function that shadows the outer variable."""
        message = "inner"  # This creates a NEW local variable!
        return message
    
    print(f"Before inner: {message}")  # Before inner: outer
    result = inner_function()
    print(f"After inner: {message}")  # Still "outer"!
    print(f"Inner returns: {result}")  # Inner returns: inner

outer_function()

# Correct approach using nonlocal:
def outer_function_fixed():
    """Outer function."""
    message = "outer"
    
    def inner_function():
        """Inner function that modifies enclosing variable."""
        nonlocal message
        message = "inner"
        return message
    
    print(f"Before inner: {message}")  # Before inner: outer
    result = inner_function()
    print(f"After inner: {message}")  # Now "inner"!
    print(f"Inner returns: {result}")  # Inner returns: inner

print("\nWith nonlocal:")  # (blank line)
outer_function_fixed()


# Real-life Example 1: Avoiding mutable default in API client
class APIClient:
    """API client with proper default handling."""
    
    def __init__(self, base_url: str, headers: dict = None):
        """Initialize with proper default handling."""
        self.base_url = base_url
        self.headers = headers if headers is not None else {}
    
    def add_header(self, key: str, value: str):
        """Add header without affecting defaults."""
        self.headers[key] = value
    
    def get_headers(self):
        """Get current headers."""
        return self.headers.copy()


print("\n=== Real-life Example 1: API Client ===")  # (blank line)
client1 = APIClient("https://api1.com")
client1.add_header("Authorization", "Bearer token")

client2 = APIClient("https://api2.com")
print(f"Client 1 headers: {client1.get_headers()}")  # Client 1 headers: {'Authorization': 'Bearer token'}
print(f"Client 2 headers: {client2.get_headers()}")  # Client 2 headers: {}


# Real-life Example 2: Event handlers with proper closure
class EventManager:
    """Event manager that properly handles closures."""
    
    def __init__(self):
        self.handlers = {}
    
    def register(self, event: str):
        """Register a handler that captures the event name."""
        def handler(func):
            self.handlers[event] = func
            return func
        return handler
    
    def trigger(self, event: str, data):
        """Trigger an event."""
        if event in self.handlers:
            return self.handlers[event](data)
        return None


def handle_user_created(data):
    return f"User created: {data['name']}"


manager = EventManager()
manager.register("user.created")(handle_user_created)

print("\n=== Real-life Example 2: Event Manager ===")  # (blank line)
result = manager.trigger("user.created", {"name": "Alice", "id": 1})
print(f"Result: {result}")  # Result: User created: Alice


# Real-life Example 3: Database connection with proper state
class DatabaseConnection:
    """Database connection with proper scope handling."""
    
    def __init__(self, dsn: str):
        self.dsn = dsn
        self._connection = None
        self._transactions = 0
    
    def connect(self):
        """Establish connection."""
        self._connection = {"status": "connected", "dsn": self.dsn}
        return self
    
    def begin_transaction(self):
        """Begin a new transaction."""
        if self._transactions == 0:
            print("Transaction started")
        self._transactions += 1
    
    def commit(self):
        """Commit current transaction."""
        if self._transactions > 0:
            self._transactions -= 1
        if self._transactions == 0:
            print("Transaction committed")
    
    def rollback(self):
        """Rollback current transaction."""
        self._transactions = 0
        print("Transaction rolled back")
    
    @property
    def in_transaction(self):
        """Check if in transaction."""
        return self._transactions > 0


print("\n=== Real-life Example 3: Database Connection ===")  # (blank line)
db = DatabaseConnection("postgresql://localhost/mydb")
db.connect()
db.begin_transaction()  # Transaction started
db.begin_transaction()
print(f"In transaction: {db.in_transaction}")  # In transaction: True
db.commit()
print(f"In transaction after 1 commit: {db.in_transaction}")  # In transaction after 1 commit: True
db.commit()  # Transaction committed
print(f"In transaction after 2 commits: {db.in_transaction}")  # In transaction after 2 commits: False


# Real-life Example 4: Timer with proper state management
class Timer:
    """Timer with proper state management."""
    
    def __init__(self):
        self._start_time = None
        self._elapsed = 0
    
    def start(self):
        """Start the timer."""
        import time
        self._start_time = time.time()
    
    def stop(self):
        """Stop the timer and accumulate elapsed time."""
        import time
        if self._start_time:
            self._elapsed += time.time() - self._start_time
            self._start_time = None
        return self._elapsed
    
    def reset(self):
        """Reset all accumulated time."""
        self._start_time = None
        self._elapsed = 0
    
    def get_elapsed(self):
        """Get current elapsed time."""
        import time
        if self._start_time:
            return self._elapsed + (time.time() - self._start_time)
        return self._elapsed


import time

print("\n=== Real-life Example 4: Timer ===")  # (blank line)
timer = Timer()
timer.start()
time.sleep(0.1)
print(f"After 1st stop: {timer.stop():.3f}s")  # After 1st stop: 0.100s (varies)
timer.start()
time.sleep(0.1)
print(f"After 2nd stop: {timer.get_elapsed():.3f}s")  # After 2nd stop: 0.200s (varies)
timer.reset()
print(f"After reset: {timer.get_elapsed():.3f}s")  # After reset: 0.000s


# Real-life Example 5: Cache with proper closure
def create_cache():
    """Create a cache with proper state."""
    cache = {}
    
    def get(key: str, compute_fn):
        """Get from cache or compute."""
        if key in cache:
            return cache[key]
        result = compute_fn()
        cache[key] = result
        return result
    
    def invalidate(key: str):
        """Invalidate a cache entry."""
        if key in cache:
            del cache[key]
    
    def clear():
        """Clear entire cache."""
        cache.clear()
    
    def size():
        """Get cache size."""
        return len(cache)
    
    return get, invalidate, clear, size

get_cached, invalidate_c, clear_c, size_c = create_cache()

def expensive_operation(n):
    """Simulate expensive operation."""
    return n * 2

print("\n=== Real-life Example 5: Cache ===")  # (blank line)
print(f"Size after first call: {size_c()}")  # Size after first call: 0
result1 = get_cached("key1", lambda: expensive_operation(21))
print(f"Result: {result1}, Size: {size_c()}")  # Result: 42, Size: 1
result2 = get_cached("key1", lambda: expensive_operation(99))  # Uses cached value
print(f"Result (cached): {result2}, Size: {size_c()}")  # Result (cached): 42, Size: 1
invalidate_c("key1")
print(f"Size after invalidate: {size_c()}")  # Size after invalidate: 0


# Real-life Example 6: Function decorator with proper scope
def memoize(func):
    """Memoization decorator with proper closure."""
    cache = {}
    
    def wrapper(*args, **kwargs):
        # Create a hashable key from args and kwargs
        key = str(args) + str(sorted(kwargs.items()))
        
        if key not in cache:
            cache[key] = func(*args, **kwargs)
        return cache[key]
    
    wrapper.cache = cache
    wrapper.clear_cache = lambda: cache.clear()
    return wrapper


@memoize
def fibonacci(n):
    """Calculate fibonacci number."""
    if n < 2:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)


print("\n=== Real-life Example 6: Memoization ===")  # (blank line)
result = fibonacci(10)
print(f"Fibonacci(10): {result}")  # Fibonacci(10): 55
print(f"Cache size: {len(fibonacci.clear_cache())}")  # Cache size: 11
fibonacci.clear_cache()
# Call again to repopulate
fibonacci(10)
print(f"Cache size after recalc: {len(fibonacci.cache)}")  # Cache size after recalc: 11
