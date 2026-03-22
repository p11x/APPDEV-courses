# Example159.py
# Topic: Control Flow - Real-World Examples


# ============================================================
# Example 1: Calculator
# ============================================================
print("=== Calculator ===")

def calculator(a: float, b: float, op: str) -> float:
    match op:
        case "+":
            return a + b
        case "-":
            return a - b
        case "*":
            return a * b
        case "/":
            if b == 0:
                raise ValueError("Cannot divide by zero")
            return a / b
        case _:
            raise ValueError(f"Unknown operator: {op}")

print(f"10 + 5 = {calculator(10, 5, '+')}")
print(f"10 - 5 = {calculator(10, 5, '-')}")
print(f"10 * 5 = {calculator(10, 5, '*')}")
print(f"10 / 5 = {calculator(10, 5, '/')}")


# ============================================================
# Example 2: HTTP Response Handler
# ============================================================
print("\n=== HTTP Response ===")

def handle_response(status: int, data: dict) -> str:
    match status:
        case 200:
            return f"Success: {data.get('message', 'OK')}"
        case 201:
            return f"Created: {data.get('id', 'new resource')}"
        case 400:
            return f"Bad Request: {data.get('error', 'Invalid input')}"
        case 401:
            return "Unauthorized"
        case 404:
            return "Not Found"
        case 500:
            return "Internal Server Error"
        case _:
            return f"Unknown status: {status}"

responses = [
    (200, {"message": "Data loaded"}),
    (201, {"id": 123}),
    (400, {"error": "Missing required field"}),
    (404, {}),
    (500, {}),
]

for status, data in responses:
    print(f"{status}: {handle_response(status, data)}")


# ============================================================
# Example 3: Age Group Classifier
# ============================================================
print("\n=== Age Classifier ===")

def classify_age(age: int) -> str:
    match age:
        case n if n < 0:
            return "Invalid age"
        case n if n < 13:
            return "Child"
        case n if n < 20:
            return "Teenager"
        case n if n < 65:
            return "Adult"
        case _:
            return "Senior"

for age in [-5, 5, 15, 30, 70]:
    print(f"Age {age}: {classify_age(age)}")


# ============================================================
# Example 4: File Type Detection
# ============================================================
print("\n=== File Type ===")

def get_file_type(filename: str) -> str:
    ext = filename.split(".")[-1].lower() if "." in filename else ""
    
    match ext:
        case "jpg" | "jpeg" | "png" | "gif":
            return "Image"
        case "mp4" | "avi" | "mov":
            return "Video"
        case "mp3" | "wav" | "flac":
            return "Audio"
        case "pdf" | "doc" | "docx":
            return "Document"
        case "py" | "js" | "java":
            return "Source Code"
        case _:
            return "Unknown"

files = ["photo.jpg", "movie.mp4", "song.mp3", "report.pdf", "script.py", "data.xyz"]
for f in files:
    print(f"{f}: {get_file_type(f)}")


# ============================================================
# Example 5: Shopping Cart with Validation
# ============================================================
print("\n=== Shopping Cart ===")

def calculate_total(items: list[dict]) -> float:
    if not items:
        return 0.0
    
    total = 0.0
    for item in items:
        try:
            quantity = int(item.get("quantity", 1))
            price = float(item.get("price", 0))
            total += quantity * price
        except (ValueError, TypeError) as e:
            print(f"Skipping invalid item: {e}")
            continue
    
    return total

cart = [
    {"name": "Laptop", "price": 999.99, "quantity": 1},
    {"name": "Mouse", "price": 29.99, "quantity": 2},
    {"name": "Keyboard", "price": 79.99, "quantity": "invalid"},
]

total = calculate_total(cart)
print(f"Total: ${total:.2f}")


# ============================================================
# Example 6: Temperature Converter
# ============================================================
print("\n=== Temperature Converter ===")

def convert_temp(value: float, from_unit: str, to_unit: str) -> float:
    match (from_unit.upper(), to_unit.upper()):
        case ("C", "F"):
            return (value * 9/5) + 32
        case ("F", "C"):
            return (value - 32) * 5/9
        case ("C", "K"):
            return value + 273.15
        case ("K", "C"):
            return value - 273.15
        case ("F", "K"):
            c = (value - 32) * 5/9
            return c + 273.15
        case ("K", "F"):
            c = value - 273.15
            return (c * 9/5) + 32
        case _:
            raise ValueError(f"Invalid conversion: {from_unit} to {to_unit}")

print(f"0°C to F: {convert_temp(0, 'C', 'F'):.1f}°F")
print(f"32°F to C: {convert_temp(32, 'F', 'C'):.1f}°C")
print(f"100°C to K: {convert_temp(100, 'C', 'K'):.1f}K")


# ============================================================
# Example 7: Validation Pipeline
# ============================================================
print("\n=== Validation Pipeline ===")

def validate_user(data: dict) -> tuple[bool, list[str]]:
    errors: list[str] = []
    
    if not data.get("username"):
        errors.append("Username is required")
    elif len(data["username"]) < 3:
        errors.append("Username must be at least 3 characters")
    
    if not data.get("email"):
        errors.append("Email is required")
    elif "@" not in data.get("email", ""):
        errors.append("Email is invalid")
    
    age = data.get("age")
    if age is not None:
        try:
            age = int(age)
            if age < 0 or age > 150:
                errors.append("Age must be between 0 and 150")
        except ValueError:
            errors.append("Age must be a number")
    
    return (len(errors) == 0, errors)

users = [
    {"username": "alice", "email": "alice@example.com", "age": 30},
    {"username": "ab", "email": "invalid", "age": -5},
    {"username": "bob", "email": "bob@example.com"},
]

for user in users:
    valid, errors = validate_user(user)
    if valid:
        print(f"{user.get('username')}: Valid")
    else:
        print(f"{user.get('username')}: Invalid - {errors}")


# ============================================================
# Example 8: Priority Queue
# ============================================================
print("\n=== Priority Queue ===")

import heapq

class PriorityQueue:
    def __init__(self):
        self._heap = []
    
    def enqueue(self, item: str, priority: int):
        heapq.heappush(self._heap, (-priority, item))
    
    def dequeue(self) -> str | None:
        if self._heap:
            return heapq.heappop(self._heap)[1]
        return None

pq = PriorityQueue()
pq.enqueue("Low priority task", 1)
pq.enqueue("High priority task", 10)
pq.enqueue("Medium priority task", 5)

print("Processing tasks:")
while True:
    task = pq.dequeue()
    if task is None:
        break
    print(f"  - {task}")
