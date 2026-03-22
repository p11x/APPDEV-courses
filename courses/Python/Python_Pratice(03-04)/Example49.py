# Example49.py
# Topic: Lambda Functions - Comprehensive Real-World Examples

# This file provides comprehensive real-world examples demonstrating
# practical uses of lambda functions in various scenarios.


# ============================================================
# Example 1: Lambda with SQL-like operations
# ============================================================
print("=== Real-world: Database Query Simulation ===")

class QueryBuilder:
    """Simulates a database query builder using lambdas."""
    
    def __init__(self, data):
        self.data = data
    
    def select(self, fields):
        """Select specific fields."""
        return list(map(lambda row: {k: row[k] for k in fields if k in row}, self.data))
    
    def where(self, condition):
        """Filter by condition."""
        return list(filter(condition, self.data))
    
    def order_by(self, key_func):
        """Order by key function."""
        return sorted(self.data, key=key_func)
    
    def limit(self, n):
        """Limit results."""
        return self.data[:n]

# Sample data
users = [
    {"id": 1, "name": "Alice", "age": 30, "department": "Engineering"},
    {"id": 2, "name": "Bob", "age": 25, "department": "Marketing"},
    {"id": 3, "name": "Charlie", "age": 35, "department": "Engineering"},
    {"id": 4, "name": "Diana", "age": 28, "department": "Sales"},
    {"id": 5, "name": "Eve", "age": 32, "department": "Marketing"}
]

query = QueryBuilder(users)

# Select name and age
result = query.select(["name", "age"])
print("Select name, age:")
for r in result:
    print(f"  {r}")

# Where age > 28
result = query.where(lambda u: u["age"] > 28)
print(f"\nAge > 28: {[u['name'] for u in result]}")

# Order by age
result = query.order_by(lambda u: u["age"])
print(f"\nOrder by age: {[(u['name'], u['age']) for u in result]}")


# ============================================================
# Example 2: Lambda with API response transformation
# ============================================================
print("\n=== Real-world: API Response Transformation ===")

# Raw API response
api_response = {
    "data": [
        {"user_id": 1, "full_name": "Alice Johnson", "reg_date": "2023-01-15", "is_active": 1},
        {"user_id": 2, "full_name": "Bob Smith", "reg_date": "2023-02-20", "is_active": 0},
        {"user_id": 3, "full_name": "Charlie Brown", "reg_date": "2023-03-10", "is_active": 1}
    ],
    "status": "success"
}

# Transform using lambdas
users = api_response["data"]

# Transform field names
transformed = list(map(
    lambda u: {
        "id": u["user_id"],
        "name": u["full_name"],
        "registered": u["reg_date"],
        "active": bool(u["is_active"])
    },
    users
))

print("Transformed users:")
for u in transformed:
    print(f"  {u}")

# Filter active users
active = list(filter(lambda u: u["active"], transformed))
print(f"\nActive users: {[u['name'] for u in active]}")


# ============================================================
# Example 3: Lambda with financial calculations
# ============================================================
print("\n=== Real-world: Financial Calculations ===")

# Investment portfolio
portfolio = [
    {"symbol": "AAPL", "shares": 100, "price": 175.50},
    {"symbol": "GOOGL", "shares": 50, "price": 142.30},
    {"symbol": "MSFT", "shares": 75, "price": 378.90},
    {"symbol": "AMZN", "shares": 25, "price": 178.25}
]

# Calculate total value
total_value = sum(map(lambda s: s["shares"] * s["price"], portfolio))
print(f"Total portfolio value: ${total_value:,.2f}")

# Find best performer (highest price)
best = max(portfolio, key=lambda s: s["price"])
print(f"Highest priced: {best['symbol']} at ${best['price']}")

# Calculate daily change (simulated)
daily_change = list(map(
    lambda s: {
        "symbol": s["symbol"],
        "value": s["shares"] * s["price"],
        "change_pct": (s["price"] * 0.02) - (s["price"] * 0.01)  # Simulated
    },
    portfolio
))

print("\nDaily changes:")
for stock in daily_change:
    print(f"  {stock['symbol']}: ${stock['value']:,.2f} ({stock['change_pct']:.2f}%)")


# ============================================================
# Example 4: Lambda with text processing
# ============================================================
print("\n=== Real-world: Text Processing ===")

# Log entries
logs = [
    "2024-01-15 ERROR: Connection timeout",
    "2024-01-15 INFO: User logged in",
    "2024-01-15 WARNING: Memory usage high",
    "2024-01-15 ERROR: Database connection failed",
    "2024-01-15 INFO: Email sent successfully",
    "2024-01-15 ERROR: Payment processing failed"
]

# Extract error logs
errors = list(filter(lambda l: "ERROR" in l, logs))
print(f"Error logs ({len(errors)}):")
for e in errors:
    print(f"  {e}")

# Extract by level
def extract_level(logs, level):
    return list(filter(lambda l: level in l, logs))

errors = extract_level(logs, "ERROR")
warnings = extract_level(logs, "WARNING")
infos = extract_level(logs, "INFO")

print(f"\nBy level: {len(errors)} errors, {len(warnings)} warnings, {len(infos)} info")


# ============================================================
# Example 5: Lambda with data validation pipeline
# ============================================================
print("\n=== Real-world: Validation Pipeline ===")

class Validator:
    """Data validator using lambda functions."""
    
    def __init__(self):
        self.rules = []
    
    def add_rule(self, name, validator_func, error_message):
        """Add validation rule."""
        self.rules.append({
            "name": name,
            "validator": validator_func,
            "error": error_message
        })
    
    def validate(self, data):
        """Run all validation rules."""
        errors = []
        for rule in self.rules():
            if not rule["validator"](data):
                errors.append(rule["error"])
        return errors

# Define validation rules
validator = Validator()

# Lambda for email validation
validator.add_rule(
    "email",
    lambda d: "@" in d.get("email", "") and "." in d.get("email", ""),
    "Invalid email format"
)

# Lambda for password
validator.add_rule(
    "password",
    lambda d: len(d.get("password", "")) >= 8,
    "Password must be at least 8 characters"
)

# Lambda for age
validator.add_rule(
    "age",
    lambda d: d.get("age", 0) >= 18,
    "Must be at least 18 years old"
)

# Test validation
test_data = {"email": "test@example", "password": "short", "age": 15}
errors = validator.validate(test_data)
print(f"Validation errors: {errors}")


# ============================================================
# Example 6: Lambda with sorting algorithms
# ============================================================
print("\n=== Real-world: Custom Sorting ===")

# Student records
students = [
    {"name": "Alice", "math": 85, "science": 92, "english": 78},
    {"name": "Bob", "math": 90, "science": 88, "english": 95},
    {"name": "Charlie", "math": 78, "science": 85, "english": 88},
    {"name": "Diana", "math": 92, "science": 95, "english": 90}
]

# Sort by total score
by_total = sorted(students, key=lambda s: s["math"] + s["science"] + s["english"], reverse=True)
print("By total score:")
for s in by_total:
    total = s["math"] + s["science"] + s["english"]
    print(f"  {s['name']}: {total}")

# Sort by math grade
by_math = sorted(students, key=lambda s: s["math"], reverse=True)
print("\nBy math grade:")
for s in by_math:
    print(f"  {s['name']}: {s['math']}")

# Sort by best subject
def best_subject(s):
    return max(s["math"], s["science"], s["english"])

by_best = sorted(students, key=best_subject, reverse=True)
print("\nBy best subject:")
for s in by_best:
    best = best_subject(s)
    print(f"  {s['name']}: {best}")


# ============================================================
# Example 7: Lambda with caching/memoization
# ============================================================
print("\n=== Real-world: Function Caching ===")

def memoize(func):
    """Memoization decorator using lambda."""
    cache = {}
    
    def wrapper(*args):
        key = str(args)
        if key not in cache:
            cache[key] = func(*args)
        return cache[key]
    
    wrapper.cache = cache
    wrapper.clear = lambda: cache.clear()
    return wrapper

@memoize
def fibonacci(n):
    """Calculate fibonacci number."""
    if n <= 1:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)

print("Fibonacci calculations:")
for i in range(10):
    print(f"  fib({i}) = {fibonacci(i)}")

print(f"Cache size: {len(fibonacci.cache)}")
fibonacci.clear()
print(f"After clear: {len(fibonacci.cache)}")


# ============================================================
# Example 8: Lambda with notification system
# ============================================================
print("\n=== Real-world: Notification System ===")

class NotificationSystem:
    """Notification system using lambda filters."""
    
    def __init__(self):
        self.handlers = []
    
    def subscribe(self, filter_func, handler):
        """Subscribe to notifications matching filter."""
        self.handlers.append({"filter": filter_func, "handler": handler})
    
    def notify(self, notification):
        """Send notification to matching subscribers."""
        for h in self.handlers:
            if h["filter"](notification):
                h["handler"](notification)

# Create notification system
notifications = NotificationSystem()

# Subscribe with lambda filters
notifications.subscribe(
    lambda n: n["type"] == "alert",
    lambda n: print(f"ALERT: {n['message']}")
)

notifications.subscribe(
    lambda n: n["priority"] == "high",
    lambda n: print(f"HIGH PRIORITY: {n['message']}")
)

notifications.subscribe(
    lambda n: n.get("from") == "system",
    lambda n: print(f"SYSTEM: {n['message']}")
)

# Send notifications
test_notifications = [
    {"type": "alert", "priority": "normal", "message": "Security breach!", "from": "security"},
    {"type": "info", "priority": "high", "message": "Backup completed", "from": "system"},
    {"type": "alert", "priority": "high", "message": "Server down!", "from": "monitoring"},
    {"type": "info", "priority": "normal", "message": "New user registered", "from": "auth"}
]

print("Processing notifications:")
for notif in test_notifications:
    notifications.notify(notif)


# ============================================================
# Example 9: Lambda with data aggregation
# ============================================================
print("\n=== Real-world: Data Aggregation ===")

from functools import reduce

# Sales data
sales = [
    {"date": "2024-01", "product": "Widget", "quantity": 100, "price": 10},
    {"date": "2024-01", "product": "Gadget", "quantity": 50, "price": 25},
    {"date": "2024-02", "product": "Widget", "quantity": 150, "price": 10},
    {"date": "2024-02", "product": "Gadget", "quantity": 75, "price": 25},
    {"date": "2024-03", "product": "Widget", "quantity": 200, "price": 10},
    {"date": "2024-03", "product": "Gadget", "quantity": 100, "price": 25}
]

# Total sales
total_sales = reduce(
    lambda acc, s: acc + s["quantity"] * s["price"],
    sales,
    0
)
print(f"Total sales: ${total_sales:,}")

# Sales by product
def group_by(data, key_func):
    return reduce(
        lambda acc, item: {**acc, item[key_func]: acc.get(item[key_func], []) + [item]},
        data,
        {}
    )

by_product = group_by(sales, "product")
print("\nSales by product:")
for product, items in by_product.items():
    total = reduce(lambda acc, i: acc + i["quantity"] * i["price"], items, 0)
    print(f"  {product}: ${total:,}")

# Sales by month
by_month = group_by(sales, "date")
print("\nSales by month:")
for month, items in by_month.items():
    total = reduce(lambda acc, i: acc + i["quantity"] * i["price"], items, 0)
    print(f"  {month}: ${total:,}")
