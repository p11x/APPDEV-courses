# Example65.py
# Topic: Exception Handling — Basic Try/Except

# The try/except block handles errors gracefully

# === Syntax ===
# try:
#     # Code that might cause an error
# except ExceptionType:
#     # Code that runs if error occurs

# === Basic example ===
try:
    result = 10 / 0
except ZeroDivisionError:
    print("Can't divide by zero!")

# === Division example ===
def divide(a, b):
    try:
        result = a / b
        return result
    except ZeroDivisionError:
        return None

print(divide(10, 2))   # 5.0
print(divide(10, 0))   # None

# === Converting to integer ===
def to_integer(value):
    try:
        return int(value)
    except ValueError:
        return None

print(to_integer("42"))      # 42
print(to_integer("hello"))  # None
print(to_integer("3.14"))   # None (ValueError!)

# === Accessing list safely ===
def get_item(items, index):
    try:
        return items[index]
    except IndexError:
        return None

fruits = ["apple", "banana", "cherry"]
print(get_item(fruits, 0))   # apple
print(get_item(fruits, 10))  # None

# === Opening file safely ===
# def read_file(filename):
#     try:
#         f = open(filename, "r")
#         content = f.read()
#         return content
#     except FileNotFoundError:
#         return None

# Real-world: Division with error message
def safe_divide(a, b):
    try:
        result = a / b
        print("Division successful: " + str(result))
    except ZeroDivisionError:
        print("Error: Cannot divide by zero!")

safe_divide(10, 2)
safe_divide(10, 0)

# Real-world: Getting dictionary value
def get_value(data, key):
    try:
        return data[key]
    except KeyError:
        return None

user = {"name": "Alice", "age": 25}
print(get_value(user, "name"))    # Alice
print(get_value(user, "email"))   # None
