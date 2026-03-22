# Example78.py
# Topic: Real-World Examples of Single Responsibility

# This file provides practical real-world examples applying SRP.


# ============================================================
# Example 1: User Registration
# ============================================================
print("=== Real-world: User Registration ===")

# Separate functions for each responsibility
def validate_username(username):
    """Validate username."""
    return len(username) >= 3 and username.isalnum()

def validate_password(password):
    """Validate password."""
    return len(password) >= 8

def validate_email(email):
    """Validate email."""
    return '@' in email and '.' in email.split('@')[-1]

def hash_password(password):
    """Hash password (simulated)."""
    return f"hashed_{password}"

def create_user_record(username, email, password_hash):
    """Create user record."""
    return {
        'username': username,
        'email': email,
        'password': password_hash,
        'created_at': '2024-01-01'
    }

def save_user_to_database(user):
    """Save user to database (simulated)."""
    print(f"Saving user: {user['username']}")
    return True

def send_welcome_email(email):
    """Send welcome email (simulated)."""
    print(f"Sending welcome email to {email}")

def register_user(username, email, password):
    """Orchestrate user registration."""
    # Validate
    if not validate_username(username):
        return "Invalid username"
    if not validate_email(email):
        return "Invalid email"
    if not validate_password(password):
        return "Invalid password"
    
    # Process
    password_hash = hash_password(password)
    user = create_user_record(username, email, password_hash)
    
    # Save
    save_user_to_database(user)
    
    # Notify
    send_welcome_email(email)
    
    return "User registered successfully"

print(register_user("john123", "john@example.com", "password123"))


# ============================================================
# Example 2: E-commerce Order Processing
# ============================================================
print("\n=== Real-world: Order Processing ===")

def calculate_order_total(items):
    """Calculate order total."""
    return sum(item['price'] * item['qty'] for item in items)

def apply_coupon(total, coupon_code):
    """Apply discount coupon."""
    discounts = {'SAVE10': 0.1, 'SAVE20': 0.2}
    discount = discounts.get(coupon_code, 0)
    return total * (1 - discount)

def calculate_tax(subtotal, tax_rate=0.08):
    """Calculate tax."""
    return subtotal * tax_rate

def validate_inventory(items):
    """Validate all items in stock."""
    for item in items:
        if item['qty'] > item.get('stock', 100):
            return False
    return True

def reserve_inventory(items):
    """Reserve inventory (simulated)."""
    print(f"Reserving inventory for {len(items)} items")
    return True

def process_payment(amount, card_number):
    """Process payment (simulated)."""
    print(f"Processing payment: ${amount}")
    return {'success': True, 'transaction_id': '12345'}

def send_order_confirmation(email, order_details):
    """Send order confirmation (simulated)."""
    print(f"Sending confirmation to {email}")

def process_order(items, card_number, email, coupon=None):
    """Orchestrate order processing."""
    # Calculate total
    subtotal = calculate_order_total(items)
    if coupon:
        subtotal = apply_coupon(subtotal, coupon)
    tax = calculate_tax(subtotal)
    total = subtotal + tax
    
    # Validate
    if not validate_inventory(items):
        return "Item not available"
    
    # Process
    reserve_inventory(items)
    payment = process_payment(total, card_number)
    
    if payment['success']:
        send_order_confirmation(email, {'total': total, 'items': len(items)})
        return "Order processed successfully"
    
    return "Payment failed"

order_items = [
    {'name': 'Widget', 'price': 10, 'qty': 2, 'stock': 50},
    {'name': 'Gadget', 'price': 25, 'qty': 1, 'stock': 30}
]
print(process_order(order_items, "4111111111111111", "user@example.com", "SAVE10"))


# ============================================================
# Example 3: Data Pipeline
# ============================================================
print("\n=== Real-world: Data Pipeline ===")

def fetch_raw_data(source):
    """Fetch data from source (simulated)."""
    return [{"id": 1, "name": "john", "value": 100},
            {"id": 2, "name": "jane", "value": 200}]

def filter_valid_records(records):
    """Filter out invalid records."""
    return [r for r in records if r.get('id') and r.get('name')]

def transform_records(records):
    """Transform records to desired format."""
    return [{'user_id': r['id'], 'display_name': r['name'].title(), 
             'score': r.get('value', 0)} for r in records]

def validate_transformation(records):
    """Validate transformed data."""
    return all(r['score'] >= 0 for r in records)

def load_to_database(records):
    """Load data to database (simulated)."""
    print(f"Loading {len(records)} records to database")
    return True

def run_etl_pipeline(source):
    """Run complete ETL pipeline."""
    # Extract
    raw_data = fetch_raw_data(source)
    
    # Transform
    valid_records = filter_valid_records(raw_data)
    transformed = transform_records(valid_records)
    
    # Validate
    if not validate_transformation(transformed):
        return "Validation failed"
    
    # Load
    load_to_database(transformed)
    
    return f"Loaded {len(transformed)} records"

print(run_etl_pipeline("api"))


# ============================================================
# Example 4: API Handler
# ============================================================
print("\n=== Real-world: API Handler ===")

def parse_request(request_data):
    """Parse incoming request."""
    return request_data.get('body', {})

def authenticate_request(api_key):
    """Authenticate API request."""
    valid_keys = {'key1': 'user1', 'key2': 'user2'}
    return valid_keys.get(api_key)

def validate_request(data):
    """Validate request data."""
    required = ['action', 'payload']
    return all(k in data for k in required)

def execute_action(action, payload):
    """Execute the requested action."""
    actions = {
        'create': lambda p: {'created': p.get('name')},
        'read': lambda p: {'data': 'result'},
        'update': lambda p: {'updated': p.get('name')},
        'delete': lambda p: {'deleted': p.get('id')}
    }
    return actions.get(action, lambda p: {})(payload)

def format_response(data):
    """Format API response."""
    return {'status': 'success', 'data': data}

def handle_api_request(request):
    """Orchestrate API request handling."""
    data = parse_request(request)
    
    user = authenticate_request(request.get('api_key'))
    if not user:
        return {'status': 'error', 'message': 'Unauthorized'}
    
    if not validate_request(data):
        return {'status': 'error', 'message': 'Invalid request'}
    
    result = execute_action(data['action'], data['payload'])
    return format_response(result)

request = {
    'api_key': 'key1',
    'body': {'action': 'create', 'payload': {'name': 'Test'}}
}
print(handle_api_request(request))


# ============================================================
# Example 5: Report Generation
# ============================================================
print("\n=== Real-world: Report Generation ===")

def fetch_sales_data():
    """Fetch sales data (simulated)."""
    return [
        {'product': 'A', 'amount': 100, 'region': 'North'},
        {'product': 'B', 'amount': 200, 'region': 'South'},
        {'product': 'A', 'amount': 150, 'region': 'North'},
    ]

def group_by_region(data):
    """Group sales by region."""
    groups = {}
    for record in data:
        region = record['region']
        groups.setdefault(region, []).append(record)
    return groups

def calculate_region_totals(grouped_data):
    """Calculate totals per region."""
    return {region: sum(r['amount'] for r in records) 
            for region, records in grouped_data.items()}

def calculate_overall_total(data):
    """Calculate overall total."""
    return sum(r['amount'] for r in data)

def format_report(region_totals, overall_total):
    """Format the report."""
    lines = ["Sales Report", "=" * 20]
    for region, total in region_totals.items():
        lines.append(f"{region}: ${total}")
    lines.append(f"\nTotal: ${overall_total}")
    return "\n".join(lines)

def generate_report():
    """Generate sales report."""
    data = fetch_sales_data()
    grouped = group_by_region(data)
    region_totals = calculate_region_totals(grouped)
    overall = calculate_overall_total(data)
    return format_report(region_totals, overall)

print(generate_report())


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("REAL-WORLD SRP EXAMPLES")
print("=" * 50)
print("""
Real-world benefits:
- User registration: validate, process, notify
- Order processing: calculate, validate, charge, confirm
- ETL pipeline: extract, transform, load
- API handlers: parse, authenticate, execute, respond
- Reports: fetch, group, calculate, format

Each function has one clear purpose,
making the code testable and maintainable.
""")
