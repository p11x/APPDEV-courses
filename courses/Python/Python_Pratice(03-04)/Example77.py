# Example77.py
# Topic: Common Mistakes with Single Responsibility

# This file shows common mistakes when applying SRP.


# ============================================================
# Example 1: Too Much in One Function
# ============================================================
print("=== Mistake: Too Much in One Function ===")

# BAD: Function does validation, formatting, and saving
def register_user_bad(name, email, age):
    # Validation
    if not name or len(name) < 2:
        return "Invalid name"
    if '@' not in email:
        return "Invalid email"
    if age < 18:
        return "Must be 18+"
    
    # Formatting
    name = name.title().strip()
    email = email.lower().strip()
    
    # Saving
    print(f"Saving: {name}, {email}, {age}")
    return "Registered"

# GOOD: Separate functions
def validate_name(name):
    return bool(name and len(name) >= 2)

def validate_email(email):
    return '@' in email

def validate_age(age):
    return age >= 18

def format_name(name):
    return name.title().strip()

def format_email(email):
    return email.lower().strip()

def save_user(name, email, age):
    print(f"Saving: {name}, {email}, {age}")

def register_user(name, email, age):
    if not validate_name(name):
        return "Invalid name"
    if not validate_email(email):
        return "Invalid email"
    if not validate_age(age):
        return "Must be 18+"
    
    name = format_name(name)
    email = format_email(email)
    save_user(name, email, age)
    return "Registered"

print(register_user("john", "JOHN@EXAMPLE.COM", 25))


# ============================================================
# Example 2: Naming Issues
# ============================================================
print("\n=== Mistake: Poor Naming ===")

# BAD: Vague names that don't indicate responsibility
def handle_stuff(data):
    """What does this do?"""
    pass

def process(x, y):
    """Too generic"""
    pass

def do_it(item):
    """Meaningless"""
    pass

# GOOD: Specific names that indicate purpose
def calculate_total_price(items, tax_rate=0.1):
    """Calculates total with tax."""
    subtotal = sum(item['price'] * item['qty'] for item in items)
    return subtotal * (1 + tax_rate)

def validate_product_inventory(product, requested_qty):
    """Validates if enough inventory."""
    return product['quantity'] >= requested_qty

def send_order_confirmation(email, order_details):
    """Sends confirmation email."""
    print(f"Sending to {email}")


# ============================================================
# Example 3: Not Separating Concerns
# ============================================================
print("\n=== Mistake: Not Separating Concerns ===")

# BAD: Mixing business logic with I/O
def create_report_bad(data):
    # Query database
    results = f"Got {len(data)} records"
    # Transform
    formatted = [r.upper() for r in data]
    # Write file
    print(f"Writing {formatted}")
    return formatted

# GOOD: Separate I/O from logic
def fetch_data():
    """I/O: Get data from source."""
    return ["record1", "record2", "record3"]

def transform_data(data):
    """Logic: Transform data."""
    return [r.upper() for r in data]

def save_report(data):
    """I/O: Save report."""
    print(f"Writing {data}")

def create_report():
    """Orchestrate the process."""
    data = fetch_data()
    transformed = transform_data(data)
    save_report(transformed)
    return transformed


# ============================================================
# Example 4: Combining Read and Write
# ============================================================
print("\n=== Mistake: Read + Write in One Function ===")

# BAD: Function reads and modifies state
counter = 0

def increment_and_get_bad():
    global counter
    counter += 1
    return counter

# GOOD: Separate read and write, or use return values
def increment(counter):
    """Pure: Returns new value."""
    return counter + 1

def get_value(counter):
    """Pure: Returns current value."""
    return counter

# Usage
counter = increment(counter)
counter = increment(counter)
print(f"Value: {get_value(counter)}")


# ============================================================
# Example 5: Avoiding SRP Leads to Testing Issues
# ============================================================
print("\n=== Testing with SRP ===")

# BAD: Hard to test because of side effects
def process_payment_and_send_receipt_bad(payment_data):
    # Process payment (side effect)
    print("Processing payment...")
    # Send receipt (side effect)
    print("Sending receipt...")
    return "Done"

# GOOD: Easy to test because functions are pure
def calculate_payment_amount(items, discount=0):
    """Pure: Easy to test."""
    subtotal = sum(item['price'] * item['qty'] for item in items)
    return subtotal * (1 - discount)

def process_payment(amount):
    """Side effect: Could be mocked."""
    return {"status": "success", "amount": amount}

def send_receipt(email, amount):
    """Side effect: Could be mocked."""
    return {"sent": True}

def process_order(items, email, discount=0):
    """Orchestrates, easy to test logic."""
    amount = calculate_payment_amount(items, discount)
    payment = process_payment(amount)
    if payment['status'] == 'success':
        send_receipt(email, amount)
    return payment

# Test the pure function easily
items = [{'price': 10, 'qty': 2}, {'price': 5, 'qty': 3}]
print(f"Amount: {calculate_payment_amount(items)}")
print(f"With discount: {calculate_payment_amount(items, 0.1)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMMON MISTAKES WITH SRP")
print("=" * 50)
print("""
- Putting too much logic in one function
- Using vague, generic names
- Mixing different concerns (I/O + logic)
- Combining read and write operations
- Making functions hard to test

Remember:
- Name should indicate what function does
- If you use "and" to describe function, it's doing too much
- Small, focused functions are easier to test and reuse
""")
