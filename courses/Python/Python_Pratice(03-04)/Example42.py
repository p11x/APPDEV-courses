# Example42.py
# Topic: Global and Nonlocal Keywords

# This file demonstrates how to use 'global' and 'nonlocal' keywords
# to modify variables in outer scopes.


# Basic global keyword usage
counter = 0

def increment_counter():
    """Increment the global counter variable."""
    global counter
    counter += 1

print("=== Basic Global Keyword ===")  # === Basic Global Keyword ===
print(f"Initial counter: {counter}")  # Initial counter: 0
increment_counter()
print(f"After increment: {counter}")  # After increment: 1
increment_counter()
print(f"After second increment: {counter}")  # After second increment: 2


# Multiple global variables
app_name = "MyApp"
version = "1.0"

def update_app_info(new_name=None, new_version=None):
    """Update global app information."""
    global app_name, version
    if new_name:
        app_name = new_name
    if new_version:
        version = new_version

print("\n=== Multiple Global Variables ===")  # (blank line)
print(f"App: {app_name} v{version}")  # App: MyApp v1.0
update_app_info(new_version="2.0")
print(f"After update: {app_name} v{version}")  # After update: MyApp v2.0
update_app_info(new_name="NewApp", new_version="3.0")
print(f"After second update: {app_name} v{version}")  # After second update: NewApp v3.0


# Nonlocal keyword usage in nested functions
def outer_function():
    """Outer function with enclosing variable."""
    enclosing_var = "initial"
    
    def inner_function():
        """Inner function modifying enclosing variable."""
        nonlocal enclosing_var
        enclosing_var = "modified by inner"
        print(f"Inner sees: {enclosing_var}")  # Inner sees: modified by inner
    
    print(f"Before inner: {enclosing_var}")  # Before inner: initial
    inner_function()
    print(f"After inner: {enclosing_var}")  # After inner: modified by inner

print("\n=== Nonlocal Keyword ===")  # (blank line)
outer_function()  # Before inner: initial
# Inner sees: modified by inner
# After inner: modified by inner


# Real-life Example 1: Database connection pool manager
class ConnectionPool:
    """Manages database connections with global state."""
    
    # Class-level global variables
    connections = []
    max_connections = 10
    active_connections = 0
    
    @classmethod
    def acquire_connection(cls):
        """Acquire a connection from the pool."""
        global active_connections
        if cls.active_connections < cls.max_connections:
            cls.active_connections += 1
            conn_id = cls.active_connections
            return conn_id
        return None
    
    @classmethod
    def release_connection(cls, conn_id):
        """Release a connection back to the pool."""
        global active_connections
        if cls.active_connections > 0:
            cls.active_connections -= 1
    
    @classmethod
    def get_status(cls):
        """Get current pool status."""
        return {
            "active": cls.active_connections,
            "available": cls.max_connections - cls.active_connections,
            "max": cls.max_connections
        }

print("\n=== Real-life: Connection Pool ===")  # (blank line)
pool_status = ConnectionPool.get_status()
print(f"Initial: {pool_status}")  # Initial: {'active': 0, 'available': 10, 'max': 10}

conn1 = ConnectionPool.acquire_connection()
print(f"Acquired connection #{conn1}")  # Acquired connection #1
conn2 = ConnectionPool.acquire_connection()
print(f"Acquired connection #{conn2}")  # Acquired connection #2

pool_status = ConnectionPool.get_status()
print(f"After acquiring: {pool_status}")  # After acquiring: {'active': 2, 'available': 8, 'max': 10}

ConnectionPool.release_connection(conn1)
pool_status = ConnectionPool.get_status()
print(f"After releasing: {pool_status}")  # After releasing: {'active': 1, 'available': 9, 'max': 10}


# Real-life Example 2: Event logger with global state
class EventLogger:
    """Simple event logging system with global log level."""
    
    # Global log level
    LOG_LEVEL = "INFO"
    
    @classmethod
    def set_level(cls, level: str):
        """Set the global log level."""
        global LOG_LEVEL
        LOG_LEVEL = level
    
    @classmethod
    def log(cls, message: str):
        """Log a message based on current level."""
        global LOG_LEVEL
        if LOG_LEVEL == "DEBUG":
            print(f"[DEBUG] {message}")
        elif LOG_LEVEL == "INFO":
            print(f"[INFO] {message}")
        elif LOG_LEVEL == "WARNING":
            print(f"[WARNING] {message}")
        elif LOG_LEVEL == "ERROR":
            print(f"[ERROR] {message}")
    
    @classmethod
    def get_level(cls):
        """Get current log level."""
        global LOG_LEVEL
        return LOG_LEVEL

print("\n=== Real-life: Event Logger ===")  # (blank line)
EventLogger.log("Application started")  # [INFO] Application started
EventLogger.set_level("DEBUG")
EventLogger.log("Debug information")  # [DEBUG] Debug information
EventLogger.set_level("ERROR")
EventLogger.log("This is an error")  # [ERROR] This is an error


# Real-life Example 3: Nested function for order processing
def create_order_processor():
    """Create an order processor with enclosed state."""
    # Enclosing scope variables
    orders_processed = 0
    total_revenue = 0.0
    
    def process_order(order_id: str, amount: float):
        """Process a single order."""
        nonlocal orders_processed, total_revenue
        orders_processed += 1
        total_revenue += amount
        return {
            "order_id": order_id,
            "amount": amount,
            "processed": True
        }
    
    def get_stats():
        """Get processing statistics."""
        return {
            "orders": orders_processed,
            "revenue": total_revenue,
            "average": total_revenue / orders_processed if orders_processed > 0 else 0
        }
    
    return process_order, get_stats

process_order, get_stats = create_order_processor()

print("\n=== Real-life: Order Processor ===")  # (blank line)
result1 = process_order("ORD-001", 99.99)
print(f"Processed: {result1['order_id']}, Amount: ${result1['amount']}")  # Processed: ORD-001, Amount: $99.99

result2 = process_order("ORD-002", 149.99)
print(f"Processed: {result2['order_id']}, Amount: ${result2['amount']}")  # Processed: ORD-002, Amount: $149.99

stats = get_stats()
print(f"Total orders: {stats['orders']}, Revenue: ${stats['revenue']:.2f}")  # Total orders: 2, Revenue: $249.98


# Real-life Example 4: Function closure with state
def create_counter(initial_value: int = 0):
    """Create a counter with enclosed state."""
    count = [initial_value]  # Using list to allow modification
    
    def increment(by: int = 1):
        """Increment the counter."""
        nonlocal count
        count[0] += by
        return count[0]
    
    def decrement(by: int = 1):
        """Decrement the counter."""
        nonlocal count
        count[0] -= by
        return count[0]
    
    def reset():
        """Reset counter to initial value."""
        nonlocal count
        count[0] = initial_value
        return count[0]
    
    def get_value():
        """Get current count."""
        return count[0]
    
    return increment, decrement, reset, get_value

inc, dec, reset, get = create_counter(10)

print("\n=== Real-life: Counter Factory ===")  # (blank line)
print(f"Initial value: {get()}")  # Initial value: 10
print(f"After increment(5): {inc(5)}")  # After increment(5): 15
print(f"After decrement(2): {dec(2)}")  # After decrement(2): 13
print(f"After reset: {reset()}")  # After reset: 10


# Real-life Example 5: Configuration manager with nested functions
def create_config_manager(defaults: dict):
    """Create a configuration manager with nested scope."""
    config = defaults.copy()
    
    def get(key: str, default=None):
        """Get configuration value."""
        return config.get(key, default)
    
    def set(key: str, value):
        """Set configuration value."""
        config[key] = value
        return True
    
    def reset():
        """Reset to default configuration."""
        nonlocal config
        config = defaults.copy()
        return True
    
    def get_all():
        """Get all configuration."""
        return config.copy()
    
    return get, set, reset, get_all

get_cfg, set_cfg, reset_cfg, get_all_cfg = create_config_manager({
    "debug": False,
    "max_retries": 3,
    "timeout": 30
})

print("\n=== Real-life: Config Manager ===")  # (blank line)
print(f"Debug: {get_cfg('debug')}")  # Debug: False
set_cfg("debug", True)
print(f"Debug (updated): {get_cfg('debug')}")  # Debug (updated): True
reset_cfg()
print(f"Debug (reset): {get_cfg('debug')}")  # Debug (reset): False


# Real-life Example 6: Rate limiter with enclosing scope
def create_rate_limiter(max_calls: int, time_window: int):
    """Create a rate limiter with enclosed state."""
    from time import time
    
    calls = []
    
    def allow_request() -> bool:
        """Check if request is allowed."""
        nonlocal calls
        current_time = time()
        
        # Remove old calls outside time window
        calls = [t for t in calls if current_time - t < time_window]
        
        if len(calls) < max_calls:
            calls.append(current_time)
            return True
        return False
    
    def get_remaining() -> int:
        """Get remaining calls allowed."""
        current_time = time()
        calls = [t for t in calls if current_time - t < time_window]
        return max_calls - len(calls)
    
    def reset():
        """Reset the rate limiter."""
        nonlocal calls
        calls = []
        return True
    
    return allow_request, get_remaining, reset

allow_req, get_rem, reset_limiter = create_rate_limiter(3, 60)

print("\n=== Real-life: Rate Limiter ===")  # (blank line)
for i in range(5):
    result = allow_req()
    remaining = get_rem()
    print(f"Request {i+1}: {'Allowed' if result else 'Denied'}, Remaining: {remaining}")  # Request 1-3: Allowed, Remaining: 2-0; Request 4-5: Denied, Remaining: 0
