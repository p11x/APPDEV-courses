# Example74.py
# Topic: Exception Handling — Raise From

# Use raise ... from ... to chain exceptions

# === Basic raise from ===
try:
    data = {}
    value = data["missing_key"]  # Raises KeyError
except KeyError as e:
    # Wrap the original error with context
    raise RuntimeError("Failed to get data") from e

# === Why chain exceptions? ===
# - Shows the original cause of the error
# - Preserves the traceback
# - Helps with debugging

# === Practical example ===
class DataError(Exception):
    """Error when processing data."""
    pass


def get_user_data(user_id):
    try:
        users = {1: {"name": "Alice"}, 2: {"name": "Bob"}}
        return users[user_id]
    except KeyError:
        raise DataError("User not found") from KeyError()


try:
    user = get_user_data(999)
except DataError as e:
    print("Error: " + str(e))
    print("Original: " + str(e.__cause__))

# === Chaining with different exceptions ===
def process_value(value):
    try:
        num = int(value)
        return num / 0
    except ValueError as e:
        raise TypeError("Invalid type") from e
    except ZeroDivisionError as e:
        raise RuntimeError("Math error") from e


try:
    process_value("hello")
except RuntimeError as e:
    print("Error: " + str(e))
    print("Caused by: " + str(e.__cause__))

# === Explicit chaining (raise ... from ...) ===
def access_data(key):
    data = {}
    return data[key]


def get_value(key):
    try:
        return access_data(key)
    except KeyError as e:
        # Show the original error
        raise DataError("Failed to access " + key) from e


try:
    get_value("missing")
except DataError as e:
    print("DataError: " + str(e))
    print("Original cause: " + str(e.__cause__))

# === Suppress chaining with raise ... from None ===
def maybe_raise():
    try:
        raise KeyError("original")
    except KeyError:
        # Suppress the chain - no __cause__
        raise RuntimeError("new error") from None


try:
    maybe_raise()
except RuntimeError as e:
    print("Error: " + str(e))
    print("Cause: " + str(e.__cause__))
