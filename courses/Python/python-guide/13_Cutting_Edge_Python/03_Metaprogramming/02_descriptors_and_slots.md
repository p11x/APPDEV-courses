# 🔍 Descriptors and __slots__: Inside Python's Attribute System

## 🎯 What You'll Learn

- How Python's attribute access actually works (__getattribute__ vs __getattr__)
- The descriptor protocol: __get__, __set__, __delete__
- Data descriptors vs non-data descriptors
- Building Typed and Cached descriptors
- __slots__ deep dive: memory layout and performance

## 📦 Prerequisites

- Understanding of Python classes and inheritance
- Familiarity with property decorators

---

## How Attribute Access Works

When you write `obj.attr`, Python follows a specific lookup chain:

### The Lookup Chain

```python
class MyClass:
    class_attr = "I'm a class attribute"  # Defined on the class
    
    def __init__(self):
        self.instance_attr = "I'm an instance attribute"  # In __dict__

obj = MyClass()

# When you access obj.instance_attr, Python checks:
# 1. obj.__dict__["instance_attr"]  ← Found! Returns this
# 2. MyClass.__dict__["instance_attr"]  (for class attrs)
# 3. MyClass parent classes' __dict__
# 4. MyClass.__getattribute__("instance_attr")
```

### __getattr__ vs __getattribute__

```python
class Example:
    """Demonstrating __getattr__ vs __getattribute__."""
    
    def __init__(self):
        self.regular = "I exist!"  # Regular attribute
    
    def __getattribute__(self, name: str):
        """Called for EVERY attribute access."""
        print(f"🔍 Looking up: {name}")
        return super().__getattribute__(name)  # Must call parent's!
    
    def __getattr__(self, name: str):
        """Called ONLY when attribute is NOT found."""
        print(f"🤷 Attribute '{name}' not found, handling gracefully")
        return f"Default for {name}"

e = Example()
print(e.regular)    # 🔍 Looking up: regular → I exist!
print(e.missing)    # 🔍 Looking up: missing → 🤷 Attribute 'missing' not found → Default for missing
```

### 💡 Line-by-Line Breakdown

```python
class Example:
    def __init__(self):
        self.regular = "I exist!"  # This becomes an instance attribute
    
    def __getattribute__(self, name: str):  # Intercepts ALL attribute access
        print(f"🔍 Looking up: {name}")       # Log what's happening
        return super().__getattribute__(name)  # MUST call super to get actual value
    
    def __getattr__(self, name: str):  # Only called when attribute NOT found
        print(f"🤷 Attribute '{name}' not found")
        return f"Default for {name}"

e = Example()
print(e.regular)   # Triggers __getattribute__ → returns "I exist!"
print(e.missing)  # Triggers __getattribute__ → fails → triggers __getattr__ → returns default
```

---

## The Descriptor Protocol

Descriptors are objects that define `__get__`, `__set__`, or `__delete__`. They're the foundation for properties, methods, and classmethods.

### What Makes Something a Descriptor?

```python
class MyDescriptor:
    """A descriptor that intercepts attribute access."""
    
    def __get__(self, obj, objtype=None):
        """Called when the attribute is read."""
        return "Getting value!"
    
    def __set__(self, obj, value):
        """Called when the attribute is set."""
        print(f"Setting value to: {value}")
    
    def __delete__(self, obj):
        """Called when the attribute is deleted."""
        print("Deleting!")

class MyClass:
    """A class that uses the descriptor."""
    my_attr = MyDescriptor()  # Assign descriptor to class attribute

obj = MyClass()
print(obj.my_attr)  # Calls MyDescriptor.__get__ → "Getting value!"
obj.my_attr = 42   # Calls MyDescriptor.__set__ → "Setting value to: 42"
del obj.my_attr    # Calls MyDescriptor.__delete__ → "Deleting!"
```

### 💡 Line-by-Line Breakdown

```python
class MyDescriptor:
    def __get__(self, obj, objtype=None):  # obj = instance, objtype = class
        return "Getting value!"
    
    def __set__(self, obj, value):  # obj = instance, value = new value
        print(f"Setting value to: {value}")
    
    def __delete__(self, obj):
        print("Deleting!")

class MyClass:
    my_attr = MyDescriptor()  # This becomes a descriptor!

obj = MyClass()
print(obj.my_attr)  # obj.my_attr triggers __get__ → "Getting value!"
obj.my_attr = 42   # Sets trigger __set__ → "Setting value to: 42"
del obj.my_attr    # Delete triggers __delete__ → "Deleting!"
```

---

## Data vs Non-Data Descriptors

### The Difference

| Type | __set__ | __delete__ | Priority |
|------|---------|------------|----------|
| Data Descriptor | Yes | Yes | Highest (overrides instance dict) |
| Non-Data Descriptor | No | Yes | Lower (instance dict can override) |

```python
class DataDescriptor:
    """Has __set__ - takes precedence over instance.__dict__."""
    def __get__(self, obj, objtype=None):
        return "From data descriptor"
    
    def __set__(self, obj, value):
        obj._cached_value = value  # Must store somewhere else!

class NonDataDescriptor:
    """No __set__ - instance.__dict__ takes precedence."""
    def __get__(self, obj, objtype=None):
        return "From non-data descriptor"

class MyClass:
    data = DataDescriptor()
    non_data = NonDataDescriptor()

obj = MyClass()

# Non-data descriptors can be shadowed by instance attributes
obj.non_data = "Shadowed!"  # This goes to obj.__dict__
print(obj.non_data)  # "Shadowed!" - instance attribute wins!

# Data descriptors always win
obj.data = "Value"  # Triggers __set__
print(obj.data)  # "From data descriptor" - descriptor wins!
```

---

## Practical Example: Typed Descriptor

```python
class Typed:
    """Descriptor that enforces a specific type."""
    
    def __init__(self, expected_type: type):
        self.expected_type = expected_type
        self.name = None
    
    def __set_name__(self, owner, name):
        """Called when descriptor is assigned to class attribute."""
        self.name = name
    
    def __get__(self, obj, objtype=None):
        """Get the value from instance storage."""
        if obj is None:
            return self  # Accessing on class, not instance
        return obj.__dict__.get(self.name)
    
    def __set__(self, obj, value):
        """Validate and set the value."""
        if not isinstance(value, self.expected_type):
            raise TypeError(
                f"{self.name} must be {self.expected_type.__name__}, "
                f"got {type(value).__name__}"
            )
        obj.__dict__[self.name] = value

class Person:
    name = Typed(str)      # Must be str
    age = Typed(int)       # Must be int
    score = Typed(float)    # Must be float

# Use it
person = Person()
person.name = "Alice"  # Works
person.age = 30       # Works
person.score = 95.5   # Works

# person.age = "thirty"  # TypeError: age must be int, got str
```

### 💡 Line-by-Line Breakdown

```python
class Typed:
    def __init__(self, expected_type: type):  # Store expected type
        self.expected_type = expected_type
        self.name = None
    
    def __set_name__(self, owner, name):  # Auto-set when class is created
        self.name = name
    
    def __get__(self, obj, objtype=None):  # Get value from instance dict
        if obj is None:
            return self  # Class-level access returns descriptor itself
        return obj.__dict__.get(self.name)  # Get from instance's __dict__
    
    def __set__(self, obj, value):  # Validate and store
        if not isinstance(value, self.expected_type):
            raise TypeError(f"{self.name} must be {self.expected_type.__name__}")
        obj.__dict__[self.name] = value  # Store in instance dict

class Person:
    name = Typed(str)  # Creates descriptor for name attribute
    age = Typed(int)
    score = Typed(float)

person = Person()
person.name = "Alice"  # Triggers __set__ with validation
```

---

## Practical Example: Cached Descriptor

```python
class cached_property:
    """Descriptor that computes value once and caches it."""
    
    def __init__(self, func):
        self.func = func
        self.name = None
    
    def __set_name__(self, owner, name):
        self.name = name
    
    def __get__(self, obj, objtype=None):
        if obj is None:
            return self
        
        # Check if already cached
        if self.name not in obj.__dict__:
            # Not cached - compute and store
            obj.__dict__[self.name] = self.func(obj)
        
        return obj.__dict__[self.name]

class DataProcessor:
    def __init__(self, data: list[int]):
        self.data = data
    
    @cached_property
    def mean(self):
        """Compute mean - only done once!"""
        print("Computing mean...")  # Shows it's being computed
        return sum(self.data) / len(self.data)
    
    @cached_property
    def sorted_data(self):
        """Return sorted data - computed once."""
        print("Sorting data...")  # Shows it's being computed
        return sorted(self.data)

# Use it
processor = DataProcessor([5, 2, 8, 1, 9])

print(processor.mean)       # Computing mean... → 5.0
print(processor.mean)       # Just returns cached 5.0 - no recomputation!

print(processor.sorted_data)  # Sorting data... → [1, 2, 5, 8, 9]
print(processor.sorted_data)  # Just returns cached - no recomputation!
```

---

## __slots__ Deep Dive

### How __slots__ Works

```python
class RegularClass:
    """Regular class - has __dict__ for attributes."""
    def __init__(self, x, y):
        self.x = x
        self.y = y

class SlottedClass:
    """Class with __slots__ - no __dict__!"""
    __slots__ = ['x', 'y']
    
    def __init__(self, x, y):
        self.x = x
        self.y = y

# Compare memory usage
import sys

regular = RegularClass(1, 2)
slotted = SlottedClass(1, 2)

print(f"Regular class: {sys.getsizeof(regular.__dict__)} bytes for dict + overhead")
print(f"Slotted class: {sys.getsizeof(slotted)} bytes (no dict!)")

# Slotted classes can't have arbitrary attributes
# slotted.z = 3  # AttributeError!
```

### 💡 Line-by-Line Breakdown

```python
class RegularClass:         # Regular class
    def __init__(self, x, y):
        self.x = x         # Stored in __dict__
        self.y = y

class SlottedClass:        # Memory-efficient class
    __slots__ = ['x', 'y'] # Declare allowed attributes
    
    def __init__(self, x, y):
        self.x = x         # Still works, but stored differently
        self.y = y

regular = RegularClass(1, 2)  # Has __dict__
slotted = SlottedClass(1, 2)  # No __dict__ - more memory efficient!

# slotted.z = 3  # AttributeError - can't add new attributes!
```

### __slots__ with Inheritance

```python
class Base:
    __slots__ = ['x']
    
class Child(Base):
    __slots__ = ['y']  # Adds to parent's slots

obj = Child()
obj.x = 1
obj.y = 2
# obj.z = 3  # Still error - z not in __slots__!
```

---

## ✅ Summary

- Python attribute access follows a lookup chain: instance dict → descriptors → class
- `__getattribute__` handles all lookups; `__getattr__` handles missing ones
- Descriptors define `__get__`, `__set__`, `__delete__` to control attribute access
- Data descriptors (with `__set__`) always take precedence over instance dict
- `__slots__` removes `__dict__` and saves memory for large numbers of objects

## ➡️ Next Steps

Continue to [03_dynamic_code_generation.md](./03_dynamic_code_generation.md) to learn about exec(), eval(), and the ast module for dynamic code generation.

## 🔗 Further Reading

- [Descriptor HowTo Guide](https://docs.python.org/3/howto/descriptor.html)
- [__slots__ Tutorial](https://www.python-course.eu/python3_slots.php)
- [Python Descriptors Explained](https://realpython.com/python-descriptors/)
