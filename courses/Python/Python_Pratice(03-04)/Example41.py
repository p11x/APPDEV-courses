# Example41.py
# Topic: The LEGB Rule - Understanding Variable Scope

# This file demonstrates Python's LEGB rule: Local, Enclosing, Global, Built-in
# Each scope level is explored with practical examples.


# Built-in scope: Python's built-in functions and exceptions
# These are always available without import
result = len([1, 2, 3, 4, 5])  # len() is from built-in scope
print(f"Built-in scope - len(): {result}")  # 5

result = max(10, 20, 30)  # max() is from built-in scope
print(f"Built-in scope - max(): {result}")  # 30

result = sum([1, 2, 3, 4, 5])  # sum() is from built-in scope
print(f"Built-in scope - sum(): {result}")  # 15


# Global scope: Variables defined at module level
global_var = "I am global"

def access_global():
    """Access global variable from function."""
    print(f"Enclosing: Accessing {global_var}")  # Enclosing: Accessing I am global

access_global()


# Local scope: Variables defined inside a function
def local_scope_demo():
    """Demonstrate local scope."""
    local_var = "I am local"
    print(f"Local scope: {local_var}")  # Local scope: I am local

local_scope_demo()


# Enclosing scope: Variables in outer function (for nested functions)
def outer_function():
    """Outer function with enclosing variable."""
    enclosing_var = "I am enclosing"
    
    def inner_function():
        """Inner function accessing enclosing scope."""
        print(f"Inner function sees: {enclosing_var}")  # Inner function sees: I am enclosing
    
    inner_function()

outer_function()


# Complete LEGB demonstration
demo_var = "GLOBAL"

def level_1():
    """First level function."""
    demo_var = "ENCLOSING"
    
    def level_2():
        """Second level function."""
        demo_var = "LOCAL"
        print(f"Level 2 (LOCAL): {demo_var}")  # Level 2 (LOCAL): LOCAL
    
    def level_2b():
        """Second level function - accessing enclosing."""
        nonlocal demo_var
        demo_var = "MODIFIED ENCLOSING"
        print(f"Level 2b (after nonlocal): {demo_var}")  # Level 2b (after nonlocal): MODIFIED ENCLOSING
    
    level_2()
    print(f"Level 1 (after level_2): {demo_var}")  # Level 1 (after level_2): ENCLOSING
    level_2b()
    print(f"Level 1 (after level_2b): {demo_var}")  # Level 1 (after level_2b): MODIFIED ENCLOSING

print("\n=== LEGB Demonstration ===")  # (blank line)
level_1()
print(f"Global (unchanged): {demo_var}")  # Global (unchanged): GLOBAL


# Real-life Example 1: Configuration management with global defaults
# Global scope - application configuration
APP_CONFIG = {
    "debug": False,
    "max_connections": 100,
    "timeout": 30
}

def get_config(key: str, default=None):
    """Get configuration value from global scope."""
    return APP_CONFIG.get(key, default)

def update_config(key: str, value):
    """Update configuration in global scope."""
    global APP_CONFIG
    APP_CONFIG[key] = value

print("\n=== Real-life: Config Management ===")  # (blank line)
print(f"Initial debug: {get_config('debug')}")  # Initial debug: False
update_config("debug", True)
print(f"Updated debug: {get_config('debug')}")  # Updated debug: True


# Real-life Example 2: Request counter using global variable
request_count = 0

def handle_request():
    """Increment and return request count."""
    global request_count
    request_count += 1
    return request_count

print("\n=== Real-life: Request Counter ===")  # (blank line)
for i in range(3):
    count = handle_request()
    print(f"Request #{count} processed")  # Request #1 processed, #2 processed, #3 processed


# Real-life Example 3: Nested function for data processing pipeline
def create_processor():
    """Create a data processor with enclosing state."""
    data = []
    
    def add_item(item):
        """Add item to processor's data."""
        data.append(item)
        return len(data)
    
    def get_all():
        """Return all items."""
        return data.copy()
    
    def clear():
        """Clear all items."""
        data.clear()
    
    return add_item, get_all, clear

add, get_items, clear_items = create_processor()

print("\n=== Real-life: Data Processor ===")  # (blank line)
add("first")
add("second")
add("third")
print(f"Items: {get_items()}")  # Items: ['first', 'second', 'third']
clear_items()
print(f"After clear: {get_items()}")  # After clear: []


# Real-life Example 4: Function factory with enclosing scope
def create_greeting_function(greeting: str):
    """Create a customized greeting function."""
    count = [0]  # Using list to allow modification in nested function
    
    def greet(name: str):
        """Greet with customized greeting."""
        count[0] += 1
        return f"{greeting}, {name}! (call #{count[0]})"
    
    def get_call_count():
        """Return how many times the greeting was used."""
        return count[0]
    
    return greet, get_call_count

hello_fn, hello_count = create_greeting_function("Hello")
hi_fn, hi_count = create_greeting_function("Hi")

print("\n=== Real-life: Function Factory ===")  # (blank line)
print(hello_fn("Alice"))  # Hello, Alice! (call #1)
print(hello_fn("Bob"))  # Hello, Bob! (call #2)
print(hi_fn("Charlie"))  # Hi, Charlie! (call #1)
print(f"Hello used {hello_count()} times")  # Hello used 2 times


# Real-life Example 5: Caching with enclosing scope
def create_cached_calculator():
    """Create calculator with simple caching."""
    cache = {}
    
    def calculate(key: str, compute_fn):
        """Calculate or return cached result."""
        if key in cache:
            return cache[key]
        result = compute_fn()
        cache[key] = result
        return result
    
    def clear_cache():
        """Clear the cache."""
        cache.clear()
    
    return calculate, clear_cache

calc, clear_calc = create_cached_calculator()

def expensive_computation():
    """Simulate expensive computation."""
    import time
    time.sleep(0.1)
    return 42 * 2

print("\n=== Real-life: Cached Calculator ===")  # (blank line)
result1 = calc("doubled_42", expensive_computation)
print(f"First call: {result1}")  # First call: 84
result2 = calc("doubled_42", expensive_computation)
print(f"Second call (cached): {result2}")  # Second call (cached): 84
clear_calc()
result3 = calc("doubled_42", expensive_computation)
print(f"After clear: {result3}")  # After clear: 84


# Real-life Example 6: State machine with enclosing scope
def create_state_machine(initial_state: str):
    """Create a simple state machine."""
    current_state = [initial_state]
    transitions = {}
    
    def add_transition(from_state: str, to_state: str, action: str):
        """Add a state transition."""
        if from_state not in transitions:
            transitions[from_state] = {}
        transitions[from_state][action] = to_state
    
    def transition(action: str):
        """Perform a state transition."""
        if current_state[0] in transitions:
            if action in transitions[current_state[0]]:
                current_state[0] = transitions[current_state[0]][action]
                return current_state[0]
        return None
    
    def get_state():
        """Get current state."""
        return current_state[0]
    
    return add_transition, transition, get_state

add_trans, trans, get_state = create_state_machine("idle")

print("\n=== Real-life: State Machine ===")  # (blank line)
add_trans("idle", "loading", "start")
add_trans("loading", "ready", "complete")
add_trans("ready", "processing", "process")
add_trans("processing", "done", "finish")

print(f"Initial: {get_state()}")  # Initial: idle
trans("start")
print(f"After start: {get_state()}")  # After start: loading
trans("complete")
print(f"After complete: {get_state()}")  # After complete: ready
trans("process")
print(f"After process: {get_state()}")  # After process: processing
