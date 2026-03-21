# Example16.py
# Topic: Type Hints - Optional and Union Types

print("=" * 50)
print("TYPE HINTS - OPTIONAL AND UNION TYPES")    # TYPE HINTS - OPTIONAL AND UNION TYPES
print("=" * 50)
# Optional Type (Python 3.10+)
print("\n--- Optional Type (Python 3.10+) ---\n") # \n--- Optional Type (Python 3.10+) ---\n
    
# Syntax: Type | None
name: str | None = None
age: int | None = None
email: str | None = "alice@example.com"
    
print("name: str | None = " + str(name))
print("age: int | None = " + str(age))
print("email: str | None = " + str(email))
    
# Assign values
name = "Alice"
age = 25
print("\nAfter assignment:")                      # \nAfter assignment:
print("name = " + str(name))
print("age = " + str(age))
print("email = " + str(email))
# Optional With Older Syntax
print("\n--- Optional with typing.Optional ---\n")# \n--- Optional with typing.Optional ---\n
    
# Older syntax (works in all Python 3 versions)
from typing import Optional
    
name_old: Optional[str] = None
age_old: Optional[int] = None
    
print("Optional[str] = " + str(name_old))
print("Optional[int] = " + str(age_old))
# Union Type
print("\n--- Union Type ---\n")                   # \n--- Union Type ---\n
    
# Syntax: Type1 | Type2 | Type3
number: int | float = 42
print("number: int | float = " + str(number) + " (int)")# number: int | float = " + str(number) + " (int)
    
number = 3.14
print("number: int | float = " + str(number) + " (float)")# number: int | float = " + str(number) + " (float)
    
# Multiple types in one variable
result: int | float | str = 100
print("result: int | float | str = " + str(result) + " (int)")# result: int | float | str = " + str(result) + " (int)
    
result = 99.9
print("result: int | float | str = " + str(result) + " (float)")# result: int | float | str = " + str(result) + " (float)
    
result = "completed"
print("result: int | float | str = " + str(result) + " (str)")# result: int | float | str = " + str(result) + " (str)
# Practical Examples
print("\n--- Practical Examples ---\n")           # \n--- Practical Examples ---\n
    
# Function with optional parameter
def greet(name: str | None = None) -> str:
    if name is None:
        return "Hello, Guest!"
    return "Hello, " + str(name) + "!"
    
print("greet(): " + str(greet()))
print("greet('Alice'): " + str(greet('Alice')))
    
# Function with Union return type
def process_value(value: str | int) -> str | int:
    if isinstance(value, str):
        return value.upper()
    return value * 2
    
print("process_value('hello'): " + str(process_value('hello')))
print("process_value(5): " + str(process_value(5)))
    
# Dictionary with optional values
def get_user(user_id: int) -> dict[str, str | int | None]:
    # Simulated database
    users: dict[int, dict[str, str | int | None]] = {
        1: {"name": "Alice", "age": 25, "email": "alice@example.com"},
        2: {"name": "Bob", "age": 30, "email": None}
    }
    return users.get(user_id, {})
    
user1: dict[str, str | int | None] = get_user(1)
user2: dict[str, str | int | None] = get_user(2)
user3: dict[str, str | int | None] = get_user(999)
    
print("\nget_user(1): " + str(user1))
print("get_user(2): " + str(user2))
print("get_user(999): " + str(user3))
# Checking Optional Values
print("\n--- Checking Optional Values ---\n")     # \n--- Checking Optional Values ---\n
    
value: str | None = None
    
# Using 'is None' check
if value is None:
    print("Value is None")                            # Value is None
else:
    print("Value is: " + str(value))
    
# Assign and check again
value = "Hello"
if value is None:
    print("Value is None")                            # Value is None
else:
    print("Value is: " + str(value))
    
# Using walrus operator (Python 3.8+)
def process_input(data: str | None) -> str:
    if (cleaned := data) is not None:
        return cleaned.strip().upper()
    return "No data provided"
    
print("\nprocess_input('  hello  '): " + str(process_input('  hello  ')))
print("process_input(None): " + str(process_input(None)))
# Complex Union Types
print("\n--- Complex Union Types ---\n")          # \n--- Complex Union Types ---\n
    
# Union with list and dict
data_container: list[dict[str, int]] | dict[str, list[int]] = [{}]
print("Initial: " + str(data_container))
    
data_container = {"values": [1, 2, 3]}
print("After: " + str(data_container))
    
# Multiple union types
status_code: int | str = 200
status_code = "OK"
print("status_code: " + str(status_code))
# Summary
print("\n" + "=" * 50)
print("OPTIONAL AND UNION TYPES SUMMARY")         # OPTIONAL AND UNION TYPES SUMMARY
print("=" * 50)
print("Key Points:")                              # Key Points:
print("- Optional[T] means T or None")            # - Optional[T] means T or None
print("- Use Python 3.10+ syntax: T | None")      # - Use Python 3.10+ syntax: T | None
print("- Union means multiple types allowed")     # - Union means multiple types allowed
print("- Syntax: Type1 | Type2 | Type3")          # - Syntax: Type1 | Type2 | Type3
print("- Use is None to check optional values")   # - Use is None to check optional values
print("- Old syntax: Optional[T] = T | None")     # - Old syntax: Optional[T] = T | None

# Real-world example: