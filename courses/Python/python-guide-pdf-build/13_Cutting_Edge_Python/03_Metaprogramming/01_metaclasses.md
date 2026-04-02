# 🎭 Python Metaclasses: Classes That Create Classes

## 🎯 What You'll Learn

- What a metaclass is and why it exists
- The type() function with 3 arguments for dynamic class creation
- Creating custom metaclasses with __new__ and __init__
- Using __init_subclass__ as a modern alternative
- Building a practical plugin registration system

## 📦 Prerequisites

- Understanding of Python classes and inheritance
- Familiarity with class attributes and methods

---

## What is a Metaclass?

### The Hierarchy

In Python, everything is an object. Classes are objects too! And classes are created by metaclasses:

```
┌─────────────────────────────────────────────────────────────┐
│                    THE PYTHON OBJECT HIERARCHY              │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│   📦 OBJECT (the base)                                     │
│        │                                                    │
│        │ is instance of                                     │
│        ▼                                                    │
│   🎫 CLASS (like str, int, Dog)                            │
│        │                                                    │
│        │ is instance of                                     │
│        ▼                                                    │
│   🎫 METACLASS (like type)                                  │
│        │                                                    │
│        │ is instance of                                     │
│        ▼                                                    │
│   🎫 type (the metaclass of all classes by default)       │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 💡 Explanation

- Every object in Python is an instance of some class
- Every class is an instance of a metaclass
- By default, classes are instances of `type`
- You can create custom metaclasses to control how classes are created

---

## Using type() with 3 Arguments

The `type()` function can do more than just check types — it can create classes dynamically:

### Basic Dynamic Class Creation

```python
# type(name, bases, dict) creates a class dynamically
# type("ClassName", (ParentClass,), {"attribute": value})

# Create a simple class dynamically
Animal = type("Animal", (), {"legs": 4, "sound": "Some sound"})

# Create an instance
dog = Animal()
print(dog.legs)  # 4
print(dog.sound)  # Some sound

# Add methods dynamically
def speak(self):
    return self.sound

Animal = type("Animal", (), {"legs": 4, "sound": "Some sound", "speak": speak})

cat = Animal()
cat.sound = "Meow!"
print(cat.speak())  # Meow!
```

### 💡 Line-by-Line Breakdown

```python
# type() with 3 args: type(name, bases, namespace_dict)

Animal = type("Animal", (), {"legs": 4, "sound": "Some sound"})
# "Animal" = class name
# () = parent classes (empty tuple = no inheritance)
# {"legs": 4, "sound": "Some sound"} = class attributes

dog = Animal()  # Create instance
print(dog.legs)  # Access class attribute: 4

# Add a method
def speak(self):  # Method needs 'self'
    return self.sound

# Recreate with method
Animal = type("Animal", (), {"legs": 4, "sound": "Some sound", "speak": speak})

cat = Animal()  # New instance
cat.sound = "Meow!"  # Set instance attribute
print(cat.speak())  # Call the method: Meow!
```

---

## Custom Metaclasses

### Building a Plugin Registration System

```python
class PluginMeta(type):
    """Metaclass that auto-registers plugins."""
    
    # This dictionary stores all registered plugins
    _plugins: dict[str, type] = {}
    
    def __new__(mcs, name: str, bases: tuple, namespace: dict) -> type:
        """Create a new class and register it if it's a plugin."""
        cls = super().__new__(mcs, name, bases, namespace)
        
        # Register the plugin (unless it's the base class)
        if name != "Plugin":
            mcs._plugins[name.lower()] = cls
            print(f"📦 Registered plugin: {name}")
        
        return cls
    
    def get_plugin(cls, name: str) -> type | None:
        """Get a plugin by name."""
        return cls._plugins.get(name)

# Define plugins using the metaclass
class Plugin(metaclass=PluginMeta):
    """Base plugin class."""
    def execute(self) -> str:
        raise NotImplementedError

class MathPlugin(Plugin):
    """Plugin that does math operations."""
    def execute(self) -> str:
        return "Math result: 42"

class TextPlugin(Plugin):
    """Plugin that processes text."""
    def execute(self) -> str:
        return "Text processed!"

class ImagePlugin(Plugin):
    """Plugin that processes images."""
    def execute(self) -> str:
        return "Image processed!"

# Use the plugin system
print(f"\n📋 Available plugins: {list(PluginMeta._plugins.keys())}")

math = Plugin.get_plugin("mathplugin")()
print(math.execute())  # Math result: 42
```

### 💡 Line-by-Line Breakdown

```python
class PluginMeta(type):                    # Inherit from type = this is a metaclass
    _plugins: dict[str, type] = {}          # Class variable - shared across all instances
    
    def __new__(mcs, name, bases, namespace):  # Called when class is defined
        cls = super().__new__(mcs, name, bases, namespace)  # Create the class
        
        # Register plugins (not the base class)
        if name != "Plugin":
            mcs._plugins[name.lower()] = cls  # Register by lowercase name
            print(f"📦 Registered plugin: {name}")
        
        return cls  # Return the created class
    
    def get_plugin(cls, name: str) -> type | None:  # Class method on metaclass
        return cls._plugins.get(name)

# Define plugins - metaclass __new__ runs for each class
class Plugin(metaclass=PluginMeta):  # Use metaclass=
    def execute(self) -> str:
        raise NotImplementedError

class MathPlugin(Plugin):  # Triggers PluginMeta.__new__
    def execute(self) -> str:
        return "Math result: 42"
# Output: 📦 Registered plugin: MathPlugin
```

---

## __init_subclass__: The Modern Alternative

Python 3.6+ provides `__init_subclass__` as a simpler way to achieve registration:

```python
class PluginBase:
    """Base class with automatic subclass registration."""
    
    _subclasses: dict[str, type] = {}
    
    def __init_subclass__(cls, name: str = None, **kwargs):
        """Called when a subclass is defined."""
        super().__init_subclass__(**kwargs)
        
        # Register the subclass
        register_name = name or cls.__name__.lower()
        cls._subclasses[register_name] = cls
        print(f"📦 Registered: {register_name}")
    
    @classmethod
    def get(cls, name: str) -> type | None:
        """Get a subclass by name."""
        return cls._subclasses.get(name)

class Adder(PluginBase, name="adder"):
    """Adds numbers."""
    def process(self, a: int, b: int) -> int:
        return a + b

class Multiplier(PluginBase, name="multiplier"):
    """Multiplies numbers."""
    def process(self, a: int, b: int) -> int:
        return a * b

# Use it
print(f"\n📋 Available: {list(PluginBase._subclasses.keys())}")

adder_cls = PluginBase.get("adder")
adder = adder_cls()
print(adder.process(3, 4))  # 7
```

### 💡 Line-by-Line Breakdown

```python
class PluginBase:                      # Base class
    _subclasses: dict[str, type] = {}  # Registry of subclasses
    
    def __init_subclass__(cls, name: str = None, **kwargs):  # Called automatically!
        super().__init_subclass__(**kwargs)  # Keep parent initialization
        
        register_name = name or cls.__name__.lower()  # Use custom name or auto-gen
        cls._subclasses[register_name] = cls  # Register this class
    
    @classmethod
    def get(cls, name: str) -> type | None:  # Get subclass by name
        return cls._subclasses.get(name)

# Define subclasses - __init_subclass__ runs for each!
class Adder(PluginBase, name="adder"):  # Custom name
    def process(self, a: int, b: int) -> int:
        return a + b
```

---

## When to Use Metaclass vs __init_subclass__

| Feature | Metaclass | __init_subclass__ |
|---------|-----------|-------------------|
| Complexity | High | Low |
| Flexibility | Maximum | Good |
| When class is created | Definition time | Definition time |
| Use case | Complex control | Simple registration |
| Python version | All | 3.6+ |

### 🧪 Try It

```python
# Use __init_subclass__ for simple cases like:
# - Auto-registering plugins
# - Tracking all subclasses
# - Enforcing constraints

# Use metaclasses for complex cases like:
# - Modifying class creation process
# - Changing how attributes work
# - Advanced framework features
```

---

## Real-World Example: Enforcing Abstract Methods

```python
class AutoRegister(type):
    """Metaclass that enforces abstract method implementation."""
    
    def __new__(mcs, name: str, bases: tuple, namespace: dict) -> type:
        cls = super().__new__(mcs, name, bases, namespace)
        
        # Check for unimplemented abstract methods
        if bases:  # Not the base class
            for attr_name, attr_value in namespace.items():
                if callable(attr_value) and attr_name.startswith('_'):
                    # Could add enforcement here
                    pass
        
        return cls

class Base(metaclass=AutoRegister):
    def required_method(self):
        raise NotImplementedError("Subclasses must implement required_method!")

# This will work but will fail at runtime if method not implemented
class GoodImplementation(Base):
    def required_method(self):
        return "Implemented!"

class BadImplementation(Base):
    pass  # Forgot to implement!

# Test
good = GoodImplementation()
print(good.required_method())  # Implemented!

bad = BadImplementation()
print(bad.required_method())  # NotImplementedError: Subclasses must implement...
```

---

## ✅ Summary

- Metaclasses control how classes are created — they're "classes of classes"
- Use `type(name, bases, dict)` for simple dynamic class creation
- Custom metaclasses override `__new__` or `init` to control class creation
- `__init_subclass__` is the modern, simpler alternative for registration
- For most cases, `__init_subclass__` is sufficient — use metaclasses only when needed

## ➡️ Next Steps

Continue to [02_descriptors_and_slots.md](./02_descriptors_and_slots.md) to learn about descriptors, __slots__, and how Python's attribute access works under the hood.

## 🔗 Further Reading

- [PEP 487: __init_subclass__](https://peps.python.org/pep-0487/)
- [Understanding Python Metaclasses](https://realpython.com/python-metaclasses/)
- [What is a metaclass in Python?](https://stackoverflow.com/questions/100003/what-is-a-metaclass-in-python)
