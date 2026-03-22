# Example45.py
# Topic: Comprehensive Scope and Namespaces Examples

# This file provides comprehensive real-world examples combining
# LEGB rule, global/nonlocal, namespaces, and best practices.


# Complete Example 1: Authentication system with proper scope
class AuthSystem:
    """Authentication system demonstrating scope management."""
    
    # Global configuration
    MAX_LOGIN_ATTEMPTS = 5
    SESSION_TIMEOUT = 3600
    
    def __init__(self):
        self.users = {
            "admin": {"password": "admin123", "role": "admin"},
            "user": {"password": "user123", "role": "user"}
        }
        self.sessions = {}
        self.failed_attempts = {}
    
    def authenticate(self, username: str, password: str) -> dict:
        """Authenticate a user."""
        # Check for too many failed attempts
        if username in self.failed_attempts:
            if self.failed_attempts[username] >= self.MAX_LOGIN_ATTEMPTS:
                return {"success": False, "error": "Account locked"}
        
        # Check credentials
        if username in self.users:
            if self.users[username]["password"] == password:
                # Reset failed attempts on success
                self.failed_attempts[username] = 0
                session_id = self._create_session(username)
                return {"success": True, "session_id": session_id}
            else:
                # Increment failed attempts
                self.failed_attempts[username] = self.failed_attempts.get(username, 0) + 1
                return {"success": False, "error": "Invalid password"}
        
        return {"success": False, "error": "User not found"}
    
    def _create_session(self, username: str) -> str:
        """Create a session (private method)."""
        import uuid
        session_id = str(uuid.uuid4())
        self.sessions[session_id] = {
            "username": username,
            "role": self.users[username]["role"]
        }
        return session_id


print("=== Real-world: Authentication System ===")  # === Real-world: Authentication System ===
auth = AuthSystem()
result = auth.authenticate("admin", "admin123")
print(f"Login success: {result['success']}")  # Login success: True

result = auth.authenticate("admin", "wrong")
print(f"Login failed: {result['success']}, error: {result.get('error')}")  # Login failed: False, error: Invalid password


# Complete Example 2: Plugin system with namespace isolation
class PluginNamespace:
    """Plugin system demonstrating namespace isolation."""
    
    def __init__(self):
        self._plugins = {}
        self._hooks = {}
    
    def register(self, name: str, plugin: callable):
        """Register a plugin."""
        self._plugins[name] = plugin
        return plugin
    
    def register_hook(self, hook_name: str, callback: callable):
        """Register a hook callback."""
        if hook_name not in self._hooks:
            self._hooks[hook_name] = []
        self._hooks[hook_name].append(callback)
    
    def execute_hook(self, hook_name: str, *args, **kwargs):
        """Execute all callbacks for a hook."""
        results = []
        if hook_name in self._hooks:
            for callback in self._hooks[hook_name]:
                results.append(callback(*args, **kwargs))
        return results
    
    def get_plugin(self, name: str):
        """Get a registered plugin."""
        return self._plugins.get(name)


# Create global plugin namespace
plugins = PluginNamespace()


def on_user_created(user_data):
    """Hook: Handle new user creation."""
    return f"Welcome email sent to {user_data['email']}"


def on_user_created_log(user_data):
    """Hook: Log new user creation."""
    return f"User {user_data['name']} created in database"


plugins.register_hook("user.created", on_user_created)
plugins.register_hook("user.created", on_user_created_log)


print("\n=== Real-world: Plugin System ===")  # (blank line)
results = plugins.execute_hook("user.created", {"name": "Alice", "email": "alice@example.com"})
for result in results:
    print(f"  Hook result: {result}")  # Hook result: Welcome email sent to alice@example.com


# Complete Example 3: Request/Response middleware with enclosing scope
def create_middleware_stack():
    """Create middleware stack with proper scope management."""
    middleware = []
    
    def use(middleware_func):
        """Add middleware to the stack."""
        middleware.append(middleware_func)
        return middleware_func
    
    def process(request: dict):
        """Process request through middleware stack."""
        response = {"status": 200, "body": {}}
        
        for mw in middleware:
            # Middleware can modify request/response
            result = mw(request, response)
            if result:
                response = result
        
        return response
    
    def clear():
        """Clear all middleware."""
        middleware.clear()
    
    return use, process, clear


use_mw, process_req, clear_mw = create_middleware_stack()


@use_mw
def logging_middleware(request: dict, response: dict):
    """Log incoming requests."""
    print(f"  Logging: {request.get('method')} {request.get('path')}")
    return response


@use_mw
def auth_middleware(request: dict, response: dict):
    """Check authentication."""
    if request.get("requires_auth") and not request.get("user"):
        return {"status": 401, "error": "Unauthorized"}
    return response


print("\n=== Real-world: Middleware Stack ===")  # (blank line)
request = {"method": "GET", "path": "/users", "requires_auth": True}
response = process_req(request)
print(f"Response: {response}")  # Response: {'status': 401, 'error': 'Unauthorized'}


# Complete Example 4: Class with module-level configuration
# Simulating a config module

# Module-level config (global scope)
_CONFIG = {
    "environment": "production",
    "debug": False,
    "features": {
        "beta": False,
        "analytics": True
    }
}

def get_config(key: str, default=None):
    """Get configuration value."""
    return _CONFIG.get(key, default)

def set_config(key: str, value):
    """Set configuration value."""
    global _CONFIG
    _CONFIG[key] = value

def get_feature(feature_name: str) -> bool:
    """Check if a feature is enabled."""
    return _CONFIG.get("features", {}).get(feature_name, False)


class Service:
    """Service class using module-level config."""
    
    def __init__(self, name: str):
        self.name = name
        self.environment = get_config("environment")
    
    def is_debug(self) -> bool:
        """Check if debug mode is enabled."""
        return get_config("debug", False)
    
    def is_feature_enabled(self, feature: str) -> bool:
        """Check if feature is enabled."""
        return get_feature(feature)


print("\n=== Real-world: Module-level Config ===")  # (blank line)
print(f"Environment: {get_config('environment')}")  # Environment: production
service = Service("MyService")
print(f"Service environment: {service.environment}")  # Service environment: production
print(f"Debug enabled: {service.is_debug()}")  # Debug enabled: False
print(f"Beta feature: {service.is_feature_enabled('beta')}")  # Beta feature: False


# Complete Example 5: Generator with enclosing scope for state
def create_pagination(items: list, page_size: int):
    """Create a paginator with enclosing scope state."""
    current_page = 0
    total_items = len(items)
    total_pages = (total_items + page_size - 1) // page_size
    
    def next_page():
        """Get next page of results."""
        nonlocal current_page
        if current_page >= total_pages:
            return None
        
        start = current_page * page_size
        end = start + page_size
        page_items = items[start:end]
        current_page += 1
        
        return {
            "page": current_page,
            "total_pages": total_pages,
            "items": page_items
        }
    
    def has_next():
        """Check if there are more pages."""
        return current_page < total_pages
    
    def reset():
        """Reset to first page."""
        nonlocal current_page
        current_page = 0
    
    return next_page, has_next, reset


print("\n=== Real-world: Pagination ===")  # (blank line)
items = list(range(1, 11))  # 1-10
next_page, has_next, reset = create_pagination(items, 3)

while has_next():
    page_data = next_page()
    print(f"Page {page_data['page']}: {page_data['items']}")  # Page 1: [1, 2, 3], Page 2: [4, 5, 6], etc.


# Complete Example 6: Context manager with proper scope handling
from typing import Any

class TransactionScope:
    """Transaction context manager with proper scope."""
    
    def __init__(self, name: str):
        self.name = name
        self._active = False
        self._operations = []
    
    def __enter__(self):
        """Enter the transaction scope."""
        self._active = True
        print(f"[{self.name}] Transaction started")
        return self
    
    def __exit__(self, exc_type: Any, exc_val: Any, exc_tb: Any):
        """Exit the transaction scope."""
        if exc_type is None:
            print(f"[{self.name}] Transaction committed")
        else:
            print(f"[{self.name}] Transaction rolled back: {exc_val}")
        
        self._active = False
        return False  # Don't suppress exceptions
    
    def add_operation(self, op_name: str):
        """Add an operation to the transaction."""
        if self._active:
            self._operations.append(op_name)
            print(f"  + {op_name}")
    
    def get_operations(self):
        """Get all operations in this transaction."""
        return self._operations.copy()


print("\n=== Real-world: Transaction Scope ===")  # (blank line)
with TransactionScope("Order") as txn:
    txn.add_operation("Validate items")  # + Validate items
    txn.add_operation("Check inventory")  # + Check inventory
    txn.add_operation("Calculate total")  # + Calculate total
    txn.add_operation("Process payment")  # + Process payment
    print(f"Total operations: {len(txn.get_operations())}")  # Total operations: 4
# [Order] Transaction started
# [Order] Transaction committed


# Complete Example 7: Decorator factory with configuration
def create_logger(level: str = "INFO"):
    """Create a logger with configurable level."""
    # Enclosing scope variable
    log_messages = []
    
    def logger(func):
        """Logger decorator."""
        def wrapper(*args, **kwargs):
            message = f"[{level}] Calling {func.__name__}"
            log_messages.append(message)
            print(f"  {message}")
            return func(*args, **kwargs)
        
        wrapper.get_logs = lambda: log_messages.copy()
        return wrapper
    
    return logger


@create_logger("DEBUG")
def process_data(data):
    """Process some data."""
    return f"Processed: {data}"


@create_logger("ERROR")
def process_error(data):
    """Process with error."""
    raise ValueError(data)


print("\n=== Real-world: Logger Decorator ===")  # (blank line)
result = process_data("test")  # [DEBUG] Calling process_data
print(f"Logs: {process_data.get_logs()}")  # Logs: ['[DEBUG] Calling process_data']

try:
    process_error("error")  # [ERROR] Calling process_error
except ValueError as e:
    print(f"Caught: {e}")  # Caught: error


# Complete Example 8: Service locator pattern with global registry
class ServiceLocator:
    """Service locator with global registry."""
    
    _services = {}
    
    @classmethod
    def register(cls, name: str, factory: callable):
        """Register a service."""
        cls._services[name] = factory
    
    @classmethod
    def get(cls, name: str):
        """Get a service."""
        if name in cls._services:
            return cls._services[name]()
        raise KeyError(f"Service '{name}' not registered")
    
    @classmethod
    def has(cls, name: str) -> bool:
        """Check if service exists."""
        return name in cls._services


def create_database_service():
    """Database service factory."""
    return {"type": "postgresql", "connected": True}


def create_cache_service():
    """Cache service factory."""
    return {"type": "redis", "connected": True}


ServiceLocator.register("database", create_database_service)
ServiceLocator.register("cache", create_cache_service)


print("\n=== Real-world: Service Locator ===")  # (blank line)
db = ServiceLocator.get("database")
print(f"Database: {db}")  # Database: {'type': 'postgresql', 'connected': True}
print(f"Has database: {ServiceLocator.has('database')}")  # Has database: True
print(f"Has queue: {ServiceLocator.has('queue')}")  # Has queue: False
