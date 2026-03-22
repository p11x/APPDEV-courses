# Example162.py
# Topic: Complex Control Flow Patterns


# ============================================================
# Example 1: Early Return Pattern
# ============================================================
print("=== Early Return Pattern ===")

def process_data(data: dict) -> dict:
    if not data:
        return {"error": "No data provided"}
    
    if "name" not in data:
        return {"error": "Missing name field"}
    
    if not isinstance(data.get("age"), int):
        return {"error": "Age must be an integer"}
    
    return {"success": True, "processed": data}

print(process_data({}))
print(process_data({"name": "Alice"}))
print(process_data({"name": "Bob", "age": "invalid"}))
print(process_data({"name": "Charlie", "age": 30}))


# ============================================================
# Example 2: Guard Clauses
# ============================================================
print("\n=== Guard Clauses ===")

def withdraw(balance: float, amount: float) -> str:
    if amount <= 0:
        return "Amount must be positive"
    
    if amount > balance:
        return "Insufficient funds"
    
    if amount > 10000:
        return "Amount exceeds daily limit"
    
    return f"Withdrawn: ${amount}"

print(withdraw(5000, -100))
print(withdraw(5000, 6000))
print(withdraw(5000, 15000))
print(withdraw(5000, 500))


# ============================================================
# Example 3: State Pattern with Dictionary
# ============================================================
print("\n=== State Pattern ===")

class StateMachine:
    def __init__(self):
        self.state = "initial"
        self.handlers = {
            "initial": self.handle_initial,
            "loading": self.handle_loading,
            "success": self.handle_success,
            "error": self.handle_error,
        }
    
    def handle(self):
        handler = self.handlers.get(self.state, self.handle_error)
        return handler()
    
    def handle_initial(self):
        self.state = "loading"
        return "Starting..."
    
    def handle_loading(self):
        self.state = "success"
        return "Loading data..."
    
    def handle_success(self):
        return "Data loaded!"
    
    def handle_error(self):
        return "An error occurred"

sm = StateMachine()
for _ in range(5):
    print(sm.handle())


# ============================================================
# Example 4: Strategy Pattern
# ============================================================
print("\n=== Strategy Pattern ===")

class PaymentStrategy:
    def pay(self, amount: float) -> str:
        raise NotImplementedError

class CreditCardPayment(PaymentStrategy):
    def pay(self, amount: float) -> str:
        return f"Paid ${amount} with Credit Card"

class PayPalPayment(PaymentStrategy):
    def pay(self, amount: float) -> str:
        return f"Paid ${amount} with PayPal"

class CryptoPayment(PaymentStrategy):
    def pay(self, amount: float) -> str:
        return f"Paid ${amount} with Crypto"

def process_payment(amount: float, strategy: PaymentStrategy) -> str:
    return strategy.pay(amount)

print(process_payment(100, CreditCardPayment()))
print(process_payment(200, PayPalPayment()))
print(process_payment(300, CryptoPayment()))


# ============================================================
# Example 5: Chain of Responsibility
# ============================================================
print("\n=== Chain of Responsibility ===")

class Handler:
    def __init__(self):
        self.next_handler = None
    
    def set_next(self, handler):
        self.next_handler = handler
        return handler
    
    def handle(self, request: str) -> str:
        if self.next_handler:
            return self.next_handler.handle(request)
        return "No handler found"

class AuthHandler(Handler):
    def handle(self, request: str) -> str:
        if not request.get("authenticated"):
            return "Authentication failed"
        return super().handle(request)

class ValidationHandler(Handler):
    def handle(self, request: dict) -> str:
        if not request.get("data"):
            return "Validation failed"
        return super().handle(request)

chain = AuthHandler()
chain.set_next(ValidationHandler())

print(chain.handle({"authenticated": True, "data": "valid"}))
print(chain.handle({"authenticated": False, "data": "valid"}))
print(chain.handle({"authenticated": True, "data": None}))


# ============================================================
# Example 6: Loop with Counter
# ============================================================
print("\n=== Loop with Counter ===")

data = ["a", "b", "c", "d", "e"]

for i, item in enumerate(data):
    print(f"Index {i}: {item}")

print("\nWith start:")
for i, item in enumerate(data, start=1):
    print(f"#{i}: {item}")


# ============================================================
# Example 7: Nested Dictionary Iteration
# ============================================================
print("\n=== Nested Dict Iteration ===")

config = {
    "database": {
        "host": "localhost",
        "port": 5432,
        "settings": {"timeout": 30, "pool": 10}
    },
    "cache": {
        "enabled": True,
        "ttl": 3600
    }
}

def flatten_dict(d: dict, prefix: str = "") -> dict:
    result = {}
    for key, value in d.items():
        new_key = f"{prefix}.{key}" if prefix else key
        if isinstance(value, dict):
            result.update(flatten_dict(value, new_key))
        else:
            result[new_key] = value
    return result

print(flatten_dict(config))


# ============================================================
# Example 8: Recursive Loop Detection
# ============================================================
print("\n=== Recursive Functions ===")

def factorial(n: int, depth: int = 0) -> int:
    indent = "  " * depth
    print(f"{indent}factorial({n})")
    if n <= 1:
        print(f"{indent}  -> 1")
        return 1
    result = n * factorial(n - 1, depth + 1)
    print(f"{indent}  -> {result}")
    return result

print(factorial(5))
