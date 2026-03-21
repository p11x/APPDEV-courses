# Example17.py
# Topic: Return Type Hints

# This file demonstrates how to use return type hints with the -> syntax.
# The return type hint specifies what type of value the function will return.


# Function that returns an integer
# The -> int indicates the function returns an integer value
def add(a: int, b: int) -> int:
    return a + b

result = add(5, 3)    # int — result of adding 5 and 3
print("5 + 3 = " + str(result))    # 5 + 3 = 8

result2 = add(100, 200)    # int — result of adding 100 and 200
print("100 + 200 = " + str(result2))    # 100 + 200 = 300


# Function that returns None
# The -> None indicates the function doesn't return any value
def print_hello() -> None:
    print("Hello, World!")

result = print_hello()    # None — function only prints, returns nothing
print("Return value: " + str(result))    # Return value: None


# Function that returns a string
def format_name(first: str, last: str) -> str:
    return last + ", " + first

formatted = format_name("John", "Doe")    # str — formatted name "Doe, John"
print("Formatted: " + formatted)    # Formatted: Doe, John


# Function that returns a boolean
def is_even(number: int) -> bool:
    return number % 2 == 0

check = is_even(10)    # bool — True because 10 is even
print("Is 10 even? " + str(check))    # Is 10 even? True

check2 = is_even(7)    # bool — False because 7 is odd
print("Is 7 even? " + str(check2))    # Is 7 even? False


# Function that returns a float
def divide(a: float, b: float) -> float:
    return a / b

result = divide(10, 4)    # float — 10 divided by 4
print("10 / 4 = " + str(result))    # 10 / 4 = 2.5


# Function that returns a list
def get_numbers() -> list:
    return [1, 2, 3, 4, 5]

nums = get_numbers()    # list — [1, 2, 3, 4, 5]
print("Numbers: " + str(nums))    # Numbers: [1, 2, 3, 4, 5]


# Function that returns a dictionary
def get_config() -> dict:
    return {"host": "localhost", "port": 8080, "debug": True}

config = get_config()    # dict — configuration dictionary
print("Config: " + str(config))    # Config: {'host': 'localhost', 'port': 8080, 'debug': True}


# Function that returns a tuple
def get_coordinates() -> tuple:
    return (10, 20)

coords = get_coordinates()    # tuple — (10, 20)
print("Coordinates: " + str(coords))    # Coordinates: (10, 20)


# Function with conditional return types
# This function can return either a string or None
def find_user(user_id: int) -> str:
    users = {1: "Alice", 2: "Bob", 3: "Charlie"}
    if user_id in users:
        return users[user_id]
    return "User not found"

user = find_user(1)    # str — "Alice"
print("User 1: " + user)    # User 1: Alice

user2 = find_user(99)    # str — "User not found"
print("User 99: " + user2)    # User 99: User not found


# Function returning a custom object (simulated with dict)
def create_user(name: str, age: int) -> dict:
    return {
        "name": name,
        "age": age,
        "status": "active"
    }

new_user = create_user("Alice", 25)    # dict — user data dictionary
print("New user: " + str(new_user))    # New user: {'name': 'Alice', 'age': 25, 'status': 'active'}


# Function returning multiple values (as tuple)
def get_stats(numbers: list) -> tuple:
    total = sum(numbers)    # int — sum of numbers
    average = total / len(numbers)    # float — average of numbers
    return (total, average)

stats = get_stats([10, 20, 30])    # tuple — (60, 20.0)
print("Stats: total=" + str(stats[0]) + ", average=" + str(stats[1]))    # Stats: total=60, average=20.0


# Function that can return different types based on input
def process(value: str) -> str:
    if value.isdigit():
        return "Number: " + value
    elif value.isalpha():
        return "Word: " + value.upper()
    return "Mixed: " + value

result1 = process("123")    # str — "Number: 123"
print(result1)    # Number: 123

result2 = process("hello")    # str — "Word: HELLO"
print(result2)    # Word: HELLO


# Function with early return (returns None implicitly)
def print_and_return(name: str) -> None:
    if not name:
        return    # exits early, returns None
    print("Hello, " + name + "!")

result = print_and_return("Alice")    # None — prints but returns nothing
print("Return: " + str(result))    # Return: None


# Function demonstrating all return types covered
def get_data(data_type: str) -> str:
    if data_type == "name":
        return "Alice"
    elif data_type == "age":
        return "25"
    elif data_type == "city":
        return "NYC"
    return "Unknown"

data = get_data("name")    # str — "Alice"
print("Data: " + data)    # Data: Alice
