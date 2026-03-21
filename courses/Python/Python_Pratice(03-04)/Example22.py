# Example22.py
# Topic: Callable Type Hints

# This file demonstrates how to type hint callable objects (functions).
# Callable type hints show that a parameter is itself a function.


# Import Callable from typing for older Python versions
from typing import Callable


# Function that takes another function as parameter
# Callable[[int], int] means: takes an int, returns an int
def apply_func(func: Callable[[int], int], value: int) -> int:
    return func(value)

def double(x: int) -> int:
    return x * 2

def square(x: int) -> int:
    return x * x

result1 = apply_func(double, 5)    # int — 5 * 2 = 10
print("Double 5: " + str(result1))    # Double 5: 10

result2 = apply_func(square, 5)    # int — 5 ^ 2 = 25
print("Square 5: " + str(result2))    # Square 5: 25


# Function with Callable that takes multiple arguments
# Callable[[int, int], int] means: takes two ints, returns an int
def calculate(func: Callable[[int, int], int], a: int, b: int) -> int:
    return func(a, b)

def add(x: int, y: int) -> int:
    return x + y

def multiply(x: int, y: int) -> int:
    return x * y

result1 = calculate(add, 5, 3)    # int — 5 + 3 = 8
print("Add: " + str(result1))    # Add: 8

result2 = calculate(multiply, 5, 3)    # int — 5 * 3 = 15
print("Multiply: " + str(result2))    # Multiply: 15


# Function that returns a callable
# Callable[[], int] means: takes no arguments, returns an int
def create_counter() -> Callable[[], int]:
    count = 0    # int — nonlocal counter
    def counter() -> int:
        nonlocal count
        count = count + 1
        return count
    return counter

counter = create_counter()    # Callable — the counter function
c1 = counter()    # int — 1
c2 = counter()    # int — 2
c3 = counter()    # int — 3
print("Counts: " + str(c1) + ", " + str(c2) + ", " + str(c3))    # Counts: 1, 2, 3


# Function with lambda parameter
def process_items(items: list, func: Callable[[int], int]) -> list:
    result = []    # list — transformed items
    for item in items:
        result.append(func(item))
    return result

numbers = [1, 2, 3, 4, 5]    # list — original numbers
doubled = process_items(numbers, lambda x: x * 2)    # list — [2, 4, 6, 8, 10]
print("Doubled: " + str(doubled))    # Doubled: [2, 4, 6, 8, 10]

squared = process_items(numbers, lambda x: x ** 2)    # list — [1, 4, 9, 16, 25]
print("Squared: " + str(squared))    # Squared: [1, 4, 9, 16, 25]


# Callable with no return value
# Callable[[str], None] means: takes a string, returns nothing
def execute(func: Callable[[str], None], message: str) -> None:
    func(message)

def print_message(msg: str) -> None:
    print("Message: " + msg)

execute(print_message, "Hello World!")    # Message: Hello World!


# Function that takes a callable and optional args
def apply_with_args(func: Callable[[int, int], int], a: int, b: int = 10) -> int:
    return func(a, b)

def power(base: int, exp: int) -> int:
    return base ** exp

result1 = apply_with_args(power, 2)    # int — 2 ^ 10 = 1024 (uses default b)
print("Power: " + str(result1))    # Power: 1024

result2 = apply_with_args(power, 2, 3)    # int — 2 ^ 3 = 8
print("Power: " + str(result2))    # Power: 8


# Callable that takes any type
# Callable[[any], str] means: takes any type, returns string
def convert_to_string(func: Callable[[], any]) -> str:
    value = func()
    return str(value)

def get_number() -> int:
    return 42

def get_list() -> list:
    return [1, 2, 3]

result1 = convert_to_string(get_number)    # str — "42"
print("As string: " + result1)    # As string: 42

result2 = convert_to_string(get_list)    # str — "[1, 2, 3]"
print("As string: " + result2)    # As string: [1, 2, 3]


# Predicate function - returns bool
# Callable[[int], bool] means: takes an int, returns a boolean
def filter_numbers(numbers: list, predicate: Callable[[int], bool]) -> list:
    result = []    # list — filtered numbers
    for num in numbers:
        if predicate(num):
            result.append(num)
    return result

is_even = lambda x: x % 2 == 0    # Callable — checks if even
is_positive = lambda x: x > 0    # Callable — checks if positive

numbers = [-2, -1, 0, 1, 2, 3, 4]    # list — mixed numbers
evens = filter_numbers(numbers, is_even)    # list — [0, 2, 4]
print("Evens: " + str(evens))    # Evens: [0, 2, 4]

positives = filter_numbers(numbers, is_positive)    # list — [1, 2, 3, 4]
print("Positives: " + str(positives))    # Positives: [1, 2, 3, 4]


# Nested callable - function that returns a function
def create_greeting(prefix: str) -> Callable[[str], str]:
    def greeting(name: str) -> str:
        return prefix + ", " + name + "!"
    return greeting

hello = create_greeting("Hello")    # Callable — hello greeting
hi = create_greeting("Hi")    # Callable — hi greeting

result1 = hello("Alice")    # str — "Hello, Alice!"
result2 = hi("Bob")    # str — "Hi, Bob!"
print(result1)    # Hello, Alice!
print(result2)    # Hi, Bob!


# Real-world: sorting with custom key function
def sort_by_key(items: list, key_func: Callable[[dict], any]) -> list:
    return sorted(items, key=key_func)

users = [
    {"name": "Alice", "age": 25},
    {"name": "Bob", "age": 20},
    {"name": "Charlie", "age": 30}
]

by_age = sort_by_key(users, lambda u: u["age"])    # list — sorted by age
print("By age: " + str(by_age))    # By age: [{'name': 'Bob', ...}, {'name': 'Alice', ...}, {'name': 'Charlie', ...}]

by_name = sort_by_key(users, lambda u: u["name"])    # list — sorted by name
print("By name: " + str(by_name))    # By name: [{'name': 'Alice', ...}, {'name': 'Bob', ...}, {'name': 'Charlie', ...}]
