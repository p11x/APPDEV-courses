# Example76.py
# Topic: Single Responsibility Principle

# This file demonstrates the Single Responsibility Principle (SRP) in function design.


# ============================================================
# Example 1: What is Single Responsibility?
# ============================================================
print("=== Single Responsibility Principle ===")

# BAD: Function doing multiple things
def process_user_bad(user_data):
    # Validates
    if not user_data.get('email'):
        return False
    # Formats
    user_data['name'] = user_data['name'].title()
    # Saves
    print(f"Saving {user_data}")
    return True

# GOOD: Each function has one responsibility
def validate_user(user_data):
    """Only validates user data."""
    return bool(user_data.get('email'))

def format_user(user_data):
    """Only formats user data."""
    return {**user_data, 'name': user_data['name'].title()}

def save_user(user_data):
    """Only saves user data."""
    print(f"Saving {user_data}")
    return True

# Use them together
user = {'name': 'john doe', 'email': 'john@example.com'}
if validate_user(user):
    formatted = format_user(user)
    save_user(formatted)


# ============================================================
# Example 2: Benefits of SRP
# ============================================================
print("\n=== Benefits of SRP ===")

# Easy to test
def validate_email(email):
    """Single responsibility: validate email."""
    return '@' in email and '.' in email.split('@')[-1]

def sanitize_email(email):
    """Single responsibility: sanitize email."""
    return email.strip().lower()

def send_email(email, message):
    """Single responsibility: send email."""
    print(f"Sending to {email}: {message}")

# Each can be tested independently
print(f"Validate: {validate_email('test@example.com')}")
print(f"Sanitize: {sanitize_email('  TEST@Example.COM  ')}")


# ============================================================
# Example 3: Refactoring complex functions
# ============================================================
print("\n=== Refactoring Complex Functions ===")

# BEFORE: One function doing everything
def process_order_bad(order):
    # Validate
    if not order.get('items'):
        return "No items"
    # Calculate
    total = sum(item['price'] * item['qty'] for item in order['items'])
    # Apply discount
    if total > 100:
        total *= 0.9
    # Save
    print(f"Saving order: {total}")
    return total

# AFTER: Separate responsibilities
def validate_order(order):
    return bool(order.get('items'))

def calculate_total(order):
    return sum(item['price'] * item['qty'] for item in order['items'])

def apply_discount(total, threshold=100, discount=0.9):
    return total * discount if total > threshold else total

def save_order(order, total):
    print(f"Saving order: {total}")
    return total

# Compose them
def process_order(order):
    if not validate_order(order):
        return "No items"
    total = calculate_total(order)
    total = apply_discount(total)
    return save_order(order, total)

order = {'items': [{'price': 50, 'qty': 2}, {'price': 25, 'qty': 1}]}
result = process_order(order)
print(f"Order total: {result}")


# ============================================================
# Example 4: Naming and SRP
# ============================================================
print("\n=== Naming and SRP ===")

# Clear, specific names indicate single responsibility
def get_user_by_id(user_id):
    """Retrieves a single user."""
    pass

def calculate_user_age(birth_date):
    """Calculates age from birth date."""
    pass

def format_user_display_name(first_name, last_name):
    """Formats display name."""
    pass

def send_welcome_email(user_email):
    """Sends welcome email."""
    pass

# Bad names suggest multiple responsibilities
def handle_user_everything():  # Too vague
    pass


# ============================================================
# Example 5: Real-world example
# ============================================================
print("\n=== Real-world: Payment Processing ===")

# Each function does one thing
def validate_card(card_number):
    """Validate card number."""
    return len(card_number) == 16 and card_number.isdigit()

def calculate_fee(amount, fee_percent=2.5):
    """Calculate processing fee."""
    return amount * (fee_percent / 100)

def charge_card(card_number, amount):
    """Charge the card."""
    print(f"Charging ${amount} to card {card_number[-4:]}")
    return {"status": "success", "amount": amount}

def send_receipt(email, amount):
    """Send receipt email."""
    print(f"Receipt sent to {email}")

def process_payment(card_number, amount, email):
    """Orchestrate payment processing."""
    if not validate_card(card_number):
        return "Invalid card"
    
    fee = calculate_fee(amount)
    total = amount + fee
    
    result = charge_card(card_number, total)
    if result['status'] == 'success':
        send_receipt(email, total)
    
    return result

# Test
result = process_payment("1234567890123456", 100, "user@example.com")
print(f"Payment result: {result}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Single Responsibility Principle")
print("=" * 50)
print("""
- Each function should do ONE thing
- Makes functions:
  * Easier to test
  * Easier to understand
  * More reusable
  * Easier to maintain
- Use clear, specific names
- Compose small functions for complex tasks
""")
