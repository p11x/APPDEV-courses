# Example51.py
# Topic: Closures and Scope - Understanding the Basics

# This file demonstrates closures and scope in Python, including
# the LEGB rule and how closures work.


# ============================================================
# Understanding the LEGB Rule
# ============================================================

# Built-in scope - Python's built-in functions
print("=== LEGB Rule Demonstration ===")
print(f"Built-in len: {len([1, 2, 3])}")  # len is from built-in scope

# Global scope - module level variables
global_var = "I am global"

def outer_function():
    # Enclosing scope - variables in outer function
    enclosing_var = "I am enclosing"
    
    def inner_function():
        # Local scope - variables defined inside this function
        local_var = "I am local"
        
        print(f"Local: {local_var}")
        print(f"Enclosing: {enclosing_var}")
        print(f"Global: {global_var}")
        print(f"Built-in: {len([1])}")
    
    inner_function()

outer_function()


# ============================================================
# Basic Closure Example
# ============================================================
print("\n=== Basic Closure ===")

def make_greeting(greeting):
    """Create a greeting function that remembers the greeting."""
    def greet(name):
        return f"{greeting}, {name}!"
    return greet

hello = make_greeting("Hello")
hi = make_greeting("Hi")
goodbye = make_greeting("Goodbye")

print(hello("Alice"))      # Hello, Alice!
print(hi("Bob"))           # Hi, Bob!
print(goodbye("Charlie"))  # Goodbye, Charlie!


# ============================================================
# Closure with State
# ============================================================
print("\n=== Closure with State ===")

def make_counter():
    """Create a counter function with state."""
    count = 0  # This variable is in the enclosing scope
    
    def increment():
        nonlocal count  # Allow modification of enclosing variable
        count += 1
        return count
    
    return increment

counter1 = make_counter()
counter2 = make_counter()

print(f"counter1: {counter1()}")  # 1
print(f"counter1: {counter1()}")  # 2
print(f"counter1: {counter1()}")  # 3
print(f"counter2: {counter2()}")  # 1 (separate counter!)


# ============================================================
# Closure with Multiple Enclosed Variables
# ============================================================
print("\n=== Multiple Enclosed Variables ===")

def make_calculator():
    """Create a calculator with memory."""
    total = 0
    history = []
    
    def add(amount):
        nonlocal total
        total += amount
        history.append(f"+{amount}")
        return total
    
    def subtract(amount):
        nonlocal total
        total -= amount
        history.append(f"-{amount}")
        return total
    
    def get_total():
        return total
    
    def get_history():
        return history.copy()
    
    return add, subtract, get_total, get_history

add, subtract, total, history = make_calculator()

print(f"Add 10: {add(10)}")     # 10
print(f"Add 5: {add(5)}")       # 15
print(f"Subtract 3: {subtract(3)}")  # 12
print(f"Total: {total()}")      # 12
print(f"History: {history()}")   # ['+10', '+5', '-3']


# ============================================================
# Real-life Example 1: Function Factory
# ============================================================
print("\n=== Real-life: Function Factory ===")

def make_multiplier(factor):
    """Create a function that multiplies by a factor."""
    def multiply(x):
        return x * factor
    return multiply

double = make_multiplier(2)
triple = make_multiplier(3)
quadruple = make_multiplier(4)

print(f"Double 5: {double(5)}")    # 10
print(f"Triple 5: {triple(5)}")    # 15
print(f"Quadruple 5: {quadruple(5)}")  # 20


# ============================================================
# Real-life Example 2: Logger Factory
# ============================================================
print("\n=== Real-life: Logger Factory ===")

def make_logger(prefix):
    """Create a logger with a custom prefix."""
    def log(message, level="INFO"):
        print(f"[{prefix}] {level}: {message}")
    return log

error_logger = make_logger("ERROR")
debug_logger = make_logger("DEBUG")
api_logger = make_logger("API")

error_logger("Database connection failed")
debug_logger("Processing request")
api_logger("GET /users")


# ============================================================
# Real-life Example 3: Validation Factory
# ============================================================
print("\n=== Real-life: Validation Factory ===")

def make_validator(min_value, max_value):
    """Create a validator for a range."""
    def validate(value):
        if value < min_value:
            return f"Value {value} is below minimum {min_value}"
        if value > max_value:
            return f"Value {value} is above maximum {max_value}"
        return "Valid"
    return validate

validate_age = make_validator(0, 150)
validate_score = make_validator(0, 100)
validate_year = make_validator(1900, 2100)

print(validate_age(25))     # Valid
print(validate_age(200))    # Value 200 is above maximum 150
print(validate_score(85))   # Valid
print(validate_score(-5))   # Value -5 is below minimum 0


# ============================================================
# Real-life Example 4: Configuration Handler
# ============================================================
print("\n=== Real-life: Configuration Handler ===")

def make_config_handler(defaults):
    """Create a config handler with default values."""
    config = defaults.copy()
    
    def get(key):
        return config.get(key)
    
    def set(key, value):
        config[key] = value
        return config
    
    def reset():
        nonlocal config
        config = defaults.copy()
        return config
    
    def get_all():
        return config.copy()
    
    return get, set, reset, get_all

get, set_config, reset, all_config = make_config_handler({
    "debug": False,
    "max_connections": 100,
    "timeout": 30
})

print(f"Debug: {get('debug')}")
set_config("debug", True)
print(f"Debug (updated): {get('debug')}")
print(f"All config: {all_config()}")


# ============================================================
# Real-life Example 5: Event Handler
# ============================================================
print("\n=== Real-life: Event Handler ===")

def make_event_handler():
    """Create an event handler with tracking."""
    handlers = []
    call_count = 0
    
    def on(event, callback):
        handlers.append({"event": event, "callback": callback})
        return callback
    
    def trigger(event, *args, **kwargs):
        nonlocal call_count
        call_count += 1
        results = []
        for h in handlers:
            if h["event"] == event:
                results.append(h["callback"](*args, **kwargs))
        return results
    
    def get_count():
        return call_count
    
    return on, trigger, get_count

on, trigger, get_count = make_event_handler()

# Register handlers
on("user.created", lambda u: f"Welcome {u['name']}")
on("user.created", lambda u: f"Sending email to {u['email']}")
on("user.login", lambda u: f"Logging in {u['name']}")

# Trigger events
results = trigger("user.created", {"name": "Alice", "email": "alice@example.com"})
print(f"Triggered: {results}")
print(f"Call count: {get_count()}")


# ============================================================
# Real-life Example 6: Timer/Stopwatch
# ============================================================
print("\n=== Real-life: Timer ===")

def make_timer():
    """Create a timer with lap tracking."""
    import time
    start_time = None
    laps = []
    
    def start():
        nonlocal start_time
        start_time = time.time()
        laps.clear()
        return "Timer started"
    
    def lap():
        if start_time is None:
            return "Timer not started"
        current = time.time()
        lap_time = current - start_time
        if laps:
            lap_time = current - laps[-1]
        laps.append(current)
        return f"Lap {len(laps)}: {lap_time:.2f}s"
    
    def stop():
        if start_time is None:
            return "Timer not started"
        elapsed = time.time() - start_time
        return f"Total time: {elapsed:.2f}s"
    
    return start, lap, stop

start_timer, lap_timer, stop_timer = make_timer()
start_timer()
print(lap_timer())  # Lap 1
print(lap_timer())  # Lap 2
print(stop_timer())


# ============================================================
# Multiple Closures in One Function
# ============================================================
print("\n=== Multiple Closures ===")

def make_bank_account(initial_balance):
    """Create a bank account with multiple operations."""
    balance = initial_balance
    transactions = [f"Initial: {initial_balance}"]
    
    def deposit(amount):
        nonlocal balance
        balance += amount
        transactions.append(f"Deposit: +{amount}")
        return balance
    
    def withdraw(amount):
        nonlocal balance
        if amount > balance:
            return "Insufficient funds"
        balance -= amount
        transactions.append(f"Withdraw: -{amount}")
        return balance
    
    def get_balance():
        return balance
    
    def get_transactions():
        return transactions.copy()
    
    return deposit, withdraw, get_balance, get_transactions

deposit, withdraw, balance, transactions = make_bank_account(1000)

print(f"Initial balance: {balance()}")
print(f"After deposit 500: {deposit(500)}")
print(f"After withdraw 200: {withdraw(200)}")
print(f"Current balance: {balance()}")
print(f"Transactions: {transactions()}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("KEY TAKEAWAYS:")
print("=" * 50)
print("""
1. CLOSURE: A function that remembers variables from its
   enclosing scope even after the outer function returns

2. LEGB RULE:
   - Local: Variables in current function
   - Enclosing: Variables in outer function
   - Global: Module-level variables
   - Built-in: Python's built-in functions

3. nonlocal: Used to modify enclosing (non-global) variables

4. USE CASES:
   - Function factories
   - State management
   - Callbacks
   - Configuration handlers
   - Event systems
""")
