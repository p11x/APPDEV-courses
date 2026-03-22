# Example58.py
# Topic: Advanced Decorator Patterns

# This file demonstrates advanced decorator patterns including
# decorators with arguments, stacking decorators, and more.


# ============================================================
# Decorator with Arguments
# ============================================================
print("=== Decorator with Arguments ===")

def repeat(times):
    """Decorator that repeats function execution."""
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
print(f"Results: {results}")


# ============================================================
# Multiple Decorators
# ============================================================
print("\n=== Multiple Decorators ===")

def uppercase(func):
    """Convert result to uppercase."""
    def wrapper(*args, **kwargs):
        result = func(*args, **kwargs)
        return result.upper() if isinstance(result, str) else result
    return wrapper

def exclamation(func):
    """Add exclamation to result."""
    def wrapper(*args, **kwargs):
        result = func(*args, **kwargs)
        return result + "!" if isinstance(result, str) else result
    return wrapper

@uppercase
def get_message():
    return "hello world"

print(f"Uppercase: {get_message()}")

@exclamation
def get_message2():
    return "hello world"

print(f"Exclamation: {get_message2()}")

@uppercase
@exclamation
def get_message3():
    return "hello world"

print(f"Both: {get_message3()}")


# ============================================================
# Decorator That Modifies Arguments
# ============================================================
print("\n=== Decorator That Modifies Arguments ===")

def sanitize_input(func):
    """Clean and validate input arguments."""
    def wrapper(*args, **kwargs):
        # Sanitize string arguments
        sanitized_args = []
        for arg in args:
            if isinstance(arg, str):
                arg = arg.strip()
            sanitized_args.append(arg)
        
        sanitized_kwargs = {}
        for key, value in kwargs.items():
            if isinstance(value, str):
                value = value.strip()
            sanitized_kwargs[key] = value
        
        return func(*sanitized_args, **sanitized_kwargs)
    return wrapper

@sanitize_input
def create_user(name, email):
    return {"name": name, "email": email}

user = create_user("  Alice  ", "  alice@example.com  ")
print(f"User: {user}")


# ============================================================
# Decorator That Returns Different Type
# ============================================================
print("\n=== Decorator That Returns Different Type ===")

def listify(func):
    """Ensure result is a list."""
    def wrapper(*args, **kwargs):
        result = func(*args, **kwargs)
        if isinstance(result, str):
            return [result]
        return list(result) if hasattr(result, '__iter__') else [result]
    return wrapper

@listify
def get_single_value():
    return "hello"

@listify
def get_values():
    return "hello", "world"

print(f"Single: {get_single_value()}")
print(f"Multiple: {get_values()}")


# ============================================================
# Class as Decorator
# ============================================================
print("\n=== Class as Decorator ===")

class CallCounter:
    """Decorator class that counts calls."""
    
    def __init__(self, func):
        self.func = func
        self.call_count = 0
    
    def __call__(self, *args, **kwargs):
        self.call_count += 1
        print(f"Call #{self.call_count} to {self.func.__name__}")
        return self.func(*args, **kwargs)

@CallCounter
def say_hello():
    return "Hello!"

for _ in range(3):
    say_hello()


# ============================================================
# Real-life Example 1: Rate Limiter
# ============================================================
print("\n=== Real-life: Rate Limiter ===")

import time

def rate_limit(max_calls, period):
    """Decorator that limits how often a function can be called."""
    calls = []
    
    def decorator(func):
        def wrapper(*args, **kwargs):
            now = time.time()
            # Remove old calls
            calls[:] = [c for c in calls if now - c < period]
            
            if len(calls) >= max_calls:
                raise RuntimeError(f"Rate limit exceeded: max {max_calls} calls per {period}s")
            
            calls.append(now)
            return func(*args, **kwargs)
        return wrapper
    return decorator

@rate_limit(max_calls=3, period=5)
def api_request(endpoint):
    return f"Response from {endpoint}"

for i in range(3):
    try:
        result = api_request(f"/api/{i}")
        print(f"Request {i}: {result}")
    except RuntimeError as e:
        print(f"Request {i}: {e}")


# ============================================================
# Real-life Example 2: Memoization with TTL
# ============================================================
print("\n=== Real-life: Memoization with TTL ===")

import time

def memoize_ttl(ttl_seconds):
    """Decorator with time-to-live cache."""
    cache = {}
    
    def decorator(func):
        def wrapper(*args):
            now = time.time()
            
            if args in cache:
                result, timestamp = cache[args]
                if now - timestamp < ttl_seconds:
                    return result
            
            result = func(*args)
            cache[args] = (result, now)
            return result
        return wrapper
    return decorator

@memoize_ttl(ttl_seconds=2)
def expensive_operation(n):
    print(f"Computing for {n}...")
    time.sleep(0.1)
    return n * 2

# First call
result1 = expensive_operation(5)
print(f"Result: {result1}")

# Second call (cached)
result2 = expensive_operation(5)
print(f"Result: {result2}")

# Wait for TTL
print("Waiting for cache to expire...")
time.sleep(3)

# Third call (recomputed)
result3 = expensive_operation(5)
print(f"Result: {result3}")


# ============================================================
# Real-life Example 3: Deprecation Warning
# ============================================================
print("\n=== Real-life: Deprecation Warning ===")

import warnings

def deprecated(func):
    """Mark function as deprecated."""
    def wrapper(*args, **kwargs):
        warnings.warn(
            f"{func.__name__} is deprecated and will be removed in future versions",
            DeprecationWarning,
            stacklevel=2
        )
        return func(*args, **kwargs)
    return wrapper

@deprecated
def old_function():
    return "This is the old function"

result = old_function()
print(f"Result: {result}")


# ============================================================
# Real-life Example 4: Type Checking
# ============================================================
print("\n=== Real-life: Type Checking ===")

def type_check(*arg_types, **kwarg_types):
    """Decorator that checks argument types."""
    def decorator(func):
        def wrapper(*args, **kwargs):
            # Check positional arguments
            for i, (arg, expected_type) in enumerate(zip(args, arg_types)):
                if not isinstance(arg, expected_type):
                    raise TypeError(
                        f"Argument {i} to {func.__name__} must be {expected_type.__name__}, "
                        f"got {type(arg).__name__}"
                    )
            
            # Check keyword arguments
            for kwarg, expected_type in kwarg_types.items():
                if kwarg in kwargs:
                    value = kwargs[kwarg]
                    if not isinstance(value, expected_type):
                        raise TypeError(
                            f"Keyword argument '{kwarg}' to {func.__name__} must be "
                            f"{expected_type.__name__}, got {type(value).__name__}"
                        )
            
            return func(*args, **kwargs)
        return wrapper
    return decorator

@type_check(int, int, result_type=int)
def add(a, b, result_type=int):
    return a + b

result = add(5, 3)
print(f"Result: {result}")

try:
    result = add("5", 3)
except TypeError as e:
    print(f"Error: {e}")


# ============================================================
# Real-life Example 5: Retry with Backoff
# ============================================================
print("\n=== Real-life: Retry with Backoff ===")

import random

def retry_with_backoff(max_attempts, backoff_factor):
    """Decorator with exponential backoff."""
    def decorator(func):
        def wrapper(*args, **kwargs):
            for attempt in range(max_attempts):
                try:
                    return func(*args, **kwargs)
                except Exception as e:
                    if attempt == max_attempts - 1:
                        raise
                    wait_time = backoff_factor * (2 ** attempt)
                    print(f"Attempt {attempt + 1} failed: {e}. Retrying in {wait_time}s...")
                    time.sleep(wait_time)
        return wrapper
    return decorator

@retry_with_backoff(max_attempts=3, backoff_factor=0.1)
def unreliable_api_call():
    if random.random() < 0.7:
        raise ConnectionError("Network error")
    return "API Response Success!"

try:
    result = unreliable_api_call()
    print(f"Result: {result}")
except Exception as e:
    print(f"Failed: {e}")


# ============================================================
# Real-life Example 6: Performance Benchmark
# ============================================================
print("\n=== Real-life: Performance Benchmark ===")

def benchmark(func):
    """Decorator that benchmarks function performance."""
    import time
    
    def wrapper(*args, **kwargs):
        start = time.perf_counter()
        result = func(*args, **kwargs)
        end = time.perf_counter()
        
        print(f"{func.__name__} took {(end - start) * 1000:.4f}ms")
        return result
    return wrapper

@benchmark
def quick_operation():
    return sum(range(1000))

@benchmark
def slow_operation():
    time.sleep(0.1)
    return "Done!"

quick_operation()
slow_operation()


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("ADVANCED PATTERNS:")
print("=" * 50)
print("""
1. DECORATOR WITH ARGUMENTS:
   - Need triple nesting
   - def decorator(arg): -> def decorator(func): -> def wrapper()

2. MULTIPLE DECORATORS:
   - Applied bottom to top
   - @uppercase
   @exclamation

3. CLASS AS DECORATOR:
   - Implement __call__ method
   - Maintain state

4. COMMON PATTERNS:
   - Rate limiting
   - Caching/Memoization
   - Type checking
   - Retry logic
   - Deprecation warnings
   - Performance benchmarking
""")
