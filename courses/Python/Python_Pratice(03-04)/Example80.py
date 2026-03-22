# Example80.py
# Topic: Comprehensive Review - Single Responsibility

# This file provides a comprehensive review of SRP.


# ============================================================
# Example 1: SRP Principles
# ============================================================
print("=== SRP Principles ===")

# Each function has ONE responsibility
def get_user(user_id):
    """One thing: retrieve user."""
    return {'id': user_id, 'name': 'John'}

def validate_user(user):
    """One thing: validate user."""
    return 'name' in user and 'id' in user

def format_user(user):
    """One thing: format user data."""
    return {'id': user['id'], 'name': user['name'].title()}

def save_user(user):
    """One thing: save user."""
    print(f"Saving: {user}")
    return True


# ============================================================
# Example 2: Identifying Responsibilities
# ============================================================
print("\n=== Identifying Responsibilities ===")

# A "register user" has multiple responsibilities:
# 1. Validate input
# 2. Check duplicates
# 3. Hash password
# 4. Create record
# 5. Send email

# Each should be separate
def validate_input(data):
    return 'email' in data and 'password' in data

def check_duplicates(email):
    return False  # Simulated

def hash_password(password):
    return f"hash_{password}"

def create_record(email, password_hash):
    return {'email': email, 'password': password_hash}

def send_email(email):
    print(f"Email sent to {email}")

# Compose them
def register_user(email, password):
    if not validate_input({'email': email, 'password': password}):
        return "Invalid input"
    if check_duplicates(email):
        return "User exists"
    
    password_hash = hash_password(password)
    record = create_record(email, password_hash)
    save_user(record)
    send_email(email)
    
    return "Registered"


# ============================================================
# Example 3: When to Split Functions
# ============================================================
print("\n=== When to Split Functions ===")

# SPLIT when:
# 1. Function does more than one thing
# 2. Function is hard to name
# 3. Function is hard to test
# 4. Function has multiple reasons to change

# Example: Calculate and print
def calculate_and_print_bad(amount):
    """Does two things: calculates and prints."""
    result = amount * 1.1
    print(f"Result: {result}")
    return result

# Better: Separate
def calculate_total(amount, tax=0.1):
    return amount * (1 + tax)

def print_result(result):
    print(f"Result: {result}")

# Usage
result = calculate_total(100)
print_result(result)


# ============================================================
# Example 4: SRP and Code Organization
# ============================================================
print("\n=== SRP and Code Organization ===")

# Group related single-responsibility functions
class UserService:
    def __init__(self):
        self.users = {}
    
    def validate_email(self, email):
        return '@' in email
    
    def validate_password(self, password):
        return len(password) >= 8
    
    def hash_password(self, password):
        return f"hashed_{password}"
    
    def create_user(self, email, password):
        return {'email': email, 'password': self.hash_password(password)}
    
    def save_user(self, user):
        self.users[user['email']] = user
        return user

service = UserService()
if service.validate_email("test@example.com"):
    user = service.create_user("test@example.com", "password123")
    service.save_user(user)
    print(f"User created: {user}")


# ============================================================
# Example 5: Summary Review
# ============================================================
print("\n=== Summary Review ===")

# BAD examples
def do_everything_bad(data):
    # Validate
    # Transform
    # Save
    # Send notification
    # Log
    pass

# GOOD examples - each does one thing
def validate(data):
    pass

def transform(data):
    pass

def save(data):
    pass

def notify(data):
    pass

def log(data):
    pass

def process(data):
    """Orchestrates the steps."""
    if not validate(data):
        return "Invalid"
    transformed = transform(data)
    save(transformed)
    notify(transformed)
    log(transformed)
    return "Done"


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMPREHENSIVE SUMMARY: SRP")
print("=" * 50)
print("""
SINGLE RESPONSIBILITY PRINCIPLE:

- Each function should have ONE reason to change
- Name should clearly indicate what function does
- Use "and" in description? Probably too much!
- Benefits:
  * Easier to test
  * Easier to understand
  * More reusable
  * Easier to maintain

Remember:
- Do one thing well
- Name specifically
- Compose small functions
""")
