# Example48.py
# Topic: Lambda Functions - When NOT to Use and Best Practices

# This file demonstrates scenarios where lambdas should be avoided
# and best practices for using them correctly.


# ============================================================
# WRONG: Lambda with complex logic - Use regular function
# ============================================================
print("=== AVOID: Complex Logic in Lambda ===")

# WRONG - Too complex for lambda
# complex_calc = lambda x: [i for i in range(x) if i % 2 == 0 and (i % 3 == 0 or i % 5 == 0)]

# CORRECT - Use regular function
def complex_calculations(n):
    """Calculate numbers divisible by 2 and either 3 or 5."""
    return [i for i in range(n) if i % 2 == 0 and (i % 3 == 0 or i % 5 == 0)]

result = complex_calculations(30)
print(f"Complex calculation result: {result}")


# ============================================================
# WRONG: Lambda with multiple statements - Use regular function
# ============================================================
print("\n=== AVOID: Multiple Statements in Lambda ===")

# WRONG - Lambda can't have multiple statements
# abs_value = lambda x: if x > 0: return x else: return -x

# CORRECT - Use regular function
def abs_value(x):
    """Return absolute value."""
    if x > 0:
        return x
    return -x

print(f"abs_value(-15) = {abs_value(-15)}")


# ============================================================
# WRONG: Lambda for reusable functions - Use regular function
# ============================================================
print("\n=== AVOID: Lambda for Reusable Functions ===")

# WRONG - Can't easily call lambda again
# square = lambda x: x ** 2
# How to call it again? Need to store reference.

# CORRECT - Use regular function
def square(x):
    """Return square of x."""
    return x ** 2

print(f"square(5) = {square(5)}")
print(f"square(10) = {square(10)}")


# ============================================================
# WRONG: Lambda with side effects - Use regular function
# ============================================================
print("\n=== AVOID: Lambda with Side Effects ===")

# WRONG - Lambdas should be pure functions
# process_with_logging = lambda x: print(f"Processing {x}") or x * 2

# CORRECT - Use regular function
def process_with_logging(x):
    """Process value with logging."""
    print(f"Processing {x}")
    return x * 2

result = process_with_logging(5)


# ============================================================
# WRONG: Lambda for methods that need 'self' - Use regular function
# ============================================================
print("\n=== AVOID: Lambda for Instance Methods ===")

# WRONG - Can't access instance methods properly
class Counter:
    def __init__(self):
        self.count = 0
    
    # WRONG: Can't use lambda here
    # increment = lambda self: setattr(self, 'count', self.count + 1)

# CORRECT - Use regular method
class CounterCorrect:
    def __init__(self):
        self.count = 0
    
    def increment(self):
        """Increment the counter."""
        self.count += 1
        return self.count

counter = CounterCorrect()
print(f"Count after increment: {counter.increment()}")
print(f"Count after increment: {counter.increment()}")


# ============================================================
# BEST PRACTICE: Use lambda for simple key functions
# ============================================================
print("\n=== BEST PRACTICE: Simple Key Functions ===")

# GOOD - Lambda perfect for this
names = ["Alice", "Bob", "Charlie", "Diana"]
sorted_by_length = sorted(names, key=lambda x: len(x))
print(f"Sorted by length: {sorted_by_length}")

# GOOD - Lambda perfect for sorting
data = [{"id": 3, "name": "Charlie"}, {"id": 1, "name": "Alice"}, {"id": 2, "name": "Bob"}]
sorted_by_id = sorted(data, key=lambda x: x["id"])
print(f"Sorted by id: {[d['id'] for d in sorted_by_id]}")


# ============================================================
# BEST PRACTICE: Use lambda with built-in functions
# ============================================================
print("\n=== BEST PRACTICE: With Built-in Functions ===")

# GOOD - Lambda with sorted()
prices = [100, 50, 200, 75]
sorted_prices = sorted(prices)
print(f"Sorted: {sorted_prices}")

# GOOD - Lambda with map()
doubled = list(map(lambda x: x * 2, [1, 2, 3, 4, 5]))
print(f"Doubled: {doubled}")

# GOOD - Lambda with filter()
evens = list(filter(lambda x: x % 2 == 0, range(10)))
print(f"Evens: {evens}")


# ============================================================
# BEST PRACTICE: Use lambda for one-liner transformations
# ============================================================
print("\n=== BEST PRACTICE: One-liner Transformations ===")

# GOOD - Simple transformation
words = ["hello", "world", "python"]
uppercased = list(map(lambda x: x.upper(), words))
print(f"Uppercased: {uppercased}")

# GOOD - Simple formatting
numbers = [1, 2, 3]
formatted = list(map(lambda x: f"Item {x}", numbers))
print(f"Formatted: {formatted}")


# Real-life Example 1: When NOT to use lambda - Complex validation
print("\n=== Real-life: When NOT to Use Lambda ===")

# WRONG - Too complex
# def validate_user_data(data):
#     errors = []
#     if not data.get('email') or '@' not in data['email']:
#         errors.append('Invalid email')
#     if not data.get('password') or len(data['password']) < 8:
#         errors.append('Password too short')
#     if not data.get('age') or data['age'] < 18:
#         errors.append('Must be 18 or older')
#     return errors

# CORRECT - Regular function for complex validation
def validate_user(user_data):
    """Validate user registration data."""
    errors = []
    
    email = user_data.get("email", "")
    if not email or "@" not in email or "." not in email.split("@")[1]:
        errors.append("Invalid email format")
    
    password = user_data.get("password", "")
    if len(password) < 8:
        errors.append("Password must be at least 8 characters")
    if not any(c.isupper() for c in password):
        errors.append("Password must contain uppercase letter")
    if not any(c.isdigit() for c in password):
        errors.append("Password must contain a number")
    
    age = user_data.get("age", 0)
    if age < 18:
        errors.append("Must be 18 or older")
    
    return errors

user_data = {"email": "test", "password": "short", "age": 15}
errors = validate_user(user_data)
print(f"Validation errors: {errors}")


# Real-life Example 2: When to use lambda - Simple sorting
print("\n=== Real-life: When to Use Lambda ===")

# GOOD - Simple sort key
products = [
    {"name": "Laptop", "price": 999, "rating": 4.5},
    {"name": "Mouse", "price": 29, "rating": 4.2},
    {"name": "Keyboard", "price": 79, "rating": 4.7}
]

# Sort by price (low to high)
by_price = sorted(products, key=lambda p: p["price"])
print("Sorted by price:", [p["name"] for p in by_price])

# Sort by rating (high to low)
by_rating = sorted(products, key=lambda p: p["rating"], reverse=True)
print("Sorted by rating:", [p["name"] for p in by_rating])


# Real-life Example 3: Lambda in event handlers (simple case)
print("\n=== Real-life: Lambda in Simple Event Handler ===")

class Button:
    def __init__(self, label):
        self.label = label
        self.handlers = []
    
    def on_click(self, handler):
        """Register a click handler."""
        self.handlers.append(handler)
    
    def click(self):
        """Simulate button click."""
        for handler in self.handlers:
            handler(self.label)

# Using lambda for simple handler
button = Button("Submit")
button.on_click(lambda label: print(f"Button '{label}' was clicked!"))
button.click()


# Real-life Example 4: Complex event handler - Use regular function
print("\n=== Real-life: Regular Function for Complex Handler ===")

class Form:
    def __init__(self):
        self.fields = {}
        self.errors = []
    
    def add_field(self, name, value):
        self.fields[name] = value
    
    def validate(self):
        """Complex validation - use regular function."""
        self.errors = []
        
        # Validate email
        email = self.fields.get("email", "")
        if not email or "@" not in email:
            self.errors.append("Invalid email")
        
        # Validate password match
        password = self.fields.get("password", "")
        confirm = self.fields.get("confirm_password", "")
        if password != confirm:
            self.errors.append("Passwords do not match")
        
        return len(self.errors) == 0

form = Form()
form.add_field("email", "invalid")
form.add_field("password", "pass123")
form.add_field("confirm_password", "pass124")

if not form.validate():
    print(f"Validation errors: {form.errors}")


# Real-life Example 5: Lambda for simple callbacks
print("\n=== Real-life: Lambda for Simple Callbacks ===")

def process_items(items, callback):
    """Process items with a callback function."""
    results = []
    for item in items:
        result = callback(item)
        results.append(result)
    return results

# GOOD - Simple transformation callback
numbers = [1, 2, 3, 4, 5]
squared = process_items(numbers, lambda x: x ** 2)
print(f"Squared: {squared}")

# GOOD - Simple filter callback
filtered = process_items(numbers, lambda x: x > 2)
print(f"Filtered (>2): {filtered}")


# Real-life Example 6: Complex callback - Use regular function
print("\n=== Real-life: Regular Function for Complex Callback ===")

class DataProcessor:
    def __init__(self):
        self.processors = []
    
    def add_processor(self, processor):
        """Add a complex processor - use regular function."""
        self.processors.append(processor)
    
    def process(self, data):
        results = []
        for proc in self.processors:
            result = proc(data)
            if result:  # Only add successful results
                results.append(result)
        return results

def complex_processor(data):
    """Complex processing logic."""
    processed = data.copy()
    if "value" in processed:
        processed["value"] = processed["value"] * 2
        processed["processed"] = True
    return processed

processor = DataProcessor()
processor.add_processor(complex_processor)

test_data = {"value": 10, "name": "test"}
results = processor.process(test_data)
print(f"Processed results: {results}")


# ============================================================
# SUMMARY: When to use Lambda vs Regular Function
# ============================================================
print("\n=== SUMMARY ===")
print("""
USE LAMBDA FOR:
  - Simple key functions (sorted, max, min)
  - Simple transformations (map)
  - Simple filters (filter)
  - One-liner callbacks
  - When you need a throwaway function

USE REGULAR FUNCTION FOR:
  - Complex logic
  - Multiple statements
  - Reusable functions
  - Functions with side effects
  - Functions that need 'self'
  - Complex validation
  - Documentation purposes
""")
