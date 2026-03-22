# Example79.py
# Topic: Testing with Single Responsibility

# This file demonstrates how SRP makes testing easier.


# ============================================================
# Example 1: Easy Unit Testing
# ============================================================
print("=== Easy Unit Testing with SRP ===")

# Functions with single responsibility are easy to test
def validate_age(age):
    """Validate age is between 0 and 150."""
    return isinstance(age, int) and 0 <= age <= 150

def calculate_retirement_age(current_age):
    """Calculate years until retirement."""
    return 65 - current_age if current_age < 65 else 0

def format_retirement_message(years_left):
    """Format retirement message."""
    if years_left == 0:
        return "You're already retired!"
    return f"{years_left} years until retirement"

# Easy to test each function
print(f"Validate age 25: {validate_age(25)}")
print(f"Validate age -5: {validate_age(-5)}")
print(f"Years to retirement from 25: {calculate_retirement_age(25)}")
print(f"Years to retirement from 70: {calculate_retirement_age(70)}")
print(f"Message for 10 years: {format_retirement_message(10)}")


# ============================================================
# Example 2: Testable Business Logic
# ============================================================
print("\n=== Testable Business Logic ===")

# Pure functions are easy to test
def calculate_discount(amount, discount_percent):
    """Calculate discount amount."""
    return amount * (discount_percent / 100)

def apply_tax(amount, tax_percent):
    """Calculate tax amount."""
    return amount * (tax_percent / 100)

def calculate_total(subtotal, discount_percent=0, tax_percent=10):
    """Calculate final total."""
    discount = calculate_discount(subtotal, discount_percent)
    after_discount = subtotal - discount
    tax = apply_tax(after_discount, tax_percent)
    return after_discount + tax

# Easy to test each scenario
print(f"Total $100, 0% discount, 10% tax: ${calculate_total(100):.2f}")
print(f"Total $100, 10% discount, 10% tax: ${calculate_total(100, 10):.2f}")
print(f"Total $200, 20% discount, 8% tax: ${calculate_total(200, 20, 8):.2f}")


# ============================================================
# Example 3: Mocking Side Effects
# ============================================================
print("\n=== Mocking Side Effects ===")

# Separate pure logic from side effects
class Database:
    def __init__(self):
        self.data = {}
    
    def save(self, key, value):
        """Side effect: writes to database."""
        self.data[key] = value
        return True

def calculate_and_save(key, value, db):
    """Mixed function (harder to test)."""
    calculated = value * 2
    db.save(key, calculated)
    return calculated

# Better: separate concerns
def calculate(value):
    """Pure: only calculation."""
    return value * 2

def save_result(key, value, db):
    """Side effect: save to database."""
    db.save(key, value)

def process_and_save(key, value, db):
    """Orchestrates pure and impure."""
    result = calculate(value)
    save_result(key, result, db)
    return result

# Test with mock
class MockDB:
    def __init__(self):
        self.saved = {}
    def save(self, key, value):
        self.saved[key] = value

mock_db = MockDB()
result = process_and_save("test", 5, mock_db)
print(f"Result: {result}")
print(f"Mock DB saved: {mock_db.saved}")


# ============================================================
# Example 4: Composable Functions
# ============================================================
print("\n=== Composable Functions ===")

# Small functions can be composed
def add_tax(price, tax_rate=0.1):
    return price * (1 + tax_rate)

def add_shipping(price, base=5, free_threshold=100):
    return 0 if price >= free_threshold else base

def calculate_final(price, tax_rate=0.1, shipping_base=5):
    """Compose the functions."""
    with_tax = add_tax(price, tax_rate)
    return add_shipping(with_tax, shipping_base)

# Easy to test individual parts
print(f"Add tax $100: ${add_tax(100):.2f}")
print(f"Add shipping $50: ${add_shipping(50)}")
print(f"Add shipping $150: ${add_shipping(150)}")
print(f"Final $100: ${calculate_final(100):.2f}")
print(f"Final $150: ${calculate_final(150):.2f}")


# ============================================================
# Example 5: Property-Based Testing
# ============================================================
print("\n=== Property-Based Testing ===")

# Pure functions are great for property-based testing
def add(a, b):
    return a + b

def multiply(a, b):
    return a * b

def distributive_property(a, b, c):
    """Test: a * (b + c) == a*b + a*c"""
    left = multiply(a, add(b, c))
    right = add(multiply(a, b), multiply(a, c))
    return left == right

# Test with many values
test_cases = [(1, 2, 3), (5, 10, 15), (0, 1, 2), (100, 50, 25)]
for a, b, c in test_cases:
    result = distributive_property(a, b, c)
    print(f"a={a}, b={b}, c={c}: {result}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("TESTING WITH SRP")
print("=" * 50)
print("""
Benefits of SRP for testing:

1. Easy unit tests:
   - Each function has one responsibility
   - Test cases are simple and clear

2. Easy mocking:
   - Separate pure functions from side effects
   - Mock the side effects

3. Composition:
   - Small functions compose well
   - Test compositions

4. Property-based testing:
   - Pure functions can be tested with
     many random inputs
   - Verify mathematical properties
""")
