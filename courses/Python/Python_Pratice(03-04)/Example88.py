# Example88.py
# Topic: Real-World Examples with Function Signatures

# This file provides practical real-world examples.


# ============================================================
# Example 1: API Functions
# ============================================================
print("=== Real-world: API Functions ===")

# API endpoint with clear parameters
def get_user(user_id, /, *, include_profile=False, include_settings=False):
    """
    Get user by ID.
    
    Args:
        user_id: Positional-only (required)
        include_profile: Keyword-only (optional)
        include_settings: Keyword-only (optional)
    """
    result = {"id": user_id, "name": "John"}
    
    if include_profile:
        result["profile"] = {"bio": "Hello"}
    if include_settings:
        result["settings"] = {"theme": "dark"}
    
    return result

# Test
print(get_user(1))
print(get_user(1, include_profile=True))
print(get_user(1, include_profile=True, include_settings=True))


# ============================================================
# Example 2: Database Functions
# ============================================================
print("\n=== Real-world: Database Functions ===")

def insert_record(table, /, **columns):
    """
    Insert record into table.
    
    Args:
        table: Positional-only
        **columns: Keyword-only data
    """
    return f"INSERT INTO {table} ({', '.join(columns.keys())}) VALUES ({', '.join(repr(v) for v in columns.values())})"

# Test
print(insert_record("users", name="John", email="john@example.com"))
print(insert_record("products", name="Widget", price=10))


# ============================================================
# Example 3: Configuration Functions
# ============================================================
print("\n=== Real-world: Configuration ===")

def create_server(host, /, port=8080, *, ssl=False, workers=1):
    """
    Create server configuration.
    
    Args:
        host: Positional-only (required)
        port: Either (optional, default 8080)
        ssl: Keyword-only (optional)
        workers: Keyword-only (optional)
    """
    config = {
        "host": host,
        "port": port,
        "ssl": ssl,
        "workers": workers
    }
    return config

# Test
print(create_server("localhost"))
print(create_server("localhost", 9000))
print(create_server("localhost", ssl=True, workers=4))


# ============================================================
# Example 4: Validation Functions
# ============================================================
print("\n=== Real-world: Validation ===")

def validate_field(name, /, *, required=True, min_length=None, max_length=None):
    """
    Validate a form field.
    
    Args:
        name: Positional-only
        required: Keyword-only
        min_length: Keyword-only
        max_length: Keyword-only
    """
    rules = {"name": name, "required": required}
    if min_length:
        rules["min_length"] = min_length
    if max_length:
        rules["max_length"] = max_length
    return rules

# Test
print(validate_field("username", required=True, min_length=3))
print(validate_field("email", required=True))


# ============================================================
# Example 5: Calculator Class
# ============================================================
print("\n=== Real-world: Calculator ===")

class Calculator:
    def __init__(self, /, initial_value=0):
        """Positional-only initial value."""
        self.value = initial_value
    
    def add(self, n):
        self.value += n
        return self
    
    def subtract(self, n):
        self.value -= n
        return self
    
    def multiply(self, n):
        self.value *= n
        return self
    
    def get_value(self):
        return self.value

# Test
calc = Calculator(10)
result = calc.add(5).multiply(2).subtract(10).get_value()
print(f"Result: {result}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("REAL-WORLD FUNCTION SIGNATURES")
print("=" * 50)
print("""
Use cases:

1. APIs:
   - Required params: positional-only
   - Optional params: keyword-only

2. Database:
   - Table name: positional-only
   - Data: keyword-only

3. Configuration:
   - Required: positional-only
   - Optional: keyword-only

4. Validation:
   - Field name: positional-only
   - Rules: keyword-only

Benefits:
- Clear API contracts
- Prevent accidental keyword args
- Enforce required vs optional
""")
