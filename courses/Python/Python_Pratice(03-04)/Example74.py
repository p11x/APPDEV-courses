# Example74.py
# Topic: Real-World Examples with functools

# This file provides practical real-world examples using functools.


# ============================================================
# Example 1: Memoization for expensive operations
# ============================================================
print("=== Real-world: API Response Caching ===")

from functools import lru_cache
import json

@lru_cache(maxsize=100)
def get_user_profile(user_id):
    """Cache user profiles to reduce API calls."""
    # Simulate database lookup
    profiles = {
        1: {"name": "Alice", "email": "alice@example.com"},
        2: {"name": "Bob", "email": "bob@example.com"},
    }
    return profiles.get(user_id, {"name": "Unknown", "email": "N/A"})

# First call - fetches from DB
print(f"User 1: {get_user_profile(1)}")

# Second call - from cache
print(f"User 1 (cached): {get_user_profile(1)}")
print(f"Cache stats: {get_user_profile.cache_info()}")


# ============================================================
# Example 2: Partial for formatters
# ============================================================
print("\n=== Real-world: Data Formatting ===")

from functools import partial

def format_number(value, prefix, suffix, decimals):
    """Generic number formatter."""
    formatted = f"{value:.{decimals}f}" if decimals else str(int(value))
    return f"{prefix}{formatted}{suffix}"

# Create specific formatters
format_currency = partial(format_number, prefix="$", suffix="", decimals=2)
format_percentage = partial(format_number, prefix="", suffix="%", decimals=1)
format_score = partial(format_number, prefix="", suffix=" pts", decimals=0)

print(f"Price: {format_currency(1234.56)}")
print(f"Completion: {format_percentage(87.5)}")
print(f"Score: {format_score(1500)}")


# ============================================================
# Example 3: cached_property for expensive computations
# ============================================================
print("\n=== Real-world: Expensive Computations ===")

from functools import cached_property

class Report:
    def __init__(self, data):
        self.data = data
    
    @cached_property
    def summary(self):
        """Expensive aggregation - computed once."""
        print("Computing summary...")
        return {
            'total': sum(self.data),
            'count': len(self.data),
            'avg': sum(self.data) / len(self.data)
        }
    
    @cached_property
    def statistics(self):
        """Another expensive computation."""
        print("Computing statistics...")
        return {
            'min': min(self.data),
            'max': max(self.data),
            'range': max(self.data) - min(self.data)
        }

report = Report([10, 20, 30, 40, 50])

# First access - computes
print(f"Summary: {report.summary}")

# Second access - cached
print(f"Summary again: {report.summary}")


# ============================================================
# Example 4: Event system with partial
# ============================================================
print("\n=== Real-world: Event System ===")

from functools import partial

class EventBus:
    def __init__(self):
        self.handlers = {}
    
    def subscribe(self, event_type, handler):
        if event_type not in self.handlers:
            self.handlers[event_type] = []
        self.handlers[event_type].append(handler)
    
    def publish(self, event_type, data):
        if event_type in self.handlers:
            for handler in self.handlers[event_type]:
                handler(data)

bus = EventBus()

# Create specific handlers
def send_email(to, message):
    print(f"Email to {to}: {message}")

def log_to_file(filename, message):
    print(f"Logged to {filename}: {message}")

# Subscribe with partial
email_alert = partial(send_email, "admin@example.com")
bus.subscribe("alert", email_alert)
bus.subscribe("log", partial(log_to_file, "events.log"))

# Publish
bus.publish("alert", "System error!")
bus.publish("log", "Error occurred")


# ============================================================
# Example 5: Reduce for aggregation
# ============================================================
print("\n=== Real-world: Data Aggregation ===")

from functools import reduce

transactions = [
    {'type': 'credit', 'amount': 100},
    {'type': 'debit', 'amount': 30},
    {'type': 'credit', 'amount': 200},
    {'type': 'debit', 'amount': 50},
]

# Calculate totals
credits = reduce(lambda acc, t: acc + t['amount'], 
                 filter(lambda t: t['type'] == 'credit', transactions), 0)
debits = reduce(lambda acc, t: acc + t['amount'], 
                filter(lambda t: t['type'] == 'debit', transactions), 0)

print(f"Total credits: ${credits}")
print(f"Total debits: ${debits}")
print(f"Net: ${credits - debits}")


# ============================================================
# Example 6: Sorting with custom keys
# ============================================================
print("\n=== Real-world: Custom Sorting ===")

from functools import cmp_to_key

class Employee:
    def __init__(self, name, dept, salary):
        self.name = name
        self.dept = dept
        self.salary = salary
    def __repr__(self):
        return f"{self.name}({self.dept}, ${self.salary})"

employees = [
    Employee("Alice", "IT", 90000),
    Employee("Bob", "HR", 70000),
    Employee("Charlie", "IT", 85000),
    Employee("Diana", "Sales", 75000),
]

def compare_employees(e1, e2):
    # Sort by department first, then salary descending
    if e1.dept != e2.dept:
        return -1 if e1.dept < e2.dept else 1
    return -1 if e1.salary > e2.salary else (1 if e1.salary < e2.salary else 0)

sorted_emp = sorted(employees, key=cmp_to_key(compare_employees))
print("Sorted by dept, then salary:")
for e in sorted_emp:
    print(f"  {e}")


# ============================================================
# Example 7: Function overloading with singledispatch
# ============================================================
print("\n=== Real-world: Type-specific Processing ===")

from functools import singledispatch

@singledispatch
def process_data(data):
    print(f"Unknown type: {data}")

@process_data.register
def process_dict(data):
    total = sum(data.values())
    print(f"Dict sum: {total}")

@process_data.register  
def process_list(data):
    print(f"List length: {len(data)}")

@process_data.register
def process_string(data):
    print(f"String upper: {data.upper()}")

process_data({'a': 10, 'b': 20})
process_data([1, 2, 3, 4])
process_data("hello world")


# ============================================================
# Example 8: Decorator pattern with wraps
# ============================================================
print("\n=== Real-world: Logging Decorator ===")

from functools import wraps
import time

def logged(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print(f"Calling {func.__name__} with {args}, {kwargs}")
        start = time.perf_counter()
        result = func(*args, **kwargs)
        elapsed = time.perf_counter() - start
        print(f"{func.__name__} returned {result} in {elapsed:.4f}s")
        return result
    return wrapper

@logged
def slow_function():
    time.sleep(0.1)
    return "Done"

result = slow_function()
print(f"Function name preserved: {slow_function.__name__}")


# ============================================================
# Example 9: Retry with cache
# ============================================================
print("\n=== Real-world: Cached Retry ===")

from functools import lru_cache, wraps
import time

@lru_cache(maxsize=10)
def fetch_from_api(endpoint, params=None):
    """Cached API call."""
    print(f"Fetching {endpoint}...")
    time.sleep(0.1)  # Simulate network
    return {"data": f"Result from {endpoint}"}

# First call
print(fetch_from_api("/users"))

# Cached
print(fetch_from_api("/users"))

print(f"Cache info: {fetch_from_api.cache_info()}")


# ============================================================
# Example 10: Complex data pipeline
# ============================================================
print("\n=== Real-world: Data Pipeline ===")

from functools import reduce, partial

def pipeline(*functions):
    """Create a function pipeline."""
    def apply(data):
        return reduce(lambda d, f: f(d), functions, data)
    return apply

# Define transformations
add_id = lambda items: [{'id': i, **item} for i, item in enumerate(items)]
filter_active = lambda items: [item for item in items if item.get('active', True)]
sort_by_name = lambda items: sorted(items, key=lambda x: x.get('name', ''))

# Create pipeline
process = pipeline(add_id, filter_active, sort_by_name)

data = [
    {'name': 'Charlie', 'active': True},
    {'name': 'Alice', 'active': False},
    {'name': 'Bob', 'active': True},
]

result = process(data)
print("Processed:")
for item in result:
    print(f"  {item}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("REAL-WORLD functools USES")
print("=" * 50)
print("""
- Caching: @lru_cache, @cache, @cached_property
- Partial: Pre-configure functions for callbacks
- Reduce: Aggregate data
- Wraps: Preserve metadata in decorators
- Singledispatch: Type-specific functions
- cmp_to_key: Custom sorting
""")
