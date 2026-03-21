# Example19.py
# Topic: Optional Types

# This file demonstrates Optional types - when something can be a specific type or None.
# Python 3.10+ can use str | None, older code uses Optional[str] from typing.


# Using Python 3.10+ union syntax for Optional
# str | None means the parameter can be a string or None
def greet(name: str | None) -> str:
    if name is None:
        return "Hello, Guest!"
    return "Hello, " + name + "!"

result1 = greet("Alice")    # str — "Hello, Alice!"
print(result1)    # Hello, Alice!

result2 = greet(None)    # str — "Hello, Guest!"
print(result2)    # Hello, Guest!


# Using typing.Optional (older syntax, still works in all versions)
# This is equivalent to str | None
from typing import Optional

def greet_old(name: Optional[str]) -> str:
    if name is None:
        return "Hello, Guest!"
    return "Hello, " + name + "!"

result = greet_old("Bob")    # str — "Hello, Bob!"
print(result)    # Hello, Bob!


# Optional with integer - can return int or None
def find_max(numbers: list) -> Optional[int]:
    if not numbers:
        return None    # return None for empty list
    max_val = numbers[0]    # int — start with first element
    for num in numbers:
        if num > max_val:
            max_val = num
    return max_val

result1 = find_max([1, 5, 3, 9, 2])    # int — 9
print("Max: " + str(result1))    # Max: 9

result2 = find_max([])    # None — empty list
print("Max: " + str(result2))    # Max: None


# Optional with dict - can return dict or None
def get_config(key: str) -> Optional[dict]:
    configs = {
        "db": {"host": "localhost", "port": 5432},
        "cache": {"host": "localhost", "port": 6379}
    }
    if key in configs:
        return configs[key]
    return None

result1 = get_config("db")    # dict — database config
print("Config: " + str(result1))    # Config: {'host': 'localhost', 'port': 5432}

result2 = get_config("unknown")    # None — key not found
print("Config: " + str(result2))    # Config: None


# Optional parameter with default None
def process_data(data: Optional[list] = None) -> int:
    if data is None:
        return 0
    return len(data)

result1 = process_data([1, 2, 3])    # int — length 3
print("Length: " + str(result1))    # Length: 3

result2 = process_data()    # int — 0 (using default)
print("Length: " + str(result2))    # Length: 0


# Optional string that might be empty
def sanitize(name: Optional[str]) -> str:
    if name is None or name == "":
        return "Anonymous"
    return name.strip()

result1 = sanitize("  Alice  ")    # str — "Alice" (trimmed)
print("Name: " + result1)    # Name: Alice

result2 = sanitize("")    # str — "Anonymous" (empty)
print("Name: " + result2)    # Name: Anonymous

result3 = sanitize(None)    # str — "Anonymous" (None)
print("Name: " + result3)    # Name: Anonymous


# Optional int parameter for optional multiplier
def calculate(value: int, multiplier: Optional[int] = None) -> int:
    if multiplier is None:
        multiplier = 1
    return value * multiplier

result1 = calculate(10)    # int — 10 * 1 = 10
print("Result: " + str(result1))    # Result: 10

result2 = calculate(10, 5)    # int — 10 * 5 = 50
print("Result: " + str(result2))    # Result: 50


# Function that can return Optional string
def find_name(user_id: int) -> Optional[str]:
    users = {1: "Alice", 2: "Bob", 3: "Charlie"}
    return users.get(user_id)    # returns None if not found

result1 = find_name(1)    # str — "Alice"
print("User: " + str(result1))    # User: Alice

result2 = find_name(99)    # None — not found
print("User: " + str(result2))    # User: None


# Optional bool for optional flag
def configure(debug: Optional[bool] = None) -> str:
    if debug is None:
        return "Default configuration"
    elif debug:
        return "Debug mode enabled"
    return "Debug mode disabled"

result1 = configure()    # str — default config
print(result1)    # Default configuration

result2 = configure(True)    # str — debug enabled
print(result2)    # Debug mode enabled

result3 = configure(False)    # str — debug disabled
print(result3)    # Debug mode disabled


# Optional list parameter
def merge_lists(list1: Optional[list], list2: Optional[list]) -> list:
    if list1 is None:
        list1 = []
    if list2 is None:
        list2 = []
    return list1 + list2

result1 = merge_lists([1, 2], [3, 4])    # list — [1, 2, 3, 4]
print("Merged: " + str(result1))    # Merged: [1, 2, 3, 4]

result2 = merge_lists([1, 2], None)    # list — [1, 2]
print("Merged: " + str(result2))    # Merged: [1, 2]

result3 = merge_lists(None, None)    # list — []
print("Merged: " + str(result3))    # Merged: []


# Real-world example: optional user profile fields
def create_profile(name: str, email: Optional[str] = None, age: Optional[int] = None) -> dict:
    profile = {}    # dict — user profile dictionary
    profile["name"] = name
    if email is not None:
        profile["email"] = email
    if age is not None:
        profile["age"] = age
    return profile

profile1 = create_profile("Alice", "alice@example.com", 25)    # dict — full profile
print("Profile: " + str(profile1))    # Profile: {'name': 'Alice', 'email': 'alice@example.com', 'age': 25}

profile2 = create_profile("Bob")    # dict — minimal profile
print("Profile: " + str(profile2))    # Profile: {'name': 'Bob'}

profile3 = create_profile("Charlie", age=30)    # dict — name and age only
print("Profile: " + str(profile3))    # Profile: {'name': 'Charlie', 'age': 30}
