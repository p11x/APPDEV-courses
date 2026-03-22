# Example151.py
# Topic: Try-Except Basics


# ============================================================
# Example 1: Basic Try-Except
# ============================================================
print("=== Basic Try-Except ===")

try:
    result: int = 10 / 0
except ZeroDivisionError:
    print("Cannot divide by zero!")


# ============================================================
# Example 2: Multiple Except Blocks
# ============================================================
print("\n=== Multiple Except ===")

try:
    value: int = int("abc")
except ValueError:
    print("ValueError: invalid literal")
except TypeError:
    print("TypeError")


# ============================================================
# Example 3: Catch All Exceptions
# ============================================================
print("\n=== Catch All ===")

try:
    x: int = 1 / 0
except Exception as e:
    print(f"Error: {e}")


# ============================================================
# Example 4: Try-Except-Finally
# ============================================================
print("\n=== Try-Except-Finally ===")

try:
    file = open("test.txt", "r")
    content: str = file.read()
except FileNotFoundError:
    print("File not found")
finally:
    print("Cleanup code runs always")


# ============================================================
# Example 5: Try-Except-Else
# ============================================================
print("\n=== Try-Except-Else ===")

def safe_divide(a: float, b: float) -> float | None:
    try:
        result: float = a / b
    except ZeroDivisionError:
        print("Cannot divide by zero")
        return None
    else:
        print("Division successful")
        return result

print(safe_divide(10, 2))
print(safe_divide(10, 0))


# ============================================================
# Example 6: Raising Exceptions
# ============================================================
print("\n=== Raising Exceptions ===")

def validate_age(age: int) -> None:
    if age < 0:
        raise ValueError("Age cannot be negative")
    if age > 150:
        raise ValueError("Age seems unrealistic")
    print(f"Valid age: {age}")

try:
    validate_age(25)
    validate_age(-5)
except ValueError as e:
    print(f"Validation error: {e}")


# ============================================================
# Example 7: Real-World: Exception Handling
# ============================================================
print("\n=== Real-World Example ===")

def process_user_data(data: dict) -> dict:
    required_fields: list[str] = ["name", "age", "email"]
    
    for field in required_fields:
        if field not in data:
            raise ValueError(f"Missing required field: {field}")
    
    try:
        age: int = int(data["age"])
        if age < 0 or age > 150:
            raise ValueError(f"Invalid age: {age}")
    except ValueError as e:
        raise ValueError(f"Age must be a valid number: {e}")
    
    return {"status": "success", "processed": data}

try:
    user1 = {"name": "Alice", "age": "30", "email": "alice@example.com"}
    result = process_user_data(user1)
    print(f"Result: {result}")
    
    user2 = {"name": "Bob", "age": "invalid", "email": "bob@example.com"}
    result = process_user_data(user2)
except ValueError as e:
    print(f"Error: {e}")


# ============================================================
# Example 8: Exception Chaining
# ============================================================
print("\n=== Exception Chaining ===")

def inner_function(x: int) -> int:
    return 10 / x

def outer_function(x: int) -> int:
    try:
        return inner_function(x)
    except ZeroDivisionError as e:
        raise ValueError("Outer function failed") from e

try:
    outer_function(0)
except ValueError as e:
    print(f"Error: {e}")
    print(f"Caused by: {e.__cause__}")
