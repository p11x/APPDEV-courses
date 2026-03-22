# Example32.py
# Topic: Combining Regular Parameters with *args and **kwargs

# This file demonstrates how to use regular parameters, *args, and **kwargs
# together in various combinations.


from typing import Any, Optional


# Function with required, optional, *args, and **kwargs
def func_demo(required: str, *args: Any, **kwargs: Any) -> None:
    print("Required: " + required)
    print("Args: " + str(args))
    print("Kwargs: " + str(kwargs))

func_demo("hello", 1, 2, 3, name="Alice", age=30)
# Required: hello
# Args: (1, 2, 3)
# Kwargs: {'name': 'Alice', 'age': 30}


# Function with defaults, *args, and **kwargs
def configure(name: str, version: str = "1.0", *features: str, **settings: Any) -> dict:
    config: dict = {"name": name, "version": version}
    if features:
        config["features"] = list(features)
    config.update(settings)
    return config

result = configure("MyApp", "2.0", "auth", "logging", debug=True, port=8080)
print("Config: " + str(result))
# Config: {'name': 'MyApp', 'version': '2.0', 'features': ['auth', 'logging'], 'debug': True, 'port': 8080}


# Function with only required and **kwargs
def create_record(record_type: str, **fields: Any) -> dict:
    record = {"type": record_type}
    record.update(fields)
    return record

user = create_record("user", name="Alice", email="alice@example.com", age=25)
print("Record: " + str(user))
# Record: {'type': 'user', 'name': 'Alice', 'email': 'alice@example.com', 'age': 25}


# Function with required, *args only
def sum_with_prefix(prefix: str, *args: int) -> str:
    total = sum(args)
    return prefix + str(total)

result = sum_with_prefix("Total: ", 1, 2, 3, 4, 5)
print(result)    # Total: 15


# Function with default, *args, and **kwargs
def process_data(data_type: str, multiplier: float = 1.0, *values: float, **options: Any) -> dict:
    result = {
        "type": data_type,
        "multiplied": [v * multiplier for v in values],
        "options": options
    }
    return result

processed = process_data("sales", 1.5, 100, 200, 300, format="json", verbose=True)
print("Processed: " + str(processed))
# Processed: {'type': 'sales', 'multiplied': [150.0, 300.0, 450.0], 'options': {'format': 'json', 'verbose': True}}


# Function with multiple required parameters
def build_message(greeting: str, recipient: str, *args: str, **kwargs: Any) -> str:
    message = greeting + ", " + recipient + "!"
    if args:
        message += " " + " ".join(args)
    if kwargs.get("excited"):
        message = message.replace("!", "!!")
    return message

msg = build_message("Hello", "World", "Welcome to Python", excited=True)
print(msg)    # Hello, World! Welcome to Python!!


# Function where **kwargs must come after *args (syntax demonstration)
def display(format_type: str, *items: str, **config: Any) -> None:
    print("Format: " + format_type)
    if format_type == "list":
        for i, item in enumerate(items, 1):
            print(f"  {i}. {item}")
    elif format_type == "comma":
        print(", ".join(items))
    else:
        print(" ".join(items))
    
    if config.get("uppercase"):
        print("(All items converted to uppercase)")

display("list", "apple", "banana", "cherry")
display("comma", "red", "green", "blue", uppercase=True)


# Real-life Example 1: Order processor with flexible parameters
def process_order(customer: str, *items: str, priority: bool = False, **details: Any) -> dict:
    order = {
        "customer": customer,
        "items": list(items),
        "priority": priority,
        "details": details
    }
    return order

order = process_order("Alice", "Laptop", "Mouse", priority=True, shipping="express", gift_wrap=True)
print(f"Order for {order['customer']}: {len(order['items'])} items, Priority: {order['priority']}")
# Order for Alice: 2 items, Priority: True


# Real-life Example 2: API endpoint builder
def build_endpoint(base_url: str, *path_segments: str, **query_params: Any) -> str:
    path = "/".join(path_segments)
    url = f"{base_url}/{path}"
    
    if query_params:
        params = "&".join(f"{k}={v}" for k, v in query_params.items())
        url += "?" + params
    
    return url

endpoint = build_endpoint("https://api.example.com", "users", "123", "profile", format="json", fields="name,email")
print("Endpoint: " + endpoint)
# Endpoint: https://api.example.com/users/123/profile?format=json&fields=name,email


# Real-life Example 3: Employee report generator
def generate_report(title: str, *employee_ids: int, department: str = "All", **metrics: Any) -> dict:
    return {
        "title": title,
        "department": department,
        "employee_count": len(employee_ids),
        "employee_ids": list(employee_ids),
        "metrics": metrics
    }

report = generate_report(
    "Q1 Performance",
    101, 102, 103, 104,
    department="Engineering",
    avg_rating=4.2,
    retention_rate=0.95,
    projects_completed=15
)
print(f"Report: {report['title']} - {report['employee_count']} employees in {report['department']}")
# Report: Q1 Performance - 4 employees in Engineering


# Real-life Example 4: Email sender with flexible options
def send_email(to: str, *attachments: str, subject: str = "No Subject", **options: Any) -> dict:
    return {
        "to": to,
        "subject": subject,
        "attachments": list(attachments),
        "cc": options.get("cc", []),
        "bcc": options.get("bcc", []),
        "reply_to": options.get("reply_to"),
        "html": options.get("html", False)
    }

email = send_email(
    "user@example.com",
    "report.pdf", "invoice.pdf",
    subject="Monthly Report",
    cc=["manager@example.com"],
    html=True
)
print(f"Email to: {email['to']}, Subject: {email['subject']}, Attachments: {len(email['attachments'])}")
# Email to: user@example.com, Subject: Monthly Report, Attachments: 2


# Real-life Example 5: Database query builder
def execute_query(query: str, *params: Any, connection: str = "default", **options: Any) -> dict:
    return {
        "query": query,
        "parameters": list(params),
        "connection": connection,
        "timeout": options.get("timeout", 30),
        "retry": options.get("retry", 0),
        "cache": options.get("cache", True)
    }

query = execute_query(
    "SELECT * FROM users WHERE status = ?",
    "active",
    connection="read_replica",
    timeout=60,
    cache=True
)
print(f"Query: {query['query'][:30]}..., Connection: {query['connection']}")
# Query: SELECT * FROM users WHERE st..., Connection: read_replica


# Real-life Example 6: Task scheduler
def schedule_task(task_name: str, *tags: str, cron: Optional[str] = None, **kwargs: Any) -> dict:
    return {
        "task_name": task_name,
        "tags": list(tags),
        "cron": cron,
        "retry_count": kwargs.get("retry_count", 0),
        "timeout": kwargs.get("timeout", 300),
        "notify_on_success": kwargs.get("notify_on_success", False),
        "notify_on_failure": kwargs.get("notify_on_failure", True)
    }

task = schedule_task(
    "backup_database",
    "maintenance", "critical",
    cron="0 2 * * *",
    retry_count=3,
    notify_on_failure=True
)
print(f"Task: {task['task_name']}, Schedule: {task['cron']}, Tags: {', '.join(task['tags'])}")
# Task: backup_database, Schedule: 0 2 * * *, Tags: maintenance, critical
