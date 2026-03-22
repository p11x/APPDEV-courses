# Example156.py
# Topic: Control Flow - Comprehensive Examples


# ============================================================
# Example 1: FizzBuzz Problem
# ============================================================
print("=== FizzBuzz ===")

for i in range(1, 16):
    if i % 3 == 0 and i % 5 == 0:
        print("FizzBuzz", end=" ")
    elif i % 3 == 0:
        print("Fizz", end=" ")
    elif i % 5 == 0:
        print("Buzz", end=" ")
    else:
        print(i, end=" ")
print()


# ============================================================
# Example 2: Prime Number Check
# ============================================================
print("\n=== Prime Check ===")

def is_prime(n: int) -> bool:
    if n < 2:
        return False
    for i in range(2, int(n**0.5) + 1):
        if n % i == 0:
            return False
    return True

for num in range(1, 21):
    print(f"{num}: {'Prime' if is_prime(num) else 'Not prime'}")


# ============================================================
# Example 3: Factorial
# ============================================================
print("\n=== Factorial ===")

def factorial(n: int) -> int:
    if n <= 1:
        return 1
    return n * factorial(n - 1)

for i in range(6):
    print(f"{i}! = {factorial(i)}")


# ============================================================
# Example 4: Fibonacci
# ============================================================
print("\n=== Fibonacci ===")

def fibonacci(n: int) -> int:
    if n <= 1:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)

print("First 10 Fibonacci numbers:")
for i in range(10):
    print(fibonacci(i), end=" ")
print()


# ============================================================
# Example 5: Error Handling Chain
# ============================================================
print("\n=== Error Handling Chain ===")

def robust_divide(a: float, b: float) -> str:
    try:
        if b == 0:
            raise ZeroDivisionError("Cannot divide by zero")
        result = a / b
        return f"Result: {result}"
    except ZeroDivisionError as e:
        return f"Error: {e}"
    except TypeError as e:
        return f"Type error: {e}"
    except Exception as e:
        return f"Unexpected error: {e}"

print(robust_divide(10, 2))
print(robust_divide(10, 0))
print(robust_divide(10, "two"))


# ============================================================
# Example 6: Custom Exception
# ============================================================
print("\n=== Custom Exception ===")

class ValidationError(Exception):
    pass

def validate_email(email: str) -> None:
    if "@" not in email:
        raise ValidationError(f"Invalid email: {email}")
    print(f"Valid email: {email}")

try:
    validate_email("alice@example.com")
    validate_email("invalid-email")
except ValidationError as e:
    print(f"Caught: {e}")


# ============================================================
# Example 7: Retry Pattern
# ============================================================
print("\n=== Retry Pattern ===")

class NetworkError(Exception):
    pass

def fetch_data(attempt: int) -> str:
    if attempt < 3:
        raise NetworkError(f"Attempt {attempt} failed")
    return "Data fetched successfully"

attempts = 0
max_attempts = 5

while attempts < max_attempts:
    attempts += 1
    try:
        result = fetch_data(attempts)
        print(result)
        break
    except NetworkError as e:
        print(f"Error: {e}, retrying...")
else:
    print("Max attempts reached")


# ============================================================
# Example 8: State Machine Pattern
# ============================================================
print("\n=== State Machine ===")

class OrderState:
    PENDING = "pending"
    CONFIRMED = "confirmed"
    SHIPPED = "shipped"
    DELIVERED = "delivered"

def process_order(state: str, action: str) -> str:
    match (state, action):
        case (OrderState.PENDING, "confirm"):
            return OrderState.CONFIRMED
        case (OrderState.CONFIRMED, "ship"):
            return OrderState.SHIPPED
        case (OrderState.SHIPPED, "deliver"):
            return OrderState.DELIVERED
        case (OrderState.DELIVERED, _):
            return "Order already completed"
        case _:
            return f"Invalid transition: {state} + {action}"

state = OrderState.PENDING
print(f"Initial: {state}")

transitions = ["confirm", "ship", "deliver"]
for action in transitions:
    state = process_order(state, action)
    print(f"After '{action}': {state}")
