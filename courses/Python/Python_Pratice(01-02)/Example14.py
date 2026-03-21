# Example14.py
# Topic: Type Hints - Variable Annotations

print("=" * 50)
print("TYPE HINTS - VARIABLE ANNOTATIONS")        # TYPE HINTS - VARIABLE ANNOTATIONS
print("=" * 50)
# Basic Variable Annotations
print("\n--- Basic Variable Annotations ---\n")   # \n--- Basic Variable Annotations ---\n
    
# Simple type annotations
count = 0 # int  — whole number, no quotes
name = "Alice" # str  — text, always wrapped in quotes
price = 19.99 # float — decimal number
is_active = True # bool — can only be True or False
    
print("count: int = " + str(count))
print("name: str = " + str(name))
print("price: float = " + str(price))
print("is_active: bool = " + str(is_active))
# Accessing Annotations
print("\n--- Accessing Annotations ---\n")        # \n--- Accessing Annotations ---\n
    
# Annotations are stored in __annotations__
print("count.__annotations__: " + str(count.__annotations__))
    
# Using __annotations__ dict
print("\nGlobal annotations:")                    # \nGlobal annotations:
for var_name, var_value in globals().items():
    if var_name in ['count', 'name', 'price', 'is_active']:
        print("  " + str(var_name) + ": " + str(type(var_value).__name__))
# Annotations Without Initialization
print("\n--- Annotations Without Initialization ---\n")# \n--- Annotations Without Initialization ---\n
    
# You can annotate without assigning (using PEP 526)
# This requires a special import or just assign later
count2: int
name2: str
    
# Now assign values
count2 = 10
name2 = "Bob"
    
print("count2 = " + str(count2))
print("name2 = " + str(name2))
# Class Variable Annotations
print("\n--- Class Variable Annotations ---\n")   # \n--- Class Variable Annotations ---\n
    
class User:
    # Class variables with annotations
    name: str
    age: int
    email: str
        
    def __init__(self, name: str, age: int, email: str) -> None:
        self.name = name
        self.age = age
        self.email = email
    
user = User("Alice", 25, "alice@example.com")
print("User: " + str(user.name) + ", " + str(user.age) + ", " + str(user.email))
# Type Annotations In Functions
print("\n--- Type Annotations in Functions ---\n")# \n--- Type Annotations in Functions ---\n
    
def greet(name: str) -> str:
    return "Hello, " + str(name) + "!"
    
def add(a: int, b: int) -> int:
    return a + b
    
def process_data(data: list[str]) -> dict[str, int]:
    result: dict[str, int] = {}
    for item in data:
        result[item] = len(item)
    return result
    
print("greet('Alice'): " + str(greet('Alice')))
print("add(5, 3): " + str(add(5, 3)))
print("process_data(['hi', 'hello']): " + str(process_data(['hi', 'hello'])))
# Annotated Example
print("\n--- Annotated Example ---\n")            # \n--- Annotated Example ---\n
    
def calculate_total(items: list[float], tax_rate: float = 0.08) -> float:
    """Calculate total price with tax."""

    subtotal = sum(items) # float — decimal number
    tax = subtotal * tax_rate # float — decimal number
    total = subtotal + tax # float — decimal number
    return total
    
prices: list[float] = [10.0, 20.0, 30.0]
total = calculate_total(prices) # float — decimal number
    
print("Items: " + str(prices))
print("Total with tax: $" + str(total))
# Summary
print("\n" + "=" * 50)
print("VARIABLE ANNOTATIONS SUMMARY")             # VARIABLE ANNOTATIONS SUMMARY
print("=" * 50)
print("Key Points:")                              # Key Points:
print("- Syntax: variable_name: Type = value")    # - Syntax: variable_name: Type = value
print("- Annotations stored in __annotations__")  # - Annotations stored in __annotations__
print("- Can annotate without immediate assignment")# - Can annotate without immediate assignment
print("- Works with class variables")             # - Works with class variables
print("- Essential for function parameters and return types")# - Essential for function parameters and return types

# Real-world example: