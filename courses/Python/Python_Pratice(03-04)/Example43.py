# Example43.py
# Topic: Understanding Python Namespaces

# This file demonstrates Python's namespace system - dictionaries that
# map names to objects. Different namespaces exist simultaneously.


# Accessing the global namespace
global_var = "I am global"

def show_globals():
    """Display some entries from the global namespace."""
    print("=== Global Namespace ===")  # === Global Namespace ===
    print(f"'global_var' in globals(): {'global_var' in globals()}")  # True
    print(f"'len' in globals(): {'len' in globals()}")  # True


show_globals()


# Accessing the local namespace
def show_locals():
    """Display the local namespace."""
    local_var = "I am local"
    print("\n=== Local Namespace ===")  # (blank line)
    print(f"Local variables: {locals()}")  # {'local_var': 'I am local', ...}
    print(f"'local_var' in locals(): {'local_var' in locals()}")  # True


show_locals()


# Module namespaces - each module has its own global namespace
import math
import datetime

print("\n=== Module Namespaces ===")  # (blank line)
print(f"math module name: {math.__name__}")  # math
print(f"datetime module name: {datetime.__name__}")  # datetime
print(f"'sqrt' in dir(math): {'sqrt' in dir(math)}")  # True


# Built-in namespace
import builtins

print("\n=== Built-in Namespace ===")  # (blank line)
print(f"Number of built-in names: {len(dir(builtins))}")  # ~152
print(f"'len' is a built-in: {hasattr(builtins, 'len')}")  # True
print(f"'print' is a built-in: {hasattr(builtins, 'print')}")  # True


# Using globals() and locals()
module_level = "module level"

def function_level():
    """Show globals vs locals."""
    function_var = "function level"
    print("\n=== globals() vs locals() ===")  # (blank line)
    print(f"'module_level' in globals(): {'module_level' in globals()}")  # True
    print(f"'module_level' in locals(): {'module_level' in locals()}")  # False
    print(f"'function_var' in locals(): {'function_var' in locals()}")  # True


function_level()


# Namespace collision demonstration
var = "global"

def outer():
    var = "enclosing"
    
    def inner():
        var = "local"
        print(f"\n=== Namespace Resolution ===")  # (blank line)
        print(f"Inner sees: '{var}' (local)")  # Inner sees: 'local' (local)
    
    inner()
    print(f"Outer sees: '{var}' (enclosing)")  # Outer sees: 'enclosing' (enclosing)


outer()
print(f"Module sees: '{var}' (global)")  # Module sees: 'global' (global)


# Real-life Example 1: Inspecting function namespaces
def demonstrate_function_namespaces():
    """Demonstrate function namespace attributes."""
    
    def inner_function():
        pass
    
    print("\n=== Real-life: Function Namespace Attributes ===")  # (blank line)
    print(f"__name__: {demonstrate_function_namespaces.__name__}")  # demonstrate_function_namespaces
    print(f"__qualname__: {demonstrate_function_namespaces.__qualname__}")  # demonstrate_function_namespaces
    print(f"__globals__ contains 'print': {'print' in demonstrate_function_namespaces.__globals__}")  # True
    print(f"__closure__: {demonstrate_function_namespaces.__closure__}")  # None


demonstrate_function_namespaces()


# Real-life Example 2: Class namespace
class MyClass:
    """A sample class to demonstrate class namespace."""
    
    class_attr = "I am a class attribute"
    
    def __init__(self, instance_attr):
        self.instance_attr = instance_attr
    
    def method(self):
        return "I am a method"


print("\n=== Real-life: Class Namespace ===")  # (blank line)
print(f"Class attributes: {MyClass.__dict__.keys()}")  # dict_keys(['__module__', 'class_attr', ...])
print(f"'class_attr' value: {MyClass.class_attr}")  # I am a class attribute

obj = MyClass("instance value")
print(f"Instance attribute: {obj.instance_attr}")  # instance value
print(f"'method' in class: {'method' in MyClass.__dict__}")  # True


# Real-life Example 3: Module configuration system
# In a real application, each module would be in a separate file
# Here we simulate the module namespace pattern

# Simulating module_a
module_a_vars = {"config_value": 100, "enabled": True}

# Simulating module_b  
module_b_vars = {"config_value": 200, "enabled": False}

def access_module_a():
    """Access module_a's namespace."""
    return module_a_vars.get("config_value")

def access_module_b():
    """Access module_b's namespace."""
    return module_b_vars.get("config_value")

print("\n=== Real-life: Module Namespace Isolation ===")  # (blank line)
print(f"Module A config: {access_module_a()}")  # 100
print(f"Module B config: {access_module_b()}")  # 200


# Real-life Example 4: Dynamic code execution with namespaces
def execute_in_context(code: str, context: dict):
    """Execute code with a custom namespace."""
    exec(code, context)

print("\n=== Real-life: Dynamic Execution Context ===")  # (blank line)
context = {"x": 10, "y": 20}
execute_in_context("result = x + y", context)
print(f"Context after execution: {context}")  # {'x': 10, 'y': 20, 'result': 30}
print(f"Result from context: {context.get('result')}")  # 30


# Real-life Example 5: Namespace package pattern
# This pattern is used in Python for organizing packages
class NamespaceBuilder:
    """Builds a namespace from multiple sources."""
    
    def __init__(self):
        self.namespace = {}
    
    def add_builtin(self, name: str, value):
        """Add a built-in to the namespace."""
        self.namespace[name] = value
        return self
    
    def add_function(self, func):
        """Add a function to the namespace."""
        self.namespace[func.__name__] = func
        return self
    
    def add_class(self, cls):
        """Add a class to the namespace."""
        self.namespace[cls.__name__] = cls
        return self
    
    def build(self):
        """Build and return the namespace."""
        return self.namespace


def hello():
    return "Hello"


class Greeter:
    def greet(self, name):
        return f"Hello, {name}!"


print("\n=== Real-life: Namespace Builder ===")  # (blank line)
ns_builder = NamespaceBuilder()
ns = (ns_builder
      .add_builtin("version", "1.0")
      .add_function(hello)
      .add_class(Greeter)
      .build())

print(f"Built-in: {ns.get('version')}")  # Built-in: 1.0
print(f"Function: {ns.get('hello')()}")  # Function: Hello
greeter = ns.get("Greeter")()
print(f"Class method: {greeter.greet('World')}")  # Class method: Hello, World!


# Real-life Example 6: Using namespace for dependency injection
class Container:
    """Simple dependency injection container."""
    
    def __init__(self):
        self.services = {}
    
    def register(self, name: str, factory):
        """Register a service."""
        self.services[name] = factory
        return self
    
    def get(self, name: str):
        """Get a registered service."""
        if name in self.services:
            return self.services[name]()
        raise KeyError(f"Service '{name}' not found")
    
    def has(self, name: str) -> bool:
        """Check if service is registered."""
        return name in self.services


def create_database():
    """Database service factory."""
    return {"type": "postgresql", "connected": True}


def create_cache():
    """Cache service factory."""
    return {"type": "redis", "enabled": True}


def create_logger():
    """Logger service factory."""
    return {"level": "INFO", "handler": "console"}


print("\n=== Real-life: Dependency Injection Container ===")  # (blank line)
container = Container()
container.register("database", create_database)
container.register("cache", create_cache)
container.register("logger", create_logger)

db = container.get("database")
print(f"Database: {db}")  # Database: {'type': 'postgresql', 'connected': True}
print(f"Has database: {container.has('database')}")  # Has database: True
print(f"Has queue: {container.has('queue')}")  # Has queue: False
