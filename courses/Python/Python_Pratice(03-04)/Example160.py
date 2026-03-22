# Example160.py
# Topic: Advanced Exception Handling Patterns


# ============================================================
# Example 1: Exception Hierarchy
# ============================================================
print("=== Exception Hierarchy ===")

try:
    raise ValueError("test")
except Exception as e:
    print(f"Caught: {type(e).__name__}: {e}")

try:
    raise TypeError("test type")
except ValueError:
    print("Caught ValueError")
except TypeError:
    print("Caught TypeError")


# ============================================================
# Example 2: Custom Exceptions
# ============================================================
print("\n=== Custom Exceptions ===")

class AppError(Exception):
    def __init__(self, message: str, code: int = 0):
        super().__init__(message)
        self.code = code

class ValidationError(AppError):
    pass

class DatabaseError(AppError):
    pass

try:
    raise ValidationError("Invalid input", code=400)
except ValidationError as e:
    print(f"ValidationError: {e}, code: {e.code}")
except AppError as e:
    print(f"AppError: {e}")


# ============================================================
# Example 3: Exception as Value
# ============================================================
print("\n=== Exception as Value ===")

def divide(a: float, b: float) -> float | Exception:
    try:
        return a / b
    except ZeroDivisionError as e:
        return e

result = divide(10, 0)
if isinstance(result, Exception):
    print(f"Error: {result}")
else:
    print(f"Result: {result}")


# ============================================================
# Example 4: Cleanup with Finally
# ============================================================
print("\n=== Finally Cleanup ===")

def risky_operation():
    try:
        print("Starting operation")
        raise ValueError("Something went wrong")
    except ValueError as e:
        print(f"Caught: {e}")
        raise
    finally:
        print("Cleanup always runs")

try:
    risky_operation()
except ValueError:
    print("Re-raised exception handled")


# ============================================================
# Example 5: Retrying with Exponential Backoff
# ============================================================
print("\n=== Retry Pattern ===")

import time

class RetryError(Exception):
    pass

def retry_with_backoff(func, max_retries: int = 3):
    for attempt in range(max_retries):
        try:
            return func()
        except Exception as e:
            if attempt == max_retries - 1:
                raise RetryError(f"Failed after {max_retries} attempts") from e
            wait_time = 2 ** attempt
            print(f"Attempt {attempt + 1} failed, retrying in {wait_time}s...")
            time.sleep(0.1)
    
def unstable_function():
    import random
    if random.random() < 0.7:
        raise ValueError("Random failure")
    return "Success!"

try:
    result = retry_with_backoff(unstable_function)
    print(f"Result: {result}")
except RetryError as e:
    print(f"Failed: {e}")


# ============================================================
# Example 6: Context Manager for Exception Handling
# ============================================================
print("\n=== Exception Context Manager ===")

from contextlib import contextmanager

@contextmanager
def exception_handler(handler):
    try:
        yield
    except Exception as e:
        handler(e)

def handle_error(e):
    print(f"Handled: {e}")

with exception_handler(handle_error):
    print("Normal execution")
    raise ValueError("Error!")

print("Program continues...")


# ============================================================
# Example 7: Assert Statements
# ============================================================
print("\n=== Assert Statements ===")

def factorial(n: int) -> int:
    assert n >= 0, "Factorial requires non-negative input"
    if n <= 1:
        return 1
    return n * factorial(n - 1)

try:
    print(f"Factorial of 5: {factorial(5)}")
    print(f"Factorial of -1: {factorial(-1)}")
except AssertionError as e:
    print(f"Assertion failed: {e}")


# ============================================================
# Example 8: Real-World: API Error Handling
# ============================================================
print("\n=== Real-World: API Errors ===")

class APIError(Exception):
    def __init__(self, status: int, message: str):
        self.status = status
        super().__init__(message)

def handle_api_response(response_code: int) -> str:
    error_messages = {
        400: "Bad Request - Check your input",
        401: "Unauthorized - Please login",
        403: "Forbidden - Access denied",
        404: "Not Found - Resource doesn't exist",
        500: "Internal Server Error - Try again later",
    }
    
    match response_code:
        case 200 | 201 | 204:
            return "Success"
        case n if n >= 400 and n < 500:
            return error_messages.get(n, f"Client Error: {n}")
        case n if n >= 500:
            return error_messages.get(n, f"Server Error: {n}")
        case _:
            return f"Unexpected status: {response_code}"

for code in [200, 201, 400, 401, 404, 500, 418]:
    print(f"Code {code}: {handle_api_response(code)}")
