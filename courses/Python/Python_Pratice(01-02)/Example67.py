# Example67.py
# Topic: Exception Handling — Multiple Except Blocks

# Handle different exceptions differently

# === Multiple except blocks ===
def process_input(value):
    try:
        number = int(value)
        result = 10 / number
        return result
    except ValueError:
        return "Error: That's not a valid number"
    except ZeroDivisionError:
        return "Error: Cannot divide by zero"

print(process_input("42"))      # 0.238...
print(process_input("hello"))   # Error message
print(process_input("0"))        # Error message

# === Order matters ===
# More specific exceptions first, general ones later

def handle_error(value):
    try:
        result = int(value) / 0
    except ZeroDivisionError:
        return "Can't divide by zero"
    except ArithmeticError:
        return "Math error"
    except Exception:
        return "Something else went wrong"

# === Practical example: Calculator ===
def calculate(a, b, operation):
    try:
        a = float(a)
        b = float(b)
        
        if operation == "+":
            return a + b
        elif operation == "-":
            return a - b
        elif operation == "*":
            return a * b
        elif operation == "/":
            return a / b
        else:
            return "Unknown operation"
            
    except ValueError:
        return "Invalid number entered"
    except ZeroDivisionError:
        return "Cannot divide by zero"
    except TypeError:
        return "Invalid types"

print(calculate("10", "5", "+"))   # 15.0
print(calculate("10", "0", "/"))   # Cannot divide by zero
print(calculate("abc", "5", "+"))  # Invalid number entered
print(calculate("10", "5", "%"))  # Unknown operation

# === Exception as variable ===
def handle_exception(value):
    try:
        number = int(value)
        result = 10 / number
        return result
    except (ValueError, ZeroDivisionError) as e:
        return "Error: " + str(e)

print(handle_exception("42"))
print(handle_exception("hello"))

# === Catching multiple exceptions in one block ===
def safe_operation(value):
    try:
        num = int(value)
        result = 10 / num
    except (ValueError, ZeroDivisionError):
        return "Error occurred"
    return result

print(safe_operation("5"))     # 2.0
print(safe_operation("hello"))  # Error occurred
