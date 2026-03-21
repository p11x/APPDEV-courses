# Example64.py
# Topic: Exception Handling — What Are Exceptions?

# Exceptions are errors that occur during program execution
# When something goes wrong, Python raises an exception

# === Without handling - program crashes ===

# This causes ZeroDivisionError
# result = 10 / 0
# ZeroDivisionError: division by zero

# === Different types of exceptions ===

# ZeroDivisionError - dividing by zero
try:
    result = 10 / 0
except ZeroDivisionError:
    print("Can't divide by zero!")

# ValueError - wrong type value
try:
    num = int("hello")
except ValueError:
    print("That's not a valid number!")

# TypeError - wrong operation type
try:
    result = "hello" + 5
except TypeError:
    print("Can't add string and number!")

# IndexError - list index out of range
try:
    items = [1, 2, 3]
    item = items[10]
except IndexError:
    print("Index doesn't exist!")

# KeyError - dictionary key not found
try:
    data = {"name": "Alice"}
    value = data["age"]
except KeyError:
    print("Key not found!")

# FileNotFoundError - file doesn't exist
# try:
#     f = open("nonexistent.txt")
# except FileNotFoundError:
#     print("File doesn't exist!")

# NameError - variable not defined
try:
    print(unknown_variable)
except NameError:
    print("Variable not defined!")

# === Exception objects ===
try:
    result = 10 / 0
except ZeroDivisionError as e:
    print("Error: " + str(e))

# === Exception messages ===
try:
    num = int("abc")
except ValueError as e:
    print("Error message: " + str(e))
