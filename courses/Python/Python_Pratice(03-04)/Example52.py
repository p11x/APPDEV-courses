# Example52.py
# Topic: Closures and Scope - Advanced Patterns

# This file demonstrates advanced closure patterns, common mistakes,
# and practical applications.


# ============================================================
# Advanced Closure Pattern: Multiple Levels
# ============================================================
print("=== Advanced: Multiple Closure Levels ===")

def outer(x):
    """Outer function."""
    def middle(y):
        """Middle function."""
        def inner(z):
            """Inner function - accesses all levels."""
            return x + y + z
        return inner
    return middle

# Create closures at different levels
add_5_and_10 = outer(5)(10)
print(f"5 + 10 + 3 = {add_5_and_10(3)}")  # 18

# Chain creation
step1 = outer(10)
step2 = step1(20)
result = step2(30)
print(f"10 + 20 + 30 = {result}")  # 60


# ============================================================
# Advanced Closure: Conditional Closure
# ============================================================
print("\n=== Advanced: Conditional Closure ===")

def make_operation(operation):
    """Create different operation functions."""
    if operation == "add":
        return lambda a, b: a + b
    elif operation == "multiply":
        return lambda a, b: a * b
    elif operation == "power":
        return lambda a, b: a ** b
    else:
        return lambda a, b: None

add_fn = make_operation("add")
multiply_fn = make_operation("multiply")
power_fn = make_operation("power")

print(f"5 + 3 = {add_fn(5, 3)}")
print(f"5 * 3 = {multiply_fn(5, 3)}")
print(f"5 ** 3 = {power_fn(5, 3)}")


# ============================================================
# Advanced Closure: Closure with List/Dict (no nonlocal needed)
# ============================================================
print("\n=== Advanced: Mutable Container Trick ===")

def make_tracker():
    """Create a tracker using mutable container."""
    data = {"count": 0, "items": []}
    
    def increment(item=None):
        data["count"] += 1
        if item:
            data["items"].append(item)
        return data["count"]
    
    def get_count():
        return data["count"]
    
    def get_items():
        return data["items"].copy()
    
    def reset():
        data["count"] = 0
        data["items"].clear()
    
    return increment, get_count, get_items, reset

increment, get_count, get_items, reset = make_tracker()

print(f"Count: {increment('first')}")   # 1
print(f"Count: {increment('second')}")  # 2
print(f"Items: {get_items()}")
print(f"Count: {get_count()}")
reset()
print(f"After reset: {get_count()}")


# ============================================================
# Common Mistake: Forgetting nonlocal
# ============================================================
print("\n=== Common Mistake: Without nonlocal ===")

# WRONG - This will cause UnboundLocalError
def counter_without_nonlocal():
    count = 0
    
    def increment():
        # Without nonlocal, this creates a new local variable!
        # count = count + 1  # Would cause UnboundLocalError!
        return count  # Just reading works
    
    return increment

# CORRECT - Using nonlocal
def counter_with_nonlocal():
    count = 0
    
    def increment():
        nonlocal count
        count += 1
        return count
    
    return increment

counter = counter_with_nonlocal()
print(f"Counter: {counter()}")  # 1
print(f"Counter: {counter()}")  # 2
print(f"Counter: {counter()}")  # 3


# ============================================================
# Common Mistake: Global vs nonlocal
# ============================================================
print("\n=== Common Mistake: Global vs Nonlocal ===")

module_level = 10

def modify_global():
    global module_level
    module_level += 1
    return module_level

print(f"Before global mod: {module_level}")
modify_global()
print(f"After global mod: {module_level}")


def enclosing_scope_demo():
    enclosing_var = 100
    
    def modify_enclosing():
        nonlocal enclosing_var
        enclosing_var += 10
        return enclosing_var
    
    return modify_enclosing

modify_enclosing = enclosing_scope_demo()
print(f"After enclosing mod: {modify_enclosing()}")


# ============================================================
# Real-life Example 1: Decorator Factory
# ============================================================
print("\n=== Real-life: Decorator Factory ===")

def repeat(times):
    """Decorator factory that repeats function calls."""
    def decorator(func):
        def wrapper(*args, **kwargs):
            results = []
            for _ in range(times):
                results.append(func(*args, **kwargs))
            return results
        return wrapper
    return decorator

@repeat(times=3)
def greet(name):
    return f"Hello, {name}!"

results = greet("Alice")
for i, r in enumerate(results, 1):
    print(f"Call {i}: {r}")


# ============================================================
# Real-life Example 2: Rate Limiter
# ============================================================
print("\n=== Real-life: Rate Limiter ===")

def make_rate_limiter(max_calls, window_seconds):
    """Create a rate limiter."""
    import time
    calls = []
    
    def allow():
        current = time.time()
        # Remove old calls
        calls[:] = [c for c in calls if current - c < window_seconds]
        
        if len(calls) < max_calls:
            calls.append(current)
            return True, len(calls)
        return False, max_calls - len(calls)
    
    def reset():
        calls.clear()
    
    def status():
        return {"used": len(calls), "remaining": max_calls - len(calls)}
    
    return allow, reset, status

allow, reset, status = make_rate_limiter(max_calls=3, window_seconds=60)

for i in range(5):
    allowed, info = allow()
    print(f"Request {i+1}: {'Allowed' if allowed else 'Denied'} (info: {info})")
print(f"Status: {status()}")


# ============================================================
# Real-life Example 3: Memoization/Caching
# ============================================================
print("\n=== Real-life: Memoization ===")

def make_memoizer():
    """Create a memoization cache."""
    cache = {}
    
    def memoize(func):
        def wrapper(*args):
            if args not in cache:
                cache[args] = func(*args)
            return cache[args]
        
        wrapper.cache = cache
        wrapper.clear = lambda: cache.clear()
        return wrapper
    
    return memoize

memoizer = make_memoizer()

@memoizer
def expensive_function(n):
    """Simulate expensive computation."""
    import time
    time.sleep(0.1)
    return n * n

# First call - slow
import time
start = time.time()
result1 = expensive_function(100)
time1 = time.time() - start

# Second call - fast (cached)
start = time.time()
result2 = expensive_function(100)
time2 = time.time() - start

print(f"First call: {result1} took {time1:.3f}s")
print(f"Second call: {result2} took {time2:.6f}s")


# ============================================================
# Real-life Example 4: Command Pattern
# ============================================================
print("\n=== Real-life: Command Pattern ===")

def make_command(execute_fn, undo_fn=None):
    """Create a command with execute and optional undo."""
    executed = False
    
    def execute(*args, **kwargs):
        nonlocal executed
        result = execute_fn(*args, **kwargs)
        executed = True
        return result
    
    def undo():
        nonlocal executed
        if not executed:
            return "Nothing to undo"
        if undo_fn:
            return undo_fn()
        return "Undo not supported"
    
    def is_executed():
        return executed
    
    return execute, undo, is_executed

# Create commands
def add_to_list(item):
    return f"Added {item}"

def remove_from_list(item):
    return f"Removed {item}"

add_cmd, undo_add, was_added = make_command(add_to_list, lambda: "Undid add")

print(f"Execute: {add_cmd('book')}")
print(f"Was executed: {was_added()}")
print(f"Undo: {undo_add()}")


# ============================================================
# Real-life Example 5: Middleware Pattern
# ============================================================
print("\n=== Real-life: Middleware ===")

def make_middleware_chain():
    """Create a middleware chain."""
    middlewares = []
    
    def use(middleware):
        middlewares.append(middleware)
        return middleware
    
    def process(request):
        response = {"status": 200, "body": ""}
        
        for mw in middlewares:
            request, response = mw(request, response)
        
        return response
    
    return use, process

use_mw, process = make_middleware_chain()

# Add middlewares
@use_mw
def logging_mw(request, response):
    print(f"Logging: {request.get('method')} {request.get('path')}")
    return request, response

@use_mw
def auth_mw(request, response):
    if request.get("requires_auth") and not request.get("user"):
        response["status"] = 401
        response["body"] = "Unauthorized"
    return request, response

@use_mw
def transform_mw(request, response):
    response["body"] = f"Processed: {request.get('path')}"
    return request, response

# Process request
req = {"method": "GET", "path": "/api/users", "requires_auth": True}
resp = process(req)
print(f"Response: {resp['status']} - {resp['body']}")


# ============================================================
# Real-life Example 6: Strategy Pattern
# ============================================================
print("\n=== Real-life: Strategy Pattern ===")

def make_sorter(sort_strategy):
    """Create a sorter with a specific strategy."""
    def sort(data):
        return sort_strategy(data)
    return sort

# Different strategies
ascending = make_sorter(lambda data: sorted(data))
descending = make_sorter(lambda data: sorted(data, reverse=True))
by_length = make_sorter(lambda data: sorted(data, key=len))

numbers = [5, 2, 8, 1, 9]
words = ["cat", "elephant", "dog", "bird"]

print(f"Numbers ascending: {ascending(numbers)}")
print(f"Numbers descending: {descending(numbers)}")
print(f"Words by length: {by_length(words)}")


# ============================================================
# Summary: Best Practices
# ============================================================
print("\n" + "=" * 50)
print("BEST PRACTICES:")
print("=" * 50)
print("""
1. Use nonlocal to modify enclosing scope variables

2. Use mutable containers (list/dict) to share state
   without nonlocal when appropriate

3. Use global only for module-level state

4. Closures are great for:
   - Function factories
   - State management
   - Decorators
   - Caching/Memoization
   - Middleware

5. Avoid:
   - Creating closures in loops (can cause bugs)
   - Modifying global state unnecessarily
   - Too many nested closures (hard to read)
""")
