# Example50.py
# Topic: Lambda Functions - Additional Practical Examples

# This file provides additional practical examples of lambda functions
# in real-world applications.


# ============================================================
# Example 1: Lambda with class methods
# ============================================================
print("=== Lambda with Class Methods ===")

class StringHelper:
    """String manipulation methods."""
    
    @staticmethod
    def transform(strings, transform_func):
        """Transform strings using provided function."""
        return list(map(transform_func, strings))

helper = StringHelper()

words = ["hello", "world", "python"]

# Using lambda
result = helper.transform(words, lambda w: w.upper())
print(f"Uppercased: {result}")

result = helper.transform(words, lambda w: w[0].upper() + w[1:])
print(f"Capitalized: {result}")

result = helper.transform(words, lambda w: "-".join(w))
print(f"With hyphen: {result}")


# ============================================================
# Example 2: Lambda with defaultdict
# ============================================================
print("\n=== Lambda with Collections ===")

from collections import defaultdict, Counter

# Lambda with defaultdict
word_lengths = defaultdict(lambda: "unknown")
word_lengths["apple"] = 5
word_lengths["banana"] = 6

print(f"apple: {word_lengths['apple']}")
print(f"cherry: {word_lengths['cherry']}")  # Returns default

# Lambda with Counter
text = "apple banana apple cherry banana apple"
word_count = Counter(text.split())
print(f"\nWord counts: {word_count}")

# Most common
most_common = word_count.most_common(2)
print(f"Most common: {most_common}")


# ============================================================
# Example 3: Lambda with sorting custom objects
# ============================================================
print("\n=== Lambda with Custom Objects ===")

class Employee:
    def __init__(self, name, salary, department):
        self.name = name
        self.salary = salary
        self.department = department
    
    def __repr__(self):
        return f"Employee({self.name}, ${self.salary}, {self.department})"

employees = [
    Employee("Alice", 75000, "Engineering"),
    Employee("Bob", 55000, "Marketing"),
    Employee("Charlie", 82000, "Engineering"),
    Employee("Diana", 60000, "Sales"),
    Employee("Eve", 70000, "Marketing")
]

# Sort by salary
by_salary = sorted(employees, key=lambda e: e.salary)
print("By salary:")
for e in by_salary:
    print(f"  {e}")

# Sort by department, then salary
by_dept_salary = sorted(employees, key=lambda e: (e.department, e.salary))
print("\nBy department then salary:")
for e in by_dept_salary:
    print(f"  {e}")


# ============================================================
# Example 4: Lambda with datetime operations
# ============================================================
print("\n=== Lambda with DateTime ===")

from datetime import datetime, timedelta

# Sample timestamps
timestamps = [
    "2024-01-15 10:30:00",
    "2024-01-15 14:45:00",
    "2024-01-16 09:00:00",
    "2024-01-16 16:30:00",
    "2024-01-17 11:15:00"
]

# Parse and sort by date
parsed = list(map(lambda t: datetime.strptime(t, "%Y-%m-%d %H:%M:%S"), timestamps))
sorted_dates = sorted(parsed)
print("Sorted dates:")
for d in sorted_dates:
    print(f"  {d.strftime('%Y-%m-%d %H:%M')}")

# Filter by hour
afternoon = list(filter(lambda d: d.hour >= 12, parsed))
print(f"\nAfternoon entries: {len(afternoon)}")

# Group by date
from collections import defaultdict
by_date = defaultdict(list)
for t in parsed:
    by_date[t.date()].append(t)

print("\nBy date:")
for date, times in by_date.items():
    print(f"  {date}: {len(times)} entries")


# ============================================================
# Example 5: Lambda with JSON operations
# ============================================================
print("\n=== Lambda with JSON ===")

import json

# Sample JSON data
json_data = '''
[
    {"id": 1, "name": "Product A", "category": "Electronics", "price": 299},
    {"id": 2, "name": "Product B", "category": "Clothing", "price": 49},
    {"id": 3, "name": "Product C", "category": "Electronics", "price": 599},
    {"id": 4, "name": "Product D", "category": "Books", "price": 19},
    {"id": 5, "name": "Product E", "category": "Clothing", "price": 79}
]
'''

products = json.loads(json_data)

# Filter electronics
electronics = list(filter(lambda p: p["category"] == "Electronics", products))
print("Electronics:")
for p in electronics:
    print(f"  {p['name']}: ${p['price']}")

# Sort by price
by_price = sorted(products, key=lambda p: p["price"])
print(f"\nCheapest: {by_price[0]['name']} (${by_price[0]['price']})")
print(f"Most expensive: {by_price[-1]['name']} (${by_price[-1]['price']})")

# Average price
avg_price = sum(map(lambda p: p["price"], products)) / len(products)
print(f"\nAverage price: ${avg_price:.2f}")


# ============================================================
# Example 6: Lambda with file operations
# ============================================================
print("\n=== Lambda with File Data ===")

# Simulated file lines
file_lines = [
    "2024-01-15 10:30:00 INFO Starting application",
    "2024-01-15 10:30:05 DEBUG Loading configuration",
    "2024-01-15 10:30:10 INFO Application started",
    "2024-01-15 10:35:00 ERROR Database connection failed",
    "2024-01-15 10:35:05 ERROR Retry attempt 1",
    "2024-01-15 10:35:10 INFO Database connected",
    "2024-01-15 11:00:00 INFO User logged in",
    "2024-01-15 11:15:00 WARNING Memory usage high",
    "2024-01-15 12:00:00 INFO Backup started",
    "2024-01-15 12:05:00 INFO Backup completed"
]

# Extract level counts
levels = list(map(lambda l: l.split()[2], file_lines))
level_counts = Counter(levels)
print("Log level counts:")
for level, count in level_counts.items():
    print(f"  {level}: {count}")

# Extract errors
errors = list(filter(lambda l: "ERROR" in l, file_lines))
print(f"\nErrors ({len(errors)}):")
for e in errors:
    print(f"  {e}")

# Extract by time range (simulated)
lines_with_time = list(map(lambda l: {
    "line": l,
    "time": datetime.strptime(l.split()[1], "%H:%M:%S")
}, file_lines))

# Filter morning (before 11:00)
morning = list(filter(lambda x: x["time"].hour < 11, lines_with_time))
print(f"\nMorning entries: {len(morning)}")


# ============================================================
# Example 7: Lambda with REST API parameters
# ============================================================
print("\n=== Lambda with API Parameters ===")

# Query parameters
params = {
    "page": 1,
    "limit": 10,
    "sort": "name",
    "order": "asc",
    "filter": "active"
}

# Build query string using lambdas
def build_query(params):
    parts = list(map(lambda k: f"{k}={params[k]}", params.keys()))
    return "?" + "&".join(parts)

print(f"Query string: {build_query(params)}")

# Filter valid parameters
valid_keys = ["page", "limit", "sort", "order", "filter"]
filtered = dict(filter(lambda item: item[0] in valid_keys, params.items()))
print(f"Filtered params: {filtered}")


# ============================================================
# Example 8: Lambda with state machines
# ============================================================
print("\n=== Lambda with State Machine ===")

class StateMachine:
    def __init__(self, initial_state):
        self.state = initial_state
        self.transitions = {}
    
    def add_transition(self, from_state, to_state, condition):
        key = (from_state, to_state)
        self.transitions[key] = condition
    
    def can_transition(self, from_state, to_state):
        key = (from_state, to_state)
        return key in self.transitions
    
    def transition(self, to_state):
        key = (self.state, to_state)
        if key in self.transitions and self.transitions[key]():
            self.state = to_state
            return True
        return False
    
    def get_state(self):
        return self.state

# Create state machine
sm = StateMachine("idle")

# Add transitions with lambda conditions
sm.add_transition("idle", "loading", lambda: True)
sm.add_transition("loading", "ready", lambda: True)
sm.add_transition("ready", "processing", lambda: True)
sm.add_transition("processing", "done", lambda: True)
sm.add_transition("done", "idle", lambda: True)

# Test transitions
print(f"Initial state: {sm.get_state()}")
sm.transition("loading")
print(f"After transition to loading: {sm.get_state()}")
sm.transition("ready")
print(f"After transition to ready: {sm.get_state()}")


# ============================================================
# Example 9: Lambda with data transformation chains
# ============================================================
print("\n=== Lambda with Transformation Chains ===")

# Data transformation pipeline
class Pipeline:
    def __init__(self, data):
        self.data = data
    
    def filter(self, condition):
        self.data = list(filter(condition, self.data))
        return self
    
    def map(self, transform):
        self.data = list(map(transform, self.data))
        return self
    
    def sort(self, key_func):
        self.data = sorted(self.data, key=key_func)
        return self
    
    def result(self):
        return self.data

# Use pipeline
numbers = list(range(1, 21))

result = (
    Pipeline(numbers)
    .filter(lambda x: x % 2 == 0)  # Keep evens
    .map(lambda x: x ** 2)  # Square them
    .sort(reverse=True)  # Sort descending
    .result()
)

print(f"Pipeline result: {result}")


# ============================================================
# Example 10: Lambda with caching strategies
# ============================================================
print("\n=== Lambda with Caching ===")

class Cache:
    def __init__(self, ttl=60):
        self.cache = {}
        self.ttl = ttl
    
    def get(self, key):
        if key in self.cache:
            import time
            data, timestamp = self.cache[key]
            if time.time() - timestamp < self.ttl:
                return data
            del self.cache[key]
        return None
    
    def set(self, key, value):
        import time
        self.cache[key] = (value, time.time())
    
    def clear(self):
        self.cache.clear()

# Create cache
cache = Cache(ttl=3600)

# Cache with lambda
def get_user_cached(user_id):
    cached = cache.get(user_id)
    if cached:
        print(f"Cache hit for user {user_id}")
        return cached
    
    print(f"Cache miss for user {user_id}")
    # Simulated database lookup
    user = {"id": user_id, "name": f"User {user_id}"}
    cache.set(user_id, user)
    return user

# Test caching
get_user_cached(1)  # Miss
get_user_cached(1)  # Hit
get_user_cached(2)  # Miss
get_user_cached(2)  # Hit


# ============================================================
# Summary: Key Takeaways
# ============================================================
print("\n" + "=" * 50)
print("KEY TAKEAWAYS:")
print("=" * 50)
print("""
1. Lambdas are best for simple, throwaway functions

2. Use with: sorted(), map(), filter(), max(), min(), reduce()

3. Avoid for: complex logic, multiple statements, reusable code

4. Common patterns:
   - Key functions for sorting
   - Simple transformations
   - Filtering data
   - Callbacks and event handlers

5. Real-world uses:
   - Data processing pipelines
   - API response transformation
   - Validation rules
   - Caching
   - State machines
   - Query builders
""")
