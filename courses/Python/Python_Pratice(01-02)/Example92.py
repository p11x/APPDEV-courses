# Example92.py
# Topic: Pattern Matching — Basic Mapping Patterns

# Match dictionaries using mapping patterns

# === Basic dictionary matching ===
# Match specific keys in a dictionary

user = {"name": "Alice", "age": 30}

match user:
    case {"name": name, "age": age}:
        print(name + " is " + str(age) + " years old")
    case _:
        print("Not a valid user")


# === Matching with specific values ===
config = {"mode": "dark", "language": "en"}

match config:
    case {"mode": "dark"}:
        print("Dark mode enabled")
    case {"mode": "light"}:
        print("Light mode enabled")
    case _:
        print("Unknown mode")


# === Matching partial keys ===
person = {"name": "Bob", "city": "NYC", "age": 25}

match person:
    case {"name": name}:
        print("Name: " + name)
    case _:
        print("No name found")


# === Matching multiple specific keys ===
order = {"id": 123, "status": "shipped", "total": 99.99}

match order:
    case {"id": order_id, "status": "shipped"}:
        print("Order #" + str(order_id) + " has shipped")
    case {"id": order_id, "status": "pending"}:
        print("Order #" + str(order_id) + " is pending")
    case _:
        print("Unknown order status")


# === Optional keys with default ===
settings = {"theme": "blue"}

match settings:
    case {"theme": theme, "language": lang}:
        print("Theme: " + theme + ", Language: " + lang)
    case {"theme": theme}:
        print("Theme: " + theme + ", Language: default")
    case _:
        print("No settings")


# === Matching empty dict ===
empty_config = {}

match empty_config:
    case {}:
        print("Empty configuration")
    case _:
        print("Has configuration")


# === Practical: API response handler ===
response = {"status": 200, "data": {"users": []}}

match response:
    case {"status": 200, "data": data}:
        print("Success! Data: " + str(data))
    case {"status": 404, "error": error}:
        print("Not found: " + error)
    case {"status": 500, "error": error}:
        print("Server error: " + error)
    case _:
        print("Unknown response")


# === Practical: User profile ===
profile = {"username": "john_doe", "email": "john@example.com", "role": "admin"}

match profile:
    case {"username": username, "role": "admin"}:
        print("Admin user: " + username)
    case {"username": username, "role": "moderator"}:
        print("Moderator: " + username)
    case {"username": username, "role": "user"}:
        print("Regular user: " + username)
    case _:
        print("Unknown role")


# === Type + mapping combined ===
# Note: Mapping patterns work with any mapping type (dict)
from collections import OrderedDict

data = OrderedDict([("x", 1), ("y", 2)])

match data:
    case {"x": x, "y": y}:
        print("X: " + str(x) + ", Y: " + str(y))
    case _:
        print("No match")
