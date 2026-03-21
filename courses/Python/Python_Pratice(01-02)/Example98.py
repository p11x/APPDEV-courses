# Example98.py
# Topic: Pattern Matching — Guards Combined with Patterns

# Combining guards with literal, type, sequence, and mapping patterns

# === Guard + Literal pattern ===
status = "active"

match status:
    case "active" if True:
        print("Active status")
    case "pending" if status == "pending":
        print("Pending status")
    case _:
        print("Other status")


# === Guard + Type pattern ===
value = "hello"

match value:
    case str() as s if len(s) > 5:
        print("Long string: " + s)
    case str() as s if len(s) > 3:
        print("Medium string: " + s)
    case str() as s:
        print("Short string: " + s)
    case _:
        print("Not a string")


# === Guard + Sequence pattern ===
data = [1, 2, 3]

match data:
    case []:
        print("Empty")
    case [first] if first == 0:
        print("Starts with zero")
    case [first, *rest] if first > 0:
        print("Positive first: " + str(first) + ", rest: " + str(rest))
    case [first, *rest]:
        print("Non-positive first: " + str(first))
    case _:
        print("Other")


# === Guard + Mapping pattern ===
config = {"mode": "debug", "level": 1}

match config:
    case {"mode": "debug", "level": level} if level > 0:
        print("Debug level " + str(level))
    case {"mode": "debug", "level": level}:
        print("Debug with no level")
    case {"mode": mode} if mode == "production":
        print("Production mode")
    case {"mode": mode}:
        print("Mode: " + mode)
    case _:
        print("No config")


# === Guard + OR pattern (|) ===
code = 404

match code:
    case 400 | 401 | 403 if code == 403:
        print("Forbidden (403)")
    case 400 | 401 | 403:
        print("Client error: " + str(code))
    case 500 | 502 | 503 if code >= 500:
        print("Server error: " + str(code))
    case 500 | 502 | 503:
        print("Server error")
    case _:
        print("Other code: " + str(code))


# === Guard + Nested patterns ===
user = {"name": "Alice", "settings": {"theme": "dark", "notifications": True}}

match user:
    case {"name": name, "settings": {"theme": theme}} if theme == "dark":
        print(name + " uses dark theme")
    case {"name": name, "settings": {"theme": theme}}:
        print(name + " uses " + theme + " theme")
    case _:
        print("No user settings")


# === Guard + Multiple conditions on nested data ===
order = {"items": [{"price": 50}, {"price": 30}], "status": "processing"}

match order:
    case {"items": items, "status": "processing"} if len(items) >= 2 and any(i.get("price", 0) > 40 for i in items):
        print("Processing order with expensive items")
    case {"items": items, "status": "processing"} if len(items) >= 2:
        print("Processing order with " + str(len(items)) + " items")
    case {"items": items}:
        print("Order with " + str(len(items)) + " items")
    case _:
        print("Invalid order")


# === Practical: Complex validation ===
def validate(data):
    match data:
        case {"username": name, "email": email} if "@" in email and len(name) >= 3:
            return "Valid: " + name
        case {"username": name} if len(name) < 3:
            return "Username too short"
        case {"email": email} if "@" not in email:
            return "Invalid email"
        case {"username": _, "email": _}:
            return "Invalid data"
        case _:
            return "Missing fields"


print(validate({"username": "alice", "email": "alice@example.com"}))
print(validate({"username": "ab", "email": "test@example.com"}))
print(validate({"username": "bob", "email": "invalid"}))
print(validate({"username": "charlie"}))


# === Guard + Range check ===
temperature = 25

match temperature:
    case int() if temperature < 0:
        print("Freezing: " + str(temperature))
    case int() if 0 <= temperature < 10:
        print("Cold: " + str(temperature))
    case int() if 10 <= temperature < 20:
        print("Cool: " + str(temperature))
    case int() if 20 <= temperature < 30:
        print("Warm: " + str(temperature))
    case int() if temperature >= 30:
        print("Hot: " + str(temperature))
    case _:
        print("Invalid temperature")


# === Guard + Class pattern (simulated with type) ===
# Note: Class patterns use ClassName() syntax
from datetime import datetime

now = datetime.now()

match now:
    case datetime() as dt if dt.hour < 12:
        print("Morning: " + str(dt.hour) + ":" + str(dt.minute))
    case datetime() as dt if dt.hour < 17:
        print("Afternoon: " + str(dt.hour) + ":" + str(dt.minute))
    case datetime() as dt:
        print("Evening: " + str(dt.hour) + ":" + str(dt.minute))
    case _:
        print("Not a datetime")
