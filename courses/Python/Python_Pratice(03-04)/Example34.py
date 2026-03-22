# Example34.py
# Topic: Advanced *args and **kwargs Examples

# This file demonstrates more advanced and practical uses of *args and **kwargs
# including decorators, wrapper functions, and complex scenarios.


from typing import Any, Callable


# Wrapper function that logs all arguments
def log_function_call(func: Callable) -> Callable:
    def wrapper(*args: Any, **kwargs: Any) -> Any:
        print(f"Calling {func.__name__} with args={args}, kwargs={kwargs}")
        result = func(*args, **kwargs)
        print(f"{func.__name__} returned: {result}")
        return result
    return wrapper

@log_function_call
def add(a: int, b: int) -> int:
    return a + b

result = add(5, 3)
# Calling add with args=(5, 3), kwargs={}
# add returned: 8


# Decorator that measures execution time
def timer(func: Callable) -> Callable:
    import time
    def wrapper(*args: Any, **kwargs: Any) -> Any:
        start = time.time()
        result = func(*args, **kwargs)
        elapsed = time.time() - start
        print(f"{func.__name__} took {elapsed:.4f} seconds")
        return result
    return wrapper

@timer
def slow_function():
    import time
    time.sleep(0.1)
    return "Done"

slow_function()    # slow_function took 0.1012 seconds


# Function that accepts either positional or keyword arguments
def configure_value(value: int, *, multiplier: int = 1) -> int:
    return value * multiplier

# Works with keyword argument (forced due to *)
result = configure_value(10, multiplier=5)
print("Result: " + str(result))    # Result: 50


# Using *args to capture excess positional arguments
def format_strings(separator: str, *words: str) -> str:
    return separator.join(words)

result = format_strings("-", "apple", "banana", "cherry")
print("Formatted: " + result)    # apple-banana-cherry


# Using **kwargs to forward arguments to another function
def proxy_function(**kwargs: Any) -> dict:
    return {
        "passed_kwargs": kwargs,
        "count": len(kwargs)
    }

result = proxy_function(a=1, b=2, c=3)
print("Proxy result: " + str(result))
# Proxy result: {'passed_kwargs': {'a': 1, 'b': 2, 'c': 3}, 'count': 3}


# Building a flexible function that handles multiple call patterns
def flexible_func(required: str, *args: Any, **kwargs: Any) -> dict:
    return {
        "required": required,
        "positional_count": len(args),
        "keyword_keys": list(kwargs.keys())
    }

# Various ways to call this function
r1 = flexible_func("a")                                    # Only required
r2 = flexible_func("a", 1, 2, 3)                          # With *args
r3 = flexible_func("a", x=1, y=2)                         # With **kwargs
r4 = flexible_func("a", 1, 2, x=1, y=2)                  # Both

print(f"r1: {r1['positional_count']}, r2: {r2['positional_count']}, r3: {r3['keyword_keys']}")
# r1: 0, r2: 3, r3: ['x', 'y']


# Real-life Example 1: Event handler with dynamic handlers
def register_event(event_name: str, *handlers: Callable, **config: Any) -> dict:
    return {
        "event": event_name,
        "handlers": len(handlers),
        "is_async": config.get("async", False),
        "priority": config.get("priority", "normal")
    }

def handler1(): pass
def handler2(): pass
def handler3(): pass

registration = register_event(
    "click",
    handler1, handler2, handler3,
    async_flag=True,
    priority="high"
)
print(f"Event '{registration['event']}' has {registration['handlers']} handlers, async={registration['is_async']}")


# Real-life Example 2: REST API router
def route_request(path: str, *path_params: str, method: str = "GET", **query_params: Any) -> dict:
    return {
        "path": path,
        "path_params": list(path_params),
        "method": method,
        "query_params": query_params,
        "authenticated": query_params.get("auth_token") is not None
    }

request = route_request(
    "/users",
    "123", "profile",
    method="GET",
    format="json",
    auth_token="abc123"
)
print(f"Route: {request['method']} {request['path']}/{'/'.join(request['path_params'])}")
# Route: GET /users/123/profile


# Real-life Example 3: Build SQL query dynamically
def build_select(table: str, *columns: str, **conditions: Any) -> str:
    cols = ", ".join(columns) if columns else "*"
    query = f"SELECT {cols} FROM {table}"
    
    if conditions:
        where_clauses = [f"{k} = '{v}'" for k, v in conditions.items()]
        query += " WHERE " + " AND ".join(where_clauses)
    
    return query

query = build_select("users", "id", "name", "email", status="active", role="admin")
print("Query: " + query)
# Query: SELECT id, name, email FROM users WHERE status = 'active' AND role = 'admin'


# Real-life Example 4: HTTP client with flexible options
def make_http_request(url: str, *headers: str, **options: Any) -> dict:
    return {
        "url": url,
        "headers": dict(h.split(": ") for h in headers) if headers else {},
        "method": options.get("method", "GET"),
        "timeout": options.get("timeout", 30),
        "verify_ssl": options.get("verify_ssl", True)
    }

request = make_http_request(
    "https://api.example.com/data",
    "Content-Type: application/json",
    "Accept: application/json",
    method="POST",
    timeout=60
)
print(f"HTTP {request['method']} to {request['url']}, timeout={request['timeout']}")
# HTTP POST to https://api.example.com/data, timeout=60


# Real-life Example 5: Generate test data factory
def create_test_entity(entity_type: str, *required_fields: str, **optional_fields: Any) -> dict:
    entity = {"type": entity_type}
    
    for i, field in enumerate(required_fields):
        entity[f"field_{i+1}"] = field
    
    entity.update(optional_fields)
    return entity

user = create_test_entity("user", "john@example.com", "John Doe", is_active=True, created_by="admin")
print(f"Created {user['type']} with {len(user)} fields")
# Created user with 5 fields


# Real-life Example 6: Plugin system with hooks
def register_plugin(name: str, *capabilities: str, **plugin_config: Any) -> dict:
    return {
        "name": name,
        "capabilities": list(capabilities),
        "enabled": plugin_config.get("enabled", True),
        "version": plugin_config.get("version", "1.0.0"),
        "dependencies": plugin_config.get("dependencies", [])
    }

plugin = register_plugin(
    "auth_plugin",
    "login", "logout", "register", "password_reset",
    enabled=True,
    version="2.1.0",
    dependencies=["database_plugin", "logger_plugin"]
)
print(f"Plugin '{plugin['name']}' v{plugin['version']} has {len(plugin['capabilities'])} capabilities")
# Plugin 'auth_plugin' v2.1.0 has 4 capabilities


# Real-life Example 7: Batch job processor
def process_batch(job_name: str, *item_ids: int, **job_config: Any) -> dict:
    return {
        "job": job_name,
        "items": list(item_ids),
        "batch_size": job_config.get("batch_size", 100),
        "retry_failed": job_config.get("retry_failed", True),
        "notify_on_complete": job_config.get("notify_on_complete", False)
    }

batch = process_batch(
    "send_emails",
    1001, 1002, 1003, 1004, 1005,
    batch_size=50,
    retry_failed=True,
    notify_on_complete=True
)
print(f"Job '{batch['job']}' processing {len(batch['items'])} items in batches of {batch['batch_size']}")
# Job 'send_emails' processing 5 items in batches of 50


# Real-life Example 8: Validation pipeline
def validate_input(input_data: Any, *validators: Callable, **validation_options: Any) -> dict:
    results = []
    for validator in validators:
        try:
            valid = validator(input_data)
            results.append({"validator": validator.__name__, "passed": valid})
        except Exception as e:
            results.append({"validator": validator.__name__, "error": str(e)})
    
    return {
        "data": input_data,
        "validation_results": results,
        "strict_mode": validation_options.get("strict_mode", False)
    }

def is_string(value: Any) -> bool:
    return isinstance(value, str)

def has_length(value: Any) -> bool:
    return len(value) > 0

result = validate_input("test", is_string, has_length, strict_mode=True)
print(f"Validation: {len(result['validation_results'])} validators ran")
# Validation: 2 validators ran
