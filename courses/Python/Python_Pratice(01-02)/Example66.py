# Example66.py
# Topic: Exception Handling — Catching Specific Exceptions

# Always catch specific exceptions when possible

# === WRONG: Bare except catches everything ===
# This is bad practice because:
# - Catches everything including KeyboardInterrupt
# - Hard to debug - don't know what went wrong

try:
    result = 10 / 0
except:
    print("Something went wrong")

# === CORRECT: Catch specific exceptions ===

# ZeroDivisionError - dividing by zero
try:
    result = 10 / 0
except ZeroDivisionError:
    print("Can't divide by zero!")

# ValueError - invalid value conversion
try:
    num = int("hello")
except ValueError:
    print("Invalid number!")

# TypeError - wrong type operation
try:
    result = "hello" + 5
except TypeError:
    print("Type error!")

# IndexError - list index out of range
try:
    items = [1, 2, 3]
    item = items[100]
except IndexError:
    print("Index out of range!")

# KeyError - dict key not found
try:
    data = {"name": "Alice"}
    value = data["missing"]
except KeyError:
    print("Key not found!")

# === Catching multiple specific types ===
def process(value):
    try:
        # Multiple risky operations
        num = int(value)
        result = 10 / num
        return result
    except ValueError:
        return "Invalid number"
    except ZeroDivisionError:
        return "Cannot divide by zero"

print(process("42"))      # 0.238095...
print(process("hello"))   # Invalid number
print(process("0"))        # Cannot divide by zero

# === Common Exception Types ===

# ZeroDivisionError: division by zero
try:
    x = 1 / 0
except ZeroDivisionError:
    print("Zero division")

# ValueError: invalid value conversion
try:
    x = int("abc")
except ValueError:
    print("Value error")

# TypeError: wrong type operation
try:
    x = "str" + 5
except TypeError:
    print("Type error")

# IndexError: list index out of range
try:
    x = [1, 2, 3][10]
except IndexError:
    print("Index error")

# KeyError: dictionary key not found
try:
    x = {}["missing"]
except KeyError:
    print("Key error")

# FileNotFoundError: file doesn't exist
# try:
#     x = open("missing.txt")
# except FileNotFoundError:
#     print("File not found")

# NameError: variable not defined
try:
    x = undefined_var
except NameError:
    print("Name error")

# AttributeError: attribute not found
try:
    x = "hello".unknown_method()
except AttributeError:
    print("Attribute error")
