# Example28.py
# Topic: Return Values - Optional Return Values

# This file demonstrates functions that return Optional values, where
# the return type can be either a specific type or None.


from typing import Optional


# Returns the first negative number or None if none exists
def find_first_negative(numbers: list[int]) -> Optional[int]:
    for num in numbers:
        if num < 0:
            return num
    return None

result = find_first_negative([1, 2, -3, 4, 5])
if result is not None:
    print("First negative: " + str(result))    # First negative: -3
else:
    print("No negative numbers")

result = find_first_negative([1, 2, 3, 4, 5])
print("Result: " + str(result))    # Result: None


# Returns the first element of a list or None if empty
def get_first(items: list[str]) -> Optional[str]:
    if items:
        return items[0]
    return None

print(get_first(["apple", "banana"]))    # apple
print(get_first([]))                     # None


# Looks up a user by ID and returns None if not found
def get_user(user_id: int) -> Optional[dict]:
    users = {
        1: {"name": "Alice", "email": "alice@example.com"},
        2: {"name": "Bob", "email": "bob@example.com"}
    }
    return users.get(user_id)

user = get_user(1)
if user:
    print("Found: " + user["name"])    # Found: Alice
else:
    print("User not found")

user = get_user(99)
print("User: " + str(user))    # User: None


# Returns the middle element of a list or None for empty/single element lists
def get_middle(items: list) -> Optional[object]:
    if len(items) == 0:
        return None
    if len(items) == 1:
        return items[0]
    mid = len(items) // 2
    if len(items) % 2 == 0:
        return [items[mid - 1], items[mid]]
    return items[mid]

print(get_middle([1, 2, 3, 4, 5]))      # 3
print(get_middle([1, 2, 3, 4]))         # [2, 3]
print(get_middle([1]))                  # 1
print(get_middle([]))                   # None


# Finds a substring and returns its index or None
def find_substring(text: str, substring: str) -> Optional[int]:
    if substring in text:
        return text.find(substring)
    return None

index = find_substring("Hello World", "World")
if index is not None:
    print("Found at index: " + str(index))    # Found at index: 6
else:
    print("Not found")

print("Result: " + str(find_substring("Hello", "xyz")))    # Result: None


# Returns the value associated with a key or None
def get_value(data: dict, key: str) -> Optional[str]:
    if key in data:
        return data[key]
    return None

info = {"name": "Alice", "age": "30"}
value = get_value(info, "name")
print("Value: " + str(value))    # Value: Alice

value = get_value(info, "email")
print("Value: " + str(value))    # Value: None


# Finds the first prime number in a list or returns None
def is_prime(n: int) -> bool:
    if n < 2:
        return False
    for i in range(2, int(n ** 0.5) + 1):
        if n % i == 0:
            return False
    return True

def find_first_prime(numbers: list[int]) -> Optional[int]:
    for num in numbers:
        if is_prime(num):
            return num
    return None

print("First prime in [4,6,8,9,10,11]: " + str(find_first_prime([4, 6, 8, 9, 10, 11])))    # 11
print("First prime in [4,6,8,9,10]: " + str(find_first_prime([4, 6, 8, 9, 10])))          # None


# Returns the maximum of a list or None if empty
def get_maximum(numbers: list[int]) -> Optional[int]:
    if not numbers:
        return None
    return max(numbers)

print("Max of [3,1,4,1,5]: " + str(get_maximum([3, 1, 4, 1, 5])))    # 5
print("Max of []: " + str(get_maximum([])))                           # None


# Finds a student by name or returns None
def find_student(students: list[dict], name: str) -> Optional[dict]:
    for student in students:
        if student.get("name") == name:
            return student
    return None

students = [
    {"name": "Alice", "grade": "A"},
    {"name": "Bob", "grade": "B"},
    {"name": "Charlie", "grade": "A"}
]

student = find_student(students, "Bob")
if student:
    print("Found: " + student["name"] + " - " + student["grade"])    # Found: Bob - B
else:
    print("Not found")

print("Result: " + str(find_student(students, "Diana")))    # Result: None


# Parses an integer from a string, returns None if invalid
def parse_int(value: str) -> Optional[int]:
    try:
        return int(value)
    except ValueError:
        return None

print("parse_int('42'): " + str(parse_int("42")))       # 42
print("parse_int('abc'): " + str(parse_int("abc")))     # None
print("parse_int('3.14'): " + str(parse_int("3.14")))   # None


# Returns the square root of a number or None for negative numbers
import math

def safe_sqrt(n: float) -> Optional[float]:
    if n < 0:
        return None
    return math.sqrt(n)

print("sqrt(16): " + str(safe_sqrt(16)))    # 4.0
print("sqrt(-4): " + str(safe_sqrt(-4)))    # None


# Real-life Example 1: Look up customer by email
def find_customer_by_email(customers: list[dict], email: str) -> Optional[dict]:
    for customer in customers:
        if customer.get("email", "").lower() == email.lower():
            return customer
    return None

customers = [
    {"id": 1, "name": "Alice Johnson", "email": "alice@example.com", "tier": "gold"},
    {"id": 2, "name": "Bob Smith", "email": "bob@example.com", "tier": "silver"},
    {"id": 3, "name": "Carol White", "email": "carol@example.com", "tier": "bronze"}
]

customer = find_customer_by_email(customers, "bob@example.com")
if customer:
    print(f"Found: {customer['name']} ({customer['tier']} tier)")
    # Found: Bob Smith (silver tier)
else:
    print("Customer not found")

result = find_customer_by_email(customers, "unknown@example.com")
print(f"Result: {result}")    # Result: None


# Real-life Example 2: Get discount percentage based on membership tier
def get_discount_by_tier(tier: str) -> Optional[float]:
    discounts = {
        "bronze": 0.05,
        "silver": 0.10,
        "gold": 0.15,
        "platinum": 0.20
    }
    return discounts.get(tier.lower())

discount = get_discount_by_tier("gold")
if discount is not None:
    print(f"Gold tier discount: {discount * 100}%")    # Gold tier discount: 15.0%
else:
    print("Gold tier discount: Not available")

discount = get_discount_by_tier("diamond")
print(f"Diamond tier discount: {discount}")         # Diamond tier discount: None


# Real-life Example 3: Parse phone number and return formatted version
def format_phone_number(phone: str) -> Optional[str]:
    digits = "".join(c for c in phone if c.isdigit())
    
    if len(digits) == 10:
        return f"({digits[:3]}) {digits[3:6]}-{digits[6:]}"
    elif len(digits) == 11 and digits[0] == "1":
        return f"+1 ({digits[1:4]}) {digits[4:7]}-{digits[7:]}"
    return None

formatted = format_phone_number("555-123-4567")
print(f"Formatted: {formatted}")         # Formatted: (555) 123-4567
formatted = format_phone_number("5551234567")
print(f"Formatted: {formatted}")         # Formatted: (555) 123-4567
formatted = format_phone_number("123-456")
print(f"Formatted: {formatted}")         # Formatted: None


# Real-life Example 4: Get shipping estimate based on zip code
def get_shipping_estimate(zip_code: str) -> Optional[tuple[int, float]]:
    zone_map = {
        "0": (1, 5.99), "1": (1, 5.99), "2": (2, 7.99),
        "3": (2, 7.99), "4": (3, 9.99), "5": (3, 9.99),
        "6": (4, 12.99), "7": (4, 12.99), "8": (5, 15.99),
        "9": (5, 15.99)
    }
    
    if not zip_code or len(zip_code) < 1:
        return None
    
    first_digit = zip_code[0]
    if first_digit in zone_map:
        return zone_map[first_digit]
    return None

estimate = get_shipping_estimate("90210")
if estimate:
    zone, cost = estimate
    print(f"Zone: {zone}, Shipping: ${cost}")
    # Zone: 5, Shipping: $15.99
else:
    print("Unable to estimate shipping")

estimate = get_shipping_estimate("")
print(f"Estimate: {estimate}")    # Estimate: None


# Real-life Example 5: Find expired products in inventory
def find_expired_products(products: list[dict], current_date: str) -> Optional[list[dict]]:
    if not products:
        return None
    
    expired = []
    for product in products:
        if product.get("expiry_date", "") < current_date:
            expired.append(product)
    
    return expired if expired else None

inventory = [
    {"name": "Milk", "expiry_date": "2024-01-01", "price": 3.99},
    {"name": "Bread", "expiry_date": "2024-12-31", "price": 2.99},
    {"name": "Cheese", "expiry_date": "2024-06-15", "price": 5.99}
]

expired = find_expired_products(inventory, "2024-03-01")
if expired:
    print("Expired products:")
    for p in expired:
        print(f"  - {p['name']} (expired {p['expiry_date']})")
    # Expired products:
    #   - Milk (expired 2024-01-01)
else:
    print("No expired products")

expired = find_expired_products([], "2024-03-01")
print(f"Result: {expired}")    # Result: None


# Real-life Example 6: Get exchange rate for currency conversion
def get_exchange_rate(from_currency: str, to_currency: str) -> Optional[float]:
    rates = {
        ("USD", "EUR"): 0.85,
        ("USD", "GBP"): 0.73,
        ("USD", "JPY"): 110.0,
        ("EUR", "USD"): 1.18,
        ("GBP", "USD"): 1.37,
        ("EUR", "GBP"): 0.86
    }
    
    return rates.get((from_currency.upper(), to_currency.upper()))

rate = get_exchange_rate("USD", "EUR")
if rate:
    print(f"1 USD = {rate} EUR")    # 1 USD = 0.85 EUR
else:
    print("Exchange rate not available")

converted = get_exchange_rate("JPY", "USD")
print(f"Result: {converted}")    # Result: None
