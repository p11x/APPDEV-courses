# Example38.py
# Topic: Control Flow — Match Class Patterns

# Class patterns match the structure of objects
# You can match on attributes and their values

# Note: For simplicity, using dictionaries instead of classes
# (Class patterns work the same way conceptually)

# === Matching Object Structure ===

# Using a simple class-like structure (dict)
# In real code, you'd use actual classes with __match_args__

# Simulating a Point class with a dict
point = {"x": 0, "y": 0}

# Match based on the structure
if "x" in point and "y" in point:
    x = point["x"]
    y = point["y"]
    
    if x == 0 and y == 0:
        print("Origin point")
    elif x == 0:
        print("On Y-axis at y=" + str(y))
    elif y == 0:
        print("On X-axis at x=" + str(x))
    else:
        print("Point at (" + str(x) + ", " + str(y) + ")")

# === Real-world example: HTTP Response ===

# Representing an HTTP response as a dict
response = {"status": 200, "data": {"user": "Alice"}}

# Match the response structure
status = response["status"]

match status:
    case 200:
        print("Success - got data")
    case 201:
        print("Created successfully")
    case 400:
        print("Bad request")
    case 401:
        print("Unauthorized")
    case 404:
        print("Not found")
    case 500:
        print("Server error")
    case _:
        print("Unknown status")

# === Real-world example: User Profile ===

user = {"name": "Alice", "role": "admin", "active": True}

# Match user role
role = user["role"]

match role:
    case "admin":
        print("Full access granted")
    case "editor":
        print("Can edit content")
    case "viewer":
        print("Can view content")
    case "guest":
        print("Limited access")
    case _:
        print("Unknown role")

# === Real-world example: API Event ===

event = {"type": "click", "button": "left", "x": 100, "y": 200}

event_type = event["type"]

match event_type:
    case "click":
        button = event.get("button", "unknown")
        print("Mouse click: button=" + button)
    case "keypress":
        key = event.get("key", "unknown")
        print("Key press: key=" + key)
    case "scroll":
        delta = event.get("delta", 0)
        print("Scroll: delta=" + str(delta))
    case _:
        print("Unknown event type")

# === Real-world example: File Status ===

file_info = {"name": "data.txt", "size": 1024, "is_dir": False}

# Match on file properties
size = file_info["size"]

match size:
    case 0:
        print("Empty file: " + file_info["name"])
    case s if s < 1024:
        print("Small file: " + file_info["name"] + " (" + str(s) + " bytes)")
    case s if s < 1024 * 1024:
        print("Medium file: " + file_info["name"] + " (" + str(s) + " bytes)")
    case _:
        print("Large file: " + file_info["name"])

# === Real-world example: Game Move ===

move = {"type": "chess", "from": "e2", "to": "e4", "piece": "pawn"}

game_type = move["type"]

match game_type:
    case "chess":
        piece = move.get("piece", "")
        from_sq = move.get("from", "")
        to_sq = move.get("to", "")
        print(piece + " moves from " + from_sq + " to " + to_sq)
    case "checkers":
        from_sq = move.get("from", "")
        to_sq = move.get("to", "")
        print("Checker moves from " + from_sq + " to " + to_sq)
    case "go":
        print("Go move processed")
    case _:
        print("Unknown game")

# === Real-world example: Config Settings ===

config = {"debug": True, "log_level": "info", "max_connections": 100}

# Match on debug status
debug = config["debug"]

match debug:
    case True:
        print("Debug mode is ON")
    case False:
        print("Debug mode is OFF")

# Match log level
log_level = config["log_level"]

match log_level:
    case "debug":
        print("Logging: All messages")
    case "info":
        print("Logging: Info and above")
    case "warning":
        print("Logging: Warning and above")
    case "error":
        print("Logging: Errors only")
    case _:
        print("Logging: Unknown level")
