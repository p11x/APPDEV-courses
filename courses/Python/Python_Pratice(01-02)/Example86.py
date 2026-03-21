# Example86.py
# Topic: Pattern Matching — Type Matching

# Match based on the type of value using type patterns

# === Basic type matching ===
# Use type() with parentheses to match types

value = "hello world"

match value:
    case str():
        print("It's a string: " + value)
    case int():
        print("It's an integer")
    case float():
        print("It's a decimal")
    case bool():
        print("It's a boolean")
    case list():
        print("It's a list")
    case _:
        print("Unknown type")


# === Type matching with integers ===
number = 42

match number:
    case str():
        print("String: " + number)
    case int():
        print("Integer: " + str(number))
    case float():
        print("Float")
    case _:
        print("Other type")


# === Type matching with floats ===
price = 19.99

match price:
    case str():
        print("String")
    case int():
        print("Integer")
    case float():
        print("Float: " + str(price))
    case _:
        print("Other")


# === Type matching with lists ===
data = [1, 2, 3]

match data:
    case str():
        print("String")
    case int():
        print("Integer")
    case list():
        print("List with " + str(len(data)) + " items")
    case dict():
        print("Dictionary")
    case _:
        print("Other type")


# === Type matching with dictionaries ===
config = {"host": "localhost"}

match config:
    case str():
        print("String")
    case list():
        print("List")
    case dict():
        print("Dictionary with keys: " + str(list(config.keys())))
    case _:
        print("Other")


# === Type matching with booleans ===
flag = False

match flag:
    case str():
        print("String")
    case int():
        print("Integer")
    case bool():
        print("Boolean: " + str(flag))
    case _:
        print("Other")


# === Practical: Type-based processing ===
def process(value):
    match value:
        case str():
            return "String of length " + str(len(value))
        case int():
            return "Integer: " + str(value)
        case float():
            return "Float: " + str(value)
        case list():
            return "List with " + str(len(value)) + " items"
        case dict():
            return "Dict with " + str(len(value)) + " keys"
        case _:
            return "Unknown type"


print(process("hello"))
print(process(100))
print(process(3.14))
print(process([1, 2, 3]))
print(process({"a": 1}))
print(process((1, 2)))


# === Type matching order matters ===
# More specific types should come first
mixed = 42

match mixed:
    case int():
        print("Integer")
    case str():
        print("String")
    case _:
        print("Other")
