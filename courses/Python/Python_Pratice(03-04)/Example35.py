# Example35.py
# Topic: Comprehensive *args and **kwargs Examples

# This file provides additional real-world examples demonstrating the practical
# use of *args and **kwargs in various programming scenarios.


from typing import Any


# Real-life Example 1: Dynamic class factory
def create_class(class_name: str, *attributes: str, **methods: Callable) -> type:
    def __init__(self, **kwargs):
        for attr in attributes:
            setattr(self, attr, kwargs.get(attr, None))
    
    def __repr__(self):
        attrs = ", ".join(f"{a}={getattr(self, a, None)!r}" for a in attributes)
        return f"{class_name}({attrs})"
    
    class_dict = {"__init__": __init__, "__repr__": __repr__}
    class_dict.update(methods)
    
    return type(class_name, (), class_dict)

Person = create_class("Person", "name", "age", "city", greet=lambda self: f"Hello, I'm {self.name}")
p = Person(name="Alice", age=30, city="NYC")
print(f"Created: {p}")    # Created: Person(name='Alice', age=30, city='NYC')
print(p.greet())          # Hello, I'm Alice


# Real-life Example 2: Command pattern with dynamic arguments
def execute_command(command_name: str, *args: Any, **kwargs: Any) -> dict:
    commands = {
        "add": lambda a, b: a + b,
        "multiply": lambda a, b: a * b,
        "greet": lambda name: f"Hello, {name}!",
        "config": lambda **opts: opts
    }
    
    if command_name not in commands:
        return {"error": f"Unknown command: {command_name}"}
    
    try:
        if args and kwargs:
            result = commands[command_name](*args, **kwargs)
        elif kwargs:
            result = commands[command_name](**kwargs)
        elif args:
            result = commands[command_name](*args)
        else:
            result = commands[command_name]()
        return {"command": command_name, "result": result}
    except Exception as e:
        return {"command": command_name, "error": str(e)}

cmd1 = execute_command("add", 5, 3)
cmd2 = execute_command("greet", "World")
cmd3 = execute_command("config", host="localhost", port=8080)
print(f"Add: {cmd1['result']}, Greet: {cmd2['result']}")
# Add: 8, Greet: Hello, World!


# Real-life Example 3: Middleware chain
def create_middleware_chain(*middlewares: Callable) -> Callable:
    def chain(request: dict, **kwargs: Any) -> dict:
        result = request
        for middleware in middlewares:
            result = middleware(result, **kwargs)
        return result
    return chain

def add_timestamp_middleware(request: dict, **kwargs: Any) -> dict:
    import datetime
    request["timestamp"] = datetime.datetime.now().isoformat()
    return request

def add_user_middleware(request: dict, **kwargs: Any) -> dict:
    request["user"] = kwargs.get("user", "anonymous")
    return request

def add_headers_middleware(request: dict, **kwargs: Any) -> dict:
    request["headers"] = kwargs.get("headers", {})
    return request

chain = create_middleware_chain(add_timestamp_middleware, add_user_middleware, add_headers_middleware)
result = chain({"url": "/api/data"}, user="alice123", headers={"Content-Type": "application/json"})
print(f"Request: {result['url']}, User: {result['user']}, Timestamp: {result['timestamp'][:19]}")
# Request: /api/data, User: alice123, Timestamp: 2024-01-15T10:30:00


# Real-life Example 4: Dynamic SQL builder
def build_query(table: str, *columns: str, **conditions: Any) -> str:
    cols = ", ".join(columns) if columns else "*"
    query_parts = [f"SELECT {cols} FROM {table}"]
    
    joins = conditions.get("joins", [])
    for join_table, join_condition in joins:
        query_parts.append(f"INNER JOIN {join_table} ON {join_condition}")
    
    where_clauses = []
    for k, v in conditions.items():
        if k == "joins":
            continue
        if isinstance(v, (list, tuple)):
            where_clauses.append(f"{k} IN ({', '.join(repr(str(x)) for x in v)})")
        elif v is None:
            where_clauses.append(f"{k} IS NULL")
        else:
            where_clauses.append(f"{k} = {repr(str(v))}")
    
    if where_clauses:
        query_parts.append("WHERE " + " AND ".join(where_clauses))
    
    if conditions.get("order_by"):
        query_parts.append(f"ORDER BY {conditions['order_by']}")
    
    if conditions.get("limit"):
        query_parts.append(f"LIMIT {conditions['limit']}")
    
    return " ".join(query_parts)

query = build_query(
    "users u",
    "u.id", "u.name", "u.email",
    joins=[("orders o", "u.id = o.user_id")],
    status="active",
    order_by="u.name",
    limit=10
)
print("Query: " + query)
# Query: SELECT u.id, u.name, u.email FROM users u INNER JOIN orders o ON u.id = o.user_id WHERE status = 'active' ORDER BY u.name LIMIT 10


# Real-life Example 5: Flexible function for data transformation pipeline
def transform_data(data: Any, *transforms: Callable, **config: Any) -> Any:
    result = data
    
    for transform in transforms:
        if config.get("verbose"):
            print(f"Applying {transform.__name__}...")
        result = transform(result)
    
    return result

def uppercase(s): return str(s).upper()
def trim(s): return str(s).strip()
def add_prefix(s): return f"PREFIX: {s}"
def add_suffix(s): return f"{s} :SUFFIX"

result = transform_data("  hello  ", uppercase, trim, add_prefix, verbose=True)
print(f"Result: '{result}'")
# Applying uppercase...
# Applying trim...
# Applying add_prefix...
# Result: 'PREFIX: HELLO'


# Real-life Example 6: Event system with listeners
class EventEmitter:
    def __init__(self):
        self.listeners = {}
    
    def on(self, event: str, *callbacks: Callable) -> None:
        if event not in self.listeners:
            self.listeners[event] = []
        self.listeners[event].extend(callbacks)
    
    def emit(self, event: str, *args: Any, **kwargs: Any) -> list:
        results = []
        if event in self.listeners:
            for callback in self.listeners[event]:
                results.append(callback(*args, **kwargs))
        return results

emitter = EventEmitter()

def on_click_handler(*args, **kwargs):
    return f"Click at ({kwargs.get('x')}, {kwargs.get('y')})"

def on_hover_handler(*args, **kwargs):
    return f"Hovering over {kwargs.get('element')}"

emitter.on("click", on_click_handler)
emitter.on("hover", on_hover_handler)
emitter.on("click", lambda *a, **k: "Second click handler")

results = emitter.emit("click", x=100, y=200)
for r in results:
    print(f"Event result: {r}")
# Event result: Click at (100, 200)
# Event result: Second click handler


# Real-life Example 7: Generic data serializer
def serialize(data: Any, *fields: str, **options: Any) -> dict:
    include_all = options.get("include_all", False)
    exclude = options.get("exclude", [])
    
    if isinstance(data, dict):
        if fields:
            result = {k: data.get(k) for k in fields if k not in exclude}
        elif include_all:
            result = data.copy()
        else:
            result = {k: v for k, v in data.items() if k not in exclude}
    
    elif hasattr(data, "__dict__"):
        if fields:
            result = {k: getattr(data, k, None) for k in fields if k not in exclude}
        elif include_all:
            result = data.__dict__.copy()
        else:
            result = {k: v for k, v in data.__dict__.items() if k not in exclude}
    else:
        result = {"value": data}
    
    return result

class User:
    def __init__(self):
        self.id = 1
        self.name = "Alice"
        self.email = "alice@example.com"
        self.password = "secret"

user = User()
serialized = serialize(user, "name", "email", exclude=["id"])
print(f"Serialized: {serialized}")    # Serialized: {'name': 'Alice', 'email': 'alice@example.com'}


# Real-life Example 8: Dynamic function builder for API clients
def create_api_method(http_method: str, endpoint: str) -> Callable:
    def api_method(*path_params: str, **query_params: Any) -> dict:
        url_parts = [endpoint] + list(path_params)
        url = "/".join(url_parts)
        
        return {
            "method": http_method,
            "url": url,
            "params": query_params
        }
    
    return api_method

get_user = create_api_method("GET", "/users")
create_post = create_api_method("POST", "/posts")
delete_comment = create_api_method("DELETE", "/posts/{post_id}/comments")

result1 = get_user()
result2 = get_user("123")
result3 = delete_comment("456", force=True)
print(f"API 1: {result1['method']} {result1['url']}")
print(f"API 2: {result2['url']}")
print(f"API 3: {result3['method']} {result3['url']}")
# API 1: GET /users
# API 2: /users/123
# API 3: DELETE /posts/456/comments
