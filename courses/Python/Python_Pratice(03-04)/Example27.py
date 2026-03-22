# Example27.py
# Topic: Return Values - Early Returns and Guard Clauses

# This file demonstrates the use of early returns and guard clauses
# to handle special cases and validate inputs before processing.


# Uses early return to check for empty list
def get_first_item(items: list) -> str:
    if not items:
        return "No items"
    return items[0]

print(get_first_item(["apple", "banana"]))    # apple
print(get_first_item([]))                     # No items


# Uses guard clause to validate age
def validate_age(age: int) -> str:
    if age < 0:
        return "Invalid age"
    if age < 18:
        return "Minor"
    return "Adult"

print(validate_age(25))    # Adult
print(validate_age(15))    # Minor
print(validate_age(-5))    # Invalid age


# Uses early return for division by zero
def safe_divide(a: float, b: float) -> float:
    if b == 0:
        return 0.0
    return a / b

print("10 / 2 = " + str(safe_divide(10, 2)))    # 10 / 2 = 5.0
print("10 / 0 = " + str(safe_divide(10, 0)))    # 10 / 0 = 0.0


# Uses guard clause to validate password
def check_password(password: str) -> str:
    if not password:
        return "Password is required"
    if len(password) < 8:
        return "Password too short"
    if " " in password:
        return "Password cannot contain spaces"
    return "Password valid"

print(check_password("secret123"))              # Password valid
print(check_password("abc"))                     # Password too short
print(check_password(""))                        # Password is required
print(check_password("no spaces allowed"))      # Password cannot contain spaces


# Uses early return to find first even number
def find_first_even(numbers: list[int]) -> int:
    for num in numbers:
        if num % 2 == 0:
            return num
    return -1

print("First even in [1,3,5,6,7]: " + str(find_first_even([1, 3, 5, 6, 7])))    # 6
print("First even in [1,3,5,7]: " + str(find_first_even([1, 3, 5, 7])))        # -1


# Uses guard clause to check for valid index
def get_element(items: list, index: int) -> str:
    if index < 0 or index >= len(items):
        return "Index out of bounds"
    return items[index]

fruits = ["apple", "banana", "cherry"]
print(get_element(fruits, 1))     # banana
print(get_element(fruits, 10))    # Index out of bounds
print(get_element(fruits, -1))    # Index out of bounds


from typing import Optional

# Uses early return to handle None input
def process_text(text: Optional[str]) -> str:
    if text is None:
        return ""
    return text.strip().upper()

print(process_text("  hello  "))    # HELLO
print(process_text(None))           # (empty string)


# Uses guard clauses to validate user input
def create_user(username: str, email: str) -> tuple[bool, str]:
    if not username:
        return False, "Username required"
    if len(username) < 3:
        return False, "Username too short"
    if not email or "@" not in email:
        return False, "Invalid email"
    return True, "User created"

success, message = create_user("alice", "alice@example.com")
print(str(success) + ": " + message)    # True: User created
success, message = create_user("ab", "alice@example.com")
print(str(success) + ": " + message)    # False: Username too short


# Uses early return to skip processing
def apply_discount(price: float, discount: float) -> float:
    if discount <= 0:
        return price
    if discount >= 1:
        return 0.0
    return price * (1 - discount)

print("$100 with 10% off: $" + str(apply_discount(100, 0.10)))    # $90.0
print("$100 with 0% off: $" + str(apply_discount(100, 0.0)))      # $100.0
print("$100 with 100% off: $" + str(apply_discount(100, 1.0)))     # $0.0


# Uses guard clauses for file path validation
def validate_path(path: str) -> str:
    if not path:
        return "Path cannot be empty"
    if path.startswith("/"):
        return "Absolute path"
    if path.startswith("./"):
        return "Relative path"
    return "Simple filename"

print(validate_path("/home/user/file.txt"))    # Absolute path
print(validate_path("./relative/path"))        # Relative path
print(validate_path("filename.txt"))            # Simple filename


# Uses early return to find negative number
def find_negative(numbers: list[int]) -> Optional[int]:
    for num in numbers:
        if num < 0:
            return num
    return None

print("First negative in [1,2,-3,4]: " + str(find_negative([1, 2, -3, 4])))    # -3
print("First negative in [1,2,3]: " + str(find_negative([1, 2, 3])))          # None


# Real-life Example 1: Process credit card payment with validation
def process_payment(card_number: str, amount: float, cvv: str) -> tuple[bool, str]:
    if not card_number:
        return False, "Card number is required"
    if len(card_number) < 13 or len(card_number) > 19:
        return False, "Invalid card number length"
    if not card_number.isdigit():
        return False, "Card number must contain only digits"
    if not cvv or len(cvv) not in [3, 4]:
        return False, "Invalid CVV"
    if amount <= 0:
        return False, "Amount must be positive"
    if amount > 10000:
        return False, "Amount exceeds maximum limit"
    return True, "Payment processed successfully"

success, msg = process_payment("4111111111111111", 99.99, "123")
print(f"Payment: {success}, {msg}")    # Payment: True, Payment processed successfully
success, msg = process_payment("4111111111111111", 15000, "123")
print(f"Payment: {success}, {msg}")    # Payment: False, Amount exceeds maximum limit


# Real-life Example 2: Validate email registration form
def validate_registration(username: str, email: str, password: str, confirm: str) -> tuple[bool, str]:
    if not username or len(username) < 3:
        return False, "Username must be at least 3 characters"
    if len(username) > 20:
        return False, "Username too long (max 20 characters)"
    if not email or "@" not in email or "." not in email.split("@")[1]:
        return False, "Invalid email format"
    if not password or len(password) < 8:
        return False, "Password must be at least 8 characters"
    if not any(c.isupper() for c in password):
        return False, "Password must contain at least one uppercase letter"
    if not any(c.isdigit() for c in password):
        return False, "Password must contain at least one number"
    if password != confirm:
        return False, "Passwords do not match"
    return True, "Registration successful"

success, msg = validate_registration("john_doe", "john@example.com", "SecurePass123", "SecurePass123")
print(f"Registration: {success}, {msg}")    # Registration: True, Registration successful
success, msg = validate_registration("john_doe", "john@example.com", "weak", "weak")
print(f"Registration: {success}, {msg}")    # Registration: False, Password must be at least 8 characters


# Real-life Example 3: Check user permissions for resource access
def check_resource_access(user_role: str, resource: str, action: str) -> tuple[bool, str]:
    if not user_role:
        return False, "User not authenticated"
    
    permissions = {
        "admin": {"documents": ["read", "write", "delete"], "users": ["read", "write", "delete"]},
        "editor": {"documents": ["read", "write"], "users": ["read"]},
        "viewer": {"documents": ["read"], "users": []}
    }
    
    if user_role not in permissions:
        return False, "Invalid user role"
    if resource not in permissions[user_role]:
        return False, f"Resource '{resource}' not accessible"
    if action not in permissions[user_role][resource]:
        return False, f"Action '{action}' not permitted for {user_role}"
    
    return True, f"{action.capitalize()} access granted to {resource}"

success, msg = check_resource_access("editor", "documents", "write")
print(f"Access: {success}, {msg}")    # Access: True, Write access granted to documents
success, msg = check_resource_access("viewer", "users", "delete")
print(f"Access: {success}, {msg}")    # Access: False, Action 'delete' not permitted for viewer


# Real-life Example 4: Calculate shipping cost with early returns
def calculate_shipping_cost(weight: float, distance: float, expedited: bool) -> tuple[bool, float, str]:
    if weight <= 0:
        return False, 0.0, "Invalid weight"
    if weight > 50:
        return False, 0.0, "Weight exceeds maximum (50 kg)"
    if distance <= 0:
        return False, 0.0, "Invalid distance"
    if distance > 5000:
        return False, 0.0, "Distance exceeds maximum (5000 km)"
    
    base_rate = 5.0 if weight <= 10 else 10.0
    cost = base_rate + (distance * 0.01) + (weight * 0.5)
    
    if expedited:
        cost *= 1.5
        return True, cost, "Expedited shipping"
    
    return True, cost, "Standard shipping"

success, cost, service = calculate_shipping_cost(5, 500, False)
print(f"Shipping: {service}, Cost: ${cost:.2f}")    # Shipping: Standard shipping, Cost: $15.00
success, cost, service = calculate_shipping_cost(60, 500, False)
print(f"Shipping: {success}, {cost}, {service}")   # Shipping: False, 0.0, Weight exceeds maximum (50 kg)


# Real-life Example 5: Search product inventory with early return
def search_inventory(products: list[dict], search_term: str, category: Optional[str]) -> tuple[bool, list[dict], str]:
    if not search_term and not category:
        return False, [], "Please provide search term or category"
    
    results = []
    for product in products:
        if category and product.get("category") != category:
            continue
        if search_term:
            if search_term.lower() in product.get("name", "").lower():
                results.append(product)
        else:
            results.append(product)
    
    if not results:
        return False, [], "No products found"
    
    return True, results, f"Found {len(results)} product(s)"

inventory = [
    {"name": "iPhone 15", "category": "electronics", "price": 999},
    {"name": "MacBook Pro", "category": "electronics", "price": 1999},
    {"name": "Nike Air Max", "category": "shoes", "price": 150},
    {"name": "Adidas Sneakers", "category": "shoes", "price": 120}
]

success, items, msg = search_inventory(inventory, "phone", None)
print(f"{msg}: {len(items)} items")    # Found 1 product(s): 1 items
success, items, msg = search_inventory(inventory, "", "shoes")
print(f"{msg}: {len(items)} items")    # Found 2 product(s): 2 items


# Real-life Example 6: Parse and validate JSON configuration
def parse_config(config: dict) -> tuple[bool, str, dict]:
    if not isinstance(config, dict):
        return False, "Config must be a dictionary", {}
    
    if "app_name" not in config:
        return False, "Missing required field: app_name", {}
    if not isinstance(config["app_name"], str):
        return False, "app_name must be a string", {}
    
    if "port" not in config:
        return False, "Missing required field: port", {}
    if not isinstance(config["port"], int) or config["port"] < 1 or config["port"] > 65535:
        return False, "port must be between 1 and 65535", {}
    
    if "debug" in config and not isinstance(config["debug"], bool):
        return False, "debug must be a boolean", {}
    
    debug = config.get("debug", False)
    return True, "Config valid", {"app": config["app_name"], "port": config["port"], "debug": debug}

success, msg, parsed = parse_config({"app_name": "MyApp", "port": 8080})
print(f"{msg}: {parsed}")    # Config valid: {'app': 'MyApp', 'port': 8080, 'debug': False}
success, msg, parsed = parse_config({"app_name": "MyApp", "port": 70000})
print(f"{msg}: {parsed}")    # port must be between 1 and 65535: {}
