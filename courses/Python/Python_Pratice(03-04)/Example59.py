# Example59.py
# Topic: Comprehensive Decorator Examples

# This file provides comprehensive real-world decorator examples.


# ============================================================
# Example 1: Authentication Decorator
# ============================================================
print("=== Real-world: Authentication ===")

from functools import wraps

def authenticate(func):
    """Decorator to check if user is authenticated."""
    @wraps(func)
    def wrapper(*args, **kwargs):
        token = kwargs.get('auth_token')
        if not token or token != 'valid-token-123':
            raise PermissionError("Invalid or missing authentication token")
        return func(*args, **kwargs)
    return wrapper

def require_role(role):
    """Decorator to check user role."""
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            user_role = kwargs.get('user_role', 'guest')
            if user_role != role and user_role != 'admin':
                raise PermissionError(f"Requires {role} role, got {user_role}")
            return func(*args, **kwargs)
        return wrapper
    return decorator

@authenticate
@require_role('admin')
def delete_user(user_id, auth_token=None, user_role='admin'):
    return f"User {user_id} deleted successfully"

# Test
try:
    result = delete_user(123, auth_token='valid-token-123', user_role='admin')
    print(f"Success: {result}")
except PermissionError as e:
    print(f"Error: {e}")


# ============================================================
# Example 2: Validation Decorator
# ============================================================
print("\n=== Real-world: Validation ===")

def validate(*arg_names, **kwarg_validators):
    """Decorator to validate function arguments."""
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            # Get function signature
            import inspect
            sig = inspect.signature(func)
            bound = sig.bind(*args, **kwargs)
            bound.apply_defaults()
            
            # Validate positional arguments
            for i, name in enumerate(arg_names):
                if name in bound.arguments:
                    value = bound.arguments[name]
                    if isinstance(value, (int, float)) and value < 0:
                        raise ValueError(f"Argument '{name}' must be non-negative, got {value}")
            
            # Validate keyword arguments
            for name, validator in kwarg_validators.items():
                if name in bound.arguments:
                    value = bound.arguments[name]
                    if not validator(value):
                        raise ValueError(f"Validation failed for '{name}' = {value}")
            
            return func(*args, **kwargs)
        return wrapper
    return decorator

@validate('age', 'score', is_active=lambda x: isinstance(x, bool))
def register_user(name, age, score, is_active=True):
    return f"Registered: {name}, age={age}, score={score}, active={is_active}"

try:
    result = register_user("Alice", 25, 95, True)
    print(f"Success: {result}")
    
    result = register_user("Bob", -5, 90, True)
except ValueError as e:
    print(f"Error: {e}")


# ============================================================
# Example 3: Timing Decorator
# ============================================================
print("\n=== Real-world: Timing ===")

import time

def timing(func):
    """Decorator to time function execution."""
    @wraps(func)
    def wrapper(*args, **kwargs):
        start = time.perf_counter()
        result = func(*args, **kwargs)
        end = time.perf_counter()
        duration = end - start
        print(f"{func.__name__} executed in {duration:.6f} seconds")
        return result
    return wrapper

@timing
def fetch_data():
    time.sleep(0.1)
    return "Data fetched"

result = fetch_data()


# ============================================================
# Example 4: Caching Decorator
# ============================================================
print("\n=== Real-world: Caching ===")

def cached(func):
    """Simple caching decorator."""
    cache = {}
    
    @wraps(func)
    def wrapper(*args):
        if args in cache:
            print(f"Cache hit for {args}")
            return cache[args]
        
        result = func(*args)
        cache[args] = result
        return result
    
    wrapper.cache = cache
    wrapper.clear_cache = lambda: cache.clear()
    return wrapper

@cached
def fibonacci(n):
    if n <= 1:
        return n
    return fibonacci(n-1) + fibonacci(n-2)

result = fibonacci(10)
print(f"Fibonacci(10): {result}")
print(f"Cache: {fibonacci.cache}")


# ============================================================
# Example 5: Retry Decorator
# ============================================================
print("\n=== Real-world: Retry ===")

def retry(max_attempts=3, delay=1, backoff=2):
    """Decorator to retry failed function calls."""
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            import time
            attempts = 0
            current_delay = delay
            
            while attempts < max_attempts:
                try:
                    return func(*args, **kwargs)
                except Exception as e:
                    attempts += 1
                    if attempts == max_attempts:
                        raise
                    print(f"Attempt {attempts} failed: {e}. Retrying in {current_delay}s...")
                    time.sleep(current_delay)
                    current_delay *= backoff
            
            return None
        return wrapper
    return decorator

@retry(max_attempts=3, delay=0.1)
def unstable_operation(should_fail=True):
    if should_fail and __import__('random').random() < 1:
        raise ConnectionError("Connection failed")
    return "Success!"

try:
    result = unstable_operation(False)
    print(f"Result: {result}")
except Exception as e:
    print(f"Failed after retries: {e}")


# ============================================================
# Example 6: Decorator for Class Methods
# ============================================================
print("\n=== Real-world: Class Method Decorators ===")

def log_method_call(func):
    """Decorator for class methods."""
    @wraps(func)
    def wrapper(self, *args, **kwargs):
        print(f"Calling {func.__name__} on {self.__class__.__name__}")
        result = func(self, *args, **kwargs)
        print(f"{func.__name__} completed")
        return result
    return wrapper

class DataProcessor:
    def __init__(self):
        self.data = []
    
    @log_method_call
    def add(self, item):
        self.data.append(item)
        return len(self.data)
    
    @log_method_call
    def remove(self):
        if self.data:
            return self.data.pop()
        return None
    
    @log_method_call
    def process(self):
        return [x * 2 for x in self.data]

processor = DataProcessor()
processor.add(1)
processor.add(2)
processor.add(3)
processor.process()
processor.remove()


# ============================================================
# Example 7: Decorator Factory
# ============================================================
print("\n=== Real-world: Decorator Factory ===")

def measure(label):
    """Factory for measuring operations."""
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            import time
            start = time.perf_counter()
            result = func(*args, **kwargs)
            end = time.perf_counter()
            print(f"[{label}] {func.__name__}: {(end-start)*1000:.2f}ms")
            return result
        return wrapper
    return decorator

@measure("DATABASE")
def query_database():
    import time
    time.sleep(0.05)
    return [{"id": 1, "name": "Alice"}]

@measure("API")
def call_api():
    import time
    time.sleep(0.03)
    return {"status": "ok"}

query_database()
call_api()


# ============================================================
# Example 8: Property with Validation
# ============================================================
print("\n=== Real-world: Property Validation ===")

class Account:
    """Bank account with property validation."""
    
    def __init__(self, account_id, balance=0):
        self._account_id = account_id
        self._balance = balance
    
    @property
    def account_id(self):
        return self._account_id
    
    @property
    def balance(self):
        return self._balance
    
    @balance.setter
    def balance(self, value):
        if value < 0:
            raise ValueError("Balance cannot be negative")
        self._balance = value
    
    @property
    def formatted_balance(self):
        return f"${self._balance:,.2f}"

account = Account("ACC-001", 1000)
print(f"Account: {account.account_id}")
print(f"Balance: {account.formatted_balance}")

account.balance = 500
print(f"After change: {account.formatted_balance}")

try:
    account.balance = -100
except ValueError as e:
    print(f"Error: {e}")


# ============================================================
# Example 9: Static and Class Methods Combined
# ============================================================
print("\n=== Real-world: Static and Class Methods ===")

class Temperature:
    """Temperature conversion utilities."""
    
    def __init__(self, celsius):
        self._celsius = celsius
    
    @property
    def celsius(self):
        return self._celsius
    
    @celsius.setter
    def celsius(self, value):
        self._celsius = value
    
    @property
    def fahrenheit(self):
        return self._celsius * 9/5 + 32
    
    @staticmethod
    def c_to_f(c):
        """Convert Celsius to Fahrenheit."""
        return c * 9/5 + 32
    
    @staticmethod
    def f_to_c(f):
        """Convert Fahrenheit to Celsius."""
        return (f - 32) * 5/9
    
    @classmethod
    def from_fahrenheit(cls, f):
        """Create from Fahrenheit."""
        c = (f - 32) * 5/9
        return cls(c)

temp_c = Temperature(25)
print(f"25°C = {temp_c.fahrenheit:.1f}°F")

temp_f = Temperature.from_fahrenheit(77)
print(f"77°F = {temp_f.celsius:.1f}°C")


# ============================================================
# Example 10: Complete Example
# ============================================================
print("\n=== Complete Example: API Handler ===")

class APIHandler:
    """API handler with multiple decorators."""
    
    def __init__(self):
        self._cache = {}
        self._calls = 0
    
    def cache_result(self, func):
        """Cache decorator."""
        @wraps(func)
        def wrapper(*args, **kwargs):
            key = str(args) + str(kwargs)
            if key in self._cache:
                return self._cache[key]
            result = func(*args, **kwargs)
            self._cache[key] = result
            return result
        return wrapper
    
    def count_calls(self, func):
        """Count calls decorator."""
        @wraps(func)
        def wrapper(*args, **kwargs):
            self._calls += 1
            return func(*args, **kwargs)
        return wrapper
    
    def log_calls(self, func):
        """Log calls decorator."""
        @wraps(func)
        def wrapper(*args, **kwargs):
            print(f"[API] {func.__name__} called with {args}, {kwargs}")
            return func(*args, **kwargs)
        return wrapper
    
    @count_calls
    @log_calls
    @cache_result
    def get_user(self, user_id):
        return {"id": user_id, "name": f"User {user_id}"}
    
    @count_calls
    @log_calls
    def create_user(self, name, email):
        return {"id": 999, "name": name, "email": email}
    
    def get_stats(self):
        return {"calls": self._calls, "cached": len(self._cache)}

handler = APIHandler()

print("First call:")
user1 = handler.get_user(1)
print(f"User: {user1}")

print("\nSecond call (cached):")
user2 = handler.get_user(1)
print(f"User: {user2}")

print("\nCreating user:")
handler.create_user("Alice", "alice@example.com")

print(f"\nStats: {handler.get_stats()}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMPREHENSIVE REVIEW:")
print("=" * 50)
print("""
DECORATORS IN PYTHON:

1. BASIC DECORATOR:
   def decorator(func):
       def wrapper(*args, **kwargs):
           # code before
           result = func(*args, **kwargs)
           # code after
           return result
       return wrapper

2. DECORATOR WITH ARGUMENTS:
   def decorator(arg):
       def decorator(func):
           def wrapper(*args, **kwargs):
               return func(*args, **kwargs)
           return wrapper
       return decorator

3. PRESERVE METADATA:
   from functools import wraps
   @wraps(func)

4. COMMON USE CASES:
   - Authentication/Authorization
   - Validation
   - Timing
   - Caching
   - Retry logic
   - Logging

5. BUILT-IN DECORATORS:
   - @property
   - @staticmethod
   - @classmethod
""")
