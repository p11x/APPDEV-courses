# Example83.py
# Topic: Real-World Examples with Pure Functions

# This file provides practical real-world examples of pure functions.


# ============================================================
# Example 1: Business Logic
# ============================================================
print("=== Real-world: Business Logic ===")

# Pure functions for pricing
def calculate_subtotal(items):
    """Pure: Calculate subtotal."""
    return sum(item['price'] * item['qty'] for item in items)

def apply_discount(amount, discount_percent):
    """Pure: Apply discount."""
    return amount * (1 - discount_percent / 100)

def calculate_tax(amount, tax_rate):
    """Pure: Calculate tax."""
    return amount * tax_rate

def calculate_total(items, discount_percent=0, tax_rate=0.1):
    """Pure: Calculate final total."""
    subtotal = calculate_subtotal(items)
    after_discount = apply_discount(subtotal, discount_percent)
    tax = calculate_tax(after_discount, tax_rate)
    return after_discount + tax

# Test
cart = [
    {'name': 'Widget', 'price': 10, 'qty': 2},
    {'name': 'Gadget', 'price': 25, 'qty': 1},
]
print(f"Total: ${calculate_total(cart, 10, 0.08):.2f}")


# ============================================================
# Example 2: Data Transformation
# ============================================================
print("\n=== Real-world: Data Transformation ===")

# Pure transformations
def normalize_name(name):
    """Pure: Normalize name."""
    return name.strip().title()

def normalize_email(email):
    """Pure: Normalize email."""
    return email.strip().lower()

def validate_email(email):
    """Pure: Validate email."""
    return '@' in email and '.' in email.split('@')[-1]

def transform_user(raw_data):
    """Pure: Transform user data."""
    return {
        'name': normalize_name(raw_data.get('name', '')),
        'email': normalize_email(raw_data.get('email', '')),
        'age': raw_data.get('age', 0)
    }

# Test
raw = {'name': '  JOHN DOE  ', 'EMAIL': 'JOHN@EXAMPLE.COM ', 'age': 30}
transformed = transform_user(raw)
print(f"Transformed: {transformed}")
print(f"Original unchanged: {raw}")


# ============================================================
# Example 3: Validation
# ============================================================
print("\n=== Real-world: Validation ===")

# Pure validators
def is_valid_email(email):
    return '@' in email and len(email) > 5

def is_valid_password(password):
    return len(password) >= 8

def is_valid_username(username):
    return len(username) >= 3 and username.isalnum()

def validate_registration(name, email, password):
    """Pure: Validate all fields."""
    errors = []
    if not is_valid_username(name):
        errors.append("Invalid username")
    if not is_valid_email(email):
        errors.append("Invalid email")
    if not is_valid_password(password):
        errors.append("Invalid password")
    return errors

# Test
print(f"Valid: {validate_registration('john', 'john@example.com', 'password123')}")
print(f"Invalid: {validate_registration('ab', 'bad', 'short')}")


# ============================================================
# Example 4: Calculations
# ============================================================
print("\n=== Real-world: Calculations ===")

# Pure calculation functions
def calculate_circle_area(radius):
    """Pure: Calculate circle area."""
    return 3.14159 * radius ** 2

def calculate_circle_circumference(radius):
    """Pure: Calculate circumference."""
    return 2 * 3.14159 * radius

def calculate_distance(x1, y1, x2, y2):
    """Pure: Calculate distance between points."""
    return ((x2 - x1) ** 2 + (y2 - y1) ** 2) ** 0.5

def calculate_bmi(weight_kg, height_m):
    """Pure: Calculate BMI."""
    return weight_kg / (height_m ** 2)

# Test
print(f"Area (r=5): {calculate_circle_area(5):.2f}")
print(f"Distance (0,0 to 3,4): {calculate_distance(0, 0, 3, 4)}")
print(f"BMI (70kg, 1.75m): {calculate_bmi(70, 1.75):.2f}")


# ============================================================
# Example 5: String Processing
# ============================================================
print("\n=== Real-world: String Processing ===")

# Pure string functions
def count_words(text):
    """Pure: Count words."""
    return len(text.split())

def count_chars(text):
    """Pure: Count characters."""
    return len(text)

def to_title_case(text):
    """Pure: Convert to title case."""
    return text.title()

def truncate(text, max_length):
    """Pure: Truncate text."""
    if len(text) <= max_length:
        return text
    return text[:max_length - 3] + "..."

def analyze_text(text):
    """Pure: Analyze text."""
    return {
        'word_count': count_words(text),
        'char_count': count_chars(text),
        'title_case': to_title_case(text),
        'truncated': truncate(text, 20)
    }

# Test
sample = "This is a sample text for analysis"
result = analyze_text(sample)
print(f"Analysis: {result}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("REAL-WORLD PURE FUNCTIONS")
print("=" * 50)
print("""
Use pure functions for:
- Business logic (pricing, discounts)
- Data transformation
- Validation
- Calculations
- String processing

Benefits in real-world:
- Easy to test
- Easy to reason about
- Easy to cache
- Parallel execution
""")
