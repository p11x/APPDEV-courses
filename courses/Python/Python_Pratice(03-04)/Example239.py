# Example239: functools - wraps, singledispatch
from functools import wraps, singledispatch

# wraps - preserve function metadata when creating decorators
def my_decorator(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print(f"Calling {func.__name__}")
        return func(*args, **kwargs)
    return wrapper

@my_decorator
def example():
    """This is the docstring."""
    return 42

print("wraps - preserve metadata:")
print(f"Function name: {example.__name__}")
print(f"Docstring: {example.__doc__}")
print(f"Result: {example()}")

# singledispatch - function overloading by type
@singledispatch
def process(data):
    print(f"Default: {data}")

@process.register(int)
def process_int(data):
    print(f"Integer: {data * 2}")

@process.register(str)
def process_str(data):
    print(f"String: {data.upper()}")

@process.register(list)
def process_list(data):
    print(f"List: {len(data)} items")

print("\nsingledispatch - type-based overloading:")
process(42)        # int
process("hello")   # str
process([1, 2, 3]) # list

# Register with lambda (using lambda requires a workaround in older Python)
from functools import singledispatchmethod

class Handler:
    @singledispatchmethod
    def handle(self, data):
        print(f"Default: {data}")
    
    @handle.register(int)
    def _(self, data):
        print(f"Integer: {data * 2}")
    
    @handle.register(str)
    def _(self, data):
        print(f"String: {data.upper()}")

print("\nsingledispatchmethod:")
h = Handler()
h.handle(42)
h.handle("hello")

# Practical: different formatting for different types
print("\nPractical - format different types:")
@singledispatch
def format_data(data):
    return str(data)

@format_data.register
def _(data: int):
    return f"Integer: {data:,.0f}"

@format_data.register
def _(data: float):
    return f"Float: {data:.2f}"

@format_data.register
def _(data: list):
    return f"List with {len(data)} items"

print(format_data(1000))
print(format_data(3.14159))
print(format_data([1, 2, 3]))
