# Example23.py
# Topic: Fully Typed Function Examples

# This file demonstrates comprehensive type hints with complex types,
# combining all the concepts from previous examples into practical functions.


# Import necessary types for comprehensive type hints
from typing import Any, Optional, Callable


# --- Basic Typed Functions ---

# Simple add function with full type hints
def add(a: int, b: int) -> int:
    return a + b

result = add(5, 3)    # int — 8
print("5 + 3 = " + str(result))    # 5 + 3 = 8


# Division with Optional return (can return None for division by zero)
def divide(a: float, b: float) -> Optional[float]:
    if b == 0:
        return None
    return a / b

result1 = divide(10, 2)    # float — 5.0
print("10 / 2 = " + str(result1))    # 10 / 2 = 5.0

result2 = divide(10, 0)    # None — cannot divide by zero
print("10 / 0 = " + str(result2))    # 10 / 0 = None


# --- Collection Typed Functions ---

# Find maximum in a list of floats
def find_max(numbers: list[float]) -> Optional[float]:
    if not numbers:
        return None
    max_val = numbers[0]    # float — current maximum
    for num in numbers:
        if num > max_val:
            max_val = num
    return max_val

numbers = [1.5, 2.7, 0.3, 9.9, 5.5]    # list — float values
max_val = find_max(numbers)    # float — 9.9
print("Max: " + str(max_val))    # Max: 9.9


# Merge two dictionaries
def merge_dicts(dict1: dict[str, Any], dict2: dict[str, Any]) -> dict[str, Any]:
    result = dict1.copy()    # dict — start with copy of first dict
    for key, value in dict2.items():
        result[key] = value    # add keys from second dict
    return result

d1 = {"name": "Alice", "age": 25}    # dict — first dictionary
d2 = {"city": "NYC", "country": "USA"}    # dict — second dictionary
merged = merge_dicts(d1, d2)    # dict — combined
print("Merged: " + str(merged))    # Merged: {'name': 'Alice', 'age': 25, 'city': 'NYC', 'country': 'USA'}


# --- Optional Types ---

# Get user name by ID with default
def get_user(user_id: int, default_name: str = "Guest") -> str:
    users = {1: "Alice", 2: "Bob", 3: "Charlie"}    # dict — user database
    return users.get(user_id, default_name)    # str — user name or default

result1 = get_user(1)    # str — "Alice"
print("User 1: " + result1)    # User 1: Alice

result2 = get_user(999)    # str — "Guest" (default)
print("User 999: " + result2)    # User 999: Guest


# Parse integer with Optional return
def parse_int(value: str) -> Optional[int]:
    try:
        return int(value)    # int — parsed integer
    except ValueError:
        return None

result1 = parse_int("42")    # int — 42
print("Parsed '42': " + str(result1))    # Parsed '42': 42

result2 = parse_int("abc")    # None — invalid
print("Parsed 'abc': " + str(result2))    # Parsed 'abc': None


# --- Complex Typed Functions ---

# Filter list of dictionaries
def filter_items(
    items: list[dict[str, Any]],
    filter_key: str,
    filter_value: Any
) -> list[dict[str, Any]]:
    filtered = []    # list — items matching filter
    for item in items:
        if item.get(filter_key) == filter_value:
            filtered.append(item)
    return filtered

products = [
    {"name": "Laptop", "category": "electronics", "price": 1000},
    {"name": "Shirt", "category": "clothing", "price": 30},
    {"name": "Phone", "category": "electronics", "price": 800}
]

electronics = filter_items(products, "category", "electronics")    # list — electronics only
print("Electronics: " + str(electronics))    # Electronics: [{'name': 'Laptop', ...}, {'name': 'Phone', ...}]


# Apply function to each number in a list
def apply_to_each(
    func: Callable[[int], int],
    numbers: list[int]
) -> list[int]:
    result = []    # list — transformed numbers
    for n in numbers:
        result.append(func(n))
    return result

nums = [1, 2, 3, 4, 5]    # list — original numbers
doubled = apply_to_each(lambda x: x * 2, nums)    # list — [2, 4, 6, 8, 10]
print("Doubled: " + str(doubled))    # Doubled: [2, 4, 6, 8, 10]


# --- Practical Examples ---

# Create user with full type hints
def create_user(
    name: str,
    email: str,
    age: int,
    user_id: Optional[int] = None
) -> dict[str, Any]:
    user = {}    # dict — new user dictionary
    if user_id is None:
        user_id = hash(email) % 10000    # int — auto-generate ID
    user["id"] = user_id
    user["name"] = name
    user["email"] = email
    user["age"] = age
    return user

user = create_user("Alice", "alice@example.com", 25)    # dict — created user
print("Created user: " + str(user))    # Created user: {'id': ..., 'name': 'Alice', ...}


# Validate user data with list of errors
def validate_user(user: dict[str, Any]) -> list[str]:
    errors = []    # list — validation error messages
    
    if not user.get("name") or len(user.get("name", "")) < 2:
        errors.append("Name must be at least 2 characters")
    
    email = user.get("email", "")    # str — email to validate
    if "@" not in email:
        errors.append("Invalid email format")
    
    age = user.get("age", 0)    # int — age to validate
    if age < 0 or age > 150:
        errors.append("Age must be between 0 and 150")
    
    return errors

valid_user = {"name": "Alice", "email": "alice@example.com", "age": 25}    # dict — valid user
errors = validate_user(valid_user)    # list — empty (no errors)
print("Valid user errors: " + str(errors))    # Valid user errors: []

invalid_user = {"name": "A", "email": "invalid", "age": 200}    # dict — invalid user
errors2 = validate_user(invalid_user)    # list — multiple errors
print("Invalid user errors: " + str(errors2))    # Invalid user errors: ['Name must be at least 2 characters', ...]


# Process order with typed input and output
def process_order(
    order_items: list[dict[str, Any]],
    apply_discount: bool = False,
    discount_percent: float = 0.0
) -> dict[str, Any]:
    subtotal = 0.0    # float — sum of item prices
    for item in order_items:
        price = item.get("price", 0)    # float — item price
        quantity = item.get("quantity", 1)    # int — item quantity
        subtotal = subtotal + (price * quantity)
    
    discount_amount = 0.0    # float — discount to apply
    if apply_discount:
        discount_amount = subtotal * discount_percent
    
    total = subtotal - discount_amount    # float — final total
    
    return {
        "items": order_items,
        "subtotal": subtotal,
        "discount": discount_amount,
        "total": round(total, 2)
    }

order = [
    {"name": "Laptop", "price": 1000.0, "quantity": 1},
    {"name": "Mouse", "price": 25.0, "quantity": 2}
]

processed = process_order(order)    # dict — order without discount
print("Without discount: " + str(processed))    # Without discount: {'items': [...], 'subtotal': 1050.0, ...}

processed2 = process_order(order, True, 0.1)    # dict — order with 10% discount
print("With discount: " + str(processed2))    # With discount: {'items': [...], 'subtotal': 1050.0, 'discount': 105.0, 'total': 945.0}


# --- Union Type Examples ---

# Process value that can be int or float
def process_number(value: int | float) -> str:
    if isinstance(value, int):
        return "Integer: " + str(value * 2)
    return "Float: " + str(round(value * 2, 2))

result1 = process_number(10)    # str — "Integer: 20"
print(result1)    # Integer: 20

result2 = process_number(3.14)    # str — "Float: 6.28"
print(result2)    # Float: 6.28


# Handle data that might be string or None
def sanitize_input(value: str | None) -> str:
    if value is None:
        return ""
    return value.strip().lower()

result1 = sanitize_input("  HELLO  ")    # str — "hello"
print("Sanitized: '" + result1 + "'")    # Sanitized: 'hello'

result2 = sanitize_input(None)    # str — ""
print("Sanitized: '" + result2 + "'")    # Sanitized: ''
