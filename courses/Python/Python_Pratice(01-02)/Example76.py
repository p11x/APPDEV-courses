# Example76.py
# Topic: Exception Handling — Common Mistakes

# Common mistakes when raising exceptions

# === MISTAKE 1: Raising Wrong Exception Type ===

# WRONG - don't use strings as exceptions
# raise "Something went wrong"  # SyntaxError!

# CORRECT - raise exception instances
raise ValueError("Something went wrong")

# === MISTAKE 2: Not Re-raising Exceptions ===

# WRONG - catching and not re-raising loses the error
def bad_handler():
    try:
        x = 1 / 0
    except Exception:
        print("Error!")  
        # Error is now lost!

# CORRECT - re-raise to preserve error
def good_handler():
    try:
        x = 1 / 0
    except Exception as e:
        print("Error: " + str(e))
        raise  # Re-raise the exception


# === MISTAKE 3: Using Exceptions for Control Flow ===

# WRONG - don't use exceptions for normal logic
def get_value_bad(data):
    try:
        return data["key"]
    except KeyError:
        return "default"


# CORRECT - use .get() method
def get_value_good(data):
    return data.get("key", "default")


# === MISTAKE 4: Catching Too Broad ===

# WRONG - catching everything
# except:  # Bare except is bad!
#     pass

# CORRECT - catch specific
try:
    x = 1 / 0
except ZeroDivisionError:
    print("Division error")


# === MISTAKE 5: Not Using Custom Exceptions ===

# Using generic exceptions makes debugging hard
# Better to use specific custom exceptions


# === MISTAKE 6: Raising in Wrong Place ===

# WRONG - raise outside try/except or function
# if age < 0:
#     raise ValueError("Negative age")  # Outside function context


# CORRECT - raise in appropriate context
def set_age(age):
    if age < 0:
        raise ValueError("Age cannot be negative")
    return age


# === Best Practices ===

# 1. Use specific exception types
# 2. Write meaningful error messages
# 3. Re-raise exceptions when needed
# 4. Use custom exceptions for your app
# 5. Don't use exceptions for flow control

# === Good Example ===
class InvalidAgeError(Exception):
    pass


def validate_age(age):
    if not isinstance(age, int):
        raise TypeError("Age must be an integer")
    if age < 0:
        raise InvalidAgeError("Age cannot be negative")
    if age > 150:
        raise InvalidAgeError("Age is unrealistic")
    return age


# Test
tests = [25, -5, "hello", 200]

for test in tests:
    try:
        validate_age(test)
    except (InvalidAgeError, TypeError) as e:
        print("Validation failed for " + str(test) + ": " + str(e))
