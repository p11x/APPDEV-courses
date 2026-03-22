# Example37.py
# Topic: Type Annotations in Depth

# This file demonstrates comprehensive type annotations including
# basic types, Optional, Union, List, Dict, Callable, and custom types.


from typing import Optional, Union, List, Dict, Callable, Any, Tuple


# Basic Type Annotations
def greet(name: str, age: int) -> str:
    """Greet a person with their name and age."""
    return f"Hello, {name}! You are {age} years old."

print(greet("Alice", 30))    # Hello, Alice! You are 30 years old.


# Optional Type - parameter may be None
def find_user(user_id: int, include_email: bool = True) -> Optional[Dict[str, str]]:
    """Find a user by ID.
    
    Args:
        user_id: The unique identifier of the user.
        include_email: Whether to include email in results.
        
    Returns:
        User dictionary or None if not found.
    """
    users = {
        1: {"name": "Alice", "email": "alice@example.com"},
        2: {"name": "Bob", "email": "bob@example.com"}
    }
    
    if user_id not in users:
        return None
    
    user = users[user_id]
    if not include_email:
        return {"name": user["name"]}
    return user

user = find_user(1)
print(f"User: {user}")    # {'name': 'Alice', 'email': 'alice@example.com'}

not_found = find_user(999)
print(f"Not found: {not_found}")    # None


# Union Type - parameter can be multiple types
def process_value(value: Union[int, float, str]) -> str:
    """Process a value and return its type description."""
    if isinstance(value, int):
        return f"Integer: {value * 2}"
    elif isinstance(value, float):
        return f"Float: {value ** 2}"
    else:
        return f"String: {value.upper()}"

print(process_value(10))      # Integer: 20
print(process_value(3.14))     # Float: 9.8596
print(process_value("hello"))  # String: HELLO


# List Type Annotation
def calculate_sum(numbers: List[int]) -> int:
    """Calculate the sum of a list of integers."""
    return sum(numbers)

def get_first_three(items: List[str]) -> List[str]:
    """Return the first three items from a list."""
    return items[:3] if len(items) >= 3 else items

print(calculate_sum([1, 2, 3, 4, 5]))                    # 15
print(get_first_three(["a", "b", "c", "d", "e"]))        # ['a', 'b', 'c']


# Dict Type Annotation
def create_user_profile(name: str, age: int, **kwargs: str) -> Dict[str, Union[str, int]]:
    """Create a user profile dictionary."""
    profile = {"name": name, "age": age}
    profile.update(kwargs)
    return profile

profile = create_user_profile("Alice", 30, city="NYC", occupation="Engineer")
print(f"Profile: {profile}")
# Profile: {'name': 'Alice', 'age': 30, 'city': 'NYC', 'occupation': 'Engineer'}


# Tuple Type Annotation
def get_coordinates() -> Tuple[float, float]:
    """Return latitude and longitude."""
    return (40.7128, -74.0060)

def get_name_and_age() -> Tuple[str, int]:
    """Return name and age as a tuple."""
    return ("Alice", 30)

lat, lon = get_coordinates()
print(f"Location: {lat}, {lon}")    # Location: 40.7128, -74.0060

name, age = get_name_and_age()
print(f"Name: {name}, Age: {age}")  # Name: Alice, Age: 30


# Callable Type Annotation
def apply_operation(a: int, b: int, operation: Callable[[int, int], int]) -> int:
    """Apply an operation to two numbers."""
    return operation(a, b)

def add(x: int, y: int) -> int:
    return x + y

def multiply(x: int, y: int) -> int:
    return x * y

print(apply_operation(5, 3, add))       # 8
print(apply_operation(5, 3, multiply))  # 15


# Callable with no return value
def execute_callback(callback: Callable[[], None]) -> None:
    """Execute a callback function."""
    print("Before callback")
    callback()
    print("After callback")

def my_callback() -> None:
    print("Callback executed!")

execute_callback(my_callback)
# Before callback
# Callback executed!
# After callback


# Any Type - when type is unknown
def serialize_json(data: Any) -> str:
    """Serialize data to JSON string."""
    return str(data)

print(serialize_json(42))           # 42
print(serialize_json([1, 2, 3]))    # [1, 2, 3]
print(serialize_json({"key": "value"}))  # {'key': 'value'}


# Complex Type Example: Function that takes a list of functions
def apply_all_operations(values: List[int], operations: List[Callable[[int], int]]) -> List[int]:
    """Apply multiple operations to a list of values."""
    results = []
    for value in values:
        for op in operations:
            results.append(op(value))
    return results

def double(x: int) -> int:
    return x * 2

def square(x: int) -> int:
    return x * x

result = apply_all_operations([1, 2, 3], [double, square])
print(f"Results: {result}")    # Results: [2, 1, 4, 4, 6, 9]


# Type Alias - creating descriptive type names
UserID = int
UserDict = Dict[str, Union[str, int, bool]]

def get_user_by_id(user_id: UserID) -> Optional[UserDict]:
    """Get user by their ID."""
    users: Dict[UserID, UserDict] = {
        1: {"name": "Alice", "age": 30, "active": True},
        2: {"name": "Bob", "age": 25, "active": False}
    }
    return users.get(user_id)

user = get_user_by_id(1)
print(f"User: {user}")    # User: {'name': 'Alice', 'age': 30, 'active': True}


# Real-life Example 1: Type-annotated data processor
ResultType = Tuple[bool, Optional[str], Optional[Dict[str, Any]]]

def process_payment(
    amount: float,
    currency: str,
    card_number: str,
    cvv: str,
    expiry: str
) -> ResultType:
    """Process a payment transaction.
    
    Args:
        amount: Payment amount.
        currency: Currency code (USD, EUR, etc.).
        card_number: Credit card number.
        cvv: Card verification value.
        expiry: Card expiry date (MM/YY).
        
    Returns:
        Tuple of (success, error_message, transaction_data).
    """
    if amount <= 0:
        return (False, "Amount must be positive", None)
    
    if len(card_number) < 13:
        return (False, "Invalid card number", None)
    
    # Simulate successful payment
    transaction = {
        "transaction_id": "TXN123456",
        "amount": amount,
        "currency": currency,
        "status": "completed"
    }
    return (True, None, transaction)

success, error, txn = process_payment(99.99, "USD", "4111111111111111", "123", "12/25")
if success:
    print(f"Payment successful: {txn['transaction_id']}")
# Payment successful: TXN123456


# Real-life Example 2: Type-annotated API handler
RequestHandler = Callable[[Dict[str, Any]], Dict[str, Any]]

def handle_api_request(
    endpoint: str,
    method: str,
    handlers: Dict[str, RequestHandler],
    headers: Optional[Dict[str, str]] = None
) -> Dict[str, Any]:
    """Handle an API request using registered handlers."""
    if endpoint not in handlers:
        return {
            "status": 404,
            "error": f"No handler for endpoint: {endpoint}"
        }
    
    request = {
        "endpoint": endpoint,
        "method": method,
        "headers": headers or {}
    }
    
    return handlers[endpoint](request)

def users_handler(request: Dict[str, Any]) -> Dict[str, Any]:
    return {"status": 200, "data": [{"id": 1, "name": "Alice"}]}

handlers = {
    "/api/users": users_handler
}

response = handle_api_request("/api/users", "GET", handlers)
print(f"API Response: {response['status']}")    # API Response: 200


# Real-life Example 3: Type-annotated data pipeline
DataTransformer = Callable[[List[Dict[str, Any]]], List[Dict[str, Any]]]

def run_data_pipeline(
    source_data: List[Dict[str, Any]],
    transformers: List[DataTransformer],
    validator: Optional[Callable[[Dict[str, Any]], bool]] = None
) -> Tuple[bool, List[Dict[str, Any]], List[str]]:
    """Run a data transformation pipeline.
    
    Args:
        source_data: Input data records.
        transformers: List of transformation functions.
        validator: Optional validation function.
        
    Returns:
        Tuple of (success, transformed_data, error_messages).
    """
    errors: List[str] = []
    current_data = source_data
    
    for i, transformer in enumerate(transformers):
        try:
            current_data = transformer(current_data)
        except Exception as e:
            errors.append(f"Transformer {i} failed: {str(e)}")
            return (False, [], errors)
    
    if validator:
        for i, record in enumerate(current_data):
            if not validator(record):
                errors.append(f"Record {i} failed validation")
    
    return (True, current_data, errors)

def filter_active(records: List[Dict[str, Any]]) -> List[Dict[str, Any]]:
    return [r for r in records if r.get("active", False)]

def add_timestamp(records: List[Dict[str, Any]]) -> List[Dict[str, Any]]:
    import datetime
    return [{**r, "timestamp": datetime.datetime.now().isoformat()} for r in records]

data = [
    {"id": 1, "name": "Alice", "active": True},
    {"id": 2, "name": "Bob", "active": False},
    {"id": 3, "name": "Carol", "active": True}
]

success, result, errors = run_data_pipeline(data, [filter_active, add_timestamp])
print(f"Success: {success}, Records: {len(result)}")
# Success: True, Records: 2
