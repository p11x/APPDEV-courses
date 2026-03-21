# Example16.py
# Topic: Parameter Type Hints

# This file demonstrates how to use type hints for function parameters.
# Type hints make functions self-documenting and help catch type-related bugs.


# Function with a single parameter type hint
# The : str indicates the parameter 'name' should be a string
def greet(name: str) -> str:
    return "Hello, " + name + "!"

result = greet("Alice")    # str — greeting message for Alice
print("Result: " + result)    # Result: Hello, Alice!

result2 = greet("Bob")    # str — greeting message for Bob
print("Result: " + result2)    # Result: Hello, Bob!


# Function with multiple parameter type hints
# Each parameter has its own type annotation
def add(a: int, b: int) -> int:
    return a + b

sum_result = add(5, 3)    # int — sum of 5 and 3
print("Sum: " + str(sum_result))    # Sum: 8

sum_result2 = add(10, 20)    # int — sum of 10 and 20
print("Sum: " + str(sum_result2))    # Sum: 30


# Function with different parameter types
def describe(name: str, age: int, height: float) -> str:
    return name + " is " + str(age) + " years old and " + str(height) + "m tall"

description = describe("Alice", 25, 1.75)    # str — formatted description
print("Description: " + description)    # Description: Alice is 25 years old and 1.75m tall


# Function with default parameter and type hints
# The default value must match the type hint
def create_user(name: str, age: int = 0, active: bool = True) -> str:
    status = "active" if active else "inactive"
    return name + ", age " + str(age) + ", status: " + status

user1 = create_user("Alice")    # str — user with default age and active
print("User1: " + user1)    # User1: Alice, age 0, status: active

user2 = create_user("Bob", 30, False)    # str — user with custom values
print("User2: " + user2)    # User2: Bob, age 30, status: inactive


# Function with list parameter type hint
def sum_list(numbers: list) -> int:
    total = 0    # int — running total
    for num in numbers:
        total = total + num
    return total

result = sum_list([1, 2, 3, 4, 5])    # int — sum of all numbers
print("List sum: " + str(result))    # List sum: 15

result2 = sum_list([10, 20, 30])    # int — sum of 10+20+30
print("List sum: " + str(result2))    # List sum: 60


# Function with dict parameter type hint
def print_dict(data: dict) -> None:
    for key, value in data.items():
        print(str(key) + ": " + str(value))

sample_dict = {"name": "Alice", "age": 25, "city": "NYC"}    # dict — sample data
print_dict(sample_dict)    # name: Alice, age: 25, city: NYC


# Function with mixed types in parameters
def process(value: str, count: int) -> str:
    return value * count

result = process("Ha", 3)    # str — "Ha" repeated 3 times
print("Result: " + result)    # Result: HaHaHa

result2 = process("Yo", 2)    # str — "Yo" repeated 2 times
print("Result: " + result2)    # Result: YoYo


# Function returning a boolean
def is_adult(age: int) -> bool:
    return age >= 18

adult_check = is_adult(25)    # bool — True for age 25
print("Is adult (25): " + str(adult_check))    # Is adult (25): True

adult_check2 = is_adult(15)    # bool — False for age 15
print("Is adult (15): " + str(adult_check2))    # Is adult (15): False


# Function returning a list
def get_list() -> list:
    return [1, 2, 3, 4, 5]

my_list = get_list()    # list — [1, 2, 3, 4, 5]
print("List: " + str(my_list))    # List: [1, 2, 3, 4, 5]


# Function returning a dict
def get_user() -> dict:
    return {"name": "Alice", "age": 25, "email": "alice@example.com"}

user_dict = get_user()    # dict — user data dictionary
print("User: " + str(user_dict))    # User: {'name': 'Alice', 'age': 25, 'email': 'alice@example.com'}


# Function with no parameters and no return value
def say_hello() -> None:
    print("Hello, World!")

say_hello()    # None — prints greeting but returns nothing
