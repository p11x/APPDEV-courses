# Example31.py
# Topic: **kwargs - Variable Keyword Arguments

# This file demonstrates the use of **kwargs to accept any number of
# keyword arguments, which are packed into a dictionary.


from typing import Any


# Creates and returns a user profile from keyword arguments
def create_user_profile(**kwargs: str) -> dict:
    return {
        "username": kwargs.get("username", "anonymous"),
        "email": kwargs.get("email", "no@email.com"),
        "age": kwargs.get("age", "unknown"),
        "location": kwargs.get("location", "unknown")
    }

profile = create_user_profile(username="alice", email="alice@example.com", age="25", location="NYC")
print("User profile: " + str(profile))
# {'username': 'alice', 'email': 'alice@example.com', 'age': '25', 'location': 'NYC'}


# Returns configuration settings with defaults
def get_server_config(**kwargs: Any) -> dict:
    return {
        "host": kwargs.get("host", "localhost"),
        "port": kwargs.get("port", 8080),
        "debug": kwargs.get("debug", False),
        "timeout": kwargs.get("timeout", 30),
        "max_connections": kwargs.get("max_connections", 100)
    }

config = get_server_config(host="0.0.0.0", port=3000, debug=True)
print("Server config: " + str(config))
# {'host': '0.0.0.0', 'port': 3000, 'debug': True, 'timeout': 30, 'max_connections': 100}


# Prints all keyword arguments with their values
def print_settings(**kwargs: Any) -> None:
    for key, value in kwargs.items():
        print(f"  {key}: {value}")

print("Settings:")
print_settings(theme="dark", language="en", notifications=True, font_size=14)


# Returns a product with all provided attributes
def create_product(**kwargs: Any) -> dict:
    return {
        "name": kwargs.get("name", "Unnamed Product"),
        "price": kwargs.get("price", 0.0),
        "category": kwargs.get("category", "general"),
        "in_stock": kwargs.get("in_stock", True),
        "tags": kwargs.get("tags", [])
    }

product = create_product(name="Laptop", price=999.99, category="electronics", tags=["computer", "tech"])
print("Product: " + str(product))
# {'name': 'Laptop', 'price': 999.99, 'category': 'electronics', 'in_stock': True, 'tags': ['computer', 'tech']}


# Returns statistics from keyword arguments
def calculate_stats(**kwargs: int) -> dict:
    values = list(kwargs.values())
    if not values:
        return {"count": 0, "sum": 0, "average": 0}
    
    return {
        "count": len(values),
        "sum": sum(values),
        "average": sum(values) / len(values),
        "min": min(values),
        "max": max(values)
    }

stats = calculate_stats(apple=5, banana=3, orange=8, mango=12)
print("Stats: " + str(stats))
# {'count': 4, 'sum': 28, 'average': 7.0, 'min': 3, 'max': 12}


# Returns a formatted employee record
def create_employee(**kwargs: str) -> dict:
    return {
        "first_name": kwargs.get("first_name", ""),
        "last_name": kwargs.get("last_name", ""),
        "employee_id": kwargs.get("employee_id", "0000"),
        "department": kwargs.get("department", "Unassigned"),
        "title": kwargs.get("title", "Employee")
    }

emp = create_employee(first_name="John", last_name="Doe", employee_id="12345", department="Engineering", title="Senior Developer")
print("Employee: " + str(emp))
# {'first_name': 'John', 'last_name': 'Doe', 'employee_id': '12345', 'department': 'Engineering', 'title': 'Senior Developer'}


# Returns weather data from keyword arguments
def get_weather_data(**kwargs: float) -> dict:
    return {
        "temperature": kwargs.get("temperature", 0.0),
        "humidity": kwargs.get("humidity", 0.0),
        "wind_speed": kwargs.get("wind_speed", 0.0),
        "pressure": kwargs.get("pressure", 1013.25)
    }

weather = get_weather_data(temperature=72.5, humidity=65.0, wind_speed=10.0)
print("Weather: " + str(weather))
# {'temperature': 72.5, 'humidity': 65.0, 'wind_speed': 10.0, 'pressure': 1013.25}


# Returns all kwargs as a formatted string
def format_attributes(**kwargs: Any) -> str:
    parts = []
    for key, value in kwargs.items():
        parts.append(f"{key}={value}")
    return ", ".join(parts)

result = format_attributes(name="Widget", price=19.99, in_stock=True)
print("Attributes: " + result)
# Attributes: name=Widget, price=19.99, in_stock=True


# Real-life Example 1: Configure email campaign settings
def configure_email_campaign(**settings: Any) -> dict:
    return {
        "campaign_name": settings.get("campaign_name", "Untitled Campaign"),
        "subject": settings.get("subject", ""),
        "from_name": settings.get("from_name", "Company"),
        "from_email": settings.get("from_email", "noreply@company.com"),
        "send_time": settings.get("send_time", "now"),
        "track_opens": settings.get("track_opens", True),
        "track_clicks": settings.get("track_clicks", True)
    }

campaign = configure_email_campaign(
    campaign_name="Summer Sale",
    subject="50% Off Everything!",
    from_name="Sales Team",
    send_time="2024-06-15 09:00"
)
print(f"Campaign: {campaign['campaign_name']}, Subject: {campaign['subject']}")
# Campaign: Summer Sale, Subject: 50% Off Everything!


# Real-life Example 2: Build database query parameters
def build_query_params(**params: Any) -> dict:
    return {
        "table": params.get("table", ""),
        "select": params.get("select", "*"),
        "where": params.get("where", {}),
        "order_by": params.get("order_by", "id"),
        "limit": params.get("limit", 100),
        "offset": params.get("offset", 0)
    }

query = build_query_params(
    table="users",
    select="id, name, email",
    where={"status": "active", "role": "admin"},
    order_by="created_at",
    limit=50
)
print(f"Query: {query['table']}, Select: {query['select']}")
# Query: users, Select: id, name, email


# Real-life Example 3: Create API response with metadata
def create_api_response(data: Any, **metadata: Any) -> dict:
    return {
        "success": metadata.get("success", True),
        "data": data,
        "message": metadata.get("message", "Operation successful"),
        "timestamp": metadata.get("timestamp", "2024-01-01T00:00:00Z"),
        "version": metadata.get("version", "1.0"),
        "errors": metadata.get("errors", [])
    }

response = create_api_response(
    {"user_id": 123, "name": "Alice"},
    message="User retrieved",
    version="2.0"
)
print(f"Response success: {response['success']}, Message: {response['message']}")
# Response success: True, Message: User retrieved


# Real-life Example 4: Configure payment processor
def configure_payment_processor(**config: Any) -> dict:
    return {
        "processor": config.get("processor", "stripe"),
        "api_key": config.get("api_key", ""),
        "webhook_url": config.get("webhook_url", ""),
        "currency": config.get("currency", "USD"),
        "test_mode": config.get("test_mode", True),
        "timeout": config.get("timeout", 30)
    }

payment = configure_payment_processor(
    processor="stripe",
    webhook_url="https://mysite.com/webhook",
    currency="EUR",
    test_mode=False
)
print(f"Processor: {payment['processor']}, Currency: {payment['currency']}, Test: {payment['test_mode']}")
# Processor: stripe, Currency: EUR, Test: False


# Real-life Example 5: Build notification payload
def build_notification(**notification: Any) -> dict:
    return {
        "type": notification.get("type", "info"),
        "title": notification.get("title", ""),
        "body": notification.get("body", ""),
        "priority": notification.get("priority", "normal"),
        " recipients": notification.get("recipients", []),
        "schedule": notification.get("schedule", None)
    }

notif = build_notification(
    type="alert",
    title="System Maintenance",
    body="Scheduled maintenance in 1 hour",
    priority="high",
    recipients=["all_users"]
)
print(f"Notification: {notif['type']} - {notif['title']}")
# Notification: alert - System Maintenance


# Real-life Example 6: Configure image processing options
def configure_image_processor(**options: Any) -> dict:
    return {
        "width": options.get("width", 800),
        "height": options.get("height", 600),
        "format": options.get("format", "jpeg"),
        "quality": options.get("quality", 85),
        "compression": options.get("compression", "balanced"),
        "watermark": options.get("watermark", False)
    }

image_config = configure_image_processor(
    width=1920,
    height=1080,
    format="png",
    quality=95,
    watermark=True
)
print(f"Image: {image_config['width']}x{image_config['height']}, Format: {image_config['format']}")
# Image: 1920x1080, Format: png
