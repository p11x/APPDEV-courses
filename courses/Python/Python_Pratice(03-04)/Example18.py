# Example18.py
# Topic: Union Types with | (Python 3.10+)

# This file demonstrates how to use the | operator for union types.
# Union types allow a parameter or return type to be multiple types.
# This is the modern Python 3.10+ syntax (older code uses Optional or Union).


# Function with union type parameter
# The int | float means the parameter can be either an integer or float
def process(value: int | float) -> int | float:
    return value * 2

result1 = process(5)    # int — 5 * 2 = 10
print("Integer input: " + str(result1))    # Integer input: 10

result2 = process(3.14)    # float — 3.14 * 2 = 6.28
print("Float input: " + str(result2))    # Float input: 6.28


# Function returning union type
# Can return either an integer or a string
def describe(value: int | str) -> str:
    return "Value: " + str(value)

result1 = describe(42)    # str — "Value: 42"
print(result1)    # Value: 42

result2 = describe("hello")    # str — "Value: hello"
print(result2)    # Value: hello


# Function with union type for parameter and return
def add_or_concat(a: int | str, b: int | str) -> int | str:
    if isinstance(a, str) or isinstance(b, str):
        return str(a) + str(b)
    return a + b

result1 = add_or_concat(5, 3)    # int — 5 + 3 = 8
print("Number + Number: " + str(result1))    # Number + Number: 8

result2 = add_or_concat("Hello", "World")    # str — concatenated string
print("String + String: " + str(result2))    # String + String: HelloWorld


# Function that can return None or a value
# The int | None means the function might not return anything
def find_in_list(items: list, target: int) -> int | None:
    for index, item in enumerate(items):
        if item == target:
            return index
    return None

result1 = find_in_list([1, 2, 3, 4, 5], 3)    # int — index 2
print("Found at index: " + str(result1))    # Found at index: 2

result2 = find_in_list([1, 2, 3], 99)    # None — not found
print("Found: " + str(result2))    # Found: None


# Function with multiple possible return types
def format_data(value: int | float | str) -> str:
    if isinstance(value, int):
        return "Integer: " + str(value)
    elif isinstance(value, float):
        return "Float: " + str(value)
    return "String: " + value

result1 = format_data(42)    # str — "Integer: 42"
print(result1)    # Integer: 42

result2 = format_data(3.14)    # str — "Float: 3.14"
print(result2)    # Float: 3.14

result3 = format_data("hello")    # str — "String: hello"
print(result3)    # String: hello


# Union type with three types
def process_value(value: int | float | bool) -> int | float | bool:
    if isinstance(value, bool):
        return not value    # bool — invert boolean
    return value * 2    # int or float — double the value

result1 = process_value(10)    # int — 20
print("Int: " + str(result1))    # Int: 20

result2 = process_value(2.5)    # float — 5.0
print("Float: " + str(result2))    # Float: 5.0

result3 = process_value(True)    # bool — False
print("Bool: " + str(result3))    # Bool: False


# Function with optional parameter using union syntax
def greet(name: str | None) -> str:
    if name is None:
        return "Hello, Guest!"
    return "Hello, " + name + "!"

result1 = greet("Alice")    # str — "Hello, Alice!"
print(result1)    # Hello, Alice!

result2 = greet(None)    # str — "Hello, Guest!"
print(result2)    # Hello, Guest!


# Parse function that can fail
def parse_number(value: str) -> int | float | None:
    if "." in value:
        try:
            return float(value)    # float — parsed as decimal
        except ValueError:
            return None
    else:
        try:
            return int(value)    # int — parsed as integer
        except ValueError:
            return None

result1 = parse_number("42")    # int — 42
print("Parsed int: " + str(result1) + " (type: " + str(type(result1).__name__) + ")")    # Parsed int: 42 (type: int)

result2 = parse_number("3.14")    # float — 3.14
print("Parsed float: " + str(result2) + " (type: " + str(type(result2).__name__) + ")")    # Parsed float: 3.14 (type: float)

result3 = parse_number("abc")    # None — invalid input
print("Parsed invalid: " + str(result3))    # Parsed invalid: None


# Union type in dictionary values
def get_config(key: str) -> str | int | bool | None:
    configs = {
        "host": "localhost",
        "port": 8080,
        "debug": False,
        "timeout": None
    }
    return configs.get(key)

result1 = get_config("host")    # str — "localhost"
print("Host: " + str(result1))    # Host: localhost

result2 = get_config("port")    # int — 8080
print("Port: " + str(result2))    # Port: 8080

result3 = get_config("debug")    # bool — False
print("Debug: " + str(result3))    # Debug: False


# Complex union type example
def process_input(data: int | str | list) -> str:
    if isinstance(data, int):
        return "Integer: " + str(data * 2)
    elif isinstance(data, str):
        return "String: " + data.upper()
    elif isinstance(data, list):
        return "List length: " + str(len(data))
    return "Unknown type"

result1 = process_input(5)    # str — "Integer: 10"
print(result1)    # Integer: 10

result2 = process_input("hello")    # str — "String: HELLO"
print(result2)    # String: HELLO

result3 = process_input([1, 2, 3])    # str — "List length: 3"
print(result3)    # List length: 3
