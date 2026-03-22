# Example33.py
# Topic: Unpacking Iterables into Function Calls

# This file demonstrates how to unpack iterables (lists, tuples, dicts)
# into function arguments using * and ** operators.


from typing import Any


# Function that accepts multiple arguments
def greet(name: str, age: int, city: str) -> str:
    return f"{name} is {age} years old and lives in {city}"

# Unpacking a list/tuple into positional arguments
person = ["Alice", 30, "New York"]
result = greet(*person)    # Unpacks list into 3 positional arguments
print(result)    # Alice is 30 years old and lives in New York


# Function with default arguments
def introduce(name: str, profession: str = "Unknown", country: str = "USA") -> str:
    return f"{name} is a {profession} from {country}"

# Unpacking with some defaults
details = ["Bob", "Engineer"]
result = introduce(*details)    # Only provides 2 args, country uses default
print(result)    # Bob is a Engineer from USA


# Function that accepts **kwargs
def create_user(**info: str) -> dict:
    return info

# Unpacking a dictionary into keyword arguments
user_data = {"username": "alice123", "email": "alice@example.com", "age": "25"}
user = create_user(**user_data)    # Unpacks dict into keyword arguments
print("User: " + str(user))
# User: {'username': 'alice123', 'email': 'alice@example.com', 'age': '25'}


# Function with both *args and **kwargs
def func(*args: Any, **kwargs: Any) -> None:
    print("Positional: " + str(args))
    print("Keyword: " + str(kwargs))

# Unpacking both list and dict
numbers = [1, 2, 3]
config = {"debug": True, "verbose": False}
func(*numbers, **config)
# Positional: (1, 2, 3)
# Keyword: {'debug': True, 'verbose': False}


# Real example: Math operations with list
def calculate(a: float, b: float, c: float) -> dict:
    return {
        "sum": a + b + c,
        "product": a * b * c,
        "average": (a + b + c) / 3
    }

values = [10, 20, 30]
result = calculate(*values)
print("Results: " + str(result))
# Results: {'sum': 60, 'product': 6000, 'average': 20.0}


# Unpacking mixed with regular arguments
def configure(host: str, port: int, **options: Any) -> dict:
    config = {"host": host, "port": port}
    config.update(options)
    return config

base_config = {"host": "localhost"}
extra_options = {"port": 8080, "debug": True}

result = configure(**base_config, **{"port": 8080, "debug": True})
print("Config: " + str(result))
# Config: {'host': 'localhost', 'port': 8080, 'debug': True}


# Using unpacking with range
def print_range(start: int, end: int, step: int = 1) -> None:
    print("Range: ", end="")
    for i in range(start, end, step):
        print(i, end=" ")
    print()

numbers = [0, 10, 2]
print_range(*numbers)    # Range: 0 2 4 6 8


# Unpacking dictionary for function with specific keys
def order_product(product_id: str, quantity: int, price: float) -> dict:
    return {
        "product_id": product_id,
        "quantity": quantity,
        "total": quantity * price
    }

order_data = {"product_id": "ABC123", "quantity": 5}
result = order_product(**order_data, price=19.99)
print("Order: " + str(result))
# Order: {'product_id': 'ABC123', 'quantity': 5, 'total': 99.95}


# Real-life Example 1: Database connection configuration
def connect_database(host: str, port: Any, username: str, password: str, **options: Any) -> dict:
    return {
        "host": host,
        "port": port,
        "user": username,
        "database": options.get("database", "default"),
        "pool_size": options.get("pool_size", 10)
    }

base_conn = {"host": "db.example.com", "port": 5432}
auth = {"username": "admin", "password": "secret"}

conn = connect_database(**base_conn, **auth, database="production")
print(f"Connected to {conn['host']}:{conn['port']}/{conn['database']}")
# Connected to db.example.com:5432/production


# Real-life Example 2: Build HTTP request
def make_request(method: str, url: str, **headers: Any) -> dict:
    return {
        "method": method,
        "url": url,
        "headers": headers
    }

method = "GET"
url = "https://api.example.com/data"

default_headers = {"Accept": "application/json", "User-Agent": "MyApp/1.0"}
auth_headers = {"Authorization": "Bearer token123"}

request = make_request(method, url, **default_headers, **auth_headers)
print(f"Request: {request['method']} {request['url']}")
print(f"Headers: {list(request['headers'].keys())}")
# Request: GET https://api.example.com/data
# Headers: ['Accept', 'User-Agent', 'Authorization']


# Real-life Example 3: Process multiple file paths
def process_file(filepath: str, mode: str = "r", encoding: str = "utf-8") -> str:
    return f"Processing {filepath} in {mode} mode with {encoding} encoding"

files = ["data.txt", "config.json", "output.csv"]
default_mode = "r"
encoding = "utf-8"

for f in files:
    result = process_file(f, default_mode, encoding)
    print(result)
# Processing data.txt in r mode with utf-8 encoding
# Processing config.json in r mode with utf-8 encoding
# Processing output.csv in r mode with utf-8 encoding


# Real-life Example 4: Employee onboarding
def onboard_employee(name: str, department: str, start_date: str, **benefits: Any) -> dict:
    return {
        "name": name,
        "department": department,
        "start_date": start_date,
        "health_insurance": benefits.get("health_insurance", True),
        "retirement_401k": benefits.get("retirement_401k", False),
        "stock_options": benefits.get("stock_options", 0)
    }

base_info = {"name": "Alice Johnson", "department": "Engineering", "start_date": "2024-01-15"}
benefit_options = {"health_insurance": True, "retirement_401k": True, "stock_options": 100}

employee = onboard_employee(**base_info, **benefit_options)
print(f"Employee: {employee['name']}, Dept: {employee['department']}, Stock: {employee['stock_options']}")
# Employee: Alice Johnson, Dept: Engineering, Stock: 100


# Real-life Example 5: Send notification to multiple recipients
def send_notification(title: str, message: str, *recipients: str, **options: Any) -> dict:
    return {
        "title": title,
        "message": message,
        "recipients": list(recipients),
        "priority": options.get("priority", "normal"),
        "channel": options.get("channel", "email")
    }

notification_base = {"title": "System Update", "message": "Server maintenance scheduled"}
recipients = ["user1@example.com", "user2@example.com", "user3@example.com"]
options = {"priority": "high", "channel": "sms"}

notification = send_notification(*recipients, **notification_base, **options)
print(f"Notification: {notification['title']} to {len(notification['recipients'])} recipients via {notification['channel']}")
# Notification: System Update to 3 recipients via sms


# Real-life Example 6: Configure machine learning model
def train_model(model_type: str, **config: Any) -> dict:
    return {
        "model_type": model_type,
        "learning_rate": config.get("learning_rate", 0.001),
        "epochs": config.get("epochs", 100),
        "batch_size": config.get("batch_size", 32),
        "optimizer": config.get("optimizer", "adam")
    }

model_defaults = {"learning_rate": 0.01, "epochs": 50}
model_overrides = {"batch_size": 64, "optimizer": "sgd"}

model = train_model("neural_network", **model_defaults, **model_overrides)
print(f"Model: {model['model_type']}, LR: {model['learning_rate']}, Epochs: {model['epochs']}")
# Model: neural_network, LR: 0.01, Epochs: 50
