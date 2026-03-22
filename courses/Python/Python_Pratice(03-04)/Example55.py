# Example55.py
# Topic: Closures and Scope - Comprehensive Review

# This file provides a comprehensive review of closures and scope
# with additional practical examples and patterns.


# ============================================================
# Review: LEGB Rule
# ============================================================
print("=" * 60)
print("COMPREHENSIVE REVIEW: LEGB RULE")
print("=" * 60)

# Built-in scope
print("\n1. BUILT-IN SCOPE:")
print(f"   len('hello') = {len('hello')}")
print(f"   max([1,2,3]) = {max([1,2,3])}")

# Global scope
global_var = "I am global"

def function_with_enclosing():
    enclosing_var = "I am enclosing"
    
    def inner_function():
        local_var = "I am local"
        # Access all scopes
        print(f"   Local: {local_var}")
        print(f"   Enclosing: {enclosing_var}")
        print(f"   Global: {global_var}")
        print(f"   Built-in: {len('test')}")
    
    inner_function()

function_with_enclosing()


# ============================================================
# Review: nonlocal vs global
# ============================================================
print("\n2. NONLOCAL VS GLOBAL:")

# Global example
counter_global = 0

def increment_global():
    global counter_global
    counter_global += 1

increment_global()
print(f"   Global counter: {counter_global}")

# Nonlocal example
def make_nonlocal_counter():
    count = 0
    
    def increment():
        nonlocal count
        count += 1
        return count
    
    return increment

counter = make_nonlocal_counter()
counter()
counter()
print(f"   Nonlocal counter: {counter()}")


# ============================================================
# Review: Closure Factory Pattern
# ============================================================
print("\n3. CLOSURE FACTORY PATTERN:")

# Function factory
def make_power_function(exponent):
    def power(base):
        return base ** exponent
    return power

square = make_power_function(2)
cube = make_power_function(3)
fourth = make_power_function(4)

for func, name in [(square, "square"), (cube, "cube"), (fourth, "fourth")]:
    print(f"   {name}(5) = {func(5)}")


# ============================================================
# Real-world Example 1: Class-like Behavior with Closures
# ============================================================
print("\n4. REAL-WORLD: CLASS-LIKE WITH CLOSURES:")

def create_person(name, age):
    """Create a person-like object using closures."""
    # Private variables (in enclosing scope)
    _name = name
    _age = age
    _history = []
    
    # Methods (closure functions)
    def get_name():
        return _name
    
    def get_age():
        return _age
    
    def set_age(new_age):
        nonlocal _age
        old_age = _age
        _age = new_age
        _history.append(f"Age changed from {old_age} to {new_age}")
        return _age
    
    def get_history():
        return _history.copy()
    
    # Return methods as a dictionary (simulating methods)
    return {
        "get_name": get_name,
        "get_age": get_age,
        "set_age": set_age,
        "get_history": get_history
    }

person = create_person("Alice", 30)
print(f"   Name: {person['get_name']()}")
print(f"   Age: {person['get_age']()}")
person['set_age'](31)
print(f"   New Age: {person['get_age']()}")
print(f"   History: {person['get_history']()}")


# ============================================================
# Real-world Example 2: Module Pattern (Revealing Module)
# ============================================================
print("\n5. REAL-WORLD: REVEALING MODULE PATTERN:"")

def create_module():
    """Simulate a module with private state."""
    # Private state
    _config = {}
    _initialized = False
    
    # Private functions
    def _validate_config(config):
        return isinstance(config, dict)
    
    def _load_defaults():
        return {"debug": False, "timeout": 30, "retries": 3}
    
    # Public API
    def init(config=None):
        nonlocal _initialized
        if _initialized:
            return "Already initialized"
        
        if config and _validate_config(config):
            _config.update(config)
        else:
            _config.update(_load_defaults())
        
        _initialized = True
        return f"Initialized with config: {_config}"
    
    def get_config(key=None):
        if key:
            return _config.get(key)
        return _config.copy()
    
    def update_config(key, value):
        _config[key] = value
        return _config
    
    def reset():
        nonlocal _initialized
        _config.clear()
        _initialized = False
    
    # Return public API
    return {
        "init": init,
        "get_config": get_config,
        "update_config": update_config,
        "reset": reset
    }

module = create_module()
print(f"   {module['init']({'debug': True})}")
print(f"   Config: {module['get_config']()}")
module['update_config']('port', 8080)
print(f"   After update: {module['get_config']('port')}")


# ============================================================
# Real-world Example 3: Counterfeit-Proof Counter
# ============================================================
print("\n6. REAL-WORLD: SECURE COUNTER:")

def make_secure_counter(start=0):
    """Counter that can't be manipulated from outside."""
    count = [start]  # Mutable container
    
    def get_count():
        return count[0]
    
    def increment():
        count[0] += 1
        return count[0]
    
    def decrement():
        count[0] -= 1
        return count[0]
    
    def reset():
        count[0] = start
    
    return {
        "get": get_count,
        "inc": increment,
        "dec": decrement,
        "reset": reset
    }

secure = make_secure_counter(100)
print(f"   Initial: {secure['get']()}")
secure['inc']()
secure['inc']()
secure['dec']()
print(f"   After ops: {secure['get']()}")


# ============================================================
# Real-world Example 4: Function Registry
# ============================================================
print("\n7. REAL-WORLD: FUNCTION REGISTRY:")

def make_registry():
    """Create a function registry."""
    registry = {}
    
    def register(name, func):
        registry[name] = func
        return func
    
    def call(name, *args, **kwargs):
        if name in registry:
            return registry[name](*args, **kwargs)
        raise KeyError(f"Function '{name}' not found")
    
    def list_functions():
        return list(registry.keys())
    
    def unregister(name):
        if name in registry:
            del registry[name]
    
    return {
        "register": register,
        "call": call,
        "list": list_functions,
        "unregister": unregister
    }

registry = make_registry()

@registry["register"]("double")
def double(x):
    return x * 2

@registry["register"]("triple")
def triple(x):
    return x * 3

print(f"   Functions: {registry['list']()}")
print(f"   Double(5): {registry['call']('double', 5)}")
print(f"   Triple(5): {registry['call']('triple', 5)}")


# ============================================================
# Real-world Example 5: Lazy Evaluation
# ============================================================
print("\n8. REAL-WORLD: LAZY EVALUATION:")

def make_lazy(value_func):
    """Create a lazily evaluated value."""
    computed = [False]
    value = [None]
    
    def get():
        if not computed[0]:
            value[0] = value_func()
            computed[0] = True
        return value[0]
    
    def reset():
        computed[0] = False
        value[0] = None
    
    return get, reset

# Expensive computation
call_count = [0]

def expensive_computation():
    call_count[0] += 1
    return sum(range(10000))

lazy_value, reset_lazy = make_lazy(expensive_computation)

print(f"   First call (computes): {lazy_value()}")
print(f"   Second call (cached): {lazy_value()}")
print(f"   Computation was called {call_count[0]} time(s)")

reset_lazy()
call_count[0] = 0
print(f"   After reset, call again: {lazy_value()}")
print(f"   Computation called {call_count[0]} time(s)")


# ============================================================
# Real-world Example 6: Weighted Random Selection
# ============================================================
print("\n9. REAL-WORLD: WEIGHTED RANDOM:"")

def make_weighted_selector(items, weights):
    """Create a weighted random selector."""
    import random
    
    def select():
        return random.choices(items, weights=weights, k=1)[0]
    
    def select_many(n):
        return random.choices(items, weights=weights, k=n)
    
    def get_weights():
        return dict(zip(items, weights))
    
    return {
        "select": select,
        "select_many": select_many,
        "get_weights": get_weights
    }

selector = make_weighted_selector(
    items=["common", "rare", "legendary"],
    weights=[60, 30, 10]
)

print(f"   Weights: {selector['get_weights']()}")
print(f"   Single selection: {selector['select']()}")
print(f"   10 selections: {selector['select_many'](10)}")


# ============================================================
# Final Summary
# ============================================================
print("\n" + "=" * 60)
print("FINAL SUMMARY: CLOSURES AND SCOPE")
print("=" * 60)
print("""
CLOSURES AND SCOPE ARE FUNDAMENTAL TO PYTHON:

1. LEGB RULE: Local → Enclosing → Global → Built-in
   - Python resolves variable names in this order

2. CLOSURES: Functions that remember their enclosing scope
   - Created when a function is defined inside another
   - Can capture variables from the outer function

3. NONLOCAL: Modifies variables in enclosing (not global) scope
   - Use when you need to modify enclosing function variables

4. COMMON PATTERNS:
   - Function factories
   - Data encapsulation
   - Caching/memoization
   - Event handlers
   - Middleware
   - State management

5. PITFALLS TO AVOID:
   - Closures in loops (use default arguments)
   - Mutable default arguments (use None)
   - Forgetting nonlocal when modifying enclosing vars

Mastering closures and scope will make you a more
effective Python programmer!
""")
